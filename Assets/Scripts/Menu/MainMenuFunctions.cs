using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public static class MainMenuFunctions
{
    #region disclaimer, title screen, and menu loading sequence

    //This displays the disclaimer, the title, then loads the menu
    public static IEnumerator RunMenu()
    {
        //This changes the cursor from unity's default to the open galaxy cursor
        OGSettingsFunctions.SetOGCursor();

        GameObject background = LoadBackground();
        GameObject disclaimer = LoadDisclaimer();
        GameObject title = LoadTitle();
        GameObject menu = LoadMenu();

        //This starts the menu background music
        PlayBackgroundMusic(true);

        if (background != null)
        {
            CanvasGroup canvasGroup = background.GetComponent<CanvasGroup>();

            if (canvasGroup != null)
            {
                Task a = new Task(FadeInCanvas(canvasGroup, 0.5f));
            }
        }

        yield return new WaitForSeconds(2);

        if (disclaimer != null)
        {
            CanvasGroup canvasGroup = disclaimer.GetComponent<CanvasGroup>();

            if (canvasGroup != null)
            {
                Task a = new Task(FadeInCanvas(canvasGroup, 0.5f));

                yield return new WaitForSeconds(5);

                Task b = new Task(FadeOutAndDeactivate(canvasGroup, 0.5f));
            }
        }

        yield return new WaitForSeconds(2f);

        if (title != null)
        {
            CanvasGroup canvasGroup = title.GetComponent<CanvasGroup>();

            if (canvasGroup != null)
            {
                Task a = new Task(FadeInCanvas(canvasGroup, 0.5f));

                yield return new WaitForSeconds(7);

                Task b = new Task(FadeOutAndDeactivate(canvasGroup, 0.5f));
            }
        }

        yield return new WaitForSeconds(1f);

        if (menu != null)
        {
            CanvasGroup canvasGroup = menu.GetComponent<CanvasGroup>();

            if (canvasGroup != null)
            {
                Task a = new Task(FadeInCanvas(canvasGroup, 0.5f));
            }
        }

        if (background != null)
        {
            CanvasGroup canvasGroup = background.GetComponent<CanvasGroup>();

            if (canvasGroup != null)
            {
                Task a = new Task(FadeOutAndDeactivate(canvasGroup, 0.5f));
            }
        }
    }

    //This loads the disclaimer
    public static GameObject LoadDisclaimer()
    {      
        GameObject menuPrefab = Resources.Load("Menu/Disclaimer") as GameObject;
        GameObject disclaimer = GameObject.Instantiate(menuPrefab);
        disclaimer.name = "Disclaimer";
        return disclaimer;
    }

    //This loads the Title Menu
    public static GameObject LoadTitle()
    {
        GameObject menuPrefab = Resources.Load("Menu/Title") as GameObject;
        GameObject title = GameObject.Instantiate(menuPrefab);
        GameObject tip = GameObject.Find("Tip");
        title.name = "Title";

        if (tip != null)
        {
            Text messageText = tip.GetComponent<Text>();
            DisplayMessageOnTitleScreen(messageText);
        }

        return title;
    }

    //This loads the background
    public static GameObject LoadBackground()
    {
        GameObject menuPrefab = Resources.Load("Menu/Background") as GameObject;
        GameObject background = GameObject.Instantiate(menuPrefab);
        background.name = "Background";
        return background;
    }

    //This Loads the main menu
    public static GameObject LoadMenu()
    {
        GameObject menuPrefab = Resources.Load("Menu/Menu") as GameObject;
        GameObject menu = GameObject.Instantiate(menuPrefab);
        menu.name = "Menu";
        return menu;
    }

    #endregion

    #region menu function dictionary

    //This creates a dictionary of the all the functions the menu can call
    public static void CreateFunctionDictionary(MainMenu mainMenu)
    {
        mainMenu.functions = new Dictionary<string, System.Delegate>();

        //Add your functions here
        mainMenu.functions.Add("LoadMainMission", new System.Action<string>(LoadMission));
        mainMenu.functions.Add("LoadCustomMission", new System.Action<string>(LoadCustomMission));
        mainMenu.functions.Add("LoadMissionEditor", new System.Action(LoadMissionEditor));
        mainMenu.functions.Add("QuitToDesktop", new System.Action(QuitToDesktop));
        mainMenu.functions.Add("SelectCockpitAssets", new System.Action<string>(SelectCockpitAssets));
        mainMenu.functions.Add("SetWindowMode", new System.Action<string>(SetWindowMode));
        mainMenu.functions.Add("SetEditorWindowMode", new System.Action<string>(SetEditorWindowMode));
        mainMenu.functions.Add("ToggleDebugging", new System.Action<string>(ToggleDebugging));
        mainMenu.functions.Add("ChangeResolution", new System.Action<string>(ChangeResolution));
        mainMenu.functions.Add("ChangePlanetTextureResolution", new System.Action<string>(ChangePlanetTextureResolution));
        mainMenu.functions.Add("ActivateSubMenu", new System.Action<string>(ActivateSubMenu));
        mainMenu.functions.Add("InvertHorizontal", new System.Action<string>(InvertHorizontal));
        mainMenu.functions.Add("InvertVertical", new System.Action<string>(InvertVertical));
        mainMenu.functions.Add("OpenWebAddress", new System.Action<string>(OpenWebAddress));
    }

    #endregion

    #region menu loading functions

    //This grabs all the button prefabs ready to instantiate
    public static void LoadButtons()
    {
        MainMenu mainMenu = GameObject.FindObjectOfType<MainMenu>();

        mainMenu.buttons = Resources.LoadAll<GameObject>("MenuButtons");
    }

    //This loads json file and instaniates the buttons so the menu is ready to use
    public static void LoadMenuData()
    {
        //This function grabs all the button prefabs ready to instantiate
        LoadButtons();

        //This gets the reference to the main menu (NOTE: if you have more than one main menu you will need to change this line as this line will just grab the first menu reference it can find)
        MainMenu mainMenu = GameObject.FindObjectOfType<MainMenu>();

        //This creates the function dictionary
        CreateFunctionDictionary(mainMenu);

        //This loads all the information for the menu from the Json file
        TextAsset menuItemsFile = Resources.Load("Data/Files/Menu") as TextAsset;
        MenuItems menuItems = JsonUtility.FromJson<MenuItems>(menuItemsFile.text);

        //This creates the menu lists ready to use
        List<string> subMenus = new List<string>();
        mainMenu.SubMenus = new List<GameObject>();

        //This creates a list of all the sub menus
        foreach (MenuItem menuItem in menuItems.menuData)
        {
            bool addMenu = true;

            foreach (string menuItem2 in subMenus)
            {
                if (menuItem.ParentMenu == menuItem2)
                {
                    addMenu = false;
                }
            }

            if (addMenu == true & menuItem.ParentMenu != "none")
            {
                subMenus.Add(menuItem.ParentMenu);
            }  
        }

        //This adds three extra sub menus just for open galaxy
        subMenus.Add("Missions");
        subMenus.Add("Custom Missions");

        //This allows the script to log the first menu created. It will be the first loaded
        bool firstSubMenuLogged = false;

        //This creates all the sub menus
        foreach (string tempMenu in subMenus)
        {
            if (firstSubMenuLogged == false)
            {
                mainMenu.firstMenu = tempMenu;
                firstSubMenuLogged = true;
            }

            GameObject subMenu = new GameObject();
            subMenu.transform.SetParent(mainMenu.MenuContent.transform);
            mainMenu.SubMenus.Add(subMenu);
            subMenu.AddComponent<RectTransform>();
            RectTransform tempRect = subMenu.GetComponent<RectTransform>();
            tempRect.anchorMin = new Vector2(0, 1);
            tempRect.anchorMax = new Vector2(0, 1);
            tempRect.pivot = new Vector2(0, 1);
            tempRect.pivot = new Vector2(0, 1);
            subMenu.transform.localPosition = new Vector3(0, 0, 0);
            subMenu.transform.localScale = new Vector3(1, 1, 1);
            subMenu.name = tempMenu + "_Settings";

            //This stores the custom mission menu for later access
            if (subMenu.name == "Custom Missions_Settings")
            {
                mainMenu.CustomMissionMenu = subMenu;
            }
        }

        //This adds the menu buttons to the left bar
        float buttonDrop = 20;

        foreach (MenuItem menuItem in menuItems.menuData)
        {            
            if (menuItem.ParentMenu == "none")
            {
                buttonDrop = CreateParentMenuButton(mainMenu, buttonDrop, menuItem.Name, menuItem.Type, menuItem.Function, menuItem.Variable);
            }
        }

        //This adds the actual menu content
        foreach (GameObject subMenu in mainMenu.SubMenus)
        {
            buttonDrop = 20;

            foreach (MenuItem menuItem in menuItems.menuData)
            {
                if (menuItem.ParentMenu + "_Settings" == subMenu.name)
                {
                    buttonDrop = CreateSubMenuButton(mainMenu, subMenu, buttonDrop, menuItem.Name, menuItem.Type, menuItem.Function, menuItem.Description, menuItem.Variable);
                }
            }

            //This sets the size of the sub menu according to the how many buttons have been generated
            RectTransform rt = subMenu.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(0, buttonDrop);
        }

        //This adds all the missions buttons for internal missions
        Object[] mainMissions = Resources.LoadAll("Data/Files/Missions_Main", typeof(TextAsset));
        CreateMissionButtons(mainMenu, mainMissions, "Missions_Settings", "LoadMainMission");

        //This adds all the missions buttons for external missions
        var info = new DirectoryInfo(Application.persistentDataPath + "/Custom Missions/");

        if (info.Exists == false)
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Custom Missions/");
            info = new DirectoryInfo(Application.persistentDataPath + "/Custom Missions/");
        }

        List<TextAsset> customMissionsList = new List<TextAsset>();

        if (info.Exists == true)
        {
            var fileInfo = info.GetFiles("*.json");

            foreach (FileInfo file in fileInfo)
            {
                string path = Application.persistentDataPath + "/Custom Missions/" + file.Name;
                string missionDataString = File.ReadAllText(path);
                TextAsset missionDataTextAsset = new TextAsset(missionDataString);
                missionDataTextAsset.name = System.IO.Path.GetFileNameWithoutExtension(path);
                customMissionsList.Add(missionDataTextAsset);
            }
        }

        Object[] customMissions = customMissionsList.ToArray();
        CreateMissionButtons(mainMenu, customMissions, "Custom Missions_Settings", "LoadCustomMission");

        //This turns off all the sub menus
        foreach (GameObject subMenu in mainMenu.SubMenus)
        {
            subMenu.SetActive(false);
        }

        //This loads the first menu created
        ActivateSubMenu(mainMenu.firstMenu);

    }

    //This creates parent menu buttons on the left hand side
    public static float CreateParentMenuButton(MainMenu mainMenu, float buttonDrop, string name, string buttonType, string function, string variable)
    {
        GameObject button = null;

        foreach (GameObject tempButton in mainMenu.buttons)
        {
            if (tempButton.name == buttonType)
            {
                button = GameObject.Instantiate<GameObject>(tempButton);
            }
        }

        if (button != null)
        {

            button.GetComponentInChildren<Text>().text = name;

            button.transform.SetParent(mainMenu.MenuBar.transform);
            button.transform.localPosition = new Vector3(20, 0, 0);
            button.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, buttonDrop, 20);
            button.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

            if (button.GetComponent<Button>() != null & function != "none")
            {

                if (variable == "none")
                {
                    button.GetComponent<Button>().onClick.AddListener(() => mainMenu.functions[function].DynamicInvoke());
                }
                else
                {
                    button.GetComponent<Button>().onClick.AddListener(() => mainMenu.functions[function].DynamicInvoke(variable));
                }

            }

            button.name = name;

            buttonDrop = buttonDrop + button.GetComponent<ButtonInfo>().buttonShift;

        }

        return buttonDrop;
    }

    //This creates a sub menu button
    public static float CreateSubMenuButton(MainMenu mainMenu, GameObject subMenu, float buttonDrop, string buttonName, string buttonType, string functionName, string buttonDescription = "", string variable = "none")
    {
        GameObject button = null;

        foreach (GameObject tempButton in mainMenu.buttons)
        {
            if (tempButton.name == buttonType)
            {
                button = GameObject.Instantiate<GameObject>(tempButton);
            }
        }

        button.transform.SetParent(subMenu.transform);
        button.GetComponent<ButtonInfo>().buttonName.text = buttonName;

        if (button.GetComponent<ButtonInfo>().description != null)
        {
            button.GetComponent<ButtonInfo>().description.text = buttonDescription;
        }

        button.transform.localPosition = new Vector3(20, 0, 0);
        button.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, buttonDrop, 20);
        button.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
       
        if (button.GetComponent<Button>() != null)
        {
            if (variable == "none")
            {
                button.GetComponent<Button>().onClick.AddListener(() => mainMenu.functions[functionName].DynamicInvoke());
            }
            else
            {
                button.GetComponent<Button>().onClick.AddListener(() => mainMenu.functions[functionName].DynamicInvoke(variable));
            }
        }

        if (button.GetComponent<ButtonInfo>() != null)
        {
            buttonDrop = buttonDrop + button.GetComponent<ButtonInfo>().buttonShift;
        }
        else
        {
            buttonDrop = 60;
        }

        return buttonDrop;
    }

    //This adds a button for each mission avaible in the folder
    public static void CreateMissionButtons(MainMenu mainMenu, Object[] missions, string subMenuName, string function)
    {
        foreach (GameObject subMenu in mainMenu.SubMenus)
        {
            if (subMenu.name == subMenuName)
            {
                float buttonDrop = 20;

                buttonDrop = CreateSubMenuButton(mainMenu, subMenu, buttonDrop, "Back to Start Game", "ContentButton02", "ActivateSubMenu", "", "Start Game");

                foreach (Object mission in missions)
                {
                    buttonDrop = CreateSubMenuButton(mainMenu, subMenu, buttonDrop, mission.name, "ContentButton02", function, "", mission.name);
                }

                //This sets the size of the sub menu according to the how many buttons have been generated
                RectTransform rt = subMenu.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(0, buttonDrop);
            }
        }
    }

    //This reloads the custom missions
    public static void ReloadCustomMissions()
    {
        //This gets rid of the old custom menu buttons
        MainMenu mainMenu = GameObject.FindObjectOfType<MainMenu>();

        Transform[] buttons = GameObjectUtils.GetAllChildTransforms(mainMenu.CustomMissionMenu.transform);

        foreach (Transform button in buttons)
        {
            GameObject.Destroy(button.gameObject);
        }

        //This generates new custom mission buttons
        var info = new DirectoryInfo(Application.persistentDataPath + "/Custom Missions/");

        if (info.Exists == false)
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Custom Missions/");
            info = new DirectoryInfo(Application.persistentDataPath + "/Custom Missions/");
        }

        List<TextAsset> customMissionsList = new List<TextAsset>();

        if (info.Exists == true)
        {
            var fileInfo = info.GetFiles("*.json");

            foreach (FileInfo file in fileInfo)
            {
                string path = Application.persistentDataPath + "/Custom Missions/" + file.Name;
                string missionDataString = File.ReadAllText(path);
                TextAsset missionDataTextAsset = new TextAsset(missionDataString);
                missionDataTextAsset.name = System.IO.Path.GetFileNameWithoutExtension(path);
                customMissionsList.Add(missionDataTextAsset);
            }
        }

        Object[] customMissions = customMissionsList.ToArray();
        CreateMissionButtons(mainMenu, customMissions, "Custom Missions_Settings", "LoadCustomMission");
    }

    #endregion

    #region navigation functions

    //This activate a sub menu
    public static void ActivateSubMenu(string menuName)
    {
        MainMenu mainMenu = GameObject.FindObjectOfType<MainMenu>();

        int i = 1;

        foreach (GameObject subMenu in mainMenu.SubMenus)
        {

            if (subMenu.name == menuName + "_Settings")
            {
                //This resizes the content window to fit the sub menu
                RectTransform subMenuRT = subMenu.GetComponent<RectTransform>(); 
                RectTransform contentWindowRT = subMenu.GetComponentInParent<RectTransform>();
                contentWindowRT.sizeDelta = new Vector2(0, subMenuRT.sizeDelta.y);
                
                //This activates the selected menu
                subMenu.SetActive(true);
                mainMenu.MenuTitle.text = menuName;

                //This selects the first button in the menu
                Button firstButton = subMenu.GetComponentInChildren<Button>();

                if (firstButton != null)
                {
                    subMenu.GetComponentInChildren<Button>().Select();
                }

            }
            else
            {
                subMenu.SetActive(false); //This sets all the menu lists to inactive
            }

            i = i + 1;
        }

        mainMenu.activeMenu = menuName;
    }

    //Move back to parent menu
    public static void ActivateParentMenu()
    {
        MainMenu mainMenu = GameObject.FindObjectOfType<MainMenu>();

        //This loads all the information for the menu from the Json file
        TextAsset menuItemsFile = Resources.Load("Data/Files/Menu") as TextAsset;
        MenuItems menuItems = JsonUtility.FromJson<MenuItems>(menuItemsFile.text);

        foreach (MenuItem menuItem in menuItems.menuData)
        {
            if (menuItem.Name == mainMenu.activeMenu)
            {
                if (menuItem.ParentMenu != "none")
                {
                    ActivateSubMenu(menuItem.ParentMenu);
                }
                else
                {
                    ActivateSubMenu(mainMenu.firstMenu);
                }
            }
        }
    }

    #endregion

    #region menu functions

    //Displays a message on the title screen
    public static void DisplayMessageOnTitleScreen(Text titleScreenMessageBox)
    {
        string[] messages = new string[10];
        messages[0] = "Welcome to Open Galaxy. Version 0.9.62 completes all techincal goals for version 1.0.";
        messages[1] = "Open Galaxy's aim is to be a platform for X-Wing and Tie Fighter style custom missions.";
        messages[2] = "Open Galaxy has an inbuilt and easy to use mission editor. Why not try it out?";
        messages[3] = "Flying a ship isn't like dusting crops. Familiarise yourself with the controls first.";
        messages[4] = "Open Galaxy generates a real Star Wars galaxy with accurately positioned stars and planets.";
        messages[5] = "You can lower the quality of the planet heightmap for faster loadtimes.";
        messages[6] = "Open Galaxy is designed for both a controller and keyboard and mouse.";
        messages[7] = "Open Galaxy is possible through the generous contribution of others. Check out the credits.";
        messages[8] = "Open Galaxy is in active development, so if you find a bug, report it.";
        messages[9] = "Post 1.0 plans for Open Galaxy include terrain, ion cannons and a lot more missions";
        Random.InitState(System.DateTime.Now.Millisecond);
        int randomMessageNo = Random.Range(0, 9);
        titleScreenMessageBox.text = messages[randomMessageNo];
    }

    //This loads a custom mission
    public static void LoadCustomMission(string name)
    {
        Task a = new Task(MissionFunctions.RunMission(name, Application.persistentDataPath + "/Custom Missions/", true));

        GameObject menu = GameObject.Find("Menu");

        if (menu != null)
        {
            CanvasGroup canvasGroup = menu.GetComponent<CanvasGroup>();
            Task i = new Task(FadeOutAndDeactivate(canvasGroup, 0.25f));
        }
    }

    //This loads a main mission
    public static void LoadMission(string name)
    {
        Task a = new Task(MissionFunctions.RunMission(name, "Data/Files/Missions_Main/"));

        GameObject menu = GameObject.Find("Menu");

        if (menu != null)
        {
            CanvasGroup canvasGroup = menu.GetComponent<CanvasGroup>();
            Task i = new Task(FadeOutAndDeactivate(canvasGroup, 0.25f));
        }
    }

    //This loads the mission editor
    public static void LoadMissionEditor()
    {
        GameObject missionEditor = GameObject.Find("MissionEditor");
        
        if (missionEditor == null)
        {
            GameObject tempMissionEditor = Resources.Load("MissionEditor/MissionEditor") as GameObject;
            missionEditor = GameObject.Instantiate(tempMissionEditor);
            missionEditor.name = "MissionEditor";
        }

        missionEditor.SetActive(true);

        PlayBackgroundMusic(false);
    }
    
    //This changes the heightmap resolution
    public static void ChangePlanetTextureResolution(string resolution)
    {
        OGSettings settings = OGSettingsFunctions.GetSettings();

        settings.heightMapResolution = int.Parse(resolution);

        OGSettingsFunctions.SaveSettingsData();

        ActivateSubMenu("Settings");

        OutputMenuMessage("The planet resolution was set to " + resolution);
    }

    //This sets the screen resolution
    public static void ChangeResolution(string resolution)
    {
        OGSettings settings = OGSettingsFunctions.GetSettings();

        FullScreenMode screenMode = Screen.fullScreenMode;

        OutputMenuMessage("The resolutionwas set to " + resolution);

        if (resolution == "Detect Screen Resolution")
        {
            int width = Display.main.systemWidth;
            int height = Display.main.systemHeight;
            Screen.SetResolution(width, height, screenMode);
            settings.screenResX = width;
            settings.screenResY = height;
        }
        else if (resolution == "640 x 480 (4:3)")
        {
            Screen.SetResolution(640, 480, screenMode);
            settings.screenResX = 640;
            settings.screenResY = 480;
        }
        else if (resolution == "640 x 360 (16:9)")
        {
            Screen.SetResolution(640, 360, screenMode);
            settings.screenResX = 640;
            settings.screenResY = 360;
        }
        else if (resolution == "640 x 400 (16:10)")
        {
            Screen.SetResolution(640, 400, screenMode);
            settings.screenResX = 640;
            settings.screenResY = 400;
        }
        else if (resolution == "800 x 600 (4:3)")
        {
            Screen.SetResolution(800, 600, screenMode);
            settings.screenResX = 800;
            settings.screenResY = 600;
        }
        else if (resolution == "848 x 450 (16:9)")
        {
            Screen.SetResolution(848, 450, screenMode);
            settings.screenResX = 848;
            settings.screenResY = 450;
        }
        else if (resolution == "960 x 600 (16:10)")
        {
            Screen.SetResolution(960, 600, screenMode);
            settings.screenResX = 960;
            settings.screenResY = 600;
        }
        else if (resolution == "1024 x 768 (4:3)")
        {
            Screen.SetResolution(1024, 768, screenMode);
            settings.screenResX = 1024;
            settings.screenResY = 768;
        }
        else if (resolution == "1024 x 576 (16:9)")
        {
            Screen.SetResolution(1024, 576, screenMode);
            settings.screenResX = 1024;
            settings.screenResY = 576;
        }
        else if (resolution == "1024 x 640 (16:10)")
        {
            Screen.SetResolution(1024, 640, screenMode);
            settings.screenResX = 1024;
            settings.screenResY = 640;
        }
        else if (resolution == "1220 x 915 (4:3)")
        {
            Screen.SetResolution(1220, 915, screenMode);
            settings.screenResX = 1220;
            settings.screenResY = 915;
        }
        else if (resolution == "1220 x 720 (16:9)")
        {
            Screen.SetResolution(1220, 720, screenMode);
            settings.screenResX = 1220;
            settings.screenResY = 620;
        }
        else if (resolution == "1280 x 800 (16:10)")
        {
            Screen.SetResolution(1280, 800, screenMode);
            settings.screenResX = 1280;
            settings.screenResY = 800;
        }
        else if (resolution == "1680 × 1260 (4:3)")
        {
            Screen.SetResolution(1680, 1260, screenMode);
            settings.screenResX = 1680;
            settings.screenResY = 1260;
        }
        else if (resolution == "1680 × 945 (16:9)")
        {
            Screen.SetResolution(1680, 945, screenMode);
            settings.screenResX = 1680;
            settings.screenResY = 945;
        }
        else if (resolution == "1680 × 1050 (16:10)")
        {
            Screen.SetResolution(1680, 1050, screenMode);
            settings.screenResX = 1680;
            settings.screenResY = 1050;
        }
        else if (resolution == "1920 x 1440 (4:3)")
        {
            Screen.SetResolution(1920, 1440, screenMode);
            settings.screenResX = 1920;
            settings.screenResY = 1440;
        }
        else if (resolution == "1920 x 1080 (16:9)")
        {
            Screen.SetResolution(1920, 1080, screenMode);
            settings.screenResX = 1920;
            settings.screenResY = 1080;
        }
        else if (resolution == "1920 x 1200 (16:10)")
        {
            Screen.SetResolution(1920, 1200, screenMode);
            settings.screenResX = 1920;
            settings.screenResY = 1200;
        }
        else if (resolution == "2560 x 1920 (4:3)")
        {
            Screen.SetResolution(2560, 1920, screenMode);
            settings.screenResX = 2560;
            settings.screenResY = 1920;
        }
        else if (resolution == "2560 x 1440 (16:9)")
        {
            Screen.SetResolution(2560, 1440, screenMode);
            settings.screenResX = 2560;
            settings.screenResY = 1440;
        }
        else if (resolution == "2560 x 1600 (16:10)")
        {
            Screen.SetResolution(2560, 1600, screenMode);
            settings.screenResX = 2560;
            settings.screenResY = 1600;
        }
        else if (resolution == "3840 x 2880 (4:3)")
        {
            Screen.SetResolution(3840, 2880, screenMode);
            settings.screenResX = 3840;
            settings.screenResY = 2880;
        }
        else if (resolution == "3840 x 2160 (16:9)")
        {
            Screen.SetResolution(3840, 2160, screenMode);
            settings.screenResX = 3840;
            settings.screenResY = 2160;
        }
        else if (resolution == "3840 x 2400 (16:10)")
        {
            Screen.SetResolution(3840, 2400, screenMode);
            settings.screenResX = 3840;
            settings.screenResY = 2400;
        }
        else if (resolution == "7680 x 5760 (4:3)")
        {
            Screen.SetResolution(7680, 5760, screenMode);
            settings.screenResX = 7680;
            settings.screenResY = 5760;
        }
        else if (resolution == "7680 x 4320 (16:9)")
        {
            Screen.SetResolution(7680, 4320, screenMode);
            settings.screenResX = 7680;
            settings.screenResY = 4320;
        }
        else if (resolution == "7680 x 4800 (16:10)")
        {
            Screen.SetResolution(7680, 4800, screenMode);
            settings.screenResX = 7680;
            settings.screenResY = 4800;
        }

        OGSettingsFunctions.SaveSettingsData();

        ActivateSubMenu("Settings");
    }

    //This selects the cockpit types to be used in the game
    public static void SelectCockpitAssets(string type)
    {
        OGSettingsFunctions.SelectCockpitAssets(type);

        ActivateSubMenu("Settings");

        OutputMenuMessage("The cockpit assets were set to " + type);
    }

    //This sets the window mode
    public static void SetWindowMode(string windowMode)
    {
        OGSettingsFunctions.SetGameWindowMode(windowMode);

        ActivateSubMenu("Settings");

        OutputMenuMessage("The window mode was set to " + windowMode);
    }

    //This sets the window mode
    public static void SetEditorWindowMode(string windowMode)
    {
        OGSettings settings = OGSettingsFunctions.GetSettings();

        settings.editorWindowMode = windowMode;

        OGSettingsFunctions.SaveSettingsData();

        ActivateSubMenu("MissionEditor");

        OutputMenuMessage("The editor window was set to " + windowMode);
    }

    //Stand in function for inverting the horizontal view
    public static void InvertHorizontal(string choice)
    {
        OGSettings settings = OGSettingsFunctions.GetSettings();

        if (choice == "true")
        {
            settings.invertX = true;
            OutputMenuMessage("The x-axis was inverted.");
        }
        else
        {
            settings.invertX = false;
            OutputMenuMessage("The x-axis was set to normal.");
        }

        OGSettingsFunctions.SaveSettingsData();

        ActivateSubMenu("Controls");

    }

    //Stand in function for inverting the vertical view
    public static void InvertVertical(string choice)
    {
        OGSettings settings = OGSettingsFunctions.GetSettings();

        if (choice == "true")
        {
            settings.invertY = true;
            OutputMenuMessage("The y-axis was inverted.");
        }
        else
        {
            settings.invertY = false;
            OutputMenuMessage("The y-axis was set to normal.");
        }

        OGSettingsFunctions.SaveSettingsData();

        ActivateSubMenu("Controls");

    }

    //This turns the debugging window on and off
    public static void ToggleDebugging(string choice)
    {
        if (bool.TryParse(choice, out _))
        {
            Debug.developerConsoleVisible = bool.Parse(choice);

            if (choice.ToLower() == "true")
            {
                Debug.LogError("The developer console was toggled on");
            }
        }

        ActivateSubMenu("Settings");
    }

    //This quits the application and return you to the desk. NOTE: doesn't work in editor (obviously) only build
    public static void QuitToDesktop()
    {
        Debug.Log("Quitting the Game - NOTE: Only works in build");

        Application.Quit();
    }

    //THis opens a website
    public static void OpenWebAddress(string url)
    {
        Application.OpenURL(url);
    }

    //This outputs a message to the menu
    public static void OutputMenuMessage(string message)
    {
        MainMenu mainMenu = GameObject.FindObjectOfType<MainMenu>();

        if (mainMenu.menuMessage != null)
        {
            mainMenu.menuMessage.text = message;
        }
    }

    #endregion

    #region menu utils

    //This toggles the menu music
    public static void PlayBackgroundMusic (bool input)
    {
        MainMenu mainMenu = GameObject.FindObjectOfType<MainMenu>();

        if (mainMenu.musicAudioSoure != null)
        {
            if (input == true)
            {
                mainMenu.musicAudioSoure.Play();
            }
            else
            {
                mainMenu.musicAudioSoure.Pause();
            }
        }
    } 

    //This allows you to pause the game when the menu is active
    public static void PauseGame(bool isPaused)
    {

        if (isPaused == true)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

    }

    //This coroutine can be used to fade the canvas group in
    public static IEnumerator FadeInCanvas(CanvasGroup canvasGroup, float duration)
    {

        //This sets the starting alpha value to 0
        float alpha1 = 0;

        //This fades in the canvas
        while (alpha1 < 1)
        {
            alpha1 = alpha1 + (1f / (60f * duration));
            canvasGroup.alpha = alpha1;
            yield return new WaitForSecondsRealtime(0.016f);
        }


    }

    //This coroutine can be used to fade the canvas group out
    public static IEnumerator FadeOutCanvas(CanvasGroup canvasGroup, float duration)
    {
        //This sets the starting alpha value to 1
        float alpha2 = 1;

        //This fades the canvas out
        while (alpha2 > 0)
        {
            alpha2 = alpha2 - (1f / (60f * duration));
            canvasGroup.alpha = alpha2;
            yield return new WaitForSecondsRealtime(0.016f);
        }
    }

    //This coroutine can be used to fade the canvas group out
    public static IEnumerator FadeOutAndDeactivate(CanvasGroup canvasGroup, float duration)
    {
        //This sets the starting alpha value to 1
        float alpha2 = 1;

        //This fades the canvas out
        while (alpha2 > 0)
        {
            alpha2 = alpha2 - (1f / (60f * duration));
            canvasGroup.alpha = alpha2;
            yield return new WaitForSecondsRealtime(0.016f);
        }

        canvasGroup.gameObject.SetActive(false);
    }

    #endregion

}
