using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    //NEW VARIABLES
    public SmallShip smallShip;
    public LargeShip largeShip;
    public GameObject shipGO;
    public GameObject turretGO;
    public GameObject targetGO;
    public Transform[] turretPositions;
    public ParticleSystem particleSystem;
    public Audio audioManager;
    public string audioFile = "Turbolaser";
    public string allegiance;
    public string accuracy;
    public string laserColor = "red";


    //OLD VARIABLES
    public GameObject turretBase;
    public GameObject turretArm;
    public GameObject turretParticleSystem;

    public Transform[] firepoints;




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


