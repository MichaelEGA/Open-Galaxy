using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ExitMenuFunctions
{
    //This display the exit menu
    public static void DisplayExitMenu(bool isDisplaying)
    {
        GameObject exitMenu = GameObject.Find("ExitMenu");

        bool videoIsPlaying = OGVideoPlayerFunctions.VideoIsPlaying();

        if (videoIsPlaying == false)
        {
            if (isDisplaying == true)
            {

                if (exitMenu == null)
                {
                    GameObject exitMenuPrefab = Resources.Load(OGGetAddress.menus + "ExitMenu") as GameObject;
                    exitMenu = GameObject.Instantiate(exitMenuPrefab);
                    exitMenu.name = "ExitMenu";
                }

                if (exitMenu != null)
                {
                    MissionFunctions.PauseGame(false);

                    //This selects the button for when players are using the controller
                    exitMenu.GetComponentInChildren<Button>().Select();
                }
            }
            else
            {
                if (exitMenu != null)
                {
                    exitMenu.SetActive(false);
                }

                MissionFunctions.ResumeGame();
            }
        }
    }
}
