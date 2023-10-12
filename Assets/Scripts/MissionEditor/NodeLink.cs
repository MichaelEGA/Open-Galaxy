using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NodeLink : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public NodeLink connectedNode;
    public string nodeID;
    public string mode = "male"; 

    MissionEditor missionEditor;
    LineRenderer line;
    Vector2 targetPosition;
    bool dragging;
    bool distanceChecked;

    // Start is called before the first frame update
    void Start()
    {
        missionEditor = MissionEditorFunctions.GetMissionEditor();
    }

    // Update is called once per frame
    void Update()
    {
        if (connectedNode != null)
        {
            nodeID = connectedNode.nodeID;
        }      
        else
        {
            nodeID = "";
        }
    }

    void OnGUI()
    {
        if (mode == "male")
        {
            if (dragging == true)
            {
                if (line == null)
                {
                    line = GetComponent<LineRenderer>();
                }

                targetPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

                Vector3[] pathPoints = { transform.position, targetPosition };
                line.positionCount = 2;
                line.SetPositions(pathPoints);

                distanceChecked = false;
            }
            else if (dragging == false & distanceChecked == false)
            {
                foreach (NodeLink nodeLink in missionEditor.nodeLinks)
                {
                    float distance = Vector2.Distance(targetPosition, nodeLink.transform.position);

                    if (distance < 2)
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
                Vector3[] pathPoints = { transform.position, connectedNode.transform.position };
                line.positionCount = 2;
                line.SetPositions(pathPoints);
            }
            else
            {
                line.positionCount = 0;
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

}
