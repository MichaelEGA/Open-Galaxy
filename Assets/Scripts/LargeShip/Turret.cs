using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public LargeShip largeShip;
    public GameObject turretBase;
    public GameObject turretArm;
    public Transform[] firepoints;
    public float turnInput;
    public float pitchInput;
    public float turnInputActual;
    public float pitchInputActual;

    void Update()
    {
        TurretFunctions.TurretInput(this, largeShip);
        TurretFunctions.RotateTurret(this);
    }

}


