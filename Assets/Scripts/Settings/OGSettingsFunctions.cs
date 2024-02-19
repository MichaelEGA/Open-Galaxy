using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OGSettingsFunctions
{
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
            TextAsset internalSettingsDataFile = Resources.Load("Data/Files/Settings") as TextAsset;
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
            settings.gameWindowMode = ogSettingsClass.ogSettingsData[0].gameWindowMode;
            settings.editorWindowMode = ogSettingsClass.ogSettingsData[0].editorWindowMode;
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
            TextAsset internalSettingsDataFile = Resources.Load("Data/Files/Settings") as TextAsset;
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

    #endregion

}
