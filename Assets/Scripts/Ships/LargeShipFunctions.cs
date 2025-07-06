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
    public static void RestoreShipsSystems(LargeShip largeShip)
    {
        if (largeShip.isDisabled == true & largeShip.systemsLevel > 0)
        {
            HudFunctions.AddToShipLog(largeShip.name.ToUpper() + " has restored systems");
            largeShip.isDisabled = false;
        }
    }

    //This selects and runs the appropriate explosion type
    public static void Explode(LargeShip largeShip)
    {
        if (largeShip.hullLevel <= 0 & largeShip.explode == false)
        {
            Task a = new Task(Explosion(largeShip));
            AddTaskToPool(largeShip, a);
            largeShip.explode = true;
        }      
    }

    //Multiple small explosions before ship blows up
    public static IEnumerator Explosion(LargeShip largeShip)
    {
        largeShip.spinShip = true;

        Component rendererComp;

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
                            //Vector3 scenePoint = largeShip.scene.transform.InverseTransformPoint(worldPoint);

                            ParticleFunctions.InstantiateExplosion(largeShip.scene.gameObject, worldPoint, "explosion_largeship", explosionsScale, largeShip.audioManager, "proton_explosion1", 1500, "Explosions");

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
                    float explosionsScale = largeShip.shipLength / 50;

                    ParticleFunctions.InstantiateExplosion(largeShip.scene.gameObject, largeShip.gameObject.transform.position, "explosion_largeship", explosionsScale, largeShip.audioManager, "proton_explosion2", 3000, "Explosions");

                    yield return new WaitForSeconds(0.25f);

                    int scaleNumber = (int)Mathf.Abs(largeShip.shipLength / 100f);

                    TriggerDebrisExplosion(largeShip.gameObject.transform.position, 50 * scaleNumber);

                    HudFunctions.AddToShipLog(largeShip.name.ToUpper() + " was destroyed");

                    DeactivateShip(largeShip);
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

                    TriggerDebrisExplosion(largeShip.gameObject.transform.position, 50 * scaleNumber);

                    HudFunctions.AddToShipLog(largeShip.name.ToUpper() + " was destroyed");

                    DeactivateShip(largeShip);
                }
            }
        }
    }

    //This deactivates the ship so that it no longer appears in the scene
    public static void DeactivateShip(LargeShip largeShip)
    {
        EndAllTasks(largeShip);

        //This sets the ship up for the next time it is loaded from the pool
        largeShip.spinShip = false;
        largeShip.explode = false;

        largeShip.gameObject.SetActive(false);
    }

    //This creates a debris exxplosions
    public static void TriggerDebrisExplosion(Vector3 position, int debrisCount = 10)
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
