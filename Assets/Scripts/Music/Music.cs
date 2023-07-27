using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{

    public AudioClip[] musicClips;
    public List<AudioClip> actionTracks;
    public List<AudioClip> tensionTracks;
    public List<AudioClip> themeTracks;
    public AudioSource track1;
    public AudioSource track2;
    public string musicType = "action";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MusicFunctions.PlayMusicType(this, musicType);
    }
}
