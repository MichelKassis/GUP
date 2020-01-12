using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RopeSystemL : MonoBehaviour
{
    public GameObject pseudoLauncherLeft;
    public GameObject ropeHingeAnchorLeft;
    public DistanceJoint2D ropeJointLeft;
    public Transform crosshairLeft;
    public SpriteRenderer crosshairSpriteLeft;
    private bool ropeAttachedLeft;
    private Vector2 playerPositionLeft;
    private Rigidbody2D ropeHingeAnchorRbLeft;
    private SpriteRenderer ropeHingeAnchorSpriteLeft;

    public LineRenderer ropeRendererLeft;
    public LayerMask ropeLayerMaskLeft;
    private float ropeMaxCastDistanceLeft = 30f;
    private List<Vector2> ropePositionsLeft = new List<Vector2>();

    private bool distanceSetLeft;

    public float climbSpeedLeft = 3f;
    private bool isCollidingLeft;

    public int clickNoLeft;

    public float aimAngleLeft;

    


    void AwakeLeft()
    {
        // 2
        ropeJointLeft.enabled = false;
        playerPositionLeft = pseudoLauncherLeft.transform.position;
        ropeHingeAnchorRbLeft = ropeHingeAnchorLeft.GetComponent<Rigidbody2D>();
        ropeHingeAnchorSpriteLeft = ropeHingeAnchorLeft.GetComponent<SpriteRenderer>();
    }

    void UpdateLeft()
    {
        // 3
        var worldMousePosition =
            Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        var facingDirection = worldMousePosition - pseudoLauncherLeft.transform.position;

        aimAngleLeft = Mathf.Atan2(facingDirection.y, facingDirection.x);

        if (aimAngleLeft < 0f)
        {
            aimAngleLeft = Mathf.PI * 2 + aimAngleLeft;
        }


        // 4
        var aimDirection = Quaternion.Euler(0, 0, aimAngleLeft * Mathf.Rad2Deg) * Vector2.right;
        // 5
        playerPositionLeft = pseudoLauncherLeft.transform.position;

        // 6

        if (!pseudoLauncherLeft.GetComponent<psuedoLaunch>().launched)
        {
            pseudoLauncherLeft.transform.localEulerAngles = new Vector3(0, 0, aimAngleLeft * Mathf.Rad2Deg - transform.eulerAngles.z);
        }

        if (!ropeAttachedLeft)
        {
            SetCrosshairPositionLeft(aimAngleLeft);
        }
        else
        {
            crosshairSpriteLeft.enabled = false;
        }

        HandleInputLeft();
        UpdateRopePositionsLeft();
        HandleRopeLengthLeft();
    }

    private void SetCrosshairPositionLeft(float aimAngleLeft)
    {
        if (!crosshairSpriteLeft.enabled)
        {
            crosshairSpriteLeft.enabled = true;
        }

        var x = pseudoLauncherLeft.transform.position.x + 4.2f * Mathf.Cos(aimAngleLeft);
        var y = pseudoLauncherLeft.transform.position.y + 4.2f * Mathf.Sin(aimAngleLeft);

        var crossHairPosition = new Vector3(x, y, 0);
        crosshairLeft.transform.position = crossHairPosition;
    }

    private void HandleInputLeft()
    {
        if (Input.GetMouseButtonDown(clickNoLeft))
        {
            if (ropeAttachedLeft) return;

            pseudoLauncherLeft.GetComponent<SpriteRenderer>().enabled = true;

            if (pseudoLauncherLeft.GetComponent<psuedoLaunch>().launched != true)
            {
                if (pseudoLauncherLeft.GetComponent<psuedoLaunch>().readied)
                {
                    pseudoLauncherLeft.GetComponent<psuedoLaunch>().launch();
                }
            }
        }
        // if (Input.GetMouseButtonUp(clickNoLeft))
        // {
        //     ResetRope();
        // }
    }

    public void CastRopeLeft(Vector3 aimDirection)
    {
        // 2
        ropeRendererLeft.enabled = true;

        var hit = Physics2D.Raycast(playerPositionLeft, aimDirection, ropeMaxCastDistanceLeft, ropeLayerMaskLeft);

        // 3

        ropeAttachedLeft = true;
        if (!ropePositionsLeft.Contains(hit.point))
        {
            // 4
            // Jump slightly to distance the player a little from the ground after grappling to something.
            //transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 2f), ForceMode2D.Impulse);
            ropePositionsLeft.Add(hit.point);
            ropeJointLeft.distance = Vector2.Distance(playerPositionLeft, hit.point) - 1f;
            ropeJointLeft.enabled = true;
            ropeHingeAnchorSpriteLeft.enabled = true;
        }
        // 5
    }

    // 6
    private void ResetRopeLeft()
    {
        ropeJointLeft.enabled = false;
        ropeAttachedLeft = false;
        ropeRendererLeft.positionCount = 2;
        ropeRendererLeft.SetPosition(0, pseudoLauncherLeft.transform.position);
        ropeRendererLeft.SetPosition(1, pseudoLauncherLeft.transform.position);
        ropePositionsLeft.Clear();
        ropeHingeAnchorSpriteLeft.enabled = false;
        pseudoLauncherLeft.GetComponent<SpriteRenderer>().enabled = true;
    }

    private void UpdateRopePositionsLeft()
    {
        // 1
        if (!ropeAttachedLeft)
        {
            return;
        }

        // 2
        ropeRendererLeft.positionCount = ropePositionsLeft.Count + 1;

        // 3
        for (var i = ropeRendererLeft.positionCount - 1; i >= 0; i--)
        {
            if (i != ropeRendererLeft.positionCount - 1) // if not the Last point of line renderer
            {
                ropeRendererLeft.SetPosition(i, ropePositionsLeft[i]);

                // 4
                if (i == ropePositionsLeft.Count - 1 || ropePositionsLeft.Count == 1)
                {
                    var ropePosition = ropePositionsLeft[ropePositionsLeft.Count - 1];
                    if (ropePositionsLeft.Count == 1)
                    {
                        ropeHingeAnchorRbLeft.transform.position = ropePosition;
                        if (!distanceSetLeft)
                        {
                            ropeJointLeft.distance = Vector2.Distance(pseudoLauncherLeft.transform.position, ropePosition);
                            distanceSetLeft = true;
                        }
                    }
                    else
                    {
                        ropeHingeAnchorRbLeft.transform.position = ropePosition;
                        if (!distanceSetLeft)
                        {
                            ropeJointLeft.distance = Vector2.Distance(pseudoLauncherLeft.transform.position, ropePosition);
                            distanceSetLeft = true;
                        }
                    }
                }
                // 5
                else if (i - 1 == ropePositionsLeft.IndexOf(ropePositionsLeft.Last()))
                {
                    var ropePosition = ropePositionsLeft.Last();
                    ropeHingeAnchorRbLeft.transform.position = ropePosition;
                    if (!distanceSetLeft)
                    {
                        ropeJointLeft.distance = Vector2.Distance(pseudoLauncherLeft.transform.position, ropePosition);
                        distanceSetLeft = true;
                    }
                }
            }
            else
            {
                // 6
                ropeRendererLeft.SetPosition(i, pseudoLauncherLeft.transform.position);
            }
        }
    }

    private void HandleRopeLengthLeft()
    {
        // 1
        if (Input.GetMouseButton(clickNoLeft) && ropeAttachedLeft && !isCollidingLeft)
        {
            ropeJointLeft.distance -= Time.deltaTime * climbSpeedLeft;
        }
        /*
        else if (Input.GetAxis("Vertical") < 0f && ropeAttachedLeft)
        {
            ropeJointLeft.distance += Time.deltaTime * cLeftlimbSpeed;
        }
        */
    }

    void OnTriggerStay2D(Collider2D colliderStay)
    {
        isCollidingLeft = true;
    }

    private void OnTriggerExit2D(Collider2D colliderOnExit)
    {
        isCollidingLeft = false;
    }
}