    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
   
    public class PlayerController : MonoBehaviour {
        public float speed = 30;        // movement speed in units/sec
        public Camera camera;
        Rigidbody2D rb;
        float movement = 0f;
        void Start(){
            rb = GetComponent<Rigidbody2D>();
            //Camera = GameObject.Find("Main Camera");
        }
        
        void Update () {
            Vector3 pos = transform.position;
            movement += Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit    hit;
            //transform.TransformDirection(Vector2.up)
            Debug.DrawRay(transform.position, Input.mousePosition, Color.red);
            if (Physics.Raycast(ray, out hit)) {
            Transform objectHit = hit.transform;
            Debug.DrawRay(transform.position, Input.mousePosition, Color.green);
            // Do something with the object that was hit by the raycast.
            //Vector3   direction = (hit.point - transform.position);
        }

        }

        void FixedUpdate()
	    {
            Vector2 velocity = rb.velocity;
            velocity.x = movement;
            rb.velocity = velocity;
	    }

         
    }
 