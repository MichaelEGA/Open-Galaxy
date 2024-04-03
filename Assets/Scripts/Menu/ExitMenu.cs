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
        MissionFunctions.ExitAndUnload();
    }
}
