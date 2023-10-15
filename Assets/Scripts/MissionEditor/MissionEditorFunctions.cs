using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class MissionEditorFunctions
{

    public static void SelectNodeType(string nodeType)
    {
        MissionEditor missionEditor = GetMissionEditor();

        missionEditor.selectedNodeTypeToLoad = nodeType;

        if (missionEditor.AddNodeTextBox == null)
        {
            GameObject AddNodeTextBoxGO = GameObject.Find("AddNodeTextBox");

            if (AddNodeTextBoxGO != null)
            {
                missionEditor.AddNodeTextBox = AddNodeTextBoxGO.GetComponentInChildren<Text>();
            }
        }

        if (missionEditor.AddNodeTextBox != null)
        {
            if (nodeType == "Test Node")
            {
                missionEditor.AddNodeTextBox.text = "This node is used to test the node system during development";
            }
            else if (nodeType == "Start Node")
            {
                missionEditor.AddNodeTextBox.text = "This is the first node the game looks for and is used to indicate what event is to be run first.";
            }
            else
            {
                missionEditor.AddNodeTextBox.text = "";
            }
        }     
    }

    public static void AddNode()
    {
        MissionEditor missionEditor = GetMissionEditor();

        if (missionEditor.nodes ==  null)
        {
            missionEditor.nodes = new List<Node>();
        }
        
        GameObject editorContent = GameObject.Find("EditorContent");

        GameObject node1GO = new GameObject();
        node1GO.transform.SetParent(editorContent.transform);
        Node node = node1GO.AddComponent<Node>();
        node.eventType = missionEditor.selectedNodeTypeToLoad;
        missionEditor.nodes.Add(node);
    }

    public static void ScaleGrid(MissionEditor missionEditor)
    {
        missionEditor.scale += Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * 20;

        if (missionEditor.editorContentRect != null)
        {
            missionEditor.editorContentRect.localScale = new Vector3(missionEditor.scale, missionEditor.scale);
        }
    }

    public static void SetWindowMode(MissionEditor missionEditor)
    {
        OGSettings settings = OGSettingsFunctions.GetSettings();

        if (settings.editorWindowMode == "fullscreen")
        {
            int widthRes = Screen.currentResolution.width;
            int heightRes = Screen.currentResolution.height;
            Screen.SetResolution(widthRes, heightRes, FullScreenMode.FullScreenWindow);
        }
        else if (settings.editorWindowMode == "window")
        {
            int widthRes = Screen.currentResolution.width / 2;
            int heightRes = Screen.currentResolution.height / 2;
            Screen.SetResolution(widthRes, heightRes, FullScreenMode.Windowed);
        }
    }

    public static void ExitMissionEditor()
    {
        MissionEditor missionEditor = GetMissionEditor();

        OGSettings settings = OGSettingsFunctions.GetSettings();

        OGSettingsFunctions.SetGameWindowMode(settings.gameWindowMode);

        if (missionEditor != null)
        {
            missionEditor.gameObject.SetActive(false);
        }
    }

    public static MissionEditor GetMissionEditor()
    {
        MissionEditor missionEditor = GameObject.FindObjectOfType<MissionEditor>();

        return missionEditor;
    }

    public static void ToggleScrolling(MissionEditor missionEditor)
    {
        if (missionEditor.scrolling == true)
        {
            missionEditor.scrollRect.horizontal = true;
            missionEditor.scrollRect.vertical = true;
        }
        else
        {
            missionEditor.scrollRect.horizontal = false;
            missionEditor.scrollRect.vertical = false;
        }
    }

    //This passes the data from text boxes back into a format that can be easily handled
    public static MissionEvent[] SaveNodeData(MissionEditor missionEditor)
    {
        List<MissionEvent> missionEvents = new List<MissionEvent>();

        foreach (Node node in missionEditor.nodes)
        {
            MissionEvent missionEvent = new MissionEvent();

            missionEvent.eventID = node.eventID;
            missionEvent.eventType = node.eventType;

            if (node.conditionTimeText != null)
            {
                if (int.TryParse(node.conditionTimeText.text, out _))
                {
                    missionEvent.conditionTime = int.Parse(node.conditionTimeText.text);
                }
            }

            if (node.conditionLocationText != null)
            {
                missionEvent.conditionLocation = node.conditionLocationText.text;
            }

            if (node.xText != null)
            {
                if (int.TryParse(node.xText.text, out _))
                {
                    missionEvent.x = int.Parse(node.xText.text);
                }
            }

            if (node.yText != null)
            {
                if (int.TryParse(node.yText.text, out _))
                {
                    missionEvent.y = int.Parse(node.yText.text);
                }
            }

            if (node.zText != null)
            {
                if (int.TryParse(node.zText.text, out _))
                {
                    missionEvent.z = int.Parse(node.zText.text);
                }
            }

            if (node.xRotationText != null)
            {
                if (int.TryParse(node.xRotationText.text, out _))
                {
                    missionEvent.xRotation = int.Parse(node.xRotationText.text);
                }
            }

            if (node.yRotationText != null)
            {
                if (int.TryParse(node.yRotationText.text, out _))
                {
                    missionEvent.yRotation = int.Parse(node.yRotationText.text);
                }
            }

            if (node.zRotationText != null)
            {
                if (int.TryParse(node.zRotationText.text, out _))
                {
                    missionEvent.zRotation = int.Parse(node.zRotationText.text);
                }
            }

            if (node.data1Text != null)
            {
                missionEvent.data1 = node.data1Text.text;
            }

            if (node.data2Text != null)
            {
                missionEvent.data2 = node.data2Text.text;
            }

            if (node.data3Text != null)
            {
                missionEvent.data3 = node.data3Text.text;
            }

            if (node.data4Text != null)
            {
                missionEvent.data4 = node.data4Text.text;
            }

            if (node.data5Text != null)
            {
                missionEvent.data5 = node.data5Text.text;
            }

            if (node.data6Text != null)
            {
                missionEvent.data6 = node.data6Text.text;
            }

            if (node.data7Text != null)
            {
                missionEvent.data7 = node.data7Text.text;
            }

            if (node.data8Text != null)
            {
                missionEvent.data8 = node.data8Text.text;
            }

            if (node.data9Text != null)
            {
                missionEvent.data9 = node.data9Text.text;
            }

            if (node.data10Text != null)
            {
                missionEvent.data10 = node.data10Text.text;
            }

            if (node.data11Text != null)
            {
                missionEvent.data11 = node.data11Text.text;
            }

            if (node.data12Text != null)
            {
                missionEvent.data12 = node.data12Text.text;
            }

            if (node.data13Text != null)
            {
                missionEvent.data13 = node.data13Text.text;
            }

            if (node.data14Text != null)
            {
                missionEvent.data14 = node.data14Text.text;
            }

            if (node.data15Text != null)
            {
                missionEvent.data15 = node.data15Text.text;
            }

            if (node.nextEvent1Text != null)
            {
                missionEvent.nextEvent1 = node.nextEvent1Text.text;
            }

            if (node.nextEvent2Text != null)
            {
                missionEvent.nextEvent2 = node.nextEvent2Text.text;
            }

            if (node.nextEvent3Text != null)
            {
                missionEvent.nextEvent3 = node.nextEvent3Text.text;
            }

            if (node.nextEvent4Text != null)
            {
                missionEvent.nextEvent4 = node.nextEvent4Text.text;
            }

            missionEvent.nodePosX = node.transform.localPosition.x;
            missionEvent.nodePosY = node.transform.localPosition.y;

            missionEvents.Add(missionEvent);
        }

        return missionEvents[];
    }

}
