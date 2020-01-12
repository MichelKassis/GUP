using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The water rises slowly, if it touches fish it dies

public class RisingWater : MonoBehaviour {
    public float speed = 3.0f;

    private bool mainCharacterDied = false;
    private float deadMainCharacterY;

    void Update() {
        if (!mainCharacterDied || transform.localPosition.y < deadMainCharacterY + 10) {
            transform.localPosition += speed * Vector3.up * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        mainCharacterDied = true;
        deadMainCharacterY = collision.transform.localPosition.y;

        collision.gameObject.SetActive(false);
    }
}
