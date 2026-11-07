using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    //Key References
    public Scene scene;
    public Ship mainShip;
    public Camera mainCamera;

    //Object lists
    public List<CanvasGroup> canvasGroups;
    public List<Text> textBoxes;
    public List<Slider> sliders;
    
    //Radar Display
    public GameObject radarTarget;
    public GameObject radarTargetSelected;
    public List<GameObject> radarTargets;

    //Targeting System
    public GameObject target;
    public GameObject targetSelected;
    public List<GameObject> targets;

    //Ship Preview
    public List<GameObject> wireframePool; 
    public GameObject wireframeObject;
    public Material wireframeMaterial;

    //Waypoint
    public bool waypointActive;
    public string waypointTitle;
    public GameObject waypointGO;

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        HudFunctions.FindMainShip(this);
        HudFunctions.DisplayShipInfo(this);
        HudFunctions.DisplayFrontRadar(this);
        HudFunctions.DisplayTargets(this);
        HudFunctions.DisplayEnergyManagementMeters(this);
        HudFunctions.DisplayShipPreview(this);
        HudFunctions.DisplayTargetInformation(this);
        HudFunctions.DisplayInterceptPoint(this);
        HudFunctions.DisplayWaypoint(this);
    }
}
