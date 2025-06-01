using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.Audio;

public static class AudioFunctions
{
    #region start functions

    //This creates the audio manager and loads all availible audio clips
    public static void CreateAudioManager(string address = "none", bool filesAreExternal = false)
    {
        GameObject audioManagerGO = new GameObject();
        audioManagerGO.name = "Audio Manager";
        Audio audioManager = audioManagerGO.AddComponent<Audio>();
        audioManager.audioMixer = Resources.Load<AudioMixer>(OGGetAddress.audiomixers + "OGAudio");

        LoadAudioClips(audioManager);

        if (address != "none")
        {
            Task a = new Task(LoadMissionAudioClips(audioManager, address, filesAreExternal));
        }
    }

    //This loads all the availible audio clips
    public static void LoadAudioClips(Audio audio)
    {
        AudioClip[] audioClips = Resources.LoadAll<AudioClip>(OGGetAddress.audioclips);
        audio.audioClips = new AudioClip[audioClips.Length];
        audio.audioClips = audioClips;
    }

    //This loads all availble mission audio files
    public static IEnumerator LoadMissionAudioClips(Audio audio, string address, bool filesAreExternal = false)
    {
        MissionManager missionManager = MissionFunctions.GetMissionManager();

        if (filesAreExternal == false) //This loads the audio files from the internal mission folder
        {
            AudioClip[] missionAudioClips = Resources.LoadAll<AudioClip>(address);
            audio.missionAudioClips = missionAudioClips;
        }
        else //This loads the audio files from the external mission folder
        {
            var info = new DirectoryInfo(address);

            if (info.Exists == true)
            {
                var fileInfo = info.GetFiles("*.wav");
                List<AudioClip> missionAudioClipsList = new List<AudioClip>();

                foreach (FileInfo file in fileInfo)
                {
                    UnityWebRequest audioFile = UnityWebRequestMultimedia.GetAudioClip(address + string.Format("{0}", file.Name), AudioType.WAV);
                    yield return audioFile.SendWebRequest();

                    if (audioFile.result == UnityWebRequest.Result.ConnectionError)
                    {
                        Debug.Log(audioFile.error);
                        Debug.Log(address + string.Format("{0}", file.Name));
                    }
                    else
                    {
                        AudioClip clip = DownloadHandlerAudioClip.GetContent(audioFile);
                        clip.name = System.IO.Path.GetFileNameWithoutExtension(address + file.Name);
                        missionAudioClipsList.Add(clip);
                    }
                }

                audio.missionAudioClips = missionAudioClipsList.ToArray();
            }
        }   
        
        if(missionManager != null)
        {
            missionManager.audioLoaded = true;
        }
    }

    #endregion

    #region play audio functions

    //This plays the requested sound
    public static AudioSource PlayAudioClip(Audio audioManager, string audioName, string mixer = "external", Vector3 location = new Vector3(), float spatialBlend = 1, float pitch = 1, float distance = 500, float volume = 0.5f, int priority = 128)
    {
        AudioSource audioSource = null;

        if (audioManager == null)
        {
            audioManager = GetAudioManager();
        }

        //This prevents the game from playing audio that's nowhere near the player
        bool dontPlay = false;

        if (audioManager != null)
        {
            if (audioManager.scene != null)
            {

                GameObject mainShip = audioManager.scene.mainShip;

                if (mainShip != null)
                {
                    float tempDistance = Vector3.Distance(location, mainShip.transform.position);

                    if (tempDistance > distance)
                    {
                        dontPlay = true;
                    }

                }
            }

            //This finds and plays the audio clip
            if (dontPlay == false)
            {
                AudioClip audioClip = GetAudioClip(audioManager, audioName);
                audioSource = GetAudioSource(audioManager);

                if (audioClip != null & audioSource != null)
                {
                    audioSource.clip = audioClip;
                    audioSource.pitch = pitch;
                    audioSource.spatialBlend = spatialBlend;
                    audioSource.reverbZoneMix = 1;
                    audioSource.dopplerLevel = 0;
                    audioSource.spread = 45;
                    audioSource.volume = volume;
                    audioSource.maxDistance = distance;
                    audioSource.priority = priority;
                    audioSource.rolloffMode = AudioRolloffMode.Linear;
                    audioSource.loop = false;
                    audioSource.gameObject.transform.position = location;
                    ConnectAudioMixerGroup(audioManager, audioSource, mixer);

                    AudioDistortionFilter audioDistortionFilter = audioSource.gameObject.GetComponent<AudioDistortionFilter>();
                    AudioHighPassFilter audioHighPassFilter = audioSource.gameObject.GetComponent<AudioHighPassFilter>();

                    if (audioDistortionFilter != null)
                    {
                        audioDistortionFilter.enabled = false;
                        audioDistortionFilter.distortionLevel = 0.5f;
                    }

                    if (audioHighPassFilter != null)
                    {
                        audioHighPassFilter.enabled = false;
                        audioHighPassFilter.cutoffFrequency = 4000;
                    }

                    audioSource.Play();

                }

            }
        }

        return audioSource;
    }

    //This plays the requested voice audio file
    public static AudioSource PlayMissionAudioClip(Audio audioManager, string audioName, string mixer = "voice", Vector3 location = new Vector3(), float spatialBlend = 1, float pitch = 1, float distance = 500, float volume = 0.5f, int priority = 128, bool distortion = false, float distortionLevel = 0.5f, bool external = false)
    {
        AudioSource audioSource = null;

        if (audioManager == null)
        {
            audioManager = GetAudioManager();
        }

        //This prevents the game from playing audio that's nowhere near the player
        bool dontPlay = false;

        if (audioManager != null)
        {
            if (audioManager.scene != null)
            {
                GameObject mainShip = audioManager.scene.mainShip;

                if (mainShip != null)
                {
                    float tempDistance = Vector3.Distance(location, mainShip.transform.position);

                    if (tempDistance > distance)
                    {
                        dontPlay = true;
                    }

                }
            }

            //This finds and plays the audio clip
            if (dontPlay == false)
            {
                AudioClip audioClip = GetMissionAudioClip(audioManager, audioName, external);
                audioSource = GetAudioSource(audioManager);

                if (audioClip != null & audioSource != null)
                {
                    audioSource.clip = audioClip;
                    audioSource.pitch = pitch;
                    audioSource.spatialBlend = spatialBlend;
                    audioSource.reverbZoneMix = 1;
                    audioSource.dopplerLevel = 0;
                    audioSource.spread = 45;
                    audioSource.volume = volume;
                    audioSource.maxDistance = distance;
                    audioSource.priority = priority;
                    audioSource.rolloffMode = AudioRolloffMode.Linear;
                    audioSource.loop = false;
                    audioSource.gameObject.transform.position = location;
                    ConnectAudioMixerGroup(audioManager, audioSource, mixer);

                    AudioDistortionFilter audioDistortionFilter = audioSource.gameObject.GetComponent<AudioDistortionFilter>();
                    AudioHighPassFilter audioHighPassFilter = audioSource.gameObject.GetComponent<AudioHighPassFilter>();

                    if (audioDistortionFilter == null)
                    {
                        audioDistortionFilter = audioSource.gameObject.AddComponent<AudioDistortionFilter>();                      
                    }

                    if (audioHighPassFilter == null)
                    {
                        audioHighPassFilter = audioSource.gameObject.AddComponent<AudioHighPassFilter>();
                    }

                    if (audioDistortionFilter != null)
                    {
                        audioDistortionFilter.enabled = distortion;
                        audioDistortionFilter.distortionLevel = distortionLevel;
                    }

                    if (audioHighPassFilter != null)
                    {
                        audioHighPassFilter.enabled = distortion;
                        audioHighPassFilter.cutoffFrequency = 2000;
                    }

                    audioSource.Play();
                }
            }
        }

        return audioSource;
    }

    //This function specifcally plays engine noise
    public static void PlayEngineNoise(SmallShip smallShip)
    {
        float pitch = 1;
        float spatialBlend = 1;
        int priority = 128;

        if (smallShip.isAI == false)
        {
            pitch = (1f / smallShip.speedRating) * smallShip.thrustSpeed;
            spatialBlend = 0;
            priority = 128;
        }

        if (smallShip.engineAudioSource == null)
        {
            AudioClip audioClip = GetAudioClip(smallShip.audioManager, smallShip.engineAudio);
            AudioSource audioSource = GetAudioSource(smallShip.audioManager);

            if (audioClip != null & audioSource != null)
            {
                smallShip.engineAudioSource = audioSource;
                smallShip.engineAudioSource.clip = audioClip;

                AudioDistortionFilter audioDistortionFilter = audioSource.gameObject.GetComponent<AudioDistortionFilter>();
                AudioHighPassFilter audioHighPassFilter = audioSource.gameObject.GetComponent<AudioHighPassFilter>();

                if (audioDistortionFilter != null)
                {
                    audioDistortionFilter.enabled = false;
                    audioDistortionFilter.distortionLevel = 0.5f;
                }

                if (audioHighPassFilter != null)
                {
                    audioHighPassFilter.enabled = false;
                    audioHighPassFilter.cutoffFrequency = 4000;
                }
            }
        }

        if (smallShip.engineAudioSource != null)
        {
            smallShip.engineAudioSource.priority = priority;
            smallShip.engineAudioSource.spatialBlend = spatialBlend;
            smallShip.engineAudioSource.pitch = pitch;

            if (smallShip.engineAudioSource.isPlaying == false & smallShip.engineAudioSource.enabled == true)
            {
                smallShip.engineAudioSource.reverbZoneMix = 1;
                smallShip.engineAudioSource.dopplerLevel = 0f;
                smallShip.engineAudioSource.spread = 45;
                smallShip.engineAudioSource.maxDistance = 500;
                smallShip.engineAudioSource.volume = 0.2f;
                smallShip.engineAudioSource.rolloffMode = AudioRolloffMode.Linear;
                smallShip.engineAudioSource.loop = true;
                smallShip.engineAudioSource.Play();
            }

            smallShip.engineAudioSource.gameObject.transform.position = smallShip.transform.position;
        }
    }

    //This function specifcally plays hud shake noise
    public static void PlayCockpitShakeNoise(SmallShip smallShip)
    {
        if (smallShip.audioManager == null)
        {
            smallShip.audioManager = GetAudioManager();
        }

        if (smallShip.cockpitAudioSource == null & smallShip.audioManager != null)
        {
            AudioClip audioClip = GetAudioClip(smallShip.audioManager, "shaking01");
            AudioSource audioSource = GetAudioSource(smallShip.audioManager);

            if (audioClip != null & audioSource != null)
            {
                smallShip.cockpitAudioSource = audioSource;
                smallShip.cockpitAudioSource.clip = audioClip;
            }
        }

        if (smallShip.cockpitAudioSource != null)
        {
            smallShip.cockpitAudioSource.volume = smallShip.speedShakeMagnitude * 500;

            if (smallShip.cockpitAudioSource.isPlaying == false)
            {
                smallShip.cockpitAudioSource.priority = 128;
                smallShip.cockpitAudioSource.spatialBlend = 0;
                smallShip.cockpitAudioSource.pitch = 1;
                smallShip.cockpitAudioSource.reverbZoneMix = 1;
                smallShip.cockpitAudioSource.dopplerLevel = 0f;
                smallShip.cockpitAudioSource.spread = 45;
                smallShip.cockpitAudioSource.maxDistance = 1000;
                smallShip.cockpitAudioSource.rolloffMode = AudioRolloffMode.Linear;
                smallShip.cockpitAudioSource.loop = true;
                smallShip.cockpitAudioSource.Play();
            }

            smallShip.cockpitAudioSource.gameObject.transform.position = smallShip.transform.position;
        }
    }

    #endregion

    #region get audio clip and get audio source functions

    //This gets the audio clip to be played
    public static AudioClip GetAudioClip(Audio audioManager, string audioName)
    {
        AudioClip audioClip = null;

        if (audioManager != null)
        {
            if (audioManager.audioClips != null)
            {
                foreach (AudioClip tempAudioClip in audioManager.audioClips)
                {
                    if (tempAudioClip != null) 
                    {
                        if (tempAudioClip.name == audioName)
                        {
                            audioClip = tempAudioClip;
                            break;
                        }
                    }
                }
            }
        }     

        return audioClip;
    }

    //This gets the voice clip to be played
    public static AudioClip GetMissionAudioClip(Audio audioManager, string audioName, bool external)
    {
        AudioClip audioClip = null;

        if (audioManager != null)
        {
            if (external == true)
            {
                if (audioManager.missionAudioClips != null)
                {
                    foreach (AudioClip tempAudioClip in audioManager.missionAudioClips)
                    {
                        if (tempAudioClip != null)
                        {
                            if (tempAudioClip.name == audioName)
                            {
                                audioClip = tempAudioClip;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                if (audioManager.audioClips != null)
                {
                    foreach (AudioClip tempAudioClip in audioManager.audioClips)
                    {
                        if (tempAudioClip != null)
                        {
                            if (tempAudioClip.name == audioName)
                            {
                                audioClip = tempAudioClip;
                                break;
                            }
                        }
                    }
                }
            }
        }

        return audioClip;
    }

    //This gets an inactive audio source or makes a new one
    public static AudioSource GetAudioSource(Audio audioManager)
    {
        AudioSource audioSource = null;

        if (audioManager.audioSources == null)
        {
            audioManager.audioSources = new List<AudioSource>();
        }

        if (audioManager != null)
        {
            if (audioManager.audioSources != null)
            {
                foreach (AudioSource tempAudioSource in audioManager.audioSources)
                {
                    if (tempAudioSource.isPlaying == false)
                    {
                        audioSource = tempAudioSource;
                        break;
                    }
                }
            }
        }

        if (audioSource == null)
        {
            GameObject audioSourceGO = new GameObject();
            audioSourceGO.name = "Audio Source " + Random.Range(1000,9999);
            audioSource = audioSourceGO.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioManager.audioSources.Add(audioSource);
        }

        return audioSource;
    }

    //This gets an inactive audio source or makes a new one
    public static AudioSource GetIndependentAudioSource(Audio audioManager)
    {
        AudioSource audioSource = null;

        if (audioManager.independentAudioSources == null)
        {
            audioManager.independentAudioSources = new List<AudioSource>();
        }


        if (audioManager.independentAudioSources != null)
        {
            GameObject audioSourceGO = new GameObject();
            audioSourceGO.name = "Audio Source " + Random.Range(1000, 9999);
            audioSource = audioSourceGO.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioManager.independentAudioSources.Add(audioSource);
        }
       
        return audioSource;
    }

    //This returns an audio source connected to the designated mixer group
    public static void ConnectAudioMixerGroup(Audio audioManager, AudioSource audioSource, string mixer)
    {
        if (audioManager.audioMixer != null)
        {
            audioSource.outputAudioMixerGroup = audioManager.audioMixer.FindMatchingGroups(mixer)[0];
        }
    }

    #endregion

    #region unloading functions

    //This calls all the unloading functions and then destroys the audio manager
    public static void UnloadAudioManager()
    {
        Audio audioManager = GetAudioManager();

        if (audioManager != null)
        {
            DestroyAllAudioSources();
            ClearAudioClipPrefabPools(audioManager);
            GameObject.Destroy(audioManager.gameObject);
        }

    }

    //This clears the clip prefab pool
    public static void ClearAudioClipPrefabPools(Audio audioManager)
    {
        audioManager.audioClips = null;
    }

    //This clears all the current audio sources
    public static void DestroyAllAudioSources()
    {
        AudioSource[] audioSources = GameObject.FindObjectsOfType<AudioSource>();

        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.gameObject.name != "Menu")
            {
                GameObject.Destroy(audioSource.gameObject);
            }
        }
    }

    #endregion

    #region audio utilities

    //This stops all audio sources from playing
    public static void StopAudioSources(Audio audioManager)
    {
        foreach (AudioSource audioSource in audioManager.audioSources)
        {
            if (audioSource.isPlaying == true)
            {
                audioSource.Stop();
            }
        }
    }

    //This Get the scene manager
    public static void GetScene(Audio audioManager)
    {
        if (audioManager.scene == null)
        {
            audioManager.scene = GameObject.FindFirstObjectByType<Scene>();
        }
    }

    //This gets the AudioManager
    public static Audio GetAudioManager()
    {
        Audio audio;

        audio = GameObject.FindFirstObjectByType<Audio>();

        return audio;
    }

    #endregion
}
