using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;


//These functions are used to generate a scene including scenery and ship loading/unloading
public static class SceneFunctions
{
    #region start functions

    //This loads the scene script and object
    public static void CreateScene()
    {
        GameObject sceneGO = GameObject.Find("scene");

        if (sceneGO == null)
        {
            sceneGO = new GameObject();
            Scene scene = sceneGO.AddComponent<Scene>();
            scene.loadTime = Time.time;
            sceneGO.name = "scene";

            LoadScenePrefabs();
            GetCameras();
        }
    }

    //This loads all the all the scene objects
    public static void LoadScenePrefabs()
    {
        Scene scene = GetScene();
        OGSettings settings = OGSettingsFunctions.GetSettings();

        //This loads the ship prefabs
        Object[] otShipPrefabs = Resources.LoadAll(OGGetAddress.ships_originaltrilogy, typeof(GameObject));

        if (otShipPrefabs != null)
        {
            scene.otShipsPrefabPool = new GameObject[otShipPrefabs.Length];
            scene.otShipsPrefabPool = otShipPrefabs;
        }

        Object[] fsShipPrefabs = Resources.LoadAll(OGGetAddress.ships_firststrike, typeof(GameObject));

        if (fsShipPrefabs != null)
        {
            scene.fsShipsPrefabPool = new GameObject[fsShipPrefabs.Length];
            scene.fsShipsPrefabPool = fsShipPrefabs;
        }

        Object[] gcShipPrefabs = Resources.LoadAll(OGGetAddress.ships_galacticconquest, typeof(GameObject));

        if (gcShipPrefabs != null)
        {
            scene.gcShipsPrefabPool = new GameObject[gcShipPrefabs.Length];
            scene.gcShipsPrefabPool = gcShipPrefabs;
        }

        Object[] caShipPrefabs = Resources.LoadAll(OGGetAddress.ships_communityassets, typeof(GameObject));

        if (caShipPrefabs != null)
        {
            scene.caShipsPrefabPool = new GameObject[caShipPrefabs.Length];
            scene.caShipsPrefabPool = caShipPrefabs;
        }

        //This loads the cockpit prefabs
        Object[] fsCockpitPrefabs = Resources.LoadAll(OGGetAddress.cockpits_firststrike, typeof(GameObject));

        if (fsCockpitPrefabs != null)
        {
            scene.fsCockpitPrefabPool = new GameObject[fsCockpitPrefabs.Length];
            scene.fsCockpitPrefabPool = fsCockpitPrefabs;
        }

        Object[] gcCockpitPrefabs = Resources.LoadAll(OGGetAddress.cockpits_galacticconquest, typeof(GameObject));

        if (gcCockpitPrefabs != null)
        {
            scene.gcCockpitPrefabPool = new GameObject[gcCockpitPrefabs.Length];
            scene.gcCockpitPrefabPool = gcCockpitPrefabs;
        }

        Object[] caCockpitPrefabs = Resources.LoadAll(OGGetAddress.cockpits_communityassets, typeof(GameObject));

        if (caCockpitPrefabs != null)
        {
            scene.caCockpitPrefabPool = new GameObject[caCockpitPrefabs.Length];
            scene.caCockpitPrefabPool = caCockpitPrefabs;
        }

        //This loads the asteroids
        Object[] asteroidPrefabs = Resources.LoadAll(OGGetAddress.asteroids, typeof(GameObject));

        if (caShipPrefabs != null)
        {
            scene.asteroidPrefabPool = new GameObject[asteroidPrefabs.Length];
            scene.asteroidPrefabPool = asteroidPrefabs;
        }

        //This loads the particle prefabs
        Object[] particlePrefabs = Resources.LoadAll(OGGetAddress.particles, typeof(GameObject));

        if (particlePrefabs != null)
        {
            scene.particlePrefabPool = new GameObject[particlePrefabs.Length];
            scene.particlePrefabPool = particlePrefabs;
        }

        //ADD THIS TO THE ADDRESS SCRIPT THEN CONTINUE WORK FROM HERE
        //This loads the planet materials
        Object[] planetMaterials = Resources.LoadAll(OGGetAddress.planets_planetmaterials, typeof(Material));

        if (planetMaterials != null)
        {
            scene.planetMaterialPool = new Material[planetMaterials.Length];
            scene.planetMaterialPool = planetMaterials;
        }

        Object[] cloudMaterials = Resources.LoadAll(OGGetAddress.planets_cloudmaterials, typeof(Material));

        if (cloudMaterials != null)
        {
            scene.cloudMaterialPool = new Material[cloudMaterials.Length];
            scene.cloudMaterialPool = cloudMaterials;
        }

        Object[] atmosphereMaterials = Resources.LoadAll(OGGetAddress.planets_atmospherematerials, typeof(Material));

        if (atmosphereMaterials != null)
        {
            scene.atmosphereMaterialPool = new Material[atmosphereMaterials.Length];
            scene.atmosphereMaterialPool = atmosphereMaterials;
        }

        scene.hyperspaceTunnelPrefab = Resources.Load(OGGetAddress.hyperspace + "HyperspaceTunnel") as GameObject;

        scene.skyboxes = Resources.LoadAll<Material>(OGGetAddress.skyboxes);    
    }

    //This creates the starfield Camera
    public static void CreateCameras()
    {
        GameObject starfieldCameraGO = GameObject.Find("Starfield Camera");
        Camera starfieldCamera = null;

        bool loading = false;

        if (starfieldCameraGO == null)
        {
            starfieldCameraGO = new GameObject();
            starfieldCameraGO.name = "Starfield Camera";
            starfieldCamera = starfieldCameraGO.AddComponent<Camera>();
            starfieldCamera.nearClipPlane = 0.01f;
            starfieldCamera.cullingMask = LayerMask.GetMask("starfield");
            loading = true;
        }

        GameObject planetCameraGO = GameObject.Find("Planet Camera");
        Camera planetCamera = null;

        if (planetCameraGO == null)
        {
            planetCameraGO = new GameObject();
            planetCameraGO.name = "Planet Camera";
            planetCamera = planetCameraGO.AddComponent<Camera>();
            planetCamera.cullingMask = LayerMask.GetMask("planet");
            planetCamera.nearClipPlane = 0.01f;
            var planetCameraData = planetCamera.GetUniversalAdditionalCameraData();
            planetCameraData.renderType = CameraRenderType.Overlay;
        }

        GameObject mainCameraGO = GameObject.Find("Main Camera");
        Camera mainCamera = null;

        if (mainCameraGO == null)
        {
            mainCameraGO = new GameObject();
            mainCameraGO.name = "Main Camera";
            mainCameraGO.tag = "MainCamera";
            mainCameraGO.AddComponent<AudioListener>();
            mainCamera = mainCameraGO.AddComponent<Camera>();
            mainCamera.cullingMask = LayerMask.GetMask("Default", "collision_asteroid", "collision01", "collision02", "collision03", "collision04", "collision05", "collision06", "collision07", "collision08", "collision09", "collision10");
            mainCamera.nearClipPlane = 0.01f;
            mainCamera.farClipPlane = 90000;
            var mainCameraData = mainCamera.GetUniversalAdditionalCameraData();
            mainCameraData.renderType = CameraRenderType.Overlay;
            mainCameraData.renderPostProcessing = true;
        }

        GameObject cockpitCameraGO = GameObject.Find("Cockpit Camera");
        Camera cockpitCamera = null;

        if (cockpitCameraGO == null)
        {
            cockpitCameraGO = new GameObject();
            cockpitCameraGO.name = "Cockpit Camera";
            cockpitCamera = cockpitCameraGO.AddComponent<Camera>();
            cockpitCamera.cullingMask = LayerMask.GetMask("cockpit");
            cockpitCamera.nearClipPlane = 0.01f;
            var cockpitCameraData = cockpitCamera.GetUniversalAdditionalCameraData();
            cockpitCameraData.renderType = CameraRenderType.Overlay;
        }

        if (loading == true)
        {
            var starfieldCameraData = starfieldCamera.GetUniversalAdditionalCameraData();
            starfieldCameraData.cameraStack.Add(planetCamera);
            starfieldCameraData.cameraStack.Add(mainCamera);
            starfieldCameraData.cameraStack.Add(cockpitCamera);
        }
    }

    //This gets the cameras and adds them to the scene script
    public static void GetCameras()
    {
        Scene scene = GetScene();

        GameObject starfieldCamera = GameObject.Find("Starfield Camera");
        GameObject planetCamera = GameObject.Find("Planet Camera");
        GameObject mainCamera = GameObject.Find("Main Camera");
        GameObject cockpitCamera = GameObject.Find("Cockpit Camera");

        scene.starfieldCamera = starfieldCamera;
        scene.planetCamera = planetCamera;
        scene.mainCamera = mainCamera;
        scene.cockpitCamera = cockpitCamera;
    }

    //This ensures all the right cameras are on
    public static void ResetCameras()
    {
        Scene scene = GetScene();

        if (scene.starfieldCamera != null)
        {
            scene.starfieldCamera.GetComponent<Camera>().enabled = true;
        }

        if (scene.planetCamera != null)
        {
            scene.planetCamera.GetComponent<Camera>().enabled = true;
        }

        if (scene.mainCamera != null)
        {
            scene.mainCamera.GetComponent<Camera>().enabled = true;
        }

        if (scene.cockpitCamera != null)
        {
            scene.cockpitCamera.GetComponent<Camera>().enabled = true;
        }
    }

    #endregion

    #region starfield creation

    //This creates the starfield
    public static IEnumerator GenerateStarField()
    {
        GameObject starfield = new GameObject();
        starfield.name = "starfield";

        starfield.layer = LayerMask.NameToLayer("starfield");

        ParticleSystem particleSystem = starfield.AddComponent<ParticleSystem>();
        ParticleSystemRenderer particleSystemRenderer = starfield.GetComponent<ParticleSystemRenderer>();

        var psMain = particleSystem.main;
        psMain.startSize = 1;
        psMain.simulationSpace = ParticleSystemSimulationSpace.World;
        psMain.loop = false;
        psMain.playOnAwake = false;
        psMain.maxParticles = 10000;
        psMain.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;

        var psEmission = particleSystem.emission;
        psEmission.enabled = false;

        var psShape = particleSystem.shape;
        psShape.enabled = false;

        Material starMaterial = Resources.Load(OGGetAddress.particles + "Star/Star") as Material;
        starfield.GetComponent<ParticleSystemRenderer>().material = starMaterial as Material;

        yield return new WaitForSeconds(10);

        //This loads the Json file
        TextAsset starSystemFile = Resources.Load(OGGetAddress.files + "StarSystems") as TextAsset;
        StarSystems starSystems = JsonUtility.FromJson<StarSystems>(starSystemFile.text);

        //This creates the key values
        Vector3 starVelocity = new Vector3(1, 1, 1);
        int i = 0;

        //This creates the particles
        ParticleSystem.Particle[] points = new ParticleSystem.Particle[10000];

        //This loads a star for every planet in the star wars galaxy
        foreach (StarSystem starsystem in starSystems.starSystemsData)
        {
            float xCoord = (starsystem.X / 15000f) * 50f;
            float yCoord = (starsystem.Z / 15000f) * 25f; //The y coord is actaully the z coord and vice versa b/c original data was vector 2 
            float zCoord = (starsystem.Y / 15000f) * 50f;

            points[i].position = new Vector3(xCoord, yCoord, zCoord);
            points[i].startSize = Random.Range(0.05f, 0.05f);
            points[i].startColor = new Color(1, 1, 1, 1);
            points[i].velocity = new Vector3(1, 1);
            i += 1;
        }

        //This creates another 8000 or so stars to fill in the space, especially the unknown regions
        while (i < 10000)
        {
            float xCoord = Random.Range(-50, 50);
            float yCoord = Random.Range(-25, 25); //The y coord is actaully the z coord and vice versa b/c original data was vector 2 
            float zCoord = Random.Range(-50, 50);

            points[i].position = new Vector3(xCoord, yCoord, zCoord);
            points[i].startSize = Random.Range(0.05f, 0.05f);
            points[i].startColor = new Color(1, 1, 1, 1);
            points[i].velocity = new Vector3(1, 1);
            i += 1;
        }

        particleSystemRenderer.maxParticleSize = 0.005f;
        particleSystemRenderer.minParticleSize = 0f;
        particleSystemRenderer.renderMode = ParticleSystemRenderMode.Billboard;
        particleSystemRenderer.alignment = ParticleSystemRenderSpace.View;
        particleSystem.SetParticles(points, points.Length);
        particleSystemRenderer.lengthScale = 1;
    }

    //Stretch Starfield
    public static IEnumerator StretchStarfield()
    {
        GameObject starfield = GetStarfield();
        Scene scene = GetScene();

        if (starfield != null & scene != null)
        {
            ParticleSystem particleSystem = starfield.GetComponent<ParticleSystem>();
            ParticleSystemRenderer particleSystemRenderer = starfield.GetComponent<ParticleSystemRenderer>();

            if (particleSystemRenderer != null & scene.mainShip != null)
            {
                Rigidbody rigidbody = scene.mainShip.GetComponent<Rigidbody>();

                if (rigidbody != null)
                {
                    ParticleSystem.Particle[] points = new ParticleSystem.Particle[particleSystem.particleCount];

                    int particleNo = particleSystem.GetParticles(points);

                    for (int i = 0; i < particleNo; i++)
                    {
                        points[i].velocity = rigidbody.linearVelocity;
                    }

                    particleSystem.SetParticles(points, points.Length);
                    particleSystemRenderer.renderMode = ParticleSystemRenderMode.Stretch;

                    float waitTime = 10f / 1000f;

                    while (particleSystemRenderer.lengthScale < 1000)
                    {
                        particleSystemRenderer.lengthScale += 20;
                        yield return new WaitForSecondsRealtime(waitTime);
                    }
                }
            }
        }
    }

    //Shrink Starfield
    public static IEnumerator ShrinkStarfield()
    {
        GameObject starfield = GetStarfield();
        Scene scene = GetScene();

        if (starfield != null & scene != null)
        {
            ParticleSystem particleSystem = starfield.GetComponent<ParticleSystem>();
            ParticleSystemRenderer particleSystemRenderer = starfield.GetComponent<ParticleSystemRenderer>();

            if (particleSystemRenderer != null & scene.mainShip != null)
            {
                Rigidbody rigidbody = scene.mainShip.GetComponent<Rigidbody>();

                if (rigidbody != null)
                {
                    ParticleSystem.Particle[] points = new ParticleSystem.Particle[particleSystem.particleCount];

                    int particleNo = particleSystem.GetParticles(points);

                    for (int i = 0; i < particleNo; i++)
                    {
                        points[i].velocity = rigidbody.linearVelocity;
                    }

                    particleSystem.SetParticles(points, points.Length);
                    
                    float waitTime = 10f / 1000f;

                    while (particleSystemRenderer.lengthScale > 1)
                    {
                        particleSystemRenderer.lengthScale -= 20;
                        yield return new WaitForSecondsRealtime(waitTime);
                    }

                    particleSystemRenderer.renderMode = ParticleSystemRenderMode.Billboard;
                }
            }
        }
    }

    //Reset starfield
    public static void ResetStarfield()
    {
        GameObject starfield = GetStarfield();
        Scene scene = GetScene();

        if (starfield != null & scene != null)
        {
            ParticleSystem particleSystem = starfield.GetComponent<ParticleSystem>();
            ParticleSystemRenderer particleSystemRenderer = starfield.GetComponent<ParticleSystemRenderer>();

            if (particleSystemRenderer != null & scene.mainShip != null)
            {
                ParticleSystem.Particle[] points = new ParticleSystem.Particle[particleSystem.particleCount];

                int particleNo = particleSystem.GetParticles(points);

                for (int i = 0; i < particleNo; i++)
                {
                    points[i].velocity = new Vector3(0,0,0);
                }

                particleSystem.SetParticles(points, points.Length);

                particleSystemRenderer.lengthScale = 1;

                particleSystemRenderer.renderMode = ParticleSystemRenderMode.Billboard;
            }
            
        }
    }

    //Move starfield camera to planet location
    public static void MoveStarfieldCamera(Vector3 coordinates)
    {
        //This finds the starfield camera
        GameObject starfieldCamera = GameObject.Find("Starfield Camera");

        //This prevents the coordinates being outside the bound of the galaxy
        if (coordinates.x > 50)
        {
            coordinates.x = 50;
        }

        if (coordinates.y > 25)
        {
            coordinates.y = 25;
        }

        if (coordinates.z > 50)
        {
            coordinates.z = 50;
        }

        //This moves the starfield camera to the designated coordinates
        if (starfieldCamera != null)
        {
            starfieldCamera.transform.position = coordinates;
        }
    }

    //This grabs the starfield
    public static GameObject GetStarfield()
    {
        GameObject starfield = null;

        starfield = GameObject.Find("starfield");

        return starfield;
    }

    #endregion

    #region transitions

    //Turns the hyperspace tunnel off
    public static void ResetHyperSpaceTunnel()
    {
        Scene scene = GetScene();

        if (scene.hyperspaceTunnel != null)
        {
            scene.hyperspaceTunnel.SetActive(false);
        }
    }

    #endregion

    #region planet creation

    //This generates the planet texture
    public static void GeneratePlanet(string planetType, string cloudType, string atmosphereType, string ringsType, float distance, float planetXRot, float planetYRot, float planetZRot, float pivotXRot, float pivotYRot, float pivotZRot )
    {
        //This gets key references
        Scene scene = GetScene();

        scene.centerPivot = GameObject.Find("centerpivot");

        IgnoreCollisionWithPlanet();

        if (scene.centerPivot == null)
        {
            GameObject planetPrefab = Resources.Load(OGGetAddress.planets + "centerpivot") as GameObject;
            scene.centerPivot = GameObject.Instantiate(planetPrefab);
            scene.planetPivot = GameObject.Find("planetpivot");
            scene.planet = GameObject.Find("planet");
            scene.deathstar = GameObject.Find("deathstar");
            scene.deathstar2 = GameObject.Find("deathstar2");
            scene.clouds = GameObject.Find("clouds");
            scene.atmosphere = GameObject.Find("atmosphere");
            scene.rings = GameObject.Find("rings");
        }

        Renderer planetRenderer = scene.planet.GetComponent<Renderer>();
        Renderer cloudRenderer = scene.clouds.GetComponent<Renderer>();
        Renderer atmosphereRenderer = scene.atmosphere.GetComponent<Renderer>();
        Renderer ringsRenderer = scene.rings.GetComponent<Renderer>();

        //This disables/enables gameobjects
        if (cloudType == "none")
        {
            scene.clouds.SetActive(false);
        }
        else
        {
            scene.clouds.SetActive(true);
        }

        if (atmosphereType == "none")
        {
            scene.atmosphere.SetActive(false);
        }
        else
        {
            scene.atmosphere.SetActive(true);
        }

        if (ringsType == "none")
        {
            scene.rings.SetActive(false);
        }
        else
        {
            scene.rings.SetActive(true);
        }

        if (planetType == "deathstar" || planetType == "deathstar2")
        {
            if (planetType == "deathstar")
            {
                scene.deathstar.SetActive(true);
                scene.deathstar2.SetActive(false);
            }
            else
            {
                scene.deathstar2.SetActive(true);
                scene.deathstar.SetActive(false);
            }

            scene.planet.SetActive(false);
        }
        else
        {
            scene.deathstar.SetActive(false);
            scene.deathstar2.SetActive(false);
            scene.planet.SetActive(true);
        }

        //Set materials
        if (planetType != "deathstar")
        {
            if (planetRenderer != null)
            {
                foreach (Object planetMaterial in scene.planetMaterialPool)
                {
                    if (planetMaterial.name == planetType)
                    {
                        planetRenderer.material = (Material)planetMaterial;
                        break;
                    }
                }
            }
        }

        if (cloudRenderer != null)
        {
            foreach (Object cloudMaterial in scene.cloudMaterialPool)
            {
                if (cloudMaterial.name == cloudType)
                {
                    cloudRenderer.material = (Material)cloudMaterial;
                    break;
                }
            }
        }
        
        if (atmosphereRenderer != null)
        {
            foreach (Object atmosphereMaterial in scene.atmosphereMaterialPool)
            {
                if (atmosphereMaterial.name == atmosphereType)
                {
                    atmosphereRenderer.material = (Material)atmosphereMaterial;
                    break;
                }
            }
        }
        
        //Set rotation and distance
        float actualDistance = (0.6f / 100f) * distance;

        float x = 0.4f + actualDistance;
        float y = 0;
        float z = 0.4f + actualDistance;

        scene.planetPivot.transform.localPosition = new Vector3(x, y, z);

        float xRot = planetXRot;
        float yRot = planetYRot;
        float zRot = planetZRot;

        scene.planetPivot.transform.rotation = Quaternion.Euler(xRot, yRot, zRot);

        xRot = pivotXRot;
        yRot = pivotYRot;
        zRot = pivotZRot;

        scene.centerPivot.transform.rotation = Quaternion.Euler(xRot, yRot, zRot);
    }

    //This return the name of a random location
    public static (string planet, string type, Vector3 location, int seed, string allegiance, string region, string sector) GetRandomLocation()
    {
        LoadScreenFunctions.AddLogToLoadingScreen("Searching for a random planet.", 0, false);

        string planet = null;
        string type = null;
        Vector3 location = new Vector3(0, 0, 0);
        int seed = 0;
        string allegiance = "none";
        string region = "none";
        string sector = "none";

        //This loads the Json file
        TextAsset starSystemFile = Resources.Load(OGGetAddress.files + "StarSystems") as TextAsset;
        StarSystems starSystems = JsonUtility.FromJson<StarSystems>(starSystemFile.text);

        int numberOfStarSystems = starSystems.starSystemsData.Length;
        int randomLocation = Random.Range(0, numberOfStarSystems - 1);

        StarSystem starSystem = starSystems.starSystemsData[randomLocation];

        float xCoord = (starSystem.X / 15000f) * 50f;
        float yCoord = (starSystem.Z / 15000f) * 25f; //The y coord is actaully the z coord and vice versa b/c original data was vector 2 
        float zCoord = (starSystem.Y / 15000f) * 50f;

        planet = starSystem.Planet;
        type = starSystem.planetType;
        location = new Vector3(xCoord, yCoord, zCoord);
        seed = starSystem.seed;
        allegiance = starSystem.faction;
        region = starSystem.Region;
        sector = starSystem.Sector;

        LoadScreenFunctions.AddLogToLoadingScreen("Random Planet found.", 0, false);

        return (planet, type, location, seed, allegiance, region, sector);
    }

    //This returns the data of a specific location
    public static (string planet, string type, Vector3 location, int seed, string allegiance, string region, string sector, bool wasFound) FindLocation(string name)
    {
        LoadScreenFunctions.AddLogToLoadingScreen("Searching for planet data.", 0, false);

        string planet = null;
        string type = null;
        Vector3 location = new Vector3(0, 0, 0);
        int seed = 0;
        string allegiance = "none";
        string region = "none";
        string sector = "none";
        bool wasFound = false;

        //This loads the Json file
        TextAsset starSystemFile = Resources.Load(OGGetAddress.files + "StarSystems") as TextAsset;
        StarSystems starSystems = JsonUtility.FromJson<StarSystems>(starSystemFile.text);

        //This finds specific data on the location
        foreach (StarSystem starSystem in starSystems.starSystemsData)
        {

            if (starSystem.Planet == name)
            {
                float xCoord = (starSystem.X / 15000f) * 50f;
                float yCoord = (starSystem.Z / 15000f) * 25f; //The y coord is actaully the z coord and vice versa b/c original data was vector 2 
                float zCoord = (starSystem.Y / 15000f) * 50f;

                planet = starSystem.Planet;
                type = starSystem.planetType;
                location = new Vector3(xCoord, yCoord, zCoord);
                seed = starSystem.seed;
                allegiance = starSystem.faction;
                region = starSystem.Region;
                sector = starSystem.Sector;
                wasFound = true;
                LoadScreenFunctions.AddLogToLoadingScreen("Planet Data Found.", 0, false);
                break;
            }
        }

        //This provides a random location if the specific planet couldn't be find
        if (planet == null)
        {
            LoadScreenFunctions.AddLogToLoadingScreen("Warning planet data not found. Planet location and data will be randomised.", 0, false);

            int numberOfStarSystems = starSystems.starSystemsData.Length;
            int randomLocation = Random.Range(0, numberOfStarSystems - 1);

            StarSystem starSystem = starSystems.starSystemsData[randomLocation];

            float xCoord = (starSystem.X / 15000f) * 50f;
            float yCoord = (starSystem.Z / 15000f) * 25f; //The y coord is actaully the z coord and vice versa b/c original data was vector 2 
            float zCoord = (starSystem.Y / 15000f) * 50f;

            planet = name;
            type = starSystem.planetType;
            location = new Vector3(xCoord, yCoord, zCoord);
            seed = starSystem.seed;
            allegiance = starSystem.faction;
            region = starSystem.Region;
            sector = starSystem.Sector;
            wasFound = false;
        }

        return (planet, type, location, seed, allegiance, region, sector, wasFound);
    }

    //This ensures the planet does not hit any actual in scene objects
    public static void IgnoreCollisionWithPlanet()
    {
        Physics.IgnoreLayerCollision(27, 6, true);
        Physics.IgnoreLayerCollision(27, 7, true);
        Physics.IgnoreLayerCollision(27, 8, true);
        Physics.IgnoreLayerCollision(27, 9, true);
        Physics.IgnoreLayerCollision(27, 10, true);
        Physics.IgnoreLayerCollision(27, 11, true);
        Physics.IgnoreLayerCollision(27, 12, true);
        Physics.IgnoreLayerCollision(27, 13, true);
        Physics.IgnoreLayerCollision(27, 14, true);
        Physics.IgnoreLayerCollision(27, 15, true);
        Physics.IgnoreLayerCollision(27, 16, true);
        Physics.IgnoreLayerCollision(27, 17, true);
        Physics.IgnoreLayerCollision(27, 18, true);
        Physics.IgnoreLayerCollision(27, 19, true);
        Physics.IgnoreLayerCollision(27, 20, true);
        Physics.IgnoreLayerCollision(27, 21, true);
        Physics.IgnoreLayerCollision(27, 22, true);
        Physics.IgnoreLayerCollision(27, 23, true);
    }

    #endregion

    #region asteroid loading

    //This loads asteroids 
    public static IEnumerator LoadAsteroids(float number, string type, Vector3 location, float width, float height, float length, float maxSize, float minSize, string preference = "none", float percentage = 50, int seed = 1138)
    {
        Vector3[] positions = GetAsteroidPostions(number, height, width, length, seed);
        Vector3[] rotations = GetAsteroidRotations(number, seed);
        float[] sizes = GetAsteroidSizes(number, minSize, maxSize, preference, percentage, seed);
        int[] asteroidTypes = GetAsteroidTypes(number, type, seed);

        Scene scene = GetScene();

        GameObject asteroidAnchor = new GameObject();
        asteroidAnchor.name = "asteroidanchor";
        asteroidAnchor.transform.SetParent(scene.transform);
        asteroidAnchor.transform.localPosition = location;

        Vector3 centerPosition = new Vector3(width/2f, height/2f, length/2f);

        int yieldCount = 0;

        for(int i =0; i < positions.Length; i++)
        {
            GameObject asteroid = GameObject.Instantiate((GameObject)scene.asteroidPrefabPool[asteroidTypes[i]]);
            asteroid.transform.SetParent(asteroidAnchor.transform);
            asteroid.transform.localPosition = positions[i] - centerPosition;
            asteroid.transform.localScale = new Vector3(sizes[i], sizes[i], sizes[i]);
            asteroid.transform.rotation = Quaternion.Euler(rotations[i]);
            asteroid.name = "asteroid";
            asteroid.layer = LayerMask.NameToLayer("collision_asteroid");
            Rigidbody rigidBody = GameObjectUtils.AddRigidbody(asteroid, 100f, 0, 0);
            //MeshCollider meshCollider = asteroid.AddComponent<MeshCollider>();
            //meshCollider.convex = true;

            SphereCollider sphereCollider = asteroid.AddComponent<SphereCollider>();

            //This spins the asteroids up
            rigidBody.angularVelocity = Random.onUnitSphere * Random.Range(0.01f, 0.1f);
            rigidBody.angularDamping = 0; //Prevents the spin from stopping
            rigidBody.linearVelocity = Random.onUnitSphere * Random.Range(0.01f, 0.1f);
            rigidBody.linearDamping = 0; //Prevents the movements from stopping

            if (scene.asteroidPool == null)
            {
                scene.asteroidPool = new List<GameObject>();
            }

            scene.asteroidPool.Add(asteroid);

            yieldCount++;

            if (yieldCount > 100)
            {
                yield return null;
                yieldCount = 0;
            }
        }
    }

    //This gets all the asteroids that can be loaded
    public static int[] GetAsteroidTypes(float number, string type, int seed)
    {
        List<int> asteroidSelection = new List<int>();

        Random.InitState(seed);

        Scene scene = GetScene();

        int asteroidNo = 0;

        //This selects all the asteroid of a certain type
        List<int> asteroidTypes = new List<int>();

        foreach (Object asteroid in scene.asteroidPrefabPool)
        {
            if (asteroid.name.Contains(type))
            {
                asteroidTypes.Add(asteroidNo);
            }

            asteroidNo++;
        }

        //This selects asteroids at randome from the given type
        int asteroidCount = asteroidTypes.Count - 1;

        for (int i = 0; i < number; i++)
        {
            int asteroid = Random.Range(0, asteroidCount);
            asteroidSelection.Add(asteroidTypes[asteroid]);
        }

        return asteroidSelection.ToArray();
    }

    //This gets all the rotations of the asteroids
    public static Vector3[] GetAsteroidRotations(float number, int seed)
    {
        List<Vector3> rotations = new List<Vector3>();

        Random.InitState(seed);

        for (int i = 0; i < number; i++)
        {
            float x = Random.Range(0, 360);
            float y = Random.Range(0, 360);
            float z = Random.Range(0, 360);

            rotations.Add(new Vector3(x, y, z));
        }

        return rotations.ToArray();
    }

    //This gets all the sizes of the asteroids
    public static float[] GetAsteroidSizes(float number, float minSize, float maxSize, string preference, float percentage, int seed)
    {
        List<float> asteroidSizes = new List<float>();

        Random.InitState(seed);

        if (preference == "none") //This results in a asteroid field that is fairly uniform
        {
            for (int i = 0; i < number; i++)
            {
                float size = Random.Range(minSize, maxSize);
                asteroidSizes.Add(size);
            }
        }
        else if (preference == "large") //This results in a asteroid field that is more variegated with some large features
        {
            float limit = (maxSize / 10f);

            if (limit < minSize)
            {
                limit = minSize;
            }

            for (int i = 0; i < number; i++)
            {
                float randomNumber = Random.Range(0, 100);

                bool loadLarge = false;

                if (randomNumber < percentage)
                {
                    loadLarge = true;
                }

                if (loadLarge == true)
                {
                    float size = Random.Range(limit, maxSize);
                    asteroidSizes.Add(size);
                }
                else
                {
                    float size = Random.Range(minSize, limit);
                    asteroidSizes.Add(size);
                }
            }
        }
        else if (preference == "small") //This results in a asteroid field that is more variegated but less large features
        {
            float average = (maxSize + minSize) / 2f;

            for (int i = 0; i < number; i++)
            {
                float randomNumber = Random.Range(0, 100);

                bool loadSmall = false;

                if (randomNumber < percentage)
                {
                    loadSmall = true;
                }

                if (loadSmall == true)
                {
                    float size = Random.Range(minSize, average);
                    asteroidSizes.Add(size);
                }
                else
                {
                    float size = Random.Range(minSize, average);
                    asteroidSizes.Add(size);
                }  
            }
        }

       return asteroidSizes.ToArray();
    }

    //This gets all the positions at once so as not to break the seed
    public static Vector3[] GetAsteroidPostions(float number, float height, float width, float length, int seed)
    {
        List<Vector3> asteroidPositions = new List<Vector3>();

        Random.InitState(seed);

        for (int i = 0; i < number; i++)
        {
            float x = Random.Range(0, width);
            float y = Random.Range(0, height);
            float z = Random.Range(0, length);

            Vector3 position = new Vector3(x, y, z);
            asteroidPositions.Add(position);
        }

        return asteroidPositions.ToArray();
    }

    #endregion

    #region terrain loading

    //This loads the terrain mesh and applies the correct material
    public static void LoadTerrain(string terrainName, string terrainMaterialName, float terrainPosition)
    {
        Scene scene = GetScene();

        //This loads the terrain gameobject
        GameObject terrain = null;

        Object[] terrainPrefabs = Resources.LoadAll(OGGetAddress.terrainmeshes, typeof(GameObject));

        foreach (Object tempTerrain in terrainPrefabs)
        {
            if (terrainName == tempTerrain.name)
            {
                terrain = GameObject.Instantiate((GameObject)tempTerrain);
            }
        }

        if (terrain != null & scene != null)
        {
            Rigidbody terrainRigidbody = terrain.AddComponent<Rigidbody>();
            terrainRigidbody.isKinematic = true;
            terrain.AddComponent<MeshCollider>();
            terrain.layer = 7;
            terrain.transform.SetParent(scene.transform);
            terrain.transform.localPosition = new Vector3(0, terrainPosition, 0);
            scene.terrain = terrain;
        }

        //This loads the view distance plane
        GameObject plane = null;

        foreach (Object tempTerrain in terrainPrefabs)
        {
            if (tempTerrain.name == "Plane")
            {
                plane = GameObject.Instantiate((GameObject)tempTerrain);
            }
        }

        if (plane != null)
        {
            plane.transform.SetParent(scene.transform);
            plane.transform.localPosition = new Vector3(0, -15000, 0);
            scene.viewDistancePlane = plane;
        }

        //This applies the material
        Object[] terrainMaterials = Resources.LoadAll(OGGetAddress.terrainmaterials, typeof(Material));

        Material terrainMaterial = null;

        foreach (Object tempMaterial in terrainMaterials)
        {
            if (terrainMaterialName == tempMaterial.name)
            {
                terrainMaterial = (Material)tempMaterial;
            }
        }

        if (terrain != null & terrainMaterial != null)
        {
            terrain.GetComponent<Renderer>().material = terrainMaterial;
        }
    }

    //This unloads the terrain mesh
    public static void UnloadTerrain()
    {
        Scene scene = GetScene();

        if(scene.terrain != null)
        {
            GameObject.Destroy(scene.terrain);
        }

        if (scene.viewDistancePlane != null)
        {
            GameObject.Destroy(scene.viewDistancePlane);
        }
    }

    #endregion

    #region skybox loading

    //This sets the skybox
    public static void SetSkybox(string name, bool stars, string skyboxColour)
    {
        //This gets the scene reference
        Scene scene = GetScene();

        //This toggles the starfield on and off
        GameObject starfield = GameObject.Find("starfield");

        if (stars == false)
        {
            starfield.layer = LayerMask.NameToLayer("invisible");
        }
        else
        {
            starfield.layer = LayerMask.NameToLayer("starfield");
        }

        //This sets the skybox
        foreach (Material skybox in scene.skyboxes)
        {
            if (skybox.name == name)
            {
                RenderSettings.skybox = skybox;
                break;
            }
        }

        //This sets the fog color to match the skybox
        Color newColour;

        if (ColorUtility.TryParseHtmlString(skyboxColour, out newColour))
        {
            RenderSettings.fogColor = newColour;
        }
    }

    //Creates a circular wall of fog at the set radius from the center point
    public static void DynamicFogWall(Scene scene)
    {
        if (scene.fogwall == null)
        {
            GameObject fogwall = Resources.Load(OGGetAddress.fogwall + "fogwall") as GameObject;
            scene.fogwall = GameObject.Instantiate(fogwall);
            scene.fogwall.transform.SetParent(scene.transform);
            scene.fogwall.transform.localPosition = new Vector3(0, 0, 0);    
        }

        if (scene.mainCamera != null & scene.fogwall != null)
        {
            float distance = 0;

            LayerMask layerMask = LayerMask.GetMask("fogwall");

            RaycastHit hit;

            Physics.queriesHitBackfaces = true;

            if (Physics.Raycast(scene.mainCamera.transform.position, scene.mainCamera.transform.forward, out hit, 30000, layerMask))
            {
                distance = Vector3.Distance(scene.mainCamera.transform.position, hit.point);
            }
                
            RenderSettings.fogEndDistance = distance;
            RenderSettings.fogStartDistance = distance - 1000;

            float scale = scene.fogDistanceFromCenter * 2;

            scene.fogwall.transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    #endregion

    #region ship loading

    //This loads an individual ship in the scene
    public static void LoadSingleShip(       
        Vector3 position,
        Quaternion rotation,
        string type,
        string name,
        string allegiance,
        string cargo,
        bool exitingHyperspace,
        bool isAI, 
        bool dontModifyPosition,
        string laserColor)
    {
        //Get scene script reference
        Scene scene = GetScene();
        Audio audioManager = AudioFunctions.GetAudioManager();

        //This gets the Json ship data
        TextAsset shipTypesFile = Resources.Load(OGGetAddress.files + "ShipTypes") as TextAsset;
        ShipTypes shipTypes = JsonUtility.FromJson<ShipTypes>(shipTypesFile.text);

        //Check for ship type in shipTypeData
        ShipType shipType = null;

        foreach (ShipType tempShipType in shipTypes.shipTypeData)
        {
            if (tempShipType.type == type)
            {
                shipType = tempShipType;
                break;
            }
        }

        //Look for ship model in prefabs and load;
        GameObject ship = InstantiateShipPrefab(shipType.prefab);

        if (cargo.ToLower().Contains("random") || cargo.ToLower().Contains("randomise"))
        {
            string[] cargoTypes = GetCargoTypesList();
            int cargoTypeNo = cargoTypes.Length;
            int randomChoice = Random.Range(0, cargoTypeNo - 1);
            cargo = cargoTypes[randomChoice];
        }

        if (ship != null)
        {

            if (shipType.scriptType == "smallship")
            {
                //Add appropriate ship script
                SmallShip smallShip = ship.AddComponent<SmallShip>();

                if (scene.smallShips == null)
                {
                    scene.smallShips = new List<SmallShip>();
                }

                scene.smallShips.Add(smallShip);

                //Load ship data into script
                smallShip.accelerationRating = shipType.accelerationRating;

                if (name == "none")
                {
                    smallShip.name = shipType.callsign + "- Alpha " + Random.Range(1, 99).ToString("00");
                }
                else
                {
                    smallShip.name = shipType.callsign + "-" + name;
                }

                smallShip.allegiance = allegiance;
                smallShip.wepRating = shipType.wepRating;
                smallShip.hullRating = shipType.hullRating;
                smallShip.hullLevel = shipType.hullRating;
                smallShip.systemsRating = shipType.hullRating;
                smallShip.systemsLevel = shipType.hullRating;
                smallShip.laserFireRating = shipType.laserFireRating;
                smallShip.laserRating = shipType.laserRating;
                smallShip.maneuverabilityRating = shipType.maneuverabilityRating;
                smallShip.shieldRating = shipType.shieldRating;
                smallShip.shieldLevel = shipType.shieldRating;
                smallShip.frontShieldLevel = shipType.shieldRating / 2f;
                smallShip.rearShieldLevel = shipType.shieldRating / 2f;
                smallShip.speedRating = shipType.speedRating;
                smallShip.laserColor = laserColor;
                smallShip.healthSave = shipType.shieldRating + shipType.hullRating;  
                smallShip.type = type;
                smallShip.thrustType = shipType.thrustType;
                smallShip.prefabName = shipType.prefab;
                smallShip.shipClass = shipType.shipClass;
                smallShip.laserAudio = shipType.laserAudio;
                smallShip.engineAudio = shipType.engineAudio;
                smallShip.torpedoNumber = shipType.torpedoRating;
                smallShip.torpedoType = shipType.torpedoType;
                smallShip.cockpitName = shipType.cockpitPrefab;
                smallShip.scene = scene;
                smallShip.audioManager = audioManager;
                smallShip.loadTime = Time.time;
                smallShip.cargo = cargo;
                smallShip.explosionType = shipType.explosionType;
                ship.name = smallShip.name;

                if (smallShip.torpedoNumber == 0)
                {
                    smallShip.hasTorpedos = false;
                }

                //Marks whether the ship is the player or AI
                smallShip.isAI = isAI;

                if (isAI == false)
                {
                    OGSettings settings = OGSettingsFunctions.GetSettings();

                    smallShip.invertUpDown = settings.invertY;
                    smallShip.invertLeftRight = settings.invertX;

                    scene.mainShip = smallShip.gameObject;
                }
            }

            if (shipType.scriptType == "largeship")
            {
                //Add appropriate ship script
                LargeShip largeShip = ship.AddComponent<LargeShip>();

                if (scene.largeShips == null)
                {
                    scene.largeShips = new List<LargeShip>();
                }

                scene.largeShips.Add(largeShip);

                //Load ship data into script
                largeShip.accelerationRating = shipType.accelerationRating;

                if (name == "none")
                {
                    largeShip.name = shipType.callsign + "- Alpha " + Random.Range(1, 99).ToString("00");
                }
                else
                {
                    largeShip.name = shipType.callsign + "-" + name;
                }

                largeShip.allegiance = allegiance;
                largeShip.wepRating = shipType.wepRating;
                largeShip.hullRating = shipType.hullRating;
                largeShip.hullLevel = shipType.hullRating;
                largeShip.systemsRating = shipType.hullRating;
                largeShip.systemsLevel = shipType.hullRating;
                largeShip.laserFireRating = shipType.laserFireRating;
                largeShip.laserRating = shipType.laserRating;
                largeShip.maneuverabilityRating = shipType.maneuverabilityRating;
                largeShip.shieldRating = shipType.shieldRating;
                largeShip.shieldLevel = shipType.shieldRating;
                largeShip.frontShieldLevel = shipType.shieldRating / 2f;
                largeShip.rearShieldLevel = shipType.shieldRating / 2f;
                largeShip.speedRating = shipType.speedRating;
                largeShip.laserColor = laserColor;
                largeShip.type = type;
                largeShip.prefabName = shipType.prefab;
                largeShip.thrustType = shipType.thrustType;
                largeShip.scene = scene;
                largeShip.audioManager = audioManager;
                largeShip.shipClass = shipType.shipClass;
                largeShip.loadTime = Time.time;
                largeShip.cargo = cargo;
                largeShip.explosionType = shipType.explosionType;
                ship.name = largeShip.name;
            }

            //Set ship position and rotation

            if (dontModifyPosition == false)
            {
                ship.transform.position = scene.transform.position + position;
            }
            else
            {
                ship.transform.position = position;
            }          

            ship.transform.rotation = rotation;

            //Add ship to the object pool
            scene.objectPool = PoolUtils.AddToPool(scene.objectPool, ship);

            //parent it to the scene
            ship.transform.SetParent(scene.transform);

            //This sets the layer of the ship
            if (isAI == true || shipType.scriptType == "largeship")
            {
                //This gets the Json ship data
                TextAsset allegiancesFile = Resources.Load(OGGetAddress.files + "Allegiances") as TextAsset;
                Allegiances allegiances = JsonUtility.FromJson<Allegiances>(allegiancesFile.text);
                Allegiance allegianceScript = null;
                int i = 0;

                foreach (Allegiance tempAllegiance in allegiances.allegianceData)
                {
                    if (tempAllegiance.allegiance == allegiance)
                    {
                        allegianceScript = tempAllegiance;
                        break;
                    }

                    i++;
                }

                ship.layer = i + 8;

                GameObjectUtils.SetLayerAllChildren(ship.transform, i + 8);

            }
            else
            {
                ship.layer = LayerMask.NameToLayer("collision_player");

                GameObjectUtils.SetLayerAllChildren(ship.transform, LayerMask.NameToLayer("collision_player"));
            }

            //This causes the ship to come out of hyperspace on loading
            if (exitingHyperspace == true)
            {
                ship.transform.localPosition = ship.transform.localPosition + (- ship.transform.forward * 30000);

                if (shipType.scriptType == "largeship")
                {
                    LargeShip largeShip = ship.GetComponent<LargeShip>();
                    Task a = new Task(LargeShipFunctions.ExitHyperspace(largeShip));
                }
                else if (shipType.scriptType == "smallship")
                {
                    SmallShip smallShip = ship.GetComponent<SmallShip>();
                    Task a = new Task(SmallShipFunctions.ExitHyperspace(smallShip));
                }
            }

        }
    }
    
    //This instanties the designated prefabe from the pool listed in the settings or finds a substitute from another pool if the prefab is not present
    public static GameObject InstantiateShipPrefab(string name)
    {
        GameObject shipPrefab = null;
        GameObject tempPrefab = null;

        OGSettings settings = OGSettingsFunctions.GetSettings();

        //This gets the prefab from the designated pool
        tempPrefab = ReturnShipPrefab(name, settings.shipAssets);

        //This gets a backup prefab from another pool if the selected pool doesn't have the requested prefab
        if (tempPrefab == null)
        {
            tempPrefab = ReturnShipPrefab(name, "originaltrilogy");
        }

        if (tempPrefab == null)
        {
            tempPrefab = ReturnShipPrefab(name, "firststrike");
        }

        if (tempPrefab == null)
        {
            tempPrefab = ReturnShipPrefab(name, "galacticconquest");
        }

        if (tempPrefab == null)
        {
            tempPrefab = ReturnShipPrefab(name, "communityassets");
        }

        //This instantiates the prefab
        if (tempPrefab != null)
        {
            shipPrefab = GameObject.Instantiate(tempPrefab) as GameObject;
        }

        return shipPrefab;
    }

    //This returns the designated prefab from the designated pool
    public static GameObject ReturnShipPrefab(string name, string pool)
    {
        GameObject prefab = null;

        Scene scene = SceneFunctions.GetScene();

        if (pool == "originaltrilogy")
        {
            foreach (GameObject objectPrefab in scene.otShipsPrefabPool)
            {
                if (objectPrefab.name == name)
                {
                    prefab = objectPrefab;
                    break;
                }
            }
        }
        else if (pool == "firststrike")
        {
            foreach (GameObject objectPrefab in scene.fsShipsPrefabPool)
            {
                if (objectPrefab.name == name)
                {
                    prefab = objectPrefab;
                    break;
                }
            }
        }
        else if (pool == "galacticconquest")
        {
            foreach (GameObject objectPrefab in scene.gcShipsPrefabPool)
            {
                if (objectPrefab.name == name)
                {
                    prefab = objectPrefab;
                    break;
                }
            }
        }
        else if (pool == "communityassets")
        {
            foreach (GameObject objectPrefab in scene.caShipsPrefabPool)
            {
                if (objectPrefab.name == name)
                {
                    prefab = objectPrefab;
                    break;
                }
            }
        }
       
        return prefab;
    }

    //A wrapper function for load single ships that loads multiple ships
    public static IEnumerator LoadMultipleShips( 
        Vector3 position,
        Quaternion rotation,
        string type,
        string name,
        string allegiance,
        string cargo,
        int number, 
        string pattern, 
        float width, 
        float length, 
        float height, 
        int shipsPerLine, 
        float positionVariance,
        bool exitingHyperspace,
        bool includePlayer, 
        int playerNo,
        string laserColor)
    {

        //This gets the scene reference
        Scene scene = GetScene();

        //This gets the Json ship data
        TextAsset shipTypesFile = Resources.Load(OGGetAddress.files + "ShipTypes") as TextAsset;
        ShipTypes shipTypes = JsonUtility.FromJson<ShipTypes>(shipTypesFile.text);

        //This finds the ship type to load 
        ShipType shipType = null;

        foreach (ShipType tempShipType in shipTypes.shipTypeData)
        {
            if (tempShipType.type == type)
            {
                shipType = tempShipType;
                break;
            }
        }

        if (name == "none")
        {
            string[] names = new string[] { "alpha", "beta", "gamma", "delta", "epislon", "zeta", "eta", "theta", "iota", "kappa", "lambda", "mu", "nu", "xi", "omicron" };
            int nameNo = names.Length;
            int selectName = Random.Range(0, names.Length - 1);
            name = names[selectName];
        }

        Vector3[] positions = GetPositions(pattern, position, width, length, height, number, shipsPerLine, positionVariance);

        int shipNumber = 0;
        int shipCallNumber = 1;

        foreach (Vector3 tempPosition in positions)
        {
            bool isAI = true;

            if (includePlayer == true & shipNumber == playerNo)
            {
                isAI = false;
            }

            LoadSingleShip(tempPosition, rotation, type, name + shipCallNumber.ToString("00"), allegiance, cargo, exitingHyperspace, isAI, false, laserColor);

            shipCallNumber += 1;
            shipNumber++;

            yield return null;
        }
    }

    //A wrapper functions for load single ships that loads multiple ships on the ground
    public static IEnumerator LoadMultipleShipsOnGround(
        Vector3 position,
        Quaternion rotation,
        string type,
        string name, 
        string allegiance,
        string cargo,
        int number,  
        float length, 
        float width,
        float distanceAboveGround,
        int shipsPerLine,
        float positionVariance,
        bool ifRaycastFailsStillLoad,
        string laserColor)
    {
        //This gets the scene reference
        Scene scene = GetScene();

        //This gets the Json ship data
        TextAsset shipTypesFile = Resources.Load(OGGetAddress.files + "ShipTypes") as TextAsset;
        ShipTypes shipTypes = JsonUtility.FromJson<ShipTypes>(shipTypesFile.text);

        //This finds the ship type to load 
        ShipType shipType = null;

        foreach (ShipType tempShipType in shipTypes.shipTypeData)
        {
            if (tempShipType.type == type)
            {
                shipType = tempShipType;
                break;
            }
        }

        Vector3[] positions = GetPositions("special_rectanglehorizontal", position, width, length, 0, number, shipsPerLine, positionVariance);

        foreach (Vector3 tempPosition in positions)
        {
            Vector3 relativePosition = scene.transform.TransformPoint(tempPosition); 

            Vector3 raycastPos = new Vector3(relativePosition.x, 15000, relativePosition.z);

            RaycastHit hit;

            LayerMask mask = LayerMask.GetMask("collision_asteroid");

            Debug.DrawRay(raycastPos, Vector3.down * 30000, Color.red, 500);

            //NOTE: Raycasts only work when the time scale is set to zero IF "Auto Sync Transform" is set to true in the project settings

            if (Physics.Raycast(raycastPos, Vector3.down, out hit, 30000, mask))
            {
                Vector3 newPosition = hit.point + new Vector3(0,distanceAboveGround,0);
                LoadSingleShip(newPosition, rotation, type, name, allegiance, cargo, false, true, true, laserColor);
            }
            else if (ifRaycastFailsStillLoad == true)
            {
                Vector3 newPosition = new Vector3(relativePosition.x, 0, relativePosition.z);
                LoadSingleShip(newPosition, rotation, type, name, allegiance, cargo, false, true, true, laserColor);
            }

            yield return null;
        }
    }

    public static IEnumerator LoadMultipleShipsFromHangar(
        string type,
        string name,
        string allegiance,
        string cargo,
        int number,
        string launchship,
        int hangarNo,
        float delay,
        string laserColor)
    {
        //This gets the scene reference
        Scene scene = GetScene();

        //This gets the Json ship data
        TextAsset shipTypesFile = Resources.Load(OGGetAddress.files + "ShipTypes") as TextAsset;
        ShipTypes shipTypes = JsonUtility.FromJson<ShipTypes>(shipTypesFile.text);

        //This finds the ship type to load 
        ShipType shipType = null;

        foreach (ShipType tempShipType in shipTypes.shipTypeData)
        {
            if (tempShipType.type == type)
            {
                shipType = tempShipType;
                break;
            }
        }

        if (name == "none")
        {
            string[] names = new string[] { "alpha", "beta", "gamma", "delta", "epislon", "zeta", "eta", "theta", "iota", "kappa", "lambda", "mu", "nu", "xi", "omicron" };
            int nameNo = names.Length;
            int selectName = Random.Range(0, names.Length - 1);
            name = names[selectName];
        }

        GameObject launchShipGO = FindShip(launchship);
        Transform[] hangars = null;
        Transform hangarLaunch = null;

        //This gets all availible hangars on the ship
        if (launchShipGO != null)
        {
            hangars = GameObjectUtils.FindAllChildTransformsContaining(launchShipGO.transform, "hangarLaunch");
        }

        //This selects the chosen hangar
        if (hangars != null)
        {
            //This ensures the hangar number is within the range of availible hangars
            if (hangarNo > hangars.Length)
            {
                hangarNo = hangars.Length;
            }
            else if (hangarNo < 0)
            {
                hangarNo = 0;
            }

            hangarLaunch = hangars[hangarNo];
        }

        //This loads the ship if the hangar launch is found
        if (hangarLaunch != null)
        {
            for (int i = 0; i < number; i++)
            {
                int shipNo = i + 1;

                LoadSingleShip(hangarLaunch.localPosition, hangarLaunch.localRotation, type, name + shipNo.ToString("00"), allegiance, cargo, false, true, false, laserColor);

                yield return new WaitForSeconds(delay);
            }
        }
    }

    //This grabs the locations using the chosen pattern
    public static Vector3[] GetPositions(string pattern, Vector3 position, float width, float length, float height, int shipNumber, int shipsPerLine, float positionVariance)
    {
        if (shipNumber < 1)
        {
            shipNumber = 1;
        }

        if (shipsPerLine < 1)
        {
            shipsPerLine = 1;
        }

        Vector3[] shipPositions = null;
        
        if (pattern == "rectanglehorizontal")
        {
            shipPositions = Pattern_RectangleHorizontal(position, width, length, shipNumber, shipsPerLine, positionVariance);
        }
        else if (pattern == "rectanglevertical")
        {
            shipPositions = Pattern_RectangleVertical(position, width, height, shipNumber, shipsPerLine, positionVariance);
        }
        else if (pattern == "arrowhorizontal")
        {
            shipPositions = Pattern_ArrowHorizontal(position, width, length, shipNumber, positionVariance);
        }
        else if (pattern == "arrowhorizontalinverted")
        {
            shipPositions = Pattern_ArrowHorizontalInverted(position, width, length, shipNumber, positionVariance);
        }
        else if (pattern == "linehorizontallongways")
        {
            shipPositions = Pattern_LineHorizontalLongWays(position, length, shipNumber, positionVariance);
        }
        else if (pattern == "linehorizontalsideways")
        {
            shipPositions = Pattern_LineHorizontalSideWays(position, width, shipNumber, positionVariance);
        }
        else if (pattern == "linevertical")
        {
            shipPositions = Pattern_LineVertical(position, height, shipNumber, positionVariance);
        }
        else if (pattern == "randominsidecube")
        {
            shipPositions = Pattern_RandomInsideCube(position, width, length, height, shipNumber);
        }
        else if (pattern == "special_rectanglehorizontal")
        {
            shipPositions = SpecialPattern_RectangleHorizontal(position, width, length, shipNumber, shipsPerLine, positionVariance);
        }

        return shipPositions;
    }

    //This returns a set of positions in the shape of a rectangle
    public static Vector3[] Pattern_RectangleHorizontal(Vector3 position, float width, float length, int shipNumber, int shipsPerLine, float positionVariance)
    {
        List<Vector3> shipPositions = new List<Vector3>();

        if (shipsPerLine < 2)
        {
            shipsPerLine = 2;
        }

        float lengthRadius = length / 2;
        float widthRadius = width / 2;
        float increment_width = width / (float)shipsPerLine;
        float increment_length = length / (Mathf.Floor((float)shipNumber / (float)shipsPerLine));
        float positionX = 0;
        float positionZ = 0;

        Vector3 adjustedCenterPoint = new Vector3(position.x - widthRadius, position.y, position.z - lengthRadius);

        for (int i = 0; i < shipNumber; i++)
        {
            for (int i2 = 0; i2 < shipsPerLine; i2++)
            {
                float varianceX = Random.Range(0, positionVariance);
                float varianceY = Random.Range(0, positionVariance);
                float varianceZ = Random.Range(0, positionVariance);

                Vector3 variancePosition = new Vector3(varianceX, varianceY, varianceZ);
                Vector3 relativeShipPosition = new Vector3(positionX, 0, positionZ);
                Vector3 actualShipPosition = relativeShipPosition + adjustedCenterPoint + variancePosition;

                shipPositions.Add(actualShipPosition);

                positionX += increment_width;
                i++;
            }

            positionX = 0;
            positionZ += increment_length;
        }

        return shipPositions.ToArray();
    }

    //This returns a set of positions in the shape of a rectangle
    public static Vector3[] Pattern_RectangleVertical(Vector3 position, float width, float height, int shipNumber, int shipsPerLine, float positionVariance)
    {
        List<Vector3> shipPositions = new List<Vector3>();

        if (shipsPerLine < 2)
        {
            shipsPerLine = 2;
        }

        float heightRadius = height / 2;
        float widthRadius = width / 2;
        float increment_width = width / (float)shipsPerLine;
        float increment_height = height / (Mathf.Floor((float)shipNumber / (float)shipsPerLine));
        float positionX = 0;
        float positionY = 0;

        Vector3 adjustedCenterPoint = new Vector3(position.x - widthRadius, position.y - heightRadius, position.z);

        for (int i = 0; i < shipNumber; i++)
        {
            for (int i2 = 0; i2 < shipsPerLine; i2++)
            {
                float varianceX = Random.Range(0, positionVariance);
                float varianceY = 0;
                float varianceZ = Random.Range(0, positionVariance);

                Vector3 variancePosition = new Vector3(varianceX, varianceY, varianceZ);
                Vector3 relativeShipPosition = new Vector3(positionX, positionY, 0);
                Vector3 actualShipPosition = relativeShipPosition + adjustedCenterPoint + variancePosition;

                shipPositions.Add(actualShipPosition);

                positionX += increment_width;
                i++;
            }

            positionX = 0;
            positionY += increment_height;
        }

        return shipPositions.ToArray();
    }

    //This returns a set of positions in the shape of an arrow
    public static Vector3[] Pattern_ArrowHorizontal(Vector3 position, float width, float length, int shipNumber, float positionVariance)
    {
        List<Vector3> shipPositions = new List<Vector3>();

        float lengthRadius = length / 2;
        float widthRadius = width / 2;
        float increment_width = widthRadius / ((Mathf.Floor((float)shipNumber - 1f) / 2));
        float increment_length = length / ((Mathf.Floor((float)shipNumber - 1f) / 2) + 1);
        float positionX = 0;
        float positionZ = 0;

        Vector3 adjustedCenterPoint = new Vector3(position.x - widthRadius, position.y, position.z - lengthRadius);

        for (int i = 0; i < shipNumber; i++)
        {
            float varianceX = Random.Range(0, positionVariance);
            float varianceY = Random.Range(0, positionVariance);
            float varianceZ = Random.Range(0, positionVariance);

            Vector3 variancePosition = new Vector3(varianceX, varianceY, varianceZ);
            Vector3 relativeShipPosition = new Vector3();
            Vector3 actualShipPosition = new Vector3();

            if (i == 0)
            {
                relativeShipPosition = new Vector3(positionX, 0, positionZ);
                actualShipPosition = relativeShipPosition + adjustedCenterPoint + variancePosition;
                shipPositions.Add(actualShipPosition);
            }
            else
            {
                relativeShipPosition = new Vector3(positionX, 0, positionZ);
                actualShipPosition = relativeShipPosition + adjustedCenterPoint + variancePosition;
                shipPositions.Add(actualShipPosition);
                
                if (i + 1 < shipNumber)
                {
                    relativeShipPosition = new Vector3(-positionX, 0, positionZ);
                    actualShipPosition = relativeShipPosition + adjustedCenterPoint + variancePosition;
                    shipPositions.Add(actualShipPosition);
                    i++;
                }
            }

            positionX += increment_width;
            positionZ += increment_length;
        }

        return shipPositions.ToArray();
    }

    //This returns a set of positions in the shape of an arrow pointing in the opposite direction
    public static Vector3[] Pattern_ArrowHorizontalInverted(Vector3 position, float width, float length, int shipNumber, float positionVariance)
    {
        List<Vector3> shipPositions = new List<Vector3>();

        float lengthRadius = length / 2;
        float widthRadius = width / 2;
        float increment_width = widthRadius / ((Mathf.Floor((float)shipNumber - 1f) / 2));
        float increment_length = length / ((Mathf.Floor((float)shipNumber - 1f) / 2) + 1);
        float positionX = 0;
        float positionZ = 0;

        Vector3 adjustedCenterPoint = new Vector3(position.x + widthRadius, position.y, position.z + lengthRadius);

        for (int i = 0; i < shipNumber; i++)
        {
            float varianceX = Random.Range(0, positionVariance);
            float varianceY = Random.Range(0, positionVariance);
            float varianceZ = Random.Range(0, positionVariance);

            Vector3 variancePosition = new Vector3(varianceX, varianceY, varianceZ);
            Vector3 relativeShipPosition = new Vector3();
            Vector3 actualShipPosition = new Vector3();

            if (i == 0)
            {
                relativeShipPosition = new Vector3(positionX, 0, positionZ);
                actualShipPosition = relativeShipPosition + adjustedCenterPoint + variancePosition;
                shipPositions.Add(actualShipPosition);
            }
            else
            {
                relativeShipPosition = new Vector3(positionX, 0, positionZ);
                actualShipPosition = relativeShipPosition + adjustedCenterPoint + variancePosition;
                shipPositions.Add(actualShipPosition);

                if (i + 1 < shipNumber)
                {
                    relativeShipPosition = new Vector3(-positionX, 0, positionZ);
                    actualShipPosition = relativeShipPosition + adjustedCenterPoint + variancePosition;
                    shipPositions.Add(actualShipPosition);
                    i++;
                }
            }

            positionX -= increment_width;
            positionZ -= increment_length;
        }

        return shipPositions.ToArray();
    }

    //This returns a set of positions in a line
    public static Vector3[] Pattern_LineHorizontalLongWays(Vector3 position, float length, int shipNumber, float positionVariance)
    {
        List<Vector3> shipPositions = new List<Vector3>();

        float lengthRadius = length / 2;
        float increment_length = length / (float)shipNumber;
        float positionZ = 0;

        Vector3 adjustedCenterPoint = new Vector3(position.x, position.y, position.z - lengthRadius);

        for (int i = 0; i < shipNumber; i++)
        {
            float varianceX = Random.Range(0, positionVariance);
            float varianceY = Random.Range(0, positionVariance);
            float varianceZ = Random.Range(0, positionVariance);

            Vector3 variancePosition = new Vector3(varianceX, varianceY, varianceZ);
            Vector3 relativeShipPosition = new Vector3(0, 0, positionZ);
            Vector3 actualShipPosition = relativeShipPosition + variancePosition + adjustedCenterPoint;

            shipPositions.Add(actualShipPosition);
            positionZ += increment_length;
        }

        return shipPositions.ToArray();
    }

    //This returns a set of positions in a line
    public static Vector3[] Pattern_LineHorizontalSideWays(Vector3 position, float width, int shipNumber, float positionVariance)
    {
        List<Vector3> shipPositions = new List<Vector3>();

        float widthRadius = width / 2;
        float increment_width = width / (float)shipNumber;
        float positionX = 0;

        Vector3 adjustedCenterPoint = new Vector3(position.x - widthRadius, position.y, position.z);

        for (int i = 0; i < shipNumber; i++)
        {
            float varianceX = Random.Range(0, positionVariance);
            float varianceY = Random.Range(0, positionVariance);
            float varianceZ = Random.Range(0, positionVariance);

            Vector3 variancePosition = new Vector3(varianceX, varianceY, varianceZ);
            Vector3 relativeShipPosition = new Vector3(positionX, 0, 0);
            Vector3 actualShipPosition = relativeShipPosition + variancePosition + adjustedCenterPoint;

            shipPositions.Add(actualShipPosition);
            positionX += increment_width;
        }

        return shipPositions.ToArray();
    }

    //This returns a set of positions in a line
    public static Vector3[] Pattern_LineVertical(Vector3 position, float height, int shipNumber, float positionVariance)
    {
        List<Vector3> shipPositions = new List<Vector3>();

        float heightRadius = height / 2;
        float increment_height = height / (float)shipNumber;
        float positionY = 0;

        Vector3 adjustedCenterPoint = new Vector3(position.x, position.y - heightRadius, position.z);

        for (int i = 0; i < shipNumber; i++)
        {
            float varianceX = Random.Range(0, positionVariance);
            float varianceY = Random.Range(0, positionVariance);
            float varianceZ = Random.Range(0, positionVariance);

            Vector3 variancePosition = new Vector3(varianceX, varianceY, varianceZ);
            Vector3 relativeShipPosition = new Vector3(0, positionY, 0);
            Vector3 actualShipPosition = relativeShipPosition + variancePosition + adjustedCenterPoint;

            shipPositions.Add(actualShipPosition);
            positionY += increment_height;
        }

        return shipPositions.ToArray();
    }

    //This returns a set of random positions inside a rectangle
    public static Vector3[] Pattern_RandomInsideCube(Vector3 position, float width, float length, float height, int shipNumber)
    {
        List<Vector3> shipPositions = new List<Vector3>();

        float lengthRadius = length / 2;
        float widthRadius = width / 2;
        float heightRadius = height / 2;

        Vector3 adjustedCenterPoint = new Vector3(position.x - widthRadius, position.y - heightRadius, position.z - lengthRadius);

        for (int i = 0; i < shipNumber; i++)
        {
            float varianceX = Random.Range(0, width);
            float varianceY = Random.Range(0, height);
            float varianceZ = Random.Range(0, length);

            Vector3 variancePosition = new Vector3(varianceX, varianceY, varianceZ);
            Vector3 actualShipPosition = variancePosition + adjustedCenterPoint;

            shipPositions.Add(actualShipPosition);
        }

        return shipPositions.ToArray();
    }

    //This returns a set of positions in the shape of a rectangle
    public static Vector3[] SpecialPattern_RectangleHorizontal(Vector3 position, float width, float length, int shipNumber, int shipsPerLine, float positionVariance)
    {
        List<Vector3> shipPositions = new List<Vector3>();

        float lengthRadius = length / 2;
        float widthRadius = width / 2;
        float increment_width = width / (float)shipsPerLine;
        float increment_length = length / (Mathf.Floor((float)shipNumber / (float)shipsPerLine));
        float positionX = 0;
        float positionZ = 0;

        Vector3 adjustedCenterPoint = new Vector3(position.x - widthRadius, position.y, position.z - lengthRadius);

        for (int i = 0; i < shipNumber; i++)
        {
            for (int i2 = 0; i2 < shipsPerLine; i2++)
            {
                float varianceX = Random.Range(0, positionVariance);
                float varianceY = 0;
                float varianceZ = Random.Range(0, positionVariance);

                Vector3 variancePosition = new Vector3(varianceX, varianceY, varianceZ);
                Vector3 relativeShipPosition = new Vector3(positionX, 0, positionZ);
                Vector3 actualShipPosition = relativeShipPosition + adjustedCenterPoint + variancePosition;

                shipPositions.Add(actualShipPosition);

                positionX += increment_width;
                i++;
            }

            positionX = 0;
            positionZ += increment_length;
        }

        return shipPositions.ToArray();
    }

    //This is a list of all possible types of randomised cargo
    public static string[] GetCargoTypesList()
    {
        string[] cargoTypesList = new string[] {"Food Stuffs", "Ship Parts", "Machinery", "Passengers", "Workers", "Ag Equipement", "Weapons", "Crew", "No Cargo", "Medical Supplies", "Refrigerated Goods", "Fuel", "Chemicals", "Duracrete", "Durasteel" };

        return cargoTypesList;
    }

    #endregion

    #region cockpit loading

    //This instanties the designated prefabe from the pool listed in the settings or finds a substitute from another pool if the prefab is not present
    public static GameObject InstantiateCockpitPrefab(string name)
    {
        GameObject cockpitPrefab = null;
        GameObject tempPrefab = null;

        OGSettings settings = OGSettingsFunctions.GetSettings();

        //This gets the prefab from the designated pool
        tempPrefab = ReturnCockpitPrefab(name, settings.cockpitAssets);

        //This gets a backup prefab from another pool if the selected pool doesn't have the requested prefab
        if (settings.cockpitAssets != "nocockpits")
        {
            if (tempPrefab == null)
            {
                tempPrefab = ReturnCockpitPrefab(name, "firststrike");
            }

            if (tempPrefab == null)
            {
                tempPrefab = ReturnCockpitPrefab(name, "galacticconquest");
            }

            if (tempPrefab == null)
            {
                tempPrefab = ReturnCockpitPrefab(name, "communityassets");
            }
        }

        //This instantiates the prefab
        if (tempPrefab != null)
        {
            cockpitPrefab = GameObject.Instantiate(tempPrefab) as GameObject;
            cockpitPrefab.transform.position = new Vector3(0, 0, 0);
            cockpitPrefab.layer = LayerMask.NameToLayer("cockpit");
            GameObjectUtils.SetLayerAllChildren(cockpitPrefab.transform, 28);
        }

        return cockpitPrefab;
    }

    //This returns the designated prefab from the designated pool
    public static GameObject ReturnCockpitPrefab(string name, string pool)
    {
        GameObject prefab = null;

        Scene scene = SceneFunctions.GetScene();

        if (pool == "firststrike")
        {
            foreach (GameObject objectPrefab in scene.fsCockpitPrefabPool)
            {
                if (objectPrefab.name == name)
                {
                    prefab = objectPrefab;
                    break;
                }
            }
        }
        else if (pool == "galacticconquest")
        {
            foreach (GameObject objectPrefab in scene.gcCockpitPrefabPool)
            {
                if (objectPrefab.name == name)
                {
                    prefab = objectPrefab;
                    break;
                }
            }
        }
        else if (pool == "communityassets")
        {
            foreach (GameObject objectPrefab in scene.caCockpitPrefabPool)
            {
                if (objectPrefab.name == name)
                {
                    prefab = objectPrefab;
                    break;
                }
            }
        }

        return prefab;
    }

    #endregion

    #region floating point control and camera rotation

    //This recenter the scene so the player is always close to the center to prevent floating point inaccuraries 
    public static void RecenterScene(GameObject playerShip)
    {
        if (playerShip != null)
        {
            Vector3 shipPosition = playerShip.transform.position; //This checks the ships position

            if (shipPosition.x > 1000 || shipPosition.x < -1000 || shipPosition.y > 1000 || shipPosition.y < -1000 || shipPosition.z > 1000 || shipPosition.z < -1000)
            {

                Scene scene = GameObject.FindObjectOfType<Scene>();  //This gets the scene reference

                playerShip.transform.SetParent(null); //This unparents the ship from the scene anchor

                scene.gameObject.transform.SetParent(playerShip.transform); //This parents the scene anchor to the ship

                playerShip.transform.position = new Vector3(0, 0, 0); //This moves the ship back to 0,0,0

                scene.gameObject.transform.SetParent(null); //This unparents the scene anchor from the ship

                playerShip.transform.SetParent(scene.gameObject.transform); //This reparents the ship to the scene anchor

            }
        }
    }

    //This rotates the starfield camera to follow the players rotation
    public static void RotateStarfieldAndPlanetCamera(Scene scene)
    {
        if (scene.starfieldCamera != null & scene.planetCamera != null & scene.mainShip != null)
        {
            scene.starfieldCamera.transform.rotation = scene.mainShip.transform.rotation;
            scene.planetCamera.transform.rotation = scene.mainShip.transform.rotation;
        }
    }

    #endregion

    #region unloading functions

    //This calls all the unloading functions and then deletes the scene
    public static IEnumerator UnloadScene()
    {
        Scene scene = GetScene();

        if (scene != null)
        {
            AvoidCollisionsFunctions.StopAvoidCollision();
            UnloadTerrain();
            yield return null;
            ClearScene(scene);
            GameObject.Destroy(scene.gameObject);
        }
    }

    //This destroys all gameobjects in the scene and then clears the lists
    public static void ClearScene(Scene scene)
    {
        GameObject Camera = GameObject.Find("Main Camera");

        if (Camera != null)
        {
            Camera.transform.parent = null;
        }

        if (scene.objectPool != null)
        {
            foreach (GameObject gameobject in scene.objectPool)
            {
                GameObject.Destroy(gameobject);
            }

            scene.objectPool.Clear();
        }

        if (scene.asteroidPool != null)
        {
            foreach (GameObject gameobject in scene.asteroidPool)
            {
                GameObject.Destroy(gameobject);
            }

            scene.asteroidPool.Clear();
        }

        if (scene.particlesPool != null)
        {
            foreach (GameObject gameobject in scene.particlesPool)
            {
                GameObject.Destroy(gameobject);
            }

            scene.particlesPool.Clear();
        }

        if (scene.lasersPool != null)
        {
            foreach (GameObject gameobject in scene.lasersPool)
            {
                GameObject.Destroy(gameobject);
            }

            scene.lasersPool.Clear();
        }

        if (scene.torpedosPool != null)
        {
            foreach (GameObject gameobject in scene.torpedosPool)
            {
                GameObject.Destroy(gameobject);
            }

            scene.torpedosPool.Clear();
        }

        if (scene.cockpit != null)
        {
            GameObject.Destroy(scene.cockpit);
        }

        if (scene.centerPivot != null)
        {
            GameObject.Destroy(scene.centerPivot);
        }
    }

    //This only clears the scene but leaves the scene loaded and the player intact
    public static void ClearLocation()
    {
        Scene scene = GetScene();

        UnloadTerrain();

        if (scene.objectPool != null)
        {
            foreach (GameObject gameobject in scene.objectPool)
            {
                if (scene.mainShip != gameobject)
                {
                    GameObject.Destroy(gameobject);
                }
            }

            scene.objectPool.Clear();

            scene.objectPool.Add(scene.mainShip);
        }

        //Clears the smallship list but preserves the player smallship
        if (scene.smallShips != null)
        {
            scene.smallShips.Clear();

            SmallShip smallShip = scene.mainShip.GetComponent<SmallShip>();

            if (smallShip != null)
            {
                scene.smallShips.Add(smallShip);
            }
        }

        if (scene.largeShips != null)
        {
            scene.largeShips.Clear();
        }

        if (scene.turrets != null)
        {
            scene.turrets.Clear();
        }

        if (scene.lasersPool != null)
        {
            foreach (GameObject gameobject in scene.lasersPool)
            {
                GameObject.Destroy(gameobject);              
            }

            scene.lasersPool.Clear();
        }

        if (scene.asteroidPool != null)
        {
            foreach (GameObject gameobject in scene.asteroidPool)
            {
                GameObject.Destroy(gameobject);
            }

            scene.asteroidPool.Clear();
        }

        if (scene.centerPivot != null)
        {
            GameObject.Destroy(scene.centerPivot);
        }
    }

    //This unloads the starfield
    public static void UnloadStarfield()
    {
        GameObject starfield = GameObject.Find("starfield");

        if (starfield != null)
        {
            GameObject.Destroy(starfield);
        }
    }

    #endregion

    #region misc functions and utils

    //Allows the player ship or AI to declare itself to the scene as the main ship
    public static void IdentifyAsMainShip(SmallShip smallShip)
    {
        Scene scene = GameObject.FindObjectOfType<Scene>();  //This gets the scene reference

        scene.mainShip = smallShip.gameObject;

    }

    //Allows other scripts to easily grab the scene script so they can access commonly held values
    public static Scene GetScene()
    {
        Scene scene;

        scene = GameObject.FindObjectOfType<Scene>();//This gets the scene reference

        if (scene == null)
        {
            CreateScene();
            scene = GameObject.FindObjectOfType<Scene>();
        }

        return scene;
    }

    //This returns the first ship with that contains the given name
    public static GameObject FindShip(string name)
    {
        GameObject ship = null;

        Scene scene = SceneFunctions.GetScene();

        foreach (GameObject tempShip in scene.objectPool)
        {
            if (tempShip != null)
            {
                if (tempShip.activeSelf == true)
                {
                    if (tempShip.name.Contains(name))
                    {
                        ship = tempShip;
                        break;
                    }
                }
            }
        }

        return ship;
    }

    #endregion

}
