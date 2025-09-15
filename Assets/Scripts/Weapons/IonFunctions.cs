using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//These functions are called by the small ship functions script
public static class IonFunctions
{
    #region intiate particle system

    //This sets all the correct settings on the provided particle system to fire ions
    public static void LoadIonParticleSystem(SmallShip smallShip)
    {
        //This loads the necessary prefabs
        GameObject ion = Resources.Load(OGGetAddress.particles + "models/ion") as GameObject; //This needs to be created then changed

        Mesh ionMesh = ion.GetComponent<MeshFilter>().sharedMesh;

        Material ionMaterial = Resources.Load(OGGetAddress.particles + "materials/ion_material") as Material;  //This needs to be created then changed
        GameObject ionLight = Resources.Load(OGGetAddress.particles + "lights/ion_light") as GameObject;  //This needs to be created then changed

        //This loads the particle system and the particle collider
        smallShip.ionParticleSystem = new GameObject();
        smallShip.ionParticleSystem.name = "ionparticlesystem_" + smallShip.gameObject.name;
        ParticleSystem particleSystem = smallShip.ionParticleSystem.AddComponent<ParticleSystem>();
        ParticleSystemRenderer particleSystemRenderer = smallShip.ionParticleSystem.GetComponent<ParticleSystemRenderer>();
        OnIonHit onIonHit = smallShip.ionParticleSystem.AddComponent<OnIonHit>();
        onIonHit.particleSystemScript = particleSystem;
        onIonHit.smallShip = smallShip;

        //This adds the new particle system to the pool
        if (smallShip.scene != null)
        {
            if (smallShip.scene.ionPool == null)
            {
                smallShip.scene.ionPool = new List<GameObject>();
            }

            smallShip.scene.ionPool.Add(smallShip.ionParticleSystem);
        }

        //This sets the paticle to operate in world space (as opposed to local)
        var main = particleSystem.main;
        main.simulationSpace = ParticleSystemSimulationSpace.Custom;
        main.customSimulationSpace = smallShip.scene.transform;
        main.startLifetime = 15;
        main.startSize3D = true;
        main.startSizeX = 0.25f;
        main.startSizeY = 0.25f;
        main.startSizeZ = 5;
        main.startSpeed = 750;
        main.loop = false;
        main.playOnAwake = false;

        //This causes the particle emmiter to only emit one particle per play
        var emission = particleSystem.emission;
        emission.enabled = true;
        emission.rateOverTime = 0;
        emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0f, 1), });

        //This makes the particle emitter fire in one direction
        var shape = particleSystem.shape;
        shape.enabled = true;
        shape.scale = new Vector3(0, 0, 0);
        shape.shapeType = ParticleSystemShapeType.Rectangle;

        //This gets the particle moving in the right direction
        var velocity = particleSystem.inheritVelocity;
        velocity.enabled = true;
        velocity.mode = ParticleSystemInheritVelocityMode.Initial;
        velocity.curveMultiplier = 0.0001f;

        //This gets up the light system
        var lights = particleSystem.lights;
        lights.enabled = true;

        //This enables the particle to collide
        var collision = particleSystem.collision;
        collision.enabled = true;
        collision.type = ParticleSystemCollisionType.World;
        collision.bounce = 0;
        collision.lifetimeLoss = 1;
        collision.sendCollisionMessages = true;
        collision.collidesWith = SetIonCollisionLayers(smallShip);

        //This makes the particle looks like a ion
        particleSystemRenderer.renderMode = ParticleSystemRenderMode.Mesh;
        particleSystemRenderer.alignment = ParticleSystemRenderSpace.Velocity;
        particleSystemRenderer.mesh = ionMesh;

        particleSystemRenderer.material = ionMaterial;
        lights.light = ionLight.GetComponent<Light>();

        //This prevents the particle system playing when loaded
        particleSystem.Stop();
    }

    //This sets all the correct settings on the provided particle system to make a muzzle flash
    public static void LoadIonMuzzleFlashParticleSystem(SmallShip smallShip)
    {
        //This loads the necessary prefabs
        GameObject blueMuzzleFlashLight = Resources.Load(OGGetAddress.particles + "lights/ion_light") as GameObject;

        Material blueMuzzleFlashMaterial = Resources.Load(OGGetAddress.particles + "materials/muzzleflash_blue") as Material;

        //This loads the particle system and the particle collider
        smallShip.ionMuzzleFlashParticleSystem = new GameObject();
        smallShip.ionMuzzleFlashParticleSystem.name = "ionmuzzleflashparticlesystem_" + smallShip.gameObject.name;
        ParticleSystem particleSystem = smallShip.ionMuzzleFlashParticleSystem.AddComponent<ParticleSystem>();
        ParticleSystemRenderer particleSystemRenderer = smallShip.ionMuzzleFlashParticleSystem.GetComponent<ParticleSystemRenderer>();

        //This sets the particle system to be subordinate to the smallship
        particleSystem.transform.SetParent(smallShip.transform);

        //This adds the new particle system to the pool
        if (smallShip.scene != null)
        {
            if (smallShip.scene.lasersPool == null)
            {
                smallShip.scene.lasersPool = new List<GameObject>();
            }

            smallShip.scene.lasersPool.Add(smallShip.laserParticleSystem);
        }

        //This sets the paticle to operate in scene space (as opposed to local and world)
        var main = particleSystem.main;
        main.loop = false;
        main.playOnAwake = false;
        main.startSpeed = new ParticleSystem.MinMaxCurve(0, 0);
        main.simulationSpace = ParticleSystemSimulationSpace.Local;
        main.customSimulationSpace = smallShip.transform;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.1f, 0.2f);
        main.startSize = 3;

        //This causes the particle emmiter to only emit one particle per play
        var emission = particleSystem.emission;
        emission.enabled = true;
        emission.rateOverTime = 0;
        emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0f, 1), });

        //This makes the particle emitter fire in one direction
        var shape = particleSystem.shape;
        shape.enabled = true;
        shape.scale = new Vector3(1, 1, 1);
        shape.radius = 0.0001f;
        shape.radiusThickness = 0.001f;
        shape.shapeType = ParticleSystemShapeType.Sphere;

        //This makes the particle emitter fire in one direction
        var textureSheetAnimation = particleSystem.textureSheetAnimation;
        textureSheetAnimation.enabled = true;
        textureSheetAnimation.numTilesX = 4;
        textureSheetAnimation.numTilesY = 1;

        //Values for the renderer
        particleSystemRenderer.sortingFudge = 10;
        particleSystemRenderer.minParticleSize = 0;
        particleSystemRenderer.maxParticleSize = 5;
        particleSystemRenderer.sortMode = ParticleSystemSortMode.Distance;
        particleSystemRenderer.flip = new Vector3(0.5f, 0.5f, 0.5f);
        particleSystemRenderer.sortingOrder = 2;

        //This gets up the light system
        var lights = particleSystem.lights;
        lights.enabled = true;

        particleSystemRenderer.material = blueMuzzleFlashMaterial;
        lights.light = blueMuzzleFlashLight.GetComponent<Light>();
        
        //This prevents the particle system playing when loaded
        particleSystem.Stop();
    }

    //This sets the collision layer for the ions
    public static LayerMask SetIonCollisionLayers(SmallShip smallShip)
    {
        LayerMask collisionLayers = new LayerMask();

        if (smallShip.isAI != false)
        {

            //This gets the Json ship data
            TextAsset allegiancesFile = Resources.Load(OGGetAddress.files + "Allegiances") as TextAsset;
            Allegiances allegiances = JsonUtility.FromJson<Allegiances>(allegiancesFile.text);

            Allegiance allegiance = null;

            List<string> layerNames = new List<string>();

            foreach (Allegiance tempAllegiance in allegiances.allegianceData)
            {
                if (tempAllegiance.allegiance == smallShip.allegiance)
                {
                    allegiance = tempAllegiance;
                }

                layerNames.Add(tempAllegiance.allegiance); //This makes a list of collision layers and their corresponding integer

            }

            collisionLayers = LayerMask.GetMask("collision_player", "collision_asteroid", "collision01", "collision02", "collision03", "collision04", "collision05", "collision06", "collision07", "collision08", "collision09", "collision10");

            collisionLayers &= ~(1 << GetLayerInt(allegiance.allegiance, layerNames));

        }
        else
        {
            collisionLayers = LayerMask.GetMask("collision_asteroid", "collision01", "collision02", "collision03", "collision04", "collision05", "collision06", "collision07", "collision08", "collision09", "collision10");
        }

        return collisionLayers;

    }

    //This gets the int for the ion collision layers
    public static int GetLayerInt(string layer, List<string> layerNames)
    {
        int layerNumber = 0;
        int i = 0;

        foreach (string tempLayer in layerNames)
        {

            if (tempLayer == layer)
            {
                layerNumber = i + 8; //The first nine layers are already allocated, so they are skipped
                break;
            }

            i++;
        }

        return layerNumber;
    }

    #endregion

    #region ship cannons

    //This grabs all the ships ion cannons
    public static void GetCannons(SmallShip smallShip)
    {
        Transform ion1 = smallShip.gameObject.transform.Find("ionbank01/ionbank01-01");
        Transform ion2 = smallShip.gameObject.transform.Find("ionbank01/ionbank01-02");
        Transform ion3 = smallShip.gameObject.transform.Find("ionbank01/ionbank01-03");
        Transform ion4 = smallShip.gameObject.transform.Find("ionbank01/ionbank01-04");

        if (ion1 != null)
        {
            smallShip.ionCannon1 = ion1.gameObject;
        }

        if (ion2 != null)
        {
            smallShip.ionCannon2 = ion2.gameObject;
        }

        if (ion3 != null)
        {
            smallShip.ionCannon3 = ion3.gameObject;
        }
        else
        {
            ion3 = smallShip.gameObject.transform.Find("ionbank02/ionbank02-01");

            if (ion3 != null)
            {
                smallShip.ionCannon3 = ion3.gameObject;
            }
        }

        if (ion4 != null)
        {
            smallShip.ionCannon4 = ion4.gameObject;
        }
        else
        {
            ion4 = smallShip.gameObject.transform.Find("ionbank02/ionbank02-02");

            if (ion4 != null)
            {
                smallShip.ionCannon4 = ion4.gameObject;
            }
        }

        if (ion1 != null || ion2 != null || ion3 != null || ion4 != null)
        {
            smallShip.hasIon = true;
        }
    }

    //This sets the rotation of the ions to angle at the correct distance for the targetted ship
    public static void SetCannons(SmallShip smallShip)
    {
        if (smallShip.autoaim == true & smallShip.target != null & smallShip.targetRigidbody != null & smallShip.targetForward > 0.99f)
        {
            Vector3 interceptPoint = GameObjectUtils.CalculateInterceptPoint(smallShip.transform.position, smallShip.target.transform.position, smallShip.targetRigidbody.linearVelocity, 750);

            if (smallShip.ionCannon1 != null)
            {
                smallShip.ionCannon1.transform.LookAt(interceptPoint);
            }

            if (smallShip.ionCannon2 != null)
            {
                smallShip.ionCannon2.transform.LookAt(interceptPoint);
            }

            if (smallShip.ionCannon3 != null)
            {
                smallShip.ionCannon3.transform.LookAt(interceptPoint);
            }

            if (smallShip.ionCannon4 != null)
            {
                smallShip.ionCannon4.transform.LookAt(interceptPoint);
            }
        }
        else
        {
            if (smallShip.ionCannon1 != null)
            {
                smallShip.ionCannon1.transform.LookAt(smallShip.gameObject.transform.position + (smallShip.gameObject.transform.forward * smallShip.interceptDistance));
            }

            if (smallShip.ionCannon2 != null)
            {
                smallShip.ionCannon2.transform.LookAt(smallShip.gameObject.transform.position + (smallShip.gameObject.transform.forward * smallShip.interceptDistance));
            }

            if (smallShip.ionCannon3 != null)
            {
                smallShip.ionCannon3.transform.LookAt(smallShip.gameObject.transform.position + (smallShip.gameObject.transform.forward * smallShip.interceptDistance));
            }

            if (smallShip.ionCannon4 != null)
            {
                smallShip.ionCannon4.transform.LookAt(smallShip.gameObject.transform.position + (smallShip.gameObject.transform.forward * smallShip.interceptDistance));
            }
        }
    }

    #endregion

    #region ion mode

    //This cycles the ion mode between single, dual, and quad ions
    public static void ToggleWeaponMode(SmallShip smallShip)
    {
        if (smallShip.toggleWeaponNumber == true & Time.time > smallShip.ionModePressedTime & smallShip.activeWeapon == "ion")
        {
            if (smallShip.weaponMode == "single" & smallShip.ionCannon2 != null)
            {
                smallShip.weaponMode = "dual";
            }
            else if (smallShip.weaponMode == "dual" & smallShip.ionCannon3 != null)
            {
                smallShip.weaponMode = "all";
            }
            else if (smallShip.weaponMode == "dual" & smallShip.hasRadidFire == true || smallShip.weaponMode == "all" & smallShip.hasRadidFire == true)
            {
                smallShip.weaponMode = "rapid";
            }
            else
            {
                smallShip.weaponMode = "single";
            }

            smallShip.ionModePressedTime = Time.time + 0.2f;

            AudioFunctions.PlayAudioClip(smallShip.audioManager, "beep01_toggle", "Cockpit", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);

        }
    }

    #endregion

    #region ion fire functions

    //This allows the player to fire the ions
    public static void InitiateFiringPlayer(SmallShip smallShip)
    {
        if (smallShip.fireWeapon == true & smallShip.isAI == false & smallShip.isDisabled == false)
        {
            InitiateFiring(smallShip);
        }
    }

    //This executes the firing according to the ion mode
    public static void InitiateFiring(SmallShip smallShip)
    {
        SetCannons(smallShip); //This sets cannon angle prior to firing the laser

        if (smallShip.isDisabled == false)
        {
            //This calculates the delay before the next ion fires
            float ionWaitTime = 0.1f + (1 - (smallShip.laserFireRating / 100f)) * 0.250f;

            if (smallShip.weaponMode == "dual")
            {
                ionWaitTime = ionWaitTime * 2;
            }
            else if (smallShip.weaponMode == "all")
            {
                ionWaitTime = ionWaitTime * 4;
            }
            else if (smallShip.weaponMode == "rapid")
            {
                ionWaitTime = ionWaitTime * 0.25f;
            }

            if (Time.time > smallShip.ionPressedTime & smallShip.ionfiring != true & smallShip.activeWeapon == "ion" & smallShip.weaponsLock == false)
            {
                if (smallShip.weaponMode == "single" || smallShip.weaponMode == "rapid")
                {
                    if (smallShip.ionCannon3 != null & smallShip.ionCannon4 != null)
                    {
                        smallShip.ionCycleNumber = smallShip.ionCycleNumber + 1;

                        if (smallShip.ionCycleNumber > 4)
                        {
                            smallShip.ionCycleNumber = 1;
                        }

                        if (smallShip.ionCycleNumber == 1) { Task a = new Task(FireIons(smallShip, 1, smallShip.ionCannon1)); }
                        else if (smallShip.ionCycleNumber == 2) { Task a = new Task(FireIons(smallShip, 1, smallShip.ionCannon2)); }
                        else if (smallShip.ionCycleNumber == 3) { Task a = new Task(FireIons(smallShip, 1, smallShip.ionCannon3)); }
                        else if (smallShip.ionCycleNumber == 4) { Task a = new Task(FireIons(smallShip, 1, smallShip.ionCannon4)); }

                    }
                    else if (smallShip.ionCannon1 != null & smallShip.ionCannon2 != null & smallShip.ionCannon3 != null)
                    {
                        smallShip.ionCycleNumber = smallShip.ionCycleNumber + 1;

                        if (smallShip.ionCycleNumber > 3)
                        {
                            smallShip.ionCycleNumber = 1;
                        }

                        if (smallShip.ionCycleNumber == 1) { Task a = new Task(FireIons(smallShip, 1, smallShip.ionCannon1)); }
                        else if (smallShip.ionCycleNumber == 2) { Task a = new Task(FireIons(smallShip, 1, smallShip.ionCannon2)); }
                        else if (smallShip.ionCycleNumber == 3) { Task a = new Task(FireIons(smallShip, 1, smallShip.ionCannon3)); }
                    }
                    else if (smallShip.ionCannon1 != null & smallShip.ionCannon2 != null)
                    {
                        smallShip.ionCycleNumber = smallShip.ionCycleNumber + 1;

                        if (smallShip.ionCycleNumber > 2)
                        {
                            smallShip.ionCycleNumber = 1;
                        }

                        if (smallShip.ionCycleNumber == 1) { Task a = new Task(FireIons(smallShip, 1, smallShip.ionCannon1)); }
                        else if (smallShip.ionCycleNumber == 2) { Task a = new Task(FireIons(smallShip, 1, smallShip.ionCannon2)); }
                    }
                    else if (smallShip.ionCannon1 != null)
                    {
                        Task a = new Task(FireIons(smallShip, 1, smallShip.ionCannon1));
                    }

                }
                else if (smallShip.weaponMode == "dual")
                {

                    smallShip.ionCycleNumber = smallShip.ionCycleNumber + 1;

                    if (smallShip.ionCycleNumber > 2)
                    {
                        smallShip.ionCycleNumber = 1;
                    }

                    if (smallShip.ionCycleNumber == 1 & smallShip.ionCannon1 != null & smallShip.ionCannon2 != null)
                    {
                        Task a = new Task(FireIons(smallShip, 2, smallShip.ionCannon1, smallShip.ionCannon2));
                    }
                    else if (smallShip.ionCycleNumber == 2 & smallShip.ionCannon2 != null & smallShip.ionCannon3 != null & smallShip.ionCannon4 == null)
                    {
                        Task a = new Task(FireIons(smallShip, 2, smallShip.ionCannon2, smallShip.ionCannon3));
                    }
                    else if (smallShip.ionCycleNumber == 2 & smallShip.ionCannon3 != null & smallShip.ionCannon4 != null)
                    {
                        Task a = new Task(FireIons(smallShip, 2, smallShip.ionCannon3, smallShip.ionCannon4));
                    }
                }
                else if (smallShip.weaponMode == "all")
                {
                    if (smallShip.ionCannon1 != null & smallShip.ionCannon2 != null & smallShip.ionCannon3 != null & smallShip.ionCannon4 == null)
                    {
                        Task a = new Task(FireIons(smallShip, 3, smallShip.ionCannon1, smallShip.ionCannon2, smallShip.ionCannon3));
                    }
                    else if (smallShip.ionCannon1 != null & smallShip.ionCannon2 != null & smallShip.ionCannon3 != null & smallShip.ionCannon4 != null)
                    {
                        Task a = new Task(FireIons(smallShip, 4, smallShip.ionCannon1, smallShip.ionCannon2, smallShip.ionCannon3, smallShip.ionCannon4));
                    } 
                }

                smallShip.ionPressedTime = Time.time + ionWaitTime;
            }
        }
    }

    //This fires a ion from the selected cannon
    public static IEnumerator FireIons(SmallShip smallShip, float ionCannonsToFire, GameObject firstCannon, GameObject secondCannon = null, GameObject thirdCannon = null, GameObject fourthCannon = null)
    {
        smallShip.ionfiring = true;

        if (smallShip != null)
        {
            if (smallShip.ionParticleSystem != null)
            {
                ParticleSystem particleSystem = smallShip.ionParticleSystem.GetComponent<ParticleSystem>();
                ParticleSystem particleSystemMuzzleFlash = smallShip.ionMuzzleFlashParticleSystem.GetComponent<ParticleSystem>();

                float spatialBlend = 1f;
                string mixer = "External";

                if (smallShip.isAI == false)
                {
                    spatialBlend = 0;
                    mixer = "Cockpit";
                }

                string audioFile = smallShip.ionAudio;

                if (ionCannonsToFire == 1 || ionCannonsToFire == 2 || ionCannonsToFire == 3 || ionCannonsToFire == 4)
                {
                    if (particleSystem != null & firstCannon != null & smallShip != null)
                    {
                        particleSystemMuzzleFlash.transform.position = firstCannon.transform.position;
                        particleSystemMuzzleFlash.transform.rotation = firstCannon.transform.rotation;
                        particleSystemMuzzleFlash.Play();

                        particleSystem.transform.position = firstCannon.transform.position;
                        particleSystem.transform.rotation = firstCannon.transform.rotation;
                        particleSystem.Play();
                        AudioFunctions.PlayAudioClip(smallShip.audioManager, audioFile, mixer, firstCannon.transform.position, spatialBlend, 1, 500, 0.6f);

                        if (smallShip.isAI == false)
                        {
                            Task a = new Task(SmallShipFunctions.ShakeControllerForSetTime(0.05f, 0.40f, 0.40f));
                        }

                        yield return null;
                    }
                }

                if (ionCannonsToFire == 2 || ionCannonsToFire == 3 || ionCannonsToFire == 4)
                {
                    if (particleSystem != null & secondCannon != null & smallShip != null)
                    {
                        yield return null;
                        particleSystemMuzzleFlash.transform.position = secondCannon.transform.position;
                        particleSystemMuzzleFlash.transform.rotation = secondCannon.transform.rotation;
                        particleSystemMuzzleFlash.Play();

                        particleSystem.transform.position = secondCannon.transform.position;
                        particleSystem.transform.rotation = secondCannon.transform.rotation;
                        particleSystem.Play();
                        AudioFunctions.PlayAudioClip(smallShip.audioManager, audioFile, mixer, secondCannon.transform.position, spatialBlend, 1, 500, 0.6f);

                        if (smallShip.isAI == false)
                        {
                            Task a = new Task(SmallShipFunctions.ShakeControllerForSetTime(0.05f, 0.40f, 0.40f));
                        }

                    }
                }

                if (ionCannonsToFire == 3 || ionCannonsToFire == 4)
                {
                    if (particleSystem != null & thirdCannon != null & smallShip != null)
                    {
                        yield return null;
                        particleSystemMuzzleFlash.transform.position = thirdCannon.transform.position;
                        particleSystemMuzzleFlash.transform.rotation = thirdCannon.transform.rotation;
                        particleSystemMuzzleFlash.Play();

                        particleSystem.transform.position = thirdCannon.transform.position;
                        particleSystem.transform.rotation = thirdCannon.transform.rotation;
                        particleSystem.Play();
                        AudioFunctions.PlayAudioClip(smallShip.audioManager, audioFile, mixer, thirdCannon.transform.position, spatialBlend, 1, 500, 0.6f);

                        if (smallShip.isAI == false)
                        {
                            Task a = new Task(SmallShipFunctions.ShakeControllerForSetTime(0.05f, 0.40f, 0.40f));
                        }
                    }
                }

                if (ionCannonsToFire == 4)
                {
                    if (particleSystem != null & fourthCannon != null & smallShip != null)
                    {
                        yield return null;
                        particleSystemMuzzleFlash.transform.position = fourthCannon.transform.position;
                        particleSystemMuzzleFlash.transform.rotation = fourthCannon.transform.rotation;
                        particleSystemMuzzleFlash.Play();

                        particleSystem.transform.position = fourthCannon.transform.position;
                        particleSystem.transform.rotation = fourthCannon.transform.rotation;
                        particleSystem.Play();
                        AudioFunctions.PlayAudioClip(smallShip.audioManager, audioFile, mixer, fourthCannon.transform.position, spatialBlend, 1, 500, 0.6f);

                        if (smallShip.isAI == false)
                        {
                            Task a = new Task(SmallShipFunctions.ShakeControllerForSetTime(0.05f, 0.40f, 0.40f));
                        }
                    }
                }

                smallShip.ionfiring = false;
            }
        }
    }

    #endregion

    #region collision functions

    //This handles an event where the laser hits something
    public static void RunCollisionEvent(GameObject objectHit, List<ParticleCollisionEvent> collisionEvents, ParticleSystem particleSystemScript, SmallShip smallShip)
    {
        //Get collision information
        List<Vector3> hitPositions = new List<Vector3>();

        int events = particleSystemScript.GetCollisionEvents(objectHit, collisionEvents); //This grabs all the collision events

        for (int i = 0; i < events; i++) //This cycles through all the collision events and deals with one at a time
        {
            Vector3 hitPosition = collisionEvents[i].intersection; //This gets the position of the collision event

            GameObject objectHitParent = ReturnParent(objectHit); //This gets the colliders object parent  

            if (smallShip != null & objectHitParent != null)
            {
                if (objectHitParent != smallShip.gameObject)
                {
                    //This gets key information on the object hit
                    var objectHitDetails = LaserFunctions.ObjectHitDetails(objectHit, hitPosition);

                    float shieldFront = objectHitDetails.shieldFront;
                    float shieldBack = objectHitDetails.shieldBack;
                    float forward = objectHitDetails.forward;
                    bool hasPlasma = objectHitDetails.hasPlasma;

                    Audio audioManager = GameObject.FindFirstObjectByType<Audio>();

                    //This instantiates an explosion at the hit position
                    LaserFunctions.InstantiateLaserExplosion(smallShip.gameObject, objectHit, hitPosition, forward, shieldFront, shieldBack, smallShip.laserColor, hasPlasma, audioManager);

                    //This applies damage to the target
                    ApplyDamage(smallShip, objectHit, hitPosition);
                }
            }
        }
    }

    //This function returns the root parent of the prefab by looking for the component that will only be attached to the parent gameobject
    public static GameObject ReturnParent(GameObject objectHit)
    {
        GameObject parent = null;

        SmallShip smallShip = objectHit.gameObject.GetComponentInParent<SmallShip>();
        LargeShip largeShip = objectHit.gameObject.GetComponentInParent<LargeShip>();

        if (smallShip != null)
        {
            parent = smallShip.gameObject;
        }
        else if (largeShip != null)
        {
            parent = largeShip.gameObject;
        }

        return parent;
    }

    //This gets key information from the object that has been hit by the laser
    public static (float shieldFront, float shieldBack, float forward, bool hasPlasma) ObjectHitDetails(GameObject objectHit, Vector3 hitPosition)
    {
        float shieldFront = 0;
        float shieldBack = 0;
        float forward = 0;
        bool hasPlasma = false;

        SmallShip smallShip = objectHit.gameObject.GetComponentInParent<SmallShip>(); //This gets the smallship function if avaiblible
        LargeShip largeShip = objectHit.gameObject.GetComponentInParent<LargeShip>();

        if (smallShip != null)
        {
            hasPlasma = smallShip.hasPlasma;

            shieldFront = smallShip.frontShieldLevel;
            shieldBack = smallShip.rearShieldLevel;

            Vector3 relativePosition = smallShip.gameObject.transform.position - hitPosition;
            forward = -Vector3.Dot(smallShip.gameObject.transform.position, relativePosition.normalized);
        }

        if (largeShip != null)
        {
            hasPlasma = largeShip.hasPlasma;

            shieldFront = largeShip.frontShieldLevel;
            shieldBack = largeShip.rearShieldLevel;

            Vector3 relativePosition = largeShip.gameObject.transform.position - hitPosition;
            forward = -Vector3.Dot(largeShip.gameObject.transform.position, relativePosition.normalized);
        }

        return (shieldFront, shieldBack, forward, hasPlasma);
    }

    //This instantiates the correct explosion at the hit position
    public static void InstantiateLaserExplosion(GameObject turretGO, GameObject objectHit, Vector3 hitPosition, float forward, float shieldFront, float shieldBack, string laserColor, bool hasPlasma, Audio audioManager)
    {
        //This selects the correct explosion colour
        string explosionChoice = "laserblast_red";

        if (forward > 0 & shieldFront > 0 || forward < 0 & shieldBack > 0)
        {
            if (hasPlasma == true)
            {
                explosionChoice = "blackhole";
            }
            else if (laserColor == "red")
            {
                explosionChoice = "laserblast_red";
            }
            else if (laserColor == "green")
            {
                explosionChoice = "laserblast_green";
            }
        }
        else
        {
            explosionChoice = "hullstrike";
        }

        //This instantiates an explosion at the point of impact
        ParticleFunctions.InstantiateExplosion(objectHit, hitPosition, explosionChoice, 6, audioManager);

    }

    //This calculates and applies damage to the 
    public static void ApplyDamage(SmallShip playerShip, GameObject objectHit, Vector3 hitPosition)
    {
        SmallShip smallShip = objectHit.gameObject.GetComponentInParent<SmallShip>();
        LargeShip largeShip = objectHit.gameObject.GetComponentInParent<LargeShip>();

        float damage = CalculateIonDamage(playerShip);

        if (smallShip != null)
        {
            DamageFunctions.TakeSystemDamage_SmallShip(smallShip, damage, hitPosition, false);
        }

        if (largeShip != null)
        {
            DamageFunctions.TakeSystemDamage_LargeShip(largeShip, damage, hitPosition);
        }
    }

    //This calculates the laser damage
    public static float CalculateIonDamage(SmallShip smallShip)
    {
        float damage = 0;
        float ionPower = 0;
        float ionRating = 0;
        float ionDamage = 0;

        ionPower = smallShip.laserPower;
        ionRating = smallShip.laserRating;
        ionDamage = 50;

        if (ionPower > 50)
        {
            damage = (ionDamage / 100F) * ionRating;
        }
        else if (ionPower == 50f)
        {
            damage = (ionDamage / 100F) * ionRating;
        }
        else if (ionPower < 50f)
        {
            damage = (ionDamage / 100F) * ionRating;
        }

        return damage;
    }

    #endregion

}
