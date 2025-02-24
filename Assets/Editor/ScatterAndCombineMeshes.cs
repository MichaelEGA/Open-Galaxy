using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ObjectScatter : EditorWindow
{
    private GameObject objectToScatter;
    private GameObject targetObject;
    private int scatterCount = 10;
    private int numberInClump = 30;
    private float clumpRadius = 100f;
    private float maxSlopeAngle = 30.0f;
    private float objectsPlaced = 0;
    private float attemptedToPlaceObject = 0;
    private int seed = 0;
    private bool randomiseScale = false;
    private float scaleMax = 150;
    private float scaleMin = 50;
    private bool randomiseRotation = false;
    private bool useLimit = false;
    private int limit = 0;

    [UnityEditor.MenuItem("Tools/Object Scatter")]
    public static void ShowWindow()
    {
        GetWindow<ObjectScatter>("Object Scatter");
    }

    void OnGUI()
    {
        GUILayout.Label("Object Scatter", EditorStyles.boldLabel);

        objectToScatter = (GameObject)EditorGUILayout.ObjectField("Object to Scatter", objectToScatter, typeof(GameObject), true);
        targetObject = (GameObject)EditorGUILayout.ObjectField("Target Object", targetObject, typeof(GameObject), true);
        scatterCount = EditorGUILayout.IntField("Scatter Count", scatterCount);
        numberInClump = EditorGUILayout.IntField("Number In Count", numberInClump);
        clumpRadius = EditorGUILayout.FloatField("Clump Radius", clumpRadius);
        maxSlopeAngle = EditorGUILayout.FloatField("Max Slope Angle", maxSlopeAngle);
        randomiseScale = EditorGUILayout.Toggle("Randomise Scale", randomiseScale);
        scaleMax = EditorGUILayout.FloatField("Scale Maximum", scaleMax);
        scaleMin = EditorGUILayout.FloatField("Scale Minimum", scaleMin);
        randomiseRotation = EditorGUILayout.Toggle("Randomise Rotation", randomiseRotation);
        useLimit = EditorGUILayout.Toggle("Use Limit", useLimit);
        limit = EditorGUILayout.IntField("Limit", limit);
        seed = EditorGUILayout.IntField("Seed", seed);
        

        if (GUILayout.Button("Scatter"))
        {
            ScatterAndCombine();
        }
    }

    void ScatterAndCombine()
    {
        objectsPlaced = 0;

        if (objectToScatter == null || targetObject == null)
        {
            Debug.LogError("Please assign both the object to scatter and the target object.");
            return;
        }

        if (targetObject.GetComponent<MeshCollider>() == null)
        {
            Debug.LogError("Target object must have a MeshCollider.");
            return;
        }

        MeshCollider targetCollider = targetObject.GetComponent<MeshCollider>();
        List<GameObject> scatteredObjects = new List<GameObject>();

        Random.InitState(seed);

        for (int i = 0; i < scatterCount; i++)
        {
            Vector3 clumpCenter = GetRandomPointInMesh(targetCollider);

            for (int j = 0; j < numberInClump; j++)
            {
                Vector3 randomPointInClump = clumpCenter + Random.insideUnitSphere * clumpRadius;
                randomPointInClump = GetClosestPointOnMesh(targetCollider, randomPointInClump);

                Quaternion rotation = Quaternion.identity;

                if (randomiseRotation == true)
                {
                    rotation = Quaternion.Euler(0, Random.Range(0, 180), 0);
                }

                float scale = 100;

                if (randomiseScale == true)
                {
                    scale = Random.Range(scaleMin, scaleMax);
                }

                attemptedToPlaceObject++;

                if (IsSlopeAcceptable(randomPointInClump, targetCollider))
                {
                    GameObject instance = Instantiate(objectToScatter);
                    instance.transform.localScale = (instance.transform.localScale / 100f) * scale;
                    instance.transform.position = randomPointInClump;
                    instance.transform.rotation = rotation;
                    instance.transform.SetParent(targetObject.transform);
                    scatteredObjects.Add(instance);
                    objectsPlaced++;

                    if (useLimit == true)
                    {
                        if (objectsPlaced >= limit)
                        {
                            Debug.Log("Limit of objects placed was reached in clump");
                            break;
                        }
                    }

                }
            }

            if (useLimit == true)
            {
                if (objectsPlaced >= limit)
                {
                    Debug.Log("Limit of objects placed was reached overall");
                    break;
                }
            }
        }

        CombineScatteredObjects(scatteredObjects);
    }

    Vector3 GetRandomPointInMesh(MeshCollider meshCollider)
    {
        Mesh mesh = meshCollider.sharedMesh;
        int[] triangles = mesh.triangles;
        Vector3[] vertices = mesh.vertices;

        int triangleIndex = Random.Range(0, triangles.Length / 3) * 3;
        Vector3 p0 = vertices[triangles[triangleIndex]];
        Vector3 p1 = vertices[triangles[triangleIndex + 1]];
        Vector3 p2 = vertices[triangles[triangleIndex + 2]];

        Vector3 randomPoint = RandomPointInTriangle(p0, p1, p2);
        return meshCollider.transform.TransformPoint(randomPoint);
    }

    Vector3 RandomPointInTriangle(Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = Random.value;
        float v = Random.value;

        if (u + v > 1)
        {
            u = 1 - u;
            v = 1 - v;
        }

        return p0 + u * (p1 - p0) + v * (p2 - p0);
    }

    Vector3 GetClosestPointOnMesh(MeshCollider meshCollider, Vector3 point)
    {
        Ray ray = new Ray(point + Vector3.up * 10, Vector3.down);
        RaycastHit hit;

        if (meshCollider.Raycast(ray, out hit, 100.0f))
        {
            return hit.point;
        }

        return point;
    }

    bool IsSlopeAcceptable(Vector3 point, MeshCollider meshCollider)
    {
        Ray ray = new Ray(point + Vector3.up * 10, Vector3.down);
        RaycastHit hit;

        if (meshCollider.Raycast(ray, out hit, 100.0f))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            return slopeAngle <= maxSlopeAngle;
        }

        return false;
    }

    void CombineScatteredObjects(List<GameObject> scatteredObjects)
    {
        List<MeshFilter> meshFilters = new List<MeshFilter>();
        Dictionary<Material, List<CombineInstance>> materialToCombineInstances = new Dictionary<Material, List<CombineInstance>>();

        foreach (GameObject obj in scatteredObjects)
        {
            MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
            MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();

            if (meshFilter == null || meshRenderer == null)
            {
                Debug.LogError("Scattered object is missing a MeshFilter or MeshRenderer.");
                return;
            }

            for (int subMeshIndex = 0; subMeshIndex < meshFilter.sharedMesh.subMeshCount; subMeshIndex++)
            {
                Material material = meshRenderer.sharedMaterials[subMeshIndex];
                CombineInstance combineInstance = new CombineInstance
                {
                    mesh = meshFilter.sharedMesh,
                    subMeshIndex = subMeshIndex,
                    transform = meshFilter.transform.localToWorldMatrix
                };

                if (!materialToCombineInstances.ContainsKey(material))
                {
                    materialToCombineInstances[material] = new List<CombineInstance>();
                }
                materialToCombineInstances[material].Add(combineInstance);
            }

            DestroyImmediate(obj);
        }

        GameObject combinedMeshObject = new GameObject("Combined Mesh");
        MeshFilter combinedMeshFilter = combinedMeshObject.AddComponent<MeshFilter>();
        combinedMeshFilter.sharedMesh = new Mesh();
        combinedMeshFilter.sharedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        List<CombineInstance> finalCombineInstances = new List<CombineInstance>();
        List<Material> materials = new List<Material>();

        foreach (var kvp in materialToCombineInstances)
        {
            CombineInstance[] combineArray = kvp.Value.ToArray();
            Mesh combinedSubMesh = new Mesh();
            combinedSubMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            combinedSubMesh.CombineMeshes(combineArray, true, true);

            CombineInstance finalCombineInstance = new CombineInstance
            {
                mesh = combinedSubMesh,
                subMeshIndex = 0,
                transform = Matrix4x4.identity
            };
            finalCombineInstances.Add(finalCombineInstance);
            materials.Add(kvp.Key);
        }

        combinedMeshFilter.sharedMesh.CombineMeshes(finalCombineInstances.ToArray(), false, false);

        MeshRenderer combinedMeshRenderer = combinedMeshObject.AddComponent<MeshRenderer>();
        combinedMeshRenderer.materials = materials.ToArray();

        combinedMeshObject.SetActive(true);

        Debug.Log("Number of objects succesfully placed is " + objectsPlaced + " of " + attemptedToPlaceObject + " attempted. For a vertex count of " + combinedMeshFilter.sharedMesh.vertexCount);
    }
}