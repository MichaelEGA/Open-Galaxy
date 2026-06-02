using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script controls a ship by calling the appropriate functions from: Small Ship Functions, Small Ship Laser Functions, and Small Ship AI Functions
public class SmallShip : MonoBehaviour
{
    [Header("Key Reference")]
    [HideInInspector] public Scene scene;
    [HideInInspector] public OGInput ogInput;

    [Header("Ship Information")]
    [HideInInspector] public string allegiance; //Value set in inspector or by loading script
    [HideInInspector] public string type;
    [HideInInspector] public string shipClass;
    [HideInInspector] public string prefabName;
    [HideInInspector] public float loadTime;
    [HideInInspector] public float shipLength;
    [HideInInspector] public string thrustType;
    [HideInInspector] public bool exploded;
    [HideInInspector] public bool scanned = false;
    [HideInInspector] public bool jumpingToHyperspace;
    [HideInInspector] public bool exitingHyperspace;
    [HideInInspector] public string shieldType;
    [HideInInspector] public string cargo = "no cargo";
    [HideInInspector] public string explosionType;
    [HideInInspector] public string cockpitName;

    [Header("Ship Components")]
    [HideInInspector] public Rigidbody shipRigidbody;
    [HideInInspector] public Collider[] colliders;

    [Header("Ship Ratings")]
    [HideInInspector] public float accelerationRating = 50; //Value set in inspector or by loading script
    [HideInInspector] public float speedRating = 50; //Value set in inspector or by loading script
    [HideInInspector] public float maneuverabilityRating = 50; //Value set in inspector or by loading script
    [HideInInspector] public float hullRating = 50; //Value set in inspector or by loading script
    [HideInInspector] public float systemsRating = 50;
    [HideInInspector] public float shieldRating = 50; //Value set in inspector or by loading script
    [HideInInspector] public float laserFireRating = 50; //Value set in inspector or by loading script
    [HideInInspector] public float laserRating = 50; //Value set in inspector or by loading script
    [HideInInspector] public float wepRating = 50;//Value set in inspector or by loading script

    [Header("Ship Speed")]
    [HideInInspector] public float thrustSpeed = 70;
    [HideInInspector] public float thrustInput = 1;
    [HideInInspector] public float thrustTimeStamp;
    [HideInInspector] public bool wep;

    [Header("Ship Rotation")]
    [HideInInspector] public float pitchSpeed;
    [HideInInspector] public float pitchInput;
    [HideInInspector] public float turnSpeed;
    [HideInInspector] public float turnInput;
    [HideInInspector] public float rollSpeed;
    [HideInInspector] public float rollInput;
    [HideInInspector] public float rollInputActual;
    [HideInInspector] public bool automaticRotationTurnAround;
    [HideInInspector] public bool automaticRotationSpin;
    [HideInInspector] public bool messageSent;
    [HideInInspector] public bool spinShip;
    [HideInInspector] public bool avoidGimbalLock;

    [Header("Ship Levels")]
    [HideInInspector] public float systemsLevel = 100;
    [HideInInspector] public float hullLevel = 100;
    [HideInInspector] public float shieldLevel = 200;
    [HideInInspector] public float frontShieldLevel = 100;
    [HideInInspector] public float rearShieldLevel = 100;
    [HideInInspector] public float wepLevel;
    [HideInInspector] public float shieldRecharge; //Value set in inspector or by loading script
    [HideInInspector] public float shieldDischarge; //Value set in inspector or by loading script
    [HideInInspector] public float wepRecharge; //Value set in inspector or by loading script
    [HideInInspector] public float wepDischarge; //Value set in inspector or by loading script
    [HideInInspector] public bool invincible;
    [HideInInspector] public bool cannotbedisabled;
    [HideInInspector] public bool isDisabled;
    [HideInInspector] public bool warningSoundPlayed;

    [Header("Ship Power Distribution")]
    [HideInInspector] public string powerMode = "reset";
    [HideInInspector] public float laserPower = 100;
    [HideInInspector] public float enginePower = 100;
    [HideInInspector] public float shieldPower = 100;
    [HideInInspector] public float powerPressedTime;

    [Header("Ship Controls")]
    [HideInInspector] public bool controlLock = false;
    [HideInInspector] public bool invertUpDown;
    [HideInInspector] public bool invertLeftRight;
    [HideInInspector] public bool powerToShields;
    [HideInInspector] public bool powerToLasers;
    [HideInInspector] public bool powerToEngine;
    [HideInInspector] public bool resetPowerLevels;
    [HideInInspector] public bool fireWeapon;
    [HideInInspector] public bool rapidFire;
    [HideInInspector] public bool getNextTarget;
    [HideInInspector] public bool getNextEnemy;
    [HideInInspector] public bool getClosestEnemy;
    [HideInInspector] public bool selectTargetInFront;
    [HideInInspector] public bool toggleWeapons;
    [HideInInspector] public bool toggleWeaponNumber;
    [HideInInspector] public bool matchSpeed;
    [HideInInspector] public bool focusCamera;
    [HideInInspector] public bool fireCounterMeasures;

    [Header("Hyperspace")]
    [HideInInspector] public bool inHyperspace;

    [Header("Ship Audio")]
    [HideInInspector] public Audio audioManager;
    [HideInInspector] public AudioSource engineAudioSource;
    [HideInInspector] public string laserAudio;
    [HideInInspector] public string ionAudio = "weapon_ioncannon";
    [HideInInspector] public string plasmaAudio = "weapon_plasma";
    [HideInInspector] public string engineAudio;

    [Header("Ship Cameras Positions")]
    [HideInInspector] public GameObject cameraPosition;
    [HideInInspector] public GameObject followCameraPosition;
    [HideInInspector] public GameObject focusCameraPosition;

    [Header("Docking")]
    [HideInInspector] public GameObject targetDockingPoint;
    [HideInInspector] public DockingPoint dockingPoint;
    [HideInInspector] public bool docking;

    [Header("Ship Weapons")]
    [HideInInspector] public bool weaponsLock = false;
    [HideInInspector] public bool preventWeaponChange = false;
    [HideInInspector] public bool hasRapidFire = false;
    [HideInInspector] public string activeWeapon = "lasers";
    [HideInInspector] public string weaponMode = "single";
    [HideInInspector] public float laserCharge;
    [HideInInspector] public float ionCharge;
    [HideInInspector] public float plasmaCharge;
    [HideInInspector] public float weaponRechargeDelay;
    [HideInInspector] public bool laserRecharged;
    [HideInInspector] public bool ionRecharged;
    [HideInInspector] public bool plasmaRecharged;
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
    [HideInInspector] public GameObject waypoint;
    [HideInInspector] public GameObject target;
    [HideInInspector] public bool dontSelectLargeShips;
    [HideInInspector] public bool autoaim;
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
    [HideInInspector] public Vector3 interceptPoint;
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
    [HideInInspector] public List<string> aiTags;
    [HideInInspector] public string aiTargetingMode;
    [HideInInspector] public Vector3 aiTargetingErrorMargin = new Vector3(0, 0, 0);
    [HideInInspector] public float aiRetreatTime;
    [HideInInspector] public float aiAttackTime;
    [HideInInspector] public float healthSave;
    [HideInInspector] public bool withdraw;
    [HideInInspector] public bool isAI;
    [HideInInspector] public bool requestingTarget;
    [HideInInspector] public bool aiMatchSpeed;
    [HideInInspector] public bool aiStarted;
    [HideInInspector] public bool aiEvade;
    [HideInInspector] public bool boostIsActive;

    [Header("Formation Flying")]
    [HideInInspector] public SmallShip followTarget;
    [HideInInspector] public bool flyInFormation;
    [HideInInspector] public bool positionLocked;
    [HideInInspector] public float xFormationPos;
    [HideInInspector] public float yFormationPos;
    [HideInInspector] public float zFormationPos;

    [Header("Particle Effcets")]
    [HideInInspector] public GameObject smokeTrail;
    [HideInInspector] public ParticleSystem movementEffect;

    [Header("Ship Loading")]
    [HideInInspector] public bool loaded;

    [Header("Ship Collisions")]
    [HideInInspector] public bool isCurrentlyColliding;
    [HideInInspector] public bool isCurrentlyCollidingSmallShip;

    [Header("Systems")]
    [HideInInspector] public float restoreDelayTime;

    [Header("Wings")]
    [HideInInspector] public bool wingsOpen = true;
    [HideInInspector] public Transform[] wings;
    [HideInInspector] public GameObject wing01;
    [HideInInspector] public GameObject wing02;
    [HideInInspector] public GameObject wing03;
    [HideInInspector] public GameObject wing04;
    [HideInInspector] public GameObject wing01_open;
    [HideInInspector] public GameObject wing01_closed;
    [HideInInspector] public GameObject wing02_open;
    [HideInInspector] public GameObject wing02_closed;
    [HideInInspector] public GameObject wing03_open;
    [HideInInspector] public GameObject wing03_closed;
    [HideInInspector] public GameObject wing04_open;
    [HideInInspector] public GameObject wing04_closed;

    [Header("Ship Coroutine Tasks")]
    [HideInInspector] public List<Task> tasks;

    // Update is called once per frame
    void Update()
    {
        SmallShipFunctions.RunShipUpdateFunctions(this);
    }
    
    void FixedUpdate()
    {
        SmallShipFunctions.RunShipFixedUpdateFunctions(this);
    }

    void OnCollisionEnter(Collision collision)
    {
        DamageFunctions.StartCollision_SmallShip(this, collision.gameObject);

        Debug.Log("Collided with " + collision.gameObject.name + " " + collision.collider.gameObject.name);
    }

    void OnCollisionExit(Collision collision)
    {
        DamageFunctions.EndCollision_SmallShip(this);
    }
}
