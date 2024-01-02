using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class MissionBriefingFunctions
{
    //This deactivates the menu and sets the time scale to zero so the player can start playing the game
    public static void StartGame(MissionBriefing missionBriefing)
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        if (missionBriefing.missionBriefingAudio != null)
        {
            missionBriefing.missionBriefingAudio.Stop();
        }

        missionBriefing.gameObject.SetActive(false);
    }

    //This activates the mission briefing when called by a mission event
    public static void ActivateMissionBriefing(string briefingText, AudioSource audioSource)
    {
        MissionBriefing missionBriefing = GameObject.FindObjectOfType<MissionBriefing>();

        if (missionBriefing == null)
        {
            
            GameObject missionBriefingPrefab = Resources.Load("Menu/MissionBriefing") as GameObject;
            GameObject missionBriefingGO = GameObject.Instantiate(missionBriefingPrefab);
            missionBriefingGO.name = "MissionBriefing";
            missionBriefing = missionBriefingGO.GetComponent<MissionBriefing>();
        }

        missionBriefing.missionBriefingAudio = audioSource;

        GameObject missionBriefingTextGO = GameObject.Find("MissionInfo");
        Text missionBriefingText = missionBriefingTextGO.GetComponent<Text>();

        if (missionBriefingText != null)
        {
            missionBriefingText.text = briefingText;
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }
}
