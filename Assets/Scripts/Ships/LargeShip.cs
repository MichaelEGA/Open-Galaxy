using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeShip : MonoBehaviour
{
    [Header("Ship Information")]
    public string allegiance;
    public string type;
    public string prefabName;
    public float loadTime;
    public string shipClass = "large";
    public bool scanned = false;
    public bool jumpingToHyperspace;
    public bool exitingHyperspace;
    public string cargo = "no cargo";
    public string thrustType = "thruster_blue_large";
    public string explosionType;
    public float shipLength;
    [HideInInspector] public bool explode;

    [Header("Ship Components")]
    [HideInInspector] public Rigidbody shipRigidbody;
    [HideInInspector] public Collider[] colliders;
    [HideInInspector] public Transform[] explosionPoints;

    [Header("Scene Reference")]
    public Scene scene;

    [Header("Ship Audio")]
    [HideInInspector] public Audio audioManager;
    [HideInInspector] public AudioSource engineAudioSource;

    [Header("Ship Ratings")]
    public float accelerationRating = 50;
    public float speedRating = 50;
    public float maneuverabilityRating = 50;
    public float hullRating = 50;
    public float systemsRating = 50;
    public float shieldRating = 50;
    public float laserFireRating = 50;
    public float laserRating = 50; 
    public float wepRating = 50;

    [Header("Ship Speed")]
    public float thrustSpeed = 20;
    public float speedInKms;
    [HideInInspector] public float thrustInput = 1;

    [Header("Ship Rotation")]
    public bool reducemaneuvarability;
    public bool avoidGimbalLock = false;
    public float avoidGimbalLockRight = 1;
    public float avoidGimbalLockUp = 1;
    [HideInInspector] public float pitchSpeed;
    [HideInInspector] public float pitchInput;
    [HideInInspector] public float pitchInputActual;
    [HideInInspector] public float turnSpeed;
    [HideInInspector] public float turnInput;
    [HideInInspector] public float turnInputActual;
    [HideInInspector] public float rollSpeed;
    [HideInInspector] public float rollInput;
    [HideInInspector] public float rollInputActual;
    [HideInInspector] public float maneuvarabilityActual;
    [HideInInspector] public bool spinShip;


    [Header("Docking")]
    [HideInInspector] public DockingPoint[] dockingPoints;
    public GameObject targetDockingPoint;
    public bool docking;

    [Header("Ship Levels")]
    public bool invincible = false;
    public bool cannotbedisabled = false;
    public bool isDisabled = false;
    public float hullLevel = 100;
    public float shieldLevel = 200;
    public float frontShieldLevel = 100;
    public float rearShieldLevel = 100;
    public float systemsLevel = 100;

    [Header("Ship Weapons")]
    public bool weaponsLock = false;
    [HideInInspector] public string laserColor = "red";

    [Header("Ship Controls")]
    public bool controlLock;
    [HideInInspector] public bool getNextTarget;
    [HideInInspector] public bool getNextEnemy;
    [HideInInspector] public bool getClosestEnemy;

    [Header("Ship Targetting")]
    public GameObject waypoint;
    public GameObject target;
    public string mode = "largeship";
    [HideInInspector] public SmallShip targetSmallShip;
    [HideInInspector] public LargeShip targetLargeShip;
    [HideInInspector] public Rigidbody targetRigidbody;
    [HideInInspector] public string targetAllegiance;
    [HideInInspector] public string targetName;
    [HideInInspector] public string targetType;
    [HideInInspector] public string targetPrefabName;
    [HideInInspector] public int targetNumber;
    [HideInInspector] public bool targetIsHostile;
    public float targetForward;
    public float targetRight;
    [HideInInspector] public float targetUp;
    public float targetDistance;
    [HideInInspector] public float targetSpeed;
    [HideInInspector] public float targetShield;
    [HideInInspector] public float targetHull;
    public float waypointForward;
    public float waypointRight;
    public float waypointUp;
    public float waypointDistance;
    [HideInInspector] public float targetPressedTime;

    [Header("Turrets")]
    public bool turretsLoaded;
    public Turret[] turrets;
    public string largeturret;
    public string smallturret;

    [Header("Collisions")]
    public Transform castPoint;

    [Header("Ship AI")]
    public List<string> aiTags;
    public bool aiEvade;
    public bool requestingTarget;
    public bool aiStarted;

    [Header("Ship Loading")]
    [HideInInspector] public bool loaded;

    [Header("Ship Task Pool")]
    public List<Task> tasks;

    // Update is called once per frame
    void Update()
    {
        //Start functions
        LargeShipFunctions.PrepareShip(this);
        TurretFunctions.LoadTurrets(this);
        
        //Input functions
        LargeShipFunctions.GetAIInput(this);

        //Targetting functions
        TargetingFunctions.GetTargetInfo_LargeShipAI(this);

        if (shipClass != "station" & shipClass != "turret" & shipClass != "buoy" & shipClass != "container")
        {
            //Ship movement functions
            LargeShipFunctions.CalculateThrustSpeed(this);
            LargeShipFunctions.CalculatePitchTurnRollSpeeds(this);
        }

        //Damage functions
        LargeShipFunctions.Explode(this);

        //Systems functions
        LargeShipFunctions.RestoreShipsSystems(this);
    }

    void FixedUpdate()
    {
        if (shipClass != "station" & shipClass != "turret" & shipClass != "buoy" & shipClass != "container")
        {
            LargeShipFunctions.MoveShip(this);
        }           
    }
}
