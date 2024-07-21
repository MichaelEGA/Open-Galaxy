using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.IO;

public static class MissionEditorFunctions
{

    #region draw editor

    public static void Draw_MissionEditor(MissionEditor missionEditor)
    {
        Draw_MenuBar(missionEditor);
        Draw_InfoBar(missionEditor);
        Draw_ScaleIndicator(missionEditor);
        Draw_MesssageTextBox(missionEditor);
        Draw_CurrentFileTextBox(missionEditor);
        Draw_MainMenu(missionEditor);
        OGSettingsFunctions.SetDefaultCursor();
    }

    public static void Draw_ScaleIndicator(MissionEditor missionEditor)
    {
        //This draws the input label
        GameObject scaleIndGO = new GameObject();
        scaleIndGO.name = "Scale Indicator";

        scaleIndGO.transform.SetParent(missionEditor.transform);
        RectTransform rectTransform = scaleIndGO.AddComponent<RectTransform>();
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.pivot = new Vector2(0, 0);
        rectTransform.anchoredPosition = new Vector2(10, 0);
        rectTransform.sizeDelta = new Vector2(90, 12f);
        rectTransform.localScale = new Vector3(1, 1, 1);

        Text text = scaleIndGO.AddComponent<Text>();
        text.supportRichText = false;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.text = "100%";
        text.fontSize = 7;
        text.color = Color.white;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.alignment = TextAnchor.MiddleLeft;

        missionEditor.scaleIndicator = text;
    }

    public static void Draw_MesssageTextBox(MissionEditor missionEditor)
    {
        //This draws the input label
        GameObject messageTextboxGO = new GameObject();
        messageTextboxGO.name = "Action Indicator";

        messageTextboxGO.transform.SetParent(missionEditor.transform);
        RectTransform rectTransform = messageTextboxGO.AddComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(1, 0);
        rectTransform.pivot = new Vector2(0.5f, 0);
        rectTransform.anchoredPosition = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(0, 12f);
        rectTransform.localScale = new Vector3(1, 1, 1);

        Text text = messageTextboxGO.AddComponent<Text>();
        text.supportRichText = false;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.text = "Mission Editor Loaded";
        text.fontSize = 7;
        text.color = Color.white;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.alignment = TextAnchor.MiddleCenter;

        missionEditor.messageTextbox = text;
    }

    public static void Draw_CurrentFileTextBox(MissionEditor missionEditor)
    {
        //This draws the input label
        GameObject currentMissionTextboxGO = new GameObject();
        currentMissionTextboxGO.name = "Mission Name";

        currentMissionTextboxGO.transform.SetParent(missionEditor.transform);
        RectTransform rectTransform = currentMissionTextboxGO.AddComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 1);
        rectTransform.anchorMax = new Vector2(0.5f, 1);
        rectTransform.pivot = new Vector2(0.5f, 1);
        rectTransform.anchoredPosition = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(200f, 12f);
        rectTransform.localScale = new Vector3(1, 1, 1);

        Text text = currentMissionTextboxGO.AddComponent<Text>();
        text.supportRichText = false;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.text = "Untitled Mission";
        text.fontSize = 7;
        text.color = Color.black;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.alignment = TextAnchor.MiddleCenter;

        missionEditor.missionName = text;
    }

    public static void Draw_MenuBar(MissionEditor missionEditor)
    {
        //This draws the input label
        GameObject menuBarGO = new GameObject();
        menuBarGO.name = "Menu Bar";

        menuBarGO.transform.SetParent(missionEditor.transform);
        RectTransform rectTransform = menuBarGO.AddComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.pivot = new Vector2(0.5f, 1);
        rectTransform.anchoredPosition = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(0, 12f);
        rectTransform.localScale = new Vector3(1, 1, 1);

        missionEditor.menuBarRectTransform = rectTransform;

        Image image = menuBarGO.AddComponent<Image>();

        Color color = Color.red;

        if (ColorUtility.TryParseHtmlString("#FFFFFF", out color))
        {
            image.color = color;
        }
    }

    public static void Draw_InfoBar(MissionEditor missionEditor)
    {
        //This creates the button bar
        GameObject infoBarGO = new GameObject();
        infoBarGO.name = "InfoBar";

        infoBarGO.transform.SetParent(missionEditor.transform);
        RectTransform rectTransform = infoBarGO.AddComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(1, 0);
        rectTransform.pivot = new Vector2(0.5f, 0);
        rectTransform.anchoredPosition = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(0, 12f);
        rectTransform.localScale = new Vector3(1, 1, 1);

        Image image = infoBarGO.AddComponent<Image>();

        Color color = Color.black;

        if (ColorUtility.TryParseHtmlString("#404040", out color))
        {
            image.color = color;
        }
    }

    public static void Draw_MainMenu(MissionEditor missionEditor)
    {
        if (missionEditor.menuBarRectTransform != null)
        {
            float shiftRight = 0;
            DrawTextButton(missionEditor.menuBarRectTransform.transform, shiftRight, 0, 12, 25, "File", 7, "ActivateMenu", TextAnchor.MiddleCenter);
            shiftRight += 25;
            DrawTextButton(missionEditor.menuBarRectTransform.transform, shiftRight, 0, 12, 25, "Edit", 7, "ActivateMenu", TextAnchor.MiddleCenter);
            shiftRight += 25;
            DrawTextButton(missionEditor.menuBarRectTransform.transform, shiftRight, 0, 12, 30, "Events", 7, "ActivateMenu", TextAnchor.MiddleCenter);
            shiftRight += 30;
            DrawTextButton(missionEditor.menuBarRectTransform.transform, shiftRight, 0, 12, 30, "Window", 7, "ActivateMenu", TextAnchor.MiddleCenter);
            shiftRight += 30;
            DrawTextButton(missionEditor.menuBarRectTransform.transform, shiftRight, 0, 12, 25, "Help", 7, "ActivateMenu", TextAnchor.MiddleCenter);
        }

        float shiftRight02 = 0;
        string spaces = "        ";

        List<string> file_Buttons = new List<string>();
        file_Buttons.Add(spaces + "New");
        file_Buttons.Add(spaces + "Open");
        file_Buttons.Add(spaces + "Merge");
        file_Buttons.Add(spaces + "Save");
        file_Buttons.Add(spaces + "Save As");
        file_Buttons.Add(spaces + "Export Selection As");
        file_Buttons.Add(spaces + "Exit to Open Galaxy");
        file_Buttons.Add(spaces + "Exit to Windows");

        List<string> file_Functions = new List<string>();
        file_Functions.Add("OpenNewWindow");
        file_Functions.Add("OpenOpenWindow");
        file_Functions.Add("OpenMergeWindow");
        file_Functions.Add("Save");
        file_Functions.Add("OpenSaveAsWindow");
        file_Functions.Add("OpenExportSelectionAsWindow");
        file_Functions.Add("ExitMissionEditor");
        file_Functions.Add("ExitToWindows");

        List<string> file_Shortcuts = new List<string>();
        file_Shortcuts.Add("");
        file_Shortcuts.Add("");
        file_Shortcuts.Add("");
        file_Shortcuts.Add("");
        file_Shortcuts.Add("");
        file_Shortcuts.Add("");
        file_Shortcuts.Add("");
        file_Shortcuts.Add("");

        GameObject fileMenu = DrawDropDownMenu(missionEditor.transform, "File", shiftRight02, file_Buttons.ToArray(), file_Functions.ToArray(), file_Shortcuts.ToArray());

        List<string> edit_Buttons = new List<string>();
        edit_Buttons.Add(spaces + "Cut");
        edit_Buttons.Add(spaces + "Copy");
        edit_Buttons.Add(spaces + "Paste");
        edit_Buttons.Add(spaces + "Select All");
        edit_Buttons.Add(spaces + "Select None");
        edit_Buttons.Add(spaces + "Delete");

        List<string> edit_Functions = new List<string>();
        edit_Functions.Add("Cut");
        edit_Functions.Add("Copy");
        edit_Functions.Add("Paste");
        edit_Functions.Add("SelectAll");
        edit_Functions.Add("SelectNone");
        edit_Functions.Add("Delete");

        List<string> edit_Shortcuts = new List<string>();
        edit_Shortcuts.Add("Ctrl+X");
        edit_Shortcuts.Add("Ctrl+C");
        edit_Shortcuts.Add("Ctrl+V");
        edit_Shortcuts.Add("");
        edit_Shortcuts.Add("");
        edit_Shortcuts.Add("Del");

        shiftRight02 += 25;

        GameObject EditMenu = DrawDropDownMenu(missionEditor.transform, "Edit", shiftRight02, edit_Buttons.ToArray(), edit_Functions.ToArray(), edit_Shortcuts.ToArray());

        List<string> event_Buttons = new List<string>();
        event_Buttons.Add(spaces + "Add New Event");
        event_Buttons.Add(spaces + "Display Location");

        List<string> event_Functions = new List<string>();
        event_Functions.Add("OpenAddNewEvent");
        event_Functions.Add("OpenDisplayLocation");

        List<string> event_Shortcuts = new List<string>();
        event_Shortcuts.Add("");
        event_Shortcuts.Add("");

        shiftRight02 += 25;

        GameObject EventsMenu = DrawDropDownMenu(missionEditor.transform, "Events", shiftRight02, event_Buttons.ToArray(), event_Functions.ToArray(), event_Shortcuts.ToArray());

        List<string> window_Buttons = new List<string>();
        window_Buttons.Add(spaces + "Make Fullscreen");
        window_Buttons.Add(spaces + "Make Windowed");

        List<string> window_Functions = new List<string>();
        window_Functions.Add("MakeFullscreen");
        window_Functions.Add("MakeWindowed");

        List<string> window_Shortcuts = new List<string>();
        window_Shortcuts.Add("");
        window_Shortcuts.Add("");

        shiftRight02 += 30;

        GameObject windowMenu = DrawDropDownMenu(missionEditor.transform, "Window", shiftRight02, window_Buttons.ToArray(), window_Functions.ToArray(), window_Shortcuts.ToArray());

        List<string> help_Buttons = new List<string>();
        help_Buttons.Add(spaces + "Open Open-Galaxy Wiki");
        help_Buttons.Add(spaces + "Open Open-Galaxy Github");
        help_Buttons.Add(spaces + "About OG Mission Editor");

        List<string> help_Functions = new List<string>();
        help_Functions.Add("OpenWiki");
        help_Functions.Add("OpenGitHub");
        help_Functions.Add("OpenAbout");

        List<string> help_Shortcuts = new List<string>();
        help_Shortcuts.Add("");
        help_Shortcuts.Add("");
        help_Shortcuts.Add("");

        shiftRight02 += 30;

        GameObject helpMenu = DrawDropDownMenu(missionEditor.transform, "Help", shiftRight02, help_Buttons.ToArray(), help_Functions.ToArray(), help_Shortcuts.ToArray());

        if (missionEditor.menus == null)
        {
            missionEditor.menus = new List<GameObject>();
        }
        else
        {
            missionEditor.menus.Add(fileMenu);
            missionEditor.menus.Add(EditMenu);
            missionEditor.menus.Add(EventsMenu);
            missionEditor.menus.Add(windowMenu);
            missionEditor.menus.Add(helpMenu);

            foreach (GameObject menu in missionEditor.menus)
            {
                menu.SetActive(false);
            }
        }
    }

    public static GameObject DrawDropDownMenu(Transform parent, string name, float xPosition, string[] buttons, string[] functions, string[] shortcuts)
    {
        GameObject menuBaseGO = new GameObject();

        menuBaseGO.name = name;

        menuBaseGO.transform.SetParent(parent);

        //This sets up the node background
        RectTransform rectTransform = menuBaseGO.AddComponent<RectTransform>();

        float height = buttons.Length * 12;
        float drop = 0;
        int buttonNo = 0;

        Image background = menuBaseGO.AddComponent<Image>();
        background.sprite = Resources.Load<Sprite>(OGGetAddress.missioneditor + "NodeSprite_Light");
        background.type = Image.Type.Sliced;
        background.pixelsPerUnitMultiplier = 40;
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.pivot = new Vector2(0, 1);
        rectTransform.anchoredPosition = new Vector2(xPosition, -12);
        rectTransform.sizeDelta = new Vector2(150, height);
        rectTransform.localScale = new Vector3(1, 1, 1);

        foreach (string button in buttons)
        {
            DrawTwoSidedTextButton(rectTransform.transform, 2, drop, 12, 146, button, shortcuts[buttonNo], 7, functions[buttonNo]);
            drop -= 12;
            buttonNo += 1;
        }

        return menuBaseGO;
    }

    public static void DrawTextButton(Transform parent, float xPos, float yPos, float height, float width, string buttonText, int fontSize, string functionType, TextAnchor alignement = TextAnchor.MiddleCenter)
    {
        GameObject buttonGO = new GameObject();
        GameObject buttonTextGO = new GameObject();

        buttonGO.name = "button_" + functionType;
        buttonTextGO.name = "ButtonText_" + functionType;

        buttonGO.transform.SetParent(parent);
        buttonTextGO.transform.SetParent(buttonGO.transform);

        RectTransform rectTransform1 = buttonGO.AddComponent<RectTransform>();
        rectTransform1.anchorMax = new Vector2(0, 1);
        rectTransform1.anchorMin = new Vector2(0, 1);
        rectTransform1.pivot = new Vector2(0, 1);
        rectTransform1.anchoredPosition = new Vector2(xPos, yPos);
        rectTransform1.sizeDelta = new Vector2(width, height);
        rectTransform1.localScale = new Vector3(1, 1, 1);

        RectTransform rectTransform2 = buttonTextGO.AddComponent<RectTransform>();
        rectTransform2.anchorMax = new Vector2(0, 1);
        rectTransform2.anchorMin = new Vector2(0, 1);
        rectTransform2.pivot = new Vector2(0, 1);
        rectTransform2.anchoredPosition = new Vector2(0, 0);
        rectTransform2.sizeDelta = new Vector2(width, height);
        rectTransform2.localScale = new Vector3(1, 1, 1);

        Text text = buttonTextGO.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = fontSize;
        text.text = buttonText;
        text.alignment = alignement;
        text.color = Color.black;

        Image buttonImage = buttonGO.AddComponent<Image>();
        buttonImage.color = Color.white;

        Button button = buttonGO.AddComponent<Button>();
        button.targetGraphic = buttonImage;

        Color color = Color.white;

        MissionEditor missionEditor = GetMissionEditor();

        if (ColorUtility.TryParseHtmlString("#C3C3C3", out color))
        {
            ColorBlock colorVar = button.colors;
            colorVar.highlightedColor = color;
        }

        if (functionType == "ActivateMenu")
        {
            button.onClick.AddListener(() => { ActivateMenu(buttonText); });
        }
        else if (functionType == "Copy")
        {
            button.onClick.AddListener(() => { Copy(); });
        }
        else if (functionType == "Cut")
        {
            button.onClick.AddListener(() => { Cut(); });
        }
        else if (functionType == "Delete")
        {
            button.onClick.AddListener(() => { DeleteNodes(); });
        }
        else if (functionType == "ExitMissionEditor")
        {
            button.onClick.AddListener(() => { ExitMissionEditor(); });
        }
        else if (functionType == "ExitToWindows")
        {
            button.onClick.AddListener(() => { ExitToWindows(); });
        }
        else if (functionType == "OpenSaveAsWindow")
        {
            button.onClick.AddListener(() => { OpenWindow("savemissionas"); });
        }
        else if (functionType == "OpenExportSelectionAsWindow")
        {
            button.onClick.AddListener(() => { OpenWindow("exportselectionas"); });
        }
        else if (functionType == "OpenOpenWindow")
        {
            button.onClick.AddListener(() => { OpenWindow("loadmission"); });
        }
        else if (functionType == "OpenMergeWindow")
        {
            button.onClick.AddListener(() => { OpenWindow("mergemissions"); });
        }
        else if (functionType == "OpenNewWindow")
        {
            button.onClick.AddListener(() => { NewMission(); });
        }
        else if (functionType == "OpenAddNewEvent")
        {
            button.onClick.AddListener(() => { OpenWindow("addnodes"); });
        }
        else if (functionType == "OpenDisplayLocation")
        {
            button.onClick.AddListener(() => { OpenWindow("displaylocation"); });
        }
        else if (functionType == "MakeFullscreen")
        {
            button.onClick.AddListener(() => { SetWindowMode("fullscreen"); });
        }
        else if (functionType == "MakeWindowed")
        {
            button.onClick.AddListener(() => { SetWindowMode("window"); });
        }
        else if (functionType == "OpenWiki")
        {
            button.onClick.AddListener(() => { OpenWebAddress("https://github.com/MichaelEGA/Open-Galaxy/wiki"); });
        }
        else if (functionType == "OpenGitHub")
        {
            button.onClick.AddListener(() => { OpenWebAddress("https://github.com/MichaelEGA/Open-Galaxy"); });
        }
        else if (functionType == "OpenAbout")
        {
            button.onClick.AddListener(() => { OpenWindow("abouteditor"); });
        }
        else if (functionType == "Paste")
        {
            button.onClick.AddListener(() => { Paste(missionEditor); });
        }
        else if (functionType == "Save")
        {
            button.onClick.AddListener(() => { SaveMission(); });
        }
        else if (functionType == "SelectAll")
        {
            button.onClick.AddListener(() => { SelectAll(missionEditor); });
        }
        else if (functionType == "SelectNone")
        {
            button.onClick.AddListener(() => { SelectNone(missionEditor); });
        }
    }

    public static void DrawTwoSidedTextButton(Transform parent, float xPos, float yPos, float height, float width, string buttonText, string buttonText2, int fontSize, string functionType)
    {
        GameObject buttonGO = new GameObject();
        GameObject buttonTextGO = new GameObject();
        GameObject buttonTextGO2 = new GameObject();

        buttonGO.name = "button_" + functionType;
        buttonTextGO.name = "ButtonText_" + functionType;

        buttonGO.transform.SetParent(parent);
        buttonTextGO.transform.SetParent(buttonGO.transform);
        buttonTextGO2.transform.SetParent(buttonGO.transform);

        RectTransform rectTransform1 = buttonGO.AddComponent<RectTransform>();
        rectTransform1.anchorMax = new Vector2(0, 1);
        rectTransform1.anchorMin = new Vector2(0, 1);
        rectTransform1.pivot = new Vector2(0, 1);
        rectTransform1.anchoredPosition = new Vector2(xPos, yPos);
        rectTransform1.sizeDelta = new Vector2(width, height);
        rectTransform1.localScale = new Vector3(1, 1, 1);

        RectTransform rectTransform2 = buttonTextGO.AddComponent<RectTransform>();
        rectTransform2.anchorMax = new Vector2(0, 1);
        rectTransform2.anchorMin = new Vector2(0, 1);
        rectTransform2.pivot = new Vector2(0, 1);
        rectTransform2.anchoredPosition = new Vector2(0, 0);
        rectTransform2.sizeDelta = new Vector2(width, height);
        rectTransform2.localScale = new Vector3(1, 1, 1);

        RectTransform rectTransform3 = buttonTextGO2.AddComponent<RectTransform>();
        rectTransform3.anchorMax = new Vector2(1, 1);
        rectTransform3.anchorMin = new Vector2(1, 1);
        rectTransform3.pivot = new Vector2(1, 1);
        rectTransform3.anchoredPosition = new Vector2(0, 0);
        rectTransform3.sizeDelta = new Vector2(width, height);
        rectTransform3.localScale = new Vector3(1, 1, 1);

        Text text = buttonTextGO.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = fontSize;
        text.text = buttonText;
        text.alignment = TextAnchor.MiddleLeft;
        text.color = Color.black;

        Text text2 = buttonTextGO2.AddComponent<Text>();
        text2.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text2.fontSize = fontSize;
        text2.text = buttonText2 + "     ";
        text2.alignment = TextAnchor.MiddleRight;
        text2.color = Color.black;

        Image buttonImage = buttonGO.AddComponent<Image>();
        buttonImage.color = Color.white;

        Button button = buttonGO.AddComponent<Button>();
        button.targetGraphic = buttonImage;

        Color color = Color.white;

        MissionEditor missionEditor = GetMissionEditor();

        if (ColorUtility.TryParseHtmlString("#C3C3C3", out color))
        {
            ColorBlock colorVar = button.colors;
            colorVar.highlightedColor = color;
        }

        if (functionType == "ActivateMenu")
        {
            button.onClick.AddListener(() => { ActivateMenu(buttonText); });
        }
        else if (functionType == "Copy")
        {
            button.onClick.AddListener(() => { Copy(); });
        }
        else if (functionType == "Cut")
        {
            button.onClick.AddListener(() => { Cut(); });
        }
        else if (functionType == "Delete")
        {
            button.onClick.AddListener(() => { DeleteNodes(); });
        }
        else if (functionType == "ExitMissionEditor")
        {
            button.onClick.AddListener(() => { ExitMissionEditor(); });
        }
        else if (functionType == "ExitToWindows")
        {
            button.onClick.AddListener(() => { ExitToWindows(); });
        }
        else if (functionType == "OpenSaveAsWindow")
        {
            button.onClick.AddListener(() => { OpenWindow("savemissionas"); });
        }
        else if (functionType == "OpenExportSelectionAsWindow")
        {
            button.onClick.AddListener(() => { OpenWindow("exportselectionas"); });
        }
        else if (functionType == "OpenOpenWindow")
        {
            button.onClick.AddListener(() => { OpenWindow("loadmission"); });
        }
        else if (functionType == "OpenMergeWindow")
        {
            button.onClick.AddListener(() => { OpenWindow("mergemissions"); });
        }
        else if (functionType == "OpenNewWindow")
        {
            button.onClick.AddListener(() => { NewMission(); });
        }
        else if (functionType == "OpenAddNewEvent")
        {
            button.onClick.AddListener(() => { OpenWindow("addnodes"); });
        }
        else if (functionType == "OpenDisplayLocation")
        {
            button.onClick.AddListener(() => { OpenWindow("displaylocation"); });
        }
        else if (functionType == "MakeFullscreen")
        {
            button.onClick.AddListener(() => { SetWindowMode("fullscreen"); });
        }
        else if (functionType == "MakeWindowed")
        {
            button.onClick.AddListener(() => { SetWindowMode("window"); });
        }
        else if (functionType == "OpenWiki")
        {
            button.onClick.AddListener(() => { OpenWebAddress("https://github.com/MichaelEGA/Open-Galaxy/wiki"); });
        }
        else if (functionType == "OpenGitHub")
        {
            button.onClick.AddListener(() => { OpenWebAddress("https://github.com/MichaelEGA/Open-Galaxy"); });
        }
        else if (functionType == "OpenAbout")
        {
            button.onClick.AddListener(() => { OpenWindow("abouteditor"); });
        }
        else if (functionType == "Paste")
        {
            button.onClick.AddListener(() => { Paste(missionEditor); });
        }
        else if (functionType == "Save")
        {
            button.onClick.AddListener(() => { SaveMission(); });
        }
        else if (functionType == "SelectAll")
        {
            button.onClick.AddListener(() => { SelectAll(missionEditor); });
        }
        else if (functionType == "SelectNone")
        {
            button.onClick.AddListener(() => { SelectNone(missionEditor); });
        }
    }

    //This modifies the menu and info bar position to ensure they remain on top
    public static void ModifyBarPosition()
    {
        GameObject menuBar = GameObject.Find("Menu Bar");
        GameObject infoBar = GameObject.Find("InfoBar");
        GameObject actionIndicator = GameObject.Find("Action Indicator");
        GameObject missionName = GameObject.Find("Mission Name");
        GameObject scaleInd = GameObject.Find("Scale Indicator");

        if (menuBar != null)
        {
            menuBar.transform.SetAsLastSibling();
            infoBar.transform.SetAsLastSibling();
            actionIndicator.transform.SetAsLastSibling();
            missionName.transform.SetAsLastSibling();
            scaleInd.transform.SetAsLastSibling();
        }
    }

    #endregion

    #region menus functions

    public static void ActivateMenu(string menuName)
    {
        MissionEditor missionEditor = GetMissionEditor();

        if (missionEditor.menus != null)
        {
            foreach (GameObject menu in missionEditor.menus)
            {
                if (menu != null)
                {
                    if (menu.name == menuName)
                    {
                        menu.SetActive(true);
                        menu.transform.SetAsLastSibling();
                        missionEditor.menusClosed = false;
                    }
                    else
                    {
                        menu.SetActive(false);
                    }
                }
            }
        }
    }

    public static void CloseAllMenus()
    {
        MissionEditor missionEditor = GetMissionEditor();

        if (missionEditor.menus != null)
        {
            foreach (GameObject menu in missionEditor.menus)
            {
                if (menu != null)
                {
                    menu.SetActive(false);
                    missionEditor.menusClosed = true;
                }
            }
        }
    }

    #endregion

    #region windows functions

    public static void OpenWindow(string windowName)
    {
        MissionEditor missionEditor = GetMissionEditor();

        if (missionEditor.windows == null)
        {
            missionEditor.windows = new List<Window>();
        }

        bool open = true;

        foreach(Window tempWindow in missionEditor.windows)
        {
            if (tempWindow != null)
            {
                if (tempWindow.windowType == windowName)
                {
                    open = false;
                    break;
                }
            }
        }

        if (open == true)
        {
            GameObject windowGO = new GameObject();
            windowGO.transform.SetParent(missionEditor.gameObject.transform);
            Window window = windowGO.AddComponent<Window>();
            window.windowType = windowName;
            missionEditor.windows.Add(window);
        }

        ModifyBarPosition(); //This ensures the top and bottom bars of the program stay above the windows
    }

    #endregion

    #region edit tools

    //This deletes all currently selected nodes
    public static void Shortcuts(MissionEditor missionEditor)
    {
        var keyboard = Keyboard.current;
        float delay = 0.1f + missionEditor.timePressed;

        if (keyboard.deleteKey.isPressed == true & Time.time > delay)
        {
            bool inputFieldIsActive = CheckInputFields();

            if (inputFieldIsActive == false)
            {
                DeleteNodes();
                missionEditor.timePressed = Time.time;
            }   
        }

        if (keyboard.xKey.isPressed == true & keyboard.ctrlKey.isPressed == true & Time.time > delay)
        {
            Cut();
            missionEditor.timePressed = Time.time;
        }

        if (keyboard.cKey.isPressed == true & keyboard.ctrlKey.isPressed == true & Time.time > delay)
        {
            bool inputFieldIsActive = CheckInputFields();

            if (inputFieldIsActive == false)
            {
                Copy();
                missionEditor.timePressed = Time.time;
            }   
        }

        if (keyboard.vKey.isPressed == true & keyboard.ctrlKey.isPressed == true & Time.time > delay)
        {
            bool inputFieldIsActive = CheckInputFields();

            if (inputFieldIsActive == false)
            {
                Paste(missionEditor);
                missionEditor.timePressed = Time.time;
            }
        }
    }

    //This deletes all currently selected nodes
    public static void DeleteNodes()
    {
        NodeFunctions.DeleteSelectedNodes();
        CloseAllMenus();
    }

    //This creates a selection box
    public static void SelectionBox(MissionEditor missionEditor)
    {
        if (missionEditor.dragging == true)
        {
            //This gets the mouse down
            if (missionEditor.draggingGridStarted == false)
            {
                missionEditor.mouseStartPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                missionEditor.draggingGridStarted = true;
            }

            //This gets the original click position and the current mouse position
            Vector2 clickPosition = missionEditor.mouseStartPos;
            Vector2 currentPosition = Input.mousePosition;

            //This calculates the width and height of the selection box
            int width = (int)(currentPosition.x - clickPosition.x);
            int height = -(int)(currentPosition.y - clickPosition.y);
            Vector2 selectionBoxSize = new Vector2(width, height);

            //This allows the selection box reversed (i.e. able to be pulled in all directions)
            if (selectionBoxSize.x < 0)
            {
                selectionBoxSize.x = Mathf.Abs(selectionBoxSize.x);
                clickPosition.x = currentPosition.x;
            }

            if (selectionBoxSize.y < 0)
            {
                selectionBoxSize.y = Mathf.Abs(selectionBoxSize.y);
                clickPosition.y = currentPosition.y;
            }

            //This creates the selection box if it doesn't already exist
            if (missionEditor.selectionRectTransform == null)
            {
                missionEditor.selectionRectTransform = CreateRectTransformWithImage("SelectionBox", "NodeSprite_Selection", missionEditor.transform);
            }

            //This sets the selection box gameobject to active
            missionEditor.selectionRectTransform.gameObject.SetActive(true);

            //This applies the correct anchor position for the box
            missionEditor.selectionRectTransform.anchorMax = new Vector2(0, 1);
            missionEditor.selectionRectTransform.anchorMin = new Vector2(0, 1);
            missionEditor.selectionRectTransform.pivot = new Vector2(0, 1);

            //This applies the position and size of the selection box
            missionEditor.selectionRectTransform.sizeDelta = selectionBoxSize;
            missionEditor.selectionRectTransform.position = clickPosition;

            missionEditor.selectionHasRun = false;
        }
        else if (missionEditor.dragging == false & missionEditor.selectionHasRun == false)
        {
            GetNodesWithinBounds(missionEditor, missionEditor.selectionRectTransform);

            if (missionEditor.selectionRectTransform != null)
            {
                missionEditor.selectionRectTransform.gameObject.SetActive(false);
            }

            missionEditor.selectionHasRun = true;
        }
        else
        {
            if (missionEditor.selectionRectTransform != null)
            {
                missionEditor.selectionRectTransform.sizeDelta = new Vector2(0, 0);
            }

            missionEditor.draggingGridStarted = false;
        }
    }

    //This grabs all the nodes within the given bounds of a rect (i.e. the selection box)
    public static void GetNodesWithinBounds(MissionEditor missionEditor, RectTransform selectionRect)
    {
        if (missionEditor.nodes != null)
        {
            //This deselects all current nodes
            var keyboard = Keyboard.current;

            if (keyboard.ctrlKey.isPressed == false)
            {
                foreach (Node node in missionEditor.nodes)
                {
                    if (node != null)
                    {
                        node.selected = false;

                    }
                }
            }

            //This selects a new set of nodes
            foreach (Node node in missionEditor.nodes)
            {
                if (node != null)
                {
                    Vector2 position = node.transform.position;

                    bool withinBounds = RectTransformUtility.RectangleContainsScreenPoint(missionEditor.selectionRectTransform, position); //missionEditor.selectionRectTransform.rect.Contains(position); //IsPointInRT(position, missionEditor.selectionRectTransform, node);

                    if (withinBounds == true)
                    {
                        node.selected = true;
                    }
                }
            }
        }
    }

    //This selects the provided node and deselects all others
    public static void SelectOnlyThisNode(MissionEditor missionEditor, Node node)
    {
        foreach (Node tempNode in missionEditor.nodes)
        {
            tempNode.selected = false;
        }

        node.selected = true;
    }

    //This adds the node to the current selection
    public static void AddNodeToCurrentSelection(Node node)
    {
        node.selected = true;
    }

    //This selects all nodes
    public static void SelectAll(MissionEditor missionEditor)
    {
        foreach (Node node in missionEditor.nodes)
        {
            node.selected = true;
        }

        CloseAllMenus();
    }

    //This deselects all nodes
    public static void SelectNone(MissionEditor missionEditor)
    {
        foreach (Node node in missionEditor.nodes)
        {
            node.selected = false;
        }

        CloseAllMenus();
    }

    //This copies the selected nodes
    public static void Copy()
    {
        CopySelection();
        CloseAllMenus();
    }

    //This cuts the selected nodes
    public static void Cut()
    {
        CopySelection();
        DeleteNodes();
        CloseAllMenus();
    }

    //This pastes anything in the clipboard
    public static void Paste(MissionEditor missionEditor)
    {
        if (missionEditor.pasting == false)
        {
            Task a = new Task(PasteMissionData());
        }

        CloseAllMenus();
    }

    //This checks if there is an active input field
    public static bool CheckInputFields()
    {
        bool isFocused = false;

        InputField[] inputFields = GameObject.FindObjectsOfType<InputField>();

        foreach (InputField inputField in inputFields)
        {
            if (inputField.isFocused == true)
            {
                isFocused = true;
                break;
            }
        }

        return isFocused;
    }

    #endregion

    #region saving

    public static void SaveMission(Window window = null)
    {
        List<MissionEvent> missionList = new List<MissionEvent>();

        MissionEditor missionEditor = GetMissionEditor();

        string missionName = GetMissionNameFromSaveDialog();

        UpdateMissionName(missionName);

        if (missionName != "Untitled Mission")
        {
            foreach (Node node in missionEditor.nodes)
            {
                if (node != null)
                {

                    MissionEvent missionEvent = new MissionEvent();

                    missionEvent.eventID = ParseTextToString(node.eventID);
                    missionEvent.eventType = ParseTextToString(node.eventType);
                    missionEvent.conditionLocation = ParseTextToString(node.conditionLocation);
                    missionEvent.conditionTime = ParseTextToFloat(node.conditionTime);
                    missionEvent.x = ParseTextToFloat(node.x);
                    missionEvent.y = ParseTextToFloat(node.y);
                    missionEvent.z = ParseTextToFloat(node.z);
                    missionEvent.xRotation = ParseTextToFloat(node.xRotation);
                    missionEvent.yRotation = ParseTextToFloat(node.yRotation);
                    missionEvent.zRotation = ParseTextToFloat(node.zRotation);
                    missionEvent.data1 = ParseTextToString(node.data1);
                    missionEvent.data2 = ParseTextToString(node.data2);
                    missionEvent.data3 = ParseTextToString(node.data3);
                    missionEvent.data4 = ParseTextToString(node.data4);
                    missionEvent.data5 = ParseTextToString(node.data5);
                    missionEvent.data6 = ParseTextToString(node.data6);
                    missionEvent.data7 = ParseTextToString(node.data7);
                    missionEvent.data8 = ParseTextToString(node.data8);
                    missionEvent.data9 = ParseTextToString(node.data9);
                    missionEvent.data10 = ParseTextToString(node.data10);
                    missionEvent.data11 = ParseTextToString(node.data11);
                    missionEvent.data12 = ParseTextToString(node.data12);
                    missionEvent.data13 = ParseTextToString(node.data13);
                    missionEvent.data14 = ParseTextToString(node.data14);
                    missionEvent.data15 = ParseTextToString(node.data15);
                    missionEvent.nextEvent1 = ParseTextToString(node.nextEvent1);
                    missionEvent.nextEvent2 = ParseTextToString(node.nextEvent2);
                    missionEvent.nextEvent3 = ParseTextToString(node.nextEvent3);
                    missionEvent.nextEvent4 = ParseTextToString(node.nextEvent4);
                    missionEvent.nodePosX = node.nodePosX;
                    missionEvent.nodePosY = node.nodePosY;

                    missionList.Add(missionEvent);
                }
            }

            MissionEvent[] missionEventData = missionList.ToArray();

            string jsonString = JsonHelper.ToJson(missionEventData, true);

            string saveFile = "none";

            if (missionEditor.missionName.text.Contains(".json"))
            {
                saveFile = OGGetAddress.missions_custom + missionEditor.missionName.text;
            }
            else
            {
                saveFile = OGGetAddress.missions_custom + missionEditor.missionName.text + ".json";
            }

            File.WriteAllText(saveFile, jsonString);

            DisplayMessage(missionEditor.missionName.text + " saved to " + OGGetAddress.missions_custom);

            if (window != null)
            {
                WindowFunctions.DeleteWindow(window);
            }
        }
        else
        {
            OpenWindow("savemissionas");
        }

        CloseAllMenus();
    }

    public static void ExportSelectionAs(Window window = null)
    {
        List<MissionEvent> exportList = new List<MissionEvent>();

        MissionEditor missionEditor = GetMissionEditor();

        string exportFileName = GetMissionNameFromExportDialog();

        foreach (Node node in missionEditor.nodes)
        {
            if (node != null)
            {
                if (node.selected == true)
                {
                    MissionEvent missionEvent = new MissionEvent();

                    missionEvent.eventID = ParseTextToString(node.eventID);
                    missionEvent.eventType = ParseTextToString(node.eventType);
                    missionEvent.conditionLocation = ParseTextToString(node.conditionLocation);
                    missionEvent.conditionTime = ParseTextToFloat(node.conditionTime);
                    missionEvent.x = ParseTextToFloat(node.x);
                    missionEvent.y = ParseTextToFloat(node.y);
                    missionEvent.z = ParseTextToFloat(node.z);
                    missionEvent.xRotation = ParseTextToFloat(node.xRotation);
                    missionEvent.yRotation = ParseTextToFloat(node.yRotation);
                    missionEvent.zRotation = ParseTextToFloat(node.zRotation);
                    missionEvent.data1 = ParseTextToString(node.data1);
                    missionEvent.data2 = ParseTextToString(node.data2);
                    missionEvent.data3 = ParseTextToString(node.data3);
                    missionEvent.data4 = ParseTextToString(node.data4);
                    missionEvent.data5 = ParseTextToString(node.data5);
                    missionEvent.data6 = ParseTextToString(node.data6);
                    missionEvent.data7 = ParseTextToString(node.data7);
                    missionEvent.data8 = ParseTextToString(node.data8);
                    missionEvent.data9 = ParseTextToString(node.data9);
                    missionEvent.data10 = ParseTextToString(node.data10);
                    missionEvent.data11 = ParseTextToString(node.data11);
                    missionEvent.data12 = ParseTextToString(node.data12);
                    missionEvent.data13 = ParseTextToString(node.data13);
                    missionEvent.data14 = ParseTextToString(node.data14);
                    missionEvent.data15 = ParseTextToString(node.data15);
                    missionEvent.nextEvent1 = ParseTextToString(node.nextEvent1);
                    missionEvent.nextEvent2 = ParseTextToString(node.nextEvent2);
                    missionEvent.nextEvent3 = ParseTextToString(node.nextEvent3);
                    missionEvent.nextEvent4 = ParseTextToString(node.nextEvent4);
                    missionEvent.nodePosX = node.nodePosX;
                    missionEvent.nodePosY = node.nodePosY;

                    exportList.Add(missionEvent);
                }
            }
        }

        MissionEvent[] exportListEventData = exportList.ToArray();

        string jsonString = JsonHelper.ToJson(exportListEventData, true);

        string saveFile = "none";

        saveFile = OGGetAddress.missions_custom + exportFileName + ".json";       

        File.WriteAllText(saveFile, jsonString);

        DisplayMessage(exportFileName + " saved to " + OGGetAddress.missions_custom);

        if (window != null)
        {
            WindowFunctions.DeleteWindow(window);
        }

        CloseAllMenus();
    }

    public static void CopySelection()
    {
        List<MissionEvent> exportList = new List<MissionEvent>();

        MissionEditor missionEditor = GetMissionEditor();

        string exportFileName = GetMissionNameFromExportDialog();

        foreach (Node node in missionEditor.nodes)
        {
            if (node != null)
            {
                if (node.selected == true)
                {
                    MissionEvent missionEvent = new MissionEvent();

                    missionEvent.eventID = ParseTextToString(node.eventID);
                    missionEvent.eventType = ParseTextToString(node.eventType);
                    missionEvent.conditionLocation = ParseTextToString(node.conditionLocation);
                    missionEvent.conditionTime = ParseTextToFloat(node.conditionTime);
                    missionEvent.x = ParseTextToFloat(node.x);
                    missionEvent.y = ParseTextToFloat(node.y);
                    missionEvent.z = ParseTextToFloat(node.z);
                    missionEvent.xRotation = ParseTextToFloat(node.xRotation);
                    missionEvent.yRotation = ParseTextToFloat(node.yRotation);
                    missionEvent.zRotation = ParseTextToFloat(node.zRotation);
                    missionEvent.data1 = ParseTextToString(node.data1);
                    missionEvent.data2 = ParseTextToString(node.data2);
                    missionEvent.data3 = ParseTextToString(node.data3);
                    missionEvent.data4 = ParseTextToString(node.data4);
                    missionEvent.data5 = ParseTextToString(node.data5);
                    missionEvent.data6 = ParseTextToString(node.data6);
                    missionEvent.data7 = ParseTextToString(node.data7);
                    missionEvent.data8 = ParseTextToString(node.data8);
                    missionEvent.data9 = ParseTextToString(node.data9);
                    missionEvent.data10 = ParseTextToString(node.data10);
                    missionEvent.data11 = ParseTextToString(node.data11);
                    missionEvent.data12 = ParseTextToString(node.data12);
                    missionEvent.data13 = ParseTextToString(node.data13);
                    missionEvent.data14 = ParseTextToString(node.data14);
                    missionEvent.data15 = ParseTextToString(node.data15);
                    missionEvent.nextEvent1 = ParseTextToString(node.nextEvent1);
                    missionEvent.nextEvent2 = ParseTextToString(node.nextEvent2);
                    missionEvent.nextEvent3 = ParseTextToString(node.nextEvent3);
                    missionEvent.nextEvent4 = ParseTextToString(node.nextEvent4);
                    missionEvent.nodePosX = node.nodePosX;
                    missionEvent.nodePosY = node.nodePosY;

                    exportList.Add(missionEvent);
                }
            }
        }

        MissionEvent[] exportListEventData = exportList.ToArray();

        string clipboard = JsonHelper.ToJson(exportListEventData, true);

        missionEditor.clipboard = clipboard;

        CloseAllMenus();
    }

    public static void UpdateMissionName(string name)
    {
        MissionEditor missionEditor = GetMissionEditor();

        if (missionEditor != null)
        {
            missionEditor.missionName.text = name;
        }
    }

    public static string GetMissionNameFromSaveDialog()
    {
        MissionEditor missionEditor = GetMissionEditor();

        string name = "Untitled Mission";

        if (missionEditor != null)
        {
            name = missionEditor.missionName.text;
        }

        GameObject MissionNameField = GameObject.Find("MissionNameField");

        if (MissionNameField != null)
        {
            Text missionName = MissionNameField.GetComponent<Text>();

            if (missionName != null)
            {
                name = missionName.text;
            }
        }

        return name;
    }

    public static string GetMissionNameFromExportDialog()
    {
        MissionEditor missionEditor = GetMissionEditor();

        string name = "Untitled Export";

        if (missionEditor != null)
        {
            name = missionEditor.missionName.text;
        }

        GameObject MissionNameField = GameObject.Find("FileNameField");

        if (MissionNameField != null)
        {
            Text missionName = MissionNameField.GetComponent<Text>();

            if (missionName != null)
            {
                name = missionName.text;
            }
        }

        return name;
    }

    public static void DisplayMessage(string message)
    {
        MissionEditor missionEditor = GetMissionEditor();

        if (missionEditor != null)
        {
            if (missionEditor.messageTextbox != null)
            {
                missionEditor.messageTextbox.text = message;
            }
        }

    }

    public static float ParseTextToFloat(Text text)
    {
        float input = 0;

        if (text != null)
        {
            if (float.TryParse(text.text, out _))
            {
                input = float.Parse(text.text);
            }
        }

        return input;
    }

    public static string ParseTextToString(Text text)
    {
        string input = "none";

        if (text != null)
        {
            if (text.text != "")
            {
                input = text.text;
            }

        }

        return input;
    }

    #endregion

    #region new mission

    public static void NewMission()
    {
        NodeFunctions.DeleteAllNodes();
        CloseAllMenus();
    }

    #endregion

    #region loading functions

    public static void SelectNodeType(string nodeType)
    {
        MissionEditor missionEditor = GetMissionEditor();

        missionEditor.selectedNodeTypeToLoad = nodeType;

        if (missionEditor.AddNodeTextBox == null)
        {
            GameObject AddNodeTextBoxGO = GameObject.Find("AddNodeTextBox");

            if (AddNodeTextBoxGO != null)
            {
                missionEditor.AddNodeTextBox = AddNodeTextBoxGO.GetComponentInChildren<Text>();
            }
        }

        if (missionEditor.AddNodeTextBox != null)
        {
            missionEditor.AddNodeTextBox.text = NodeDescriptions.GetNodeDescription(nodeType);
        }
    }

    public static void AddSelectedNodeType()
    {
        MissionEditor missionEditor = GetMissionEditor();

        int x = Screen.width / 2;
        int y = Screen.height / 2;

        Vector2 pointOnGrid = missionEditor.editorContentRect.InverseTransformPoint(new Vector3(x, y));

        AddNode(missionEditor.selectedNodeTypeToLoad, true, pointOnGrid.x, pointOnGrid.y);

        DisplayMessage("Added " + missionEditor.selectedNodeTypeToLoad);
    }

    public static Node AddNode(string nodeType, bool setPosition = false, float nodePosX = 0, float nodePosY = 0)
    {
        MissionEditor missionEditor = GetMissionEditor();

        if (missionEditor.nodes == null)
        {
            missionEditor.nodes = new List<Node>();
        }

        GameObject editorContent = GameObject.Find("EditorContent");

        GameObject nodeGO = new GameObject();
        nodeGO.transform.SetParent(editorContent.transform);
        Node node = nodeGO.AddComponent<Node>();
        node.nodeType = nodeType;
        missionEditor.nodes.Add(node);

        if (setPosition == true)
        {
            NodeFunctions.SetNodePosition(node, nodePosX, nodePosY);
        }

        //The caret doens't load until a frame afer the component is added, and so this function needs to be delayed slighty
        Task a = new Task(NodeFunctions.ModifyCaretPositionTimed(1f));
        NodeFunctions.SetDropDownMenu();

        return node;
    }

    public static void LoadMission(Window window)
    {
        MissionEditor missionEditor = GetMissionEditor();

        NodeFunctions.DeleteAllNodes();

        if (missionEditor != null)
        {
            string missionAddress = OGGetAddress.missions_custom + missionEditor.selectedMissionToLoad;
            string missionDataString = File.ReadAllText(missionAddress);
            TextAsset missionDataTextAsset = new TextAsset(missionDataString);
            Mission mission = JsonUtility.FromJson<Mission>(missionDataTextAsset.text);
            Task a = new Task(LoadMissionData(mission));
        }

        UpdateMissionName(missionEditor.selectedMissionToLoad);

        WindowFunctions.DeleteWindow(window);
    }

    public static void MergeMissions(Window window)
    {
        MissionEditor missionEditor = GetMissionEditor();

        if (missionEditor != null)
        {
            string missionAddress = OGGetAddress.missions_custom + missionEditor.selectedMissionToLoad;
            string missionDataString = File.ReadAllText(missionAddress);
            TextAsset missionDataTextAsset = new TextAsset(missionDataString);
            Mission mission = JsonUtility.FromJson<Mission>(missionDataTextAsset.text);
            Task a = new Task(LoadMissionData(mission));

            DisplayMessage("Loaded " + missionEditor.selectedMissionToLoad);
        }

        WindowFunctions.DeleteWindow(window);
    }

    public static IEnumerator LoadMissionData(Mission mission)
    {
        float number = mission.missionEventData.Length * 2;
        float count = 0;

        foreach (MissionEvent missionEvent in mission.missionEventData)
        {
            Node node = AddNode(missionEvent.eventType, true, missionEvent.nodePosX, missionEvent.nodePosY);

            yield return null;

            InputData(node.eventID, missionEvent.eventID);
            InputData(node.eventType, missionEvent.eventType);
            InputData(node.conditionTime, missionEvent.conditionTime.ToString());
            InputData(node.conditionLocation, missionEvent.conditionLocation);
            InputData(node.x, missionEvent.x.ToString());
            InputData(node.y, missionEvent.y.ToString());
            InputData(node.z, missionEvent.z.ToString());
            InputData(node.xRotation, missionEvent.xRotation.ToString());
            InputData(node.yRotation, missionEvent.yRotation.ToString());
            InputData(node.zRotation, missionEvent.zRotation.ToString());
            InputData(node.data1, missionEvent.data1);
            InputData(node.data2, missionEvent.data2);
            InputData(node.data3, missionEvent.data3);
            InputData(node.data4, missionEvent.data4);
            InputData(node.data5, missionEvent.data5);
            InputData(node.data6, missionEvent.data6);
            InputData(node.data7, missionEvent.data7);
            InputData(node.data8, missionEvent.data8);
            InputData(node.data9, missionEvent.data9);
            InputData(node.data10, missionEvent.data10);
            InputData(node.data11, missionEvent.data11);
            InputData(node.data12, missionEvent.data12);
            InputData(node.data13, missionEvent.data13);
            InputData(node.data14, missionEvent.data14);
            InputData(node.data15, missionEvent.data15);
            InputData(node.nextEvent1, missionEvent.nextEvent1);
            InputData(node.nextEvent2, missionEvent.nextEvent2);
            InputData(node.nextEvent3, missionEvent.nextEvent3);
            InputData(node.nextEvent4, missionEvent.nextEvent4);
            node.nodePosX = missionEvent.nodePosX;
            node.nodePosY = missionEvent.nodePosY;

            float percentage = (count / number) * 100;
            DisplayMessage("Loading " + percentage.ToString("00") + "% Complete");
            count++;
        }

        foreach (MissionEvent missionEvent in mission.missionEventData)
        {
            MissionEditor missionEditor = GetMissionEditor();

            Node firstNode = SearchNodes(missionEvent.eventID, missionEditor.nodes.ToArray());

            if (firstNode != null)
            {
                if (firstNode.maleNodeLinks != null)
                {
                    if (missionEvent.nextEvent1 != "none" & firstNode.maleNodeLinks.Count > 0)
                    {
                        Node nextEvent1 = SearchNodes(missionEvent.nextEvent1, missionEditor.nodes.ToArray());

                        if (nextEvent1 != null & firstNode.maleNodeLinks[0] != null)
                        {
                            if (nextEvent1.femaleNodeLink != null)
                            {
                                firstNode.maleNodeLinks[0].connectedNode = nextEvent1.femaleNodeLink;
                            }
                        }
                    }

                    if (missionEvent.nextEvent2 != "none" & firstNode.maleNodeLinks.Count > 1)
                    {
                        Node nextEvent2 = SearchNodes(missionEvent.nextEvent2, missionEditor.nodes.ToArray());

                        if (nextEvent2 != null & firstNode.maleNodeLinks[1] != null)
                        {
                            if (nextEvent2.femaleNodeLink != null)
                            {
                                firstNode.maleNodeLinks[1].connectedNode = nextEvent2.femaleNodeLink;
                            }
                        }
                    }

                    if (missionEvent.nextEvent3 != "none" & firstNode.maleNodeLinks.Count > 2)
                    {
                        Node nextEvent3 = SearchNodes(missionEvent.nextEvent3, missionEditor.nodes.ToArray());

                        if (nextEvent3 != null & firstNode.maleNodeLinks[2] != null)
                        {
                            if (nextEvent3.femaleNodeLink != null)
                            {
                                firstNode.maleNodeLinks[2].connectedNode = nextEvent3.femaleNodeLink;
                            }
                        }
                    }

                    if (missionEvent.nextEvent4 != "none" & firstNode.maleNodeLinks.Count > 3)
                    {
                        Node nextEvent4 = SearchNodes(missionEvent.nextEvent4, missionEditor.nodes.ToArray());

                        if (nextEvent4 != null & firstNode.maleNodeLinks[3] != null)
                        {
                            if (nextEvent4.femaleNodeLink != null)
                            {
                                firstNode.maleNodeLinks[3].connectedNode = nextEvent4.femaleNodeLink;
                            }
                        }
                    }
                }
            }

            float percentage = (count / number) * 100;
            DisplayMessage("Loading " + percentage.ToString("00") + "% Complete");
            count++;

            yield return null;
        }

        //This modifies the caret position to ensure that they display on top of the nodes and not behind them
        NodeFunctions.ModifyCaretPosition();
        NodeFunctions.SetDropDownMenu();

        DisplayMessage("Loading Mission Complete");
    }

    public static IEnumerator PasteMissionData(bool useMousePosition = false)
    {
        MissionEditor missionEditor = GetMissionEditor();

        missionEditor.pasting = true;

        Mission clipBoardMissionData = JsonUtility.FromJson<Mission>(missionEditor.clipboard);

        float number = clipBoardMissionData.missionEventData.Length * 2;
        float count = 0;

        List<Node> pasteNodeList = new List<Node>();

        //This gets the mouse position
        Vector2 placementPosition = new Vector3();

        if (useMousePosition == false) //Position at center of screen
        {
            int x = Screen.width / 2;
            int y = Screen.height / 2;

            placementPosition = missionEditor.editorContentRect.InverseTransformPoint(new Vector3(x, y));
        }
        else //Mouse position
        {
            placementPosition = missionEditor.editorContentRect.InverseTransformPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
        }

        //This gets the node in the selection closest to 0,0
        float distance = Mathf.Infinity;
        Vector2 basePosition = new Vector2();

        foreach (MissionEvent missionEvent in clipBoardMissionData.missionEventData)
        {
            float x = missionEvent.nodePosX;
            float y = missionEvent.nodePosY;

            Vector2 tempPosition = new Vector2(x, y);

            float tempDistance = Vector2.Distance(new Vector2(0, 0), tempPosition);

            if (tempDistance < distance)
            {
                distance = tempDistance;
                basePosition = tempPosition;
            }
        }

        //This creates the new node and inputs the data
        foreach (MissionEvent missionEvent in clipBoardMissionData.missionEventData)
        {
            //This calculates a new node position based on the mouse position when paste was called
            Vector2 originalNodePosition = new Vector2(missionEvent.nodePosX, missionEvent.nodePosY);
            Vector2 position = originalNodePosition - basePosition + placementPosition;

            Node node = AddNode(missionEvent.eventType, true, position.x, position.y);

            yield return null;

            InputData(node.eventID, missionEvent.eventID);
            InputData(node.eventType, missionEvent.eventType);
            InputData(node.conditionTime, missionEvent.conditionTime.ToString());
            InputData(node.conditionLocation, missionEvent.conditionLocation);
            InputData(node.x, missionEvent.x.ToString());
            InputData(node.y, missionEvent.y.ToString());
            InputData(node.z, missionEvent.z.ToString());
            InputData(node.xRotation, missionEvent.xRotation.ToString());
            InputData(node.yRotation, missionEvent.yRotation.ToString());
            InputData(node.zRotation, missionEvent.zRotation.ToString());
            InputData(node.data1, missionEvent.data1);
            InputData(node.data2, missionEvent.data2);
            InputData(node.data3, missionEvent.data3);
            InputData(node.data4, missionEvent.data4);
            InputData(node.data5, missionEvent.data5);
            InputData(node.data6, missionEvent.data6);
            InputData(node.data7, missionEvent.data7);
            InputData(node.data8, missionEvent.data8);
            InputData(node.data9, missionEvent.data9);
            InputData(node.data10, missionEvent.data10);
            InputData(node.data11, missionEvent.data11);
            InputData(node.data12, missionEvent.data12);
            InputData(node.data13, missionEvent.data13);
            InputData(node.data14, missionEvent.data14);
            InputData(node.data15, missionEvent.data15);
            InputData(node.nextEvent1, missionEvent.nextEvent1);
            InputData(node.nextEvent2, missionEvent.nextEvent2);
            InputData(node.nextEvent3, missionEvent.nextEvent3);
            InputData(node.nextEvent4, missionEvent.nextEvent4);
            node.nodePosX = position.x;
            node.nodePosY = position.y;

            pasteNodeList.Add(node);

            float percentage = (count / number) * 100;
            DisplayMessage("Pasting " + percentage.ToString("00") + "% Complete");
            count++;
        }

        //This creates the node links
        foreach (MissionEvent missionEvent in clipBoardMissionData.missionEventData)
        {
            Node firstNode = SearchNodes(missionEvent.eventID, pasteNodeList.ToArray());

            if (firstNode != null)
            {
                if (firstNode.maleNodeLinks != null)
                {
                    if (missionEvent.nextEvent1 != "none" & firstNode.maleNodeLinks.Count > 0)
                    {
                        Node nextEvent1 = SearchNodes(missionEvent.nextEvent1, pasteNodeList.ToArray());

                        if (nextEvent1 != null & firstNode.maleNodeLinks[0] != null)
                        {
                            if (nextEvent1.femaleNodeLink != null)
                            {
                                firstNode.maleNodeLinks[0].connectedNode = nextEvent1.femaleNodeLink;
                            }
                        }
                    }

                    if (missionEvent.nextEvent2 != "none" & firstNode.maleNodeLinks.Count > 1)
                    {
                        Node nextEvent2 = SearchNodes(missionEvent.nextEvent2, pasteNodeList.ToArray());

                        if (nextEvent2 != null & firstNode.maleNodeLinks[1] != null)
                        {
                            if (nextEvent2.femaleNodeLink != null)
                            {
                                firstNode.maleNodeLinks[1].connectedNode = nextEvent2.femaleNodeLink;
                            }
                        }
                    }

                    if (missionEvent.nextEvent3 != "none" & firstNode.maleNodeLinks.Count > 2)
                    {
                        Node nextEvent3 = SearchNodes(missionEvent.nextEvent3, pasteNodeList.ToArray());

                        if (nextEvent3 != null & firstNode.maleNodeLinks[2] != null)
                        {
                            if (nextEvent3.femaleNodeLink != null)
                            {
                                firstNode.maleNodeLinks[2].connectedNode = nextEvent3.femaleNodeLink;
                            }
                        }
                    }

                    if (missionEvent.nextEvent4 != "none" & firstNode.maleNodeLinks.Count > 3)
                    {
                        Node nextEvent4 = SearchNodes(missionEvent.nextEvent4, pasteNodeList.ToArray());

                        if (nextEvent4 != null & firstNode.maleNodeLinks[3] != null)
                        {
                            if (nextEvent4.femaleNodeLink != null)
                            {
                                firstNode.maleNodeLinks[3].connectedNode = nextEvent4.femaleNodeLink;
                            }
                        }
                    }
                }
            }

            float percentage = (count / number) * 100;
            DisplayMessage("Pasting " + percentage.ToString("00") + "% Complete");
            count++;

            yield return null;
        }

        //This gives the pasted nodes a new ID
        foreach (Node node in pasteNodeList)
        {
            NodeFunctions.GetUniqueNodeID(node, true);
        }

        //This modifies the caret position to ensure that they display on top of the nodes and not behind them
        NodeFunctions.ModifyCaretPosition();
        NodeFunctions.SetDropDownMenu();

        missionEditor.pasting = false;

        DisplayMessage("Nodes Pasted from Clipboard");
    }

    //This searches the given list for a node with a matching id
    public static Node SearchNodes(string eventID, Node[] nodes)
    {
        Node node = null;

        if (nodes != null)
        {
            foreach (Node tempNode in nodes)
            {
                if (tempNode != null)
                {
                    if (tempNode.eventID != null)
                    {
                        if (tempNode.eventID.text == eventID)
                        {
                            node = tempNode;
                        }
                    }
                }
            }
        }
        
        return node;
    } 

    public static void InputData(Text text, string input)
    {
        if (text != null)
        {
            InputField inputField = text.GetComponentInParent<InputField>();

            if (inputField != null)
            {
                inputField.text = input;
            }

            text.text = input;
        }
    }

    public static void SelectMission(string mission)
    {
        MissionEditor missionEditor = GetMissionEditor();

        missionEditor.selectedMissionToLoad = mission;
    }

    #endregion

    #region mission editor functions

    //This scales the grid where the nodes are displayed
    public static void ScaleGrid(MissionEditor missionEditor)
    {
        bool scalingActive = true;
        bool scaling = false;

        //Checks whether mouse is in position to scale
        foreach (Window window in missionEditor.windows)
        {
            if (window != null)
            {
                if (window.gameObject.activeSelf == true)
                {
                    if (window.scaling == false)
                    {
                        scalingActive = false;
                    }
                }
            }
        }

        //This changes the scale
        if (scalingActive == true)
        {
            missionEditor.scale += Input.GetAxis("Mouse ScrollWheel") * Time.unscaledDeltaTime * 20;
        }

        //This locks the scale within certain bounds
        float percentage = ((Mathf.Abs(missionEditor.scale) - 0.3f) / 0.7f) * 100;

        if (percentage > 200)
        {
            missionEditor.scale = 1.7f;
            percentage = 200;
        }
        else if (percentage < 0)
        {
            missionEditor.scale = 0.3f;
            percentage = 0;
        }

        //This applies the scale
        if (scalingActive == true & missionEditor.scaleSave != missionEditor.scale)
        {
            if (missionEditor.editorContentRect != null)
            {
                int x = Screen.width / 2;
                int y = Screen.height / 2;

                Vector2 screenCenter = missionEditor.editorContentRect.InverseTransformPoint(new Vector3(x, y));

                Vector2 targetPosition = missionEditor.editorContentRect.InverseTransformPoint(Input.mousePosition);

                missionEditor.editorContentRect.localScale = new Vector3(missionEditor.scale, missionEditor.scale);

                SnapTo(missionEditor.scrollRect, screenCenter);
            }
        }

        //This outputs the scale to the indicator
        if (missionEditor.scaleIndicator != null)
        {
            missionEditor.scaleIndicator.text = percentage.ToString("000") + "%";
        }

        missionEditor.scaleSave = missionEditor.scale;
    }

    //This snaps the scrollrect to the designated coordinates keeping the view centered while zooming //This code is from geoathome on stackoverflow
    public static void SnapTo(ScrollRect instance, Vector2 child)
    {
        instance.content.ForceUpdateRectTransforms();
        instance.viewport.ForceUpdateRectTransforms();

        // now takes scaling into account
        Vector2 viewportLocalPosition = instance.viewport.localPosition;
        Vector2 childLocalPosition = child;
        Vector2 newContentPosition = new Vector2(
            0 - ((viewportLocalPosition.x * instance.viewport.localScale.x) + (childLocalPosition.x * instance.content.localScale.x)),
            0 - ((viewportLocalPosition.y * instance.viewport.localScale.y) + (childLocalPosition.y * instance.content.localScale.y))
        );

        // clamp positions
        instance.content.localPosition = newContentPosition;
        Rect contentRectInViewport = TransformRectFromTo(instance.content.transform, instance.viewport);
        float deltaXMin = contentRectInViewport.xMin - instance.viewport.rect.xMin;
        if (deltaXMin > 0) // clamp to <= 0
        {
            newContentPosition.x -= deltaXMin;
        }
        float deltaXMax = contentRectInViewport.xMax - instance.viewport.rect.xMax;
        if (deltaXMax < 0) // clamp to >= 0
        {
            newContentPosition.x -= deltaXMax;
        }
        float deltaYMin = contentRectInViewport.yMin - instance.viewport.rect.yMin;
        if (deltaYMin > 0) // clamp to <= 0
        {
            newContentPosition.y -= deltaYMin;
        }
        float deltaYMax = contentRectInViewport.yMax - instance.viewport.rect.yMax;
        if (deltaYMax < 0) // clamp to >= 0
        {
            newContentPosition.y -= deltaYMax;
        }

        // apply final position
        instance.content.localPosition = newContentPosition;
        instance.content.ForceUpdateRectTransforms();
    }

    public static Rect TransformRectFromTo(Transform from, Transform to)
    {
        RectTransform fromRectTrans = from.GetComponent<RectTransform>();
        RectTransform toRectTrans = to.GetComponent<RectTransform>();

        if (fromRectTrans != null && toRectTrans != null)
        {
            Vector3[] fromWorldCorners = new Vector3[4];
            Vector3[] toLocalCorners = new Vector3[4];
            Matrix4x4 toLocal = to.worldToLocalMatrix;
            fromRectTrans.GetWorldCorners(fromWorldCorners);
            for (int i = 0; i < 4; i++)
            {
                toLocalCorners[i] = toLocal.MultiplyPoint3x4(fromWorldCorners[i]);
            }

            return new Rect(toLocalCorners[0].x, toLocalCorners[0].y, toLocalCorners[2].x - toLocalCorners[1].x, toLocalCorners[1].y - toLocalCorners[0].y);
        }

        return default(Rect);
    }

    //This sets the window mode on the editor
    public static void SetWindowMode(string mode = "none")
    {
        if (mode != "window" & mode != "fullscreen" || mode == "none")
        {
            OGSettings settings = OGSettingsFunctions.GetSettings();

            mode = settings.editorWindowMode;
        }

        OGSettingsFunctions.SetEditorWindowMode(mode);

        CloseAllMenus();
    }

    //This exits the mission editor back to open galaxy
    public static void ExitMissionEditor()
    {
        MissionEditor missionEditor = GetMissionEditor();

        OGSettings settings = OGSettingsFunctions.GetSettings();

        OGSettingsFunctions.SetGameWindowMode(settings.gameWindowMode);

        OGSettingsFunctions.SetOGCursor();

        MainMenu mainMenu = MainMenuFunctions.GetMainMenu();

        if (mainMenu != null)
        {
            GameObject.Destroy(mainMenu.gameObject);
        }

        MainMenuFunctions.RunMenuWithoutIntroduction();

        if (missionEditor != null)
        {
            GameObject.Destroy(missionEditor.gameObject);
        }
    }

    //This exits both the editor and the game to windows
    public static void ExitToWindows()
    {
        Debug.Log("Quitting to windows - NOTE: Only works in build");

        Application.Quit();
    }

    //This toggles scrolling on and off
    public static void ToggleScrolling(MissionEditor missionEditor)
    {
        if (missionEditor.scrolling == true)
        {
            missionEditor.scrollRect.horizontal = true;
            missionEditor.scrollRect.vertical = true;
        }
        else
        {
            missionEditor.scrollRect.horizontal = false;
            missionEditor.scrollRect.vertical = false;
        }
    }

    //This opens a web page
    public static void OpenWebAddress(string url)
    {
        Application.OpenURL(url);
        CloseAllMenus();
    }

    #endregion

    #region mission editor utils

    //This function returns the active mission editor script when called
    public static MissionEditor GetMissionEditor()
    {
        MissionEditor missionEditor = GameObject.FindObjectOfType<MissionEditor>();

        return missionEditor;
    }

    //This creates a new rect transform
    public static RectTransform CreateRectTransformWithImage(string name, string imageName, Transform parent)
    {
        //This creates the game object
        GameObject gameObject = new GameObject();
        gameObject.name = name;

        //This adds the rect transform and sets the parent
        RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
        rectTransform.SetParent(parent);

        //This adds the image to the recttransform
        Image image = gameObject.AddComponent<Image>();
        image.sprite = Resources.Load<Sprite>(OGGetAddress.missioneditor + imageName);
        image.type = Image.Type.Sliced;

        //This returns the rect transform
        return rectTransform;
    }

    //This sets the rect transform
    public static void SetRectTransform(RectTransform rectTransform, string mode, Vector2 position, float width, float height, float scale)
    {
        if (mode == "center")
        {
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
        }
        else if (mode == "centerleft")
        {
            rectTransform.anchorMax = new Vector2(0, 0.5f);
            rectTransform.anchorMin = new Vector2(0, 0.5f);
            rectTransform.pivot = new Vector2(0, 0.5f);
        }
        else if (mode == "topleft")
        {
            rectTransform.anchorMax = new Vector2(0, 1);
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.pivot = new Vector2(0, 1);
        }

        rectTransform.anchoredPosition = new Vector2(position.x, position.y);
        rectTransform.sizeDelta = new Vector2(width, height);
        rectTransform.localScale = new Vector3(scale, scale, scale);
    }

    #endregion

}
