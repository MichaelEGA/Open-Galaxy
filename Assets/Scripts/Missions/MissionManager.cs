using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public Scene scene;
    public int[] eventNo;
    public float pressedTime;
    public bool unloading;
    public bool running;
    public string missionName;
    public string missionAddress;
    public string missionData;

    // Update is called once per frame
    void Update()
    {
        MissionFunctions.ExitOnPlayerDestroy(this);
        MissionFunctions.ActivateExitMenu(this);
    }
}
