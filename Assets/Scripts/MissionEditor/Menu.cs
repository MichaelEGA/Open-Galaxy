using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public string menuType = "Add Node";
    public float xPos = 0;
    public float yPos = 0;

    public RectTransform rectTransform;
    public float sizeX = 100;
    public float sizeZ = 200;
    public Image background;

    private bool dragging;

    private bool MenuDrawn;

    MissionEditor missionEditor;

    // Start is called before the first frame update
    void Start()
    {
        //This gets the reference to the mission editor
        missionEditor = MissionEditorFunctions.GetMissionEditor();

        MenuFunctions.DrawMenuBase(this);
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

        if (MenuDrawn == false)
        {
            MenuFunctions.DrawMainMenu(this);
            MenuDrawn = true;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;
        Debug.Log("dragging is true");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
    }

    public void OnDrag(PointerEventData eventData)
    {

    }
}
