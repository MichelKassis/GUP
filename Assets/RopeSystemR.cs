using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RopeSystemR : MonoBehaviour
{
    public GameObject pseudoLauncherRight;

    public GameObject ropeHingeAnchorRight;
    public DistanceJoint2D ropeJointRight;
    public Transform crosshairRight;
    public SpriteRenderer crosshairSpriteRight;
    private bool ropeAttachedRight;
    private Vector2 playerPositionRight;
    private Rigidbody2D ropeHingeAnchorRbRight;
    private SpriteRenderer ropeHingeAnchorSpriteRight;

    public LineRenderer ropeRendererRight;
    public LayerMask ropeLayerMaskRight;
    private float ropeMaxCastDistanceRight = 40f;
    private List<Vector2> ropePositionsRight = new List<Vector2>();

    private bool distanceSetRight;

    public float climbSpeedRight = 3f;
    private bool isCollidingRight;

    public int clickNoRight;

    public float aimAngleRight;

    void AwakeRight()
    {
        // 2
        ropeJointRight.enabled = false;
        playerPositionRight = pseudoLauncherRight.transform.position;
        ropeHingeAnchorRbRight = ropeHingeAnchorRight.GetComponent<Rigidbody2D>();
        ropeHingeAnchorSpriteRight = ropeHingeAnchorRight.GetComponent<SpriteRenderer>();
    }

    void UpdateRight()
    {
        // 3
        var worldMousePosition =
            Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        var facingDirection = worldMousePosition - pseudoLauncherRight.transform.position;

        aimAngleRight = Mathf.Atan2(facingDirection.y, facingDirection.x);
        
        if (aimAngleRight < 0f)
        {
            aimAngleRight = Mathf.PI * 2 + aimAngleRight;
        }
        

        // 4
        var aimDirection = Quaternion.Euler(0, 0, aimAngleRight * Mathf.Rad2Deg) * Vector2.right;
        // 5
        playerPositionRight = pseudoLauncherRight.transform.position;

        // 6

        if (!pseudoLauncherRight.GetComponent<psuedoLaunch>().launched)
        {
            pseudoLauncherRight.transform.localEulerAngles = new Vector3(0, 0, aimAngleRight * Mathf.Rad2Deg - transform.eulerAngles.z);
        }

        if (!ropeAttachedRight)
        {
            SetCrosshairPositionRight(aimAngleRight);
        }
        else
        {
            crosshairSpriteRight.enabled = false;
        }

        HandleInputRight();
        UpdateRopePositionsRight();
        HandleRopeLengthRight();
    }

    private void SetCrosshairPositionRight(float aimAngleRight)
    {
        if (!crosshairSpriteRight.enabled)
        {
            crosshairSpriteRight.enabled = true;
        }

        var x = pseudoLauncherRight.transform.position.x + 4.2f * Mathf.Cos(aimAngleRight);
        var y = pseudoLauncherRight.transform.position.y + 4.2f * Mathf.Sin(aimAngleRight);

        var crossHairPosition = new Vector3(x, y, 0);
        crosshairRight.transform.position = crossHairPosition;
    }

    private void HandleInputRight()
    {
        if (Input.GetMouseButtonDown(clickNoRight))
        {
            if (ropeAttachedRight) return;

            pseudoLauncherRight.GetComponent<SpriteRenderer>().enabled = true;

            if (pseudoLauncherRight.GetComponent<psuedoLaunch>().launched != true)
            {
                if (pseudoLauncherRight.GetComponent<psuedoLaunch>().readied)
                {
                    pseudoLauncherRight.GetComponent<psuedoLaunch>().launch();
                }
            }
        }
        // if (Input.GetMouseButtonUp(clickNoRight))
        // {
        //     ResetRopeRight();
        // }
    }

    public void CastRopeRight(Vector3 aimDirection)
    {
        // 2
        ropeRendererRight.enabled = true;

        var hit = Physics2D.Raycast(playerPositionRight, aimDirection, ropeMaxCastDistanceRight, ropeLayerMaskRight);

        // 3

        ropeAttachedRight = true;
        if (!ropePositionsRight.Contains(hit.point))
        {
            // 4
            // Jump slightly to distance the player a little from the ground after grappling to something.
            //transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 2f), ForceMode2D.Impulse);
            ropePositionsRight.Add(hit.point);
            ropeJointRight.distance = Vector2.Distance(playerPositionRight, hit.point) - 1f;
            ropeJointRight.enabled = true;
            ropeHingeAnchorSpriteRight.enabled = true;
        }
        // 5
    }

    // 6
    private void ResetRopeRight()
    {
        ropeJointRight.enabled = false;
        ropeAttachedRight = false;
        ropeRendererRight.positionCount = 2;
        ropeRendererRight.SetPosition(0, pseudoLauncherRight.transform.position);
        ropeRendererRight.SetPosition(1, pseudoLauncherRight.transform.position);
        ropePositionsRight.Clear();
        ropeHingeAnchorSpriteRight.enabled = false;
        pseudoLauncherRight.GetComponent<SpriteRenderer>().enabled = true;
    }

    private void UpdateRopePositionsRight()
    {
        // 1
        if (!ropeAttachedRight)
        {
            return;
        }

        // 2
        ropeRendererRight.positionCount = ropePositionsRight.Count + 1;

        // 3
        for (var i = ropeRendererRight.positionCount - 1; i >= 0; i--)
        {
            if (i != ropeRendererRight.positionCount - 1) // if not the Last point of line renderer
            {
                ropeRendererRight.SetPosition(i, ropePositionsRight[i]);

                // 4
                if (i == ropePositionsRight.Count - 1 || ropePositionsRight.Count == 1)
                {
                    var ropePosition = ropePositionsRight[ropePositionsRight.Count - 1];
                    if (ropePositionsRight.Count == 1)
                    {
                        ropeHingeAnchorRbRight.transform.position = ropePosition;
                        if (!distanceSetRight)
                        {
                            ropeJointRight.distance = Vector2.Distance(pseudoLauncherRight.transform.position, ropePosition);
                            distanceSetRight = true;
                        }
                    }
                    else
                    {
                        ropeHingeAnchorRbRight.transform.position = ropePosition;
                        if (!distanceSetRight)
                        {
                            ropeJointRight.distance = Vector2.Distance(pseudoLauncherRight.transform.position, ropePosition);
                            distanceSetRight = true;
                        }
                    }
                }
                // 5
                else if (i - 1 == ropePositionsRight.IndexOf(ropePositionsRight.Last()))
                {
                    var ropePosition = ropePositionsRight.Last();
                    ropeHingeAnchorRbRight.transform.position = ropePosition;
                    if (!distanceSetRight)
                    {
                        ropeJointRight.distance = Vector2.Distance(pseudoLauncherRight.transform.position, ropePosition);
                        distanceSetRight = true;
                    }
                }
            }
            else
            {
                // 6
                ropeRendererRight.SetPosition(i, pseudoLauncherRight.transform.position);
            }
        }
    }

    private void HandleRopeLengthRight()
    {
        // 1
        if (Input.GetMouseButton(clickNoRight) && ropeAttachedRight && !isCollidingRight)
        {
            ropeJointRight.distance -= Time.deltaTime * climbSpeedRight;
        }
        /*
        else if (Input.GetAxis("Vertical") < 0f && ropeAttachedRight)
        {
            ropeJointRight.distance += Time.deltaTime * climbSpeedRight;
        }
        */
    }

    void OnTriggerStay2D(Collider2D colliderStay)
    {
        isCollidingRight = true;
    }

    private void OnTriggerExit2D(Collider2D colliderOnExit)
    {
        isCollidingRight = false;
    }
}
