using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TileManager : MonoBehaviour
{
    Scene scene;

    public GameObject tilePrefab; // Assign a tile prefab with a mesh
    public int tileSize = 1000;   // 1km per tile
    public int initialAreaSize = 30; // 30km x 30km
    public float tileNoiseScale = 0.2f; // adjust for desired terrain roughness
    public Transform player;
    public int tilesPerFrame = 2; // Number of tiles to generate per frame
    public string terrainTextureType = "forest-mixed";

    private Dictionary<Vector2Int, GameObject> tiles = new Dictionary<Vector2Int, GameObject>();
    private int edgeBuffer = 10; // 10km from edge to trigger generation
    private HashSet<Vector2Int> tilesToGenerate = new HashSet<Vector2Int>();
    private Coroutine tileGenCoroutine;
    private bool initialGenerationComplete = false;
    private FastNoiseLite fastNoiseLite;

    void Start()
    {
        fastNoiseLite = new FastNoiseLite();
        fastNoiseLite.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);

        fastNoiseLite.SetFrequency(0.01f);
        fastNoiseLite.SetFractalLacunarity(2f);
        fastNoiseLite.SetFractalGain(0.5f);
        fastNoiseLite.SetFractalType(FastNoiseLite.FractalType.FBm);
        // You can further configure frequency, seed, etc.
    }

    void Update()
    {
        if (initialGenerationComplete == true)
        {
            QueueEdgeTiles();

            if (tileGenCoroutine == null)
            {
                tileGenCoroutine = StartCoroutine(GenerateTiles());
            }
        }
    }

    public IEnumerator GenerateTerain()
    {
        scene = SceneFunctions.GetScene();

        GameObject newTile = new GameObject();
        newTile.AddComponent<MeshFilter>();
        newTile.AddComponent<MeshRenderer>();
        newTile.AddComponent<MeshCollider>();

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

    IEnumerator GenerateTiles()
    {
        while (tilesToGenerate.Count > 0)
        {
            var keys = new List<Vector2Int>(tilesToGenerate);

            foreach (var key in keys)
            {
                int x = key.x;
                int z = key.y;

                Vector3 tilePosition = new Vector3(x * tileSize, 0, z * tileSize);

                GameObject tileObj = Instantiate(tilePrefab, tilePosition, Quaternion.identity);

                tileObj.transform.parent = transform;

                tileObj.transform.localPosition = tilePosition;

                Task a = new Task(GenerateTileMesh(tileObj, tilePosition, x, z));

                while (a.Running == true)
                {
                    yield return null;
                }

                if (scene.terrainTexturesMaterialPool != null)
                {
                    if (scene.terrainTexturesMaterialPool.Length > 0)
                    {
                        MeshRenderer meshRenderer = tileObj.GetComponent<MeshRenderer>();

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
        }

        tileGenCoroutine = null;
    }

    IEnumerator GenerateTileMesh(GameObject tileObj, Vector3 position, int xIndex, int zIndex)
    {
        // Procedurally generate mesh using Perlin noise
        Mesh mesh = new Mesh();
        int resolution = 50; // 100 x 100 vertices

        Vector3[] vertices = new Vector3[resolution * resolution];
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        Vector2[] uv = new Vector2[vertices.Length];

        int count = 0;

        float noiseCheck = 0;

        for (int z = 0; z < resolution; z++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = z * resolution + x;

                float worldX = position.x + ((float)x / (resolution - 1)) * tileSize;
                float worldZ = position.z + ((float)z / (resolution - 1)) * tileSize;

                //float height = Mathf.PerlinNoise(
                //    worldX * tileNoiseScale,
                //    worldZ * tileNoiseScale
                //) * 50;

                float height = (fastNoiseLite.GetNoise(worldX * tileNoiseScale, worldZ * tileNoiseScale) + 1f) * 25f;

                noiseCheck = height;

                vertices[i] = new Vector3(
                    (float)x / (resolution - 1) * tileSize,
                    height,
                    (float)z / (resolution - 1) * tileSize
                );

                uv[i] = new Vector2((float)x / (resolution - 1), (float)z / (resolution - 1));
            }

            count += 1;

            //if (count > 25)
            //{
            //    yield return null;
            //    count = 0;
            //    Debug.Log("this was run B: " + noiseCheck.ToString());
            //}
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

            count += 1;

            //if (count > 25)
            //{
            //    yield return null;
            //    count = 0;
            //}
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();

        MeshFilter mf = tileObj.GetComponent<MeshFilter>();
        if (mf == null) mf = tileObj.AddComponent<MeshFilter>();
        mf.mesh = mesh;

        MeshRenderer mr = tileObj.GetComponent<MeshRenderer>();
        if (mr == null) mr = tileObj.AddComponent<MeshRenderer>();

        yield return null;
    }
}