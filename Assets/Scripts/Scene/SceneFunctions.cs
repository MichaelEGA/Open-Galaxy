using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using LibNoise;
using LibNoise.Generator;
using LibNoise.Operator;
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
            InstantiateCockpits();
        }
    }

    //This loads all the all the scene objects
    public static void LoadScenePrefabs()
    {
        Scene scene = GetScene();
        OGSettings settings = OGSettingsFunctions.GetSettings();

        Object[] objectPrefabs = Resources.LoadAll("ObjectPrefabs", typeof(GameObject));

        if (objectPrefabs != null)
        {
            scene.objectPrefabPool = new GameObject[objectPrefabs.Length];
            scene.objectPrefabPool = objectPrefabs;
        }

        Object[] cockpitPrefabs = null;

        if (settings != null)
        {
            if (settings.cockpitAssetsAddress != null)
            {
                cockpitPrefabs = Resources.LoadAll(settings.cockpitAssetsAddress, typeof(GameObject));
            }     
        }
        
        if (cockpitPrefabs == null)
        {
            cockpitPrefabs = Resources.LoadAll("CockpitPrefabs/fs_cockpits/", typeof(GameObject));
        }

        if (cockpitPrefabs != null)
        {
            scene.cockpitPrefabPool = new GameObject[cockpitPrefabs.Length];
            scene.cockpitPrefabPool = cockpitPrefabs;
        }

        Object[] tilePrefabs = Resources.LoadAll("TilePrefabs", typeof(GameObject));

        if (tilePrefabs != null)
        {
            scene.tilesPrefabPool = new GameObject[tilePrefabs.Length];
            scene.tilesPrefabPool = tilePrefabs;
        }

        Object[] asteroidMaterials = Resources.LoadAll("AsteroidMaterials", typeof(Material));

        if (asteroidMaterials != null)
        {
            scene.asteroidMaterialsPool = new Material[asteroidMaterials.Length];
            scene.asteroidMaterialsPool = asteroidMaterials;
        }

        Object[] particlePrefabs = Resources.LoadAll("ParticlePrefabs", typeof(GameObject));

        if (particlePrefabs != null)
        {
            scene.particlePrefabPool = new GameObject[particlePrefabs.Length];
            scene.particlePrefabPool = particlePrefabs;
        }

        scene.space = Resources.Load("Data/SkyboxAssets/Space") as Material;
        scene.sky = Resources.Load("Data/SkyboxAssets/Sky") as Material;      
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
            mainCameraGO.AddComponent<AudioListener>();
            mainCamera = mainCameraGO.AddComponent<Camera>();
            mainCamera.cullingMask = LayerMask.GetMask("Default", "collision_asteroid", "collision01", "collision02", "collision03", "collision04", "collision05", "collision06", "collision07", "collision08", "collision09", "collision10");
            mainCamera.nearClipPlane = 0.01f;
            mainCamera.farClipPlane = 30000;
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

        Material starMaterial = Resources.Load("ParticlePrefabs/Star/Star") as Material;
        starfield.GetComponent<ParticleSystemRenderer>().material = starMaterial as Material;

        yield return new WaitForSeconds(10);

        //This loads the Json file
        TextAsset starSystemFile = Resources.Load("Data/Files/StarSystems") as TextAsset;
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

    //Move starfield camera to planet location
    public static void MoveStarfieldCamera(Vector3 location)
    {
        GameObject starfieldCamera = GameObject.Find("Starfield Camera");

        if (starfieldCamera != null)
        {
            starfieldCamera.transform.position = location;
        }
    }

    #endregion

    #region planet creation

    //This generates the planet texture
    public static IEnumerator GeneratePlanetHeightmap(string type, int seed)
    {
        //This gets key references
        Scene scene = GetScene();

        scene.planet = GameObject.Find("Planet");

        if (scene.planet == null)
        {
            GameObject planetPrefab = Resources.Load("Planet/Planet") as GameObject;
            scene.planet = GameObject.Instantiate(planetPrefab);
            scene.planet.name = "Planet";
        }

        GameObject atmosphere = GameObject.Find("Atmosphere");
        Renderer planetRenderer = scene.planet.GetComponent<Renderer>();
        Material planetMaterial = planetRenderer.sharedMaterial;

        Random.InitState(seed);

        //This sets the input for libnoise
        float baseflatFrequency = 2.0f;
        float flatScale = 0.125f;
        float flatBias = -0.25f;
        float terraintypeFrequency = Random.Range(0.25f, 1f); //0.5f;
        float terraintypePersistence = 0.25f;
        float terrainSelectorEdgeFalloff = 0.125f;
        float finalterrainFrequency = 4.0f * Random.Range(1.0f, 2.0f);
        float finalterrainPower = 0.125f * Random.Range(0.5f, 1.5f);

        //This combines all the different libnoise modules/effects needed to create the hightmap
        RidgedMultifractal mountainTerrain = new RidgedMultifractal();

        Billow baseFlatTerrain = new Billow();
        baseFlatTerrain.Frequency = baseflatFrequency;
        baseFlatTerrain.Seed = seed;

        ScaleBias flatTerrain = new ScaleBias(flatScale, flatBias, baseFlatTerrain);

        Perlin terrainType = new Perlin();
        terrainType.Frequency = terraintypeFrequency;
        terrainType.Persistence = terraintypePersistence;
        terrainType.Seed = seed;

        Select terrainSelector = new Select(flatTerrain, mountainTerrain, terrainType);
        terrainSelector.SetBounds(0.0, 1000.0);
        terrainSelector.FallOff = terrainSelectorEdgeFalloff;

        float controlPointNumber = Random.Range(0, 15);
        Terrace terraceTerrain = new Terrace(terrainSelector);
        terraceTerrain.Add(-1f);
        terraceTerrain.Add(1f);

        while (controlPointNumber > 0)
        {
            terraceTerrain.Add(Random.Range(-0.99f, 0.99f));
            controlPointNumber = controlPointNumber - 1;
        }

        Turbulence finalTerrain = new Turbulence(terraceTerrain);
        finalTerrain.Frequency = finalterrainFrequency;
        finalTerrain.Power = finalterrainPower;
        finalTerrain.Seed = seed;

        ModuleBase myModule;
        myModule = finalTerrain;

        //This sets the heightmap resolution according to the player settings
        OGSettings settings = OGSettingsFunctions.GetSettings();

        int mapSize = settings.heightMapResolution;

        Noise2D heightMap = new Noise2D(mapSize, mapSize, myModule);

        //This actually generates the map
        Task a = new Task(heightMap.GenerateSpherical(90, -90, -180, 180, 1, false));
        while (a.Running == true) { yield return new WaitForEndOfFrame(); }

        //This outputs the libnoise result to the planet material use a greyscale preset
        Gradient grayscale = GradientPresets.Grayscale;
        Task b = new Task(heightMap.GetTexture(grayscale, planetRenderer));
        while (b.Running == true) { yield return new WaitForEndOfFrame(); }

        //This loads the planet type data
        TextAsset planetTypesFile = Resources.Load("Data/Files/PlanetTypes") as TextAsset;
        PlanetTypes planetTypes = JsonUtility.FromJson<PlanetTypes>(planetTypesFile.text);

        //This applies the planet type date to the material
        foreach (PlanetType planetTypeTemp in planetTypes.planetTypesData)
        {
            if (planetTypeTemp.planetType == type)
            {
                Texture2D terrainTexture = Resources.Load<Texture2D>("Data/PlanetAssets/Textures/" + planetTypeTemp.terrainTexture);
                Texture2D waterTexture = Resources.Load<Texture2D>("Data/PlanetAssets/Textures/" + planetTypeTemp.waterTexture);
                Texture2D lightsTexture = Resources.Load<Texture2D>("Data/PlanetAssets/Textures/" + planetTypeTemp.lightsTexture);

                Color lightsColor = new Color(255f, 255f, 255f);
                ColorUtility.TryParseHtmlString(planetTypeTemp.lightsColor, out lightsColor);

                Color depthsColor = new Color(255f, 255f, 255f);
                ColorUtility.TryParseHtmlString(planetTypeTemp.depthsColor, out depthsColor);

                Color shallowsColor = new Color(255f, 255f, 255f);
                ColorUtility.TryParseHtmlString(planetTypeTemp.shallowsColor, out shallowsColor);

                Color shoreColor = new Color(255f, 255f, 255f);
                ColorUtility.TryParseHtmlString(planetTypeTemp.shoreColor, out shoreColor);

                Color sandColor = new Color(255f, 255f, 255f);
                ColorUtility.TryParseHtmlString(planetTypeTemp.sandColor, out sandColor);

                Color grassColor = new Color(255f, 255f, 255f);
                ColorUtility.TryParseHtmlString(planetTypeTemp.grassColor, out grassColor);

                Color dirtColor = new Color(255f, 255f, 255f);
                ColorUtility.TryParseHtmlString(planetTypeTemp.dirtColor, out dirtColor);

                Color rockColor = new Color(255f, 255f, 255f);
                ColorUtility.TryParseHtmlString(planetTypeTemp.rockColor, out rockColor);

                Color snowColor = new Color(255f, 255f, 255f);
                ColorUtility.TryParseHtmlString(planetTypeTemp.snowColor, out snowColor);

                Color atmosphereColor = new Color(255f, 255f, 255f);
                ColorUtility.TryParseHtmlString(planetTypeTemp.atmosphereColor, out atmosphereColor);

                //This applies all the material settings on the planet
                planetMaterial.SetTexture("_Terrain_Texture", terrainTexture);
                planetMaterial.SetTexture("_Water_Texture", waterTexture);

                planetMaterial.SetFloat("_Terrain_Scale", planetTypeTemp.terrainScale);
                planetMaterial.SetFloat("_Water_Scale", planetTypeTemp.waterScale);
                planetMaterial.SetFloat("_Terrain_Smoothness", planetTypeTemp.terrainSmoothness);
                planetMaterial.SetFloat("_Water_Smoothness", planetTypeTemp.waterSmoothness);
                planetMaterial.SetFloat("_Terrain_Metallic", planetTypeTemp.terrainMetallic);
                planetMaterial.SetFloat("_Water_Metallic", planetTypeTemp.waterMetallic);
                planetMaterial.SetFloat("_Water_Speed", planetTypeTemp.waterSpeed);

                planetMaterial.SetTexture("_Lights_Texture", lightsTexture);

                planetMaterial.SetFloat("_Lights", planetTypeTemp.lights);
                planetMaterial.SetFloat("_Lights_Height", planetTypeTemp.lightsHeight);
                planetMaterial.SetColor("_Lights_Color", lightsColor);
                planetMaterial.SetFloat("_Lights_Scale", planetTypeTemp.lightsScale);
                planetMaterial.SetFloat("_Lights_Visibility", planetTypeTemp.lightsVisibility);

                planetMaterial.SetFloat("_Depths_Height", planetTypeTemp.depthsHeight);
                planetMaterial.SetFloat("_Shallows_Height", planetTypeTemp.shallowsHeight);
                planetMaterial.SetFloat("_Shore_Height", planetTypeTemp.shoreHeight);
                planetMaterial.SetFloat("_Sand_Height", planetTypeTemp.sandHeight);
                planetMaterial.SetFloat("_Grass_Height", planetTypeTemp.grassHeight);
                planetMaterial.SetFloat("_Dirt_Height", planetTypeTemp.dirtHeight);
                planetMaterial.SetFloat("_Rock_Height", planetTypeTemp.rockHeight);
                planetMaterial.SetFloat("_Snow_Height", planetTypeTemp.snowHeight);

                planetRenderer.material.SetColor("_Depths_Color", depthsColor);
                planetRenderer.material.SetColor("_Shallows_Color", shallowsColor);
                planetRenderer.material.SetColor("_Shore_Color", shoreColor);
                planetRenderer.material.SetColor("_Sand_Color", sandColor);
                planetRenderer.material.SetColor("_Grass_Color", grassColor);
                planetRenderer.material.SetColor("_Dirt_Color", dirtColor);
                planetRenderer.material.SetColor("_Rock_Color", rockColor);
                planetRenderer.material.SetColor("_Snow_Color", snowColor);

                //This changes the material settings on the atompshere
                Renderer atmosphereRenderer = atmosphere.GetComponent<Renderer>();
                atmosphereRenderer.material.EnableKeyword("_EMISSION");
                atmosphereRenderer.material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
                atmosphereRenderer.material.SetColor("_EmissionColor", atmosphereColor * 8);
            }
        }
    }

    //This return the name of a random location
    public static (string planet, string type, Vector3 location, int seed) GetRandomLocation()
    {
        LoadScreenFunctions.AddLogToLoadingScreen("Searching for a random planet.", 0, false);

        string planet = null;
        string type = null;
        Vector3 location = new Vector3(0, 0, 0);
        int seed = 0;

        //This loads the Json file
        TextAsset starSystemFile = Resources.Load("Data/Files/StarSystems") as TextAsset;
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

        LoadScreenFunctions.AddLogToLoadingScreen("Random Planet found.", 0, false);

        return (planet, type, location, seed);
    }

    //This returns the data of a specific location
    public static (string planet, string type, Vector3 location, int seed) GetSpecificLocation(string name)
    {
        LoadScreenFunctions.AddLogToLoadingScreen("Searching for planet data.", 0, false);

        string planet = null;
        string type = null;
        Vector3 location = new Vector3(0, 0, 0);
        int seed = 0;

        //This loads the Json file
        TextAsset starSystemFile = Resources.Load("Data/Files/StarSystems") as TextAsset;
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
                LoadScreenFunctions.AddLogToLoadingScreen("Planet Data Found.", 0, false);
                break;
            }
        }

        //This provides a random location if the specific planet couldn't be find
        if (planet == null)
        {
            LoadScreenFunctions.AddLogToLoadingScreen("Planet data not found. Providing a random substitute.", 0, false);

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
        }

        return (planet, type, location, seed);
    }

    //This sets the distance from the planet
    public static void SetPlanetDistance(int seed)
    {
        GameObject planet = GameObject.Find("Planet");

        Random.InitState(seed);
        float variable = Random.Range(1, 100);
        float distance = (0.6f / 100f) * variable;

        float x = 0.4f + distance;
        float y = 0;
        float z = 0.4f + distance;

        planet.transform.position = new Vector3(x, y, z);

        float xRot = Random.Range(0, 360);
        float yRot = Random.Range(0, 360);
        float zRot = Random.Range(0, 360);

        planet.transform.rotation = Quaternion.Euler(xRot, yRot, zRot);
    }

    #endregion

    #region asteroid field creation

    //This generates a random asteroid field
    public static IEnumerator GenerateAsteroidField(int seed, bool ignoreSeed = false, int asteroidNo = 100)
    {
        LoadScreenFunctions.AddLogToLoadingScreen("Generating asteroid field...", 0, false);

        //This gets the scene reference
        Scene scene = GameObject.FindObjectOfType<Scene>();

        //Search for all asteroid objects and add them to the list
        List<int> asteroidRef = new List<int>();
        int i = 0;

        foreach (GameObject objectPrefab in scene.objectPrefabPool)
        {
            if (objectPrefab.name.Contains("asteroid"))
            {
                asteroidRef.Add(i);
            }

            i++;
        }

        //This sets the seed so that asteroids don't change position or number when you visit the same area twice
        Random.InitState(seed);

        //Pick an asteroid to use for the scene
        int materialChoice = Random.Range(0, scene.asteroidMaterialsPool.Length - 1);

        //Pick number of asteroids to load in the scene
        int asteroidNumber = Random.Range(100, 1000);

        if (ignoreSeed == true)
        {
            asteroidNumber = asteroidNo;
        }

        float changeScale = Random.Range(5, 15);

        //Get asteroid collision layer
        int layerNumber = LayerMask.NameToLayer("collision_asteroid");

        //Instantiate asteroids
        for (int i2 = 0; i2 < asteroidNumber; i2++)
        {
            int asteroidChoice = Random.Range(0, asteroidRef.Count);

            GameObject asteroid = GameObject.Instantiate(scene.objectPrefabPool[asteroidRef[asteroidChoice]]) as GameObject;
            asteroid.transform.SetParent(scene.gameObject.transform);

            Material material = scene.asteroidMaterialsPool[materialChoice] as Material;

            asteroid.GetComponent<Renderer>().material = material;

            float scale = Random.Range(0.001f, 0.1f);

            if (i2 == changeScale)
            {
                scale = Random.Range(1f, 2f);
                changeScale = changeScale + changeScale;
            }

            asteroid.transform.localScale = new Vector3(scale, scale, scale);

            asteroid.layer = layerNumber;

            float distance = 15000;

            float xPos = Random.Range(-distance, distance);
            float yPos = Random.Range(-distance, distance);
            float zPos = Random.Range(-distance, distance);
            asteroid.transform.position = new Vector3(xPos, yPos, zPos);

            float xRotation = Random.Range(0, 360);
            float yRotation = Random.Range(0, 360);
            float zRotation = Random.Range(0, 360);
            asteroid.transform.rotation = Quaternion.Euler(xRotation, yRotation, zRotation);

            if (asteroid.GetComponent<Rigidbody>() == null)
            {
                Rigidbody rigidbody = asteroid.AddComponent<Rigidbody>();
                rigidbody.isKinematic = true;
                rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            }

            if (asteroid.GetComponent<MeshCollider>() == null)
            {
                asteroid.AddComponent<MeshCollider>();
            }

            scene.asteroidPool = PoolUtils.AddToPool(scene.asteroidPool, asteroid);

            //yield return null;
        }

        yield return null;

    }

    #endregion

    #region ground tile creation

    public static IEnumerator GenerateTiles(string sceneType, int xDistance, int yDistance, int tileSize, int seed)
    {
        LoadTileSet();

        Scene scene = GetScene();

        int numberOfTilesX = xDistance / tileSize;
        int numberOfTilesY = yDistance / tileSize;

        float xDistanceRadius = xDistance / 2;
        float yDistanceRadius = yDistance / 2;

        float tileRadius = tileSize / 2f;

        GameObject[,] tiles = new GameObject[numberOfTilesX, numberOfTilesY];

        //This sets the seed so that asteroids don't change position or number when you visit the same area twice
        Random.InitState(seed);

        for (int x = 0; x < numberOfTilesX - 1; x++)
        {
            for (int y = 0; y < numberOfTilesY - 1; y++)
            {
                //This gets the correct tile
                Transform tilePointUp = GetTilePointDownOfTileUp(tiles, x, y);
                Transform tilePointLeft = GetTilePointRightOfTileLeft(tiles, x, y);
                tiles[x, y] = PickTiles(tilePointUp, tilePointLeft);

                //This positions the tile on the grid
                float positionX = ((x * tileSize) + tileRadius) - xDistanceRadius;
                float positionY = ((y * tileSize) + tileRadius) - yDistanceRadius;
                tiles[x, y].transform.position = new Vector3(positionX, 0, positionY);
                tiles[x, y].transform.SetParent(scene.gameObject.transform);

                //yield return null;
            }
        }

        yield return null;

        UnloadTileSet();
    }

    public static void LoadTileSet()
    {
        Scene scene = GetScene();

        foreach (Object tilePrefab in scene.tilesPrefabPool)
        {
            GameObject tempTile = GameObject.Instantiate(tilePrefab) as GameObject;

            if (scene.tilesSetPool == null)
            {
                scene.tilesSetPool = new List<GameObject>();
            }

            scene.tilesSetPool.Add(tempTile);
            tempTile.name = tilePrefab.name;                    
        }
    }

    public static void UnloadTileSet()
    {
        Scene scene = GetScene();

        foreach (GameObject tile in scene.tilesSetPool)
        {
            GameObject.Destroy(tile);
        }

        scene.tilesSetPool.Clear();
    }

    public static GameObject PickTiles(Transform tileUpPointDown, Transform tileLeftPointRight)
    {
        GameObject tileToLoad = null;

        Scene scene = GetScene();

        List<string> useableTilesName = new List<string>();
        List<int> useableTilesRotation = new List<int>();

        foreach (GameObject tile in scene.tilesSetPool)
        {
            tile.SetActive(true);

            Transform connectionPointUp = null;
            Transform connectionPointLeft = null;

            for (int rotation = 0; rotation < 360; rotation = rotation + 90)
            {
                tile.transform.rotation = Quaternion.Euler(0, rotation, 0);

                connectionPointUp = GetTilePointUp(tile);
                connectionPointLeft = GetTilePointLeft(tile);

                if (tileUpPointDown != null)
                {
                    if (connectionPointUp.name == tileUpPointDown.name)
                    {
                        if (tileLeftPointRight != null)
                        {
                            if (connectionPointLeft.name == tileLeftPointRight.name)
                            {
                                useableTilesName.Add(tile.name);
                                useableTilesRotation.Add(rotation);
                            }
                        }
                        else
                        {
                            useableTilesName.Add(tile.name);
                            useableTilesRotation.Add(rotation);
                        }
                    }
                }
                else
                {
                    if (tileLeftPointRight != null)
                    {
                        if (connectionPointLeft.name == tileLeftPointRight.name)
                        {
                            useableTilesName.Add(tile.name);
                            useableTilesRotation.Add(rotation);
                        }
                    }
                    else
                    {
                        useableTilesName.Add(tile.name);
                        useableTilesRotation.Add(rotation);
                    }
                }
            }

            tile.SetActive(false);
        }

        int useableTilesCount = useableTilesName.Count - 1;

        int tileChoice = Random.Range(0, useableTilesCount);

        foreach (GameObject tile in scene.tilesSetPool)
        {
            if (tile.name == useableTilesName[tileChoice])
            {
                tile.SetActive(true);
                
                tileToLoad = GameObject.Instantiate(tile);
                tileToLoad.transform.rotation = Quaternion.Euler(0, useableTilesRotation[tileChoice], 0);

                tileToLoad.name = "Tile" + Random.Range(0, 1000).ToString("0000");
                Rigidbody rigidbody = tileToLoad.AddComponent<Rigidbody>();
                rigidbody.isKinematic = true;
                GameObjectUtils.AddColliders(tileToLoad, false);
                tileToLoad.transform.gameObject.layer = 7;
                GameObjectUtils.SetLayerAllChildren(tileToLoad.transform, 7);

                if (scene.tilesPool == null)
                {
                    scene.tilesPool = new List<GameObject>();
                }

                scene.tilesPool.Add(tileToLoad);

                tile.transform.rotation = Quaternion.identity;
                tile.SetActive(false);
                break;
            }
        }

        return tileToLoad;
    }

    public static Transform GetTilePointDownOfTileUp(GameObject[,] tileTypes, int x, int y)
    {
        GameObject tileUp = null;
        Transform tileUpConnectionPointDown = null;

        int yAxis = y - 1;

        if (yAxis >= 0)
        {
            tileUp = tileTypes[x, yAxis];

            Transform[] tileUpPoints = tileUp.transform.GetComponentsInChildren<Transform>();

            foreach (Transform tilePoint in tileUpPoints)
            {
                GameObject rotationTest = new GameObject();
                rotationTest.transform.position = tileUp.transform.position;

                Vector3 tilePointRelativePosition = tilePoint.position - tileUp.transform.position;
                
                float tilePosition = Vector3.Dot(rotationTest.transform.forward, tilePointRelativePosition.normalized);

                GameObject.Destroy(rotationTest);

                if (tilePosition > 0.55)
                {
                    tileUpConnectionPointDown = tilePoint;
                    break;
                }
            }
        }

        return tileUpConnectionPointDown;
    }

    public static Transform GetTilePointRightOfTileLeft(GameObject[,] tileTypes, int x, int y)
    {
        GameObject tileLeft = null;
        Transform tileLeftConnectionPointRight = null;

        int xAxis = x - 1;

        if (xAxis >= 0)
        {
            tileLeft = tileTypes[xAxis, y];

            Transform[] tileLeftPoints = tileLeft.transform.GetComponentsInChildren<Transform>();

            foreach (Transform tilePoint in tileLeftPoints)
            {
                GameObject rotationTest = new GameObject();
                rotationTest.transform.position = tileLeft.transform.position;

                Vector3 tilePointRelativePosition = tilePoint.position - tileLeft.transform.position;
                float tilePosition = Vector3.Dot(rotationTest.transform.right, tilePointRelativePosition.normalized);

                GameObject.Destroy(rotationTest);

                if (tilePosition > 0.55)
                {
                    tileLeftConnectionPointRight = tilePoint;
                    break;
                }
            }
        }

        return tileLeftConnectionPointRight;
    }

    public static Transform GetTilePointUp(GameObject tile)
    {
        Transform connectionPointUp = null;

        Transform[] tileUpPoints = tile.transform.GetComponentsInChildren<Transform>();

        foreach (Transform tilePoint in tileUpPoints)
        {
            GameObject rotationTest = new GameObject();
            rotationTest.transform.position = tile.transform.position;

            Vector3 tilePointRelativePosition = tilePoint.position - tile.transform.position;
            float tilePosition = Vector3.Dot(rotationTest.transform.forward, tilePointRelativePosition.normalized);

            GameObject.Destroy(rotationTest);

            if (tilePosition < -0.55)
            {
                connectionPointUp = tilePoint;
                break;
            }
        }

        return connectionPointUp;
    }

    public static Transform GetTilePointLeft(GameObject tile)
    {
        Transform connectionPointLeft = null;

        Transform[] tileUpPoints = tile.transform.GetComponentsInChildren<Transform>();

        foreach (Transform tilePoint in tileUpPoints)
        {
            GameObject rotationTest = new GameObject();
            rotationTest.transform.position = tile.transform.position;

            Vector3 tilePointRelativePosition = tilePoint.position - tile.transform.position;
            float tilePosition = Vector3.Dot(rotationTest.transform.right, tilePointRelativePosition.normalized);

            GameObject.Destroy(rotationTest);

            if (tilePosition < -0.55)
            {
                connectionPointLeft = tilePoint;
                break;
            }
        }

        return connectionPointLeft;
    }

    #endregion

    #region ship creation

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
        bool dontModifyPosition)
    {
        //Get scene script reference
        Scene scene = GetScene();
        Audio audioManager = AudioFunctions.GetAudioManager();

        //This gets the Json ship data
        TextAsset shipTypesFile = Resources.Load("Data/Files/ShipTypes") as TextAsset;
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
        GameObject ship = null;

        if (shipType != null)
        {
            foreach (GameObject objectPrefab in scene.objectPrefabPool)
            {
                if (objectPrefab.name == shipType.prefab)
                {
                    ship = GameObject.Instantiate(objectPrefab) as GameObject;
                    break;
                }
            }
        }

        if (cargo.ToLower().Contains("random") || cargo.ToLower().Contains("randomise"))
        {
            string[] cargoTypes = new string[] { "Food Stuffs", "Ship Parts" };
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

                //Load ship data into script
                smallShip.accelerationRating = shipType.accelerationRating;

                if (allegiance == "none")
                {
                    smallShip.allegiance = shipType.allegiance;
                    allegiance = shipType.allegiance;
                }
                else
                {
                    smallShip.allegiance = allegiance;
                }

                if (name == "none")
                {
                    smallShip.name = shipType.callsign + "- Alpha " + Random.Range(1, 99).ToString("00");
                }
                else
                {
                    smallShip.name = shipType.callsign + "-" + name;
                }

                smallShip.wepRating = shipType.wepRating;
                smallShip.hullRating = shipType.hullRating;
                smallShip.hullLevel = shipType.hullRating;
                smallShip.laserFireRating = shipType.laserFireRating;
                smallShip.laserRating = shipType.laserRating;
                smallShip.maneuverabilityRating = shipType.maneuverabilityRating;
                smallShip.shieldRating = shipType.shieldRating;
                smallShip.shieldLevel = shipType.shieldRating;
                smallShip.frontShieldLevel = shipType.shieldRating / 2f;
                smallShip.rearShieldLevel = shipType.shieldRating / 2f;
                smallShip.speedRating = shipType.speedRating;
                smallShip.laserColor = shipType.laserColor;
                smallShip.healthSave = shipType.shieldRating + shipType.hullRating;  
                smallShip.aiSkillLevel = "easy";
                smallShip.type = type;
                smallShip.thrustType = shipType.thrustType;
                smallShip.prefabName = shipType.prefab;
                smallShip.classType = shipType.shipClass;
                smallShip.laserAudio = shipType.laserAudio;
                smallShip.engineAudio = shipType.engineAudio;
                smallShip.torpedoNumber = shipType.torpedoRating;
                smallShip.torpedoType = shipType.torpedoType;
                smallShip.cockpitName = shipType.cockpitPrefab;
                smallShip.scene = scene;
                smallShip.audioManager = audioManager;
                smallShip.loadTime = Time.time;
                smallShip.cargo = cargo;
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

                //Load ship data into script
                largeShip.accelerationRating = shipType.accelerationRating;

                if (allegiance == "none")
                {
                    largeShip.allegiance = shipType.allegiance;
                    allegiance = shipType.allegiance;
                }
                else
                {
                    largeShip.allegiance = allegiance;
                }

                if (name == "none")
                {
                    largeShip.name = shipType.callsign + "- Alpha " + Random.Range(1, 99).ToString("00");
                }
                else
                {
                    largeShip.name = shipType.callsign + "-" + name;
                }

                largeShip.wepRating = shipType.wepRating;
                largeShip.hullRating = shipType.hullRating;
                largeShip.hullLevel = shipType.hullRating;
                largeShip.laserFireRating = shipType.laserFireRating;
                largeShip.laserRating = shipType.laserRating;
                largeShip.maneuverabilityRating = shipType.maneuverabilityRating;
                largeShip.shieldRating = shipType.shieldRating;
                largeShip.shieldLevel = shipType.shieldRating;
                largeShip.frontShieldLevel = shipType.shieldRating / 2f;
                largeShip.rearShieldLevel = shipType.shieldRating / 2f;
                largeShip.speedRating = shipType.speedRating;
                largeShip.laserColor = shipType.laserColor;
                largeShip.aiSkillLevel = "easy";
                largeShip.type = type;
                largeShip.prefabName = shipType.prefab;
                largeShip.thrustType = shipType.thrustType;
                largeShip.scene = scene;
                largeShip.audioManager = audioManager;
                largeShip.classType = shipType.classType;
                largeShip.loadTime = Time.time;
                largeShip.cargo = cargo;
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
            if (isAI == true)
            {
                //This gets the Json ship data
                TextAsset allegiancesFile = Resources.Load("Data/Files/Allegiances") as TextAsset;
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
        int playerNo)
    {

        //This gets the scene reference
        Scene scene = GetScene();

        //This gets the Json ship data
        TextAsset shipTypesFile = Resources.Load("Data/Files/ShipTypes") as TextAsset;
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

        int shipCallNumber = 0;

        foreach (Vector3 tempPosition in positions)
        {
            shipCallNumber += 1;

            bool isAI = true;

            if (includePlayer == true & shipCallNumber == playerNo)
            {
                isAI = false;
            }

            LoadSingleShip(tempPosition, rotation, type, name + shipCallNumber.ToString("00"), allegiance, cargo, exitingHyperspace, isAI, false);

            yield return null;
        }
    }

    public static IEnumerator LoadMultipleShipByClassAndAllegiance(     
        Vector3 position,
        Quaternion rotation,
        string shipClass,
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
        int playerNo)
    {
        //This gets the scene reference
        Scene scene = GetScene();

        //This gets the Json ship data
        TextAsset shipTypesFile = Resources.Load("Data/Files/ShipTypes") as TextAsset;
        ShipTypes shipTypes = JsonUtility.FromJson<ShipTypes>(shipTypesFile.text);

        //This finds the ship types to load
        List<ShipType> shipTypesList = new List<ShipType>();

        foreach (ShipType tempShipType in shipTypes.shipTypeData)
        {
            if (tempShipType.allegiance == allegiance & tempShipType.shipClass.Contains(shipClass))
            {
                shipTypesList.Add(tempShipType);
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

        int i = 0;
        int shipType = 0;

        foreach (Vector3 tempPosition in positions)
        {
            bool isAI = true;

            if (includePlayer == true & i == playerNo)
            {
                isAI = false;
            }

            int shipCallNumber = i + 1; //This ensures the ship call number starts at 01 not 00

            shipType += Random.Range(0, shipTypesList.Count);

            LoadSingleShip(tempPosition, rotation, shipTypesList[shipType].type, name + shipCallNumber.ToString("00"), allegiance, cargo, exitingHyperspace, isAI, false);

            i++;

            yield return null;
        }    
    }

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
        bool ifRaycastFailsStillLoad)
    {
        //This gets the scene reference
        Scene scene = GetScene();

        //This gets the Json ship data
        TextAsset shipTypesFile = Resources.Load("Data/Files/ShipTypes") as TextAsset;
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
            Vector3 raycastPos = new Vector3(tempPosition.x, 2000, tempPosition.z);

            RaycastHit hit;

            LayerMask mask = ~0;

            Debug.DrawRay(raycastPos, Vector3.down * 5000, Color.red, 500);

            //NOTE: Raycasts only work when the time scale is set to zero IF "Auto Sync Transform" is set to true in the project settings

            if (Physics.Raycast(raycastPos, Vector3.down, out hit, 5000, mask))
            {
                Vector3 newPosition = hit.point + new Vector3(0,distanceAboveGround,0);
                LoadSingleShip(newPosition, rotation, type, name, allegiance, cargo, false, true, true);
            }
            else if (ifRaycastFailsStillLoad == true)
            {
                Vector3 newPosition = new Vector3(tempPosition.x, 0, tempPosition.z);
                LoadSingleShip(newPosition, rotation, type, name, allegiance, cargo, false, true, true);
            }

            yield return null;
        }
    }

    //This grabs the locations using the chosen pattern
    public static Vector3[] GetPositions(string pattern, Vector3 position, float width, float length, float height, int shipNumber, int shipsPerLine, float positionVariance)
    {
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
        else if (pattern == "special_randomhorizontal")
        {
            shipPositions = SpecialPattern_RectangleHorizontal(position, width, length, shipNumber, shipsPerLine, positionVariance);
        }

        return shipPositions;
    }

    //This returns a set of positions in the shape of a rectangle
    public static Vector3[] Pattern_RectangleHorizontal(Vector3 position, float width, float length, int shipNumber, int shipsPerLine, float positionVariance)
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

                relativeShipPosition = new Vector3(-positionX, 0, positionZ);
                actualShipPosition = relativeShipPosition + adjustedCenterPoint + variancePosition;
                shipPositions.Add(actualShipPosition);
                i++;
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

                relativeShipPosition = new Vector3(-positionX, 0, positionZ);
                actualShipPosition = relativeShipPosition + adjustedCenterPoint + variancePosition;
                shipPositions.Add(actualShipPosition);
                i++;
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

    #endregion

    #region cockpit creation

    public static void InstantiateCockpits()
    {
        Scene scene = GetScene();

        if (scene.cockpitPool == null)
        {
            scene.cockpitPool = new List<GameObject>();
        }

        foreach (GameObject cockpitPrefab in scene.cockpitPrefabPool)
        {
            GameObject cockpitObject = GameObject.Instantiate(cockpitPrefab) as GameObject;
            scene.cockpitPool.Add(cockpitObject);
            cockpitObject.transform.position = new Vector3(0, 0, 0);
            cockpitObject.layer = LayerMask.NameToLayer("cockpit");
            GameObjectUtils.SetLayerAllChildren(cockpitObject.transform, 28);
            cockpitObject.SetActive(false);
        }
    }

    #endregion

    #region skybox

    public static void SetSkybox(string mode)
    {
        Scene scene = GetScene();

        GameObject starfield = GameObject.Find("starfield");

        if (mode == "sky")
        {
            RenderSettings.skybox = scene.sky;
            starfield.layer = LayerMask.NameToLayer("invisible");

        }
        else
        {
            RenderSettings.skybox = scene.space;
            starfield.layer = LayerMask.NameToLayer("starfield");
        }
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
            yield return null;
            ClearGameObjectPools(scene);
            GameObject.Destroy(scene.gameObject);
        }
    }

    //This destroys all gameobjects in the scene and then clears the lists
    public static void ClearGameObjectPools(Scene scene)
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

        if (scene.tilesPool != null)
        {
            foreach (GameObject gameobject in scene.tilesPool)
            {
                GameObject.Destroy(gameobject);
            }

            scene.tilesPool.Clear();
        }

        if (scene.tilesSetPool != null)
        {
            foreach (GameObject gameobject in scene.tilesSetPool)
            {
                GameObject.Destroy(gameobject);
            }

            scene.tilesSetPool.Clear();
        }

        if (scene.cockpitPool != null)
        {
            foreach (GameObject gameobject in scene.cockpitPool)
            {
                GameObject.Destroy(gameobject);
            }

            scene.cockpitPool.Clear();
        }

        if (scene.planet != null)
        {
            GameObject.Destroy(scene.planet);
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

    //This runs the screenshot function
    public static void TakeScreeenShot(Scene scene)
    {
        Keyboard keyboard = Keyboard.current;

        if (keyboard.f11Key.isPressed == true & scene.pressTime < Time.time)
        {
            Task a = new Task(ScreenCaptureFunctions.CaptureScreenNoHud());
            scene.pressTime = Time.time + 0.2f;
        }
    }

    #endregion

}
