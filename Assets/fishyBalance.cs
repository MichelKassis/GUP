using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishyBalance : MonoBehaviour
{
    public GameObject mech_suit;
    private Transform hold_transform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hold_transform = gameObject.transform;
        hold_transform.localEulerAngles = new Vector3(0,0,-mech_suit.transform.eulerAngles.z);
        gameObject.transform.rotation = hold_transform.rotation;
    }
}
