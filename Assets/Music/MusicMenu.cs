using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMenu : MonoBehaviour {
    public AK.Wwise.Event menuStart;
    public GameObject wwiseObj;

     public void Start() {

        menuStart.Post(wwiseObj);

    }
}
