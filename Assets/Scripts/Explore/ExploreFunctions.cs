using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExploreFunctions
{
    //This executes the different mission events
    public static IEnumerator RunExplore()
    {
        //This looks for the mission manager and if it doesn't find one creates one
        ExploreManager exploreManager = GetExploreManager();

        //This marks the start time of the script
        float startTime = Time.time;

        Scene scene = SceneFunctions.GetScene();

        //This pauses the game to prevent action starting before everything is loaded 
        Time.timeScale = 0;

        //This marks the time using unscaled time
        float time = Time.unscaledTime;

        //This displays the loading screen
        DisplayLoadingScreen(true);

        //This tells the player that the mission is being loaded
        LoadScreenFunctions.AddLogToLoadingScreen("Start loading Explore", Time.unscaledTime - time);

        //This loads the base game scene
        LoadScene();

        //This sets the skybox to the default space
        SceneFunctions.SetSkybox("space");

        exploreManager.currentLocation = "Tatooine";

        //This runs all the preload events like loading the planet and asteroids and objects already in scene
        Task a = new Task(LoadLocationScenery(exploreManager.currentLocation));
        while (a.Running == true) { yield return null; }

        //UNFINISHED: This loads the ships in the scene

        //This loads the player in the scene
        LoadPlayerShip();

        //This tells the player to get ready, starts the game, locks the cursor and gets rid of the loading screen
        LoadScreenFunctions.AddLogToLoadingScreen("Explore mode loaded.", Time.unscaledTime - time);

        //This unpause the game 
        Time.timeScale = 1;

        //This locks and hides the cursor during gameplay
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        //This removes the loading screen 
        DisplayLoadingScreen(false);

        //This sets the mission manager to running
        exploreManager.running = true;
    }

    //This displays the loading screen
    public static void DisplayLoadingScreen(bool display)
    {
        if (display == true)
        {
            string[] messages = new string[10];
            messages[0] = "Settle into your seat pilot. Flying a starfighter requires practice, skill, and focus.";
            messages[1] = "If you constantly fly at top speed you will almost always overshoot your target.";
            messages[2] = "Linking your lasers is a good way to compensate when you need power for other systems.";
            messages[3] = "Even if your shields are at full strength, your ship will take damage from physical impacts.";
            messages[4] = "In more maneuvarable craft you can hold down the match speed button to stay on someone's tail.";
            messages[5] = "In less maneuvrable craft you will need to lower your speed manually to perform sharp turns.";
            messages[6] = "Your ship is most maneuverable at half speed, so try slowing down if you can't track a target.";
            messages[7] = "When you push all your energy to engines or lasers your shields will start to lose strength.";
            messages[8] = "When you push all your energy to engines your WEP system becomes availible.";
            messages[9] = "Remember you can link your torpedoes and lasers for greater impact.";
            int randomMessageNo = Random.Range(0, 9);
            LoadScreenFunctions.LoadingScreen(true, "Explore Mode", messages[randomMessageNo]);
        }
        else
        {
            LoadScreenFunctions.LoadingScreen(false);
        }
    }

    //This creates the scene 
    public static void LoadScene()
    {
        Scene scene = SceneFunctions.GetScene();

        //This marks the load time using unscaled time
        float time = Time.unscaledTime;

        //This creates the scene and gets the cameras
        SceneFunctions.CreateScene();
        SceneFunctions.GetCameras();
        LoadScreenFunctions.AddLogToLoadingScreen("Scene created.", Time.unscaledTime - time);

        //This creates the hud
        HudFunctions.CreateHud();
        LoadScreenFunctions.AddLogToLoadingScreen("Hud created.", Time.unscaledTime - time);

        //THis loads the audio and music manager
        AudioFunctions.CreateAudioManager("explore_audio/", false);
        LoadScreenFunctions.AddLogToLoadingScreen("Audio Manager created", Time.unscaledTime - time);
        MusicFunctions.CreateMusicManager();
        LoadScreenFunctions.AddLogToLoadingScreen("Music Manager created", Time.unscaledTime - time);
    }

    //This looks for and runs preload events
    public static IEnumerator LoadLocationScenery(string location)
    {
        var locationData = SceneFunctions.FindLocation(location);

        string type = locationData.type;
        int seed = locationData.seed;
        string planetType = locationData.planet;

        float time = Time.unscaledTime;

        //Load Asteroids 
        Task a = new Task(SceneFunctions.GenerateAsteroidField(seed, false));
        while (a.Running == true) { yield return null; }
        LoadScreenFunctions.AddLogToLoadingScreen("Asteroids loaded", Time.unscaledTime - time);

        //Load Planet
        LoadScreenFunctions.AddLogToLoadingScreen("Generating unique planet heightmap. This may take a while...", Time.unscaledTime - time);
        Task b = new Task(SceneFunctions.GeneratePlanetHeightmap(type, seed));
        while (b.Running == true) { yield return null; }
        LoadScreenFunctions.AddLogToLoadingScreen("Planet loaded", Time.unscaledTime - time);
    }

    public static void LoadPlayerShip()
    {
        ExploreManager exploreManager = GetExploreManager();

        Vector3 position = exploreManager.playerPosition;
        Quaternion rotation = exploreManager.playerRotation;
        string type = exploreManager.playerShipType;
        string name = exploreManager.playerShipName;
        string allegiance = exploreManager.playerAllegiance;
        string cargo = "none";

        SceneFunctions.LoadSingleShip(position, rotation, type, name, allegiance, cargo, false, false, false);
    }

    public static ExploreManager GetExploreManager()
    {
        ExploreManager exploreManager = GameObject.FindObjectOfType<ExploreManager>();

        if (exploreManager == null)
        {
            GameObject missionManagerGO = new GameObject();
            missionManagerGO.name = "MissionManager";
            exploreManager = missionManagerGO.AddComponent<ExploreManager>();
        }

        return exploreManager;
    }
}
