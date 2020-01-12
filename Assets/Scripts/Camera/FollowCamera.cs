using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;

    public Vector2 offset = Vector2.up * 5;
    public float speed = 2;
    public float size = 20;
    public float min_dist = 0.5f;

    public int floor;

    void Start()
    {
        Camera cam = GetComponent<Camera>();
        cam.orthographicSize = size;
    }

    // Update is called once per frame
    void Update()
    {
        if (target.position.y > floor)
        {
            Vector2 dir = new Vector2 (0,target.position.y - transform.position.y);
            dir += offset;
            if (dir.sqrMagnitude > min_dist * min_dist)
            {
                transform.position += (Vector3)dir * speed * Time.deltaTime;
            }
        }
    }
}
