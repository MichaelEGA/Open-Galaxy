using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class MissionFunctions
{
    #region event system

    //This executes the different mission events
    public static IEnumerator RunMission(string missionName, string missionAddress, bool addressIsExternal = false)
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
        TextAsset missionDataFile = new TextAsset(); 
        
        if (addressIsExternal == false)
        {
            missionDataFile = Resources.Load(missionManager.missionAddress + missionName) as TextAsset;
        }
        else
        {
            string missionDataString = File.ReadAllText(missionManager.missionAddress + missionName + ".json");
            missionDataFile = new TextAsset(missionDataString);
        }

        missionManager.missionData = missionDataFile.text;
        Mission mission = JsonUtility.FromJson<Mission>(missionManager.missionData);
        Scene scene = SceneFunctions.GetScene();

        //This pauses the game to prevent action starting before everything is loaded 
        Time.timeScale = 0;

        //This marks the time using unscaled time
        float time = Time.unscaledTime;

        //This displays the loading screen
        DisplayLoadingScreen(missionName, true);

        //This tells the player that the mission is being loaded
        LoadScreenFunctions.AddLogToLoadingScreen("Start loading " + missionName + ".", Time.unscaledTime - time);

        //This loads the base game scene
        LoadScene(missionName, missionAddress, addressIsExternal);

        //This sets the skybox to the default space
        SceneFunctions.SetSkybox("space");

        //This finds and sets the first location
        MissionEvent firstLocationNode = FindFirstLocationNode(mission);
        SetGalaxyLocation(firstLocationNode);

        //This finds all the location you can jump to in the mission
        FindAllLocations(mission);

        //This runs all the preload events like loading the planet and asteroids and objects already in scene
        Task a = new Task(FindAndRunPreLoadEvents(mission, firstLocationNode.conditionLocation, time));
        while (a.Running == true) { yield return null; }

        //This tells the player to get ready, starts the game, locks the cursor and gets rid of the loading screen
        LoadScreenFunctions.AddLogToLoadingScreen(missionName + " loaded.", 0);

        //This unpause the game 
        Time.timeScale = 1;

        //This locks and hides the cursor during gameplay
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        //This removes the loading screen 
        DisplayLoadingScreen(missionName, false);

        //This sets the mission manager to running
        missionManager.running = true;

        //This checks how many event series exist in the mission file
        int eventSeriesTotal = 0;

        foreach (MissionEvent missionEvent in mission.missionEventData)
        {
            if (missionEvent.eventType == "starteventseries")
            {
                eventSeriesTotal++;
            }
        }

        missionManager.eventNo = new int[eventSeriesTotal];

        //This starts running the event series
        int eventSeriesNo = 0;

        foreach (MissionEvent missionEvent in mission.missionEventData)
        {
            if (missionEvent.eventType == "starteventseries")
            {
                FindNextEvent(missionEvent.nextEvent1, eventSeriesNo);
                Task b = new Task(RunEventSeries(mission, eventSeriesNo));
                eventSeriesNo++;
            }
        }
    }

    //This finds the first location in the game and loads it
    public static MissionEvent FindFirstLocationNode(Mission mission)
    {
        MissionEvent missionEvent = null;

        foreach (MissionEvent tempMissionEvent in mission.missionEventData)
        {
            if (tempMissionEvent.eventType == "createlocation")
            {
                if (tempMissionEvent.data1 == "true")
                {
                    missionEvent = tempMissionEvent;
                    LoadScreenFunctions.AddLogToLoadingScreen("Starting location node found", 0);
                    break;
                }
            }
        }

        if (missionEvent == null)
        {
            LoadScreenFunctions.AddLogToLoadingScreen("No starting location node found searching for another location node.", 0);

            foreach (MissionEvent tempMissionEvent in mission.missionEventData)
            {
                if (tempMissionEvent.eventType == "createlocation")
                {
                    missionEvent = tempMissionEvent;
                    LoadScreenFunctions.AddLogToLoadingScreen("Secondary location node found.", 0);
                    break;
                }
            }
        }

        if (missionEvent == null)
        {
            LoadScreenFunctions.AddLogToLoadingScreen("No starting location found aborting load", 0);
        }

        return missionEvent;
    }

    //This finds all the locations you can visit in the mission
    public static void FindAllLocations(Mission mission)
    {
        Scene scene = SceneFunctions.GetScene();

        if (scene.availibleLocations == null)
        {
            scene.availibleLocations = new List<string>();
        }

        foreach (MissionEvent missionEvent in mission.missionEventData)
        {
            if (missionEvent.eventType == "createlocation")
            {
                scene.availibleLocations.Add(missionEvent.conditionLocation);
            }
        }
    }

    //This looks for and runs preload events
    public static IEnumerator FindAndRunPreLoadEvents(Mission mission, string location, float time)
    {
        foreach (MissionEvent missionEvent in mission.missionEventData)
        {
            if (missionEvent.eventType == "preload_loadasteroids" & missionEvent.conditionLocation == location)
            {
                Task a = new Task(LoadAsteroids(missionEvent));
                while (a.Running == true) { yield return null; }
                LoadScreenFunctions.AddLogToLoadingScreen("Asteroids loaded", Time.unscaledTime - time);
            }
            else if (missionEvent.eventType == "preload_loadplanet" & missionEvent.conditionLocation == location)
            {
                LoadScreenFunctions.AddLogToLoadingScreen("Generating unique planet heightmap. This may take a while...", Time.unscaledTime - time);
                Task a = new Task(LoadPlanet());
                while (a.Running == true) { yield return null; }
                LoadScreenFunctions.AddLogToLoadingScreen("Planet loaded", Time.unscaledTime - time);
            }
            else if (missionEvent.eventType == "preload_loadtiles" & missionEvent.conditionLocation == location)
            {
                LoadScreenFunctions.AddLogToLoadingScreen("Loading unique tile configuration. This may take a while...", Time.unscaledTime - time);
                Task a = new Task(LoadTiles(missionEvent));
                while (a.Running == true) { yield return null; }
                LoadScreenFunctions.AddLogToLoadingScreen("Tiles loaded", Time.unscaledTime - time);
            }
            else if (missionEvent.eventType == "preload_loadmultipleshipsonground" & missionEvent.conditionLocation == location)
            {
                Task a = new Task(LoadMultipleShipsOnGround(missionEvent));
                while (a.Running == true) { yield return null; }
                LoadScreenFunctions.AddLogToLoadingScreen("Multiple ships loaded on the ground", Time.unscaledTime - time);
            }
            else if (missionEvent.eventType == "preload_loadsingleship" & missionEvent.conditionLocation == location)
            {
                LoadSingleShip(missionEvent);
                LoadScreenFunctions.AddLogToLoadingScreen("Single ship created", Time.unscaledTime - time);
            }
            else if (missionEvent.eventType == "preload_loadmultipleships" & missionEvent.conditionLocation == location)
            {
                Task a = new Task(LoadMultipleShips(missionEvent));
                while (a.Running == true) { yield return null; }
                LoadScreenFunctions.AddLogToLoadingScreen("Batch of ships created by name", Time.unscaledTime - time);
            }
            else if (missionEvent.eventType == "preload_loadmultipleshipsbyclassandallegiance" & missionEvent.conditionLocation == location)
            {
                Task a = new Task(LoadMultipleShipsByClassAndAllegiance(missionEvent));
                while (a.Running == true) { yield return null; }
                LoadScreenFunctions.AddLogToLoadingScreen("Batch of ships created by type and allegiance", Time.unscaledTime - time);
            }
            else if (missionEvent.eventType == "preload_setsceneradius" & missionEvent.conditionLocation == location)
            {
                SetSceneRadius(missionEvent);
                LoadScreenFunctions.AddLogToLoadingScreen("Scene radius set", Time.unscaledTime - time);
            }
            else if (missionEvent.eventType == "preload_setskybox" & missionEvent.conditionLocation == location)
            {
                SetSkyBox(missionEvent);
                LoadScreenFunctions.AddLogToLoadingScreen("Skybox set", Time.unscaledTime - time);
            }
        }
    }

    //This runs a series of events
    public static IEnumerator RunEventSeries(Mission mission, int eventSeries)
    {
        MissionManager missionManager = GameObject.FindObjectOfType<MissionManager>();

        while (missionManager.running == true)
        {
            MissionEvent missionEvent = mission.missionEventData[missionManager.eventNo[eventSeries]];

            float markTime = Time.time;

            if (!missionEvent.eventType.Contains("preload") & missionEvent.eventType != "loadscene")
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
                    if (missionEvent.conditionLocation != missionManager.scene.currentLocation)
                    {
                        float startPause = Time.time;
                        yield return new WaitUntil(() => missionEvent.conditionLocation == missionManager.scene.currentLocation);
                    }
                }

                RunEvent(missionEvent, eventSeries);
            }

            //This makes sure the mission that mission events aren't run when the mission manager has been deleted
            if (missionManager == null || missionManager.eventNo[eventSeries] == 11111)
            {
                break;
            }

            yield return null;
        }
    }

    //This looks for the next event to run
    public static void FindNextEvent(string nextEvent, int eventSeries)
    {
        MissionManager missionManager = GameObject.FindObjectOfType<MissionManager>();

        if (nextEvent != "none" & missionManager != null)
        {
            Mission mission = null;

            mission = JsonUtility.FromJson<Mission>(missionManager.missionData);

            if (mission == null)
            {
                missionManager.running = false;
            }
            else
            {
                int i = 0;

                foreach (MissionEvent missionEvent in mission.missionEventData)
                {
                    if (missionEvent.eventID == nextEvent)
                    {
                        missionManager.eventNo[eventSeries] = i;
                        break;
                    }

                    i++;
                }
            }
        }
        else if (missionManager != null)
        {
            missionManager.eventNo[eventSeries] = 11111;
        }
    }

    //This runs the appropriate event function
    public static void RunEvent(MissionEvent missionEvent, int eventSeries)
    {
        //This runs the requested function
        if (missionEvent.eventType == "activatehyperspace")
        {
            ActivateHyperspace(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "clearaioverride")
        {
            ClearAIOverride(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "exitmission")
        {
            ExitMission();
        }
        else if (missionEvent.eventType == "deactivateship")
        {
            DeactivateShip(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "displaydialoguebox")
        {
            DisplayDialogueBox(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "displaylargemessage")
        {
            DisplayLargeMessage(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "displaylocation")
        {
            DisplayLocation(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "displaymessage")
        {
            DisplayMessage(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "displaymissionbriefing")
        {
            DisplayMissionBriefing(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "ifshipshullislessthan")
        {
            bool isLessThan = IfShipsHullIsLessThan(missionEvent);

            if (isLessThan == true)
            {
                FindNextEvent(missionEvent.nextEvent1, eventSeries);
            }
            else
            {
                FindNextEvent(missionEvent.nextEvent2, eventSeries);
            }
        }
        else if (missionEvent.eventType == "ifshipislessthandistancetoothership")
        {
            bool isLessThanDistance = IfShipIsLessThanDistanceToOtherShip(missionEvent);

            if (isLessThanDistance == true)
            {
                FindNextEvent(missionEvent.nextEvent1, eventSeries);
            }
            else
            {
                FindNextEvent(missionEvent.nextEvent2, eventSeries);
            }
        }
        else if (missionEvent.eventType == "ifshipislessthandistancetowaypoint")
        {
            bool isLessThanDistance = IfShipIsLessThanDistanceToWaypoint(missionEvent);

            if (isLessThanDistance == true)
            {
                FindNextEvent(missionEvent.nextEvent1, eventSeries);
            }
            else
            {
                FindNextEvent(missionEvent.nextEvent2, eventSeries);
            }
        }     
        else if (missionEvent.eventType == "ifshipisactive")
        {
            bool shipTypeIsActive = IfShipIsActive(missionEvent);

            if (shipTypeIsActive == true)
            {
                FindNextEvent(missionEvent.nextEvent1, eventSeries);
            }
            else
            {
                FindNextEvent(missionEvent.nextEvent2, eventSeries);
            }
        }
        else if (missionEvent.eventType == "ifshiphasbeenscanned")
        {
            bool shiphasbeenscanned = IfShipHasBeenScanned(missionEvent);

            if (shiphasbeenscanned == true)
            {
                FindNextEvent(missionEvent.nextEvent1, eventSeries);
            }
            else
            {
                FindNextEvent(missionEvent.nextEvent2, eventSeries);
            }
        }
        else if (missionEvent.eventType == "ifshiphasntbeenscanned")
        {
            bool shiphasntbeenscanned = IfShipHasntBeenScanned(missionEvent);

            if (shiphasntbeenscanned == true)
            {
                FindNextEvent(missionEvent.nextEvent1, eventSeries);
            }
            else
            {
                FindNextEvent(missionEvent.nextEvent2, eventSeries);
            }
        }
        else if (missionEvent.eventType == "iftypeofshipisactive")
        {
            bool shipTypeIsActive = IfTypeOfShipIsActive(missionEvent);

            if (shipTypeIsActive == true)
            {
                FindNextEvent(missionEvent.nextEvent1, eventSeries);
            }
            else
            {
                FindNextEvent(missionEvent.nextEvent2, eventSeries);
            }
        }
        else if (missionEvent.eventType == "loadsingleship")
        {
            LoadSingleShip(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "loadsingleshipatdistanceandanglefromplayer")
        {
            LoadSingleShipAtDistanceAndAngleFromPlayer(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "loadmultipleshipsonground")
        {
            Task a = new Task(LoadMultipleShipsOnGround(missionEvent));
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "loadmultipleships")
        {
            Task a = new Task(LoadMultipleShips(missionEvent));
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "loadmultipleshipsbyclassandallegiance")
        {
            Task a = new Task(LoadMultipleShipsByClassAndAllegiance(missionEvent));
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "playmusictrack")
        {
            PlayMusicTrack(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "setaioverride")
        {
            SetAIOverride(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "setcargo")
        {
            SetCargo(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "setobjective")
        {
            SetObjective(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "setdontattacklargeships")
        {
            SetDontAttackLargeShips(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "setshipallegiance")
        {
            SetShipAllegiance(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "setshiptarget")
        {
            SetShipTarget(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "setshiptargettoclosestenemy")
        {
            SetShipTargetToClosestEnemy(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "setshiptoinvincible")
        {
            SetShipToInvincible(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "setwaypoint")
        {
            SetWaypoint(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "setweaponslock")
        {
            SetWeaponsLock(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
    }

    #endregion

    #region main scene loading function

    //This creates the scene 
    public static void LoadScene(string missionName, string missionAddress, bool addressIsExternal)
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
        AudioFunctions.CreateAudioManager(missionAddress + missionName + "_audio/", addressIsExternal);
        LoadScreenFunctions.AddLogToLoadingScreen("Audio Manager created", Time.unscaledTime - time);
        MusicFunctions.CreateMusicManager();
        LoadScreenFunctions.AddLogToLoadingScreen("Music Manager created", Time.unscaledTime - time);
    }

    //This displays the loading screen
    public static void DisplayLoadingScreen(string missionName, bool display)
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
            LoadScreenFunctions.LoadingScreen(true, missionName, messages[randomMessageNo]);
        }
        else
        {
            LoadScreenFunctions.LoadingScreen(false);
        }
    }

    #endregion

    #region event functions

    //This causes the designated ships to jump to hyperspace
    public static void ActivateHyperspace(MissionEvent missionEvent)
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
                            Task a = new Task(SmallShipFunctions.JumpToHyperspace(smallShip));
                        }

                        LargeShip largeShip = ship.GetComponent<LargeShip>();

                        if (largeShip != null)
                        {
                            Task a = new Task(LargeShipFunctions.JumpToHyperspace(largeShip));
                        }

                    }
                }
            }
        }
    }

    //This clears any AI overides on a ship/ships i.e. "waypoint", "dontattack" etc
    public static void ClearAIOverride(MissionEvent missionEvent)
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
                            smallShip.aiOverideMode = "none";
                        }
                    }
                }
            }
        }
    }

    //This exits the mission back to the main menu
    public static void ExitMission()
    {
        MissionManager missionManager = GameObject.FindObjectOfType<MissionManager>();
        
        if (missionManager != null)
        {
            missionManager.running = false;
        }

        ExitMenuFunctions.ExitAndUnload();

    }

    //This deactivates a ship so that it is no longer part of the scene
    public static void DeactivateShip(MissionEvent missionEvent)
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
                            SmallShipFunctions.DeactivateShip(smallShip);
                        }

                        LargeShip largeShip = ship.GetComponent<LargeShip>();

                        if (largeShip != null)
                        {
                            LargeShipFunctions.DeactivateShip(largeShip);
                        }
                    }
                }
            }
        }
    }

    //This displays the dialogue box with a message
    public static void DisplayDialogueBox(MissionEvent missionEvent)
    {
        DialogueBoxFunctions.DisplayDialogueBox(true, missionEvent.data1);
    }

    //This temporary displays a large message in the center of the screen
    public static void DisplayLargeMessage(MissionEvent missionEvent)
    {
        HudFunctions.DisplayLargeMessage(missionEvent.data1);
    }

    //This displays the location + text before and after if desired
    public static void DisplayLocation(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        string textBefore = missionEvent.data1;
        string textAfter = missionEvent.data2;

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
            HudFunctions.DisplayLargeMessage(textBefore.ToUpper() + scene.currentLocation.ToUpper() + textAfter.ToUpper());
        }       
    }

    //This adds a message to the log and can also play an audio file
    public static void DisplayMessage(MissionEvent missionEvent)
    {
        string audio = missionEvent.data2;
        string message = missionEvent.data1;
        string internalAudioFile = missionEvent.data3;

        if (message != "none")
        {
            HudFunctions.AddToShipLog(message);
        }

        if (audio != "none" & internalAudioFile != "true")
        {
            AudioFunctions.PlayMissionAudioClip(null, audio, "Voice", new Vector3(0, 0, 0), 0, 1, 500, 1f, 1);
        }
        else if (audio != "none" & internalAudioFile == "true")
        {
            AudioFunctions.PlayAudioClip(null, audio, "Voice", new Vector3(0, 0, 0), 0, 1, 500, 1f, 1);
        }
    }

    //This displays the mission briefing screen
    public static void DisplayMissionBriefing(MissionEvent missionEvent)
    {
        MissionBriefingFunctions.ActivateMissionBriefing(missionEvent.data1);
    }

    //This checks the ship distance to its waypoint
    public static bool IfShipsHullIsLessThan(MissionEvent missionEvent)
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
                        LargeShip largeShip = ship.GetComponent<LargeShip>();

                        if (smallShip != null)
                        {
                            if (smallShip.hullLevel < hullLevel)
                            {
                                islessthanamount = true;
                                break;
                            }
                        }
                        else if (largeShip != null)
                        {
                            if (largeShip.hullLevel < hullLevel)
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

    //This checks the distance between two different ships
    public static bool IfShipIsLessThanDistanceToOtherShip(MissionEvent missionEvent)
    {
        bool isLessThanDistance = false;

        Scene scene = SceneFunctions.GetScene();

        string shipA = missionEvent.data1;
        string shipB = missionEvent.data2;
        float distance = Mathf.Infinity;

        if (float.TryParse(missionEvent.data3, out _))
        {
            distance = float.Parse(missionEvent.data3);
        }

        if (scene != null)
        {
            if (scene.objectPool != null)
            {
                foreach (GameObject tempShipA in scene.objectPool)
                {
                    if (tempShipA.name.Contains(shipA))
                    {
                        foreach (GameObject tempShipB in scene.objectPool)
                        {
                            if (tempShipB.name.Contains(shipB))
                            {
                                if (tempShipA.activeSelf == true & tempShipB.activeSelf == true)
                                {
                                    float actualDistance = Vector3.Distance(tempShipA.transform.position, tempShipB.transform.position);

                                    if (actualDistance < distance)
                                    {
                                        isLessThanDistance = true;
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }

        return isLessThanDistance;
    }

    //This checks the ship distance to its waypoint
    public static bool IfShipIsLessThanDistanceToWaypoint(MissionEvent missionEvent)
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

    //This checks whether the requested ship is present and active
    public static bool IfShipIsActive(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        bool shipIsActive = false;

        if (scene != null)
        {
            if (scene.objectPool != null)
            {
                foreach (GameObject ship in scene.objectPool)
                {
                    if (ship != null)
                    {
                        if (ship.name.Contains(missionEvent.data1))
                        {
                            if (ship.activeSelf == true)
                            {
                                shipIsActive = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        return shipIsActive;
    }

    //This checks a single ship as to whether it has been scanned
    public static bool IfShipHasBeenScanned(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        bool shipHasBeenScanned = false;

        if (scene != null)
        {
            if (scene.objectPool != null)
            {
                foreach (GameObject ship in scene.objectPool)
                {
                    if (ship != null)
                    {
                        if (ship.name.Contains(missionEvent.data1))
                        {
                            SmallShip smallShip = ship.GetComponent<SmallShip>();
                            LargeShip largeShip = ship.GetComponent<LargeShip>();

                            if (smallShip != null)
                            {
                                if (smallShip.scanned == true)
                                {
                                    shipHasBeenScanned = true;
                                    break;
                                }
                            }
                            else if (largeShip != null)
                            {
                                if (largeShip.scanned == true)
                                {
                                    shipHasBeenScanned = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        return shipHasBeenScanned;
    }

    //This checks a group of ships to check if one has not been scanned
    public static bool IfShipHasntBeenScanned(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        bool shipHasntBeenScanned = false;

        if (scene != null)
        {
            if (scene.objectPool != null)
            {
                foreach (GameObject ship in scene.objectPool)
                {
                    if (ship != null)
                    {
                        if (ship.name.Contains(missionEvent.data1))
                        {
                            SmallShip smallShip = ship.GetComponent<SmallShip>();
                            LargeShip largeShip = ship.GetComponent<LargeShip>();

                            if (smallShip != null)
                            {
                                if (smallShip.scanned == false)
                                {
                                    shipHasntBeenScanned = true;
                                    break;
                                }
                            }
                            else if (largeShip != null)
                            {
                                if (largeShip.scanned == false)
                                {
                                    shipHasntBeenScanned = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        return shipHasntBeenScanned;
    }

    //This checks whether there are any active ships of a certain allegiance, i.e. are there any imperial ships left
    public static bool IfTypeOfShipIsActive(MissionEvent missionEvent)
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
                        if (ship.activeSelf == true & smallShip.allegiance == missionEvent.data1)
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

    //This loads the asteroid field
    public static IEnumerator LoadAsteroids(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        if (missionEvent.data1 != "none" & missionEvent.data1 != "0")
        {
            Task b = new Task(SceneFunctions.GenerateAsteroidField(scene.planetSeed, true, int.Parse(missionEvent.data1)));
            while (b.Running == true) { yield return null; }
        }
        else
        {
            Task b = new Task(SceneFunctions.GenerateAsteroidField(scene.planetSeed));
            while (b.Running == true) { yield return null; }
        }
    }

    //This loads a planet in the scene
    public static IEnumerator LoadPlanet()
    {
        Scene scene = SceneFunctions.GetScene();
        Task a = new Task(SceneFunctions.GeneratePlanetHeightmap(scene.planetType, scene.planetSeed));
        while (a.Running == true) { yield return null; }
        SceneFunctions.SetPlanetDistance(scene.planetSeed);
    }

    //This loads a single ship by name
    public static void LoadSingleShip(MissionEvent missionEvent)
    {
        float x = missionEvent.x;
        float y = missionEvent.y;
        float z = missionEvent.z;

        Vector3 position = new Vector3(x, y, z);

        float xRotation = missionEvent.xRotation;
        float yRotation = missionEvent.yRotation;
        float zRotation = missionEvent.zRotation;

        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, zRotation);

        string type = "tiefighter";
        if (missionEvent.data1 != "none") { type = missionEvent.data1; }

        string name = "alpha";
        if (missionEvent.data2 != "none") { name = missionEvent.data2; }

        string allegiance = "imperial";
        if (missionEvent.data3 != "none") { allegiance = missionEvent.data3; }

        string cargo = "no cargo";
        if (missionEvent.data4 != "none") { cargo = missionEvent.data4; }

        bool exitingHyperspace = false;

        if (bool.TryParse(missionEvent.data5, out _))
        {
            exitingHyperspace = bool.Parse(missionEvent.data5);
        }

        bool isAI = false;

        if (bool.TryParse(missionEvent.data6, out _))
        {
            isAI = bool.Parse(missionEvent.data6);
        }

        SceneFunctions.LoadSingleShip(position, rotation, type, name, allegiance, cargo, exitingHyperspace, isAI, false);
    }

    //This loads a single ship at a certain distance and angle from the player
    public static void LoadSingleShipAtDistanceAndAngleFromPlayer(MissionEvent missionEvent)
    {
        float xEulerAngle = missionEvent.x;
        float yEulerAngle = missionEvent.y;
        float zEulerAngle = missionEvent.z;

        Quaternion angle = Quaternion.Euler(xEulerAngle, yEulerAngle, zEulerAngle);

        float xRotation = missionEvent.xRotation;
        float yRotation = missionEvent.yRotation;
        float zRotation = missionEvent.zRotation;

        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, zRotation);

        string type = "tiefighter";
        if (missionEvent.data1 != "none") { type = missionEvent.data1; }

        string name = "alpha";
        if (missionEvent.data2 != "none") { name = missionEvent.data2; }

        string allegiance = "imperial";
        if (missionEvent.data3 != "none") { allegiance = missionEvent.data3; }

        string cargo = "no cargo";
        if (missionEvent.data4 != "none") { cargo = missionEvent.data4; }

        float distance = 1000;

        if (float.TryParse(missionEvent.data5, out _))
        {
            distance = float.Parse(missionEvent.data5);
        }

        bool exitingHyperspace = false;

        if (bool.TryParse(missionEvent.data6, out _))
        {
            exitingHyperspace = bool.Parse(missionEvent.data6);
        }

        Scene scene = SceneFunctions.GetScene();

        Vector3 newPosition = new Vector3(0, 0, 0);

        if (scene != null)
        {
            if (scene.mainShip != null)
            {
                newPosition = (angle * scene.mainShip.transform.forward).normalized * distance;
            }
        }

        SceneFunctions.LoadSingleShip(newPosition, rotation, type, name, allegiance, cargo, exitingHyperspace, true, false);
    }

    //This loads multiple ships by name
    public static IEnumerator LoadMultipleShipsOnGround(MissionEvent missionEvent)
    {
        float x = missionEvent.x;
        float y = missionEvent.y;
        float z = missionEvent.z;

        Vector3 position = new Vector3(x, y, z);

        float xRotation = missionEvent.xRotation;
        float yRotation = missionEvent.yRotation;
        float zRotation = missionEvent.zRotation;

        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, zRotation);

        string type = "dsturrettall";
        if (missionEvent.data1 != "none") { type = missionEvent.data1; }

        string name = "tower";
        if (missionEvent.data2 != "none") { name = missionEvent.data2; }

        string allegiance = "imperial";
        if (missionEvent.data3 != "none") { allegiance = missionEvent.data3; }

        string cargo = "no cargo";
        if (missionEvent.data4 != "none") { cargo = missionEvent.data4; }

        int number = 1;

        if (int.TryParse(missionEvent.data5, out _))
        {
            number = int.Parse(missionEvent.data5);
        }

        string pattern = "rectanglehorizontal";
        if (missionEvent.data6 != "none") { pattern = missionEvent.data6; }

        float width = 1000;

        if (float.TryParse(missionEvent.data7, out _))
        {
            width = float.Parse(missionEvent.data7);
        }

        float length = 1000;

        if (float.TryParse(missionEvent.data8, out _))
        {
            length = float.Parse(missionEvent.data8);
        }

        float distanceAboveGround = 0;

        if (float.TryParse(missionEvent.data9, out _))
        {
            distanceAboveGround = float.Parse(missionEvent.data9);
        }

        int shipsPerLine = 1;

        if (int.TryParse(missionEvent.data10, out _))
        {
            shipsPerLine = int.Parse(missionEvent.data10);
        }

        float positionVariance = 10;

        if (float.TryParse(missionEvent.data11, out _))
        {
            positionVariance = float.Parse(missionEvent.data11);
        }

        bool ifRaycastFailsStillLoad = false;

        if (bool.TryParse(missionEvent.data13, out _))
        {
            ifRaycastFailsStillLoad = bool.Parse(missionEvent.data13);
        }

        Task c = new Task(SceneFunctions.LoadMultipleShipsOnGround(position, rotation, type, name, allegiance, cargo, number, length, width, distanceAboveGround, shipsPerLine, positionVariance, ifRaycastFailsStillLoad));
        while (c.Running == true) { yield return null; }
    }

    //This loads multiple ships by name
    public static IEnumerator LoadMultipleShips(MissionEvent missionEvent)
    {
        float x = missionEvent.x;
        float y = missionEvent.y;
        float z = missionEvent.z;

        Vector3 position = new Vector3(x, y, z);

        float xRotation = missionEvent.xRotation;
        float yRotation = missionEvent.yRotation;
        float zRotation = missionEvent.zRotation;

        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, zRotation);

        string type = "tiefighter";
        if (missionEvent.data1 != "none") { type = missionEvent.data1; }

        string name = "alpha";
        if (missionEvent.data2 != "none") { name = missionEvent.data2; }

        string allegiance = "imperial";
        if (missionEvent.data3 != "none") { allegiance = missionEvent.data3; }

        string cargo = "no cargo";
        if (missionEvent.data4 != "none") { cargo = missionEvent.data4; }

        int number = 1;

        if (int.TryParse(missionEvent.data5, out _))
        {
            number = int.Parse(missionEvent.data5);
        }

        string pattern = "rectanglehorizontal";
        if (missionEvent.data6 != "none") {pattern = missionEvent.data6; }

        float width = 1000;

        if (float.TryParse(missionEvent.data7, out _))
        {
            width = float.Parse(missionEvent.data7);
        }

        float length = 1000;

        if (float.TryParse(missionEvent.data8, out _))
        {
            length = float.Parse(missionEvent.data8);
        }

        float height = 1000;

        if (float.TryParse(missionEvent.data9, out _))
        {
            height = float.Parse(missionEvent.data9);
        }

        int shipsPerLine = 1;

        if (int.TryParse(missionEvent.data10, out _))
        {
            shipsPerLine = int.Parse(missionEvent.data10);
        }

        float positionVariance = 10;

        if (float.TryParse(missionEvent.data11, out _))
        {
            positionVariance = float.Parse(missionEvent.data11);
        }

        bool exitingHyperspace = false;

        if (bool.TryParse(missionEvent.data12, out _))
        {
            exitingHyperspace = bool.Parse(missionEvent.data12);
        }

        bool includePlayer = false;

        if (bool.TryParse(missionEvent.data13, out _))
        {
            includePlayer = bool.Parse(missionEvent.data13);
        }

        int playerNo = 0;

        if (int.TryParse(missionEvent.data14, out _))
        {
            playerNo = int.Parse(missionEvent.data14);
        }

        if (playerNo > number - 1)
        {
            playerNo = number - 1;
        }

        Task c = new Task(SceneFunctions.LoadMultipleShips(position, rotation, type, name, allegiance, cargo, number, pattern, width, length, height, shipsPerLine, positionVariance, exitingHyperspace, includePlayer, playerNo));
        while (c.Running == true) { yield return null; }
    }

    //This loads multiple ships by type and allegiance
    public static IEnumerator LoadMultipleShipsByClassAndAllegiance(MissionEvent missionEvent)
    {
        float x = missionEvent.x;
        float y = missionEvent.y;
        float z = missionEvent.z;

        Vector3 position = new Vector3(x, y, z);

        float xRotation = missionEvent.xRotation;
        float yRotation = missionEvent.yRotation;
        float zRotation = missionEvent.zRotation;

        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, zRotation);

        string shipClass = "fighter";
        if (missionEvent.data1 != "none") { shipClass = missionEvent.data1; }

        string name = "alpha";
        if (missionEvent.data2 != "none") { name = missionEvent.data2; }

        string allegiance = "imperial";
        if (missionEvent.data3 != "none") { allegiance = missionEvent.data3; }

        string cargo = "no cargo";
        if (missionEvent.data4 != "none") { cargo = missionEvent.data4; }

        int number = 1;

        if (int.TryParse(missionEvent.data5, out _))
        {
            number = int.Parse(missionEvent.data5);
        }

        string pattern = "rectanglehorizontal";
        if (missionEvent.data6 != "none") { pattern = missionEvent.data6; }

        float width = 1000;

        if (float.TryParse(missionEvent.data7, out _))
        {
            width = float.Parse(missionEvent.data7);
        }

        float length = 1000;

        if (float.TryParse(missionEvent.data8, out _))
        {
            length = float.Parse(missionEvent.data8);
        }

        float height = 1000;

        if (float.TryParse(missionEvent.data9, out _))
        {
            height = float.Parse(missionEvent.data9);
        }

        int shipsPerLine = 1;

        if (int.TryParse(missionEvent.data10, out _))
        {
            shipsPerLine = int.Parse(missionEvent.data10);
        }

        float positionVariance = 10;

        if (float.TryParse(missionEvent.data11, out _))
        {
            positionVariance = float.Parse(missionEvent.data11);
        }

        bool exitingHyperspace = false;

        if (bool.TryParse(missionEvent.data12, out _))
        {
            exitingHyperspace = bool.Parse(missionEvent.data12);
        }

        bool includePlayer = false;

        if (bool.TryParse(missionEvent.data13, out _))
        {
            includePlayer = bool.Parse(missionEvent.data13);
        }

        int playerNo = 0;

        if (int.TryParse(missionEvent.data14, out _))
        {
            playerNo = int.Parse(missionEvent.data14);
        }

        if (playerNo > number - 1)
        {
            playerNo = number - 1;
        }

        Task c = new Task(SceneFunctions.LoadMultipleShipByClassAndAllegiance(position, rotation, shipClass, name, allegiance, cargo, number, pattern, width, length, height, shipsPerLine, positionVariance, exitingHyperspace, includePlayer, playerNo));
        while (c.Running == true) { yield return null; }
    }

    //This loads a tile set for ground / other scenery
    public static IEnumerator LoadTiles(MissionEvent missionEvent)
    {
        string type = missionEvent.data1;
        int distanceX = (int)missionEvent.x;
        int distanceY = (int)missionEvent.y;
        int tileSize = (int)missionEvent.z;
        int seed = Random.Range(0, 5000);

        if (int.TryParse(missionEvent.data2, out _))
        {
            seed = int.Parse(missionEvent.data2);
        }

        Task a = new Task(SceneFunctions.GenerateTiles(type, distanceX, distanceY, tileSize, seed));
        while (a.Running == true) { yield return null; }
    }

    //This changes the type of music that is playing
    public static void PlayMusicTrack(MissionEvent missionEvent)
    {
        Music musicManager = GameObject.FindObjectOfType<Music>();

        string track = missionEvent.data1;
        bool loop = false;

        if (bool.TryParse(missionEvent.data2, out _))
        {
            loop = bool.Parse(missionEvent.data2);
        }

        if (musicManager != null)
        {
            Task a = new Task(MusicFunctions.PlayMusicTrack(musicManager, track, loop));
        }       
    }

    //This manually sets the ai flight mode
    public static void SetAIOverride(MissionEvent missionEvent)
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
                            smallShip.aiOverideMode = missionEvent.data2;
                        }
                    }
                }
            }
        }
    }

    //This sets the size of the play area
    public static void SetSceneRadius(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        float sceneRadius = ParseStringToFloat(missionEvent.data1);

        if (scene != null)
        {
            scene.sceneRadius = sceneRadius;
        }
    }

    //This changes the selected ship/s cargo
    public static void SetCargo(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        string cargo = missionEvent.data2;

        if (cargo.ToLower().Contains("random") || cargo.ToLower().Contains("randomise"))
        {
            string[] cargoTypes = SceneFunctions.GetCargoTypesList();
            int cargoTypeNo = cargoTypes.Length;
            int randomChoice = Random.Range(0, cargoTypeNo - 1);
            cargo = cargoTypes[randomChoice];
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
                            smallShip.cargo = cargo;
                        }

                        LargeShip largeShip = ship.GetComponent<LargeShip>();

                        if (largeShip != null)
                        {
                            largeShip.cargo = cargo;
                        }

                    }
                }
            }
        }
    }

    //This sets the designated ships target, provided both the ship and its target can be found
    public static void SetDontAttackLargeShips(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        if (bool.TryParse(missionEvent.data2, out _))
        {
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
                                smallShip.dontSelectLargeShips = bool.Parse(missionEvent.data2);
                            }
                        }
                    }
                }
            }
        }   
    }

    //This sets the galaxy camera position
    public static void SetGalaxyLocation(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        string location = missionEvent.conditionLocation;

        var planetData = SceneFunctions.GetSpecificLocation(location);
        scene.currentLocation = planetData.planet;
        scene.planetType = planetData.type;
        scene.planetSeed = planetData.seed;
        SceneFunctions.MoveStarfieldCamera(planetData.location);
    }

    //This sets an objective for the player to complete, removes an objective, or clears all objectives.
    public static void SetObjective(MissionEvent missionEvent)
    {
        MissionManager missionManager = GameObject.FindObjectOfType<MissionManager>();

        string mode = missionEvent.data1;
        string objective = missionEvent.data2;
        List<string> newObjectiveList = new List<string>();

        if (missionManager.objectiveList == null)
        {
            missionManager.objectiveList = new string[0];
        }

        if (missionManager != null)
        {
            if (mode == "addobjective")
            {
                newObjectiveList.Add(objective);

                foreach (string tempObjective in missionManager.objectiveList)
                {
                    newObjectiveList.Add(tempObjective);
                }

                missionManager.objectiveList = newObjectiveList.ToArray();

                HudFunctions.UpdateObjectives(missionManager.objectiveList);
                HudFunctions.AddToShipLog("NEW OBJECTIVE: " + objective);
            }
            else if (mode == "cancelobjective")
            {
                foreach (string tempObjective in missionManager.objectiveList)
                {
                    if (tempObjective != objective)
                    {
                        newObjectiveList.Add(tempObjective);
                    }

                    missionManager.objectiveList = newObjectiveList.ToArray();

                    HudFunctions.UpdateObjectives(missionManager.objectiveList);               
                }

                HudFunctions.AddToShipLog("OBJECTIVE CANCELLED: " + objective);
            }
            else if (mode == "completeobjective")
            {
                foreach (string tempObjective in missionManager.objectiveList)
                {
                    if (tempObjective != objective)
                    {
                        newObjectiveList.Add(tempObjective);
                    }

                    missionManager.objectiveList = newObjectiveList.ToArray();

                    HudFunctions.UpdateObjectives(missionManager.objectiveList);                   
                }

                HudFunctions.AddToShipLog("OBJECTIVE COMPLETED: " + objective);
            }
            else if (mode == "clearallobjectives")
            {
                missionManager.objectiveList = newObjectiveList.ToArray();

                HudFunctions.UpdateObjectives(missionManager.objectiveList);
            }
        }
    }

    //This sets the designated ships target to the closest enemy, provided both the ship can be found
    public static void SetShipAllegiance(MissionEvent missionEvent)
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
                            smallShip.allegiance = missionEvent.data2;
                        }
                    }
                }
            }
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

                        LargeShip largeShip = ship.GetComponent<LargeShip>();

                        if (largeShip != null)
                        {
                            largeShip.invincible = isInvincible;
                        }

                    }
                }
            }
        }
    }

    //This sets the designated ships target, provided both the ship and its target can be found
    public static void SetShipTarget(MissionEvent missionEvent)
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
                            TargetingFunctions.GetSpecificTarget(smallShip, missionEvent.data2);
                        }
                    }
                }
            }
        }
    }

    //This sets the designated ships target to the closest enemy, provided both the ship can be found
    public static void SetShipTargetToClosestEnemy(MissionEvent missionEvent)
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
                            TargetingFunctions.GetClosestEnemy(smallShip, true);
                        }
                    }
                }
            }
        }
    }

    //This sets the skybox
    public static void SetSkyBox(MissionEvent missionEvent)
    {
        SceneFunctions.SetSkybox(missionEvent.data1);
    }

    //This tells a ship or ships to move toward a waypoint by position its waypoint and setting its ai override mode
    public static void SetWaypoint(MissionEvent missionEvent)
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
                            if (smallShip.waypoint != null)
                            {
                                float x = missionEvent.x;
                                float y = missionEvent.y;
                                float z = missionEvent.z;
                                Vector3 waypoint = scene.transform.position + new Vector3(x, y, z);

                                smallShip.waypoint.transform.position = waypoint;
                            }
                        }
                    }
                }
            }
        }
    }

    //This toggles whether a ship can fire their weapons or not
    public static void SetWeaponsLock(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        bool isLocked = false;

        if (missionEvent.data2 != "none")
        {
            isLocked = bool.Parse(missionEvent.data2);
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
                            smallShip.weaponsLock = isLocked;
                        }

                        LargeShip largeShip = ship.GetComponent<LargeShip>();

                        if (largeShip != null)
                        {
                            largeShip.weaponsLock = isLocked;
                        }
                    }
                }
            }
        }
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
                        Cursor.visible = true;
                        Cursor.lockState = CursorLockMode.None;
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

    //This parses a string to a float
    public static float ParseStringToFloat(string data)
    {
        float number = 0;

        if (float.TryParse(data, out _))
        {
            number = float.Parse(data);
        }

        return number;
    }

    //This parses a string to an int
    public static int ParseStringToInt(string data)
    {
        int number = 0;

        if (int.TryParse(data, out _))
        {
            number = int.Parse(data);
        }

        return number;
    }

    //This parses a string to a bool
    public static bool ParseStringToBool(string data)
    {
        bool option = false;

        if (bool.TryParse(data, out _))
        {
            option = bool.Parse(data);
        }

        return option;
    }

    #endregion
}
