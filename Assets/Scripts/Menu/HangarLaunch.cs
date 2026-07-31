using UnityEngine;

public class HangarLaunch : MonoBehaviour
{
    public GameObject startlocation;
    public GameObject endlocation;
    public GameObject hangarCamera;
    public GameObject hangar;

    public void CloseHangarLaunch()
    {
        Task a = new Task(HangarLaunchFunctions.LaunchShip(this));
    }
}
