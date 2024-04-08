using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class LoadScreenFunctions
{
    //This displays the loading screen
    public static void LoadingScreen(bool display, string loadingScreenTitle = "", string message = "")
    {
        GameObject loadingScreen = GameObject.Find("LoadingScreen");

        if (display == true)
        {
            if (loadingScreen == null)
            {
                GameObject loadingScreenPrefab = Resources.Load(OGGetAddress.menus + "LoadingScreen") as GameObject;
                loadingScreen = GameObject.Instantiate(loadingScreenPrefab);
                loadingScreen.name = "LoadingScreen";
            }

            GameObject gameType = GameObject.Find("Gametype");

            if (gameType != null)
            {
                gameType.GetComponent<Text>().text = loadingScreenTitle;
            }

            GameObject tip = GameObject.Find("Tip");

            if (tip != null)
            {
                tip.GetComponent<Text>().text = message;
            }

            if (loadingScreen != null)
            {
                loadingScreen.SetActive(true);
            }
        }
        else
        {
            if (loadingScreen != null)
            {
                CanvasGroup canvasGroup = loadingScreen.GetComponent<CanvasGroup>();

                if (canvasGroup != null)
                {
                    Task a = new Task(MainMenuFunctions.FadeOutAndDeactivate(canvasGroup, 0.25f));
                }
            }

            GameObject hud = GameObject.Find("Hud");

            if (hud != null)
            {
                CanvasGroup hudCanvasGroup = hud.GetComponent<CanvasGroup>();

                if (hudCanvasGroup != null)
                {
                    Task a = new Task(MainMenuFunctions.FadeInCanvas(hudCanvasGroup, 2f));
                }

            }
        }
    }

    //This adds a message to the ship log
    public static void AddLogToLoadingScreen(string message, float time, bool showTime = true)
    {
        GameObject loadingInfo = GameObject.Find("LoadingInfo");

        if (loadingInfo != null)
        {
            Text loadingInfoText = loadingInfo.GetComponent<Text>();

            if (loadingInfoText != null)
            {
                if (showTime == true)
                {
                    loadingInfoText.text = loadingInfoText.text + "\n" + time.ToString("00:00") + " " + message;
                }
                else
                {
                    loadingInfoText.text = loadingInfoText.text + "\n" + "          " + message;
                }  
            }
        }
    }
}
