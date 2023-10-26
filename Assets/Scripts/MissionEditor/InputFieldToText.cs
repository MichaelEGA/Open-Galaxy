using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldToText : MonoBehaviour
{
    public Text text;
    public InputField inputField;

    // Update is called once per frame
    void Update()
    {
        if (text != null & inputField != null)
        {
            text.text = inputField.text;
        }
    }
}
