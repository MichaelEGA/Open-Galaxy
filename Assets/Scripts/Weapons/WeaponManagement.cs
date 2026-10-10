using UnityEngine;

public class WeaponManagement : MonoBehaviour
{
    public FlightControlSystem_Small smallShip;
    public EnergyWeapon energyWeapon;
    public TorpedoSystem torpedoTubes;

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
        smallShip = GetComponent<FlightControlSystem_Small>();
        energyWeapon = GetComponent<EnergyWeapon>();
        torpedoTubes = GetComponent<TorpedoSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        WeaponManagementFunctions.ToggleWeapons(this);
    }
}
