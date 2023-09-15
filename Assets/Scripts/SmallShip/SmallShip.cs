using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script controls a ship by calling the appropriate functions from: Small Ship Functions, Small Ship Laser Functions, and Small Ship AI Functions
public class SmallShip : MonoBehaviour
{
    [Header("Ship Information")]
    public string allegiance; //Value set in inspector or by loading script
    public string type;
    public string prefabName;
    public float loadTime;
    public string thrustType;
    public bool exploded;

    [Header("Ship Components")]
    public Rigidbody shipRigidbody;
    public Collider[] colliders;

    [Header("Ship Audio")]
    [HideInInspector] public Audio audioManager;
    [HideInInspector] public AudioSource engineAudioSource;
    [HideInInspector] public string laserAudio;
    [HideInInspector] public string engineAudio;

    [Header("Ship LODs")]
    public Scene scene;
    public GameObject[] LODs;
    [HideInInspector] public GameObject currentLOD;
    [HideInInspector] public float distanceToPlayer;
    [HideInInspector] public float savedPlayerDistance;

    [Header("Ship Cameras")]
    [HideInInspector] public GameObject mainCamera;
    [HideInInspector] public bool cameraAttached;
    [HideInInspector] public bool attachCameraToAI;
    [HideInInspector] public Vector3 cameraLocalPosition;
    [HideInInspector] public GameObject cockpitCamera;
    [HideInInspector] public GameObject cameraPosition;

    [Header("Ship Ratings")]
    public float accelerationRating = 50; //Value set in inspector or by loading script
    public float speedRating = 50; //Value set in inspector or by loading script
    public float maneuverabilityRating = 50; //Value set in inspector or by loading script
    public float hullRating = 50; //Value set in inspector or by loading script
    public float shieldRating = 50; //Value set in inspector or by loading script
    public float laserFireRating = 50; //Value set in inspector or by loading script
    public float laserRating = 50; //Value set in inspector or by loading script
    public float wepRating = 50;//Value set in inspector or by loading script

    [Header("Ship Speed")]
    public float thrustSpeed = 70;
    public float speedInKms;
    [HideInInspector] public float thrustInput = 1;
    [HideInInspector] public float thrustTimeStamp;
    [HideInInspector] public bool wep;

    [Header("Ship Rotation")]
    [HideInInspector] public float pitchSpeed;
    [HideInInspector] public float pitchInput;
    [HideInInspector] public float pitchInputActual;
    [HideInInspector] public float turnSpeed;
    [HideInInspector] public float turnInput;
    [HideInInspector] public float turnInputActual;
    [HideInInspector] public float rollSpeed;
    [HideInInspector] public float rollInput;
    [HideInInspector] public float rollInputActual;
    [HideInInspector] public bool automaticRotationTurnAround;
    [HideInInspector] public bool automaticRotationSpin;
    [HideInInspector] public bool messageSent;
    [HideInInspector] public bool spinShip;

    [Header("Ship Power Distribution")]
    public string powerMode = "reset";
    public float laserPower = 100;
    public float enginePower = 100;
    public float shieldPower = 100;
    [HideInInspector] public float powerPressedTime;

    [Header("Ship Levels")]
    public float hullLevel = 100;
    public float shieldLevel = 200;
    public float frontShieldLevel = 100;
    public float rearShieldLevel = 100;
    public float wepLevel;
    [HideInInspector] public float shieldRecharge; //Value set in inspector or by loading script
    [HideInInspector] public float shieldDischarge; //Value set in inspector or by loading script
    [HideInInspector] public float wepRecharge; //Value set in inspector or by loading script
    [HideInInspector] public float wepDischarge; //Value set in inspector or by loading script
    [HideInInspector] public bool invincible;

    [Header("Ship Controls")]
    public bool keyboadAndMouse = true;
    public bool invertUpDown;
    public bool invertLeftRight;
    [HideInInspector] public float controllerSenstivity = 0.05f;
    [HideInInspector] public float controllerPitch;
    [HideInInspector] public float controllerRoll;
    [HideInInspector] public float controllerTurn;
    [HideInInspector] public float controllerThrust;
    [HideInInspector] public bool powerToShields;
    [HideInInspector] public bool powerToLasers;
    [HideInInspector] public bool powerToEngine;
    [HideInInspector] public bool resetPowerLevels;
    [HideInInspector] public bool fireWeapon;
    [HideInInspector] public bool getNextTarget;
    [HideInInspector] public bool getNextEnemy;
    [HideInInspector] public bool getClosestEnemy;
    [HideInInspector] public bool toggleWeapons;
    [HideInInspector] public bool toggleWeaponNumber;
    [HideInInspector] public bool lookRight;
    [HideInInspector] public bool lookLeft;
    [HideInInspector] public bool matchSpeed;
    [HideInInspector] public bool contextButton;

    [Header("Ship Weapons")]
    public bool weaponsLock = false;
    public string activeWeapon = "lasers";
    public string weaponMode = "single";
    [HideInInspector] public float toggleWeaponPressedTime;

    [HideInInspector] public GameObject laserParticleSystem;
    [HideInInspector] public GameObject laserCannon1;
    [HideInInspector] public GameObject laserCannon2;
    [HideInInspector] public GameObject laserCannon3;
    [HideInInspector] public GameObject laserCannon4;
    [HideInInspector] public string laserColor = "red"; //Value set in inspector or by loading script
    [HideInInspector] public float laserCycleNumber;
    [HideInInspector] public float laserPressedTime;
    [HideInInspector] public float laserModePressedTime;
    [HideInInspector] public bool laserfiring;

    [HideInInspector] public GameObject torpedoTube1;
    [HideInInspector] public GameObject torpedoTube2;
    [HideInInspector] public GameObject torpedoTube3;
    [HideInInspector] public GameObject torpedoTube4;
    [HideInInspector] public string torpedoType = "proton torpedo";
    [HideInInspector] public float torpedoNumber = 0;
    [HideInInspector] public float torpedoPressedTime;
    [HideInInspector] public float torpedoLockOnTime;
    [HideInInspector] public int torpedoCycleNumber;
    [HideInInspector] public bool hasTorpedos;
    [HideInInspector] public bool torpedoLockingOn;
    [HideInInspector] public bool torpedoLockedOn;

    [Header("Ship Targetting")]
    public GameObject waypoint;
    public GameObject target;
    [HideInInspector] public SmallShip targetSmallShip;
    [HideInInspector] public LargeShip targetLargeShip;
    [HideInInspector] public Rigidbody targetRigidbody;
    [HideInInspector] public string targetAllegiance;
    [HideInInspector] public string targetName;
    [HideInInspector] public string targetType;
    [HideInInspector] public string targetPrefabName;
    [HideInInspector] public int targetNumber;
    [HideInInspector] public bool targetIsHostile;
    [HideInInspector] public float targetForward;
    [HideInInspector] public float targetRight;
    [HideInInspector] public float targetUp;
    [HideInInspector] public float targetDistance;
    [HideInInspector] public float targetSpeed;
    [HideInInspector] public float targetShield;
    [HideInInspector] public float targetHull;
    [HideInInspector] public float interceptForward;
    [HideInInspector] public float interceptRight;
    [HideInInspector] public float interceptUp;
    [HideInInspector] public float interceptDistance;
    [HideInInspector] public float waypointForward;
    [HideInInspector] public float waypointRight;
    [HideInInspector] public float waypointUp;
    [HideInInspector] public float waypointDistance;
    [HideInInspector] public float targetPressedTime;
    [HideInInspector] public int numberTargeting = 0;

    [Header("Ship AI")]
    public bool isAI; //Value set in inspector or by loading script
    public string aiMode;
    public string aiOverideMode = "none";
    public string aiSkillLevel; //Three levels available: easy, medium, hard
    [HideInInspector] public float aiAttackTime = 30;
    [HideInInspector] public float aiRetreatTime;
    [HideInInspector] public float aiSpeedWhileTurning = 1.5f; //i.e. how much the ship slows to do a sharper turn
    [HideInInspector] public float aiPitchInput;
    [HideInInspector] public float aiTurnInput;
    [HideInInspector] public float aiRollInput;
    [HideInInspector] public float healthSave;
    [HideInInspector] public bool withdraw;

    [Header("Particle Effcets")]
    [HideInInspector] public GameObject smokeTrail;
    [HideInInspector] public ParticleSystem movementEffect;

    [Header("Ship Loading")]
    [HideInInspector] public bool loaded;

    [Header("Ship Collisions")]
    [HideInInspector] public bool isCurrentlyColliding;

    [Header("Ship Cockpit")]
    [HideInInspector] public bool cockpitShake;
    [HideInInspector] public float shakeMagnitude = 0.1f;
    [HideInInspector] public string cockpitName;
    [HideInInspector] public GameObject cockpitAnchor;
    [HideInInspector] public GameObject cockpit;
    [HideInInspector] public bool cockpitDamageShake;
    [HideInInspector] public bool cockpitSpeedShake;
    [HideInInspector] public float speedShakeMagnitude;
    [HideInInspector] public Vector3 basePosition = new Vector3(0,0,0);
    [HideInInspector] public AudioSource cockpitAudioSource;

    // Update is called once per frame
    void Update()
    {
        //Input functions
        SmallShipFunctions.GetAIInput(this);
        SmallShipFunctions.GetKeyboardAndMouseInput(this);
        SmallShipFunctions.GetGameControllerInput(this);
        SmallShipFunctions.DetectInputType(this);
        SmallShipFunctions.TurnShipAround(this);
        SmallShipFunctions.SpinShip(this);

        //Start functions
        SmallShipFunctions.PrepareShip(this);

        //Camera Functions
        SmallShipFunctions.SetMainCamera(this);

        //LOD functions
        SmallShipFunctions.LODCheck(this);

        //Energy Management functions
        SmallShipFunctions.CalculatePower(this);
        SmallShipFunctions.CalculateLevels(this);

        //Ship movement functions
        SmallShipFunctions.MatchSpeed(this);
        SmallShipFunctions.CalculateThrustSpeed(this);
        SmallShipFunctions.CalculatePitchTurnRollSpeeds(this);
        SmallShipFunctions.MovementEffect(this);
        AudioFunctions.PlayEngineNoise(this);

        //Targetting functions
        TargetingFunctions.GetClosestEnemy(this);
        TargetingFunctions.GetNextEnemy(this);
        TargetingFunctions.GetNextTarget(this);
        TargetingFunctions.GetTargetInfo(this);

        //Weapon functions
        SmallShipFunctions.ToggleWeapons(this);

        //Laser functions
        LaserFunctions.ToggleWeaponMode(this);
        LaserFunctions.SetCannons(this);
        LaserFunctions.InitiateFiring(this);

        //Torpedo functions
        TorpedoFunctions.EstablishLockOn(this);
        TorpedoFunctions.FireTorpedo(this);
        TorpedoFunctions.ToggleWeaponMode(this);

        //Damage functions
        SmallShipFunctions.TakeCollisionDamage(this);
        SmallShipFunctions.SmokeTrail(this);
        SmallShipFunctions.Explode(this);

        //Cockpit Functions
        SmallShipFunctions.ActivateCockpit(this);
        SmallShipFunctions.RunCockpitShake(this);
        SmallShipFunctions.CockpitCameraMovement(this);
        SmallShipFunctions.CockpitAnchorRotation(this);
    }
    
    void FixedUpdate()
    {
        SmallShipFunctions.MoveShip(this);
    }

    void OnCollisionEnter(Collision collision)
    {
        SmallShipFunctions.StartCollision(this, collision.gameObject);
    }

    void OnCollisionExit(Collision collision)
    {
        SmallShipFunctions.EndCollision(this);
    }
}
