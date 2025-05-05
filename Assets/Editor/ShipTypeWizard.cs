using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class ShipTypeWizard : ScriptableWizard
{
    public TextAsset jsonFile; // The JSON file to work with.
    private ShipTypeDataWrapper shipTypeDataWrapper;
    private int currentIndex = 0;
    private float manualLengthInput;

    // Fields for displaying and modifying the current ship data
    private ShipTypeData currentShip;

    [UnityEditor.MenuItem("Tools/Ship Type Wizard")]
    static void CreateWizard()
    {
        DisplayWizard<ShipTypeWizard>("Ship Type Wizard", "Close");
    }

    private void OnEnable()
    {
        if (jsonFile != null)
        {
            LoadJsonData();
        }
    }

    private void LoadJsonData()
    {
        try
        {
            var json = jsonFile.text;
            shipTypeDataWrapper = JsonUtility.FromJson<ShipTypeDataWrapper>(json);
            if (shipTypeDataWrapper != null && shipTypeDataWrapper.shipTypeData.Count > 0)
            {
                currentIndex = 0;
                currentShip = shipTypeDataWrapper.shipTypeData[currentIndex];
                manualLengthInput = currentShip.shipLength;
            }

            
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to parse JSON file: {ex.Message}");
        }
    }

    private void OnGUI()
    {
        if (shipTypeDataWrapper == null || shipTypeDataWrapper.shipTypeData.Count == 0)
        {
            EditorGUILayout.HelpBox("No valid JSON data loaded. Please assign a valid JSON file.", MessageType.Warning);
            jsonFile = (TextAsset)EditorGUILayout.ObjectField("JSON File", jsonFile, typeof(TextAsset), false);

            if (GUILayout.Button("Load JSON Data") && jsonFile != null)
            {
                LoadJsonData();
            }
            return;
        }

        // Display all fields of the current ship data
        EditorGUILayout.LabelField("Ship Type Data", EditorStyles.boldLabel);
        currentShip.type = EditorGUILayout.TextField("Type:", currentShip.type);
        currentShip.prefab = EditorGUILayout.TextField("Prefab:", currentShip.prefab);
        currentShip.cockpitPrefab = EditorGUILayout.TextField("Cockpit Prefab:", currentShip.cockpitPrefab);
        currentShip.callsign = EditorGUILayout.TextField("Callsign:", currentShip.callsign);
        currentShip.scriptType = EditorGUILayout.TextField("Script Type:", currentShip.scriptType);
        currentShip.accelerationRating = EditorGUILayout.FloatField("Acceleration Rating:", currentShip.accelerationRating);
        currentShip.speedRating = EditorGUILayout.FloatField("Speed Rating:", currentShip.speedRating);
        currentShip.maneuverabilityRating = EditorGUILayout.FloatField("Maneuverability Rating:", currentShip.maneuverabilityRating);
        currentShip.hullRating = EditorGUILayout.FloatField("Hull Rating:", currentShip.hullRating);
        currentShip.shieldRating = EditorGUILayout.FloatField("Shield Rating:", currentShip.shieldRating);
        currentShip.systemsRating = EditorGUILayout.FloatField("Systems Rating:", currentShip.systemsRating);
        currentShip.laserFireRating = EditorGUILayout.FloatField("Laser Fire Rating:", currentShip.laserFireRating);
        currentShip.laserRating = EditorGUILayout.FloatField("Laser Rating:", currentShip.laserRating);
        currentShip.wepRating = EditorGUILayout.FloatField("Weapon Rating:", currentShip.wepRating);
        currentShip.torpedoRating = EditorGUILayout.FloatField("Torpedo Rating:", currentShip.torpedoRating);
        currentShip.torpedoType = EditorGUILayout.TextField("Torpedo Type:", currentShip.torpedoType);
        currentShip.laserAudio = EditorGUILayout.TextField("Laser Audio:", currentShip.laserAudio);
        currentShip.engineAudio = EditorGUILayout.TextField("Engine Audio:", currentShip.engineAudio);
        currentShip.thrustType = EditorGUILayout.TextField("Thrust Type:", currentShip.thrustType);
        currentShip.shipClass = EditorGUILayout.TextField("Ship Class:", currentShip.shipClass);
        currentShip.explosionType = EditorGUILayout.TextField("Explosion Type:", currentShip.explosionType);
        currentShip.smallturret = EditorGUILayout.TextField("Small Turret:", currentShip.smallturret);
        currentShip.largeturret = EditorGUILayout.TextField("Large Turret:", currentShip.largeturret);
        currentShip.shipLength = EditorGUILayout.FloatField("Ship Length:", currentShip.shipLength);
        currentShip.modelauthor = EditorGUILayout.TextField("Model:", currentShip.modelauthor);
        currentShip.textureauthor = EditorGUILayout.TextField("Texture:", currentShip.textureauthor);
        currentShip.allegiance = EditorGUILayout.TextField("Allegiance:", currentShip.allegiance);
        currentShip.era = EditorGUILayout.TextField("Era:", currentShip.era);

        GUILayout.Space(5);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Previous Entry"))
        {
            DecrementIndex();
        }

        if (GUILayout.Button("Next Entry"))
        {
            IncrementIndex();
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Save Changes"))
        {
            SaveJsonData();
        }

        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        // Buttons for navigation and saving
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Largeship - Stationary - Large"))
        {
            SetScriptType("largeship");
            SetShipClass("stationary");
            SetAccelrationRating(0);
            SetSpeedRating(0);
            SetManeuverabilityRating(0);
            SetHullRating(28500);
            SetShieldRating(24000);
            SetSystemsRating(24000);
            SetLaserFireRating(60);
            SetLaserRating(50);
            SetWepRating(0);
            SetTorpedoRating(0);
        }

        if (GUILayout.Button("Largeship - Stationary - Small"))
        {
            SetScriptType("largeship");
            SetShipClass("stationary");
            SetAccelrationRating(0);
            SetSpeedRating(0);
            SetManeuverabilityRating(0);
            SetHullRating(200);
            SetShieldRating(100);
            SetSystemsRating(24000);
            SetLaserFireRating(60);
            SetLaserRating(50);
            SetWepRating(0);
            SetTorpedoRating(0);
        }

        if (GUILayout.Button("Largeship - Superlarge"))
        {
            SetScriptType("largeship");
            SetShipClass("superlarge");
            SetAccelrationRating(62);
            SetSpeedRating(45);
            SetManeuverabilityRating(10);
            SetHullRating(28500);
            SetShieldRating(24000);
            SetSystemsRating(24000);
            SetLaserFireRating(60);
            SetLaserRating(50);
            SetWepRating(0);
            SetTorpedoRating(0);
        }

        if (GUILayout.Button("Largeship - Large"))
        {
            SetScriptType("largeship");
            SetShipClass("large");
            SetAccelrationRating(62);
            SetSpeedRating(50);
            SetManeuverabilityRating(15);
            SetHullRating(5700);
            SetShieldRating(4800);
            SetSystemsRating(4800);
            SetLaserFireRating(60);
            SetLaserRating(50);
            SetWepRating(0);
            SetTorpedoRating(0);
        }

        if (GUILayout.Button("Largeship - Medium"))
        {
            SetScriptType("largeship");
            SetShipClass("medium");
            SetAccelrationRating(65);
            SetSpeedRating(55);
            SetManeuverabilityRating(17);
            SetHullRating(3800);
            SetShieldRating(2560);
            SetSystemsRating(2560);
            SetLaserFireRating(60);
            SetLaserRating(50);
            SetWepRating(0);
            SetTorpedoRating(0);
        }

        if (GUILayout.Button("Largeship - Small"))
        {
            SetScriptType("largeship");
            SetShipClass("small");
            SetAccelrationRating(67);
            SetSpeedRating(60);
            SetManeuverabilityRating(20);
            SetHullRating(1900);
            SetShieldRating(1600);
            SetSystemsRating(1600);
            SetLaserFireRating(60);
            SetLaserRating(50);
            SetWepRating(0);
            SetTorpedoRating(0);
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Smallship - Large"))
        {
            SetScriptType("smallship");
            SetShipClass("large");
        }

        if (GUILayout.Button("Smallship - Medium"))
        {
            SetScriptType("smallship");
            SetShipClass("medium");
        }

        if (GUILayout.Button("Smallship - Small"))
        {
            SetScriptType("smallship");
            SetShipClass("small");
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Smallship - VeryFast"))
        {
            SetScriptType("smallship");
            SetAccelrationRating(74);
            SetSpeedRating(160);
            SetManeuverabilityRating(80);
            SetHullRating(50);
            SetShieldRating(50);
            SetSystemsRating(50);
            SetLaserFireRating(85);
            SetLaserRating(50);
            SetWepRating(35);
        }

        if (GUILayout.Button("Smallship - Fast"))
        {
            SetScriptType("smallship");
            SetAccelrationRating(62);
            SetSpeedRating(145);
            SetManeuverabilityRating(75);
            SetHullRating(120);
            SetShieldRating(80);
            SetSystemsRating(100);
            SetLaserFireRating(83);
            SetLaserRating(50);
            SetWepRating(30);
        }

        if (GUILayout.Button("Smallship - Medium"))
        {
            SetScriptType("smallship");
            SetAccelrationRating(62);
            SetSpeedRating(110);
            SetManeuverabilityRating(70);
            SetHullRating(120);
            SetShieldRating(120);
            SetSystemsRating(100);
            SetLaserFireRating(80);
            SetLaserRating(50);
            SetWepRating(25);
        }

        if (GUILayout.Button("Smallship - Slow"))
        {
            SetScriptType("smallship");
            SetAccelrationRating(62);
            SetSpeedRating(102);
            SetManeuverabilityRating(54);
            SetHullRating(95);
            SetShieldRating(160);
            SetSystemsRating(100);
            SetLaserFireRating(80);
            SetLaserRating(50);
            SetWepRating(20);
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Torpedos - 0"))
        {
            SetTorpedoRating(0);
        }

        if (GUILayout.Button("Torpedos - 6"))
        {
            SetTorpedoRating(6);
        }

        if (GUILayout.Button("Torpedos - 12"))
        {
            SetTorpedoRating(12);
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("ProtonTorpedos"))
        {
            SetTorpedoType("proton torpedo");
        }

        if (GUILayout.Button("ConcussionMissiles"))
        {
            SetTorpedoType("concussion missile");
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("ImperialLaserSound"))
        {
            SetLaserAudio("weapon01_tiefighterlaser");
        }

        if (GUILayout.Button("RebelLasersSound"))
        {
            SetLaserAudio("weapon02_xwinglaser");
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("TieFighterEngine"))
        {
            SetEngineAudio("engine01_tiefighter");
        }

        if (GUILayout.Button("XWingEngine"))
        {
            SetEngineAudio("engine02_xwing");
        }

        if (GUILayout.Button("ShuttleEngine"))
        {
            SetEngineAudio("engine03_lambda");
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("allegiance: old rep"))
        {
            SetAllegiance("oldrepublic");
        }

        if (GUILayout.Button("sith emp"))
        {
            SetAllegiance("kraytempire");
        }

        if (GUILayout.Button("republic"))
        {
            SetAllegiance("republic");
        }

        if (GUILayout.Button("empire"))
        {
            SetAllegiance("empire");
        }

        if (GUILayout.Button("rebel"))
        {
            SetAllegiance("rebel");
        }

        if (GUILayout.Button("blacksun"))
        {
            SetAllegiance("blacksun");
        }

        if (GUILayout.Button("pirate"))
        {
            SetAllegiance("pirate");
        }

        if (GUILayout.Button("gal all"))
        {
            SetAllegiance("galacticalliance");
        }

        if (GUILayout.Button("fel empire"))
        {
            SetAllegiance("felempire");
        }

        if (GUILayout.Button("kra empire"))
        {
            SetAllegiance("kraytempire");
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("era: old republic"))
        {
            SetEra("oldrepublic");
        }

        if (GUILayout.Button("high republic"))
        {
            SetEra("highrepublic");
        }

        if (GUILayout.Button("republic"))
        {
            SetEra("republic");
        }

        if (GUILayout.Button("empire"))
        {
            SetEra("empire");
        }

        if (GUILayout.Button("new rep"))
        {
            SetEra("newrepublic");
        }

        if (GUILayout.Button("gal all"))
        {
            SetEra("galacticalliance");
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("wn"))
        {
            SetModelAndTextureAuthor("warb_null");
        }

        if (GUILayout.Button("ej"))
        {
            SetModelAndTextureAuthor("evillejedi");
        }

        if (GUILayout.Button("fs"))
        {
            SetModelAndTextureAuthor("firststrike");
        }

        if (GUILayout.Button("gc"))
        {
            SetModelAndTextureAuthor("galaticconquest");
        }

        if (GUILayout.Button("at"))
        {
            SetModelAndTextureAuthor("ArvisTaljiks");
        }

        if (GUILayout.Button("df"))
        {
            SetModelAndTextureAuthor("doclusifer2");
        }

        if (GUILayout.Button("jr"))
        {
            SetModelAndTextureAuthor("Jeroenimo");
        }

        if (GUILayout.Button("spw"))
        {
            SetModelAndTextureAuthor("spylakewalker");
        }

        GUILayout.EndHorizontal();
    }

    private void SetAccelrationRating(float rating)
    {
        currentShip.accelerationRating = rating;
    }

    private void SetSpeedRating(float rating)
    {
        currentShip.speedRating = rating;
    }

    private void SetManeuverabilityRating(float rating)
    {
        currentShip.maneuverabilityRating = rating;
    }

    private void SetHullRating(float rating)
    {
        currentShip.hullRating = rating;
    }

    private void SetSystemsRating(float rating)
    {
        currentShip.systemsRating = rating;
    }

    private void SetLaserFireRating(float rating)
    {
        currentShip.laserFireRating = rating;
    }

    private void SetLaserRating(float rating)
    {
        currentShip.laserRating = rating;
    }

    private void SetShieldRating(float rating)
    {
        currentShip.shieldRating = rating;
    }

    private void SetWepRating(float rating)
    {
        currentShip.wepRating = rating;
    }

    private void SetTorpedoRating(float rating)
    {
        currentShip.torpedoRating = rating;
    }

    private void SetScriptType(string shipType)
    {
        currentShip.scriptType = shipType;
    }

    private void SetShipClass(string shipClass)
    {
        currentShip.shipClass = shipClass;
    }

    private void SetModelAndTextureAuthor(string author)
    {
        currentShip.modelauthor = author;
        currentShip.textureauthor = author;
    }

    private void SetLaserAudio(string audio)
    {
        currentShip.laserAudio = audio;
    }

    private void SetEngineAudio(string audio)
    {
        currentShip.engineAudio = audio;
    }

    private void SetAllegiance(string allegiance)
    {
        currentShip.allegiance = allegiance;
    }

    private void SetEra(string era)
    {
        currentShip.era = era;
    }

    private void SetTorpedoType(string torpedo)
    {
        currentShip.torpedoType = torpedo;
    }

    private void IncrementIndex()
    {
        currentIndex = (currentIndex + 1) % shipTypeDataWrapper.shipTypeData.Count;
        currentShip = shipTypeDataWrapper.shipTypeData[currentIndex];
    }

    private void DecrementIndex()
    {
        currentIndex = (currentIndex - 1 + shipTypeDataWrapper.shipTypeData.Count) % shipTypeDataWrapper.shipTypeData.Count;
        currentShip = shipTypeDataWrapper.shipTypeData[currentIndex];
    }

    private void SaveJsonData()
    {
        try
        {
            var json = JsonUtility.ToJson(shipTypeDataWrapper, true); // Pretty print for readability
            var path = AssetDatabase.GetAssetPath(jsonFile);
            File.WriteAllText(path, json);
            AssetDatabase.Refresh();

            Debug.Log("JSON data saved successfully.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to save JSON data: {ex.Message}");
        }
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
        public string modelauthor;
        public string textureauthor;
        public string allegiance;
        public string era;
    }

    [System.Serializable]
    public class ShipTypeDataWrapper
    {
        public List<ShipTypeData> shipTypeData = new List<ShipTypeData>();
    }
}