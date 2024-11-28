using UnityEngine;

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

        nextMission.nextMissionName = nextMissionName;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }
}
