using System.Collections.Generic;
using UnityEngine;

public class TileStreamer : MonoBehaviour
{
    [Header("Generation")]
    [SerializeField] public int seed = 1138;

    [Header("Tile Settings")]
    [SerializeField] private float tileSize = 1000f;

    [Tooltip("The ship is surrounded by this many kilometres of tiles.")]
    [SerializeField] private float tileRadiusInKilometres = 30f;

    [Header("Ship")]
    [SerializeField] public Transform ship;

    [Header("Tile Parent")]
    [SerializeField] public Transform tileParent;

    [Header("Tile Prefabs")]
    [SerializeField] public string tileType;
    [SerializeField] private string resourcesFolder = "objects/tiles/";

    private readonly Dictionary<Vector2Int, GameObject> loadedTiles = new();

    private GameObject[] tilePrefabs;
    private Vector2Int currentShipTile;
    private int tileRadiusInTiles;
    private bool initialized;

    private void Start()
    {
        if (ship == null)
        {
            Debug.LogError("TerrainTileStreamer: Ship is not assigned.");
            enabled = false;
            return;
        }

        if (tileSize <= 0f)
        {
            Debug.LogError("TerrainTileStreamer: Tile size must be greater than zero.");
            enabled = false;
            return;
        }

        if (tileRadiusInKilometres <= 0f)
        {
            Debug.LogError(
                "TerrainTileStreamer: Tile radius must be greater than zero."
            );

            enabled = false;
            return;
        }

        if (tileParent == null)
        {
            GameObject parent = new GameObject("Generated Terrain Tiles");
            tileParent = parent.transform;
        }

        tilePrefabs = Resources.LoadAll<GameObject>(resourcesFolder);

        if (tilePrefabs == null || tilePrefabs.Length == 0)
        {
            Debug.LogError(
                $"TerrainTileStreamer: No tile prefabs found in " +
                $"Resources/{resourcesFolder}"
            );

            enabled = false;
            return;
        }

        // For 30 km and 1 km tiles, this is 30 tiles in each direction.
        tileRadiusInTiles = Mathf.CeilToInt(
            tileRadiusInKilometres * 1000f / tileSize
        );

        Vector3 shipLocalPosition = tileParent.InverseTransformPoint(ship.position);
        currentShipTile = LocalPositionToTileCoordinate(shipLocalPosition);

        UpdateLoadedTiles();

        initialized = true;
    }

    private void Update()
    {
        if (!initialized)
            return;

        // Convert the ship's world position into the tile parent's local space.
        Vector3 shipLocalPosition = tileParent.InverseTransformPoint(ship.position);

        // Only local X and local Z affect tile streaming.
        Vector2Int shipTile = LocalPositionToTileCoordinate(shipLocalPosition);

        if (shipTile != currentShipTile)
        {
            currentShipTile = shipTile;
            UpdateLoadedTiles();
        }
    }

    private Vector2Int LocalPositionToTileCoordinate(Vector3 localPosition)
    {
        /*
         * The tile grid uses the tile parent's local X/Z axes.
         *
         * localPosition.y is ignored.
         */
        int tileX = Mathf.FloorToInt(localPosition.x / tileSize);
        int tileZ = Mathf.FloorToInt(localPosition.z / tileSize);

        return new Vector2Int(tileX, tileZ);
    }

    private Vector3 TileToLocalPosition(Vector2Int coordinate)
    {
        /*
         * Tiles are always positioned at local Y = 0.
         */
        return new Vector3(
            coordinate.x * tileSize + tileSize * 0.5f,
            0f,
            coordinate.y * tileSize + tileSize * 0.5f
        );
    }

    private void UpdateLoadedTiles()
    {
        HashSet<Vector2Int> requiredTiles = new();

        /*
         * A square is used instead of a circle so the ship is always
         * surrounded in every direction without missing corner tiles.

         * With a 30 km radius and 1 km tiles:
         * -X: 30 tiles
         * +X: 30 tiles
         * -Z: 30 tiles
         * +Z: 30 tiles
         *
         * This loads up to 61 x 61 tiles.
         */
        for (int z = -tileRadiusInTiles; z <= tileRadiusInTiles; z++)
        {
            for (int x = -tileRadiusInTiles; x <= tileRadiusInTiles; x++)
            {
                Vector2Int tileCoordinate = new Vector2Int(
                    currentShipTile.x + x,
                    currentShipTile.y + z
                );

                requiredTiles.Add(tileCoordinate);

                if (!loadedTiles.ContainsKey(tileCoordinate))
                {
                    LoadTile(tileCoordinate);
                }
            }
        }

        List<Vector2Int> tilesToUnload = new();

        foreach (Vector2Int loadedCoordinate in loadedTiles.Keys)
        {
            if (!requiredTiles.Contains(loadedCoordinate))
            {
                tilesToUnload.Add(loadedCoordinate);
            }
        }

        foreach (Vector2Int coordinate in tilesToUnload)
        {
            UnloadTile(coordinate);
        }
    }

    private void LoadTile(Vector2Int coordinate)
    {
        int prefabIndex = GetDeterministicPrefabIndex(coordinate);
        GameObject prefab = tilePrefabs[prefabIndex];

        GameObject tile = Instantiate(
            prefab,
            tileParent
        );

        // Set the tile's position using local coordinates.
        tile.transform.localPosition = TileToLocalPosition(coordinate);

        // Rotate around the tile parent's local Y axis.
        tile.transform.localRotation = GetDeterministicRotation(coordinate);

        tile.name = $"Tile_{coordinate.x}_{coordinate.y}";
        loadedTiles.Add(coordinate, tile);
    }

    private void UnloadTile(Vector2Int coordinate)
    {
        if (!loadedTiles.TryGetValue(coordinate, out GameObject tile))
            return;

        if (tile != null)
        {
            Destroy(tile);
        }

        loadedTiles.Remove(coordinate);
    }

    private int GetDeterministicPrefabIndex(Vector2Int coordinate)
    {
        int hash = CoordinateHash(
            seed,
            coordinate.x,
            coordinate.y
        );

        return PositiveModulo(hash, tilePrefabs.Length);
    }

    private Quaternion GetDeterministicRotation(Vector2Int coordinate)
    {
        int hash = CoordinateHash(
            seed + 1000,
            coordinate.x,
            coordinate.y
        );

        int rotationStep = PositiveModulo(hash, 4);

        return Quaternion.Euler(
            0f,
            rotationStep * 90f,
            0f
        );
    }

    private static int CoordinateHash(int seed, int x, int y)
    {
        unchecked
        {
            int hash = seed;

            hash = hash * 31 + x;
            hash = hash * 31 + y;

            hash ^= hash << 13;
            hash ^= hash >> 17;
            hash ^= hash << 5;

            return hash;
        }
    }

    private static int PositiveModulo(int value, int modulus)
    {
        int result = value % modulus;
        return result < 0 ? result + modulus : result;
    }

    private void OnDestroy()
    {
        foreach (GameObject tile in loadedTiles.Values)
        {
            if (tile != null)
            {
                Destroy(tile);
            }
        }

        loadedTiles.Clear();
    }
}