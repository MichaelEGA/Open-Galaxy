using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public Scene scene;
    public List<int> eventNo;
    public float pressedTime;
    public bool unloading;
    public bool running;
    public string missionName;
    public string missionAddress;
    public string missionData;
    public string[] objectiveList;
    public MissionBriefing missionBriefing;
    public List<Task> missionTasks;
    public bool audioLoaded = false;
    public bool pauseEventSeries;

    // Update is called once per frame
    void Update()
    {
        MissionFunctions.ExitOnPlayerDestroy(this);
        MissionFunctions.ActivateExitMenu(this);
    }
}
