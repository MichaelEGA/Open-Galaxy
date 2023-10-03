using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OGSettings : MonoBehaviour
{
    public int heightMapResolution = 2048;
    public int screenResX = 1024;
    public int screenResY = 768;
    public bool invertY = true;
    public bool invertX = true;
    public string cockpitAssetsAddress = "CockpitPrefabs/fs_cockpits/";
    public string gameWindowMode = "FullScreenWindow";
    public string editorWindowMode = "fullscreen";
}
