using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The water rises slowly, if it touches fish it dies

public class RisingWater : MonoBehaviour {
    public float speed = 1;

    // Update is called once per frame
    void Update() {
        transform.localScale += speed * 2 * Vector3.up / 60;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        collision.gameObject.SetActive(false);
    }
}
