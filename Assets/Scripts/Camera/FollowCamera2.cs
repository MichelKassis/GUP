using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera2 : MonoBehaviour
{
    public Transform target;

    public Vector3 offset = Vector3.up * 5;
    public float speed = 2;
    public float size = 20;
    public float min_dist = 0.5f;
    
    public float minX = 4.1f;
    public float maxX = -3.9f;
    public float minY = 54.0f;
    public float maxY = 100.0f;
   
    void Start()
    {
        Camera cam = GetComponent<Camera>();
        //cam.orthographicSize = size;
        target = GameObject.Find("Fish").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(target.position.y > minY){
        transform.position = new Vector3(transform.position.x, target.position.y, -10);
        }
    }
}
