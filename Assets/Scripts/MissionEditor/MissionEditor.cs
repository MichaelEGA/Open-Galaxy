using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MissionEditor : MonoBehaviour
{
    public List<Node> nodes;
    public List<Menu> menus;
    public Canvas canvas;
    public RectTransform editorContentRect;
    public ScrollRect scrollRect;
    public float scale = 1;
    public string gameWindowMode;
    public bool scrolling;

    void Start()
    {
        GameObject editorContentGo = GameObject.Find("EditorContent");
        editorContentRect = editorContentGo.GetComponent<RectTransform>();
        scrollRect = editorContentGo.GetComponentInParent<ScrollRect>();

        MissionEditorFunctions.SetWindowMode(this);

        nodes = new List<Node>();
        GameObject editorContent = GameObject.Find("EditorContent");

        GameObject node1GO = new GameObject();
        node1GO.transform.SetParent(editorContent.transform);
        Node node = node1GO.AddComponent<Node>();
        node.nodeType = "testnode";
        nodes.Add(node);

        menus = new List<Menu>();

        GameObject menu1GO = new GameObject();
        menu1GO.transform.SetParent(gameObject.transform);
        Menu menu1 = menu1GO.AddComponent<Menu>();
        menu1.menuType = "mainmenu";
        menus.Add(menu1);
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void OnGUI()
    {
        MissionEditorFunctions.ScaleGrid(this);
        MissionEditorFunctions.ToggleScrolling(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("I was clicked");
    }
}
