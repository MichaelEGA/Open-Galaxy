using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum TerrainType
{
    Mountains,
    Hills,
    Desert,
    Cliffside,
    Plains
}

public enum BlendType
{
    Mountains,
    Hills,
    Desert,
    Cliffside,
    Plains,
    None
}

public enum TerraceType
{
    Terrace,
    None
}

public enum MaskType
{
    Canyons,
    None
}
public class TileManager : MonoBehaviour
{
    Scene scene;

    public GameObject tilePrefab; // Assign a tile prefab with a mesh
    public int tileSize = 1000;   // 1km per tile
    public int initialAreaSize = 30; // 30km x 30km
    public float tileNoiseScale = 0.2f; // adjust for desired terrain roughness
    public Transform player;
    public int tilesPerFrame = 3; // Number of tiles to generate per frame
    public int tilesBeingGenerated;
    public string terrainTextureType = "forest-mixed";
    public float terrainHeight = 50;

    private Dictionary<Vector2Int, GameObject> tiles = new Dictionary<Vector2Int, GameObject>();
    private int edgeBuffer = 10; // 10km from edge to trigger generation
    private HashSet<Vector2Int> tilesToGenerate = new HashSet<Vector2Int>();
    private Coroutine tileGenCoroutine;
    private bool initialGenerationComplete = false;
   
    private FastNoiseLite mountainNoise = new FastNoiseLite();
    private FastNoiseLite hillNoise = new FastNoiseLite();
    private FastNoiseLite desertNoise = new FastNoiseLite();
    private FastNoiseLite cliffsideNoise = new FastNoiseLite();
    private FastNoiseLite plainsNoise = new FastNoiseLite();

    private FastNoiseLite canyonNoise = new FastNoiseLite();

    public TerrainType terrainType = TerrainType.Plains;
    public MaskType maskType = MaskType.Canyons;
    public TerraceType terraceType = TerraceType.None;
    public BlendType blendType = BlendType.None;

    public float blendFactor = 0.5f;
    public float canyonDepth = 50f;
    public int terraceNumber = 10;
    public int seed = 1337;

    void Start()
    {
        SetTerrainType();
    }

    void Update()
    {
        if (initialGenerationComplete == true)
        {
            QueueEdgeTiles();
            CleanupDistantTiles();

            if (tileGenCoroutine == null)
            {
                tileGenCoroutine = StartCoroutine(GenerateTiles());
            }
        }
    }

    void SetTerrainType()
    {
        //Terrain Noise Types
        mountainNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        mountainNoise.SetFrequency(0.01f);
        mountainNoise.SetFractalType(FastNoiseLite.FractalType.Ridged);
        mountainNoise.SetFractalOctaves(5);
        mountainNoise.SetFractalLacunarity(2.2f);
        mountainNoise.SetFractalGain(0.6f);

        hillNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        hillNoise.SetFrequency(0.02f);
        hillNoise.SetFractalType(FastNoiseLite.FractalType.FBm);
        hillNoise.SetFractalOctaves(4);
        hillNoise.SetFractalLacunarity(2f);
        hillNoise.SetFractalGain(0.4f);

        desertNoise.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
        desertNoise.SetFrequency(0.01f);
        desertNoise.SetFractalType(FastNoiseLite.FractalType.None);

        cliffsideNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        cliffsideNoise.SetFrequency(0.015f);
        cliffsideNoise.SetFractalType(FastNoiseLite.FractalType.Ridged);
        cliffsideNoise.SetFractalOctaves(6);
        cliffsideNoise.SetFractalLacunarity(2.5f);
        cliffsideNoise.SetFractalGain(0.7f);

        plainsNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        plainsNoise.SetFrequency(0.005f);
        plainsNoise.SetFractalType(FastNoiseLite.FractalType.FBm);
        plainsNoise.SetFractalOctaves(3);
        plainsNoise.SetFractalLacunarity(2f);
        plainsNoise.SetFractalGain(0.3f);

        //Terrain Mask Noise
        canyonNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        canyonNoise.SetFrequency(0.005f);
        canyonNoise.SetFractalType(FastNoiseLite.FractalType.FBm);
        canyonNoise.SetFractalOctaves(3);
        canyonNoise.SetFractalLacunarity(2f);
        canyonNoise.SetFractalGain(0.3f);
    }

    public IEnumerator GenerateTerain()
    {
        scene = SceneFunctions.GetScene();

        GameObject newTile = new GameObject();
        newTile.AddComponent<MeshFilter>();
        newTile.AddComponent<MeshRenderer>();
        newTile.AddComponent<MeshCollider>();
        newTile.layer = 7;

        tilePrefab = newTile;

        QueueInitialTiles();

        Task a = new Task(GenerateTiles());

        while (a.Running == true)
        {
            yield return null;
        }

        initialGenerationComplete = true;
    }

    void QueueInitialTiles()
    {
        int halfArea = initialAreaSize / 2;
        for (int x = -halfArea; x < halfArea; x++)
        {
            for (int z = -halfArea; z < halfArea; z++)
            {
                Vector2Int key = new Vector2Int(x, z);
                if (!tiles.ContainsKey(key))
                    tilesToGenerate.Add(key);
            }
        }
    }

    void QueueEdgeTiles()
    {
        Vector3 playerPos = player.position;
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
                if (!tiles.ContainsKey(key))
                    tilesToGenerate.Add(key);
            }
        }
    }

    void CleanupDistantTiles()
    {
        Vector3 playerPos = player.position;
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

        foreach (var kvp in tiles)
        {
            Vector2Int key = kvp.Key;

            // Check if tile is outside the square bounds
            if (key.x < minX || key.x >= maxX || key.y < minZ || key.y >= maxZ)
            {
                GameObject tile = kvp.Value;
                if (tile != null)
                {
                    Destroy(tile);
                }
                keysToRemove.Add(key);
            }
        }

        // Clean up dictionary
        foreach (var key in keysToRemove)
        {
            tiles.Remove(key);
        }
    }

    IEnumerator GenerateTiles()
    {
        while (tilesToGenerate.Count > 0)
        {
            var keys = new List<Vector2Int>(tilesToGenerate);

            foreach (var key in keys)
            {
                while (tilesBeingGenerated > tilesPerFrame)
                {
                    if (gameObject == null){break;}

                    yield return null;
                }

                if (gameObject == null) { break; }

                int x = key.x;
                int z = key.y;

                Vector3 tilePosition = new Vector3(x * tileSize, 0, z * tileSize);

                GameObject tileObj = Instantiate(tilePrefab, tilePosition, Quaternion.identity);

                tileObj.transform.parent = transform;

                tileObj.transform.localPosition = tilePosition;

                Task a = new Task(GenerateTileMesh(tileObj, tilePosition, x, z));

                MeshRenderer meshRenderer = tileObj.GetComponent<MeshRenderer>();

                if (scene.terrainTexturesMaterialPool != null)
                {
                    if (scene.terrainTexturesMaterialPool.Length > 0)
                    {
                        List<Material> selectedMaterials = new List<Material>();

                        foreach(Object obj in scene.terrainTexturesMaterialPool)
                        {
                            if (obj.name.Contains(terrainTextureType))
                            {
                                selectedMaterials.Add((Material)obj);
                            }
                        }

                        if (selectedMaterials.Count > 0)
                        {
                            if (meshRenderer != null)
                            {
                                int selection = UnityEngine.Random.Range(0, selectedMaterials.Count);

                                meshRenderer.sharedMaterial = selectedMaterials[selection];
                            }
                        }
                    }
                }
               
                tiles[new Vector2Int(x, z)] = tileObj;

                tilesToGenerate.Remove(key);
            }

            if (gameObject == null) { break; }
        }

        tileGenCoroutine = null;
    }

    IEnumerator GenerateTileMesh(GameObject tileObj, Vector3 position, int xIndex, int zIndex)
    {
        tilesBeingGenerated += 1;

        // Procedurally generate mesh using Perlin noise
        Mesh mesh = new Mesh();
        int resolution = 50;

        Vector3[] vertices = new Vector3[resolution * resolution];
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        Vector2[] uv = new Vector2[vertices.Length];

        for (int z = 0; z < resolution; z++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = z * resolution + x;

                float worldX = position.x + ((float)x / (resolution - 1)) * tileSize;
                float worldZ = position.z + ((float)z / (resolution - 1)) * tileSize;

                float height = 0;

                //Add terrain noise
                if (terrainType == TerrainType.Mountains)
                {
                    height = (mountainNoise.GetNoise(worldX * tileNoiseScale, worldZ * tileNoiseScale) + 1) * terrainHeight;
                }
                else if (terrainType == TerrainType.Hills)
                {
                    height = (hillNoise.GetNoise(worldX * tileNoiseScale, worldZ * tileNoiseScale) + 1f) * terrainHeight;
                }
                else if (terrainType == TerrainType.Desert)
                {
                    height = (desertNoise.GetNoise(worldX * tileNoiseScale, worldZ * tileNoiseScale) + 1f) * terrainHeight;
                }
                else if (terrainType == TerrainType.Cliffside)
                {
                    height = (cliffsideNoise.GetNoise(worldX * tileNoiseScale, worldZ * tileNoiseScale) + 1f) * terrainHeight;
                }
                else if (terrainType == TerrainType.Plains)
                {
                    height = (plainsNoise.GetNoise(worldX * tileNoiseScale, worldZ * tileNoiseScale) + 1f) * terrainHeight;
                }

                //Add mask noise
                if (maskType == MaskType.Canyons)
                {
                    float canyonNoise = this.canyonNoise.GetNoise(worldX * tileNoiseScale, worldZ * tileNoiseScale);

                    // Carve if the noise is below a certain negative threshold
                    if (canyonNoise < 0.3f) // Adjust -0.5f to control river width/occurrence
                    {
                        height -= canyonDepth;
                    }
                }

                //Add Terraces
                if (terraceType == TerraceType.Terrace)
                {
                    height = Mathf.Round(height * (terraceNumber - 1)) / (terraceNumber - 1);
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
                else if (blendType == BlendType.Cliffside)
                {
                    float blendHeight = (cliffsideNoise.GetNoise(worldX * tileNoiseScale, worldZ * tileNoiseScale) + 1f) * terrainHeight;
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

        tilesBeingGenerated -= 1;
    }

}