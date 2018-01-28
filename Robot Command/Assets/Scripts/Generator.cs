using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour {

    public AudioClip hackClip;

    public Color corruptedColor;
    Keygon keygon;

    public int id;

    void Start() {
        if (Keygon.score > 0) {
            for (int i = 0; i < Keygon.ids.Count; i++) {
                if (Keygon.ids[i] == id) {
                    transform.GetChild(0).GetComponent<Light>().color = corruptedColor;
                }
            }
        }
    }

    public void Corrupted() {
        transform.GetComponent<AudioSource>().PlayOneShot(hackClip);
        transform.GetChild(0).GetComponent<Light>().color = corruptedColor;
        FindObjectOfType<Keygon>().Score(id);
    }
}
