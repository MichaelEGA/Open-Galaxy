using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class ShipData
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
    public string modelauthor;
    public string textureauthor;
    public string allegiance;
    public string era;
}

[System.Serializable]
public class ShipTypeData
{
    public List<ShipData> shipTypeData;
}

public class UpdateCallsignsEditor : EditorWindow
{
    private TextAsset jsonFile;
    private ShipTypeData shipTypeData;
    private string outputPath = "UpdatedShipData.json";

    [UnityEditor.MenuItem("Tools/Update Callsigns")]
    public static void ShowWindow()
    {
        GetWindow<UpdateCallsignsEditor>("Update Callsigns");
    }

    private void OnGUI()
    {
        GUILayout.Label("Update Ship Callsigns", EditorStyles.boldLabel);

        jsonFile = (TextAsset)EditorGUILayout.ObjectField("JSON File", jsonFile, typeof(TextAsset), false);

        if (GUILayout.Button("Load JSON"))
        {
            LoadJson();
        }

        if (shipTypeData != null && GUILayout.Button("Update Callsigns"))
        {
            UpdateCallsigns();
        }

        if (shipTypeData != null && GUILayout.Button("Save Updated JSON"))
        {
            SaveJson();
        }
    }

    private void LoadJson()
    {
        if (jsonFile != null)
        {
            shipTypeData = JsonUtility.FromJson<ShipTypeData>(jsonFile.text);
            Debug.Log("JSON loaded successfully.");
        }
        else
        {
            Debug.LogError("No JSON file selected!");
        }
    }

    private void UpdateCallsigns()
    {
        foreach (var ship in shipTypeData.shipTypeData)
        {
            if (!string.IsNullOrEmpty(ship.type) && ship.type.Length >= 3)
            {
                ship.callsign = ship.type.Substring(0, 3);
            }
            else
            {
                Debug.LogWarning($"Ship type is either null or too short: {ship.type}");
            }
        }
        Debug.Log("Callsigns updated successfully.");
    }

    private void SaveJson()
    {
        string json = JsonUtility.ToJson(shipTypeData, true);
        string path = EditorUtility.SaveFilePanel("Save Updated JSON", "", outputPath, "json");
        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllText(path, json);
            Debug.Log($"Updated JSON saved to: {path}");
        }
    }
}
