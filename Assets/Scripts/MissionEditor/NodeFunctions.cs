using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeFunctions : MonoBehaviour
{
    #region select node functions

    //This selects and draws the chosen node type
    public static void SelectNodeToDraw(Node node)
    {
        //This gets the reference to the mission editor
        node.missionEditor = MissionEditorFunctions.GetMissionEditor();

        if (node.nodeType == "custom_node")
        {
            NodeTypes.Draw_CustomNode(node);
        }
        else if (node.nodeType == "campaigninformation")
        {
            NodeTypes.Draw_CampaignInformation(node);
        }
        else if (node.nodeType == "createlocation")
        {
            NodeTypes.Draw_CreateLocation(node);
        }
        else if (node.nodeType == "preload_loadasteroids")
        {
            NodeTypes.Draw_PreLoad_LoadAsteroids(node);
        }
        else if (node.nodeType == "preload_loadplanet")
        {
            NodeTypes.Draw_PreLoad_LoadPlanet(node);
        }
        else if (node.nodeType == "preload_loadterrain")
        {
            NodeTypes.Draw_PreLoad_LoadTerrain(node);
        }
        else if (node.nodeType == "preload_loadmultipleshipsonground")
        {
            NodeTypes.Draw_PreLoad_LoadMultipleShipsOnGround(node);
        }
        else if (node.nodeType == "preload_loadsingleship")
        {
            NodeTypes.Draw_PreLoad_LoadSingleShip(node);
        }
        else if (node.nodeType == "preload_loadmultipleships")
        {
            NodeTypes.Draw_PreLoad_LoadMultipleShips(node);
        }
        else if (node.nodeType == "preload_setgalaxylocation")
        {
            NodeTypes.Draw_PreLoad_SetGalaxyLocation(node);
        }
        else if (node.nodeType == "preload_sethudcolour")
        {
            NodeTypes.Draw_PreLoad_SetHudColour(node);
        }
        else if (node.nodeType == "preload_setsceneradius")
        {
            NodeTypes.Draw_PreLoad_SetSceneRadius(node);
        }
        else if (node.nodeType == "preload_setskybox")
        {
            NodeTypes.Draw_PreLoad_SetSkybox(node);
        }
        else if (node.nodeType == "starteventseries")
        {
            NodeTypes.Draw_StartEventSeries(node);
        }
        else if (node.nodeType == "spliteventseries")
        {
            NodeTypes.Draw_SplitEventSeries(node);
        }
        else if (node.nodeType == "activatehyperspace")
        {
            NodeTypes.Draw_ActivateHyperspace(node);
        }
        else if (node.nodeType == "changelocation")
        {
            NodeTypes.Draw_ChangeLocation(node);
        }
        else if (node.nodeType == "clearaioverride")
        {
            NodeTypes.Draw_ClearAIOverride(node);
        }
        else if (node.nodeType == "deactivateship")
        {
            NodeTypes.Draw_DeactivateShip(node);
        }
        else if (node.nodeType == "displaydialoguebox")
        {
            NodeTypes.Draw_DisplayDialogueBox(node);
        }
        else if (node.nodeType == "displaytitle")
        {
            NodeTypes.Draw_DisplayTitle(node);
        }
        else if (node.nodeType == "displaymessage")
        {
            NodeTypes.Draw_DisplayMessage(node);
        }
        else if (node.nodeType == "displaymissionbriefing")
        {
            NodeTypes.Draw_DisplayMissionBriefing(node);
        }
        else if (node.nodeType == "exitmission")
        {
            NodeTypes.Draw_ExitMission(node);
        }
        else if (node.nodeType == "ifshipshullislessthan")
        {
            NodeTypes.Draw_IfShipsHullIsLessThan(node);
        }
        else if (node.nodeType == "ifshipislessthandistancetoothership")
        {
            NodeTypes.Draw_IfShipIsLessThanDistanceToOtherShip(node);
        }
        else if (node.nodeType == "ifshipislessthandistancetowaypoint")
        {
            NodeTypes.Draw_IfShipIsLessThanDistanceToWaypoint(node);
        }
        else if (node.nodeType == "ifshipisactive")
        {
            NodeTypes.Draw_IfShipIsActive(node);
        }
        else if (node.nodeType == "ifshiphasbeenscanned")
        {
            NodeTypes.Draw_IfShipHasBeenScanned(node);
        }
        else if (node.nodeType == "ifshiphasntbeenscanned")
        {
            NodeTypes.Draw_IfShipHasntBeenScanned(node);
        }
        else if (node.nodeType == "ifshipofallegianceisactive")
        {
            NodeTypes.Draw_IfShipOfAllegianceIsActive(node);
        }
        else if (node.nodeType == "loadsingleship")
        {
            NodeTypes.Draw_LoadSingleShip(node);
        }
        else if (node.nodeType == "loadsingleshipatdistanceandanglefromplayer")
        {
            NodeTypes.Draw_LoadSingleShipAtDistanceAndAngleFromPlayer(node);
        }
        else if (node.nodeType == "loadmultipleshipsonground")
        {
            NodeTypes.Draw_LoadMultipleShipsOnGround(node);
        }
        else if (node.nodeType == "loadmultipleships")
        {
            NodeTypes.Draw_LoadMultipleShips(node);
        }
        else if (node.nodeType == "pausesequence")
        {
            NodeTypes.Draw_PauseSequence(node);
        }
        else if (node.nodeType == "playmusictrack")
        {
            NodeTypes.Draw_PlayMusicTrack(node);
        }
        else if (node.nodeType == "setaioverride")
        {
            NodeTypes.Draw_SetAIOverride(node);
        }
        else if (node.nodeType == "setcargo")
        {
            NodeTypes.Draw_SetCargo(node);
        }
        else if (node.nodeType == "setcontrollock")
        {
            NodeTypes.Draw_SetControlLock(node);
        }
        else if (node.nodeType == "setobjective")
        {
            NodeTypes.Draw_SetObjective(node);
        }
        else if (node.nodeType == "setdontattacklargeships")
        {
            NodeTypes.Draw_SetDontAttackLargeShips(node);
        }
        else if (node.nodeType == "setshipallegiance")
        {
            NodeTypes.Draw_SetShipAllegiance(node);
        }
        else if (node.nodeType == "setshipstats")
        {
            NodeTypes.Draw_SetShipStats(node);
        }
        else if (node.nodeType == "setshiptarget")
        {
            NodeTypes.Draw_SetShipTarget(node);
        }
        else if (node.nodeType == "setshiptargettoclosestenemy")
        {
            NodeTypes.Draw_SetShipTargetToClosestEnemy(node);
        }
        else if (node.nodeType == "setshiptoinvincible")
        {
            NodeTypes.Draw_SetShipToInvincible(node);
        }
        else if (node.nodeType == "setwaypoint")
        {
            NodeTypes.Draw_SetWayPoint(node);
        }
        else if (node.nodeType == "setweaponslock") 
        {
            NodeTypes.Draw_SetWeaponsLock(node);
        }
        else
        {
            NodeTypes.Draw_CustomNode(node);
        }
    }

    #endregion

    #region general node functions

    //This deletes the node
    public static void DeleteNode(Node node)
    {
        MissionEditor missionEditor = MissionEditorFunctions.GetMissionEditor();

        foreach (NodeLink nodeLink in missionEditor.nodeLinks)
        {
            if (nodeLink != null)
            {
                if (nodeLink.node == node)
                {
                    GameObject.Destroy(nodeLink.line);
                    GameObject.Destroy(nodeLink.gameObject);
                }
            }
        }

        GameObject.Destroy(node.gameObject);
    }

    public static void DeleteAllNodes()
    {
        MissionEditor missionEditor = MissionEditorFunctions.GetMissionEditor();

        foreach (NodeLink nodeLink in missionEditor.nodeLinks)
        {
            if (nodeLink != null)
            {
                GameObject.Destroy(nodeLink.line);
                GameObject.Destroy(nodeLink.gameObject);
            }
        }

        foreach (Node node in missionEditor.nodes)
        {
            if (node != null)
            {
                GameObject.Destroy(node.gameObject);
            }
        }

        MissionEditorFunctions.UpdateMissionName("Untitled Mission");
    }

    public static void GetUniqueNodeID(Node node)
    {
        MissionEditor missionEditor = MissionEditorFunctions.GetMissionEditor();

        if (node.eventID != null)
        {
            if (node.eventID.text == "")
            {
                node.eventID.text = "E" + Random.Range(0, 99999).ToString("00000");
            }
        }

        int i = 0;

        foreach (Node tempNode in missionEditor.nodes)
        {
            if (tempNode != null)
            {
                if (tempNode.eventID != null & node.eventID != null)
                {
                    if (tempNode != node)
                    {
                        if (tempNode.eventID.text == node.eventID.text)
                        {
                            node.eventID.text = node.eventID.text + i;
                            break;
                        }
                    }
                }
            }

            i += 1;
        }
    }

    #endregion

    #region node drawing functions

    //This sets the nodes size
    public static void SetNodeSize(Node node, float sizeX, float sizeY)
    {
        node.sizeX = sizeX;
        node.sizeX = sizeY;

        if (node.rectTransform != null)
        {
            node.rectTransform.sizeDelta = new Vector2(sizeX, sizeY);
        }       
    }

    //This sets the nodes position
    public static void SetNodePosition(Node node, float nodePosX, float nodePosY)
    {
        node.nodePosX = nodePosX;
        node.nodePosY = nodePosY;

        if (node.rectTransform != null)
        {
            node.rectTransform.localPosition = new Vector2(nodePosX, nodePosY);
        }        
    }

    //This draws the base node gameobject
    public static void DrawNodeBase(Node node)
    {
        //This sets up the node background
        node.rectTransform = node.gameObject.AddComponent<RectTransform>();

        Image background = node.gameObject.AddComponent<Image>();
        background.sprite = Resources.Load<Sprite>("Data/EditorAssets/NodeSprite_Dark");
        background.type = Image.Type.Sliced;
        background.pixelsPerUnitMultiplier = 5;
        node.rectTransform.sizeDelta = new Vector2(node.sizeX, node.sizeZ);
        node.rectTransform.localPosition = new Vector2(node.nodePosX, node.nodePosY);
        node.rectTransform.localScale = new Vector3(1, 1, 1);

        node.name = "Node";
    }

    //This draws a title
    public static Text DrawText(Node node, string textString, int fontSize, float xPos, float yPos, float height, float width)
    {
        //This draws the input label
        GameObject titleGO = new GameObject();

        titleGO.name = "Text_" + textString;

        titleGO.transform.SetParent(node.rectTransform.transform);
        RectTransform rectTransform = titleGO.AddComponent<RectTransform>();
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.pivot = new Vector2(0, 1);
        rectTransform.anchoredPosition = new Vector2(xPos, yPos);
        rectTransform.sizeDelta = new Vector2(width, height);
        rectTransform.localScale = new Vector3(1, 1, 1);

        Text text = titleGO.AddComponent<Text>();
        text.supportRichText = false;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.text = textString;
        text.fontSize = fontSize;
        text.color = Color.white;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.alignment = TextAnchor.MiddleLeft;

        return text;
    }

    //This draws a line break
    public static void DrawLineBreak(Node node, string color, float xPos, float yPos, float height, float width)
    {
        GameObject lineBreakGO = new GameObject();

        lineBreakGO.name = "linebreak";

        lineBreakGO.transform.SetParent(node.rectTransform.transform);
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
    }

    //This draws an empty input field
    public static Text DrawInputField(Node node, string label, string startvalue, int fontSize, float xPos, float yPos, float height, float width, float gap = 10)
    {
        float halfwidth = (width - gap) / 2f;
        float shiftedXPosition = xPos + halfwidth + (gap / 2f);

        //This draws the input label
        GameObject labelGO = new GameObject();
        labelGO.name = "Label_" + label;

        labelGO.transform.SetParent(node.rectTransform.transform);
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

        inputFieldBackgroundGO.transform.SetParent(node.rectTransform.transform);
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
        inputFieldGO.name = "InputFieldGO_" + label;

        inputFieldGO.transform.SetParent(node.rectTransform.transform);

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
        inputField.caretColor = Color.gray;
        inputField.text = startvalue;

        GameObject transitionTextGO = new GameObject();
        transitionTextGO.name = "TransitionText_" + label;

        transitionTextGO.transform.SetParent(inputFieldGO.transform);
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
        ModifyCaretPosition(node);

        return transitionText;
    }

    //This draws an empty input field
    public static Text DrawInputFieldLarge(Node node, string label, string startvalue, int fontSize, float xPos, float yPos, float height, float width)
    {
        float shiftedYPosition = yPos - 12.5f;
        float modifiedHeight = height - 12.5f;

        //This draws the input label
        GameObject labelGO = new GameObject();
        labelGO.name = "Label_" + label;

        labelGO.transform.SetParent(node.rectTransform.transform);
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

        inputFieldBackgroundGO.transform.SetParent(node.rectTransform.transform);
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
        inputFieldGO.name = "InputField_" + label;

        inputFieldGO.transform.SetParent(node.rectTransform.transform);

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
        transitionTextGO.name = "TransitionText_" + label;

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
        ModifyCaretPosition(node);

        return transitionText;
    }

    //This draws a drop down box
    public static Text DrawDropDownMenu(Node node, List<string> options, string label, string startvalue, int fontSize, float xPos, float yPos, float height, float width, float gap = 10)
    {
        float halfwidth = (width - gap) / 2f;
        float shiftedXPosition = xPos + halfwidth + (gap / 2f);

        //This draws the input label
        GameObject exteriorLabelGO = new GameObject();
        exteriorLabelGO.name = "ExteriorLabel_" + label;

        exteriorLabelGO.transform.SetParent(node.rectTransform.transform);
        RectTransform rectTransform2 = exteriorLabelGO.AddComponent<RectTransform>();
        rectTransform2.anchorMax = new Vector2(0, 1);
        rectTransform2.anchorMin = new Vector2(0, 1);
        rectTransform2.pivot = new Vector2(0, 1);
        rectTransform2.anchoredPosition = new Vector2(xPos, yPos);
        rectTransform2.sizeDelta = new Vector2(halfwidth, height);
        rectTransform2.localScale = new Vector3(1, 1, 1);

        Text labelText = exteriorLabelGO.AddComponent<Text>();
        labelText.supportRichText = false;
        labelText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        labelText.text = label;
        labelText.fontSize = fontSize;
        labelText.color = Color.white;
        labelText.alignment = TextAnchor.MiddleLeft;

        //This draws the background of the input field
        GameObject dropdownGO = new GameObject();
        dropdownGO.name = "DropDown_" + label;

        dropdownGO.transform.SetParent(node.rectTransform.transform);
        RectTransform rectTransform3 = dropdownGO.AddComponent<RectTransform>();
        rectTransform3.anchorMax = new Vector2(0, 1);
        rectTransform3.anchorMin = new Vector2(0, 1);
        rectTransform3.pivot = new Vector2(0, 1);
        rectTransform3.anchoredPosition = new Vector2(shiftedXPosition, yPos);
        rectTransform3.sizeDelta = new Vector2(halfwidth, height);
        rectTransform3.localScale = new Vector3(1, 1, 1);

        Image inputFieldImage = dropdownGO.AddComponent<Image>();
        inputFieldImage.sprite = Resources.Load<Sprite>("Data/EditorAssets/NodeSprite_Grey");
        inputFieldImage.type = Image.Type.Sliced;
        inputFieldImage.pixelsPerUnitMultiplier = 30;

        //This draws the input field
        GameObject labelGO = new GameObject();
        labelGO.name = "LabelGO_" + label;

        labelGO.transform.SetParent(node.rectTransform.transform);
        RectTransform rectTransform = labelGO.AddComponent<RectTransform>();
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.pivot = new Vector2(0, 1);
        rectTransform.anchoredPosition = new Vector2(shiftedXPosition + 1, yPos);
        rectTransform.sizeDelta = new Vector2(halfwidth - 2, height);
        rectTransform.localScale = new Vector3(1, 1, 1);

        Text captionText = labelGO.AddComponent<Text>();
        captionText.supportRichText = false;
        captionText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        captionText.text = startvalue;
        captionText.fontSize = fontSize;
        captionText.color = Color.white;
        captionText.alignment = TextAnchor.MiddleLeft;

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
        templateText.alignment = TextAnchor.MiddleLeft;

        Image templateBackground = templateGO.AddComponent<Image>();
        templateBackground.sprite = Resources.Load<Sprite>("Data/EditorAssets/NodeSprite_Grey");
        templateBackground.type = Image.Type.Sliced;
        templateBackground.pixelsPerUnitMultiplier = 30;

        templateGO.SetActive(false);

        //This adds the drop down component
        Dropdown dropdown = dropdownGO.AddComponent<Dropdown>();
        dropdown.captionText = captionText;
        dropdown.itemText = templateText;
        dropdown.template = templateRectTransform;
        dropdown.AddOptions(options);

        return captionText;
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

    //This draws a button and allocates a function
    public static void DrawButton(Node node, float xPos, float yPos, float height, float width, string imageName, string functionType)
    {
        GameObject buttonGO = new GameObject();
        buttonGO.name = "Button_" + functionType;

        buttonGO.transform.SetParent(node.rectTransform.transform);
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
        else if (functionType == "DeleteNode")
        {
            button.onClick.AddListener(() => { DeleteNode(node); });
        }
    }

    //This draws a node link for connection the node with other nodes
    public static Text DrawNodeLink(Node node, float xPos, float yPos, float height, float width, string mode = "male", string label = "none", int fontsize = 7, float gap = 10)
    {
        Text textbox = null;

        float halfwidth = (width - gap) / 2f;
        float shiftedXPosition1 = xPos + halfwidth + (gap / 2f);
        float shiftedXPosition2 = shiftedXPosition1 + halfwidth -1;

        GameObject nodeLinkGO = new GameObject();
        GameObject nodeLinkImageGO = new GameObject();

        nodeLinkGO.name = "NodeLink";
        nodeLinkImageGO.name = "NodeLinkImage";

        nodeLinkGO.transform.SetParent(node.rectTransform.transform);
        RectTransform rectTransform = nodeLinkGO.AddComponent<RectTransform>();
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.pivot = new Vector2(0, 1);
        rectTransform.anchoredPosition = new Vector2(xPos, yPos);
        rectTransform.sizeDelta = new Vector2(10, 10);
        rectTransform.localScale = new Vector3(1, 1, 1);

        nodeLinkImageGO.transform.SetParent(nodeLinkGO.transform);
        RectTransform rectTransform1 = nodeLinkImageGO.AddComponent<RectTransform>();
        rectTransform1.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform1.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform1.pivot = new Vector2(0.5f, 0.5f);
        rectTransform1.anchoredPosition = new Vector2(-5, 5);
        rectTransform1.sizeDelta = new Vector2(10, 10);
        rectTransform1.localScale = new Vector3(1, 1, 1);

        Image nodeLinkImage = nodeLinkImageGO.AddComponent<Image>();
        nodeLinkImage.sprite = Resources.Load<Sprite>("Data/EditorAssets/target");

        NodeLink nodeLink = nodeLinkGO.AddComponent<NodeLink>();
        nodeLink.node = node;
        nodeLink.mode = mode;
        
        if (mode == "male")
        {
            if (node.maleNodeLinks == null)
            {
                node.maleNodeLinks = new List<NodeLink>();
                node.maleNodeLinks.Add(nodeLink);
            }
            else
            {
                node.maleNodeLinks.Add(nodeLink);
            }
        }
        else
        {
            if (node.femaleNodeLink == null)
            {
                node.femaleNodeLink = nodeLink;
            }
        }

        if (mode == "male")
        {
            GameObject labelGO = new GameObject();
            labelGO.name = "Label_" + label;

            labelGO.transform.SetParent(node.rectTransform.transform);
            RectTransform rectTransform2 = labelGO.AddComponent<RectTransform>();
            rectTransform2.anchorMax = new Vector2(0, 1);
            rectTransform2.anchorMin = new Vector2(0, 1);
            rectTransform2.pivot = new Vector2(0, 1);
            rectTransform2.anchoredPosition = new Vector2(xPos, yPos);
            rectTransform2.sizeDelta = new Vector2(halfwidth, height);
            rectTransform2.localScale = new Vector3(1, 1, 1);

            Text text = labelGO.AddComponent<Text>();
            text.supportRichText = false;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.text = label;
            text.fontSize = fontsize;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleLeft;

            GameObject textBoxGO = new GameObject();
            textBoxGO.name = "TextBox_" + label;

            textBoxGO.transform.SetParent(node.rectTransform.transform);
            RectTransform rectTransform3 = textBoxGO.AddComponent<RectTransform>();
            rectTransform3.anchorMax = new Vector2(0, 1);
            rectTransform3.anchorMin = new Vector2(0, 1);
            rectTransform3.pivot = new Vector2(0, 1);
            rectTransform3.anchoredPosition = new Vector2(shiftedXPosition1, yPos);
            rectTransform3.sizeDelta = new Vector2(halfwidth - 1, height);
            rectTransform3.localScale = new Vector3(1, 1, 1);

            Text text2 = textBoxGO.AddComponent<Text>();
            text2.supportRichText = false;
            text2.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text2.text = "none";
            text2.fontSize = fontsize;
            text2.color = Color.white;
            text2.alignment = TextAnchor.MiddleLeft;

            //This moves the node line inline with the text boxes
            rectTransform.anchoredPosition = new Vector2(shiftedXPosition2, yPos -7.5f);

            textbox = text2;
            nodeLink.textbox = text2;
        }    

        return textbox;
    }

    //This modifies the caret position to ensure its on top
    public static void ModifyCaretPosition (Node node)
    {
        Transform[] carets = GameObjectUtils.FindAllChildTransformsContaining(node.transform, "Caret");

        int childNumber = node.transform.childCount;

        foreach (Transform caret in carets)
        {
            caret.SetAsLastSibling();
        }
    }

    #endregion

}
