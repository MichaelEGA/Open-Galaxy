using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torpedo : MonoBehaviour
{
    [Header("Key References")]
    public SmallShip attackingShip;
    public SmallShip targetSmallShip;
    public LargeShip targetLargeShip;
    [HideInInspector] public Rigidbody torpedoRigidbody;
    [HideInInspector] public Audio audioManager;

    [Header("Key Properties")]
    public string type;
    public string color;
    public float damagePower;
    public float thrustSpeed;
    public float pitchSpeed;
    public float rollSpeed;
    public float turnSpeed;
    public float destroyAfter;
    public float fireTime;

    [Header("Torpedo Audio")]
    [HideInInspector] public string launchAudio;
    [HideInInspector] public string explosionAudio;

    [Header("Target Information")]
    public GameObject target;
    [HideInInspector] public float targetForward;
    [HideInInspector] public float targetUp;
    [HideInInspector] public float targetRight;

    [Header("Torpedo Inputs")]
    [HideInInspector] public float pitchInput;
    [HideInInspector] public float rollInput;
    [HideInInspector] public float turnInput;

    [Header("Counter Measures")]
    public bool targetWarned;
    public float pressedTime;
    public Hud hud;

    private void Start()
    {
        TorpedoSystemFunctions.IgnoreColliders(this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TorpedoSystemFunctions.GetTargetInfo(this);
        TorpedoSystemFunctions.AngleTowardsTarget(this);
        TorpedoSystemFunctions.TorpedoMove(this);
        TorpedoSystemFunctions.DestroyCloseToTarget(this);
        TorpedoSystemFunctions.DestroyAfterTime(this);
        TorpedoSystemFunctions.CounterMeasures(this);
    }

    void OnCollisionEnter(Collision collision)
    {
        TorpedoSystemFunctions.RunCollisionEvent(this, collision);
    }
}
