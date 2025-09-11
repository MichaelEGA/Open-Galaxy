using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurret : MonoBehaviour
{
    public SmallShip smallShip;
    public LargeShip largeShip;
    public Mesh smallLaserMesh;
    public Mesh largeLaserMesh;
    public Transform[] turretPositions;
    public GameObject shipGO;
    public GameObject turretGO;
    public GameObject targetGO;
    public GameObject largeParticleSystemGO;
    public ParticleSystem largeParticleSystem;
    public ParticleSystemRenderer largeParticleSystemRenderer;
    public GameObject smallParticleSystemGO;
    public ParticleSystem smallParticleSystem;
    public ParticleSystemRenderer smallParticleSystemRenderer;
    public Audio audioManager;
    public float smallTurretDelay;
    public float largeTurretDelay;
    public float largeTurretDamage = 50;
    public float smallTurretDamage = 10;
    public string audioFile = "Turbolaser";
    public string allegiance;
    public string accuracy;
    public string laserColor = "red";
    public string mode;
    public bool turretSetUp = false;
    public bool requestingTarget;
    public Task turretTask;

    void Start()
    {
        LaserTurretFunctions.SetUpTurrets(this);
    }

    void Update()
    {
        if (turretTask == null)
        {
            turretTask = new Task(LaserTurretFunctions.RunTurrets(this));
        }

        if(turretTask != null)
        {
            if (turretTask.Running == false)
            {
                turretTask = new Task(LaserTurretFunctions.RunTurrets(this));
            }
        }
    }
}


