using UnityEngine;

public class WeaponManagement : MonoBehaviour
{
    [Header("Key References")]
    public Ship ship;
    public EnergyWeapon energyWeapon;
    public TorpedoSystem torpedoTubes;

    [Header("Information")]
    public string weaponType = "lasers";
    public string weaponMode = "single";
    public bool hasTorpedos;
    public bool hasIon;
    public bool hasPlasma;
    public bool hasRapidFire;

    [Header("Controls")]
    public bool fireWeapon;
    public bool rapidFire;
    public bool fireCounterMeasures;
    public bool toggleWeapons;
    public bool toggleWeaponNumber;
    public float toggleWeaponPressedTime;
    public bool weaponsLock = false;
    public bool preventWeaponChange = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ship = GetComponent<Ship>();
        energyWeapon = GetComponent<EnergyWeapon>();
        torpedoTubes = GetComponent<TorpedoSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        WeaponManagementFunctions.ToggleWeapons(this);
    }
}
