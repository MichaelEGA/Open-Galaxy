using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public LargeShip largeShip;
    public GameObject turretBase;
    public GameObject turretArm;
    public GameObject turretParticleSystem;
    public GameObject target;
    public Transform[] firepoints;
    public Audio audioManager;
    public string allegiance;
    public string laserColor = "red";
    public string audioFile = "Turbolaser";
    public string turretType;
    public float turretSpeed = 100; //Percentage
    public float turnInput;
    public float pitchInput;
    public float turnInputActual;
    public float pitchInputActual;
    public float yRotationMax = 30;
    public float yRotationMin = -30;
    public float xRotationMax = 90;
    public float xRotationMin = 0;
    public float laserDamage = 50;
    public float hullLevel = 100;
    public float systemsLevel = 100;
    public float targetForward;
    public float targetRight;
    public float targetUp;
    public float fireDelay;
    public float fireDelayCount;
    public bool turretFiring;
    public bool isUpsideDown;
    public bool yRotationIsRestricted = false;
    public bool requestingTarget;

    void Update()
    {
        TurretFunctions.GetTarget(this);
        TurretFunctions.TurretInput(this, largeShip);
        TurretFunctions.RotateTurret(this);
        TurretFunctions.FireTurret(this);   
        TurretFunctions.Explode(this);
    }
}


