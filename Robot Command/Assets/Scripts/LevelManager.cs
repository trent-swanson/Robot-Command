using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public GameObject playerRobot;
    Transform robotSpawn;
    Terminal terminal;

    public float turnTime = 2f;
    public float turnTimer = 0;
    public bool endOfTurn = false;
    public bool guar1EndOfTurn = false;
    public bool playMode = false;
    public bool isFirstMove = true;
    public bool wait = false;

    public GameObject robotDeathCount;

    void Start() {
        terminal = GameObject.FindGameObjectWithTag("Terminal").GetComponent<Terminal>();
        robotSpawn = GameObject.FindGameObjectWithTag("Spawn").transform;
        robotDeathCount.GetComponent<Text>().text = "Robot Death Count: " + Keygon.deathCount;
        NewGame();
    }

    void Update()
    {
        if(!isFirstMove) {
            if (endOfTurn) {
                if (wait) {
                    turnTimer = turnTime + 2f;
                } else {
                    turnTimer = turnTime;
                }
                endOfTurn = false;
                //guar1EndOfTurn = false;
                wait = false;
            }
            if (turnTimer > 0) {
                turnTimer -= Time.deltaTime;
            } else {
                turnTimer = 0;
            }
        }
    }

    public void NewGame() {
        Instantiate(playerRobot, robotSpawn.position, robotSpawn.rotation);
        terminal.ClearTerminal();
    }

    public void IncreaseDeathCount() {
        Keygon.deathCount++;
    }
}
