using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OGSettingsFunctions
{
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

        settings.heightMapResolution = ogSettingsClass.ogSettingsData[0].heightMapResolution;
        settings.invertX = ogSettingsClass.ogSettingsData[0].invertX;
        settings.invertY = ogSettingsClass.ogSettingsData[0].invertY;
        settings.screenResX = ogSettingsClass.ogSettingsData[0].screenResX;
        settings.screenResY = ogSettingsClass.ogSettingsData[0].screenResY;
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
}
