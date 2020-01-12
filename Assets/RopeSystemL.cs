using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RopeSystemL : MonoBehaviour
{
    public GameObject pseudoLauncher;

    public GameObject ropeHingeAnchor;
    public DistanceJoint2D ropeJoint;
    public Transform crosshair;
    public SpriteRenderer crosshairSprite;
    private bool ropeAttached;
    private Vector2 playerPosition;
    private Rigidbody2D ropeHingeAnchorRb;
    private SpriteRenderer ropeHingeAnchorSprite;

    public LineRenderer ropeRenderer;
    public LayerMask ropeLayerMask;
    private float ropeMaxCastDistance = 30f;
    private List<Vector2> ropePositions = new List<Vector2>();

    private bool distanceSet;

    public float climbSpeed = 3f;
    private bool isColliding;

    public int clickNo;

    public float aimAngle;

    void Awake()
    {
        // 2
        ropeJoint.enabled = false;
        playerPosition = pseudoLauncher.transform.position;
        ropeHingeAnchorRb = ropeHingeAnchor.GetComponent<Rigidbody2D>();
        ropeHingeAnchorSprite = ropeHingeAnchor.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 3
        var worldMousePosition =
            Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        var facingDirection = worldMousePosition - pseudoLauncher.transform.position;

        aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);

        if (aimAngle < 0f)
        {
            aimAngle = Mathf.PI * 2 + aimAngle;
        }


        // 4
        var aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;
        // 5
        playerPosition = pseudoLauncher.transform.position;

        // 6

        if (!pseudoLauncher.GetComponent<psuedoLaunch>().launched)
        {
            pseudoLauncher.transform.localEulerAngles = new Vector3(0, 0, aimAngle * Mathf.Rad2Deg - transform.eulerAngles.z);
        }

        if (!ropeAttached)
        {
            SetCrosshairPosition(aimAngle);
        }
        else
        {
            crosshairSprite.enabled = false;
        }

        HandleInput();
        UpdateRopePositions();
        HandleRopeLength();
    }

    private void SetCrosshairPosition(float aimAngle)
    {
        if (!crosshairSprite.enabled)
        {
            crosshairSprite.enabled = true;
        }

        var x = pseudoLauncher.transform.position.x + 4.2f * Mathf.Cos(aimAngle);
        var y = pseudoLauncher.transform.position.y + 4.2f * Mathf.Sin(aimAngle);

        var crossHairPosition = new Vector3(x, y, 0);
        crosshair.transform.position = crossHairPosition;
    }

    private void HandleInput()
    {
        if (Input.GetMouseButton(clickNo))
        {
            if (ropeAttached) return;

            pseudoLauncher.GetComponent<SpriteRenderer>().enabled = true;

            if (pseudoLauncher.GetComponent<psuedoLaunch>().launched != true)
            {
                if (pseudoLauncher.GetComponent<psuedoLaunch>().readied)
                {
                    pseudoLauncher.GetComponent<psuedoLaunch>().launch();
                }
            }
        }
        if (Input.GetMouseButtonUp(clickNo))
        {
            ResetRope();
        }
    }

    public void CastRope(Vector3 aimDirection)
    {
        // 2
        ropeRenderer.enabled = true;

        var hit = Physics2D.Raycast(playerPosition, aimDirection, ropeMaxCastDistance, ropeLayerMask);

        // 3

        ropeAttached = true;
        if (!ropePositions.Contains(hit.point))
        {
            // 4
            // Jump slightly to distance the player a little from the ground after grappling to something.
            //transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 2f), ForceMode2D.Impulse);
            ropePositions.Add(hit.point);
            ropeJoint.distance = Vector2.Distance(playerPosition, hit.point) - 1f;
            ropeJoint.enabled = true;
            ropeHingeAnchorSprite.enabled = true;
        }
        // 5
    }

    // 6
    private void ResetRope()
    {
        ropeJoint.enabled = false;
        ropeAttached = false;
        ropeRenderer.positionCount = 2;
        ropeRenderer.SetPosition(0, pseudoLauncher.transform.position);
        ropeRenderer.SetPosition(1, pseudoLauncher.transform.position);
        ropePositions.Clear();
        ropeHingeAnchorSprite.enabled = false;
        pseudoLauncher.GetComponent<SpriteRenderer>().enabled = true;
    }

    private void UpdateRopePositions()
    {
        // 1
        if (!ropeAttached)
        {
            return;
        }

        // 2
        ropeRenderer.positionCount = ropePositions.Count + 1;

        // 3
        for (var i = ropeRenderer.positionCount - 1; i >= 0; i--)
        {
            if (i != ropeRenderer.positionCount - 1) // if not the Last point of line renderer
            {
                ropeRenderer.SetPosition(i, ropePositions[i]);

                // 4
                if (i == ropePositions.Count - 1 || ropePositions.Count == 1)
                {
                    var ropePosition = ropePositions[ropePositions.Count - 1];
                    if (ropePositions.Count == 1)
                    {
                        ropeHingeAnchorRb.transform.position = ropePosition;
                        if (!distanceSet)
                        {
                            ropeJoint.distance = Vector2.Distance(pseudoLauncher.transform.position, ropePosition);
                            distanceSet = true;
                        }
                    }
                    else
                    {
                        ropeHingeAnchorRb.transform.position = ropePosition;
                        if (!distanceSet)
                        {
                            ropeJoint.distance = Vector2.Distance(pseudoLauncher.transform.position, ropePosition);
                            distanceSet = true;
                        }
                    }
                }
                // 5
                else if (i - 1 == ropePositions.IndexOf(ropePositions.Last()))
                {
                    var ropePosition = ropePositions.Last();
                    ropeHingeAnchorRb.transform.position = ropePosition;
                    if (!distanceSet)
                    {
                        ropeJoint.distance = Vector2.Distance(pseudoLauncher.transform.position, ropePosition);
                        distanceSet = true;
                    }
                }
            }
            else
            {
                // 6
                ropeRenderer.SetPosition(i, pseudoLauncher.transform.position);
            }
        }
    }

    private void HandleRopeLength()
    {
        // 1
        if (Input.GetAxis("Vertical") >= 1f && ropeAttached && !isColliding)
        {
            ropeJoint.distance -= Time.deltaTime * climbSpeed;
        }
        /*
        else if (Input.GetAxis("Vertical") < 0f && ropeAttached)
        {
            ropeJoint.distance += Time.deltaTime * climbSpeed;
        }
        */
    }

    void OnTriggerStay2D(Collider2D colliderStay)
    {
        isColliding = true;
    }

    private void OnTriggerExit2D(Collider2D colliderOnExit)
    {
        isColliding = false;
    }
}