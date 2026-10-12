using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public bool focusCamera;
    public bool matchSpeed;

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

    [Header("Movement Effect")]
    public ParticleSystem movementEffect;

    private void Start()
    {
        ship = GetComponent<Ship>();
    }

    // Update is called once per frame
    void Update()
    {
        //Input functions
        FlightControlSystem_SmallFunctions.GetInput(this);
        FlightControlSystem_SmallFunctions.TurnShipAround(this);
        FlightControlSystem_SmallFunctions.SpinShip(this);
        FlightControlSystem_SmallFunctions.ControlLock(this);

        //Start functions
        FlightControlSystem_SmallFunctions.PrepareShip(this);

        //Ship movement functions
        FlightControlSystem_SmallFunctions.MatchSpeed(this);
        FlightControlSystem_SmallFunctions.CalculateThrustSpeed(this);
        FlightControlSystem_SmallFunctions.CalculatePitchTurnRollSpeeds(this);
        FlightControlSystem_SmallFunctions.MovementEffect(this);
        AudioFunctions.PlayEngineNoise_Ship(ship);
    }
    
    void FixedUpdate()
    {
        FlightControlSystem_SmallFunctions.MoveShip(this);
    }
}
