using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RopeSystem : MonoBehaviour
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
    private float ropeMaxCastDistance = 55f;
    private List<Vector2> ropePositionsLeft = new List<Vector2>();

    private bool distanceSetLeft;

    public float climbSpeed = 3f;
    private bool isCollidingLeft;

    public int clickNoLeft;

    public float aimAngleLeft;


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
    private List<Vector2> ropePositionsRight = new List<Vector2>();

    private bool distanceSetRight;

    private bool isCollidingRight;

    public int clickNoRight;

    public float aimAngleRight;
    private bool didLeftRope = false; 

    void Awake() {
        AwakeLeft();
        AwakeRight();
    }
    void Update() {
        UpdateLeft();
        UpdateRight();

        HandleInput();
        // HandleInputLeft();
        // HandleInputRight();

        UpdateRopePositionsLeft();
        UpdateRopePositionsRight();

        HandleRopeLength();
    }


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

    
    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(clickNoLeft))
        {
            if (!didLeftRope)
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
            else 
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
        }
            if (Input.GetMouseButtonUp(clickNoLeft))
            {
                if (!didLeftRope && ropeAttachedLeft) {ResetRopeLeft();}
                else{
                ResetRopeRight();
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

    private void HandleRopeLength()
    {
        // 1
        if (Input.GetMouseButton(clickNoLeft) && ropeAttachedLeft && ropeAttachedRight){
            ropeJointLeft.distance -= Time.deltaTime * climbSpeed;
            ropeJointRight.distance -= Time.deltaTime * climbSpeed;
        }
        else if (Input.GetMouseButton(clickNoLeft) && ropeAttachedLeft)
        {
            ropeJointLeft.distance -= Time.deltaTime * climbSpeed;
        }

        else if (Input.GetMouseButton(clickNoLeft) && ropeAttachedRight)
        {
            ropeJointRight.distance -= Time.deltaTime * climbSpeed;
        }
    }

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




}
