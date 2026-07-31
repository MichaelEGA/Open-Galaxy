using UnityEngine;

public class HangarLaunch : MonoBehaviour
{
    public GameObject ship;
    public GameObject startlocation;
    public GameObject endlocation;
    public GameObject groundlocation;
    public GameObject camera;
    public GameObject hangar;

    public void CloseHangarLaunch()
    {
        Task a = new Task(HangarLaunchFunctions.LaunchShip(this));
    }
}
