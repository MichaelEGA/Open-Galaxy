using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class ShipTypeJsonGenerator : EditorWindow
{
    private Object[] selectedPrefabs;
    private string jsonFilePath = "Assets/shipTypes.json";

    [UnityEditor.MenuItem("Tools/Generate Ship Types JSON")]
    public static void ShowWindow()
    {
        GetWindow<ShipTypeJsonGenerator>("Generate Ship Types JSON");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Select Prefabs", EditorStyles.boldLabel);

        if (GUILayout.Button("Select Prefabs"))
        {
            selectedPrefabs = Selection.objects;
            Debug.Log($"{selectedPrefabs.Length} prefabs selected.");
        }

        if (GUILayout.Button("Generate JSON File"))
        {
            GenerateJson();
        }
    }

    private void GenerateJson()
    {
        if (selectedPrefabs == null || selectedPrefabs.Length == 0)
        {
            Debug.LogError("No prefabs selected. Please select prefabs to generate the JSON file.");
            return;
        }

        List<ShipTypeData> shipTypeDataList = new List<ShipTypeData>();

        foreach (Object obj in selectedPrefabs)
        {
            if (PrefabUtility.IsPartOfPrefabAsset(obj))
            {
                string prefabName = obj.name;

                // Extract the 'type' by removing the first word and underscore
                string[] nameParts = prefabName.Split('_');
                string typeName = nameParts.Length > 1 ? prefabName.Substring(prefabName.IndexOf('_') + 1) : prefabName;

                // Create a new ShipTypeData object and populate the fields
                ShipTypeData shipTypeData = new ShipTypeData
                {
                    type = typeName,
                    prefab = prefabName,
                    cockpitPrefab = "none",
                    callsign = "none",
                    scriptType = "smallship",
                    accelerationRating = 50,
                    speedRating = 50,
                    maneuverabilityRating = 50,
                    hullRating = 50,
                    shieldRating = 50,
                    systemsRating = 50,
                    laserFireRating = 50,
                    laserRating = 50,
                    wepRating = 50,
                    torpedoRating = 50,
                    torpedoType = "proton torpedo",
                    laserAudio = "weapon02_xwinglaser",
                    engineAudio = "engine02_xwing",
                    thrustType = "none",
                    shipClass = "fighter",
                    explosionType = "default",
                    smallturret = "none",
                    largeturret = "none",
                    shipLength = 10.0f
                };

                shipTypeDataList.Add(shipTypeData);
            }
        }

        // Wrap the list in a ShipTypeWrapper
        ShipTypeWrapper wrapper = new ShipTypeWrapper { shipTypeData = shipTypeDataList };

        // Serialize to JSON
        string jsonContent = JsonUtility.ToJson(wrapper, true);

        // Ensure the directory exists
        string directoryPath = Path.GetDirectoryName(jsonFilePath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Write to the file
        File.WriteAllText(jsonFilePath, jsonContent);

        Debug.Log($"JSON file generated at {jsonFilePath}");
        AssetDatabase.Refresh();
    }

    [System.Serializable]
    public class ShipTypeWrapper
    {
        public List<ShipTypeData> shipTypeData;
    }

    [System.Serializable]
    public class ShipTypeData
    {
        public string type;
        public string prefab;
        public string cockpitPrefab;
        public string callsign;
        public string scriptType;
        public float accelerationRating;
        public float speedRating;
        public float maneuverabilityRating;
        public float hullRating;
        public float shieldRating;
        public float systemsRating;
        public float laserFireRating;
        public float laserRating;
        public float wepRating;
        public float torpedoRating;
        public string torpedoType;
        public string laserAudio;
        public string engineAudio;
        public string thrustType;
        public string shipClass;
        public string explosionType;
        public string smallturret;
        public string largeturret;
        public float shipLength;
    }
}