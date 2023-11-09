using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Node : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{

    //Mission Event data carried by the node, strings are data that is not displayed
    public Text eventID;
    public Text eventType;
    public Text conditionTime;
    public Text conditionLocation;
    public Text x;
    public Text y;
    public Text z;
    public Text xRotation;
    public Text yRotation;
    public Text zRotation;
    public Text data1;
    public Text data2;
    public Text data3;
    public Text data4;
    public Text data5;
    public Text data6;
    public Text data7;
    public Text data8;
    public Text data9;
    public Text data10;
    public Text data11;
    public Text data12;
    public Text data13;
    public Text data14;
    public Text data15;
    public Text nextEvent1;
    public Text nextEvent2;
    public Text nextEvent3;
    public Text nextEvent4;
    public float nodePosX;
    public float nodePosY;

    public string nodeType;

    public NodeLink femaleNodeLink;
    public List<NodeLink> maleNodeLinks;

    public RectTransform rectTransform;
    public float sizeX = 100;
    public float sizeZ = 200;

    private bool dragging; //Indicates whether the node is being dragged
    private bool scrollReset; //Tells the mission editor not to drag the canvas when the node is being dragged

    public MissionEditor missionEditor;

    // Start is called before the first frame update
    void Start()
    {
        NodeFunctions.SelectNodeToDraw(this);

        if (eventID != null)
        {
            if (eventID.text == "")
            {
                eventID.text = "E" + Random.Range(1000, 9999);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        if (dragging == true)
        {
            Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            transform.position = mousePosition;
            missionEditor.scrolling = false;
            scrollReset = false;
        }
        else if (scrollReset == false)
        {
            missionEditor.scrolling = true;
            scrollReset = true;
        }

        nodePosX = transform.localPosition.x;
        nodePosY = transform.localPosition.y;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;
        MissionEditorFunctions.CloseAllMenus();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;        
    }

    public void OnDrag(PointerEventData eventData)
    {

    }



}
