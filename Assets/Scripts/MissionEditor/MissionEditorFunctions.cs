using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public static class MissionEditorFunctions
{

    #region draw mission editor

    public static void Draw_MissionEditor(MissionEditor missionEditor)
    {
        Draw_ScaleIndicator(missionEditor);
    }

    public static void Draw_ScaleIndicator(MissionEditor missionEditor)
    {
        //This draws the input label
        GameObject titleGO = new GameObject();

        titleGO.transform.SetParent(missionEditor.transform);
        RectTransform rectTransform = titleGO.AddComponent<RectTransform>();
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.pivot = new Vector2(0, 0);
        rectTransform.anchoredPosition = new Vector2(10, 10);
        rectTransform.sizeDelta = new Vector2(90, 12.5f);
        rectTransform.localScale = new Vector3(1, 1, 1);

        Text text = titleGO.AddComponent<Text>();
        text.supportRichText = false;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.text = "100%";
        text.fontSize = 8;
        text.color = Color.white;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.alignment = TextAnchor.MiddleLeft;

        titleGO.name = "Scale Indicator";

        missionEditor.scaleIndicator = text;
    }

    #endregion

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
            if (nodeType == "custom_node")
            {
                missionEditor.AddNodeTextBox.text = "This node is for manually inputing event information. If a node doesn't exist for a specific event or function you can use the custom node to access it.";
            }
            else if (nodeType == "preload_loadasteroids")
            {
                missionEditor.AddNodeTextBox.text = "Use this node to generate an asteroid field before the mission starts. Preload functions will automatically execute before the mission starts and do not need to be linked to any other events.";
            }
            else
            {
                missionEditor.AddNodeTextBox.text = "";
            }
        }
    }

    public static void SelectMission(string mission)
    {
        MissionEditor missionEditor = GetMissionEditor();

        missionEditor.selectedMissionToLoad = mission;
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
        bool scaling = true;

        //Checks whether mouse is in position to scale
        foreach (Menu menu in missionEditor.menus)
        {
            if (menu.scaling == false)
            {
                scaling = false;
            }
        }

        //This changes the scale
        if (scaling == true)
        {
            missionEditor.scale += Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * 20;
        }

        //This locks the scale within certain bounds
        float percentage = ((Mathf.Abs(missionEditor.scale) - 0.3f) / 0.7f) * 100;

        if (percentage > 200)
        {
            missionEditor.scale = 1.7f;
            percentage = 200;
        }
        else if (percentage < 0)
        {
            missionEditor.scale = 0.3f;
            percentage = 0;
        }

        //This applies the scale
        if (scaling == true)
        {
            if (missionEditor.editorContentRect != null)
            {
                missionEditor.editorContentRect.localScale = new Vector3(missionEditor.scale, missionEditor.scale);
            }
        }

        //This outputs the scale to the indicator
        if (missionEditor.scaleIndicator != null)
        {
            missionEditor.scaleIndicator.text = percentage.ToString("000") + "%";
        }
    }

    public static void SetWindowMode(MissionEditor missionEditor)
    {
        OGSettings settings = OGSettingsFunctions.GetSettings();

        OGSettingsFunctions.SetEditorWindowMode(settings.editorWindowMode);
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

    public static void LoadMission()
    {
        MissionEditor missionEditor = GetMissionEditor();

        if (missionEditor != null)
        {
            string missionAddress = Application.persistentDataPath + "/Custom Missions/" + missionEditor.selectedMissionToLoad;
            string missionDataString = File.ReadAllText(missionAddress);
            TextAsset missionDataTextAsset = new TextAsset(missionDataString);
            Mission mission = JsonUtility.FromJson<Mission>(missionDataTextAsset.text);
            Task a = new Task(LoadMissionData(mission));
        }
    }

    public static IEnumerator LoadMissionData(Mission mission)
    {
        foreach (MissionEvent missionEvent in mission.missionEventData)
        {
            Node node = AddNode(missionEvent.eventType, true, missionEvent.nodePosX, missionEvent.nodePosY);

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

        foreach (MissionEvent missionEvent in mission.missionEventData)
        {
            MissionEditor missionEditor = GetMissionEditor();

            Node firstNode = SearchNodes(missionEvent.eventID);

            if (firstNode != null)
            {
                if (firstNode.maleNodeLinks != null)
                {
                    if (missionEvent.nextEvent1 != "none" & firstNode.maleNodeLinks.Count > 0)
                    {
                        Node nextEvent1 = SearchNodes(missionEvent.nextEvent1);

                        if (nextEvent1 != null & firstNode.maleNodeLinks[0] != null)
                        {
                            if (nextEvent1.femaleNodeLink != null)
                            {
                                firstNode.maleNodeLinks[0].connectedNode = nextEvent1.femaleNodeLink;
                            }
                        }
                    }

                    if (missionEvent.nextEvent1 != "none" & firstNode.maleNodeLinks.Count > 1)
                    {
                        Node nextEvent2 = SearchNodes(missionEvent.nextEvent2);

                        if (nextEvent2 != null & firstNode.maleNodeLinks[1] != null)
                        {
                            if (nextEvent2.femaleNodeLink != null)
                            {
                                firstNode.maleNodeLinks[1].connectedNode = nextEvent2.femaleNodeLink;
                            }
                        }
                    }

                    if (missionEvent.nextEvent1 != "none" & firstNode.maleNodeLinks.Count > 2)
                    {
                        Node nextEvent3 = SearchNodes(missionEvent.nextEvent3);

                        if (nextEvent3 != null & firstNode.maleNodeLinks[2] != null)
                        {
                            if (nextEvent3.femaleNodeLink != null)
                            {
                                firstNode.maleNodeLinks[2].connectedNode = nextEvent3.femaleNodeLink;
                            }
                        }
                    }

                    if (missionEvent.nextEvent1 != "none" & firstNode.maleNodeLinks.Count > 3)
                    {
                        Node nextEvent4 = SearchNodes(missionEvent.nextEvent4);

                        if (nextEvent4 != null & firstNode.maleNodeLinks[3] != null)
                        {
                            if (nextEvent4.femaleNodeLink != null)
                            {
                                firstNode.maleNodeLinks[3].connectedNode = nextEvent4.femaleNodeLink;
                            }
                        }
                    }
                }


            }
        }
    }

    public static Node SearchNodes(string eventID)
    {
        MissionEditor missionEditor = GetMissionEditor();

        Node node = null;

        if (missionEditor != null)
        {
            if (missionEditor.nodes != null)
            {
                foreach (Node tempNode in missionEditor.nodes)
                {
                    if (tempNode != null)
                    {
                        if (tempNode.eventID != null)
                        {
                            if (tempNode.eventID.text == eventID)
                            {
                                node = tempNode;
                            }
                        }
                    }
                }
            }
        }

        return node;
    }

    public static void InputData(Text text, string input)
    {
        if (text != null)
        {
            InputField inputField = text.GetComponentInParent<InputField>();

            if (inputField != null)
            {
                inputField.text = input;
            }

            text.text = input;
        }
    }

    public static void SaveMission()
    {
        List<MissionEvent> missionList = new List<MissionEvent>();

        MissionEditor missionEditor = GetMissionEditor();

        UpdateMissionName();

        foreach (Node node in missionEditor.nodes)
        {
            if (node != null)
            {

                MissionEvent missionEvent = new MissionEvent();

                if (node.eventID != null)
                {
                    missionEvent.eventID = node.eventID.text;
                }
                else
                {
                    missionEvent.eventID = "none";
                }

                if (node.eventType != null)
                {
                    missionEvent.eventType = node.eventType.text;
                }
                else
                {
                    missionEvent.eventType = "none";
                }

                if (node.conditionLocation != null)
                {
                    missionEvent.conditionLocation = node.conditionLocation.text;
                }
                else
                {
                    missionEvent.conditionLocation = "none";
                }

                if (node.conditionTime != null)
                {
                    missionEvent.conditionTime = float.Parse(node.conditionTime.text);
                }
                else
                {
                    missionEvent.conditionTime = 0;
                }

                if (node.x != null)
                {
                    missionEvent.x = float.Parse(node.x.text);
                }
                else
                {
                    missionEvent.x = 0;
                }

                if (node.y != null)
                {
                    missionEvent.y = float.Parse(node.y.text);
                }
                else
                {
                    missionEvent.y = 0;
                }

                if (node.z != null)
                {
                    missionEvent.z = float.Parse(node.z.text);
                }
                else
                {
                    missionEvent.z = 0;
                }

                if (node.xRotation != null)
                {
                    missionEvent.xRotation = float.Parse(node.xRotation.text);
                }
                else
                {
                    missionEvent.xRotation = 0;
                }

                if (node.yRotation != null)
                {
                    missionEvent.yRotation = float.Parse(node.yRotation.text);
                }
                else
                {
                    missionEvent.yRotation = 0;
                }

                if (node.zRotation != null)
                {
                    missionEvent.zRotation = float.Parse(node.zRotation.text);
                }
                else
                {
                    missionEvent.zRotation = 0;
                }

                if (node.data1 != null)
                {
                    missionEvent.data1 = node.data1.text;
                }
                else
                {
                    missionEvent.data1 = "none";
                }

                if (node.data2 != null)
                {
                    missionEvent.data2 = node.data2.text;
                }
                else
                {
                    missionEvent.data2 = "none";
                }

                if (node.data3 != null)
                {
                    missionEvent.data3 = node.data3.text;
                }
                else
                {
                    missionEvent.data3 = "none";
                }

                if (node.data4 != null)
                {
                    missionEvent.data4 = node.data4.text;
                }
                else
                {
                    missionEvent.data4 = "none";
                }

                if (node.data5 != null)
                {
                    missionEvent.data5 = node.data5.text;
                }
                else
                {
                    missionEvent.data5 = "none";
                }

                if (node.data6 != null)
                {
                    missionEvent.data6 = node.data6.text;
                }
                else
                {
                    missionEvent.data6 = "none";
                }

                if (node.data7 != null)
                {
                    missionEvent.data7 = node.data7.text;
                }
                else
                {
                    missionEvent.data7 = "none";
                }

                if (node.data8 != null)
                {
                    missionEvent.data8 = node.data8.text;
                }
                else
                {
                    missionEvent.data8 = "none";
                }

                if (node.data9 != null)
                {
                    missionEvent.data9 = node.data9.text;
                }
                else
                {
                    missionEvent.data9 = "none";
                }

                if (node.data10 != null)
                {
                    missionEvent.data10 = node.data10.text;
                }
                else
                {
                    missionEvent.data10 = "none";
                }

                if (node.data10 != null)
                {
                    missionEvent.data11 = node.data11.text;
                }
                else
                {
                    missionEvent.data11 = "none";
                }

                if (node.data12 != null)
                {
                    missionEvent.data12 = node.data12.text;
                }
                else
                {
                    missionEvent.data12 = "none";
                }

                if (node.data13 != null)
                {
                    missionEvent.data13 = node.data13.text;
                }
                else
                {
                    missionEvent.data13 = "none";
                }

                if (node.data14 != null)
                {
                    missionEvent.data14 = node.data14.text;
                }
                else
                {
                    missionEvent.data14 = "none";
                }

                if (node.data15 != null)
                {
                    missionEvent.data15 = node.data15.text;
                }
                else
                {
                    missionEvent.data15 = "none";
                }

                if (node.nextEvent1 != null)
                {
                    missionEvent.nextEvent1 = node.nextEvent1.text;
                }
                else
                {
                    missionEvent.nextEvent1 = "none";
                }

                if (node.nextEvent2 != null)
                {
                    missionEvent.nextEvent2 = node.nextEvent2.text;
                }
                else
                {
                    missionEvent.nextEvent2 = "none";
                }

                if (node.nextEvent3 != null)
                {
                    missionEvent.nextEvent3 = node.nextEvent3.text;
                }
                else
                {
                    missionEvent.nextEvent3 = "none";
                }

                if (node.nextEvent4 != null)
                {
                    missionEvent.nextEvent4 = node.nextEvent4.text;
                }
                else
                {
                    missionEvent.nextEvent4 = "none";
                }

                missionEvent.nodePosX = node.nodePosX;
                missionEvent.nodePosY = node.nodePosY;

                missionList.Add(missionEvent);
            }
        }

        MissionEvent[] missionEventData = missionList.ToArray();

        string jsonString = JsonHelper.ToJson(missionEventData, true);

        string saveFile = Application.persistentDataPath + "/Custom Missions/" + missionEditor.missionName + ".json";

        File.WriteAllText(saveFile, jsonString);
    }

    public static void UpdateMissionName()
    {
        GameObject MissionNameField = GameObject.Find("MissionNameField");

        if (MissionNameField != null)
        {
            Text missionName = MissionNameField.GetComponent<Text>();

            MissionEditor missionEditor = GetMissionEditor();

            if (missionEditor != null)
            {
                missionEditor.missionName = missionName.text;
            }
        }
    }

}
