using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    public SmallShip smallShip;


    public GameObject waypoint;
    public GameObject target;
    public bool dontSelectLargeShips;
    public bool autoaim;
    public SmallShip targetSmallShip;
    public LargeShip targetLargeShip;
    public Rigidbody targetRigidbody;
    public string targetAllegiance;
    public string targetName;
    public string targetType;
    public string targetPrefabName;
    public int targetNumber;
    public bool targetIsHostile;
    public float targetForward;
    public float targetRight;
    public float targetUp;
    public float targetDistance;
    public float targetSpeed;
    public float targetShield;
    public float targetHull;
    public Vector3 interceptPoint;
    public float interceptForward;
    public float interceptRight;
    public float interceptUp;
    public float interceptDistance;
    public float waypointForward;
    public float waypointRight;
    public float waypointUp;
    public float waypointDistance;
    public float targetPressedTime;
    public int numberTargeting = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        smallShip = GetComponent<SmallShip>();
    }

    // Update is called once per frame
    void Update()
    {
        //Targeting Functions
        TargetingSystemFunctions.RunPlayerTargetingFunctions(smallShip);
        TargetingSystemFunctions.GetTargetInfo_SmallShip(smallShip);
    }
}
