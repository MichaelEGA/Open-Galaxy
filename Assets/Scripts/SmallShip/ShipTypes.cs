using UnityEngine;
using System.Collections;

[System.Serializable]
public class ShipType
{
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
    public float laserFireRating;
    public float laserRating;
    public float wepRating;
    public float torpedoRating;
    public string torpedoType;
    public string laserAudio;
    public string engineAudio;
    public string thrustType;
    public string shipClass;
}

[System.Serializable]
public class ShipTypes
{
    public ShipType[] shipTypeData;
}
