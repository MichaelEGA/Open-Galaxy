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

        while (missionManager.audioLoaded == false)
        {
            yield return null;
        }

        //This sets the skybox to the default space and view distance
        SceneFunctions.SetSkybox("space_black", true);
        RenderSettings.fogEndDistance = 40000;
        RenderSettings.fogStartDistance = 30000;

        //This finds and sets the first location
        MissionEvent firstLocationNode = FindFirstLocationNode(mission);

        //This finds all the location you can jump to in the mission
        FindAllLocations(mission);

        //This sets the current location
        scene.currentLocation = firstLocationNode.conditionLocation;

        //This runs all the preload events like loading the planet and asteroids and objects already in scene
        Task a = new Task(FindAndRunPreLoadEvents(mission, firstLocationNode.conditionLocation, time, true));
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

        missionManager.eventNo = new List<int>();

        //This starts running the event series
        int eventSeriesNo = 0;

        if (missionManager.missionTasks == null)
        {
            missionManager.missionTasks = new List<Task>();
        }

        foreach (MissionEvent missionEvent in mission.missionEventData)
        {
            if (missionEvent.eventType == "starteventseries")
            {
                missionManager.eventNo.Add(0); //The zero means nothing
                FindNextEvent(missionEvent.nextEvent1, eventSeriesNo);
                Task b = new Task(RunEventSeries(mission, eventSeriesNo));
                missionManager.missionTasks.Add(b);
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
    public static IEnumerator FindAndRunPreLoadEvents(Mission mission, string location, float savedTime, bool firstRun = false)
    {
        //This preloads all scene objects first
        foreach (MissionEvent missionEvent in mission.missionEventData)
        {
            if (missionEvent.eventType == "preload_loadplanet" & missionEvent.conditionLocation == location)
            {
                LoadScreenFunctions.AddLogToLoadingScreen("Generating unique planet heightmap. This may take a while...", Time.unscaledTime - savedTime);
                LoadPlanet(missionEvent);
                LoadScreenFunctions.AddLogToLoadingScreen("Planet loaded", savedTime);
            }
            else if (missionEvent.eventType == "preload_sethudcolour" & missionEvent.conditionLocation == location)
            {
                SetHudColour(missionEvent);
                LoadScreenFunctions.AddLogToLoadingScreen("Hud Colour Set", savedTime);
            }
            else if (missionEvent.eventType == "preload_setsceneradius" & missionEvent.conditionLocation == location)
            {
                SetSceneRadius(missionEvent);
                LoadScreenFunctions.AddLogToLoadingScreen("Scene radius set", savedTime);
            }
            else if (missionEvent.eventType == "preload_setskybox" & missionEvent.conditionLocation == location)
            {
                SetSkyBox(missionEvent);
                LoadScreenFunctions.AddLogToLoadingScreen("Skybox set", savedTime);
            }
            else if (missionEvent.eventType == "preload_loadasteroids" & missionEvent.conditionLocation == location)
            {
                Task a = new Task(LoadAsteroids(missionEvent));
                while (a.Running == true) { yield return null; }
                LoadScreenFunctions.AddLogToLoadingScreen("Asteroids loaded", savedTime);
            }
        }

        //Then this preloads all the objects in the scene
        foreach (MissionEvent missionEvent in mission.missionEventData)
        {
            if (missionEvent.eventType == "preload_loadmultipleshipsonground" & missionEvent.conditionLocation == location)
            {
                Task a = new Task(LoadMultipleShipsOnGround(missionEvent));
                while (a.Running == true) { yield return null; }
                LoadScreenFunctions.AddLogToLoadingScreen("Multiple ships loaded on the ground", savedTime);  
            }
            else if (missionEvent.eventType == "preload_loadsingleshipsonground" & missionEvent.conditionLocation == location)
            {
                if (firstRun == false & missionEvent.data10 == "false" || firstRun == false & missionEvent.data10 == "none" || firstRun == true)
                {
                    LoadSingleShipOnGround(missionEvent);
                    LoadScreenFunctions.AddLogToLoadingScreen("Single ship loaded on the ground", savedTime);
                }   
            }
            else if (missionEvent.eventType == "preload_loadsingleship" & missionEvent.conditionLocation == location)
            {
                //This extra check is run to prevent the game loading the player ship twice
                if (firstRun == false & missionEvent.data8 == "false" || firstRun == false & missionEvent.data8 == "none" || firstRun == true)
                {
                    LoadSingleShip(missionEvent);
                    LoadScreenFunctions.AddLogToLoadingScreen("Single ship created", savedTime);
                } 
            }
            else if (missionEvent.eventType == "preload_loadmultipleships" & missionEvent.conditionLocation == location)
            {
                Task a = new Task(LoadMultipleShips(missionEvent));
                while (a.Running == true) { yield return null; }
                LoadScreenFunctions.AddLogToLoadingScreen("Batch of ships created by name", savedTime);                 
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

            //This pauses the event series if needed 
            yield return new WaitUntil(() => missionManager.pauseEventSeries == false);

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

                RunEvent(missionManager, missionEvent, eventSeries);
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
            missionManager.eventNo[eventSeries] = 11111; //This code is used to cancel events
        }
    }

    //This runs the appropriate event function
    public static void RunEvent(MissionManager missionManager, MissionEvent missionEvent, int eventSeries)
    {
        //This runs the requested function
        if (missionEvent.eventType == "spliteventseries")
        {
            SplitEventSeries(missionEvent, eventSeries);
        }
        else if (missionEvent.eventType == "activatedocking")
        {
            ActivateDocking(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "activatehyperspace")
        {
            ActivateHyperspace(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "addaitagtolargeship")
        {
            AddAITagToLargeShip(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "addaitagtosmallship")
        {
            AddAITagToSmallShip(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "changelocation")
        {
            Task a = new Task(ChangeLocation(missionEvent));
            missionManager.missionTasks.Add(a);
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
        else if (missionEvent.eventType == "displayhint")
        {
            DisplayHint(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "displaytitle")
        {
            DisplayTitle(missionEvent);
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
        else if (missionEvent.eventType == "ifobjectiveisactive")
        {
            bool ifObjectiveIsActive = IfObjectiveIsActive(missionEvent);

            if (ifObjectiveIsActive == true)
            {
                FindNextEvent(missionEvent.nextEvent1, eventSeries);
            }
            else
            {
                FindNextEvent(missionEvent.nextEvent2, eventSeries);
            }
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
        else if (missionEvent.eventType == "ifshiphasbeendisabled")
        {
            bool shipHasBeenDisabled = IfShipHasBeenDisabled(missionEvent);

            if (shipHasBeenDisabled == true)
            {
                FindNextEvent(missionEvent.nextEvent1, eventSeries);
            }
            else
            {
                FindNextEvent(missionEvent.nextEvent2, eventSeries);
            }
        }
        else if (missionEvent.eventType == "ifshiphasntbeendisabled")
        {
            bool shiphasntbeendisabled = IfShipHasntBeenDisabled(missionEvent);

            if (shiphasntbeendisabled == true)
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
        else if (missionEvent.eventType == "ifshipofallegianceisactive")
        {
            bool shipTypeIsActive = IfShipOfAllegianceIsActive(missionEvent);

            if (shipTypeIsActive == true)
            {
                FindNextEvent(missionEvent.nextEvent1, eventSeries);
            }
            else
            {
                FindNextEvent(missionEvent.nextEvent2, eventSeries);
            }
        }
        else if (missionEvent.eventType == "ifshipssystemsarelessthan")
        {
            bool isLessThan = IfShipsSystemsAreLessThan(missionEvent);

            if (isLessThan == true)
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
        else if (missionEvent.eventType == "loadsingleshiponground")
        {
            LoadSingleShipOnGround(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "loadmultipleships")
        {
            Task a = new Task(LoadMultipleShips(missionEvent));
            missionManager.missionTasks.Add(a);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "loadmultipleshipsfromhangar")
        {
            Task a = new Task(LoadMultipleShipsFromHangar(missionEvent));
            missionManager.missionTasks.Add(a);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "loadmultipleshipsonground")
        {
            Task a = new Task(LoadMultipleShipsOnGround(missionEvent));
            missionManager.missionTasks.Add(a);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }      
        else if (missionEvent.eventType == "pausesequence")
        {
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "playmusictrack")
        {
            PlayMusicTrack(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "setcargo")
        {
            SetCargo(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "setcontrollock")
        {
            SetControlLock(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "setfollowtarget")
        {
            SetFollowTarget(missionEvent);
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
        else if (missionEvent.eventType == "setshiplevels")
        {
            SetShipLevels(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "setshipstats")
        {
            SetShipStats(missionEvent);
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
        else if (missionEvent.eventType == "setshiptocannotbedisabled")
        {
            SetShipToCannotBeDisabled(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "setshiptoinvincible")
        {
            SetShipToInvincible(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "settorpedoes")
        {
            SetTorpedoes(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "setwaypoint")
        {
            SetWaypoint(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "setwaypointtoship")
        {
            SetWaypointToShip(missionEvent);
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

    //This splits an event series in two
    public static void SplitEventSeries(MissionEvent missionEvent, int eventSeries)
    {
        MissionManager missionManager = GetMissionManager();

        missionManager.eventNo[eventSeries] = 11111; //This sets the code to cancel the current event series

        int eventSeriesNo = missionManager.eventNo.Count; //This gets new number for the event series so as to not interfere with other event series

        Mission mission = JsonUtility.FromJson<Mission>(missionManager.missionData);

        if (missionEvent.nextEvent1 != "none")
        {
            missionManager.eventNo.Add(0); //The zero means nothing
            FindNextEvent(missionEvent.nextEvent1, eventSeriesNo);
            Task a = new Task(RunEventSeries(mission, eventSeriesNo));
            missionManager.missionTasks.Add(a);
            eventSeriesNo++;
        }

        if (missionEvent.nextEvent2 != "none")
        {
            missionManager.eventNo.Add(0); //The zero means nothing
            FindNextEvent(missionEvent.nextEvent2, eventSeriesNo);
            Task a = new Task(RunEventSeries(mission, eventSeriesNo));
            missionManager.missionTasks.Add(a);
            eventSeriesNo++;
        }

        if (missionEvent.nextEvent3 != "none")
        {
            missionManager.eventNo.Add(0); //The zero means nothing
            FindNextEvent(missionEvent.nextEvent3, eventSeriesNo);
            Task a = new Task(RunEventSeries(mission, eventSeriesNo));
            missionManager.missionTasks.Add(a);
            eventSeriesNo++;
        }

        if (missionEvent.nextEvent4 != "none")
        {
            missionManager.eventNo.Add(0); //The zero means nothing
            FindNextEvent(missionEvent.nextEvent4, eventSeriesNo);
            Task a = new Task(RunEventSeries(mission, eventSeriesNo));
            missionManager.missionTasks.Add(a);
            eventSeriesNo++;
        }
    }

    //This activates or deactives the docking procedure
    public static void ActivateDocking(MissionEvent missionEvent)
    {
        MissionManager missionManager = GetMissionManager();

        string shipName = missionEvent.data1;
        string targetShipName = missionEvent.data2;
        bool activateDocking = false;
        float rotationSpeed = 1;
        float movementSpeed = 1;

        if (bool.TryParse(missionEvent.data3, out _))
        {
            activateDocking = bool.Parse(missionEvent.data3);
        }

        if (float.TryParse(missionEvent.data4, out _))
        {
            rotationSpeed = float.Parse(missionEvent.data4);
        }

        if (float.TryParse(missionEvent.data5, out _))
        {
            movementSpeed = float.Parse(missionEvent.data5);
        }

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
                            if (activateDocking == true)
                            {
                                Quaternion rotation = Quaternion.Euler(0, 180, 180);
                                DockingPoint targetDockingPoint = DockingFunctions.GetTargetDockingPoint(ship.transform, targetShipName);
                                DockingPoint dockingPoint = DockingFunctions.GetDockingPoint(ship.transform);
                                Task a = new Task(DockingFunctions.StartDocking(ship.transform, dockingPoint, targetDockingPoint, rotation, rotationSpeed, movementSpeed));
                                missionManager.missionTasks.Add(a);
                            }
                            else
                            {
                                DockingPoint targetDockingPoint = DockingFunctions.GetTargetDockingPoint(ship.transform, targetShipName, true);
                                DockingPoint dockingPoint = DockingFunctions.GetDockingPoint(ship.transform, targetDockingPoint.transform, true);
                                Task a = new Task(DockingFunctions.EndDocking(ship.transform, dockingPoint, targetDockingPoint, movementSpeed));
                                missionManager.missionTasks.Add(a);
                            }

                            break;
                        }

                        LargeShip largeShip = ship.GetComponent<LargeShip>();

                        if (largeShip != null)
                        {
                            if (activateDocking == true)
                            {
                                Quaternion rotation = Quaternion.Euler(0, 180, 0);
                                DockingPoint targetDockingPoint = DockingFunctions.GetTargetDockingPoint(ship.transform, targetShipName);
                                DockingPoint dockingPoint = DockingFunctions.GetDockingPoint(ship.transform, targetDockingPoint.transform);
                                Task a = new Task(DockingFunctions.StartDocking(ship.transform, dockingPoint, targetDockingPoint, rotation, rotationSpeed, movementSpeed));
                                missionManager.missionTasks.Add(a);
                            }
                            else
                            {
                                DockingPoint targetDockingPoint = DockingFunctions.GetTargetDockingPoint(ship.transform, targetShipName, true);
                                DockingPoint dockingPoint = DockingFunctions.GetDockingPoint(ship.transform, targetDockingPoint.transform, true);
                                Task a = new Task(DockingFunctions.EndDocking(ship.transform, dockingPoint, targetDockingPoint, movementSpeed));
                                missionManager.missionTasks.Add(a);
                            }

                            break;
                        }
                    }
                }
            }
        }
    }

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

    //This adds an ai tag to a largeship
    public static void AddAITagToLargeShip(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        string speedControlTag = missionEvent.data2;
        string weaponControlTag = missionEvent.data3;
        string flightPatternsTag = missionEvent.data4;

        if (scene != null)
        {
            if (scene.objectPool != null)
            {
                foreach (GameObject ship in scene.objectPool)
                {
                    if (ship.name.Contains(missionEvent.data1))
                    {
                        LargeShip largeShip = ship.GetComponent<LargeShip>();

                        if (largeShip != null)
                        {
                            if (speedControlTag != "none" & speedControlTag != "nochange")
                            {
                                LargeShipAIFunctions.AddTag(largeShip, speedControlTag);
                            }

                            if (weaponControlTag != "none" & weaponControlTag != "nochange")
                            {
                                LargeShipAIFunctions.AddTag(largeShip, weaponControlTag);
                            }

                            if (flightPatternsTag != "none" & flightPatternsTag != "nochange")
                            {
                                LargeShipAIFunctions.AddTag(largeShip, flightPatternsTag);
                            }
                        }
                    }
                }
            }
        }
    }

    //THis adds an ai tag to a smallship
    public static void AddAITagToSmallShip(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        string speedControlTag = missionEvent.data2;
        string weaponControlTag = missionEvent.data3;
        string weaponAccuracyTag = missionEvent.data4;
        string flightPatternsTag = missionEvent.data5;
        string enermyManagementTag = missionEvent.data6;
        string targetingControlTag = missionEvent.data7;

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
                            if (speedControlTag != "none" & speedControlTag != "nochange")
                            {
                                SmallShipAIFunctions.AddTag(smallShip, speedControlTag);
                            }

                            if (weaponControlTag != "none" & weaponControlTag != "nochange")
                            {
                                SmallShipAIFunctions.AddTag(smallShip, weaponControlTag);
                            }

                            if (weaponAccuracyTag != "none" & weaponAccuracyTag != "nochange")
                            {
                                SmallShipAIFunctions.AddTag(smallShip, weaponAccuracyTag);
                            }

                            if (flightPatternsTag != "none" & flightPatternsTag != "nochange")
                            {
                                SmallShipAIFunctions.AddTag(smallShip, flightPatternsTag);
                            }

                            if (enermyManagementTag != "none" & enermyManagementTag != "nochange")
                            {
                                SmallShipAIFunctions.AddTag(smallShip, enermyManagementTag);
                            }

                            if (targetingControlTag != "none" & targetingControlTag != "nochange")
                            {
                                SmallShipAIFunctions.AddTag(smallShip, targetingControlTag);
                            }
                        }
                    }
                }
            }
        }
    }

    //This unloads the current location and loads a new one from the avaiblible locations while simulating a hyperspace jump
    public static IEnumerator ChangeLocation(MissionEvent missionEvent)
    {
        //This gets all the required data from the event node
        float x = missionEvent.x;
        float y = missionEvent.y;
        float z = missionEvent.z;

        float xRotation = missionEvent.xRotation;
        float yRotation = missionEvent.yRotation;
        float zRotation = missionEvent.zRotation;

        string jumpLocation = missionEvent.data1;

        //This gets several important references
        Scene scene = SceneFunctions.GetScene();
        SmallShip smallShip = scene.mainShip.GetComponent<SmallShip>();
        MissionManager missionManager = GetMissionManager();
        Mission mission = JsonUtility.FromJson<Mission>(missionManager.missionData);
        GameObject starfield = SceneFunctions.GetStarfield();

        missionManager.pauseEventSeries = true;

        bool controlLock = smallShip.controlLock; //This preserves the current lock position

        smallShip.controlLock = true; //This locks the player ship controls so the ship remains correctly orientated to the hyperspace effect
        smallShip.invincible = true; //This sets the ship to invincible so that any objects the ship may hit while the scene changes doesn't destroy it

        //This fades out the hud
        Task fadeOut = new Task(HudFunctions.FadeOutHud(1));

        SmallShipFunctions.CloseWings(smallShip);

        yield return new WaitForSecondsRealtime(3.4f);

        //This plays the hyperspace entry sound
        AudioFunctions.PlayAudioClip(smallShip.audioManager, "hyperspace01_entry", "Cockpit", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);

        yield return new WaitForSecondsRealtime(2); //This gives the audio clip time to play

        //This marks the jump time
        float time = Time.unscaledTime;

        //This clears the current location
        SceneFunctions.ClearLocation();

        //This makes the stars stretch out
        scene.planetCamera.GetComponent<Camera>().enabled = false;
        scene.mainCamera.GetComponent<Camera>().enabled = false;

        Time.timeScale = 0;

        //This makes the stars stretch out
        Task a = new Task(SceneFunctions.StretchStarfield());
        while(a.Running == true) { yield return null; }

        smallShip.inHyperspace = true;

        //This activates the hyperspace tunnel
        if(scene.hyperspaceTunnel != null)
        {
            scene.hyperspaceTunnel.SetActive(true);
            SceneFunctions.SetStarfieldToInvisible(true);
        }

        //This sets the rotation of the ship in the new location designated in the node
        smallShip.transform.rotation = Quaternion.Euler(xRotation, yRotation, zRotation);

        //This sets the position of the ship in the new location designated in the node
        smallShip.transform.localPosition = new Vector3(x, y, z);

        //This resets the skybox to black
        SceneFunctions.SetSkybox("space_black", true);

        //This sets the scene location
        scene.currentLocation = jumpLocation;

        //This finds and loads all 'preload' nodes for the new location
        Task b = new Task(MissionFunctions.FindAndRunPreLoadEvents(mission, jumpLocation, time, false));
        while(b.Running == true) { yield return null; }

        yield return new WaitForSecondsRealtime(1);

        //This ensures that hyperspace continues for atleast ten seconds
        while (time + 10 > Time.unscaledTime)
        {
            yield return null;
        }

        //This time scale needs to be turned on before shrinking the starfield to ensure that the function can get the correct velocity angle from the ship
        Time.timeScale = 1;

        yield return new WaitForSecondsRealtime(1); //This gives the rigidbody time to calculate the new velocity

        smallShip.inHyperspace = false;

        //This deactivates the hyperspace tunnel
        if (scene.hyperspaceTunnel != null)
        {
            SceneFunctions.SetStarfieldToInvisible(false);
            scene.hyperspaceTunnel.SetActive(false);
        }

        //This plays the hyperspace exit
        AudioFunctions.PlayAudioClip(smallShip.audioManager, "hyperspace03_exit", "Cockpit", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);

        //This shrinks the starfield
        Task c = new Task(SceneFunctions.ShrinkStarfield());
        while (c.Running == true) { yield return null; }

        //This makes the stars stretch out
        scene.planetCamera.GetComponent<Camera>().enabled = true;
        scene.mainCamera.GetComponent<Camera>().enabled = true;

        //This fades the hud back in
        Task fadeIN = new Task(HudFunctions.FadeInHud(1));

        //This unlocks the player controls and turns off invincibility on the player ship
        smallShip.controlLock = controlLock; //This restores the set lock position
        smallShip.invincible = false;

        yield return new WaitForSecondsRealtime(1);

        SmallShipFunctions.OpenWings(smallShip);

        //This unpauses the event series
        missionManager.pauseEventSeries = false;
    }

    //This exits the mission back to the main menu
    public static void ExitMission()
    {
        MissionManager missionManager = GameObject.FindObjectOfType<MissionManager>();
        
        if (missionManager != null)
        {
            missionManager.running = false;
        }

        MissionFunctions.ExitAndUnload();

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

    //This temporary displays a large print hint in the center bottom of the screen
    public static void DisplayHint(MissionEvent missionEvent)
    {
        string hint = missionEvent.data1;
        int fontsize = 12;

        if (int.TryParse(missionEvent.data2, out _))
        {
            fontsize = int.Parse(missionEvent.data2);
        }

        HudFunctions.DisplayHint(hint, fontsize);
    }

    //This temporary displays a large message in the center of the screen
    public static void DisplayTitle(MissionEvent missionEvent)
    {
        string title = missionEvent.data1;
        int fontsize = 12;
        string colour = missionEvent.data3;

        if (int.TryParse(missionEvent.data2, out _))
        {
            fontsize = int.Parse(missionEvent.data2);
        }

        HudFunctions.DisplayTitle(title, fontsize, colour);
    }

    //This adds a message to the log and can also play an audio file
    public static void DisplayMessage(MissionEvent missionEvent)
    {
        string audio = missionEvent.data2;
        string message = missionEvent.data1;
        string internalAudioFile = missionEvent.data3;

        bool distortion = false;

        if (bool.TryParse(missionEvent.data4, out _))
        {
            distortion = bool.Parse(missionEvent.data4);
        }

        float distortionLevel = 0.5f;

        if (float.TryParse(missionEvent.data5, out _))
        {
            distortionLevel = float.Parse(missionEvent.data5);
        }

        if (message != "none")
        {
            HudFunctions.AddToShipLog(message);
        }

        if (audio != "none" & internalAudioFile != "true")
        {
            AudioFunctions.PlayMissionAudioClip(null, audio, "Voice", new Vector3(0, 0, 0), 0, 1, 500, 1f, 1, distortion, distortionLevel, true);
        }
        else if (audio != "none" & internalAudioFile == "true")
        {
            AudioFunctions.PlayMissionAudioClip(null, audio, "Voice", new Vector3(0, 0, 0), 0, 1, 500, 1f, 1, distortion, distortionLevel, false);
        }
    }

    //This displays the mission briefing screen
    public static void DisplayMissionBriefing(MissionEvent missionEvent)
    {
        string audio = missionEvent.data2;
        string message = missionEvent.data1;
        string internalAudioFile = missionEvent.data3;

        bool distortion = false;

        if (bool.TryParse(missionEvent.data4, out _))
        {
            distortion = bool.Parse(missionEvent.data4);
        }

        float distortionLevel = 0.5f;

        if (float.TryParse(missionEvent.data5, out _))
        {
            distortionLevel = float.Parse(missionEvent.data5);
        }

        AudioSource missionBriefingAudio = null;

        if (audio != "none" & internalAudioFile != "true")
        {
            missionBriefingAudio = AudioFunctions.PlayMissionAudioClip(null, audio, "Voice", new Vector3(0, 0, 0), 0, 1, 500, 1f, 1, distortion, distortionLevel, true);
        }
        else if (audio != "none" & internalAudioFile == "true")
        {
            missionBriefingAudio = AudioFunctions.PlayMissionAudioClip(null, audio, "Voice", new Vector3(0, 0, 0), 0, 1, 500, 1f, 1, distortion, distortionLevel, true);
        }

        MissionBriefingFunctions.ActivateMissionBriefing(message, missionBriefingAudio);
    }

    //This checks whether a mission objective is active or not
    public static bool IfObjectiveIsActive(MissionEvent missionEvent)
    {
        bool objectiveIsActive = false;

        Hud hud = HudFunctions.GetHud();

        if (hud != null)
        {
            if (hud.objectiveLog != null)
            {
                if (hud.objectiveLog.text.Contains(missionEvent.data1))
                {
                    objectiveIsActive = true;
                }
            }
        }

        return objectiveIsActive;
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

                        LargeShip largeShip = ship.GetComponent<LargeShip>();

                        if (largeShip != null)
                        {
                            if (largeShip.waypoint != null)
                            {
                                float tempDistance = Vector3.Distance(largeShip.transform.position, largeShip.waypoint.transform.position);

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

    //This checks whether the requested ship has been disabled or not
    public static bool IfShipHasBeenDisabled(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        bool shipHasBeenDisabled = false;

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
                                if (smallShip.isDisabled == true)
                                {
                                    shipHasBeenDisabled = true;
                                    break;
                                }
                            }
                            else if (largeShip != null)
                            {
                                if (largeShip.isDisabled == true)
                                {
                                    shipHasBeenDisabled = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        return shipHasBeenDisabled;
    }

    //This checks a group of ships to check if one has not been scanned
    public static bool IfShipHasntBeenDisabled(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        bool shipHasntBeenDisabled = false;

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
                                if (smallShip.isDisabled == false)
                                {
                                    shipHasntBeenDisabled = true;
                                    break;
                                }
                            }
                            else if (largeShip != null)
                            {
                                if (largeShip.isDisabled == false)
                                {
                                    shipHasntBeenDisabled = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        return shipHasntBeenDisabled;
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
    public static bool IfShipOfAllegianceIsActive(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        bool shipTypeIsActive = false;

        string mode = missionEvent.data2;

        if (scene != null)
        {
            if (scene.objectPool != null)
            {
                foreach (GameObject ship in scene.objectPool)
                {
                    if (mode == "none" || mode == "smallships" ||  mode == "allships")
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

                    if (mode == "none" || mode == "largeships" || mode == "allships")
                    {
                        LargeShip largeship = ship.GetComponent<LargeShip>();

                        if (largeship != null)
                        {
                            if (ship.activeSelf == true & largeship.allegiance == missionEvent.data1)
                            {
                                shipTypeIsActive = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        return shipTypeIsActive;
    }

    //This checks the ship distance to its waypoint
    public static bool IfShipsSystemsAreLessThan(MissionEvent missionEvent)
    {
        bool islessthanamount = false;

        Scene scene = SceneFunctions.GetScene();

        float systemsLevel = Mathf.Infinity;

        if (missionEvent.data2 != "none")
        {
            systemsLevel = float.Parse(missionEvent.data2);
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
                            if (smallShip.systemsLevel < systemsLevel)
                            {
                                islessthanamount = true;
                                break;
                            }
                        }
                        else if (largeShip != null)
                        {
                            if (largeShip.systemsLevel < systemsLevel)
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

    //This loads the asteroid field
    public static IEnumerator LoadAsteroids(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        float x = missionEvent.x;
        float y = missionEvent.y;
        float z = missionEvent.z;
        Vector3 position = new Vector3(x, y, z);      
        string type = missionEvent.data2;
        
        float number = 1000;

        if (float.TryParse(missionEvent.data1, out _))
        {
            number = float.Parse(missionEvent.data1);
        }

        float width = 30000;

        if (float.TryParse(missionEvent.data3, out _))
        {
            width = float.Parse(missionEvent.data3);
        }

        float height = 1000;

        if (float.TryParse(missionEvent.data4, out _))
        {
            height = float.Parse(missionEvent.data4);
        }

        float length = 30000;

        if (float.TryParse(missionEvent.data5, out _))
        {
            length = float.Parse(missionEvent.data5);
        }

        int seed = 0;

        if (int.TryParse(missionEvent.data6, out _))
        {
            seed = int.Parse(missionEvent.data6);
        }

        Task a = new Task(SceneFunctions.LoadAsteroids(number, type, position, width, height, length, seed));
        while (a.Running == true) { yield return null; }

        yield return null;
    }

    //This loads a planet in the scene
    public static void LoadPlanet(MissionEvent missionEvent)
    {
        float planetRotationX = missionEvent.x;
        float planetRotationY = missionEvent.y;
        float planetRotationZ = missionEvent.z;

        float pivotRotationX = missionEvent.xRotation;
        float pivotRotationY = missionEvent.yRotation;
        float pivotRotationZ = missionEvent.zRotation;

        string planetType = missionEvent.data2;
        string cloudsType = missionEvent.data3;
        string atmosphereType = missionEvent.data4;
        string ringsType = missionEvent.data5;

        float distance = 50;

        if (float.TryParse(missionEvent.data1, out _))
        {
            distance = float.Parse(missionEvent.data1);
        }

        Scene scene = SceneFunctions.GetScene();

        SceneFunctions.GeneratePlanet(planetType, cloudsType, atmosphereType, ringsType, distance, planetRotationX, planetRotationY, planetRotationZ, pivotRotationX, pivotRotationY, pivotRotationZ);
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

        string laserColor = "red";
        if (missionEvent.data7 != "none") { laserColor = missionEvent.data7; }

        SceneFunctions.LoadSingleShip(position, rotation, type, name, allegiance, cargo, exitingHyperspace, isAI, false, laserColor);
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

        string laserColor = "red";
        if (missionEvent.data7 != "none") { laserColor = missionEvent.data7; }

        Scene scene = SceneFunctions.GetScene();

        Vector3 newPosition = new Vector3(0, 0, 0);

        if (scene != null)
        {
            if (scene.mainShip != null)
            {
                newPosition = (angle * scene.mainShip.transform.forward).normalized * distance;
            }
        }

        SceneFunctions.LoadSingleShip(newPosition, rotation, type, name, allegiance, cargo, exitingHyperspace, true, false, laserColor);
    }

    //This loads a single ship on the ground
    public static void LoadSingleShipOnGround(MissionEvent missionEvent)
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

        bool isAI = false;

        if (bool.TryParse(missionEvent.data5, out _))
        {
            isAI = bool.Parse(missionEvent.data5);
        }

        float distanceAboveGround = 0;

        if (float.TryParse(missionEvent.data6, out _))
        {
            distanceAboveGround = float.Parse(missionEvent.data6);
        }

        float positionVariance = 10;

        if (float.TryParse(missionEvent.data7, out _))
        {
            positionVariance = float.Parse(missionEvent.data7);
        }

        bool ifRaycastFailsStillLoad = false;

        if (bool.TryParse(missionEvent.data8, out _))
        {
            ifRaycastFailsStillLoad = bool.Parse(missionEvent.data8);
        }

        string laserColor = "red";
        if (missionEvent.data9 != "none") { laserColor = missionEvent.data9; }

        SceneFunctions.LoadSingleShipOnGround(position, rotation, type, name, allegiance, cargo, isAI, distanceAboveGround, positionVariance, ifRaycastFailsStillLoad, laserColor);
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

        string laserColor = "red";
        if (missionEvent.data15 != "none") { laserColor = missionEvent.data15; }

        Task c = new Task(SceneFunctions.LoadMultipleShips(position, rotation, type, name, allegiance, cargo, number, pattern, width, length, height, shipsPerLine, positionVariance, exitingHyperspace, includePlayer, playerNo, laserColor));
        while (c.Running == true) { yield return null; }
    }

    //This loads multiple ships from another ships hangar
    public static IEnumerator LoadMultipleShipsFromHangar(MissionEvent missionEvent)
    {
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

        string launchShip = "none";
        if (missionEvent.data6 != "none") { launchShip = missionEvent.data6; }

        int hangarNo = 0;

        if (int.TryParse(missionEvent.data7, out _))
        {
            hangarNo = int.Parse(missionEvent.data7);
        }

        float delay = 5;

        if (float.TryParse(missionEvent.data8, out _))
        {
            delay = float.Parse(missionEvent.data8);
        }

        string laserColor = "red";
        if (missionEvent.data9 != "none") { laserColor = missionEvent.data9; }

        Task c = new Task(SceneFunctions.LoadMultipleShipsFromHangar(type, name, allegiance, cargo, number, launchShip, hangarNo, delay, laserColor));
        while (c.Running == true) { yield return null; }
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

        if (bool.TryParse(missionEvent.data12, out _))
        {
            ifRaycastFailsStillLoad = bool.Parse(missionEvent.data12);
        }

        string laserColor = "red";
        if (missionEvent.data13 != "none") { laserColor = missionEvent.data13; }

        Task a = new Task(SceneFunctions.LoadMultipleShipsOnGround(position, rotation, type, name, allegiance, cargo, number, length, width, distanceAboveGround, shipsPerLine, positionVariance, ifRaycastFailsStillLoad, laserColor));
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

    //This turns the control lock on and off
    public static void SetControlLock(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        bool isLocked = false;

        if (bool.TryParse(missionEvent.data2, out _))
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
                            smallShip.controlLock = isLocked;
                        }

                        LargeShip largeShip = ship.GetComponent<LargeShip>();

                        if (largeShip != null)
                        {
                            largeShip.controlLock = isLocked;
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

    //This sets the follow target needed for formation flying
    public static void SetFollowTarget(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();
        float xOffset = 0;
        float yOffset = 0;
        float zOffset = 0;

        if (scene != null)
        {
            if (scene.objectPool != null)
            {
                //This finds the ship 
                foreach (GameObject tempShip in scene.objectPool)
                {
                    if (tempShip.name.Contains(missionEvent.data2))
                    {
                        SmallShip followTarget = tempShip.GetComponent<SmallShip>();

                        if (followTarget != null)
                        {
                            //This finds the target to follow
                            foreach (GameObject tempShip2 in scene.objectPool)
                            {
                                if (tempShip2.name.Contains(missionEvent.data1))
                                {
                                    SmallShip smallShip = tempShip2.GetComponent<SmallShip>();

                                    if (smallShip != null)
                                    {
                                        if (smallShip != followTarget)
                                        {
                                            if (missionEvent.data3 == "arrow")
                                            {
                                                var newPosition = GetNewPosition_Arrow(xOffset, yOffset, zOffset);
                                                xOffset = newPosition.x;
                                                yOffset = newPosition.y;
                                                zOffset = newPosition.z;
                                            }
                                            else if (missionEvent.data3 == "linesingle")
                                            {
                                                var newPosition = GetNewPosition_LineSingle(xOffset, yOffset, zOffset);
                                                xOffset = newPosition.x;
                                                yOffset = newPosition.y;
                                                zOffset = newPosition.z;
                                            }
                                            else if (missionEvent.data3 == "linedual")
                                            {
                                                var newPosition = GetNewPosition_LineSingle(xOffset, yOffset, zOffset);
                                                xOffset = newPosition.x;
                                                yOffset = newPosition.y;
                                                zOffset = newPosition.z;
                                            }
                                            else
                                            {
                                                var newPosition = GetNewPosition_Random(xOffset, yOffset, zOffset);
                                                xOffset = newPosition.x;
                                                yOffset = newPosition.y;
                                                zOffset = newPosition.z;
                                            }

                                            smallShip.followTarget = followTarget;
                                            smallShip.xFormationPos = xOffset;
                                            smallShip.yFormationPos = yOffset;
                                            smallShip.zFormationPos = zOffset;
                                        }
                                    }
                                }
                            }
                        }

                        break;
                    }
                }
            }
        }
    }

    //This returns the next position in the formation based on the received values
    public static (float x, float y, float z) GetNewPosition_Arrow(float x, float y, float z)
    {
        if (x == 0)
        {
            x = 30;
            z = -50;
        }
        else if (x > 0)
        {
            x = -x;
        }
        else
        {
            x = Mathf.Abs(x) + 30;
            z = z - 50;
        }

        return (x, y, z);
    }

    //This returns the next position in the formation based on the received values
    public static (float x, float y, float z) GetNewPosition_LineSingle(float x, float y, float z)
    {
        if (x == 0)
        {
            x = 0;
            z = -50;
        }
        else
        {
            z = z - 50;
        }

        return (x, y, z);
    }

    //This returns the next position in the formation based on the received values
    public static (float x, float y, float z) GetNewPosition_LineDual(float x, float y, float z)
    {
        if (x == 0)
        {
            x = 50;
            z = -50;
        }
        else if (x > 0)
        {
            x = -x;
        }
        else if (x < 0)
        {
            x = Mathf.Abs(x);
            z = z - 50;
        }

        return (x, y, z);
    }

    //This returns the next position in the formation based on the received values
    public static (float x, float y, float z) GetNewPosition_Random(float x, float y, float z)
    {
        if (x == 0)
        {
            x = Random.Range(-200, 200);
            z = Random.Range(-200, 200);
            y = Random.Range(-200, 200);
        }

        return (x, y, z);
    }

    //This sets the coloured aspects of the hud
    public static void SetHudColour(MissionEvent missionEvent)
    {
        string colour = missionEvent.data1;

        HudFunctions.SetHudColour(colour);
    }

    //This sets an objective for the player to complete, removes an objective, or clears all objectives.
    public static void SetObjective(MissionEvent missionEvent)
    {
        MissionManager missionManager = GameObject.FindFirstObjectByType<MissionManager>();

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
            else if (mode == "clearobjective")
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
            }
            else if (mode == "clearallobjectives")
            {
                missionManager.objectiveList = newObjectiveList.ToArray();

                HudFunctions.UpdateObjectives(missionManager.objectiveList);
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
    public static void SetShipToCannotBeDisabled(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        bool cannotBeDisabled = false;

        if (missionEvent.data2 != "none")
        {
            cannotBeDisabled = bool.Parse(missionEvent.data2);
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
                            smallShip.cannotbedisabled = cannotBeDisabled;
                        }

                        LargeShip largeShip = ship.GetComponent<LargeShip>();

                        if (largeShip != null)
                        {
                            largeShip.cannotbedisabled = cannotBeDisabled;
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

    //This changes a ships levels
    public static void SetShipLevels(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        float hullLevel = 0;
        float shieldLevel = 0;
        float systemsLevel = 0;
        float wepLevel = 0;

        bool noChangeHullLevel = false;
        bool noChangeShieldLevel = false;
        bool noChangeSystemsLevel = false;
        bool noChangeWepLevel = false;

        if (float.TryParse(missionEvent.data2, out _))
        {
            hullLevel = float.Parse(missionEvent.data2);

            if (hullLevel > 100)
            {
                hullLevel = 100;
            }
        }
        else
        {
            noChangeHullLevel = true;
        }

        if (float.TryParse(missionEvent.data3, out _))
        {
            shieldLevel = float.Parse(missionEvent.data3);

            if (shieldLevel > 100)
            {
                shieldLevel = 100;
            }
        }
        else
        {
            noChangeShieldLevel = true;
        }

        if (float.TryParse(missionEvent.data4, out _))
        {
            systemsLevel = float.Parse(missionEvent.data4);

            if (systemsLevel > 100)
            {
                systemsLevel = 100;
            }
        }
        else
        {
            noChangeSystemsLevel = false;
        }

        if (float.TryParse(missionEvent.data5, out _))
        {
            wepLevel = float.Parse(missionEvent.data5);

            if (wepLevel > 100)
            {
                wepLevel = 100;
            }
        }
        else
        {
            noChangeWepLevel = false;
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
                            //Change hull level
                            if (noChangeHullLevel == true)
                            {
                                hullLevel = smallShip.hullLevel;
                            }

                            if (hullLevel > smallShip.hullRating)
                            {
                                hullLevel = smallShip.hullRating;
                            }

                            smallShip.hullLevel = hullLevel;

                            //Change shield level
                            if (noChangeShieldLevel == true)
                            {
                                shieldLevel = smallShip.shieldLevel;
                            }

                            if (shieldLevel > smallShip.shieldRating)
                            {
                                shieldLevel = smallShip.shieldRating;
                            }

                            smallShip.shieldLevel = shieldLevel;

                            //Change systems level
                            if (noChangeSystemsLevel == true)
                            {
                                systemsLevel = smallShip.systemsLevel;
                            }

                            if (systemsLevel > smallShip.systemsRating)
                            {
                                systemsLevel = smallShip.systemsRating;
                            }

                            if (systemsLevel <= 0)
                            {
                                systemsLevel = 0;
                                smallShip.isDisabled = true;
                            }

                            if (systemsLevel > 0)
                            {
                                smallShip.isDisabled = false;
                            }

                            smallShip.systemsLevel = systemsLevel;

                            //Change wep level
                            if (noChangeWepLevel == true)
                            {
                                wepLevel = smallShip.wepLevel;
                            }

                            if (wepLevel > smallShip.wepRating)
                            {
                                wepLevel = smallShip.wepRating;
                            }

                            smallShip.wepLevel = wepLevel;
                        }
                    }
                }
            }
        }
    }

    //This allows you to change a ships current stats
    public static void SetShipStats(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        float accelerationRating = 0;
        float speedRating = 0;
        float maneuverabilityRating = 0;
        float hullRating = 0;
        float shieldRating = 0;
        float laserFireRating = 0;
        float laserRating = 0;
        float WEPRating = 0;

        if (float.TryParse(missionEvent.data2, out _))
        {
            accelerationRating = float.Parse(missionEvent.data2);

            if (accelerationRating > 100)
            {
                accelerationRating = 100;
            }
        }

        if (float.TryParse(missionEvent.data3, out _))
        {
            speedRating = float.Parse(missionEvent.data3);

            if (speedRating > 100)
            {
                speedRating = 100;
            }
        }

        if (float.TryParse(missionEvent.data4, out _))
        {
            maneuverabilityRating = float.Parse(missionEvent.data4);

            if (maneuverabilityRating > 100)
            {
                maneuverabilityRating = 100;
            }
        }

        if (float.TryParse(missionEvent.data5, out _))
        {
            hullRating = float.Parse(missionEvent.data5);

            if (hullRating > 100)
            {
                hullRating = 100;
            }
        }

        if (float.TryParse(missionEvent.data6, out _))
        {
            shieldRating = float.Parse(missionEvent.data6);

            if (shieldRating > 100)
            {
                shieldRating = 100;
            }
        }

        if (float.TryParse(missionEvent.data7, out _))
        {
            laserFireRating = float.Parse(missionEvent.data7);

            if (laserFireRating > 100)
            {
                laserFireRating = 100;
            }
        }

        if (float.TryParse(missionEvent.data8, out _))
        {
            laserRating = float.Parse(missionEvent.data8);

            if (laserRating > 100)
            {
                laserRating = 100;
            }
        }

        if (float.TryParse(missionEvent.data9, out _))
        {
            WEPRating = float.Parse(missionEvent.data9);

            if (WEPRating > 100)
            {
                WEPRating = 100;
            }
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
                            smallShip.accelerationRating = accelerationRating;
                            smallShip.speedRating = speedRating;
                            smallShip.maneuverabilityRating = maneuverabilityRating;
                            smallShip.hullRating = hullRating;
                            smallShip.shieldRating = shieldRating;
                            smallShip.laserFireRating = laserFireRating;
                            smallShip.laserRating = laserRating;
                            smallShip.wepRating = WEPRating;
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
                            TargetingFunctions.GetSpecificTarget_SmallShipPlayer(smallShip, missionEvent.data2);
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
                            TargetingFunctions.GetClosestEnemy_SmallShipPlayer(smallShip, true);
                        }
                    }
                }
            }
        }
    }

    //This sets the skybox
    public static void SetSkyBox(MissionEvent missionEvent)
    {
        string skybox = missionEvent.data1;
        bool stars = true;

        if (bool.TryParse(missionEvent.data2, out _))
        {
            stars = bool.Parse(missionEvent.data2);
        }

        SceneFunctions.SetSkybox(skybox, stars);
    }

    //This sets the number and type of torpedoes
    public static void SetTorpedoes(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        string torpedoType = missionEvent.data2;
        float torpedoNo = 0;
        bool noChangeToTorpedoNo = false;

        if (float.TryParse(missionEvent.data3, out _))
        {
            torpedoNo = float.Parse(missionEvent.data3);
        }
        else
        {
            noChangeToTorpedoNo = true;
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
                            smallShip.torpedoType = torpedoType;

                            if (noChangeToTorpedoNo == true)
                            {
                                torpedoNo = smallShip.torpedoNumber;
                            }

                            smallShip.torpedoNumber = torpedoNo;
                        }
                    }
                }
            }
        }
    }

    //This moves a ships waypoint to the designated position
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

                        LargeShip largeShip = ship.GetComponent<LargeShip>();

                        if (largeShip != null)
                        {
                            if (largeShip.waypoint != null)
                            {
                                float x = missionEvent.x;
                                float y = missionEvent.y;
                                float z = missionEvent.z;
                                Vector3 waypoint = scene.transform.position + new Vector3(x, y, z);

                                largeShip.waypoint.transform.position = waypoint;
                            }
                        }
                    }
                }
            }
        }
    }

    //This moves a ships waypoint to the position of the designated ship
    public static void SetWaypointToShip(MissionEvent missionEvent)
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
                                foreach (GameObject ship2 in scene.objectPool)
                                {
                                    if (ship2.name.Contains(missionEvent.data2))
                                    {
                                        smallShip.waypoint.transform.position = ship2.transform.position;
                                    }
                                }
                            }
                        }

                        LargeShip largeShip = ship.GetComponent<LargeShip>();

                        if (largeShip != null)
                        {
                            if (largeShip.waypoint != null)
                            {
                                foreach (GameObject ship2 in scene.objectPool)
                                {
                                    if (ship2.name.Contains(missionEvent.data2))
                                    {
                                        largeShip.waypoint.transform.position = ship2.transform.position;
                                    }
                                }
                            }
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

                        missionManager.missionTasks.Add(a);
                        missionManager.missionTasks.Add(b);

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

        MissionFunctions.ExitAndUnload();
    }

    //This activates the exit menu
    public static void ActivateExitMenu(MissionManager missionManager)
    {
        Keyboard keyboard = Keyboard.current;
        Gamepad gamepad = Gamepad.current;
        bool allowDisplay = true;

        if (missionManager.missionBriefing == null)
        {
            missionManager.missionBriefing = GameObject.FindObjectOfType<MissionBriefing>();
        }

        if (missionManager.missionBriefing != null)
        {
            if(missionManager.missionBriefing.gameObject.activeSelf == true)
            {
                allowDisplay = false;
            }
        }

        if (allowDisplay == true)
        {
            if (keyboard != null)
            {
                if (keyboard.escapeKey.isPressed == true & missionManager.pressedTime < Time.time)
                {
                    ExitMenuFunctions.DisplayExitMenu(true);
                    missionManager.pressedTime = Time.time + 0.5f;
                }
            }

            if (gamepad != null)
            {
                if (gamepad.startButton.isPressed == true & missionManager.pressedTime < Time.time)
                {
                    ExitMenuFunctions.DisplayExitMenu(true);
                    missionManager.pressedTime = Time.time + 0.5f;
                }
            }
        }
    }

    //This unloads the game and exits back to the main screen
    public static void ExitAndUnload()
    {
        HudFunctions.UnloadHud();
        MusicFunctions.UnloadMusicManager();
        AudioFunctions.UnloadAudioManager();
        Task a = new Task(SceneFunctions.UnloadScene());

        ExitMenu exitMenu = GameObject.FindObjectOfType<ExitMenu>();
        GameObject loadingScreen = GameObject.Find("LoadingScreen");
        GameObject missionBriefing = GameObject.Find("MissionBriefing");

        if (exitMenu != null) { GameObject.Destroy(exitMenu.gameObject); }
        if (loadingScreen != null) { GameObject.Destroy(loadingScreen); }
        if (missionBriefing != null) { GameObject.Destroy(missionBriefing); }

        MainMenu mainMenu = GameObject.FindObjectOfType<MainMenu>(true);

        if (mainMenu != null)
        {
            if (mainMenu.menu != null)
            {
                mainMenu.menu.SetActive(true);
                mainMenu.missionRunning = false;
                CanvasGroup canvasGroup = mainMenu.menu.GetComponent<CanvasGroup>();
                Task b = new Task(MainMenuFunctions.FadeInCanvas(canvasGroup, 0.5f));
                MainMenuFunctions.ActivateStartGameMenu();
            }
        }

        MissionManager missionManager = GameObject.FindObjectOfType<MissionManager>();

        //This stops any active event series coroutines so they don't continue running when a new mission is loaded
        foreach (Task eventSeries in missionManager.missionTasks)
        {
            if (eventSeries != null)
            {
                eventSeries.Stop();
            }
        }

        //This resets the starfield to ensure that it's not stretched
        SceneFunctions.ResetStarfield();
        SceneFunctions.ResetCameras();
        SceneFunctions.ResetHyperSpaceTunnel();

        //This destroys the mission manager
        if (missionManager != null) { GameObject.Destroy(missionManager.gameObject); }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    #endregion

    #region mission utils

    //This returns the mission manager when requested
    public static MissionManager GetMissionManager()
    {
        MissionManager missionManager = GameObject.FindObjectOfType<MissionManager>();

        return missionManager;
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
