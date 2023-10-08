using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeFunctions : MonoBehaviour
{
    public static void DrawCustomNode(Node node)
    {
        DrawInputField(node, "inputfield", 30);       
    }

    public static void DrawInputField(Node node, string name, float drop)
    {
        GameObject inputfieldGO = new GameObject();
        inputfieldGO.transform.SetParent(node.rectTransform.transform);
        RectTransform rectTransform = inputfieldGO.AddComponent<RectTransform>();
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.pivot = new Vector2(0, 1);
        rectTransform.anchoredPosition = new Vector2(0, 0);
        Text inputFieldText = inputfieldGO.AddComponent<Text>();
        inputFieldText.supportRichText = false;
        inputFieldText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        inputFieldText.text = "Check";
        InputField inputField = inputfieldGO.AddComponent<InputField>();
        inputField.textComponent = inputFieldText;
    }
}
