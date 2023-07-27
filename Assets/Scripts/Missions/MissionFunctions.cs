using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class MissionFunctions
{
    #region event system

    //This executes the different mission events
    public static IEnumerator RunMission(string missionName, string missionAddress)
    {
        //This looks for the mission manager and if it doesn't find one creates one
        MissionManager missionManager = GameObject.FindObjectOfType<MissionManager>();      

        if (missionManager == null)
        {
            GameObject missionManagerGO = new GameObject();
            missionManagerGO.name = "MissionManager";
            missionManager = missionManagerGO.AddComponent<MissionManager>();
        }

        missionManager.missionAddress = missionAddress;

        //This marks the start time of the script
        float startTime = Time.time;

        //This loads the Json file
        TextAsset missionDataFile = Resources.Load(missionManager.missionAddress + missionName) as TextAsset;
        Mission mission = JsonUtility.FromJson<Mission>(missionDataFile.text);
        Scene scene = SceneFunctions.GetScene();

        //This loads scene and any ships set to "preload"
        Time.timeScale = 0;

        float time = Time.unscaledTime;

        DisplayLoadingScreen(missionName, true);

        LoadScreenFunctions.AddLogToLoadingScreen("Start loading " + missionName + ".", Time.unscaledTime - time);

        foreach (MissionEvent missionEvent in mission.missionEventData)
        {
            if (missionEvent.eventType == "loadscene")
            {
                Task a = new Task(LoadScene(missionName, missionEvent.data1, false, missionAddress));
                while (a.Running == true) { yield return null; }
            }
        }

        foreach (MissionEvent missionEvent in mission.missionEventData)
        {
            if (missionEvent.eventType == "preload_loadship" )
            {
                LoadShip(missionEvent);
                LoadScreenFunctions.AddLogToLoadingScreen("single ship created", 0);
            }
            else if (missionEvent.eventType == "preload_loadshipsbyname")
            {
                Task a = new Task(LoadShipsByName(missionEvent));
                while (a.Running == true) { yield return null; }
                LoadScreenFunctions.AddLogToLoadingScreen("batch of ships created by name", 0);
            }
            else if (missionEvent.eventType == "preload_loadshipsbytypeandallegiance")
            {
                Task a = new Task(LoadShipsByTypeAndAllegiance(missionEvent));
                while (a.Running == true) { yield return null; }
                LoadScreenFunctions.AddLogToLoadingScreen("batch of ships created by type and allegiance", 0);
            }
        }

        Time.timeScale = 1;

        //This tells the player to get ready
        LoadScreenFunctions.AddLogToLoadingScreen(missionName + " loaded.", 0);
        LoadScreenFunctions.AddLogToLoadingScreen("Get ready to launch in 5 seconds...", 0);

        float delay = Time.unscaledTime + 5;

        while (Time.unscaledTime < delay) { yield return null; }

        DisplayLoadingScreen(missionName, false);

        //This sets the mission manager to running
        missionManager.running = true;

        //This finds the first event number
        int eventNo = 0;

        foreach (MissionEvent missionEvent in mission.missionEventData)
        {
            if (missionEvent.eventType == "startmission")
            {
                FindNextEvent(missionName, missionEvent.nextEvent1);
                break;
            }

            eventNo++;
        }

        //This runs the events according to the logic of the mission file
        while (missionManager.running == true)
        {
            MissionEvent missionEvent = mission.missionEventData[missionManager.eventNo];

            float markTime = Time.time;

            if  (!missionEvent.eventType.Contains("preload") & missionEvent.eventType != "loadscene")
            {
                //This makes the event run at the correct time
                if (missionEvent.conditionTime != 0)
                {
                    if (markTime + missionEvent.conditionTime > Time.time)
                    {
                        yield return new WaitUntil(() => markTime + missionEvent.conditionTime < Time.time);
                    }
                }

                //This makes the event run in the correct location
                if (missionEvent.conditionLocation != "none")
                {
                    if (missionEvent.conditionLocation != scene.location)
                    {
                        float startPause = Time.time;
                        yield return new WaitUntil(() => missionEvent.conditionLocation == scene.location);
                    }
                }

                RunEvent(missionName, missionEvent);
            }

            yield return null;
        }
    }

    //This runs the appropriate event function
    public static void RunEvent(string missionName, MissionEvent missionEvent)
    {
        //This runs the requested function
        if (missionEvent.eventType == "changemusicvolume")
        {
            ChangeMusicVolume(float.Parse(missionEvent.data1));
            FindNextEvent(missionName, missionEvent.nextEvent1);
        }
        else if (missionEvent.eventType == "clearaioverride")
        {
            ClearAIOverride(missionEvent.data1);
            FindNextEvent(missionName, missionEvent.nextEvent1);
        }
        else if (missionEvent.eventType == "displaylargemessagethenexit")
        {
            Task a = new Task(DisplayLargeMesssageThenExit(missionEvent.data1));
            FindNextEvent(missionName, missionEvent.nextEvent1);
        }
        else if (missionEvent.eventType == "dialoguebox")
        {
            DialogueBox(missionEvent.data1);
            FindNextEvent(missionName, missionEvent.nextEvent1);
        }
        else if (missionEvent.eventType == "displaylargemessage")
        {
            DisplayLargeMessage(missionEvent.data1);
            FindNextEvent(missionName, missionEvent.nextEvent1);
        }
        else if (missionEvent.eventType == "displaylocation")
        {
            DisplayLocation(missionEvent.data1, missionEvent.data2);
            FindNextEvent(missionName, missionEvent.nextEvent1);
        }
        else if (missionEvent.eventType == "displayloadingscreen")
        {
            DisplayLoadingScreen(missionName, bool.Parse(missionEvent.data1));
            FindNextEvent(missionName, missionEvent.nextEvent1);
        }
        else if (missionEvent.eventType == "iftypeofshipisactive")
        {
            bool shipTypeIsActive = IfTypeOfShipIsActive(missionEvent.data1);

            if (shipTypeIsActive == true)
            {
                FindNextEvent(missionName, missionEvent.nextEvent1);
            }
            else
            {
                FindNextEvent(missionName, missionEvent.nextEvent2);
            }
        }
        else if (missionEvent.eventType == "loadship")
        {
            LoadShip(missionEvent);
            FindNextEvent(missionName, missionEvent.nextEvent1);
        }
        else if (missionEvent.eventType == "loadshipatdistanceandanglefromplayer")
        {
            LoadShipAtDistanceAndAngleFromPlayer(missionEvent);
            FindNextEvent(missionName, missionEvent.nextEvent1);
        }
        else if (missionEvent.eventType == "loadshipsbyname")
        {
            Task a = new Task(LoadShipsByName(missionEvent));
            FindNextEvent(missionName, missionEvent.nextEvent1);
        }
        else if (missionEvent.eventType == "loadshipsbytypeandallegiance")
        {
            Task a = new Task(LoadShipsByTypeAndAllegiance(missionEvent));
            FindNextEvent(missionName, missionEvent.nextEvent1);
        }
        else if (missionEvent.eventType == "lockmainshipweapons")
        {
            LockMainShipWeapons(missionEvent);
            FindNextEvent(missionName, missionEvent.nextEvent1);
        }
        else if (missionEvent.eventType == "message")
        {
            Message(missionEvent);
            FindNextEvent(missionName, missionEvent.nextEvent1);
        }
        else if (missionEvent.eventType == "movetowaypoint")
        {
            MoveToWaypoint(missionEvent);
            FindNextEvent(missionName, missionEvent.nextEvent1);
        }
        else if (missionEvent.eventType == "playmusictype")
        {
            PlayMusicType(missionEvent.data1);
            FindNextEvent(missionName, missionEvent.nextEvent1);
        }
        else if (missionEvent.eventType == "setshiptoinvincible")
        {
            SetShipToInvincible(missionEvent);
            FindNextEvent(missionName, missionEvent.nextEvent1);
        }
        else if (missionEvent.eventType == "shipshullislessthan")
        {
            bool isLessThan = ShipsHullIsLessThan(missionEvent);

            if (isLessThan == true)
            {
                FindNextEvent(missionName, missionEvent.nextEvent1);
            }
            else
            {
                FindNextEvent(missionName, missionEvent.nextEvent2);
            }
        }
        else if (missionEvent.eventType == "shipislessthandistancetowaypoint")
        {
            bool isLessThanDistance = ShipIsLessThanDistanceToWaypoint(missionEvent);

            if (isLessThanDistance == true)
            {
                FindNextEvent(missionName, missionEvent.nextEvent1);
            }
            else
            {
                FindNextEvent(missionName, missionEvent.nextEvent2);
            }
        }
    }

    //This looks for the next event to run
    public static void FindNextEvent(string missionName, string nextEvent)
    {
        MissionManager missionManager = GameObject.FindObjectOfType<MissionManager>();

        if (nextEvent != "none")
        {
            TextAsset missionDataFile = Resources.Load(missionManager.missionAddress + missionName) as TextAsset;
            Mission mission = JsonUtility.FromJson<Mission>(missionDataFile.text);

            int i = 0;

            foreach (MissionEvent missionEvent in mission.missionEventData)
            {
                if (missionEvent.eventID == nextEvent)
                {
                    missionManager.eventNo = i;
                    break;
                }

                i++;
            }
        }
        else
        {
            missionManager.running = false;
        }
    }

    #endregion

    #region main scene loading function

    //This creates the scene 
    public static IEnumerator LoadScene(string missionName, string location, bool randomiseLocation, string missionAddress)
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
        AudioFunctions.CreateAudioManager(missionAddress + missionName + "_audio" + "/", false);
        LoadScreenFunctions.AddLogToLoadingScreen("Audio Manager created", Time.unscaledTime - time);
        MusicFunctions.CreateMusicManager();
        LoadScreenFunctions.AddLogToLoadingScreen("Music Manager created", Time.unscaledTime - time);

        //This radommises the planet location if requested
        if (randomiseLocation == true)
        {
            location = "none";
        }

        //This gets the planet data
        var planetData = SceneFunctions.GetSpecificLocation(location);
        SceneFunctions.MoveStarfieldCamera(planetData.location);
        scene.location = planetData.planet;

        Task a = new Task(SceneFunctions.GeneratePlanetHeightmap(planetData.type, planetData.seed, 2048));
        while (a.Running == true) { yield return null; }
        SceneFunctions.SetPlanetDistance(planetData.seed);
        LoadScreenFunctions.AddLogToLoadingScreen("Planet created", Time.unscaledTime - time);

        //This creates the asteroids in the area
        Task b = new Task(SceneFunctions.CreateAsteroidField(planetData.seed));
        while (b.Running == true) { yield return null; }
        LoadScreenFunctions.AddLogToLoadingScreen("Asteroids created", Time.unscaledTime - time);
    }

    #endregion

    #region event functions

    //THis changes the volume of the music 
    public static void ChangeMusicVolume(float volume)
    {
        Music musicManager = GameObject.FindObjectOfType<Music>();

        if (musicManager != null)
        {
            MusicFunctions.ChangeMusicVolume(musicManager, volume);
        }
    }

    //This clears any AI overides on a ship/ships i.e. "waypoint", "dontattack" etc
    public static void ClearAIOverride(string shipName)
    {
        Scene scene = SceneFunctions.GetScene();

        if (scene != null)
        {
            if (scene.objectPool != null)
            {
                foreach (GameObject ship in scene.objectPool)
                {
                    if (ship.name.Contains(shipName))
                    {
                        SmallShip smallShip = ship.GetComponent<SmallShip>();

                        if (smallShip != null)
                        {
                            smallShip.aiOverideMode = "none";
                        }
                    }
                }
            }
        }
    }

    //This displays "[Allegiance] Victory" and then returns the player to the main screen after five seconds
    public static IEnumerator DisplayLargeMesssageThenExit(string message)
    {
        DisplayLargeMessage(message.ToUpper());

        yield return new WaitForSeconds(5);

        ExitMenuFunctions.ExitAndUnload();
    }

    //This displays the dialogue box with a message
    public static void DialogueBox(string message)
    {
        DialogueBoxFunctions.DisplayDialogueBox(true, message);
    }

    //This temporary displays a large message in the center of the screen
    public static void DisplayLargeMessage(string message)
    {
        HudFunctions.DisplayLargeMessage(message);
    }

    //This displays the location + text before and after if desired
    public static void DisplayLocation(string textBefore, string textAfter)
    {
        Scene scene = SceneFunctions.GetScene();

        if (textAfter == "none")
        {
            textAfter = "";
        }

        if (textBefore == "none")
        {
            textBefore = "";
        }

        if (scene != null)
        {
            HudFunctions.DisplayLargeMessage(textBefore.ToUpper() + scene.location.ToUpper() + textAfter.ToUpper());
        }       
    }

    //This displays the loading screen
    public static void DisplayLoadingScreen(string missionName, bool display)
    {
        if (display == true)
        {
            string[] messages = new string[10];
            messages[0] = "Settle into your seat pilot. Flying a starfighter requires practice, skill, and focus.";
            LoadScreenFunctions.LoadingScreen(true, missionName, messages[0]);
        }
        else
        {
            LoadScreenFunctions.LoadingScreen(false);
        }
    }

    //This checks whether there are any active ships of a certain allegiance, i.e. are there any imperial ships left
    public static bool IfTypeOfShipIsActive(string allegiance)
    {
        Scene scene = SceneFunctions.GetScene();

        bool shipTypeIsActive = false;

        if (scene != null)
        {
            if (scene.objectPool != null)
            {
                foreach (GameObject ship in scene.objectPool)
                {
                    SmallShip smallShip = ship.GetComponent<SmallShip>();

                    if (smallShip != null)
                    {
                        if (ship.activeSelf == true & smallShip.allegiance == allegiance)
                        {
                            shipTypeIsActive = true;
                            break;
                        }
                    }
                }
            }
        }

        return shipTypeIsActive;
    }

    //This loads a single ship by name
    public static void LoadShip(MissionEvent missionEvent)
    {
        string ship = missionEvent.data1;
        float x = missionEvent.x;
        float y = missionEvent.y;
        float z = missionEvent.z;
        string allegiance = missionEvent.data4;
        string squadronName = missionEvent.data5;

        bool randomise = bool.Parse(missionEvent.data3);

        if (ship == "none" || randomise == true)
        {
            string[] shipTypes = { "tiefighter", "tieinterceptor", "tiebomber", "tieadvanced", "assaultgunboat", "ywing", "xwing", "awing", "z95headhunter" };
            int shipTypesCount = shipTypes.Length;
            int randomShipNo = Random.Range(0, shipTypesCount);
            ship = shipTypes[randomShipNo];
        }

        if (randomise == true)
        {
            x = Random.Range(-10000, 10000);
            y = Random.Range(-10000, 10000);
            z = Random.Range(-10000, 10000);
        }

        SceneFunctions.LoadShip(ship, bool.Parse(missionEvent.data2), new Vector3(x, y, z), allegiance, false, "easy", squadronName);
    }

    //This loads a single ship at a certain distance and angle from the player
    public static void LoadShipAtDistanceAndAngleFromPlayer(MissionEvent missionEvent)
    {
        string ship = missionEvent.data1;
        float x = missionEvent.x;
        float y = missionEvent.y;
        float z = missionEvent.z;
        string allegiance = missionEvent.data4;
        float distance = float.Parse(missionEvent.data5);
        string squadronName = missionEvent.data6;

        bool randomise = bool.Parse(missionEvent.data3);

        if (ship == "none" || randomise == true)
        {
            string[] shipTypes = { "tiefighter", "tieinterceptor", "tiebomber", "tieadvanced", "assaultgunboat", "ywing", "xwing", "awing", "z95headhunter" };
            int shipTypesCount = shipTypes.Length;
            int randomShipNo = Random.Range(0, shipTypesCount);
            ship = shipTypes[randomShipNo];
        }

        Scene scene = SceneFunctions.GetScene();

        Vector3 newPosition = new Vector3(0, 0, 0);

        if (scene != null)
        {
            if (scene.mainShip != null)
            {
                newPosition = (Quaternion.Euler(x, y, z) * scene.mainShip.transform.forward).normalized * distance;
            }
        }

        SceneFunctions.LoadShip(ship, bool.Parse(missionEvent.data2), newPosition, allegiance, false, "easy", squadronName);
    }

    //This loads multiple ships by name
    public static IEnumerator LoadShipsByName(MissionEvent missionEvent)
    {
        float x = missionEvent.x;
        float y = missionEvent.y;
        float z = missionEvent.z;

        string shipName = "xwing";
        if (missionEvent.data1 != "none") { shipName = missionEvent.data1;}

        int shipNo = 1;
        if (missionEvent.data2 != "none") { shipNo = int.Parse(missionEvent.data2); }

        string skillLevel = "easy";
        if (missionEvent.data3 != "none") { skillLevel = missionEvent.data3; }

        int groupsOf = 4;
        if (missionEvent.data4 != "none") { groupsOf = int.Parse(missionEvent.data4); }

        float groupingDistance = 50;
        if (missionEvent.data5 != "none") { groupingDistance = float.Parse(missionEvent.data5); }

        float groupingDiffereniation = 250;
        if (missionEvent.data6 != "none") { groupingDiffereniation = float.Parse(missionEvent.data6); }

        bool radomisePosition = false;
        if (missionEvent.data7 != "none") { radomisePosition = bool.Parse(missionEvent.data7); }

        float positionVariance = 10;
        if (missionEvent.data8 != "none") { positionVariance = float.Parse(missionEvent.data8); }

        bool randomise = false;
        if (missionEvent.data9 != "none") { randomise = bool.Parse(missionEvent.data9); }

        string squadronName = "none";
        if (missionEvent.data10 != "none") { squadronName = missionEvent.data10; }

        bool includePlayer = false;
        if (missionEvent.data11 != "none" & missionEvent.data11 != null) { includePlayer = bool.Parse(missionEvent.data11); }

        int playerNo = 0;
        if (missionEvent.data12 != "none" & missionEvent.data12 != null) { playerNo = int.Parse(missionEvent.data12); }

        if (randomise == true)
        {
            x = Random.Range(-10000, 10000);
            y = Random.Range(-10000, 10000);
            z = Random.Range(-10000, 10000);
            shipNo = Random.Range(1, 30);
            groupsOf = Random.Range(1, 10);
            playerNo = Random.Range(0, shipNo - 1);
        }

        Task c = new Task(SceneFunctions.LoadShipsByName(shipName, shipNo, skillLevel, groupsOf, groupingDistance, groupingDiffereniation, radomisePosition, positionVariance, new Vector3(x, y, z), squadronName, includePlayer, playerNo));
        while (c.Running == true) { yield return null; }
    }

    //This loads multiple ships by type and allegiance
    public static IEnumerator LoadShipsByTypeAndAllegiance(MissionEvent missionEvent)
    {
        float x = missionEvent.x;
        float y = missionEvent.y;
        float z = missionEvent.z;

        string shipType = "fighter";
        if (missionEvent.data1 != "none") { shipType = missionEvent.data1; }

        string allegiance = "rebel";
        if (missionEvent.data2 != "none") { allegiance = missionEvent.data2; }

        string skillLevel = "easy";
        if (missionEvent.data3 != "none") { skillLevel = missionEvent.data3; }

        int shipNo = 1;
        if (missionEvent.data4 != "none") { shipNo = int.Parse(missionEvent.data4); }

        int groupsOf = 4;
        if (missionEvent.data5 != "none") { groupsOf = int.Parse(missionEvent.data5); }

        float groupingDistance = 50;
        if (missionEvent.data6 != "none") { groupingDistance = float.Parse(missionEvent.data6); }

        float groupingDiffereniation = 250;
        if (missionEvent.data7 != "none") { groupingDiffereniation = float.Parse(missionEvent.data7); }

        bool radomisePosition = false;
        if (missionEvent.data8 != "none") { radomisePosition = bool.Parse(missionEvent.data8); }

        float positionVariance = 10;
        if (missionEvent.data9 != "none") { positionVariance = float.Parse(missionEvent.data9); }

        bool randomise = false;
        if (missionEvent.data10 != "none") { randomise = bool.Parse(missionEvent.data10); }

        string squadronName = "none";
        if (missionEvent.data11 != "none" & missionEvent.data11 != null) { squadronName = missionEvent.data11; }

        bool includePlayer = false;
        if (missionEvent.data12 != "none" & missionEvent.data12 != null) { includePlayer = bool.Parse(missionEvent.data12); }

        int playerNo = 0;
        if (missionEvent.data13 != "none" & missionEvent.data13 != null) { playerNo = int.Parse(missionEvent.data13); }

        if (randomise == true)
        {
            x = Random.Range(-10000, 10000);
            y = Random.Range(-10000, 10000);
            z = Random.Range(-10000, 10000);
            shipNo = Random.Range(1, 30);
            groupsOf = Random.Range(1, 10);
            playerNo = Random.Range(0, shipNo - 1);
        }

        Task c = new Task(SceneFunctions.LoadShipsByTypeAndAllegiance(missionEvent.data1, missionEvent.data2, missionEvent.data3, shipNo, groupsOf, float.Parse(missionEvent.data6), float.Parse(missionEvent.data7), bool.Parse(missionEvent.data8), float.Parse(missionEvent.data9), new Vector3(x, y, z), squadronName, includePlayer, playerNo));
        while (c.Running == true) { yield return null; }
    }

    //This toggles whether the player can fire their weapons or not
    public static void LockMainShipWeapons(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        if (scene != null)
        {
            if (scene.mainShip != null)
            {
                SmallShip smallShip = scene.mainShip.GetComponent<SmallShip>();

                if (smallShip != null)
                {
                    smallShip.weaponsLock = bool.Parse(missionEvent.data1);
                }
            }
        }
    }

    //This adds a message to the log and can also play an audio file
    public static void Message(MissionEvent missionEvent)
    {
        string audio = missionEvent.data2;
        string message = missionEvent.data1;
        string internalAudioFile = missionEvent.data3;

        HudFunctions.AddToShipLog(message);

        if (audio != "none" & internalAudioFile != "true")
        {
            AudioFunctions.PlayVoiceClip(null, audio, new Vector3(0,0,0), 0, 1, 500, 1f, 1);
        }
        else if (audio != "none" & internalAudioFile == "true")
        {
            AudioFunctions.PlayAudioClip(null, audio, new Vector3(0, 0, 0), 0, 1, 500, 1f, 1);
        }
    }

    //This tells a ship or ships to move toward a waypoint by position its waypoint and setting its ai override mode
    public static void MoveToWaypoint(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        if (scene != null)
        {
            if (scene.objectPool != null)
            {
                foreach (GameObject ship in scene.objectPool)
                {
                    if (ship.name.Contains(missionEvent.data1))
                    {
                        SmallShip smallShip = ship.GetComponent<SmallShip>();

                        if (smallShip != null)
                        {
                            smallShip.aiOverideMode = "MoveToWaypoint";

                            if (smallShip.waypoint != null)
                            {
                                float x = missionEvent.x;
                                float y = missionEvent.y;
                                float z = missionEvent.z;
                                Vector3 waypoint = scene.transform.position + new Vector3(x,y,z);

                                smallShip.waypoint.transform.position = waypoint;
                            }
                        }
                    }
                }
            }
        }
    }

    //This changes the type of music that is playing
    public static void PlayMusicType(string musicType)
    {
        Music musicManager = GameObject.FindObjectOfType<Music>();

        if (musicManager != null)
        {
            MusicFunctions.PlayMusicType(musicManager, musicType, true);
        }       
    }

    //This tells a ship or ships to move toward a waypoint by position its waypoint and setting its ai override mode
    public static void SetShipToInvincible(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        bool isInvincible = false;

        if (missionEvent.data2 != "none")
        {
            isInvincible = bool.Parse(missionEvent.data2);
        }

        if (scene != null)
        {
            if (scene.objectPool != null)
            {
                foreach (GameObject ship in scene.objectPool)
                {
                    if (ship.name.Contains(missionEvent.data1))
                    {
                        SmallShip smallShip = ship.GetComponent<SmallShip>();

                        if (smallShip != null)
                        {
                            smallShip.invincible = isInvincible;
                        }
                    }
                }
            }
        }
    }

    //This checks the ship distance to its waypoint
    public static bool ShipsHullIsLessThan(MissionEvent missionEvent)
    {
        bool islessthanamount = false;

        Scene scene = SceneFunctions.GetScene();

        float hullLevel = Mathf.Infinity;

        if (missionEvent.data2 != "none")
        {
            hullLevel = float.Parse(missionEvent.data2);
        }

        if (scene != null)
        {
            if (scene.objectPool != null)
            {
                foreach (GameObject ship in scene.objectPool)
                {
                    if (ship.name.Contains(missionEvent.data1))
                    {
                        SmallShip smallShip = ship.GetComponent<SmallShip>();

                        if (smallShip != null)
                        {
                            if (smallShip.hullLevel < hullLevel)
                            {
                                islessthanamount = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        return islessthanamount;
    }

    //This checks the ship distance to its waypoint
    public static bool ShipIsLessThanDistanceToWaypoint(MissionEvent missionEvent)
    {
        bool isLessThanDistance = false;

        Scene scene = SceneFunctions.GetScene();

        float distance = Mathf.Infinity;

        if (missionEvent.data2 != "none")
        {
            distance = float.Parse(missionEvent.data2);
        }

        if (scene != null)
        {
            if (scene.objectPool != null)
            {
                foreach (GameObject ship in scene.objectPool)
                {
                    if (ship.name.Contains(missionEvent.data1))
                    {
                        SmallShip smallShip = ship.GetComponent<SmallShip>();

                        if (smallShip != null)
                        {
                            smallShip.aiOverideMode = "MoveToWaypoint";

                            if (smallShip.waypoint != null)
                            {
                                float tempDistance = Vector3.Distance(smallShip.transform.position, smallShip.waypoint.transform.position);

                                if (tempDistance < distance)
                                {
                                    isLessThanDistance = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        return isLessThanDistance;
    }

    #endregion

    #region exit functions

    //This checks whether the player ship has been destroyed
    public static void ExitOnPlayerDestroy(MissionManager missionManager)
    {
        if (missionManager.unloading == false)
        {
            if (missionManager.scene == null)
            {
                missionManager.scene = SceneFunctions.GetScene();
            }

            if (missionManager.scene != null)
            {
                if (missionManager.scene.mainShip != null)
                {
                    if (missionManager.scene.mainShip.activeSelf == false)
                    {
                        Task a = new Task(UnloadAfter(5));
                        Task b = new Task(HudFunctions.FadeOutHud(0.25f));
                        missionManager.unloading = true;
                    }
                }
            }
        }
    }

    //This returns to the main menu after a set period of time
    public static IEnumerator UnloadAfter(float time)
    {
        yield return new WaitForSeconds(time);

        ExitMenuFunctions.ExitAndUnload();

    }

    //This activates the exit menu
    public static void ActivateExitMenu(MissionManager missionManager)
    {
        Keyboard keyboard = Keyboard.current;

        if (keyboard.escapeKey.isPressed == true & missionManager.pressedTime < Time.time)
        {
            ExitMenuFunctions.DisplayExitMenu(true);
            missionManager.pressedTime = Time.time + 0.5f;
        }

    }

    #endregion
}
