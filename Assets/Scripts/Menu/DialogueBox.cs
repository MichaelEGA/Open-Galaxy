using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBox : MonoBehaviour
{

    public float loadTime;

    // Update is called once per frame
    void Update()
    {
        DialogueBoxFunctions.ReturnToGame(this, gameObject);
    }
}
