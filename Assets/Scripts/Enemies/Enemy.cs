using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// If the rope hits Floaty, it dies

public class Enemy : MonoBehaviour {

    public GameObject enemy_container;
    public PolygonCollider2D mainCharacter;
    void Start()
    {
        mainCharacter = enemy_container.GetComponent<EnemyReference>().mainCharacter;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider == mainCharacter) {
            mainCharacter.gameObject.SetActive(false);
        }
    }
}
