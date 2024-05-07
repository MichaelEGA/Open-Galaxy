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
        GameObject ion = Resources.Load(OGGetAddress.particles + "ion/ion") as GameObject; //This needs to be created then changed

        Mesh ionMesh = ion.GetComponent<MeshFilter>().sharedMesh;

        Material ionMaterial = Resources.Load(OGGetAddress.particles + "ion/ion_material") as Material;  //This needs to be created then changed
        GameObject ionLight = Resources.Load(OGGetAddress.particles + "ion/ion_light") as GameObject;  //This needs to be created then changed

        //This loads the particle system and the particle collider
        smallShip.ionParticleSystem = new GameObject();
        smallShip.ionParticleSystem.name = "ionparticlesystem_" + smallShip.gameObject.name;
        ParticleSystem particleSystem = smallShip.ionParticleSystem.AddComponent<ParticleSystem>();
        ParticleSystemRenderer particleSystemRenderer = smallShip.ionParticleSystem.GetComponent<ParticleSystemRenderer>();
        OnLaserHit particleCollision = smallShip.ionParticleSystem.AddComponent<OnLaserHit>();
        particleCollision.relatedGameObject = smallShip.gameObject;
        particleCollision.type = "ion";

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
        main.startSize3D = true;
        main.startSizeX = 0.25f;
        main.startSizeY = 0.25f;
        main.startSizeZ = 5;
        main.startSpeed = 1000;
        main.loop = false;
        main.playOnAwake = false;

        //This speeds the particle up over time, start slow so they show on the screen then speed up quickly
        var velocityOverLifetime = particleSystem.velocityOverLifetime;
        velocityOverLifetime.enabled = true;

        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0.0f, 0.333f);
        curve.AddKey(0.1f, 1.0f);

        ParticleSystem.MinMaxCurve minMaxCurve = new ParticleSystem.MinMaxCurve(3.0f, curve);
        velocityOverLifetime.speedModifier = minMaxCurve;

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
        if (smallShip.ionCannon1 != null)
        {
            smallShip.ionCannon1.transform.LookAt(smallShip.gameObject.transform.position + (smallShip.gameObject.transform.forward * smallShip.interceptDistance));
        }
        else if (smallShip.ionCannon2 != null)
        {
            smallShip.ionCannon2.transform.LookAt(smallShip.gameObject.transform.position + (smallShip.gameObject.transform.forward * smallShip.interceptDistance));
        }
        else if (smallShip.ionCannon3 != null)
        {
            smallShip.ionCannon3.transform.LookAt(smallShip.gameObject.transform.position + (smallShip.gameObject.transform.forward * smallShip.interceptDistance));
        }
        else if (smallShip.ionCannon4 != null)
        {
            smallShip.ionCannon4.transform.LookAt(smallShip.gameObject.transform.position + (smallShip.gameObject.transform.forward * smallShip.interceptDistance));
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
            else if (smallShip.weaponMode == "dual" & smallShip.ionCannon3 != null & smallShip.ionCannon4 != null)
            {
                smallShip.weaponMode = "quad";
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
        if (smallShip.fireWeapon == true & smallShip.isAI == false)
        {
            InitiateFiring(smallShip);
        }
    }

    //This executes the firing according to the ion mode
    public static void InitiateFiring(SmallShip smallShip)
    {
        //This calculates the delay before the next ion fires
        float ionWaitTime = 0.1f + (1 - (smallShip.laserFireRating / 100f)) * 0.250f;

        if (smallShip.weaponMode == "dual")
        {
            ionWaitTime = ionWaitTime * 2;
        }
        else if (smallShip.weaponMode == "quad")
        {
            ionWaitTime = ionWaitTime * 4;
        }

        if (Time.time > smallShip.ionPressedTime & smallShip.ionfiring != true & smallShip.activeWeapon == "ion" & smallShip.weaponsLock == false)
        {
            if (smallShip.weaponMode == "single")
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
                else if (smallShip.ionCycleNumber == 2 & smallShip.ionCannon3 != null & smallShip.ionCannon4 != null)
                {
                    Task a = new Task(FireIons(smallShip, 2, smallShip.ionCannon3, smallShip.ionCannon4));
                }
            }
            else if (smallShip.weaponMode == "quad" & smallShip.ionCannon1 != null & smallShip.ionCannon2 != null & smallShip.ionCannon3 != null & smallShip.ionCannon4 != null)
            {
                Task a = new Task(FireIons(smallShip, 4, smallShip.ionCannon1, smallShip.ionCannon2, smallShip.ionCannon3, smallShip.ionCannon4));
            }

            smallShip.ionPressedTime = Time.time + ionWaitTime;
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

                float spatialBlend = 1f;
                string mixer = "External";

                if (smallShip.isAI == false)
                {
                    spatialBlend = 0;
                    mixer = "Cockpit";
                }

                string audioFile = smallShip.ionAudio;

                if (ionCannonsToFire == 1 || ionCannonsToFire == 2 || ionCannonsToFire == 4)
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

                if (ionCannonsToFire == 2 || ionCannonsToFire == 4)
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

                if (ionCannonsToFire == 4)
                {
                    if (particleSystem != null & thirdCannon != null & smallShip != null)
                    {
                        yield return null;
                        particleSystem.transform.position = thirdCannon.transform.position;
                        particleSystem.transform.rotation = thirdCannon.transform.rotation;
                        particleSystem.Play();
                        AudioFunctions.PlayAudioClip(smallShip.audioManager, audioFile, mixer, thirdCannon.transform.position, spatialBlend, 1, 500, 0.6f);
                    }

                    if (particleSystem != null & fourthCannon != null & smallShip != null)
                    {
                        yield return null;
                        particleSystem.transform.position = fourthCannon.transform.position;
                        particleSystem.transform.rotation = fourthCannon.transform.rotation;
                        particleSystem.Play();
                        AudioFunctions.PlayAudioClip(smallShip.audioManager, audioFile, mixer, fourthCannon.transform.position, spatialBlend, 1, 500, 0.6f);
                    }
                }

                smallShip.ionfiring = false;
            }
        }
    }

    #endregion

}
