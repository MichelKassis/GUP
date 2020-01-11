using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorRotation : MonoBehaviour
{
    public GameObject mech;
    private Vector2 mech_position;
    private Transform hold_transform;
    // Update is called once per frame
    void Update()
    {
        hold_transform = transform;
        var aimAngle = Mathf.Atan2(transform.localPosition.y, transform.localPosition.x);
        hold_transform.localEulerAngles = new Vector3(0, 0, aimAngle * Mathf.Rad2Deg);
        transform.rotation = hold_transform.rotation;
    }
}
