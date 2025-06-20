using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitMenu : MonoBehaviour
{
    public void ReturnToGame()
    {
        ExitMenuFunctions.DisplayExitMenu(false);
    }

    public void ExitGame()
    {
        AudioFunctions.UnmuteSelectedAudio("voicevolume");
        AudioFunctions.UnmuteSelectedAudio("externalvolume");
        AudioFunctions.UnmuteSelectedAudio("enginevolume");
        AudioFunctions.UnmuteSelectedAudio("explosionsvolume");
        AudioFunctions.UnmuteSelectedAudio("cockpitvolume");

        MissionFunctions.UnloadMission();
    }
}
