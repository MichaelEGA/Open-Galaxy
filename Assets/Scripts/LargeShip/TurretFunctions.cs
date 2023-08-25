using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TurretFunctions
{

    #region loading functions

    //This loads all the turrets on the ship
    public static void LoadTurrets(LargeShip largeShip)
    {
        if (largeShip.turretsLoaded == false)
        {
            List<Turret> turrets = new List<Turret>();
            Transform[] turretTransforms = GameObjectUtils.FindAllChildTransformsContaining(largeShip.transform, "turret", "turrettransforms");

            GameObject turretLarge = Resources.Load("TurretPrefabs/" + largeShip.prefabName + "_turretlarge") as GameObject;
            GameObject turretSmall = Resources.Load("TurretPrefabs/" + largeShip.prefabName + "_turretsmall") as GameObject;

            foreach (Transform turretTransform in turretTransforms)
            {
                if (turretTransform != null)
                {
                    GameObject turretGameObject = null;

                    if (turretTransform.name == "turretsmall")
                    {
                        turretGameObject = GameObject.Instantiate(turretSmall);
                    }
                    else if (turretTransform.name == "turretlarge")
                    {
                        turretGameObject = GameObject.Instantiate(turretLarge);
                    }

                    if (turretGameObject != null)
                    {                
                        Turret tempTurret = turretGameObject.AddComponent<Turret>();
                        Rigidbody rigidbody = turretGameObject.AddComponent<Rigidbody>();
                        rigidbody.isKinematic = true;
                        tempTurret.largeShip = largeShip;
                        tempTurret.turretBase = turretGameObject;
                        tempTurret.allegiance = largeShip.allegiance;

                        Transform turretArm = GameObjectUtils.FindChildTransformContaining(turretGameObject.transform, "arm");

                        if (turretArm != null)
                        {
                            tempTurret.turretArm = turretArm.gameObject;

                            Transform firePoint = GameObjectUtils.FindChildTransformContaining(turretGameObject.transform, "fire");

                            if (firePoint != null)
                            {
                                tempTurret.firepoints = GameObjectUtils.GetAllChildTransforms(firePoint); 
                            }
                        }

                        turretGameObject.transform.position = turretTransform.position;
                        turretGameObject.transform.rotation = turretTransform.rotation;
                        turretGameObject.transform.SetParent(turretTransform);

                        CheckTurretType(tempTurret);

                        LoadLaserParticleSystem(tempTurret);

                        GameObjectUtils.AddColliders(turretGameObject, false);

                        GameObjectUtils.SetLayerAllChildren(turretTransform, turretTransform.gameObject.layer);

                        turrets.Add(tempTurret);
                    }
                }
            }

            largeShip.turrets = turrets.ToArray();

            largeShip.turretsLoaded = true;
        }
    }

    //This inputs the rotation values
    public static void CheckTurretType(Turret turret)
    {
        if (turret.gameObject != null)
        {
            if (turret.gameObject.name.Contains("isd_turretlarge"))
            {
                turret.yRotationMax = 30;
                turret.yRotationMin = -30;
                turret.xRotationMax = 0;
                turret.xRotationMin = -90;
                turret.turretSpeed = 70;
                turret.fireDelay = 6f;
                turret.laserColor = "green";
                turret.laserDamage = 50;
                turret.turretType = "large";
                turret.yRotationIsRestricted = true;
            }
            else if (turret.gameObject.name.Contains("isd_turretsmall"))
            {
                turret.yRotationMax = 90;
                turret.yRotationMin = -90;
                turret.xRotationMax = 0;
                turret.xRotationMin = -90;
                turret.turretSpeed = 90;
                turret.fireDelay = 4f;
                turret.laserColor = "green";
                turret.laserDamage = 10;
                turret.turretType = "small";
                turret.yRotationIsRestricted = true;
            }
            if (turret.gameObject.name.Contains("mc80a_turretlarge"))
            {
                turret.yRotationMax = 60;
                turret.yRotationMin = -60;
                turret.xRotationMax = 50;
                turret.xRotationMin = 0;
                turret.turretSpeed = 80;
                turret.fireDelay = 6f;
                turret.laserColor = "red";
                turret.laserDamage = 50;
                turret.turretType = "large";
                turret.yRotationIsRestricted = true;
            }
            else if (turret.gameObject.name.Contains("mc80a_turretsmall"))
            {
                turret.yRotationMax = 60;
                turret.yRotationMin = -60;
                turret.xRotationMax = 50;
                turret.xRotationMin = 0;
                turret.turretSpeed = 90;
                turret.fireDelay = 4f;
                turret.laserColor = "red";
                turret.laserDamage = 10;
                turret.turretType = "small";
                turret.yRotationIsRestricted = true;
            }
            else if (turret.gameObject.name.Contains("cr90_turretlarge"))
            {
                turret.yRotationMax = 180;
                turret.yRotationMin = -180;
                turret.xRotationMax = 0;
                turret.xRotationMin = -90;
                turret.turretSpeed = 80;
                turret.fireDelay = 4f;
                turret.laserColor = "red";
                turret.laserDamage = 30;
                turret.turretType = "large";
                turret.yRotationIsRestricted = false;
            }
        }
    }

    //This sets all the correct settings on the provided particle system to fire lasers
    public static void LoadLaserParticleSystem(Turret turret)
    {
        GameObject laser = null;

        //This loads the necessary prefabs
        if (turret.name.Contains("small"))
        {
            laser = Resources.Load("ParticlePrefabs/Laser01/laser01") as GameObject;
        }
        else
        {
            laser = Resources.Load("ParticlePrefabs/Laser_Turbo/laser_turbo") as GameObject;
        }

        Mesh laserMesh = laser.GetComponent<MeshFilter>().sharedMesh;
        Material redLaserMaterial = Resources.Load("ParticlePrefabs/Laser01/laser01_material_red") as Material;
        Material greenLaserMaterial = Resources.Load("ParticlePrefabs/Laser01/laser01_material_green") as Material;
        GameObject redLaserLight = Resources.Load("ParticlePrefabs/Laser01/laser01_light_red") as GameObject;
        GameObject greenLaserLight = Resources.Load("ParticlePrefabs/Laser01/laser01_light_green") as GameObject;

        //This loads the particle system and the particle collider
        turret.turretParticleSystem = new GameObject();
        turret.turretParticleSystem.name = "laserparticlesystem_" + turret.gameObject.name;
        ParticleSystem particleSystem = turret.turretParticleSystem.AddComponent<ParticleSystem>();
        ParticleSystemRenderer particleSystemRenderer = turret.turretParticleSystem.GetComponent<ParticleSystemRenderer>();
        ParticleCollision particleCollision = turret.turretParticleSystem.AddComponent<ParticleCollision>();
        particleCollision.relatedGameObject = turret.gameObject;

        //This adds the new particle system to the pool
        Scene scene = SceneFunctions.GetScene();

        if (scene != null)
        {
            if (scene.lasersPool == null)
            {
                scene.lasersPool = new List<GameObject>();
            }

            scene.lasersPool.Add(turret.turretParticleSystem);
        }

        //This sets the paticle to operate in world space (as opposed to local)
        var main = particleSystem.main;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.startSize3D = true;
        main.startSizeX = 1;
        main.startSizeY = 1;
        main.startSizeZ = 1;
        main.startSpeed = 1500;
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
        collision.collidesWith = SetLaserCollisionLayers(turret);

        //This makes the particle looks like a laser
        particleSystemRenderer.renderMode = ParticleSystemRenderMode.Mesh;
        particleSystemRenderer.alignment = ParticleSystemRenderSpace.Velocity;
        particleSystemRenderer.mesh = laserMesh;

        if (turret.laserColor == "red")
        {
            particleSystemRenderer.material = redLaserMaterial;
            lights.light = redLaserLight.GetComponent<Light>();
        }
        else
        {
            particleSystemRenderer.material = greenLaserMaterial;
            lights.light = greenLaserLight.GetComponent<Light>();
        }
    }

    //This sets the collision layer for the lasers
    public static LayerMask SetLaserCollisionLayers(Turret turret)
    {
        LayerMask collisionLayers = new LayerMask();

        //This gets the Json ship data
        TextAsset allegiancesFile = Resources.Load("Data/Files/Allegiances") as TextAsset;
        Allegiances allegiances = JsonUtility.FromJson<Allegiances>(allegiancesFile.text);

        Allegiance allegiance = null;

        List<string> layerNames = new List<string>();

        foreach (Allegiance tempAllegiance in allegiances.allegianceData)
        {
            if (tempAllegiance.allegiance == turret.allegiance)
            {
                allegiance = tempAllegiance;
            }

            layerNames.Add(tempAllegiance.allegiance); //This makes a list of collision layers and their corresponding integer

        }

        collisionLayers = LayerMask.GetMask("collision_player", "collision_asteroid", "collision01", "collision02", "collision03", "collision04", "collision05", "collision06", "collision07", "collision08", "collision09", "collision10");

        collisionLayers &= ~(1 << GetLayerInt(allegiance.allegiance, layerNames));

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

    #region turret input

    //This gets the input for the turret from ship
    public static void TurretInput(Turret turret, LargeShip largeShip)
    {
        if (turret.target != null)
        {
            //This gets the targets relative position
            Vector3 targetRelativePosition = turret.target.transform.position - turret.transform.position;

            //This conmpares the turret with the ship transform to see if it's upside down or not
            if (Vector3.Dot(turret.turretBase.transform.up, -largeShip.transform.up) > 0)
            {
                turret.isUpsideDown = true;
            }

            //This sets the target position values
            if (turret.isUpsideDown == false)
            {
                turret.targetForward = Vector3.Dot(turret.turretArm.transform.forward, targetRelativePosition.normalized);
                turret.targetRight = Vector3.Dot(turret.turretArm.transform.right, targetRelativePosition.normalized);
                turret.targetUp = Vector3.Dot(turret.turretArm.transform.up, targetRelativePosition.normalized);
            }
            else
            {
                turret.targetForward = Vector3.Dot(turret.turretArm.transform.forward, targetRelativePosition.normalized);
                turret.targetRight = Vector3.Dot(-turret.turretArm.transform.right, targetRelativePosition.normalized);
                turret.targetUp = Vector3.Dot(-turret.turretArm.transform.up, targetRelativePosition.normalized);
            }

            //This transforms the target data into input
            TurretFunctions.SmoothTurnInput(turret, turret.targetRight);
            TurretFunctions.SmoothPitchInput(turret, -turret.targetUp);
        }   
    }

    //This smoothly transitions between input variables as if the computer is using a mouse a joystick
    public static void SmoothPitchInput(Turret turret, float pitchInput)
    {
        float step = +Time.deltaTime / 0.01f;
        turret.pitchInput = Mathf.Lerp(turret.pitchInput, pitchInput, step);
    }

    //This smoothly transitions between input variables as if the computer is using a mouse a joystick
    public static void SmoothTurnInput(Turret turret, float turnInput)
    {
        float step = +Time.deltaTime / 0.01f;
        turret.turnInput = Mathf.Lerp(turret.turnInput, turnInput, step);
    }

    #endregion

    #region turretRotation

    //This rotates the turret
    public static void RotateTurret(Turret turret)
    {
        turret.pitchInputActual = Mathf.Lerp(turret.pitchInputActual, turret.pitchInput, 0.1f);
        turret.turnInputActual = Mathf.Lerp(turret.turnInputActual, turret.turnInput, 0.1f);

        float armSpeed = (10f / 100f) * turret.turretSpeed;
        float baseSpeed = (10f / 100f) * turret.turretSpeed;

        Vector3 armRotation;
        Vector3 baseRotation;

        if (turret.isUpsideDown == false)
        {
            armRotation = turret.turretBase.transform.right * turret.pitchInputActual;
            baseRotation = turret.turretArm.transform.up * turret.turnInputActual;
        }
        else
        {
            armRotation = -turret.turretBase.transform.right * turret.pitchInputActual;
            baseRotation = -turret.turretArm.transform.up * turret.turnInputActual;
        }  

        if (turret.turretBase != null)
        {
            //This applies the rotation
            turret.turretBase.transform.Rotate(baseRotation, Time.fixedDeltaTime * baseSpeed, Space.World);
            turret.turretBase.transform.localRotation = Quaternion.Euler(0, turret.turretBase.transform.localRotation.eulerAngles.y, 0);

            if (turret.yRotationIsRestricted == true)
            {
                //This tracks the rotation and prevents it from going beyond predefined parameters
                float rotation = turret.turretBase.transform.localRotation.eulerAngles.y;

                //This keeps the rotation value between 180 and -180     
                if (rotation > 180)
                {
                    rotation -= 360;
                }
                else if (rotation < -180)
                {
                    rotation += 360;
                }

                //This cause the turret to stop rotating at its limites
                if (rotation < turret.yRotationMin)
                {
                    turret.turretBase.transform.localRotation = Quaternion.Euler(0, turret.yRotationMin, 0);
                }
                else if (rotation > turret.yRotationMax)
                {
                    turret.turretBase.transform.localRotation = Quaternion.Euler(0, turret.yRotationMax, 0);
                }
            }
        }

        if (turret.turretArm != null)
        {
            //This applies the rotation
            turret.turretArm.transform.Rotate(armRotation, Time.fixedDeltaTime * armSpeed, Space.World);
            turret.turretArm.transform.localRotation = Quaternion.Euler(turret.turretArm.transform.localRotation.eulerAngles.x, 0, 0); //This removes unwanted transforms on y and z asix

            //This tracks the rotation and prevents it from going beyond predefined parameters
            float rotation = turret.turretArm.transform.localRotation.eulerAngles.x;

            //This keeps the rotation value between 180 and - 180
            if (rotation > 180)
            {
                rotation -= 360;
            }
            else if (rotation < -180)
            {
                rotation += 360;
            }

            //This cause the turret to stop rotating at its limites
            if (rotation < turret.xRotationMin)
            {
                turret.turretArm.transform.localRotation = Quaternion.Euler(turret.xRotationMin, 0, 0);
            }
            else if (rotation > turret.xRotationMax)
            {
                turret.turretArm.transform.localRotation = Quaternion.Euler(turret.xRotationMax, 0, 0);
            }

        }   
    }

    #endregion

    #region fire lasers

    public static void FireTurret(Turret turret)
    {
        if (turret.target != null & turret.largeShip.dontfire == false)
        {
            if (turret.target.activeSelf != false)
            {
                if (turret.targetForward > 0.75 & turret.fireDelayCount < Time.time & turret.turretFiring == false)
                {
                    Task a = new Task(FireLasers(turret));

                    if (turret.fireDelay == 0)
                    {
                        turret.fireDelay = 3;
                    }

                    turret.fireDelayCount = Time.time + turret.fireDelay;
                }
            }
        }
    }

    public static IEnumerator FireLasers(Turret turret)
    {
        turret.turretFiring = true;

        ParticleSystem particleSystem = turret.turretParticleSystem.GetComponent<ParticleSystem>();

        foreach (Transform turretTransform in turret.firepoints)
        {
            particleSystem.transform.position = turretTransform.position;
            particleSystem.transform.rotation = turretTransform.transform.rotation;
            particleSystem.Play();
            //AudioFunctions.PlayAudioClip(smallShip.audioManager, audioFile, firstCannon.transform.position, spatialBlend, 1, 500, 0.6f);
            yield return null;
        }

        turret.turretFiring = false;
    }

    #endregion

    #region damage

    //This causes the ship to take damage from lasers
    public static void TakeLaserDamage(Turret turret, float damage, Vector3 hitPosition)
    {
        if (Time.time - turret.largeShip.loadTime > 10)
        {
            Vector3 relativePosition = turret.largeShip.gameObject.transform.position - hitPosition;
            float forward = -Vector3.Dot(turret.largeShip.gameObject.transform.position, relativePosition.normalized);

            if (turret.largeShip.hullLevel > 0)
            {
                if (forward > 0)
                {
                    if (turret.largeShip.frontShieldLevel > 0)
                    {
                        turret.largeShip.frontShieldLevel = turret.largeShip.frontShieldLevel - damage;
                        turret.largeShip.shieldLevel = turret.largeShip.shieldLevel - damage;
                    }
                    else
                    {
                        if (turret.hullLevel - damage < 5 & turret.largeShip.invincible == true)
                        {
                            turret.hullLevel = 5;
                        }
                        else
                        {
                            turret.hullLevel = turret.hullLevel - damage;
                        }
                    }
                }
                else
                {
                    if (turret.largeShip.rearShieldLevel > 0)
                    {
                        turret.largeShip.rearShieldLevel = turret.largeShip.rearShieldLevel - damage;
                        turret.largeShip.shieldLevel = turret.largeShip.shieldLevel - damage;
                    }
                    else
                    {
                        if (turret.hullLevel - damage < 5 & turret.largeShip.invincible == true)
                        {
                            turret.hullLevel = 5;
                        }
                        else
                        {
                            turret.hullLevel = turret.hullLevel - damage;
                        }
                    }
                }

                if (turret.largeShip.frontShieldLevel < 0) { turret.largeShip.frontShieldLevel = 0; }
                if (turret.largeShip.rearShieldLevel < 0) { turret.largeShip.rearShieldLevel = 0; }
                if (turret.largeShip.shieldLevel < 0) { turret.largeShip.shieldLevel = 0; }
            }
        }
    }

    //This causes the turret to disappear if it's destroyed
    public static void Explode(Turret turret)
    {
        if (turret.hullLevel <= 0)
        {
            turret.hullLevel = 0;
            turret.gameObject.SetActive(false);
        }
    }

    #endregion
}
