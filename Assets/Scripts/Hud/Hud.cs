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
    [HideInInspector] public Camera secondaryCamera;
    [HideInInspector] public Camera starfieldCamera;
    [HideInInspector] public float startTime;
    [HideInInspector] public Color colour;

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
    [HideInInspector] public Text systemsText;
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
    public AudioSource lockBeep;
    [HideInInspector] public GameObject targetObject;
    [HideInInspector] public Text targetDistanceText;
    [HideInInspector] public Text targetType;
    [HideInInspector] public Text targetName;
    [HideInInspector] public Text targetCargo;
    [HideInInspector] public Text targetSpeedText;
    [HideInInspector] public Text targetShieldsText;
    [HideInInspector] public Text targetSystemsText;
    [HideInInspector] public Text targetHullText;
    [HideInInspector] public RawImage reticule;
    [HideInInspector] public RawImage targetLockingReticule;
    [HideInInspector] public RawImage targetLockedReticule;
    [HideInInspector] public bool reticuleFlashing;
    [HideInInspector] public Vector3 lastPosition;
    [HideInInspector] public float speed;

    [Header("Ship Log")]
    [HideInInspector] public Text shipLog;
    [HideInInspector] public Text objectiveLog;

    [Header("Moving Reticule")]
    [HideInInspector] public RawImage movingReticleImage;
    [HideInInspector] public GameObject movingReticule;
    [HideInInspector] public GameObject centerReticule;
    [HideInInspector] public GameObject line;

    [Header("Player Information")]
    public Text title;
    public Text hintTextGO;
    public RawImage hintImage;

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
    [HideInInspector] public Material wireframeMaterial;
    [HideInInspector] public float previousArrowRotation;
    [HideInInspector] public float arrowTargetRotation;
    [HideInInspector] public float arrowLerpTime;

    [Header("Intercept Point")]
    [HideInInspector] public GameObject interceptPoint;

    [Header("Waypoint Information")]
    public bool waypointIsActive;
    [HideInInspector] public GameObject waypointArrow;
    [HideInInspector] public GameObject waypointMarker;
    [HideInInspector] public Text waypointText;
    [HideInInspector] public Text waypointTitle;
    [HideInInspector] public string waypointTitleString;
    [HideInInspector] public float waypointPreviousArrowRotation;
    [HideInInspector] public float waypointArrowTargetRotation;
    [HideInInspector] public float waypointArrowLerpTime;

    [Header("Hud Object Pools")]
    [HideInInspector] public List<GameObject> radarPool;
    [HideInInspector] public List<GameObject> frontRadarDotsPool;
    [HideInInspector] public List<GameObject> rearRadarDotsPool;

    [Header("Hud Task Pool")]
    public List<Task> tasks;

    // Update is called once per frame
    void Update()
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
        HudFunctions.DisplaySystemsStrength(this);

        HudFunctions.DisplayTargetDistance(this);
        HudFunctions.DisplayTargetType(this);
        HudFunctions.DisplayTargetName(this);
        HudFunctions.DisplayTargetCargo(this);
        HudFunctions.DisplayTargetSpeed(this);
        HudFunctions.DisplayTargetShield(this);
        HudFunctions.DisplayTargetSystems(this);
        HudFunctions.DisplayTargetHull(this);
        HudFunctions.DisplayReticule(this);

        HudFunctions.MoveReticule(this);

        HudFunctions.DisplayFrontRadar(this);
        HudFunctions.DisplayRearRadar(this);

        HudFunctions.DisplaySelectionBraces(this);
        HudFunctions.DisplayTargetLockReticule(this);

        HudFunctions.DisplayInterceptPoint(this);

        HudFunctions.DisplayWaypointMarker(this);

        HudFunctions.DisplayShipPreview(this);             
    }
}
