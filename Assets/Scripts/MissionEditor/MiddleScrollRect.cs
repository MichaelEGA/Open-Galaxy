using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MiddleScrollRect : ScrollRect
{
    MissionEditor missionEditor;

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
            missionEditor.leftButtonGrid = true;
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

        missionEditor.leftButtonGrid = false;

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
}
