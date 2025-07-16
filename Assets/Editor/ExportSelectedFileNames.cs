using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;

public class ExportSelectedFileNames
{
    [UnityEditor.MenuItem("Tools/Export Selected File Names to TXT")]
    public static void ExportNamesToTxt()
    {
        // Get selected asset GUIDs
        var guids = Selection.assetGUIDs;
        if (guids == null || guids.Length == 0)
        {
            EditorUtility.DisplayDialog("Export File Names", "No files selected!", "OK");
            return;
        }

        // Get asset paths and names
        var assetPaths = guids.Select(guid => AssetDatabase.GUIDToAssetPath(guid)).ToArray();
        var fileNames = assetPaths.Select(path => Path.GetFileName(path)).ToArray();

        // Ask for save location
        string savePath = EditorUtility.SaveFilePanel(
            "Save File Names As TXT",
            Application.dataPath,
            "SelectedFileNames.txt",
            "txt"
        );

        if (string.IsNullOrEmpty(savePath))
            return;

        // Write file names to txt
        File.WriteAllLines(savePath, fileNames);

        EditorUtility.RevealInFinder(savePath);
        EditorUtility.DisplayDialog("Export File Names", $"Exported {fileNames.Length} file names to:\n{savePath}", "OK");
    }
}