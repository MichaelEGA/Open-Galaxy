using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class WindowFunctions
{
    #region Window Types

    //This selects the type of window to load according to the name
    public static void SelectWindowType(Window window)
    {
        if (window.windowType == "addnodes")
        {
            WindowFunctions.Draw_AddEvent(window);
        }
        else if (window.windowType == "displaylocation")
        {
            WindowFunctions.Draw_DisplayLocation(window);
        }
        else if (window.windowType == "displayshipinformation")
        {
            WindowFunctions.Draw_ShipInformation(window);
        }
        else if (window.windowType == "abouteditor")
        {
            WindowFunctions.Draw_AboutWindow(window);
        }

        //This closes all menus when a new window is being added
        MissionEditorFunctions.CloseAllMenus();
    }

    //This draws the add now window
    public static void Draw_AddEvent(Window window)
    {
        window.transform.name = "window_addevent";

        DrawWindowBase(window, 250, 200);

        DrawText(window, "Add Event Node", 8, 5, -5, 12.5f, 90);

        DrawImageButton(window, 235, -6.5f, 10, 10, "cross", "DeleteWindow");

        DrawLineBreak(window, "#808080", 0, -20, 1, 250);

        List<string> buttonList = new List<string>();

        buttonList.Add("activatedocking");
        buttonList.Add("activatehyperspace");
        buttonList.Add("activatewaypointmarker");
        buttonList.Add("addaitagtolargeship");
        buttonList.Add("addaitagtosmallship");
        buttonList.Add("campaigninformation");
        buttonList.Add("changelocation");
        buttonList.Add("changelocationfade");
        buttonList.Add("createlocation");
        buttonList.Add("custom_node");
        buttonList.Add("deactivateship");
        buttonList.Add("displayhint");
        buttonList.Add("displaytitle");
        buttonList.Add("displaymessage");
        buttonList.Add("displaymissionbriefing");
        buttonList.Add("exitmission");
        buttonList.Add("exitanddisplaynextmissionmenu");
        buttonList.Add("ifobjectiveisactive");
        buttonList.Add("ifshipisactive");
        buttonList.Add("ifshiphasbeendisabled");
        buttonList.Add("ifshiphasntbeendisabled");
        buttonList.Add("ifshiphasbeenscanned");
        buttonList.Add("ifshiphasntbeenscanned");
        buttonList.Add("ifshipshullislessthan");
        buttonList.Add("ifshipislessthandistancetoothership");
        buttonList.Add("ifshipislessthandistancetowaypoint");
        buttonList.Add("ifshipofallegianceisactive");
        buttonList.Add("ifshipssystemsarelessthan");
        buttonList.Add("loadmultipleships");
        buttonList.Add("loadmultipleshipsfromhangar");
        buttonList.Add("loadmultipleshipsonground");
        buttonList.Add("loadsingleship");
        buttonList.Add("loadsingleshipatdistanceandanglefromplayer");
        buttonList.Add("loadsingleshiponground");
        buttonList.Add("pausesequence");
        buttonList.Add("playmusictrack");
        buttonList.Add("preload_loadasteroids");
        buttonList.Add("preload_loadenvironment");
        buttonList.Add("preload_loadplanet");
        buttonList.Add("preload_loadmultipleships");
        buttonList.Add("preload_loadmultipleshipsonground");
        buttonList.Add("preload_loadsingleship");
        buttonList.Add("preload_loadsingleshiponground");
        buttonList.Add("preload_setfogdistanceandcolor");
        buttonList.Add("preload_sethudcolour");
        buttonList.Add("preload_setlighting");
        buttonList.Add("preload_setsceneradius");
        buttonList.Add("preload_setskybox");
        buttonList.Add("setcargo");
        buttonList.Add("setcontrollock");
        buttonList.Add("setfollowtarget");
        buttonList.Add("setobjective");
        buttonList.Add("setdontattacklargeships");
        buttonList.Add("setshipallegiance");
        buttonList.Add("setshiplevels");
        buttonList.Add("setshipstats");
        buttonList.Add("setshiptarget");
        buttonList.Add("setshiptargettoclosestenemy");
        buttonList.Add("setshiptocannotbedisabled");
        buttonList.Add("setshiptoinvincible");
        buttonList.Add("settorpedoes");
        buttonList.Add("setwaypoint");
        buttonList.Add("setwaypointtoship");
        buttonList.Add("spliteventseries");
        buttonList.Add("starteventseries");

        List<string> functionList = new List<string>();

        foreach (string button in buttonList)
        {
            functionList.Add("SelectNodeTypeToLoad");
        }

        string[] buttons = buttonList.ToArray();
        string[] functions = functionList.ToArray();

        DrawScrollableButtons(window, 5, -29, 146f, 115, 10, 7, buttons, functions);

        DrawScrollableText(window, 127.5f, -29, 146f, 115, 7, "No Event Selected", 200f, "AddNodeTextBox");

        DrawTextButton(window, 127.5f, -182.5f, 10, 117.5f, "none", "Add Node", 7, "AddNode", TextAnchor.MiddleCenter);
    }

    //This draws the display ship information window
    public static void Draw_ShipInformation(Window window)
    {
        window.transform.name = "window_shipinformation";

        DrawWindowBase(window, 250, 200);

        DrawText(window, "Ship Information", 8, 5, -5, 12.5f, 90);

        DrawImageButton(window, 235, -6.5f, 10, 10, "cross", "DeleteWindow");

        DrawLineBreak(window, "#808080", 0, -20, 1, 250);

        List<string> buttonList = new List<string>();
        List<string> options = new List<string>();

        options.Add("all");

        TextAsset shipTypesFile = Resources.Load(OGGetAddress.files + "ShipTypes") as TextAsset;
        ShipTypes shipTypes = JsonUtility.FromJson<ShipTypes>(shipTypesFile.text);

        string firstShip = "xwing";
        bool firstShipCaptured = false;

        foreach (ShipType shipType in shipTypes.shipTypeData)
        {
            //This adds the ship to the list
            buttonList.Add(shipType.type);

            //This captures the name of the first ship to display
            if (firstShipCaptured == false)
            {
                firstShip = shipType.type;
                firstShipCaptured = true;
            }

            //This creates the list to sort the ships
            bool addString = true;

            foreach (string sortString in options)
            {
                if (sortString == shipType.allegiance)
                {
                    addString = false;
                }
            }

            if (addString == true)
            {
                options.Add(shipType.allegiance);
            }

            addString = true;

            foreach (string sortString in options)
            {
                if (sortString == shipType.scriptType)
                {
                    addString = false;
                }
            }

            if (addString == true)
            {
                options.Add(shipType.scriptType);
            }
        }

        List<string> functionList = new List<string>();

        foreach (string button in buttonList)
        {
            functionList.Add("DisplayShipInformation");
        }

        string[] buttons = buttonList.ToArray();
        string[] functions = functionList.ToArray();

        DrawScrollableButtons(window, 5, -29, 161f, 115, 10, 7, buttons, functions, "scollableshiplist");

        DrawRawImage(window, 147.5f, -29, 73f, 73f, "assetpreview");

        DrawScrollableText(window, 127.5f, -102, 88f, 115, 7, "No Event Selected", 225f, "ShipInformationTextBox");

        DrawDropDownMenuSansLabel(window, options, "ships", "all", 7, 151f, -7.4f, 10, 65);

        DisplayShipInformation(firstShip);
    }

    //This draws a window that displays the location of all relevant nodes on the map
    public static void Draw_DisplayLocation(Window window)
    {
        window.transform.name = "window_displaylocation";

        DrawWindowBase(window, 250, 208);

        DrawText(window, "Display Location", 8, 5, -5, 12.5f, 90);

        DrawImageButton(window, 235, -6.5f, 10, 10, "cross", "DeleteWindow");

        DrawLineBreak(window, "#808080", 0, -20, 1, 250);

        float drop = -29;

        DrawRawImage(window, 5, drop, 170f, 170, "background"); //Creating the background image first ensures that it is behind the grid

        DrawRawImage(window, 5, drop, 170f, 170, "grid");

        DrawText(window, "View Angle", 7, 180, drop, 10, 65f, true, null, "topleft", TextAnchor.MiddleCenter);

        drop -= 12.5f;

        List<string> options1 = new List<string>();
        options1.Add("top");
        options1.Add("side");

        DrawDropDownMenuSansLabel(window, options1, "view", "top", 7, 180f, drop, 10, 65);

        drop -= 12.5f;

        DrawText(window, "Display Nodes", 7, 180, drop, 10, 65f, true, null, "topleft", TextAnchor.MiddleCenter);

        drop -= 12.5f;

        List<string> options3 = new List<string>();
        options3.Add("all");
        options3.Add("selected");

        DrawDropDownMenuSansLabel(window, options3, "nodes", "all", 7, 180f, drop, 10, 65);

        drop -= 12.5f;

        DrawText(window, "Background", 7, 180, drop, 10, 65f, true, null, "topleft", TextAnchor.MiddleCenter);

        drop -= 12.5f;

        DrawTextButton(window, 180f, drop, 10, 65f, "none", "Update", 7, "UpdateDisplay", TextAnchor.MiddleCenter);

        drop -= 12.5f;

        DrawTextButton(window, 180f, drop, 10, 65f, "none", "Clear", 7, "ClearDisplay", TextAnchor.MiddleCenter);

        //This updates the display for the first time
        UpdateDisplay();
    }

    //This draws the mission editor about window
    public static void Draw_AboutWindow(Window window)
    {
        window.transform.name = "window_aboutwindow";

        DrawWindowBase(window, 250, 100);

        DrawText(window, "About OG Mission Editor", 8, 5, -5, 12.5f, 90);

        DrawImageButton(window, 235, -6.5f, 10, 10, "cross", "DeleteWindow");

        DrawLineBreak(window, "#808080", 0, -20, 1, 250);

        DrawText(window, "Version " + UnityEngine.Application.version + " made with Unity " + UnityEngine.Application.unityVersion, 8, 5, -25, 12.5f, 240);
    }

    #endregion

    #region Draw Window Functions

    //This draws the base window rect transforms
    public static void DrawWindowBase(Window window, float width, float height)
    {
        //This sets up the base recttransform
        window.rectTransform = window.gameObject.AddComponent<RectTransform>();

        window.rectTransform.sizeDelta = new Vector2(width, height);
        window.rectTransform.localPosition = new Vector2(window.xPos, window.yPos);
        window.rectTransform.localScale = new Vector3(1, 1, 1);

        //This sets up the node highlight
        GameObject nodeHighlight = new GameObject();
        nodeHighlight.name = "windowHighlight";
        nodeHighlight.transform.SetParent(window.gameObject.transform);

        Image highlight = nodeHighlight.AddComponent<Image>();
        highlight.sprite = Resources.Load<Sprite>(OGGetAddress.missioneditor + "NodeSprite_Light");
        highlight.type = Image.Type.Sliced;
        highlight.pixelsPerUnitMultiplier = 5;
        highlight.color = new Color(90f / 250f, 90f / 250f, 90f / 250f);

        window.highlightRect = nodeHighlight.GetComponent<RectTransform>();
        window.highlightRect.sizeDelta = new Vector2(width + 1, height + 1);
        window.highlightRect.localPosition = new Vector2(0, 0);
        window.highlightRect.localScale = new Vector3(1, 1, 1);

        //This sets up the node background
        GameObject nodeBackground = new GameObject();
        nodeBackground.name = "windowBackground";
        nodeBackground.transform.SetParent(window.gameObject.transform);

        Image background = nodeBackground.AddComponent<Image>();
        background.sprite = Resources.Load<Sprite>(OGGetAddress.missioneditor + "NodeSprite_Light");
        background.type = Image.Type.Sliced;
        background.pixelsPerUnitMultiplier = 5;
        background.color = new Color(45f / 250f, 45f / 250f, 45f / 250f);

        window.backgrounRect = nodeBackground.GetComponent<RectTransform>();
        window.backgrounRect.sizeDelta = new Vector2(width, height);
        window.backgrounRect.localPosition = new Vector2(0, 0);
        window.backgrounRect.localScale = new Vector3(1, 1, 1);
    }

    //This draws a title
    public static void DrawText(Window window, string title, int fontSize, float xPos, float yPos, float height, float width, bool windowIsParent = true, Transform alternativeParent = null, string alignment = "topleft", TextAnchor textAnchor = TextAnchor.MiddleLeft)
    {
        //This draws the input label
        GameObject titleGO = new GameObject();
        titleGO.name = "Title_" + title;

        if (windowIsParent == true || alternativeParent == null)
        {
            titleGO.transform.SetParent(window.rectTransform.transform);
        }
        else
        {
            titleGO.transform.SetParent(alternativeParent);
        }

        RectTransform rectTransform = titleGO.AddComponent<RectTransform>();
        SetRectTransformAlignment(rectTransform, alignment);
        rectTransform.anchoredPosition = new Vector2(xPos, yPos);
        rectTransform.sizeDelta = new Vector2(width, height);
        rectTransform.localScale = new Vector3(1, 1, 1);

        Text labelText = titleGO.AddComponent<Text>();
        labelText.supportRichText = false;
        labelText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        labelText.text = title;
        labelText.fontSize = fontSize;
        labelText.color = Color.white;
        labelText.alignment = textAnchor;
    }

    //This draws a line break
    public static void DrawLineBreak(Window window, string color, float xPos, float yPos, float height, float width)
    {
        GameObject lineBreakGO = new GameObject();
        lineBreakGO.name = "LineBreak";

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
    public static void DrawImage(Window window, float xPos, float yPos, float height, float width, string imageName, bool parentToNode = true, Transform differentTransform = null, bool center = false)
    {
        GameObject imageGO = new GameObject();
        imageGO.name = "Button_" + imageName;

        if (parentToNode == true)
        {
            imageGO.transform.SetParent(window.rectTransform.transform);
        }
        else
        {
            imageGO.transform.SetParent(differentTransform);
        }

        RectTransform rectTransform = imageGO.AddComponent<RectTransform>();
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.pivot = new Vector2(0, 1);
        rectTransform.anchoredPosition = new Vector2(xPos, yPos);
        rectTransform.sizeDelta = new Vector2(width, height);
        rectTransform.localScale = new Vector3(1, 1, 1);

        if (center == true)
        {
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
        }

        Image image = imageGO.AddComponent<Image>();
        image.sprite = Resources.Load<Sprite>(OGGetAddress.missioneditor + imageName);
    }

    //This draws a button and allocates a function
    public static void DrawRawImage(Window window, float xPos, float yPos, float height, float width, string imageName, bool parentToNode = true, Transform differentTransform = null)
    {
        GameObject imageGO = new GameObject();
        imageGO.name = "Button_" + imageName;

        if (parentToNode == true)
        {
            imageGO.transform.SetParent(window.rectTransform.transform);
        }
        else
        {
            imageGO.transform.SetParent(differentTransform);
        }

        RectTransform rectTransform = imageGO.AddComponent<RectTransform>();
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.pivot = new Vector2(0, 1);
        rectTransform.anchoredPosition = new Vector2(xPos, yPos);
        rectTransform.sizeDelta = new Vector2(width, height);
        rectTransform.localScale = new Vector3(1, 1, 1);

        RawImage rawimage = imageGO.AddComponent<RawImage>();
        rawimage.texture = Resources.Load<Texture2D>(OGGetAddress.missioneditor + imageName);
    }

    //This draws a button and allocates a function
    public static void DrawImageButton(Window window, float xPos, float yPos, float height, float width, string imageName, string functionType, bool parentToNode = true, Transform differentTransform = null)
    {
        GameObject buttonGO = new GameObject();
        buttonGO.name = "Button_" + functionType;

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
        buttonImage.sprite = Resources.Load<Sprite>(OGGetAddress.missioneditor + imageName);

        UnityEngine.UI.Button button = buttonGO.AddComponent<UnityEngine.UI.Button>();
        button.image = buttonImage;

        if (functionType == "ExitMissionEditor")
        {
            button.onClick.AddListener(() => { MissionEditorFunctions.ExitMissionEditor(); });
        }  
        else if (functionType == "DeleteWindow")
        {
            button.onClick.AddListener(() => { DeleteWindow(window); });
        }
    }

    //This draws a button and allocates a function
    public static void DrawTextButton(Window window, float xPos, float yPos, float height, float width, string buttonImageName, string buttonText, int fontSize, string functionType, TextAnchor alignement = TextAnchor.MiddleCenter, bool parentToNode = true, Transform differentTransform = null)
    {
        GameObject buttonGO = new GameObject();
        GameObject buttonTextGO = new GameObject();

        buttonGO.name = "Button_" + functionType;
        buttonTextGO.name = "ButtonText_" + functionType;

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
            buttonImage.sprite = Resources.Load<Sprite>(OGGetAddress.missioneditor + buttonImageName);
        }
        else
        {
            Color color = Color.black;

            if (ColorUtility.TryParseHtmlString("#404040", out color))
            {
                buttonImage.color = color;
            }
        }

        UnityEngine.UI.Button button = buttonGO.AddComponent<UnityEngine.UI.Button>();
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
        else if (functionType == "ClearDisplay")
        {
            button.onClick.AddListener(() => { ClearDisplay(); });
        }
        else if (functionType == "UpdateDisplay")
        {
            button.onClick.AddListener(() => { UpdateDisplay(); });
        }
        else if (functionType == "ExportSelectionAs")
        {
            button.onClick.AddListener(() => { MissionEditorFunctions.ExportSelectionAs(); });
        }
        else if (functionType == "DeleteWindow")
        {
            button.onClick.AddListener(() => { DeleteWindow(window); });
        }
        else if (functionType == "DisplayShipInformation")
        {
            button.onClick.AddListener(() => { DisplayShipInformation(buttonText); });
        }
    }

    //This draws a field with scrollable buttons
    public static void DrawScrollableButtons(Window window, float xPos, float yPos, float height, float width, float buttonHeight, int fontSize, string[] buttons, string[] functions, string name = "scrollablebuttons")
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

        baseRectTransform.name = name;
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
        scrollRect.scrollSensitivity = 10;

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

    //This draws a field with scollable test
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
        scrollRect.scrollSensitivity = 100;
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

    //This draws a vertical scrollbar on a scrollable field
    public static void DrawVerticalScrollBar(Transform parent, ScrollRect scrollRect, float width)
    {
        GameObject scrollbarVertical = new GameObject();
        GameObject slidingArea = new GameObject();
        GameObject handle = new GameObject();

        scrollbarVertical.transform.SetParent(parent.transform);
        slidingArea.transform.SetParent(scrollbarVertical.transform);
        handle.transform.SetParent(slidingArea.transform);

        scrollbarVertical.name = "ScrollbarVertical";
        slidingArea.name = "SlidingArea";
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

        scrollbarImage.sprite = Resources.Load<Sprite>(OGGetAddress.missioneditor + "NodeSprite_Dark");
        scrollbarImage.type = Image.Type.Sliced;
        scrollbarImage.pixelsPerUnitMultiplier = 40;

        handleImage.sprite = Resources.Load<Sprite>(OGGetAddress.missioneditor + "NodeSprite_Grey");
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

        if (name != "none")
        {
            textboxGO.name = name;
        }
        else
        {
            textboxGO.name = "TextBox";
        }

        textBoxTextGO.name = "TextBoxText";

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
            backgroundImage.sprite = Resources.Load<Sprite>(OGGetAddress.missioneditor + backgroundImageName);
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
    }

    //Thsi draws a standard input field
    public static Text DrawInputField(Window window, string label, string startvalue, int fontSize, float xPos, float yPos, float height, float width, float gap = 10, string name = "none")
    {
        float halfwidth = (width - gap) / 2f;
        float shiftedXPosition = xPos + halfwidth + (gap / 2f);

        //This draws the input label
        GameObject labelGO = new GameObject();
        labelGO.name = "Label_" + label;

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
        inputFieldBackgroundGO.name = "InputFieldBackground_" + label;

        inputFieldBackgroundGO.transform.SetParent(window.rectTransform.transform);
        RectTransform rectTransform3 = inputFieldBackgroundGO.AddComponent<RectTransform>();
        rectTransform3.anchorMax = new Vector2(0, 1);
        rectTransform3.anchorMin = new Vector2(0, 1);
        rectTransform3.pivot = new Vector2(0, 1);
        rectTransform3.anchoredPosition = new Vector2(shiftedXPosition, yPos);
        rectTransform3.sizeDelta = new Vector2(halfwidth, height);
        rectTransform3.localScale = new Vector3(1, 1, 1);

        Image inputFieldImage = inputFieldBackgroundGO.AddComponent<Image>();
        inputFieldImage.sprite = Resources.Load<Sprite>(OGGetAddress.missioneditor + "NodeSprite_Light");
        inputFieldImage.type = Image.Type.Sliced;
        inputFieldImage.pixelsPerUnitMultiplier = 30;

        //This draws the input field
        GameObject inputFieldGO = new GameObject();
        inputFieldGO.name = "InputField_" + label;

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

        if (name != "none")
        {
            transitionTextGO.name = name;
        }
        else
        {
            transitionTextGO.name = "inputfield";
        }

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
        labelGO.name = "Label_" + label;

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
        inputFieldBackgroundGO.name = "InputFieldBackground_" + label;

        inputFieldBackgroundGO.transform.SetParent(window.rectTransform.transform);
        RectTransform rectTransform3 = inputFieldBackgroundGO.AddComponent<RectTransform>();
        rectTransform3.anchorMax = new Vector2(0, 1);
        rectTransform3.anchorMin = new Vector2(0, 1);
        rectTransform3.pivot = new Vector2(0, 1);
        rectTransform3.anchoredPosition = new Vector2(xPos, shiftedYPosition);
        rectTransform3.sizeDelta = new Vector2(width, modifiedHeight);
        rectTransform3.localScale = new Vector3(1, 1, 1);

        Image inputFieldImage = inputFieldBackgroundGO.AddComponent<Image>();
        inputFieldImage.sprite = Resources.Load<Sprite>(OGGetAddress.missioneditor + "NodeSprite_Light");
        inputFieldImage.type = Image.Type.Sliced;
        inputFieldImage.pixelsPerUnitMultiplier = 30;

        //This draws the input field
        GameObject inputFieldGO = new GameObject();
        inputFieldGO.name = "InputField_" + label;

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

        if (name != "none")
        {
            transitionTextGO.name = name;
        }
        else
        {
            transitionTextGO.name = "inputfieldlarge";
        }

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

        //If this is not run the caret will display behind the text box...
        ModifyCaretPosition(window);

        return transitionText;
    }

    //This draws a drop down box
    public static Text DrawDropDownMenuSansLabel(Window window, List<string> options, string label, string startvalue, int fontSize, float xPos, float yPos, float height, float width)
    {
        //This draws the background of the input field
        GameObject dropdownGO = new GameObject();
        dropdownGO.name = "DropDown_" + label;

        dropdownGO.transform.SetParent(window.rectTransform.transform);
        RectTransform rectTransform3 = dropdownGO.AddComponent<RectTransform>();
        rectTransform3.anchorMax = new Vector2(0, 1);
        rectTransform3.anchorMin = new Vector2(0, 1);
        rectTransform3.pivot = new Vector2(0, 1);
        rectTransform3.anchoredPosition = new Vector2(xPos, yPos);
        rectTransform3.sizeDelta = new Vector2(width, height);
        rectTransform3.localScale = new Vector3(1, 1, 1);

        Image inputFieldImage = dropdownGO.AddComponent<Image>();
        inputFieldImage.sprite = Resources.Load<Sprite>(OGGetAddress.missioneditor + "NodeSprite_Grey");
        inputFieldImage.type = Image.Type.Sliced;
        inputFieldImage.pixelsPerUnitMultiplier = 30;

        //This draws the input field
        GameObject labelGO = new GameObject();
        labelGO.name = "LabelGO_" + label;

        labelGO.transform.SetParent(window.rectTransform.transform);
        RectTransform rectTransform = labelGO.AddComponent<RectTransform>();
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.pivot = new Vector2(0, 1);
        rectTransform.anchoredPosition = new Vector2(xPos, yPos);
        rectTransform.sizeDelta = new Vector2(width - 2, height);
        rectTransform.localScale = new Vector3(1, 1, 1);

        Text captionText = labelGO.AddComponent<Text>();
        captionText.supportRichText = false;
        captionText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        captionText.text = startvalue;
        captionText.fontSize = fontSize;
        captionText.color = Color.white;
        captionText.alignment = TextAnchor.MiddleCenter;

        //This draws the template item
        GameObject templateGO = new GameObject();
        GameObject viewportGO = new GameObject();
        GameObject contentGO = new GameObject();
        GameObject itemGO = new GameObject();
        GameObject itemLabelGO = new GameObject();

        labelGO.name = "Label";
        templateGO.name = "Template";
        viewportGO.name = "Viewport";
        contentGO.name = "Content";
        itemGO.name = "Item";
        itemLabelGO.name = "Item Label";

        templateGO.transform.SetParent(dropdownGO.transform);
        labelGO.transform.SetParent(dropdownGO.transform);
        viewportGO.transform.SetParent(templateGO.transform);
        contentGO.transform.SetParent(viewportGO.transform);
        itemGO.transform.SetParent(contentGO.transform);
        itemLabelGO.transform.SetParent(itemGO.transform);

        RectTransform templateRectTransform = templateGO.AddComponent<RectTransform>();
        RectTransform viewportRectTransform = viewportGO.AddComponent<RectTransform>();
        RectTransform contentRectTransform = contentGO.AddComponent<RectTransform>();
        RectTransform itemRectTransform = itemGO.AddComponent<RectTransform>();
        RectTransform itemLabelRectTransform = itemLabelGO.AddComponent<RectTransform>();

        viewportGO.AddComponent<RectMask2D>();

        itemGO.AddComponent<Toggle>();

        float totalHeight = height * options.Count;

        //If the total height is larger than 100 the size is restricted to 100 and a scrollrect is added
        if (totalHeight > 100)
        {
            totalHeight = 100;

            ScrollRect scrollRect = templateGO.AddComponent<ScrollRect>();
            scrollRect.viewport = viewportRectTransform;
            scrollRect.content = contentRectTransform;
            scrollRect.movementType = ScrollRect.MovementType.Clamped;
            scrollRect.horizontal = false;

            DrawVerticalScrollBar(templateGO.transform, scrollRect, 3);
        }

        templateRectTransform.anchorMax = new Vector2(0, 1);
        templateRectTransform.anchorMin = new Vector2(0, 1);
        templateRectTransform.pivot = new Vector2(0, 1);
        templateRectTransform.anchoredPosition = new Vector2(0, -height);
        templateRectTransform.sizeDelta = new Vector2(width, totalHeight);
        templateRectTransform.localScale = new Vector3(1, 1, 1);

        viewportRectTransform.anchorMax = new Vector2(0, 1);
        viewportRectTransform.anchorMin = new Vector2(0, 1);
        viewportRectTransform.pivot = new Vector2(0, 1);
        viewportRectTransform.anchoredPosition = new Vector2(0, 0);
        viewportRectTransform.sizeDelta = new Vector2(width - 2, totalHeight);
        viewportRectTransform.localScale = new Vector3(1, 1, 1);

        contentRectTransform.anchorMax = new Vector2(0, 1);
        contentRectTransform.anchorMin = new Vector2(0, 1);
        contentRectTransform.pivot = new Vector2(0, 1);
        contentRectTransform.anchoredPosition = new Vector2(0, 0);
        contentRectTransform.sizeDelta = new Vector2(width - 2, height);
        contentRectTransform.localScale = new Vector3(1, 1, 1);

        itemRectTransform.anchorMax = new Vector2(0, 1);
        itemRectTransform.anchorMin = new Vector2(0, 1);
        itemRectTransform.pivot = new Vector2(0, 1);
        itemRectTransform.anchoredPosition = new Vector2(0, 0);
        itemRectTransform.sizeDelta = new Vector2(width - 2, height);
        itemRectTransform.localScale = new Vector3(1, 1, 1);

        itemLabelRectTransform.anchorMax = new Vector2(0, 1);
        itemLabelRectTransform.anchorMin = new Vector2(0, 1);
        itemLabelRectTransform.pivot = new Vector2(0, 1);
        itemLabelRectTransform.anchoredPosition = new Vector2(1, 0);
        itemLabelRectTransform.sizeDelta = new Vector2(width - 2, height);
        itemLabelRectTransform.localScale = new Vector3(1, 1, 1);

        Text templateText = itemLabelGO.AddComponent<Text>();
        templateText.supportRichText = false;
        templateText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        templateText.text = startvalue;
        templateText.fontSize = fontSize;
        templateText.color = Color.white;
        templateText.alignment = TextAnchor.MiddleCenter;

        Image templateBackground = templateGO.AddComponent<Image>();
        templateBackground.sprite = Resources.Load<Sprite>(OGGetAddress.missioneditor + "NodeSprite_Grey");
        templateBackground.type = Image.Type.Sliced;
        templateBackground.pixelsPerUnitMultiplier = 30;

        templateGO.SetActive(false);

        //This adds the drop down component
        Dropdown dropdown = dropdownGO.AddComponent<Dropdown>();
        dropdown.name = label;
        dropdown.captionText = captionText;
        dropdown.itemText = templateText;
        dropdown.template = templateRectTransform;
        dropdown.AddOptions(options);
        dropdown.onValueChanged.AddListener(delegate { DropDownValueChanged(dropdown); });

        return captionText;
    }

    //This registers a change in the drop box
    public static void DropDownValueChanged(Dropdown dropdown)
    {
        if (dropdown.name == "view")
        {
            ChangeView(dropdown.captionText.text);
        }
        else if (dropdown.name == "nodes")
        {
            ChangeNodesToDisplay(dropdown.captionText.text);
        }
        else if (dropdown.name == "ships")
        {
            SortShipList(dropdown.captionText.text);
        }
    }

    //This sets the rect transform
    public static void SetRectTransformAlignment(RectTransform rectTransform, string mode)
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

    #region Display Location Functions

    //This displays the location of all nodes that utilise a realworld position on the display location window
    public static void DisplayLocations(string displayAngle, string nodesToDisplay)
    {
        //This removes any current location markers
        DestroyLocationMarkers();

        //This gets the mission editor
        MissionEditor missionEditor = MissionEditorFunctions.GetMissionEditor();

        //This sets the display mode in the mission editor
        missionEditor.displayAngle = displayAngle;
        missionEditor.nodesToDisplay = nodesToDisplay;

        //This gets a list of all the locations and the names of the nodes
        List<Node> locationNodes = new List<Node>();

        foreach(Node node in missionEditor.nodes)
        {
            if (node.nodeType == "preload_loadasteroids" || 
                node.nodeType == "preload_loadmultipleshipsonground" ||
                node.nodeType == "preload_loadsingleship" ||
                node.nodeType == "preload_loadmultipleships" ||
                node.nodeType == "loadsingleship" ||
                node.nodeType == "loadsingleshipatdistanceandanglefromplayer" ||
                node.nodeType == "loadmultipleships" ||
                node.nodeType == "loadmultipleshipsonground" ||
                node.nodeType == "setwaypoint" ||
                node.nodeType == "changelocation")
            {
                if (nodesToDisplay == "all")
                {
                    locationNodes.Add(node);
                }
                else if (nodesToDisplay == "selected")
                {
                    if (node.selected == true)
                    {
                        locationNodes.Add(node);
                    }
                }               
            }
        }

        //This gets the display location window
        Window displayLocation = null;

        foreach(Window window in missionEditor.windows)
        {
            if (window.windowType == "displaylocation")
            {
                displayLocation = window;
            }
        }

        //This gets the display grid, also its size and length
        GameObject grid = null;
        float height = 0;
        float width = 0;

        if (displayLocation != null)
        {
            RawImage[] rawimages = displayLocation.gameObject.GetComponentsInChildren<RawImage>();

            foreach(RawImage rawImage in rawimages)
            {
                if (rawImage.gameObject.name.Contains("grid"))
                {
                    RawImage gridRawImage = rawImage;
                    RectTransform gridRectTransform = gridRawImage.gameObject.GetComponent<RectTransform>();
                    grid = gridRawImage.gameObject;
                    height = gridRectTransform.sizeDelta.y;
                    width = gridRectTransform.sizeDelta.x;
                    break;
                }
            }
        }

        //This creates the location nodes
        if (grid != null )
        {
            foreach (Node node in locationNodes)
            {
                //This creates the text to go next to the location marker
                string markerText = "";

                if (node.eventID != null)
                {
                    markerText = node.eventID.text;
                }

                if (node.eventType != null)
                {
                    if (node.nodeType == "preload_loadmultipleshipsonground" ||
                        node.nodeType == "preload_loadsingleship" ||
                        node.nodeType == "preload_loadmultipleships" ||
                        node.nodeType == "loadsingleship" ||
                        node.nodeType == "loadsingleshipatdistanceandanglefromplayer" ||
                        node.nodeType == "loadmultipleships" ||
                        node.nodeType == "loadmultipleshipsonground")
                    {
                        markerText = markerText + " " + node.data2.text;
                    }
                    else
                    {
                        markerText = markerText + " " + node.eventType.text;
                    }
                }

                //This calculates the location markers position
                float xPos = 0;
                float yPos = 0; 

                if (displayAngle == "top")
                {
                    xPos = (((width/2f) / 15000f) * float.Parse(node.x.text));
                    yPos = (((height/2f) / 15000f) * float.Parse(node.z.text));
                }
                else if (displayAngle == "side")
                {
                    xPos = (((width/2f) / 15000f) * float.Parse(node.x.text));

                    if (!node.nodeType.Contains("loadmultipleshipsonground"))
                    {
                        yPos = (((height / 2f) / 15000f) * float.Parse(node.y.text));
                    }
                    else
                    {
                        yPos = 0; //loadmultipleshipsonground have no set y value and so are just displayed at 0 when viewed from the side
                    }
                }

                //This creates a new location marker and sets its parent
                GameObject locationMarker = new GameObject();
                locationMarker.transform.SetParent(grid.transform);

                //This sets the values of the rect transfrom
                RectTransform rectTransform = locationMarker.AddComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(xPos, yPos);
                rectTransform.sizeDelta = new Vector2(0, 0);
                rectTransform.localScale = new Vector2(1, 1);

                //This loads the location marker image and the text beside it
                DrawImage(displayLocation, 0, 0, 10, 10, "target", false, locationMarker.transform, true);
                DrawText(displayLocation, markerText, 5, 5, 0, 10, 100, false, locationMarker.transform, "centerleft");

                //This adds the location marker to the list
                if (missionEditor.locationMarkers == null)
                {
                    missionEditor.locationMarkers = new List<GameObject>();
                }

                missionEditor.locationMarkers.Add(locationMarker);
            }
        }
    }

    //This toggles the view between top and side based on the selection
    public static void ChangeView(string displayAngle)
    {
        //This gets the mission editor
        MissionEditor missionEditor = MissionEditorFunctions.GetMissionEditor();

        DisplayLocations(displayAngle, missionEditor.nodesToDisplay);
    }

    //This toggles the view between top and side based on the selection
    public static void ChangeNodesToDisplay(string nodesToDisplay)
    {
        //This gets the mission editor
        MissionEditor missionEditor = MissionEditorFunctions.GetMissionEditor();

        DisplayLocations(missionEditor.displayAngle, nodesToDisplay);
    }

    //This resets the display of locations after a change has been made
    public static void UpdateDisplay()
    {
        //This gets the mission editor
        MissionEditor missionEditor = MissionEditorFunctions.GetMissionEditor();

        DisplayLocations(missionEditor.displayAngle, missionEditor.nodesToDisplay);
    }

    //This clears the display
    public static void ClearDisplay()
    {
        DestroyLocationMarkers();
    }

    //This destroys all location markers
    public static void DestroyLocationMarkers()
    {
        //This gets the mission editor
        MissionEditor missionEditor = MissionEditorFunctions.GetMissionEditor();

        if (missionEditor != null)
        {
            if (missionEditor.locationMarkers != null)
            {
                foreach (GameObject locationMarker in missionEditor.locationMarkers)
                {
                    GameObject.Destroy(locationMarker);
                }

                missionEditor.locationMarkers.Clear();
            }
        }
    }

    #endregion

    #region Misc Windows Functions

    //This deletes the selected window
    public static void DeleteWindow(Window window)
    {
        GameObject.Destroy(window.gameObject);
    }

    //This sorts the ships and remakes the window
    public static void SortShipList(string value)
    {
        GameObject shipPreviewGO = GameObject.Find("scollableshiplist");

        Window window = shipPreviewGO.GetComponentInParent<Window>();

        if (shipPreviewGO != null)
        {
            GameObject.Destroy(shipPreviewGO);
        }

        List<string> buttonList = new List<string>();

        TextAsset shipTypesFile = Resources.Load(OGGetAddress.files + "ShipTypes") as TextAsset;
        ShipTypes shipTypes = JsonUtility.FromJson<ShipTypes>(shipTypesFile.text);

        if (value != "all")
        {
            foreach (ShipType shipType in shipTypes.shipTypeData)
            {
                //This adds the ship to the list
                if (shipType.allegiance == value)
                {
                    buttonList.Add(shipType.type);
                }

                if (shipType.scriptType == value)
                {
                    buttonList.Add(shipType.type);
                }
            }
        }
        else
        {
            foreach (ShipType shipType in shipTypes.shipTypeData)
            {
                buttonList.Add(shipType.type);
            }
        }

        List<string> functionList = new List<string>();

        foreach (string button in buttonList)
        {
            functionList.Add("DisplayShipInformation");
        }

        string[] buttons = buttonList.ToArray();
        string[] functions = functionList.ToArray();

        DrawScrollableButtons(window, 5, -29, 161f, 115, 10, 7, buttons, functions, "scollableshiplist");

    }

    public static void DisplayShipInformation(string ship)
    {
        MissionEditor missionEditor = MissionEditorFunctions.GetMissionEditor();

        if (missionEditor.DisplayShipInformationTextBox == null)
        {
            GameObject DisplayShipInformationTextBoxGO = GameObject.Find("ShipInformationTextBox");

            if (DisplayShipInformationTextBoxGO != null)
            {
                missionEditor.DisplayShipInformationTextBox = DisplayShipInformationTextBoxGO.GetComponentInChildren<Text>();
            } 
        }

        if (missionEditor.DisplayShipInformationTextBox != null)
        {
            TextAsset shipTypesFile = Resources.Load(OGGetAddress.files + "ShipTypes") as TextAsset;
            ShipTypes shipTypes = JsonUtility.FromJson<ShipTypes>(shipTypesFile.text);

            foreach (ShipType shipType in shipTypes.shipTypeData)
            {
                if (shipType.type == ship)
                {
                    missionEditor.DisplayShipInformationTextBox.text = GetClassInfo(shipType);                       
  
                    GameObject shipPreviewGO = GameObject.Find("Button_assetpreview");

                    if (shipPreviewGO != null)
                    {
                        RawImage shipPreview = shipPreviewGO.GetComponentInChildren<RawImage>();

                        if (missionEditor.previewCamera != null)
                        {
                            GameObject.Destroy(missionEditor.previewCamera.gameObject);
                        }

                        if (missionEditor.previewRenderTexture != null)
                        {
                            GameObject.Destroy(missionEditor.previewRenderTexture);
                        }

                        if (missionEditor.previewModel != null)
                        {
                            GameObject.Destroy(missionEditor.previewModel.gameObject);
                        }

                        LoadModelAndRenderStatic("objects/ships/" + shipType.prefab, new Vector3(0, 0, 0), new Vector3(0, 0, 0), 256, 256, shipPreview, out missionEditor.previewModel, out missionEditor.previewCamera, out missionEditor.previewRenderTexture);

                        if (missionEditor.previewRenderTexture != null)
                        {
                            shipPreview.texture = missionEditor.previewRenderTexture;
                        }
                    }
                        
                    break;
                }
            }
        }
    }

    //This converts the class information to text
    public static string GetClassInfo(object obj)
    {
        if (obj == null) return "Object is null.";

        Type type = obj.GetType();
        StringBuilder sb = new StringBuilder();

        // Fields
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        foreach (var field in fields)
        {
            object value;
            try { value = field.GetValue(obj); }
            catch { value = "N/A"; }
            sb.AppendLine($" {field.Name}: {value}");
        }

        return sb.ToString();
    }

    //This loads a preview model
    public static void LoadModelAndRenderStatic(
        string modelPath,
        Vector3 modelPosition,
        Vector3 modelRotation,
        int textureWidth,
        int textureHeight,
        RawImage targetRawImage,
        out GameObject loadedModel,
        out Camera renderCamera,
        out RenderTexture renderTexture)
    {
        loadedModel = null;
        renderCamera = null;
        renderTexture = null;

        // Load the model prefab from Resources
        GameObject modelPrefab = Resources.Load<GameObject>(modelPath);
        if (modelPrefab == null)
        {
            Debug.LogError($"Model at '{modelPath}' could not be found in Resources.");
            return;
        }

        // Instantiate the model
        loadedModel = GameObject.Instantiate(modelPrefab, modelPosition, Quaternion.Euler(modelRotation));

        //This scales the model
        SceneFunctions.ScaleGameObjectByZAxis(loadedModel, 10);

        // This slowly rotates the model
        loadedModel.AddComponent<SlowRotate>();

        // Create a new camera to render the model
        GameObject camObj = new GameObject("ModelRenderCamera_Static");
        renderCamera = camObj.AddComponent<Camera>();
        renderCamera.clearFlags = CameraClearFlags.Color;
        renderCamera.backgroundColor = Color.clear;
        renderCamera.cullingMask = LayerMask.GetMask("Default"); // Adjust if your models are on a custom layer

        // Position camera to look at the model
        Bounds bounds = GetBoundsRecursive(loadedModel);
        Vector3 camPos = bounds.center + new Vector3(0, 0, -bounds.size.magnitude);
        renderCamera.transform.position = camPos;
        renderCamera.transform.LookAt(bounds.center);

        // Create and assign the render texture
        renderTexture = new RenderTexture(textureWidth, textureHeight, 16);
        renderCamera.targetTexture = renderTexture;

        // Assign to RawImage
        if (targetRawImage != null)
        {
            targetRawImage.texture = renderTexture;
        }
        else
        {
            Debug.LogWarning("No RawImage assigned to display the RenderTexture.");
        }
    }

    private static Bounds GetBoundsRecursive(GameObject go)
    {
        Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
            return new Bounds(go.transform.position, Vector3.one);

        Bounds bounds = renderers[0].bounds;
        foreach (Renderer r in renderers)
        {
            bounds.Encapsulate(r.bounds);
        }
        return bounds;
    }
    #endregion
}
