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

        if (missionBriefing.readyroom != null)
        {
            GameObject.Destroy(missionBriefing.readyroom);
        }

        //This makes the hud visible again
        HudFunctions.SetHudTransparency(1);

        SceneFunctions.ActivateCameras(true);
    }

    //This activates the mission briefing when called by a mission event
    public static IEnumerator ActivateMissionBriefing(string briefingText, string audioName, string internalAudioFile, bool distortion, float distortionLevel)
    {
        //This makes the hud invisible
        HudFunctions.SetHudTransparency(0);

        //This turns off all the other cameras not used for the mission briefing scene
        SceneFunctions.ActivateCameras(false);

        //This loads the ready room
        GameObject readyroomGO = Resources.Load<GameObject>("objects/readyrooms/readyroom_rebel");
        GameObject readyroom = GameObject.Instantiate(readyroomGO) as GameObject;

        //This moves the camera into position
        Vector3 endPosition = new Vector3(0, 1.94000006f, -4.6500001f);
        Vector3 startPosition = new Vector3(0, 1.94000006f, -12.4399996f);
        Camera camera = readyroom.GetComponentInChildren<Camera>();
        Task a = new Task(LerpPosition(camera.transform.gameObject, startPosition, endPosition, 2));

        //This gives the camera time to zoom in
        while (a.Running == true)
        {
            yield return null;
        }

        //This activates the mission briefing screen
        MissionBriefing missionBriefing = GameObject.FindFirstObjectByType<MissionBriefing>();

        if (missionBriefing == null)
        {        
            GameObject missionBriefingPrefab = Resources.Load(OGGetAddress.menus + "MissionBriefing") as GameObject;
            GameObject missionBriefingGO = GameObject.Instantiate(missionBriefingPrefab);
            missionBriefingGO.name = "MissionBriefing";
            missionBriefing = missionBriefingGO.GetComponent<MissionBriefing>();
        }

        missionBriefing.readyroom = readyroom;

        GameObject missionBriefingTextGO = GameObject.Find("MissionInfo");
        Text missionBriefingText = missionBriefingTextGO.GetComponent<Text>();

        //This starts the audio playing
        AudioSource missionBriefingAudio = null;

        if (audioName != "none" & internalAudioFile != "true")
        {
            missionBriefingAudio = AudioFunctions.PlayMissionAudioClip(null, audioName, "Voice", new Vector3(0, 0, 0), 0, 1, 500, 1f, 1, distortion, distortionLevel, true);
        }
        else if (audioName != "none" & internalAudioFile == "true")
        {
            missionBriefingAudio = AudioFunctions.PlayMissionAudioClip(null, audioName, "Voice", new Vector3(0, 0, 0), 0, 1, 500, 1f, 1, distortion, distortionLevel, true);
        }

        missionBriefing.missionBriefingAudio = missionBriefingAudio;

        //This selects the button for when players are using the controller
        missionBriefing.gameObject.GetComponentInChildren<Button>().Select();

        if (missionBriefingText != null)
        {
            missionBriefingText.text = briefingText;
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }

    //This lerps the camera to it's new position
    public static IEnumerator LerpPosition(GameObject gameObject, Vector3 startPosition, Vector3 endPosition, float lerpDuration)
    {
         float timeElapsed = 0;

        while (timeElapsed < lerpDuration)
        {
            // Calculate the lerp factor
            float t = timeElapsed / lerpDuration;
            // Lerp the local position
            gameObject.transform.localPosition = Vector3.Lerp(startPosition, endPosition, t);
            // Increment the time elapsed
            timeElapsed += Time.unscaledDeltaTime;
            // Wait for the next frame
            yield return null;
        }

        // Ensure the final position is set
        gameObject.transform.localPosition = endPosition;
    }

}
