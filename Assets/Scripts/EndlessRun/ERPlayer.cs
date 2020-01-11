using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ERPlayer : MonoBehaviour {

    public EndlessRun er;

	void Start () {
		
	}
	
	void Update () {
		if(Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left *5*Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * 5 * Time.deltaTime);
        }
    }

    //If you use only2D object use OnTriggerEnter2D(Collider2D other)
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Obstacles"))
            er.isGameOver = true;
    }
}
