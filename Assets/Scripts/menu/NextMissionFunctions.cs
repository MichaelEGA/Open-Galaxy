using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NextMissionFunctions 
{
    //This activates the mission briefing when called by a mission event
    public static IEnumerator ActivateNextMissionMenu(string nextMissionName, string model = "none", bool displayNextMissionButton = true, string debriefingText = "", string audioName = "", string internalAudioFile = "false", float distortionLevel = 0, bool distortion = false)
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

        //This mutes game sounds
        AudioFunctions.MuteSelectedAudio("externalvolume");
        AudioFunctions.MuteSelectedAudio("enginevolume");
        AudioFunctions.MuteSelectedAudio("explosionsvolume");
        AudioFunctions.MuteSelectedAudio("cockpitvolume");

        //This turns off all the other cameras not used for the mission briefing scene
        SceneFunctions.ActivateCameras(false);

        //This makes the hud invisible
        HudFunctions.SetHudTransparency(0);

        //This resets the lighting to default
        SceneFunctions.SetLighting("#E2EAF4", false, 1, 1, 0, 0, 0, 60, 0, 0);

        //This loads the environment background
        GameObject environmentGO = Resources.Load<GameObject>("objects/readyrooms/readyroom_white");
        GameObject environment = GameObject.Instantiate(environmentGO) as GameObject;

        nextMission.environment = environment;

        //This loads the chosen model
        Transform modelTransform = GameObjectUtils.FindChildTransformContaining(environment.transform, model);

        if (modelTransform != null)
        {
            modelTransform.gameObject.SetActive(true);
        }

        //This adds the mission briefing text
        GameObject missionDebriefingTextGO = GameObject.Find("MissionInfo");
        Text missionDebriefingText = missionDebriefingTextGO.GetComponent<Text>();

        if (missionDebriefingText != null)
        {
            missionDebriefingText.text = debriefingText;
        }

        if (displayNextMissionButton == false)
        {
            GameObject nextMissionButton = GameObject.Find("NextMissionButton");

            if (nextMissionButton != null)
            {
                nextMissionButton.SetActive(false);
            }
        }

        yield return new WaitForSecondsRealtime(1);

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

        if (missionBriefingAudio != null)
        {
            missionBriefingAudio.name = "NextMissionMenu"; //This gives the audio a name that prevents it being deleted when the mission is unloaded
        }

        nextMission.missionDebriefingAudio = missionBriefingAudio;
    }

    //This checks if the mission exists
    public static (bool externalMision, bool internalMission) CheckIfMissionExists(string missionName)
    {
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

        return (externalMissionExists, internalMissionExists);
    }

    //This unloads the next mission menu
    public static void UnloadNextMissionMenu(bool loadNextMission = false, string missionName = "none")
    {
        //This reactivates the time scale
        Time.timeScale = 1;

        //This unmutes game sounds
        AudioFunctions.UnmuteSelectedAudio("externalvolume");
        AudioFunctions.UnmuteSelectedAudio("enginevolume");
        AudioFunctions.UnmuteSelectedAudio("explosionsvolume");
        AudioFunctions.UnmuteSelectedAudio("cockpitvolume");

        NextMission nextMission = GameObject.FindFirstObjectByType<NextMission>();

        //This makes the hud visible again
        HudFunctions.SetHudTransparency(1);

        SceneFunctions.ActivateCameras(true);

        //This stopes the audio briefing if it is still playing
        if (nextMission.missionDebriefingAudio != null)
        {
            GameObject.Destroy(nextMission.missionDebriefingAudio.gameObject);
        }

        if (nextMission.environment != null)
        {
            GameObject.Destroy(nextMission.environment);
        }

        //This unloads the mission
        MissionFunctions.UnloadMission(loadNextMission, missionName);
    }
}
