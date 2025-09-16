using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class ShipTypeEditor : ScriptableWizard
{
    public TextAsset jsonFile; // The JSON file to work with.
    private ShipTypeDataWrapper shipTypeDataWrapper;
    private int currentIndex = 0;
    private float manualLengthInput;
    private string searchforship;
    private Vector2 scrollPosition;
    private GameObject selectedPrefab; // The prefab selected by the user
    private Texture2D previewTexture;

    // Fields for displaying and modifying the current ship data
    private ShipTypeData currentShip;

    [UnityEditor.MenuItem("Tools/Ship Type Editor")]
    static void CreateEditor()
    {
        DisplayWizard<ShipTypeEditor>("Ship Type Editor", "Close");
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
        else
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Width(position.width), GUILayout.Height(position.height));

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();

            searchforship = EditorGUILayout.TextField("Search for Ship:", searchforship);

            if (GUILayout.Button("Search"))
            {
                SearchForEntry();
            }

            GUILayout.EndHorizontal();

            selectedPrefab = Resources.Load<GameObject>("objects/ships/" + currentShip.prefab);

            //This adds a new ship entry
            if (GUILayout.Button("Add New Ship Type"))
            {
                shipTypeDataWrapper.shipTypeData.Add(new ShipTypeData());

                currentIndex = shipTypeDataWrapper.shipTypeData.Count - 1;

                currentShip = shipTypeDataWrapper.shipTypeData[currentIndex];
            }

            // Display the preview if a prefab is selected
            if (selectedPrefab != null)
            {
                // Get the preview texture
                previewTexture = AssetPreview.GetAssetPreview(selectedPrefab);

                if (previewTexture != null)
                {
                    // Display the preview texture
                    GUILayout.Label(previewTexture, GUILayout.Width(previewTexture.width), GUILayout.Height(previewTexture.height));
                }
                else
                {
                    EditorGUILayout.HelpBox("Preview is not available for this asset.", MessageType.Info);
                }
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
            currentShip.smallturret = EditorGUILayout.FloatField("Small Turret:", currentShip.smallturret);
            currentShip.largeturret = EditorGUILayout.FloatField("Large Turret:", currentShip.largeturret);
            currentShip.shipLength = EditorGUILayout.FloatField("Ship Length:", currentShip.shipLength);
            currentShip.shieldType = EditorGUILayout.TextField("Shield Type:", currentShip.shieldType);
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

            if (GUILayout.Button("Remove"))
            {
                shipTypeDataWrapper.shipTypeData.RemoveAt(currentIndex);
            }

            GUILayout.EndHorizontal();

            // Buttons for navigation and saving
            EditorGUILayout.LabelField("Largeship", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Stationary - Large"))
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

            if (GUILayout.Button("Stationary - Small"))
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

            if (GUILayout.Button("Superlarge"))
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

            if (GUILayout.Button("Large"))
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

            if (GUILayout.Button("Medium"))
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

            if (GUILayout.Button("Small"))
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

            EditorGUILayout.LabelField("Smallship", EditorStyles.boldLabel);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Large"))
            {
                SetScriptType("smallship");
                SetShipClass("large");
            }

            if (GUILayout.Button("Medium"))
            {
                SetScriptType("smallship");
                SetShipClass("medium");
            }

            if (GUILayout.Button("Small"))
            {
                SetScriptType("smallship");
                SetShipClass("small");
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("VeryFast"))
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

            if (GUILayout.Button("Fast"))
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

            if (GUILayout.Button("Medium"))
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

            if (GUILayout.Button("Slow"))
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

            if (GUILayout.Button("No Torpedos"))
            {
                SetTorpedoRating(0);
            }

            if (GUILayout.Button("Protontorpedos - 6"))
            {
                SetTorpedoRating(6);
                SetTorpedoType("proton torpedo");
            }

            if (GUILayout.Button("Protontorpedos - 12"))
            {
                SetTorpedoRating(12);
                SetTorpedoType("proton torpedo");
            }

            if (GUILayout.Button("Concussion Missile - 6"))
            {
                SetTorpedoRating(6);
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

            EditorGUILayout.LabelField("Allegiance", EditorStyles.boldLabel);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("old rep"))
            {
                SetAllegiance("oldrepublic");
            }

            if (GUILayout.Button("sith emp"))
            {
                SetAllegiance("sithempire");
            }

            if (GUILayout.Button("mando"))
            {
                SetAllegiance("mandalorian");
            }

            if (GUILayout.Button("rep"))
            {
                SetAllegiance("republic");
            }

            if (GUILayout.Button("cis"))
            {
                SetAllegiance("cis");
            }

            if (GUILayout.Button("emp"))
            {
                SetAllegiance("empire");
            }

            if (GUILayout.Button("rebel"))
            {
                SetAllegiance("rebel");
            }

            if (GUILayout.Button("bsun"))
            {
                SetAllegiance("blacksun");
            }

            if (GUILayout.Button("pirate"))
            {
                SetAllegiance("pirate");
            }

            if (GUILayout.Button("vong"))
            {
                SetAllegiance("vong");
            }

            if (GUILayout.Button("fo"))
            {
                SetAllegiance("firstorder");
            }

            if (GUILayout.Button("galall"))
            {
                SetAllegiance("galacticalliance");
            }

            if (GUILayout.Button("fel emp"))
            {
                SetAllegiance("felempire");
            }

            if (GUILayout.Button("kra emp"))
            {
                SetAllegiance("kraytempire");
            }

            GUILayout.EndHorizontal();

            EditorGUILayout.LabelField("Era", EditorStyles.boldLabel);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("old rep"))
            {
                SetEra("oldrepublic");
            }

            if (GUILayout.Button("high rep"))
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

            if (GUILayout.Button("fo"))
            {
                SetEra("firstorder");
            }

            if (GUILayout.Button("gal all"))
            {
                SetEra("galacticalliance");
            }

            GUILayout.EndHorizontal();

            EditorGUILayout.LabelField("Author", EditorStyles.boldLabel);

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

        EditorGUILayout.EndScrollView();
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

    private void SearchForEntry()
    {
        for (int i = 0; i < shipTypeDataWrapper.shipTypeData.Count; i ++)
        {
            ShipTypeData tempShip = shipTypeDataWrapper.shipTypeData[i];

            if (tempShip.type.Contains(searchforship))
            {
                currentIndex = i;
                currentShip = tempShip;
                break;
            }
        }
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
        // Sort the list alphabetically by the 'type' field
        shipTypeDataWrapper.shipTypeData.Sort((a, b) => string.Compare(a.type, b.type, System.StringComparison.OrdinalIgnoreCase));

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
        public float smallturret;
        public float largeturret;
        public float shipLength;
        public string shieldType;
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