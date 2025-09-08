using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//These functions are called by the small ship functions script
public static class PlasmaFunctions
{
    #region intiate particle system

    //This sets all the correct settings on the provided particle system to fire lasers
    public static void LoadPlasmaParticleSystem(SmallShip smallShip)
    {
        //This loads the necessary prefabs
        GameObject plasma = Resources.Load(OGGetAddress.particles + "models/plasma") as GameObject;

        Mesh plasmaMesh = plasma.GetComponent<MeshFilter>().sharedMesh;

        Material plasmaMaterial = Resources.Load(OGGetAddress.particles + "materials/plasma_material") as Material;

        GameObject plasmaLight = Resources.Load(OGGetAddress.particles + "lights/plasma_light") as GameObject;

        //This loads the particle system and the particle collider
        smallShip.plasmaParticleSystem = new GameObject();
        smallShip.plasmaParticleSystem.name = "plasmaparticlesystem_" + smallShip.gameObject.name;       
        ParticleSystem particleSystem = smallShip.plasmaParticleSystem.AddComponent<ParticleSystem>();
        ParticleSystemRenderer particleSystemRenderer = smallShip.plasmaParticleSystem.GetComponent<ParticleSystemRenderer>();
        OnLaserHit particleCollision = smallShip.plasmaParticleSystem.AddComponent<OnLaserHit>();
        particleCollision.relatedGameObject = smallShip.gameObject;
        particleCollision.type = "plasma";

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
        collision.collidesWith = SetPlasmaCollisionLayers(smallShip);

        //This makes the particle looks like a laser
        particleSystemRenderer.renderMode = ParticleSystemRenderMode.Mesh;
        particleSystemRenderer.alignment = ParticleSystemRenderSpace.Velocity;
        particleSystemRenderer.mesh = plasmaMesh;

        particleSystemRenderer.material = plasmaMaterial;
        lights.light = plasmaLight.GetComponent<Light>();
        
        //This prevents the particle system playing when loaded
        particleSystem.Stop();   
    }

    //This sets all the correct settings on the provided particle system to make a muzzle flash
    public static void LoadPlasmaMuzzleFlashParticleSystem(SmallShip smallShip)
    {
        //This loads the necessary prefabs
        GameObject orangeMuzzleFlashLight = Resources.Load(OGGetAddress.particles + "lights/plasma_light") as GameObject;

        Material orangeMuzzleFlashMaterial = Resources.Load(OGGetAddress.particles + "materials/muzzleflash_orange") as Material;

        //This loads the particle system and the particle collider
        smallShip.plasmaMuzzleFlashParticleSystem = new GameObject();
        smallShip.plasmaMuzzleFlashParticleSystem.name = "plasmaMuzzleflashparticlesystem_" + smallShip.gameObject.name;
        ParticleSystem particleSystem = smallShip.plasmaMuzzleFlashParticleSystem.AddComponent<ParticleSystem>();
        ParticleSystemRenderer particleSystemRenderer = smallShip.plasmaMuzzleFlashParticleSystem.GetComponent<ParticleSystemRenderer>();

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

        particleSystemRenderer.material = orangeMuzzleFlashMaterial;
        lights.light = orangeMuzzleFlashLight.GetComponent<Light>();

        //This prevents the particle system playing when loaded
        particleSystem.Stop();
    }

    //This sets the collision layer for the lasers
    public static LayerMask SetPlasmaCollisionLayers(SmallShip smallShip)
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

    //This gets the int for the laser collision layers
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

    //This grabs all the ships laser cannons
    public static void GetCannons(SmallShip smallShip)
    {
        Transform plasma1 = smallShip.gameObject.transform.Find("plasmabank01/plasmabank01-01");
        Transform plasma2 = smallShip.gameObject.transform.Find("plasmabank01/plasmabank01-02");
        Transform plasma3 = smallShip.gameObject.transform.Find("plasmabank01/plasmabank01-03");
        Transform plasma4 = smallShip.gameObject.transform.Find("plasmabank01/plasmabank01-04");

        if (plasma1 != null)
        {
            smallShip.plasmaCannon1 = plasma1.gameObject;
        }

        if (plasma2 != null)
        {
            smallShip.plasmaCannon2 = plasma2.gameObject;
        }

        if (plasma3 != null)
        {
            smallShip.plasmaCannon3 = plasma3.gameObject;
        }
        else
        {
            plasma3 = smallShip.gameObject.transform.Find("plasmabank02/plasmabank02-01");

            if (plasma3 != null)
            {
                smallShip.plasmaCannon3 = plasma3.gameObject;
            }
        }

        if (plasma4 != null)
        {
            smallShip.plasmaCannon4 = plasma4.gameObject;
        }
        else
        {
            plasma4 = smallShip.gameObject.transform.Find("plasmabank02/plasmabank02-02");

            if (plasma4 != null)
            {
                smallShip.plasmaCannon4 = plasma4.gameObject;
            }
        }

        if (plasma1 != null || plasma2 != null || plasma3 != null || plasma4 != null)
        {
            smallShip.hasPlasma = true;
            smallShip.activeWeapon = "plasma";
            SmallShipAIFunctions.AddTag(smallShip, "singleplasma");
        }

    }

    //This sets the rotation of the lasers to angle at the correct distance for the targetted ship
    public static void SetCannons(SmallShip smallShip)
    {      

        if (smallShip.autoaim == true & smallShip.target != null & smallShip.targetRigidbody != null & smallShip.targetForward > 0.99f)
        {

            Vector3 interceptPoint = GameObjectUtils.CalculateInterceptPoint(smallShip.transform.position, smallShip.target.transform.position, smallShip.targetRigidbody.linearVelocity, 750);

            if (smallShip.plasmaCannon1 != null)
            {
                smallShip.plasmaCannon1.transform.LookAt(interceptPoint);
            }

            if (smallShip.plasmaCannon2 != null)
            {
                smallShip.plasmaCannon2.transform.LookAt(interceptPoint);
            }

            if (smallShip.plasmaCannon3 != null)
            {
                smallShip.plasmaCannon3.transform.LookAt(interceptPoint);
            }

            if (smallShip.plasmaCannon4 != null)
            {
                smallShip.plasmaCannon4.transform.LookAt(interceptPoint);
            }
        }
        else
        {
            if (smallShip.plasmaCannon1 != null)
            {
                smallShip.plasmaCannon1.transform.LookAt(smallShip.cameraPosition.transform.position + (smallShip.cameraPosition.transform.forward * smallShip.interceptDistance));
            }

            if (smallShip.plasmaCannon2 != null)
            {
                smallShip.plasmaCannon2.transform.LookAt(smallShip.cameraPosition.transform.position + (smallShip.cameraPosition.transform.forward * smallShip.interceptDistance));
            }

            if (smallShip.plasmaCannon3 != null)
            {
                smallShip.plasmaCannon3.transform.LookAt(smallShip.cameraPosition.transform.position + (smallShip.cameraPosition.transform.forward * smallShip.interceptDistance));
            }

            if (smallShip.plasmaCannon4 != null)
            {
                smallShip.plasmaCannon4.transform.LookAt(smallShip.cameraPosition.transform.position + (smallShip.cameraPosition.transform.forward * smallShip.interceptDistance));
            }
        }
    }

    #endregion

    #region laser mode

    //This cycles the laser mode between single, dual, and quad lasers
    public static void ToggleWeaponMode(SmallShip smallShip)
    {
        if (smallShip.toggleWeaponNumber == true & Time.time > smallShip.plasmaModePressedTime & smallShip.activeWeapon == "plasma")
        {
            if (smallShip.weaponMode == "single" & smallShip.plasmaCannon2 != null)
            {
                smallShip.weaponMode = "dual";
            }
            else if (smallShip.weaponMode == "dual" & smallShip.plasmaCannon3 != null)
            {
                smallShip.weaponMode = "all";
            }
            else
            {
                smallShip.weaponMode = "single";
            }

            smallShip.laserModePressedTime = Time.time + 0.2f;

            AudioFunctions.PlayAudioClip(smallShip.audioManager, "beep01_toggle", "Cockpit", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);

        }
    }

    #endregion

    #region laser fire functions

    //This allows the player to fire the lasers
    public static void InitiateFiringPlayer(SmallShip smallShip)
    {
        if (smallShip.fireWeapon == true & smallShip.isAI == false)
        {
            InitiateFiring(smallShip);
        }
    }

    //This executes the firing according to the laser mode
    public static void InitiateFiring(SmallShip smallShip)
    {
        SetCannons(smallShip); //This sets cannon angle prior to firing the laser

        if (smallShip.isDisabled == false)
        {
            //This calculates the delay before the next laser fires
            float plasmaWaitTime = 0.1f + (1 - (smallShip.laserFireRating / 100f)) * 0.250f;

            if (smallShip.weaponMode == "dual")
            {
                plasmaWaitTime = plasmaWaitTime * 2;
            }
            else if (smallShip.weaponMode == "all")
            {
                plasmaWaitTime = plasmaWaitTime * 4;
            }
            else if (smallShip.weaponMode == "rapid")
            {
                plasmaWaitTime = plasmaWaitTime * 0.25f;
            }

            if (Time.time > smallShip.plasmaPressedTime & smallShip.laserfiring != true & smallShip.activeWeapon == "plasma" & smallShip.weaponsLock == false)
            {
                if (smallShip.weaponMode == "single" || smallShip.weaponMode == "rapid")
                {
                    if (smallShip.plasmaCannon3 != null & smallShip.plasmaCannon4 != null)
                    {
                        smallShip.plasmaCycleNumber = smallShip.plasmaCycleNumber + 1;

                        if (smallShip.plasmaCycleNumber > 4)
                        {
                            smallShip.plasmaCycleNumber = 1;
                        }

                        if (smallShip.plasmaCycleNumber == 1) { Task a = new Task(FirePlasma(smallShip, 1, smallShip.plasmaCannon1)); }
                        else if (smallShip.plasmaCycleNumber == 2) { Task a = new Task(FirePlasma(smallShip, 1, smallShip.plasmaCannon2)); }
                        else if (smallShip.plasmaCycleNumber == 3) { Task a = new Task(FirePlasma(smallShip, 1, smallShip.plasmaCannon3)); }
                        else if (smallShip.plasmaCycleNumber == 4) { Task a = new Task(FirePlasma(smallShip, 1, smallShip.plasmaCannon4)); }

                    }
                    else if (smallShip.plasmaCannon1 != null & smallShip.plasmaCannon2 != null & smallShip.plasmaCannon3 != null)
                    {
                        smallShip.plasmaCycleNumber = smallShip.plasmaCycleNumber + 1;

                        if (smallShip.plasmaCycleNumber > 3)
                        {
                            smallShip.plasmaCycleNumber = 1;
                        }

                        if (smallShip.plasmaCycleNumber == 1) { Task a = new Task(FirePlasma(smallShip, 1, smallShip.plasmaCannon1)); }
                        else if (smallShip.plasmaCycleNumber == 2) { Task a = new Task(FirePlasma(smallShip, 1, smallShip.plasmaCannon2)); }
                        else if (smallShip.plasmaCycleNumber == 3) { Task a = new Task(FirePlasma(smallShip, 1, smallShip.plasmaCannon3)); }
                    }
                    else if (smallShip.plasmaCannon1 != null & smallShip.plasmaCannon2 != null)
                    {
                        smallShip.plasmaCycleNumber = smallShip.plasmaCycleNumber + 1;

                        if (smallShip.plasmaCycleNumber > 2)
                        {
                            smallShip.plasmaCycleNumber = 1;
                        }

                        if (smallShip.plasmaCycleNumber == 1) { Task a = new Task(FirePlasma(smallShip, 1, smallShip.plasmaCannon1)); }
                        else if (smallShip.plasmaCycleNumber == 2) { Task a = new Task(FirePlasma(smallShip, 1, smallShip.plasmaCannon2)); }
                    }
                    else if (smallShip.plasmaCannon1 != null)
                    {
                        Task a = new Task(FirePlasma(smallShip, 1, smallShip.plasmaCannon1));
                    }

                }
                else if (smallShip.weaponMode == "dual")
                {

                    smallShip.plasmaCycleNumber = smallShip.plasmaCycleNumber + 1;

                    if (smallShip.plasmaCycleNumber > 2)
                    {
                        smallShip.plasmaCycleNumber = 1;
                    }

                    if (smallShip.plasmaCycleNumber == 1 & smallShip.plasmaCannon1 != null & smallShip.plasmaCannon2 != null)
                    {
                        Task a = new Task(FirePlasma(smallShip, 2, smallShip.plasmaCannon1, smallShip.plasmaCannon2));
                    }
                    else if (smallShip.plasmaCycleNumber == 2 & smallShip.plasmaCannon2 != null & smallShip.plasmaCannon3 != null & smallShip.plasmaCannon4 == null)
                    {
                        Task a = new Task(FirePlasma(smallShip, 2, smallShip.plasmaCannon2, smallShip.plasmaCannon3));
                    }
                    else if (smallShip.plasmaCycleNumber == 2 & smallShip.plasmaCannon3 != null & smallShip.plasmaCannon4 != null)
                    {
                        Task a = new Task(FirePlasma(smallShip, 2, smallShip.plasmaCannon3, smallShip.plasmaCannon4));
                    }
                }
                else if (smallShip.weaponMode == "all")
                {
                    if (smallShip.plasmaCannon1 != null & smallShip.plasmaCannon2 != null & smallShip.plasmaCannon3 != null & smallShip.plasmaCannon4 == null)
                    {
                        Task a = new Task(FirePlasma(smallShip, 3, smallShip.plasmaCannon1, smallShip.plasmaCannon2, smallShip.plasmaCannon3));
                    }
                    else if (smallShip.plasmaCannon1 != null & smallShip.plasmaCannon2 != null & smallShip.plasmaCannon3 != null & smallShip.plasmaCannon4 != null)
                    {
                        Task a = new Task(FirePlasma(smallShip, 4, smallShip.plasmaCannon1, smallShip.plasmaCannon2, smallShip.plasmaCannon3, smallShip.plasmaCannon4));
                    }
                }
               
                smallShip.plasmaPressedTime = Time.time + plasmaWaitTime;
            }
        }
    }

    //This fires a laser from the selected cannon
    public static IEnumerator FirePlasma(SmallShip smallShip, float plasmaCannonsToFire, GameObject firstCannon, GameObject secondCannon = null, GameObject thirdCannon = null, GameObject fourthCannon = null)
    {
        smallShip.plasmafiring = true;

        if (smallShip != null)
        {
            if (smallShip.plasmaParticleSystem != null)
            {
                ParticleSystem particleSystem = smallShip.plasmaParticleSystem.GetComponent<ParticleSystem>();
                ParticleSystem particleSystemMuzzleFlash = smallShip.plasmaMuzzleFlashParticleSystem.GetComponent<ParticleSystem>();

                float spatialBlend = 1f;
                string mixer = "External";

                if (smallShip.isAI == false)
                {
                    spatialBlend = 0;
                    mixer = "Cockpit";
                }

                string audioFile = smallShip.plasmaAudio;

                if (plasmaCannonsToFire == 1 || plasmaCannonsToFire == 2 || plasmaCannonsToFire == 3 || plasmaCannonsToFire == 4)
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

                if (plasmaCannonsToFire == 2 || plasmaCannonsToFire == 3 || plasmaCannonsToFire == 4)
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

                        if (smallShip.isAI == false)
                        {
                            Task a = new Task(SmallShipFunctions.ShakeControllerForSetTime(0.05f, 0.40f, 0.40f));
                        }

                        AudioFunctions.PlayAudioClip(smallShip.audioManager, audioFile, mixer, secondCannon.transform.position, spatialBlend, 1, 500, 0.6f);
                    }
                }

                if (plasmaCannonsToFire == 3 || plasmaCannonsToFire == 4)
                {
                    if (particleSystem != null & thirdCannon != null)
                    {
                        yield return null;
                        particleSystemMuzzleFlash.transform.position = thirdCannon.transform.position;
                        particleSystemMuzzleFlash.transform.rotation = thirdCannon.transform.rotation;
                        particleSystemMuzzleFlash.Play();

                        particleSystem.transform.position = thirdCannon.transform.position;
                        particleSystem.transform.rotation = thirdCannon.transform.rotation;
                        particleSystem.Play();

                        if (smallShip.isAI == false)
                        {
                            Task a = new Task(SmallShipFunctions.ShakeControllerForSetTime(0.05f, 0.40f, 0.40f));
                        }

                        AudioFunctions.PlayAudioClip(smallShip.audioManager, audioFile, mixer, thirdCannon.transform.position, spatialBlend, 1, 500, 0.6f);
                    }
                }

                if (plasmaCannonsToFire == 4)
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

                        if (smallShip.isAI == false)
                        {
                            Task a = new Task(SmallShipFunctions.ShakeControllerForSetTime(0.05f, 0.40f, 0.40f));
                        }

                        AudioFunctions.PlayAudioClip(smallShip.audioManager, audioFile, mixer, fourthCannon.transform.position, spatialBlend, 1, 500, 0.6f);
                    }
                }

                smallShip.plasmafiring = false;
            }
        } 
    }

    #endregion

}
