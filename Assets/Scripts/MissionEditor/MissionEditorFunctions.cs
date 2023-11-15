using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public static class MissionEditorFunctions
{

    #region draw mission editor

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
        GameObject titleGO = new GameObject();

        titleGO.transform.SetParent(missionEditor.transform);
        RectTransform rectTransform = titleGO.AddComponent<RectTransform>();
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.pivot = new Vector2(0, 0);
        rectTransform.anchoredPosition = new Vector2(10, 0);
        rectTransform.sizeDelta = new Vector2(90, 12f);
        rectTransform.localScale = new Vector3(1, 1, 1);

        Text text = titleGO.AddComponent<Text>();
        text.supportRichText = false;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.text = "100%";
        text.fontSize = 7;
        text.color = Color.white;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.alignment = TextAnchor.MiddleLeft;

        titleGO.name = "Scale Indicator";

        missionEditor.scaleIndicator = text;
    }

    public static void Draw_MesssageTextBox(MissionEditor missionEditor)
    {
        //This draws the input label
        GameObject messageTextboxGO = new GameObject();

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

        messageTextboxGO.name = "Action Indicator";

        missionEditor.messageTextbox = text;
    }

    public static void Draw_CurrentFileTextBox(MissionEditor missionEditor)
    {
        //This draws the input label
        GameObject currentMissionTextboxGO = new GameObject();

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

        currentMissionTextboxGO.name = "Mission Name";

        missionEditor.missionName = text;
    }

    public static void Draw_MenuBar(MissionEditor missionEditor)
    {
        //This draws the input label
        GameObject menuBarGO = new GameObject();

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

        menuBarGO.name = "Menu Bar";
    }

    public static void Draw_InfoBar(MissionEditor missionEditor)
    {
        //This creates the button bar
        GameObject infoBar1GO = new GameObject();

        infoBar1GO.transform.SetParent(missionEditor.transform);
        RectTransform rectTransform = infoBar1GO.AddComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(1, 0);
        rectTransform.pivot = new Vector2(0.5f, 0);
        rectTransform.anchoredPosition = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(0, 12f);
        rectTransform.localScale = new Vector3(1, 1, 1);

        Image image = infoBar1GO.AddComponent<Image>();

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
            DrawTextButton(missionEditor.menuBarRectTransform.transform, 0, 0, 12, 25, "File", 7, "ActivateMenu", TextAnchor.MiddleCenter);
            DrawTextButton(missionEditor.menuBarRectTransform.transform, 25, 0, 12, 45, "Add Event", 7, "ActivateMenu", TextAnchor.MiddleCenter);
            DrawTextButton(missionEditor.menuBarRectTransform.transform, 70, 0, 12, 30, "Window", 7, "ActivateMenu", TextAnchor.MiddleCenter);
            DrawTextButton(missionEditor.menuBarRectTransform.transform, 100, 0, 12, 25, "Help", 7, "ActivateMenu", TextAnchor.MiddleCenter);
        }

        float shift = 0;
        string spaces = "        ";

        List<string> file_Buttons = new List<string>();
        file_Buttons.Add(spaces + "New");
        file_Buttons.Add(spaces + "Open");
        file_Buttons.Add(spaces + "Merge");
        file_Buttons.Add(spaces + "Save");
        file_Buttons.Add(spaces + "SaveAs");
        file_Buttons.Add(spaces + "Exit to Open Galaxy");
        file_Buttons.Add(spaces + "Exit to Windows");

        List<string> file_Functions = new List<string>();
        file_Functions.Add("OpenNewWindow");
        file_Functions.Add("OpenOpenWindow");
        file_Functions.Add("OpenMergeWindow");
        file_Functions.Add("Save");
        file_Functions.Add("OpenSaveAsWindow");
        file_Functions.Add("ExitMissionEditor");
        file_Functions.Add("ExitToWindows");

        GameObject fileMenu = DrawDropDownMenu(missionEditor.transform, "File", shift, file_Buttons.ToArray(), file_Functions.ToArray());

        List<string> addEvent_Buttons = new List<string>();
        addEvent_Buttons.Add(spaces + "Add New Event");

        List<string> addEvent_Functions = new List<string>();
        addEvent_Functions.Add("OpenAddNewEvent");

        shift += 25;

        GameObject addEventMenu = DrawDropDownMenu(missionEditor.transform, "Add Event", shift, addEvent_Buttons.ToArray(), addEvent_Functions.ToArray());

        List<string> window_Buttons = new List<string>();
        window_Buttons.Add(spaces + "Make Fullscreen");
        window_Buttons.Add(spaces + "Make Windowed");

        List<string> window_Functions = new List<string>();
        window_Functions.Add("MakeFullscreen");
        window_Functions.Add("MakeWindowed");

        shift += 45;

        GameObject windowMenu = DrawDropDownMenu(missionEditor.transform, "Window", shift, window_Buttons.ToArray(), window_Functions.ToArray());

        List<string> help_Buttons = new List<string>();
        help_Buttons.Add(spaces + "Open Open-Galaxy Github");
        help_Buttons.Add(spaces + "About OG Mission Editor");

        List<string> help_Functions = new List<string>();
        help_Functions.Add("OpenGitHub");
        help_Functions.Add("OpenAbout");

        shift += 30;

        GameObject helpMenu = DrawDropDownMenu(missionEditor.transform, "Help", shift, help_Buttons.ToArray(), help_Functions.ToArray());

        if (missionEditor.menus == null)
        {
            missionEditor.menus = new List<GameObject>();
        }
        else
        {
            missionEditor.menus.Add(fileMenu);
            missionEditor.menus.Add(addEventMenu);
            missionEditor.menus.Add(windowMenu);
            missionEditor.menus.Add(helpMenu);

            foreach (GameObject menu in missionEditor.menus)
            {
                menu.SetActive(false);
            }
        }
    }

    public static void DrawTextButton(Transform parent, float xPos, float yPos, float height, float width, string buttonText, int fontSize, string functionType, TextAnchor alignement = TextAnchor.MiddleCenter)
    {
        GameObject buttonGO = new GameObject();
        GameObject buttonTextGO = new GameObject();
 
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

        if (ColorUtility.TryParseHtmlString("#C3C3C3", out color))
        {
            ColorBlock colorVar = button.colors;
            colorVar.highlightedColor = color;
        }

        if (functionType == "ActivateMenu")
        {
            button.onClick.AddListener(() => { ActivateMenu(buttonText); });
        }
        else if (functionType == "ExitMissionEditor")
        {
            button.onClick.AddListener(() => { ExitMissionEditor(); });
        }
        else if (functionType == "ExitToWindows")
        {
            button.onClick.AddListener(() => { ExitToWindows(); });
        }
        else if (functionType == "Save")
        {
            button.onClick.AddListener(() => { SaveMission(); });
        }
        else if (functionType == "OpenSaveAsWindow")
        {
            button.onClick.AddListener(() => { OpenWindow("savemissionas"); });
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
            button.onClick.AddListener(() => { NodeFunctions.DeleteAllNodes(); });
        }
        else if (functionType == "OpenAddNewEvent")
        {
            button.onClick.AddListener(() => { OpenWindow("addnodes"); });
        }
        else if (functionType == "MakeFullscreen")
        {
            button.onClick.AddListener(() => { SetWindowMode("fullscreen"); });
        }
        else if (functionType == "MakeWindowed")
        {
            button.onClick.AddListener(() => { SetWindowMode("window"); });
        }
        else if (functionType == "OpenGitHub")
        {
            button.onClick.AddListener(() => { OpenWebAddress("https://github.com/MichaelEGA/Open-Galaxy"); });
        }
        else if (functionType == "OpenAbout")
        {
            button.onClick.AddListener(() => { OpenWindow("abouteditor"); });
        }

        buttonGO.name = "button_" + functionType;
    }

    public static GameObject DrawDropDownMenu(Transform parent, string name, float xPosition, string[] buttons, string[] functions)
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
        background.sprite = Resources.Load<Sprite>("Data/EditorAssets/NodeSprite_Light");
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
            DrawTextButton(rectTransform.transform, 2, drop, 12, 146, button, 7, functions[buttonNo], TextAnchor.MiddleLeft);
            drop -= 12;
            buttonNo += 1;
        }

        return menuBaseGO;
    }

    #endregion

    #region menus

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

    #region windows

    public static void OpenWindow(string windowName)
    {
        MissionEditor missionEditor = GetMissionEditor();

        if (missionEditor.windows == null)
        {
            missionEditor.windows = new List<Window>();
        }

        GameObject windowGO = new GameObject();
        windowGO.transform.SetParent(missionEditor.gameObject.transform);
        Window window = windowGO.AddComponent<Window>();
        window.windowType = windowName;
        missionEditor.windows.Add(window);
    }

    #endregion

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
            if (nodeType == "custom_node")
            {
                missionEditor.AddNodeTextBox.text = "This node is for manually inputing event information. If a node doesn't exist for a specific event or function you can use the custom node to access it.";
            }
            else if (nodeType == "preload_loadasteroids")
            {
                missionEditor.AddNodeTextBox.text = "Use this node to generate an asteroid field before the mission starts. Preload functions will automatically execute before the mission starts and do not need to be linked to any other events.";
            }
            else
            {
                missionEditor.AddNodeTextBox.text = "";
            }
        }
    }

    public static void SelectMission(string mission)
    {
        MissionEditor missionEditor = GetMissionEditor();

        missionEditor.selectedMissionToLoad = mission;
    }

    public static void AddSelectedNodeType()
    {
        MissionEditor missionEditor = GetMissionEditor();
        AddNode(missionEditor.selectedNodeTypeToLoad);
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

        return node;
    }

    public static void ScaleGrid(MissionEditor missionEditor)
    {
        bool scaling = true;

        //Checks whether mouse is in position to scale
        foreach (Window window in missionEditor.windows)
        {
            if (window != null)
            {
                if (window.gameObject.activeSelf == true)
                {
                    if (window.scaling == false)
                    {
                        scaling = false;
                    }
                }
            }
        }

        //This changes the scale
        if (scaling == true)
        {
            missionEditor.scale += Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * 20;
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
        if (scaling == true)
        {
            if (missionEditor.editorContentRect != null)
            {
                missionEditor.editorContentRect.localScale = new Vector3(missionEditor.scale, missionEditor.scale);
            }
        }

        //This outputs the scale to the indicator
        if (missionEditor.scaleIndicator != null)
        {
            missionEditor.scaleIndicator.text = percentage.ToString("000") + "%";
        }
    }

    public static void SetWindowMode(string mode = "none")
    {
        if (mode != "window" & mode != "fullscreen" || mode == "none")
        {
            OGSettings settings = OGSettingsFunctions.GetSettings();

            mode = settings.editorWindowMode;
        }

        OGSettingsFunctions.SetEditorWindowMode(mode);
    }

    public static void ExitMissionEditor()
    {
        MissionEditor missionEditor = GetMissionEditor();

        OGSettings settings = OGSettingsFunctions.GetSettings();

        OGSettingsFunctions.SetGameWindowMode(settings.gameWindowMode);

        OGSettingsFunctions.SetOGCursor();

        if (missionEditor != null)
        {
            missionEditor.gameObject.SetActive(false);
        }
    }

    public static void ExitToWindows()
    {
        Debug.Log("Quitting to windows - NOTE: Only works in build");

        Application.Quit();
    }

    public static MissionEditor GetMissionEditor()
    {
        MissionEditor missionEditor = GameObject.FindObjectOfType<MissionEditor>();

        return missionEditor;
    }

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

    public static void LoadMission(Window window)
    {
        MissionEditor missionEditor = GetMissionEditor();

        NodeFunctions.DeleteAllNodes();

        if (missionEditor != null)
        {
            string missionAddress = Application.persistentDataPath + "/Custom Missions/" + missionEditor.selectedMissionToLoad;
            string missionDataString = File.ReadAllText(missionAddress);
            TextAsset missionDataTextAsset = new TextAsset(missionDataString);
            Mission mission = JsonUtility.FromJson<Mission>(missionDataTextAsset.text);
            Task a = new Task(LoadMissionData(mission));

            DisplayMessage("Loaded " + missionEditor.selectedMissionToLoad);
        }

        UpdateMissionName(missionEditor.selectedMissionToLoad);

        WindowFunctions.DeleteWindow(window);
    }

    public static void MergeMissions(Window window)
    {
        MissionEditor missionEditor = GetMissionEditor();

        if (missionEditor != null)
        {
            string missionAddress = Application.persistentDataPath + "/Custom Missions/" + missionEditor.selectedMissionToLoad;
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
        foreach (MissionEvent missionEvent in mission.missionEventData)
        {
            Node node = AddNode(missionEvent.eventType, true, missionEvent.nodePosX, missionEvent.nodePosY);

            yield return new WaitForEndOfFrame();

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
            InputData(node.data15, missionEvent.data5);
            InputData(node.nextEvent1, missionEvent.nextEvent1);
            InputData(node.nextEvent2, missionEvent.nextEvent2);
            InputData(node.nextEvent3, missionEvent.nextEvent3);
            InputData(node.nextEvent4, missionEvent.nextEvent4);
            node.nodePosX = missionEvent.nodePosX;
            node.nodePosY = missionEvent.nodePosY;
        }

        foreach (MissionEvent missionEvent in mission.missionEventData)
        {
            MissionEditor missionEditor = GetMissionEditor();

            Node firstNode = SearchNodes(missionEvent.eventID);

            if (firstNode != null)
            {
                if (firstNode.maleNodeLinks != null)
                {
                    if (missionEvent.nextEvent1 != "none" & firstNode.maleNodeLinks.Count > 0)
                    {
                        Node nextEvent1 = SearchNodes(missionEvent.nextEvent1);

                        if (nextEvent1 != null & firstNode.maleNodeLinks[0] != null)
                        {
                            if (nextEvent1.femaleNodeLink != null)
                            {
                                firstNode.maleNodeLinks[0].connectedNode = nextEvent1.femaleNodeLink;
                            }
                        }
                    }

                    if (missionEvent.nextEvent1 != "none" & firstNode.maleNodeLinks.Count > 1)
                    {
                        Node nextEvent2 = SearchNodes(missionEvent.nextEvent2);

                        if (nextEvent2 != null & firstNode.maleNodeLinks[1] != null)
                        {
                            if (nextEvent2.femaleNodeLink != null)
                            {
                                firstNode.maleNodeLinks[1].connectedNode = nextEvent2.femaleNodeLink;
                            }
                        }
                    }

                    if (missionEvent.nextEvent1 != "none" & firstNode.maleNodeLinks.Count > 2)
                    {
                        Node nextEvent3 = SearchNodes(missionEvent.nextEvent3);

                        if (nextEvent3 != null & firstNode.maleNodeLinks[2] != null)
                        {
                            if (nextEvent3.femaleNodeLink != null)
                            {
                                firstNode.maleNodeLinks[2].connectedNode = nextEvent3.femaleNodeLink;
                            }
                        }
                    }

                    if (missionEvent.nextEvent1 != "none" & firstNode.maleNodeLinks.Count > 3)
                    {
                        Node nextEvent4 = SearchNodes(missionEvent.nextEvent4);

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
        }
    }

    public static Node SearchNodes(string eventID)
    {
        MissionEditor missionEditor = GetMissionEditor();

        Node node = null;

        if (missionEditor != null)
        {
            if (missionEditor.nodes != null)
            {
                foreach (Node tempNode in missionEditor.nodes)
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

            string saveFile = Application.persistentDataPath + "/Custom Missions/" + missionEditor.missionName.text + ".json";

            File.WriteAllText(saveFile, jsonString);

            DisplayMessage(missionEditor.missionName.text + " saved to " + Application.persistentDataPath + "/Custom Missions/");

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

    public static void OpenWebAddress(string url)
    {
        Application.OpenURL(url);
        CloseAllMenus();
    }

}
