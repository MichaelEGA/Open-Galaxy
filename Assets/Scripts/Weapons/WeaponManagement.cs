using UnityEngine;

public class WeaponManagement : MonoBehaviour
{
    public SmallShip smallShip;
    public EnergyWeapon energyWeapon;
    public TorpedoTubes torpedoTubes;

    public string weaponType = "lasers";
    public string weaponMode = "single";

    public bool weaponsLock = false;
    public bool preventWeaponChange = false;

    public bool hasTorpedos;
    public bool hasIon;
    public bool hasPlasma;
    public bool hasRapidFire;

    public bool toggleWeapons;
    public bool toggleWeaponNumber;
    public float toggleWeaponPressedTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        smallShip = GetComponent<SmallShip>();
        energyWeapon = GetComponent<EnergyWeapon>();
        torpedoTubes = GetComponent<TorpedoTubes>();
    }

    // Update is called once per frame
    void Update()
    {
        WeaponManagementFunctions.ToggleWeapons(this);
    }
}
