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
    public string cockpitAssetsAddress;
    public string editorWindowMode;
}

[System.Serializable]
public class OGSettingsClass
{
    public OGSettingData[] ogSettingsData;
}
