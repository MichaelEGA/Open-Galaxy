using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.Rendering.CameraUI;

//These functions are called by the small ship functions script
public static class EnergyWeaponFunctions
{
    #region intiate particle system

    public static void PrepareEnergyWeapon(EnergyWeapon laser)
    {
        if (laser != null)
        {
            laser.smallShip = laser.GetComponent<SmallShip>();
        }

        if (laser.energyWeaponParticleSystem == null)
        {
            SmallShip smallShip = laser.smallShip;

            EnergyWeaponFunctions.GetCannons(laser);
            EnergyWeaponFunctions.LoadEnergyWeaponParticleSystem(laser);
            EnergyWeaponFunctions.LoadEnergyWeaponMuzzleFlashParticleSystem(laser);
        }
    }

    //This sets all the correct settings on the provided particle system to fire lasers
    public static void LoadEnergyWeaponParticleSystem(EnergyWeapon laser)
    {
        SmallShip smallShip = laser.smallShip;

        //This loads the necessary prefabs
        GameObject laserGO = Resources.Load(OGGetAddress.particles + "models/laser") as GameObject;

        Mesh laserMesh = laserGO.GetComponent<MeshFilter>().sharedMesh;

        Material redLaserMaterial = Resources.Load(OGGetAddress.particles + "materials/laser_material_red") as Material;
        Material greenLaserMaterial = Resources.Load(OGGetAddress.particles + "materials/laser_material_green") as Material;
        Material yellowLaserMaterial = Resources.Load(OGGetAddress.particles + "materials/laser_material_yellow") as Material;

        GameObject redLaserLight = Resources.Load(OGGetAddress.particles + "lights/laser_light_red") as GameObject;
        GameObject greenLaserLight = Resources.Load(OGGetAddress.particles + "lights/laser_light_green") as GameObject;
        GameObject yellowLaserLight = Resources.Load(OGGetAddress.particles + "lights/laser_light_yellow") as GameObject;

        //This loads the particle system and the particle collider
        laser.energyWeaponParticleSystem = new GameObject();
        laser.energyWeaponParticleSystem.name = "laserparticlesystem_" + smallShip.gameObject.name;       
        ParticleSystem particleSystem = laser.energyWeaponParticleSystem.AddComponent<ParticleSystem>();
        ParticleSystemRenderer particleSystemRenderer = laser.energyWeaponParticleSystem.GetComponent<ParticleSystemRenderer>();
        laser.particleSystemScript = particleSystem;
        laser.smallShip = smallShip;

        //This creates an anchor for all the laser particle systems
        GameObject laserparticlesanchor = GameObject.Find("laserparticleanchor");

        if (laserparticlesanchor == null)
        {
            laserparticlesanchor = new GameObject("laserparticleanchor");

            laserparticlesanchor.transform.SetParent(smallShip.scene.transform);
        }

        particleSystem.transform.SetParent(laserparticlesanchor.transform);

        //This adds the new particle system to the pool
        if (smallShip.scene != null)
        {
            if (smallShip.scene.lasersPool == null)
            {
                smallShip.scene.lasersPool = new List<GameObject>();
            }

            smallShip.scene.lasersPool.Add(laser.energyWeaponParticleSystem);
        }

        //This sets the paticle to operate in scene space (as opposed to local and world)
        var main = particleSystem.main;
        main.simulationSpace = ParticleSystemSimulationSpace.Custom;
        main.customSimulationSpace = smallShip.scene.transform;
        main.startLifetime = 15;
        main.startSize3D = true;
        main.startSizeX = 0.25f;
        main.startSizeY = 0.25f;
        main.startSizeZ = 10; //5
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
        collision.collidesWith = SetEnergyWeaponCollisionLayers(smallShip);

        //This makes the particle looks like a laser
        particleSystemRenderer.renderMode = ParticleSystemRenderMode.Mesh;
        particleSystemRenderer.alignment = ParticleSystemRenderSpace.Velocity;
        particleSystemRenderer.mesh = laserMesh;

        if (smallShip.energyWeapon.laserColour == "red")
        {
            particleSystemRenderer.material = redLaserMaterial;
            lights.light = redLaserLight.GetComponent<Light>();
        }
        else if (smallShip.energyWeapon.laserColour == "green")
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

    //This sets all the correct settings on the provided particle system to make a muzzle flash
    public static void LoadEnergyWeaponMuzzleFlashParticleSystem(EnergyWeapon laser)
    {
        SmallShip smallShip = laser.smallShip;

        //This loads the necessary prefabs
        GameObject redMuzzleFlashLight = Resources.Load(OGGetAddress.particles + "lights/laser_light_red") as GameObject;
        GameObject greenMuzzleFlashLight = Resources.Load(OGGetAddress.particles + "lights/laser_light_green") as GameObject;
        GameObject yellowMuzzleFlashLight = Resources.Load(OGGetAddress.particles + "lights/laser_light_yellow") as GameObject;

        Material redMuzzleFlashMaterial = Resources.Load(OGGetAddress.particles + "materials/muzzleflash_red") as Material;
        Material greenMuzzleFlashMaterial = Resources.Load(OGGetAddress.particles + "materials/muzzleflash_green") as Material;
        Material yellowMuzzleFlashMaterial = Resources.Load(OGGetAddress.particles + "materials/muzzleflash_yellow") as Material;

        //This loads the particle system and the particle collider
        laser.energyWeaponMuzzleFlashParticleSystem = new GameObject();
        laser.energyWeaponMuzzleFlashParticleSystem.layer = smallShip.gameObject.layer;
        laser.energyWeaponMuzzleFlashParticleSystem.name = "muzzleflashparticlesystem_" + smallShip.gameObject.name;
        ParticleSystem particleSystem = laser.energyWeaponMuzzleFlashParticleSystem.AddComponent<ParticleSystem>();
        ParticleSystemRenderer particleSystemRenderer = laser.energyWeaponMuzzleFlashParticleSystem.GetComponent<ParticleSystemRenderer>();

        //This sets the particle system to be subordinate to the smallship
        particleSystem.transform.SetParent(smallShip.transform);
        laser.energyWeaponMuzzleFlashParticleSystem.transform.localScale = new Vector3(1, 1, 1);

        //This adds the new particle system to the pool
        if (smallShip.scene != null)
        {
            if (smallShip.scene.lasersPool == null)
            {
                smallShip.scene.lasersPool = new List<GameObject>();
            }

            smallShip.scene.lasersPool.Add(laser.energyWeaponParticleSystem);
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

        if (smallShip.energyWeapon.laserColour == "red")
        {
            particleSystemRenderer.material = redMuzzleFlashMaterial;
            lights.light = redMuzzleFlashLight.GetComponent<Light>();
        }
        else if (smallShip.energyWeapon.laserColour == "green")
        {
            particleSystemRenderer.material = greenMuzzleFlashMaterial;
            lights.light = greenMuzzleFlashLight.GetComponent<Light>();
        }
        else
        {
            particleSystemRenderer.material = yellowMuzzleFlashMaterial;
            lights.light = yellowMuzzleFlashLight.GetComponent<Light>();
        }

        //This prevents the particle system playing when loaded
        particleSystem.Stop();
    }

    //This modifies the collision layers to the player layer so that it's visible in film mode
    public static void ChangeCollisionLayerToPlayer(SmallShip smallShip)
    {
        EnergyWeapon laser = GetEnergyWeapon(smallShip);

        EnergyWeapon onLaserHit = laser.energyWeaponParticleSystem.GetComponent<EnergyWeapon>();

        if (onLaserHit != null)
        {
            ParticleSystem particleSystem = onLaserHit.particleSystemScript;

            if (particleSystem != null)
            {
                var collision = particleSystem.collision;
                collision.collidesWith = SetEnergyWeaponCollisionLayers(smallShip, true);
            }
        } 
    }

    //This resets the collision layers
    public static void ResetCollisionLayers(SmallShip smallShip)
    {
        EnergyWeapon laser = GetEnergyWeapon(smallShip);

        EnergyWeapon onLaserHit = laser.energyWeaponParticleSystem.GetComponent<EnergyWeapon>();

        if (onLaserHit != null)
        {
            ParticleSystem particleSystem = onLaserHit.particleSystemScript;

            if (particleSystem != null)
            {
                var collision = particleSystem.collision;
                collision.collidesWith = SetEnergyWeaponCollisionLayers(smallShip);
            }
        }
    }

    //This sets the collision layer for the lasers
    public static LayerMask SetEnergyWeaponCollisionLayers(SmallShip smallShip, bool markAsPlayer = false)
    {
        LayerMask collisionLayers = new LayerMask(); 

        if (smallShip.isAI == true & markAsPlayer == false)
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

            collisionLayers = LayerMask.GetMask("collision_player", "collision_asteroid", "collision01", "collision02", "collision03", "collision04", "collision05", "collision06", "collision07", "collision08", "collision09", "collision10", "collision11", "collision12", "collision13", "collision14", "collision15", "collision16", "invisible");

            collisionLayers &= ~(1 << ((GetLayerInt(allegiance.allegiance, layerNames))));
        }
        else
        {
            collisionLayers = LayerMask.GetMask("collision_asteroid", "collision01", "collision02", "collision03", "collision04", "collision05", "collision06", "collision07", "collision08", "collision09", "collision10", "collision11", "collision12", "collision13", "collision14", "collision15", "collision16", "invisible");
        }

        return collisionLayers;
    }

    //This gets the int for the laser collision layers
    public static int GetLayerInt(string layer, List<string> layerNames)
    {
        int layerNumber = 0;
        int i = 8; //The first seven layers are already allocated, so they are skipped

        foreach (string tempLayer in layerNames)
        {

            if (tempLayer == layer)
            {
                layerNumber = i; 
                break;
            }

            i++;
        }

        return layerNumber;
    }

    #endregion

    #region ship cannons

    //This grabs all the ships laser cannons
    public static void GetCannons(EnergyWeapon laser)
    {
        SmallShip smallShip = laser.smallShip;

        Transform laser1 = smallShip.gameObject.transform.Find("gunbank01/gunbank01-01");
        Transform laser2 = smallShip.gameObject.transform.Find("gunbank01/gunbank01-02");
        Transform laser3 = smallShip.gameObject.transform.Find("gunbank01/gunbank01-03");
        Transform laser4 = smallShip.gameObject.transform.Find("gunbank01/gunbank01-04");

        if (laser1 != null)
        {
            laser.laserCannon1 = laser1.gameObject;
        }

        if (laser2 != null)
        {
            laser.laserCannon2 = laser2.gameObject;
        }

        if (laser3 != null)
        {
            laser.laserCannon3 = laser3.gameObject;
        }
        else
        {
            laser3 = smallShip.gameObject.transform.Find("gunbank02/gunbank02-01");

            if (laser3 != null)
            {
                laser.laserCannon3 = laser3.gameObject;
            }
        }

        if (laser4 != null)
        {
            laser.laserCannon4 = laser4.gameObject;
        }
        else
        {
            laser4 = smallShip.gameObject.transform.Find("gunbank02/gunbank02-02");

            if (laser4 != null)
            {
                laser.laserCannon4 = laser4.gameObject;
            }
        }

    }

    //This sets the rotation of the lasers to angle at the correct distance for the targetted ship
    public static void SetCannons(SmallShip smallShip)
    {
        EnergyWeapon laser = GetEnergyWeapon(smallShip);

        if (smallShip.autoaim == true & smallShip.target != null & smallShip.targetRigidbody != null & smallShip.targetForward > 0.995f & smallShip.targetLargeShip == null)
        {

            Vector3 interceptPoint = GameObjectUtils.CalculateInterceptPoint(smallShip.transform.position, smallShip.target.transform.position, smallShip.targetRigidbody.linearVelocity, 750);

            if (laser.laserCannon1 != null)
            {
                laser.laserCannon1.transform.LookAt(interceptPoint);
            }

            if (laser.laserCannon2 != null)
            {
                laser.laserCannon2.transform.LookAt(interceptPoint);
            }

            if (laser.laserCannon3 != null)
            {
                laser.laserCannon3.transform.LookAt(interceptPoint);
            }

            if (laser.laserCannon4 != null)
            {
                laser.laserCannon4.transform.LookAt(interceptPoint);
            }
        }
        else
        {
            if (laser.laserCannon1 != null)
            {
                laser.laserCannon1.transform.LookAt(smallShip.cameraPosition.transform.position + (smallShip.cameraPosition.transform.forward * smallShip.interceptDistance));
            }

            if (laser.laserCannon2 != null)
            {
                laser.laserCannon2.transform.LookAt(smallShip.cameraPosition.transform.position + (smallShip.cameraPosition.transform.forward * smallShip.interceptDistance));
            }

            if (laser.laserCannon3 != null)
            {
                laser.laserCannon3.transform.LookAt(smallShip.cameraPosition.transform.position + (smallShip.cameraPosition.transform.forward * smallShip.interceptDistance));
            }

            if (laser.laserCannon4 != null)
            {
                laser.laserCannon4.transform.LookAt(smallShip.cameraPosition.transform.position + (smallShip.cameraPosition.transform.forward * smallShip.interceptDistance));
            }
        }
    }

    #endregion

    #region laser mode

    //This cycles the laser mode between single, dual, and quad lasers
    public static void ToggleWeaponMode(EnergyWeapon energyWeapon)
    {
        WeaponManagement weaponManagement = energyWeapon.weaponManagement;

        if (weaponManagement.toggleWeaponNumber == true & Time.time > energyWeapon.energyWeaponModePressedTime & weaponManagement.weaponType == "lasers")
        {
            if (weaponManagement.weaponMode == "single" & energyWeapon.laserCannon2 != null)
            {
                weaponManagement.weaponMode = "dual";
            }
            else if (weaponManagement.weaponMode == "dual" & energyWeapon.laserCannon3 != null)
            {
                weaponManagement.weaponMode = "all";
            }
            else
            {
                weaponManagement.weaponMode = "single";
            }

            energyWeapon.energyWeaponModePressedTime = Time.time + 0.2f;

            AudioFunctions.PlayAudioClip(energyWeapon.smallShip.audioManager, "beep01_toggle", "Cockpit", energyWeapon.smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);
        }
    }

    #endregion

    #region laser charging

    //This charges the laser
    public static void EnergyWeaponCharging(EnergyWeapon energyWeapon)
    {


        if (energyWeapon.weaponRechargeDelay + 1 < Time.time)
        {
            if (energyWeapon.smallShip.energyWeaponPower == 50)
            {
                if (energyWeapon.smallShip.energyWeaponCharge < 50)
                {
                    energyWeapon.smallShip.energyWeaponCharge += 0.5f;
                }
                else if (energyWeapon.smallShip.energyWeaponCharge > 50)
                {
                    energyWeapon.smallShip.energyWeaponCharge -= 0.5f;
                    energyWeapon.energyWeaponRecharged = true;
                }
                else
                {
                    energyWeapon.energyWeaponRecharged = true;
                }
            }
            else if (energyWeapon.smallShip.energyWeaponPower > 50)
            {
                if (energyWeapon.smallShip.energyWeaponCharge < 100)
                {
                    energyWeapon.smallShip.energyWeaponCharge += 0.5f;
                }
                else if (energyWeapon.smallShip.energyWeaponCharge > 100)
                {
                    energyWeapon.smallShip.energyWeaponCharge -= 0.5f;
                    energyWeapon.energyWeaponRecharged = true;
                }
                else
                {
                    energyWeapon.energyWeaponRecharged = true;
                }
            }
            else if (energyWeapon.smallShip.energyWeaponPower < 50)
            {
                if (energyWeapon.smallShip.energyWeaponCharge < 25)
                {
                    energyWeapon.smallShip.energyWeaponCharge += 0.5f;
                }
                else if (energyWeapon.smallShip.energyWeaponCharge > 25)
                {
                    energyWeapon.smallShip.energyWeaponCharge -= 0.5f;
                    energyWeapon.energyWeaponRecharged = true;
                }
                else
                {
                    energyWeapon.energyWeaponRecharged = true;
                }
            }
        }

        if (energyWeapon.smallShip.energyWeaponCharge <= 0)
        {
            energyWeapon.energyWeaponRecharged = false;
        }
    }

    #endregion

    #region laser fire functions

    //This allows the player to fire the lasers
    public static void InitiateFiringPlayer(EnergyWeapon energyWeapon)
    {
        SmallShip smallShip = energyWeapon.smallShip;

        if (smallShip.fireWeapon == true & smallShip.isAI == false || smallShip.rapidFire == true & smallShip.isAI == false & energyWeapon.weaponManagement.hasRapidFire == true)
        {
            InitiateFiring(smallShip);
        }
    }

    //This executes the firing according to the laser mode
    public static void InitiateFiring(SmallShip smallShip)
    {
        EnergyWeapon energyWeapon = GetEnergyWeapon(smallShip);

        SetCannons(smallShip); //This sets cannon angle prior to firing the laser

        if (smallShip.isDisabled == false & smallShip.energyWeaponCharge > 0 & smallShip.weaponManagement.weaponType == "lasers")
        {
            int weaponMode = WeaponManagementFunctions.GetWeaponMode(energyWeapon.weaponManagement);

            //This calculates the delay before the next laser fires
            float laserWaitTime = 0.1f + (1 - (smallShip.energyWeaponFireRating / 100f)) * 0.250f;

            if (weaponMode == 1) //Dual
            {
                laserWaitTime = laserWaitTime * 2;
            }
            else if (weaponMode == 2) //All
            {
                laserWaitTime = laserWaitTime * 4;
            }
            else if (weaponMode == 3) //Rapid
            {
                laserWaitTime = laserWaitTime * 0.25f;
            }

            //This calculates the weapon charge the laser is going to use
            float weaponCharge = 0.5f;

            if (weaponMode == 1) //Dual
            {
                weaponCharge = weaponCharge * 2;
            }
            else if (weaponMode ==2) //All
            {
                weaponCharge = weaponCharge * 4;
            }

            if (weaponMode != 3) //!= rapid
            {
                smallShip.energyWeaponCharge -= weaponCharge;
                smallShip.energyWeapon.weaponRechargeDelay = Time.time;
            }

            //This intiates firing for the lasers
            if (Time.time > energyWeapon.energyWeaponPressedTime & energyWeapon.energyWeaponFiring != true & smallShip.weaponManagement.weaponsLock == false)
            {
                if (weaponMode == 0 || weaponMode == 3) //Single || Rapid
                {
                    if (energyWeapon.laserCannon3 != null & energyWeapon.laserCannon4 != null)
                    {
                        energyWeapon.energyWeaponCycleNumber = energyWeapon.energyWeaponCycleNumber + 1;

                        if (energyWeapon.energyWeaponCycleNumber > 4)
                        {
                            energyWeapon.energyWeaponCycleNumber = 1;
                        }

                        if (energyWeapon.energyWeaponCycleNumber == 1) { Task a = new Task(FireEnergyWeapons(smallShip, 1, energyWeapon.laserCannon1)); }
                        else if (energyWeapon.energyWeaponCycleNumber == 2) { Task a = new Task(FireEnergyWeapons(smallShip, 1, energyWeapon.laserCannon2)); }
                        else if (energyWeapon.energyWeaponCycleNumber == 3) { Task a = new Task(FireEnergyWeapons(smallShip, 1, energyWeapon.laserCannon3)); }
                        else if (energyWeapon.energyWeaponCycleNumber == 4) { Task a = new Task(FireEnergyWeapons(smallShip, 1, energyWeapon.laserCannon4)); }

                    }
                    else if (energyWeapon.laserCannon1 != null & energyWeapon.laserCannon2 != null & energyWeapon.laserCannon3 != null)
                    {
                        energyWeapon.energyWeaponCycleNumber = energyWeapon.energyWeaponCycleNumber + 1;

                        if (energyWeapon.energyWeaponCycleNumber > 3)
                        {
                            energyWeapon.energyWeaponCycleNumber = 1;
                        }

                        if (energyWeapon.energyWeaponCycleNumber == 1) { Task a = new Task(FireEnergyWeapons(smallShip, 1, energyWeapon.laserCannon1)); }
                        else if (energyWeapon.energyWeaponCycleNumber == 2) { Task a = new Task(FireEnergyWeapons(smallShip, 1, energyWeapon.laserCannon2)); }
                        else if (energyWeapon.energyWeaponCycleNumber == 3) { Task a = new Task(FireEnergyWeapons(smallShip, 1, energyWeapon.laserCannon3)); }
                    }
                    else if (energyWeapon.laserCannon1 != null & energyWeapon.laserCannon2 != null)
                    {
                        energyWeapon.energyWeaponCycleNumber = energyWeapon.energyWeaponCycleNumber + 1;

                        if (energyWeapon.energyWeaponCycleNumber > 2)
                        {
                            energyWeapon.energyWeaponCycleNumber = 1;
                        }

                        if (energyWeapon.energyWeaponCycleNumber == 1) { Task a = new Task(FireEnergyWeapons(smallShip, 1, energyWeapon.laserCannon1)); }
                        else if (energyWeapon.energyWeaponCycleNumber == 2) { Task a = new Task(FireEnergyWeapons(smallShip, 1, energyWeapon.laserCannon2)); }
                    }
                    else if (energyWeapon.laserCannon1 != null)
                    {
                        Task a = new Task(FireEnergyWeapons(smallShip, 1, energyWeapon.laserCannon1));
                    }

                }
                else if (weaponMode == 1) //Dual
                {

                    energyWeapon.energyWeaponCycleNumber = energyWeapon.energyWeaponCycleNumber + 1;

                    if (energyWeapon.energyWeaponCycleNumber > 2)
                    {
                        energyWeapon.energyWeaponCycleNumber = 1;
                    }

                    if (energyWeapon.energyWeaponCycleNumber == 1 & energyWeapon.laserCannon1 != null & energyWeapon.laserCannon2 != null)
                    {
                        Task a = new Task(FireEnergyWeapons(smallShip, 2, energyWeapon.laserCannon1, energyWeapon.laserCannon2));
                    }
                    else if (energyWeapon.energyWeaponCycleNumber == 2 & energyWeapon.laserCannon2 != null & energyWeapon.laserCannon3 != null & energyWeapon.laserCannon4 == null)
                    {
                        Task a = new Task(FireEnergyWeapons(smallShip, 2, energyWeapon.laserCannon2, energyWeapon.laserCannon3));
                    }
                    else if (energyWeapon.energyWeaponCycleNumber == 2 & energyWeapon.laserCannon3 != null & energyWeapon.laserCannon4 != null)
                    {
                        Task a = new Task(FireEnergyWeapons(smallShip, 2, energyWeapon.laserCannon3, energyWeapon.laserCannon4));
                    }
                }
                else if (weaponMode == 2) //All
                {
                    if (energyWeapon.laserCannon1 != null & energyWeapon.laserCannon2 != null & energyWeapon.laserCannon3 != null & energyWeapon.laserCannon4 == null)
                    {
                        Task a = new Task(FireEnergyWeapons(smallShip, 3, energyWeapon.laserCannon1, energyWeapon.laserCannon2, energyWeapon.laserCannon3));
                    }
                    else if (energyWeapon.laserCannon1 != null & energyWeapon.laserCannon2 != null & energyWeapon.laserCannon3 != null & energyWeapon.laserCannon4 != null)
                    {
                        Task a = new Task(FireEnergyWeapons(smallShip, 4, energyWeapon.laserCannon1, energyWeapon.laserCannon2, energyWeapon.laserCannon3, energyWeapon.laserCannon4));
                    }
                }

                energyWeapon.energyWeaponPressedTime = Time.time + laserWaitTime;
            }
        }
    }

    //This runs through the cannons and runs the FireSingleEnergyWeapon script
    public static IEnumerator FireEnergyWeapons(SmallShip smallShip, float lasersToFire, GameObject firstCannon, GameObject secondCannon = null, GameObject thirdCannon = null, GameObject fourthCannon = null)
    {
        EnergyWeapon energyWeapon = GetEnergyWeapon(smallShip);

        energyWeapon.energyWeaponFiring = true;

        float power = smallShip.energyWeaponPower;
        float rating = smallShip.energyWeaponFireRating;
        int type = WeaponManagementFunctions.GetWeaponType(energyWeapon.weaponManagement);
        int mode = WeaponManagementFunctions.GetWeaponMode(energyWeapon.weaponManagement);

        float volume = 0.6f;
        float pitch = 1;

        if (mode == 1) //weapon mode one is rapid fire
        {
            volume = 0.3f;
            pitch = 1.1f;
        }

        if (smallShip.ogInput == null)
        {
            smallShip.ogInput = OGInputFunctions.GetOGInput();
        }

        if (smallShip != null)
        {
            if (energyWeapon.energyWeaponParticleSystem != null)
            {
                ParticleSystem particleSystem = energyWeapon.energyWeaponParticleSystem.GetComponent<ParticleSystem>();
                ParticleSystem particleSystemMuzzleFlash = energyWeapon.energyWeaponMuzzleFlashParticleSystem.GetComponent<ParticleSystem>();

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
                        FireSingleEnergyWeapon(smallShip, energyWeapon, particleSystem, particleSystemMuzzleFlash, firstCannon, power, rating, type, mode);
                        PlayEnergyWeaponSound(smallShip, firstCannon, audioFile, mixer, spatialBlend, pitch, volume);
                    }  
                }

                if (lasersToFire == 2 || lasersToFire == 3 || lasersToFire == 4)
                {
                    if (particleSystem != null & secondCannon != null & smallShip != null)
                    {
                        yield return null;

                        FireSingleEnergyWeapon(smallShip, energyWeapon, particleSystem, particleSystemMuzzleFlash, secondCannon, power, rating, type, mode);
                        PlayEnergyWeaponSound(smallShip, secondCannon, audioFile, mixer, spatialBlend, pitch, volume);
                    }
                }

                if (lasersToFire == 3 || lasersToFire == 4)
                {
                    if (particleSystem != null & thirdCannon != null)
                    {
                        yield return null;

                        FireSingleEnergyWeapon(smallShip, energyWeapon, particleSystem, particleSystemMuzzleFlash, thirdCannon, power, rating, type, mode);
                        PlayEnergyWeaponSound(smallShip, thirdCannon, audioFile, mixer, spatialBlend, pitch, volume);
                    }
                }

                if (lasersToFire == 4)
                {
                    if (particleSystem != null & fourthCannon != null & smallShip != null)
                    {
                        yield return null;

                        FireSingleEnergyWeapon(smallShip, energyWeapon, particleSystem, particleSystemMuzzleFlash, fourthCannon, power, rating, type, mode);
                        PlayEnergyWeaponSound(smallShip, fourthCannon, audioFile, mixer, spatialBlend, pitch, volume);
                    }
                }

                energyWeapon.energyWeaponFiring = false;
            }
        } 
    }

    //This actuallly fires the energy weapon
    public static void FireSingleEnergyWeapon(SmallShip smallShip, EnergyWeapon energyWeapon, ParticleSystem particleSystem, ParticleSystem particleSystemMuzzleFlash, GameObject cannon, float power, float rating, float type, float mode)
    {
        particleSystemMuzzleFlash.transform.position = cannon.transform.position;
        particleSystemMuzzleFlash.transform.rotation = cannon.transform.rotation;
        particleSystemMuzzleFlash.Play();

        particleSystem.transform.position = cannon.transform.position;
        particleSystem.transform.rotation = cannon.transform.rotation;
        particleSystem.Emit(1);

        energyWeapon.customData.Clear();
        energyWeapon.customData.Add(new Vector4(power, rating, type, mode));
        particleSystem.SetCustomParticleData(energyWeapon.customData, ParticleSystemCustomData.Custom1);

        if (smallShip.isAI == false & smallShip.ogInput.keyboardAndMouse == false)
        {
            Task a = new Task(OGInputFunctions.ShakeControllerForSetTime(0.05f, 0.40f, 0.40f));
        }
    }

    //Plays the energy weapon sound
    public static void PlayEnergyWeaponSound(SmallShip smallShip, GameObject cannon, string audioFile, string mixer, float spatialBlend, float pitch, float volume)
    {
        AudioFunctions.PlayAudioClip(smallShip.audioManager, audioFile, mixer, cannon.transform.position, spatialBlend, pitch, 500, volume);
    }

    #endregion

    #region collision and damage functions

    //This handles an event where the laser hits something
    public static void RunCollisionEvent(ParticleSystem particleSystemScript, SmallShip smallShip)
    {
        //Get collision information
        List<Vector3> hitPositions = new List<Vector3>();
        List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
        List<Vector4> customData = new List<Vector4>();

        int events = particleSystemScript.GetCollisionEvents(smallShip.gameObject, collisionEvents); //This grabs all the collision events
        particleSystemScript.GetCustomParticleData(customData, ParticleSystemCustomData.Custom1);

        for (int i = 0; i < events; i++) //This cycles through all the collision events and deals with one at a time
        {
            //This gets key data from the collision event
            Vector3 hitPosition = collisionEvents[i].intersection; //This gets the position of the collision event 
            float power = customData[i].x;
            float rating = customData[i].y;
            float type = customData[i].z;

            if (smallShip != null)
            {
                //This gets a key reference, audiomanager
                Audio audioManager = GameObject.FindFirstObjectByType<Audio>();

                //This gets key information on the object hit
                var objectHitDetails = ObjectHitDetails(smallShip.gameObject, hitPosition);
                float shieldFront = objectHitDetails.shieldFront;
                float shieldBack = objectHitDetails.shieldBack;
                float forward = objectHitDetails.forward;
                string shieldType = objectHitDetails.shieldType;

                //This instantiates an explosion
                InstantiateLaserExplosion(hitPosition, forward, shieldFront, shieldBack, smallShip.energyWeapon.laserColour, shieldType, audioManager);

                //This makes the screen flash
                if (objectHitDetails.isAI == false)
                {
                    HudFunctions.ScreenFlash();
                }

                //This causes the ship to take damage
                float damage = CalculateWeaponDamage(power, rating);
                DamageFunctions.TakeDamage_SmallShip(smallShip, damage, hitPosition, false);              
            }
        }
    }

    //This function returns the root parent of the prefab by looking for the component that will only be attached to the parent gameobject
    public static GameObject ReturnParent(GameObject objectHit)
    {
        GameObject parent = objectHit; //This assumes the object his is the parent unless another is found

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
    public static (float shieldFront, float shieldBack, float forward, string shieldType, bool isAI) ObjectHitDetails(GameObject objectHit, Vector3 hitPosition)
    {
        float shieldFront = 0;
        float shieldBack = 0;
        float forward = 0;
        string shieldType = "default";
        bool isAI = true;

        SmallShip smallShip = objectHit.gameObject.GetComponentInParent<SmallShip>(); //This gets the smallship function if avaiblible
        LargeShip largeShip = objectHit.gameObject.GetComponentInParent<LargeShip>();

        if (smallShip != null)
        {
            shieldType = smallShip.shieldType;

            shieldFront = smallShip.frontShieldLevel;
            shieldBack = smallShip.rearShieldLevel;

            Vector3 relativePosition = smallShip.gameObject.transform.position - hitPosition;
            forward = -Vector3.Dot(smallShip.gameObject.transform.position, relativePosition.normalized);

            isAI = smallShip.isAI;
        }
        else if (largeShip != null)
        {
            shieldType = largeShip.shieldType;

            shieldFront = largeShip.frontShieldLevel;
            shieldBack = largeShip.rearShieldLevel;

            Vector3 relativePosition = largeShip.gameObject.transform.position - hitPosition;
            forward = -Vector3.Dot(largeShip.gameObject.transform.position, relativePosition.normalized);
        }

        return (shieldFront, shieldBack, forward, shieldType, isAI);
    }

    //This instantiates the correct explosion at the hit position
    public static void InstantiateLaserExplosion(Vector3 hitPosition, float forward, float shieldFront, float shieldBack, string laserColor, string shieldType, Audio audioManager)
    {
        //This selects the correct explosion colour
        string explosionChoice = "laserblast_red";

        if (forward > 0 & shieldFront > 0 || forward < 0 & shieldBack > 0)
        {
            if (shieldType == "blackhole")
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
        ParticleFunctions.InstantiateExplosion(hitPosition, explosionChoice, 6, audioManager);
        
    }

    //This calculates the laser damage
    public static float CalculateWeaponDamage(float laserPower, float laserRating)
    {
        float damage = 0;
        float laserDamage = 0;

        laserDamage = 50;
        
        if (laserPower > 50)
        {
            damage = (laserDamage / 100F) * laserRating;
        }
        else if (laserPower == 50f)
        {
            damage = (laserDamage / 100F) * laserRating;
        }
        else if (laserPower < 50f)
        {
            damage = (laserDamage / 100F) * laserRating;
        }

        return damage;
    }

    #endregion

    #region utilities

    public static EnergyWeapon GetEnergyWeapon(SmallShip smallShip)
    {
        EnergyWeapon laser = null;

        if (smallShip != null)
        {
            laser = smallShip.GetComponent<EnergyWeapon>();
        }

        return laser;
    }

    #endregion

}
