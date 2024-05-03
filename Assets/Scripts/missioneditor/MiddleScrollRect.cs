using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MiddleScrollRect : ScrollRect, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    MissionEditor missionEditor;

    public void OnPointerDown(PointerEventData eventData)
    {
       
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //This gets the editor reference if its not found
        if (missionEditor == null)
        {
            missionEditor = MissionEditorFunctions.GetMissionEditor();
        }

        if (missionEditor != null)
        {
            if (missionEditor.dragging != true)
            {
                MissionEditorFunctions.SelectNone(missionEditor);
            }
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        //This preserves the button pressed
        string buttonPressed = eventData.button.ToString();

        //This switches the drag button from the left button to the right
        if (eventData.button == PointerEventData.InputButton.Middle)
        {
            eventData.button = PointerEventData.InputButton.Left;
            base.OnBeginDrag(eventData);
        }

        //This gets the editor reference if its not found
        if (missionEditor == null)
        {
            missionEditor = MissionEditorFunctions.GetMissionEditor();
        }

        //This tells the mission editor that the left mouse button was clicked
        if (buttonPressed == "Left")
        {
            missionEditor.dragging = true;
        }
        else if (buttonPressed == "Middle")
        {
            missionEditor.scrolling = true;
        }

        //This closes all menus
        MissionEditorFunctions.CloseAllMenus();
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Middle)
        {
            eventData.button = PointerEventData.InputButton.Left;
            base.OnEndDrag(eventData);
        }

        if (missionEditor == null)
        {
            missionEditor = MissionEditorFunctions.GetMissionEditor();
        }

        Task a = new Task(EndDragging(missionEditor));

        MissionEditorFunctions.CloseAllMenus();
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Middle)
        {
            eventData.button = PointerEventData.InputButton.Left;
            base.OnDrag(eventData);
        }
    }

    //This needs to be delayed slighty so that the editor doesn't register it was a pure left click release
    public static IEnumerator EndDragging(MissionEditor missionEditor)
    {
        yield return new WaitForSeconds(0.1f);

        missionEditor.dragging = false;
    }
}
