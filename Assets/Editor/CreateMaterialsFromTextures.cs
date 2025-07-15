using UnityEditor;
using UnityEngine;
using System.IO;

public class CreateMaterialsFromTextures
{
    [UnityEditor.MenuItem("Tools/Standard Materials From Textures")]
    public static void CreateStandardMaterials()
    {
        // Get selected textures in the project window
        Object[] textures = Selection.GetFiltered(typeof(Texture), SelectionMode.Assets);

        if (textures.Length == 0)
        {
            EditorUtility.DisplayDialog("Create Materials", "No textures selected!", "OK");
            return;
        }

        string materialFolder = "Assets/GeneratedMaterials";
        if (!AssetDatabase.IsValidFolder(materialFolder))
        {
            AssetDatabase.CreateFolder("Assets", "GeneratedMaterials");
        }

        foreach (Object texObj in textures)
        {
            Texture texture = texObj as Texture;
            if (texture == null) continue;

            // Generate material name and path
            string texPath = AssetDatabase.GetAssetPath(texture);
            string texName = Path.GetFileNameWithoutExtension(texPath);
            string matPath = $"{materialFolder}/{texName}_Mat.mat";

            // Create a Standard shader material
            Material material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            material.mainTexture = texture;

            // Optionally set other Standard shader properties here, e.g. metallic, smoothness

            // Save material asset
            AssetDatabase.CreateAsset(material, matPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("Create Materials", $"Created {textures.Length} materials in {materialFolder}.", "OK");
    }
}