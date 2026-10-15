using System.Collections.Generic;
using UnityEngine;

public class Unity6ForestRenderer : MonoBehaviour
{
    [Header("Assets")]
    [Tooltip("Single mesh used for all trees.")]
    [SerializeField] private Mesh treeMesh;

    [Tooltip("Up to 6 materials. Each material will be used in a separate DrawMeshInstancedIndirect call.")]
    [SerializeField] private Material[] materials = new Material[1];

    [Header("Forest / Placement")]
    [Tooltip("Total number of trees to generate.")]
    [SerializeField] private int totalTreeCount = 100000;
    [Tooltip("If clusterCount <= 0, clusterCount will be computed as ceil(totalTreeCount / avgClusterSize).")]
    [SerializeField] private int clusterCount = 0;
    [Tooltip("Average cluster size used when clusterCount is auto-computed")]
    [SerializeField] private int avgClusterSize = 100;
    [SerializeField] private Vector2 forestSize = new Vector2(15000f, 15000f);

    [Header("Random Scale")]
    [SerializeField] private float minTreeScale = 0.9f;
    [SerializeField] private float maxTreeScale = 1.2f;
    [Tooltip("Fractional ± jitter applied to uniform scale (e.g. 0.15 => ±15%)")]
    [SerializeField] private float sizeJitter = 0.12f;
    [Tooltip("Per-axis non-uniform jitter (fraction).")]
    [SerializeField] private float axisJitter = 0.05f;

    [Header("Clustering")]
    [Tooltip("Default cluster radius when variable radius is disabled")]
    [SerializeField] private float clusterRadius = 40f;
    [Range(0f, 1f)]
    [SerializeField] private float clusterJitter = 0.6f; // jitter for cluster center placement

    [Header("Variable cluster sizes (optional)")]
    [Tooltip("When true, per-cluster counts are randomized then normalized to sum to totalTreeCount")]
    [SerializeField] private bool useVariableClusterSizes = true;

    [Header("Variable cluster radius")]
    [Tooltip("When true, pick a random radius per-cluster in [clusterMinRadius, clusterMaxRadius].")]
    [SerializeField] private bool useVariableClusterRadius = false;
    [SerializeField] private float clusterMinRadius = 100f;
    [SerializeField] private float clusterMaxRadius = 400f;

    [Header("Steep Slope / Placement Filtering")]
    [Tooltip("When true, avoids placing trees on slopes steeper than maxSlopeDegrees.")]
    [SerializeField] private bool avoidSteepSlopes = true;
    [Tooltip("Maximum allowed slope (degrees from up). Trees will not be placed where surface normal angle > this.")]
    [SerializeField] private float maxSlopeDegrees = 30f;
    [Tooltip("How many attempts per instance to find a valid placement before giving up.")]
    [SerializeField] private int maxPlacementAttempts = 8;

    [Header("Anchor / Height Sampling (MESH ONLY)")]
    [Tooltip("Attach this script to the parent GameObject that contains all tile meshes and MeshColliders.")]
    [SerializeField] private Transform anchorTransform;
    [Tooltip("Layer mask used for physics raycasts to hit the terrain tile MeshColliders (tiles must have MeshCollider).")]
    [SerializeField] private LayerMask terrainLayer = ~0;
    [SerializeField] private float raycastStartHeight = 500f;

    [Header("Player & Culling")]
    [Tooltip("Transform of the player used for distance culling (can be camera or player object).")]
    [SerializeField] private Transform playerTransform;
    [Tooltip("Trees farther than this distance from the player will be hidden.")]
    [SerializeField] private float maxDisplayDistance = 15000f;
    [Tooltip("Minimum player movement (world units) to force a culling update.")]
    [SerializeField] private float playerMoveThreshold = 10f;
    [Tooltip("Culling update interval in seconds (when player is near-stationary).")]
    [SerializeField] private float cullingUpdateInterval = 0.5f;

    [Header("Runtime Options")]
    [Tooltip("When true, shader receives a small _WorldOffset so we don't have to re-upload huge buffers on origin changes.")]
    [SerializeField] private bool useShaderWorldOffset = true;
    [Tooltip("Use large bounds while debugging so culling won't hide instances.")]
    [SerializeField] private bool useHugeDebugBounds = true;

    // Internal instance data (full set)
    private Matrix4x4[] fullMatrices;
    private Vector3[] localPositions;
    private Quaternion[] rotations;
    private Vector3[] scales;
    private int[] instanceMaterialIndex; // material assignment per instance

    // Per-cluster radii (if variable)
    private float[] perClusterRadii;

    // Per-material buffers and args
    private GraphicsBuffer[] visibleTransformBuffers;
    private GraphicsBuffer[] argsBuffers;
    private uint[][] argsArrays; // per-material args arrays

    // Temp storage for visible matrices per material
    private Matrix4x4[][] tempVisibleMatricesPerMaterial;
    private int[] visibleCountsPerMaterial;

    // State for culling
    private int visibleTotalCount = 0;
    private float sqrMaxDisplayDistance;
    private Vector3 lastPlayerPos = Vector3.positiveInfinity;
    private float timeSinceLastCull = 0f;

    // Misc
    private Bounds generationBounds;
    private Vector3 worldOffset = Vector3.zero;
    private Vector3 baseForestCenter;

    // Limits
    private const int MAX_MATERIALS = 6;

    void Start()
    {
        if (treeMesh == null)
        {
            Debug.LogError("Unity6ForestRenderer: assign treeMesh in inspector.");
            enabled = false;
            return;
        }

        // Clamp materials count to allowed range and validate
        if (materials == null || materials.Length == 0)
        {
            Debug.LogError("Unity6ForestRenderer: assign at least one material in inspector.");
            enabled = false;
            return;
        }
        if (materials.Length > MAX_MATERIALS)
        {
            Debug.LogWarning($"Unity6ForestRenderer: materials length > {MAX_MATERIALS}. Truncating to first {MAX_MATERIALS} materials.");
            var tmp = new Material[MAX_MATERIALS];
            System.Array.Copy(materials, tmp, MAX_MATERIALS);
            materials = tmp;
        }

        if (anchorTransform == null) anchorTransform = transform;
        if (playerTransform == null && Camera.main != null) playerTransform = Camera.main.transform;

        sqrMaxDisplayDistance = maxDisplayDistance * maxDisplayDistance;
        baseForestCenter = anchorTransform.position;
        generationBounds = useHugeDebugBounds
            ? new Bounds(baseForestCenter, new Vector3(99999f, 99999f, 99999f))
            : new Bounds(baseForestCenter, new Vector3(forestSize.x, 1000f, forestSize.y));

        // Ensure all materials have instancing enabled
        foreach (var mat in materials)
        {
            if (mat != null) mat.enableInstancing = true;
        }

        InitializeAndGenerate();
    }

    private void InitializeAndGenerate()
    {
        // Determine cluster count if user left it <= 0
        int finalClusterCount = clusterCount;
        if (finalClusterCount <= 0)
        {
            int guess = Mathf.CeilToInt((float)totalTreeCount / Mathf.Max(1, avgClusterSize));
            finalClusterCount = Mathf.Clamp(guess, 1, totalTreeCount);
        }

        // Create cluster centers
        List<Vector2> clusterCenters = CreateClusterCenters(finalClusterCount);

        // Decide per-cluster radii
        perClusterRadii = new float[finalClusterCount];
        if (useVariableClusterRadius)
        {
            float minR = Mathf.Max(0f, clusterMinRadius);
            float maxR = Mathf.Max(minR, clusterMaxRadius);
            for (int i = 0; i < finalClusterCount; i++)
                perClusterRadii[i] = Random.Range(minR, maxR);
        }
        else
        {
            for (int i = 0; i < finalClusterCount; i++)
                perClusterRadii[i] = clusterRadius;
        }

        // Decide per-cluster counts (weights normalized to totalTreeCount if variable sizes enabled)
        int[] perClusterCounts = new int[finalClusterCount];
        if (useVariableClusterSizes)
        {
            float[] weights = new float[finalClusterCount];
            float weightSum = 0f;
            for (int i = 0; i < finalClusterCount; i++) { weights[i] = Random.value + 0.0001f; weightSum += weights[i]; }

            int remaining = totalTreeCount;
            for (int i = 0; i < finalClusterCount; i++)
            {
                int alloc = Mathf.FloorToInt(weights[i] / weightSum * totalTreeCount);
                perClusterCounts[i] = alloc;
                remaining -= alloc;
            }
            int idx = 0;
            while (remaining > 0)
            {
                perClusterCounts[idx]++;
                remaining--;
                idx++;
                if (idx >= finalClusterCount) idx = 0;
            }
        }
        else
        {
            int baseCount = totalTreeCount / finalClusterCount;
            int rem = totalTreeCount % finalClusterCount;
            for (int i = 0; i < finalClusterCount; i++)
                perClusterCounts[i] = baseCount + (i < rem ? 1 : 0);
        }

        // Temporary allocations sized to requested total (will trim if placement fails)
        Matrix4x4[] tmpMatrices = new Matrix4x4[totalTreeCount];
        Vector3[] tmpLocalPositions = new Vector3[totalTreeCount];
        Quaternion[] tmpRotations = new Quaternion[totalTreeCount];
        Vector3[] tmpScales = new Vector3[totalTreeCount];
        int[] tmpMaterialIndex = new int[totalTreeCount];

        Vector3 anchorOrigin = anchorTransform.position;
        if (terrainLayer.value == 0) Debug.LogWarning("Unity6ForestRenderer: terrainLayer is default (Everything). Consider using a dedicated terrain layer.");

        int writeIndex = 0;
        for (int c = 0; c < finalClusterCount; c++)
        {
            Vector2 clusterCenter = clusterCenters[c];
            int countForCluster = perClusterCounts[c];
            float thisClusterRadius = perClusterRadii[c];

            for (int k = 0; k < countForCluster && writeIndex < totalTreeCount; k++)
            {
                bool placed = false;
                // try up to maxPlacementAttempts for a valid spot (raycast hit + slope check)
                for (int attempt = 0; attempt < Mathf.Max(1, maxPlacementAttempts); attempt++)
                {
                    float r = thisClusterRadius * Mathf.Sqrt(Random.value);
                    float a = Random.value * Mathf.PI * 2f;
                    float rx = clusterCenter.x + r * Mathf.Cos(a);
                    float rz = clusterCenter.y + r * Mathf.Sin(a);

                    rx = Mathf.Clamp(rx, -forestSize.x * 0.5f, forestSize.x * 0.5f);
                    rz = Mathf.Clamp(rz, -forestSize.y * 0.5f, forestSize.y * 0.5f);

                    float worldX = anchorOrigin.x + rx;
                    float worldZ = anchorOrigin.z + rz;

                    float posY = anchorOrigin.y;
                    Vector3 rayStart = new Vector3(worldX, anchorOrigin.y + raycastStartHeight, worldZ);
                    if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, raycastStartHeight * 2f, terrainLayer))
                    {
                        // slope check
                        if (avoidSteepSlopes)
                        {
                            float angle = Vector3.Angle(hit.normal, Vector3.up);
                            if (angle > maxSlopeDegrees)
                            {
                                // too steep, try another sample
                                continue;
                            }
                        }

                        posY = hit.point.y;

                        // accepted placement
                        Vector3 worldPos = new Vector3(worldX, posY, worldZ);
                        tmpLocalPositions[writeIndex] = anchorTransform.InverseTransformPoint(worldPos);
                        tmpRotations[writeIndex] = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

                        float baseScale = Random.Range(minTreeScale, maxTreeScale);
                        float jitter = Random.Range(1f - sizeJitter, 1f + sizeJitter);
                        float sx = baseScale * jitter * Random.Range(1f - axisJitter, 1f + axisJitter);
                        float sy = baseScale * jitter * Random.Range(1f - axisJitter, 1f + axisJitter);
                        float sz = baseScale * jitter * Random.Range(1f - axisJitter, 1f + axisJitter);
                        tmpScales[writeIndex] = new Vector3(sx, sy, sz);

                        tmpMaterialIndex[writeIndex] = Random.Range(0, materials.Length);

                        tmpMatrices[writeIndex] = anchorTransform.localToWorldMatrix * Matrix4x4.TRS(tmpLocalPositions[writeIndex], tmpRotations[writeIndex], tmpScales[writeIndex]);

                        placed = true;
                        writeIndex++;
                        break; // placed this instance
                    }
                    else
                    {
                        // raycast missed; try again (or will fail after attempts)
                        continue;
                    }
                } // attempts

                if (!placed)
                {
                    // Give up placing this instance (skip). Don't increment writeIndex, move to next.
                    // Optionally could log low-volume info
                    if (writeIndex < 6) Debug.Log($"Unity6ForestRenderer: skipped a placement after {maxPlacementAttempts} attempts (cluster {c}).");
                    // continue to next requested sample
                }
            } // per-cluster
        } // clusters

        int placedCount = writeIndex;
        if (placedCount == 0)
        {
            Debug.LogError("Unity6ForestRenderer: no instances were placed. Check raycast layer, MeshColliders, and placement parameters.");
            return;
        }

        if (placedCount < totalTreeCount)
        {
            Debug.LogWarning($"Unity6ForestRenderer: requested {totalTreeCount} placements, but only {placedCount} were placed after slope/miss filtering and max attempts. Trimming arrays.");
        }

        // Trim arrays to actual placedCount
        fullMatrices = new Matrix4x4[placedCount];
        localPositions = new Vector3[placedCount];
        rotations = new Quaternion[placedCount];
        scales = new Vector3[placedCount];
        instanceMaterialIndex = new int[placedCount];

        for (int i = 0; i < placedCount; i++)
        {
            fullMatrices[i] = tmpMatrices[i];
            localPositions[i] = tmpLocalPositions[i];
            rotations[i] = tmpRotations[i];
            scales[i] = tmpScales[i];
            instanceMaterialIndex[i] = tmpMaterialIndex[i];
        }

        totalTreeCount = placedCount; // update to actual placed

        // Create per-material visible buffers and args buffers sized to placedCount
        visibleTransformBuffers = new GraphicsBuffer[materials.Length];
        argsBuffers = new GraphicsBuffer[materials.Length];
        argsArrays = new uint[materials.Length][];

        const int matrixStride = 16 * 4;
        for (int m = 0; m < materials.Length; m++)
        {
            visibleTransformBuffers[m] = new GraphicsBuffer(GraphicsBuffer.Target.Structured, totalTreeCount, matrixStride);
            argsBuffers[m] = new GraphicsBuffer(GraphicsBuffer.Target.IndirectArguments, 1, 5 * sizeof(uint));
            argsArrays[m] = new uint[5] { 0u, 0u, 0u, 0u, 0u };

            argsArrays[m][0] = (uint)treeMesh.GetIndexCount(0);
            argsArrays[m][1] = 0u;
            argsArrays[m][2] = (uint)treeMesh.GetIndexStart(0);
            argsArrays[m][3] = (uint)treeMesh.GetBaseVertex(0);
            argsArrays[m][4] = 0u;
            argsBuffers[m].SetData(argsArrays[m]);
        }

        tempVisibleMatricesPerMaterial = new Matrix4x4[materials.Length][];
        visibleCountsPerMaterial = new int[materials.Length];
        for (int m = 0; m < materials.Length; m++) tempVisibleMatricesPerMaterial[m] = new Matrix4x4[totalTreeCount];

        // initial culling
        sqrMaxDisplayDistance = maxDisplayDistance * maxDisplayDistance;
        PerformCullingImmediate();

        // set initial shader world offset for each material
        if (useShaderWorldOffset)
        {
            foreach (var mat in materials) if (mat != null) mat.SetVector("_WorldOffset", Vector4.zero);
        }

        Debug.Log($"Unity6ForestRenderer initialized: requested={tmpMatrices.Length}, placed={totalTreeCount}, materials={materials.Length}");
    }

    // Create grid-based cluster centers with jitter
    private List<Vector2> CreateClusterCenters(int desiredCount)
    {
        List<Vector2> centers = new List<Vector2>(desiredCount);

        int cols = Mathf.CeilToInt(Mathf.Sqrt(desiredCount));
        int rows = Mathf.CeilToInt((float)desiredCount / cols);
        float stepX = forestSize.x / cols;
        float stepZ = forestSize.y / rows;
        float halfX = forestSize.x * 0.5f;
        float halfZ = forestSize.y * 0.5f;

        for (int r = 0, idx = 0; r < rows && idx < desiredCount; r++)
        {
            for (int c = 0; c < cols && idx < desiredCount; c++, idx++)
            {
                float baseX = -halfX + (c + 0.5f) * stepX;
                float baseZ = -halfZ + (r + 0.5f) * stepZ;

                float jx = (Random.value * 2f - 1f) * stepX * clusterJitter * 0.5f;
                float jz = (Random.value * 2f - 1f) * stepZ * clusterJitter * 0.5f;
                centers.Add(new Vector2(baseX + jx, baseZ + jz));
            }
        }

        return centers;
    }

    void Update()
    {
        // safety
        if (visibleTransformBuffers == null || argsBuffers == null) return;

        timeSinceLastCull += Time.deltaTime;
        bool shouldCull = false;

        if (playerTransform != null)
        {
            if (lastPlayerPos == Vector3.positiveInfinity)
            {
                lastPlayerPos = playerTransform.position;
                shouldCull = true;
            }
            else
            {
                float sqrMove = (playerTransform.position - lastPlayerPos).sqrMagnitude;
                if (sqrMove >= playerMoveThreshold * playerMoveThreshold)
                {
                    shouldCull = true;
                    lastPlayerPos = playerTransform.position;
                }
            }
        }
        else
        {
            if (timeSinceLastCull >= cullingUpdateInterval) shouldCull = true;
        }

        if (timeSinceLastCull >= cullingUpdateInterval) shouldCull = true;

        if (shouldCull)
        {
            PerformCullingImmediate();
            timeSinceLastCull = 0f;
        }

        if (!useHugeDebugBounds)
        {
            generationBounds.center = anchorTransform != null ? anchorTransform.position + worldOffset : baseForestCenter + worldOffset;
            generationBounds.extents = new Vector3(forestSize.x * 0.5f, 500f, forestSize.y * 0.5f);
        }

        // Draw per material
        for (int m = 0; m < materials.Length; m++)
        {
            if (materials[m] == null) continue;
            uint instanceCount = argsArrays[m][1];
            if (instanceCount == 0) continue;

            materials[m].SetBuffer("_TransformBuffer", visibleTransformBuffers[m]);

            Graphics.DrawMeshInstancedIndirect(
                treeMesh,
                0,
                materials[m],
                generationBounds,
                argsBuffers[m]
            );
        }
    }

    // Fill per-material visible buffers and update argsBuffers
    private void PerformCullingImmediate()
    {
        Vector3 playerPos = (playerTransform != null) ? playerTransform.position : (anchorTransform != null ? anchorTransform.position : Vector3.zero);

        // reset counters
        for (int m = 0; m < materials.Length; m++)
            visibleCountsPerMaterial[m] = 0;

        int dstTotal = 0;
        float sqrMax = sqrMaxDisplayDistance;

        for (int i = 0; i < totalTreeCount; i++)
        {
            Vector3 worldPos = anchorTransform != null ? anchorTransform.TransformPoint(localPositions[i]) : localPositions[i];
            Vector3 effectiveWorldPos = worldPos + worldOffset;

            if ((effectiveWorldPos - playerPos).sqrMagnitude <= sqrMax)
            {
                int matIdx = Mathf.Clamp(instanceMaterialIndex[i], 0, materials.Length - 1);
                int dst = visibleCountsPerMaterial[matIdx];
                tempVisibleMatricesPerMaterial[matIdx][dst] = Matrix4x4.TRS(worldPos, rotations[i], scales[i]);
                visibleCountsPerMaterial[matIdx] = dst + 1;
                dstTotal++;
            }
        }

        visibleTotalCount = dstTotal;

        // Upload per-material visible matrices and update args
        for (int m = 0; m < materials.Length; m++)
        {
            int count = visibleCountsPerMaterial[m];
            if (count > 0)
            {
                visibleTransformBuffers[m].SetData(tempVisibleMatricesPerMaterial[m], 0, 0, count);
            }
            argsArrays[m][1] = (uint)count;
            argsBuffers[m].SetData(argsArrays[m]);

            if (materials[m] != null)
                materials[m].SetBuffer("_TransformBuffer", visibleTransformBuffers[m]);
        }
    }

    // Public: call when floating-origin system shifts the world by 'shift' (scene moved by +shift)
    public void ApplyOriginShift(Vector3 shift)
    {
        if (useShaderWorldOffset)
        {
            worldOffset -= shift;
            foreach (var mat in materials) if (mat != null) mat.SetVector("_WorldOffset", new Vector4(worldOffset.x, worldOffset.y, worldOffset.z, 0f));
        }
        else
        {
            for (int i = 0; i < totalTreeCount; i++)
            {
                Vector3 worldPos = anchorTransform.TransformPoint(localPositions[i]);
                worldPos -= shift;
                localPositions[i] = anchorTransform.InverseTransformPoint(worldPos);
                fullMatrices[i] = anchorTransform.localToWorldMatrix * Matrix4x4.TRS(localPositions[i], rotations[i], scales[i]);
            }
            // After changing localPositions/fullMatrices we should re-run culling
            PerformCullingImmediate();
        }

        generationBounds.center -= shift;
    }

    public void RebuildFromAnchor()
    {
        for (int i = 0; i < totalTreeCount; i++)
            fullMatrices[i] = anchorTransform.localToWorldMatrix * Matrix4x4.TRS(localPositions[i], rotations[i], scales[i]);

        PerformCullingImmediate();
    }

    void OnDisable() => ReleaseBuffers();
    void OnDestroy() => ReleaseBuffers();

    private void ReleaseBuffers()
    {
        if (visibleTransformBuffers != null)
        {
            foreach (var b in visibleTransformBuffers) if (b != null) b.Release();
            visibleTransformBuffers = null;
        }
        if (argsBuffers != null)
        {
            foreach (var b in argsBuffers) if (b != null) b.Release();
            argsBuffers = null;
        }
    }
}