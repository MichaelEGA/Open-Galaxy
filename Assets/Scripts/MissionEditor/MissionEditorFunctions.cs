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
        node.nodeType = missionEditor.selectedNodeTypeToLoad;
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

}
