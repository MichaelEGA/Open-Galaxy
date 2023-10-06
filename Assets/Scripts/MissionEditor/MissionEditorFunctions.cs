using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MissionEditorFunctions
{

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

    public static void ExitMissionEditor(MissionEditor missionEditor)
    {
        OGSettings settings = OGSettingsFunctions.GetSettings();

        OGSettingsFunctions.SetGameWindowMode(settings.gameWindowMode);

        missionEditor.gameObject.SetActive(false);

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
