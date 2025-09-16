using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script controls a ship by calling the appropriate functions from: Small Ship Functions, Small Ship Laser Functions, and Small Ship AI Functions
public class SmallShip : MonoBehaviour
{
    [Header("Ship Information")]
    public string allegiance; //Value set in inspector or by loading script
    public string type;
    public string shipClass;
    public string prefabName;
    public float loadTime;
    public float shipLength;
    public string thrustType;
    public bool exploded;
    public bool scanned = false;
    public bool jumpingToHyperspace;
    public bool exitingHyperspace;
    public string shieldType;
    public string cargo = "no cargo";
    public string explosionType;

    [Header("Ship Components")]
    public Rigidbody shipRigidbody;
    public Collider[] colliders;

    [Header("Ship Audio")]
    [HideInInspector] public Audio audioManager;
    [HideInInspector] public AudioSource engineAudioSource;
    [HideInInspector] public string laserAudio;
    [HideInInspector] public string ionAudio = "weapon_ioncannon";
    [HideInInspector] public string plasmaAudio = "weapon_plasma";
    [HideInInspector] public string engineAudio;

    [Header("Scene Reference")]
    public Scene scene;

    [Header("Ship Cameras")]
    [HideInInspector] public GameObject mainCamera;
    [HideInInspector] public GameObject followCamera;
    [HideInInspector] public bool cameraAttached;
    [HideInInspector] public bool attachCameraToAI;
    [HideInInspector] public Vector3 cameraLocalPosition;
    [HideInInspector] public GameObject cockpitCamera;
    [HideInInspector] public GameObject cameraPosition;
    [HideInInspector] public GameObject followCameraPosition;
    [HideInInspector] public GameObject focusCameraPosition;

    [Header("Ship Ratings")]
    public float accelerationRating = 50; //Value set in inspector or by loading script
    public float speedRating = 50; //Value set in inspector or by loading script
    public float maneuverabilityRating = 50; //Value set in inspector or by loading script
    public float hullRating = 50; //Value set in inspector or by loading script
    public float systemsRating = 50;
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

    [Header("Controller Inputs")]
    [HideInInspector] public float controllerSenstivity = 0.05f;
    [HideInInspector] public float controllerPitch;
    [HideInInspector] public float controllerRoll;
    [HideInInspector] public float controllerTurn;
    [HideInInspector] public float controllerThrust;
    [HideInInspector] public bool keyboardAndMouse = true;

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
    [HideInInspector] public bool avoidGimbalLock;

    [Header("Docking")]
    public GameObject targetDockingPoint;
    public DockingPoint dockingPoint;
    public bool docking;

    [Header("Ship Power Distribution")]
    public string powerMode = "reset";
    public float laserPower = 100;
    public float enginePower = 100;
    public float shieldPower = 100;
    [HideInInspector] public float powerPressedTime;

    [Header("Ship Levels")]
    public float systemsLevel = 100;
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
    [HideInInspector] public bool cannotbedisabled;
    [HideInInspector] public bool isDisabled;

    [Header("Ship Controls")]
    public bool controlLock = false;
    public bool invertUpDown;
    public bool invertLeftRight;
    [HideInInspector] public bool powerToShields;
    [HideInInspector] public bool powerToLasers;
    [HideInInspector] public bool powerToEngine;
    [HideInInspector] public bool resetPowerLevels;
    [HideInInspector] public bool fireWeapon;
    [HideInInspector] public bool getNextTarget;
    [HideInInspector] public bool getNextEnemy;
    [HideInInspector] public bool getClosestEnemy;
    [HideInInspector] public bool selectTargetInFront;
    [HideInInspector] public bool toggleWeapons;
    [HideInInspector] public bool toggleWeaponNumber;
    [HideInInspector] public bool lookRight;
    [HideInInspector] public bool lookLeft;
    [HideInInspector] public bool matchSpeed;
    [HideInInspector] public bool focusCamera;
    [HideInInspector] public bool contextButton;
    [HideInInspector] public float toggleCameraPressTime;

    [Header("Ship Weapons")]
    public bool weaponsLock = false;
    public bool preventWeaponChange = false;
    public bool hasRapidFire = false;
    public string activeWeapon = "lasers";
    public string weaponMode = "single";
    [HideInInspector] public float toggleWeaponPressedTime;

    [HideInInspector] public GameObject laserParticleSystem;
    [HideInInspector] public GameObject laserMuzzleFlashParticleSystem;
    [HideInInspector] public GameObject laserCannon1;
    [HideInInspector] public GameObject laserCannon2;
    [HideInInspector] public GameObject laserCannon3;
    [HideInInspector] public GameObject laserCannon4;
    [HideInInspector] public string laserColor = "red"; //Value set in inspector or by loading script
    [HideInInspector] public float laserCycleNumber;
    [HideInInspector] public float laserPressedTime;
    [HideInInspector] public float laserModePressedTime;
    [HideInInspector] public bool laserfiring;

    [HideInInspector] public GameObject ionParticleSystem;
    [HideInInspector] public GameObject ionMuzzleFlashParticleSystem;
    [HideInInspector] public GameObject ionCannon1;
    [HideInInspector] public GameObject ionCannon2;
    [HideInInspector] public GameObject ionCannon3;
    [HideInInspector] public GameObject ionCannon4;
    [HideInInspector] public float ionCycleNumber;
    [HideInInspector] public float ionPressedTime;
    [HideInInspector] public float ionModePressedTime;
    [HideInInspector] public bool ionfiring;
    [HideInInspector] public bool hasIon;

    [HideInInspector] public GameObject plasmaParticleSystem;
    [HideInInspector] public GameObject plasmaMuzzleFlashParticleSystem;
    [HideInInspector] public GameObject plasmaCannon1;
    [HideInInspector] public GameObject plasmaCannon2;
    [HideInInspector] public GameObject plasmaCannon3;
    [HideInInspector] public GameObject plasmaCannon4;
    [HideInInspector] public float plasmaCycleNumber;
    [HideInInspector] public float plasmaPressedTime;
    [HideInInspector] public float plasmaModePressedTime;
    [HideInInspector] public bool plasmafiring;
    [HideInInspector] public bool hasPlasma;

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
    public bool dontSelectLargeShips;
    public bool autoaim;
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
    public List<string> aiTags;
    [HideInInspector] public string aiTargetingMode;
    [HideInInspector] public Vector3 aiTargetingErrorMargin = new Vector3(0, 0, 0);
    [HideInInspector] public float aiRetreatTime;
    [HideInInspector] public float aiAttackTime;
    [HideInInspector] public float healthSave;
    [HideInInspector] public bool withdraw;
    [HideInInspector] public bool isAI;
    public bool requestingTarget;
    [HideInInspector] public bool aiMatchSpeed;
    [HideInInspector] public bool aiStarted;
    [HideInInspector] public bool aiEvade;
    [HideInInspector] public bool boostIsActive;

    [Header("Formation Flying")]
    public SmallShip followTarget;
    public bool flyInFormation;
    public bool positionLocked;
    public float xFormationPos;
    public float yFormationPos;
    public float zFormationPos;

    [Header("Particle Effcets")]
    [HideInInspector] public GameObject smokeTrail;
    [HideInInspector] public ParticleSystem movementEffect;

    [Header("Ship Loading")]
    [HideInInspector] public bool loaded;

    [Header("Ship Collisions")]
    [HideInInspector] public bool isCurrentlyColliding;

    [Header("Ship Cockpit")]
    public bool inHyperspace;
    [HideInInspector] public bool cockpitShake;
    [HideInInspector] public float shakeMagnitude = 0.1f;
    [HideInInspector] public float movementTime = 0.1f;
    [HideInInspector] public float randomisationX = 0;
    [HideInInspector] public float randomisationY = 0;
    [HideInInspector] public string cockpitName;
    [HideInInspector] public GameObject cockpitAnchor;
    [HideInInspector] public GameObject cockpit;
    [HideInInspector] public bool cockpitDamageShake;
    [HideInInspector] public bool cockpitSpeedShake;
    [HideInInspector] public float speedShakeMagnitude;
    [HideInInspector] public Vector3 basePosition = new Vector3(0,0,0);
    [HideInInspector] public AudioSource cockpitAudioSource;
    [HideInInspector] public Vector3 currentPosition;
    [HideInInspector] public Quaternion currentRotation;

    [Header("Systems")]
    public float restoreDelayTime;

    [Header("Wings")]
    public bool wingsOpen = true;
    public Transform[] wings;
    public GameObject wing01;
    public GameObject wing02;
    public GameObject wing03;
    public GameObject wing04;
    public GameObject wing01_open;
    public GameObject wing01_closed;
    public GameObject wing02_open;
    public GameObject wing02_closed;
    public GameObject wing03_open;
    public GameObject wing03_closed;
    public GameObject wing04_open;
    public GameObject wing04_closed;

    [Header("Ship Coroutine Tasks")]
    public List<Task> tasks;

    // Update is called once per frame
    void Update()
    {
        //Input functions
        SmallShipFunctions.GetAIInput(this);
        SmallShipFunctions.DetectInputType(this);
        SmallShipFunctions.GetKeyboardAndMouseInput(this);
        SmallShipFunctions.GetControllerInput(this);
        SmallShipFunctions.TurnShipAround(this);
        SmallShipFunctions.SpinShip(this);
        SmallShipFunctions.ControlLock(this);

        //Start functions
        SmallShipFunctions.PrepareShip(this);
        SmallShipFunctions.LoadLaserParticleSystem(this);

        //Energy Management functions
        SmallShipFunctions.CalculatePower(this);
        SmallShipFunctions.CalculateLevels(this);

        //Ship movement functions
        SmallShipFunctions.MatchSpeed(this);
        SmallShipFunctions.CalculateThrustSpeed(this);
        SmallShipFunctions.CalculatePitchTurnRollSpeeds(this);
        SmallShipFunctions.MovementEffect(this);
        AudioFunctions.PlayEngineNoise(this);

        //Targeting Functions
        TargetingFunctions.RunPlayerTargetingFunctions(this);
        TargetingFunctions.GetTargetInfo_SmallShip(this);

        //Weapon functions
        SmallShipFunctions.ToggleWeapons(this);

        //Laser functions
        LaserFunctions.ToggleWeaponMode(this);
        LaserFunctions.InitiateFiringPlayer(this);

        //Ion Cannon functions
        IonFunctions.ToggleWeaponMode(this);
        IonFunctions.InitiateFiringPlayer(this);

        //Ion Cannon functions
        PlasmaFunctions.ToggleWeaponMode(this);
        PlasmaFunctions.InitiateFiringPlayer(this);

        //Torpedo functions
        TorpedoFunctions.EstablishLockOn(this);
        TorpedoFunctions.FireTorpedoPlayer(this);
        TorpedoFunctions.ToggleWeaponMode(this);

        //Damage functions
        DamageFunctions.TakeCollisionDamage_SmallShip(this);
        DamageFunctions.SmokeTrail_SmallShip(this);
        DamageFunctions.Explode_SmallShip(this);

        //Systems functions
        DamageFunctions.RestoreShipsSystems_SmallShip(this);

        //Cockpit Functions
        CockpitFunctions.RunCockpitFunctions(this);
        
    }
    
    void FixedUpdate()
    {
        SmallShipFunctions.MoveShip(this);
        CockpitFunctions.FollowCamera(this);
    }

    void OnCollisionEnter(Collision collision)
    {
        DamageFunctions.StartCollision_SmallShip(this, collision.gameObject);
    }

    void OnCollisionExit(Collision collision)
    {
        DamageFunctions.EndCollision_SmallShip(this);
    }
}
