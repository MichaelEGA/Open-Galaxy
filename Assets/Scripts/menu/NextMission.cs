using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextMission : MonoBehaviour
{
    public bool missionExists;
    public bool internalMissionExists;
    public bool externalMissionExists;
    public string nextMissionName;
    public GameObject environment;
    public AudioSource missionDebriefingAudio;

    //This runs the next selected mission
    public void RunNextMission()
    {
        NextMissionFunctions.UnloadNextMissionMenu(true, nextMissionName);
    }

    //This returns to the main menu
    public void ReturnToMainMenu()
    {
        NextMissionFunctions.UnloadNextMissionMenu();
    }

}
