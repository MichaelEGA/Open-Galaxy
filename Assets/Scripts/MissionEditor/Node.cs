using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{

    //Mission Event data carried by the node, strings are data that is not displayed
    public string eventID;
    public string eventType;
    public Text conditionTimeText;
    public Text conditionLocationText;
    public Text xText;
    public Text yText;
    public Text zText;
    public Text xRotationText;
    public Text yRotationText;
    public Text zRotationText;
    public Text data1Text;
    public Text data2Text;
    public Text data3Text;
    public Text data4Text;
    public Text data5Text;
    public Text data6Text;
    public Text data7Text;
    public Text data8Text;
    public Text data9Text;
    public Text data10Text;
    public Text data11Text;
    public Text data12Text;
    public Text data13Text;
    public Text data14Text;
    public Text data15Text;
    public Text nextEvent1Text;
    public Text nextEvent2Text;
    public Text nextEvent3Text;
    public Text nextEvent4Text;
    public float nodePosX;
    public float nodePosY;

    public RectTransform rectTransform;
    public float sizeX = 100;
    public float sizeZ = 200;

    private bool dragging; //Indicates whether the node is being dragged
    private bool scrollReset; //Tells the mission editor not to drag the canvas when the node is being dragged

    MissionEditor missionEditor;

    // Start is called before the first frame update
    void Start()
    {
        //This gets the reference to the mission editor
        missionEditor = MissionEditorFunctions.GetMissionEditor();

        NodeFunctions.DrawNodeBase(this);

        NodeFunctions.DrawTestNode(this);
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
