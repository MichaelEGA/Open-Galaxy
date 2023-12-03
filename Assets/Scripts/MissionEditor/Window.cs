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
    public Image background;

    private bool dragging;
    public bool scaling;

    public ScrollRect scrollRect;

    MissionEditor missionEditor;

    Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        //This gets the reference to the mission editor
        missionEditor = MissionEditorFunctions.GetMissionEditor();
        WindowFunctions.SelectWindowType(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        if (dragging == true)
        {
            transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y) - startPos;
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
        startPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y) - transform.position;
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
