using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRobot : MonoBehaviour {

    LevelManager levelManager;

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

    public List<int> commandList = new List<int>();
    public int commandListPos = 0;

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
            if (commandListPos < commandList.Count && !moveToPoint) {
                switch (commandList[commandListPos]) {
                    case 1:
                        if (WallCheck(1) != 1) {
                            MoveForward();
                        } else {
                        }
                        break;
                    case 2:
                        if (WallCheck(2) != 2) {
                            MoveBack();
                        } else {
                        }
                        break;
                    case 3:
                        TurnLeft();
                        break;
                    case 4:
                        TurnRight();
                        break;
                    case 5:
                        Wait();
                        break;
                    case 6:
                        Corrupt();
                        break;
                }
                commandListPos++;
                levelManager.isFirstMove = false;
            }
        }
    }

    //Add to command list
    public void AddCommand(int p_command) {
        commandList.Add(p_command);
    }

    //Commands
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
    public void Wait() {
        float tempTime = 1;
        if (tempTime > 0) {
            tempTime -= Time.deltaTime;
        } else {
            moveToPoint = true;
        }
    }
    public void Corrupt() {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position, fwd, Color.red);
        if (Physics.Raycast(transform.position, fwd, out hit, 1.2f)) {
            if (hit.transform.tag == "Generator") {
                hit.transform.GetComponent<Generator>().Corrupted();
            }
        }
        moveToPoint = true;
    }

    //wall check
    int WallCheck(int direction) {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position, fwd, Color.green);
        if (Physics.Raycast(transform.position, fwd, out hit, 1.2f)) {
            if (hit.transform.tag == "Trap" && direction == 1) {
                KillRobot();
            }
            return 1;
        }
        Vector3 back = transform.TransformDirection(-Vector3.forward);
        Debug.DrawRay(transform.position, back, Color.green);
        if (Physics.Raycast(transform.position, back, out hit, 1.2f)) {
            if (hit.transform.tag == "Trap" && direction == 2) {
                KillRobot();
            }
            return 2;
        }
        return 0;
    }

    //Undo
    public void Undo() {
        if (commandList.Count > 0) {
            commandList.RemoveAt(commandList.Count - 1);
        }
    }

    //restart
    public void Restart() {
        SceneManager.LoadScene("main");
    }

    void KillRobot() {
        levelManager.IncreaseDeathCount();
        SceneManager.LoadScene("main");
    }

    //Begin
    public void Begin() {
        if (!levelManager.playMode) {
            levelManager.playMode = true;
        }
    }

    //Quit
    public void Quit() {
        Keygon.score = 0;
        Keygon.deathCount = 0;
        SceneManager.LoadScene("menu");
    }
}
