using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TurretFunctions
{

    #region loading functions

    //This loads all the turrets on the ship
    public static void LoadTurrets(LargeShip largeShip)
    {
        if(largeShip.largeturret == "default")
        {
            largeShip.largeturret = "default_turretmedium_norestriction";
        }

        if(largeShip.smallturret == "default")
        {
            largeShip.smallturret = "default_turretsmall_norestriction";
        }

        if (largeShip.largeturret == "default_large")
        {
            largeShip.largeturret = "default_turretlarge_norestriction";
        }

        if (largeShip.largeturret == "default_medium")
        {
            largeShip.largeturret = "default_turretmedium_norestriction";
        }

        if (largeShip.largeturret == "default_large_locked")
        {
            largeShip.largeturret = "default_turretlarge";
        }

        if (largeShip.largeturret == "default_medium_locked")
        {
            largeShip.largeturret = "default_turretmedium";
        }

        if (largeShip.largeturret == "default_locked")
        {
            largeShip.largeturret = "default_turretmedium";
        }

        if (largeShip.smallturret == "default_locked")
        {
            largeShip.smallturret = "default_turretsmall";
        }

        if (largeShip.turretsLoaded == false)
        {
            List<Turret> turrets = new List<Turret>();
            Transform[] turretTransforms = GameObjectUtils.FindAllChildTransformsContaining(largeShip.transform, "turret", "turrettransforms");

            GameObject turretLarge = Resources.Load(OGGetAddress.turrets + largeShip.largeturret) as GameObject;
            GameObject turretSmall = Resources.Load(OGGetAddress.turrets + largeShip.smallturret) as GameObject;

            foreach (Transform turretTransform in turretTransforms)
            {
                if (turretTransform != null)
                {
                    GameObject turretGameObject = null;

                    if (turretTransform.name == "turretsmall")
                    {
                        if (turretSmall != null)
                        {
                            turretGameObject = GameObject.Instantiate(turretSmall);
                        }
                    }
                    else if (turretTransform.name == "turretlarge")
                    {
                        if (turretLarge != null)
                        {
                            turretGameObject = GameObject.Instantiate(turretLarge);
                        }
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

                        CheckTurretType(tempTurret, largeShip.laserColor);

                        LoadLaserParticleSystem(tempTurret);

                        GameObjectUtils.AddMeshColliders(turretGameObject, false);

                        GameObjectUtils.SetLayerAllChildren(turretTransform, turretTransform.gameObject.layer);

                        if (largeShip.scene != null)
                        {
                            if (largeShip.scene.turrets == null)
                            {
                                largeShip.scene.turrets = new List<Turret>();
                            }

                            largeShip.scene.turrets.Add(tempTurret);
                        }

                        turrets.Add(tempTurret);
                    }
                }
            }

            largeShip.turrets = turrets.ToArray();

            largeShip.turretsLoaded = true;
        }
    }

    //This inputs the rotation values
    public static void CheckTurretType(Turret turret, string colour)
    {
        //This gets the Json ship data
        TextAsset turretTypesFile = Resources.Load(OGGetAddress.files + "TurretTypes") as TextAsset;
        TurretTypes turretTypes = JsonUtility.FromJson<TurretTypes>(turretTypesFile.text);

        //Check for ship type in shipTypeData
        TurretType turretType = null;

        if (turret.gameObject != null)
        {
            foreach (TurretType tempTurretType in turretTypes.turretTypeData)
            {
                if (turret.gameObject.name.Contains(tempTurretType.type))
                {
                    turretType = tempTurretType;
                    break;
                }
            }

            if (turretType != null)
            {
                turret.yRotationMax = turretType.yRotationMax;
                turret.yRotationMin = turretType.yRotationMin;
                turret.xRotationMax = turretType.xRotationMax;
                turret.xRotationMin = turretType.xRotationMin;
                turret.turretSpeed = turretType.turretSpeed;
                turret.fireDelay = turretType.fireDelay;
                turret.laserColor = colour;
                turret.laserDamage = turretType.laserDamage;
                turret.turretType = turretType.turretType;
                turret.systemsLevel = turretType.systemsLevel;
                turret.hullLevel = turretType.hullLevel;
                turret.yRotationIsRestricted = turretType.yRotationIsRestricted;
            }
            else
            {
                turret.yRotationMax = 90;
                turret.yRotationMin = -90;
                turret.xRotationMax = 0;
                turret.xRotationMin = -90;
                turret.turretSpeed = 90;
                turret.fireDelay = 4f;
                turret.laserColor = colour;
                turret.laserDamage = 10;
                turret.turretType = "small";
                turret.systemsLevel = 100;
                turret.hullLevel = 100;
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
            laser = Resources.Load(OGGetAddress.particles + "models/laser") as GameObject;
        }
        else
        {
            laser = Resources.Load(OGGetAddress.particles + "models/laser_turbo") as GameObject;
        }

        Mesh laserMesh = laser.GetComponent<MeshFilter>().sharedMesh;

        Material redLaserMaterial = Resources.Load(OGGetAddress.particles + "materials/laser_material_red") as Material;
        Material greenLaserMaterial = Resources.Load(OGGetAddress.particles + "materials/laser_material_green") as Material;
        Material yellowLaserMaterial = Resources.Load(OGGetAddress.particles + "materials/laser_material_yellow") as Material;

        GameObject redLaserLight = Resources.Load(OGGetAddress.particles + "lights/laser_light_red") as GameObject;
        GameObject greenLaserLight = Resources.Load(OGGetAddress.particles + "lights/laser_light_green") as GameObject;
        GameObject yellowLaserLight = Resources.Load(OGGetAddress.particles + "lights/laser_light_yellow") as GameObject;

        //This loads the particle system and the particle collider
        turret.turretParticleSystem = new GameObject();
        turret.turretParticleSystem.name = "laserparticlesystem_" + turret.gameObject.name;
        ParticleSystem particleSystem = turret.turretParticleSystem.AddComponent<ParticleSystem>();
        ParticleSystemRenderer particleSystemRenderer = turret.turretParticleSystem.GetComponent<ParticleSystemRenderer>();
        OnLaserHit particleCollision = turret.turretParticleSystem.AddComponent<OnLaserHit>();
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
        main.simulationSpace = ParticleSystemSimulationSpace.Custom;
        main.customSimulationSpace = scene.transform;
        main.startLifetime = 15;
        main.startSize3D = true;
        main.startSizeX = 1;
        main.startSizeY = 1;
        main.startSizeZ = 1;
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
        else if (turret.laserColor == "green")
        {
            particleSystemRenderer.material = greenLaserMaterial;
            lights.light = greenLaserLight.GetComponent<Light>();
        }
        else if (turret.laserColor == "yellow")
        {
            particleSystemRenderer.material = yellowLaserMaterial;
            lights.light = yellowLaserLight.GetComponent<Light>();
        }

        //This prevents the particle system playing when loaded
        particleSystem.Stop();
    }
    #endregion

    #region turret input

    //This gets the turrets target
    public static void GetTarget(Turret turret)
    {
        if (turret.targetGO == null)
        {
            turret.requestingTarget = true;
           
        }
        else if (turret.targetGO.gameObject.activeSelf == false)
        {
            turret.requestingTarget = true;
        }
        else if (turret.targetForward < 0)
        {
            turret.requestingTarget = true;
        }
        else
        {
            turret.requestingTarget = false;
        }
    }

    //This gets the input for the turret from ship
    public static void TurretInput(Turret turret, LargeShip largeShip)
    {
        if (turret.targetGO != null)
        {
            //This gets the targets relative position
            Vector3 targetRelativePosition = turret.targetGO.transform.position - turret.transform.position;

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
            turret.turretBase.transform.Rotate(baseRotation, Time.deltaTime * baseSpeed, Space.World);
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
            turret.turretArm.transform.Rotate(armRotation, Time.deltaTime * armSpeed, Space.World);
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
        if (turret.targetGO != null & turret.largeShip.weaponsLock == false & turret.largeShip.systemsLevel > 0 & turret.systemsLevel > 0)
        {
            if (turret.targetGO.activeSelf != false)
            {
                if (turret.targetForward > 0.75 & turret.fireDelayCount < Time.time & turret.turretFiring == false)
                {
                    Task a = new Task(FireLasers(turret));

                    if (turret.fireDelay == 0)
                    {
                        turret.fireDelay = 3;
                    }

                    turret.fireDelayCount = Time.time + turret.fireDelay + Random.Range(0,3);
                }
            }
        }
    }

    public static IEnumerator FireLasers(Turret turret)
    {
        turret.turretFiring = true;

        if (turret != null)
        {
            ParticleSystem particleSystem = turret.turretParticleSystem.GetComponent<ParticleSystem>();

            if (particleSystem != null)
            {
                foreach (Transform turretTransform in turret.firepoints)
                {
                    if (turretTransform != null)
                    {
                        particleSystem.transform.position = turretTransform.position;
                        particleSystem.transform.rotation = turretTransform.transform.rotation;
                        particleSystem.Play();

                        if (turret.audioManager == null)
                        {
                            turret.audioManager = AudioFunctions.GetAudioManager();
                        }
                        else
                        {
                            AudioFunctions.PlayAudioClip(turret.audioManager, turret.audioFile, "External", turret.transform.position, 1, 1, 500, 0.6f);
                        }

                        yield return null;
                    }
                }
            }

            turret.turretFiring = false;
        }
    }

    #endregion

    #region damage

    //This causes the ship to take damage from lasers
    public static void TakeDamage(Turret turret, float damage, Vector3 hitPosition)
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

    //This causes the ship to take damage from lasers
    public static void TakeSystemsDamage(Turret turret, float damage, Vector3 hitPosition)
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
                        if (turret.systemsLevel - damage < 5 & turret.largeShip.invincible == true)
                        {
                            turret.systemsLevel = 5;
                        }
                        else
                        {
                            turret.systemsLevel = turret.systemsLevel - damage;
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
                        if (turret.systemsLevel - damage < 5 & turret.largeShip.invincible == true)
                        {
                            turret.systemsLevel = 5;
                        }
                        else
                        {
                            turret.systemsLevel = turret.systemsLevel - damage;
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

            Scene scene = SceneFunctions.GetScene();

            ParticleFunctions.InstantiateExplosion(turret.largeShip.gameObject, turret.gameObject.transform.position, "explosion02", 12);
            turret.gameObject.SetActive(false);
        }
    }

    #endregion

    #region new turret code

    //This function cycles through avalaible turrets and fire lasers
    public static IEnumerator RunTurrets(Turret turret)
    {
        //This loads the turret particle system
        if (turret.particleSystem == null)
        {
            turret.particleSystem = LoadTurretParticleSystem(turret);
        }

        //This creates the turret GO for calculating rotation and position
        if (turret.turretGO == null)
        {
            turret.turretGO = new GameObject();
            turret.turretGO.name = "turretBase_" + turret.shipGO.name;
        }

        //This gets all the current turret positions
        if (turret.turretPositions == null)
        {
            turret.turretPositions = GameObjectUtils.FindAllChildTransformsContaining(turret.shipGO.transform, "turret", "turrettransforms", "turretBase");
        }

        //This cycles through all the turrets and fires them
        if (turret.turretPositions != null & turret.turretGO != null & turret.targetGO != null)
        {
            foreach (Transform turretPosition in  turret.turretPositions)
            {
                //This gets key references
                GameObject turretGO = turret.turretGO;
                GameObject targetGO = turret.targetGO;
                GameObject shipGO = turret.shipGO;
                ParticleSystem particleSystem = turret.particleSystem;
                Audio audioManager = turret.audioManager;
                string audioFile = turret.audioFile;

                //This positions and aims the turret
                bool restrictForward = false;

                if (turretPosition.name.Contains("restrictforward"))
                {
                    restrictForward = true;
                }

                PositionAndAimTurret(turretGO, turretPosition.gameObject, targetGO, shipGO, restrictForward, turret.accuracy);

                //This checks if the turret is clear to fire
                bool clearToFire = ClearToFire(turretGO, targetGO);

                //This calculates the fire positions
                int firePointsNo = 1;

                if (turretPosition.name.Contains("2"))
                {
                    firePointsNo = 2;
                }
                else if (turretPosition.name.Contains("4"))
                {
                    firePointsNo = 4;
                }

                Vector3[] firePoints = CalculateFirePointPositions(turretGO, firePointsNo, 5);

                //This fires the lasers using the fire points
                foreach(Vector3 firePoint in firePoints)
                {
                    FireTurret(turretGO, particleSystem, firePoint, audioManager, audioFile);
                    yield return null;
                }

                yield return null;
            }
        }
    }

    //CREATE FUNCTION TO GET KEY VALUES FROM SHIP LIKE ALLEGIANCE, TARGET, AND WHATNOT

    //This loads the turret particle system
    public static ParticleSystem LoadTurretParticleSystem(Turret turret)
    {
        GameObject laser = null;

        //This loads the necessary prefabs
        if (turret.name.Contains("small"))
        {
            laser = Resources.Load(OGGetAddress.particles + "models/laser") as GameObject;
        }
        else
        {
            laser = Resources.Load(OGGetAddress.particles + "models/laser_turbo") as GameObject;
        }

        Mesh laserMesh = laser.GetComponent<MeshFilter>().sharedMesh;

        Material redLaserMaterial = Resources.Load(OGGetAddress.particles + "materials/laser_material_red") as Material;
        Material greenLaserMaterial = Resources.Load(OGGetAddress.particles + "materials/laser_material_green") as Material;
        Material yellowLaserMaterial = Resources.Load(OGGetAddress.particles + "materials/laser_material_yellow") as Material;

        GameObject redLaserLight = Resources.Load(OGGetAddress.particles + "lights/laser_light_red") as GameObject;
        GameObject greenLaserLight = Resources.Load(OGGetAddress.particles + "lights/laser_light_green") as GameObject;
        GameObject yellowLaserLight = Resources.Load(OGGetAddress.particles + "lights/laser_light_yellow") as GameObject;

        //This loads the particle system and the particle collider
        turret.turretParticleSystem = new GameObject();
        turret.turretParticleSystem.name = "laserparticlesystem_" + turret.gameObject.name;
        ParticleSystem particleSystem = turret.turretParticleSystem.AddComponent<ParticleSystem>();
        ParticleSystemRenderer particleSystemRenderer = turret.turretParticleSystem.GetComponent<ParticleSystemRenderer>();
        OnLaserHit particleCollision = turret.turretParticleSystem.AddComponent<OnLaserHit>();
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
        main.simulationSpace = ParticleSystemSimulationSpace.Custom;
        main.customSimulationSpace = scene.transform;
        main.startLifetime = 15;
        main.startSize3D = true;
        main.startSizeX = 1;
        main.startSizeY = 1;
        main.startSizeZ = 1;
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

        //This sets up the light system
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
        else if (turret.laserColor == "green")
        {
            particleSystemRenderer.material = greenLaserMaterial;
            lights.light = greenLaserLight.GetComponent<Light>();
        }
        else if (turret.laserColor == "yellow")
        {
            particleSystemRenderer.material = yellowLaserMaterial;
            lights.light = yellowLaserLight.GetComponent<Light>();
        }

        //This prevents the particle system playing when loaded
        particleSystem.Stop();

        return (particleSystem);
    }

    //This sets the collision layer for the lasers
    public static LayerMask SetLaserCollisionLayers(Turret turret)
    {
        LayerMask collisionLayers = new LayerMask();

        //This gets the Json ship data
        TextAsset allegiancesFile = Resources.Load(OGGetAddress.files + "Allegiances") as TextAsset;
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

        if (allegiance == null)
        {
            Debug.Log("Allegiance is null");
        }

        if (layerNames == null)
        {
            Debug.Log("Layer names is null");
        }

        if (allegiance != null)
        {
            collisionLayers &= ~(1 << GetLayerInt(allegiance.allegiance, layerNames));
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

    //This positions and aims the turret
    public static void PositionAndAimTurret(GameObject turret, GameObject turretPosition, GameObject target, GameObject ship, bool restrictForward = false, string accuracy = "low")
    {
        //This gets the target rigidbody for the intercept point calculation
        Rigidbody targetRigidBody = target.GetComponent<Rigidbody>();

        //This gets the intercept point
        Vector3 interceptPoint = GameObjectUtils.CalculateInterceptPoint(turret.transform.position, target.transform.position, targetRigidBody.linearVelocity, 750);

        //This sets the error margin for the shot
        Vector3 errorMargin = SetTargetingErrorMargin(accuracy);

        //This positions the turret
        turret.transform.position = turretPosition.transform.position;

        //This checks whether the turret is upside down or not
        bool isUpsideDown = false;

        if (Vector3.Dot(turret.transform.up, -ship.transform.up) > 0)
        {
            isUpsideDown = true;
        }

        //This aims the turret
        turret.transform.LookAt(interceptPoint + errorMargin);

        // This calculates the restricted rotation
        if (restrictForward == true)
        {
            //Get current rotation as euler
            Vector3 eulerRotation = turret.transform.eulerAngles;

            // Restrict the y rotation between -90 and 90 degrees
            eulerRotation.y = Mathf.Clamp(eulerRotation.y, -90f, 90f);

            // Restrict the x rotation between -90 and 90 degrees
            eulerRotation.x = Mathf.Clamp(eulerRotation.x, 0, -180f);

            if (isUpsideDown == true)
            {
                // Restrict the x rotation between -90 and 90 degrees
                eulerRotation.x = Mathf.Clamp(eulerRotation.x, 0, 180f);
            }
           
            // Apply the modified rotation back to the transform
            turret.transform.eulerAngles = eulerRotation;
        }
    }

    //This sets the targetting error margin for the turret
    public static Vector3 SetTargetingErrorMargin(string accuracy = "low")
    {
        float low = 100;
        float medium = 50;
        float high = 25;

        float x = 0;
        float y = 0;
        float z = 0;

        if (accuracy == "low")
        {
            x = Random.Range(-low, low);
            y = Random.Range(-low, low);
            z = Random.Range(-low, low);
        }
        else if (accuracy == "medium")
        {
            x = Random.Range(-medium, medium);
            y = Random.Range(-medium, medium);
            z = Random.Range(-medium, medium);
        }
        else if (accuracy == "high")
        {
            x = Random.Range(-high, high);
            y = Random.Range(-high, high);
            z = Random.Range(-high, high);
        }

        return(new Vector3(x, y, z));       
    }

    //This checks the target is in sight before firing
    public static bool ClearToFire(GameObject turret, GameObject target)
    {
        bool clearToFire = false;

        //This checks the target is in the sights of the cannon
        Vector3 targetRelativePosition = target.transform.position - turret.transform.position;

        float targetForward = Vector3.Dot(turret.transform.forward, targetRelativePosition.normalized);

        if (targetForward > 0.9)
        {
            clearToFire = true;
        }

        return clearToFire;
    }

    //This calculates the number of fire points on the turret
    public static Vector3[] CalculateFirePointPositions(GameObject turret, int firePointsNo, float distanceBetweenFirePoints)
    {
        List<Vector3> firePoints = new List<Vector3>();

        //This gets the firepoint positions directly right and left of center
        float currentPositionRight = distanceBetweenFirePoints / 2f;
        float currentPositionLeft = -(distanceBetweenFirePoints / 2f);

        Vector3 firePoint01 = turret.transform.right * (currentPositionRight);
        Vector3 firePoint02 = turret.transform.right * (currentPositionLeft);

        firePoints.Add(firePoint01);
        firePoints.Add(firePoint02);

        //This gets the rest of the firepoints
        for (int i = 0; i < firePointsNo; i = i + 2) 
        {
            currentPositionRight = currentPositionRight + distanceBetweenFirePoints;
            currentPositionLeft = currentPositionLeft - distanceBetweenFirePoints;

            Vector3 firePointRight = turret.transform.right * (currentPositionRight);
            Vector3 firePointLeft = turret.transform.right * (currentPositionLeft);

            firePoints.Add(firePointRight);
            firePoints.Add(firePointLeft);
        }

        return firePoints.ToArray();
    }

    //This fires the turret using a given firepoint
    public static void FireTurret(GameObject turret, ParticleSystem particleSystem, Vector3 firePoint, Audio audioManager, string audioFile)
    {
        particleSystem.transform.position = firePoint;
        particleSystem.transform.rotation = turret.transform.rotation;
        particleSystem.Play();

        AudioFunctions.PlayAudioClip(audioManager, audioFile, "External", firePoint, 1, 1, 500, 0.6f);
    }

    #endregion
}
