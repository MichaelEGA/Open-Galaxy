using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class CanvasUtils
{

    //This generates a button on the canvas
    public static void GenerateButton(GameObject parent, string buttonText, float positionX, float positionY, float sizeX, float sizeY, List<GameObject> pool, bool additive = false, string imageAddress = "none",  string normalColor = "FFFFFF", string highlightColor = "FFFFFF", string pressedColor = "C8C8C8", string selectedColor = "FFFFFF")
    {

        GameObject pivot = new GameObject();
        pivot.AddComponent<RectTransform>();
        pivot.transform.SetParent(parent.transform, false);
        pivot.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1);
        pivot.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
        pivot.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1);
        pivot.GetComponent<RectTransform>().sizeDelta = new Vector2(1, 1);

        if (additive == true)
        {
            pivot.transform.localPosition = pivot.transform.localPosition + new Vector3(positionX, positionY, 0);
        }
        else
        {
            pivot.transform.localPosition = new Vector3(positionX, positionY, 0);
        }
        
        pivot.name = buttonText;

        GameObject newButton = DefaultControls.CreateButton(new DefaultControls.Resources());
        newButton.transform.SetParent(pivot.transform, false);
        newButton.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        newButton.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        newButton.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        newButton.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeX, sizeY);

        //Color Color1 = Color.clear; ColorUtility.TryParseHtmlString(normalColor, out Color1);
        //newButton.GetComponent<Button>().colors.normalColor.

        if (imageAddress != "none")
        {
            Sprite image = Resources.Load<Sprite>(imageAddress);
            newButton.GetComponent<Image>().sprite = image;
        }

        newButton.GetComponentInChildren<Text>().text = buttonText;
        newButton.name = buttonText;

        //This adds the ui objects to the ui objects pool
        PoolUtils.AddToPool(pool, pivot);

    }

    //This generates a sprite on the canvas
    public static void GenerateImage(GameObject parent, string name, float positionX, float positionY, float sizeX, float sizeY, List<GameObject> pool, bool additive = false, string imageAddress = "none")
    {
        GameObject pivot = new GameObject();
        pivot.AddComponent<RectTransform>();
        pivot.transform.SetParent(parent.transform, false);
        pivot.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1);
        pivot.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
        pivot.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1);
        pivot.GetComponent<RectTransform>().sizeDelta = new Vector2(1, 1);

        if (additive == true)
        {
            pivot.transform.localPosition = pivot.transform.localPosition + new Vector3(positionX, positionY, 0);
        }
        else
        {
            pivot.transform.localPosition = new Vector3(positionX, positionY, 0);
        }

        pivot.name = name;

        GameObject newImage = DefaultControls.CreateImage(new DefaultControls.Resources());
        newImage.transform.SetParent(pivot.transform, false);
        newImage.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        newImage.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        newImage.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        newImage.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeX, sizeY);

        if (imageAddress != "none")
        {
            Sprite image = Resources.Load<Sprite>(imageAddress);
            newImage.GetComponent<Image>().sprite = image;
        }

        //This adds the ui objects to the ui objects pool
        PoolUtils.AddToPool(pool, pivot);

    }

    //Makes a line on the ui canvas between two points
    public static GameObject MakeLine(GameObject canvas, GameObject firstPoint, GameObject secondPoint, float lineWidth)
    {
        GameObject newLine = new GameObject();

        float ax = firstPoint.transform.localPosition.x;
        float ay = firstPoint.transform.localPosition.y;

        float bx = secondPoint.transform.localPosition.x;
        float by = secondPoint.transform.localPosition.y;

        Image NewImage = newLine.AddComponent<Image>();
        RectTransform rect = newLine.GetComponent<RectTransform>();
        rect.SetParent(canvas.transform);
        rect.localScale = Vector3.one;
        Vector2 graphScale = canvas.transform.localScale;

        var tempColor = NewImage.color;
        tempColor.a = 0.5f;
        NewImage.color = tempColor;

        Vector3 a = new Vector3(ax * graphScale.x, ay * graphScale.y, 0);
        Vector3 b = new Vector3(bx * graphScale.x, by * graphScale.y, 0);

        rect.localPosition = (a + b) / 2;
        Vector3 dif = a - b;
        rect.sizeDelta = new Vector3(dif.magnitude, lineWidth);
        rect.rotation = Quaternion.Euler(new Vector3(0, 0, 180 * Mathf.Atan(dif.y / dif.x) / Mathf.PI));

        newLine.transform.SetParent(canvas.transform);
        newLine.transform.SetAsFirstSibling();

        return newLine;
    }

    //Makes a line on the ui canvas between two points
    public static void DynamicallyMoveLine(GameObject line, GameObject canvas, GameObject firstPoint, GameObject secondPoint, float lineWidth)
    {
        float ax = firstPoint.transform.localPosition.x;
        float ay = firstPoint.transform.localPosition.y;

        float bx = secondPoint.transform.localPosition.x;
        float by = secondPoint.transform.localPosition.y;

        RectTransform rect = line.GetComponent<RectTransform>();
        Vector2 graphScale = canvas.transform.localScale;

        Vector3 a = new Vector3(ax * graphScale.x, ay * graphScale.y, 0);
        Vector3 b = new Vector3(bx * graphScale.x, by * graphScale.y, 0);

        rect.localPosition = (a + b) / 2;
        Vector3 dif = a - b;
        rect.sizeDelta = new Vector3(dif.magnitude, lineWidth);
        rect.rotation = Quaternion.Euler(new Vector3(0, 0, 180 * Mathf.Atan(dif.y / dif.x) / Mathf.PI));
    }

}
