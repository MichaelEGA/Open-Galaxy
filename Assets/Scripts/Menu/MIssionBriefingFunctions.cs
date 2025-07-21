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

        //This unmutes game sounds
        AudioFunctions.UnmuteSelectedAudio("externalvolume");
        AudioFunctions.UnmuteSelectedAudio("enginevolume");
        AudioFunctions.UnmuteSelectedAudio("explosionsvolume");
        AudioFunctions.UnmuteSelectedAudio("cockpitvolume");

        //This restores the scenes lighting
        SceneFunctions.SetLighting("#" + missionBriefing.colour, missionBriefing.sunIsEnabled, missionBriefing.sunIntensity, missionBriefing.sunScale, missionBriefing.x, missionBriefing.y, missionBriefing.z, missionBriefing.xRot, missionBriefing.yRot, missionBriefing.zRot);

        //This stopes the audio briefing if it is still playing
        if (missionBriefing.missionBriefingAudio != null)
        {
            missionBriefing.missionBriefingAudio.Stop();
        }

        //This destroys the environment
        if (missionBriefing.environment != null)
        {
            GameObject.Destroy(missionBriefing.environment);
        }

        missionBriefing.gameObject.SetActive(false);

        //This makes the hud visible again
        HudFunctions.SetHudTransparency(1);

        SceneFunctions.ActivateCameras(true);

        Task a = new Task(UnlockPlayerControlsAfter(2));
    }

    //This activates the mission briefing when called by a mission event
    public static IEnumerator ActivateMissionBriefing(string briefingText, string audioName, string internalAudioFile, bool distortion, float distortionLevel, string model)
    {
        //This gets the scene reference
        Scene scene = SceneFunctions.GetScene();

        //This pauses the game
        Time.timeScale = 0;

        //This gets the current settings for the scene lighting and changes
        var lightingData = SceneFunctions.GetLightingData();

        //This resets the lighting to default
        SceneFunctions.SetLighting("#E2EAF4", false, 1, 1, 0, 0, 0, 60, 0, 0);

        //This makes the hud invisible
        HudFunctions.SetHudTransparency(0);

        //This turns off all the other cameras not used for the mission briefing scene
        SceneFunctions.ActivateCameras(false);

        //This loads the ready room
        GameObject environmentGO = Resources.Load<GameObject>("objects/readyrooms/readyroom_white");
        GameObject environment = GameObject.Instantiate(environmentGO) as GameObject;

        //This mutes game sounds
        AudioFunctions.MuteSelectedAudio("externalvolume");
        AudioFunctions.MuteSelectedAudio("enginevolume");
        AudioFunctions.MuteSelectedAudio("explosionsvolume");
        AudioFunctions.MuteSelectedAudio("cockpitvolume");

        //This locks the player controls
        LockPlayerControls();

        //This moves the camera into position
        if (model != "none" & model != "") //This checks to see whether the camera should zoom in or not
        {
            Camera camera = environment.GetComponentInChildren<Camera>();
            Vector3 endPosition = camera.transform.localPosition;
            Vector3 startPosition = camera.transform.localPosition - new Vector3(0, 0, -1);
            Task a = new Task(LerpPosition(camera.transform.gameObject, startPosition, endPosition, 2));

            //This activates one of the models in the ready room to display
            Transform modelTransform = GameObjectUtils.FindChildTransformContaining(environment.transform, model);

            if (modelTransform != null)
            {
                modelTransform.gameObject.SetActive(true);
            }

            //This gives the camera time to zoom in
            while (a.Running == true)
            {
                yield return null;
            }
        }

        //This activates the mission briefing screen
        MissionBriefing missionBriefing = GameObject.FindFirstObjectByType<MissionBriefing>();

        if (missionBriefing == null)
        {        
            GameObject missionBriefingPrefab = Resources.Load(OGGetAddress.menus + "MissionBriefing") as GameObject;
            GameObject missionBriefingGO = GameObject.Instantiate(missionBriefingPrefab);
            missionBriefingGO.name = "MissionBriefing";
            missionBriefing = missionBriefingGO.GetComponent<MissionBriefing>();
            scene.missionBriefing = missionBriefingGO;
        }

        //This adds the enviroment to mission briefing so it can access it later to destroy it
        missionBriefing.environment = environment;

        //This stores the main scenes lighting data
        missionBriefing.colour = lightingData.colour;
        missionBriefing.sunIsEnabled = lightingData.sunIsEnabled;
        missionBriefing.sunIntensity = lightingData.sunIntensity;
        missionBriefing.sunScale = lightingData.sunScale;
        missionBriefing.x = lightingData.x;
        missionBriefing.y = lightingData.y;
        missionBriefing.z = lightingData.z;
        missionBriefing.xRot = lightingData.xRot;
        missionBriefing.yRot = lightingData.yRot;
        missionBriefing.zRot = lightingData.zRot;

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

        //This adds the mission briefing text
        GameObject missionBriefingTextGO = GameObject.Find("MissionInfo");
        Text missionBriefingText = missionBriefingTextGO.GetComponent<Text>();

        if (missionBriefingText != null)
        {
            missionBriefingText.text = briefingText;
        }

        //This selects the button for when players are using the controller
        missionBriefing.gameObject.GetComponentInChildren<Button>().Select();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        yield return null;
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
    public static void LockPlayerControls()
    {
        Scene scene = SceneFunctions.GetScene();

        if (scene != null)
        {
            SmallShip smallShip = scene.mainShip.GetComponent<SmallShip>();

            if (smallShip != null)
            {
                smallShip.controlLock = true;
            }
        }
    }
    public static IEnumerator UnlockPlayerControlsAfter(float time)
    {
        yield return new WaitForSeconds(time);

        Scene scene = SceneFunctions.GetScene();

        if (scene != null)
        {
            SmallShip smallShip = scene.mainShip.GetComponent<SmallShip>();

            if (smallShip != null)
            {
                smallShip.controlLock = false;
            }
        }  
    }

}
