using UnityEngine;

public class Ship : MonoBehaviour
{
    //Universal Ship Values
    public string type;
    public string prefab;
    public string cockpitPrefab;
    public string callsign;
    public string scriptType;
    public float accelerationRating;
    public float speedRating;
    public float maneuverabilityRating;
    public float hullRating;
    public float shieldRating;
    public float systemsRating;
    public float laserFireRating;
    public float laserRating;
    public float wepRating;
    public float torpedoRating;
    public string torpedoType;
    public string laserAudio;
    public string engineAudio;
    public string thrustType;
    public string shipClass;
    public string explosionType;
    public float smallturret;
    public float largeturret;
    public float shipLength;
    public string shieldType;
    public string modelauthor;
    public string textureauthor;
    public string allegiance;
    public string era;

    //Universal Variables
    public string cargo = "no cargo";
    public bool isAI;
    public bool scanned = false;
    public bool jumpingToHyperspace;
    public bool exitingHyperspace;
    public float loadTime;

    //Key References
    public Scene scene;
    public Audio audioManager;
    public OGInput ogInput;

    //SmallShip Systems
    public FlightControlSystem_Small flightControlSystem_Small;
    public FlightControlAI_Small flightControlAI_Small;
    public EnergyManagementSystem energyManagementSystem;
    public TargetingSystem targetingSystem;
    public WeaponManagement weaponManagement;
    public EnergyWeapon energyWeapon;
    public TorpedoSystem torpedoSystem;
    public DamageSystem damageSystem;
    public WingSystems wingSystems;

    //LargeShip Systems


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Scene scene = SceneFunctions.GetScene();

        //Find small ship systems
        flightControlSystem_Small = GetComponent<FlightControlSystem_Small>();
        flightControlAI_Small = GetComponent<FlightControlAI_Small>();
        energyManagementSystem = GetComponent<EnergyManagementSystem>();
        targetingSystem = GetComponent<TargetingSystem>();
        weaponManagement = GetComponent<WeaponManagement>();
        energyWeapon = GetComponent<EnergyWeapon>();
        torpedoSystem = GetComponent<TorpedoSystem>();
        damageSystem = GetComponent<DamageSystem>();
        wingSystems = GetComponent<WingSystems>();

        //Find largeship systems
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
