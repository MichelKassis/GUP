using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera2 : MonoBehaviour
{
    public Transform target;

    public Vector2 offset = Vector2.up * 5;
    public float speed = 2;
    public float size = 20;
    public float min_dist = 0.5f;
    
    public float minX = 4.1f;
    public float maxX = -3.9f;
    public float minY = 14.0f;
    public float maxY = 100.0f;
   
    void Start()
    {
        Camera cam = GetComponent<Camera>();
        //cam.orthographicSize = size;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = target.position - transform.position;
        dir += offset;
        if (dir.sqrMagnitude > min_dist * min_dist)
        {   Vector3 yPosition = (Vector3)dir * speed * Time.deltaTime;
            transform.position += (Vector3)dir * speed * Time.deltaTime;
        }
    }
}
