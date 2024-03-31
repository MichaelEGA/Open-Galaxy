using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DockingFunctions
{
    //This finds and adds the docking points to the smallship
    public static void AddDockingPointsSmallShip(SmallShip smallShip)
    {
        Transform dockingPoint = smallShip.gameObject.transform.Find("dockingPoint");

        if (dockingPoint != null)
        {
            smallShip.dockingPoint = dockingPoint.gameObject;
            smallShip.dockingPoint.AddComponent<DockingPoint>();
        }
    }

    //This finds and adds the docking points to the large ship
    public static void AddDockingPointsLargeShip(LargeShip largeShip)
    {
        Transform[] dockingPoints = GameObjectUtils.FindAllChildTransformsContaining(largeShip.transform, "dockingPoint");

        List<GameObject> dockingPointGameObjects = new List<GameObject>();

        foreach (Transform dockingPoint in dockingPoints)
        {
            dockingPointGameObjects.Add(dockingPoint.gameObject);
            dockingPoint.gameObject.AddComponent<DockingPoint>();
        }

        largeShip.dockingPoints = dockingPointGameObjects.ToArray();
    }

    //This gets the docking point on the designated ship
    public static GameObject GetDockingPoint(Transform ship, Transform target = null)
    {
        GameObject dockingPointGO = null;

        SmallShip smallShip = ship.GetComponent<SmallShip>();
        LargeShip largeShip = ship.GetComponent<LargeShip>();

        if (smallShip != null)
        {
            smallShip.docking = true;
        }

        if (largeShip != null)
        {
            largeShip.docking = true;
        }

        if (smallShip != null)
        {
            dockingPointGO = smallShip.dockingPoint;           
        }

        if (largeShip != null & target != null)
        {
            float distance = Mathf.Infinity;

            foreach (GameObject tempDockingPoint in largeShip.dockingPoints)
            {
                float tempDistance = Vector3.Distance(target.transform.position, tempDockingPoint.transform.position);

                if (tempDistance < distance)
                {
                    dockingPointGO = tempDockingPoint;
                }
            }
        }

        return dockingPointGO;
    }
    
    //This gets the target docking point on the designated ship
    public static GameObject GetTargetDockingPoint(Transform ship, string targetShipName)
    {
        GameObject dockingPoint = null;
        Scene scene = SceneFunctions.GetScene();
        bool dockingPointFound = false;

        LargeShip largeShip = ship.GetComponent<LargeShip>();

        //This searches for the docking point on a smallship
        if (largeShip == null)
        {
            foreach (SmallShip tempSmallShip in scene.smallShips)
            {
                if (tempSmallShip.name.Contains(targetShipName))
                {
                    dockingPoint = tempSmallShip.dockingPoint;
                    dockingPointFound = true;
                    break;
                }
            }
        }

        //This searches for a docking point on a large ship
        if (dockingPointFound == false)
        {
            foreach (LargeShip tempLargeShip in scene.largeShips)
            {
                if (tempLargeShip.name.Contains(targetShipName))
                {
                    float distance = Mathf.Infinity;

                    foreach (GameObject tempDockingPoint in tempLargeShip.dockingPoints)
                    {
                        //This gets the closest docking point on the large ship
                        Vector3 tempDockPointLocalPosition = scene.transform.InverseTransformPoint(tempDockingPoint.transform.position);

                        float tempDistance = Vector3.Distance(tempDockPointLocalPosition, ship.localPosition);

                        if (tempDistance < distance)
                        {
                            if (largeShip == null & !tempDockingPoint.name.Contains("ls"))
                            {
                                distance = tempDistance;
                                dockingPoint = tempDockingPoint;
                            }
                            else if (tempDockingPoint.name.Contains("ls"))
                            {
                                distance = tempDistance;
                                dockingPoint = tempDockingPoint;
                            }  
                        }
                    }

                    dockingPointFound = true;
                    break;
                }
            }
        }

        return (dockingPoint);
    }

    //This intiates the docking sequence
    public static IEnumerator StartDocking(Transform ship, Transform shipDockingPoint, Transform targetDockingPoint, Quaternion flip, float rotationSpeed, float movementSpeed)
    {
        SmallShip smallShip = ship.GetComponent<SmallShip>();
        LargeShip largeShip = ship.GetComponent<LargeShip>();

        if (smallShip != null)
        {
            smallShip.docking = true;
        }

        if (largeShip != null)
        {
            largeShip.docking = true;
        }

        if (ship != null & shipDockingPoint != null & shipDockingPoint.IsChildOf(ship) & targetDockingPoint != null)
        {
            Scene scene = SceneFunctions.GetScene();

            Quaternion startRotation = ship.transform.localRotation;
            Quaternion endRotation = targetDockingPoint.localRotation * Quaternion.Inverse(Quaternion.Inverse(ship.localRotation) * shipDockingPoint.localRotation) * flip;

            float timeElapsed = 0;
            float lerpDuration = rotationSpeed;

            while (timeElapsed < lerpDuration)
            {
                ship.transform.localRotation = Quaternion.Lerp(startRotation, endRotation, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            ship.transform.localRotation = endRotation;

            Vector3 startPosition = ship.localPosition;
            Vector3 tdockingPoint = scene.transform.InverseTransformPoint(targetDockingPoint.position);
            Vector3 sDockingPoint = scene.transform.InverseTransformPoint(shipDockingPoint.position);
            Vector3 endPosition = tdockingPoint + (ship.localPosition - sDockingPoint);

            timeElapsed = 0;
            lerpDuration = movementSpeed;

            while (timeElapsed < lerpDuration)
            {
                ship.transform.localPosition = Vector3.Lerp(startPosition, endPosition, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            ship.transform.localPosition = endPosition;

            HudFunctions.AddToShipLog(ship.name.ToUpper() + " docked with " + targetDockingPoint.transform.parent.name);
        }
    }

    //This ends the docking sequence
    public static IEnumerator EndDocking(Transform ship, Transform targetDockingPoint, float speed)
    {
        SmallShip smallShip = ship.GetComponent<SmallShip>();
        LargeShip largeShip = ship.GetComponent<LargeShip>();

        if (smallShip != null)
        {
            smallShip.docking = true;
        }

        if (largeShip != null)
        {
            largeShip.docking = true;
        }

        HudFunctions.AddToShipLog(ship.name.ToUpper() + " commencing exit dock sequence " + targetDockingPoint.transform.parent.name);

        Scene scene = SceneFunctions.GetScene();
        ship.transform.SetParent(scene.transform);

        //Move out
        Vector3 startPosition = ship.transform.localPosition;
        Vector3 endPosition = scene.transform.InverseTransformPoint(targetDockingPoint.position) + (targetDockingPoint.up * 100);

        float timeElapsed = 0;
        float lerpDuration = speed;

        while (timeElapsed < lerpDuration)
        {
            ship.localPosition = Vector3.Lerp(startPosition, endPosition, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        ship.localPosition = endPosition;

        HudFunctions.AddToShipLog(ship.name.ToUpper() + " released from dock ");

        smallShip.docking = false;
    }
}
