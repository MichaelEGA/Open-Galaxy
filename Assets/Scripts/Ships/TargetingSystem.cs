using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    public FlightControlSystem_Small flightControlSystem;

    [Header("Key Reference")]
    public Ship ship;

    [Header("Target Information")]
    public GameObject target;
    public Ship targetShip;
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

    [Header("Intercept Point Information")]
    public Vector3 interceptPoint;
    public float interceptForward;
    public float interceptRight;
    public float interceptUp;
    public float interceptDistance;

    [Header("Waypoint Information")]
    public GameObject waypoint;
    public float waypointForward;
    public float waypointRight;
    public float waypointUp;
    public float waypointDistance;
    public float targetPressedTime;
    public int numberTargeting = 0;

    [Header("Controls")]
    public bool getNextTarget;
    public bool getNextEnemy;
    public bool getClosestEnemy;
    public bool selectTargetInFront;

    [Header("Misc Values")]
    public bool dontSelectLargeShips;
    public bool autoaim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flightControlSystem = GetComponent<FlightControlSystem_Small>();
        TargetingSystemFunctions.CreateWaypoint_SmallShipPlayer(flightControlSystem);
    }

    // Update is called once per frame
    void Update()
    {
        TargetingSystemFunctions.GetInput(this);
        TargetingSystemFunctions.RunPlayerTargetingFunctions(flightControlSystem);
        TargetingSystemFunctions.GetTargetInfo_SmallShip(flightControlSystem);
    }
}
