using System.Collections.Generic;
using UnityEngine;

public class FlightControlAI_Small : MonoBehaviour
{
    public Ship ship;

    public List<string> aiTags;
    public string aiTargetingMode;
    public Vector3 aiTargetingErrorMargin = new Vector3(0, 0, 0);
    public float aiRetreatTime;
    public float aiAttackTime;
    public float healthSave;
    public bool withdraw;
    public bool isAI;
    public bool requestingTarget;
    public bool aiMatchSpeed;
    public bool aiStarted;
    public bool aiEvade;
    public bool boostIsActive;

    public FlightControlSystem_Small followTarget;
    public bool flyInFormation;
    public bool positionLocked;
    public float xFormationPos;
    public float yFormationPos;
    public float zFormationPos;

    public bool avoidGimbalLock;

    void Start()
    {
        ship = GetComponent<Ship>();
    }

    // Update is called once per frame
    void Update()
    {
        FlightControlAI_SmallFunctions.GetAIInput(this);
    }
}
