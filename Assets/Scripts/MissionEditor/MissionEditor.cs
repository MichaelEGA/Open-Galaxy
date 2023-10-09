using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MissionEditor : MonoBehaviour
{
    public List<Node> nodes;
    public Canvas canvas;
    public RectTransform editorContentRect;
    public ScrollRect scrollRect;
    public float scale = 1;
    public string gameWindowMode;
    public bool scrolling;

    void Start()
    {
        GameObject editorContentGo = GameObject.Find("EditorContent");
        editorContentRect = editorContentGo.GetComponent<RectTransform>();
        scrollRect = editorContentGo.GetComponentInParent<ScrollRect>();

        MissionEditorFunctions.SetWindowMode(this);

        nodes = new List<Node>();
        GameObject editorContent = GameObject.Find("EditorContent");

        GameObject node1GO = new GameObject();
        node1GO.transform.SetParent(editorContent.transform);
        Node node = node1GO.AddComponent<Node>();
        node.nodeType = "testnode";
        nodes.Add(node);

        GameObject node2GO = new GameObject();
        node2GO.transform.SetParent(editorContent.transform);
        Node node2 = node2GO.AddComponent<Node>();
        node2.nodeType = "menunode";
        nodes.Add(node2);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void OnGUI()
    {
        MissionEditorFunctions.ScaleGrid(this);
        MissionEditorFunctions.ToggleScrolling(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("I was clicked");
    }

    public void ExitMissionEditor()
    {
        MissionEditorFunctions.ExitMissionEditor(this);
    }



}
