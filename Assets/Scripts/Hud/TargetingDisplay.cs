using UnityEngine;
using UnityEngine.UI;

public class TargetingDisplay : MonoBehaviour
{
    //Key reference
    public Ship ship;

    //Cameras
    public Camera targetingSystemCamera;

    //Main Canvas
    public Canvas targetingDisplayCanvas;

    //Canvas Groups
    public CanvasGroup mainScreen;
    public CanvasGroup targeting;
    public CanvasGroup messages;
    public CanvasGroup objectives;
    public CanvasGroup NotificationBoxSmall;
    public CanvasGroup NotificationBoxLarge;

    //Textboxes
    public Text topBar;
    public Text bottomBar;
    public Text targetingText_Type;
    public Text targetingText_Name;
    public Text targetingText_Cargo;
    public Text targetingText_Speed;
    public Text targetingText_Distance;
    public Text targetingText_Shield;
    public Text targetingText_Hull;
    public Text targetingText_System;
    public Text messagesText;
    public Text objectivesText;
    public Text notBoxSmallText;
    public Text notBoxLargeText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TargetingDisplayFunctions.PrepareTargetingDisplay(this);
    }

    // Update is called once per frame
    void Update()
    {
        TargetingDisplayFunctions.UpdateShipVaribles(this);
    }
}
