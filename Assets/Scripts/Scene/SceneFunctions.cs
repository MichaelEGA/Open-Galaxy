using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;
using UnityEngine.XR;


//These functions are used to generate a scene including scenery and ship loading/unloading
public static class SceneFunctions
{
    #region start functions

    //This loads the scene script and object
    public static Scene CreateScene()
    {
        GameObject sceneGO = new GameObject();
        Scene scene = sceneGO.AddComponent<Scene>();
        scene.loadTime = Time.time;
        sceneGO.name = "scene";

        LoadScenePrefabs();

        return scene;
    }

    //This loads all the all the scene objects
    public static void LoadScenePrefabs()
    {
        Scene scene = GetScene();
        OGSettings settings = OGSettingsFunctions.GetSettings();

        //This loads the ship prefabs
        Object[] shipPrefabs = Resources.LoadAll(OGGetAddress.ships, typeof(GameObject));

        if (shipPrefabs != null)
        {
            scene.shipsPrefabPool = new GameObject[shipPrefabs.Length];
            scene.shipsPrefabPool = shipPrefabs;
        }

        Object[] cockpitPrefabs = Resources.LoadAll(OGGetAddress.cockpits, typeof(GameObject));

        if (cockpitPrefabs != null)
        {
            scene.cockpitPrefabPool = new GameObject[cockpitPrefabs.Length];
            scene.cockpitPrefabPool = cockpitPrefabs;
        }

        //This loads the environments
        Object[] propPrefabs = Resources.LoadAll(OGGetAddress.props, typeof(GameObject));

        if (propPrefabs != null)
        {
            scene.propPrefabPool = new GameObject[propPrefabs.Length];
            scene.propPrefabPool = propPrefabs;
        }

        //This loads the particle prefabs
        Object[] particlePrefabs = Resources.LoadAll(OGGetAddress.particles, typeof(GameObject));

        if (particlePrefabs != null)
        {
            scene.particlePrefabPool = new GameObject[particlePrefabs.Length];
            scene.particlePrefabPool = particlePrefabs;
        }

        //This loads the planet materials
        Object[] planetPrefabs = Resources.LoadAll(OGGetAddress.planets, typeof(GameObject));

        if (planetPrefabs != null)
        {
            scene.planetPrefabPool = new GameObject[planetPrefabs.Length];
            scene.planetPrefabPool = planetPrefabs;
        }

        Object[] terrainTextures = Resources.LoadAll(OGGetAddress.terrain, typeof(Texture2D));

        if (terrainTextures != null)
        {
            scene.terrainTexturesPool = new Texture2D[terrainTextures.Length];
            scene.terrainTexturesPool = terrainTextures;
        }

        scene.hyperspaceTunnelPrefab = Resources.Load(OGGetAddress.hyperspace + "HyperspaceTunnel") as GameObject;

        scene.skyboxes = Resources.LoadAll<Material>(OGGetAddress.skyboxes);
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

        Material starMaterial = Resources.Load(OGGetAddress.particles + "materials/star") as Material;
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
                    points[i].velocity = new Vector3(0, 0, 0);
                }

                particleSystem.SetParticles(points, points.Length);

                particleSystemRenderer.lengthScale = 1;

                particleSystemRenderer.renderMode = ParticleSystemRenderMode.Billboard;
            }

        }
    }

    //This grabs the starfield
    public static GameObject GetStarfield()
    {
        GameObject starfield = null;

        starfield = GameObject.Find("starfield");

        return starfield;
    }

    //This sets the starfield to invisible
    public static void SetStarfieldToInvisible(bool invisible)
    {
        GameObject starfield = GetStarfield();

        if (starfield != null)
        {
            if (invisible == true)
            {
                starfield.layer = LayerMask.NameToLayer("invisible");
            }
            else
            {
                starfield.layer = LayerMask.NameToLayer("starfield");
            }
        }
    }

    #endregion

    #region scene lighting

    //This changes the lighting in the scene
    public static void SetLighting(string colour, bool sunIsEnabled, float sunIntensity, float sunScale, float x, float y, float z, float xRot, float yRot, float zRot)
    {
        Scene scene = GetScene();

        if (scene.sceneLightGO == null)
        {
            scene.sceneLightGO = GameObject.Find("Directional Light");
        }

        if (scene.sceneLightGO != null)
        {
            if (scene.sceneLight == null)
            {
                scene.sceneLight = scene.sceneLightGO.GetComponent<Light>();
            }

            if (scene.lensFlare == null)
            {
                scene.lensFlare = scene.sceneLightGO.GetComponent<LensFlareComponentSRP>();
            }
        }

        //This sets the position and angle of the light
        if (scene.sceneLightGO != null)
        {
            Vector3 lightPosition = new Vector3(x, y, z);
            Quaternion lightRotation = Quaternion.Euler(xRot, yRot, zRot);

            scene.sceneLightGO.transform.position = lightPosition;
            scene.sceneLightGO.transform.rotation = lightRotation;
        }

        //This changes the colour temperature
        if (scene.sceneLight != null)
        {
            Color newColour;

            if (UnityEngine.ColorUtility.TryParseHtmlString(colour, out newColour))
            {
                //Do nothing
            }

            scene.sceneLight.color = newColour;
        }

        //This sets the sun on and off and changes its intensity
        if (scene.lensFlare != null)
        {
            scene.lensFlare.enabled = sunIsEnabled;
            scene.lensFlare.intensity = sunIntensity;
            scene.lensFlare.scale = sunScale;
        }
    }

    //This gets the lightning info 
    public static (string colour, bool sunIsEnabled, float sunIntensity, float sunScale, float x, float y, float z, float xRot, float yRot, float zRot) GetLightingData()
    {
        string colour = "#FFFFFF";
        bool sunIsEnabled = true;
        float sunIntensity = 1;
        float sunScale = 1;
        float x = 0;
        float y = 0;
        float z = 0;
        float xRot = 60;
        float yRot = 0;
        float zRot = 0;

        Scene scene = SceneFunctions.GetScene();

        if (scene.sceneLightGO != null)
        {
            x = scene.sceneLightGO.transform.position.x;
            y = scene.sceneLightGO.transform.position.y;
            z = scene.sceneLightGO.transform.position.z;

            xRot = scene.sceneLightGO.transform.rotation.eulerAngles.x;
            yRot = scene.sceneLightGO.transform.rotation.eulerAngles.y;
            zRot = scene.sceneLightGO.transform.rotation.eulerAngles.z;
        }

        //This changes the colour temperature
        if (scene.sceneLight != null)
        {
            colour = UnityEngine.ColorUtility.ToHtmlStringRGB(scene.sceneLight.color);
        }

        //This sets the sun on and off and changes its intensity
        if (scene.lensFlare != null)
        {
            sunIsEnabled = scene.lensFlare.enabled;
            sunIntensity = scene.lensFlare.intensity;
            scene.lensFlare.scale = sunScale;
        }

        return (colour, sunIsEnabled, sunIntensity, sunScale, x, y, z, xRot, yRot, zRot);
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
    public static void GeneratePlanet(string planetType, string ringsType, float distance, float planetXRot, float planetYRot, float planetZRot, float pivotXRot, float pivotYRot, float pivotZRot )
    {
        //This gets key references
        Scene scene = GetScene();

        GameObject planet = InstantiatePlanetPrefab(planetType);

        if (scene.planetsPool == null)
        {
            scene.planetsPool = new List<GameObject>();
        }

        scene.planetsPool.Add(planet);

        IgnoreCollisionWithPlanet();

        if (planet != null)
        {
            planet.transform.position = new Vector3(0, 0, 0);

            planet.layer = 27;

            GameObjectUtils.SetLayerAllChildren(planet.transform, 27);

            Transform[] planetTransforms = GameObjectUtils.GetAllChildTransformsIncludingInactive(planet.transform);
            
            //This turns the planet rings on and off
            foreach (Transform transform in planetTransforms)
            {
                if (transform.name == "rings")
                {
                    if (ringsType == "none")
                    {
                        transform.gameObject.SetActive(false);
                    }
                    else
                    {
                        transform.gameObject.SetActive(true);
                    }
                }

            }

            //This rotates and positions the planet
            float actualDistance = (0.6f / 100f) * distance;

            float x = 0.4f + actualDistance;
            float y = 0;
            float z = 0.4f + actualDistance;

            float xRot = planetXRot;
            float yRot = planetYRot;
            float zRot = planetZRot;

            foreach (Transform transform in planetTransforms)
            {
                if (transform.name == "planetpivot")
                {
                    transform.localPosition = new Vector3(x, y, z);
                    transform.rotation = Quaternion.Euler(xRot, yRot, zRot);
                }
            }

            //This rotates the whole system
            xRot = pivotXRot;
            yRot = pivotYRot;
            zRot = pivotZRot;

            planet.transform.rotation = Quaternion.Euler(xRot, yRot, zRot);
        }
    }

    //This instantiates a planet prefab
    public static GameObject InstantiatePlanetPrefab(string name)
    {
        Scene scene = SceneFunctions.GetScene();

        GameObject planetPrefab = null;
        GameObject tempPrefab = null;

        //This gets a backup prefab from another pool if the selected pool doesn't have the requested prefab
        foreach (GameObject objectPrefab in scene.planetPrefabPool)
        {
            if (objectPrefab.name == name)
            {
                tempPrefab = objectPrefab;
                break;
            }
        }

        //This instantiates the prefab
        if (tempPrefab != null)
        {
            planetPrefab = GameObject.Instantiate(tempPrefab) as GameObject;
        }

        return planetPrefab;
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
    public static IEnumerator LoadAsteroids(float number, string type, Vector3 position, float width, float height, float length, int seed, bool loadLarge)
    {
        Vector3[] positions = GetAsteroidPostions(number, height, width, length, seed);
        Vector3[] rotations = GetAsteroidRotations(number, seed);
        float[] sizes = GetAsteroidSizes(number, seed, loadLarge);
        int[] asteroidTypes = GetAsteroidTypes(number, type, seed);

        Scene scene = GetScene();

        GameObject asteroidAnchor = new GameObject();
        asteroidAnchor.name = "asteroidanchor";
        asteroidAnchor.transform.SetParent(scene.transform);
        asteroidAnchor.transform.localPosition = position;

        Vector3 centerPosition = new Vector3(width/2f, height/2f, length/2f);

        int generationCount = 0;

        for(int i =0; i < positions.Length; i++)
        {
            GameObject asteroid = GameObject.Instantiate((GameObject)scene.shipsPrefabPool[asteroidTypes[i]]);
            asteroid.transform.SetParent(asteroidAnchor.transform);
            asteroid.transform.localPosition = positions[i] - centerPosition;
            asteroid.transform.localScale = new Vector3(sizes[i], sizes[i], sizes[i]);
            asteroid.transform.rotation = Quaternion.Euler(rotations[i]);
            asteroid.name = "asteroid";
            asteroid.layer = LayerMask.NameToLayer("collision_asteroid");
            GameObjectUtils.SetLayerAllChildren(asteroid.transform, asteroid.layer);
            Rigidbody rigidBody = GameObjectUtils.AddRigidbody(asteroid, 100f, 0, 0);
            GameObjectUtils.AddMeshColliders(asteroid, true);

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

            generationCount++;

            if (generationCount > 100)
            {
                yield return null;
                generationCount = 0;
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

        foreach (Object asteroid in scene.shipsPrefabPool)
        {
            if (asteroid.name.Contains(type))
            {
                asteroidTypes.Add(asteroidNo);
            }

            asteroidNo++;
        }

        //This selects asteroids at random from the given type
        if (asteroidTypes.Count > 0)
        {
            int asteroidCount = asteroidTypes.Count - 1;

            for (int i = 0; i < number; i++)
            {
                int asteroid = Random.Range(0, asteroidCount);
                asteroidSelection.Add(asteroidTypes[asteroid]);
            }
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
    public static float[] GetAsteroidSizes(float number, int seed, bool loadLarge)
    {
        List<float> asteroidSizes = new List<float>();

        Random.InitState(seed);

        float maxSize = 0.5f;
        float minSize = 0.02f;
        float percentage = 3f;

        float limit = (maxSize / 10f);

        if (limit < minSize)
        {
            limit = minSize;
        }

        for (int i = 0; i < number; i++)
        {
            float randomNumber = Random.Range(0, 100);

            bool makeBigObject = false;

            if (randomNumber < percentage & loadLarge == true)
            {
                makeBigObject = true;
            }

            if (makeBigObject == true)
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

    #region skybox, fog, and lighting settings

    //This sets the skybox
    public static void SetSkybox(string name, bool stars)
    {
        //This gets the scene reference
        Scene scene = GetScene();

        //This toggles the starfield on and off
        GameObject starfield = GameObject.Find("starfield");

        if (starfield != null)
        {
            if (stars == false)
            {
                starfield.layer = LayerMask.NameToLayer("invisible");
            }
            else
            {
                starfield.layer = LayerMask.NameToLayer("starfield");
            }
        }

        //This sets the skybox
        if (scene != null)
        {
            if (scene.skyboxes != null)
            {
                foreach (Material skybox in scene.skyboxes)
                {
                    if (skybox.name == name)
                    {
                        RenderSettings.skybox = skybox;
                        break;
                    }
                }
            }
        }
    }

    //This set the fog distance and colour
    public static void SetFogDistanceAndColor(float fogStart, float fogEnd, string fogColor)
    {
        Color newColour;

        if (UnityEngine.ColorUtility.TryParseHtmlString(fogColor, out newColour))
        {
            //Do nothing
        }

        RenderSettings.fogStartDistance = fogStart;
        RenderSettings.fogEndDistance = fogEnd;
        RenderSettings.fogColor = newColour;
    }

    //This sets the radius of the scene i.e. the distance the ship can travel before being forcibly turned back
    public static void SetSceneRadius(float sceneRadius)
    {
        Scene scene = GetScene();

        if (scene != null)
        {
            scene.sceneRadius = sceneRadius;
        }
    }

    #endregion

    #region environment loading

    //This loads the environment
    public static void LoadEnvironment(string type, float x, float y, float z)
    {
        Scene scene = GetScene();

        foreach (GameObject environmentsPrefab in scene.propPrefabPool)
        {
            if (environmentsPrefab.name == type)
            {
                GameObject environment = GameObject.Instantiate(environmentsPrefab);

                if (scene != null)
                {
                    if (scene.environmentsPool == null)
                    {
                        scene.environmentsPool = new List<GameObject>();
                    }

                    environment.transform.SetParent(scene.transform);

                    environment.transform.localPosition = new Vector3(x, y, z);
                }

                break;
            }
        }
    }

    #endregion

    #region ship loading

    //This loads an individual ship in the scene
    public static void LoadSingleShip(Vector3 position, Quaternion rotation, string type, string name, string allegiance, string cargo, bool exitingHyperspace, bool isAI, bool dontModifyPosition, string laserColor, bool singleCall = false)
    {
        //Key reference
        GameObject ship = null;

        //Get scene script reference
        Scene scene = GetScene();
        Audio audioManager = AudioFunctions.GetAudioManager();

        //This gets the OG Settings
        OGSettings ogSettings = OGSettingsFunctions.GetSettings(); 

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
        if (shipType != null)
        {
            ship = InstantiateShipPrefab(shipType.prefab);

            if (cargo.ToLower().Contains("random") || cargo.ToLower().Contains("randomise"))
            {
                string[] cargoTypes = GetCargoTypesList();
                int cargoTypeNo = cargoTypes.Length;
                int randomChoice = Random.Range(0, cargoTypeNo - 1);
                cargo = cargoTypes[randomChoice];
            }
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
                smallShip.systemsRating = shipType.systemsRating;
                smallShip.systemsLevel = shipType.systemsRating;
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
                smallShip.shipLength = shipType.shipLength;
                smallShip.controllerSenstivity = ogSettings.controllersensitivity;
                smallShip.shieldType = shipType.shieldType;
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
                    smallShip.autoaim = settings.autoaim;

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
                largeShip.systemsRating = shipType.systemsRating;
                largeShip.systemsLevel = shipType.systemsRating;
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
                largeShip.shipLength = shipType.shipLength;
                largeShip.shieldType = shipType.shieldType;
                ship.name = largeShip.name;

                //THE SCRIPT NEEDS TO VERIFY THAT THE TURRETS ARE PRESENT BEFORE ADDING THE SCRIPT
                LaserTurret laserTurret = ship.AddComponent<LaserTurret>();
                laserTurret.largeTurretDamage = shipType.largeturret;
                laserTurret.smallTurretDamage = shipType.smallturret;

                PlasmaTurret plasmaTurret = ship.AddComponent<PlasmaTurret>();
                plasmaTurret.largeTurretDamage = shipType.largeturret;
                plasmaTurret.smallTurretDamage = shipType.smallturret;
            }

            //Set ship position, rotation and scale

            ScaleGameObjectByZAxis(ship, shipType.shipLength); //The scale must be applied before the ship is position and rotated otherwise the scaling will be inaccurate

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
                    LargeShipFunctions.AddTaskToPool(largeShip, a);
                }
                else if (shipType.scriptType == "smallship")
                {
                    SmallShip smallShip = ship.GetComponent<SmallShip>();
                    Task a = new Task(SmallShipFunctions.ExitHyperspace(smallShip));
                    SmallShipFunctions.AddTaskToPool(smallShip, a);
                }
            }

        }

        if (singleCall == true & exitingHyperspace != true)
        {
            HudFunctions.AddToShipLog(type + " just entered the area");
        }
    }

    //This instanties the designated prefabe from the pool listed in the settings or finds a substitute from another pool if the prefab is not present
    public static GameObject InstantiateShipPrefab(string name)
    {
        Scene scene = SceneFunctions.GetScene();

        GameObject shipPrefab = null;
        GameObject tempPrefab = null;

        //This gets a backup prefab from another pool if the selected pool doesn't have the requested prefab
        foreach (GameObject objectPrefab in scene.shipsPrefabPool)
        {
            if (objectPrefab.name == name)
            {
                tempPrefab = objectPrefab;
                break;
            }
        }      

        //This instantiates the prefab
        if (tempPrefab != null)
        {
            shipPrefab = GameObject.Instantiate(tempPrefab) as GameObject;
        }

        return shipPrefab;
    }

    //A wrapper function for load single ships that loads multiple ships
    public static IEnumerator LoadMultipleShips(Vector3 position, Quaternion rotation, string type, string name, string allegiance, string cargo, int number, string pattern, float width, float length, float height, int shipsPerLine, float positionVariance,bool exitingHyperspace,bool includePlayer, int playerNo,string laserColor)
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

        if (exitingHyperspace == false)
        {
            HudFunctions.AddToShipLog(number + " " + type.ToUpper() + "S just entered the area");
        }
    }

    //A wrapper functions for load single ships that loads a single ship on the ground
    public static void LoadSingleShipOnGround(Vector3 position, Quaternion rotation, string type, string name, string allegiance, string cargo, bool isAI, float distanceAboveGround, float positionVariance, bool ifRaycastFailsStillLoad, string laserColor)
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

        Vector3 relativePosition = scene.transform.TransformPoint(position);

        Vector3 raycastPos = new Vector3(relativePosition.x, 15000, relativePosition.z);

        RaycastHit hit;

        LayerMask mask = LayerMask.GetMask("collision_asteroid");

        Debug.DrawRay(raycastPos, Vector3.down * 30000, Color.red, 500);

        //NOTE: Raycasts only work when the time scale is set to zero IF "Auto Sync Transform" is set to true in the project settings

        if (Physics.Raycast(raycastPos, Vector3.down, out hit, 30000, mask))
        {
            Vector3 newPosition = hit.point + new Vector3(0, distanceAboveGround, 0);
            LoadSingleShip(newPosition, rotation, type, name, allegiance, cargo, false, isAI, true, laserColor);
        }
        else if (ifRaycastFailsStillLoad == true)
        {
            Vector3 newPosition = new Vector3(relativePosition.x, 0, relativePosition.z);
            LoadSingleShip(newPosition, rotation, type, name, allegiance, cargo, false, isAI, true, laserColor);
        }

        HudFunctions.AddToShipLog(type.ToUpper() + " just entered the area");
    }

    //A wrapper functions for load single ships that loads multiple ships on the ground
    public static IEnumerator LoadMultipleShipsOnGround(Vector3 position, Quaternion rotation, string type, string name, string allegiance, string cargo, int number, float length, float width, float distanceAboveGround, int shipsPerLine, float positionVariance, bool ifRaycastFailsStillLoad, string laserColor)
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

        int shipNo = 1;

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
                LoadSingleShip(newPosition, rotation, type, name + shipNo.ToString("00"), allegiance, cargo, false, true, true, laserColor);
                shipNo++;
            }
            else if (ifRaycastFailsStillLoad == true)
            {
                Vector3 newPosition = new Vector3(relativePosition.x, 0, relativePosition.z);
                LoadSingleShip(newPosition, rotation, type, name + shipNo.ToString("00"), allegiance, cargo, false, true, true, laserColor);
                shipNo++;
            }

            yield return null;
        }

        HudFunctions.AddToShipLog(number + " " + type.ToUpper() + "S just entered the area");
    }

    //A wrapper functions for load single ship that loads multiple ships from another ships hangar
    public static IEnumerator LoadMultipleShipsFromHangar(string type, string name, string allegiance, string cargo, int number, string launchship, int hangarNo, float delay, string laserColor)
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
            hangars = GameObjectUtils.FindAllChildTransformsContaining(launchShipGO.transform, "hangarlaunch");

            if (hangars.Length < 1)
            {
                hangars = GameObjectUtils.FindAllChildTransformsContaining(launchShipGO.transform, "hangarLaunch");
            }
        }

        if (hangars == null || hangars.Length < 1)
        {
            LargeShip largeShip = launchShipGO.GetComponent<LargeShip>();
            SmallShip smallShip = launchShipGO.GetComponent<SmallShip>();

            GameObject hangarLaunchGO = new GameObject();
            hangarLaunchGO.name = "hangarlaunch";
            hangarLaunchGO.transform.SetParent(launchShipGO.transform);

            float shipLength = 0;

            if (largeShip != null)
            {
                shipLength = largeShip.shipLength;
            }

            if (smallShip != null)
            {
                shipLength = smallShip.shipLength;
            }

            hangarLaunchGO.transform.localPosition = new Vector3(0, -shipLength, 0);

            hangars = new Transform[]{hangarLaunchGO.transform};
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
            HudFunctions.AddToShipLog(number + " " + type.ToUpper() + "S are exiting the " + launchship.ToUpper() + "'S hangar");

            for (int i = 0; i < number; i++)
            {
                if (hangarLaunch != null)
                {
                    int shipNo = i + 1;

                    LoadSingleShip(hangarLaunch.position, hangarLaunch.rotation, type, name + shipNo.ToString("00"), allegiance, cargo, false, true, true, laserColor);

                    yield return new WaitForSeconds(delay);
                }
            }
        }
    }

    //A wrapper functions for load single ship that loads multiple ships from another ships hangar
    public static void LoadSingleShipFromHangar(string type, string name, string allegiance, string cargo, string launchship, int hangarNo, string laserColor)
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
            hangars = GameObjectUtils.FindAllChildTransformsContaining(launchShipGO.transform, "hangarlaunch");

            if (hangars.Length < 1)
            {
                hangars = GameObjectUtils.FindAllChildTransformsContaining(launchShipGO.transform, "hangarLaunch");
            }
        }

        if (hangars == null || hangars.Length < 1)
        {
            LargeShip largeShip = launchShipGO.GetComponent<LargeShip>();
            SmallShip smallShip = launchShipGO.GetComponent<SmallShip>();

            GameObject hangarLaunchGO = new GameObject();
            hangarLaunchGO.name = "hangarlaunch";
            hangarLaunchGO.transform.SetParent(launchShipGO.transform);

            float shipLength = 0;

            if (largeShip != null)
            {
                shipLength = largeShip.shipLength;
            }

            if (smallShip != null)
            {
                shipLength = smallShip.shipLength;
            }

            hangarLaunchGO.transform.localPosition = new Vector3(0, -shipLength, 0);

            hangars = new Transform[] { hangarLaunchGO.transform };
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
            HudFunctions.AddToShipLog(type.ToUpper() + "is exiting the " + launchship.ToUpper() + "'S hangar");

            LoadSingleShip(hangarLaunch.position, hangarLaunch.rotation, type, name, allegiance, cargo, false, true, true, laserColor);
        }
    }

    //This loads a single ship as a wreck
    public static void LoadSingleShipAsWreck(Vector3 position, Quaternion rotation, string type, string name, int fireNumber, float fileScaleMin, float fireScaleMax, int seed)
    {
        //Key reference
        GameObject ship = null;

        //Get scene script reference
        Scene scene = GetScene();

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
        if (shipType != null)
        {
            ship = InstantiateShipPrefab(shipType.prefab);
            ship.name = shipType.callsign + "_wreck";
        }

        if (ship != null)
        {
            //Set ship position, rotation and scale
            ScaleGameObjectByZAxis(ship, shipType.shipLength); //The scale must be applied before the ship is position and rotated otherwise the scaling will be inaccurate
            ship.transform.position = scene.transform.position + position;
            ship.transform.rotation = rotation;

            //parent it to the scene
            ship.transform.SetParent(scene.transform);

            //This sets the layer of the ship
            ship.layer = LayerMask.NameToLayer("collision_asteroid");

            GameObjectUtils.SetLayerAllChildren(ship.transform, LayerMask.NameToLayer("collision_asteroid"));

            //Attach mesh collider for ship
            if (shipType.scriptType == "largeship")
            {
                GameObjectUtils.AddMeshColliders(ship.gameObject, false);
            }
            else if (shipType.scriptType == "smallship")
            {
                GameObjectUtils.AddMeshColliders(ship.gameObject, true);
            }

            //Attach smoke particle systems
            if (fireNumber > 0)
            {
                LoadFires(ship, fireNumber, shipType.shipLength, fileScaleMin, fireScaleMax, seed);
            }

            //Add ship to a pool
            scene.asteroidPool.Add(ship);
        }
    }

    //This loads the 'fires' on the wreck
    public static void LoadFires(GameObject ship, int number, float shipLength, float scaleMin, float scaleMax, int seed)
    {
        Random.InitState(seed);

        var largest = GameObjectUtils.FindLargestMeshByLength(ship);

        MeshFilter largestMeshFilter = null;
        SkinnedMeshRenderer largestSkinnedMeshRenderer = null;
        Mesh mainShipMesh = null;
        Transform meshTransform = null;

        if (largest is MeshFilter meshFilter)
        {
            largestMeshFilter = largest as MeshFilter;

            mainShipMesh = largestMeshFilter.sharedMesh;
            meshTransform = largestMeshFilter.transform;
        }
        else if (largest is SkinnedMeshRenderer skinnedMeshRenderer)
        {
            largestSkinnedMeshRenderer = largest as SkinnedMeshRenderer;

            mainShipMesh = largestSkinnedMeshRenderer.sharedMesh;
            meshTransform = largestSkinnedMeshRenderer.transform;
        }

        int explosionsNumber = number;

        List<Vector3> explosionPoints = GameObjectUtils.GetRandomPointsOnMesh(mainShipMesh, meshTransform, explosionsNumber);
        List<ParticleSystem> explosions = new List<ParticleSystem>();

        foreach (Vector3 explosionPoint in explosionPoints)
        {
            if (explosionPoint != null)
            {
                Vector3 worldPoint = meshTransform.TransformPoint(explosionPoint);

                float scale = Random.Range(scaleMin, scaleMax);

                ParticleFunctions.InstantiatePersistantExplosion(ship, worldPoint, "explosion_wreck", scale);
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

    public static Vector3[] Pattern_RectangleHorizontal(Vector3 position, float width, float length, int shipNumber, int shipsPerLine, float positionVariance)
    {
        List<Vector3> shipPositions = new List<Vector3>();

        // Calculate spacing between points
        float xSpacing = width / (shipsPerLine - 1);
        float zSpacing = length / Mathf.Ceil((float)shipNumber / shipsPerLine);

        int pointsCreated = 0;

        // Generate points
        for (int i = 0; pointsCreated < shipNumber; i++) // Row iteration
        {
            for (int j = 0; j < shipsPerLine && pointsCreated < shipNumber; j++) // Column iteration
            {
                float varianceX = Random.Range(0, positionVariance);
                float varianceY = Random.Range(0, positionVariance);
                float varianceZ = Random.Range(0, positionVariance);

                float x = j * xSpacing - width / 2; // Offset by width/2 to center the grid
                float z = i * zSpacing - length / 2; // Offset by length/2 to center the grid

                Vector3 point = new Vector3(x + varianceX, 0 + varianceY, z + varianceZ) + position; // Shift by the center point
                shipPositions.Add(point);
                pointsCreated++;
            }
        }

        return shipPositions.ToArray();
    }

    public static Vector3[] Pattern_RectangleVertical(Vector3 position, float width, float length, int shipNumber, int shipsPerLine, float positionVariance)
    {
        List<Vector3> shipPositions = new List<Vector3>();

        // Calculate spacing between points
        float ySpacing = width / (shipsPerLine - 1);
        float zSpacing = length / Mathf.Ceil((float)shipNumber / shipsPerLine);

        int pointsCreated = 0;

        // Generate points
        for (int i = 0; pointsCreated < shipNumber; i++) // Row iteration
        {
            for (int j = 0; j < shipsPerLine && pointsCreated < shipNumber; j++) // Column iteration
            {
                float varianceX = Random.Range(0, positionVariance);
                float varianceY = Random.Range(0, positionVariance);
                float varianceZ = Random.Range(0, positionVariance);

                float y = j * ySpacing - width / 2; // Offset by width/2 to center the grid
                float z = i * zSpacing - length / 2; // Offset by length/2 to center the grid

                Vector3 point = new Vector3(position.x + varianceX, y + varianceY, z + varianceZ) + position; // Adjust Y and Z, keep X constant
                shipPositions.Add(point);
                pointsCreated++;
            }
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

    #region prop loading

    //This instantiates a single prop at the given point
    public static void LoadSinglePropOnGround(Vector3 position, Quaternion rotation, string type, bool ifRaycastFailsStillLoad = true)
    {
        //This gets the scene reference
        Scene scene = GetScene();

        //This finds the ship type to load 
        Object propPrefab = null;

        foreach (Object tempPropPrefab in scene.propPrefabPool)
        {
            if (tempPropPrefab.name == type)
            {
                propPrefab = tempPropPrefab;
                break;
            }
        }

        Vector3 relativePosition = scene.transform.TransformPoint(position);

        Vector3 raycastPos = new Vector3(relativePosition.x, 15000, relativePosition.z);

        RaycastHit hit;

        LayerMask mask = LayerMask.GetMask("collision_asteroid");

        if (Physics.Raycast(raycastPos, Vector3.down, out hit, 30000, mask))
        {
            Vector3 newPosition = hit.point;
            GameObject newObject = GameObject.Instantiate(propPrefab, scene.transform) as GameObject;
            newObject.transform.position = newPosition;
        }
        else if (ifRaycastFailsStillLoad == true)
        {
            Vector3 newPosition = new Vector3(relativePosition.x, 0, relativePosition.z);
            GameObject newObject = GameObject.Instantiate(propPrefab, scene.transform) as GameObject;
            newObject.transform.position = newPosition;
            Debug.Log("Raycast for prop " + propPrefab.name + " was unsuccesful instantiating at 0 on the y-axis");
        }
    }

    //This instantiates a group of props on the ground
    public static IEnumerator LoadMultiplePropsOnGround(Vector3 position, Quaternion rotation, string type1, string type2, string type3, string type4, string type5, string pattern, float width, float length, int number, float separation = 10, int seed = 1, bool ifRaycastFailsStillLoad = true)
    {
        //This gets the scene reference
        Scene scene = GetScene();

        //This sets the seed to ensure consistent results
        Random.InitState(seed);

        //This finds the ship type to load 
        List<Object> propPrefabs = new List<Object>();

        foreach (Object tempProp in scene.propPrefabPool)
        {
            if (tempProp.name == type1 || tempProp.name == type2 || tempProp.name == type3 || tempProp.name == type4 || tempProp.name == type5)
            {
                propPrefabs.Add(tempProp);
            }
        }

        Vector3[] positions = GetPropPositions(pattern, position, width, length, number, separation);

        if (propPrefabs.Count > 0 & positions.Length > 0)
        {
            int generationCount = 0;

            foreach (Vector3 tempPosition in positions)
            {
                Vector3 relativePosition = scene.transform.TransformPoint(tempPosition);

                Vector3 raycastPos = new Vector3(relativePosition.x, 15000, relativePosition.z);

                RaycastHit hit;

                LayerMask mask = LayerMask.GetMask("collision_asteroid");

                int selection = Random.Range(0, propPrefabs.Count);

                if (Physics.Raycast(raycastPos, Vector3.down, out hit, 30000, mask))
                {
                    Vector3 newPosition = hit.point;
                    GameObject newObject = GameObject.Instantiate(propPrefabs[selection], scene.transform) as GameObject;
                    newObject.transform.position = newPosition;
                }
                else if (ifRaycastFailsStillLoad == true)
                {
                    Vector3 newPosition = new Vector3(relativePosition.x, 0, relativePosition.z);
                    GameObject newObject = GameObject.Instantiate(propPrefabs[selection], scene.transform) as GameObject;
                    newObject.transform.position = newPosition;
                    Debug.Log("Raycast for prop " + propPrefabs[selection].name + " was unsuccesful instantiating at 0 on the y-axis");
                }

                generationCount++;

                if (generationCount > 100)
                {
                    yield return null;
                    generationCount = 0;
                }
            }
        }
    }

    public static Vector3[] GetPropPositions(string pattern, Vector3 position, float width, float length, int number, float separation = 10)
    {
        if (number < 1)
        {
            number = 1;
        }

        Vector3[] propPositions = null;

        if (pattern == "treepositions")
        {
            propPositions = GenerateTreePositions(length, width, position, number);
        }
        else if (pattern == "buildingpositions")
        {
            propPositions = GenerateBuildingPositions(length, width, position, number, separation);
        }

        return propPositions;
    }

    public static Vector3[] GenerateTreePositions(float length, float width, Vector3 position, int number)
    {
        Vector3[] positions = new Vector3[number];

        float halfLength = length / 2f;
        float halfWidth = width / 2f;

        for (int i = 0; i < number; i++)
        {
            // Generate random X and Z coordinates within the defined bounds
            float randomX = Random.Range(position.x - halfLength, position.x + halfLength);
            float randomZ = Random.Range(position.z - halfWidth, position.z + halfWidth);

            // Use the Y-coordinate from the centerPosition, assuming it's the desired ground level.
            // You might want to adjust this based on actual terrain later (e.g., using Physics.Raycast for height).
            positions[i] = new Vector3(randomX, position.y, randomZ);
        }

        return positions;
    }

    public static Vector3[] GenerateBuildingPositions(float length, float width, Vector3 position, int number, float minBuildingSeparation)
    {
        if (number <= 0)
        {
            Debug.LogWarning("Number of buildings must be greater than 0.");
            return new Vector3[0]; // Return an empty array
        }

        if (minBuildingSeparation < 0)
        {
            Debug.LogWarning("Minimum building separation cannot be negative. Setting to 0.");
            minBuildingSeparation = 0;
        }

        List<Vector3> buildingPositions = new List<Vector3>();

        float halfLength = length / 2f;
        float halfWidth = width / 2f;

        int maxAttemptsPerBuilding = 50; // Max tries to find a spot for a single building
        int currentBuildingsGenerated = 0;

        for (int i = 0; i < number; i++)
        {
            int attempts = 0;
            bool positionFound = false;

            while (attempts < maxAttemptsPerBuilding && !positionFound)
            {
                float randomX = Random.Range(position.x - halfLength, position.x + halfLength);
                float randomZ = Random.Range(position.z - halfWidth, position.z + halfWidth);

                // Use the Y-coordinate from the centerPosition, assuming it's the desired ground level.
                // Similar to trees, you might want to adjust this based on actual terrain later (e.g., using Physics.Raycast).
                Vector3 newBuildingPos = new Vector3(randomX, position.y, randomZ);

                bool tooClose = false;
                foreach (Vector3 existingPos in buildingPositions)
                {
                    if (Vector3.Distance(newBuildingPos, existingPos) < minBuildingSeparation)
                    {
                        tooClose = true;
                        break;
                    }
                }

                if (!tooClose)
                {
                    buildingPositions.Add(newBuildingPos);
                    currentBuildingsGenerated++;
                    positionFound = true;
                }
                attempts++;
            }

            if (!positionFound)
            {
                // If we couldn't find a spot after maxAttemptsPerBuilding, we'll just stop trying for this building.
                // This prevents infinite loops if the area is too small or separation too large.
                Debug.LogWarning($"Could not find a suitable position for building {i + 1} after {maxAttemptsPerBuilding} attempts. Consider increasing city area or decreasing number of buildings/separation.");
            }
        }

        return buildingPositions.ToArray();
    }

    #endregion

    #region cockpit loading

    //This activates the designated cockpit
    public static GameObject ActivateCockpit(string name)
    {
        GameObject cockpit = null;

        //This gets the scene reference
        Scene scene = GetScene();

        //This gets the cockpit anchor
        GameObject cockpitAnchor = GameObject.Find("cockpitanchor");

        if (cockpitAnchor == null)
        {
            cockpitAnchor = new GameObject();
            cockpitAnchor.name = "cockpitanchor";
        }

        //This checks whether the requested cockpit exists or not
        bool cockpitExists = false;

        foreach (GameObject objectPrefab in scene.cockpitPrefabPool)
        {
            if (objectPrefab.name == name)
            {
                cockpitExists = true;
                break;
            }
        }

        if (cockpitExists == true)
        {
            //This sets up the cockpit pool if needed
            if (scene.cockpitsPool == null)
            {
                scene.cockpitsPool = new List<GameObject>();
            }

            //This sets the cockpit to active if it exists in the list
            bool cockpitLoaded = false;

            foreach (GameObject tempCockpit in scene.cockpitsPool)
            {
                if (tempCockpit.name == name)
                {
                    tempCockpit.SetActive(false);
                    cockpit = tempCockpit;
                    cockpitLoaded = true;
                    break;
                }
            }

            //This loads the cockpit if it hasn't been loaded
            if (cockpitLoaded == false)
            {
                foreach (GameObject objectPrefab in scene.cockpitPrefabPool)
                {
                    if (objectPrefab.name == name)
                    {
                        cockpit = GameObject.Instantiate(objectPrefab) as GameObject;
                        cockpit.transform.position = new Vector3(0, 0, 0);
                        cockpit.transform.parent = cockpitAnchor.transform;
                        cockpit.transform.localRotation = Quaternion.identity;
                        cockpit.layer = LayerMask.NameToLayer("cockpit");
                        GameObjectUtils.SetLayerAllChildren(cockpit.transform, 28);
                        scene.cockpitsPool.Add(cockpit);
                        cockpit.SetActive(true);
                    }
                }
            }
        }

        return cockpit;
    }

    //This deactivates all cockpits
    public static void DeactivateCockpits()
    {
        //This gets the scene reference
        Scene scene = GetScene();

        if (scene.cockpitsPool != null)
        {
            foreach (GameObject tempCockpit in scene.cockpitsPool)
            {
                if (tempCockpit != null)
                {
                    tempCockpit.SetActive(false);
                }
            }
        }
    }

    #endregion

    #region floating point control and camera rotation

    //This recenter the scene so the player is always close to the center to prevent floating point inaccuraries 
    public static void RecenterScene(Scene scene)
    {      
        if (scene.ogCamera == null)
        {
            scene.ogCamera = OGCameraFunctions.GetOGCamera();
        }

        if (scene.ogCamera != null)
        {
            OGCamera oGCamera = scene.ogCamera;

            if (oGCamera.mainCamera != null)
            {
                Vector3 cameraPosition = oGCamera.mainCamera.transform.position; //This checks the ships position

                if (cameraPosition.x > 1000 || cameraPosition.x < -1000 || cameraPosition.y > 1000 || cameraPosition.y < -1000 || cameraPosition.z > 1000 || cameraPosition.z < -1000)
                {
                    GameObject tempGO = new GameObject();

                    tempGO.transform.position = oGCamera.mainCamera.transform.position;

                    scene.gameObject.transform.SetParent(tempGO.transform); //This parents the scene anchor to the ship

                    tempGO.transform.position = new Vector3(0, 0, 0); //This moves the ship back to 0,0,0

                    scene.gameObject.transform.SetParent(null); //This unparents the scene anchor from the ship

                    GameObject.Destroy(tempGO);
                }
            }
        }
    }

    //This rotates the starfield camera to follow the players rotation
    public static void RotateStarfieldAndPlanetCamera(Scene scene)
    {
        if (scene.ogCamera == null)
        {
            scene.ogCamera = OGCameraFunctions.GetOGCamera();
        }

        if (scene.ogCamera != null)
        {
            OGCamera ogCamera = scene.ogCamera;

            if (ogCamera.starfieldCamera != null & ogCamera.planetCamera != null)
            {
                ogCamera.starfieldCamera.transform.rotation = ogCamera.mainCamera.transform.rotation;
                ogCamera.planetCamera.transform.rotation = ogCamera.mainCamera.transform.rotation;
            }
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
            ClearScene(scene);
            GameObject.Destroy(scene);
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

        GameObject FollowCamera = GameObject.Find("Follow Camera");

        if (FollowCamera != null)
        {
            FollowCamera.transform.parent = null;
        }

        GameObject FilmCamera = GameObject.Find("Film Camera");

        if (FilmCamera != null)
        {
            FilmCamera.transform.parent = null;
        }

        GameObject CockpitCamera = GameObject.Find("Cockpit Camera");

        if (CockpitCamera != null)
        {
            CockpitCamera.transform.parent = null;
        }

        if (scene.objectPool != null)
        {
            foreach (GameObject gameobject in scene.objectPool)
            {
                if (gameobject != null)
                {
                    GameObject.Destroy(gameobject);
                }
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

        if (scene.environmentsPool != null)
        {
            foreach (GameObject gameobject in scene.environmentsPool)
            {
                GameObject.Destroy(gameobject);
            }

            scene.environmentsPool.Clear();
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

        if (scene.ionPool != null)
        {
            foreach (GameObject gameobject in scene.ionPool)
            {
                GameObject.Destroy(gameobject);
            }

            scene.ionPool.Clear();
        }

        if (scene.plasmaPool != null)
        {
            foreach (GameObject gameobject in scene.plasmaPool)
            {
                GameObject.Destroy(gameobject);
            }

            scene.plasmaPool.Clear();
        }

        if (scene.torpedosPool != null)
        {
            foreach (GameObject gameobject in scene.torpedosPool)
            {
                GameObject.Destroy(gameobject);
            }

            scene.torpedosPool.Clear();
        }

        if (scene.planetsPool != null)
        {
            foreach (GameObject gameobject in scene.planetsPool)
            {
                GameObject.Destroy(gameobject);
            }

            scene.planetsPool.Clear();
        }

        GameObject CockpitAnchor = GameObject.Find("Cockpit Anchor");

        if (CockpitAnchor != null)
        {
            GameObject.Destroy(CockpitAnchor);
        }
    }

    //This only clears the scene but leaves the scene loaded and the player intact
    public static void ClearLocation()
    {
        Scene scene = GetScene();

        if (scene.objectPool != null)
        {
            foreach (GameObject gameobject in scene.objectPool)
            {
                if (gameobject != null)
                {
                    if (scene.mainShip != gameobject)
                    {
                        GameObject.Destroy(gameobject);
                    }
                }
            }

            scene.objectPool.Clear();

            scene.objectPool.Add(scene.mainShip);
        }

        if (scene.environmentsPool != null)
        {
            foreach (GameObject gameobject in scene.environmentsPool)
            {
                GameObject.Destroy(gameobject);
            }

            scene.environmentsPool.Clear();
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

        if (scene.ionPool != null)
        {
            foreach (GameObject gameobject in scene.ionPool)
            {
                GameObject.Destroy(gameobject);
            }

            scene.ionPool.Clear();
        }

        if (scene.plasmaPool != null)
        {
            foreach (GameObject gameobject in scene.plasmaPool)
            {
                GameObject.Destroy(gameobject);
            }

            scene.plasmaPool.Clear();
        }

        if (scene.asteroidPool != null)
        {
            foreach (GameObject gameobject in scene.asteroidPool)
            {
                GameObject.Destroy(gameobject);
            }

            scene.asteroidPool.Clear();
        }

        if (scene.planetsPool != null)
        {
            foreach (GameObject gameobject in scene.planetsPool)
            {
                GameObject.Destroy(gameobject);
            }

            scene.planetsPool.Clear();
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

        scene = GameObject.FindFirstObjectByType<Scene>(FindObjectsInactive.Include);//This gets the scene reference

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

    //This scales the ship to the gameobject / ship to the size given in meteres 
    public static void ScaleGameObjectByZAxis(GameObject gameObject, float targetZLengthInMeters)
    {
        if (gameObject == null)
        {
            Debug.LogError("GameObject is null. Please provide a valid GameObject.");
            return;
        }

        if (targetZLengthInMeters <= 0)
        {
            targetZLengthInMeters = 1;
            Debug.Log("Target Z-axis length was less than zero. Length will be set to 1.");
            return;
        }

        // Reset scale to 1 before calculating bounds
        Vector3 originalScale = gameObject.transform.localScale;
        gameObject.transform.localScale = Vector3.one;

        // Calculate the bounds of the GameObject
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
        {
            Debug.LogError("GameObject does not have a Renderer component. Cannot calculate bounds.");
            gameObject.transform.localScale = originalScale; // Restore original scale
            return;
        }

        Bounds gameObjectBounds = new Bounds(renderers[0].bounds.center, Vector3.zero);

        foreach (Renderer renderer in renderers)
        {
            gameObjectBounds.Encapsulate(renderer.bounds);
        }

        // Get the current size along the Z-axis
        float currentZLength = gameObjectBounds.size.z;

        // Calculate the uniform scale factor based on the Z-axis
        float scaleFactor = targetZLengthInMeters / currentZLength;

        // Apply the scale factor proportionally to all axes
        gameObject.transform.localScale = originalScale * scaleFactor;

        //Bounds newGameObjectBounds = new Bounds(renderers[0].bounds.center, Vector3.zero);

        //foreach (Renderer renderer in renderers)
        //{
        //    newGameObjectBounds.Encapsulate(renderer.bounds);
        //}

        //Debug.Log($"GameObject '{gameObject.name}' scaled to fit Z-axis length {targetZLengthInMeters} meters. But actual length is " + newGameObjectBounds.size.z + " from a size of " + currentZLength);
    }

    //This scales the ship to the gameobject / ship to the size given in meteres 
    public static void ScaleGameObjectByXYZAxis(GameObject gameObject, float sizeLimit)
    {
        if (gameObject == null)
        {
            Debug.LogError("GameObject is null. Please provide a valid GameObject.");
            return;
        }

        if (sizeLimit <= 0)
        {
            sizeLimit = 1;
            Debug.Log("Target Z-axis length was less than zero. Length will be set to 1.");
            return;
        }

        // Reset scale to 1 before calculating bounds
        Vector3 originalScale = gameObject.transform.localScale;
        gameObject.transform.localScale = Vector3.one;

        // Calculate the bounds of the GameObject
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
        {
            Debug.LogError("GameObject does not have a Renderer component. Cannot calculate bounds.");
            gameObject.transform.localScale = originalScale; // Restore original scale
            return;
        }

        Bounds gameObjectBounds = new Bounds(renderers[0].bounds.center, Vector3.zero);

        foreach (Renderer renderer in renderers)
        {
            gameObjectBounds.Encapsulate(renderer.bounds);
        }

        // Get the current size along all axis
        float yLength = gameObjectBounds.size.y;
        float xLength = gameObjectBounds.size.x;
        float zLength = gameObjectBounds.size.z;

        //This checks for the longest length and then resizes the object by that length
        if (xLength > yLength & xLength > zLength)
        {
            // Calculate the uniform scale factor based on the Z-axis
            float scaleFactor = sizeLimit / xLength;

            // Apply the scale factor proportionally to all axes
            gameObject.transform.localScale = originalScale * scaleFactor;
        }
        else if (yLength > xLength & yLength > zLength)
        {
            // Calculate the uniform scale factor based on the Z-axis
            float scaleFactor = sizeLimit / yLength;

            // Apply the scale factor proportionally to all axes
            gameObject.transform.localScale = originalScale * scaleFactor;
        }
        else
        {
            // Calculate the uniform scale factor based on the Z-axis
            float scaleFactor = sizeLimit / zLength;

            // Apply the scale factor proportionally to all axes
            gameObject.transform.localScale = originalScale * scaleFactor;
        } 
    }


    #endregion
}
