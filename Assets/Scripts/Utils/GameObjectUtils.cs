using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

public static class GameObjectUtils
{

    //This finds the biggest mesh on the give gameobject
    public static Object FindLargestMeshByLength(GameObject root)
    {
        MeshFilter[] meshFilters = root.GetComponentsInChildren<MeshFilter>();
        SkinnedMeshRenderer[] skinnedMeshRenderers = root.GetComponentsInChildren<SkinnedMeshRenderer>();

        Object largest = null;
        float maxLength = 0f;

        foreach (var mf in meshFilters)
        {
            if (mf.sharedMesh == null) continue;
            Bounds bounds = mf.sharedMesh.bounds;
            float length = bounds.size.magnitude;
            if (length > maxLength)
            {
                maxLength = length;
                largest = mf;
            }
        }

        foreach (var smr in skinnedMeshRenderers)
        {
            if (smr.sharedMesh == null) continue;
            Bounds bounds = smr.sharedMesh.bounds;
            float length = bounds.size.magnitude;
            if (length > maxLength)
            {
                maxLength = length;
                largest = smr;
            }
        }

        return largest;
    }

    //This gets a group of random points on the mesh of the object
    public static List<Vector3> GetRandomPointsOnMesh(Mesh mesh, Transform meshTransform, int count)
    {
        List<Vector3> points = new List<Vector3>(count);
        if (mesh == null || mesh.triangles.Length < 3 || count <= 0)
            return points;

        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        // Step 1: Compute areas of all triangles
        float[] triangleAreas = new float[triangles.Length / 3];
        float totalArea = 0f;

        for (int i = 0; i < triangleAreas.Length; i++)
        {
            Vector3 a = vertices[triangles[i * 3 + 0]];
            Vector3 b = vertices[triangles[i * 3 + 1]];
            Vector3 c = vertices[triangles[i * 3 + 2]];
            float area = Vector3.Cross(b - a, c - a).magnitude * 0.5f;
            triangleAreas[i] = area;
            totalArea += area;
        }

        // Step 2: Build cumulative area array for sampling
        float[] cumulativeAreas = new float[triangleAreas.Length];
        float cumArea = 0;
        for (int i = 0; i < triangleAreas.Length; i++)
        {
            cumArea += triangleAreas[i];
            cumulativeAreas[i] = cumArea;
        }

        // Step 3: Sample random points
        for (int i = 0; i < count; i++)
        {
            // Pick a triangle with weighted probability
            float r = Random.value * totalArea;

            int triIndex = System.Array.BinarySearch(cumulativeAreas, r);
            if (triIndex < 0)
            {
                triIndex = ~triIndex;
            }

            // Get triangle vertices
            Vector3 a = vertices[triangles[triIndex * 3 + 0]];
            Vector3 b = vertices[triangles[triIndex * 3 + 1]];
            Vector3 c = vertices[triangles[triIndex * 3 + 2]];

            // Barycentric coordinates for random point on triangle
            float u = Random.value;
            float v = Random.value;
            if (u + v > 1f)
            {
                u = 1f - u;
                v = 1f - v;
            }
            float w = 1f - u - v;

            Vector3 localPoint = a * u + b * v + c * w;

            points.Add(localPoint);
        }

        return points;
    }

    //This applys a certain material to all meshes under a given gameobject
    public static void ApplyMaterialToAllMeshes(GameObject gameObject, Material material)
    {
        if (gameObject == null || material == null)
        {
            Debug.LogWarning("Root GameObject or Material is null. Aborting material application.");
            return;
        }

        // Get all MeshRenderer components on the object itself
        MeshRenderer[] renderers = gameObject.GetComponents<MeshRenderer>();

        foreach (MeshRenderer renderer in renderers)
        {
            var mats = renderer.sharedMaterials;
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = material;
            }
            renderer.sharedMaterials = mats;
        }

        // Get all MeshRenderer components in the hierarchy (including inactive ones)
        renderers = gameObject.GetComponentsInChildren<MeshRenderer>(true);

        foreach (MeshRenderer renderer in renderers)
        {
            var mats = renderer.sharedMaterials;
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = material;
            }
            renderer.sharedMaterials = mats;
        }

        // Get all SkinnedMeshRenderer components on the object itself
        SkinnedMeshRenderer[] skinnedRenderers = gameObject.GetComponents<SkinnedMeshRenderer>();

        foreach (SkinnedMeshRenderer renderer in skinnedRenderers)
        {
            var mats = renderer.sharedMaterials;
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = material;
            }
            renderer.sharedMaterials = mats;
        }

        // Get all SkinnedMeshRenderer components in the hierarchy (including inactive ones)
        skinnedRenderers = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true);

        foreach (SkinnedMeshRenderer renderer in skinnedRenderers)
        {
            var mats = renderer.sharedMaterials;
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = material;
            }
            renderer.sharedMaterials = mats;
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

    //This functions adds a mesh collider to all the mesh gameobjects
    public static void AddMeshColliders(GameObject gameObject, bool convex)
    {
        MeshFilter[] meshFilters = gameObject.GetComponentsInChildren<MeshFilter>();

        foreach (MeshFilter meshFilter in meshFilters)
        {
            if (!meshFilter.gameObject.name.Contains("nocollider"))
            {
                MeshCollider meshCollider = meshFilter.gameObject.AddComponent<MeshCollider>();
                meshCollider.convex = convex;
                meshCollider.sharedMesh = meshFilter.sharedMesh;
            }
        }

        SkinnedMeshRenderer[] skinnedMeshRenderers = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
        {
            if (!skinnedMeshRenderer.gameObject.name.Contains("nocollider"))
            {
                MeshCollider meshCollider = skinnedMeshRenderer.gameObject.AddComponent<MeshCollider>();
                meshCollider.convex = convex;
                meshCollider.sharedMesh = skinnedMeshRenderer.sharedMesh;
            }
        }
    }

    //This functions adds a sphere collider to all the mesh gameobjects
    public static void AddSphereColliders(GameObject gameObject)
    {
        MeshFilter[] meshFilters = gameObject.GetComponentsInChildren<MeshFilter>();

        foreach (MeshFilter meshFilter in meshFilters)
        {
            if (!meshFilter.gameObject.name.Contains("nocollider"))
            {
                SphereCollider sphereCollider = meshFilter.gameObject.AddComponent<SphereCollider>();
            }
        }

        SkinnedMeshRenderer[] skinnedMeshRenderers = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
        {
            if (!skinnedMeshRenderer.gameObject.name.Contains("nocollider"))
            {
                SphereCollider sphereCollider = skinnedMeshRenderer.gameObject.AddComponent<SphereCollider>();
            }   
        }
    }

    //This deletes colliders from a game object
    public static void RemoveColliders(GameObject gameObject)
    {
        //delete all colliders on the root gameobject
        Collider[] colliders = gameObject.GetComponents<Collider>();

        foreach (var col in colliders)
        {
            GameObject.DestroyImmediate(col);
        }

        //delete all colliders under the root gameobject
        colliders = gameObject.GetComponentsInChildren<Collider>(includeInactive: true);
        
        foreach (var col in colliders)
        {
            GameObject.DestroyImmediate(col);
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

    //Gets every child of a give transform
    public static Transform FindFirstChildTransformContaining(Transform transform, string searchString)
    {
        Transform selectedTransform = null;

        foreach (Transform tempTransform in transform)
        {
            if (tempTransform.name.Contains(searchString))
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

    //Gets every child of a give transform
    public static Transform[] GetAllChildTransformsIncludingInactive(Transform transform)
    {
        List<Transform> childTransforms = new List<Transform>();
        Transform[] allTransforms = transform.GetComponentsInChildren<Transform>(true);

        foreach (Transform t in allTransforms)
        {
            if (t != transform)
            {
                childTransforms.Add(t);
            }
        }
        return childTransforms.ToArray();
    }

    public static Transform[] FindAllChildTransformsContainingIncludingInactive(Transform transform, string searchString, string exclusionString1 = "none", string exclusionString2 = "none", string exclusionString3 = "none")
    {
        Transform[] allTransforms = GetAllChildTransformsIncludingInactive(transform);
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

    //This lerps a gameobject between two points
    public static IEnumerator LerpBetweenTwoPoints(GameObject gameObject, Vector3 endPosition, Vector3 startPosition, float lerpDuration)
    {
        float timeElapsed = 0;

        while (timeElapsed < lerpDuration)
        {
            gameObject.transform.localPosition = Vector3.Lerp(startPosition, endPosition, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        gameObject.transform.localPosition = endPosition;
    }
}
