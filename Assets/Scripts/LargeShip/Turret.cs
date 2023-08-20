using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public LargeShip largeShip;
    public GameObject turretBase;
    public GameObject turretArm;
    public Transform[] firepoints;
    public GameObject turretParticleSystem;
    public string allegiance;
    public string laserColor = "red";
    public float turretSpeed = 100; //Percentage
    public float turnInput;
    public float pitchInput;
    public float turnInputActual;
    public float pitchInputActual;
    public float yRotationMax = 30;
    public float yRotationMin = -30;
    public float laserDamage = 50;
    public float hullLevel = 100;
    public float targetForward;
    public float fireDelay;
    public float fireDelayCount;
    public bool turretFiring;

    void Update()
    {
        TurretFunctions.TurretInput(this, largeShip);
        TurretFunctions.RotateTurret(this);
        TurretFunctions.FireTurret(this);
    }
}


