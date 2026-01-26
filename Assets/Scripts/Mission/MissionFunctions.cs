using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public static class MissionFunctions
{
    #region event system

    //This executes the different mission events
    public static IEnumerator RunMission(string missionName, string missionAddress, bool addressIsExternal = false)
    {
        //This marks the start time of the script
        float startTime = Time.unscaledTime;

        //This creastes the scene
        Scene scene = SceneFunctions.CreateScene();

        SceneFunctions.LoadScenePrefabs();

        //This creates the mission manager
        GameObject missionManagerGO = new GameObject();
        missionManagerGO.name = "MissionManager";
        MissionManager missionManager = missionManagerGO.AddComponent<MissionManager>();

        missionManager.missionAddress = missionAddress;

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
        missionManager.scene = SceneFunctions.GetScene();

        //This pauses the game to prevent action starting before everything is loaded 
        Time.timeScale = 0;

        //This displays the loading screen
        DisplayLoadingScreen(missionName, true);

        //This tells the player that the mission is being loaded
        MainMenuFunctions.AddLogToLoadingScreen("Start loading " + missionName + ".", startTime);

        //This loads the base game scene
        LoadScene(missionName, missionAddress, addressIsExternal, startTime);

        while (missionManager.audioLoaded == false)
        {
            yield return null;
        }

        //This sets the skybox to the default space and view distance
        SceneFunctions.SetSkybox("space_black01", true);

        //This sets the fog and distance color to their default settings
        SceneFunctions.SetFogDistanceAndColor(30000, 40000, "#000000");

        //This sets the scene lighting to default
        SceneFunctions.SetLighting("#E2EAF4", false, 1, 1, 0, 0, 0, 60, 0, 0);

        //This sets the scene to it's default size
        SceneFunctions.SetSceneRadius(15000);

        //This finds and sets the first location
        MissionEvent firstLocationNode = FindFirstLocationNode(mission, startTime);

        //This finds all the location you can jump to in the mission
        FindAllLocations(mission);

        //This sets the current location
        missionManager.scene.currentLocation = firstLocationNode.conditionLocation;

        //This runs all the preload events like loading the planet and asteroids and objects already in scene
        Task a = new Task(FindAndRunPreLoadEvents(mission, firstLocationNode.conditionLocation, startTime, true));
        while (a.Running == true) { yield return null; }

        //This activates the OG Camera
        OGCameraFunctions.ActivateOGCamera();

        //This tells the player to get ready, starts the game, locks the cursor and gets rid of the loading screen
        MainMenuFunctions.AddLogToLoadingScreen(missionName + " loaded.", startTime);

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

        missionManager.missionTasks = new List<Task>();

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
    public static MissionEvent FindFirstLocationNode(Mission mission, float startTime)
    {
        MissionEvent missionEvent = null;

        foreach (MissionEvent tempMissionEvent in mission.missionEventData)
        {
            if (tempMissionEvent.eventType == "createlocation")
            {
                if (tempMissionEvent.data1 == "true")
                {
                    missionEvent = tempMissionEvent;
                    MainMenuFunctions.AddLogToLoadingScreen("Starting location node found", startTime);
                    break;
                }
            }
        }

        if (missionEvent == null)
        {
            MainMenuFunctions.AddLogToLoadingScreen("No starting location node found searching for another location node.", startTime);

            foreach (MissionEvent tempMissionEvent in mission.missionEventData)
            {
                if (tempMissionEvent.eventType == "createlocation")
                {
                    missionEvent = tempMissionEvent;
                    MainMenuFunctions.AddLogToLoadingScreen("Secondary location node found.", startTime);
                    break;
                }
            }
        }

        if (missionEvent == null)
        {
            MainMenuFunctions.AddLogToLoadingScreen("No starting location found aborting load", startTime);
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
    public static IEnumerator FindAndRunPreLoadEvents(Mission mission, string location, float startTime, bool firstRun = false)
    {
        //This preloads all scene and the terrain first
        foreach (MissionEvent missionEvent in mission.missionEventData)
        {
            if (missionEvent.eventType == "preload_loadplanet" & missionEvent.conditionLocation == location)
            {
                MainMenuFunctions.AddLogToLoadingScreen("Loading planet", startTime);
                LoadPlanet(missionEvent);
                MainMenuFunctions.AddLogToLoadingScreen("Planet loaded", startTime);
            }
            else if (missionEvent.eventType == "preload_sethudcolour" & missionEvent.conditionLocation == location)
            {
                SetHudColour(missionEvent);
                MainMenuFunctions.AddLogToLoadingScreen("Hud Colour Set", startTime);
            }
            else if (missionEvent.eventType == "preload_setsceneradius" & missionEvent.conditionLocation == location)
            {
                SetSceneRadius(missionEvent);
                MainMenuFunctions.AddLogToLoadingScreen("Scene radius set", startTime);
            }
            else if (missionEvent.eventType == "preload_setskybox" & missionEvent.conditionLocation == location)
            {
                SetSkyBox(missionEvent);
                MainMenuFunctions.AddLogToLoadingScreen("Skybox set", startTime);
            }
            else if (missionEvent.eventType == "preload_setlighting" & missionEvent.conditionLocation == location)
            {
                SetLighting(missionEvent);
                MainMenuFunctions.AddLogToLoadingScreen("Lighting set", startTime);
            }
            else if (missionEvent.eventType == "preload_setfogdistanceandcolor" & missionEvent.conditionLocation == location)
            {
                SetFogDistanceAndColor(missionEvent);
                MainMenuFunctions.AddLogToLoadingScreen("Fog settings set", startTime);
            }

        }

        //Then this preloads all the none-ship objects in the scene
        foreach (MissionEvent missionEvent in mission.missionEventData)
        {
            if (missionEvent.eventType == "preload_loadasteroids" & missionEvent.conditionLocation == location)
            {
                Task a = new Task(LoadAsteroids(missionEvent));
                while (a.Running == true) { yield return null; }
                MainMenuFunctions.AddLogToLoadingScreen("Asteroids loaded", startTime);
            }
        }

        //Then this preloads all the ships in the scene
        foreach (MissionEvent missionEvent in mission.missionEventData)
        {
            if (missionEvent.eventType == "preload_loadsingleship" & missionEvent.conditionLocation == location)
            {
                //This extra check is run to prevent the game loading the player ship twice
                if (firstRun == false & missionEvent.data8 == "false" || firstRun == false & missionEvent.data8 == "none" || firstRun == true)
                {
                    LoadSingleShip(missionEvent);
                    MainMenuFunctions.AddLogToLoadingScreen("Single ship loaded", startTime);
                }
            }
            else if (missionEvent.eventType == "preload_loadmultipleships" & missionEvent.conditionLocation == location)
            {
                Task a = new Task(LoadMultipleShips(missionEvent));
                while (a.Running == true) { yield return null; }
                MainMenuFunctions.AddLogToLoadingScreen("Multiple shis loaded", startTime);
            }
            else if (missionEvent.eventType == "preload_loadsingleshipaswreck" & missionEvent.conditionLocation == location)
            {
                //This extra check is run to prevent the game loading the player ship twice
                if (firstRun == false & missionEvent.data8 == "false" || firstRun == false & missionEvent.data8 == "none" || firstRun == true)
                {
                    LoadSingleShipAsWreck(missionEvent);
                    MainMenuFunctions.AddLogToLoadingScreen("Single ship loaded as wreck", startTime);
                }
            }
        }
    }

    //This runs a series of events
    public static IEnumerator RunEventSeries(Mission mission, int eventSeries)
    {
        MissionManager missionManager = GameObject.FindFirstObjectByType<MissionManager>();

        while (missionManager.running == true & missionManager.eventNo[eventSeries] != 11111)
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

            //This makes sure that the mission events aren't run when the mission manager has been deleted
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
        MissionManager missionManager = GameObject.FindFirstObjectByType<MissionManager>();

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
        else if (missionEvent.eventType == "activaterapidfire")
        {
            ActivateRapidFire(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "activatesystemstargeting")
        {
            ActivateSystemsTargeting(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "activatewaypointmarker")
        {
            ActivateWaypointMarker(missionEvent);
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
            Task a = new Task(ChangeLocationHyperspace(missionEvent));
            missionManager.missionTasks.Add(a);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "changelocationfade")
        {
            Task a = new Task(ChangeLocationFade(missionEvent));
            missionManager.missionTasks.Add(a);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "exitmission")
        {
            ExitMission();
        }
        else if (missionEvent.eventType == "exitanddisplaynextmissionmenu")
        {
            ExitAndDisplayNextMissionMenu(missionEvent);
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
        else if (missionEvent.eventType == "displaymessageimmediate")
        {
            DisplayMessageImmediate(missionEvent);
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
        else if (missionEvent.eventType == "ifkeyboardisactive")
        {
            bool ifKeyboardIsActive = IfKeyboardIsActive(missionEvent);

            if (ifKeyboardIsActive == true)
            {
                FindNextEvent(missionEvent.nextEvent1, eventSeries);
            }
            else
            {
                FindNextEvent(missionEvent.nextEvent2, eventSeries);
            }
        }
        else if (missionEvent.eventType == "ifnumberofshipsislessthan")
        {
            bool ifNumberOfShipIsLessThan = IfNumberOfShipsIsLessThan(missionEvent);

            if (ifNumberOfShipIsLessThan == true)
            {
                FindNextEvent(missionEvent.nextEvent1, eventSeries);
            }
            else
            {
                FindNextEvent(missionEvent.nextEvent2, eventSeries);
            }
        }
        else if (missionEvent.eventType == "ifnumberofshipsofallegianceislessthan")
        {
            bool ifNumberOfShipIsLessThan = IfNumberOfShipsOfAllegianceIsLessThan(missionEvent);

            if (ifNumberOfShipIsLessThan == true)
            {
                FindNextEvent(missionEvent.nextEvent1, eventSeries);
            }
            else
            {
                FindNextEvent(missionEvent.nextEvent2, eventSeries);
            }
        }
        else if (missionEvent.eventType == "ifnumberofshipswithnameislessthan")
        {
            bool ifNumberOfShipIsLessThan = IfNumberOfShipsWithNameIsLessThan(missionEvent);

            if (ifNumberOfShipIsLessThan == true)
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
        else if (missionEvent.eventType == "ifshipislessthandistancetopointinspace")
        {
            bool isLessThanDistance = IfShipIsLessThanDistanceToPointInSpace(missionEvent);

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
        else if (missionEvent.eventType == "ifshipsshieldsarelessthan")
        {
            bool ifshipshieldislessthan = IfShipsShieldsAreLessThan(missionEvent);

            if (ifshipshieldislessthan == true)
            {
                FindNextEvent(missionEvent.nextEvent1, eventSeries);
            }
            else
            {
                FindNextEvent(missionEvent.nextEvent2, eventSeries);
            }
        }
        else if (missionEvent.eventType == "ifsystemisactive")
        {
            bool isActive = IfSystemIsActive(missionEvent);

            if (isActive == true)
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
        else if (missionEvent.eventType == "loadsingleshipfromhangar")
        {
            LoadSingleShipFromHangar(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "loadsingleshipatdistanceandanglefromplayer")
        {
            LoadSingleShipAtDistanceAndAngleFromPlayer(missionEvent);
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
        else if (missionEvent.eventType == "pausesequence")
        {
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "playmusictrack")
        {
            PlayMusicTrack(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "playvideo")
        {
            PlayVideo(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
        else if (missionEvent.eventType == "setcamera")
        {
            SetCamera(missionEvent);
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
        else if (missionEvent.eventType == "sethudmode")
        {
            SetHudMode(missionEvent);
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
        else if (missionEvent.eventType == "setweaponselectiononplayership")
        {
            SetWeaponSelectionOnPlayerShip(missionEvent);
            FindNextEvent(missionEvent.nextEvent1, eventSeries);
        }
    }

    //This creates the scene 
    public static void LoadScene(string missionName, string missionAddress, bool addressIsExternal, float startTime)
    {
        Scene scene = SceneFunctions.GetScene();

        //This creates the hud
        HudFunctions.CreateHud();
        MainMenuFunctions.AddLogToLoadingScreen("Hud created.", startTime);

        //This creates the scene and gets the cameras
        MainMenuFunctions.AddLogToLoadingScreen("Scene created.", startTime);

        //THis loads the audio and music manager
        AudioFunctions.CreateAudioManager(missionAddress + missionName + "_audio/", addressIsExternal);
        OGVideoPlayerFunctions.CreateOGVideoPlayer();
        OGVideoPlayerFunctions.LoadVideos(missionAddress + missionName + "_video/", addressIsExternal);
        MainMenuFunctions.AddLogToLoadingScreen("Audio Manager created", startTime);
        MusicFunctions.CreateMusicManager();
        MainMenuFunctions.AddLogToLoadingScreen("Music Manager created", startTime);
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
            messages[8] = "When you push all your energy to engines your WEP system increases to full strength.";
            messages[9] = "Remember you can link your torpedoes and lasers for greater impact.";
            int randomMessageNo = Random.Range(0, 9);
            MainMenuFunctions.DisplayLoadingScreen(true, missionName, messages[randomMessageNo]);
        }
        else
        {
            MainMenuFunctions.DisplayLoadingScreen(false);
        }
    }

    //This plays audio in a queue which prevents audio from overlapping
    public static IEnumerator PlayMessageFromQueue(MissionManager missionManager)
    {
        string message = missionManager.messageStringQueue.Dequeue();
        string audio = missionManager.messageAudioQueue.Dequeue();
        bool distortion = missionManager.messageDistortionQueue.Dequeue();
        float distortionLevel = missionManager.messageDistortionLevelQueue.Dequeue();
        string internalAudioFile = missionManager.messageInternalQueue.Dequeue();

        AudioSource messageAudioSource = null;

        if (message != "none")
        {
            HudFunctions.AddToShipLog(message);
        }

        if (audio != "none" & internalAudioFile != "true")
        {
            messageAudioSource = AudioFunctions.PlayMissionAudioClip(null, audio, "Voice", new Vector3(0, 0, 0), 0, 1, 500, 1f, 1, distortion, distortionLevel, true);
        }
        else if (audio != "none" & internalAudioFile == "true")
        {
            messageAudioSource = AudioFunctions.PlayMissionAudioClip(null, audio, "Voice", new Vector3(0, 0, 0), 0, 1, 500, 1f, 1, distortion, distortionLevel, false);
        }

        if (messageAudioSource != null)
        {
            while (messageAudioSource.isPlaying == true)
            {
                yield return null;

                if (messageAudioSource == null)
                {
                    break;
                }
            }
        }

        missionManager.messagePlaying = false;
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
                foreach (GameObject ship in scene.objectPool.ToList())
                {
                    if (ship != null)
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
    }

    //This causes the designated ships to jump to hyperspace
    public static void ActivateHyperspace(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        if (scene != null)
        {
            if (scene.objectPool != null)
            {
                foreach (GameObject ship in scene.objectPool.ToList())
                {
                    if (ship != null)
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
    }

    //This allows rapid fire on the target ship
    public static void ActivateRapidFire(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        string shipName = missionEvent.data1;

        bool hasRadidFire = false;

        if (bool.TryParse(missionEvent.data2, out _))
        {
            hasRadidFire = bool.Parse(missionEvent.data2);
        }

        if (scene != null)
        {
            if (scene.objectPool != null)
            {
                foreach (GameObject ship in scene.objectPool.ToList())
                {
                    if (ship != null)
                    {
                        if (ship.name.Contains(shipName))
                        {
                            SmallShip smallShip = ship.GetComponent<SmallShip>();

                            if (smallShip != null)
                            {
                                smallShip.hasRapidFire = hasRadidFire;
                            }
                        }
                    }
                }
            }
        }
    }

    //This activates systems targetting on the selected ship
    public static void ActivateSystemsTargeting(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        string shipName = missionEvent.data1;
        string system = missionEvent.data3;

        bool activate = false;

        if (bool.TryParse(missionEvent.data2, out _))
        {
            activate = bool.Parse(missionEvent.data2);
        }

        if (scene != null)
        {
            if (scene.objectPool != null)
            {
                foreach (GameObject ship in scene.objectPool.ToList())
                {
                    if (ship != null)
                    {
                        if (ship.name.Contains(shipName))
                        {
                            ShipSystemFunctions.ActivateSystemTransforms(ship, activate, system);
                        }
                    }
                }
            }
        }
    }

    //This activates the players onscreen waypoint marker
    public static void ActivateWaypointMarker(MissionEvent missionEvent)
    {
        Hud hud = HudFunctions.GetHud();

        bool waypointIsActive = false;

        if (bool.TryParse(missionEvent.data1, out _))
        {
            waypointIsActive = bool.Parse(missionEvent.data1);
        }

        string waypointTitleString = missionEvent.data2;

        if (hud != null)
        {
            hud.waypointIsActive = waypointIsActive;

            if (waypointIsActive == true)
            {
                hud.waypointTitleString = waypointTitleString;
            }
            else
            {
                hud.waypointTitleString = "";
            }

            if (hud.smallShip != null)
            {
                AudioFunctions.PlayAudioClip(hud.smallShip.audioManager, "beep01_toggle", "Cockpit", hud.smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);
            }

            HudFunctions.AddToShipLog("Waypoint activated " + waypointTitleString);
        }
    }

    //This adds an ai tag to a largeship
    public static void AddAITagToLargeShip(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        string speedControlTag = missionEvent.data2;
        string flightPatternsTag = missionEvent.data3;
        string largeTurretControlTag = missionEvent.data4;
        string smallTurretControlTag = missionEvent.data5;
        string largeTurretAccuracyControlTag = missionEvent.data6;
        string smallTurretAccuracyControlTag = missionEvent.data7;

        if (scene != null)
        {
            if (scene.objectPool != null)
            {
                foreach (GameObject ship in scene.objectPool.ToList())
                {
                    if (ship != null)
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

                                if (flightPatternsTag != "none" & flightPatternsTag != "nochange")
                                {
                                    LargeShipAIFunctions.AddTag(largeShip, flightPatternsTag);
                                }

                                if (largeTurretControlTag != "none" & largeTurretControlTag != "nochange")
                                {
                                    LargeShipAIFunctions.AddTag(largeShip, largeTurretControlTag);
                                }

                                if (smallTurretControlTag != "none" & smallTurretControlTag != "nochange")
                                {
                                    LargeShipAIFunctions.AddTag(largeShip, smallTurretControlTag);
                                }

                                if (largeTurretAccuracyControlTag != "none" & largeTurretAccuracyControlTag != "nochange")
                                {
                                    LargeShipAIFunctions.AddTag(largeShip, largeTurretAccuracyControlTag);
                                }

                                if (smallTurretAccuracyControlTag != "none" & smallTurretAccuracyControlTag != "nochange")
                                {
                                    LargeShipAIFunctions.AddTag(largeShip, smallTurretAccuracyControlTag);
                                }
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
        string collisionControlTag = missionEvent.data8;

        if (scene != null)
        {
            if (scene.objectPool != null)
            {
                foreach (GameObject ship in scene.objectPool.ToList())
                {
                    if (ship != null)
                    {
                        if (ship.name.Contains(missionEvent.data1))
                        {
                            SmallShip smallShip = ship.GetComponent<SmallShip>();

                            if (smallShip != null)
                            {
                                if (smallShip.isAI == true)
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

                                    if (collisionControlTag != "none" & collisionControlTag != "nochange")
                                    {
                                        SmallShipAIFunctions.AddTag(smallShip, collisionControlTag);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    //This unloads the current location and loads a new one from the avaiblible locations while simulating a hyperspace jump
    public static IEnumerator ChangeLocationHyperspace(MissionEvent missionEvent)
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
        OGCamera ogCamera = OGCameraFunctions.GetOGCamera();
        SmallShip smallShip = scene.mainShip.GetComponent<SmallShip>();
        MissionManager missionManager = GetMissionManager();
        Mission mission = JsonUtility.FromJson<Mission>(missionManager.missionData);
        GameObject starfield = SceneFunctions.GetStarfield();
        Hud hud = HudFunctions.GetHud();

        missionManager.pauseEventSeries = true;

        bool controlLock = smallShip.controlLock; //This preserves the current lock position

        smallShip.focusCamera = false; //This prevents the ship jumping with the camera in the focus position
        smallShip.controlLock = true; //This locks the player ship controls so the ship remains correctly orientated to the hyperspace effect
        smallShip.invincible = true; //This sets the ship to invincible so that any objects the ship may hit while the scene changes doesn't destroy it

        //This fades out the hud
        
        if (hud.mode == "hud")
        {
            Task fadeOut = new Task(HudFunctions.FadeOutHud(1));
        }

        SmallShipFunctions.CloseWings(smallShip);

        yield return new WaitForSecondsRealtime(3.4f);

        //This plays the hyperspace entry sound
        AudioFunctions.PlayAudioClip(smallShip.audioManager, "hyperspace01_entry", "Cockpit", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);

        smallShip.inHyperspace = true;

        yield return new WaitForSecondsRealtime(2); //This gives the audio clip time to play

        //This marks the jump time
        float time = Time.unscaledTime;

        //This clears the current location
        SceneFunctions.ClearLocation();

        //Task a = new Task(SceneFunctions.PlanetFlyOut());
        //while (a.Running == true) { yield return null; }

        //This makes the stars stretch out
        ogCamera.planetCamera.GetComponent<Camera>().enabled = false;

        Time.timeScale = 0;

        //This makes the stars stretch out
        Task b = new Task(SceneFunctions.StretchStarfield());
        while(b.Running == true) { yield return null; }

        //This activates the hyperspace tunnel
        if (scene.hyperspaceTunnel == null)
        {
            scene.hyperspaceTunnel = GameObject.Instantiate(scene.hyperspaceTunnelPrefab);
        }

        if (scene.hyperspaceTunnel != null)
        {
            scene.hyperspaceTunnel.transform.SetParent(smallShip.transform);
            scene.hyperspaceTunnel.transform.localPosition = new Vector3(0, 0, 0);
            scene.hyperspaceTunnel.transform.localRotation = Quaternion.identity;
            scene.hyperspaceTunnel.SetActive(true);
            
            if (smallShip.keyboardAndMouse == false)
            {
                Task a = new Task(SmallShipFunctions.ShakeControllerForSetTime(0.15f, 0.9f, 0.9f));
            }
            
            SceneFunctions.SetStarfieldToInvisible(true);
        }

        //This sets the rotation of the ship in the new location designated in the node
        smallShip.transform.rotation = Quaternion.Euler(xRotation, yRotation, zRotation);

        //This sets the position of the ship in the new location designated in the node
        smallShip.transform.localPosition = new Vector3(x, y, z);

        //This resets the skybox to black
        SceneFunctions.SetSkybox("space_black", true);

        //This resets the lighting to default
        SceneFunctions.SetLighting("#E2EAF4", false, 1, 1, 0, 0, 0, 0, 0, 0);

        //This resets the fog distanc and colour
        SceneFunctions.SetFogDistanceAndColor(30000, 40000, "#000000");

        //This sets the scene to it's default size
        SceneFunctions.SetSceneRadius(15000);

        //This sets the scene location
        scene.currentLocation = jumpLocation;

        //This finds and loads all 'preload' nodes for the new location
        Task c = new Task(MissionFunctions.FindAndRunPreLoadEvents(mission, jumpLocation, time, false));
        while(c.Running == true) { yield return null; }

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
            scene.hyperspaceTunnel.transform.SetParent(null);

            if (smallShip.keyboardAndMouse == false)
            {
                Task a = new Task(SmallShipFunctions.ShakeControllerForSetTime(0.15f, 0.9f, 0.9f));
            }
        }

        //This plays the hyperspace exit
        AudioFunctions.PlayAudioClip(smallShip.audioManager, "hyperspace03_exit", "Cockpit", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);

        ogCamera.starfieldCamera.GetComponent<Camera>().enabled = true;

        //This shrinks the starfield
        Task d = new Task(SceneFunctions.ShrinkStarfield());
        while (d.Running == true) { yield return null; }

        ogCamera.planetCamera.GetComponent<Camera>().enabled = true;

        //This fades the hud back in
        if (hud.mode == "hud")
        {
            Task fadeIN = new Task(HudFunctions.FadeInHud(1));
        }

        //This unlocks the player controls and turns off invincibility on the player ship
        smallShip.controlLock = controlLock; //This restores the set lock position
        smallShip.invincible = false;

        yield return new WaitForSecondsRealtime(1);

        SmallShipFunctions.OpenWings(smallShip);

        //This unpauses the event series
        missionManager.pauseEventSeries = false;
    }

    //This unloads the current location and loads a new one from the avaiblible locations while simulating a hyperspace jump
    public static IEnumerator ChangeLocationFade(MissionEvent missionEvent)
    {
        //This gets all the required data from the event node
        float x = missionEvent.x;
        float y = missionEvent.y;
        float z = missionEvent.z;

        float xRotation = missionEvent.xRotation;
        float yRotation = missionEvent.yRotation;
        float zRotation = missionEvent.zRotation;

        string location = missionEvent.data1;
        string colour = missionEvent.data2;

        //This gets several important references
        Scene scene = SceneFunctions.GetScene();
        OGCamera ogCamera = OGCameraFunctions.GetOGCamera();
        SmallShip smallShip = scene.mainShip.GetComponent<SmallShip>();
        MissionManager missionManager = GetMissionManager();
        Mission mission = JsonUtility.FromJson<Mission>(missionManager.missionData);
        GameObject starfield = SceneFunctions.GetStarfield();
        Hud hud = HudFunctions.GetHud();

        missionManager.pauseEventSeries = true;

        bool controlLock = smallShip.controlLock; //This preserves the current lock position

        smallShip.controlLock = true; //This locks the player ship controls so the ship remains correctly orientated to the hyperspace effect
        smallShip.invincible = true; //This sets the ship to invincible so that any objects the ship may hit while the scene changes doesn't destroy it

        //This fades out the hud
        if (hud.mode == "hud")
        {
            Task a = new Task(HudFunctions.FadeOutHud(1));

            while (a.Running == true)
            {
                yield return null;
            }
        }

        //This fades the scene out
        HudFunctions.FadeInBackground(1, colour);

        yield return new WaitForSeconds(1.5f);

        //This marks the jump time
        float time = Time.unscaledTime;

        //This clears the current location
        SceneFunctions.ClearLocation();

        Time.timeScale = 0;

        //This sets the rotation of the ship in the new location designated in the node
        smallShip.transform.rotation = Quaternion.Euler(xRotation, yRotation, zRotation);

        //This sets the position of the ship in the new location designated in the node
        smallShip.transform.localPosition = new Vector3(x, y, z);

        //This resets the skybox to black
        SceneFunctions.SetSkybox("space_black", true);

        //This resets the lighting to default
        SceneFunctions.SetLighting("#E2EAF4", false, 1, 1, 0, 0, 0, 0, 0, 0);

        //This resets the fog distanc and colour
        SceneFunctions.SetFogDistanceAndColor(30000, 40000, "#000000");

        //This sets the scene to it's default size
        SceneFunctions.SetSceneRadius(15000);

        //This sets the scene location
        scene.currentLocation = location;

        //This finds and loads all 'preload' nodes for the new location
        Task b = new Task(MissionFunctions.FindAndRunPreLoadEvents(mission, location, time, false));
        while (b.Running == true) { yield return null; }

        yield return new WaitForSecondsRealtime(1);

        //This time scale needs to be turned on before shrinking the starfield to ensure that the function can get the correct velocity angle from the ship
        Time.timeScale = 1;

        yield return new WaitForSecondsRealtime(1); //This gives the rigidbody time to calculate the new velocity

        ogCamera.planetCamera.GetComponent<Camera>().enabled = true;
        ogCamera.mainCamera.GetComponent<Camera>().enabled = true;

        //This fades the hud back in
        if (hud.mode == "hud")
        {
            Task fadeIN = new Task(HudFunctions.FadeInHud(1));

            yield return new WaitForSeconds(1.5f);

        }

        //This fades the scene back ing
        HudFunctions.FadeOutBackground(1, colour);

        //This unlocks the player controls and turns off invincibility on the player ship
        smallShip.controlLock = controlLock; //This restores the set lock position
        smallShip.invincible = false;
   
        //This unpauses the event series
        missionManager.pauseEventSeries = false;
    }

    //This exits the mission back to the main menu
    public static void ExitMission()
    {
        MissionManager missionManager = GameObject.FindFirstObjectByType<MissionManager>();
        
        if (missionManager != null)
        {
            missionManager.running = false;
        }

        UnloadMission();
    }
    
    //This exist the mission to the Nexy Mission menu
    public static void ExitAndDisplayNextMissionMenu(MissionEvent missionEvent)
    {
        string nextMissionName = missionEvent.data1;
        string model = missionEvent.data2;
        string debriefing = missionEvent.data3;
        string audioFile = missionEvent.data4;
        string internalFile = missionEvent.data5;

        bool distortion = false;

        if (bool.TryParse(missionEvent.data6, out _))
        {
            distortion = bool.Parse(missionEvent.data6);
        }

        float distortionLevel = 0.5f;

        if (float.TryParse(missionEvent.data7, out _))
        {
            distortionLevel = float.Parse(missionEvent.data7);
        }

        bool nextMissionButton = false;

        if (bool.TryParse(missionEvent.data8, out _))
        {
            nextMissionButton = bool.Parse(missionEvent.data8);
        }

        //Load next mission screen
        Task a = new Task(NextMissionFunctions.ActivateNextMissionMenu(nextMissionName, model, nextMissionButton, debriefing, audioFile, internalFile, distortionLevel, distortion));
    }

    //This deactivates a ship so that it is no longer part of the scene
    public static void DeactivateShip(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        if (scene != null)
        {
            if (scene.objectPool != null)
            {
                foreach (GameObject ship in scene.objectPool.ToList())
                {
                    if (ship != null)
                    {
                        if (ship.name.Contains(missionEvent.data1))
                        {
                            SmallShip smallShip = ship.GetComponent<SmallShip>();

                            if (smallShip != null)
                            {
                                DamageFunctions.DeactivateShip_SmallShip(smallShip);
                            }

                            LargeShip largeShip = ship.GetComponent<LargeShip>();

                            if (largeShip != null)
                            {
                                DamageFunctions.DeactivateShip_LargeShip(largeShip);
                            }
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

        HudFunctions.DisplayHint(hint);
    }

    //This temporary displays a large message in the center of the screen
    public static void DisplayTitle(MissionEvent missionEvent)
    {
        string title = missionEvent.data1;
        HudFunctions.DisplayTitle(title);
    }

    //This adds a message to the log and plays an audio file, it will wait for other messages to play before playing
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

        MissionManager missionManager = MissionFunctions.GetMissionManager();

        if (missionManager != null)
        {
            missionManager.messageStringQueue.Enqueue(message);
            missionManager.messageAudioQueue.Enqueue(audio);
            missionManager.messageInternalQueue.Enqueue(internalAudioFile);
            missionManager.messageDistortionQueue.Enqueue(distortion);
            missionManager.messageDistortionLevelQueue.Enqueue(distortionLevel);
        }
    }

    //This adds a message to the log and can also play an audio file, it will not wait for other messages to finish playing
    public static void DisplayMessageImmediate(MissionEvent missionEvent)
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

        string model = missionEvent.data6;

        Task a = new Task(MissionBriefingFunctions.ActivateMissionBriefing(message, audio, internalAudioFile, distortion, distortionLevel, model));
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

    //This checks whether the player is playing with the keyboard and mouse or not
    public static bool IfKeyboardIsActive(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        bool keyboardIsActive = false;

        if (scene != null)
        {
            if(scene.mainShip != null)
            {
                SmallShip smallShip = scene.mainShip.GetComponent<SmallShip>();

                if (smallShip != null)
                {
                    keyboardIsActive = smallShip.keyboardAndMouse; 
                }
            }
        }

        return keyboardIsActive;
    }

    //This compares the number of ships in the scene is less than a certain number
    public static bool IfNumberOfShipsIsLessThan(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        bool shipNumberIsLessThan = false;
        int number = 0;
        int shipCount = 0;

        if (int.TryParse(missionEvent.data1, out _))
        {
            number = int.Parse(missionEvent.data1);
        }

        string mode = missionEvent.data2;

        if (scene != null)
        {
            if (scene.objectPool != null)
            {
                foreach (GameObject ship in scene.objectPool.ToList())
                {
                    if (ship != null)
                    {
                        if (mode == "none" || mode == "smallships" || mode == "allships")
                        {
                            SmallShip smallShip = ship.GetComponent<SmallShip>();

                            if (smallShip != null)
                            {
                                if (ship.activeSelf == true)
                                {
                                    shipCount++;
                                }
                            }
                        }

                        if (mode == "none" || mode == "largeships" || mode == "allships")
                        {
                            LargeShip largeship = ship.GetComponent<LargeShip>();

                            if (largeship != null)
                            {
                                if (ship.activeSelf == true)
                                {
                                    shipCount++;
                                }
                            }
                        }
                    }
                }
            }
        }

        if (shipCount < number)
        {
            shipNumberIsLessThan = true;   
        }

        return shipNumberIsLessThan;
    }

    //This compares the number of ships in the scene of a certain allegiance is less than a certain number
    public static bool IfNumberOfShipsOfAllegianceIsLessThan(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        bool shipNumberIsLessThan = false;
        int number = 0;
        int shipCount = 0;
        string allegiance = missionEvent.data1;

        if (int.TryParse(missionEvent.data2, out _))
        {
            number = int.Parse(missionEvent.data2);
        }

        string mode = missionEvent.data3;

        if (scene != null)
        {
            if (scene.objectPool != null)
            {
                foreach (GameObject ship in scene.objectPool.ToList())
                {
                    if (ship != null)
                    {
                        if (mode == "none" || mode == "smallships" || mode == "allships")
                        {
                            SmallShip smallShip = ship.GetComponent<SmallShip>();

                            if (smallShip != null)
                            {
                                if (ship.activeSelf == true & smallShip.allegiance == allegiance)
                                {
                                    shipCount++;
                                }
                            }
                        }

                        if (mode == "none" || mode == "largeships" || mode == "allships")
                        {
                            LargeShip largeship = ship.GetComponent<LargeShip>();

                            if (largeship != null)
                            {
                                if (ship.activeSelf == true & largeship.allegiance == allegiance)
                                {
                                    shipCount++;
                                }
                            }
                        }
                    }
                }
            }
        }

        if (shipCount < number)
        {
            shipNumberIsLessThan = true;
        }

        return shipNumberIsLessThan;
    }

    //This compares the number of ships of a certain name in the scene is less than a certain number
    public static bool IfNumberOfShipsWithNameIsLessThan(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        bool shipNumberIsLessThan = false;
        int number = 0;
        int shipCount = 0;
        string name = missionEvent.data1;

        if (int.TryParse(missionEvent.data1, out _))
        {
            number = int.Parse(missionEvent.data2);
        }

        string mode = missionEvent.data3;

        if (scene != null)
        {
            if (scene.objectPool != null)
            {
                foreach (GameObject ship in scene.objectPool.ToList())
                {
                    if (ship != null)
                    {
                        if (mode == "none" || mode == "smallships" || mode == "allships")
                        {
                            SmallShip smallShip = ship.GetComponent<SmallShip>();

                            if (smallShip != null)
                            {
                                if (ship.activeSelf == true & smallShip.name.Contains(name))
                                {
                                    shipCount++;
                                }
                            }
                        }

                        if (mode == "none" || mode == "largeships" || mode == "allships")
                        {
                            LargeShip largeship = ship.GetComponent<LargeShip>();

                            if (largeship != null)
                            {
                                if (ship.activeSelf == true & largeship.name.Contains(name))
                                {
                                    shipCount++;
                                }
                            }
                        }
                    }
                }
            }
        }

        if (shipCount < number)
        {
            shipNumberIsLessThan = true;
        }

        return shipNumberIsLessThan;
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
                foreach (GameObject ship in scene.objectPool.ToList())
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
                foreach (GameObject ship in scene.objectPool.ToList())
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
                foreach (GameObject ship in scene.objectPool.ToList())
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
                foreach (GameObject ship in scene.objectPool.ToList())
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
                foreach (GameObject ship in scene.objectPool.ToList())
                {
                    if (ship != null)
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
                foreach (GameObject tempShipA in scene.objectPool.ToList())
                {
                    if (tempShipA != null)
                    {
                        if (tempShipA.name.Contains(shipA))
                        {
                            foreach (GameObject tempShipB in scene.objectPool.ToList())
                            {
                                if (tempShipB != null)
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
                foreach (GameObject ship in scene.objectPool.ToList())
                {
                    if (ship != null)
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
        }

        return isLessThanDistance;
    }

    //This checks the ship distance to its waypoint
    public static bool IfShipIsLessThanDistanceToPointInSpace(MissionEvent missionEvent)
    {
        bool isLessThanDistance = false;

        Scene scene = SceneFunctions.GetScene();

        float distance = Mathf.Infinity;
        
        float x = missionEvent.x;
        float y = missionEvent.y;
        float z = missionEvent.z;

        Vector3 locationInSpace = scene.transform.TransformPoint(new Vector3(x, y, z));

        if (missionEvent.data2 != "none")
        {
            distance = float.Parse(missionEvent.data2);
        }

        if (scene != null)
        {
            if (scene.objectPool != null)
            {
                foreach (GameObject ship in scene.objectPool.ToList())
                {
                    if (ship != null)
                    {
                        if (ship.name.Contains(missionEvent.data1))
                        {
                            SmallShip smallShip = ship.GetComponent<SmallShip>();

                            if (smallShip != null)
                            {
                                if (smallShip.waypoint != null)
                                {
                                    float tempDistance = Vector3.Distance(smallShip.transform.position, locationInSpace);

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
                                    float tempDistance = Vector3.Distance(largeShip.transform.position, locationInSpace);

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
                foreach (GameObject ship in scene.objectPool.ToList())
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
                foreach (GameObject ship in scene.objectPool.ToList())
                {
                    if (ship != null)
                    {
                        if (mode == "none" || mode == "smallships" || mode == "allships")
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
        }

        return shipTypeIsActive;
    }

    //This checks whether a ship's shields are less than the given amount
    public static bool IfShipsShieldsAreLessThan(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        bool isLessThan = false;

        string shipName = missionEvent.data1;
        float shieldLevel = 100;

        if (float.TryParse(missionEvent.data2, out _))
        {
            shieldLevel = float.Parse(missionEvent.data2);
        }

        if (scene != null)
        {
            if (scene.objectPool != null)
            {
                foreach (GameObject ship in scene.objectPool.ToList())
                {
                    if (ship != null)
                    {
                        if (ship.name.Contains(shipName))
                        {
                            SmallShip smallShip = ship.GetComponent<SmallShip>();
                            LargeShip largeShip = ship.GetComponent<LargeShip>();

                            if (smallShip != null)
                            {
                                if (smallShip.shieldLevel < shieldLevel)
                                {
                                    isLessThan = true;
                                    break;
                                }
                            }
                            else if (largeShip != null)
                            {
                                if (largeShip.shieldLevel < shieldLevel)
                                {
                                    isLessThan = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        return isLessThan;
    }

    //This checks if the systems health is less tahn
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
                foreach (GameObject ship in scene.objectPool.ToList())
                {
                    if (ship != null)
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
        }

        return islessthanamount;
    }

    //This checks whether a particular system on the ship is still active or not i.e. shield, engine, radar, dovin basal
    public static bool IfSystemIsActive(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        string shipName = missionEvent.data1;
        string system = missionEvent.data2;

        bool isActive = false;

        if (scene != null)
        {
            if (scene.objectPool != null)
            {
                foreach (GameObject ship in scene.objectPool.ToList())
                {
                    if (ship != null)
                    {
                        if (ship.name.Contains(shipName))
                        {
                            isActive = ShipSystemFunctions.SystemIsActive(ship, system);
                            break;
                        }
                    }
                }
            }
        }

        return isActive;
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

        bool loadLarge = true;

        if (bool.TryParse(missionEvent.data7, out _))
        {
            loadLarge = bool.Parse(missionEvent.data7);
        }

        Task a = new Task(SceneFunctions.LoadAsteroids(number, type, position, width, height, length, seed, loadLarge));
        while (a.Running == true) { yield return null; }

        yield return null;
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

        SceneFunctions.GeneratePlanet(planetType, ringsType, distance, planetRotationX, planetRotationY, planetRotationZ, pivotRotationX, pivotRotationY, pivotRotationZ);
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

        SceneFunctions.LoadSingleShip(position, rotation, type, name, allegiance, cargo, exitingHyperspace, isAI, false, laserColor, true);
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

    //This loads multiple ships from another ships hangar
    public static void LoadSingleShipFromHangar(MissionEvent missionEvent)
    {
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

        string launchShip = "none";
        if (missionEvent.data4 != "none") { type = missionEvent.data4; }

        string cargo = "no cargo";
        if (missionEvent.data5 != "none") { cargo = missionEvent.data5; }

        bool isAI = false;

        if (bool.TryParse(missionEvent.data6, out _))
        {
            isAI = bool.Parse(missionEvent.data6);
        }

        string laserColor = "red";
        if (missionEvent.data7 != "none") { laserColor = missionEvent.data7; }

        SceneFunctions.LoadSingleShipFromHangar(type, name, allegiance, cargo, launchShip, 0, laserColor);
    }

    //This loads a single ship as a wreck
    public static void LoadSingleShipAsWreck(MissionEvent missionEvent)
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

        int fireNumber = 0;

        if (int.TryParse(missionEvent.data3, out _))
        {
            fireNumber = int.Parse(missionEvent.data3);
        }

        float fireScaleMin = 1;

        if (float.TryParse(missionEvent.data4, out _))
        {
            fireScaleMin = float.Parse(missionEvent.data4);
        }

        float fireScaleMax = 1;

        if (float.TryParse(missionEvent.data5, out _))
        {
            fireScaleMax = float.Parse(missionEvent.data5);
        }

        int seed = 0;

        if (int.TryParse(missionEvent.data6, out _))
        {
            seed = int.Parse(missionEvent.data6);
        }

        SceneFunctions.LoadSingleShipAsWreck(position, rotation, type, name, fireNumber, fireScaleMin, fireScaleMax, seed);
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

    //This plays a video
    public static void PlayVideo(MissionEvent missionEvent)
    {
        string video = missionEvent.data1;

        Task a = new Task(OGVideoPlayerFunctions.RunVideo(video));
    }

    //This manually sets the camera mode
    public static void SetCamera(MissionEvent missionEvent)
    {
        float x = missionEvent.x;
        float y = missionEvent.y;
        float z = missionEvent.z;

        Vector3 position = new Vector3(x, y, z);

        float xRotation = missionEvent.xRotation;
        float yRotation = missionEvent.yRotation;
        float zRotation = missionEvent.zRotation;

        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, zRotation);

        string mode = missionEvent.data1;

        string shotType = missionEvent.data2;

        string targetName = missionEvent.data3;

        bool move = false;

        if (bool.TryParse(missionEvent.data4, out _))
        {
            move = bool.Parse(missionEvent.data4);
        }

        string moveAxis = missionEvent.data5;

        float moveSpeed = 50f;

        if (float.TryParse(missionEvent.data6, out _))
        {
            moveSpeed = float.Parse(missionEvent.data6);
        }

        bool rotate = false;

        if (bool.TryParse(missionEvent.data7, out _))
        {
            rotate = bool.Parse(missionEvent.data7);
        }

        string rotateAxis = missionEvent.data8;

        float rotateSpeed = 50f;

        if (float.TryParse(missionEvent.data9, out _))
        {
            rotateSpeed = float.Parse(missionEvent.data9);
        }

        bool shakecamera = false;

        if (bool.TryParse(missionEvent.data10, out _))
        {
            shakecamera = bool.Parse(missionEvent.data10);
        }

        float shakerate = 1;

        if (float.TryParse(missionEvent.data11, out _))
        {
            shakerate = float.Parse(missionEvent.data11);
        }

        float shakestrength = 0.7f;

        if (float.TryParse(missionEvent.data12, out _))
        {
            shakestrength = float.Parse(missionEvent.data12);
        }

        OGCameraFunctions.SetOGCameraValues(mode, shotType, position, rotation, targetName, shakecamera, shakerate, shakestrength, move, moveAxis, moveSpeed, rotate, rotateAxis, rotateSpeed);
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
                foreach (GameObject ship in scene.objectPool.ToList())
                {
                    if (ship != null)
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
                foreach (GameObject ship in scene.objectPool.ToList())
                {
                    if (ship != null)
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
                    foreach (GameObject ship in scene.objectPool.ToList())
                    {
                        if (ship != null)
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
                foreach (GameObject tempShip in scene.objectPool.ToList())
                {
                    if (tempShip != null)
                    {
                        if (tempShip.name.Contains(missionEvent.data2))
                        {
                            SmallShip followTarget = tempShip.GetComponent<SmallShip>();

                            if (followTarget != null)
                            {
                                //This finds the target to follow
                                foreach (GameObject tempShip2 in scene.objectPool.ToList())
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
                                                else if (missionEvent.data3 == "arrowinverted")
                                                {
                                                    var newPosition = GetNewPosition_ArrowInverted(xOffset, yOffset, zOffset);
                                                    xOffset = newPosition.x;
                                                    yOffset = newPosition.y;
                                                    zOffset = newPosition.z;
                                                }
                                                else if (missionEvent.data3 == "random")
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
    }

    //This selects the hud type to display
    public static void SetHudMode(MissionEvent missionEvent)
    {
        string mode = missionEvent.data1;

        HudFunctions.SetHudMode(mode);
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
    public static (float x, float y, float z) GetNewPosition_ArrowInverted(float x, float y, float z)
    {
        if (x == 0)
        {
            x = 30;
            z = +50;
        }
        else if (x > 0)
        {
            x = -x;
        }
        else
        {
            x = Mathf.Abs(x) + 30;
            z = z + 50;
        }

        return (x, y, z);
    }

    //This returns the next position in the formation based on the received values
    public static (float x, float y, float z) GetNewPosition_Random(float x, float y, float z)
    {
        x = Random.Range(-50, 50);
        z = Random.Range(-50, 50);
        y = Random.Range(-50, 50);
        
        return (x, y, z);
    }
    
    //This sets the colour and distance of the fog for the scene
    public static void SetFogDistanceAndColor(MissionEvent missionEvent)
    {
        float fogStart = 30000;
        float fogEnd = 40000;
        string fogColor = missionEvent.data3;

        if (float.TryParse(missionEvent.data1, out _))
        {
            fogStart = float.Parse(missionEvent.data1);
        }

        if (float.TryParse(missionEvent.data2, out _))
        {
            fogEnd = float.Parse(missionEvent.data2);
        }

        SceneFunctions.SetFogDistanceAndColor(fogStart, fogEnd, fogColor);
    }

    //This sets the coloured aspects of the hud
    public static void SetHudColour(MissionEvent missionEvent)
    {
        //string colour = missionEvent.data1;

        //HudFunctions.SetHudColour(colour);
    }

    //This changes the lighting in the scene
    public static void SetLighting(MissionEvent missionEvent)
    {
        float x = missionEvent.x;
        float y = missionEvent.y;
        float z = missionEvent.z;
        float rotX = missionEvent.xRotation;
        float rotY = missionEvent.yRotation;
        float rotZ = missionEvent.zRotation;
        string colour = missionEvent.data1;
        bool sunIsEnabled = false;
        float sunIntensity = 1;
        float sunScale = 1;

        if (bool.TryParse(missionEvent.data2, out _))
        {
            sunIsEnabled = bool.Parse(missionEvent.data2);
        }

        if (float.TryParse(missionEvent.data3, out _))
        {
            sunIntensity = float.Parse(missionEvent.data3);
        }

        if (float.TryParse(missionEvent.data4, out _))
        {
            sunScale = float.Parse(missionEvent.data4);
        }

        SceneFunctions.SetLighting(colour, sunIsEnabled, sunIntensity, sunScale, x, y, z, rotX, rotY, rotZ);
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
        float sceneRadius = 15000;

        if (float.TryParse(missionEvent.data1, out _))
        {
            sceneRadius = float.Parse(missionEvent.data1);
        }

        SceneFunctions.SetSceneRadius(sceneRadius);

    }

    //This sets the designated ships target to the closest enemy, provided both the ship can be found
    public static void SetShipAllegiance(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        if (scene != null)
        {
            if (scene.objectPool != null)
            {
                foreach (GameObject ship in scene.objectPool.ToList())
                {
                    if (ship != null)
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
                foreach (GameObject ship in scene.objectPool.ToList())
                {
                    if (ship != null)
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
                foreach (GameObject ship in scene.objectPool.ToList())
                {
                    if (ship != null)
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
        }
        else
        {
            noChangeHullLevel = true;
        }

        if (float.TryParse(missionEvent.data3, out _))
        {
            shieldLevel = float.Parse(missionEvent.data3);
        }
        else
        {
            noChangeShieldLevel = true;
        }

        if (float.TryParse(missionEvent.data4, out _))
        {
            systemsLevel = float.Parse(missionEvent.data4);
        }
        else
        {
            noChangeSystemsLevel = true;
        }

        if (float.TryParse(missionEvent.data5, out _))
        {
            wepLevel = float.Parse(missionEvent.data5);
        }
        else
        {
            noChangeWepLevel = false;
        }

        if (scene != null)
        {
            if (scene.objectPool != null)
            {
                foreach (GameObject ship in scene.objectPool.ToList())
                {
                    if (ship != null)
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

                                smallShip.hullLevel = hullLevel;

                                //Change shield level
                                if (noChangeShieldLevel == true)
                                {
                                    shieldLevel = smallShip.shieldLevel;
                                }

                                smallShip.shieldLevel = shieldLevel;
                                smallShip.frontShieldLevel = shieldLevel / 2f;
                                smallShip.rearShieldLevel = shieldLevel / 2f;

                                //Change systems level
                                if (noChangeSystemsLevel == true)
                                {
                                    systemsLevel = smallShip.systemsLevel;
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

                                smallShip.wepLevel = wepLevel;
                            }

                            LargeShip largeShip = ship.GetComponent<LargeShip>();

                            if (largeShip != null)
                            {
                                //Change hull level
                                if (noChangeHullLevel == true)
                                {
                                    hullLevel = largeShip.hullLevel;
                                }

                                largeShip.hullLevel = hullLevel;

                                //Change shield level
                                if (noChangeShieldLevel == true)
                                {
                                    shieldLevel = largeShip.shieldLevel;
                                }

                                largeShip.shieldLevel = shieldLevel;
                                largeShip.frontShieldLevel = shieldLevel / 2f;
                                largeShip.rearShieldLevel = shieldLevel / 2f;

                                //Change systems level
                                if (noChangeSystemsLevel == true)
                                {
                                    systemsLevel = largeShip.systemsLevel;
                                }

                                if (systemsLevel <= 0)
                                {
                                    systemsLevel = 0;
                                    largeShip.isDisabled = true;
                                }

                                if (systemsLevel > 0)
                                {
                                    largeShip.isDisabled = false;
                                }

                                largeShip.systemsLevel = systemsLevel;
                            }
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
                foreach (GameObject ship in scene.objectPool.ToList())
                {
                    if (ship != null)
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
    }

    //This sets the designated ships target, provided both the ship and its target can be found
    public static void SetShipTarget(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        if (scene != null)
        {
            if (scene.objectPool != null)
            {
                foreach (GameObject ship in scene.objectPool.ToList())
                {
                    if (ship != null)
                    {
                        if (ship.name.Contains(missionEvent.data1))
                        {
                            SmallShip smallShip = ship.GetComponent<SmallShip>();

                            if (smallShip != null)
                            {
                                TargetingFunctions.GetSpecificTarget_SmallShip(smallShip, missionEvent.data2);
                            }

                            LargeShip largeShip = ship.GetComponent<LargeShip>();

                            if (largeShip != null)
                            {
                                TargetingFunctions.GetSpecificTarget_LargeShipAI(largeShip, missionEvent.data2);
                            }
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
                foreach (GameObject ship in scene.objectPool.ToList())
                {
                    if (ship != null)
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
                foreach (GameObject ship in scene.objectPool.ToList())
                {
                    if (ship != null)
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
    }

    //This moves a ships waypoint to the designated position
    public static void SetWaypoint(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        if (scene != null)
        {
            if (scene.objectPool != null)
            {
                foreach (GameObject ship in scene.objectPool.ToList())
                {
                    if (ship != null)
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
    }

    //This moves a ships waypoint to the position of the designated ship
    public static void SetWaypointToShip(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        if (scene != null)
        {
            if (scene.objectPool != null)
            {
                foreach (GameObject ship in scene.objectPool.ToList())
                {
                    if (ship != null)
                    {
                        if (ship.name.Contains(missionEvent.data1))
                        {
                            SmallShip smallShip = ship.GetComponent<SmallShip>();

                            if (smallShip != null)
                            {
                                if (smallShip.waypoint != null)
                                {
                                    foreach (GameObject ship2 in scene.objectPool.ToList())
                                    {
                                        if (ship2 != null)
                                        {
                                            if (ship2.name.Contains(missionEvent.data2))
                                            {
                                                smallShip.waypoint.transform.position = ship2.transform.position;
                                            }
                                        }
                                    }
                                }
                            }

                            LargeShip largeShip = ship.GetComponent<LargeShip>();

                            if (largeShip != null)
                            {
                                if (largeShip.waypoint != null)
                                {
                                    foreach (GameObject ship2 in scene.objectPool.ToList())
                                    {
                                        if (ship2 != null)
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
        }
    }

    //This manually sets the weapon selection of a ship
    public static void SetWeaponSelectionOnPlayerShip(MissionEvent missionEvent)
    {
        Scene scene = SceneFunctions.GetScene();

        string weapon = missionEvent.data1;
        string mode = missionEvent.data2;
        bool preventWeaponChange = false;

        if (bool.TryParse(missionEvent.data3, out _))
        {
            preventWeaponChange = bool.Parse(missionEvent.data3);
        }

        if (scene != null)
        {
            if (scene.objectPool != null)
            {
                foreach (GameObject ship in scene.objectPool.ToList())
                {    
                    if (ship != null)
                    {
                        SmallShip smallShip = ship.GetComponent<SmallShip>();

                        if (smallShip != null)
                        {
                            if (smallShip.isAI == false)
                            {
                                SmallShipFunctions.SetWeapons(smallShip, weapon, mode);
                                smallShip.preventWeaponChange = preventWeaponChange;
                                break;
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
    public static void ExitOnPlayerDestroy()
    {
        MissionManager missionManager = GetMissionManager();

        if (missionManager.unloading == false)
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

    //This returns to the main menu after a set period of time
    public static IEnumerator UnloadAfter(float time)
    {
        yield return new WaitForSeconds(time);

        UnloadMission();
    }

    //This activates the exit menu
    public static void ActivateExitMenu(MissionManager missionManager)
    {
        Keyboard keyboard = Keyboard.current;
        Gamepad gamepad = Gamepad.current;
        bool allowDisplay = true;

        if (missionManager.missionBriefing == null)
        {
            missionManager.missionBriefing = GameObject.FindFirstObjectByType<MissionBriefing>();
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

    //This unloads the mission and exits back to the main screen
    public static void ReturnToMainMenu()
    {
        MainMenu mainMenu = GameObject.FindObjectOfType<MainMenu>(true);

        if (mainMenu != null)
        {
            if (mainMenu.mainMenu != null)
            {
                mainMenu.mainMenu.SetActive(true);
                CanvasGroup canvasGroup = mainMenu.mainMenu.GetComponent<CanvasGroup>();
                Task b = new Task(MainMenuFunctions.FadeInCanvas(canvasGroup, 0.5f));
                MainMenuFunctions.ActivateStartGameMenu();
            }
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    //This unloads the mission
    public static void UnloadMission(bool loadFollowingMision = false, string missionName = "none")
    {
        Scene scene = SceneFunctions.GetScene();

        ExitMenu exitMenu = GameObject.FindFirstObjectByType<ExitMenu>();
        GameObject fade = scene.fade;
        GameObject missionBriefing = scene.missionBriefing;
        GameObject nextMissionScreen = GameObject.Find("NextMission");

        if (exitMenu != null) { GameObject.Destroy(exitMenu.gameObject); }
        if (fade != null) { GameObject.Destroy(fade); }
        if (missionBriefing != null) { GameObject.Destroy(missionBriefing); }
        if (nextMissionScreen != null) { GameObject.Destroy(nextMissionScreen); }

        Task a = new Task(SceneFunctions.UnloadScene());

        MissionManager missionManager = GameObject.FindFirstObjectByType<MissionManager>(FindObjectsInactive.Include);
        GameObject missionManagerGO = missionManager.gameObject;

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
        SceneFunctions.ResetHyperSpaceTunnel();

        //This resets the skybox to black
        SceneFunctions.SetSkybox("space_black01", true);

        //This destroys the mission manager
        if (missionManager != null) { GameObject.Destroy(missionManager); }
        if (missionManagerGO != null) { GameObject.Destroy(missionManagerGO); }
        
        //This unloads the other systems
        AudioFunctions.UnloadAudioManager();
        HudFunctions.UnloadHud();
        MusicFunctions.UnloadMusicManager();
        SmallShipFunctions.StopShakeController();
        OGCameraFunctions.UnloadOGCamera();
        OGVideoPlayerFunctions.UnloadOGVideoPlayer();

        //This loads the next mission if requested
        if (loadFollowingMision == true)
        {
            var missionCheck =  NextMissionFunctions.CheckIfMissionExists(missionName);

            if (missionCheck.externalMision == true)
            {
                MissionDataFunctions.LoadExternalMission(missionName);
            }
            else if (missionCheck.internalMission == true)
            {
                MissionDataFunctions.LoadInternalMission(missionName);
            }
            else
            {
                //This tells the main menu that the mission is no longer running
                MainMenuFunctions.RunMainMenuWithoutIntroduction();
                MainMenuFunctions.OutputMenuMessage("The requested mission was not found");
            }
        }
        else
        {
            //This tells the main menu that the mission is no longer running
            MainMenuFunctions.RunMainMenuWithoutIntroduction();
        }
    }

    #endregion

    #region mission utils

    //This returns the mission manager when requested
    public static MissionManager GetMissionManager()
    {
        MissionManager missionManager = GameObject.FindFirstObjectByType<MissionManager>();

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
