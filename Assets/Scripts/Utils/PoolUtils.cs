using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Simple utitlies for handling game object pools
public static class PoolUtils
{
    //This finds a particular prefab object in a pool array
    public static Object FindPrefabObjectInPool(Object[] array, string name)
    {
        Object searchObject = null;

        foreach (Object tempObject in array)
        {
            if (tempObject.name.Contains(name))
            {
                searchObject = tempObject;
                break;
            }
        }

        return searchObject;
    }

    //This funds a particular gameobject in a pool list
    public static GameObject FindInactiveGameObjectInPool(List<GameObject> pool, string name)
    {
        GameObject gameObject = null;

        foreach (GameObject tempObject in pool)
        {
            if (tempObject != null)
            {
                if (tempObject.name.Contains(name) & tempObject.activeSelf == false)
                {
                    gameObject = tempObject;
                    break;
                }
            }
        }

        return gameObject;
    }

    //This adds an object to the pool list
    public static List<GameObject> AddToPool(List<GameObject> pool, GameObject itemToAdd)
    {
        if (pool == null)
        {
            pool = new List<GameObject>();
        }

        pool.Add(itemToAdd);

        return pool;
    }

    //This deactivate all objects in a pool list
    public static void DeactivatePool(List<GameObject> pool)
    {

        foreach (GameObject gameObject in pool)
        {
            gameObject.SetActive(false);
        }

    }

    //This destroys all the objects in a pool and clears the list
    public static List<GameObject> ClearPool(List<GameObject> pool)
    {

        foreach (GameObject gameObject in pool)
        {
            GameObject.Destroy(gameObject);
        }

        pool.Clear();

        return pool;

    }
}
