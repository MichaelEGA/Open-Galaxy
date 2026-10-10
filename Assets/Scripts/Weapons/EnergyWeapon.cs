using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyWeapon : MonoBehaviour
{
    public Ship ship;
    public WeaponManagement weaponManagement;

    public ParticleSystem particleSystemScript;
    public List<Vector4> customData = new List<Vector4>();

    public float energyWeaponCycleNumber;
    public float energyWeaponPressedTime;
    public float energyWeaponModePressedTime;
    public float weaponRechargeDelay;
    public bool energyWeaponRecharged;
    public bool energyWeaponFiring;

    public GameObject energyWeaponParticleSystem;
    public GameObject energyWeaponMuzzleFlashParticleSystem;

    public string laserColour = "red";

    public GameObject laserCannon1;
    public GameObject laserCannon2;
    public GameObject laserCannon3;
    public GameObject laserCannon4;

    public string laserAudio;
    public string ionAudio = "weapon_ioncannon";
    public string plasmaAudio = "weapon_plasma";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ship = GetComponent<Ship>();
        weaponManagement = GetComponent<WeaponManagement>();
    }

    // Update is called once per frame
    void Update()
    {
        //Laser functions
        EnergyWeaponFunctions.PrepareEnergyWeapon(this);
        EnergyWeaponFunctions.ToggleWeaponMode(this);
        EnergyWeaponFunctions.InitiateFiringPlayer(this);
    }

    void FixedUpdate()
    {
        //Laser functions
        EnergyWeaponFunctions.EnergyWeaponCharging(this);
    }
}
