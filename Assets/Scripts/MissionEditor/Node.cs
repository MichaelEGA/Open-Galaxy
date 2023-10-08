using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public string eventType;
    public Rect eventTypeRect;
    public GUI textfieldGUI;
    public float xPos = 0;
    public float yPos = 0;

    public RectTransform rectTransform;
    public float sizeX = 100;
    public float sizeZ = 200;

    private Image background;
    private bool dragging;
    private bool scrollReset;

    private bool nodeDrawn;

    MissionEditor missionEditor;

    // Start is called before the first frame update
    void Start()
    {
        //This gets the reference to the mission editor
        missionEditor = MissionEditorFunctions.GetMissionEditor();

        //This sets up the node background
        rectTransform = this.gameObject.AddComponent<RectTransform>();
       
        background = this.gameObject.AddComponent<Image>();
        background.sprite = Resources.Load<Sprite>("Data/EditorAssets/NodeBox");
        background.type = Image.Type.Sliced;
        background.pixelsPerUnitMultiplier = 5;
        rectTransform.sizeDelta = new Vector2(sizeX, sizeZ);
        rectTransform.localPosition = new Vector2(xPos, yPos);

        this.gameObject.AddComponent<BoxCollider2D>();
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
            NodeFunctions.DrawCustomNode(this);
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
