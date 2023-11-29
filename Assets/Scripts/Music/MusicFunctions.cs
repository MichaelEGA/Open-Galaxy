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
    public static IEnumerator PlayMusicTrack(Music musicManager, string trackName)
    {

        if (trackName != "none")
        {
            AudioClip track = null;

            foreach (AudioClip tempTrack in musicManager.musicClips)
            {
                if (tempTrack.name == trackName)
                {
                    track = tempTrack;
                }
            }

            if (musicManager.track1.isPlaying == false)
            {
                if (musicManager.track2.isPlaying == true)
                {
                    while (musicManager.track2.volume > 0)
                    {
                        musicManager.track2.volume -= 0.1f;
                        yield return new WaitForSeconds(0.1f);
                    }
                }

                musicManager.track2.Stop();

                musicManager.track1.clip = track;
                musicManager.track1.Play();
                musicManager.track1.loop = true;

                while (musicManager.track1.volume < 1)
                {
                    musicManager.track1.volume += 0.1f;
                    yield return new WaitForSeconds(0.1f);
                }
            }
            else
            {
                if (musicManager.track1.isPlaying == true)
                {
                    while (musicManager.track1.volume > 0)
                    {
                        musicManager.track1.volume -= 0.1f;
                        yield return new WaitForSeconds(0.1f);
                    }
                }

                musicManager.track1.Stop();

                musicManager.track2.clip = track;
                musicManager.track2.Play();
                musicManager.track2.loop = true;

                while (musicManager.track2.volume < 1)
                {
                    musicManager.track2.volume += 0.1f;
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
        else
        {
            if (musicManager.track1.isPlaying == true)
            {
                while (musicManager.track1.volume > 0)
                {
                    musicManager.track1.volume -= 0.1f;
                    yield return new WaitForSeconds(0.1f);
                }

                musicManager.track1.Stop();
            }

            if (musicManager.track2.isPlaying == true)
            {
                while (musicManager.track2.volume > 0)
                {
                    musicManager.track2.volume -= 0.1f;
                    yield return new WaitForSeconds(0.1f);
                }

                musicManager.track2.Stop();
            }
        }

        yield return null;
    }

    #endregion
}
