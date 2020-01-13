using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMusic : MonoBehaviour {
    public AK.Wwise.Event stopMusic;
    public AK.Wwise.Event bossMusic;

    public GameObject wwiseobj;
    void Update() {
        if (transform.localPosition.y > 700) {
            stopMusic.Post(wwiseobj);
            bossMusic.Post(wwiseobj);

        }
    }
}
