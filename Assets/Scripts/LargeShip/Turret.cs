using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public LargeShip largeShip;
    public GameObject turretBase;
    public GameObject turretArm;
    public Transform[] firepoints;
    public float turretSpeed = 100; //Percentage
    public float turnInput;
    public float pitchInput;
    public float turnInputActual;
    public float pitchInputActual;
    public float yRotationMax = 30;
    public float yRotationMin = -30;
    public float turretPower = 10;

    void Update()
    {
        TurretFunctions.TurretInput(this, largeShip);
        TurretFunctions.RotateTurret(this);
    }
}


