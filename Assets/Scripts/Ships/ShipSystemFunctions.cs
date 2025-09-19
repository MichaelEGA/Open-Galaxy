using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public static class ShipSystemFunctions
{
    //This activates and deactivates a system transform
    public static void ActivateSystemTransforms(GameObject ship, bool activate)
    {
        if (ship != null)
        {
            //This activates the transforms
            Transform[] systemTransforms = GetSystemTransforms(ship);

            if (systemTransforms != null)
            {
                foreach (Transform systemTransform in systemTransforms)
                {
                    systemTransform.gameObject.SetActive(activate);
                    
                    if (systemTransform.GetComponent<ShipSystem>() == null)
                    {
                        systemTransform.AddComponent<ShipSystem>();
                    }
                }

                AddTransformsToPool(systemTransforms);
            }

            HudFunctions.AddToShipLog(ship.name.ToUpper() + ": critical systems marked for targeting");
        }
    }

    //This adds any new system transforms to the pool
    public static void AddTransformsToPool(Transform[] systemTransforms)
    {
        //This adds the activated transforms to the scene
        Scene scene = SceneFunctions.GetScene();

        if (scene != null)
        {
            if (scene.systemTransformsPool == null)
            {
                scene.systemTransformsPool = new List<Transform>();
            }
            
            if (scene.systemTransformsPool != null)
            {
                foreach (Transform systemTransform in systemTransforms)
                {
                    if (systemTransform != null)
                    {
                        bool exists = false;

                        foreach (Transform tempTransform in scene.systemTransformsPool)
                        {
                            if (tempTransform != null)
                            {
                                if (tempTransform == systemTransform)
                                {
                                    exists = true;
                                    break;
                                }
                            }
                        }

                        if (exists == false)
                        {
                            scene.systemTransformsPool.Add(systemTransform);
                        }
                    }
                }
            }
        }
    }

    //This gets all the system transforms on a given ship
    public static Transform[] GetSystemTransforms(GameObject ship)
    {
        Transform[] systemTranforms = GameObjectUtils.FindAllChildTransformsContainingIncludingInactive(ship.transform, "system", "systemtransforms");

        if (systemTranforms != null)
        {
            foreach (Transform systemTranform in systemTranforms)
            {
                systemTranform.gameObject.layer = LayerMask.NameToLayer("invisible");
            }
        }
       
        return (systemTranforms);
    }

    //This checks if a system is active or not
    public static bool SystemIsActive(GameObject ship, string system)
    {
        bool isActive = false;

        Transform[] systemTransforms = GetSystemTransforms(ship);

        if (systemTransforms != null)
        {
            foreach (Transform systemTransform in systemTransforms)
            {
                if (systemTransform.gameObject.activeSelf == true)
                {
                    if (system == "any")
                    {
                        isActive = true;
                        break;
                    }
                    else if (systemTransform.name.Contains(system))
                    {
                        isActive = true;
                        break;
                    }
                }
            }
        }

        return isActive;
    }

}
