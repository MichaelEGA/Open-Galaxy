using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class LocationProfile
{
    public string location;
    public string region;
    public int navBuoys;
    public int asteroids;
    public int spaceJunk;
    public int civilianStations;
    public int civilianCapitalLarge;
    public int civilianCapitalMedium;
    public int civilianCapitalSmall;
    public int civilianFreightersLarge;
    public int civilianFreightersSmall;
    public int civilianFighters;
    public int civilianShuttles;
    public int civilianCargoFields;
    public int imperialStations;
    public int imperialCapitalLarge;
    public int imperialCapitalMedium;
    public int imperialCapitalSmall;
    public int imperialFreightersLarge;
    public int imperialFreightersSmall;
    public int imperialFighters;
    public int imperialShuttles;
    public int imperialCargoFields;
    public int rebelStations;
    public int rebelCapitalLarge;
    public int rebelCapitalMedium;
    public int rebelCapitalSmall;
    public int rebelFreightersLarge;
    public int rebelFreightersSmall;
    public int rebelFighters;
    public int rebelShuttles;
    public int rebelCargoFields;
    public int pirateStations;
    public int pirateCapitalLarge;
    public int pirateCapitalMedium;
    public int pirateCapitalSmall;
    public int pirateFreightersLarge;
    public int pirateFreightersSmall;
    public int pirateFighters;
    public int pirateShuttles;
    public int pirateCargoFields;
}

[System.Serializable]
public class LocationProfiles
{
    public LocationProfile[] locationProfileData;
}
