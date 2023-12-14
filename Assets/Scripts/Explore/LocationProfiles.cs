using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class LocationProfile
{
    public string location;
    public string region;
    public string mode;
    public float piracyRisk;
    public float enemyRisk;
    public float largeCapitalShips;
    public float mediumCapitalShips;
    public float smallCapitalShips;
    public float freighters;
    public float lightfreighters;
    public float shuttles;
    public float fighters;
    public float stations;
    public float cargofields;
    public float navbuoys;
    public float enemyLargeCapitalShips;
    public float enemyMediumCapitalShips;
    public float enemySmallCapitalShips;
    public float enemyFreighters;
    public float enemyLightfreighters;
    public float enemyShuttles;
    public float enemyFighters;
    public float pirateLightfreighters;
    public float pirateShuttles;
    public float pirateFighters;
}

[System.Serializable]
public class LocationProfiles
{
    public LocationProfile[] locationProfileData;
}
