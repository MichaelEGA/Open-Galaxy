using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UIElements;

public static class DamageSystemFunctions
{
    #region smallship damage functions

    //This causes the ship to take damage from lasers and torpedoes
    public static void TakeDamage_SmallShip(DamageSystem damageSystem, float damage, Vector3 hitPosition, bool isRapidFire = false)
    {
        EnergyManagementSystem energyManagementSystem = damageSystem.ship.energyManagementSystem;

        if (damageSystem.ship.isAI == false)
        {
            damage = CalculateDamage_SmallShip(damage);
        }

        if (Time.time - damageSystem.ship.loadTime > 10)
        {
            Vector3 relativePosition = damageSystem.gameObject.transform.position - hitPosition;
            float forward = -Vector3.Dot(damageSystem.gameObject.transform.position, relativePosition.normalized);

            if (damageSystem.ship.energyManagementSystem.hullLevel > 0)
            {
                if (forward > 0)
                {
                    //This calculates the damage
                    if (energyManagementSystem.frontShieldLevel > 0)
                    {
                        if (damageSystem.shieldType == "blackhole" & isRapidFire == false) //This minimises the damage on ships with black hole shields
                        {
                            damage = (damage / 100f) * 10;
                        }

                        energyManagementSystem.frontShieldLevel = energyManagementSystem.frontShieldLevel - damage;
                        energyManagementSystem.shieldLevel = energyManagementSystem.shieldLevel - damage;
                    }
                    else
                    {
                        if (isRapidFire == true) //This minimises the damage when the ship is using rapid fire
                        {
                            damage = (damage / 100f) * 2;
                        }

                        if (energyManagementSystem.hullLevel - damage < 5 & damageSystem.invincible == true)
                        {
                            energyManagementSystem.hullLevel = 5;
                        }
                        else
                        {
                            energyManagementSystem.hullLevel = energyManagementSystem.hullLevel - damage;
                        }
                    }
                }
                else
                {
                    //This calculates the damage
                    if (energyManagementSystem.rearShieldLevel > 0)
                    {
                        if (damageSystem.shieldType == "blackhole" & isRapidFire == false) //This minimises the damage on ships with black hole shields
                        {
                            damage = (damage / 100f) * 10;
                        }

                        energyManagementSystem.rearShieldLevel = energyManagementSystem.rearShieldLevel - damage;
                        energyManagementSystem.shieldLevel = energyManagementSystem.shieldLevel - damage;
                    }
                    else
                    {
                        if (isRapidFire == true) //This minimises the damage when the ship is using rapid fire
                        {
                            damage = (damage / 100f) * 2;
                        }

                        if (energyManagementSystem.hullLevel - damage < 5 & damageSystem.invincible == true)
                        {
                            energyManagementSystem.hullLevel = 5;
                        }
                        else
                        {
                            energyManagementSystem.hullLevel = energyManagementSystem.hullLevel - damage;
                        }
                    }
                }

                if (energyManagementSystem.frontShieldLevel < 0) { energyManagementSystem.frontShieldLevel = 0; }
                if (energyManagementSystem.rearShieldLevel < 0) { energyManagementSystem.rearShieldLevel = 0; }
                if (energyManagementSystem.shieldLevel < 0) { energyManagementSystem.shieldLevel = 0; }

                if (damageSystem.ship.isAI == false)
                {
                    //This shakes the cockpit camera
                    damageSystem.ship.scene.ogCamera.shipHit = true;

                    
                }

                if (damageSystem.ship.ogInput == null)
                {
                    damageSystem.ship.ogInput = OGInputFunctions.GetOGInput();
                }

                if (damageSystem.ship.isAI == false & damageSystem.ship.ogInput.keyboardAndMouse == false)
                {
                    Task b = new Task(OGInputFunctions.ShakeControllerForSetTime(0.25f, 0.65f, 0.65f));
                }
            }
        }
    }

    //This plays a sound when the player ships hull is less than 25
    public static void PlayDamageWarningSound(DamageSystem damageSystem)
    {
        if (damageSystem.ship.isAI == false)
        {
            //This plays a warning sound
            if (damageSystem.ship.energyManagementSystem.hullLevel < 25 & damageSystem.warningSoundPlayed == false)
            {
                AudioFunctions.PlayAudioClip(damageSystem.ship.audioManager, "beep_alert", "Cockpit", new Vector3(0, 0, 0), 0, 1, 500, 0.6f);

                damageSystem.warningSoundPlayed = true;
            }
        }
    }

    //This causes the ship to take damage from lasers and torpedoes
    public static void TakeSystemDamage_SmallShip(DamageSystem damageSystem, float damage, Vector3 hitPosition, bool isRapidFire = false)
    {
        //This sets the time until the ship systems start restoring
        damageSystem.restoreDelayTime = Time.time + 15;

        //This calculates the damage level for different difficultes
        if (damageSystem.ship.isAI == false)
        {
            damage = CalculateDamage_SmallShip(damage);
        }

        //This calculates the damage
        if (Time.time - damageSystem.ship.loadTime > 10)
        {
            Vector3 relativePosition = damageSystem.gameObject.transform.position - hitPosition;
            float forward = -Vector3.Dot(damageSystem.gameObject.transform.position, relativePosition.normalized);

            if (damageSystem.ship.energyManagementSystem.systemsLevel > 0)
            {
                if (forward > 0)
                {
                    if (damageSystem.ship.energyManagementSystem.frontShieldLevel > 0)
                    {
                        if (damageSystem.ship.weaponManagement.hasPlasma == true & isRapidFire == false) //This minimises the damage on ships with black hole shields
                        {
                            damage = (damage / 100f) * 10;
                        }

                        damageSystem.ship.energyManagementSystem.frontShieldLevel = damageSystem.ship.energyManagementSystem.frontShieldLevel - damage;
                        damageSystem.ship.energyManagementSystem.shieldLevel = damageSystem.ship.energyManagementSystem.shieldLevel - damage;
                    }
                    else
                    {
                        if (damageSystem.ship.weaponManagement.hasPlasma == false) //Vong ships are not harmed by ion cannons 
                        {
                            if (damageSystem.ship.energyManagementSystem.systemsLevel - damage < 5 & damageSystem.invincible == true)
                            {
                                damageSystem.ship.energyManagementSystem.systemsLevel = 5;
                            }
                            else
                            {
                                damageSystem.ship.energyManagementSystem.systemsLevel = damageSystem.ship.energyManagementSystem.systemsLevel - damage;
                            }
                        }
                    }
                }
                else
                {
                    if (damageSystem.ship.energyManagementSystem.rearShieldLevel > 0)
                    {
                        if (damageSystem.ship.weaponManagement.hasPlasma == true & isRapidFire == false) //This minimises the damage on ships with black hole shields
                        {
                            damage = (damage / 100f) * 10;
                        }

                        damageSystem.ship.energyManagementSystem.rearShieldLevel = damageSystem.ship.energyManagementSystem.rearShieldLevel - damage;
                        damageSystem.ship.energyManagementSystem.shieldLevel = damageSystem.ship.energyManagementSystem.shieldLevel - damage;
                    }
                    else
                    {
                        if (damageSystem.ship.weaponManagement.hasPlasma == false) //Vong ships are not harmed by ion cannons
                        {
                            if (damageSystem.ship.energyManagementSystem.systemsLevel - damage < 5 & damageSystem.cannotbedisabled == true)
                            {
                                damageSystem.ship.energyManagementSystem.systemsLevel = 5;
                            }
                            else
                            {
                                damageSystem.ship.energyManagementSystem.systemsLevel = damageSystem.ship.energyManagementSystem.systemsLevel - damage;
                            }
                        }
                    }
                }

                if (damageSystem.ship.energyManagementSystem.frontShieldLevel < 0) { damageSystem.ship.energyManagementSystem.frontShieldLevel = 0; }
                if (damageSystem.ship.energyManagementSystem.rearShieldLevel < 0) { damageSystem.ship.energyManagementSystem.rearShieldLevel = 0; }
                if (damageSystem.ship.energyManagementSystem.shieldLevel < 0) { damageSystem.ship.energyManagementSystem.shieldLevel = 0; }

                if (damageSystem.ship.isAI == false)
                {
                    damageSystem.ship.scene.ogCamera.shipHit = true;
                }

                if (damageSystem.ship.ogInput == null)
                {
                    damageSystem.ship.ogInput = OGInputFunctions.GetOGInput();
                }

                if (damageSystem.ship.isAI == false & damageSystem.ship.ogInput.keyboardAndMouse == false)
                {
                    Task b = new Task(OGInputFunctions.ShakeControllerForSetTime(0.25f, 0.65f, 0.65f));
                }
            }

            //This disables a ship that has less than 1 percent systems power
            if (damageSystem.isDisabled == false & damageSystem.ship.energyManagementSystem.systemsLevel <= 0)
            {
                //Stops listing the ship as targetting another ship
                if (damageSystem.ship.targetingSystem.target != null)
                {
                    if (damageSystem.ship.targetingSystem.target.gameObject.activeSelf == true)
                    {
                        if (damageSystem.ship.targetingSystem.targetShip != null)
                        {
                            damageSystem.ship.targetingSystem.targetShip.ship.targetingSystem.numberTargeting -= 1;
                        }
                    }

                    damageSystem.ship.targetingSystem.target = null;
                }

                //This tells the player that the ship has been destroyed
                HudFunctions.AddToShipLog(damageSystem.name.ToUpper() + " was disabled");
                damageSystem.isDisabled = true;

                //This causes the ship to spin a little rather than being completely stationary
                damageSystem.ship.flightControlSystem_Small.shipRigidbody.angularVelocity = Random.onUnitSphere * Random.Range(0.1f, 1f);
                damageSystem.ship.flightControlSystem_Small.shipRigidbody.angularDamping = 0; //Prevents the spin from stopping
                damageSystem.ship.flightControlSystem_Small.shipRigidbody.linearVelocity = Random.onUnitSphere * Random.Range(0.1f, 1f);
                damageSystem.ship.flightControlSystem_Small.shipRigidbody.linearDamping = 0; //Prevents the movements from stopping

                //This creates an explosion where the ship is
                float explosionsScale = damageSystem.ship.shipLength / 10;

                ParticleFunctions.InstantiateExplosion(damageSystem.gameObject.transform.position, "explosion_ion_smallship", explosionsScale);

                //This makes an explosion sound
                AudioFunctions.PlayAudioClip(damageSystem.ship.audioManager, "impact01_laserhitshield", "External", damageSystem.gameObject.transform.position, 1, 1, 1000, 1);

                if (damageSystem.ship.isAI == false & damageSystem.ship.ogInput.keyboardAndMouse == false)
                {
                    Task a = new Task(OGInputFunctions.ShakeControllerForSetTime(0.5f, 0.90f, 0.90f));
                }


            }
        }
    }

    //This restores a ships systems to the desired level
    public static void RestoreShipsSystems_SmallShip(DamageSystem damageSystem)
    {
        if (damageSystem.ship.isAI == false)
        {
            if (Time.time > damageSystem.restoreDelayTime)
            {
                if (damageSystem.ship.energyManagementSystem.systemsLevel < 0)
                {
                    damageSystem.ship.energyManagementSystem.systemsLevel = 0;
                }
                else if (damageSystem.ship.energyManagementSystem.systemsLevel < 100)
                {
                    damageSystem.ship.energyManagementSystem.systemsLevel += 1;
                }
            }
        }

        if (damageSystem.isDisabled == true & damageSystem.ship.energyManagementSystem.systemsLevel > 0)
        {
            damageSystem.ship.flightControlSystem_Small.shipRigidbody.linearVelocity = new Vector3(0f, 0f, 0f);
            damageSystem.ship.flightControlSystem_Small.shipRigidbody.angularVelocity = new Vector3(0f, 0f, 0f);
            damageSystem.ship.flightControlSystem_Small.shipRigidbody.linearDamping = 9;
            damageSystem.ship.flightControlSystem_Small.shipRigidbody.angularDamping = 7.5f;

            damageSystem.isDisabled = false;

            HudFunctions.AddToShipLog(damageSystem.name.ToUpper() + " has restored systems");
        }
    }

    //Calculate damage according to difficulty
    public static float CalculateDamage_SmallShip(float damage)
    {
        OGSettings settings = OGSettingsFunctions.GetSettings();

        if (settings != null)
        {
            if (settings.damage == "default")
            {
                //No change
            }
            else if (settings.damage == "moderate")
            {
                damage = (damage / 4f) * 3f;
            }
            else if (settings.damage == "low")
            {
                damage = (damage / 4f) * 2f;
            }
            else if (settings.damage == "minimal")
            {
                damage = (damage / 4f) * 2f;
            }
            else if (settings.damage == "nodamage")
            {
                damage = 0;
            }
        }

        return damage;
    }

    //This tells the damage system that a collision has begun
    public static void StartCollision_SmallShip(DamageSystem damageSystem, GameObject collidingWith)
    {
        if (damageSystem != null & collidingWith != null)
        {
            if (damageSystem.ship.flightControlSystem_Small.docking == false)
            {
                damageSystem.isCurrentlyColliding = true;

                DamageSystem damageSystem2 = collidingWith.GetComponentInParent<DamageSystem>();

                if (damageSystem2 != null)
                {
                    damageSystem2.isCurrentlyCollidingSmallShip = true;
                }

                if (damageSystem.ship.isAI == false & damageSystem.invincible == false)
                {
                    AudioFunctions.PlayAudioClip(damageSystem.ship.audioManager, "impact03_crash", "Cockpit", damageSystem.gameObject.transform.position, 0, 1, 500, 1, 100);

                    if (damageSystem.ship.isAI == false & damageSystem.ship.ogInput.keyboardAndMouse == false)
                    {
                        Task a = new Task(OGInputFunctions.ShakeControllerForSetTime(0.5f, 0.90f, 0.90f));
                    }
                }
            }
        }
    }

    //This tells the damage system that a collision has ended
    public static void EndCollision_SmallShip(DamageSystem damageSystem)
    {
        damageSystem.isCurrentlyColliding = false;
        damageSystem.isCurrentlyCollidingSmallShip = false;
    }

    //This called when the ship collides with something causing it to take collision damage
    public static void TakeCollisionDamage_SmallShip(DamageSystem damageSystem)
    {
        EnergyManagementSystem energyManagementSystem = damageSystem.ship.energyManagementSystem;

        if (damageSystem.isCurrentlyColliding == true & damageSystem.invincible == false & damageSystem.ship.flightControlSystem_Small.docking == false)
        {
            if (Time.time - damageSystem.ship.loadTime > 10)
            {
                if (energyManagementSystem.hullLevel > 0 & damageSystem.invincible == false)
                {
                    if (damageSystem.invincible == true & energyManagementSystem.hullLevel - 5 < 5)
                    {
                        energyManagementSystem.hullLevel = 5;
                    }
                    else
                    {
                        if (damageSystem.isCurrentlyCollidingSmallShip == true)
                        {
                            energyManagementSystem.hullLevel -= 5;
                        }
                        else
                        {
                            energyManagementSystem.hullLevel -= 50;
                        }
                    }

                    if (energyManagementSystem.hullLevel < 0)
                    {
                        energyManagementSystem.hullLevel = 0;
                    }

                    //This shakes the cockpit camera
                    if (damageSystem.ship.isAI == false)
                    {
                        damageSystem.ship.scene.ogCamera.shipHit = true;
                    }
                }
            }
        }
    }

    //This causes a smoke trail to appear behind the damaged ship
    public static void SmokeTrail_SmallShip(DamageSystem damageSystem)
    {
        EnergyManagementSystem energyManagementSystem = damageSystem.ship.energyManagementSystem;

        if (damageSystem.ship.energyManagementSystem.hullLevel < 10 & damageSystem.smokeTrail == null & damageSystem.ship.scene != null & damageSystem.ship.isAI == true)
        {
            Object tempSmokeTrail = PoolUtils.FindPrefabObjectInPool(damageSystem.ship.scene.particlePrefabPool, "SmokeTrail");

            if (tempSmokeTrail != null)
            {
                GameObject smokeTrail = GameObject.Instantiate(tempSmokeTrail) as GameObject;
                damageSystem.smokeTrail = smokeTrail;
                smokeTrail.transform.SetParent(damageSystem.transform);
                smokeTrail.transform.localPosition = new Vector3(0, 0, 0);
                smokeTrail.layer = damageSystem.gameObject.layer;
                smokeTrail.SetActive(true);
            }

            //This sets the smoke trails simulation space to the scene
            if (damageSystem.smokeTrail != null)
            {
                ParticleSystem particleSystem = damageSystem.smokeTrail.GetComponent<ParticleSystem>();

                if (particleSystem != null)
                {
                    Scene scene = SceneFunctions.GetScene();

                    var main = particleSystem.main;

                    main.customSimulationSpace = scene.transform;

                    particleSystem.transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }
        else if (energyManagementSystem.hullLevel < 10 & damageSystem.smokeTrail != null)
        {
            damageSystem.smokeTrail.SetActive(true);
        }
        else if (energyManagementSystem.hullLevel > 10 & damageSystem.smokeTrail != null)
        {
            damageSystem.smokeTrail.SetActive(false);
        }
    }

    //This causes the ship to explode
    public static void Explode_SmallShip(DamageSystem damageSystem)
    {
        EnergyManagementSystem energyManagementSystem = damageSystem.ship.energyManagementSystem;

        if (energyManagementSystem.hullLevel <= 0 & damageSystem.exploded == false || energyManagementSystem.hullLevel <= 0 & damageSystem.exploded == false & damageSystem.isCurrentlyColliding == true)
        {
            int explosion = Random.Range(0, 3);

            if (explosion == 0 & damageSystem.ship.isAI == true)
            {
                Task a = new Task(ExplosionType_Spin_SmallShip(damageSystem));
                damageSystem.exploded = true;
            }
            else
            {
                ExplosionType_Immediate_SmallShip(damageSystem);
                damageSystem.exploded = true;
            }
        }
    }

    //Explode after spinning
    public static IEnumerator ExplosionType_Spin_SmallShip(DamageSystem damageSystem)
    {
        if (damageSystem.isCurrentlyColliding == false)
        {
            damageSystem.ship.flightControlSystem_Small.spinShip = true;
            float time = Random.Range(2, 6);
            yield return new WaitForSeconds(time);
        }

        if (damageSystem != null)
        {
            if (damageSystem.ship.scene == null)
            {
                damageSystem.ship.scene = SceneFunctions.GetScene();
            }

            //This creates an explosion where the ship is
            float explosionsScale = damageSystem.ship.shipLength / 5;

            ParticleFunctions.InstantiateExplosion(damageSystem.gameObject.transform.position, "explosion_smallship", explosionsScale);

            //This makes an explosion sound
            AudioFunctions.PlayAudioClip(damageSystem.ship.audioManager, "mid_explosion_01", "External", damageSystem.gameObject.transform.position, 1, 1, 1000, 1);

            //This tells the player that the ship has been destroyed
            HudFunctions.AddToShipLog(damageSystem.name.ToUpper() + " was destroyed");

            //This deactivates the ship
            DeactivateShip_SmallShip(damageSystem);
        }
    }

    //Explode straight away
    public static void ExplosionType_Immediate_SmallShip(DamageSystem damageSystem)
    {
        if (damageSystem.ship.scene == null)
        {
            damageSystem.ship.scene = SceneFunctions.GetScene();
        }

        //This creates an explosion where the ship is
        float explosionsScale = damageSystem.ship.shipLength / 5;

        ParticleFunctions.InstantiateExplosion(damageSystem.gameObject.transform.position, "explosion_smallship", explosionsScale);

        //This makes an explosion sound
        AudioFunctions.PlayAudioClip(damageSystem.ship.audioManager, "mid_explosion_01", "External", damageSystem.gameObject.transform.position, 1, 1, 1000, 1);

        //This tells the game that the ship has been destroyed
        HudFunctions.AddToShipLog(damageSystem.name.ToUpper() + " was destroyed");

        //This deactivates the ship
        DeactivateShip_SmallShip(damageSystem);
    }

    public static IEnumerator ShipSpinSequence_SmallShip(DamageSystem damageSystem, float time)
    {
        damageSystem.ship.flightControlSystem_Small.spinShip = true;

        if (damageSystem.isCurrentlyColliding == false)
        {
            yield return new WaitForSeconds(time);
        }

        damageSystem.ship.flightControlSystem_Small.spinShip = false;
    }

    public static void DeactivateShip_SmallShip(DamageSystem damageSystem)
    {
        //This gets the scene reference
        Scene scene = damageSystem.ship.scene;

        //Stops listing the ship as targetting another ship
        if (damageSystem.ship.targetingSystem.target != null)
        {
            if (damageSystem.ship.targetingSystem.target.gameObject.activeSelf == true)
            {
                if (damageSystem.ship.targetingSystem.targetShip != null)
                {
                    damageSystem.ship.targetingSystem.targetShip.targetingSystem.numberTargeting -= 1;
                }
            }

            damageSystem.ship.targetingSystem.target = null;
        }

        //This cancels any docking procedures
        DockingFunctions.CancelDocking(damageSystem.ship, null);

        //This turns of the engine sound and release the ship audio source from the ship
        if (damageSystem.ship.audioManager != null)
        {
            if (damageSystem.ship.flightControlSystem_Small.engineAudioSource != null)
            {
                damageSystem.ship.flightControlSystem_Small.engineAudioSource.Stop();
                damageSystem.ship.flightControlSystem_Small.engineAudioSource = null;
            }

            damageSystem.ship.audioManager = null;
        }

        Transform ogCamera = GameObjectUtils.FindChildTransformContaining(damageSystem.gameObject.transform, "ogcameraGO");

        if (ogCamera != null)
        {
            ogCamera.transform.parent = null;
        }

        //This resets the ship for the next load if needed
        damageSystem.exploded = false;

        //This unloads the mission
        if (damageSystem.ship.isAI == false)
        {
            MissionFunctions.ExitOnPlayerDestroy();
        }

        //This deactivates the ship
        GameObject.Destroy(damageSystem.ship.targetingSystem.waypoint);
        GameObject.Destroy(damageSystem.gameObject);

        //This removes null objects from the pool
        scene.objectPool.RemoveAll(item => item == null);
    }

    #endregion

    #region largeship damage functions

    //This causes the ship to take damage from lasers
    public static void TakeDamage_LargeShip(LargeShip largeShip, float damage, Vector3 hitPosition)
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
    public static void TakeSystemDamage_LargeShip(LargeShip largeShip, float damage, Vector3 hitPosition)
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
                        if (largeShip.systemsLevel - damage < 5 & largeShip.cannotbedisabled == true)
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

        if (largeShip.isDisabled == false & largeShip.systemsLevel <= 0)
        {
            //This tells the player that the ship has been destroyed
            HudFunctions.AddToShipLog(largeShip.name.ToUpper() + " was disabled");
            largeShip.isDisabled = true;
        }

    }

    //This restores a ships systems to the desired level
    public static void RestoreShipsSystems_LargeShip(LargeShip largeShip)
    {
        if (largeShip.isDisabled == true & largeShip.systemsLevel > 0)
        {
            HudFunctions.AddToShipLog(largeShip.name.ToUpper() + " has restored systems");
            largeShip.isDisabled = false;
        }
    }

    //This selects and runs the appropriate explosion type
    public static void Explode_LargeShip(LargeShip largeShip)
    {
        if (largeShip.hullLevel <= 0 & largeShip.explode == false)
        {
            Task a = new Task(Explosion_LargeShip(largeShip));
            LargeShipFunctions.AddTaskToPool(largeShip, a);
            largeShip.explode = true;
        }
    }

    //Multiple small explosions before ship blows up
    public static IEnumerator Explosion_LargeShip(LargeShip largeShip)
    {
        largeShip.spinShip = true;

        var largest = GameObjectUtils.FindLargestMeshByLength(largeShip.GameObject());

        MeshFilter largestMeshFilter = null;
        SkinnedMeshRenderer largestSkinnedMeshRenderer = null;
        Mesh mainShipMesh = null;
        Transform meshTransform = null;

        if (largest is MeshFilter meshFilter)
        {
            largestMeshFilter = largest as MeshFilter;

            mainShipMesh = largestMeshFilter.sharedMesh;
            meshTransform = largestMeshFilter.transform;
        }
        else if (largest is SkinnedMeshRenderer skinnedMeshRenderer)
        {
            largestSkinnedMeshRenderer = largest as SkinnedMeshRenderer;

            mainShipMesh = largestSkinnedMeshRenderer.sharedMesh;
            meshTransform = largestSkinnedMeshRenderer.transform;
        }

        if (mainShipMesh != null)
        {
            int explosionsNumber = (int)Mathf.Abs((largeShip.shipLength / 100f) * 10f);

            List<Vector3> explosionPoints = GameObjectUtils.GetRandomPointsOnMesh(mainShipMesh, meshTransform, explosionsNumber);
            List<ParticleSystem> explosions = new List<ParticleSystem>();

            foreach (Vector3 explosionPoint in explosionPoints)
            {
                if (explosionPoint != null)
                {
                    if (largeShip != null)
                    {
                        if (largeShip.scene != null)
                        {
                            float explosionsScale = (largeShip.shipLength / 100f) * Random.Range(0.10f, 0.15f); ///15-30 

                            Vector3 worldPoint = meshTransform.TransformPoint(explosionPoint);

                            ParticleSystem explosion = ParticleFunctions.InstantiateExplosion(worldPoint, "explosion_largeship", explosionsScale, largeShip.audioManager, "proton_explosion1", 1500, "Explosions");

                            explosions.Add(explosion);

                            float waitTime = Random.Range(0.10f, 0.45f);

                            yield return new WaitForSeconds(0.25f);
                        }
                    }
                }
            }

            if (largeShip != null)
            {
                if (largeShip.scene != null)
                {
                    float explosionsScale2 = largeShip.shipLength / 25;

                    ParticleFunctions.InstantiateExplosion(largeShip.gameObject.transform.position, "explosion_largeship", explosionsScale2, largeShip.audioManager, "proton_explosion2", 3000, "Explosions");

                    yield return new WaitForSeconds(1.5f);

                    //This removes all the other explosions
                    if (explosions != null)
                    {
                        if (explosions.Count > 0)
                        {
                            foreach (ParticleSystem explosion in explosions)
                            {
                                if (explosion != null)
                                {
                                    GameObject.Destroy(explosion.gameObject);
                                }
                            }
                        }
                    }

                    int scaleNumber = (int)Mathf.Abs(largeShip.shipLength / 100f);

                    TriggerDebrisExplosion_LargeShip(largeShip.gameObject.transform.position, 50 * scaleNumber);

                    HudFunctions.AddToShipLog(largeShip.name.ToUpper() + " was destroyed");

                    yield return new WaitForSeconds(0.75f);

                    DeactivateShip_LargeShip(largeShip);
                }
            }
        }
        else
        {
            if (largeShip != null)
            {
                if (largeShip.scene != null)
                {
                    float explosionsScale = largeShip.shipLength / 25;

                    ParticleFunctions.InstantiateExplosion(largeShip.gameObject.transform.position, "explosion_largeship", explosionsScale, largeShip.audioManager, "proton_explosion2", 3000, "Explosions");

                    yield return new WaitForSeconds(1f);

                    int scaleNumber = (int)Mathf.Abs(largeShip.shipLength / 100f);

                    TriggerDebrisExplosion_LargeShip(largeShip.gameObject.transform.position, 50 * scaleNumber);

                    HudFunctions.AddToShipLog(largeShip.name.ToUpper() + " was destroyed");

                    DeactivateShip_LargeShip(largeShip);
                }
            }
        }
    }

    //This deactivates the ship so that it no longer appears in the scene
    public static void DeactivateShip_LargeShip(LargeShip largeShip)
    {
        //This gets the scene reference
        Scene scene = largeShip.scene;

        LargeShipFunctions.EndAllTasks(largeShip);

        DockingFunctions.CancelDocking(null, largeShip);

        //This sets the ship up for the next time it is loaded from the pool
        largeShip.spinShip = false;
        largeShip.explode = false;

        GameObject.Destroy(largeShip.waypoint);
        GameObject.Destroy(largeShip.gameObject);

        //This removes null objects from the pool
        scene.objectPool.RemoveAll(item => item == null);
    }

    //This creates a debris explosions
    public static void TriggerDebrisExplosion_LargeShip(Vector3 position, int debrisCount = 10)
    {
        Scene scene = SceneFunctions.GetScene();

        List<GameObject> debrisPrefabs = new List<GameObject>();

        foreach (GameObject objectPrefab in scene.shipsPrefabPool)
        {
            if (objectPrefab.name.Contains("debris02"))
            {
                debrisPrefabs.Add(objectPrefab);
            }
        }

        float spawnRadius = 1;
        float explosionForce = 100f;
        float explosionRadius = 5f;
        float upwardsModifier = 0.5f;
        float debrisLifetime = 15f;
        float scale = 0.005f;

        for (int i = 0; i < debrisCount; i++)
        {
            // Pick a random prefab
            GameObject prefab = debrisPrefabs[Random.Range(0, debrisPrefabs.Count)];
            Vector3 spawnPos = position + Random.insideUnitSphere * spawnRadius;
            GameObject debris = GameObject.Instantiate(prefab) as GameObject;

            debris.transform.position = spawnPos;
            debris.transform.rotation = Random.rotation;
            debris.transform.localScale = new Vector3(scale, scale, scale);
            debris.transform.parent = scene.transform;

            // Ensure debris has Rigidbody
            Rigidbody rb = debris.GetComponent<Rigidbody>();

            if (rb == null)
            {
                rb = debris.AddComponent<Rigidbody>();
            }

            // Apply explosion force
            rb.AddExplosionForce(explosionForce, position, explosionRadius, upwardsModifier, ForceMode.Impulse);

            // Destroy debris after some time
            GameObject.Destroy(debris, debrisLifetime);
        }
    }

    #endregion

    #region system damage functions

    //This applies damage to the system
    public static void TakeShipSystemDamage(ShipSystem shipSystem, float damage)
    {
        shipSystem.hull = shipSystem.hull - damage;

        if (shipSystem.hull <= 0)
        {
            Renderer targetRenderer = shipSystem.gameObject.GetComponent<Renderer>();

            float explosionScale = GetExplosionScale(shipSystem.gameObject, targetRenderer.bounds.size.y);
            
            if (shipSystem.ionParticleSystemGO != null)
            {
                GameObject.Destroy(shipSystem.ionParticleSystemGO);
            }

            Bounds localBounds = targetRenderer.localBounds;

            Vector3 localBottomCenter = localBounds.center - Vector3.up * localBounds.extents.y;

            Vector3 loadPosition = targetRenderer.transform.TransformPoint(localBottomCenter);

            ParticleSystem systemSmoke = ParticleFunctions.InstantiatePersistantExplosion(loadPosition, "SystemSmoke", explosionScale);
            systemSmoke.transform.SetParent(shipSystem.gameObject.transform.parent, true);
            systemSmoke.transform.localRotation = shipSystem.gameObject.transform.localRotation;
            shipSystem.gameObject.SetActive(false);
            HudFunctions.AddToShipLog(shipSystem.name.ToUpper() + " was destroyed.");
        }
    }

    //This causes the ship to take damage from lasers and torpedoes
    public static void TakeShipSystemSystemDamage(ShipSystem shipSystem, float damage)
    {
        shipSystem.systems = shipSystem.systems - damage;

        if (shipSystem.systems <= 0 & shipSystem.disabled == false)
        {
            Renderer targetRenderer = shipSystem.gameObject.GetComponent<Renderer>();

            shipSystem.disabled = true;
            float explosionScale = GetExplosionScale(shipSystem.gameObject, targetRenderer.bounds.size.y);
            ParticleSystem ionParticleSystem = ParticleFunctions.InstantiatePersistantExplosion(shipSystem.transform.position, "explosion_system_ion", explosionScale);
            shipSystem.ionParticleSystemGO = ionParticleSystem.gameObject;
            HudFunctions.AddToShipLog(shipSystem.name.ToUpper() + " was disabled.");
        }
    }

    public static float GetExplosionScale(GameObject systemGO, float boundsSize)
    {
        float scale = boundsSize / 4f;

        if (scale > 5)
        {
            scale = 5;
        }
        
        return scale;
    }

    #endregion
}
