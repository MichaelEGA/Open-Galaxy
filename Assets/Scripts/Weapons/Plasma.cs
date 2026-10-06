using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plasma : MonoBehaviour
{
    public ParticleSystem particleSystemScript;
    public List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
    public SmallShip smallShip;

    public float plasmaCycleNumber;
    public float plasmaPressedTime;
    public float plasmaModePressedTime;
    public bool plasmafiring;

    public GameObject plasmaParticleSystem;
    public GameObject plasmaMuzzleFlashParticleSystem;

    public GameObject plasmaCannon1;
    public GameObject plasmaCannon2;
    public GameObject plasmaCannon3;
    public GameObject plasmaCannon4;

    // Update is called once per frame
    void Update()
    {
        //Laser functions
        PlasmaFunctions.PreparePlasma(this);
        PlasmaFunctions.ToggleWeaponMode(this);
        PlasmaFunctions.InitiateFiringPlayer(this);
    }

    private void OnParticleCollision(GameObject objectHit)
    {
        PlasmaFunctions.RunCollisionEvent(objectHit, collisionEvents, particleSystemScript, smallShip);
    }
}
