using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionBriefing : MonoBehaviour
{
    public AudioSource missionBriefingAudio;
    public GameObject environment;

    //This is used to temporary store the lightning data and then reapply it once the mission briefing is closed
    public string colour = "#FFFFFF";
    public bool sunIsEnabled = true;
    public float sunIntensity = 1;
    public float sunScale = 1;
    public float x = 0;
    public float y = 0;
    public float z = 0;
    public float xRot = 60;
    public float yRot = 0;
    public float zRot = 0;

    public void StartGame()
    {
        MissionBriefingFunctions.StartGame(this);
    }
}
