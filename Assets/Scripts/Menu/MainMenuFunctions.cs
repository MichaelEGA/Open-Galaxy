using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public static class MainMenuFunctions
{
    #region disclaimer, title screen, and menu loading sequence

    //This displays the disclaimer, the title, then loads the menu
    public static IEnumerator RunMainMenu()
    {
        //This sets the cursor settings
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //This loads the menu
        GameObject mainMenuGO = LoadMenuPrefab();
        MainMenu mainMenu = mainMenuGO.GetComponent<MainMenu>();

        //This gets the main references
        GameObject background_black = GameObject.Find("Background_Black");
        GameObject background_image = GameObject.Find("Background_Image");
        GameObject disclaimer = GameObject.Find("Disclaimer");
        GameObject title = GameObject.Find("Title");
        GameObject menu = GameObject.Find("Menu");
        GameObject loadingScreen = GameObject.Find("LoadingScreen");

        //This gets the canvas group function
        mainMenu.background_black_cg = background_black.GetComponent<CanvasGroup>();
        mainMenu.background_image_cg = background_image.GetComponent<CanvasGroup>();
        mainMenu.disclaimer_cg = disclaimer.GetComponent<CanvasGroup>();
        mainMenu.title_cg = title.GetComponent<CanvasGroup>();
        mainMenu.menu_cg = menu.GetComponent<CanvasGroup>();
        mainMenu.loadingScreen_cg = loadingScreen.GetComponent<CanvasGroup>();

        //This loads the background image
        Texture2D texture = LoadBackgroundImage();

        RawImage menuBackgroundImage = background_image.GetComponentInChildren<RawImage>();
        menuBackgroundImage.texture = texture;

        //This applies the art credit
        GameObject artCredit = GameObject.Find("ArtCredit");
        Text creditText = artCredit.GetComponent<Text>();
        string artworkName = texture.name;
        creditText.text = artworkName;

        mainMenu.background = texture;

        //This starts the menu background music
        PlayBackgroundMusic(true);

        //This updates the version number on the menu
        UpdateVersionOnMenu();

        //Fade in background image
        Task a = new Task(FadeInCanvas(mainMenu.background_image_cg, 0.5f));
   
        yield return new WaitForSeconds(2);

        //Fade in and out disclaimer
        Task b = new Task(FadeInCanvas(mainMenu.disclaimer_cg, 0.5f));

        yield return new WaitForSeconds(5);

        Task c = new Task(FadeOutCanvas(mainMenu.disclaimer_cg, 0.5f));
     
        yield return new WaitForSeconds(2f);

        //This loads a message under the title
        GameObject tip = GameObject.Find("Tip");

        if (tip != null)
        {
            Text messageText = tip.GetComponent<Text>();
            DisplayMessageOnTitleScreen(messageText);
        }

        //Fade title in and out
        Task d = new Task(FadeInCanvas(mainMenu.title_cg, 0.5f));

        yield return new WaitForSeconds(7);

        Task e = new Task(FadeOutCanvas(mainMenu.title_cg, 0.5f));

        yield return new WaitForSeconds(1f);

        //Fade in main menu
        Task f = new Task(FadeInCanvas(mainMenu.menu_cg, 0.5f));
    }

    //This loads the menu sans the disclaimer and title
    public static void RunMenuWithoutIntroduction()
    {
        //This sets the cursor settings
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //This loads the menu
        GameObject mainMenuGO = LoadMenuPrefab();
        MainMenu mainMenu = mainMenuGO.GetComponent<MainMenu>();

        //This gets the main references
        GameObject background_black = GameObject.Find("Background_Black");
        GameObject background_image = GameObject.Find("Background_Image");
        GameObject disclaimer = GameObject.Find("Disclaimer");
        GameObject title = GameObject.Find("Title");
        GameObject menu = GameObject.Find("Menu");
        GameObject loadingScreen = GameObject.Find("LoadingScreen");

        //This gets the canvas group function
        mainMenu.background_black_cg = background_black.GetComponent<CanvasGroup>();
        mainMenu.background_image_cg = background_image.GetComponent<CanvasGroup>();
        mainMenu.disclaimer_cg = disclaimer.GetComponent<CanvasGroup>();
        mainMenu.title_cg = title.GetComponent<CanvasGroup>();
        mainMenu.menu_cg = menu.GetComponent<CanvasGroup>();
        mainMenu.loadingScreen_cg = loadingScreen.GetComponent<CanvasGroup>();

        //This loads the background image
        Texture2D texture = LoadBackgroundImage();

        RawImage menuBackgroundImage = background_image.GetComponentInChildren<RawImage>();
        menuBackgroundImage.texture = texture;

        //This applies the art credit
        GameObject artCredit = GameObject.Find("ArtCredit");
        Text creditText = artCredit.GetComponent<Text>();
        string artworkName = texture.name;
        creditText.text = artworkName;

        mainMenu.background = texture;

        //This starts the menu background music
        PlayBackgroundMusic(true);

        //This updates the version number on the menu
        UpdateVersionOnMenu();

        //Fade in background image
        Task a = new Task(FadeInCanvas(mainMenu.background_image_cg, 0.5f));

        //Fade in main menu
        Task f = new Task(FadeInCanvas(mainMenu.menu_cg, 0.5f));
    }

    //This Loads the main menu
    public static GameObject LoadMenuPrefab()
    {
        GameObject menuPrefab = Resources.Load(OGGetAddress.menus + "MainMenu") as GameObject;
        GameObject menu = GameObject.Instantiate(menuPrefab);
        return menu;
    }

    //This makes the menu automaticaly display the correct version
    public static void UpdateVersionOnMenu()
    {
        GameObject VersionInfo = GameObject.Find("VersionInfo");

        if (VersionInfo != null)
        {
            Text text = VersionInfo.GetComponent<Text>();
            text.text = "Open Galaxy " + Application.version;
        }
    }

    //This loads the background images
    public static Texture2D LoadBackgroundImage()
    {
        Texture2D texture = null;

        MainMenu mainMenu = GetMainMenu();

        Object[] backgroundImages = Resources.LoadAll("artwork/", typeof(Texture2D));

        foreach (Object backgroundImage in backgroundImages)
        {
            mainMenu.backgroundPictures.Add((Texture2D)backgroundImage);
        }

        int numberOfPictures = mainMenu.backgroundPictures.Count;

        int pictureSelection = Random.Range(0, numberOfPictures);

        texture = backgroundImages[pictureSelection] as Texture2D;

        return texture;
    }

    #endregion

    #region menu loading functions

    //This runs all the other menu loading functions
    public static void LoadMenu()
    {
        //This function grabs all the button prefabs ready to instantiate
        LoadButtons();

        //This gets the reference to the main menu (NOTE: if you have more than one main menu you will need to change this line as this line will just grab the first menu reference it can find)
        MainMenu mainMenu = GameObject.FindFirstObjectByType<MainMenu>();

        //This creates the function dictionary
        CreateFunctionDictionary(mainMenu);

        //This initiates all lists used by the main menu
        InitiateLists(mainMenu);

        //This loads and sorts the campaign data from internal and then external sources
        LoadInternalCampaignData(mainMenu);
        LoadExternalCampaignData(mainMenu);

        //This creates sub menus from both the menu data and the campaign data
        CreateSubMenus(mainMenu);

        //This creates the buttons from the menu data
        CreateSideMenuButtons(mainMenu);
        CreateSubMenuButtons(mainMenu);

        //This creates buttons from the campaign data
        CreateCampaignMenuButtons(mainMenu);
        CreateCampaignSubMenuButtons(mainMenu);

        //This creates buttons for the model credit data
        CreateModelCreditButtons(mainMenu);

        //This deactivates all menus and activates only the start menu
        ActivateStartGameMenu();
    }

    //This grabs all the button prefabs ready to instantiate
    public static void LoadButtons()
    {
        MainMenu mainMenu = GameObject.FindFirstObjectByType<MainMenu>();

        mainMenu.buttons = Resources.LoadAll<GameObject>(OGGetAddress.menubuttons);
    }

    //This initiates all the lists used by the  main menu
    public static void InitiateLists(MainMenu mainMenu)
    {
        if (mainMenu.campaigns == null)
        {
            mainMenu.campaigns = new List<string>();
        }

        if (mainMenu.campaignDescriptions == null)
        {
            mainMenu.campaignDescriptions = new List<string>();
        }

        if (mainMenu.mainMissionCampaigns == null)
        {
            mainMenu.mainMissionCampaigns = new List<string>();
        }

        if (mainMenu.mainMissionNames == null)
        {
            mainMenu.mainMissionNames = new List<string>();
        }

        if (mainMenu.customMissionCampaigns == null)
        {
            mainMenu.customMissionCampaigns = new List<string>();
        }

        if (mainMenu.customMissionNames == null)
        {
            mainMenu.customMissionNames = new List<string>();
        }

    }

    //This loads all the interal campaign data
    public static void LoadInternalCampaignData(MainMenu mainMenu)
    {
        //This loads the mission data
        Object[] mainMissions = Resources.LoadAll(OGGetAddress.missions_internal, typeof(TextAsset));

        //This gets all the main campaigns
        if (mainMissions.Length > 0)
        {
            foreach (Object mission in mainMissions)
            {
                TextAsset missionString = (TextAsset)mission;

                Mission tempMission = JsonUtility.FromJson<Mission>(missionString.text);

                string campaignName = "none";

                foreach (MissionEvent missionEvent in tempMission.missionEventData)
                {
                    if (missionEvent.eventType == "campaigninformation")
                    {
                        bool isPresent = false;

                        campaignName = missionEvent.data1;

                        foreach (string campaign in mainMenu.campaigns)
                        {
                            if (campaign == missionEvent.data1)
                            {
                                isPresent = true;
                            }
                        }

                        if (isPresent == false)
                        {
                            mainMenu.campaigns.Add(missionEvent.data1);
                            mainMenu.campaignDescriptions.Add(missionEvent.data2);
                        }
                    }
                }

                mainMenu.mainMissionNames.Add(mission.name);
                mainMenu.mainMissionCampaigns.Add(campaignName);
            }
        }
    }

    //This loads all the external campaign data
    public static void LoadExternalCampaignData(MainMenu mainMenu)
    {
        //This gets the external folder
        var info = new DirectoryInfo(OGGetAddress.missions_custom);

        //This loads the mission data
        if (info.Exists == false)
        {
            Directory.CreateDirectory(OGGetAddress.missions_custom);
            info = new DirectoryInfo(OGGetAddress.missions_custom);
        }

        List<TextAsset> customMissionsList = new List<TextAsset>();

        if (info.Exists == true)
        {
            var fileInfo = info.GetFiles("*.json");

            if (fileInfo.Length > 0)
            {
                foreach (FileInfo file in fileInfo)
                {
                    string path = OGGetAddress.missions_custom + file.Name;
                    string missionDataString = File.ReadAllText(path);
                    TextAsset missionDataTextAsset = new TextAsset(missionDataString);
                    missionDataTextAsset.name = System.IO.Path.GetFileNameWithoutExtension(path);
                    customMissionsList.Add(missionDataTextAsset);
                }
            }
        }

        Object[] customMissions = customMissionsList.ToArray();

        if (customMissions.Length > 0)
        {
            foreach (Object mission in customMissions)
            {
                TextAsset missionString = (TextAsset)mission;

                Mission tempMission = JsonUtility.FromJson<Mission>(missionString.text);

                string campaignName = "none";

                bool campaignNodeExists = false;

                foreach (MissionEvent missionEvent in tempMission.missionEventData)
                {
                    if (missionEvent.eventType == "campaigninformation")
                    {
                        bool campaignExists = false;

                        campaignName = missionEvent.data1;

                        foreach (string campaign in mainMenu.campaigns)
                        {
                            if (campaign == missionEvent.data1)
                            {
                                campaignExists = true;
                            }
                        }

                        if (campaignExists == false)
                        {
                            mainMenu.campaigns.Add(missionEvent.data1);
                            mainMenu.campaignDescriptions.Add(missionEvent.data2);
                        }

                        campaignNodeExists = true;

                        break;
                    }
                }

                if (campaignNodeExists == true)
                {
                    mainMenu.customMissionNames.Add(mission.name);
                    mainMenu.customMissionCampaigns.Add(campaignName);
                }
               
                //This creates a special folder for missions that aren't part of a larger campaign
                if (campaignNodeExists == false)
                {
                    bool campaignExists = false;

                    foreach (string campaign in mainMenu.campaigns)
                    {
                        if (campaign == "Misc")
                        {
                            campaignExists = true;
                        }
                    }

                    if (campaignExists == false)
                    {
                        mainMenu.campaigns.Add("Misc");
                        mainMenu.campaignDescriptions.Add("Single missions of different types.");
                    }

                    mainMenu.customMissionNames.Add(mission.name);
                    mainMenu.customMissionCampaigns.Add("Misc");
                }

               
            }
        }
        else
        {
            Debug.LogWarning("Open Galaxy found no external files or was unable to load them.");
        }
    }

    //This creates a dictionary of the all the functions the menu can call
    public static void CreateFunctionDictionary(MainMenu mainMenu)
    {
        mainMenu.functions = new Dictionary<string, System.Delegate>();

        //Add your functions here
        mainMenu.functions.Add("ActivateSubMenu", new System.Action<string>(ActivateSubMenu));
        mainMenu.functions.Add("ChangeDefaultCameraPosition", new System.Action<string>(ChangeDefaultCameraPosition));
        mainMenu.functions.Add("ChangeResolution", new System.Action<string>(ChangeResolution));
        mainMenu.functions.Add("ChangeDamageLevel", new System.Action<string>(ChangeDamageLevel));
        mainMenu.functions.Add("ChangeSensitivity", new System.Action<string>(ChangeSensitivity));
        mainMenu.functions.Add("ChangeQuality", new System.Action<string>(ChangeQuality));
        mainMenu.functions.Add("InvertHorizontal", new System.Action<string>(InvertHorizontal));
        mainMenu.functions.Add("InvertVertical", new System.Action<string>(InvertVertical));
        mainMenu.functions.Add("LoadMainMission", new System.Action<string>(LoadMission));
        mainMenu.functions.Add("LoadCustomMission", new System.Action<string>(LoadCustomMission));
        mainMenu.functions.Add("LoadMissionEditor", new System.Action(LoadEditor));
        mainMenu.functions.Add("OpenCustomMissionDirectory", new System.Action(OpenCustomMissionDirectory));
        mainMenu.functions.Add("OpenWebAddressAndQuit", new System.Action<string>(OpenWebAddressAndQuit));
        mainMenu.functions.Add("SetAutoaim", new System.Action<string>(SetAutoaim));
        mainMenu.functions.Add("SetWindowMode", new System.Action<string>(SetWindowMode));
        mainMenu.functions.Add("SetEditorWindowMode", new System.Action<string>(SetEditorWindowMode));
        mainMenu.functions.Add("ToggleDebugging", new System.Action<string>(ToggleDebugging));
        mainMenu.functions.Add("QuitToDesktop", new System.Action(QuitToDesktop));
    }

    //This creates all the sub menus of the menu
    public static void CreateSubMenus(MainMenu mainMenu)
    {
        //This loads all the information for the menu from the Json file
        TextAsset menuItemsFile = Resources.Load(OGGetAddress.files + "Menu") as TextAsset;
        MenuItems menuItems = JsonUtility.FromJson<MenuItems>(menuItemsFile.text);

        //This creates the menu lists ready to use
        List<string> subMenus = new List<string>();
        mainMenu.SubMenus = new List<GameObject>();

        //This creates a list of all the sub menus from the menu data
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

        //This manually creates the blank start game menu
        subMenus.Add("Start Game");
        subMenus.Add("Model Credits");

        //This creates a list of submenus from the campaign data
        foreach (string campaign in mainMenu.campaigns)
        {
            subMenus.Add(campaign);
        }

        //This creates all the sub menus
        foreach (string tempMenu in subMenus)
        {
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
        }
    }

    //This creates all theb side menus from menu data
    public static void CreateSideMenuButtons(MainMenu mainMenu)
    {
        //This loads all the information for the menu from the Json file
        TextAsset menuItemsFile = Resources.Load(OGGetAddress.files + "Menu") as TextAsset;
        MenuItems menuItems = JsonUtility.FromJson<MenuItems>(menuItemsFile.text);

        //This adds the menu buttons to the left bar
        float buttonDrop = 20;

        foreach (MenuItem menuItem in menuItems.menuData)
        {
            if (menuItem.ParentMenu == "none")
            {
                buttonDrop = CreateParentMenuButton(mainMenu, buttonDrop, menuItem.Name, menuItem.Type, menuItem.Function, menuItem.Variable);
            }
        }
    }

    //This creates all the sub menus from menu data
    public static void CreateSubMenuButtons(MainMenu mainMenu)
    {
        //This loads all the information for the menu from the Json file
        TextAsset menuItemsFile = Resources.Load(OGGetAddress.files + "Menu") as TextAsset;
        MenuItems menuItems = JsonUtility.FromJson<MenuItems>(menuItemsFile.text);

        //This adds the actual menu content
        foreach (GameObject subMenu in mainMenu.SubMenus)
        {
            float buttonDrop = 20;

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
    }

    //This creates all the campaign menu buttons from campaign data
    public static void CreateCampaignMenuButtons(MainMenu mainMenu)
    {
        foreach (GameObject subMenu in mainMenu.SubMenus)
        {
            float buttonDrop = 20;

            if ("Start Game_Settings" == subMenu.name)
            {
                foreach (string campaign in mainMenu.campaigns)
                {
                    buttonDrop = CreateCampaignButton(mainMenu, subMenu, buttonDrop, campaign, "ContentButton02", "ActivateSubMenu", "", campaign);
                }

                //This sets the size of the sub menu according to the how many buttons have been generated
                RectTransform rt = subMenu.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(0, buttonDrop);
            }
        }
    }

    //This creates all the mission menu buttons from campaign data
    public static void CreateCampaignSubMenuButtons(MainMenu mainMenu)
    {
        int i2 = 0;

        foreach (string campaign in mainMenu.campaigns)
        {
            List<string> missions = new List<string>();
            List<string> functions = new List<string>();

            for (int i = 0; i < mainMenu.mainMissionCampaigns.Count; i++)
            {
                if (mainMenu.mainMissionCampaigns[i] == campaign)
                {
                    missions.Add(mainMenu.mainMissionNames[i]);
                    functions.Add("LoadMainMission");
                }
            }

            for (int i = 0; i < mainMenu.customMissionCampaigns.Count; i++)
            {
                if (mainMenu.customMissionCampaigns[i] == campaign)
                {
                    missions.Add(mainMenu.customMissionNames[i]);
                    functions.Add("LoadCustomMission");
                }
            }

            CreateMissionButtons(mainMenu, missions.ToArray(), campaign + "_Settings", functions.ToArray(), campaign, mainMenu.campaignDescriptions[i2]);
            i2++;
        }
    }

    //This creates all the mission menu buttons from campaign data
    public static void CreateModelCreditButtons(MainMenu mainMenu)
    {
        TextAsset shipTypesFile = Resources.Load(OGGetAddress.files + "ShipTypes") as TextAsset;
        ShipTypes shipTypes = JsonUtility.FromJson<ShipTypes>(shipTypesFile.text);

        UnityEngine.Object[] props = Resources.LoadAll(OGGetAddress.props, typeof(GameObject));

        foreach (GameObject subMenu in mainMenu.SubMenus)
        {
            if (subMenu.name == "Model Credits_Settings")
            {
                float buttonDrop = 20;

                buttonDrop = CreateSubMenuButton(mainMenu, subMenu, buttonDrop, "Back to Main Credits", "ContentButton02", "ActivateSubMenu", "", "Credits");

                foreach (ShipType shipType in shipTypes.shipTypeData)
                {
                    string prefabName = shipType.type;
                    string modelAuthor = shipType.modelauthor;
                    string textureAuthor = shipType.textureauthor;

                    buttonDrop = CreateSubMenuButton(mainMenu, subMenu, buttonDrop, prefabName, "ContentButton03", "none", "Model Author: " + modelAuthor + " Texture Author: " + textureAuthor, "none"); 
                }

                foreach (Object prop in props)
                {
                    string prefabName = prop.name;
                    string modelAuthor = "Galactic Conquest Mod";
                    string textureAuthor = "Galactic Conquest Mod";

                    buttonDrop = CreateSubMenuButton(mainMenu, subMenu, buttonDrop, prefabName, "ContentButton03", "none", "Model Author: " + modelAuthor + " Texture Author: " + textureAuthor, "none");
                }

                //This sets the size of the sub menu according to the how many buttons have been generated
                RectTransform rt = subMenu.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(0, buttonDrop);
            }
        }
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

            buttonDrop = buttonDrop + button.GetComponent<ButtonInfo>().buttonShiftDown;

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
            buttonDrop = buttonDrop + button.GetComponent<ButtonInfo>().buttonShiftDown;
        }
        else
        {
            buttonDrop = 60;
        }

        return buttonDrop;
    }

    //This creates a sub menu button
    public static float CreateCampaignButton(MainMenu mainMenu, GameObject subMenu, float buttonDrop, string buttonName, string buttonType, string functionName, string buttonDescription = "", string variable = "none")
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
            buttonDrop = buttonDrop + button.GetComponent<ButtonInfo>().buttonShiftDown;
        }
        else
        {
            buttonDrop = 60;
        }

        return buttonDrop;
    }

    //This adds a button for each mission avaible in the folder
    public static void CreateMissionButtons(MainMenu mainMenu, string[] missions, string subMenuName, string[] functions, string campaignName, string campaignDescription)
    {
        foreach (GameObject subMenu in mainMenu.SubMenus)
        {
            if (subMenu.name == subMenuName)
            {
                float buttonDrop = 20;

                buttonDrop = CreateSubMenuButton(mainMenu, subMenu, buttonDrop, campaignName, "GameButton02", "ActivateSubMenu", campaignDescription, "Start Game");

                int i = 0;

                foreach (string mission in missions)
                {
                    buttonDrop = CreateSubMenuButton(mainMenu, subMenu, buttonDrop, mission, "ContentButton02", functions[i], "", mission);
                    i++;
                }

                buttonDrop = CreateSubMenuButton(mainMenu, subMenu, buttonDrop, "Back to Start Game", "ContentButton02", "ActivateSubMenu", "", "Start Game");

                //This sets the size of the sub menu according to the how many buttons have been generated
                RectTransform rt = subMenu.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(0, buttonDrop);
            }
        }
    }

    #endregion

    #region navigation functions

    //This activates the start game menu which displays all the games campaigns
    public static void ActivateStartGameMenu()
    {
        MainMenu mainMenu = GetMainMenu();

        //This turns off all the sub menus
        foreach (GameObject subMenu in mainMenu.SubMenus)
        {
            subMenu.SetActive(false);
        }

        //This loads the first menu created
        ActivateSubMenu("Start Game");
    }

    //This activate a sub menu
    public static void ActivateSubMenu(string menuName)
    {
        MainMenu mainMenu = GetMainMenu();

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
        MainMenu mainMenu = GameObject.FindFirstObjectByType<MainMenu>();

        //This loads all the information for the menu from the Json file
        TextAsset menuItemsFile = Resources.Load(OGGetAddress.files + "Menu") as TextAsset;
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
                    ActivateSubMenu("Start Game");
                }
            }
        }
    }

    #endregion

    #region menu functions

    //Displays a message on the title screen
    public static void DisplayMessageOnTitleScreen(Text titleScreenMessageBox)
    {
        string[] messages = new string[12];
        messages[0] = "Welcome to Open Galaxy. Version " + Application.version;
        messages[1] = "Open Galaxy's is a platform for X-Wing and Tie Fighter style custom missions.";
        messages[2] = "All Open Galaxy's missions were created using the inbuilt mission editor.";
        messages[3] = "Flying a ship isn't like dusting crops. Familiarise yourself with the controls first.";
        messages[4] = "Open Galaxy gives complete control to mission creators from events to AI behaviour to ship stats.";
        messages[5] = "Every ship in Open Galaxy handles a little differently. Adapt your tactics accordingly.";
        messages[6] = "Open Galaxy supports controllers and keyboard and mouse.";
        messages[7] = "Check out the credits to see who made Open Galaxy possible.";
        messages[8] = "Open Galaxy is in active development, so if you find a bug, report it.";
        messages[9] = "Open Galaxy has ships from all the major star wars timelines and periods.";
        messages[10] = "Finding it hard to hit your target? Try turning on autoaim in the controls menu.";
        messages[11] = "Game to challenging? Try changing the damage level in the settings menu";
        Random.InitState(System.DateTime.Now.Millisecond);
        int randomMessageNo = Random.Range(0, 11);
        titleScreenMessageBox.text = messages[randomMessageNo];
    }

    //This loads a custom mission
    public static void LoadCustomMission(string name)
    {
        MainMenu mainMenu = GetMainMenu();

        if (mainMenu != null)
        {
            if (mainMenu.missionRunning == false)
            {
                mainMenu.missionRunning = true; //This prevents multiple missions running

                Task a = new Task(MissionFunctions.RunMission(name, OGGetAddress.missions_custom, true));

                //Fade out menu
                Task b = new Task(FadeOutCanvas(mainMenu.menu_cg, 0.01f));

                //Fade in loading screen
                Task c = new Task(FadeInCanvas(mainMenu.loadingScreen_cg, 0.01f));
            }
        }
    }

    //This loads a main mission
    public static void LoadMission(string name)
    {
        MainMenu mainMenu = GetMainMenu();

        if (mainMenu != null)
        {
            if (mainMenu.missionRunning == false)
            {
                mainMenu.missionRunning = true; //This prevents multiple missions running

                Task a = new Task(MissionFunctions.RunMission(name, OGGetAddress.missions_internal));

                //Fade out menu
                Task b = new Task(FadeOutCanvas(mainMenu.menu_cg, 0.01f));

                //Fade in loading screen
                Task c = new Task(FadeInCanvas(mainMenu.loadingScreen_cg, 0.01f));
            }
        }   
    }

    //This loads the mission editor
    public static void LoadEditor()
    {
        GameObject missionEditor = GameObject.Find("editor");
        
        if (missionEditor == null)
        {
            GameObject tempMissionEditor = Resources.Load(OGGetAddress.missioneditor + "missioneditor") as GameObject;
            missionEditor = GameObject.Instantiate(tempMissionEditor);
            missionEditor.name = "missioneditor";
        }

        missionEditor.SetActive(true);

        PlayBackgroundMusic(false);

        MainMenu mainMenu = GetMainMenu();

        Task a = new Task(FadeOutMenuAndDestroyMainMenu(mainMenu, 0.25f));
    }

    //This sets the screen resolution
    public static void ChangeDefaultCameraPosition(string cameraPosition)
    {
        OutputMenuMessage("The default camera was set to " + cameraPosition);

        OGSettingsFunctions.SetCameraPosition(cameraPosition);

        ActivateSubMenu("Settings");
    }

    //This sets the screen resolution
    public static void ChangeResolution(string resolution)
    {
        OGSettings settings = OGSettingsFunctions.GetSettings();

        FullScreenMode screenMode = Screen.fullScreenMode;

        OutputMenuMessage("The resolution was set to " + resolution);

        if (resolution == "Detect Screen Resolution")
        {
            int width = Display.main.systemWidth;
            int height = Display.main.systemHeight;
            Screen.SetResolution(width, height, screenMode);
            settings.screenResX = width;
            settings.screenResY = height;
        }
        else if (resolution == "My Aspect Ratio - 640 Resolution")
        {
            int width = 640;
            int height = (int)Mathf.Abs((640f/ Display.main.systemWidth) * Display.main.systemHeight);
            Screen.SetResolution(width, height, screenMode);
            settings.screenResX = width;
            settings.screenResY = height;
        }
        else if (resolution == "My Aspect Ratio - 1024 Resolution")
        {
            int width = 1024;
            int height = (int)Mathf.Abs((1024f / Display.main.systemWidth) * Display.main.systemHeight);
            Screen.SetResolution(width, height, screenMode);
            settings.screenResX = width;
            settings.screenResY = height;
        }
        else if (resolution == "My Aspect Ratio - 1280 Resolution")
        {
            int width = 1280;
            int height = (int)Mathf.Abs((1280f / Display.main.systemWidth) * Display.main.systemHeight);
            Screen.SetResolution(width, height, screenMode);
            settings.screenResX = width;
            settings.screenResY = height;
        }
        else if (resolution == "My Aspect Ratio - 1680 Resolution")
        {
            int width = 1680;
            int height = (int)Mathf.Abs((1680f / Display.main.systemWidth) * Display.main.systemHeight);
            Screen.SetResolution(width, height, screenMode);
            settings.screenResX = width;
            settings.screenResY = height;
        }
        else if (resolution == "My Aspect Ratio - 1920 Resolution")
        {
            int width = 1920;
            int height = (int)Mathf.Abs((1920f / Display.main.systemWidth) * Display.main.systemHeight);
            Screen.SetResolution(width, height, screenMode);
            settings.screenResX = width;
            settings.screenResY = height;
        }
        else if (resolution == "My Aspect Ratio - 2560 Resolution")
        {
            int width = 2560;
            int height = (int)Mathf.Abs((2560f / Display.main.systemWidth) * Display.main.systemHeight);
            Screen.SetResolution(width, height, screenMode);
            settings.screenResX = width;
            settings.screenResY = height;
        }
        else if (resolution == "My Aspect Ratio - 3840 Resolution")
        {
            int width = 3840;
            int height = (int)Mathf.Abs((3840f / Display.main.systemWidth) * Display.main.systemHeight);
            Screen.SetResolution(width, height, screenMode);
            settings.screenResX = width;
            settings.screenResY = height;
        }
        else if (resolution == "My Aspect Ratio - 3840 Resolution")
        {
            int width = 3840;
            int height = (int)Mathf.Abs((3840f / Display.main.systemWidth) * Display.main.systemHeight);
            Screen.SetResolution(width, height, screenMode);
            settings.screenResX = width;
            settings.screenResY = height;
        }
        else if (resolution == "My Aspect Ratio - 7680 Resolution")
        {
            int width = 7680;
            int height = (int)Mathf.Abs((7680f / Display.main.systemWidth) * Display.main.systemHeight);
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
        else if (resolution == "800 x 600 (4:3)")
        {
            Screen.SetResolution(800, 600, screenMode);
            settings.screenResX = 800;
            settings.screenResY = 600;
        }
        else if (resolution == "1024 x 768 (4:3)")
        {
            Screen.SetResolution(1024, 768, screenMode);
            settings.screenResX = 1024;
            settings.screenResY = 768;
        }
        else if (resolution == "1220 x 915 (4:3)")
        {
            Screen.SetResolution(1220, 915, screenMode);
            settings.screenResX = 1220;
            settings.screenResY = 915;
        }
        else if (resolution == "1680 × 1260 (4:3)")
        {
            Screen.SetResolution(1680, 1260, screenMode);
            settings.screenResX = 1680;
            settings.screenResY = 1260;
        }
        else if (resolution == "1920 x 1440 (4:3)")
        {
            Screen.SetResolution(1920, 1440, screenMode);
            settings.screenResX = 1920;
            settings.screenResY = 1440;
        }
        else if (resolution == "2560 x 1920 (4:3)")
        {
            Screen.SetResolution(2560, 1920, screenMode);
            settings.screenResX = 2560;
            settings.screenResY = 1920;
        }
        else if (resolution == "3840 x 2880 (4:3)")
        {
            Screen.SetResolution(3840, 2880, screenMode);
            settings.screenResX = 3840;
            settings.screenResY = 2880;
        }
        else if (resolution == "7680 x 5760 (4:3)")
        {
            Screen.SetResolution(7680, 5760, screenMode);
            settings.screenResX = 7680;
            settings.screenResY = 5760;
        }
        else if (resolution == "640 x 360 (16:9)")
        {
            Screen.SetResolution(640, 360, screenMode);
            settings.screenResX = 640;
            settings.screenResY = 360;
        }
        else if (resolution == "848 x 450 (16:9)")
        {
            Screen.SetResolution(848, 450, screenMode);
            settings.screenResX = 848;
            settings.screenResY = 450;
        }
        else if (resolution == "1024 x 576 (16:9)")
        {
            Screen.SetResolution(1024, 576, screenMode);
            settings.screenResX = 1024;
            settings.screenResY = 576;
        }
        else if (resolution == "1220 x 720 (16:9)")
        {
            Screen.SetResolution(1220, 720, screenMode);
            settings.screenResX = 1220;
            settings.screenResY = 620;
        }
        else if (resolution == "1680 × 945 (16:9)")
        {
            Screen.SetResolution(1680, 945, screenMode);
            settings.screenResX = 1680;
            settings.screenResY = 945;
        }
        else if (resolution == "1920 x 1080 (16:9)")
        {
            Screen.SetResolution(1920, 1080, screenMode);
            settings.screenResX = 1920;
            settings.screenResY = 1080;
        }
        else if (resolution == "2560 x 1440 (16:9)")
        {
            Screen.SetResolution(2560, 1440, screenMode);
            settings.screenResX = 2560;
            settings.screenResY = 1440;
        }
        else if (resolution == "3840 x 2160 (16:9)")
        {
            Screen.SetResolution(3840, 2160, screenMode);
            settings.screenResX = 3840;
            settings.screenResY = 2160;
        }
        else if (resolution == "7680 x 4320 (16:9)")
        {
            Screen.SetResolution(7680, 4320, screenMode);
            settings.screenResX = 7680;
            settings.screenResY = 4320;
        }
        else if (resolution == "640 x 400 (16:10)")
        {
            Screen.SetResolution(640, 400, screenMode);
            settings.screenResX = 640;
            settings.screenResY = 400;
        }
        else if (resolution == "960 x 600 (16:10)")
        {
            Screen.SetResolution(960, 600, screenMode);
            settings.screenResX = 960;
            settings.screenResY = 600;
        }
        else if (resolution == "1024 x 640 (16:10)")
        {
            Screen.SetResolution(1024, 640, screenMode);
            settings.screenResX = 1024;
            settings.screenResY = 640;
        }
        else if (resolution == "1280 x 800 (16:10)")
        {
            Screen.SetResolution(1280, 800, screenMode);
            settings.screenResX = 1280;
            settings.screenResY = 800;
        }
        else if (resolution == "1680 × 1050 (16:10)")
        {
            Screen.SetResolution(1680, 1050, screenMode);
            settings.screenResX = 1680;
            settings.screenResY = 1050;
        }
        else if (resolution == "1920 x 1200 (16:10)")
        {
            Screen.SetResolution(1920, 1200, screenMode);
            settings.screenResX = 1920;
            settings.screenResY = 1200;
        }
        else if (resolution == "2560 x 1600 (16:10)")
        {
            Screen.SetResolution(2560, 1600, screenMode);
            settings.screenResX = 2560;
            settings.screenResY = 1600;
        }
        else if (resolution == "3840 x 2400 (16:10)")
        {
            Screen.SetResolution(3840, 2400, screenMode);
            settings.screenResX = 3840;
            settings.screenResY = 2400;
        }
        else if (resolution == "7680 x 4800 (16:10)")
        {
            Screen.SetResolution(7680, 4800, screenMode);
            settings.screenResX = 7680;
            settings.screenResY = 4800;
        }
        else if (resolution == "1280 x 540 (21:9)")
        {
            int width = 1280;
            int height = 540;
            Screen.SetResolution(width, height, screenMode);
            settings.screenResX = width;
            settings.screenResY = height;
        }
        else if (resolution == "2560 x 1080 (21:9)")
        {
            int width = 2560;
            int height = 1080;
            Screen.SetResolution(width, height, screenMode);
            settings.screenResX = width;
            settings.screenResY = height;
        }
        else if (resolution == "2880 x 1200 (21:9)")
        {
            int width = 2800;
            int height = 1200;
            Screen.SetResolution(width, height, screenMode);
            settings.screenResX = width;
            settings.screenResY = height;
        }
        else if (resolution == "3440 x 1440 (21:9)")
        {
            int width = 3440;
            int height = 1440;
            Screen.SetResolution(width, height, screenMode);
            settings.screenResX = width;
            settings.screenResY = height;
        }
        else if (resolution == "3840 x 1600 (21:9)")
        {
            int width = 3840;
            int height = 1600;
            Screen.SetResolution(width, height, screenMode);
            settings.screenResX = width;
            settings.screenResY = height;
        }
        else if (resolution == "4320 x 1800 (21:9)")
        {
            int width = 4320;
            int height = 1800;
            Screen.SetResolution(width, height, screenMode);
            settings.screenResX = width;
            settings.screenResY = height;
        }
        else if (resolution == "5120 x 2160 (21:9)")
        {
            int width = 5120;
            int height = 2160;
            Screen.SetResolution(width, height, screenMode);
            settings.screenResX = width;
            settings.screenResY = height;
        }
        else if (resolution == "1920 x 540 (32:9)")
        {
            int width = 1920;
            int height = 540;
            Screen.SetResolution(width, height, screenMode);
            settings.screenResX = width;
            settings.screenResY = height;
        }
        else if (resolution == "3840 x 1080 (32:9)")
        {
            int width = 3840;
            int height = 1080;
            Screen.SetResolution(width, height, screenMode);
            settings.screenResX = width;
            settings.screenResY = height;
        }
        else if (resolution == "5120 x 1440 (32:9)")
        {
            int width = 5120;
            int height = 1440;
            Screen.SetResolution(width, height, screenMode);
            settings.screenResX = width;
            settings.screenResY = height;
        }
        else if (resolution == "7680 x 2160 (32:9)")
        {
            int width = 7680;
            int height = 2160;
            Screen.SetResolution(width, height, screenMode);
            settings.screenResX = width;
            settings.screenResY = height;
        }

        OGSettingsFunctions.SaveSettingsData();

        ActivateSubMenu("Settings");
    }

    //This sets the screen resolution
    public static void ChangeDamageLevel(string level)
    {
        OGSettings settings = OGSettingsFunctions.GetSettings();

        OutputMenuMessage("The damage level was set to " + level);

        OGSettingsFunctions.SetDamageLevel(level);

        ActivateSubMenu("Settings");
    }
    
    //This sets the controller senstivity
    public static void ChangeSensitivity(string sensitivity)
    {
        float sensitvityFloat = float.Parse(sensitivity);
        sensitvityFloat = sensitvityFloat / 100f;
        OGSettingsFunctions.SetControllerSensitivity(sensitvityFloat);

        OutputMenuMessage("The controller senstivity was set to " + sensitivity + "%");

        ActivateSubMenu("Controls");
    }

    //This sets the screen resolution
    public static void ChangeQuality(string qualityLevel)
    {
        OGSettings settings = OGSettingsFunctions.GetSettings();

        OutputMenuMessage("The quality was set to " + qualityLevel);

        OGSettingsFunctions.SetQualityLevel(qualityLevel);

        ActivateSubMenu("Settings");
    }

    //This opens the custom mission directory
    public static void OpenCustomMissionDirectory()
    {
        string address = Application.persistentDataPath + "/Custom Missions/";
        address = address.Replace(@"/", @"\"); //Because explorer doesn't like forward leaning slashes

        System.Diagnostics.Process.Start("explorer.exe", "/select," + address);
    }

    //This sets the autoaim preference
    public static void SetAutoaim(string autoaim)
    {
        if (autoaim == "false")
        {
            OGSettingsFunctions.SetAutoaim(false);
        }
        else
        {
            OGSettingsFunctions.SetAutoaim(true);
        }

        ActivateSubMenu("Controls");

        OutputMenuMessage("Autoaim was set to " + autoaim);
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
    public static void OpenWebAddressAndQuit(string url)
    {
        Application.OpenURL(url);


        Application.Quit();
    }

    //This outputs a message to the menu
    public static void OutputMenuMessage(string message)
    {
        MainMenu mainMenu = GameObject.FindFirstObjectByType<MainMenu>();

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
            if (canvasGroup != null)
            {
                alpha1 = alpha1 + (1f / (60f * duration));
                canvasGroup.alpha = alpha1;
                yield return new WaitForSecondsRealtime(0.016f);
            }
            else
            {
                break;
            }
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
            if (canvasGroup != null)
            {
                alpha2 = alpha2 - (1f / (60f * duration));
                canvasGroup.alpha = alpha2;
                yield return new WaitForSecondsRealtime(0.016f);
            }
            else
            {
                break;
            }

            
        }
    }

    //This coroutine can be used to fade the canvas group out
    public static IEnumerator FadeOutMenuAndDestroyMainMenu(MainMenu mainMenu, float duration)
    {
        //This sets the starting alpha value to 1
        float alpha = 1;

        //This fades the canvas out
        while (alpha > 0)
        {
            alpha = alpha - (1f / (60f * duration));
            mainMenu.background_black_cg.alpha = alpha;
            mainMenu.background_image_cg.alpha = alpha;
            mainMenu.menu_cg.alpha = alpha;
            yield return new WaitForSecondsRealtime(0.016f);
        }

        GameObject.Destroy(mainMenu.gameObject);
    }

    //This coroutine can be used to fade the canvas group out
    public static IEnumerator FadeOutLoadingScreenAndDestroyMainMenu(MainMenu mainMenu, float duration)
    {
        //This sets the starting alpha value to 1
        float alpha = 1;

        //This fades the canvas out
        while (alpha > 0)
        {
            alpha = alpha - (1f / (60f * duration));
            mainMenu.background_black_cg.alpha = alpha;
            mainMenu.background_image_cg.alpha = alpha;
            mainMenu.loadingScreen_cg.alpha = alpha;
            yield return new WaitForSecondsRealtime(0.016f);
        }

        GameObject.Destroy(mainMenu.gameObject);
    }

    //This returns the main menu
    public static MainMenu GetMainMenu()
    {
        MainMenu mainMenu = GameObject.FindFirstObjectByType<MainMenu>(FindObjectsInactive.Include);

        if (mainMenu== null)
        {
            Debug.Log("Main Menu is null");
        }

        return mainMenu;
    }

    #endregion

}
