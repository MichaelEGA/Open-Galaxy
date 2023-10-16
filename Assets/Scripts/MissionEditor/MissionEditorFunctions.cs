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

    public static void AddSelectedNodeType()
    {
        MissionEditor missionEditor = GetMissionEditor();
        AddNode(missionEditor.selectedNodeTypeToLoad);
    }

    public static Node AddNode(string nodeType, bool setPosition = false, float nodePosX = 0, float nodePosY = 0)
    {
        MissionEditor missionEditor = GetMissionEditor();

        if (missionEditor.nodes == null)
        {
            missionEditor.nodes = new List<Node>();
        }

        GameObject editorContent = GameObject.Find("EditorContent");

        GameObject nodeGO = new GameObject();
        nodeGO.transform.SetParent(editorContent.transform);
        Node node = nodeGO.AddComponent<Node>();
        node.nodeType = nodeType;
        missionEditor.nodes.Add(node);

        if (setPosition == true)
        {
            NodeFunctions.SetNodePosition(node, nodePosX, nodePosY);
        }

        return node;
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

    public static MissionEvent[] SaveNodeData(MissionEditor missionEditor)
    {
        List<MissionEvent> missionEvents = new List<MissionEvent>();

        foreach (Node node in missionEditor.nodes)
        {
            MissionEvent missionEvent = new MissionEvent();

            if (node.eventID != null)
            {
                missionEvent.eventID = node.eventID.text;
            }

            if (node.eventType != null)
            {
                missionEvent.eventType = node.eventType.text;
            }

            if (node.conditionTime != null)
            {
                if (int.TryParse(node.conditionTime.text, out _))
                {
                    missionEvent.conditionTime = int.Parse(node.conditionTime.text);
                }
            }

            if (node.conditionLocation != null)
            {
                missionEvent.conditionLocation = node.conditionLocation.text;
            }

            if (node.x != null)
            {
                if (int.TryParse(node.x.text, out _))
                {
                    missionEvent.x = int.Parse(node.x.text);
                }
            }

            if (node.y != null)
            {
                if (int.TryParse(node.y.text, out _))
                {
                    missionEvent.y = int.Parse(node.y.text);
                }
            }

            if (node.z != null)
            {
                if (int.TryParse(node.z.text, out _))
                {
                    missionEvent.z = int.Parse(node.z.text);
                }
            }

            if (node.xRotation != null)
            {
                if (int.TryParse(node.xRotation.text, out _))
                {
                    missionEvent.xRotation = int.Parse(node.xRotation.text);
                }
            }

            if (node.yRotation != null)
            {
                if (int.TryParse(node.yRotation.text, out _))
                {
                    missionEvent.yRotation = int.Parse(node.yRotation.text);
                }
            }

            if (node.zRotation != null)
            {
                if (int.TryParse(node.zRotation.text, out _))
                {
                    missionEvent.zRotation = int.Parse(node.zRotation.text);
                }
            }

            if (node.data1 != null)
            {
                missionEvent.data1 = node.data1.text;
            }

            if (node.data2 != null)
            {
                missionEvent.data2 = node.data2.text;
            }

            if (node.data3 != null)
            {
                missionEvent.data3 = node.data3.text;
            }

            if (node.data4 != null)
            {
                missionEvent.data4 = node.data4.text;
            }

            if (node.data5 != null)
            {
                missionEvent.data5 = node.data5.text;
            }

            if (node.data6 != null)
            {
                missionEvent.data6 = node.data6.text;
            }

            if (node.data7 != null)
            {
                missionEvent.data7 = node.data7.text;
            }

            if (node.data8 != null)
            {
                missionEvent.data8 = node.data8.text;
            }

            if (node.data9 != null)
            {
                missionEvent.data9 = node.data9.text;
            }

            if (node.data10 != null)
            {
                missionEvent.data10 = node.data10.text;
            }

            if (node.data11 != null)
            {
                missionEvent.data11 = node.data11.text;
            }

            if (node.data12 != null)
            {
                missionEvent.data12 = node.data12.text;
            }

            if (node.data13 != null)
            {
                missionEvent.data13 = node.data13.text;
            }

            if (node.data14 != null)
            {
                missionEvent.data14 = node.data14.text;
            }

            if (node.data15 != null)
            {
                missionEvent.data15 = node.data15.text;
            }

            if (node.nextEvent1 != null)
            {
                missionEvent.nextEvent1 = node.nextEvent1.text;
            }

            if (node.nextEvent2 != null)
            {
                missionEvent.nextEvent2 = node.nextEvent2.text;
            }

            if (node.nextEvent3 != null)
            {
                missionEvent.nextEvent3 = node.nextEvent3.text;
            }

            if (node.nextEvent4 != null)
            {
                missionEvent.nextEvent4 = node.nextEvent4.text;
            }

            missionEvent.nodePosX = node.transform.localPosition.x;
            missionEvent.nodePosY = node.transform.localPosition.y;

            missionEvents.Add(missionEvent);
        }

        return missionEvents.ToArray();
    }

    public static void LoadMission()
    {
        TextAsset missionDataFile = new TextAsset();
        missionDataFile = Resources.Load("Data/Files/Missions_Main/7 ABY - The Krytos Trap 01 - Corrans Nightmare - Part 1") as TextAsset;
        Mission mission = JsonUtility.FromJson<Mission>(missionDataFile.text);
        Task a = new Task(LoadMissionData(mission));
    }

    public static IEnumerator LoadMissionData(Mission mission)
    {
        float xPos = -900;
        float yPos = 0;
        float count = 0;

        foreach (MissionEvent missionEvent in mission.missionEventData)
        {
            Node node = AddNode("Custom Node", true, xPos, yPos);

            xPos += 110;
            count += 1;

            if (count > 14)
            {
                count = 0;
                yPos -= 500;
                xPos = -900;
            }            

            yield return new WaitForEndOfFrame();

            InputData(node.eventID, missionEvent.eventID);
            InputData(node.eventType, missionEvent.eventType);
            InputData(node.conditionTime, missionEvent.conditionTime.ToString());
            InputData(node.conditionLocation, missionEvent.conditionLocation);
            InputData(node.x, missionEvent.x.ToString());
            InputData(node.y, missionEvent.y.ToString());
            InputData(node.z, missionEvent.z.ToString());
            InputData(node.xRotation, missionEvent.xRotation.ToString());
            InputData(node.yRotation, missionEvent.yRotation.ToString());
            InputData(node.zRotation, missionEvent.zRotation.ToString());
            InputData(node.data1, missionEvent.data1);
            InputData(node.data2, missionEvent.data2);
            InputData(node.data3, missionEvent.data3);
            InputData(node.data4, missionEvent.data4);
            InputData(node.data5, missionEvent.data5);
            InputData(node.data6, missionEvent.data6);
            InputData(node.data7, missionEvent.data7);
            InputData(node.data8, missionEvent.data8);
            InputData(node.data9, missionEvent.data9);
            InputData(node.data10, missionEvent.data10);
            InputData(node.data11, missionEvent.data11);
            InputData(node.data12, missionEvent.data12);
            InputData(node.data13, missionEvent.data13);
            InputData(node.data14, missionEvent.data14);
            InputData(node.data15, missionEvent.data5);
            InputData(node.nextEvent1, missionEvent.nextEvent1);
            InputData(node.nextEvent2, missionEvent.nextEvent2);
            InputData(node.nextEvent3, missionEvent.nextEvent3);
            InputData(node.nextEvent4, missionEvent.nextEvent4);
            node.nodePosX = missionEvent.nodePosX;
            node.nodePosY = missionEvent.nodePosY;
        }
    }

    public static void InputData(Text text, string input)
    {
        text.text = input;

        InputField inputField = text.GetComponent<InputField>();

        if(inputField != null)
        {
            inputField.text = input;
        }
    }

}
