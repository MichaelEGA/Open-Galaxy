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
    public int largeCapitalShips;
    public int mediumCapitalShips;
    public int smallCapitalShips;
    public int freighters;
    public int lightfreighters;
    public int shuttles;
    public int fighters;
    public int stations;
    public int cargofields;
    public int navbuoys;
    public int enemyLargeCapitalShips;
    public int enemyMediumCapitalShips;
    public int enemySmallCapitalShips;
    public int enemyFreighters;
    public int enemyLightfreighters;
    public int enemyShuttles;
    public int enemyFighters;
    public int pirateLightfreighters;
    public int pirateShuttles;
    public int pirateFighters;
}

[System.Serializable]
public class LocationProfiles
{
    public LocationProfile[] locationProfileData;
}
