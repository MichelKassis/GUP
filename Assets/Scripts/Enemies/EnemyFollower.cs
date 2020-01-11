using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//follow fishyboii
public class EnemyFollower : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    private Transform target;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }
}
