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
            NodeTypes.DrawCustomNode(node);
        }
        else if (node.nodeType == "loadscene")
        {
            NodeTypes.DrawLoadScene(node);
        }
        else if (node.nodeType == "preload_loadasteroids")
        {
            NodeTypes.DrawPreLoad_LoadAsteroids(node);
        }
        else if (node.nodeType == "preload_loadplanet")
        {
            NodeTypes.DrawPreLoad_LoadPlanet(node);
        }
        else if (node.nodeType == "preload_loadtiles")
        {
            NodeTypes.DrawPreLoad_LoadTiles(node);
        }
        else if (node.nodeType == "preload_loadmultipleshipsonground")
        {
            NodeTypes.DrawPreLoad_LoadMultipleShipsOnGround(node);
        }
        else if (node.nodeType == "preload_loadship")
        {
            NodeTypes.DrawPreLoad_LoadShip(node);
        }
        else if (node.nodeType == "preload_loadshipsbyname")
        {
            NodeTypes.DrawPreLoad_LoadShipsByName(node);
        }
        else if (node.nodeType == "preload_loadshipsbytypeandallegiance")
        {
            NodeTypes.DrawPreLoad_LoadShipsByTypeAndAllegiance(node);
        }
        else if (node.nodeType == "preload_setskybox")
        {
            NodeTypes.DrawPreLoad_SetSkybox(node);
        }
        else
        {
            NodeTypes.DrawNodeNotAvaible(node);   
        }
    }

    #endregion

    #region general node functions

    //This deletes the node
    public static void DeleteNode(Node node)
    {
        GameObject.Destroy(node.gameObject);
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

        node.name = "Node_" + node.eventType + "_" + node.eventID;
    }

    //This draws a title
    public static Text DrawText(Node node, string title, int fontSize, float xPos, float yPos, float height, float width)
    {
        //This draws the input label
        GameObject titleGO = new GameObject();

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
        text.text = title;
        text.fontSize = fontSize;
        text.color = Color.white;
        text.alignment = TextAnchor.MiddleLeft;

        titleGO.name = "Title_" + title;

        return text;
    }

    //This draws a line break
    public static void DrawLineBreak(Node node, string color, float xPos, float yPos, float height, float width)
    {
        GameObject lineBreakGO = new GameObject();

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

        lineBreakGO.name = "linebreak";
    }

    //This draws an empty input field
    public static Text DrawInputField(Node node, string label, string startvalue, int fontSize, float xPos, float yPos, float height, float width, float gap = 10)
    {
        float halfwidth = (width - gap) / 2f;
        float shiftedXPosition = xPos + halfwidth + (gap / 2f);

        //This draws the input label
        GameObject labelGO = new GameObject();

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
        inputFieldText.text = startvalue;
        inputFieldText.fontSize = fontSize;
        inputFieldText.color = Color.gray;
        inputFieldText.alignment = TextAnchor.MiddleLeft;
        InputField inputField = inputFieldGO.AddComponent<InputField>();
        inputField.textComponent = inputFieldText;

        return inputFieldText;
    }

    //This draws a drop down box
    public static Text DrawDropDownMenu(Node node, List<string> options, string label, string startvalue, int fontSize, float xPos, float yPos, float height, float width, float gap = 10)
    {
        float halfwidth = (width - gap) / 2f;
        float shiftedXPosition = xPos + halfwidth + (gap / 2f);

        //This draws the input label
        GameObject exteriorLabelGO = new GameObject();

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

        dropdownGO.transform.SetParent(node.rectTransform.transform);
        RectTransform rectTransform3 = dropdownGO.AddComponent<RectTransform>();
        rectTransform3.anchorMax = new Vector2(0, 1);
        rectTransform3.anchorMin = new Vector2(0, 1);
        rectTransform3.pivot = new Vector2(0, 1);
        rectTransform3.anchoredPosition = new Vector2(shiftedXPosition, yPos);
        rectTransform3.sizeDelta = new Vector2(halfwidth, height);
        rectTransform3.localScale = new Vector3(1, 1, 1);

        Image inputFieldImage = dropdownGO.AddComponent<Image>();
        inputFieldImage.sprite = Resources.Load<Sprite>("Data/EditorAssets/NodeSprite_Light");
        inputFieldImage.type = Image.Type.Sliced;
        inputFieldImage.pixelsPerUnitMultiplier = 30;

        //This draws the input field
        GameObject labelGO = new GameObject();

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
        captionText.color = Color.gray;
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

        templateGO.AddComponent<ScrollRect>();

        itemGO.AddComponent<Toggle>();

        templateRectTransform.anchorMax = new Vector2(0, 1);
        templateRectTransform.anchorMin = new Vector2(0, 1);
        templateRectTransform.pivot = new Vector2(0, 1);
        templateRectTransform.anchoredPosition = new Vector2(0, -height);
        templateRectTransform.sizeDelta = new Vector2(halfwidth, height + 50);
        templateRectTransform.localScale = new Vector3(1, 1, 1);

        viewportRectTransform.anchorMax = new Vector2(0, 1);
        viewportRectTransform.anchorMin = new Vector2(0, 1);
        viewportRectTransform.pivot = new Vector2(0, 1);
        viewportRectTransform.anchoredPosition = new Vector2(0, 0);
        viewportRectTransform.sizeDelta = new Vector2(halfwidth - 2, height + 50);
        viewportRectTransform.localScale = new Vector3(1, 1, 1);

        contentRectTransform.anchorMax = new Vector2(0, 1);
        contentRectTransform.anchorMin = new Vector2(0, 1);
        contentRectTransform.pivot = new Vector2(0, 1);
        contentRectTransform.anchoredPosition = new Vector2(0, 0);
        contentRectTransform.sizeDelta = new Vector2(halfwidth - 2, height + 50);
        contentRectTransform.localScale = new Vector3(1, 1, 1);

        itemRectTransform.anchorMax = new Vector2(0, 1);
        itemRectTransform.anchorMin = new Vector2(0, 1);
        itemRectTransform.pivot = new Vector2(0, 1);
        itemRectTransform.anchoredPosition = new Vector2(0, 0);
        itemRectTransform.sizeDelta = new Vector2(halfwidth - 2, height);
        itemRectTransform.localScale = new Vector3(1, 1, 1);

        itemLabelRectTransform.anchorMax = new Vector2(0, 1);
        itemLabelRectTransform.anchorMin = new Vector2(0, 1);
        itemLabelRectTransform.pivot = new Vector2(0, 1);
        itemLabelRectTransform.anchoredPosition = new Vector2(1, 0);
        itemLabelRectTransform.sizeDelta = new Vector2(halfwidth - 2, height);
        itemLabelRectTransform.localScale = new Vector3(1, 1, 1);

        Text templateText = itemLabelGO.AddComponent<Text>();
        templateText.supportRichText = false;
        templateText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        templateText.text = startvalue;
        templateText.fontSize = fontSize;
        templateText.color = Color.gray;
        templateText.alignment = TextAnchor.MiddleLeft;

        Image templateBackground = templateGO.AddComponent<Image>();
        templateBackground.sprite = Resources.Load<Sprite>("Data/EditorAssets/NodeSprite_Light");
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

    //This draws a button and allocates a function
    public static void DrawButton(Node node, float xPos, float yPos, float height, float width, string imageName, string functionType)
    {
        GameObject buttonGO = new GameObject();

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

        buttonGO.name = "button_" + functionType;
    }

    //This draws a node link for connection the node with other nodes
    public static Text DrawNodeLink(Node node, float xPos, float yPos, float height, float width, string mode = "male", string title = "none", int fontsize = 7, float gap = 10)
    {
        Text textbox = null;

        float halfwidth = (width - gap) / 2f;
        float shiftedXPosition1 = xPos + halfwidth + (gap / 2f);
        float shiftedXPosition2 = shiftedXPosition1 + halfwidth -1;

        GameObject nodeLinkGO = new GameObject();
        GameObject nodeLinkImageGO = new GameObject();
        nodeLinkGO.name = "NodeLink";

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
            GameObject labelGO = new GameObject();

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
            text.text = title;
            text.fontSize = fontsize;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleLeft;

            GameObject textBoxGO = new GameObject();

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

    #endregion

}
