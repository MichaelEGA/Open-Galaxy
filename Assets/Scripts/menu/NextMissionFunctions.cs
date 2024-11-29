using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class NextMissionFunctions 
{
    //This activates the mission briefing when called by a mission event
    public static void ActivateNextMissionMenu(string nextMissionName)
    {
        NextMission nextMission = GameObject.FindFirstObjectByType<NextMission>();

        if (nextMission == null)
        {
            GameObject nextMissionPrefab = Resources.Load(OGGetAddress.menus + "NextMission") as GameObject;
            GameObject nextMissionGO = GameObject.Instantiate(nextMissionPrefab);
            nextMissionGO.name = "NextMission";
            nextMission = nextMissionGO.GetComponent<NextMission>();
        }

        CanvasGroup canvasGroup = nextMission.GetComponent<CanvasGroup>();
        Task b = new Task(MainMenuFunctions.FadeInCanvas(canvasGroup, 0.5f));

        nextMission.nextMissionName = nextMissionName;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }

    //This checks if the mission exists
    public static void CheckIfMissionExists(NextMission nextMission)
    {
        string missionName = nextMission.nextMissionName;
        bool internalMissionExists = false;
        bool externalMissionExists = false;

        //This gets internal mission data
        Object[] mainMissions = Resources.LoadAll(OGGetAddress.missions_internal, typeof(TextAsset));

        //This searches to see whether the mission exists internally
        foreach (Object mission in mainMissions)
        {
           if (mission.name == missionName)
           {
                internalMissionExists = true;
           }
        }

        //This gets external mission data
        var info = new DirectoryInfo(OGGetAddress.missions_custom);

        if (info.Exists == false)
        {
            Directory.CreateDirectory(OGGetAddress.missions_custom);
            info = new DirectoryInfo(OGGetAddress.missions_custom);
        }

        List<TextAsset> customMissionsList = new List<TextAsset>();

        if (info.Exists == true)
        {
            var fileInfo = info.GetFiles("*.json");

            foreach (FileInfo file in fileInfo)
            {
                string path = OGGetAddress.missions_custom + file.Name;
                string missionDataString = File.ReadAllText(path);
                TextAsset missionDataTextAsset = new TextAsset(missionDataString);
                missionDataTextAsset.name = System.IO.Path.GetFileNameWithoutExtension(path);
                customMissionsList.Add(missionDataTextAsset);
            }
        }

        Object[] customMissions = customMissionsList.ToArray();

        //This searches to see whether the mission exists externally
        foreach (Object mission in customMissions)
        {
            if (mission.name == missionName)
            {
                externalMissionExists = true;
            }
        }

        //This return the information
        nextMission.externalMissionExists = externalMissionExists;
        nextMission.internalMissionExists = internalMissionExists;
    }

    //This unloads the next mission menu
    public static void UnloadNextMissionMenu()
    {
        NextMission nextMenu = GameObject.FindFirstObjectByType<NextMission>();

        if (nextMenu != null)
        {
            GameObject.Destroy(nextMenu.gameObject);
        }
    }
}
