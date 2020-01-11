using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// If the rope hits the hawk's center, it dies

public class HawkCenter : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D col) {
        if (col.name.Contains("SpiderRope")) {
            transform.parent.gameObject.SetActive(false);
        }
    }
}
