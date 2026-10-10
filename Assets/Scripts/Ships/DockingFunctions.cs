using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class DockingFunctions
{
    //This finds and adds the docking points to the smallship
    public static void AddDockingPointsSmallShip(Ship smallShip)
    {
        Transform dockingPoint = GameObjectUtils.FindFirstChildTransformContaining(smallShip.transform, "docking");
            
        if (dockingPoint != null)
        {
            smallShip.flightControlSystem_Small.dockingPoint = dockingPoint.gameObject.AddComponent<DockingPoint>();
        }
        else
        {
            GameObject dockingPointGO = new GameObject();
            dockingPointGO.name = "dockingpoint";

            dockingPoint = dockingPointGO.transform;
            dockingPoint.SetParent(smallShip.transform);

            dockingPoint.localPosition = new Vector3(0, -(smallShip.shipLength/2), 0);

            smallShip.flightControlSystem_Small.dockingPoint = dockingPointGO.AddComponent<DockingPoint>();
        }

    }

    //This finds and adds the docking points to the large ship
    public static void AddDockingPointsLargeShip(LargeShip largeShip)
    {
        Transform[] dockingPointTransforms = GameObjectUtils.FindAllChildTransformsContaining(largeShip.transform, "docking");

        List<DockingPoint> dockingPointGameObjects = new List<DockingPoint>();

        foreach (Transform dockingPointTransform in dockingPointTransforms)
        {
            DockingPoint dockPoint = dockingPointTransform.gameObject.AddComponent<DockingPoint>();
            dockingPointGameObjects.Add(dockPoint);

            if (dockingPointTransform.name.Contains("down"))
            {
                dockPoint.releaseDown = true;
            }
            
        }

        if (dockingPointGameObjects.Count < 1)
        {
            //This adds a small ship docking point
            GameObject dockingPointGO = new GameObject();
            dockingPointGO.name = "dockingpoint";

            Transform dockingPoint = dockingPointGO.transform;
            dockingPoint.SetParent(largeShip.transform);

            dockingPoint.localPosition = new Vector3(0, -(largeShip.shipLength/2), 0);

            DockingPoint dockPoint = dockingPointGO.AddComponent<DockingPoint>();
            dockingPointGameObjects.Add(dockPoint);

            //This adds a largeship docking point
            GameObject dockingPointlsGO = new GameObject();
            dockingPointlsGO.name = "dockingpoint_ls";

            Transform dockingPointls = dockingPointlsGO.transform;
            dockingPointls.SetParent(largeShip.transform);

            dockingPointls.localPosition = new Vector3(0, -(largeShip.shipLength / 2), 0);

            DockingPoint dockPointls = dockingPointlsGO.AddComponent<DockingPoint>();
            dockingPointGameObjects.Add(dockPointls);
        }

        largeShip.dockingPoints = dockingPointGameObjects.ToArray();
    }

    //This gets the docking point on the designated ship
    public static DockingPoint GetDockingPoint(Transform ship, Transform target = null, bool includeActive = false)
    {
        DockingPoint dockingPoint = null;

        FlightControlSystem_Small smallShip = ship.GetComponent<FlightControlSystem_Small>();
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
                        distance = tempDistance;
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
            foreach (Ship tempSmallShip in scene.smallShips)
            {
                if (tempSmallShip != null)
                {
                    if (tempSmallShip.name.Contains(targetShipName) & tempSmallShip.flightControlSystem_Small.dockingPoint != null)
                    {
                        if (tempSmallShip.flightControlSystem_Small.dockingPoint.isActive == false || tempSmallShip.flightControlSystem_Small.dockingPoint.isActive == true & includeActive == true)
                        {
                            dockingPoint = tempSmallShip.flightControlSystem_Small.dockingPoint;
                            dockingPointFound = true;
                            break;
                        }
                    }
                }
            }
        }

        //This searches for a docking point on a large ship
        if (dockingPointFound == false)
        {
            foreach (LargeShip tempLargeShip in scene.largeShips)
            {
                if (tempLargeShip != null)
                {
                    if (tempLargeShip.name.Contains(targetShipName))
                    {
                        float distance = Mathf.Infinity;

                        foreach (DockingPoint tempDockingPoint in tempLargeShip.dockingPoints)
                        {
         
                            if (tempDockingPoint != null)
                            {

                                if (tempDockingPoint.isActive == false || tempDockingPoint.isActive == true & includeActive == true)
                                {
                                    //This gets the closest docking point on the large ship
                                    float tempDistance = Vector3.Distance(tempDockingPoint.transform.position, ship.position);

                                    if (tempDistance < distance)
                                    {

                                        if (largeShip == null & !tempDockingPoint.name.Contains("ls")) //if the ship is smallship and the docking point is not a largeship docking point
                                        {
                                            distance = tempDistance;
                                            dockingPoint = tempDockingPoint;
                                        }
                                        else if (largeShip != null & tempDockingPoint.name.Contains("ls"))
                                        {
                                            distance = tempDistance;
                                            dockingPoint = tempDockingPoint;
                                        }
                                    }
                                }
                            }
                        }

                        dockingPointFound = true;
                        break;
                    }
                }
            }
        }

        return (dockingPoint);
    }

    //This intiates the docking sequence
    public static IEnumerator StartDocking(Transform shipTransform, DockingPoint shipDockingPoint, DockingPoint targetDockingPoint, Quaternion flip, float rotationSpeed, float movementSpeed)
    {
        shipDockingPoint.isActive = true;
        targetDockingPoint.isActive = true;
        bool largeshipDockingOnly = false;
        
        Ship ship = shipTransform.GetComponent<Ship>();
        LargeShip largeShip = shipTransform.GetComponent<LargeShip>();

        Ship targetShip = targetDockingPoint.GetComponentInParent<Ship>();
        LargeShip targetLargeShip = targetDockingPoint.GetComponentInParent<LargeShip>();

        if (largeShip != null & targetLargeShip != null)
        {
            largeshipDockingOnly = true;
        }

        HudFunctions.AddToShipLog(shipTransform.name.ToUpper() + " commencing docking sequence with " + targetDockingPoint.transform.parent.name.ToUpper());

        if (ship != null)
        {
            ship.flightControlSystem_Small.docking = true;
            ship.flightControlSystem_Small.thrustSpeed = 0;
            WingSystemFunctions.CloseWings(ship.wingSystems);
            ship.flightControlSystem_Small.targetDockingPoint = targetDockingPoint.gameObject;
        }

        if (largeShip != null)
        {
            largeShip.docking = true;
            largeShip.thrustSpeed = 0;
            largeShip.targetDockingPoint = targetDockingPoint.gameObject;
        }

        if (targetShip != null)
        {
            targetShip.flightControlSystem_Small.docking = true;
            targetShip.flightControlSystem_Small.thrustSpeed = 0;

            //This stops spinning on disabled ships so that the docking happens correctly
            if (targetShip.damageSystem.isDisabled == true)
            {
                targetShip.flightControlSystem_Small.shipRigidbody.linearVelocity = new Vector3(0f, 0f, 0f);
                targetShip.flightControlSystem_Small.shipRigidbody.angularVelocity = new Vector3(0f, 0f, 0f);
                targetShip.flightControlSystem_Small.shipRigidbody.linearDamping = 9;
                targetShip.flightControlSystem_Small.shipRigidbody.angularDamping = 7.5f;
            }
        }

        if (targetLargeShip != null)
        {
            targetLargeShip.docking = true;
            targetLargeShip.thrustSpeed = 0;
        }

        if (shipTransform != null & shipDockingPoint != null & shipDockingPoint.transform.IsChildOf(shipTransform) & targetDockingPoint != null)
        {
            Scene scene = SceneFunctions.GetScene();

            Quaternion startRotation = shipTransform.transform.localRotation;
            Quaternion endRotation = targetDockingPoint.transform.rotation * Quaternion.Inverse(Quaternion.Inverse(shipTransform.rotation) * shipDockingPoint.transform.rotation) * flip;
            
            if (largeshipDockingOnly == true)
            {
                endRotation = targetLargeShip.transform.rotation; //This makes sure largeships are always rotated the same way as ship they are docking with
            }
            
            float timeElapsed = 0;
            float lerpDuration = rotationSpeed;

            while (timeElapsed < lerpDuration)
            {
                if (shipTransform != null)
                {
                    shipTransform.transform.rotation = Quaternion.Lerp(startRotation, endRotation, timeElapsed / lerpDuration);
                    timeElapsed += Time.deltaTime;
                    yield return null;
                }
                else
                {
                    break;
                }
            }

            if (shipTransform != null)
            {
                shipTransform.transform.rotation = endRotation;

                Vector3 startPosition = shipTransform.localPosition;
                Vector3 tdockingPoint = scene.transform.InverseTransformPoint(targetDockingPoint.transform.position);
                Vector3 sDockingPoint = scene.transform.InverseTransformPoint(shipDockingPoint.transform.position);
                Vector3 endPosition = tdockingPoint + (shipTransform.localPosition - sDockingPoint);

                timeElapsed = 0;
                lerpDuration = movementSpeed;

                while (timeElapsed < lerpDuration)
                {
                    if (shipTransform != null)
                    {
                        shipTransform.transform.localPosition = Vector3.Lerp(startPosition, endPosition, timeElapsed / lerpDuration);
                        timeElapsed += Time.deltaTime;
                        yield return new WaitForFixedUpdate();
                    }
                    else
                    {
                        break;
                    }
                }

                if (shipTransform != null)
                {
                    shipTransform.transform.localPosition = endPosition;

                    if (ship != null)
                    {
                        if (ship.isAI == false & ship.ogInput.keyboardAndMouse == false)
                        {
                            AudioFunctions.PlayAudioClip(ship.audioManager, "clank01", "Cockpit", ship.gameObject.transform.position, 0, 1, 500, 1, 100);
                            Task a = new Task(OGInputFunctions.ShakeControllerForSetTime(0.25f, 0.6f, 0.6f));
                        }
                    }

                    HudFunctions.AddToShipLog(shipTransform.name.ToUpper() + " docked with " + targetDockingPoint.transform.parent.name.ToUpper());
                }
            }
        }
    }

    //This ends the docking sequence
    public static IEnumerator EndDocking(Transform shipTransform, DockingPoint shipDockingPoint, DockingPoint targetDockingPoint, float speed)
    {
        Ship ship = shipTransform.GetComponent<Ship>();
        LargeShip largeShip = shipTransform.GetComponent<LargeShip>();

        Ship targetShip = targetDockingPoint.GetComponentInParent<Ship>();
        LargeShip targetLargeShip = targetDockingPoint.GetComponentInParent<LargeShip>();

        HudFunctions.AddToShipLog(shipTransform.name.ToUpper() + " commencing exit dock sequence with " + targetDockingPoint.transform.parent.name.ToUpper());

        if (ship != null)
        {
            if (ship.isAI == false & ship.ogInput.keyboardAndMouse == false)
            {
                AudioFunctions.PlayAudioClip(ship.audioManager, "clank01", "Cockpit", ship.gameObject.transform.position, 0, 1, 500, 1, 100);
                Task a = new Task(OGInputFunctions.ShakeControllerForSetTime(0.25f, 0.6f, 0.6f));
            }
        }

        Scene scene = SceneFunctions.GetScene();
        shipTransform.transform.SetParent(scene.transform);

        //This sets the default position to launch up
        Vector3 startPosition = shipTransform.transform.localPosition;
        Vector3 endPosition = scene.transform.InverseTransformPoint(targetDockingPoint.transform.position) + (targetDockingPoint.transform.up * 20);

        //This modifies the positions to launch down
        if (targetDockingPoint.releaseDown == true)
        {
            startPosition = shipTransform.transform.localPosition;
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
            if (shipTransform != null)
            {
                shipTransform.localPosition = Vector3.Lerp(startPosition, endPosition, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
            else
            {
                break;
            }
        }

        if (shipTransform != null)
        {
            shipTransform.localPosition = endPosition;

            shipDockingPoint.isActive = false;
            targetDockingPoint.isActive = false;

            HudFunctions.AddToShipLog(shipTransform.name.ToUpper() + " released from dock ");

            if (ship != null)
            {
                ship.flightControlSystem_Small.docking = false;
                ship.flightControlSystem_Small.thrustSpeed = 0;
                WingSystemFunctions.OpenWings(ship.wingSystems);
            }

            if (largeShip != null)
            {
                largeShip.docking = false;
                largeShip.thrustSpeed = 0;
            }

            if (targetShip != null)
            {
                targetShip.flightControlSystem_Small.docking = false;
                targetShip.flightControlSystem_Small.thrustSpeed = 0;
            }

            if (targetLargeShip != null)
            {
                targetLargeShip.docking = false;
                targetLargeShip.thrustSpeed = 0;
            }
        }
    }

    //This cancels the docking if the ship has been deactivated or destroyed
    public static void CancelDocking (Ship ship = null, LargeShip largeShip = null)
    {
        if (ship != null)
        {
            if (ship.flightControlSystem_Small.targetDockingPoint != null)
            {
                LargeShip otherLargeShip = ship.flightControlSystem_Small.targetDockingPoint.GetComponentInParent<LargeShip>();

                if (otherLargeShip != null)
                {
                    otherLargeShip.docking = false;
                }

                FlightControlSystem_Small otherSmallShip = ship.flightControlSystem_Small.targetDockingPoint.GetComponentInParent<FlightControlSystem_Small>();

                if (otherSmallShip  != null)
                {
                    otherSmallShip.docking = false;
                }

                DockingPoint targetDockingPoint = ship.flightControlSystem_Small.targetDockingPoint.GetComponent<DockingPoint>();

                if (targetDockingPoint != null)
                {
                    targetDockingPoint.isActive = false;
                }
            }
        }
        else if (largeShip != null)
        {
            if (largeShip.targetDockingPoint != null)
            {
                LargeShip otherLargeShip = largeShip.targetDockingPoint.GetComponentInParent<LargeShip>();

                if (otherLargeShip != null)
                {
                    otherLargeShip.docking = false;
                }

                FlightControlSystem_Small otherSmallShip = largeShip.targetDockingPoint.GetComponentInParent<FlightControlSystem_Small>();

                if (otherSmallShip != null)
                {
                    otherSmallShip.docking = false;
                }

                DockingPoint targetDockingPoint = largeShip.targetDockingPoint.GetComponent<DockingPoint>();

                if (targetDockingPoint != null)
                {
                    targetDockingPoint.isActive = false;
                }
            }
        }
    }
}
