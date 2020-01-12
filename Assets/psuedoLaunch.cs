using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class psuedoLaunch : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject mech;

    public bool launched;
    private bool retract;
    private float length;
    public bool readied;

    public GameObject direction;

    public bool left;

    private int scan_size = 80;

    Collider2D[] check_colliders = new Collider2D[1];
    ContactFilter2D collider_filter = new ContactFilter2D();

    void Start()
    {
        collider_filter = collider_filter.NoFilter();
    }

    // Update is called once per frame
    void Update()
    {
        if (launched)
        {
            GetComponent<CircleCollider2D>().enabled = true;

            if (!retract)
            {
                if (length <= 30f)
                {
                    length = GetComponent<SpriteRenderer>().size.x;
                    length += 1f;
                    GetComponent<SpriteRenderer>().size = new Vector2(length, 1);

                        GetComponent<CircleCollider2D>().offset = new Vector2(length, 0);
                                        check_colliders = new Collider2D[scan_size];

                    for (int setVar = 0; setVar < scan_size; setVar++)
                    {
                        check_colliders[setVar] = null;
                    }

                    gameObject.GetComponent<Collider2D>().OverlapCollider(collider_filter, check_colliders);

                    for (int checkVar = 0; checkVar < scan_size; checkVar++)
                    {
                        if (check_colliders[checkVar] != null)
                        {
                            if (check_colliders[checkVar].gameObject.layer == 8)
                            {
                                var orientation = direction.transform.position - transform.position;
                                var CastAngle = Mathf.Atan2(orientation.y, orientation.x);

                                if (CastAngle < 0f)
                                {
                                    CastAngle = Mathf.PI * 2 + CastAngle;
                                }

                                GetComponent<SpriteRenderer>().enabled = false;
                                if (left)
                                {
                                    mech.GetComponent<RopeSystem>().CastRopeLeft(Quaternion.Euler(0, 0, CastAngle * Mathf.Rad2Deg) * new Vector2(1, 0));
                                }
                                else
                                {
                                    mech.GetComponent<RopeSystem>().CastRopeRight(Quaternion.Euler(0, 0, CastAngle * Mathf.Rad2Deg) * new Vector2(1, 0));
                                }
                                retract = true;
                            }
                        }
                    }
                }
                else
                {
                    retract = true;
                }
            }
            else
            {

                if (length >= 1)
                {
                    length = GetComponent<SpriteRenderer>().size.x;
                    length -= 4f;
                    GetComponent<SpriteRenderer>().size = new Vector2(length, 1);
                }
                else
                {
                    retract = false;
                    readied = true;

                    launched = false;
                }
            }
        }
        else if (length != 1)
        {
            length = 1;
            GetComponent<SpriteRenderer>().size = new Vector2(length, 1);
            GetComponent<CircleCollider2D>().offset = new Vector2(0, 0);
            GetComponent<CircleCollider2D>().enabled = false;
            readied = true;
        }
    }

    public void launch()
    {
        if (!launched)
        {
            launched = true;
        }
    }
}
