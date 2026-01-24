using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

public static class OGVideoPlayerFunctions
{

    #region loading functions

    //This creates the og video player
    public static void CreateOGVideoPlayer()
    {
        GameObject videoPlayerPrefab = Resources.Load(OGGetAddress.videoplayer + "VideoPlayer") as GameObject;
        GameObject videoPlayerGO = GameObject.Instantiate(videoPlayerPrefab);
        videoPlayerGO.name = "VideoPlayer";
        OGVideoPlayer ogVideoPlayer = videoPlayerGO.AddComponent<OGVideoPlayer>();
        ogVideoPlayer.videoPlayerCG = videoPlayerGO.GetComponent<CanvasGroup>();
        ogVideoPlayer.videoPlayer = videoPlayerGO.GetComponent<VideoPlayer>();
    }

    //This loads all the videos/video addresses in the mission before the mission starts
    public static void LoadVideos(string address, bool filesAreExternal = false)
    {
        OGVideoPlayer ogVideoPlayer = GetOGVideoPlayer();

        if (ogVideoPlayer != null)
        {
            if (filesAreExternal == false) //This loads the video files from the internal mission folder
            {
                VideoClip[] videoClips = Resources.LoadAll<VideoClip>(address);
                ogVideoPlayer.videoClips = videoClips;
            }
            else //This loads the video files addresses from the external mission folder
            {
                var info = new DirectoryInfo(address);

                if (info.Exists == true)
                {
                    // Get all .mp4 files in the persistent data folder
                    string[] files = Directory.GetFiles(address, "*.mp4");

                    if (ogVideoPlayer.externalVideoClipAddresses == null)
                    {
                        ogVideoPlayer.externalVideoClipAddresses = new List<string>();
                    }

                    ogVideoPlayer.externalVideoClipAddresses.Clear();
                    ogVideoPlayer.externalVideoClipAddresses.AddRange(files);
                }
            }
        }
    }

    #endregion

    #region run video

    //This plays a video
    public static IEnumerator RunVideo(string name)
    {
        OGVideoPlayer ogVideoPlayer = GetOGVideoPlayer();

        if (ogVideoPlayer != null)
        {
            if (ogVideoPlayer.videoPlayer != null)
            {
                if (ogVideoPlayer.videoClips != null)
                {
                    foreach (VideoClip videoClip in ogVideoPlayer.videoClips)
                    {
                        if (videoClip != null)
                        {
                            if (videoClip.name.Contains(name))
                            {
                                //This event series is paused
                                MissionManager missionManager = MissionFunctions.GetMissionManager();
                                missionManager.pauseEventSeries = true;

                                //The game is paused
                                Time.timeScale = 0;

                                //The video player is made visible
                                SetVideoPlayerToVisible();
                                
                                //The video is prepared and played
                                ogVideoPlayer.videoPlayer.source = VideoSource.VideoClip;
                                ogVideoPlayer.videoPlayer.clip = videoClip;
                                ogVideoPlayer.videoPlayer.Prepare();
                                ogVideoPlayer.videoPlayer.errorReceived += OnError;

                                while (ogVideoPlayer.videoPlayer.isPrepared == false)
                                {
                                    yield return null;
                                }

                                ogVideoPlayer.videoPlayer.Play();

                                //This mutes game sounds
                                AudioFunctions.MuteSelectedAudio("externalvolume");
                                AudioFunctions.MuteSelectedAudio("enginevolume");
                                AudioFunctions.MuteSelectedAudio("explosionsvolume");
                                AudioFunctions.MuteSelectedAudio("cockpitvolume");

                                //When the video is finished, stop and close the videoplayer
                                Task a = new Task(CloseVideoPlayerWhenFinished());
                                
                                break;
                            }
                        }
                    }
                }
                else if (ogVideoPlayer.externalVideoClipAddresses.Count > 0)
                {
                    foreach (string videoAddress in ogVideoPlayer.externalVideoClipAddresses)
                    {
                        if (videoAddress != null)
                        {
                            if (videoAddress.Contains(name))
                            {
                                //This event series is paused
                                MissionManager missionManager = MissionFunctions.GetMissionManager();
                                missionManager.pauseEventSeries = true;

                                //The game is paused
                                Time.timeScale = 0;

                                //The video player is made visible
                                SetVideoPlayerToVisible();

                                //The video is prepared and played
                                ogVideoPlayer.videoPlayer.source = VideoSource.Url;
                                ogVideoPlayer.videoPlayer.controlledAudioTrackCount = 1;
                                ogVideoPlayer.videoPlayer.EnableAudioTrack(0, true);
                                ogVideoPlayer.videoPlayer.url = videoAddress;
                                ogVideoPlayer.videoPlayer.Prepare();
                                ogVideoPlayer.videoPlayer.errorReceived += OnError;

                                while (ogVideoPlayer.videoPlayer.isPrepared == false)
                                {
                                    yield return null;
                                }

                                ogVideoPlayer.videoPlayer.Play();

                                //This mutes game sounds
                                AudioFunctions.MuteSelectedAudio("externalvolume");
                                AudioFunctions.MuteSelectedAudio("enginevolume");
                                AudioFunctions.MuteSelectedAudio("explosionsvolume");
                                AudioFunctions.MuteSelectedAudio("cockpitvolume");

                                //When the video is finished, stop and close the videoplayer
                                Task a = new Task(CloseVideoPlayerWhenFinished());
                                
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    //This stops the video and returns the player to the game
    public static IEnumerator CloseVideoPlayerWhenFinished()
    {
        OGVideoPlayer ogVideoPlayer = GetOGVideoPlayer();

        if (ogVideoPlayer.videoPlayer != null)
        {
            yield return new WaitForSecondsRealtime(1);

            while (ogVideoPlayer.videoPlayer.isPlaying == true)
            {
                yield return null;
            }

            MissionManager missionManager = MissionFunctions.GetMissionManager();
            missionManager.pauseEventSeries = false;

            Time.timeScale = 1;

            //This unmutes game sounds
            AudioFunctions.UnmuteSelectedAudio("externalvolume");
            AudioFunctions.UnmuteSelectedAudio("enginevolume");
            AudioFunctions.UnmuteSelectedAudio("explosionsvolume");
            AudioFunctions.UnmuteSelectedAudio("cockpitvolume");

            SetVideoPlayerToInvisible();
        }
    }

    //This sets the video player to visible
    public static void SetVideoPlayerToVisible()
    {
        OGVideoPlayer ogVideoPlayer = GetOGVideoPlayer();

        if (ogVideoPlayer.videoPlayerCG != null)
        {
            ogVideoPlayer.videoPlayerCG.alpha = 1;
        }
    }

    //This sets the video player to invisible
    public static void SetVideoPlayerToInvisible()
    {
        OGVideoPlayer ogVideoPlayer = GetOGVideoPlayer();

        if (ogVideoPlayer.videoPlayerCG != null)
        {
            ogVideoPlayer.videoPlayerCG.alpha = 0;
        }
    }

    #endregion

    #region unloading functions

    //This destroys the OG Video Player
    public static void UnloadOGVideoPlayer()
    {
        OGVideoPlayer ogVideoPlayer = GetOGVideoPlayer();

        if (ogVideoPlayer.videoPlayerCG != null)
        {
            GameObject.Destroy(ogVideoPlayer.gameObject);
        }
    }

    #endregion

    #region 

    //This grabs the OGVideoPlayer reference in the scene
    public static OGVideoPlayer GetOGVideoPlayer()
    {
        OGVideoPlayer ogVideoPlayer = GameObject.FindFirstObjectByType<OGVideoPlayer>();

        return ogVideoPlayer;
    }

    public static void OnError(VideoPlayer source, string message)
    {
        Debug.LogError("VideoPlayer error: " + message + " clip=" + (source.clip != null ? source.clip.name : "null"));
    }

    #endregion
}
