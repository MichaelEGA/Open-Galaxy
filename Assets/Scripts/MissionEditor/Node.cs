using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public float xPos = 0;
    public float yPos = 0;

    private RectTransform nodeRect;
    private Image background;
    private float sizeX = 100;
    private float sizeZ = 200;
    private bool dragging;
    private bool scrollReset;

    MissionEditor missionEditor;

    // Start is called before the first frame update
    void Start()
    {
        //This gets the reference to the mission editor
        missionEditor = MissionEditorFunctions.GetMissionEditor();

        //This sets up the node background
        nodeRect = this.gameObject.AddComponent<RectTransform>();
        background = this.gameObject.AddComponent<Image>();
        background.sprite = Resources.Load<Sprite>("Data/EditorAssets/NodeBox");
        background.type = Image.Type.Sliced;
        background.pixelsPerUnitMultiplier = 5;
        nodeRect.sizeDelta = new Vector2(sizeX, sizeZ);
        nodeRect.localPosition = new Vector2(xPos, yPos);

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
