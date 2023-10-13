using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeFunctions : MonoBehaviour
{
    //This generates the test node
    public static void DrawTestNode(Node node)
    {
        DrawNodeLink(node, 5, -6.5f, 10, 10, "female");

        DrawTitle(node, "Test Node", 8, 17.5f, -5, 12.5f, 90);

        DrawButton(node, 83, -6.5f, 10, 10, "cross", "DeleteNode");

        DrawLineBreak(node, "#808080", 0, -20, 1, 100);

        DrawInputField(node, "Event Type", "none", 7, 5, -25, 12.5f, 90, 5f);

        DrawDropDownMenu(node, "Ship Type", "none", 7, 5, -40, 12.5f, 90, 5f);

        DrawNodeLink(node, 85, -60, 10, 10, "male");
    }

    #region node drawing functions

    //This draws the base node gameobject
    public static void DrawNodeBase(Node node)
    {
        //This sets up the node background
        node.rectTransform = node.gameObject.AddComponent<RectTransform>();

        node.background = node.gameObject.AddComponent<Image>();
        node.background.sprite = Resources.Load<Sprite>("Data/EditorAssets/NodeSprite_Dark");
        node.background.type = Image.Type.Sliced;
        node.background.pixelsPerUnitMultiplier = 5;
        node.rectTransform.sizeDelta = new Vector2(node.sizeX, node.sizeZ);
        node.rectTransform.localPosition = new Vector2(node.xPos, node.yPos);
        node.rectTransform.localScale = new Vector3(1, 1, 1);

        node.name = "Node_" + node.nodeType + "_" + node.eventID;
    }

    //This draws a title
    public static void DrawTitle(Node node, string title, int fontSize, float xPos, float yPos, float height, float width)
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
    public static void DrawInputField(Node node, string label, string startvalue, int fontSize, float xPos, float yPos, float height, float width, float gap = 10)
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
    }

    //This draws a drop down box
    public static void DrawDropDownMenu(Node node, string label, string startvalue, int fontSize, float xPos, float yPos, float height, float width, float gap = 10)
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

        List<string> options = new List<string>();
        options.Add("x-wing");
        options.Add("y-wing");
        options.Add("b-wing");

        dropdown.AddOptions(options);
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
    public static void DrawNodeLink(Node node, float xPos, float yPos, float height, float width, string mode = "male")
    {
        GameObject nodeLinkGO = new GameObject();
        nodeLinkGO.name = "NodeLink";

        nodeLinkGO.transform.SetParent(node.rectTransform.transform);
        RectTransform rectTransform = nodeLinkGO.AddComponent<RectTransform>();
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.pivot = new Vector2(0, 1);
        rectTransform.anchoredPosition = new Vector2(xPos, yPos);
        rectTransform.sizeDelta = new Vector2(width, height);
        rectTransform.localScale = new Vector3(1, 1, 1);

        Image nodeLinkImage = nodeLinkGO.AddComponent<Image>();
        nodeLinkImage.sprite = Resources.Load<Sprite>("Data/EditorAssets/target");

        NodeLink nodeLink = nodeLinkGO.AddComponent<NodeLink>();
        nodeLink.mode = mode;
    }

    #endregion

    #region general node functions

    //This deletes the node
    public static void DeleteNode (Node node)
    {
        GameObject.Destroy(node.gameObject);
    }

    #endregion

}
