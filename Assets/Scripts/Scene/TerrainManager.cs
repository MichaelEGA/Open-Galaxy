using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Defines the different types of terrain that can be generated.
/// </summary>
public enum TerrainType
{
    Mountains,
    Hills,
    Desert,
    Cliffside,
    Plains
}

/// <summary>
/// Defines the types of terrain that can be blended together.
/// 'None' indicates no blending is applied.
/// </summary>
public enum BlendType
{
    Mountains,
    Hills,
    Desert,
    Cliffside,
    Plains,
    None
}

/// <summary>
/// Defines the types of masks that can be applied to the terrain.
/// 'None' indicates no mask is applied.
/// </summary>
public enum MaskType
{
    Canyons,
    None
}

/// <summary>
/// Manages the generation and updating of terrain tiles in the scene.
/// </summary>
public class TerrainManager : MonoBehaviour
{
    [Header("Scene References")]
    public Scene scene;
    public Transform player;

    [Header("Tile Settings")]
    public int tileSize = 1000;         // Size of each tile in meters (e.g., 1km per tile)
    public int initialAreaSize = 30;    // Initial generation area in tiles (e.g., 30x30 km)
    public float tileNoiseScale = 0.2f; // Adjust for desired terrain roughness

    [Header("Terrain Textures")]
    public string terrainTextureType = "forest-mixed";      // Main terrain texture type
    public string terrainCliffTextureType = "grand-canyon"; // Cliff-specific terrain texture type

    [Header("Generation State")]
    public Dictionary<Vector2Int, GameObject> tiles = new Dictionary<Vector2Int, GameObject>(); // Stores references to active tiles
    public HashSet<Vector2Int> tilesToGenerate = new HashSet<Vector2Int>();                     // Queue of tiles awaiting generation
    public int tilesBeingGenerated;                                                             // Counter for currently generating tiles
    public Task tileGeneration;                                                                  // Reference to the terrain generation task
    public bool initialGenerationComplete = false;                                              // Flag to indicate initial terrain generation is done
    public int edgeBuffer = 10; // Distance from the player to the edge of the generated area to trigger new generation (in tiles/km)
    public List<Task> terrainTasks = new List<Task>();

    [Header("Noise Generators")]
    // FastNoiseLite instances for different terrain types
    public FastNoiseLite mountainNoise = new FastNoiseLite();
    public FastNoiseLite hillNoise = new FastNoiseLite();
    public FastNoiseLite desertNoise = new FastNoiseLite();
    public FastNoiseLite cliffsideNoise = new FastNoiseLite();
    public FastNoiseLite plainsNoise = new FastNoiseLite();
    public FastNoiseLite canyonNoise = new FastNoiseLite(); // Noise for canyon masking

    [Header("Terrain Generation Parameters")]
    public TerrainType terrainType = TerrainType.Mountains; // The primary terrain type to generate
    public MaskType maskType = MaskType.None;              // The type of mask to apply (e.g., canyons)
    public BlendType blendType = BlendType.None;            // The type of terrain to blend with
    public float terrainHeight = 50;                        // Maximum height of the terrain
    public float blendFactor = 0.5f;                       // Factor for blending between terrain types
    public float canyonDepth = 50f;                        // Depth of generated canyons
    public int seed = 1337;                                // Seed for noise generation to ensure reproducible terrains

    /// <summary>
    /// Called once per frame.
    /// Continues the terrain generation process.
    /// </summary>
    void Update()
    {
        // This function handles the continuous generation of terrain tiles based on player position
        TerrainManagerFunctions.ContinueTerrainGeneration(this);
    }
}