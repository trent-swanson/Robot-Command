using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Terminal : MonoBehaviour {

    public GameObject canvas;
    public AudioClip transmissionClip;
    public AudioClip invalidClip;
    public AudioClip helpClip;

    LevelManager levelManager;
    public Help helpPanel;

    public GameObject commandList;
    public GameObject commandElement;
    public InputField inputField;
    public GameObject invalidCommandPopUp;
    public GameObject restartCommandPopUp;

    bool finishedExecute = false;

    PlayerRobot playerRobot;

    void Start() {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        invalidCommandPopUp.GetComponent<Image>().canvasRenderer.SetAlpha(0.0f);
        invalidCommandPopUp.transform.GetChild(0).GetComponent<Text>().canvasRenderer.SetAlpha(0.0f);
        restartCommandPopUp.GetComponent<Image>().canvasRenderer.SetAlpha(0.0f);
        restartCommandPopUp.transform.GetChild(0).GetComponent<Text>().canvasRenderer.SetAlpha(0.0f);
    }

    void Update() {
        if (levelManager.playMode) {
            if (playerRobot.commandListPos >= playerRobot.commandList.Count) {
                finishedExecute = true;
            } else {
                inputField.DeactivateInputField();
            }
        }
    }

    public void EnterCommand() {
        string tempData = inputField.text.ToLower();
        tempData.TrimEnd();
        tempData.TrimStart();

        if (!finishedExecute && (tempData == "forward" || tempData == "move" || tempData == "move_forward" || tempData == "moveforward" || tempData == "move forward")) {
            GameObject tempCommand = Instantiate(commandElement);
            tempCommand.transform.SetParent(commandList.transform);
            tempCommand.transform.GetChild(0).GetComponent<Text>().text = "Move_Forward";
            playerRobot.AddCommand(1);
        }
        else if (!finishedExecute && (tempData == "back" || tempData == "move_back" || tempData == "moveback" || tempData == "move back")) {
            GameObject tempCommand = Instantiate(commandElement);
            tempCommand.transform.SetParent(commandList.transform);
            tempCommand.transform.GetChild(0).GetComponent<Text>().text = "Move_Back";
            playerRobot.AddCommand(2);
        }
        else if (!finishedExecute && (tempData == "left" || tempData == "turnleft" || tempData == "turn left" || tempData == "turn_left")) {
            GameObject tempCommand = Instantiate(commandElement);
            tempCommand.transform.SetParent(commandList.transform);
            tempCommand.transform.GetChild(0).GetComponent<Text>().text = "Turn_Left";
            playerRobot.AddCommand(3);
        }
        else if (!finishedExecute && (tempData == "right" || tempData == "turnright" || tempData == "turn right" || tempData == "turn_right")) {
            GameObject tempCommand = Instantiate(commandElement);
            tempCommand.transform.SetParent(commandList.transform);
            tempCommand.transform.GetChild(0).GetComponent<Text>().text = "Turn_Right";
            playerRobot.AddCommand(4);
        }
        else if (!finishedExecute && tempData == "wait") {
            GameObject tempCommand = Instantiate(commandElement);
            tempCommand.transform.SetParent(commandList.transform);
            tempCommand.transform.GetChild(0).GetComponent<Text>().text = "Wait";
            playerRobot.AddCommand(5);
        }
        else if (!finishedExecute && tempData == "corrupt") {
            GameObject tempCommand = Instantiate(commandElement);
            tempCommand.transform.SetParent(commandList.transform);
            tempCommand.transform.GetChild(0).GetComponent<Text>().text = "Corrupt";
            playerRobot.AddCommand(6);
        } else if (!finishedExecute && (tempData == "execute" || tempData == "begin" || tempData == "transmit")) {
            canvas.GetComponent<AudioSource>().PlayOneShot(transmissionClip);
            GameObject tempCommand = Instantiate(commandElement);
            tempCommand.transform.SetParent(commandList.transform);
            tempCommand.transform.GetChild(0).GetComponent<Text>().text = "Transmiting";
            playerRobot.Begin();
        } else if (tempData == "restart") {
            GameObject tempCommand = Instantiate(commandElement);
            tempCommand.transform.SetParent(commandList.transform);
            tempCommand.transform.GetChild(0).GetComponent<Text>().text = "Restart";
            playerRobot.Restart();
        } else if (!finishedExecute && tempData == "undo") {
            GameObject tempCommand = Instantiate(commandElement);
            tempCommand.transform.SetParent(commandList.transform);
            tempCommand.transform.GetChild(0).GetComponent<Text>().text = "Undo";
            for (int i = commandList.transform.childCount - 1; i >= 0; i--) {
                if (commandList.transform.GetChild(i).GetChild(0).GetComponent<Text>().text == "Undo") {
                    continue;
                } else {
                    Destroy(commandList.transform.GetChild(i).gameObject);
                    break;
                }
            }
            playerRobot.Undo();
        } else if (tempData == "quit") {
            GameObject tempCommand = Instantiate(commandElement);
            tempCommand.transform.SetParent(commandList.transform);
            tempCommand.transform.GetChild(0).GetComponent<Text>().text = "Quit";
            playerRobot.Quit();
        } else if (tempData == "help") {
            canvas.GetComponent<AudioSource>().PlayOneShot(helpClip);
            GameObject tempCommand = Instantiate(commandElement);
            tempCommand.transform.SetParent(commandList.transform);
            tempCommand.transform.GetChild(0).GetComponent<Text>().text = "Help";
            helpPanel.OpenCloseHelp();
        } else if (tempData != "") {
            if (!finishedExecute) {
                StartCoroutine(InvalidCommand());
            } else {
                StartCoroutine(RestartCommand());
            }
        }

        inputField.text = "";
        inputField.ActivateInputField();
    }

    IEnumerator InvalidCommand() {
        canvas.GetComponent<AudioSource>().PlayOneShot(invalidClip);
        invalidCommandPopUp.GetComponent<Image>().CrossFadeAlpha(1f, 15f * Time.deltaTime, false);
        invalidCommandPopUp.transform.GetChild(0).GetComponent<Text>().CrossFadeAlpha(1f, 15f * Time.deltaTime, false);
        yield return new WaitForSeconds(0.7f);
        invalidCommandPopUp.GetComponent<Image>().CrossFadeAlpha(0f, 55.0f * Time.deltaTime, false);
        invalidCommandPopUp.transform.GetChild(0).GetComponent<Text>().CrossFadeAlpha(0f, 55.0f * Time.deltaTime, false);
    }

    IEnumerator RestartCommand() {
        canvas.GetComponent<AudioSource>().PlayOneShot(invalidClip);
        restartCommandPopUp.GetComponent<Image>().CrossFadeAlpha(1f, 15f * Time.deltaTime, false);
        restartCommandPopUp.transform.GetChild(0).GetComponent<Text>().CrossFadeAlpha(1f, 15f * Time.deltaTime, false);
        yield return new WaitForSeconds(0.7f);
        restartCommandPopUp.GetComponent<Image>().CrossFadeAlpha(0f, 55.0f * Time.deltaTime, false);
        restartCommandPopUp.transform.GetChild(0).GetComponent<Text>().CrossFadeAlpha(0f, 55.0f * Time.deltaTime, false);
    }

    public void ClearTerminal() {
        if (commandList.transform.childCount > 0) {
            foreach (Transform child in commandList.transform) {
                Destroy(child.gameObject);
            }
        }
        playerRobot = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerRobot>();
        inputField.ActivateInputField();
    }
}
