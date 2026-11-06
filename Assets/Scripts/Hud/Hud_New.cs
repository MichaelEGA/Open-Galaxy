using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud_New : MonoBehaviour
{
    //Key References
    public Scene scene;
    public Ship mainShip;
    public List<CanvasGroup> canvasGroups;
    public List<Text> textBoxes;
    
    //Radar Display
    public GameObject radarTarget;
    public GameObject radarTargetSelected;
    public List<GameObject> radarTargets;

    void Start()
    {
        HudFunctions_New.FindMainShip(this);
    }

    // Update is called once per frame
    void Update()
    {
        HudFunctions_New.DisplayShipInfo(this);
        HudFunctions_New.DisplayFrontRadar(this);
    }
}
