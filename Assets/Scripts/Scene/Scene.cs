using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

//This holds all the key elements of the scene and makes the avaible to other scripts
public class Scene : MonoBehaviour
{
    [Header("Scene Information")]
    public string currentLocation;
    public string planetType;
    public int planetSeed;
    public float sceneRadius = 15000;
    public List<string> availibleLocations;
    public string allegiance = "none";

    [Header("The Main Ship")]
    [HideInInspector] public GameObject mainShip;

    [Header("Prefab Pools")]
    [HideInInspector] public Object[] shipsPrefabPool;
    [HideInInspector] public Object[] cockpitPrefabPool;
    [HideInInspector] public Object[] propPrefabPool;
    [HideInInspector] public Object[] particlePrefabPool;
    [HideInInspector] public Object[] planetPrefabPool;
    [HideInInspector] public Object[] terrainTexturesPool;
    [HideInInspector] public GameObject hyperspaceTunnelPrefab;

    [Header("GameObject Pools")]
    [HideInInspector] public List<GameObject> objectPool;
    [HideInInspector] public List<GameObject> asteroidPool;
    [HideInInspector] public List<GameObject> particlesPool;
    [HideInInspector] public List<GameObject> lasersPool;
    [HideInInspector] public List<GameObject> ionPool;
    [HideInInspector] public List<GameObject> torpedosPool;
    [HideInInspector] public List<GameObject> environmentsPool;
    [HideInInspector] public List<GameObject> cockpitPool;
    [HideInInspector] public List<GameObject> tilesPool;
    [HideInInspector] public List<GameObject> tilesSetPool;
    [HideInInspector] public List<GameObject> planetsPool;
    [HideInInspector] public List<Transform> systemTransformsPool;
    [HideInInspector] public GameObject hyperspaceTunnel;
    [HideInInspector] public GameObject cockpit;

    [Header("Script Pools")]
    public bool allocatingTargets;
    [HideInInspector] public List<SmallShip> smallShips;
    [HideInInspector] public List<LargeShip> largeShips;
    [HideInInspector] public List<LaserTurret> turrets;

    [Header("Collision Avoidance")]
    [HideInInspector] public bool avoidSmallObjectsRunning;
    [HideInInspector] public bool avoidLargeObjectsRunning;

    [Header("Skyboxes")]
    public Material[] skyboxes;

    [Header("Lighting")]
    public GameObject sceneLightGO;
    public Light sceneLight;
    public LensFlareComponentSRP lensFlare;

    [Header("Skybox")]
    public GameObject terrain;
    public GameObject viewDistancePlane;
    public GameObject fogwall;
    public float fogDistanceFromCenter = 15000;

    [Header("Cameras")]
    [HideInInspector] public GameObject mainCamera;
    [HideInInspector] public GameObject followCamera;
    [HideInInspector] public GameObject planetCamera;
    [HideInInspector] public GameObject starfieldCamera;
    [HideInInspector] public GameObject cockpitCamera;
    [HideInInspector] public GameObject cockpitAnchor;
    [HideInInspector] public bool followCameraIsActive = true;


    [Header("Starfield")]
    [HideInInspector] public GameObject waypointObject;

    [Header("Avoid Collison")]
    [HideInInspector] public float loadTime;
    [HideInInspector] public bool runAvoidCollision = true;

    [Header("Menus")]
    [HideInInspector] public GameObject missionBriefing;
    [HideInInspector] public GameObject exitMenu;
    [HideInInspector] public GameObject loadingScreen;
    [HideInInspector] public GameObject nextMissionMenu;
    [HideInInspector] public GameObject fade;

    [Header("Screen Capture")]
    [HideInInspector] public float pressTime;

    // Update is called once per frame
    void Update()
    {
        SceneFunctions.RecenterScene(mainShip);
        SceneFunctions.RotateStarfieldAndPlanetCamera(this);
        AvoidCollisionsFunctions.AvoidCollision(this);

        if (allocatingTargets == false)
        {
            Task a = new Task(TargetingFunctions.AllocateTargets_ShipsAI(this));
        }

        Shader.SetGlobalFloat("_unscaledTime", Time.unscaledTime);
    }
}

    