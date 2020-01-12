using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSetter : MonoBehaviour
{
    // Start is called before the first frame update
    RopeSystemL leftRope;
    RopeSystemR rightRope;
    public SpiderRope[] rope;
    private int index = 0;
    void Start()
    {
       leftRope = GetComponent<RopeSystemL>();
       rightRope = GetComponent<RopeSystemR>(); 
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Mouse0))
        // {
        //     index = index > rope.Length - 1 ? 0 : index;
        //     Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //     rope[index].setStart(worldPos);
        //     index++;
        // }

        
    }

    // private void HandleInputLeft()
    // {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         if (leftRope.ropeAttached) return;

    //         pseudoLauncher.GetComponent<SpriteRenderer>().enabled = true;

    //         if (pseudoLauncher.GetComponent<psuedoLaunch>().launched != true)
    //         {
    //             if (pseudoLauncher.GetComponent<psuedoLaunch>().readied)
    //             {
    //                 pseudoLauncher.GetComponent<psuedoLaunch>().launch();
    //             }
    //         }
    //     }
        // if (Input.GetMouseButtonUp(clickNo))
        // {
        //     ResetRope();
        // }
}

