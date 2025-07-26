using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public static class TerrainManagerFunctions
{
    //This starts the terrain generation
    public static IEnumerator CommenceTerrainGeneration(TerrainManager terrainManager)
    {
        //This gets the scene reference for the Tile Manager
        terrainManager.scene = SceneFunctions.GetScene();

        //This creates the plane under the terrain
        CreateDistantPlane();

        //This sets the terrain type for the Tile Manager
        SetTerrainNoiseSettings(terrainManager);

        //This queues the base tiles to generate
        QueueInitialTiles(terrainManager);

        //This sets the seed
        Random.InitState(terrainManager.seed);

        //This generates the terrain materials
        GenerateTerrainMaterials(terrainManager);
        GenerateSeaMaterial(terrainManager);

        //This generates the base tiles
        Task a = new Task(GenerateTiles(terrainManager));
        terrainManager.terrainTasks.Add(a); //This adds the task to the list

        while (a.Running == true)
        {
            yield return null;
        }

        //This marks the initial terrain generation as complete and communicates to the tile manager that it can commence generating new tiles as necessary
        terrainManager.initialGenerationComplete = true;
    }

    //This generates the sea material
    public static void GenerateSeaMaterial(TerrainManager terrainManager)
    {
        //4. Apply a new material
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");

        Material seaMaterial = new Material(shader);

        //5.Apply texture to material
        Texture2D selectedTexture = null;

        foreach (Object texture in terrainManager.scene.terrainTexturesPool)
        {
            if (texture.name == terrainManager.seaTextureType)
            {
                selectedTexture = (Texture2D)texture;
                break;
            }
        }

        if (seaMaterial != null)
        {
            seaMaterial.SetFloat("_Metallic", 0f);
            seaMaterial.SetTexture("_BaseMap", selectedTexture);

            // Set Surface Type to Transparent
            seaMaterial.SetFloat("_Surface", 1f);

            // Set Blending Mode to Alpha Blending (Fade)
            seaMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            seaMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            seaMaterial.SetInt("_ZWrite", 0); // Disable depth writing

            // Enable/Disable relevant keywords
            seaMaterial.DisableKeyword("_ALPHATEST_ON");
            seaMaterial.EnableKeyword("_ALPHABLEND_ON");
            seaMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");

            Color currentColor = seaMaterial.color; // Get the current base color
            currentColor.a = 0.9f;          // Set the alpha component
            seaMaterial.color = currentColor;

            // Set Render Queue for transparent objects
            seaMaterial.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        }

        //6. Apply normal to material
        Texture2D normalTexture = null;

        foreach (Object texture in terrainManager.scene.terrainTexturesPool)
        {
            if (texture.name == "sea_normal")
            {
                normalTexture = (Texture2D)texture;
                break;
            }
        }

        if (seaMaterial != null)
        {
            seaMaterial.SetFloat("_BumpScale", 3.0f);
            seaMaterial.SetTexture("_BumpMap", normalTexture);
        }

        terrainManager.seaMaterial = seaMaterial;
    }

    //This generates the chosen terrain materials
    public static void GenerateTerrainMaterials(TerrainManager terrainManager)
    {
        List<string> terrainTextureNames = new List<string>();

        terrainTextureNames.Add(terrainManager.textureType1);
        terrainTextureNames.Add(terrainManager.textureType2);
        terrainTextureNames.Add(terrainManager.textureType3);
        terrainTextureNames.Add(terrainManager.textureType4);
        terrainTextureNames.Add(terrainManager.textureType5);

        List<Material> terrainMaterials =  new List<Material>();

        foreach (string textureName in terrainTextureNames)
        {
            Shader myShader = Shader.Find("Shader Graphs/Terrain");

            Material terrainMaterial = new Material(myShader);

            //Apply base terrain texture
            Texture2D selectedTexture = null;

            foreach (Object tempTexture in terrainManager.scene.terrainTexturesPool)
            {
                if (tempTexture.name == textureName)
                {
                    selectedTexture = (Texture2D)tempTexture;
                    break;
                }
            }

            terrainMaterial.SetTexture("_Main_Texture", selectedTexture);

            //Apply cliff terrain texture
            Texture2D selectedCliffTexture = null;

            foreach (Object tempTexture in terrainManager.scene.terrainTexturesPool)
            {
                if (tempTexture.name == terrainManager.cliffTextureType)
                {
                    selectedCliffTexture = (Texture2D)tempTexture;
                    break;
                }
            }

            terrainMaterial.SetTexture("_Cliff_Texture", selectedCliffTexture);

            terrainMaterials.Add(terrainMaterial);
        }

        terrainManager.terrainMaterials = terrainMaterials.ToArray();
    }

    //This functions is called in update and continues to generate tiles as needed
    public static void ContinueTerrainGeneration(TerrainManager tileManager)
    {
        if (tileManager.initialGenerationComplete == true)
        {
            QueueEdgeTiles(tileManager);
            CleanupDistantTiles(tileManager);

            if (tileManager.tileGeneration == null)
            {
                tileManager.tileGeneration = new Task(GenerateTiles(tileManager));
                tileManager.terrainTasks.Add(tileManager.tileGeneration); //This adds the task to the list
            }
            else
            {
                if (tileManager.tileGeneration.Running == false)
                {
                    tileManager.tileGeneration = new Task(GenerateTiles(tileManager));
                    tileManager.terrainTasks.Add(tileManager.tileGeneration); //This adds the task to the list
                }
            }
        }
    }

    //This sets the default settings for the terrain noise
    public static void SetTerrainNoiseSettings(TerrainManager terrainManager)
    {
        int seed = terrainManager.seed;

        //Terrain Noise Types
        terrainManager.mountainNoise.SetSeed(seed);
        terrainManager.mountainNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        terrainManager.mountainNoise.SetFrequency(0.002f);
        terrainManager.mountainNoise.SetFractalType(FastNoiseLite.FractalType.Ridged);
        terrainManager.mountainNoise.SetFractalOctaves(5);
        terrainManager.mountainNoise.SetFractalLacunarity(2.2f);
        terrainManager.mountainNoise.SetFractalGain(0.6f);

        terrainManager.hillNoise.SetSeed(seed);
        terrainManager.hillNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        terrainManager.hillNoise.SetFrequency(0.005f);
        terrainManager.hillNoise.SetFractalType(FastNoiseLite.FractalType.FBm);
        terrainManager.hillNoise.SetFractalOctaves(3);
        terrainManager.hillNoise.SetFractalLacunarity(2f);
        terrainManager.hillNoise.SetFractalGain(0.3f);

        terrainManager.desertNoise.SetSeed(seed);
        terrainManager.desertNoise.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
        terrainManager.desertNoise.SetFrequency(0.01f);
        terrainManager.desertNoise.SetFractalType(FastNoiseLite.FractalType.None);

        terrainManager.plainsNoise.SetSeed(seed);
        terrainManager.plainsNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        terrainManager.plainsNoise.SetFrequency(0.0005f);
        terrainManager.plainsNoise.SetFractalType(FastNoiseLite.FractalType.FBm);
        terrainManager.plainsNoise.SetFractalOctaves(3);
        terrainManager.plainsNoise.SetFractalLacunarity(2f);
        terrainManager.plainsNoise.SetFractalGain(0.3f);

        //Terrain Mask Noise
        terrainManager.canyonNoise.SetSeed(seed);
        terrainManager.canyonNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        terrainManager.canyonNoise.SetFrequency(0.005f);
        terrainManager.canyonNoise.SetFractalType(FastNoiseLite.FractalType.FBm);
        terrainManager.canyonNoise.SetFractalOctaves(3);
        terrainManager.canyonNoise.SetFractalLacunarity(2f);
        terrainManager.canyonNoise.SetFractalGain(0.3f);
    }

    //This queues the tiles that need to be made before the mission is started
    public static void QueueInitialTiles(TerrainManager tileManager)
    {
        int halfArea = tileManager.initialAreaSize / 2;
        for (int x = -halfArea; x < halfArea; x++)
        {
            for (int z = -halfArea; z < halfArea; z++)
            {
                Vector2Int key = new Vector2Int(x, z);
                if (!tileManager.tiles.ContainsKey(key))
                    tileManager.tilesToGenerate.Add(key);
            }
        }
    }

    //This queues the tiles that need to be generated as the player moves across the terrain
    public static void QueueEdgeTiles(TerrainManager tileManager)
    {
        Vector3 playerPos = tileManager.player.position;
        int tileSize = tileManager.tileSize; 
        int initialAreaSize = tileManager.initialAreaSize;
        int edgeBuffer = tileManager.edgeBuffer;
       
        Vector2Int playerTile = new Vector2Int(
            Mathf.FloorToInt(playerPos.x / tileSize),
            Mathf.FloorToInt(playerPos.z / tileSize)
        );

        int minX = playerTile.x - (initialAreaSize / 2 + edgeBuffer);
        int maxX = playerTile.x + (initialAreaSize / 2 + edgeBuffer);
        int minZ = playerTile.y - (initialAreaSize / 2 + edgeBuffer);
        int maxZ = playerTile.y + (initialAreaSize / 2 + edgeBuffer);

        for (int x = minX; x < maxX; x++)
        {
            for (int z = minZ; z < maxZ; z++)
            {
                Vector2Int key = new Vector2Int(x, z);
                if (!tileManager.tiles.ContainsKey(key))
                    tileManager.tilesToGenerate.Add(key);
            }
        }
    }

    //This removes unneeded tiles that are too far away from the player
    public static void CleanupDistantTiles(TerrainManager tileManager)
    {
        Vector3 playerPos = tileManager.player.position;
        int tileSize = tileManager.tileSize;
        int initialAreaSize = tileManager.initialAreaSize;
        int edgeBuffer = tileManager.edgeBuffer;


        Vector2Int playerTile = new Vector2Int(
            Mathf.FloorToInt(playerPos.x / tileSize),
            Mathf.FloorToInt(playerPos.z / tileSize)
        );

        int halfArea = initialAreaSize / 2 + edgeBuffer;

        int minX = playerTile.x - halfArea;
        int maxX = playerTile.x + halfArea;
        int minZ = playerTile.y - halfArea;
        int maxZ = playerTile.y + halfArea;

        List<Vector2Int> keysToRemove = new List<Vector2Int>();

        foreach (var kvp in tileManager.tiles)
        {
            Vector2Int key = kvp.Key;

            // Check if tile is outside the square bounds
            if (key.x < minX || key.x >= maxX || key.y < minZ || key.y >= maxZ)
            {
                GameObject tile = kvp.Value;
                if (tile != null)
                {
                    GameObject.Destroy(tile);
                }
                keysToRemove.Add(key);
            }
        }

        // Clean up dictionary
        foreach (var key in keysToRemove)
        {
            tileManager.tiles.Remove(key);
        }
    }

    //This starts the generation of all the requested tiles
    public static IEnumerator GenerateTiles(TerrainManager terrainManager)
    {
        while (terrainManager.tilesToGenerate.Count > 0)
        {
            var keys = new List<Vector2Int>(terrainManager.tilesToGenerate);

            int count = 0;

            foreach (var key in keys)
            {
                int x = key.x;
                int z = key.y;

                Vector3 tilePosition = new Vector3(x * terrainManager.tileSize, 0, z * terrainManager.tileSize);

                GameObject tileObj = new GameObject();
                tileObj.name = "terrain_tile";
                tileObj.transform.parent = terrainManager.transform;
                tileObj.transform.localPosition = tilePosition;
                tileObj.transform.rotation = Quaternion.identity;
                tileObj.AddComponent<MeshFilter>();
                tileObj.AddComponent<MeshRenderer>();
                tileObj.AddComponent<MeshCollider>();
                tileObj.layer = 7;

                if (terrainManager.initialGenerationComplete == true)
                {
                    Task a = new Task(GenerateTileMesh(terrainManager, tileObj, tilePosition, x, z));
                    terrainManager.terrainTasks.Add(a); //This adds the task to the list

                    while (a.Running == true)
                    {
                        yield return null;
                    }
                }
                else
                {
                    Task a = new Task(GenerateTileMesh(terrainManager, tileObj, tilePosition, x, z));
                    terrainManager.terrainTasks.Add(a); //This adds the task to the list

                    if (count > 5)
                    {
                        yield return null;
                        count = 0;
                    }
                    count++;
                }

                //This applies the material
                MeshRenderer meshRenderer = tileObj.GetComponent<MeshRenderer>();

                int selection = UnityEngine.Random.Range(0, terrainManager.terrainMaterials.Length);

                meshRenderer.sharedMaterial = terrainManager.terrainMaterials[selection];

                //This creates the sea tile
                GenerateSeaTile(terrainManager, tileObj);

                terrainManager.tiles[new Vector2Int(x, z)] = tileObj;

                terrainManager.tilesToGenerate.Remove(key);
            }
        }
    }

    //This generates the actual mesh from the noise
    public static IEnumerator GenerateTileMesh(TerrainManager terrainManager, GameObject tileObj, Vector3 position, int xIndex, int zIndex)
    {
        float tileNoiseScale = terrainManager.tileNoiseScale;
        float terrainHeight = terrainManager.terrainHeight;
        float canyonDepth = terrainManager.canyonDepth;
        float blendFactor = terrainManager.blendFactor;
        int tileSize = terrainManager.tileSize;
        TerrainType terrainType = terrainManager.terrainType;
        MaskType maskType = terrainManager.maskType;
        BlendType blendType = terrainManager.blendType;
        FastNoiseLite mountainNoise = terrainManager.mountainNoise;
        FastNoiseLite hillNoise = terrainManager.hillNoise;
        FastNoiseLite desertNoise = terrainManager.desertNoise;
        FastNoiseLite plainsNoise = terrainManager.plainsNoise;

        // Procedurally generate mesh using Perlin noise
        Mesh mesh = new Mesh();
        int resolution = 50;

        Vector3[] vertices = new Vector3[resolution * resolution];
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        Vector2[] uv = new Vector2[vertices.Length];

        int count = 0;

        for (int z = 0; z < resolution; z++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = z * resolution + x;

                float worldX = position.x + ((float)x / (resolution - 1)) * terrainManager.tileSize;
                float worldZ = position.z + ((float)z / (resolution - 1)) * terrainManager.tileSize;

                float height = 0;

                //Add terrain noise
                if (terrainType == TerrainType.Mountains)
                {
                    height = (mountainNoise.GetNoise(worldX * tileNoiseScale, worldZ * tileNoiseScale) + 1);
                }
                else if (terrainType == TerrainType.Hills)
                {
                    height = (hillNoise.GetNoise(worldX * tileNoiseScale, worldZ * tileNoiseScale) + 1f);
                }
                else if (terrainType == TerrainType.Desert)
                {
                    height = (desertNoise.GetNoise(worldX * tileNoiseScale, worldZ * tileNoiseScale) + 1f);
                }
                else if (terrainType == TerrainType.Plains)
                {
                    height = (plainsNoise.GetNoise(worldX * tileNoiseScale, worldZ * tileNoiseScale) + 1f);
                }
              
                //Add mask noise
                if (maskType == MaskType.Terraces)
                {
                    float numTerraces = terrainManager.terraceNumber;

                    float normalized01Noise = (height + 1f) / 2f; // Maps -1 to 0, 0 to 0.5, 1 to 1

                    // 2. Scale this 0-1 range to allow for integer stepping based on numTerraces
                    float scaledNoise = normalized01Noise * numTerraces;

                    // 3. Floor the scaled noise to get discrete steps (0, 1, 2, ..., numTerraces-1)
                    float steppedNoise = Mathf.Floor(scaledNoise);

                    height = steppedNoise;
                }

                //This applies the height only after the terracing has been applied
                height = height * terrainHeight;
               
                if (maskType == MaskType.Canyons)
                {
                    float canyonNoise = terrainManager.canyonNoise.GetNoise(worldX * tileNoiseScale, worldZ * tileNoiseScale);

                    // Carve if the noise is below a certain negative threshold
                    if (canyonNoise < 0.3f) // Adjust -0.5f to control river width/occurrence
                    {
                        height -= canyonDepth;
                    }
                }

                //Add blend noise
                if (blendType == BlendType.Mountains)
                {
                    float blendHeight = (mountainNoise.GetNoise(worldX * tileNoiseScale, worldZ * tileNoiseScale) + 1f) * terrainHeight;
                    height = Mathf.Lerp(height, blendHeight, blendFactor);
                }
                else if (blendType == BlendType.Hills)
                {
                    float blendHeight = (hillNoise.GetNoise(worldX * tileNoiseScale, worldZ * tileNoiseScale) + 1f) * terrainHeight;
                    height = Mathf.Lerp(height, blendHeight, blendFactor);
                }
                else if (blendType == BlendType.Desert)
                {
                    float blendHeight = (desertNoise.GetNoise(worldX * tileNoiseScale, worldZ * tileNoiseScale) + 1f) * terrainHeight;
                    height = Mathf.Lerp(height, blendHeight, blendFactor);
                }
                else if (blendType == BlendType.Plains)
                {
                    float blendHeight = (plainsNoise.GetNoise(worldX * tileNoiseScale, worldZ * tileNoiseScale) + 1f) * terrainHeight;
                    height = Mathf.Lerp(height, blendHeight, blendFactor);
                }

                vertices[i] = new Vector3(
                    (float)x / (resolution - 1) * tileSize,
                    height,
                    (float)z / (resolution - 1) * tileSize
                );

                uv[i] = new Vector2((float)x / (resolution - 1), (float)z / (resolution - 1));
            }

            if (terrainManager.initialGenerationComplete == true) //This slows down the terrain generation during gameplay
            {
                if (count > 25)
                {
                    yield return null;
                    count = 0;
                }

                count += 1;
            }
        }

        int t = 0;

        for (int z = 0; z < resolution - 1; z++)
        {
            for (int x = 0; x < resolution - 1; x++)
            {
                int i = z * resolution + x;
                triangles[t++] = i;
                triangles[t++] = i + resolution;
                triangles[t++] = i + 1;
                triangles[t++] = i + 1;
                triangles[t++] = i + resolution;
                triangles[t++] = i + resolution + 1;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();

        if (tileObj != null)
        {
            MeshFilter mf = tileObj.GetComponent<MeshFilter>();
            if (mf == null) { mf = tileObj.AddComponent<MeshFilter>(); }
            mf.mesh = mesh;

            MeshRenderer mr = tileObj.GetComponent<MeshRenderer>();
            if (mr == null) { mr = tileObj.AddComponent<MeshRenderer>(); }

            MeshCollider mc = tileObj.GetComponent<MeshCollider>();
            if (mc == null) { mc = tileObj.AddComponent<MeshCollider>(); }
            mc.sharedMesh = mesh;
        }

        yield return null;
    }

    //This generates the sea tile under the terrain tile
    public static void GenerateSeaTile(TerrainManager terrainManager, GameObject tile)
    {
        // 1. Create a primitive plane GameObject
        GameObject seatile = GameObject.CreatePrimitive(PrimitiveType.Plane);

        seatile.transform.SetParent(tile.transform);

        // 2. Set the Y position
        seatile.transform.localPosition = new Vector3(500, terrainManager.seaLevel, 500);

        // 3. Scale the plane to the desired dimensions
        float scaleX = 100f;
        float scaleZ = 100f;
        seatile.transform.localScale = new Vector3(scaleX, 1 , scaleZ); // Y-scale is typically 1 for a flat plane

        // 3. Give it a name
        seatile.name = "seatile";

        //4. Apply material
        MeshRenderer meshRenderer = seatile.GetComponent<MeshRenderer>();

        meshRenderer.sharedMaterial = terrainManager.seaMaterial;
    }

    //This generates the distant plane which sits under the terrain and fills out the horizon
    public static void CreateDistantPlane()
    {
        //This gets the scene and terrain references
        Scene scene = SceneFunctions.GetScene();
        TerrainManager terrainManager = GetTerrainManager();

        // 1. Create a primitive plane GameObject
        GameObject distantPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);

        terrainManager.distantPlane = distantPlane;

        distantPlane.transform.SetParent(scene.transform);

        // 2. Set the Y position
        distantPlane.transform.localPosition = new Vector3(distantPlane.transform.localPosition.x, -500, distantPlane.transform.localPosition.z);

        // 3. Scale the plane to the desired dimensions
        float scaleX = 90000f;
        float scaleZ = 90000f;
        distantPlane.transform.localScale = new Vector3(scaleX, 1, scaleZ); // Y-scale is typically 1 for a flat plane

        // 3. Give it a name
        distantPlane.name = "distantplane";

        //4. Apply a new material
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");

        Material terrainMaterial = new Material(shader);

        MeshRenderer meshRenderer = distantPlane.GetComponent<MeshRenderer>();

        meshRenderer.sharedMaterial = terrainMaterial;
    }

    //This keeps the distance plane always centered but always below the terrain
    public static void KeepDistantPlaneCentered(TerrainManager terrainManager)
    {
       GameObject distantPlane = terrainManager.distantPlane;

        if  (distantPlane != null)
        {
            distantPlane.transform.position = new Vector3(0, terrainManager.scene.transform.position.y - 500, 0);
        }
    }

    //This stops all terrain coroutins
    public static void StopTerrainCoroutines()
    {
        TerrainManager tileManager = GetTerrainManager();

        tileManager.initialGenerationComplete = false; //This stops the terrain generator from starting another coroutine

        foreach(Task task in tileManager.terrainTasks)
        {
            if (task != null)
            {
                task.Stop();
            }
        }
    }

    //This destroys all the terrain tiles
    public static void DestroyAllTiles()
    {
        TerrainManager terrainManager = GetTerrainManager();

        if (terrainManager.distantPlane != null)
        {
            GameObject.Destroy(terrainManager.distantPlane);
        }

        foreach (var kvp in terrainManager.tiles)
        {
            GameObject tile = kvp.Value;
                
            if (tile != null)
            {
                GameObject.Destroy(tile);
            }
        }
    }

    //This gets the tile manager
    public static TerrainManager GetTerrainManager()
    {
        TerrainManager tileManager = GameObject.FindFirstObjectByType<TerrainManager>();

        return tileManager;
    }

}
