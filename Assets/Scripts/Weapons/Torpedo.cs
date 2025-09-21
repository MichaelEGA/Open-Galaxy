using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torpedo : MonoBehaviour
{
    [Header("Key References")]
    public SmallShip attackingShip;
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

    private void Start()
    {
        TorpedoFunctions.IgnoreColliders(this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TorpedoFunctions.GetTargetInfo(this);
        TorpedoFunctions.AngleTowardsTarget(this);
        TorpedoFunctions.TorpedoMove(this);
        TorpedoFunctions.DestroyCloseToTarget(this);
        TorpedoFunctions.DestroyAfterTime(this);
    }

    void OnCollisionEnter(Collision collision)
    {
        TorpedoFunctions.RunCollisionEvent(this, collision);
    }
}
