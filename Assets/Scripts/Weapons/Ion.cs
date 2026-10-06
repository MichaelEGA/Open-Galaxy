using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ion : MonoBehaviour
{
    public ParticleSystem particleSystemScript;
    public List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
    public SmallShip smallShip;

    public float ionCycleNumber;
    public float ionPressedTime;
    public float ionModePressedTime;
    public bool ionfiring;

    public GameObject ionParticleSystem;
    public GameObject ionMuzzleFlashParticleSystem;

    public GameObject ionCannon1;
    public GameObject ionCannon2;
    public GameObject ionCannon3;
    public GameObject ionCannon4;

    // Update is called once per frame
    void Update()
    {
        //Laser functions
        IonFunctions.PrepareIon(this);
        IonFunctions.ToggleWeaponMode(this);
        IonFunctions.InitiateFiringPlayer(this);
    }

    private void OnParticleCollision(GameObject objectHit)
    {
        IonFunctions.RunCollisionEvent(objectHit, collisionEvents, particleSystemScript, smallShip);
    }
}
