    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
   
    public class PlayerController : MonoBehaviour {
        public float speed = 30;        // movement speed in units/sec
       
        void Update () {
            Vector3 pos = transform.position;
            pos.x += Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            pos.y += Input.GetAxis("Vertical") * speed * Time.deltaTime;
            transform.position = pos;
        }
         
    }
 