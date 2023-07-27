using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectUtils
{
    //This function adds a rigidbody to the gameobject
    public static Rigidbody AddRigidbody(GameObject gameObject, float mass, float drag, float angularDrag)
    {
        Rigidbody rigibody = gameObject.AddComponent<Rigidbody>();
        rigibody.mass = mass;
        rigibody.drag = drag;
        rigibody.angularDrag = angularDrag;
        rigibody.useGravity = false;
        return rigibody;
    }

    //This function finds all the LODs on the ship
    public static GameObject[] GetLODs(GameObject gameObject)
    {

        List<GameObject> lods = new List<GameObject>();

        for (int i = 0; i < 100; i++)
        {

            Transform lod = gameObject.transform.Find("detail" + i.ToString());

            if (lod != null)
            {
                lods.Add(lod.gameObject);

                if (i == 0)
                {
                    lod.gameObject.SetActive(true);
                }
                else
                {
                    lod.gameObject.SetActive(false);
                }
            }

            if (lod == null)
            {
                break;
            }

        }

        return lods.ToArray();

    }

    //This functions adds a collider to all the mesh gameobjects
    public static void AddColliders(GameObject gameObject, bool convex)
    {
        MeshFilter[] meshFilters = gameObject.GetComponentsInChildren<MeshFilter>();

        foreach (MeshFilter meshFilter in meshFilters)
        {
            MeshCollider meshCollider = meshFilter.gameObject.AddComponent<MeshCollider>();
            meshCollider.convex = convex;
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

}
