using UnityEngine;
using System.Collections.Generic;

public class MaterialEmissionManager : MonoBehaviour
{
    private List<Material> materialsWithEmission = new List<Material>();

    /// <summary>
    /// Finds all materials on the prefab/GameObject and disables emission on those that have it enabled.
    /// Stores the materials with emission in a list for later re-enabling.
    /// </summary>
    public void DisableEmissionOnPrefab()
    {
        materialsWithEmission.Clear();

        // Get all renderers on this GameObject and its children
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            // Get all materials from the renderer
            Material[] materials = renderer.materials;

            foreach (Material material in materials)
            {
                // Check if the material has emission enabled
                if (material.IsKeywordEnabled("_EMISSION"))
                {
                    // Add to our list to track it
                    materialsWithEmission.Add(material);

                    // Disable emission
                    material.DisableKeyword("_EMISSION");
                }
            }
        }

        Debug.Log($"Disabled emission on {materialsWithEmission.Count} materials");
    }

    /// <summary>
    /// Re-enables emission on all materials that were previously stored in the list.
    /// </summary>
    public void EnableEmissionOnStoredMaterials()
    {
        foreach (Material material in materialsWithEmission)
        {
            if (material != null)
            {
                material.EnableKeyword("_EMISSION");
            }
        }

        Debug.Log($"Re-enabled emission on {materialsWithEmission.Count} materials");
    }

    /// <summary>
    /// Clears the stored materials list.
    /// </summary>
    public void ClearStoredMaterials()
    {
        materialsWithEmission.Clear();
    }

    /// <summary>
    /// Returns the count of materials with emission that were stored.
    /// </summary>
    public int GetStoredEmissionMaterialCount()
    {
        return materialsWithEmission.Count;
    }
}