using UnityEngine;

public class HangarLaunch : MonoBehaviour
{
    public GameObject ship;
    public GameObject startlocation;
    public GameObject endlocation;
    public GameObject hangarCamera;
    public GameObject hangar;

    public void CloseHangarLaunch()
    {
        HangarLaunchFunctions.CloseHangarLaunch(this);
    }
}
