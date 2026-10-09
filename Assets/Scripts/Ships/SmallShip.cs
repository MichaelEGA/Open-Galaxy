using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script controls a ship by calling the appropriate functions from: Small Ship Functions, Small Ship Laser Functions, and Small Ship AI Functions
public class SmallShip : MonoBehaviour
{
    [Header("Key Reference")]
    public Scene scene;
    public OGInput ogInput;
    public WeaponManagement weaponManagement;
    public EnergyWeapon energyWeapon;
    public TorpedoSystem torpedoSystem;
    public TargetingSystem targetingSystem;

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
    public string cockpitName;

    [Header("Ship Components")]
    public Rigidbody shipRigidbody;
    public Collider[] colliders;

    [Header("Ship Ratings")]
    public float accelerationRating = 50; //Value set in inspector or by loading script
    public float speedRating = 50; //Value set in inspector or by loading script
    public float maneuverabilityRating = 50; //Value set in inspector or by loading script
    public float hullRating = 50; //Value set in inspector or by loading script
    public float systemsRating = 50;
    public float shieldRating = 50; //Value set in inspector or by loading script
    public float energyWeaponFireRating = 50; //Value set in inspector or by loading script
    public float energyWeaponRating = 50; //Value set in inspector or by loading script
    public float wepRating = 50;//Value set in inspector or by loading script

    [Header("Ship Speed")]
    public float thrustSpeed = 70;
    public float thrustInput = 1;
    public float thrustTimeStamp;
    public bool wep;

    [Header("Ship Rotation")]
    public float pitchSpeed;
    public float pitchInput;
    public float turnSpeed;
    public float turnInput;
    public float rollSpeed;
    public float rollInput;
    public float rollInputActual;
    public bool automaticRotationTurnAround;
    public bool automaticRotationSpin;
    public bool messageSent;
    public bool spinShip;
    public bool avoidGimbalLock;

    [Header("Ship Levels")]
    public float systemsLevel = 100;
    public float hullLevel = 100;
    public float shieldLevel = 200;
    public float frontShieldLevel = 100;
    public float rearShieldLevel = 100;
    public float wepLevel;
    public float shieldRecharge; //Value set in inspector or by loading script
    public float shieldDischarge; //Value set in inspector or by loading script
    public float wepRecharge; //Value set in inspector or by loading script
    public float wepDischarge; //Value set in inspector or by loading script
    public bool invincible;
    public bool cannotbedisabled;
    public bool isDisabled;
    public bool warningSoundPlayed;

    [Header("Ship Power Distribution")]
    public string powerMode = "reset";
    public float energyWeaponPower = 100;
    public float enginePower = 100;
    public float shieldPower = 100;
    public float energyWeaponCharge;
    public float powerPressedTime;

    [Header("Ship Controls")]
    public bool controlLock = false;
    public bool invertUpDown;
    public bool invertLeftRight;
    public bool powerToShields;
    public bool powerToLasers;
    public bool powerToEngine;
    public bool resetPowerLevels;
    public bool fireWeapon;
    public bool rapidFire;
    public bool getNextTarget;
    public bool getNextEnemy;
    public bool getClosestEnemy;
    public bool selectTargetInFront;
    public bool matchSpeed;
    public bool focusCamera;
    public bool fireCounterMeasures;

    [Header("Hyperspace")]
    public bool inHyperspace;

    [Header("Ship Audio")]
    public Audio audioManager;
    public AudioSource engineAudioSource;
    public string laserAudio;
    public string ionAudio = "weapon_ioncannon";
    public string plasmaAudio = "weapon_plasma";
    public string engineAudio;

    [Header("Ship Cameras Positions")]
    public GameObject cameraPosition;
    public GameObject followCameraPosition;
    public GameObject focusCameraPosition;

    [Header("Docking")]
    public GameObject targetDockingPoint;
    public DockingPoint dockingPoint;
    public bool docking;

    [Header("Ship AI")]
    public List<string> aiTags;
    public string aiTargetingMode;
    public Vector3 aiTargetingErrorMargin = new Vector3(0, 0, 0);
    public float aiRetreatTime;
    public float aiAttackTime;
    public float healthSave;
    public bool withdraw;
    public bool isAI;
    public bool requestingTarget;
    public bool aiMatchSpeed;
    public bool aiStarted;
    public bool aiEvade;
    public bool boostIsActive;

    [Header("Formation Flying")]
    public SmallShip followTarget;
    public bool flyInFormation;
    public bool positionLocked;
    public float xFormationPos;
    public float yFormationPos;
    public float zFormationPos;

    [Header("Particle Effcets")]
    public GameObject smokeTrail;
    public ParticleSystem movementEffect;

    [Header("Ship Loading")]
    public bool loaded;

    [Header("Ship Collisions")]
    public bool isCurrentlyColliding;
    public bool isCurrentlyCollidingSmallShip;

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

    private void Start()
    {
        weaponManagement = GetComponent<WeaponManagement>();
        energyWeapon = GetComponent<EnergyWeapon>();
        torpedoSystem = GetComponent<TorpedoSystem>();
        targetingSystem = GetComponent<TargetingSystem>();
    }

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
        DamageSystemFunctions.StartCollision_SmallShip(this, collision.gameObject);

        Debug.Log("Collided with " + collision.gameObject.name + " " + collision.collider.gameObject.name + "at point" + collision.gameObject.transform.localPosition);
    }

    void OnCollisionExit(Collision collision)
    {
        DamageSystemFunctions.EndCollision_SmallShip(this);
    }
}
