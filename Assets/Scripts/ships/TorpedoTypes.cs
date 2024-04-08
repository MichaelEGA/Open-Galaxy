using UnityEngine;
using System.Collections;

[System.Serializable]
public class TorpedoType
{
    public string name;
    public string prefab;
    public string trailColor;
    public float speedRating;
    public float maneuverabilityRating;
    public float damageRating;
    public string launchAudio;
    public string explosionAudio;
}

[System.Serializable]
public class TorpedoTypes
{
    public TorpedoType[] torpedoTypeData;
}

