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

    // Start is called before the first frame update
    void Start()
    {
        GameObject EditorContentGo = GameObject.Find("EditorContent");
        EditorContentRect = EditorContentGo.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void OnGUI()
    {
        scale += Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * 20;

        if (EditorContentRect != null)
        {
            EditorContentRect.localScale = new Vector3(scale, scale);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("I was clicked");
    }

}
