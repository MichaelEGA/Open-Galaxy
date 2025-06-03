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

    //Audio Message Queue
    public Queue<string> messageStringQueue = new Queue<string>();
    public Queue<string> messageAudioQueue = new Queue<string>();
    public Queue<string> messageInternalQueue = new Queue<string>();
    public Queue<bool> messageDistortionQueue = new Queue<bool>();
    public Queue<float> messageDistortionLevelQueue = new Queue<float>();
    public bool messagePlaying;

    // Update is called once per frame
    void Update()
    {
        MissionFunctions.ExitOnPlayerDestroy(this);
        MissionFunctions.ActivateExitMenu(this);

        //This plays the messages in the cue which prevents audio from overlapping
        if (messageStringQueue.Count > 0 & messagePlaying == false)
        {
            Task a = new Task(MissionFunctions.PlayMessageFromQueue(this));
            messagePlaying = true;
        }
    }
}
