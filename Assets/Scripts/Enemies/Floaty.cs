using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// If the rope hits Floaty, it dies

public class Floaty : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D col) {
        if (col.name.Contains("SpiderRope")) {
            gameObject.SetActive(false);
        }
    }
}
