using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public string nodeType = "Custom Node";
    public string eventType;
    public string eventID = "01";
    public Rect eventTypeRect;
    public GUI textfieldGUI;
    public float xPos = 0;
    public float yPos = 0;

    public RectTransform rectTransform;
    public float sizeX = 100;
    public float sizeZ = 200;
    public Image background;

    private bool dragging;
    private bool scrollReset;

    private bool nodeDrawn;

    MissionEditor missionEditor;

    // Start is called before the first frame update
    void Start()
    {
        //This gets the reference to the mission editor
        missionEditor = MissionEditorFunctions.GetMissionEditor();

        NodeFunctions.DrawNodeBase(this);

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI()
    {
        if (dragging == true)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            missionEditor.scrolling = false;
            scrollReset = false;
        }
        else if (scrollReset == false)
        {
            missionEditor.scrolling = true;
            scrollReset = true;
        }

        if (nodeDrawn == false)
        {
            if (nodeType == "Test Node")
            {
                NodeFunctions.DrawTestNode(this);
            }
            
            nodeDrawn = true;
        }     
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

}
