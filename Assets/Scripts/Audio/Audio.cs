using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Audio : MonoBehaviour
{
    public Scene scene;
    public AudioClip[] audioClips;
    public AudioClip[] missionAudioClips;
    public List<AudioSource> audioSources;
    public List<AudioSource> independentAudioSources;
    public AudioMixer audioMixer;
}
