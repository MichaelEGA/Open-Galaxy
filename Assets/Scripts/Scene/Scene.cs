using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This holds all the key elements of the scene and makes the avaible to other scripts
public class Scene : MonoBehaviour
{
    [Header("Scene Information")]
    public string location;

    [Header("The Main Ship")]
    [HideInInspector] public GameObject mainShip;

    [Header("Prefab and Material Pools")]
    [HideInInspector] public Object[] objectPrefabPool;
    [HideInInspector] public Object[] particlePrefabPool;
    [HideInInspector] public Object[] asteroidMaterialsPool;

    [Header("GameObject Pools")]
    [HideInInspector] public List<GameObject> objectPool;
    [HideInInspector] public List<GameObject> asteroidPool;
    [HideInInspector] public List<GameObject> particlesPool;
    [HideInInspector] public List<GameObject> lasersPool;
    [HideInInspector] public List<GameObject> torpedosPool;

    [Header("Collision Avoidance Checks")]
    [HideInInspector] public bool avoidSmallObjectsRunning;
    [HideInInspector] public bool avoidLargeObjectsRunning;

    [Header("Cameras")]
    [HideInInspector] public GameObject mainCamera;
    [HideInInspector] public GameObject planetCamera;
    [HideInInspector] public GameObject starfieldCamera;

    [Header("Avoid Collison")]
    [HideInInspector] public float loadTime;
    [HideInInspector] public bool runAvoidCollision = true;

    [Header("Screen Capture")]
    [HideInInspector] public float pressTime;

    // Update is called once per frame
    void Update()
    {
        SceneFunctions.RecenterScene(mainShip);
        SceneFunctions.RotateStarfieldAndPlanetCamera(this);
        AvoidCollisionsFunctions.AvoidCollision(this);
        SceneFunctions.TakeScreeenShot(this);
    }

}

    