using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenCaptureFunctions
{
    public static IEnumerator CaptureScreenNoHud()
    {
        GameObject hud = GameObject.Find("Hud");
            
        if (hud != null)
        {
            hud.SetActive(false);
        }

        yield return null;

        ScreenCapture.CaptureScreenshot(Application.persistentDataPath + "/screenshot" + Time.time + ".png", 4);

        yield return null;

        if (hud != null)
        {
            hud.SetActive(true);
        }
    }

    public static void CaptureScreen()
    {
        ScreenCapture.CaptureScreenshot(Application.persistentDataPath + "/screenshots" + Time.time + ".png", 4);
    }
}
