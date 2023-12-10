using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreManager : MonoBehaviour
{
    public string currentLocation;
    public string[] availibleLocations;
    public bool running;

    public Vector3 playerPosition = new Vector3();
    public Quaternion playerRotation = new Quaternion();
    public string playerShipType = "lambdashuttle";
    public string playerShipName = "Star of the Outer Rim";
    public string playerAllegiance = "neutral";

    public float pressedTime;

    // Update is called once per frame
    void Update()
    {
        ExploreFunctions.ActivateExitMenu(this);
    }
}
