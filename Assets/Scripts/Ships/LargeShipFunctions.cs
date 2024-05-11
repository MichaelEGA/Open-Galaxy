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
            largeShip.shipRigidbody = GameObjectUtils.AddRigidbody(largeShip.gameObject, 100f, 9f, 7.5f);
            largeShip.shipRigidbody.isKinematic = true;
            GameObjectUtils.AddColliders(largeShip.gameObject, false);
            largeShip.LODs = GameObjectUtils.GetLODs(largeShip.gameObject);
            largeShip.colliders = largeShip.GetComponentsInChildren<MeshCollider>();
            largeShip.castPoint = largeShip.gameObject.transform.Find("castPoint");
            LoadThrusters(largeShip);
            CreateWaypoint(largeShip);
            DockingFunctions.AddDockingPointsLargeShip(largeShip);

            largeShip.loaded = true;
        }
    }

    //This attaches a particle system to the ship engines to simulate thrust
    public static void LoadThrusters(LargeShip largeShip)
    {
        List<Transform> thrusters = new List<Transform>();

        Transform thruster1 = largeShip.gameObject.transform.Find("thrusters/thruster");
        Transform thruster2 = largeShip.gameObject.transform.Find("thrusters/thruster.001");
        Transform thruster3 = largeShip.gameObject.transform.Find("thrusters/thruster.002");
        Transform thruster4 = largeShip.gameObject.transform.Find("thrusters/thruster.003");
        Transform thruster5 = largeShip.gameObject.transform.Find("thrusters/thruster.004");
        Transform thruster6 = largeShip.gameObject.transform.Find("thrusters/thruster.005");
        Transform thruster7 = largeShip.gameObject.transform.Find("thrusters/thruster.006");
        Transform thruster8 = largeShip.gameObject.transform.Find("thrusters/thruster.007");
        Transform thruster9 = largeShip.gameObject.transform.Find("thrusters/thruster.008");
        Transform thruster10 = largeShip.gameObject.transform.Find("thrusters/thruster.009");
        Transform thruster11 = largeShip.gameObject.transform.Find("thrusters/thruster.010");

        if (thruster1 != null) { thrusters.Add(thruster1); }
        if (thruster2 != null) { thrusters.Add(thruster2); }
        if (thruster3 != null) { thrusters.Add(thruster3); }
        if (thruster4 != null) { thrusters.Add(thruster4); }
        if (thruster5 != null) { thrusters.Add(thruster5); }
        if (thruster6 != null) { thrusters.Add(thruster6); }
        if (thruster7 != null) { thrusters.Add(thruster7); }
        if (thruster8 != null) { thrusters.Add(thruster8); }
        if (thruster9 != null) { thrusters.Add(thruster9); }
        if (thruster10 != null) { thrusters.Add(thruster10); }
        if (thruster11 != null) { thrusters.Add(thruster11); }

        foreach (Transform thruster in thrusters)
        {
            AttachParticleThruster(largeShip, thruster);
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
            Object thrusterObject = PoolUtils.FindPrefabObjectInPool(largeShip.scene.particlePrefabPool, largeShip.thrustType);

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
        if (largeShip.spinShip == false & largeShip.controlLock == false)
        {
            LargeShipAIFunctions.GetAIInput(largeShip);
        }
        else if (largeShip.spinShip == false & largeShip.controlLock == true)
        {
            SmoothPitchInput(largeShip, 0);
            SmoothTurnInput(largeShip, 0);
            SmoothRollInput(largeShip, 0);
        }
        else
        {
            SmoothPitchInput(largeShip, 0);
            SmoothTurnInput(largeShip, 0);
            SmoothRollInput(largeShip, 1);
        }
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
        float actualSpeedRating = largeShip.speedRating;

        //This controls the throttle of the ship, and prevents it going above the speed rating or below zero
        if (actualSpeedRating != 0)
        {
            if (largeShip.thrustSpeed > actualSpeedRating)
            {
                largeShip.thrustSpeed = largeShip.thrustSpeed - acclerationAmount * 4;
            }
            else if (largeShip.thrustInput < 0)
            {
                largeShip.thrustSpeed = largeShip.thrustSpeed - acclerationAmount;
            }
            else if (largeShip.thrustInput > 0)
            {
                largeShip.thrustSpeed = largeShip.thrustSpeed + acclerationAmount;
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

        largeShip.maneuvarabilityActual = (10f / 100f) * (currentManeuverablity * manveurablityPercentageAsDecimal); 

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

        DeactivateShip(largeShip);
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

    #region damage

    //This causes the ship to take damage from lasers
    public static void TakeDamage(LargeShip largeShip, float damage, Vector3 hitPosition)
    {
        if (Time.time - largeShip.loadTime > 10)
        {
            Vector3 relativePosition = largeShip.gameObject.transform.position - hitPosition;
            float forward = -Vector3.Dot(largeShip.gameObject.transform.position, relativePosition.normalized);

            if (largeShip.hullLevel > 0)
            {
                if (forward > 0)
                {
                    if (largeShip.frontShieldLevel > 0)
                    {
                        largeShip.frontShieldLevel = largeShip.frontShieldLevel - damage;
                        largeShip.shieldLevel = largeShip.shieldLevel - damage;
                    }
                    else
                    {
                        if (largeShip.hullLevel - damage < 5 & largeShip.invincible == true)
                        {
                            largeShip.hullLevel = 5;
                        }
                        else
                        {
                            largeShip.hullLevel = largeShip.hullLevel - damage;
                        }
                    }
                }
                else
                {
                    if (largeShip.rearShieldLevel > 0)
                    {
                        largeShip.rearShieldLevel = largeShip.rearShieldLevel - damage;
                        largeShip.shieldLevel = largeShip.shieldLevel - damage;
                    }
                    else
                    {
                        if (largeShip.hullLevel - damage < 5 & largeShip.invincible == true)
                        {
                            largeShip.hullLevel = 5;
                        }
                        else
                        {
                            largeShip.hullLevel = largeShip.hullLevel - damage;
                        }
                    }
                }

                if (largeShip.frontShieldLevel < 0) { largeShip.frontShieldLevel = 0; }
                if (largeShip.rearShieldLevel < 0) { largeShip.rearShieldLevel = 0; }
                if (largeShip.shieldLevel < 0) { largeShip.shieldLevel = 0; }
            }
        }
    }

    //This causes the ship to take damage from lasers
    public static void TakeSystemsDamage(LargeShip largeShip, float damage, Vector3 hitPosition)
    {
        if (Time.time - largeShip.loadTime > 10)
        {
            Vector3 relativePosition = largeShip.gameObject.transform.position - hitPosition;
            float forward = -Vector3.Dot(largeShip.gameObject.transform.position, relativePosition.normalized);

            if (largeShip.systemsLevel > 0)
            {
                if (forward > 0)
                {
                    if (largeShip.frontShieldLevel > 0)
                    {
                        largeShip.frontShieldLevel = largeShip.frontShieldLevel - damage;
                        largeShip.shieldLevel = largeShip.shieldLevel - damage;
                    }
                    else
                    {
                        if (largeShip.systemsLevel - damage < 5 & largeShip.invincible == true)
                        {
                            largeShip.systemsLevel = 5;
                        }
                        else
                        {
                            largeShip.systemsLevel = largeShip.systemsLevel - damage;
                        }
                    }
                }
                else
                {
                    if (largeShip.rearShieldLevel > 0)
                    {
                        largeShip.rearShieldLevel = largeShip.rearShieldLevel - damage;
                        largeShip.shieldLevel = largeShip.shieldLevel - damage;
                    }
                    else
                    {
                        if (largeShip.systemsLevel - damage < 5 & largeShip.invincible == true)
                        {
                            largeShip.systemsLevel = 5;
                        }
                        else
                        {
                            largeShip.systemsLevel = largeShip.systemsLevel - damage;
                        }
                    }
                }

                if (largeShip.frontShieldLevel < 0) { largeShip.frontShieldLevel = 0; }
                if (largeShip.rearShieldLevel < 0) { largeShip.rearShieldLevel = 0; }
                if (largeShip.shieldLevel < 0) { largeShip.shieldLevel = 0; }
            }
        }
        else if (largeShip.isDisabled == false)
        {
            //This tells the player that the ship has been destroyed
            HudFunctions.AddToShipLog(largeShip.name.ToUpper() + " was disabled");
            largeShip.isDisabled = true;
            largeShip.engineAudioSource.Stop();
        }
    }

    //This restores a ships systems to the desired level
    public static void RestoreShipsSystems(LargeShip largeShip, float systemLevel)
    {
        if (systemLevel > 100)
        {
            systemLevel = 100;
        }

        if (systemLevel < 0)
        {
            systemLevel = 0;
        }

        largeShip.systemsLevel = systemLevel;

        if (systemLevel > 0)
        {
            largeShip.isDisabled = false;
            largeShip.engineAudioSource.Play();
        }
    }

    //This selects and runs the appropriate explosion type
    public static void Explode(LargeShip largeShip)
    {
        if (largeShip.hullLevel <= 0 & largeShip.explode == false)
        {
            if (largeShip.explosionType == "type1")
            {
                Task a = new Task(ExplosionType1(largeShip));
                largeShip.explode = true;
            }
            else if (largeShip.explosionType == "type2")
            {
                ExplosionType2(largeShip);
                largeShip.explode = true;
            }
            else
            {
                Task a = new Task(ExplosionType1(largeShip));
                largeShip.explode = true;
            }
        }
    }

    //Multiple small explosions before ship blows up
    public static IEnumerator ExplosionType1(LargeShip largeShip)
    {
        largeShip.spinShip = true;

        yield return new WaitForSeconds(2);

        if (largeShip != null)
        {
            if (largeShip.scene != null)
            {
                ParticleFunctions.InstantiateExplosion(largeShip.scene.gameObject, largeShip.gameObject.transform.position + Vector3.forward * -Random.Range(50, 100), "explosion02", 125, largeShip.audioManager, "proton_explosion1", 1500, "Explosions");
            }
        }

        yield return new WaitForSeconds(2);

        if (largeShip != null)
        {
            if (largeShip.scene != null)
            {
                ParticleFunctions.InstantiateExplosion(largeShip.scene.gameObject, largeShip.gameObject.transform.position + Vector3.right * Random.Range(25, 50), "explosion02", 125, largeShip.audioManager, "proton_explosion1", 1500, "Explosions");
            }
        }

        yield return new WaitForSeconds(2);

        if (largeShip != null)
        {
            if (largeShip.scene != null)
            {
                ParticleFunctions.InstantiateExplosion(largeShip.scene.gameObject, largeShip.gameObject.transform.position + Vector3.right * -Random.Range(25, 50), "explosion02", 125, largeShip.audioManager, "proton_explosion1", 1500, "Explosions");
            }
        }

        yield return new WaitForSeconds(2);

        if (largeShip != null)
        {
            if (largeShip.scene != null)
            {
                ParticleFunctions.InstantiateExplosion(largeShip.scene.gameObject, largeShip.gameObject.transform.position, "explosion03", 1000, largeShip.audioManager, "proton_explosion2", 3000, "Explosions");

                HudFunctions.AddToShipLog(largeShip.name.ToUpper() + " was destroyed");

                DeactivateShip(largeShip);
            }
        }
    }

    //Ship blows up straight away
    public static void ExplosionType2(LargeShip largeShip)
    {
        ParticleFunctions.InstantiateExplosion(largeShip.scene.gameObject, largeShip.gameObject.transform.position, "explosion02", 1000, largeShip.audioManager, "proton_explosion2", 3000, "Explosions");

        HudFunctions.AddToShipLog(largeShip.name.ToUpper() + " was destroyed");

        DeactivateShip(largeShip);
    }

    public static void DeactivateShip(LargeShip largeShip)
    {
        //This sets the ship up for the next time it is loaded from the pool
        largeShip.spinShip = false;
        largeShip.explode = false;

        largeShip.gameObject.SetActive(false);
    }

    #endregion
}
