using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {

    public GameObject exitTele;
    public Transform spawn;

    Vector3 exitPos;
    Quaternion exitRot;

    void Start() {
        exitPos = exitTele.GetComponent<Teleport>().spawn.position;
        exitRot = exitTele.GetComponent<Teleport>().spawn.rotation;
    }

    public void Enter(GameObject player) {
        player.transform.position = exitPos;
        player.transform.rotation = exitRot;
        player.GetComponent<PlayerRobot>().endPosition = new Vector3(exitPos.x, player.GetComponent<PlayerRobot>().endPosition.y, exitPos.z);
        player.GetComponent<PlayerRobot>().endRotation = exitTele.GetComponent<Teleport>().spawn.eulerAngles;
    }
}
