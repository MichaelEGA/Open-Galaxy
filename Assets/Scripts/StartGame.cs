using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        SceneFunctions.CreateCameras();

        Task special = new Task(SceneFunctions.GenerateStarField());

        Task a = new Task(MainMenuFunctions.RunMenu());
    
        OGSettingsFunctions.LoadSettingsData();

        OGSettings settings = OGSettingsFunctions.GetSettings();
        OGSettingsFunctions.SetGameWindowMode(settings.gameWindowMode);

        Debug.developerConsoleVisible = false;

        //SceneFunctions.LoadTerrain("Terrain01", "Forest01");
    }

    void Update()
    {
    }
}
