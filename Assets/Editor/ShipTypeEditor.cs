using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class ShipTypeEditor : EditorWindow
{
    private ShipTypeWrapper shipTypeWrapper;
    private Vector2 scrollPosition;
    private string jsonFilePath = "Assets/Resources/files/shipTypes.json";

    [UnityEditor.MenuItem("Tools/Ship Type Editor")]
    public static void ShowWindow()
    {
        GetWindow<ShipTypeEditor>("Ship Type Editor");
    }

    private void OnEnable()
    {
        LoadJson();
    }

    private void OnGUI()
    {
        if (shipTypeWrapper == null || shipTypeWrapper.shipTypeData == null)
        {
            EditorGUILayout.HelpBox("Failed to load JSON data. Check the file path and format.", MessageType.Error);
            return;
        }

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        for (int i = 0; i < shipTypeWrapper.shipTypeData.Count; i++)
        {
            EditorGUILayout.BeginVertical("box");

            ShipTypeData ship = shipTypeWrapper.shipTypeData[i];

            ship.type = EditorGUILayout.TextField("Type", ship.type);
            ship.prefab = EditorGUILayout.TextField("Prefab", ship.prefab);
            ship.cockpitPrefab = EditorGUILayout.TextField("Cockpit Prefab", ship.cockpitPrefab);
            ship.callsign = EditorGUILayout.TextField("Callsign", ship.callsign);
            ship.scriptType = EditorGUILayout.TextField("Script Type", ship.scriptType);
            ship.accelerationRating = EditorGUILayout.FloatField("Acceleration Rating", ship.accelerationRating);
            ship.speedRating = EditorGUILayout.FloatField("Speed Rating", ship.speedRating);
            ship.maneuverabilityRating = EditorGUILayout.FloatField("Maneuverability Rating", ship.maneuverabilityRating);
            ship.hullRating = EditorGUILayout.FloatField("Hull Rating", ship.hullRating);
            ship.shieldRating = EditorGUILayout.FloatField("Shield Rating", ship.shieldRating);
            ship.systemsRating = EditorGUILayout.FloatField("Systems Rating", ship.systemsRating);
            ship.laserFireRating = EditorGUILayout.FloatField("Laser Fire Rating", ship.laserFireRating);
            ship.laserRating = EditorGUILayout.FloatField("Laser Rating", ship.laserRating);
            ship.wepRating = EditorGUILayout.FloatField("Weapon Rating", ship.wepRating);
            ship.torpedoRating = EditorGUILayout.FloatField("Torpedo Rating", ship.torpedoRating);
            ship.torpedoType = EditorGUILayout.TextField("Torpedo Type", ship.torpedoType);
            ship.laserAudio = EditorGUILayout.TextField("Laser Audio", ship.laserAudio);
            ship.engineAudio = EditorGUILayout.TextField("Engine Audio", ship.engineAudio);
            ship.thrustType = EditorGUILayout.TextField("Thrust Type", ship.thrustType);
            ship.shipClass = EditorGUILayout.TextField("Ship Class", ship.shipClass);
            ship.explosionType = EditorGUILayout.TextField("Explosion Type", ship.explosionType);
            ship.smallturret = EditorGUILayout.TextField("Small Turret", ship.smallturret);
            ship.largeturret = EditorGUILayout.TextField("Large Turret", ship.largeturret);
            ship.shipLength = EditorGUILayout.FloatField("Ship Length", ship.shipLength);

            if (GUILayout.Button("Remove"))
            {
                shipTypeWrapper.shipTypeData.RemoveAt(i);
                break;
            }

            EditorGUILayout.EndVertical();
        }

        if (GUILayout.Button("Add New Ship Type"))
        {
            shipTypeWrapper.shipTypeData.Add(new ShipTypeData());
        }

        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Save Changes"))
        {
            SaveJson();
        }
    }

    private void LoadJson()
    {
        if (File.Exists(jsonFilePath))
        {
            string json = File.ReadAllText(jsonFilePath);
            shipTypeWrapper = JsonUtility.FromJson<ShipTypeWrapper>(json);

            if (shipTypeWrapper == null || shipTypeWrapper.shipTypeData == null)
            {
                shipTypeWrapper = new ShipTypeWrapper { shipTypeData = new List<ShipTypeData>() };
            }
        }
        else
        {
            shipTypeWrapper = new ShipTypeWrapper { shipTypeData = new List<ShipTypeData>() };
        }
    }

    private void SaveJson()
    {
        // Sort the list alphabetically by the 'type' field
        shipTypeWrapper.shipTypeData.Sort((a, b) => string.Compare(a.type, b.type, System.StringComparison.OrdinalIgnoreCase));

        // Convert to JSON and save
        string json = JsonUtility.ToJson(shipTypeWrapper, true);
        File.WriteAllText(jsonFilePath, json);

        // Refresh the Asset Database
        AssetDatabase.Refresh();

        Debug.Log("JSON file saved and sorted alphabetically by 'type' at " + jsonFilePath);
    }

    [System.Serializable]
    public class ShipTypeWrapper
    {
        public List<ShipTypeData> shipTypeData;
    }

    [System.Serializable]
    public class ShipTypeData
    {
        public string type = "none";
        public string prefab = "none";
        public string cockpitPrefab = "none";
        public string callsign = "none";
        public string scriptType = "smallship";
        public float accelerationRating = 50;
        public float speedRating = 50;
        public float maneuverabilityRating = 50;
        public float hullRating = 50;
        public float shieldRating = 50;
        public float systemsRating = 50;
        public float laserFireRating = 50;
        public float laserRating = 50;
        public float wepRating = 50;
        public float torpedoRating = 50;
        public string torpedoType = "proton torpedo";
        public string laserAudio = "weapon02_xwinglaser";
        public string engineAudio = "engine02_xwing";
        public string thrustType = "thruster_red_small";
        public string shipClass = "fighter_bomber";
        public string explosionType = "default";
        public string smallturret = "none";
        public string largeturret = "none";
        public float shipLength = 10;
    }
}