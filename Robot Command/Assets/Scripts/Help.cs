using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Help : MonoBehaviour {

    public GameObject helpPanel;

    bool isOpen = false;

    void Start() {
        helpPanel.GetComponent<Image>().canvasRenderer.SetAlpha(0.0f);
        helpPanel.transform.GetChild(0).GetComponent<Text>().canvasRenderer.SetAlpha(0.0f);
    }

    public void OpenCloseHelp() {
        if (!isOpen) {
            helpPanel.GetComponent<Image>().CrossFadeAlpha(1f, 20f * Time.deltaTime, false);
            helpPanel.transform.GetChild(0).GetComponent<Text>().CrossFadeAlpha(1f, 20f * Time.deltaTime, false);
            isOpen = true;
        } else {
            helpPanel.GetComponent<Image>().CrossFadeAlpha(0f, 20.0f * Time.deltaTime, false);
            helpPanel.transform.GetChild(0).GetComponent<Text>().CrossFadeAlpha(0f, 20.0f * Time.deltaTime, false);
            isOpen = false;
        }
    }

    public void Restart() {
        SceneManager.LoadScene("main");
    }

    public void Quit() {
        Keygon.score = 0;
        Keygon.deathCount = 0;
        SceneManager.LoadScene("main");
    }
}
