using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Keygon : MonoBehaviour {

    public static int score = 0;
    public static List<int> ids = new List<int>();

    public static int deathCount = 0;

    public void Start() {
        if (score > 0) {
            for (int i = 1; i <= score; i++) {
                transform.GetChild(i - 1).GetComponent<Image>().CrossFadeAlpha(0.2f, 20f * Time.deltaTime, false);
            }
        }
    }

    public void Score(int id) {
        transform.GetChild(score).GetComponent<Image>().CrossFadeAlpha(0.2f, 20f * Time.deltaTime, false);
        score += 1;
        ids.Add(id);
        if (score >= 3) {
            score = 0;
            SceneManager.LoadScene("Won");
        }
    }
}
