using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MissionEditor : MonoBehaviour
{
    public Node[] nodes;
    public Canvas canvas;
    public RectTransform EditorContentRect;
    public float scale = 1;
    public string gameWindowMode;

    void Start()
    {
        GameObject EditorContentGo = GameObject.Find("EditorContent");
        EditorContentRect = EditorContentGo.GetComponent<RectTransform>();

        MissionEditorFunctions.SetWindowMode(this);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void OnGUI()
    {
        MissionEditorFunctions.ScaleGrid(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("I was clicked");
    }

    public void ExitMissionEditor()
    {
        MissionEditorFunctions.ExitMissionEditor(this);
    }



}
