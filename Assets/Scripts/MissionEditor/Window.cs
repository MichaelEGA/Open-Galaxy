using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Window : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public string windowType = "Add Node";
    public float xPos = 0;
    public float yPos = 0;

    public RectTransform rectTransform;
    public float sizeX = 100;
    public float sizeZ = 200;
    public Image background;

    private bool dragging;
    public bool scaling;

    private bool windowDrawn;

    public ScrollRect scrollRect;

    MissionEditor missionEditor;

    // Start is called before the first frame update
    void Start()
    {
        //This gets the reference to the mission editor
        missionEditor = MissionEditorFunctions.GetMissionEditor();

        WindowFunctions.DrawWindowBase(this);

        if (windowDrawn == false)
        {
            if (windowType == "mainmenu")
            {
                WindowFunctions.Draw_MainMenu(this);
            }
            else if (windowType == "addnodes")
            {
                WindowFunctions.Draw_AddNode(this);
            }
            else if (windowType == "loadmission")
            {
                WindowFunctions.Draw_LoadMission(this);
            }
            else if (windowType == "savemission")
            {
                WindowFunctions.Draw_SaveMission(this);
            }

            windowDrawn = true;
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
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }  
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        scaling = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        scaling = true;
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
