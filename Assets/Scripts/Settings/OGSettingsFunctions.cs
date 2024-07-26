using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OGSettingsFunctions
{
    #region

    //This returns the address of the designated file or folder
    public static string RetrieveFileAddress(string name)
    {
        string address = "";

        if (name == "asteroids")
        {
            address = "objects/debris/asteroids";
        }
        else if (name == "audioclips")
        {
            address = "audio/audioclips/";
        }
        else if (name == "audiomixers")
        {
            address = "audio/audiomixers/";
        }
        else if (name == "cockpits_opengalaxy")
        {
            address = "objects/cockpits/opengalaxy/";
        }
        else if (name == "cockpits_firststrike")
        {
            address = "objects/cockpits/galacticconquest/";
        }
        else if (name == "cockpits_galacticconquest")
        {
            address = "objects/cockpits/galacticconquest/";
        }
        else if (name == "editor")
        {
            address = "editor";
        }
        else if (name == "files")
        {
            address = "files";
        }
        else if (name == "fonts")
        {
            address = "fonts";
        }
        else if (name == "hud")
        {
            address = "hud";
        }
        else if (name == "hyperspace")
        {
            address = "objects/hyperspace/";
        }
        else if (name == "menus")
        {
            address = "menus";
        }
        else if (name == "menubuttons")
        {
            address = "menus/buttons/";
        }
        else if (name == "missions")
        {
            address = "missions";
        }
        else if (name == "musicclips")
        {
            address = "audio/musicclips/";
        }
        else if (name == "particles")
        {
            address = "particles";
        }
        else if (name == "planets")
        {
            address = "objects/planets/";
        }
        else if (name == "radar")
        {
            address = "objects/radar/";
        }
        else if (name == "ships_opengalaxy")
        {
            address = "objects/ships/opengalaxy/";
        }
        else if (name == "ships_firststrike")
        {
            address = "objects/ships/galacticconquest/";
        }
        else if (name == "ships_galacticconquest")
        {
            address = "objects/ships/galacticconquest/";
        }
        else if (name == "ships_originaltrilogy")
        {
            address = "objects/ships/originaltrilogy/";
        }
        else if (name == "skyboxes")
        {
            address = "skyboxes";
        }
        else if (name == "terrainmaterials")
        {
            address = "terrains/materials/";
        }
        else if (name == "terrainmeshes")
        {
            address = "terrains/meshes/";
        }
        else if (name == "turrets")
        {
            address = "objects/turrets/";
        }

        return address;
    }

    #endregion

    #region loading and saving settings

    //This loads the settings data from an external file or if it doesn't find one makes a new one
    public static void LoadSettingsData()
    {
        //This looks for the settings file in the persistent data file
        string settingsDataFile = "none";
        string path = Application.persistentDataPath + "/settings.json";

        if (System.IO.File.Exists(path))
        {
            settingsDataFile = File.ReadAllText(path);
        }

        //If this doesn't find the file it makes a new one
        if (settingsDataFile == "none")
        {
            TextAsset internalSettingsDataFile = Resources.Load("files/Settings") as TextAsset;
            System.IO.File.WriteAllText(Application.persistentDataPath + "/settings.json", internalSettingsDataFile.text);
            settingsDataFile = internalSettingsDataFile.text;
        }

        //This loads the data into the class
        OGSettingsClass ogSettingsClass = JsonUtility.FromJson<OGSettingsClass>(settingsDataFile);

        //This applies all the values into the accesible  settings class
        OGSettings settings = GetSettings();

        if (settings != null & ogSettingsClass != null)
        {
            settings.heightMapResolution = ogSettingsClass.ogSettingsData[0].heightMapResolution;
            settings.invertX = ogSettingsClass.ogSettingsData[0].invertX;
            settings.invertY = ogSettingsClass.ogSettingsData[0].invertY;
            settings.screenResX = ogSettingsClass.ogSettingsData[0].screenResX;
            settings.screenResY = ogSettingsClass.ogSettingsData[0].screenResY;
            settings.cockpitAssets = ogSettingsClass.ogSettingsData[0].cockpitAssets;
            settings.shipAssets = ogSettingsClass.ogSettingsData[0].shipAssets;
            settings.gameWindowMode = ogSettingsClass.ogSettingsData[0].gameWindowMode;
            settings.editorWindowMode = ogSettingsClass.ogSettingsData[0].editorWindowMode;
            settings.difficultly = ogSettingsClass.ogSettingsData[0].difficultly;
            settings.quality = ogSettingsClass.ogSettingsData[0].quality;
            CheckSettingsData();
        }
    }

    //This saves the data back to the settings file or if it doesn't find one makes a new one
    public static void SaveSettingsData()
    {
        //This looks for the settings file in the persistent data file
        string settingsDataFile = "none";
        string path = Application.persistentDataPath + "/settings.json";

        if (System.IO.File.Exists(path))
        {
            settingsDataFile = File.ReadAllText(path);
        }

        //If this doesn't find the file it makes a new one
        if (settingsDataFile == "none")
        {
            TextAsset internalSettingsDataFile = Resources.Load("files/Settings") as TextAsset;
            System.IO.File.WriteAllText(Application.persistentDataPath + "/settings.json", internalSettingsDataFile.text);
            settingsDataFile = internalSettingsDataFile.text;
        }

        //This loads the data into the class
        OGSettingsClass ogSettingsClass = JsonUtility.FromJson<OGSettingsClass>(settingsDataFile);

        //This applies all the values into the accesible  settings class
        OGSettings settings = GetSettings();

        ogSettingsClass.ogSettingsData[0].heightMapResolution = settings.heightMapResolution;
        ogSettingsClass.ogSettingsData[0].invertX = settings.invertX;
        ogSettingsClass.ogSettingsData[0].invertY = settings.invertY;
        ogSettingsClass.ogSettingsData[0].screenResX = settings.screenResX;
        ogSettingsClass.ogSettingsData[0].screenResY = settings.screenResY;
        ogSettingsClass.ogSettingsData[0].cockpitAssets = settings.cockpitAssets;
        ogSettingsClass.ogSettingsData[0].shipAssets = settings.shipAssets;
        ogSettingsClass.ogSettingsData[0].gameWindowMode = settings.gameWindowMode;
        ogSettingsClass.ogSettingsData[0].editorWindowMode = settings.editorWindowMode;
        ogSettingsClass.ogSettingsData[0].difficultly = settings.difficultly;
        ogSettingsClass.ogSettingsData[0].quality = settings.quality;

        // Write JSON to file.
        string stringOFSettingsData = JsonUtility.ToJson(ogSettingsClass, true);
        File.WriteAllText(Application.persistentDataPath + "/settings.json", stringOFSettingsData);
    }

    //This gets the accessible settings class
    public static OGSettings GetSettings()
    {
        OGSettings settings = null;

        settings = GameObject.FindObjectOfType<OGSettings>();

        if (settings == null)
        {
            settings = CreateSettingsGO();
        }

        return settings;
    }

    //This creates the settings GameObject and adds the settings component
    public static OGSettings CreateSettingsGO()
    {
        OGSettings settings = null;

        //This creates the gameobject and adds the component
        GameObject settingsGO = new GameObject();
        settingsGO.name = "settings";
        settings = settingsGO.AddComponent<OGSettings>();

        return settings;
    }

    //This modifies any incorrect values to prevent errors i.e. if the player manually edits the settings data
    public static void CheckSettingsData()
    {
        OGSettings settings = GetSettings();

        if (settings != null)
        {
            if (settings.heightMapResolution <= 0)
            {
                settings.heightMapResolution = 2048;
            }

            if (settings.screenResX <= 0)
            {
                settings.screenResX = 1024;
            }

            if (settings.screenResY <= 0)
            {
                settings.screenResY = 768;
            }
        }
    }

    #endregion

    #region change settings functions

    //This function selects the level of difficultly
    public static void SetDifficultlyLevel(string level)
    {
        OGSettings settings = OGSettingsFunctions.GetSettings();

        settings.difficultly = level;

        CheckSettingsData();

        SaveSettingsData();
    }

    //This function selects the cockpits to be used 
    public static void SelectCockpitAssets(string type)
    {
        OGSettings settings = OGSettingsFunctions.GetSettings();

        settings.cockpitAssets = type;

        CheckSettingsData();

        SaveSettingsData();
    }

    //This function selects the cockpits to be used 
    public static void SelectShipAssets(string type)
    {
        OGSettings settings = OGSettingsFunctions.GetSettings();

        settings.shipAssets = type;

        CheckSettingsData();

        SaveSettingsData();
    }

    //This sets the window mode for the game
    public static void SetGameWindowMode(string windowMode)
    {
        OGSettings settings = GetSettings();

        int widthRes = settings.screenResX;
        int heightRes = settings.screenResY;

        if (windowMode == "window")
        {
            Screen.SetResolution(widthRes, heightRes, FullScreenMode.Windowed);
        }
        else if (windowMode == "maximisedwindow")
        {
            Screen.SetResolution(widthRes, heightRes, FullScreenMode.MaximizedWindow);
        }
        else if (windowMode == "fullscreen")
        {
            Screen.SetResolution(widthRes, heightRes, FullScreenMode.FullScreenWindow);
        }
        else if (windowMode == "exclusivefullscreen")
        {
            Screen.SetResolution(widthRes, heightRes, FullScreenMode.ExclusiveFullScreen);
        }

        settings.gameWindowMode = windowMode;

        OGSettingsFunctions.SaveSettingsData();
    }

    //This sets the window mode for the game
    public static void SetEditorWindowMode(string windowMode)
    {
        OGSettings settings = GetSettings();

        int widthRes = Display.main.systemWidth;
        int heightRes = Display.main.systemHeight;

        if (windowMode == "window")
        {
            Screen.SetResolution(widthRes, heightRes, FullScreenMode.Windowed);
        }
        else if (windowMode == "maximisedwindow")
        {
            Screen.SetResolution(widthRes, heightRes, FullScreenMode.MaximizedWindow);
        }
        else if (windowMode == "fullscreen")
        {
            Screen.SetResolution(widthRes, heightRes, FullScreenMode.FullScreenWindow);
        }
        else if (windowMode == "exclusivefullscreen")
        {
            Screen.SetResolution(widthRes, heightRes, FullScreenMode.ExclusiveFullScreen);
        }

        settings.editorWindowMode = windowMode;

        OGSettingsFunctions.SaveSettingsData();
    }

    //Set Open Galaxy cursor
    public static void SetOGCursor()
    {
        Texture2D cursorTexture = Resources.Load<Texture2D>("Data/HudAssets/Cursor");

        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }

    //Revert to default cursor
    public static void SetDefaultCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    //This sets the quality of the game
    public static void SetQualityLevel(string qualityMode)
    {
        OGSettings settings = OGSettingsFunctions.GetSettings();

        settings.quality = qualityMode;

        if (qualityMode == "Performant")
        {
            QualitySettings.SetQualityLevel(0, true);
        }
        else if (qualityMode == "Balanced")
        {
            QualitySettings.SetQualityLevel(1, true);
        }
        else if (qualityMode == "High Fidelity")
        {
            QualitySettings.SetQualityLevel(2, true);
        }

        CheckSettingsData();

        SaveSettingsData();
    }

    #endregion

}
