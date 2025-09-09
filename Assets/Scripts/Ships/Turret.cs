using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public SmallShip smallShip;
    public LargeShip largeShip;
    public GameObject shipGO;
    public GameObject turretGO;
    public GameObject targetGO;
    public GameObject particleSystemGO;
    public Transform[] turretPositions;
    public ParticleSystem particleSystem;
    public Audio audioManager;
    public float smallTurretDelay;
    public float largeTurretDelay;
    public float largeTurretDamage;
    public float smallTurretDamage;
    public string audioFile = "Turbolaser";
    public string allegiance;
    public string accuracy;
    public string laserColor = "red";
    public bool turretSetUp = false;
    public Task turretTask;

    void Start()
    {
        TurretFunctions.SetUpTurrets(this);
    }

    void Update()
    {
        if (turretTask == null)
        {
            turretTask = new Task(TurretFunctions.RunTurrets(this));
        }

        if(turretTask != null)
        {
            if (turretTask.Running == false)
            {
                turretTask = new Task(TurretFunctions.RunTurrets(this));
            }
        }
    }
}


