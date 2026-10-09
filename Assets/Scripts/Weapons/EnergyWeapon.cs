using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyWeapon : MonoBehaviour
{
    public ParticleSystem particleSystemScript;
    public SmallShip smallShip;
    public List<Vector4> customData = new List<Vector4>();

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
        EnergyWeaponFunctions.PrepareLasers(this);
        EnergyWeaponFunctions.ToggleWeaponMode(this);
        EnergyWeaponFunctions.InitiateFiringPlayer(this);
    }
}
