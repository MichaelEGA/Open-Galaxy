using UnityEngine;
using System.Collections;

[System.Serializable]
public class OGSettingData
{
    public int heightMapResolution;
    public int screenResX;
    public int screenResY;
    public float controllersensitivity;
    public bool invertY;
    public bool invertX;
    public bool autoaim;
    public string gameWindowMode;
    public string editorWindowMode;
    public string damage;
    public string quality;
    public string cameraPosition;
}

[System.Serializable]
public class OGSettingsClass
{
    public OGSettingData[] ogSettingsData;
}
