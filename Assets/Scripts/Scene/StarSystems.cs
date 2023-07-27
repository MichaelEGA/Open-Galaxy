
[System.Serializable]
public class StarSystem
{
    public string Planet;
    public float X;
    public float Y;
    public float Z;
    public float sectorRadius;
    public string Region;
    public string Sector;
    public string Grid;
    public string System;
    public int SystemNumber;
    public int seed;
    public string SystemSunType;
    public string planetType;
    public string faction;
}

[System.Serializable]
public class StarSystems
{
    public StarSystem[] starSystemsData;
}