using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class ExploreFunctions
{

    #region run explore mode

    //This executes the different mission events
    public static IEnumerator RunExplore()
    {
        //This looks for the mission manager and if it doesn't find one creates one
        ExploreManager exploreManager = GetExploreManager();

        //This marks the start time of the script
        float startTime = Time.time;

        //This pauses the game to prevent action starting before everything is loaded 
        Time.timeScale = 0;

        //This marks the time using unscaled time
        float time = Time.unscaledTime;

        //This displays the loading screen
        DisplayLoadingScreen(true);

        //This tells the player that the mission is being loaded
        LoadScreenFunctions.AddLogToLoadingScreen("Start loading Explore", Time.unscaledTime - time);

        //This gets the reference to the scene
        exploreManager.scene = SceneFunctions.GetScene();

        //This loads the base game scene
        LoadScene();

        //This gets the reference to the hud
        exploreManager.hud = HudFunctions.GetHud();

        //This sets the skybox to the default space
        SceneFunctions.SetSkybox("space");

        exploreManager.currentLocation = "Tatooine";

        SetGalaxyLocation("Tatooine");

        //This runs all the preload events like loading the planet and asteroids and objects already in scene
        Task a = new Task(LoadScenery(exploreManager.currentLocation));
        while (a.Running == true) { yield return null; }

        Task b = new Task(LoadShips("Tatooine"));

        //This loads the player in the scene
        LoadPlayerShip();

        //This grabs all jump locations within range
        exploreManager.availibleLocations = GetLocations("Tatooine", 1000);

        //This tells the player to get ready, starts the game, locks the cursor and gets rid of the loading screen
        LoadScreenFunctions.AddLogToLoadingScreen("Explore mode loaded.", Time.unscaledTime - time);

        //This unpause the game 
        Time.timeScale = 1;

        //This locks and hides the cursor during gameplay
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        //This removes the loading screen 
        DisplayLoadingScreen(false);

        HudFunctions.DisplayLargeMessage("TATOOINE");

        //This selects the next avaible destination
        SelectNextJumpLocation(exploreManager);

        //This sets the mission manager to running
        exploreManager.running = true;
    }

    #endregion

    #region load functions

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

    //This loads the planet and asteroids in the scene according to location data
    public static IEnumerator LoadScenery(string location)
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

        Task b = new Task(SceneFunctions.GeneratePlanet(type, seed));
        while (b.Running == true) { yield return null; }
        SceneFunctions.SetPlanetDistance(seed);

        LoadScreenFunctions.AddLogToLoadingScreen("Planet loaded", Time.unscaledTime - time);
    }

    //This loads ships in the scene according to the location data
    public static IEnumerator LoadShips(string location)
    {
        var locationData = SceneFunctions.FindLocation(location);

        string type = locationData.type;
        int seed = locationData.seed;
        string planetType = locationData.planet;
        string allegiance = locationData.allegiance;
        string region = locationData.region;

        float time = Time.unscaledTime;

        //This reads the location profile data
        TextAsset locationProfilesDataFile = new TextAsset();

        locationProfilesDataFile = Resources.Load("Data/Files/LocationProfiles") as TextAsset;

        LocationProfiles locationProfiles = JsonUtility.FromJson<LocationProfiles>(locationProfilesDataFile.text);

        LocationProfile selectedLocationProfile = null;

        //This checks for location specific data first
        foreach (LocationProfile locationProfile in locationProfiles.locationProfileData)
        {
            if (locationProfile.location == location)
            {
                selectedLocationProfile = locationProfile;
            }
        }

        //Then is there is no specific location data looks for the region data
        if (selectedLocationProfile == null)
        {
            foreach (LocationProfile locationProfile in locationProfiles.locationProfileData)
            {
                if (locationProfile.region == region)
                {
                    selectedLocationProfile = locationProfile;
                }
            }
        }

        //The game then uses the region and planet data to generate a unique set of ships
        if (selectedLocationProfile != null)
        {
            Task h = new Task(LoadShipGroup("station", allegiance, selectedLocationProfile.stations, 800, seed));
            while (h.Running) { yield return null; }
            seed++;

            Task a = new Task(LoadShipGroup("large", allegiance, selectedLocationProfile.largeCapitalShips, 1600, seed));
            while (a.Running) { yield return null; }
            seed++;

            Task b = new Task(LoadShipGroup("medium", allegiance, selectedLocationProfile.mediumCapitalShips, 400, seed));
            while (b.Running) { yield return null; }
            seed++;

            Task c = new Task(LoadShipGroup("small", allegiance, selectedLocationProfile.smallCapitalShips, 200, seed));
            while (c.Running) { yield return null; }
            seed++;

            Task d = new Task(LoadShipGroup("freighter", allegiance, selectedLocationProfile.freighters, 150, seed));
            while (d.Running) { yield return null; }
            seed++;

            Task i = new Task(LoadShipGroup("cargo", allegiance, selectedLocationProfile.cargofields, 150, seed));
            while (i.Running) { yield return null; }
            seed++;

            Task j = new Task(LoadShipGroup("navbuoy", allegiance, selectedLocationProfile.navbuoys, 100, seed));
            while (j.Running) { yield return null; }
            seed++;

            Task e = new Task(LoadShipGroup("lightfreighter", allegiance, selectedLocationProfile.lightfreighters, 40, seed));
            while (e.Running) { yield return null; }
            seed++;

            Task f = new Task(LoadShipGroup("shuttle", allegiance, selectedLocationProfile.shuttles, 40,  seed));
            while (f.Running) { yield return null; }
            seed++;

            Task g = new Task(LoadShipGroup("fighter", allegiance, selectedLocationProfile.fighters, 15, seed));
            while (g.Running) { yield return null; }
            seed++;
        }

        yield return null;
    }

    public static IEnumerator LoadShipGroup(string shipClass, string allegiance, int number, float clearance, int seed)
    {
        ExploreManager exploreManager = GetExploreManager();

        Random.InitState(seed);

        int randomCapitalShipNumber = Random.Range(0, number + 1);

        if (randomCapitalShipNumber > 0)
        {
            for (int i = 0; i < number; i++)
            {
                var positionData = GetNewPosition(clearance);

                if (positionData.cleared == false)
                {
                    Debug.Log("instance of " + shipClass + " was not cleared");
                }

                if (positionData.cleared == true)
                {

                    if (exploreManager.shipPositions != null)
                    {
                        exploreManager.shipPositions.Add(positionData.position);
                        exploreManager.shipClearance.Add(clearance);
                    }

                    Vector3 position = positionData.position;
                    Quaternion rotation = new Quaternion();
                    string name = "none";
                    string cargo = "none";
                    string pattern = "arrowhorizontal";
                    float width = 1000;
                    float height = 1000;
                    float length = 1000;
                    int shipNumber = 1;
                    int shipsPerLine = 4;
                    float positionVariance = 10;
                    bool exitingHyperspace = false;
                    bool includePlayer = false;
                    int playerNo = 0;

                    //This procedurally selects a name for the ship
                    string[] names = new string[] { "alpha", "beta", "gamma", "delta", "epislon", "zeta", "eta", "theta", "iota", "kappa", "lambda", "mu", "nu", "xi", "omicron" };
                    int nameNo = names.Length;
                    int selectName = Random.Range(0, names.Length - 1);
                    name = names[selectName];

                    Task c = new Task(SceneFunctions.LoadMultipleShipByClassAndAllegiance(position, rotation, shipClass, name, allegiance, cargo, shipNumber, pattern, width, length, height, shipsPerLine, positionVariance, exitingHyperspace, includePlayer, playerNo));


                }
            }
        }

        yield return null;
    }

    //This loads the player ship according to the details in the explore manager
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

    //This looks for a new positions that doesn't conflict with any other position
    public static (Vector3 position, bool cleared) GetNewPosition(float clearance)
    {
        Vector3 position = new Vector3();
        bool cleared = true;
        ExploreManager exploreManager = GetExploreManager();

        for (int i = 0; i < 50; i++)
        {
            cleared = true;

            float xPos = Random.Range(-10000, 10000);
            float yPos = Random.Range(-10000, 10000);
            float zPos = Random.Range(-10000, 10000);

            position = new Vector3(xPos, yPos, zPos);

            if (exploreManager.shipPositions == null)
            {
                exploreManager.shipPositions = new List<Vector3>();
                exploreManager.shipClearance = new List<float>();
            }

            if (exploreManager.shipPositions != null)
            {
                int i2 = 0;

                foreach (Vector3 shipPosition in exploreManager.shipPositions)
                {
                    if (shipPosition != null)
                    {
                        float distance = Vector3.Distance(shipPosition, position);

                        float tempClearance = clearance;
                        float objectClearance = exploreManager.shipClearance[i2];

                        if (clearance < objectClearance)
                        {
                            tempClearance = objectClearance;
                        }

                        if (distance < tempClearance)
                        {
                            cleared = false;
                        }
                    }

                    i2++;
                }

                if (cleared == true)
                {
                    break;
                }
            }
        }

        return (position, cleared);
    }

    #endregion

    #region hyperspace functions

    //This returns all jump locations within the requested distance
    public static string[] GetLocations(string currentLocation, float distance)
    {
        Vector3 locationA = new Vector3();
        Vector3 locationB = new Vector3();

        List<string> locations = new List<string>();

        //This loads the Json file
        TextAsset starSystemFile = Resources.Load("Data/Files/StarSystems") as TextAsset;
        StarSystems starSystems = JsonUtility.FromJson<StarSystems>(starSystemFile.text);

        bool locationFound = false;

        //This finds specific data on the location
        foreach (StarSystem starSystem in starSystems.starSystemsData)
        {
            if (starSystem.Planet == currentLocation)
            {
                locationA.x = starSystem.X;
                locationA.y = starSystem.Z * 0.5f; //The y coord is actaully the z coord and vice versa b/c original data was vector 2 
                locationA.z = starSystem.Y;
                locationFound = true;
                break;
            }
        }

        if (locationFound == true)
        {
            foreach (StarSystem starSystem in starSystems.starSystemsData)
            {
                locationB.x = starSystem.X;
                locationB.y = starSystem.Z * 0.5f; //The y coord is actaully the z coord and vice versa b/c original data was vector 2 
                locationB.z = starSystem.Y;

                float tempDistance = Vector3.Distance(locationA, locationB);

                if (tempDistance < distance)
                {
                    locations.Add(starSystem.Planet);
                }
            }
        }

        return locations.ToArray();
    }

    //This unloads the current location and loads a new one from the avaiblible locations while simulating a hyperspace jump
    public static IEnumerator ChangeLocation(string location, Vector3 entryPosition = new Vector3())
    {
        HudFunctions.AddToShipLog("Course set: " + location.ToUpper());

        //This gets several important references
        Scene scene = SceneFunctions.GetScene();
        SmallShip smallShip = GetSmallShip();
        ExploreManager exploreManager = GetExploreManager();

        //This checks whether the jump should be cancelled or not
        bool cancelJump = false;

        Vector3 forwardRaycast = smallShip.gameObject.transform.position + (smallShip.gameObject.transform.forward * 10);

        RaycastHit hit;

        if (Physics.SphereCast(forwardRaycast, 50, smallShip.gameObject.transform.TransformDirection(Vector3.forward), out hit, 10000))
        {
            cancelJump = true;
            HudFunctions.AddToShipLog("Exit vector not clear. Cancelling jump.");
            AudioFunctions.PlayAudioClip(smallShip.audioManager, "error", "Cockpit", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);
            yield return new WaitForSeconds(2);
            HudFunctions.AddToShipLog("Ensure exit vector is clear of objects before jumping.");
        }

        Debug.DrawRay(scene.planetCamera.gameObject.transform.position, scene.planetCamera.gameObject.transform.TransformDirection(Vector3.forward) * 5000, Color.red, 500);

        int layerMask = 1 << 27;

        if (Physics.Raycast(scene.planetCamera.gameObject.transform.position, scene.planetCamera.gameObject.transform.TransformDirection(Vector3.forward), out hit, 100, layerMask))
        {
            cancelJump = true;
            HudFunctions.AddToShipLog("Planetary gravity well disrupting calcualtions. Cancelling jump.");
            AudioFunctions.PlayAudioClip(smallShip.audioManager, "error", "Cockpit", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);
            yield return new WaitForSeconds(2);
            HudFunctions.AddToShipLog("Try jumping from another system.");
        }

        if (cancelJump == false)
        {
            exploreManager.hyperspace = true;

            smallShip.controlLock = true; //This locks the player ship controls so the ship remains correctly orientated to the hyperspace effect
            smallShip.invincible = true; //This sets the ship to invincible so that any objects the ship may hit while the scene changes doesn't destroy it

            //This plays the hyperspace entry sound
            AudioFunctions.PlayAudioClip(smallShip.audioManager, "HyperspaceEntry", "Cockpit", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);

            yield return new WaitForSeconds(2);

            //This marks the jump time
            float time = Time.unscaledTime;

            //This clears the current location
            SceneFunctions.ClearLocation();
            exploreManager.shipPositions.Clear();
            exploreManager.shipClearance.Clear();

            HudFunctions.AddToShipLog("Hyperdrive Activated");

            //This makes the stars stretch out
            scene.planetCamera.GetComponent<Camera>().enabled = false;
            scene.mainCamera.GetComponent<Camera>().enabled = false;

            Task a = new Task(SceneFunctions.StretchStarfield());
            while (a.Running == true) { yield return null; }

            //This activates the hyperspace tunnel
            if (scene.hyperspaceTunnel != null)
            {
                scene.hyperspaceTunnel.SetActive(true);
            }

            //This changes the ships position in the galaxy
            SetGalaxyLocation(location);

            //This finds and loads all 'preload' nodes for the new location
            Task b = new Task(LoadScenery(location));
            while (b.Running == true) { yield return null; }

            //This loads the ships in the area
            Task c = new Task(LoadShips(location));
            while (c.Running == true) { yield return null; }

            //This sets the position of the ship in the new location designated in the node
            smallShip.transform.localPosition = entryPosition;

            //This yield allows the new position and rotation to be registered in the rigidbody component which is needed for the shrinkstarfield function
            yield return null;

            //This ensures that hyperspace continues for atleast ten seconds
            while (time + 10 > Time.unscaledTime)
            {
                yield return null;
            }

            //This deactivates the hyperspace tunnel
            if (scene.hyperspaceTunnel != null)
            {
                scene.hyperspaceTunnel.SetActive(false);
            }

            //This plays the hyperspace exit
            AudioFunctions.PlayAudioClip(smallShip.audioManager, "HyperspaceExit", "Cockpit", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);

            //This shrinks the starfield
            Task d = new Task(SceneFunctions.ShrinkStarfield());
            while (d.Running == true) { yield return null; }

            //This makes the stars stretch out
            scene.planetCamera.GetComponent<Camera>().enabled = true;
            scene.mainCamera.GetComponent<Camera>().enabled = true;

            HudFunctions.UpdateLocation(scene.currentLocation, "");
            HudFunctions.AddToShipLog("Exiting Hyperspace at: " + location.ToUpper());

            //This gets a new list of jump points
            exploreManager.availibleLocations = GetLocations(location, 1000);

            //This unlocks the player controls and turns off invincibility on the player ship
            smallShip.controlLock = false;

            //This selects the next avaible destination
            SelectNextJumpLocation(exploreManager);

            yield return new WaitForSeconds(3);

            HudFunctions.DisplayLargeMessage(location.ToUpper());

            smallShip.invincible = false;
            exploreManager.hyperspace = false;
        }    
    }

    //This sets the galaxy camera position
    public static void SetGalaxyLocation(string location)
    {
        Scene scene = SceneFunctions.GetScene();
        var planetData = SceneFunctions.FindLocation(location);
        scene.currentLocation = planetData.planet;
        scene.planetType = planetData.type;
        scene.planetSeed = planetData.seed;
        SceneFunctions.MoveStarfieldCamera(planetData.location);
    }

    //This sets the position of the nav point marker
    public static void SetNavPointMarker(string location)
    {
        Scene scene = SceneFunctions.GetScene();
        var planetData = SceneFunctions.FindLocation(location);
        scene.planetType = planetData.type;
        scene.planetSeed = planetData.seed;

        if (scene.navPointMarker == null)
        {
            scene.navPointMarker = new GameObject();
            scene.navPointMarker.name = "NavPointMarker";
        }

        scene.navPointMarker.transform.position = planetData.location;
    }

    //This selects the next avaiblible jump location
    public static void SelectNextJumpLocation(ExploreManager exploreManager)
    {
        Keyboard keyboard = Keyboard.current;

        if (keyboard.nKey.isPressed == true & exploreManager.pressedTime + 0.25f < Time.time)
        {
            exploreManager.pressedTime = Time.time + 0.5f;

            bool nextLocation = false;
            string newLocation = "none";

            foreach(string location in exploreManager.availibleLocations)
            {
                if (exploreManager.selectedLocation == "none")
                {
                    newLocation = location;
                    break;
                }
                else if (nextLocation == false)
                {
                    if (location == exploreManager.selectedLocation)
                    {
                        nextLocation = true;
                    }
                }
                else if (nextLocation == true)
                {
                    newLocation = location;
                    break;
                }
            }

            if (newLocation == "none")
            {
                if (exploreManager.availibleLocations.Length > 0)
                {
                    newLocation = exploreManager.availibleLocations[0];
                }
            }

            exploreManager.selectedLocation = newLocation;

            SetNavPointMarker(newLocation);

            HudFunctions.UpdateLocation(exploreManager.scene.currentLocation, newLocation);
            HudFunctions.AddToShipLog("New hyperspace location set: " + newLocation.ToUpper());

            SmallShip smallShip = exploreManager.smallShip;

            if (smallShip == null)
            {
                smallShip = GetSmallShip();
            }

            AudioFunctions.PlayAudioClip(smallShip.audioManager, "beep01_toggle", "Cockpit", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);

            exploreManager.pressedTime = Time.time;
        }
    }

    //This activates the hyperdrive
    public static void ActivateHyperspace(ExploreManager exploreManager)
    {
        if (exploreManager.hyperspace == false & exploreManager.hyperdriveActive == true)
        {
            Keyboard keyboard = Keyboard.current;

            if (keyboard.spaceKey.isPressed == true)
            {
                if (exploreManager.hud != null)
                {
                    if (exploreManager.hud.hyperspaceValue < 100)
                    {
                        exploreManager.hud.hyperspaceValue += exploreManager.hyperspaceAddition;

                        AudioFunctions.PlayNavCompNoise(exploreManager);

                        if (exploreManager.hud.hyperspaceValue >= 100)
                        {
                            exploreManager.activateHyperspace = true;
                        }
                    }
                }
            }
            else
            {
                if (exploreManager.hud.hyperspaceValue > 0)
                {
                    exploreManager.hud.hyperspaceValue -= 1f;
                }

                if (exploreManager.navCompAudioSource != null)
                {
                    if (exploreManager.navCompAudioSource.isPlaying == true)
                    {
                        exploreManager.navCompAudioSource.Stop();
                    }
                }
            }

            if (exploreManager.activateHyperspace == true)
            {
                string location = exploreManager.selectedLocation;

                if (location != "none")
                {
                    Task a = new Task(ChangeLocation(location));
                }

                exploreManager.activateHyperspace = false;

                if (exploreManager.navCompAudioSource != null)
                {
                    if (exploreManager.navCompAudioSource.isPlaying == true)
                    {
                        exploreManager.navCompAudioSource.Stop();
                    }
                }
            }
        }
        else
        {
            if (exploreManager.hud.hyperspaceValue > 0)
            {
                exploreManager.hud.hyperspaceValue -= 1f;
            }

            if (exploreManager.navCompAudioSource != null)
            {
                if (exploreManager.navCompAudioSource.isPlaying == true)
                {
                    exploreManager.navCompAudioSource.Stop();
                }
            }
        }
    }

    //This checks if the hyperdrive is active or not
    public static void HyperdriveAvailible(ExploreManager exploreManager)
    { 
        if (exploreManager.scene.navPointMarker != null & exploreManager.scene.starfieldCamera.gameObject != null)
        {
            GameObject starfieldTargetPosition = exploreManager.scene.navPointMarker;
            GameObject starfieldCurrentPosition = exploreManager.scene.starfieldCamera.gameObject;

            Vector3 targetPosition = starfieldTargetPosition.transform.position - starfieldCurrentPosition.transform.position;
            float forward = Vector3.Dot(starfieldCurrentPosition.transform.forward, targetPosition.normalized);

            SmallShip smallShip = exploreManager.smallShip;

            if (smallShip == false)
            {
                smallShip = GetSmallShip();
            }

            if (forward > 0.99f)
            {
                exploreManager.hyperdriveActive = true;
                exploreManager.hud.hyperdriveActive = true;

                if (exploreManager.beepedOnAvailibility == false)
                {
                    if (exploreManager.smallShip == null)
                    {

                    }

                    AudioFunctions.PlayAudioClip(smallShip.audioManager, "beep_positive2", "Cockpit", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);
                    exploreManager.beepedOnAvailibility = true;
                }

            }
            else
            {
                exploreManager.hyperdriveActive = false;
                exploreManager.hud.hyperdriveActive = false;
                exploreManager.beepedOnAvailibility = false;
            }
        }
        else if (exploreManager.hyperspace == false)
        {
            exploreManager.hyperdriveActive = false;

            if (exploreManager.hud != null)
            {
                exploreManager.hud.hyperdriveActive = false;
                exploreManager.beepedOnAvailibility = false;
            }
        }
        else
        {
            exploreManager.hyperdriveActive = false; //The hyperdrive is not availible to be activated b/c it is already active

            if (exploreManager.hud != null)
            {
                exploreManager.hud.hyperdriveActive = true;
            }
        }
    }

    #endregion

    #region exit functions

    //This activates the exit menu
    public static void ActivateExitMenu(ExploreManager missionManager)
    {
        Keyboard keyboard = Keyboard.current;

        if (keyboard.escapeKey.isPressed == true & missionManager.pressedTime < Time.time)
        {
            ExitMenuFunctions.DisplayExitMenu(true);
            missionManager.pressedTime = Time.time + 0.5f;
        }

    }

    #endregion

    #region Explore Manager Utils

    //This returns the explore manager
    public static ExploreManager GetExploreManager()
    {
        ExploreManager exploreManager = GameObject.FindObjectOfType<ExploreManager>();

        if (exploreManager == null)
        {
            GameObject exploreManagerGO = new GameObject();
            exploreManagerGO.name = "ExploreManager";
            exploreManager = exploreManagerGO.AddComponent<ExploreManager>();
        }

        return exploreManager;
    }

    //This gets the player ship smallship scripy
    public static SmallShip GetSmallShip()
    {
        SmallShip smallShip = null;

        ExploreManager exploreManager = GetExploreManager();

        smallShip = exploreManager.scene.mainShip.GetComponent<SmallShip>();
        exploreManager.smallShip = smallShip;

        return smallShip;
    }

    #endregion
}
