using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public static class MusicFunctions
{
    #region start functions

    //This creates the music manager
    public static void CreateMusicManager()
    {
        //This creates the music manager gameobject and attaches the music manager script
        GameObject musicManager = new GameObject();
        Music musicManagerScript = musicManager.AddComponent<Music>();
        musicManager.name = "Music Manager";
        musicManagerScript.audioMixer = Resources.Load<AudioMixer>("AudioMixers/OGAudio");

        //This creates the audio sources for the different music tracks
        CreateAudioSources(musicManagerScript);

        //This loads the music clips
        LoadMusicClips(musicManagerScript);
    }

    //This loads the music clips
    public static void LoadMusicClips(Music musicManager)
    {
        AudioClip[] musicClips = Resources.LoadAll<AudioClip>("MusicClips");
        musicManager.musicClips = new AudioClip[musicClips.Length];
        musicManager.musicClips = musicClips;

        musicManager.actionTracks = new List<AudioClip>();
        musicManager.tensionTracks = new List<AudioClip>();
        musicManager.themeTracks = new List<AudioClip>();

        foreach (AudioClip musicClip in musicManager.musicClips)
        {
            if (musicClip.name.Contains("action"))
            {
                musicManager.actionTracks.Add(musicClip);
            }
            else if (musicClip.name.Contains("tension"))
            {
                musicManager.tensionTracks.Add(musicClip);
            }
            else if (musicClip.name.Contains("theme"))
            {
                musicManager.themeTracks.Add(musicClip);
            }
        }
    }

    //This creates the audio sources for the different music tracks
    public static void CreateAudioSources(Music musicManager)
    {
        musicManager.track1 = musicManager.gameObject.AddComponent<AudioSource>();
        musicManager.track1.spatialBlend = 0;
        musicManager.track1.maxDistance = 2000;
        musicManager.track1.volume = 0.6f;
        musicManager.track1.loop = false;
        musicManager.track1.outputAudioMixerGroup = musicManager.audioMixer.FindMatchingGroups("Track01")[0];

        musicManager.track2 = musicManager.gameObject.AddComponent<AudioSource>();
        musicManager.track2.spatialBlend = 0;
        musicManager.track2.maxDistance = 2000;
        musicManager.track2.volume = 0.6f;
        musicManager.track2.loop = false;
        musicManager.track2.outputAudioMixerGroup = musicManager.audioMixer.FindMatchingGroups("Track02")[0];
    }

    #endregion'

    #region unload music manager

    public static void UnloadMusicManager()
    {
        Music music = GameObject.FindObjectOfType<Music>();

        if (music != null)
        {
            GameObject.Destroy(music.gameObject);
        }
    }

    #endregion

    #region music functions

    //This randomly picks action tracks to play
    public static void PlayMusicType(Music musicManager, string type, bool forceChange = false)
    {
        if (musicManager.musicClips != null & musicManager.track1 != null & musicManager.track2 != null)
        {
            musicManager.musicType = type;

            if (musicManager.track1.isPlaying == false & musicManager.track2.isPlaying == false || forceChange == true)
            {
                if (musicManager.musicType == "action")
                {
                    if (musicManager.actionTracks.Count > 0)
                    {
                        musicManager.track1.clip = musicManager.actionTracks[Random.Range(0, musicManager.actionTracks.Count - 1)];
                        musicManager.track1.Play();
                    }
                }  
                else if (musicManager.musicType == "tension")
                {
                    if (musicManager.tensionTracks.Count > 0)
                    {
                        musicManager.track1.clip = musicManager.tensionTracks[Random.Range(0, musicManager.tensionTracks.Count - 1)];
                        musicManager.track1.Play();
                    }
                }
                else if (musicManager.musicType == "theme")
                {
                    if (musicManager.themeTracks.Count > 0)
                    {
                        musicManager.track1.clip = musicManager.themeTracks[Random.Range(0, musicManager.themeTracks.Count - 1)];
                        musicManager.track1.Play();
                    }
                }
            }
        }
    }

    //When called this funciton allows the misison script to change the level of the music
    public static void ChangeMusicVolume(Music musicManager, float volume)
    {
        if (volume > 1)
        {
            volume = 1;
        }
        else if (volume < 0)
        {
            volume = 0;
        }

        musicManager.track1.volume = volume;
        musicManager.track2.volume = volume;
    }

    #endregion
}
