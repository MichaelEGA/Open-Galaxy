using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LaserTurretFunctions
{
    #region turret set up

    //This sets up the turrets
    public static void SetUpTurrets(LaserTurret turret)
    {
        if (turret.turretSetUp == false)
        {
            //This gets the ship gameobject
            if (turret.shipGO == null)
            {
                turret.shipGO = turret.gameObject;
            }

            //This sets key values on the turret script
            if (turret.largeShip == null & turret.smallShip == null)
            {
                turret.largeShip = turret.shipGO.GetComponent<LargeShip>();
                turret.smallShip = turret.shipGO.GetComponent<SmallShip>();

                if (turret.largeShip != null)
                {
                    turret.allegiance = turret.largeShip.allegiance;
                    turret.accuracy = turret.largeShip.aiTargetingMode;
                    turret.laserColor = turret.largeShip.laserColor;
                }
                else if (turret.smallShip != null)
                {
                    turret.allegiance = turret.smallShip.allegiance;
                    turret.accuracy = turret.smallShip.aiTargetingMode;
                    turret.laserColor = turret.smallShip.laserColor;
                }
            }

            //This loads the turret particle system   
            LoadLargeLaserParticleSystem(turret);
            LoadSmallLaserParticleSystem(turret);
            
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

            turret.turretSetUp = true;
        }
    }

    //This loads the turret particle system
    public static void LoadLargeLaserParticleSystem(LaserTurret turret)
    {
        Scene scene = SceneFunctions.GetScene(); //This gets the scene reference

        //This sets the turret to fire large lasers
        GameObject largeLaser = Resources.Load(OGGetAddress.particles + "models/laser_turbo") as GameObject;
        turret.largeLaserMesh = largeLaser.GetComponent<MeshFilter>().sharedMesh;
        turret.mode = "large";

        //This sets the laser colour material
        Material redLaserMaterial = Resources.Load(OGGetAddress.particles + "materials/laser_material_red") as Material;
        Material greenLaserMaterial = Resources.Load(OGGetAddress.particles + "materials/laser_material_green") as Material;
        Material yellowLaserMaterial = Resources.Load(OGGetAddress.particles + "materials/laser_material_yellow") as Material;

        //This set the laser colour light
        GameObject redLaserLight = Resources.Load(OGGetAddress.particles + "lights/laser_light_red") as GameObject;
        GameObject greenLaserLight = Resources.Load(OGGetAddress.particles + "lights/laser_light_green") as GameObject;
        GameObject yellowLaserLight = Resources.Load(OGGetAddress.particles + "lights/laser_light_yellow") as GameObject;

        //This loads the particle system, particle system renderer and the particle collider
        GameObject particleSystemGO = new GameObject();
        particleSystemGO.name = "largelaserparticlesystem_" + turret.gameObject.name;
        ParticleSystem particleSystem = particleSystemGO.AddComponent<ParticleSystem>();
        ParticleSystemRenderer particleSystemRenderer = particleSystemGO.GetComponent<ParticleSystemRenderer>();
        OnLaserTurretHit onLaserTurretHit = particleSystemGO.AddComponent<OnLaserTurretHit>();
        onLaserTurretHit.particleSystemScript = particleSystem;
        onLaserTurretHit.laserTurret = turret;
        
        //This adds the new particle system to the pool
        if (scene != null)
        {
            if (scene.lasersPool == null)
            {
                scene.lasersPool = new List<GameObject>();
            }

            scene.lasersPool.Add(turret.largeParticleSystemGO);
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
        particleSystemRenderer.mesh = turret.largeLaserMesh;

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

        turret.largeParticleSystemGO = particleSystemGO;
        turret.largeParticleSystem = particleSystem;
        turret.largeParticleSystemRenderer = particleSystemRenderer;
    }

    //This loads the turret particle system
    public static void LoadSmallLaserParticleSystem(LaserTurret turret)
    {
        Scene scene = SceneFunctions.GetScene(); //This gets the scene reference

        //This loads the laser mesh
        GameObject smallLaser = Resources.Load(OGGetAddress.particles + "models/laser") as GameObject;
        turret.smallLaserMesh = smallLaser.GetComponent<MeshFilter>().sharedMesh;
        turret.mode = "large";

        //This sets the laser colour material
        Material redLaserMaterial = Resources.Load(OGGetAddress.particles + "materials/laser_material_red") as Material;
        Material greenLaserMaterial = Resources.Load(OGGetAddress.particles + "materials/laser_material_green") as Material;
        Material yellowLaserMaterial = Resources.Load(OGGetAddress.particles + "materials/laser_material_yellow") as Material;

        //This set the laser colour light
        GameObject redLaserLight = Resources.Load(OGGetAddress.particles + "lights/laser_light_red") as GameObject;
        GameObject greenLaserLight = Resources.Load(OGGetAddress.particles + "lights/laser_light_green") as GameObject;
        GameObject yellowLaserLight = Resources.Load(OGGetAddress.particles + "lights/laser_light_yellow") as GameObject;

        //This loads the particle system and the particle collider
        GameObject particleSystemGO = new GameObject();
        particleSystemGO.name = "smalllaserparticlesystem_" + turret.gameObject.name;
        ParticleSystem particleSystem = particleSystemGO.AddComponent<ParticleSystem>();
        ParticleSystemRenderer particleSystemRenderer = particleSystemGO.GetComponent<ParticleSystemRenderer>();
        OnLaserTurretHit onLaserTurretHit = particleSystemGO.AddComponent<OnLaserTurretHit>();
        onLaserTurretHit.particleSystemScript = particleSystem;
        onLaserTurretHit.laserTurret = turret;

        //This adds the new particle system to the pool
        if (scene != null)
        {
            if (scene.lasersPool == null)
            {
                scene.lasersPool = new List<GameObject>();
            }

            scene.lasersPool.Add(turret.largeParticleSystemGO);
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
        particleSystemRenderer.mesh = turret.smallLaserMesh;

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

        turret.smallParticleSystemGO = particleSystemGO;
        turret.smallParticleSystem = particleSystem;
        turret.smallParticleSystemRenderer = particleSystemRenderer;
    }

    //This sets the collision layer for the lasers
    public static LayerMask SetLaserCollisionLayers(LaserTurret turret)
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

    #endregion

    #region main turret function

    //This function cycles through avalaible turrets and fire lasers
    public static IEnumerator RunTurrets(LaserTurret turret)
    {
        //This cycles through all the turrets and fires them
        if (turret.turretPositions != null & turret.turretGO != null)
        {
            foreach (Transform turretPosition in  turret.turretPositions)
            {
                //This gets the target
                if (turret.largeShip != null)
                {
                    turret.targetGO = turret.largeShip.target;
                }
                else if (turret.smallShip != null)
                {
                    turret.targetGO = turret.smallShip.target;
                }

                //This gets key references
                GameObject turretGO = turret.turretGO;
                GameObject targetGO = turret.targetGO;
                GameObject shipGO = turret.shipGO;
                Audio audioManager = turret.audioManager;
                string audioFile = turret.audioFile;

                //This selects the particle system
                ParticleSystem particleSystem = turret.largeParticleSystem;

                if (turretPosition.name.Contains("small"))
                {
                    particleSystem = turret.smallParticleSystem;
                }
               
                if (turret.targetGO != null)
                {
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
                    foreach (Vector3 firePoint in firePoints)
                    {
                        FireTurret(turretGO, particleSystem, firePoint, audioManager, audioFile);
                        yield return null;
                    }

                }
            }
        }

        yield return new WaitForSeconds(5);
    }

    #endregion

    #region targetting functions

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

        if (firePointsNo > 1)
        {
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
        }
        else
        {
            Vector3 firePoint01 = turret.transform.position;
            firePoints.Add(firePoint01);
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

    #region collision functions

    public static void RunCollisionEvent(GameObject objectHit, List<ParticleCollisionEvent> collisionEvents, ParticleSystem particleSystemScript, LaserTurret laserTurret)
    {
        //Get collision information
        List<Vector3> hitPositions = new List<Vector3>();

        int events = particleSystemScript.GetCollisionEvents(objectHit, collisionEvents); //This grabs all the collision events

        for (int i = 0; i < events; i++) //This cycles through all the collision events and deals with one at a time
        {
            Vector3 hitPosition = collisionEvents[i].intersection; //This gets the position of the collision event

            GameObject objectHitParent = ReturnParent(objectHit); //This gets the colliders object parent  

            if (objectHitParent != laserTurret.gameObject)
            {
                //This gets key information on the object hit
                var objectHitDetails = LaserTurretFunctions.ObjectHitDetails(objectHit, hitPosition);

                float shieldFront = objectHitDetails.shieldFront;
                float shieldBack = objectHitDetails.shieldBack;
                float forward = objectHitDetails.forward;
                bool hasPlasma = objectHitDetails.hasPlasma;

                Audio audioManager = GameObject.FindFirstObjectByType<Audio>();

                //This instantiates an explosion at the hit position
                LaserTurretFunctions.InstantiateLaserExplosion(laserTurret.gameObject, objectHit, hitPosition, forward, shieldFront, shieldBack, laserTurret.mode, laserTurret.laserColor, hasPlasma, audioManager);

                //This applies damage to the target
                ApplyDamage(laserTurret, objectHit, hitPosition);
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
    public static void InstantiateLaserExplosion(GameObject turretGO, GameObject objectHit, Vector3 hitPosition, float forward, float shieldFront, float shieldBack, string mode, string laserColor, bool hasPlasma, Audio audioManager)
    {
        //This selects the correct explosion colour
        string explosionChoice = "laserblast_red";

        if (forward > 0 & shieldFront > 0 || forward < 0 & shieldBack > 0)
        {
            if (hasPlasma == true)
            {
                explosionChoice = "blackhole";
            }
            if (laserColor == "red")
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
        if (mode == "large")
        {
            ParticleFunctions.InstantiateExplosion(objectHit, hitPosition, explosionChoice, 100, audioManager);
        }
        else
        {
            ParticleFunctions.InstantiateExplosion(objectHit, hitPosition, explosionChoice, 6, audioManager);
        }
    }

    //This calculates and applies damage to the 
    public static void ApplyDamage(LaserTurret laserTurret, GameObject objectHit, Vector3 hitPosition)
    {
        SmallShip smallShip = objectHit.gameObject.GetComponentInParent<SmallShip>();
        LargeShip largeShip = objectHit.gameObject.GetComponentInParent<LargeShip>();

        float damage = 0;

        if (laserTurret.mode == "large")
        {
            damage = laserTurret.largeTurretDamage;
        }
        else
        {
            damage = laserTurret.smallTurretDamage;
        }

        if (smallShip != null)
        {
            DamageFunctions.TakeDamage_SmallShip(smallShip, damage, hitPosition, false);
        }

        if (largeShip != null)
        {
            DamageFunctions.TakeDamage_LargeShip(largeShip, damage, hitPosition);
        }
    }

    #endregion
}
