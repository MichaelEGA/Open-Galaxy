using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LargeShipFunctions
{

    #region start functions

    //This prepares the ship by loading lod and colliders (if not already loaded)
    public static void PrepareShip(LargeShip largeShip)
    {
        if (largeShip.loaded == false)
        {
            GameObjectUtils.AddColliders(largeShip.gameObject, true);
            Rigidbody shipRigidbody = GameObjectUtils.AddRigidbody(largeShip.gameObject, 100f, 9f, 7.5f);
            shipRigidbody.isKinematic = true;
            largeShip.LODs = GameObjectUtils.GetLODs(largeShip.gameObject);        
            largeShip.colliders = largeShip.GetComponentsInChildren<MeshCollider>();
            LoadThrusters(largeShip);
            CreateWaypoint(largeShip);
            largeShip.loaded = true;
        }
    }

    //This attaches a particle system to the ship engines to simulate thrust
    public static void LoadThrusters(LargeShip largeShip)
    {
        Transform thruster1 = largeShip.gameObject.transform.Find("thrusters/thruster");
        Transform thruster2 = largeShip.gameObject.transform.Find("thrusters/thruster.001");
        Transform thruster3 = largeShip.gameObject.transform.Find("thrusters/thruster.002");
        Transform thruster4 = largeShip.gameObject.transform.Find("thrusters/thruster.003");

        if (thruster1 != null)
        {
            AttachParticleThruster(largeShip, thruster1);
        }

        if (thruster2 != null)
        {
            AttachParticleThruster(largeShip, thruster2);
        }

        if (thruster3 != null)
        {
            AttachParticleThruster(largeShip, thruster3);
        }

        if (thruster4 != null)
        {
            AttachParticleThruster(largeShip, thruster4);
        }
    }

    //This attaches the particle trail to the torpedo
    public static void AttachParticleThruster(LargeShip largeShip, Transform thruster)
    {
        if (largeShip.scene == null)
        {
            largeShip.scene = SceneFunctions.GetScene();
        }

        if (largeShip.scene != null)
        {
            Object thrusterObject = PoolUtils.FindPrefabObjectInPool(largeShip.scene.particlePrefabPool, "Thruster_Red");

            if (thrusterObject != null)
            {
                GameObject trail = GameObject.Instantiate(thrusterObject) as GameObject;
                trail.transform.position = thruster.position;
                trail.transform.SetParent(largeShip.gameObject.transform);
            }
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
        LargeShipAIFunctions.GetAIInput(largeShip);      
    }

    //For AI Input. These functions smoothly transitions between different pitch, turn, and roll inputs by lerping between different values like the ai is using a joystick or controller
    public static void SmoothPitchInput(LargeShip largeShip, float pitchInput)
    {
        float step = +Time.deltaTime / 0.01f;
        largeShip.pitchInput = Mathf.Lerp(largeShip.pitchInput, pitchInput, step);
    }

    public static void SmoothTurnInput(LargeShip largeShip, float turnInput)
    {
        float step = +Time.deltaTime / 0.01f;
        largeShip.turnInput = Mathf.Lerp(largeShip.turnInput, turnInput, step);
    }

    public static void SmoothRollInput(LargeShip largeShip, float rollInput)
    {
        float step = +Time.deltaTime / 0.01f;
        largeShip.rollInput = Mathf.Lerp(largeShip.rollInput, rollInput, step);
    }

    #endregion

    #region ship movement

    //This calculates the thrust speed of the ship
    public static void CalculateThrustSpeed(LargeShip largeShip)
    {
        //This calculates the normal accleration and speed rating
        float acclerationAmount = (0.5f / 100f) * largeShip.accelerationRating;
        float actualSpeedRating = largeShip.speedRating;

        //This controls the throttle of the ship, and prevents it going above the speed rating or below zero
        if (largeShip.thrustSpeed > actualSpeedRating)
        {
            largeShip.thrustSpeed = largeShip.thrustSpeed - acclerationAmount * 4;
        }
        else if (largeShip.thrustInput < 0 & largeShip.thrustTimeStamp < Time.time)
        {
            largeShip.thrustSpeed = largeShip.thrustSpeed - acclerationAmount;
            largeShip.thrustTimeStamp = Time.time + 0.01f;
        }
        else if (largeShip.thrustInput > 0 & largeShip.thrustTimeStamp < Time.time)
        {
            largeShip.thrustSpeed = largeShip.thrustSpeed + acclerationAmount;
            largeShip.thrustTimeStamp = Time.time + 0.01f;
        }

        if (largeShip.thrustSpeed < 0)
        {
            largeShip.thrustSpeed = 0;
        }
    }

    //This calculates pitch, turn, and roll according to the speed of the vehicle
    public static void CalculatePitchTurnRollSpeeds(LargeShip largeShip)
    {
        float peakManeuverSpeed = largeShip.speedRating / 2f;
        float currentManeuverablity = 0f;
        float manveurablityPercentageAsDecimal = 0f;

        if (largeShip.thrustSpeed <= peakManeuverSpeed & largeShip.thrustSpeed > (peakManeuverSpeed / 3f))
        {
            currentManeuverablity = (100f / peakManeuverSpeed) * largeShip.thrustSpeed;
        }
        else if (largeShip.thrustSpeed >= peakManeuverSpeed & largeShip.thrustSpeed < (largeShip.speedRating - (peakManeuverSpeed / 3f)))
        {
            currentManeuverablity = (100f / peakManeuverSpeed) * (peakManeuverSpeed - (largeShip.thrustSpeed - peakManeuverSpeed));
        }
        else
        {
            currentManeuverablity = (100f / peakManeuverSpeed) * (peakManeuverSpeed / 3f);
        }

        manveurablityPercentageAsDecimal = (largeShip.maneuverabilityRating / 100f);

        largeShip.pitchSpeed = (120 / 100f) * (currentManeuverablity * manveurablityPercentageAsDecimal);
        largeShip.turnSpeed = (100 / 100f) * (currentManeuverablity * manveurablityPercentageAsDecimal);
        largeShip.rollSpeed = (160 / 100f) * (currentManeuverablity * manveurablityPercentageAsDecimal);
    }

    //This makes the ship move
    public static void MoveShip(LargeShip largeShip)
    {
        if (largeShip.shipRigidbody == null)
        {
            largeShip.shipRigidbody = largeShip.gameObject.GetComponent<Rigidbody>();
        }

        //This smoothly increases and decreases pitch, turn, and roll to provide smooth movement;
        largeShip.pitchInputActual = Mathf.Lerp(largeShip.pitchInputActual, largeShip.pitchInput, 0.1f);
        largeShip.turnInputActual = Mathf.Lerp(largeShip.turnInputActual, largeShip.turnInput, 0.1f);
        largeShip.rollInputActual = Mathf.Lerp(largeShip.rollInputActual, largeShip.rollInput, 0.1f);

        //This makes the vehicle fly
        if (largeShip.thrustSpeed > 0)
        {
            largeShip.gameObject.transform.Translate(Vector3.forward * Time.deltaTime * largeShip.thrustSpeed);
        }

        Vector3 x = Vector3.right * largeShip.pitchInputActual * largeShip.pitchSpeed;
        Vector3 y = Vector3.up * largeShip.turnInputActual * largeShip.turnSpeed;
        Vector3 z = Vector3.forward * largeShip.rollInputActual * largeShip.rollSpeed;

        Vector3 rotationVector = x + y + z;

        Quaternion deltaRotation = Quaternion.Euler(rotationVector * Time.deltaTime);
        largeShip.shipRigidbody.MoveRotation(largeShip.shipRigidbody.rotation * deltaRotation);
    }

    #endregion
}
