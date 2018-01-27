using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardBot : MonoBehaviour {

    LevelManager levelManager;

    public List<Transform> waypoints = new List<Transform>();
    public int currentWaypoint = 0;

    [SerializeField]
    private float distanceToMove;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float turnSpeed;

    private bool moveToPoint = true;
    public Vector3 endPosition;
    public Vector3 endRotation;
    private float timeLerped = 0;
    private float turnDegree;
    private float turnAngle;

    Vector3 relativePoint;

    void Start() {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        endPosition = transform.localPosition;
        endRotation = transform.eulerAngles;
        moveToPoint = true;
    }

    void FixedUpdate() {
        if (transform.localPosition == endPosition && Vector3.Distance(transform.eulerAngles, endRotation) < 0.01f) {
            levelManager.endOfTurn = true;
            moveToPoint = false;
            timeLerped = 0;
        } else if (levelManager.turnTimer == 0) {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, endPosition, moveSpeed * Time.deltaTime);
            timeLerped += Time.deltaTime;
            turnAngle = Mathf.LerpAngle(transform.rotation.y, turnDegree, Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, turnDegree, 0), timeLerped / turnSpeed);
        }
    }

    void Update() {
        if (levelManager.playMode) {
            if (!moveToPoint) {
                relativePoint = transform.InverseTransformPoint(waypoints[currentWaypoint].position);
                if (relativePoint.x < -0.01) {
                    //print("Object is to the left");
                    TurnLeft();
                }
                else if (relativePoint.x > 0.01) {
                    //print("Object is to the right");
                    TurnRight();
                }
                else if (transform.localPosition != waypoints[currentWaypoint].position) {
                    MoveForward();
                }
            }
        }
    }

    public void MoveForward() {
        endPosition = transform.localPosition + transform.forward * distanceToMove;
        currentWaypoint += 1;
        if (currentWaypoint > waypoints.Count) {
            currentWaypoint = 0;
        }
        moveToPoint = true;
    }
    public void MoveBack() {
        endPosition = transform.localPosition + -transform.forward * distanceToMove;
        currentWaypoint += 1;
        if (currentWaypoint > waypoints.Count) {
            currentWaypoint = 0;
        }
        moveToPoint = true;
    }
    public void TurnLeft() {
        endRotation = new Vector3(endRotation.x, endRotation.y - 90, endRotation.z);
        if (endRotation.y < 0) {
            endRotation = new Vector3(endRotation.x, endRotation.y + 360, endRotation.z);
        }
        turnDegree -= 90;
        moveToPoint = true;
    }
    public void TurnRight() {
        endRotation = new Vector3(endRotation.x, endRotation.y + 90, endRotation.z);
        if (endRotation.y >= 360) {
            endRotation = new Vector3(endRotation.x, endRotation.y - 360, endRotation.z);
        }
        turnDegree += 90;
        moveToPoint = true;
    }
}
