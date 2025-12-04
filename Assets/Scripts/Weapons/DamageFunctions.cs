using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public static class DamageFunctions
{
    #region smallship damage functions

    //This causes the ship to take damage from lasers and torpedoes
    public static void TakeDamage_SmallShip(SmallShip smallShip, float damage, Vector3 hitPosition, bool isRapidFire = false)
    {
        if (smallShip.isAI == false)
        {
            damage = CalculateDamage_SmallShip(damage);
        }

        if (Time.time - smallShip.loadTime > 10)
        {
            Vector3 relativePosition = smallShip.gameObject.transform.position - hitPosition;
            float forward = -Vector3.Dot(smallShip.gameObject.transform.position, relativePosition.normalized);

            if (smallShip.hullLevel > 0)
            {
                if (forward > 0)
                {
                    //This calculates the damage
                    if (smallShip.frontShieldLevel > 0)
                    {
                        if (smallShip.shieldType == "blackhole" & isRapidFire == false) //This minimises the damage on ships with black hole shields
                        {
                            damage = (damage / 100f) * 10;
                        }

                        smallShip.frontShieldLevel = smallShip.frontShieldLevel - damage;
                        smallShip.shieldLevel = smallShip.shieldLevel - damage;
                    }
                    else
                    {
                        if (isRapidFire == true) //This minimises the damage when the ship is using rapid fire
                        {
                            damage = (damage / 100f) * 2;
                        }

                        if (smallShip.hullLevel - damage < 5 & smallShip.invincible == true)
                        {
                            smallShip.hullLevel = 5;
                        }
                        else
                        {
                            smallShip.hullLevel = smallShip.hullLevel - damage;
                        }
                    }
                }
                else
                {
                    //This calculates the damage
                    if (smallShip.rearShieldLevel > 0)
                    {
                        if (smallShip.shieldType == "blackhole" & isRapidFire == false) //This minimises the damage on ships with black hole shields
                        {
                            damage = (damage / 100f) * 10;
                        }

                        smallShip.rearShieldLevel = smallShip.rearShieldLevel - damage;
                        smallShip.shieldLevel = smallShip.shieldLevel - damage;
                    }
                    else
                    {
                        if (isRapidFire == true) //This minimises the damage when the ship is using rapid fire
                        {
                            damage = (damage / 100f) * 2;
                        }

                        if (smallShip.hullLevel - damage < 5 & smallShip.invincible == true)
                        {
                            smallShip.hullLevel = 5;
                        }
                        else
                        {
                            smallShip.hullLevel = smallShip.hullLevel - damage;
                        }
                    }
                }

                if (smallShip.frontShieldLevel < 0) { smallShip.frontShieldLevel = 0; }
                if (smallShip.rearShieldLevel < 0) { smallShip.rearShieldLevel = 0; }
                if (smallShip.shieldLevel < 0) { smallShip.shieldLevel = 0; }

                //This shakes the cockpit camera
                Task a = new Task(CockpitFunctions.CockpitDamageShake(smallShip, 1, 0.011f));

                if (smallShip.isAI == false)
                {
                    Task b = new Task(SmallShipFunctions.ShakeControllerForSetTime(0.25f, 0.65f, 0.65f));
                }

                SmallShipFunctions.AddTaskToPool(smallShip, a);
            }
        }
    }

    //This causes the ship to take damage from lasers and torpedoes
    public static void TakeSystemDamage_SmallShip(SmallShip smallShip, float damage, Vector3 hitPosition, bool isRapidFire = false)
    {
        //This sets the time until the ship systems start restoring
        smallShip.restoreDelayTime = Time.time + 15;

        //This calculates the damage level for different difficultes
        if (smallShip.isAI == false)
        {
            damage = CalculateDamage_SmallShip(damage);
        }

        //This calculates the damage
        if (Time.time - smallShip.loadTime > 10)
        {
            Vector3 relativePosition = smallShip.gameObject.transform.position - hitPosition;
            float forward = -Vector3.Dot(smallShip.gameObject.transform.position, relativePosition.normalized);

            if (smallShip.systemsLevel > 0)
            {
                if (forward > 0)
                {
                    if (smallShip.frontShieldLevel > 0)
                    {
                        if (smallShip.hasPlasma == true & isRapidFire == false) //This minimises the damage on ships with black hole shields
                        {
                            damage = (damage / 100f) * 10;
                        }

                        smallShip.frontShieldLevel = smallShip.frontShieldLevel - damage;
                        smallShip.shieldLevel = smallShip.shieldLevel - damage;
                    }
                    else
                    {
                        if (smallShip.hasPlasma == false) //Vong ships are not harmed by ion cannons 
                        {
                            if (smallShip.systemsLevel - damage < 5 & smallShip.invincible == true)
                            {
                                smallShip.systemsLevel = 5;
                            }
                            else
                            {
                                smallShip.systemsLevel = smallShip.systemsLevel - damage;
                            }
                        }
                    }
                }
                else
                {
                    if (smallShip.rearShieldLevel > 0)
                    {
                        if (smallShip.hasPlasma == true & isRapidFire == false) //This minimises the damage on ships with black hole shields
                        {
                            damage = (damage / 100f) * 10;
                        }

                        smallShip.rearShieldLevel = smallShip.rearShieldLevel - damage;
                        smallShip.shieldLevel = smallShip.shieldLevel - damage;
                    }
                    else
                    {
                        if (smallShip.hasPlasma == false) //Vong ships are not harmed by ion cannons
                        {
                            if (smallShip.systemsLevel - damage < 5 & smallShip.cannotbedisabled == true)
                            {
                                smallShip.systemsLevel = 5;
                            }
                            else
                            {
                                smallShip.systemsLevel = smallShip.systemsLevel - damage;
                            }
                        }
                    }
                }

                if (smallShip.frontShieldLevel < 0) { smallShip.frontShieldLevel = 0; }
                if (smallShip.rearShieldLevel < 0) { smallShip.rearShieldLevel = 0; }
                if (smallShip.shieldLevel < 0) { smallShip.shieldLevel = 0; }

                //This shakes the cockpit camera
                Task a = new Task(CockpitFunctions.CockpitDamageShake(smallShip, 1, 0.011f));

                if (smallShip.isAI == false)
                {
                    Task b = new Task(SmallShipFunctions.ShakeControllerForSetTime(0.25f, 0.65f, 0.65f));
                }

                SmallShipFunctions.AddTaskToPool(smallShip, a);
            }

            //This disables a ship that has less than 1 percent systems power
            if (smallShip.isDisabled == false & smallShip.systemsLevel <= 0)
            {
                //Stops listing the ship as targetting another ship
                if (smallShip.target != null)
                {
                    if (smallShip.target.gameObject.activeSelf == true)
                    {
                        if (smallShip.targetSmallShip != null)
                        {
                            smallShip.targetSmallShip.numberTargeting -= 1;
                        }
                    }

                    smallShip.target = null;
                }

                //This tells the player that the ship has been destroyed
                HudFunctions.AddToShipLog(smallShip.name.ToUpper() + " was disabled");
                smallShip.isDisabled = true;

                //This causes the ship to spin a little rather than being completely stationary
                smallShip.shipRigidbody.angularVelocity = Random.onUnitSphere * Random.Range(0.1f, 1f);
                smallShip.shipRigidbody.angularDamping = 0; //Prevents the spin from stopping
                smallShip.shipRigidbody.linearVelocity = Random.onUnitSphere * Random.Range(0.1f, 1f);
                smallShip.shipRigidbody.linearDamping = 0; //Prevents the movements from stopping

                //This creates an explosion where the ship is
                float explosionsScale = smallShip.shipLength / 10;

                ParticleFunctions.InstantiateExplosion(smallShip.scene.gameObject, smallShip.gameObject.transform.position, "explosion_ion_smallship", explosionsScale);

                //This makes an explosion sound
                AudioFunctions.PlayAudioClip(smallShip.audioManager, "impact01_laserhitshield", "External", smallShip.gameObject.transform.position, 1, 1, 1000, 1);

                if (smallShip.isAI == false)
                {
                    Task a = new Task(SmallShipFunctions.ShakeControllerForSetTime(0.5f, 0.90f, 0.90f));
                }


            }
        }
    }

    //This restores a ships systems to the desired level
    public static void RestoreShipsSystems_SmallShip(SmallShip smallShip)
    {
        if (smallShip.isAI == false)
        {
            if (Time.time > smallShip.restoreDelayTime)
            {
                if (smallShip.systemsLevel < 0)
                {
                    smallShip.systemsLevel = 0;
                }
                else if (smallShip.systemsLevel < 100)
                {
                    smallShip.systemsLevel += 1;
                }
            }
        }

        if (smallShip.isDisabled == true & smallShip.systemsLevel > 0)
        {
            smallShip.shipRigidbody.linearVelocity = new Vector3(0f, 0f, 0f);
            smallShip.shipRigidbody.angularVelocity = new Vector3(0f, 0f, 0f);
            smallShip.shipRigidbody.linearDamping = 9;
            smallShip.shipRigidbody.angularDamping = 7.5f;

            smallShip.isDisabled = false;

            HudFunctions.AddToShipLog(smallShip.name.ToUpper() + " has restored systems");
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
    public static void StartCollision_SmallShip(SmallShip smallShip, GameObject collidingWith)
    {
        if (smallShip != null & collidingWith != null)
        {
            //Debug.Log(smallShip.name + " colliding with " + collidingWith.name);
            //Debug.LogError(smallShip.name + " colliding with " + collidingWith.name + " sub object of " + collidingWith.transform.parent.parent.name);

            if (smallShip.docking == false)
            {
                smallShip.isCurrentlyColliding = true;

                if (smallShip.isAI == false & smallShip.invincible == false)
                {
                    AudioFunctions.PlayAudioClip(smallShip.audioManager, "impact03_crash", "Cockpit", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);

                    if (smallShip.isAI == false)
                    {
                        Task a = new Task(SmallShipFunctions.ShakeControllerForSetTime(0.5f, 0.90f, 0.90f));
                    }
                }
            }
        }
    }

    //This tells the damage system that a collision has ended
    public static void EndCollision_SmallShip(SmallShip smallShip)
    {
        smallShip.isCurrentlyColliding = false;
    }

    //This called when the ship collides with something causing it to take collision damage
    public static void TakeCollisionDamage_SmallShip(SmallShip smallShip)
    {
        if (smallShip.isCurrentlyColliding == true & smallShip.invincible == false & smallShip.docking == false)
        {
            if (Time.time - smallShip.loadTime > 10)
            {

                if (smallShip.hullLevel > 0 & smallShip.invincible == false)
                {

                    if (smallShip.invincible == true & smallShip.hullLevel - 5 < 5)
                    {
                        smallShip.hullLevel = 5;
                    }
                    else
                    {
                        smallShip.hullLevel -= 5;
                    }

                    if (smallShip.hullLevel < 0)
                    {
                        smallShip.hullLevel = 0;
                    }

                    Task a = new Task(CockpitFunctions.ActivateCockpitShake(smallShip, 0.5f));
                    
                    SmallShipFunctions.AddTaskToPool(smallShip, a);
                }
            }
        }
    }

    //This causes a smoke trail to appear behind the damaged ship
    public static void SmokeTrail_SmallShip(SmallShip smallShip)
    {
        if (smallShip.hullLevel < 10 & smallShip.smokeTrail == null & smallShip.scene != null & smallShip.isAI == true)
        {
            Object tempSmokeTrail = PoolUtils.FindPrefabObjectInPool(smallShip.scene.particlePrefabPool, "SmokeTrail");

            if (tempSmokeTrail != null)
            {
                GameObject smokeTrail = GameObject.Instantiate(tempSmokeTrail) as GameObject;
                smallShip.smokeTrail = smokeTrail;
                smokeTrail.transform.SetParent(smallShip.transform);
                smokeTrail.transform.localPosition = new Vector3(1, 1, 1);
                smokeTrail.layer = smallShip.gameObject.layer;
                smokeTrail.SetActive(true);
            }

            //This sets the smoke trails simulation space to the scene
            if (smallShip.smokeTrail != null)
            {
                ParticleSystem particleSystem = smallShip.smokeTrail.GetComponent<ParticleSystem>();

                if (particleSystem != null)
                {
                    Scene scene = SceneFunctions.GetScene();

                    var main = particleSystem.main;

                    main.customSimulationSpace = scene.transform;

                    particleSystem.transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }
        else if (smallShip.hullLevel < 10 & smallShip.smokeTrail != null)
        {
            smallShip.smokeTrail.SetActive(true);
        }
        else if (smallShip.hullLevel > 10 & smallShip.smokeTrail != null)
        {
            smallShip.smokeTrail.SetActive(false);
        }
    }

    //This causes the ship to explode
    public static void Explode_SmallShip(SmallShip smallShip)
    {
        if (smallShip.hullLevel <= 0 & smallShip.exploded == false || smallShip.hullLevel <= 0 & smallShip.exploded == false & smallShip.isCurrentlyColliding == true)
        {
            int explosion = Random.Range(0, 3);

            if (explosion == 0 & smallShip.isAI == true)
            {
                Task a = new Task(ExplosionType_Spin_SmallShip(smallShip));
                SmallShipFunctions.AddTaskToPool(smallShip, a);
                smallShip.exploded = true;
            }
            else
            {
                ExplosionType_Immediate_SmallShip(smallShip);
                smallShip.exploded = true;
            }
        }
    }

    //Explode after spinning
    public static IEnumerator ExplosionType_Spin_SmallShip(SmallShip smallShip)
    {
        if (smallShip.isCurrentlyColliding == false)
        {
            smallShip.spinShip = true;
            float time = Random.Range(2, 6);
            yield return new WaitForSeconds(time);
        }

        if (smallShip != null)
        {
            if (smallShip.scene == null)
            {
                smallShip.scene = SceneFunctions.GetScene();
            }

            //This creates an explosion where the ship is
            float explosionsScale = smallShip.shipLength / 5;

            ParticleFunctions.InstantiateExplosion(smallShip.scene.gameObject, smallShip.gameObject.transform.position, "explosion_smallship", explosionsScale);

            //This makes an explosion sound
            AudioFunctions.PlayAudioClip(smallShip.audioManager, "mid_explosion_01", "External", smallShip.gameObject.transform.position, 1, 1, 1000, 1);

            //This tells the player that the ship has been destroyed
            HudFunctions.AddToShipLog(smallShip.name.ToUpper() + " was destroyed");

            //This deactivates the ship
            DeactivateShip_SmallShip(smallShip);
        }
    }

    //Explode straight away
    public static void ExplosionType_Immediate_SmallShip(SmallShip smallShip)
    {
        if (smallShip.scene == null)
        {
            smallShip.scene = SceneFunctions.GetScene();
        }

        //This creates an explosion where the ship is
        float explosionsScale = smallShip.shipLength / 5;

        ParticleFunctions.InstantiateExplosion(smallShip.scene.gameObject, smallShip.gameObject.transform.position, "explosion_smallship", explosionsScale);

        //This makes an explosion sound
        AudioFunctions.PlayAudioClip(smallShip.audioManager, "mid_explosion_01", "External", smallShip.gameObject.transform.position, 1, 1, 1000, 1);

        //This tells the game that the ship has been destroyed
        HudFunctions.AddToShipLog(smallShip.name.ToUpper() + " was destroyed");

        //This deactivates the ship
        DeactivateShip_SmallShip(smallShip);
    }

    public static IEnumerator ShipSpinSequence_SmallShip(SmallShip smallShip, float time)
    {
        smallShip.spinShip = true;

        if (smallShip.isCurrentlyColliding == false)
        {
            yield return new WaitForSeconds(time);
        }

        smallShip.spinShip = false;
    }

    public static void DeactivateShip_SmallShip(SmallShip smallShip)
    {
        //Stops listing the ship as targetting another ship
        if (smallShip.target != null)
        {
            if (smallShip.target.gameObject.activeSelf == true)
            {
                if (smallShip.targetSmallShip != null)
                {
                    smallShip.targetSmallShip.numberTargeting -= 1;
                }
            }

            smallShip.target = null;
        }

        //This stops all task being run by the ship
        SmallShipFunctions.EndAllTasks(smallShip);

        //This removes the main camera
        CockpitFunctions.RemoveMainCamera(smallShip);

        //This turns of the engine sound and release the ship audio source from the ship
        if (smallShip.audioManager != null)
        {
            if (smallShip.engineAudioSource != null)
            {
                smallShip.engineAudioSource.Stop();
                smallShip.engineAudioSource = null;
            }

            smallShip.audioManager = null;
        }

        //This deactives the cockpit
        if (smallShip.cockpit != null)
        {
            smallShip.cockpit.SetActive(false);
        }

        //This disconnects the follow camera
        if (smallShip.followCamera != null)
        {
            smallShip.followCamera.transform.SetParent(null);
        }

        //This resets the ship for the next load if needed
        smallShip.exploded = false;

        //This unloads the mission
        if (smallShip.isAI == false)
        {
            MissionFunctions.ExitOnPlayerDestroy();
        }

        //This deactivates the ship
        GameObject.Destroy(smallShip.gameObject);
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

                            ParticleSystem explosion = ParticleFunctions.InstantiateExplosion(largeShip.scene.gameObject, worldPoint, "explosion_largeship", explosionsScale, largeShip.audioManager, "proton_explosion1", 1500, "Explosions");

                            explosions.Add(explosion);

                            float waitTime = Random.Range(0.10f, 0.45f);

                            yield return new WaitForSeconds(0.25f);
                        }
                    }
                }
            }

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

            if (largeShip != null)
            {
                if (largeShip.scene != null)
                {
                    float explosionsScale2 = largeShip.shipLength / 50;

                    ParticleFunctions.InstantiateExplosion(largeShip.scene.gameObject, largeShip.gameObject.transform.position, "explosion_largeship", explosionsScale2, largeShip.audioManager, "proton_explosion2", 3000, "Explosions");

                    yield return new WaitForSeconds(0.25f);

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
                    float explosionsScale = largeShip.shipLength / 50;

                    ParticleFunctions.InstantiateExplosion(largeShip.scene.gameObject, largeShip.gameObject.transform.position, "explosion_largeship", explosionsScale, largeShip.audioManager, "proton_explosion2", 3000, "Explosions");

                    yield return new WaitForSeconds(0.25f);

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
        LargeShipFunctions.EndAllTasks(largeShip);

        //This sets the ship up for the next time it is loaded from the pool
        largeShip.spinShip = false;
        largeShip.explode = false;

        //largeShip.gameObject.SetActive(false);

        GameObject.Destroy(largeShip.gameObject);
    }

    //This creates a debris exxplosions
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
            float explosionScale = GetExplosionScale(shipSystem.gameObject);
            
            if (shipSystem.ionParticleSystemGO != null)
            {
                GameObject.Destroy(shipSystem.ionParticleSystemGO);
            }

            ParticleFunctions.InstantiatePersistantExplosion(shipSystem.gameObject, shipSystem.transform.position, "explosion_system", explosionScale);
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
            shipSystem.disabled = true;
            float explosionScale = GetExplosionScale(shipSystem.gameObject);
            ParticleSystem ionParticleSystem = ParticleFunctions.InstantiatePersistantExplosion(shipSystem.gameObject, shipSystem.transform.position, "explosion_system_ion", explosionScale);
            shipSystem.ionParticleSystemGO = ionParticleSystem.gameObject;
            HudFunctions.AddToShipLog(shipSystem.name.ToUpper() + " was disabled.");
        }
    }

    public static float GetExplosionScale(GameObject systemGO)
    {
        float scale = 1;

        Renderer targetRenderer = systemGO.GetComponent<Renderer>();
       
        if (targetRenderer != null )
        {
            Vector3 targetSize = targetRenderer.bounds.size;

            scale = targetSize.x;
        }

        return scale;
    }

    #endregion
}
