using UnityEditor;
using UnityEngine;
using System.IO;

public class LowercaseAssetNames : Editor
{
    [UnityEditor.MenuItem("Tools/Make Names Lowercase", false, 20)]
    private static void MakeNamesLowercase()
    {
        // Get the selected assets
        Object[] selectedAssets = Selection.objects;

        if (selectedAssets.Length == 0)
        {
            Debug.LogWarning("No assets selected!");
            return;
        }

        foreach (Object asset in selectedAssets)
        {
            string assetPath = AssetDatabase.GetAssetPath(asset);
            string directory = Path.GetDirectoryName(assetPath);
            string fileName = Path.GetFileNameWithoutExtension(assetPath);
            string extension = Path.GetExtension(assetPath);

            if (!string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(extension))
            {
                string lowercaseFileName = fileName.ToLower();
                string newPath = Path.Combine(directory, lowercaseFileName + extension).Replace("\\", "/");

                if (newPath != assetPath)
                {
                    AssetDatabase.RenameAsset(assetPath, lowercaseFileName);
                    Debug.Log($"Renamed {assetPath} to {newPath}");
                }
            }
        }

        // Refresh the AssetDatabase to reflect the changes
        AssetDatabase.Refresh();
    }
}