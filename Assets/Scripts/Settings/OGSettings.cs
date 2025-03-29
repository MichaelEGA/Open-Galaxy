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
    public bool autoaim = false;
    public string shipAssets = "opengalaxy";
    public string gameWindowMode = "fullscreen";
    public string editorWindowMode = "fullscreen";
    public string damage = "default";
    public string quality = "performant";
}
