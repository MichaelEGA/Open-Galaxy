using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectUtils
{
    //This applys a certain material to all meshes under a given gameobject
    public static void ApplyMaterialToAllMeshes(GameObject gameObject, Material material)
    {
        if (gameObject == null || material == null)
        {
            Debug.LogWarning("Root GameObject or Material is null. Aborting material application.");
            return;
        }

        // Get all MeshRenderer components in the hierarchy (including inactive ones)
        MeshRenderer[] renderers = gameObject.GetComponentsInChildren<MeshRenderer>(true);

        foreach (MeshRenderer renderer in renderers)
        {
            renderer.material = material;
        }
    }

    //TRANSFORM AND GAMEOBJECT FUNCTIONS

    //This function adds a rigidbody to the gameobject
    public static Rigidbody AddRigidbody(GameObject gameObject, float mass, float drag, float angularDrag, bool useInterpolation = false)
    {
        Rigidbody rigibody = gameObject.AddComponent<Rigidbody>();
        rigibody.mass = mass;
        rigibody.linearDamping = drag;
        rigibody.angularDamping = angularDrag;
        rigibody.useGravity = false;

        if (useInterpolation == true)
        {
            rigibody.interpolation = RigidbodyInterpolation.Interpolate;
        }
        else
        {
            rigibody.interpolation = RigidbodyInterpolation.None;
        }

        return rigibody;
    }

    //This functions adds a collider to all the mesh gameobjects
    public static void AddColliders(GameObject gameObject, bool convex)
    {
        MeshFilter[] meshFilters = gameObject.GetComponentsInChildren<MeshFilter>();

        foreach (MeshFilter meshFilter in meshFilters)
        {
            MeshCollider meshCollider = meshFilter.gameObject.AddComponent<MeshCollider>();
            meshCollider.convex = convex;
            meshCollider.sharedMesh = meshFilter.sharedMesh;
        }

        SkinnedMeshRenderer[] skinnedMeshRenderers = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
        {
            MeshCollider meshCollider = skinnedMeshRenderer.gameObject.AddComponent<MeshCollider>();
            meshCollider.convex = convex;
            meshCollider.sharedMesh = skinnedMeshRenderer.sharedMesh;
        }
    }

    //This ignores collisions between two sets of colliders
    public static void IgnoreCollision(GameObject gameObject1, GameObject gameObject2)
    {
        MeshCollider[] meshColliders1 = gameObject1.GetComponentsInChildren<MeshCollider>();
        MeshCollider[] meshColliders2 = gameObject2.GetComponentsInChildren<MeshCollider>();

        foreach (MeshCollider meshCollider1 in meshColliders1)
        {
            foreach (MeshCollider meshCollider2 in meshColliders2)
            {
                Physics.IgnoreCollision(meshCollider1, meshCollider2);
            }
        }
    }

    //This sets the particle to inactive
    public static IEnumerator DeactivateObjectAfterDelay(float time, GameObject gameObject)
    {
        yield return new WaitForSeconds(time);

        if (gameObject != null)
        {
            gameObject.transform.parent = null;
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            gameObject.SetActive(false);
        }
    }

    //This sets the layer choice for all the children of an object
    public static void SetLayerAllChildren(Transform root, int layer)
    {
        var children = root.GetComponentsInChildren<Transform>(includeInactive: true);

        foreach (var child in children)
        {
            child.gameObject.layer = layer;
        }
    }

    //Gets every child of a give transform
    public static Transform[] GetAllChildTransforms(Transform transform)
    {
        List<Transform> childTransforms = new List<Transform>();

        foreach (Transform tempTransform in transform)
        {
            childTransforms.Add(tempTransform);

            if (tempTransform.childCount > 0)
            {
                childTransforms.AddRange(GetAllChildTransforms(tempTransform));
            }              
        }

        return childTransforms.ToArray();
    }

    //Gets every child of a give transform
    public static Transform FindChildTransformCalled(Transform transform, string name)
    {
        Transform selectedTransform = null;

        foreach (Transform tempTransform in transform)
        {
            if (tempTransform.name == name)
            {
                selectedTransform = tempTransform;
                break;
            }
        }

        return selectedTransform;
    }

    //This searchs through all the child transforms of a particular transform looking for all transforms that containt the search string
    public static Transform[] FindAllChildTransformsContaining(Transform transform, string searchString, string exclusionString1 = "none", string exclusionString2 = "none", string exclusionString3 = "none")
    {
        Transform[] allTransforms = GetAllChildTransforms(transform);
        List<Transform> selectedTransforms = new List<Transform>();

        foreach (Transform child in allTransforms)
        {
           if (child.transform.name.Contains(searchString))
           {
                if (exclusionString1 == "none")
                {
                    selectedTransforms.Add(child);
                }
                else if (!child.name.Contains(exclusionString1) & exclusionString2 == "none" & exclusionString3 == "none")
                {
                    selectedTransforms.Add(child);
                }
                else if (!child.name.Contains(exclusionString1) & !child.name.Contains(exclusionString2) & exclusionString3 == "none")
                {
                    selectedTransforms.Add(child);
                }
                else if (!child.name.Contains(exclusionString1) & !child.name.Contains(exclusionString2) & !child.name.Contains(exclusionString3))
                {
                    selectedTransforms.Add(child);
                }
            }
        }

        return selectedTransforms.ToArray();
    }

    //This searchs through all the child transforms of a particular transform looking for all transforms that containt the search string
    public static Transform[] FindAllChildTransformsContainingBothStrings(Transform transform, string searchString1, string searchString2)
    {
        Transform[] allTransforms = GetAllChildTransforms(transform);
        List<Transform> selectedTransforms = new List<Transform>();

        foreach (Transform child in allTransforms)
        {
            if (child.transform.name.Contains(searchString1) & child.transform.name.Contains(searchString2))
            {
                selectedTransforms.Add(child);
            }
        }

        return selectedTransforms.ToArray();
    }

    //This searchs through all the child transforms of a particular transform looking for the first transform that contains the search phrase
    public static Transform FindChildTransformContaining(Transform transform, string searchString, string exclusionString1 = "none", string exclusionString2 = "none")
    {
        Transform[] allTransforms = GetAllChildTransforms(transform);
        Transform selectedTransform = null;

        foreach (Transform child in allTransforms)
        {
            if (child.transform.name.Contains(searchString))
            {
                if (exclusionString1 == "none")
                {
                    selectedTransform = child;
                    break;
                }
                else if (!child.name.Contains(exclusionString1) & exclusionString2 == "none")
                {
                    selectedTransform = child;
                    break;
                }
                else if (!child.name.Contains(exclusionString1) & !child.name.Contains(exclusionString2))
                {
                    selectedTransform = child;
                    break;
                }
            }
        }

        return selectedTransform;
    }

    //RIGIDBODY FUNCTIONS

    //This calculates the intercept point between the target and the ships lasers
    public static Vector3 CalculateInterceptPoint(Vector3 playerPosition, Vector3 targetPosition, Vector3 targetVelocity, float projectileSpeed)
    {
        Vector3 toTarget = targetPosition - playerPosition;
        float a = Vector3.Dot(targetVelocity, targetVelocity) - projectileSpeed * projectileSpeed;
        float b = 2 * Vector3.Dot(targetVelocity, toTarget);
        float c = Vector3.Dot(toTarget, toTarget);

        float discriminant = b * b - 4 * a * c;

        if (discriminant < 0)
        {
            // No real solution, return the target's current position
            return targetPosition;
        }

        float t1 = (-b - Mathf.Sqrt(discriminant)) / (2 * a);
        float t2 = (-b + Mathf.Sqrt(discriminant)) / (2 * a);

        float t = Mathf.Max(t1, t2);

        if (t < 0)
        {
            // No valid intercept point in the future, return the target's current position
            return targetPosition;
        }

        return targetPosition + t * targetVelocity;
    }

}
