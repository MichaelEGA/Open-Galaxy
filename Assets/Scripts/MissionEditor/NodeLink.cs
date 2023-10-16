using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NodeLink : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Node node;

    public NodeLink connectedNode;
    public string mode = "male";
    public GameObject line;
    public Text textbox;

    MissionEditor missionEditor;
    Vector2 targetPosition;
    bool dragging;
    bool distanceChecked;

    // Start is called before the first frame update
    void Start()
    {
        missionEditor = MissionEditorFunctions.GetMissionEditor();

        if (missionEditor.nodeLinks == null)
        {
            missionEditor.nodeLinks = new List<NodeLink>();
        }

        missionEditor.nodeLinks.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (connectedNode != null & textbox != null)
        {
            textbox.text = connectedNode.node.eventID.text;
        }      
        else if (textbox != null)
        {
            textbox.text = "";
        }
    }

    void OnGUI()
    {
        if (mode == "male")
        {
            if (dragging == true)
            {
                if (line != null)
                {
                    line.SetActive(true);
                }

                targetPosition = transform.parent.parent.InverseTransformPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
                Vector2 currentPos = transform.parent.parent.InverseTransformPoint(transform.position);

                DrawLine(currentPos, targetPosition, 2);

                distanceChecked = false;
            }
            else if (dragging == false & distanceChecked == false)
            {
                foreach (NodeLink nodeLink in missionEditor.nodeLinks)
                {
                    Vector2 targetNodeLink = transform.parent.parent.InverseTransformPoint(nodeLink.transform.position);

                    float distance = Vector2.Distance(targetPosition, targetNodeLink);

                    if (distance < 10)
                    {
                        connectedNode = nodeLink;
                        break;
                    }
                    else
                    {
                        connectedNode = null;
                    }
                }

                distanceChecked = true;
            }
            else if (connectedNode != null)
            {
                Vector2 currentPos = transform.parent.parent.InverseTransformPoint(transform.position);
                Vector2 currentTargetPos = transform.parent.parent.InverseTransformPoint(connectedNode.transform.position);
                DrawLine(currentPos, currentTargetPos, 2);
            }
            else if (line != null)
            {
                line.SetActive(false);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
    }

    public void OnDrag(PointerEventData eventData) //This is needed otherwise mouse it is not possible to drag the mouse
    {

    }

    //Makes a line on the ui canvas between two points
    public void DrawLine(Vector2 firstlocalpoint, Vector2 secondlocalpoint, float lineWidth)
    {
        if (line == null)
        {
            line = new GameObject();
            Image NewImage = line.AddComponent<Image>();        
            line.transform.SetParent(missionEditor.editorContentRect.transform);
            var tempColor = NewImage.color;
            tempColor.a = 0.5f;
            NewImage.color = tempColor;
            line.name = "link";
        }

        RectTransform rect = line.GetComponent<RectTransform>();

        float ax = firstlocalpoint.x;
        float ay = firstlocalpoint.y;

        float bx = secondlocalpoint.x;
        float by = secondlocalpoint.y;

        rect.localScale = Vector3.one;
        //Vector2 graphScale = missionEditor.editorContentRect.transform.localScale;
        Vector2 graphScale = Vector3.one;

        Vector3 a = new Vector3(ax * graphScale.x, ay * graphScale.y, 0);
        Vector3 b = new Vector3(bx * graphScale.x, by * graphScale.y, 0);

        rect.localPosition = (a + b) / 2;
        Vector3 dif = a - b;
        rect.sizeDelta = new Vector3(dif.magnitude, lineWidth);
        rect.rotation = Quaternion.Euler(new Vector3(0, 0, 180 * Mathf.Atan(dif.y / dif.x) / Mathf.PI));
    }

}
