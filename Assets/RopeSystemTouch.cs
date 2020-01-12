using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class RopeSystemTouch : MonoBehaviour
{
    public Slider slider;
    public bool retractToggle;

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
    public float ropeMaxCastDistance = 50f;
    private List<Vector2> ropePositionsLeft = new List<Vector2>();

    private bool distanceSetLeft;

    public float climbSpeed = 3f;
    private bool isCollidingLeft;
    
    public float aimAngle;

    public int minLength;

    public GameObject pseudoLauncherRight;

    public GameObject ropeHingeAnchorRight;
    public DistanceJoint2D ropeJointRight;

    private bool ropeAttachedRight;
    private Vector2 playerPositionRight;
    private Rigidbody2D ropeHingeAnchorRbRight;
    private SpriteRenderer ropeHingeAnchorSpriteRight;

    public LineRenderer ropeRendererRight;
    public LayerMask ropeLayerMaskRight;
    private List<Vector2> ropePositionsRight = new List<Vector2>();

    private bool distanceSetRight;

    private bool isCollidingRight;

    public float aimAngleRight;
    private bool didLeftRope = false;

    void Awake()
    {
        ropeJointLeft.enabled = false;
        playerPositionLeft = pseudoLauncherLeft.transform.position;
        ropeHingeAnchorRbLeft = ropeHingeAnchorLeft.GetComponent<Rigidbody2D>();
        ropeHingeAnchorSpriteLeft = ropeHingeAnchorLeft.GetComponent<SpriteRenderer>();

        ropeJointRight.enabled = false;
        playerPositionRight = pseudoLauncherRight.transform.position;
        ropeHingeAnchorRbRight = ropeHingeAnchorRight.GetComponent<Rigidbody2D>();
        ropeHingeAnchorSpriteRight = ropeHingeAnchorRight.GetComponent<SpriteRenderer>();
    }

    void Update() {

        aimAngle = (1 - slider.value) * Mathf.PI;

        var aimDirection = Quaternion.Euler(0, 0, (1 - slider.value) * 180) * Vector2.right;
        // 5
        playerPositionLeft = pseudoLauncherLeft.transform.position;

        // 6

        if (!pseudoLauncherLeft.GetComponent<psuedoLaunch>().launched)
        {
            pseudoLauncherLeft.transform.localEulerAngles = new Vector3(0, 0, (1-slider.value) * 180 - transform.eulerAngles.z);
        }

        // 5
        playerPositionRight = pseudoLauncherRight.transform.position;

        // 6

        if (!pseudoLauncherRight.GetComponent<psuedoLaunch>().launched)
        {
            pseudoLauncherRight.transform.localEulerAngles = new Vector3(0, 0, (1 - slider.value) * 180 - transform.eulerAngles.z);
        }

        SetCrosshairPosition(aimAngle);

        // HandleInputLeft();
        // HandleInputRight();

        UpdateRopePositionsLeft();
        UpdateRopePositionsRight();

        if (retractToggle)
        {
            HandleRopeLength();
        }
    }

    private void SetCrosshairPosition(float aim)
    {
            crosshairSpriteLeft.enabled = true;

        var x = pseudoLauncherLeft.transform.position.x + 4.2f * Mathf.Cos(aim);
        var y = pseudoLauncherLeft.transform.position.y + 4.2f * Mathf.Sin(aim);

        var crossHairPosition = new Vector3(x, y, 0);
        crosshairLeft.transform.position = crossHairPosition;
    }

    public void HandleInput()
    {
        retractToggle = false;

        if (!didLeftRope)
        {
            ResetRopeLeft();

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
        else
        {
            ResetRopeRight();

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
    }

    public void ToggleRetract()
    {
        retractToggle = !retractToggle;
    }

    public void HandleRopeLength()
    {
        // 1
        if (ropeAttachedLeft && ropeAttachedRight)
        {
            if (ropeJointLeft.distance > minLength)
            {
                ropeJointLeft.distance -= Time.deltaTime * climbSpeed;
            }
            else
            {
                ropeJointLeft.distance = minLength;
            }

            if (ropeJointRight.distance > minLength)
            {
                ropeJointRight.distance -= Time.deltaTime * climbSpeed;
            }
            else
            {
                ropeJointRight.distance = minLength;
            }

            if (ropeJointLeft.distance <= minLength && ropeJointRight.distance <= minLength)
            {
                ropeJointLeft.distance = minLength;
                ropeJointRight.distance = minLength;
                retractToggle = false;
            }
        }
        else if (ropeAttachedLeft && !ropeAttachedRight)
        {
            if (ropeJointLeft.distance > minLength)
            {
                ropeJointLeft.distance -= Time.deltaTime * climbSpeed;
            }
            else
            {
                retractToggle = false;
                ropeJointLeft.distance = minLength;
            }
        }

        else if (!ropeAttachedLeft && ropeAttachedRight)
        {
            if (ropeJointRight.distance > minLength)
            {
                ropeJointRight.distance -= Time.deltaTime * climbSpeed;
            }
            else
            {
                retractToggle = false;
                ropeJointRight.distance = minLength;
            }
        }
    }

    public void CastRopeLeft(Vector3 aimDirection)
    {

        // 2
        ropeRendererLeft.enabled = true;

        var hit = Physics2D.Raycast(playerPositionLeft, aimDirection, ropeMaxCastDistance, ropeLayerMaskLeft);
        // 3

        ropeAttachedLeft = true;
        didLeftRope = true;
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

    public void CastRopeRight(Vector3 aimDirection)
    { 
        // 2
        ropeRendererRight.enabled = true;

        var hit = Physics2D.Raycast(playerPositionRight, aimDirection, ropeMaxCastDistance, ropeLayerMaskRight);

        // 3

        ropeAttachedRight = true;
        didLeftRope = false;
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
}
