using UnityEngine;

public class EnergyManagementSystem : MonoBehaviour
{
    //Key References
    public Ship ship;

    //Ship Levels
    public float systemsLevel = 100;
    public float hullLevel = 100;
    public float shieldLevel = 200;
    public float frontShieldLevel = 100;
    public float rearShieldLevel = 100;
    public float wepLevel;
    public float shieldRecharge; 
    public float shieldDischarge; 
    public float wepRecharge; 
    public float wepDischarge;

    //Power Distribution
    public string powerMode = "reset";
    public float energyWeaponPower = 100;
    public float enginePower = 100;
    public float shieldPower = 100;
    public float energyWeaponCharge;
    public float powerPressedTime;

    //Controls
    public bool powerToShields;
    public bool powerToLasers;
    public bool powerToEngine;
    public bool resetPowerLevels;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ship = GetComponent<Ship>();
    }

    // Update is called once per frame
    void Update()
    {
        EnergyManagementSystemFunctions.CalculateLevels(this);
        EnergyManagementSystemFunctions.CalculatePower(this);    
    }
}
