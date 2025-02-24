using UnityEngine;
using UnityEditor;

public class TileGameObjectsEditor : EditorWindow
{
    private GameObject[] objectsToTile = new GameObject[0]; // Array of game objects to be tiled
    private int rows = 5; // Number of rows to tile
    private int columns = 5; // Number of columns to tile
    private float spacingX = 1.0f; // Spacing between objects on the X axis
    private float spacingZ = 1.0f; // Spacing between objects on the Y axis

    [UnityEditor.MenuItem("Tools/Tile GameObjects")]
    public static void ShowWindow()
    {
        GetWindow<TileGameObjectsEditor>("Tile GameObjects");
    }

    void OnGUI()
    {
        GUILayout.Label("Tile GameObjects Settings", EditorStyles.boldLabel);

        // Handle the array size
        int newSize = EditorGUILayout.IntField("Size", objectsToTile.Length);
        if (newSize != objectsToTile.Length)
        {
            GameObject[] newArray = new GameObject[newSize];
            for (int i = 0; i < newSize && i < objectsToTile.Length; i++)
            {
                newArray[i] = objectsToTile[i];
            }
            objectsToTile = newArray;
        }

        // Display fields for each array element
        for (int i = 0; i < objectsToTile.Length; i++)
        {
            objectsToTile[i] = (GameObject)EditorGUILayout.ObjectField($"Object {i}", objectsToTile[i], typeof(GameObject), true);
        }

        rows = EditorGUILayout.IntField("Rows", rows);
        columns = EditorGUILayout.IntField("Columns", columns);
        spacingX = EditorGUILayout.FloatField("Spacing X", spacingX);
        spacingZ = EditorGUILayout.FloatField("Spacing Y", spacingZ);

        if (GUILayout.Button("Tile GameObjects"))
        {
            TileObjects();
        }
    }

    void TileObjects()
    {
        if (objectsToTile == null || objectsToTile.Length == 0)
        {
            Debug.LogError("No objects assigned for tiling.");
            return;
        }

        GameObject parentObject = new GameObject("Tiled Objects");

        int i = 0;

        for (int z = 0; z < rows; z++)
        {
            for (int x = 0; x < columns; x++)
            {

                // Calculate the position for the new object
                Vector3 position = new Vector3(x * spacingX, 0, z * spacingZ);

                // Instantiate a new object from the array
                GameObject obj = GameObject.Instantiate(objectsToTile[i]) as GameObject;

                obj.transform.position = position;

                // Set the new object as a child of the parent object
                obj.transform.parent = parentObject.transform;

                if (i >= objectsToTile.Length)
                {
                    i = 0;
                }
            }
        }
    }
}