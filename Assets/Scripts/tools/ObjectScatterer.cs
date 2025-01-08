using UnityEngine;
using UnityEditor;

public class ObjectScatterer : EditorWindow
{
    private GameObject objectToScatter;
    private GameObject targetSurface;
    private int numberOfObjects = 10;

    [UnityEditor.MenuItem("Tools/Scatter Objects")]
    public static void ShowWindow()
    {
        GetWindow<ObjectScatterer>("Scatter Objects");
    }

    private void OnGUI()
    {
        GUILayout.Label("Scatter Objects Across Surface", EditorStyles.boldLabel);

        objectToScatter = EditorGUILayout.ObjectField("Object to Scatter", objectToScatter, typeof(GameObject), false) as GameObject;
        targetSurface = EditorGUILayout.ObjectField("Target Surface", targetSurface, typeof(GameObject), true) as GameObject;
        numberOfObjects = EditorGUILayout.IntField("Number of Objects", numberOfObjects);

        if (GUILayout.Button("Scatter"))
        {
            ScatterObjects();
        }
    }

    private void ScatterObjects()
    {
        if (objectToScatter == null || targetSurface == null || numberOfObjects <= 0)
        {
            Debug.LogError("Please set all parameters before scattering.");
            return;
        }

        MeshCollider surfaceCollider = targetSurface.GetComponent<MeshCollider>();
        if (surfaceCollider == null)
        {
            surfaceCollider = targetSurface.AddComponent<MeshCollider>();
        }

        for (int i = 0; i < numberOfObjects; i++)
        {
            Vector3 randomPoint = GetRandomPointOnSurface(surfaceCollider);
            Instantiate(objectToScatter, randomPoint, Quaternion.identity);
        }

        DestroyImmediate(surfaceCollider);
    }

    private Vector3 GetRandomPointOnSurface(MeshCollider surfaceCollider)
    {
        Vector3 randomPoint = Vector3.zero;
        RaycastHit hit;

        while (true)
        {
            randomPoint = new Vector3(Random.Range(-0.5f, 0.5f), 0.5f, Random.Range(-0.5f, 0.5f));
            randomPoint = surfaceCollider.transform.TransformPoint(randomPoint);

            Ray ray = new Ray(randomPoint + Vector3.up * 10f, Vector3.down);
            if (surfaceCollider.Raycast(ray, out hit, 20f))
            {
                return hit.point;
            }
        }
    }
}
