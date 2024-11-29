using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextMission : MonoBehaviour
{
    public bool missionExists;
    public bool internalMissionExists;
    public bool externalMissionExists;
    public string nextMissionName;

    //This runs the next selected mission
    public void RunNextMission()
    {
        NextMissionFunctions.CheckIfMissionExists(this);

        if (externalMissionExists == true)
        {
            MainMenuFunctions.LoadCustomMission(nextMissionName);
            Debug.Log("Loading external mision");
        }
        else if (internalMissionExists == true)
        {
            MainMenuFunctions.LoadMission(nextMissionName);
            Debug.Log("Loading internal mision");
        }
        else
        {
            MissionFunctions.ReturnToMainMenu();
            MainMenuFunctions.OutputMenuMessage("The requested mission was not found");
        }

        NextMissionFunctions.UnloadNextMissionMenu();
    }

    //This returns to the main menu
    public void ReturnToMainMenu()
    {
        NextMissionFunctions.UnloadNextMissionMenu();
        MissionFunctions.ReturnToMainMenu();
    }

    //This exits the game
    public void ExitGame()
    {
        MainMenuFunctions.QuitToDesktop();
    }

}
