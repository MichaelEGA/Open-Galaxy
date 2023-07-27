using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public static class MainMenuFunctions
{
    #region disclaimer, title screen, and menu loading sequence

    //This displays the disclaimer, the title, then loads the menu
    public static IEnumerator RunMenu()
    {
        GameObject background = LoadBackground();
        GameObject disclaimer = LoadDisclaimer();
        GameObject title = LoadTitle();
        GameObject menu = LoadMenu();

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
        title.name = "Title";
        return title;
    }

    //This loads the disclaimer
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
        mainMenu.functions.Add("LoadMainMission", new System.Action<string>(LoadMainMission));
        mainMenu.functions.Add("LoadTrainingMission", new System.Action<string>(LoadTrainingMission));
        mainMenu.functions.Add("LoadOtherGameModes", new System.Action<string>(LoadOtherGameModes));
        mainMenu.functions.Add("LoadCustomMission", new System.Action<string>(LoadCustomMission));
        mainMenu.functions.Add("QuitToDesktop", new System.Action(QuitToDesktop));
        mainMenu.functions.Add("QuitToMainMenu", new System.Action(QuitToMainMenu));
        mainMenu.functions.Add("SetWindowMode", new System.Action<string>(SetWindowMode));
        mainMenu.functions.Add("ChangeResolution", new System.Action<string>(ChangeResolution));
        mainMenu.functions.Add("ChangePlanetTextureResolution", new System.Action<string>(ChangeResolution));
        mainMenu.functions.Add("ActivateSubMenu", new System.Action<string>(ActivateSubMenu));
        mainMenu.functions.Add("InvertHorizontal", new System.Action<string>(InvertHorizontal));
        mainMenu.functions.Add("InvertVertical", new System.Action<string>(InvertVertical));
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
        subMenus.Add("Training");
        subMenus.Add("Custom Missions");
        subMenus.Add("OtherGameModes");

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

        //This adds all the missions buttons (An Open Galaxy Function)
        Object[] mainMissions = Resources.LoadAll("Data/Files/Missions_Main", typeof(TextAsset));
        CreateMissionButtons(mainMenu, mainMissions, "Missions_Settings", "LoadMainMission");
        Object[] trainingMissions = Resources.LoadAll("Data/Files/Missions_Training", typeof(TextAsset));
        CreateMissionButtons(mainMenu, trainingMissions, "Training_Settings", "LoadTrainingMission");
        //Object[] customMissions = Resources.LoadAll("Data/Files/Mission", typeof(TextAsset));
        //CreateMissionButtons(mainMenu, customMissions, "Custom Missions_Settings", "LoadCustomMission");
        Object[] OtherGameModes = Resources.LoadAll("Data/Files/Missions_Misc", typeof(TextAsset));
        CreateMissionButtons(mainMenu, OtherGameModes, "OtherGameModes_Settings", "LoadOtherGameModes");

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
            if (subMenu.name.Contains(subMenuName))
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
        TextAsset menuItemsFile = Resources.Load("Menufiles/MainMenu") as TextAsset;
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

    //This loads a main mission
    public static void LoadMainMission(string name)
    {
        Task a = new Task(MissionFunctions.RunMission(name, "Data/Files/Missions_Main/"));

        GameObject menu = GameObject.Find("Menu");

        if (menu != null)
        {
            CanvasGroup canvasGroup = menu.GetComponent<CanvasGroup>();
            Task i = new Task(FadeOutAndDeactivate(canvasGroup, 0.25f));
        }
    }

    //This loads a training missino
    public static void LoadTrainingMission(string name)
    {
        Task a = new Task(MissionFunctions.RunMission(name, "Data/Files/Missions_Training/"));

        GameObject menu = GameObject.Find("Menu");

        if (menu != null)
        {
            CanvasGroup canvasGroup = menu.GetComponent<CanvasGroup>();
            Task i = new Task(FadeOutAndDeactivate(canvasGroup, 0.25f));
        }
    }

    //This loads a custom mission
    public static void LoadCustomMission(string name)
    {
        Task a = new Task(MissionFunctions.RunMission(name, "Data/Files/Missions_Training/")); //This is a dummy address until external loading of missions is added

        GameObject menu = GameObject.Find("Menu");

        if (menu != null)
        {
            CanvasGroup canvasGroup = menu.GetComponent<CanvasGroup>();
            Task i = new Task(FadeOutAndDeactivate(canvasGroup, 0.25f));
        }
    }

    //Stand in function for starting the game
    public static void LoadOtherGameModes(string name)
    {
        Task a = new Task(MissionFunctions.RunMission(name, "Data/Files/Missions_Misc/"));

        GameObject menu = GameObject.Find("Menu");

        if (menu != null)
        {
            CanvasGroup canvasGroup = menu.GetComponent<CanvasGroup>();
            Task i = new Task(FadeOutAndDeactivate(canvasGroup, 0.25f));
        }
    }

    //This sets the screen resolution
    public static void ChangeResolution(string resolution)
    {

        FullScreenMode screenMode = Screen.fullScreenMode;

        if (resolution == "Detect Screen Resolution")
        {
            int width = Display.main.systemWidth;
            int height = Display.main.systemHeight;
            Screen.SetResolution(width, height, screenMode);
        }
        else if (resolution == "640 x 480 (4:3)")
        {
            Screen.SetResolution(640, 480, screenMode);
        }
        else if (resolution == "640 x 360 (16:9)")
        {
            Screen.SetResolution(640, 360, screenMode);
        }
        else if (resolution == "640 x 400 (16:10)")
        {
            Screen.SetResolution(640, 400, screenMode);
        }
        else if (resolution == "800 x 600 (4:3)")
        {
            Screen.SetResolution(800, 600, screenMode);
        }
        else if (resolution == "848 x 450 (16:9)")
        {
            Screen.SetResolution(848, 450, screenMode);
        }
        else if (resolution == "960 x 600 (16:10)")
        {
            Screen.SetResolution(960, 600, screenMode);
        }
        else if (resolution == "1024 x 768 (4:3)")
        {
            Screen.SetResolution(1024, 768, screenMode);
        }
        else if (resolution == "1024 x 576 (16:9)")
        {
            Screen.SetResolution(1024, 576, screenMode);
        }
        else if (resolution == "1024 x 640 (16:10)")
        {
            Screen.SetResolution(1024, 640, screenMode);
        }
        else if (resolution == "1220 x 915 (4:3)")
        {
            Screen.SetResolution(1220, 915, screenMode);
        }
        else if (resolution == "1220 x 720 (16:9)")
        {
            Screen.SetResolution(1220, 720, screenMode);
        }
        else if (resolution == "1280 x 800 (16:10)")
        {
            Screen.SetResolution(1280, 800, screenMode);
        }
        else if (resolution == "1680 × 1260 (4:3)")
        {
            Screen.SetResolution(1680, 1260, screenMode);
        }
        else if (resolution == "1680 × 945 (16:9)")
        {
            Screen.SetResolution(1680, 945, screenMode);
        }
        else if (resolution == "1680 × 1050 (16:10)")
        {
            Screen.SetResolution(1680, 1050, screenMode);
        }
        else if (resolution == "1920 x 1440 (4:3)")
        {
            Screen.SetResolution(1920, 1440, screenMode);
        }
        else if (resolution == "1920 x 1080 (16:9)")
        {
            Screen.SetResolution(1920, 1080, screenMode);
        }
        else if (resolution == "1920 x 1200 (16:10)")
        {
            Screen.SetResolution(1920, 1200, screenMode);
        }
        else if (resolution == "2560 x 1920 (4:3)")
        {
            Screen.SetResolution(2560, 1920, screenMode);
        }
        else if (resolution == "2560 x 1440 (16:9)")
        {
            Screen.SetResolution(2560, 1440, screenMode);
        }
        else if (resolution == "2560 x 1600 (16:10)")
        {
            Screen.SetResolution(2560, 1600, screenMode);
        }
        else if (resolution == "3840 x 2880 (4:3)")
        {
            Screen.SetResolution(3840, 2880, screenMode);
        }
        else if (resolution == "3840 x 2160 (16:9)")
        {
            Screen.SetResolution(3840, 2160, screenMode);
        }
        else if (resolution == "3840 x 2400 (16:10)")
        {
            Screen.SetResolution(3840, 2400, screenMode);
        }
        else if (resolution == "7680 x 5760 (4:3)")
        {
            Screen.SetResolution(7680, 5760, screenMode);
        }
        else if (resolution == "7680 x 4320 (16:9)")
        {
            Screen.SetResolution(7680, 4320, screenMode);
        }
        else if (resolution == "7680 x 4800 (16:10)")
        {
            Screen.SetResolution(7680, 4800, screenMode);
        }

        ActivateSubMenu("Video");

    }

    //This sets the window mode
    public static void SetWindowMode(string windowMode)
    {

        int widthRes = Screen.currentResolution.width;
        int heightRes = Screen.currentResolution.height;

        if (windowMode == "Windowed")
        {
            Screen.SetResolution(widthRes, heightRes, FullScreenMode.Windowed);
        }
        else if (windowMode == "MaximizedWindow")
        {
            Screen.SetResolution(widthRes, heightRes, FullScreenMode.MaximizedWindow);
        }
        else if (windowMode == "FullScreenWindow")
        {
            Screen.SetResolution(widthRes, heightRes, FullScreenMode.FullScreenWindow);
        }
        else if (windowMode == "ExclusiveFullScreen")
        {
            Screen.SetResolution(widthRes, heightRes, FullScreenMode.ExclusiveFullScreen);
        }

        ActivateSubMenu("Video");

    }

    //Stand in function for inverting the horizontal view
    public static void InvertHorizontal(string choice)
    {

        if (choice == "true")
        {
            //Your code to invert the horizontal
            Debug.Log("Horizontal Inverted");
        }
        else
        {
            //Your code to set the horizontal to normal
            Debug.Log("Horizontal Normal");
        }

        ActivateSubMenu("Controls");

    }

    //Stand in function for inverting the vertical view
    public static void InvertVertical(string choice)
    {
        if (choice == "true")
        {
            //Your code to invert the horizontal
            Debug.Log("Vertical Inverted");
        }
        else
        {
            //Your code to set the horizontal to normal
            Debug.Log("Vertical Normal");
        }

        ActivateSubMenu("Controls");

    }

    //Stand in funciton for returning to the main menu
    public static void QuitToMainMenu()
    {
        //Close game and return to main menu

        Debug.Log("Return to Main Menu");

    }

    //This quits the application and return you to the desk. NOTE: doesn't work in editor (obviously) only build
    public static void QuitToDesktop()
    {
        Debug.Log("Quitting the Game - NOTE: Only works in build");

        Application.Quit();
    }

    #endregion

    #region menu utils

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
