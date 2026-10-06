using System.Collections.Generic;
using UnityEngine;

public class SmallShipAI : MonoBehaviour
{
    [Header("Ship AI")]
    public List<string> aiTags;
    [HideInInspector] public string aiTargetingMode;
    [HideInInspector] public Vector3 aiTargetingErrorMargin = new Vector3(0, 0, 0);
    [HideInInspector] public float aiRetreatTime;
    [HideInInspector] public float aiAttackTime;
    [HideInInspector] public float healthSave;
    [HideInInspector] public bool withdraw;
    [HideInInspector] public bool isAI;
    [HideInInspector] public bool requestingTarget;
    [HideInInspector] public bool aiMatchSpeed;
    [HideInInspector] public bool aiStarted;
    [HideInInspector] public bool aiEvade;
    [HideInInspector] public bool boostIsActive;

    [Header("Formation Flying")]
    public SmallShip followTarget;
    public bool flyInFormation;
    [HideInInspector] public bool positionLocked;
    [HideInInspector] public float xFormationPos;
    [HideInInspector] public float yFormationPos;
    [HideInInspector] public float zFormationPos;

    [HideInInspector] public bool avoidGimbalLock;
}
