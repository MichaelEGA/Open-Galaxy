using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    [Header("Key References")]
    [HideInInspector] public Scene scene;
    [HideInInspector] public SmallShip smallShip;
    [HideInInspector] public Camera mainCamera;
    [HideInInspector] public float loadTime;

    [Header("Hud Elements Check")]
    [HideInInspector] public bool hudElementsSet;

    [Header("Ship Information")]
    [HideInInspector] public Text shipInfo;
    [HideInInspector] public Text shipName;
    [HideInInspector] public Text speedText;
    [HideInInspector] public Text matchSpeedText;
    [HideInInspector] public Text WEPText;
    [HideInInspector] public Text activeWeaponText;
    [HideInInspector] public Text weaponModeText;
    [HideInInspector] public Text weaponNumberText;
    [HideInInspector] public Text hyperdriveText;
    [HideInInspector] public Slider shieldMeter;
    [HideInInspector] public Slider engineMeter;
    [HideInInspector] public Slider laserMeter;
    [HideInInspector] public Slider WEPMeter;
    [HideInInspector] public RawImage shieldForwardOutside;
    [HideInInspector] public RawImage shieldForwardInside;
    [HideInInspector] public RawImage hull;
    [HideInInspector] public RawImage shieldRearInside;
    [HideInInspector] public RawImage shieldRearOutside;

    [Header("Target Information")]
    [HideInInspector] public GameObject targetObject;
    [HideInInspector] public Text targetDistance;
    [HideInInspector] public Text targetType;
    [HideInInspector] public Text targetName;
    [HideInInspector] public Text targetSpeedText;
    [HideInInspector] public Text targetShieldsText;
    [HideInInspector] public Text targetHullText;
    [HideInInspector] public RawImage reticule;
    [HideInInspector] public RawImage targetLockingReticule;
    [HideInInspector] public RawImage targetLockedReticule;
    [HideInInspector] public bool reticuleFlashing;
    [HideInInspector] public Vector3 lastPosition;
    [HideInInspector] public float speed;

    [Header("Ship Log")]
    [HideInInspector] public Text shipLog;

    [Header("Moving Reticule")]
    [HideInInspector] public GameObject movingReticule;
    [HideInInspector] public GameObject centerReticule;
    [HideInInspector] public GameObject line;

    [Header("Location Information")]
    [HideInInspector] public Text locationInfo;

    [Header("Radar Information")]
    [HideInInspector] public GameObject radarObject;
    [HideInInspector] public GameObject frontRadarCircle;
    [HideInInspector] public GameObject frontRadarDot;
    [HideInInspector] public GameObject frontRadarBrace;
    [HideInInspector] public GameObject rearRadarCircle;
    [HideInInspector] public GameObject rearRadarDot;
    [HideInInspector] public GameObject rearRadarBrace;
    [HideInInspector] public GameObject directionArrow;
    [HideInInspector] public GameObject selectionBrace;
    [HideInInspector] public float previousArrowRotation;
    [HideInInspector] public float arrowTargetRotation;
    [HideInInspector] public float arrowLerpTime;

    [Header("Hud Object Pools")]
    [HideInInspector] public Object[] radarPrefabPool;
    [HideInInspector] public List<GameObject> radarPool;
    [HideInInspector] public List<GameObject> frontRadarDotsPool;
    [HideInInspector] public List<GameObject> rearRadarDotsPool;

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0)
        {
            HudFunctions.UpdateKeyReferences(this);

            HudFunctions.DisplayShipInfo(this);
            HudFunctions.DisplayShieldMeter(this);
            HudFunctions.DisplayEngineMeter(this);
            HudFunctions.DisplayLaserMeter(this);
            HudFunctions.DisplayWEPMeter(this);

            HudFunctions.DisplayShipSpeed(this);
            HudFunctions.DisplayMatchSpeed(this);
            HudFunctions.DisplayWEP(this);
            HudFunctions.DisplayActiveWeapon(this);
            HudFunctions.DisplayWeaponMode(this);
            HudFunctions.DisplayWeaponNumber(this);
            HudFunctions.DisplayShieldAndHull(this);

            HudFunctions.DisplayTargetDistance(this);
            HudFunctions.DisplayTargetType(this);
            HudFunctions.DisplayTargetName(this);
            HudFunctions.DisplayTargetSpeed(this);
            HudFunctions.DisplayTargetShield(this);
            HudFunctions.DisplayTargetHull(this);

            HudFunctions.MoveReticule(this);

            HudFunctions.DisplayFrontRadar(this);
            HudFunctions.DisplayRearRadar(this);

            HudFunctions.DisplaySelectionBraces(this);
            HudFunctions.DisplayTargetLockReticule(this);

            HudFunctions.DisplayShipPreview(this);
        }       
    }
}
