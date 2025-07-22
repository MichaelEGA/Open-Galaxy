using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class ApplyMaterialToMeshRenderersEditor : EditorWindow
{
    private Material selectedMaterial;

    [UnityEditor.MenuItem("Tools/Apply Material to MeshRenderers")]
    public static void ShowWindow()
    {
        GetWindow<ApplyMaterialToMeshRenderersEditor>("Apply Material");
    }

    void OnGUI()
    {
        GUILayout.Label("Apply Material to MeshRenderers", EditorStyles.boldLabel);

        // Allow dragging and dropping a material
        selectedMaterial = (Material)EditorGUILayout.ObjectField(
            "Material to Apply:",
            selectedMaterial,
            typeof(Material),
            false // False because we don't want to allow scene objects
        );

        EditorGUILayout.Space();

        if (GUILayout.Button("Apply Material to Selected Objects"))
        {
            ApplyMaterial();
        }

        EditorGUILayout.HelpBox(
            "Select the GameObjects in your Hierarchy that contain MeshRenderers you want to modify. " +
            "Then, drag and drop the desired Material into the field above and click 'Apply Material'.",
            MessageType.Info
        );
    }

    void ApplyMaterial()
    {
        if (selectedMaterial == null)
        {
            EditorUtility.DisplayDialog("No Material Selected", "Please select a Material in the 'Material to Apply' field.", "OK");
            return;
        }

        // Get all selected GameObjects in the Hierarchy
        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects == null || selectedObjects.Length == 0)
        {
            EditorUtility.DisplayDialog("No Objects Selected", "Please select one or more GameObjects in the Hierarchy.", "OK");
            return;
        }

        Undo.RecordObjects(selectedObjects, "Apply Material to MeshRenderers"); // For Undo functionality

        foreach (GameObject go in selectedObjects)
        {
            // Get all MeshRenderers on the current GameObject and its children
            MeshRenderer[] meshRenderers = go.GetComponentsInChildren<MeshRenderer>(true); // 'true' to include inactive children

            if (meshRenderers.Length == 0)
            {
                Debug.LogWarning($"No MeshRenderers found on GameObject '{go.name}' or its children. Skipping.");
                continue;
            }

            foreach (MeshRenderer renderer in meshRenderers)
            {
                // Record the renderer for Undo before modifying its material
                Undo.RecordObject(renderer, $"Set Material for {renderer.gameObject.name}");

                // Assign the selected material.
                // Using 'renderer.sharedMaterial = selectedMaterial;' will directly assign
                // the chosen material asset. This means if multiple objects currently
                // use different materials, they will all *now* share this one material asset.
                // If you want to create a unique instance for each renderer, you'd need to
                // instantiate the material first (e.g., new Material(selectedMaterial))
                // and then assign it to renderer.material, but typically for batch
                // material application, sharedMaterial is the desired behavior.
                renderer.sharedMaterial = selectedMaterial;
                Debug.Log($"Applied '{selectedMaterial.name}' to MeshRenderer on '{renderer.gameObject.name}'.");
            }
        }

        // Mark the scene as dirty to ensure changes are saved with the scene/prefab
        UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();

        Debug.Log("Material application process completed.");
        this.Close(); // Close the window after completion
    }
}