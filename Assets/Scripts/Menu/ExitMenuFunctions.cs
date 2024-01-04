using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class ExitMenuFunctions
{
    //This display the exit menu
    public static void DisplayExitMenu(bool isDisplaying)
    {
        GameObject exitMenu = GameObject.Find("ExitMenu");

        if (isDisplaying == true)
        {

            if (exitMenu == null)
            {
                GameObject exitMenuPrefab = Resources.Load("Menu/ExitMenu") as GameObject;
                exitMenu = GameObject.Instantiate(exitMenuPrefab);
                exitMenu.name = "ExitMenu";
            }

            if (exitMenu != null)
            {
                Time.timeScale = 0;

                exitMenu.SetActive(true);

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
        else
        {
            Time.timeScale = 1;

            if (exitMenu != null)
            {
                exitMenu.SetActive(false);
            }

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    //This exits back to the main screen
    public static void ExitAndUnload()
    {
        HudFunctions.UnloadHud();
        MusicFunctions.UnloadMusicManager();
        AudioFunctions.UnloadAudioManager();
        Task a = new Task(SceneFunctions.UnloadScene());

        ExitMenu exitMenu = GameObject.FindObjectOfType<ExitMenu>();
        GameObject loadingScreen = GameObject.Find("LoadingScreen");
        GameObject missionBriefing = GameObject.Find("MissionBriefing");

        if (exitMenu != null) { GameObject.Destroy(exitMenu.gameObject); }
        if (loadingScreen != null) { GameObject.Destroy(loadingScreen); }
        if (missionBriefing != null) { GameObject.Destroy(missionBriefing); }

        MainMenu mainMenu = GameObject.FindObjectOfType<MainMenu>(true);

        if (mainMenu != null)
        {
            if (mainMenu.menu != null)
            {
                mainMenu.menu.SetActive(true);
                CanvasGroup canvasGroup = mainMenu.menu.GetComponent<CanvasGroup>();
                Task b = new Task(MainMenuFunctions.FadeInCanvas(canvasGroup, 0.5f));
            }
        }

        MissionManager missionManager = GameObject.FindObjectOfType<MissionManager>();
        if (missionManager != null) { GameObject.Destroy(missionManager.gameObject); }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
