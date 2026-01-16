using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class OGVideoPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public VideoClip[] videoClips;
    public List<string> externalVideoClipAddresses;
    public CanvasGroup videoPlayerCG;
}
