using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class LoadScreenFunctions
{
    //This displays the loading screen
    public static void LoadingScreen(bool display, string loadingScreenTitle = "", string message = "")
    {
        MainMenu mainMenu = MainMenuFunctions.GetMainMenu();
        GameObject loadingScreen = mainMenu.loadingScreen_cg.gameObject;

        if (display == true)
        {
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
        }
        else
        {
            Task a = new Task(MainMenuFunctions.FadeOutLoadingScreenAndDestroyMainMenu(mainMenu, 0.25f));

            GameObject hud = GameObject.Find("Hud");

            if (hud != null)
            {
                CanvasGroup hudCanvasGroup = hud.GetComponent<CanvasGroup>();

                if (hudCanvasGroup != null)
                {
                    HudFunctions.SetHudTransparency(1);
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
                    loadingInfoText.text = loadingInfoText.text + "\n" + GetTimeAsString(time) + " " + message;
                }
                else
                {
                    loadingInfoText.text = loadingInfoText.text + "\n" + "          " + message;
                }  
            }
        }
    }

    //This caculates the time in 60 second increments
    public static string GetTimeAsString(float startTime)
    {
        float timer = Time.unscaledTime - startTime;
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        string timeAsString = string.Format("{0:0}:{1:00}", minutes, seconds);

        return timeAsString;
    }
}
