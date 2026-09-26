using UnityEngine;

/// <summary>
/// Generates a single terrain tile mesh using FastNoiseLite with varied, natural terrain.
/// Uses a biome blending system similar to the proven terrain manager.
/// </summary>
public class TerrainGenerator
{
    private readonly int seed;
    private readonly float tileSize;
    private readonly int verticesPerSide;
    private readonly float maxHeight;
    private readonly AnimationCurve heightCurve;
    private readonly float tileNoiseScale;

    private readonly FastNoiseLite mountainNoise;
    private readonly FastNoiseLite hillNoise;
    private readonly FastNoiseLite desertNoise;
    private readonly FastNoiseLite plainsNoise;
    private readonly FastNoiseLite biomeNoise;

    // Amplitude multipliers for each biome - these need to be MUCH larger
    private float plainsAmp = 50f;
    private float desertAmp = 100f;
    private float hillsAmp = 200f;
    private float mountAmp = 400f;

    // Biome percentages
    private float plainsPercentage = 0.25f;
    private float desertPercentage = 0.25f;
    private float hillsPercentage = 0.25f;

    public TerrainGenerator(
        int seed,
        float tileSize,
        int verticesPerSide,
        float maxHeight,
        AnimationCurve heightCurve,
        float tileNoiseScale = 0.2f
    )
    {
        this.seed = seed;
        this.tileSize = tileSize;
        this.verticesPerSide = verticesPerSide;
        this.maxHeight = maxHeight;
        this.heightCurve = heightCurve ?? AnimationCurve.Linear(0, 0, 1, 1);
        this.tileNoiseScale = tileNoiseScale;

        mountainNoise = new FastNoiseLite(seed);
        hillNoise = new FastNoiseLite(seed + 1);
        desertNoise = new FastNoiseLite(seed + 2);
        plainsNoise = new FastNoiseLite(seed + 3);
        biomeNoise = new FastNoiseLite(seed + 4);

        InitializeNoiseGenerators();
    }

    private void InitializeNoiseGenerators()
    {
        // Mountain peaks: sharp, craggy features
        mountainNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        mountainNoise.SetFrequency(0.002f);
        mountainNoise.SetFractalType(FastNoiseLite.FractalType.Ridged);
        mountainNoise.SetFractalOctaves(5);
        mountainNoise.SetFractalLacunarity(2.2f);
        mountainNoise.SetFractalGain(0.6f);

        // Rolling hills: medium scale undulation
        hillNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        hillNoise.SetFrequency(0.005f);
        hillNoise.SetFractalType(FastNoiseLite.FractalType.FBm);
        hillNoise.SetFractalOctaves(3);
        hillNoise.SetFractalLacunarity(2.0f);
        hillNoise.SetFractalGain(0.3f);

        // Desert: cellular noise for rocky appearance
        desertNoise.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
        desertNoise.SetFrequency(0.01f);

        // Plains: large rolling terrain
        plainsNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        plainsNoise.SetFrequency(0.0005f);
        plainsNoise.SetFractalType(FastNoiseLite.FractalType.FBm);
        plainsNoise.SetFractalOctaves(3);
        plainsNoise.SetFractalLacunarity(2.0f);
        plainsNoise.SetFractalGain(0.3f);

        // Biome selector: determines which terrain type appears where
        biomeNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        biomeNoise.SetFrequency(0.0005f);
        biomeNoise.SetFractalType(FastNoiseLite.FractalType.FBm);
        biomeNoise.SetFractalOctaves(2);
        biomeNoise.SetFractalLacunarity(2.0f);
        biomeNoise.SetFractalGain(0.5f);
    }

    private float SampleTerrainHeight(float worldX, float worldZ)
    {
        // Get biome control value (0 to 1)
        float biomeControl = biomeNoise.GetNoise(worldX * tileNoiseScale * 0.2f, worldZ * tileNoiseScale * 0.2f);
        biomeControl = (biomeControl + 1f) / 2f;

        // Sample each biome's height - noise is [-1, 1], so (noise + 1f) gives [0, 2], multiply by amplitude
        float hPlains = (plainsNoise.GetNoise(worldX * tileNoiseScale, worldZ * tileNoiseScale) + 1f) * plainsAmp;
        float hDesert = (desertNoise.GetNoise(worldX * tileNoiseScale, worldZ * tileNoiseScale) + 1f) * desertAmp;
        float hHills = (hillNoise.GetNoise(worldX * tileNoiseScale, worldZ * tileNoiseScale) + 1f) * hillsAmp;
        float hMount = (mountainNoise.GetNoise(worldX * tileNoiseScale, worldZ * tileNoiseScale) + 1f) * mountAmp;

        // Calculate biome boundaries
        float valueA = plainsPercentage;
        float valueB = plainsPercentage + desertPercentage + hillsPercentage;

        if (valueA > 0.5f) valueA = 0.49f;
        if (valueB > 1f) valueB = 0.98f;

        float height = 0f;

        // Blend between biomes
        if (biomeControl < valueA)
        {
            float blendFactor = Mathf.InverseLerp(0.0f, valueA, biomeControl);
            height = Mathf.Lerp(hPlains, hDesert, blendFactor);
        }
        else if (biomeControl < valueB)
        {
            float blendFactor = Mathf.InverseLerp(valueA, valueB, biomeControl);
            height = Mathf.Lerp(hDesert, hHills, blendFactor);
        }
        else
        {
            float blendFactor = Mathf.InverseLerp(valueB, 1.0f, biomeControl);
            height = Mathf.Lerp(hHills, hMount, blendFactor);
        }

        return height;
    }

    public Mesh GenerateMesh(Vector2Int tileCoordinate)
    {
        Mesh mesh = new Mesh();
        mesh.name = $"Terrain_Tile_{tileCoordinate.x}_{tileCoordinate.y}";

        Vector3[] vertices = new Vector3[verticesPerSide * verticesPerSide];
        Vector2[] uvs = new Vector2[verticesPerSide * verticesPerSide];
        int[] triangles = new int[(verticesPerSide - 1) * (verticesPerSide - 1) * 6];

        float step = tileSize / (verticesPerSide - 1);
        float tileWorldX = tileCoordinate.x * tileSize;
        float tileWorldZ = tileCoordinate.y * tileSize;

        int vertexIndex = 0;

        for (int z = 0; z < verticesPerSide; z++)
        {
            for (int x = 0; x < verticesPerSide; x++)
            {
                float worldX = tileWorldX + x * step;
                float worldZ = tileWorldZ + z * step;

                float height = SampleTerrainHeight(worldX, worldZ);

                vertices[vertexIndex] = new Vector3(
                    x * step,
                    height,
                    z * step
                );

                uvs[vertexIndex] = new Vector2(
                    x / (float)(verticesPerSide - 1),
                    z / (float)(verticesPerSide - 1)
                );

                vertexIndex++;
            }
        }

        int triangleIndex = 0;

        for (int z = 0; z < verticesPerSide - 1; z++)
        {
            for (int x = 0; x < verticesPerSide - 1; x++)
            {
                int topLeft = z * verticesPerSide + x;
                int topRight = topLeft + 1;
                int bottomLeft = topLeft + verticesPerSide;
                int bottomRight = bottomLeft + 1;

                triangles[triangleIndex++] = topLeft;
                triangles[triangleIndex++] = bottomLeft;
                triangles[triangleIndex++] = topRight;

                triangles[triangleIndex++] = topRight;
                triangles[triangleIndex++] = bottomLeft;
                triangles[triangleIndex++] = bottomRight;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();

        return mesh;
    }

    public float GetHeightAtLocalPosition(Vector2Int tileCoordinate, Vector3 localPosition)
    {
        float worldX = tileCoordinate.x * tileSize + localPosition.x;
        float worldZ = tileCoordinate.y * tileSize + localPosition.z;

        return SampleTerrainHeight(worldX, worldZ);
    }
}