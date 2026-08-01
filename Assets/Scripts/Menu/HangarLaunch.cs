using UnityEngine;

public class HangarLaunch : MonoBehaviour
{
    public GameObject ship;
    public GameObject startlocation;
    public GameObject endlocation;
    public GameObject groundlocation;
    public GameObject cameralocation;
    public GameObject camera;
    public GameObject hangar;
    public GameObject cockpit;
    public GameObject launchbutton;

    public void CloseHangarLaunch()
    {
        Task a = new Task(HangarLaunchFunctions.LaunchShip(this));
    }
}
