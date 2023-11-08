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

        windows = new List<Window>();

        GameObject menu1GO = new GameObject();
        menu1GO.transform.SetParent(gameObject.transform);
        Window menu1 = menu1GO.AddComponent<Window>();
        menu1.windowType = "mainmenu";
        windows.Add(menu1);

        GameObject menu2GO = new GameObject();
        menu2GO.transform.SetParent(gameObject.transform);
        Window menu2 = menu2GO.AddComponent<Window>();
        menu2.windowType = "addnodes";
        windows.Add(menu2);

        GameObject menu3GO = new GameObject();
        menu3GO.transform.SetParent(gameObject.transform);
        Window menu3 = menu3GO.AddComponent<Window>();
        menu3.windowType = "savemission";
        windows.Add(menu3);

        GameObject menu4GO = new GameObject();
        menu4GO.transform.SetParent(gameObject.transform);
        Window menu4 = menu4GO.AddComponent<Window>();
        menu4.windowType = "loadmission";
        windows.Add(menu4);
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
