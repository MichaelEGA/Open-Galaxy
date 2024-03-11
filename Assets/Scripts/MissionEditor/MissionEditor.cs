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
    public List<GameObject> locationMarkers;
    public Canvas canvas;
    public RectTransform editorContentRect;
    public RectTransform menuBarRectTransform;
    public ScrollRect scrollRect;
    public Text AddNodeTextBox;
    public Text scaleIndicator;
    public Text messageTextbox;
    public float scale = 1;
    public string gameWindowMode;
    public string locationdisplaymode = "top";
    public bool scrolling = true;
    public bool menusClosed = true;
    public bool middleMouseDown = false;
    public Vector3 mouseStartPos;
    public Rect selectionRect;
    public RectTransform selectionRectTransform;
    public string selectedNodeTypeToLoad;
    public string selectedMissionToLoad;
    public Text missionName;

    void Start()
    {
        GameObject editorContentGo = GameObject.Find("EditorContent");
        editorContentRect = editorContentGo.GetComponent<RectTransform>();
        scrollRect = editorContentGo.GetComponentInParent<ScrollRect>();

        MissionEditorFunctions.SetWindowMode();
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
        MissionEditorFunctions.SelectionBox(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }
}
