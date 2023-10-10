using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class MenuFunctions
{
    //This generates the main menu node
    public static void DrawMainMenu(Menu menu)
    {
        DrawTitle(menu, "Main Menu", 8, 5, -5, 12.5f, 90);

        DrawLineBreak(menu, "#808080", 0, -20, 1, 100);

        DrawButton(menu, 10, -25, 25, 25, "exit", "ExitMissionEditor");

        DrawButton(menu, 60, -25, 25, 25, "import", "none");

        DrawButton(menu, 10, -50, 25, 25, "plus", "none");

        DrawButton(menu, 60, -50, 25, 25, "save", "none");

        DrawButton(menu, 10, -75, 25, 25, "star", "none");

        DrawButton(menu, 60, -75, 25, 25, "trashcan", "none");
    }

    #region draw menu functions

    //This draws the base node gameobject
    public static void DrawMenuBase(Menu menu)
    {
        //This sets up the node background
        menu.rectTransform = menu.gameObject.AddComponent<RectTransform>();

        menu.background = menu.gameObject.AddComponent<Image>();
        menu.background.sprite = Resources.Load<Sprite>("Data/EditorAssets/NodeSprite_Grey");
        menu.background.type = Image.Type.Sliced;
        menu.background.pixelsPerUnitMultiplier = 5;
        menu.rectTransform.sizeDelta = new Vector2(menu.sizeX, menu.sizeZ);
        menu.rectTransform.localPosition = new Vector2(menu.xPos, menu.yPos);
        menu.rectTransform.localScale = new Vector3(1, 1, 1);
    }

    //This draws a title
    public static void DrawTitle(Menu menu, string title, int fontSize, float xPos, float yPos, float height, float width)
    {
        //This draws the input label
        GameObject titleGO = new GameObject();

        titleGO.transform.SetParent(menu.rectTransform.transform);
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
    public static void DrawLineBreak(Menu menu, string color, float xPos, float yPos, float height, float width)
    {
        GameObject lineBreakGO = new GameObject();

        lineBreakGO.transform.SetParent(menu.rectTransform.transform);
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
    public static void DrawButton(Menu menu, float xPos, float yPos, float height, float width, string imageName, string functionType)
    {
        GameObject buttonGO = new GameObject();

        buttonGO.transform.SetParent(menu.rectTransform.transform);
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

        buttonGO.name = "button_" + functionType;
    }

    #endregion
}
