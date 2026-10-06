using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public ParticleSystem particleSystemScript;
    public List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
    public SmallShip smallShip;

    public float laserCycleNumber;
    public float laserPressedTime;
    public float laserModePressedTime;
    public bool laserfiring;

    public GameObject laserParticleSystem;
    public GameObject laserMuzzleFlashParticleSystem;
   
    public GameObject laserCannon1;
    public GameObject laserCannon2;
    public GameObject laserCannon3;
    public GameObject laserCannon4;

    // Update is called once per frame
    void Update()
    {
        //Laser functions
        LaserFunctions.PrepareLasers(this);
        LaserFunctions.ToggleWeaponMode(this);
        LaserFunctions.InitiateFiringPlayer(this);
    }

    private void OnParticleCollision(GameObject objectHit)
    {
        LaserFunctions.RunCollisionEvent(objectHit, collisionEvents, particleSystemScript, smallShip);
    }
}
