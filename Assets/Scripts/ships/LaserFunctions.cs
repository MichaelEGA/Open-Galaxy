using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//These functions are called by the small ship functions script
public static class LaserFunctions
{
    #region intiate particle system

    //This sets all the correct settings on the provided particle system to fire lasers
    public static void LoadLaserParticleSystem(SmallShip smallShip)
    {
        //This loads the necessary prefabs
        GameObject laser = Resources.Load(OGGetAddress.particles + "models/laser") as GameObject;

        Mesh laserMesh = laser.GetComponent<MeshFilter>().sharedMesh;

        Material redLaserMaterial = Resources.Load(OGGetAddress.particles + "materials/laser_material_red") as Material;
        Material greenLaserMaterial = Resources.Load(OGGetAddress.particles + "materials/laser_material_green") as Material;
        Material yellowLaserMaterial = Resources.Load(OGGetAddress.particles + "materials/laser_material_yellow") as Material;

        GameObject redLaserLight = Resources.Load(OGGetAddress.particles + "lights/laser_light_red") as GameObject;
        GameObject greenLaserLight = Resources.Load(OGGetAddress.particles + "lights/laser_light_green") as GameObject;
        GameObject yellowLaserLight = Resources.Load(OGGetAddress.particles + "lights/laser_light_yellow") as GameObject;

        //This loads the particle system and the particle collider
        smallShip.laserParticleSystem = new GameObject();
        smallShip.laserParticleSystem.name = "laserparticlesystem_" + smallShip.gameObject.name;       
        ParticleSystem particleSystem = smallShip.laserParticleSystem.AddComponent<ParticleSystem>();
        ParticleSystemRenderer particleSystemRenderer = smallShip.laserParticleSystem.GetComponent<ParticleSystemRenderer>();
        OnLaserHit particleCollision = smallShip.laserParticleSystem.AddComponent<OnLaserHit>();
        particleCollision.relatedGameObject = smallShip.gameObject;
        particleCollision.type = "laser";

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
        collision.collidesWith = SetLaserCollisionLayers(smallShip);

        //This makes the particle looks like a laser
        particleSystemRenderer.renderMode = ParticleSystemRenderMode.Mesh;
        particleSystemRenderer.alignment = ParticleSystemRenderSpace.Velocity;
        particleSystemRenderer.mesh = laserMesh;

        if (smallShip.laserColor == "red")
        {
            particleSystemRenderer.material = redLaserMaterial;
            lights.light = redLaserLight.GetComponent<Light>();
        }
        else if (smallShip.laserColor == "green")
        {
            particleSystemRenderer.material = greenLaserMaterial;
            lights.light = greenLaserLight.GetComponent<Light>();
        }
        else
        {
            particleSystemRenderer.material = yellowLaserMaterial;
            lights.light = yellowLaserLight.GetComponent<Light>();
        }

        //This prevents the particle system playing when loaded
        particleSystem.Stop();   
    }

    //This sets the collision layer for the lasers
    public static LayerMask SetLaserCollisionLayers(SmallShip smallShip)
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
        Transform laser1 = smallShip.gameObject.transform.Find("gunbank01/gunbank01-01");
        Transform laser2 = smallShip.gameObject.transform.Find("gunbank01/gunbank01-02");
        Transform laser3 = smallShip.gameObject.transform.Find("gunbank01/gunbank01-03");
        Transform laser4 = smallShip.gameObject.transform.Find("gunbank01/gunbank01-04");

        if (laser1 != null)
        {
            smallShip.laserCannon1 = laser1.gameObject;
        }

        if (laser2 != null)
        {
            smallShip.laserCannon2 = laser2.gameObject;
        }

        if (laser3 != null)
        {
            smallShip.laserCannon3 = laser3.gameObject;
        }
        else
        {
            laser3 = smallShip.gameObject.transform.Find("gunbank02/gunbank02-01");

            if (laser3 != null)
            {
                smallShip.laserCannon3 = laser3.gameObject;
            }
        }

        if (laser4 != null)
        {
            smallShip.laserCannon4 = laser4.gameObject;
        }
        else
        {
            laser4 = smallShip.gameObject.transform.Find("gunbank02/gunbank02-02");

            if (laser4 != null)
            {
                smallShip.laserCannon4 = laser4.gameObject;
            }
        }

    }

    //This sets the rotation of the lasers to angle at the correct distance for the targetted ship
    public static void SetCannons(SmallShip smallShip)
    {      

        if (smallShip.autoaim == true & smallShip.target != null & smallShip.targetRigidbody != null & smallShip.targetForward > 0.99f)
        {

            Vector3 interceptPoint = GameObjectUtils.CalculateInterceptPoint(smallShip.transform.position, smallShip.target.transform.position, smallShip.targetRigidbody.linearVelocity, 750);

            if (smallShip.laserCannon1 != null)
            {
                smallShip.laserCannon1.transform.LookAt(interceptPoint);
            }

            if (smallShip.laserCannon2 != null)
            {
                smallShip.laserCannon2.transform.LookAt(interceptPoint);
            }

            if (smallShip.laserCannon3 != null)
            {
                smallShip.laserCannon3.transform.LookAt(interceptPoint);
            }

            if (smallShip.laserCannon4 != null)
            {
                smallShip.laserCannon4.transform.LookAt(interceptPoint);
            }
        }
        else
        {
            if (smallShip.laserCannon1 != null)
            {
                smallShip.laserCannon1.transform.LookAt(smallShip.cameraPosition.transform.position + (smallShip.cameraPosition.transform.forward * smallShip.interceptDistance));
            }

            if (smallShip.laserCannon2 != null)
            {
                smallShip.laserCannon2.transform.LookAt(smallShip.cameraPosition.transform.position + (smallShip.cameraPosition.transform.forward * smallShip.interceptDistance));
            }

            if (smallShip.laserCannon3 != null)
            {
                smallShip.laserCannon3.transform.LookAt(smallShip.cameraPosition.transform.position + (smallShip.cameraPosition.transform.forward * smallShip.interceptDistance));
            }

            if (smallShip.laserCannon4 != null)
            {
                smallShip.laserCannon4.transform.LookAt(smallShip.cameraPosition.transform.position + (smallShip.cameraPosition.transform.forward * smallShip.interceptDistance));
            }
        }
    }

    #endregion

    #region laser mode

    //This cycles the laser mode between single, dual, and quad lasers
    public static void ToggleWeaponMode(SmallShip smallShip)
    {
        if (smallShip.toggleWeaponNumber == true & Time.time > smallShip.laserModePressedTime & smallShip.activeWeapon == "lasers")
        {
            if (smallShip.weaponMode == "single" & smallShip.laserCannon2 != null)
            {
                smallShip.weaponMode = "dual";
            }
            else if (smallShip.weaponMode == "dual" & smallShip.laserCannon3 != null)
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
            float laserWaitTime = 0.1f + (1 - (smallShip.laserFireRating / 100f)) * 0.250f;

            if (smallShip.weaponMode == "dual")
            {
                laserWaitTime = laserWaitTime * 2;
            }
            else if (smallShip.weaponMode == "all")
            {
                laserWaitTime = laserWaitTime * 4;
            }

            if (Time.time > smallShip.laserPressedTime & smallShip.laserfiring != true & smallShip.activeWeapon == "lasers" & smallShip.weaponsLock == false)
            {
                if (smallShip.weaponMode == "single")
                {
                    if (smallShip.laserCannon3 != null & smallShip.laserCannon4 != null)
                    {
                        smallShip.laserCycleNumber = smallShip.laserCycleNumber + 1;

                        if (smallShip.laserCycleNumber > 4)
                        {
                            smallShip.laserCycleNumber = 1;
                        }

                        if (smallShip.laserCycleNumber == 1) { Task a = new Task(FireLasers(smallShip, 1, smallShip.laserCannon1)); }
                        else if (smallShip.laserCycleNumber == 2) { Task a = new Task(FireLasers(smallShip, 1, smallShip.laserCannon2)); }
                        else if (smallShip.laserCycleNumber == 3) { Task a = new Task(FireLasers(smallShip, 1, smallShip.laserCannon3)); }
                        else if (smallShip.laserCycleNumber == 4) { Task a = new Task(FireLasers(smallShip, 1, smallShip.laserCannon4)); }

                    }
                    else if (smallShip.laserCannon1 != null & smallShip.laserCannon2 != null & smallShip.laserCannon3 != null)
                    {
                        smallShip.laserCycleNumber = smallShip.laserCycleNumber + 1;

                        if (smallShip.laserCycleNumber > 3)
                        {
                            smallShip.laserCycleNumber = 1;
                        }

                        if (smallShip.laserCycleNumber == 1) { Task a = new Task(FireLasers(smallShip, 1, smallShip.laserCannon1)); }
                        else if (smallShip.laserCycleNumber == 2) { Task a = new Task(FireLasers(smallShip, 1, smallShip.laserCannon2)); }
                        else if (smallShip.laserCycleNumber == 3) { Task a = new Task(FireLasers(smallShip, 1, smallShip.laserCannon3)); }
                    }
                    else if (smallShip.laserCannon1 != null & smallShip.laserCannon2 != null)
                    {
                        smallShip.laserCycleNumber = smallShip.laserCycleNumber + 1;

                        if (smallShip.laserCycleNumber > 2)
                        {
                            smallShip.laserCycleNumber = 1;
                        }

                        if (smallShip.laserCycleNumber == 1) { Task a = new Task(FireLasers(smallShip, 1, smallShip.laserCannon1)); }
                        else if (smallShip.laserCycleNumber == 2) { Task a = new Task(FireLasers(smallShip, 1, smallShip.laserCannon2)); }
                    }
                    else if (smallShip.laserCannon1 != null)
                    {
                        Task a = new Task(FireLasers(smallShip, 1, smallShip.laserCannon1));
                    }

                }
                else if (smallShip.weaponMode == "dual")
                {

                    smallShip.laserCycleNumber = smallShip.laserCycleNumber + 1;

                    if (smallShip.laserCycleNumber > 2)
                    {
                        smallShip.laserCycleNumber = 1;
                    }

                    if (smallShip.laserCycleNumber == 1 & smallShip.laserCannon1 != null & smallShip.laserCannon2 != null)
                    {
                        Task a = new Task(FireLasers(smallShip, 2, smallShip.laserCannon1, smallShip.laserCannon2));
                    }
                    else if (smallShip.laserCycleNumber == 2 & smallShip.laserCannon2 != null & smallShip.laserCannon3 != null & smallShip.laserCannon4 == null)
                    {
                        Task a = new Task(FireLasers(smallShip, 2, smallShip.laserCannon2, smallShip.laserCannon3));
                    }
                    else if (smallShip.laserCycleNumber == 2 & smallShip.laserCannon3 != null & smallShip.laserCannon4 != null)
                    {
                        Task a = new Task(FireLasers(smallShip, 2, smallShip.laserCannon3, smallShip.laserCannon4));
                    }
                }
                else if (smallShip.weaponMode == "all")
                {
                    if (smallShip.laserCannon1 != null & smallShip.laserCannon2 != null & smallShip.laserCannon3 != null & smallShip.laserCannon4 == null)
                    {
                        Task a = new Task(FireLasers(smallShip, 3, smallShip.laserCannon1, smallShip.laserCannon2, smallShip.laserCannon3));
                    }
                    else if (smallShip.laserCannon1 != null & smallShip.laserCannon2 != null & smallShip.laserCannon3 != null & smallShip.laserCannon4 != null)
                    {
                        Task a = new Task(FireLasers(smallShip, 4, smallShip.laserCannon1, smallShip.laserCannon2, smallShip.laserCannon3, smallShip.laserCannon4));
                    }
                }
               
                smallShip.laserPressedTime = Time.time + laserWaitTime;
            }
        }
    }

    //This fires a laser from the selected cannon
    public static IEnumerator FireLasers(SmallShip smallShip, float lasersToFire, GameObject firstCannon, GameObject secondCannon = null, GameObject thirdCannon = null, GameObject fourthCannon = null)
    {
        smallShip.laserfiring = true;

        if (smallShip != null)
        {
            if (smallShip.ionParticleSystem != null)
            {
                ParticleSystem particleSystem = smallShip.laserParticleSystem.GetComponent<ParticleSystem>();

                float spatialBlend = 1f;
                string mixer = "External";

                if (smallShip.isAI == false)
                {
                    spatialBlend = 0;
                    mixer = "Cockpit";
                }

                string audioFile = smallShip.laserAudio;

                if (lasersToFire == 1 || lasersToFire == 2 || lasersToFire == 3 || lasersToFire == 4)
                {
                    if (particleSystem != null & firstCannon != null & smallShip != null)
                    {
                        particleSystem.transform.position = firstCannon.transform.position;
                        particleSystem.transform.rotation = firstCannon.transform.rotation;
                        particleSystem.Play();
                        AudioFunctions.PlayAudioClip(smallShip.audioManager, audioFile, mixer, firstCannon.transform.position, spatialBlend, 1, 500, 0.6f);
                        yield return null;
                    }  
                }

                if (lasersToFire == 2 || lasersToFire == 3 || lasersToFire == 4)
                {
                    if (particleSystem != null & secondCannon != null & smallShip != null)
                    {
                        yield return null;
                        particleSystem.transform.position = secondCannon.transform.position;
                        particleSystem.transform.rotation = secondCannon.transform.rotation;
                        particleSystem.Play();
                        AudioFunctions.PlayAudioClip(smallShip.audioManager, audioFile, mixer, secondCannon.transform.position, spatialBlend, 1, 500, 0.6f);
                    }
                }

                if (lasersToFire == 3 || lasersToFire == 4)
                {
                    if (particleSystem != null & thirdCannon != null)
                    {
                        yield return null;
                        particleSystem.transform.position = thirdCannon.transform.position;
                        particleSystem.transform.rotation = thirdCannon.transform.rotation;
                        particleSystem.Play();
                        AudioFunctions.PlayAudioClip(smallShip.audioManager, audioFile, mixer, thirdCannon.transform.position, spatialBlend, 1, 500, 0.6f);
                    }
                }

                if (lasersToFire == 4)
                {
                    if (particleSystem != null & fourthCannon != null & smallShip != null)
                    {
                        yield return null;
                        particleSystem.transform.position = fourthCannon.transform.position;
                        particleSystem.transform.rotation = fourthCannon.transform.rotation;
                        particleSystem.Play();
                        AudioFunctions.PlayAudioClip(smallShip.audioManager, audioFile, mixer, fourthCannon.transform.position, spatialBlend, 1, 500, 0.6f);
                    }
                }

                smallShip.laserfiring = false;
            }
        } 
    }

    #endregion

}
