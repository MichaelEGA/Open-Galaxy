using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Windows.Forms;
using UnityEngine.InputSystem.Composites;

public class ButtonInfo : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{ 
    public Text buttonName; //The button name
    public Text description; //The description of what the button does (if button has one)
    public Text informationRight; //The information that appears on the right hand side of the button
    public float buttonShiftDown; //How far to move the button down
    public float buttonShiftRight; //How far to move the button right
    public bool noSound = false;
    
    public ScrollRect scrollRect;
    public RectTransform button;

    private MainMenu mainMenu;

    void Start()
    {
        mainMenu = GameObject.FindFirstObjectByType<MainMenu>();
        scrollRect = GetComponentInParent<ScrollRect>();
        button = GetComponent<RectTransform>();
    }

    // When highlighted with mouse.
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (mainMenu != null & noSound == false)
        {
            if (mainMenu.buttonAudioSource != null & mainMenu.buttonHighlight != null)
            {
                mainMenu.buttonAudioSource.PlayOneShot(mainMenu.buttonHighlight);                
            }
        }       
    }

    // When selected.
    public void OnSelect(BaseEventData eventData)
    {
        if (mainMenu != null & noSound == false)
        {
            if (mainMenu.buttonAudioSource != null & mainMenu.buttonHighlight != null)
            {
                mainMenu.buttonAudioSource.PlayOneShot(mainMenu.buttonHighlight);       
            }
        }

        if (button != null) 
        {
            scrollRect.content.localPosition = GetSnapToPositionToBringChildIntoView(scrollRect, button);
        }
        
    }

    public Vector2 GetSnapToPositionToBringChildIntoView(ScrollRect instance, RectTransform child)
    {
        Canvas.ForceUpdateCanvases();
        Vector2 viewportLocalPosition = instance.viewport.localPosition;
        Vector2 childLocalPosition = child.localPosition;
        Vector2 result = new Vector2(0, 0 - (viewportLocalPosition.y + childLocalPosition.y));
        return result;
    }
}
