using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreManager : MonoBehaviour
{
    //Player Data
    public Vector3 playerPosition = new Vector3();
    public Quaternion playerRotation = new Quaternion();
    public string playerShipType = "lambdashuttle";
    public string playerShipName = "Star of the Outer Rim";
    public string playerAllegiance = "neutral";

    //Location and Hyperspace
    public string currentLocation;
    public string[] availibleLocations;
    public string selectedLocation = "none";
    public bool hyperspace = false;

    public List<Vector3> shipPositions;
    public List<float> shipClearance;

    public float pressedTime;

    public bool running;

    // Update is called once per frame
    void Update()
    {
        ExploreFunctions.ActivateExitMenu(this);
        ExploreFunctions.SelectNextJumpLocation(this);
        ExploreFunctions.ActivateHyperspace(this);
    }
}
