using UnityEngine;
using System.Collections;

[System.Serializable]
public class TurretType
{
    public string type;
    public float yRotationMax;
    public float yRotationMin;
    public float xRotationMax;
    public float xRotationMin;
    public float turretSpeed;
    public float fireDelay;
    public string laserColor;
    public float laserDamage;
    public string turretType;
    public float systemsLevel;
    public float hullLevel;
    public bool yRotationIsRestricted;
}

[System.Serializable]
public class TurretTypes
{
    public TurretType[] turretTypeData;
}