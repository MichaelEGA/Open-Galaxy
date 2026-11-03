using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud_New : MonoBehaviour
{
    public Ship mainShip;
    public List<CanvasGroup> canvasGroups;
    public List<Text> textBoxes;

    void Start()
    {
        HudFunctions_New.FindMainShip(this);
    }

    // Update is called once per frame
    void Update()
    {
        HudFunctions_New.DisplayShipInfo(this);
    }
}
