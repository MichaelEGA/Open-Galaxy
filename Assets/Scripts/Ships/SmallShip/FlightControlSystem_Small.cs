using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script controls a ship by calling the appropriate functions from: Small Ship Functions, Small Ship Laser Functions, and Small Ship AI Functions
public class FlightControlSystem_Small : MonoBehaviour
{
    [Header("Key Reference")]
    public Ship ship;

    [Header("Ship Components")]
    public Rigidbody shipRigidbody;
    public Collider[] colliders;

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

    [Header("Ship Controls")]
    public bool controlLock = false;
    public bool invertUpDown;
    public bool invertLeftRight;
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
    public AudioSource engineAudioSource;
    public string engineAudio;

    [Header("Ship Cameras Positions")]
    public GameObject cameraPosition;
    public GameObject followCameraPosition;
    public GameObject focusCameraPosition;

    [Header("Docking")]
    public GameObject targetDockingPoint;
    public DockingPoint dockingPoint;
    public bool docking;

    public ParticleSystem movementEffect;

    [Header("Ship Loading")]
    public bool loaded;

    [Header("Ship Coroutine Tasks")]
    public List<Task> tasks;

    private void Start()
    {
        targetingSystem = GetComponent<TargetingSystem>();
        damageSystem = GetComponent<DamageSystem>();
        weaponManagement = GetComponent<WeaponManagement>();
        energyWeapon = GetComponent<EnergyWeapon>();
        torpedoSystem = GetComponent<TorpedoSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        //Input functions
        GetInput(smallShip);
        TurnShipAround(smallShip);
        SpinShip(smallShip);
        ControlLock(smallShip);

        //Start functions
        PrepareShip(smallShip);

        //Energy Management functions
        CalculatePower(smallShip);
        CalculateLevels(smallShip);

        //Ship movement functions
        MatchSpeed(smallShip);
        CalculateThrustSpeed(smallShip);
        CalculatePitchTurnRollSpeeds(smallShip);
        MovementEffect(smallShip);
        AudioFunctions.PlayEngineNoise_SmallShip(smallShip);
    }
    
    void FixedUpdate()
    {
        MoveShip(smallShip);
    }
}
