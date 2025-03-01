using UnityEngine;
using System.Collections;

[System.Serializable]
public class OGSettingData
{
    public int heightMapResolution;
    public int screenResX;
    public int screenResY;
    public bool invertY;
    public bool invertX;
    public string cockpitAssets;
    public string shipAssets;
    public string gameWindowMode;
    public string editorWindowMode;
    public string difficultly;
    public string quality;
}

[System.Serializable]
public class OGSettingsClass
{
    public OGSettingData[] ogSettingsData;
}
