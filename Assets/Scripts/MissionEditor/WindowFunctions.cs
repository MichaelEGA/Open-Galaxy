using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public static class WindowFunctions
{
    #region Window Types

    //This selects the type of window to load according to the name
    public static void SelectWindowType(Window window)
    {
        if (window.windowType == "addnodes")
        {
            WindowFunctions.Draw_AddNode(window);
        }
        else if (window.windowType == "loadmission")
        {
            WindowFunctions.Draw_LoadMission(window);
        }
        else if (window.windowType == "mergemissions")
        {
            WindowFunctions.Draw_MergeMissions(window);
        }
        else if (window.windowType == "savemission")
        {
            WindowFunctions.Draw_SaveMissionAs(window);
        }
        else if (window.windowType == "savemissionas")
        {
            WindowFunctions.Draw_SaveMissionAs(window);
        }
        else if (window.windowType == "abouteditor")
        {
            WindowFunctions.Draw_AboutWindow(window);
        }

        //This closes all menus when a new window is being added
        MissionEditorFunctions.CloseAllMenus();
    }

    //This draws the add now window
    public static void Draw_AddNode(Window window)
    {
        DrawWindowBase(window, 250, 200);

        DrawText(window, "Add Event Node", 8, 5, -5, 12.5f, 90);

        DrawImageButton(window, 235, -6.5f, 10, 10, "cross", "DeleteWindow");

        DrawLineBreak(window, "#808080", 0, -20, 1, 250);

        List<string> buttonList = new List<string>();

        buttonList.Add("custom_node");
        buttonList.Add("loadscene");
        buttonList.Add("preload_loadasteroids");
        buttonList.Add("preload_loadplanet");
        buttonList.Add("preload_loadtiles");
        buttonList.Add("preload_loadmultipleshipsonground");
        buttonList.Add("preload_loadsingleship");
        buttonList.Add("preload_loadmultipleships");
        buttonList.Add("preload_loadmultipleshipsbyclassandallegiance");
        buttonList.Add("preload_setskybox");
        buttonList.Add("starteventseries");
        buttonList.Add("activatehyperspace");
        buttonList.Add("clearaioverride");
        buttonList.Add("displaydialoguebox");
        buttonList.Add("displaylargemessage");
        buttonList.Add("displaylocation");
        buttonList.Add("displaymessage");
        buttonList.Add("displaymissionbriefing");
        buttonList.Add("exitmission");
        buttonList.Add("ifshipisactive");
        buttonList.Add("ifshiphasbeenscanned");
        buttonList.Add("ifshiphasntbeenscanned");
        buttonList.Add("ifshipshullislessthan");
        buttonList.Add("ifshipislessthandistancetoothership");
        buttonList.Add("ifshipislessthandistancetowaypoint");
        buttonList.Add("iftypeofshipisactive");
        buttonList.Add("loadsingleship");
        buttonList.Add("loadsingleshipatdistanceandanglefromplayer");
        buttonList.Add("loadmultipleshipsonground");
        buttonList.Add("loadmultipleshipsbyclassandallegiance");
        buttonList.Add("loadmultipleships");
        buttonList.Add("setaioverride");
        buttonList.Add("setcargo");
        buttonList.Add("setdontattacklargeships");
        buttonList.Add("setmusicvolume");
        buttonList.Add("setmusictype");
        buttonList.Add("setshipallegiance");
        buttonList.Add("setshiptarget");
        buttonList.Add("setshiptargettoclosestenemy");
        buttonList.Add("setshiptoinvincible");
        buttonList.Add("setwaypoint");
        buttonList.Add("setweaponslock");

        List<string> functionList = new List<string>();

        foreach (string button in buttonList)
        {
            functionList.Add("SelectNodeTypeToLoad");
        }

        string[] buttons = buttonList.ToArray();
        string[] functions = functionList.ToArray();

        DrawScrollableButtons(window, 5, -25, 170f, 115, 10, 7, buttons, functions);

        DrawScrollableText(window, 127.5f, -25, 150f, 115, 7, "No Event Selected", 200f, "AddNodeTextBox");

        DrawTextButton(window, 127.5f, -182.5f, 10, 117.5f, "none", "Add Node", 7, "AddNode", TextAnchor.MiddleCenter);
    }

    //This draws the load mission window
    public static void Draw_LoadMission(Window window)
    {
        DrawWindowBase(window, 250, 100);

        DrawText(window, "Load Mission", 8, 5, -5, 12.5f, 90);

        DrawImageButton(window, 235, -6.5f, 10, 10, "cross", "DeleteWindow");

        DrawLineBreak(window, "#808080", 0, -20, 1, 250);

        string[] buttons = GetMissionList();

        List<string> functionList = new List<string>();

        foreach (string button in buttons)
        {
            functionList.Add("SelectMissionToLoad");
        }

        string[] functions = functionList.ToArray();

        DrawScrollableButtons(window, 5, -25, 50, 240, 10, 7, buttons, functions);

        DrawTextButton(window, 80, -80, 10, 90, "none", "Load Mission", 7, "LoadMission", TextAnchor.MiddleCenter);
    }

    //This draws the load mission window
    public static void Draw_MergeMissions(Window window)
    {
        DrawWindowBase(window, 250, 100);

        DrawText(window, "Merge Missions", 8, 5, -5, 12.5f, 90);

        DrawImageButton(window, 235, -6.5f, 10, 10, "cross", "DeleteWindow");

        DrawLineBreak(window, "#808080", 0, -20, 1, 250);

        string[] buttons = GetMissionList();

        List<string> functionList = new List<string>();

        foreach (string button in buttons)
        {
            functionList.Add("SelectMissionToLoad");
        }

        string[] functions = functionList.ToArray();

        DrawScrollableButtons(window, 5, -25, 50, 240, 10, 7, buttons, functions);

        DrawTextButton(window, 80, -80, 10, 90, "none", "Merge Mission", 7, "MergeMission", TextAnchor.MiddleCenter);
    }

    //This draws the save mission window
    public static void Draw_SaveMissionAs(Window window)
    {
        DrawWindowBase(window, 250, 100);

        DrawText(window, "Save Mission As", 8, 5, -5, 12.5f, 90);

        DrawImageButton(window, 235, -6.5f, 10, 10, "cross", "DeleteWindow");

        DrawLineBreak(window, "#808080", 0, -20, 1, 250);

        float drop = -35f;

        DrawInputFieldLarge(window, "Mission Name", "none", 7, 5, drop, 25, 240f, "MissionNameField");

        drop -= 15;

        DrawTextButton(window, 80, -80, 10, 90, "none", "Save Mission As", 7, "SaveMissionAs", TextAnchor.MiddleCenter);
    }

    //This draws the mission editor about window
    public static void Draw_AboutWindow(Window window)
    {
        DrawWindowBase(window, 250, 100);

        DrawText(window, "About OG Mission Editor", 8, 5, -5, 12.5f, 90);

        DrawImageButton(window, 235, -6.5f, 10, 10, "cross", "DeleteWindow");

        DrawLineBreak(window, "#808080", 0, -20, 1, 250);

        DrawText(window, "Made with Unity 2022.3.12f1", 8, 5, -25, 12.5f, 240);
    }

    #endregion

    #region Draw Window Functions

    //This draws the base node gameobject
    public static void DrawWindowBase(Window window, float width, float height)
    {
        //This sets up the node background
        window.rectTransform = window.gameObject.AddComponent<RectTransform>();

        window.background = window.gameObject.AddComponent<Image>();
        window.background.sprite = Resources.Load<Sprite>("Data/EditorAssets/NodeSprite_Dark");
        window.background.type = Image.Type.Sliced;
        window.background.pixelsPerUnitMultiplier = 40;
        window.rectTransform.sizeDelta = new Vector2(width, height);
        window.rectTransform.localPosition = new Vector2(window.xPos, window.yPos);
        window.rectTransform.localScale = new Vector3(1, 1, 1);
    }

    //This draws a title
    public static void DrawText(Window window, string title, int fontSize, float xPos, float yPos, float height, float width)
    {
        //This draws the input label
        GameObject titleGO = new GameObject();

        titleGO.transform.SetParent(window.rectTransform.transform);
        RectTransform rectTransform = titleGO.AddComponent<RectTransform>();
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.pivot = new Vector2(0, 1);
        rectTransform.anchoredPosition = new Vector2(xPos, yPos);
        rectTransform.sizeDelta = new Vector2(width, height);
        rectTransform.localScale = new Vector3(1, 1, 1);

        Text labelText = titleGO.AddComponent<Text>();
        labelText.supportRichText = false;
        labelText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        labelText.text = title;
        labelText.fontSize = fontSize;
        labelText.color = Color.white;
        labelText.alignment = TextAnchor.MiddleLeft;

        titleGO.name = "Title_" + title;
    }

    //This draws a line break
    public static void DrawLineBreak(Window window, string color, float xPos, float yPos, float height, float width)
    {
        GameObject lineBreakGO = new GameObject();

        lineBreakGO.transform.SetParent(window.rectTransform.transform);
        RectTransform rectTransform = lineBreakGO.AddComponent<RectTransform>();
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.pivot = new Vector2(0, 1);
        rectTransform.anchoredPosition = new Vector2(xPos, yPos);
        rectTransform.sizeDelta = new Vector2(width, height);
        rectTransform.localScale = new Vector3(1, 1, 1);

        Image lineBreakImage = lineBreakGO.AddComponent<Image>();

        if (!ColorUtility.TryParseHtmlString(color, out Color myColor))
        {
            lineBreakImage.color = Color.black;
        }
        else
        {
            lineBreakImage.color = myColor;
        }

        lineBreakGO.name = "linebreak";
    }

    //This draws a button and allocates a function
    public static void DrawImageButton(Window window, float xPos, float yPos, float height, float width, string imageName, string functionType, bool parentToNode = true, Transform differentTransform = null)
    {
        GameObject buttonGO = new GameObject();

        if (parentToNode == true)
        {
            buttonGO.transform.SetParent(window.rectTransform.transform);
        }
        else
        {
            buttonGO.transform.SetParent(differentTransform);
        }
       
        RectTransform rectTransform = buttonGO.AddComponent<RectTransform>();
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.pivot = new Vector2(0, 1);
        rectTransform.anchoredPosition = new Vector2(xPos, yPos);
        rectTransform.sizeDelta = new Vector2(width, height);
        rectTransform.localScale = new Vector3(1, 1, 1);

        Image buttonImage = buttonGO.AddComponent<Image>();
        buttonImage.sprite = Resources.Load<Sprite>("Data/EditorAssets/" + imageName);

        Button button = buttonGO.AddComponent<Button>();
        button.image = buttonImage;

        if (functionType == "ExitMissionEditor")
        {
            button.onClick.AddListener(() => { MissionEditorFunctions.ExitMissionEditor(); });
        }  
        else if (functionType == "DeleteWindow")
        {
            button.onClick.AddListener(() => { DeleteWindow(window); });
        }

        buttonGO.name = "button_" + functionType;
    }

    //This draws a button and allocates a function
    public static void DrawTextButton(Window window, float xPos, float yPos, float height, float width, string buttonImageName, string buttonText, int fontSize, string functionType, TextAnchor alignement = TextAnchor.MiddleCenter, bool parentToNode = true, Transform differentTransform = null)
    {
        GameObject buttonGO = new GameObject();
        GameObject buttonTextGO = new GameObject();

        if (parentToNode == true)
        {
            buttonGO.transform.SetParent(window.rectTransform.transform);
        }
        else
        {
            buttonGO.transform.SetParent(differentTransform);
        }

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

        Image buttonImage = buttonGO.AddComponent<Image>();
        buttonImage.type = Image.Type.Sliced;
        buttonImage.pixelsPerUnitMultiplier = 30;

        if (buttonImageName != "none" & buttonImageName != "")
        {
            buttonImage.sprite = Resources.Load<Sprite>("Data/EditorAssets/" + buttonImageName);
        }
        else
        {
            Color color = Color.black;

            if (ColorUtility.TryParseHtmlString("#404040", out color))
            {
                buttonImage.color = color;
            }
        }

        Button button = buttonGO.AddComponent<Button>();
        button.image = buttonImage;

        Text text = buttonTextGO.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = fontSize;
        text.text = buttonText;
        text.alignment = alignement;

        if (functionType == "ExitMissionEditor")
        {
            button.onClick.AddListener(() => { MissionEditorFunctions.ExitMissionEditor(); });
        }
        else if (functionType == "SelectNodeTypeToLoad")
        {
            button.onClick.AddListener(() => { MissionEditorFunctions.SelectNodeType(buttonText); });
        }
        else if (functionType == "AddNode")
        {
            button.onClick.AddListener(() => { MissionEditorFunctions.AddSelectedNodeType(); });
        }
        else if (functionType == "SelectMissionToLoad")
        {
            button.onClick.AddListener(() => { MissionEditorFunctions.SelectMission(buttonText); });
        }
        else if (functionType == "LoadMission")
        {
            button.onClick.AddListener(() => { MissionEditorFunctions.LoadMission(window); });
        }
        else if (functionType == "MergeMission")
        {
            button.onClick.AddListener(() => { MissionEditorFunctions.MergeMissions(window); });
        }
        else if (functionType == "SaveMission")
        {
            button.onClick.AddListener(() => { MissionEditorFunctions.SaveMission(window); });
        }
        else if (functionType == "SaveMissionAs")
        {
            button.onClick.AddListener(() => { MissionEditorFunctions.SaveMission(window); });
        }
        else if (functionType == "DeleteWindow")
        {
            button.onClick.AddListener(() => { DeleteWindow(window); });
        }

        buttonGO.name = "button_" + functionType;
    }

    public static void DrawScrollableButtons(Window window, float xPos, float yPos, float height, float width, float buttonHeight, int fontSize, string[] buttons, string[] functions)
    {
        GameObject baseGO = new GameObject();
        GameObject viewportGO = new GameObject();
        GameObject contentGO = new GameObject();

        baseGO.transform.SetParent(window.transform);
        viewportGO.transform.SetParent(baseGO.transform);
        contentGO.transform.SetParent(viewportGO.transform);

        RectTransform baseRectTransform = baseGO.AddComponent<RectTransform>();
        RectTransform viewportRectTransform = viewportGO.AddComponent<RectTransform>();
        RectTransform contentRectTransform = contentGO.AddComponent<RectTransform>();

        baseRectTransform.name = "Scrollable_Buttons";
        viewportRectTransform.name = "Viewport";
        contentRectTransform.name = "ContentRect";

        baseRectTransform.anchorMax = new Vector2(0, 1);
        baseRectTransform.anchorMin = new Vector2(0, 1);
        baseRectTransform.pivot = new Vector2(0, 1);
        baseRectTransform.anchoredPosition = new Vector2(xPos, yPos);
        baseRectTransform.sizeDelta = new Vector2(width, height);
        baseRectTransform.localScale = new Vector3(1, 1, 1);

        viewportRectTransform.anchorMax = new Vector2(0, 1);
        viewportRectTransform.anchorMin = new Vector2(0, 1);
        viewportRectTransform.pivot = new Vector2(0, 1);
        viewportRectTransform.anchoredPosition = new Vector2(0, 0);
        viewportRectTransform.sizeDelta = new Vector2(width, height);
        viewportRectTransform.localScale = new Vector3(1, 1, 1);

        float contentRectHeight = buttonHeight * buttons.Length;

        contentRectTransform.anchorMax = new Vector2(0, 1);
        contentRectTransform.anchorMin = new Vector2(0, 1);
        contentRectTransform.pivot = new Vector2(0, 1);
        contentRectTransform.anchoredPosition = new Vector2(0, 0);
        contentRectTransform.sizeDelta = new Vector2(width, contentRectHeight);
        contentRectTransform.localScale = new Vector3(1, 1, 1);

        Image maskImage = viewportGO.AddComponent<Image>();
        maskImage.type = Image.Type.Sliced;
        maskImage.pixelsPerUnitMultiplier = 30;

        Color color = Color.black;

        if (ColorUtility.TryParseHtmlString("#404040", out color))
        {
            maskImage.color = color;
        }

        Mask mask = viewportGO.AddComponent<Mask>();
        
        ScrollRect scrollRect = baseGO.AddComponent<ScrollRect>();
        scrollRect.content = contentRectTransform;
        scrollRect.viewport = viewportRectTransform;
        scrollRect.horizontal = false;
        scrollRect.inertia = false;
        scrollRect.elasticity = 0;

        float buttonDrop = 0;
        int i = 0;

        foreach (string button in buttons)
        {
            DrawTextButton(window, 0, buttonDrop, buttonHeight, width, "none", button, fontSize, functions[i], TextAnchor.MiddleLeft, false, contentGO.transform);
            buttonDrop -= buttonHeight;
            i++;
        }

        DrawVerticalScrollBar(baseGO.transform, scrollRect, 5);
    }

    public static void DrawScrollableText(Window window, float xPos, float yPos, float height, float width, int fontSize, string text, float textBoxHeight, string textBoxName = "textbox")
    {
        //This draws the scrollview
        GameObject baseGO = new GameObject();
        GameObject viewportGO = new GameObject();
        GameObject contentGO = new GameObject();

        baseGO.transform.SetParent(window.transform);
        viewportGO.transform.SetParent(baseGO.transform);
        contentGO.transform.SetParent(viewportGO.transform);

        RectTransform baseRectTransform = baseGO.AddComponent<RectTransform>();
        RectTransform viewportRectTransform = viewportGO.AddComponent<RectTransform>();
        RectTransform contentRectTransform = contentGO.AddComponent<RectTransform>();

        baseRectTransform.name = "Scrollable_Buttons";
        viewportRectTransform.name = "Viewport";
        contentRectTransform.name = "ContentRect";

        baseRectTransform.anchorMax = new Vector2(0, 1);
        baseRectTransform.anchorMin = new Vector2(0, 1);
        baseRectTransform.pivot = new Vector2(0, 1);
        baseRectTransform.anchoredPosition = new Vector2(xPos, yPos);
        baseRectTransform.sizeDelta = new Vector2(width, height);
        baseRectTransform.localScale = new Vector3(1, 1, 1);

        viewportRectTransform.anchorMax = new Vector2(0, 1);
        viewportRectTransform.anchorMin = new Vector2(0, 1);
        viewportRectTransform.pivot = new Vector2(0, 1);
        viewportRectTransform.anchoredPosition = new Vector2(0, 0);
        viewportRectTransform.sizeDelta = new Vector2(width, height);
        viewportRectTransform.localScale = new Vector3(1, 1, 1);

        contentRectTransform.anchorMax = new Vector2(0, 1);
        contentRectTransform.anchorMin = new Vector2(0, 1);
        contentRectTransform.pivot = new Vector2(0, 1);
        contentRectTransform.anchoredPosition = new Vector2(0, 0);
        contentRectTransform.sizeDelta = new Vector2(width, textBoxHeight);
        contentRectTransform.localScale = new Vector3(1, 1, 1);

        Image maskImage = viewportGO.AddComponent<Image>();
        maskImage.type = Image.Type.Sliced;
        maskImage.pixelsPerUnitMultiplier = 30;

        Color color = Color.black;

        if (ColorUtility.TryParseHtmlString("#404040", out color))
        {
            maskImage.color = color;
        }

        Mask mask = viewportGO.AddComponent<Mask>();

        ScrollRect scrollRect = baseGO.AddComponent<ScrollRect>();
        scrollRect.content = contentRectTransform;
        scrollRect.viewport = viewportRectTransform;
        scrollRect.horizontal = false;
        scrollRect.inertia = false;
        scrollRect.elasticity = 0;

        //This draws the text box
        GameObject textBoxGO = new GameObject();
        textBoxGO.name = textBoxName;

        textBoxGO.transform.SetParent(contentGO.transform);
        RectTransform rectTransform = textBoxGO.AddComponent<RectTransform>();
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.pivot = new Vector2(0, 1);
        rectTransform.anchoredPosition = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(width - 5, textBoxHeight);
        rectTransform.localScale = new Vector3(1, 1, 1);

        Text textBoxText = textBoxGO.AddComponent<Text>();
        textBoxText.supportRichText = false;
        textBoxText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        textBoxText.text = text;
        textBoxText.fontSize = fontSize;
        textBoxText.color = Color.white;
        textBoxText.alignment = TextAnchor.UpperLeft;

        DrawVerticalScrollBar(baseGO.transform, scrollRect, 5);
    }

    public static void DrawVerticalScrollBar(Transform parent, ScrollRect scrollRect, float width)
    {
        GameObject scrollbarVertical = new GameObject();
        GameObject slidingArea = new GameObject();
        GameObject handle = new GameObject();

        scrollbarVertical.transform.SetParent(parent.transform);
        slidingArea.transform.SetParent(scrollbarVertical.transform);
        handle.transform.SetParent(slidingArea.transform);

        scrollbarVertical.name = "Scrollbar Vertical";
        slidingArea.name = "Sliding Area";
        handle.name = "Handle";

        RectTransform scrollBarRect = scrollbarVertical.AddComponent<RectTransform>();
        RectTransform slidingAreaRect = slidingArea.AddComponent<RectTransform>();
        RectTransform handleRect = handle.AddComponent<RectTransform>();

        scrollBarRect.anchorMin = new Vector2(1, 0);
        scrollBarRect.anchorMax = new Vector2(1, 1);
        scrollBarRect.pivot = new Vector2(1, 1);
        scrollBarRect.anchoredPosition = new Vector2(0, 0);
        scrollBarRect.sizeDelta = new Vector2(width, 0);
        scrollBarRect.localScale = new Vector3(1, 1, 1);

        slidingAreaRect.anchorMin = new Vector2(0, 0);
        slidingAreaRect.anchorMax = new Vector2(1, 1);
        slidingAreaRect.pivot = new Vector2(0.5f, 0.5f);
        slidingAreaRect.anchoredPosition = new Vector2(0, 0);
        slidingAreaRect.sizeDelta = new Vector2(10, 10);
        slidingAreaRect.localScale = new Vector3(1, 1, 1);

        handleRect.anchorMin = new Vector2(0, 0);
        handleRect.anchorMax = new Vector2(0, 0);
        handleRect.pivot = new Vector2(0.5f, 0.5f);
        handleRect.anchoredPosition = new Vector2(0, 0);
        handleRect.sizeDelta = new Vector2(-10, -10);
        handleRect.localScale = new Vector3(1, 1, 1);

        Image scrollbarImage = scrollbarVertical.AddComponent<Image>();
        Image handleImage = handle.AddComponent<Image>();

        scrollbarImage.sprite = Resources.Load<Sprite>("Data/EditorAssets/NodeSprite_Dark");
        scrollbarImage.type = Image.Type.Sliced;
        scrollbarImage.pixelsPerUnitMultiplier = 40;

        handleImage.sprite = Resources.Load<Sprite>("Data/EditorAssets/NodeSprite_Grey");
        handleImage.type = Image.Type.Sliced;
        handleImage.pixelsPerUnitMultiplier = 40;

        scrollRect.verticalScrollbar = scrollbarVertical.AddComponent<Scrollbar>();
        scrollRect.verticalScrollbar.direction = Scrollbar.Direction.BottomToTop;
        scrollRect.verticalScrollbar.targetGraphic = handleImage;
        scrollRect.verticalScrollbar.handleRect = handleRect;
    }

    //This draws a text box
    public static void DrawTextBox(Window window, float xPos, float yPos, float height, float width, string backgroundImageName, string textBoxText, int fontSize, TextAnchor alignement = TextAnchor.MiddleCenter, bool parentToNode = true, Transform differentTransform = null, string name = "none")
    {
        GameObject textboxGO = new GameObject();
        GameObject textBoxTextGO = new GameObject();

        if (parentToNode == true)
        {
            textboxGO.transform.SetParent(window.rectTransform.transform);
        }
        else
        {
            textboxGO.transform.SetParent(differentTransform);
        }

        textBoxTextGO.transform.SetParent(textboxGO.transform);

        RectTransform rectTransform1 = textboxGO.AddComponent<RectTransform>();
        rectTransform1.anchorMax = new Vector2(0, 1);
        rectTransform1.anchorMin = new Vector2(0, 1);
        rectTransform1.pivot = new Vector2(0, 1);
        rectTransform1.anchoredPosition = new Vector2(xPos, yPos);
        rectTransform1.sizeDelta = new Vector2(width, height);
        rectTransform1.localScale = new Vector3(1, 1, 1);

        RectTransform rectTransform2 = textBoxTextGO.AddComponent<RectTransform>();
        rectTransform2.anchorMax = new Vector2(0, 1);
        rectTransform2.anchorMin = new Vector2(0, 1);
        rectTransform2.pivot = new Vector2(0, 1);
        rectTransform2.anchoredPosition = new Vector2(1, -1);
        rectTransform2.sizeDelta = new Vector2(width - 2, height -2);
        rectTransform2.localScale = new Vector3(1, 1, 1);

        Image backgroundImage = textboxGO.AddComponent<Image>();
        backgroundImage.type = Image.Type.Sliced;
        backgroundImage.pixelsPerUnitMultiplier = 30;

        if (backgroundImageName != "none" & backgroundImageName != "")
        {
            backgroundImage.sprite = Resources.Load<Sprite>("Data/EditorAssets/" + backgroundImageName);
        }
        else
        {
            Color color = Color.black;

            if (ColorUtility.TryParseHtmlString("#404040", out color))
            {
                backgroundImage.color = color;
            }
        }

        Text text = textBoxTextGO.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = fontSize;
        text.text = textBoxText;
        text.alignment = alignement;

        if (name != "none")
        {
            textboxGO.name = name;
        }
        else
        {
            textboxGO.name = "textbox";
        }

    }

    public static Text DrawInputField(Window window, string label, string startvalue, int fontSize, float xPos, float yPos, float height, float width, float gap = 10, string name = "none")
    {
        float halfwidth = (width - gap) / 2f;
        float shiftedXPosition = xPos + halfwidth + (gap / 2f);

        //This draws the input label
        GameObject labelGO = new GameObject();

        labelGO.transform.SetParent(window.rectTransform.transform);
        RectTransform rectTransform2 = labelGO.AddComponent<RectTransform>();
        rectTransform2.anchorMax = new Vector2(0, 1);
        rectTransform2.anchorMin = new Vector2(0, 1);
        rectTransform2.pivot = new Vector2(0, 1);
        rectTransform2.anchoredPosition = new Vector2(xPos, yPos);
        rectTransform2.sizeDelta = new Vector2(halfwidth, height);
        rectTransform2.localScale = new Vector3(1, 1, 1);

        Text labelText = labelGO.AddComponent<Text>();
        labelText.supportRichText = false;
        labelText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        labelText.text = label;
        labelText.fontSize = fontSize;
        labelText.color = Color.white;
        labelText.alignment = TextAnchor.MiddleLeft;

        //This draws the background of the input field
        GameObject inputFieldBackgroundGO = new GameObject();

        inputFieldBackgroundGO.transform.SetParent(window.rectTransform.transform);
        RectTransform rectTransform3 = inputFieldBackgroundGO.AddComponent<RectTransform>();
        rectTransform3.anchorMax = new Vector2(0, 1);
        rectTransform3.anchorMin = new Vector2(0, 1);
        rectTransform3.pivot = new Vector2(0, 1);
        rectTransform3.anchoredPosition = new Vector2(shiftedXPosition, yPos);
        rectTransform3.sizeDelta = new Vector2(halfwidth, height);
        rectTransform3.localScale = new Vector3(1, 1, 1);

        Image inputFieldImage = inputFieldBackgroundGO.AddComponent<Image>();
        inputFieldImage.sprite = Resources.Load<Sprite>("Data/EditorAssets/NodeSprite_Light");
        inputFieldImage.type = Image.Type.Sliced;
        inputFieldImage.pixelsPerUnitMultiplier = 30;

        //This draws the input field
        GameObject inputFieldGO = new GameObject();

        inputFieldGO.transform.SetParent(window.rectTransform.transform);
        RectTransform rectTransform = inputFieldGO.AddComponent<RectTransform>();
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.pivot = new Vector2(0, 1);
        rectTransform.anchoredPosition = new Vector2(shiftedXPosition + 1, yPos);
        rectTransform.sizeDelta = new Vector2(halfwidth - 2, height);
        rectTransform.localScale = new Vector3(1, 1, 1);

        Text inputFieldText = inputFieldGO.AddComponent<Text>();
        inputFieldText.supportRichText = false;
        inputFieldText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        inputFieldText.fontSize = fontSize;
        inputFieldText.color = Color.gray;
        inputFieldText.alignment = TextAnchor.MiddleLeft;
        inputFieldText.verticalOverflow = VerticalWrapMode.Overflow;
        inputFieldText.horizontalOverflow = HorizontalWrapMode.Overflow;

        InputField inputField = inputFieldGO.AddComponent<InputField>();
        inputField.textComponent = inputFieldText;
        inputField.lineType = InputField.LineType.MultiLineNewline;
        inputField.characterLimit = 2000;
        inputField.text = startvalue;

        GameObject transitionTextGO = new GameObject();

        transitionTextGO.transform.SetParent(window.rectTransform.transform);
        RectTransform rectTransform4 = transitionTextGO.AddComponent<RectTransform>();
        rectTransform4.anchorMax = new Vector2(0, 1);
        rectTransform4.anchorMin = new Vector2(0, 1);
        rectTransform4.pivot = new Vector2(0, 1);
        rectTransform4.anchoredPosition = new Vector2(xPos, yPos);
        rectTransform4.sizeDelta = new Vector2(halfwidth, height);
        rectTransform4.localScale = new Vector3(1, 1, 1);

        Text transitionText = transitionTextGO.AddComponent<Text>();
        transitionText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        transitionText.horizontalOverflow = HorizontalWrapMode.Overflow;
        transitionText.verticalOverflow = VerticalWrapMode.Overflow;
        Color transparent = Color.black;
        transparent.a = 0;
        transitionText.color = transparent;
        InputFieldToText inputFieldToText = transitionTextGO.AddComponent<InputFieldToText>();
        inputFieldToText.text = transitionText;
        inputFieldToText.inputField = inputField;

        if (name != "none")
        {
            transitionTextGO.name = name;
        }
        else
        {
            transitionTextGO.name = "inputfield";
        }

        //If this is not run the caret will display behind the text box...
        ModifyCaretPosition(window);

        return transitionText;
    }

    //This draws an empty input field
    public static Text DrawInputFieldLarge(Window window, string label, string startvalue, int fontSize, float xPos, float yPos, float height, float width, string name = "none")
    {
        float shiftedYPosition = yPos - 12.5f;
        float modifiedHeight = height - 12.5f;

        //This draws the input label
        GameObject labelGO = new GameObject();

        labelGO.transform.SetParent(window.rectTransform.transform);
        RectTransform rectTransform2 = labelGO.AddComponent<RectTransform>();
        rectTransform2.anchorMax = new Vector2(0, 1);
        rectTransform2.anchorMin = new Vector2(0, 1);
        rectTransform2.pivot = new Vector2(0, 1);
        rectTransform2.anchoredPosition = new Vector2(xPos, yPos);
        rectTransform2.sizeDelta = new Vector2(width, 12);
        rectTransform2.localScale = new Vector3(1, 1, 1);

        Text labelText = labelGO.AddComponent<Text>();
        labelText.supportRichText = false;
        labelText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        labelText.text = label;
        labelText.fontSize = fontSize;
        labelText.color = Color.white;
        labelText.alignment = TextAnchor.MiddleLeft;

        //This draws the background of the input field
        GameObject inputFieldBackgroundGO = new GameObject();

        inputFieldBackgroundGO.transform.SetParent(window.rectTransform.transform);
        RectTransform rectTransform3 = inputFieldBackgroundGO.AddComponent<RectTransform>();
        rectTransform3.anchorMax = new Vector2(0, 1);
        rectTransform3.anchorMin = new Vector2(0, 1);
        rectTransform3.pivot = new Vector2(0, 1);
        rectTransform3.anchoredPosition = new Vector2(xPos, shiftedYPosition);
        rectTransform3.sizeDelta = new Vector2(width, modifiedHeight);
        rectTransform3.localScale = new Vector3(1, 1, 1);

        Image inputFieldImage = inputFieldBackgroundGO.AddComponent<Image>();
        inputFieldImage.sprite = Resources.Load<Sprite>("Data/EditorAssets/NodeSprite_Light");
        inputFieldImage.type = Image.Type.Sliced;
        inputFieldImage.pixelsPerUnitMultiplier = 30;

        //This draws the input field
        GameObject inputFieldGO = new GameObject();

        inputFieldGO.transform.SetParent(window.rectTransform.transform);

        RectTransform rectTransform = inputFieldGO.AddComponent<RectTransform>();
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.pivot = new Vector2(0, 1);
        rectTransform.anchoredPosition = new Vector2(xPos + 1, shiftedYPosition - 1);
        rectTransform.sizeDelta = new Vector2(width, modifiedHeight - 2);
        rectTransform.localScale = new Vector3(1, 1, 1);

        Text inputFieldText = inputFieldGO.AddComponent<Text>();
        inputFieldText.supportRichText = false;
        inputFieldText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        inputFieldText.fontSize = fontSize;
        inputFieldText.color = Color.gray;
        inputFieldText.alignment = TextAnchor.UpperLeft;
        inputFieldText.verticalOverflow = VerticalWrapMode.Overflow;
        inputFieldText.horizontalOverflow = HorizontalWrapMode.Overflow;

        InputField inputField = inputFieldGO.AddComponent<InputField>();
        inputField.textComponent = inputFieldText;
        inputField.lineType = InputField.LineType.MultiLineNewline;
        inputField.characterLimit = 2000;
        inputField.caretColor = Color.gray;
        inputField.text = startvalue;

        GameObject transitionTextGO = new GameObject();

        transitionTextGO.transform.SetParent(inputFieldGO.transform);
        RectTransform rectTransform4 = transitionTextGO.AddComponent<RectTransform>();
        rectTransform4.anchorMax = new Vector2(0, 1);
        rectTransform4.anchorMin = new Vector2(0, 1);
        rectTransform4.pivot = new Vector2(0, 1);
        rectTransform4.anchoredPosition = new Vector2(xPos, shiftedYPosition);
        rectTransform4.sizeDelta = new Vector2(width, modifiedHeight);
        rectTransform4.localScale = new Vector3(1, 1, 1);

        Text transitionText = transitionTextGO.AddComponent<Text>();
        transitionText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        transitionText.horizontalOverflow = HorizontalWrapMode.Overflow;
        transitionText.verticalOverflow = VerticalWrapMode.Overflow;
        Color transparent = Color.black;
        transparent.a = 0;
        transitionText.color = transparent;
        InputFieldToText inputFieldToText = transitionTextGO.AddComponent<InputFieldToText>();
        inputFieldToText.text = transitionText;
        inputFieldToText.inputField = inputField;

        if (name != "none")
        {
            transitionTextGO.name = name;
        }
        else
        {
            transitionTextGO.name = "inputfieldlarge";
        }

        //If this is not run the caret will display behind the text box...
        ModifyCaretPosition(window);

        return transitionText;
    }

    //This modifies the caret position to ensure its on top
    public static void ModifyCaretPosition(Window window)
    {
        Transform[] carets = GameObjectUtils.FindAllChildTransformsContaining(window.transform, "Caret");

        int childNumber = window.transform.childCount;

        foreach (Transform caret in carets)
        {
            caret.SetAsLastSibling();
        }
    }

    #endregion

    #region Misc Windows Functions

    public static void DeleteWindow(Window window)
    {
        GameObject.Destroy(window.gameObject);
    }

    public static string[] GetMissionList()
    {
        List<string> buttonList = new List<string>();

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
                string missionName = file.Name;
                buttonList.Add(missionName);
            }
        }

        return buttonList.ToArray();
    }

    #endregion
}
