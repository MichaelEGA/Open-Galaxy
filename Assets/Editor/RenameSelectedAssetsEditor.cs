using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class RenameSelectedAssetsEditor : EditorWindow
{
    private string baseName = "NewGamePrefab"; // Default base name

    [UnityEditor.MenuItem("Tools/Rename Selected")]
    public static void ShowWindow()
    {
        GetWindow<RenameSelectedAssetsEditor>("Enumerate Rename Assets");
    }

    void OnGUI()
    {
        GUILayout.Label("Rename Selected Assets", EditorStyles.boldLabel);

        baseName = EditorGUILayout.TextField("Base Name:", baseName);

        if (GUILayout.Button("Rename Selected Files"))
        {
            RenameSelectedFiles();
        }

        EditorGUILayout.HelpBox("Select the files/assets you want to rename in the Project window before clicking 'Rename Selected Files'. They will be enumerated starting from 01.", MessageType.Info);
    }

    void RenameSelectedFiles()
    {
        // Get all selected assets in the Project window
        Object[] selectedAssets = Selection.objects;

        if (selectedAssets == null || selectedAssets.Length == 0)
        {
            EditorUtility.DisplayDialog("No Assets Selected", "Please select one or more assets in the Project window to rename.", "OK");
            return;
        }

        // Filter out non-asset objects (e.g., scene objects) and sort them by name for consistent enumeration
        List<Object> assetsToRename = selectedAssets
            .Where(obj => AssetDatabase.Contains(obj))
            .OrderBy(obj => obj.name) // Sort to ensure consistent numbering
            .ToList();

        if (assetsToRename.Count == 0)
        {
            EditorUtility.DisplayDialog("No Assets Selected", "Please select one or more *assets* in the Project window to rename. Selected objects were not valid assets.", "OK");
            return;
        }

        // Start enumeration from 1
        int enumerationIndex = 1;

        // Group renames by path to avoid conflicts with AssetDatabase.RenameAsset
        // when multiple assets are in the same folder and get renamed quickly.
        var renameQueue = new Queue<(string oldPath, string newPath)>();

        foreach (Object asset in assetsToRename)
        {
            string oldPath = AssetDatabase.GetAssetPath(asset);
            if (string.IsNullOrEmpty(oldPath))
            {
                Debug.LogWarning($"Skipping {asset.name} as its path could not be determined.");
                continue;
            }

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(oldPath);
            string fileExtension = Path.GetExtension(oldPath);
            string directory = Path.GetDirectoryName(oldPath);

            // Format the new name with leading zeros (e.g., 01, 02, ..., 10, 11)
            string newFileName = $"{baseName}{enumerationIndex:D2}{fileExtension}";
            string newPath = Path.Combine(directory, newFileName);

            // Check for existing asset at the new path to prevent overwriting
            if (AssetDatabase.LoadAssetAtPath<Object>(newPath) != null)
            {
                Debug.LogWarning($"Skipping rename of '{fileNameWithoutExtension}' to '{newFileName}' as an asset already exists at '{newPath}'. Please ensure unique names or clear existing assets if intended.");
            }
            else
            {
                renameQueue.Enqueue((oldPath, newPath));
                Debug.Log($"Queueing rename from '{oldPath}' to '{newPath}'");
            }

            enumerationIndex++;
        }

        // Process the rename queue
        while (renameQueue.Count > 0)
        {
            var (oldPath, newPath) = renameQueue.Dequeue();
            string result = AssetDatabase.RenameAsset(oldPath, Path.GetFileNameWithoutExtension(newPath));

            if (!string.IsNullOrEmpty(result))
            {
                Debug.LogError($"Failed to rename '{oldPath}' to '{newPath}': {result}");
            }
            else
            {
                // If renamed successfully, try to update the reference in the Selection if possible
                // This is mainly for feedback, as AssetDatabase.RenameAsset handles actual asset paths.
                AssetDatabase.Refresh(); // Refresh to ensure AssetDatabase is up-to-date
            }
        }

        Debug.Log("Rename operation completed.");
        AssetDatabase.Refresh(); // Final refresh to ensure all changes are reflected
        this.Close(); // Close the window after completion
    }
}