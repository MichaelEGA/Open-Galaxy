using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MissionEditor : MonoBehaviour
{
    public List<Node> nodes;
    public List<Window> windows;
    public List<GameObject> menus;
    public List<NodeLink> nodeLinks;
    public Canvas canvas;
    public RectTransform editorContentRect;
    public RectTransform menuBarRectTransform;
    public ScrollRect scrollRect;
    public Text AddNodeTextBox;
    public Text scaleIndicator;
    public Text messageTextbox;
    public float scale = 1;
    public string gameWindowMode;
    public bool scrolling;
    public bool menusClosed = true;
    public string selectedNodeTypeToLoad;
    public string selectedMissionToLoad;
    public string missionName = "Untitled Mission";

    void Start()
    {
        GameObject editorContentGo = GameObject.Find("EditorContent");
        editorContentRect = editorContentGo.GetComponent<RectTransform>();
        scrollRect = editorContentGo.GetComponentInParent<ScrollRect>();

        MissionEditorFunctions.SetWindowMode(this);
        MissionEditorFunctions.Draw_MissionEditor(this);
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
        
    }
}
