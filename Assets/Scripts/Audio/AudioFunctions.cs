using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioFunctions
{
    #region start functions

    //This creates the audio manager and loads all availible audio clips
    public static void CreateAudioManager()
    {
        GameObject audioManager = new GameObject();
        audioManager.name = "Audio Manager";
        Audio audioManagerScript = audioManager.AddComponent<Audio>();

        LoadAudioClips(audioManagerScript);
    }

    //This loads all the availible audio clips
    public static void LoadAudioClips(Audio audio)
    {
        AudioClip[] audioClips = Resources.LoadAll<AudioClip>("AudioClips");
        audio.audioClips = new AudioClip[audioClips.Length];
        audio.audioClips = audioClips;
    }

    #endregion

    #region play audio functions

    //This plays the requested sound
    public static void PlayAudioClip(Audio audioManager, string audioName, Vector3 location = new Vector3(), float spatialBlend = 1, float pitch = 1, float distance = 500, float volume = 0.5f, int priority = 128)
    {
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
                AudioSource audioSource = GetAudioSource(audioManager);

                if (audioClip != null & audioSource != null)
                {
                    audioSource.clip = audioClip;
                    audioSource.pitch = pitch;
                    audioSource.spatialBlend = spatialBlend;
                    audioSource.reverbZoneMix = 1;
                    audioSource.dopplerLevel = 0;
                    audioSource.spread = 45;
                    audioSource.maxDistance = distance;
                    audioSource.priority = priority;
                    audioSource.rolloffMode = AudioRolloffMode.Linear;
                    audioSource.loop = false;
                    audioSource.Play();
                    audioSource.gameObject.transform.position = location;
                }
            }
        }
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
                smallShip.engineAudioSource.volume = 0.4f;
                smallShip.engineAudioSource.rolloffMode = AudioRolloffMode.Linear;
                smallShip.engineAudioSource.loop = true;
                smallShip.engineAudioSource.Play();
            }

            smallShip.engineAudioSource.gameObject.transform.position = smallShip.transform.position;
        }
    }

    //This function specifcally plays hud shake noise
    public static void PlayHudShakeNoise(Hud hud)
    {

        if (hud.audioManager == null)
        {
            hud.audioManager = GetAudioManager();
        }

        if (hud.shakeAudioSource == null & hud.audioManager != null)
        {
            AudioClip audioClip = GetAudioClip(hud.audioManager, "shaking01_hud");
            AudioSource audioSource = GetAudioSource(hud.audioManager);

            if (audioClip != null & audioSource != null)
            {
                hud.shakeAudioSource = audioSource;
                hud.shakeAudioSource.clip = audioClip;
            }
        }

        if (hud.shakeAudioSource != null)
        {
            hud.shakeAudioSource.volume = (1f / 5f) * hud.speedShakeMagnitude;

            if (hud.shakeAudioSource.isPlaying == false)
            {
                hud.shakeAudioSource.priority = 128;
                hud.shakeAudioSource.spatialBlend = 0;
                hud.shakeAudioSource.pitch = 1;
                hud.shakeAudioSource.reverbZoneMix = 1;
                hud.shakeAudioSource.dopplerLevel = 0f;
                hud.shakeAudioSource.spread = 45;
                hud.shakeAudioSource.maxDistance = 1000;
                hud.shakeAudioSource.rolloffMode = AudioRolloffMode.Linear;
                hud.shakeAudioSource.loop = true;
                hud.shakeAudioSource.Play();
            }

            hud.shakeAudioSource.gameObject.transform.position = hud.smallShip.transform.position;
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
                    if (tempAudioClip.name == audioName)
                    {
                        audioClip = tempAudioClip;
                        break;
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
        foreach (AudioSource tempAudioSource in audioManager.audioSources)
        {
            if (tempAudioSource.isPlaying == true)
            {
                tempAudioSource.Stop();
            }
        }
    }

    //This Get the scene manager
    public static void GetScene(Audio audioManager)
    {
        if (audioManager.scene == null)
        {
            audioManager.scene = GameObject.FindObjectOfType<Scene>();
        }
    }

    //This gets the AudioManager
    public static Audio GetAudioManager()
    {
        Audio audio;

        audio = GameObject.FindObjectOfType<Audio>();

        return audio;
    }

    #endregion
}
