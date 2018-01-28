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

    PlayerRobot playerBot;

    public float dirNum;

    void Update() {
        if (levelManager.playMode) {
            if (playerBot.commandListPos < playerBot.commandList.Count && !moveToPoint) {

                Debug.Log("currentWaypoint: " + currentWaypoint);
                Vector3 heading = waypoints[currentWaypoint].position - transform.position;
                dirNum = AngleDir(transform.forward, heading, transform.up);

                if (dirNum == -1) {
                    TurnLeft();
                    Debug.Log("GoingLeft");
                } else if (dirNum == 1) {
                    TurnRight();
                    Debug.Log("GoingRight");
                } else if (dirNum == 0) {
                    MoveForward();
                    Debug.Log("GoingForward");
                }
            }
        }
    }


    float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up) {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);

        if (dir > 0f) {
            return 1f;
        } else if (dir < 0f) {
            return -1f;
        } else {
            return 0f;
        }
    }



    void Start() {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        playerBot = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerRobot>();
        endPosition = transform.localPosition;
        endRotation = transform.eulerAngles;
        moveToPoint = true;
    }

    void FixedUpdate() {
        if (transform.localPosition == endPosition && Vector3.Distance(transform.eulerAngles, endRotation) < 0.01f) {
            levelManager.guar1EndOfTurn = true;
            moveToPoint = false;
            timeLerped = 0;
            currentWaypoint += 1;
            if (currentWaypoint > waypoints.Count) {
                currentWaypoint = 0;
            }
        } else if (levelManager.turnTimer == 0) {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, endPosition, moveSpeed * Time.deltaTime);
            //Debug.Log("hi");
            timeLerped += Time.deltaTime;
            turnAngle = Mathf.LerpAngle(transform.rotation.y, turnDegree, Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, turnDegree, 0), timeLerped / turnSpeed);
        }
    }

    

    public void MoveForward() {
        endPosition = transform.localPosition + transform.forward * distanceToMove;
        moveToPoint = true;
    }
    public void MoveBack() {
        endPosition = transform.localPosition + -transform.forward * distanceToMove;
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
