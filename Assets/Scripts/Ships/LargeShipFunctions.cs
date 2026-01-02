using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public static class LargeShipFunctions
{

    #region start functions

    //This prepares the ship by loading lod and colliders (if not already loaded)
    public static void PrepareShip(LargeShip largeShip)
    {
        if (largeShip.loaded == false)
        {
            largeShip.shipRigidbody = GameObjectUtils.AddRigidbody(largeShip.gameObject, 100f, 9f, 7.5f);
            largeShip.shipRigidbody.isKinematic = true;
            GameObjectUtils.AddMeshColliders(largeShip.gameObject, false);
            largeShip.colliders = largeShip.GetComponentsInChildren<MeshCollider>();
            largeShip.castPoint = largeShip.gameObject.transform.Find("castPoint");

            //This generates a cast point if none exists on the ship prefab
            if (largeShip.castPoint == null)
            {
                GameObject castPoint = new GameObject();
                castPoint.transform.SetParent(largeShip.transform);
                castPoint.name = "castPoint";
                castPoint.transform.localPosition = new Vector3(0, 0, largeShip.shipLength);
            }

            largeShip.explosionPoints = GameObjectUtils.FindAllChildTransformsContaining(largeShip.gameObject.transform, "explosionPoint");
            CreateWaypoint(largeShip);
            DockingFunctions.AddDockingPointsLargeShip(largeShip);

            largeShip.loaded = true;
        }
    }

    //This creates the ship waypoint
    public static void CreateWaypoint(LargeShip largeShip)
    {
        if (largeShip.waypoint == null)
        {
            Scene scene = SceneFunctions.GetScene();
            largeShip.waypoint = new GameObject();
            largeShip.waypoint.name = "waypoint_" + largeShip.name;
            largeShip.waypoint.transform.SetParent(scene.transform);
        }
    }

    #endregion

    #region ship input

    //This gets the AI input
    public static void GetAIInput(LargeShip largeShip)
    {
        if (largeShip.spinShip == false & largeShip.controlLock == false)
        {
            LargeShipAIFunctions.GetAIInput(largeShip);
        }
        else if (largeShip.spinShip == false & largeShip.controlLock == true)
        {
            LargeShipAIFunctions.SmoothPitchInput(largeShip, 0);
            LargeShipAIFunctions.SmoothTurnInput(largeShip, 0);
            LargeShipAIFunctions.SmoothRollInput(largeShip, 0);
        }
        else
        {
            LargeShipAIFunctions.SmoothPitchInput(largeShip, 0);
            LargeShipAIFunctions.SmoothTurnInput(largeShip, 0);
            LargeShipAIFunctions.SmoothRollInput(largeShip, 1);
        }
    }

    public static void NoInput(LargeShip largeShip)
    {
        largeShip.pitchInput = 0;
        largeShip.rollInput = 0;
        largeShip.turnInput = 0;
    }

    #endregion

    #region ship movement

    //This calculates the thrust speed of the ship
    public static void CalculateThrustSpeed(LargeShip largeShip)
    {
        //This calculates the normal accleration and speed rating
        float acclerationAmount = (0.5f / 100f) * largeShip.accelerationRating;

        //This controls the throttle of the ship, and prevents it going above the speed rating or below zero
        if (largeShip.speedRating != 0)
        {
            if (largeShip.thrustInput > 0 & largeShip.thrustSpeed < largeShip.speedRating)
            {
                largeShip.thrustSpeed = largeShip.thrustSpeed + acclerationAmount;
            }
            if (largeShip.thrustInput < 0 & largeShip.thrustSpeed > 0)
            {
                largeShip.thrustSpeed = largeShip.thrustSpeed - acclerationAmount;
            }

            if (largeShip.thrustSpeed < 0)
            {
                largeShip.thrustSpeed = 0;
            }
        }
        else
        {
            largeShip.thrustSpeed = 0;
        }
    }

    //This calculates pitch, turn, and roll according to the speed of the vehicle
    public static void CalculatePitchTurnRollSpeeds(LargeShip largeShip)
    {
        if (largeShip.reducemaneuvarability == true)
        {
            largeShip.maneuvarabilityActual = 1;
        }
        else
        {
            largeShip.maneuvarabilityActual = largeShip.maneuverabilityRating;
        }

        if (largeShip.spinShip == true)
        {
            largeShip.maneuvarabilityActual = 2.5f;
        }
    }

    //This makes the ship move
    public static void MoveShip(LargeShip largeShip)
    {
        if (largeShip.shipRigidbody == null)
        {
            largeShip.shipRigidbody = largeShip.gameObject.GetComponent<Rigidbody>();
        }

        if (largeShip.jumpingToHyperspace == false & largeShip.exitingHyperspace == false & largeShip.docking == false & largeShip.systemsLevel > 0) 
        {
            //This smoothly increases and decreases pitch, turn, and roll to provide smooth movement;
            float step = +Time.deltaTime / 0.1f;
            largeShip.pitchInputActual = Mathf.Lerp(largeShip.pitchInputActual, largeShip.pitchInput, step);
            largeShip.turnInputActual = Mathf.Lerp(largeShip.turnInputActual, largeShip.turnInput, step);
            largeShip.rollInputActual = Mathf.Lerp(largeShip.rollInputActual, largeShip.rollInput, step);

            //This makes the vehicle fly
            if (largeShip.thrustSpeed > 0)
            {
                largeShip.gameObject.transform.Translate(Vector3.forward * Time.deltaTime * largeShip.thrustSpeed);
            }

            Vector3 x = Vector3.right * largeShip.pitchInputActual;
            Vector3 y = Vector3.up * largeShip.turnInputActual;
            Vector3 z = Vector3.forward * largeShip.rollInputActual;

            Vector3 rotationVector = new Vector3(x.x, y.y, z.z);

            largeShip.transform.Rotate(rotationVector, Time.fixedDeltaTime * largeShip.maneuvarabilityActual, Space.World);
        }
    }

    //Jump to Hyperspace
    public static IEnumerator JumpToHyperspace(LargeShip largeShip)
    {
        largeShip.jumpingToHyperspace = true;

        Vector3 startPosition = largeShip.gameObject.transform.localPosition;
        Vector3 endPosition = largeShip.transform.localPosition + largeShip.gameObject.transform.forward * 30000;

        AudioFunctions.PlayAudioClip(largeShip.audioManager, "hyperspace03_exit", "Explosions", largeShip.transform.position, 1, 1, 10000, 1f);

        float timeElapsed = 0;
        float lerpDuration = 1;

        while (timeElapsed < lerpDuration)
        {
            largeShip.gameObject.transform.localPosition = Vector3.Lerp(startPosition, endPosition, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        HudFunctions.AddToShipLog(largeShip.name.ToUpper() + " jumped to hyperspace");

        largeShip.jumpingToHyperspace = false;

        DamageFunctions.DeactivateShip_LargeShip(largeShip);
    }

    //Exit Hyperspace
    public static IEnumerator ExitHyperspace(LargeShip largeShip)
    {
        largeShip.exitingHyperspace = true;

        Vector3 endPosition = largeShip.transform.localPosition + largeShip.gameObject.transform.forward * 30000;
        Vector3 startPosition = largeShip.gameObject.transform.localPosition;

        float timeElapsed = 0;
        float lerpDuration = 1;

        while (timeElapsed < lerpDuration)
        {
            largeShip.gameObject.transform.localPosition = Vector3.Lerp(startPosition, endPosition, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        largeShip.gameObject.transform.localPosition = endPosition;

        AudioFunctions.PlayAudioClip(largeShip.audioManager, "hyperspace03_exit", "Explosions", largeShip.transform.position, 1, 1, 10000, 1f);

        HudFunctions.AddToShipLog(largeShip.name.ToUpper() + " just exited hyperspace");

        largeShip.exitingHyperspace = false;
    }

    #endregion

    #region largeship task manager

    //This adds a task to the pool
    public static void AddTaskToPool(LargeShip largeShip, Task task)
    {
        if (largeShip.tasks == null)
        {
            largeShip.tasks = new List<Task>();
        }

        largeShip.tasks.Add(task);
    }

    //This ends all task in the ppol
    public static void EndAllTasks(LargeShip largeShip)
    {
        if (largeShip.tasks != null)
        {
            foreach (Task task in largeShip.tasks)
            {
                if (task != null)
                {
                    task.Stop();
                }
            }
        }
    }

    #endregion
}
