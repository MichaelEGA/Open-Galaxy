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
            smallShip.dockingPoint = dockingPoint.gameObject.AddComponent<DockingPoint>();
        }
    }

    //This finds and adds the docking points to the large ship
    public static void AddDockingPointsLargeShip(LargeShip largeShip)
    {
        Transform[] dockingPointTransforms = GameObjectUtils.FindAllChildTransformsContaining(largeShip.transform, "dockingPoint");

        List<DockingPoint> dockingPointGameObjects = new List<DockingPoint>();

        foreach (Transform dockingPointTransform in dockingPointTransforms)
        {
            DockingPoint dockPoint = dockingPointTransform.gameObject.AddComponent<DockingPoint>();
            dockingPointGameObjects.Add(dockPoint);

            if (dockingPointTransform.name.Contains("down"));
            {
                dockPoint.releaseDown = true;
            }
            
        }

        largeShip.dockingPoints = dockingPointGameObjects.ToArray();
    }

    //This gets the docking point on the designated ship
    public static DockingPoint GetDockingPoint(Transform ship, Transform target = null, bool includeActive = false)
    {
        DockingPoint dockingPoint = null;

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
            if (smallShip.dockingPoint.isActive == false || smallShip.dockingPoint.isActive == true & includeActive == true)
            {
                dockingPoint = smallShip.dockingPoint;
            }
        }

        if (largeShip != null & target != null)
        {
            float distance = Mathf.Infinity;

            foreach (DockingPoint tempDockingPoint in largeShip.dockingPoints)
            {
                if (tempDockingPoint.isActive == false || tempDockingPoint.isActive == true & includeActive == true)
                {
                    float tempDistance = Vector3.Distance(target.transform.position, tempDockingPoint.transform.position);

                    if (tempDistance < distance)
                    {
                        dockingPoint = tempDockingPoint;
                    }
                }
            }
        }

        return dockingPoint;
    }
    
    //This gets the target docking point on the designated ship
    public static DockingPoint GetTargetDockingPoint(Transform ship, string targetShipName, bool includeActive = false)
    {
        DockingPoint dockingPoint = null;
        Scene scene = SceneFunctions.GetScene();
        bool dockingPointFound = false;

        LargeShip largeShip = ship.GetComponent<LargeShip>();

        //This searches for the docking point on a smallship
        if (largeShip == null)
        {
            foreach (SmallShip tempSmallShip in scene.smallShips)
            {
                if (tempSmallShip.name.Contains(targetShipName) & tempSmallShip.dockingPoint != null)
                {
                    if (tempSmallShip.dockingPoint.isActive == false || tempSmallShip.dockingPoint.isActive == true & includeActive == true)
                    {
                        dockingPoint = tempSmallShip.dockingPoint;
                        dockingPointFound = true;
                        break;
                    }
                }
            }
        }

        //This searches for a docking point on a large ship
        if (dockingPointFound == false)
        {
            Debug.Log("run a");

            foreach (LargeShip tempLargeShip in scene.largeShips)
            {
                Debug.Log("run b " + tempLargeShip.name);

                if (tempLargeShip.name.Contains(targetShipName))
                {

                    Debug.Log("run c " + tempLargeShip.name);

                    float distance = Mathf.Infinity;

                    foreach (DockingPoint tempDockingPoint in tempLargeShip.dockingPoints)
                    {
                        Debug.Log("run d " + tempDockingPoint.name);

                        if (tempDockingPoint.isActive == false || tempDockingPoint.isActive == true & includeActive == true)
                        {
                            Debug.Log("run e " + tempDockingPoint.name);

                            //This gets the closest docking point on the large ship
                            float tempDistance = Vector3.Distance(tempDockingPoint.transform.position, ship.position);

                            if (tempDistance < distance)
                            {
                                Debug.Log("run f");

                                if (largeShip == null & !tempDockingPoint.name.Contains("ls"))
                                {
                                    distance = tempDistance;
                                    dockingPoint = tempDockingPoint;
                                    Debug.Log("run 1z");
                                }
                                else if (largeShip != null & tempDockingPoint.name.Contains("ls"))
                                {
                                    distance = tempDistance;
                                    dockingPoint = tempDockingPoint;
                                    Debug.Log("run 2z");
                                }
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
    public static IEnumerator StartDocking(Transform ship, DockingPoint shipDockingPoint, DockingPoint targetDockingPoint, Quaternion flip, float rotationSpeed, float movementSpeed)
    {
        shipDockingPoint.isActive = true;
        targetDockingPoint.isActive = true;
        
        SmallShip smallShip = ship.GetComponent<SmallShip>();
        LargeShip largeShip = ship.GetComponent<LargeShip>();

        SmallShip targetSmallShip = targetDockingPoint.GetComponentInParent<SmallShip>();
        LargeShip targetLargeShip = targetDockingPoint.GetComponentInParent<LargeShip>();

        HudFunctions.AddToShipLog(ship.name.ToUpper() + " commencing docking sequence with " + targetDockingPoint.transform.parent.name.ToUpper());

        if (smallShip != null)
        {
            smallShip.docking = true;
            smallShip.thrustSpeed = 0;
            SmallShipFunctions.CloseWings(smallShip);
        }

        if (largeShip != null)
        {
            largeShip.docking = true;
            largeShip.thrustSpeed = 0;
        }

        if (targetSmallShip != null)
        {
            targetSmallShip.docking = true;
            targetSmallShip.thrustSpeed = 0;

            //This stops spinning on disabled ships so that the docking happens correctly
            if (targetSmallShip.isDisabled == true)
            {
                targetSmallShip.shipRigidbody.linearVelocity = new Vector3(0f, 0f, 0f);
                targetSmallShip.shipRigidbody.angularVelocity = new Vector3(0f, 0f, 0f);
                targetSmallShip.shipRigidbody.linearDamping = 9;
                targetSmallShip.shipRigidbody.angularDamping = 7.5f;
            }
        }

        if (targetLargeShip != null)
        {
            targetLargeShip.docking = true;
            targetLargeShip.thrustSpeed = 0;
        }

        if (ship != null & shipDockingPoint != null & shipDockingPoint.transform.IsChildOf(ship) & targetDockingPoint != null)
        {
            Scene scene = SceneFunctions.GetScene();

            Quaternion startRotation = ship.transform.localRotation;
            Quaternion endRotation = targetDockingPoint.transform.rotation * Quaternion.Inverse(Quaternion.Inverse(ship.rotation) * shipDockingPoint.transform.rotation) * flip;

            float timeElapsed = 0;
            float lerpDuration = rotationSpeed;

            while (timeElapsed < lerpDuration)
            {
                ship.transform.rotation = Quaternion.Lerp(startRotation, endRotation, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            ship.transform.rotation = endRotation;

            Vector3 startPosition = ship.localPosition;
            Vector3 tdockingPoint = scene.transform.InverseTransformPoint(targetDockingPoint.transform.position);
            Vector3 sDockingPoint = scene.transform.InverseTransformPoint(shipDockingPoint.transform.position);
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

            if (smallShip != null)
            {
                if (smallShip.isAI == false)
                {
                    AudioFunctions.PlayAudioClip(smallShip.audioManager, "clank01", "Cockpit", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);
                }
            }

            HudFunctions.AddToShipLog(ship.name.ToUpper() + " docked with " + targetDockingPoint.transform.parent.name.ToUpper());
        }
    }

    //This ends the docking sequence
    public static IEnumerator EndDocking(Transform ship, DockingPoint shipDockingPoint, DockingPoint targetDockingPoint, float speed)
    {
        SmallShip smallShip = ship.GetComponent<SmallShip>();
        LargeShip largeShip = ship.GetComponent<LargeShip>();

        SmallShip targetSmallShip = targetDockingPoint.GetComponentInParent<SmallShip>();
        LargeShip targetLargeShip = targetDockingPoint.GetComponentInParent<LargeShip>();

        HudFunctions.AddToShipLog(ship.name.ToUpper() + " commencing exit dock sequence with " + targetDockingPoint.transform.parent.name.ToUpper());

        if (smallShip != null)
        {
            if (smallShip.isAI == false)
            {
                AudioFunctions.PlayAudioClip(smallShip.audioManager, "clank01", "Cockpit", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);
            }
        }

        Scene scene = SceneFunctions.GetScene();
        ship.transform.SetParent(scene.transform);

        //This sets the default position to launch up
        Vector3 startPosition = ship.transform.localPosition;
        Vector3 endPosition = scene.transform.InverseTransformPoint(targetDockingPoint.transform.position) + (targetDockingPoint.transform.up * 20);

        //This modifies the positions to launch down
        if (targetDockingPoint.releaseDown == true)
        {
            startPosition = ship.transform.localPosition;
            endPosition = scene.transform.InverseTransformPoint(targetDockingPoint.transform.position) + (targetDockingPoint.transform.up * -20);
        }

        if (largeShip != null)
        {
            endPosition = scene.transform.InverseTransformPoint(targetDockingPoint.transform.position) + (targetDockingPoint.transform.right * 100);
        }

        float timeElapsed = 0;
        float lerpDuration = speed;

        while (timeElapsed < lerpDuration)
        {
            ship.localPosition = Vector3.Lerp(startPosition, endPosition, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        ship.localPosition = endPosition;

        shipDockingPoint.isActive = false;
        targetDockingPoint.isActive = false;

        HudFunctions.AddToShipLog(ship.name.ToUpper() + " released from dock ");

        if (smallShip != null)
        {
            smallShip.docking = false;
            smallShip.thrustSpeed = 0;
            SmallShipFunctions.OpenWings(smallShip);
        }

        if (largeShip != null)
        {
            largeShip.docking = false;
            largeShip.thrustSpeed = 0;
        }

        if (targetSmallShip != null)
        {
            targetSmallShip.docking = false;
            targetSmallShip.thrustSpeed = 0;
        }

        if (targetLargeShip != null)
        {
            targetLargeShip.docking = false;
            targetLargeShip.thrustSpeed = 0;
        }
    }
}
