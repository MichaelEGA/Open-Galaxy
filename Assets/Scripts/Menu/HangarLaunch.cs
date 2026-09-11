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
    public GameObject displayShip01;
    public GameObject displayShip02;
    public GameObject displayShip03;
    public GameObject displayShip04;
    public GameObject displayShip05;
    public GameObject displayShip06;
    public GameObject displayShip07;
    public GameObject displayShip08;

    public void CloseHangarLaunch()
    {
        Task a = new Task(HangarLaunchFunctions.LaunchShip(this));
    }
}
