using UnityEngine;

public class TorpedoSystem : MonoBehaviour
{
    public FlightControlSystem_Small smallShip;
    public WeaponManagement weaponManagement;
    public GameObject torpedoTube1;
    public GameObject torpedoTube2;
    public GameObject torpedoTube3;
    public GameObject torpedoTube4;
    public string torpedoType = "proton torpedo";
    public float torpedoNumber = 0;
    public float torpedoPressedTime;
    public float torpedoLockOnTime;
    public int torpedoCycleNumber;
    public float torpedoModePressedTime;
    public bool hasTorpedos;
    public bool torpedoLockingOn;
    public bool torpedoLockedOn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        smallShip = GetComponent<FlightControlSystem_Small>();
        weaponManagement = GetComponent<WeaponManagement>();
    }

    // Update is called once per frame
    void Update()
    {
        //Torpedo functions
        TorpedoSystemFunctions.EstablishLockOn(smallShip);
        TorpedoSystemFunctions.FireTorpedoPlayer(smallShip);
        TorpedoSystemFunctions.ToggleWeaponMode(smallShip);
    }
}
