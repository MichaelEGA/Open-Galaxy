using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MissionEditor : MonoBehaviour
{
    public Canvas canvas;
    public List<Node> nodes;
    public List<Window> windows;
    public List<GameObject> menus;
    public List<NodeLink> nodeLinks;
    public List<GameObject> locationMarkers;
    public List<InputField> inputfields;
    public Vector3 mouseStartPos;
    public RectTransform editorContentRect;
    public RectTransform menuBarRectTransform;
    public RectTransform selectionRectTransform;
    public ScrollRect scrollRect;
    public Text AddNodeTextBox;
    public Text scaleIndicator;
    public Text messageTextbox;
    public Text missionName;
    public string gameWindowMode;
    public string locationdisplaymode = "top";
    public string clipboard;
    public string selectedNodeTypeToLoad;
    public string selectedMissionToLoad;
    public float scale = 1;
    public float scaleSave;
    public float timePressed;
    public bool scrolling = true;
    public bool menusClosed = true;
    public bool draggingGridStarted = false;
    public bool leftButtonGrid;
    public bool selectionHasRun;
    public bool pasting;


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
        MissionEditorFunctions.Shortcuts(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }
}
