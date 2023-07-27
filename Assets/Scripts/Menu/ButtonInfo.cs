using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInfo : MonoBehaviour
{
    public Text buttonName; //The button name
    public Text description; //The description of what the button does (if button has one)
    public float buttonShift; //How far to move before placing the next button so that it doesn't overlap with this one
}
