using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public static class PlasmaTurretFunctions
{
    #region turret set up

    //This sets up the turrets
    public static void SetUpTurrets(PlasmaTurret turret)
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
                    turret.largeTurretAccuracy = turret.largeShip.aiTargetingMode;
                    turret.largeShip.hasPlasma = true;
                }
                else if (turret.smallShip != null)
                {
                    turret.allegiance = turret.smallShip.allegiance;
                    turret.largeTurretAccuracy = turret.smallShip.aiTargetingMode;
                    turret.smallShip.hasPlasma = true;
                }
            }

            //This loads the turret particle system   
            LoadLargePlasmaParticleSystem(turret);
            LoadSmallLaserParticleSystem(turret);
            
            //This creates the turret GO for calculating rotation and position
            if (turret.turretGO == null)
            {
                turret.turretGO = new GameObject();
                turret.turretGO.name = "plasmaturretBase_" + turret.shipGO.name;
            }

            //This gets all the current turret positions
            if (turret.turretPositions == null)
            {
                turret.turretPositions = GameObjectUtils.FindAllChildTransformsContaining(turret.shipGO.transform, "plasmaturret", "plasmaturrettransforms", "plasmaturretBase");
            }

            turret.turretSetUp = true;
        }
    }

    //This loads the turret particle system
    public static void LoadLargePlasmaParticleSystem(PlasmaTurret turret)
    {
        Scene scene = SceneFunctions.GetScene(); //This gets the scene reference

        //This sets the turret to fire large lasers
        GameObject largeLaser = Resources.Load(OGGetAddress.particles + "models/plasma_turbo") as GameObject;
        turret.largeLaserMesh = largeLaser.GetComponent<MeshFilter>().sharedMesh;
        turret.plasmaSize = "large";

        //This sets the laser colour material
        Material yellowLaserMaterial = Resources.Load(OGGetAddress.particles + "materials/laser_material_yellow") as Material;

        //This set the laser colour light
        GameObject yellowLaserLight = Resources.Load(OGGetAddress.particles + "lights/laser_light_yellow") as GameObject;

        //This loads the particle system, particle system renderer and the particle collider
        GameObject particleSystemGO = new GameObject();
        particleSystemGO.name = "largeplasmaparticlesystem_" + turret.gameObject.name;
        ParticleSystem particleSystem = particleSystemGO.AddComponent<ParticleSystem>();
        ParticleSystemRenderer particleSystemRenderer = particleSystemGO.GetComponent<ParticleSystemRenderer>();
        OnPlasmaTurretHit onPlasmaTurretHit = particleSystemGO.AddComponent<OnPlasmaTurretHit>();
        onPlasmaTurretHit.particleSystemScript = particleSystem;
        onPlasmaTurretHit.plasmaTurret = turret;
        
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

        particleSystemRenderer.material = yellowLaserMaterial;
        lights.light = yellowLaserLight.GetComponent<Light>();
        
        //This prevents the particle system playing when loaded
        particleSystem.Stop();

        turret.largeParticleSystemGO = particleSystemGO;
        turret.largeParticleSystem = particleSystem;
        turret.largeParticleSystemRenderer = particleSystemRenderer;
    }

    //This loads the turret particle system
    public static void LoadSmallLaserParticleSystem(PlasmaTurret turret)
    {
        Scene scene = SceneFunctions.GetScene(); //This gets the scene reference

        //This loads the laser mesh
        GameObject smallLaser = Resources.Load(OGGetAddress.particles + "models/plasma") as GameObject;
        turret.smallLaserMesh = smallLaser.GetComponent<MeshFilter>().sharedMesh;
        turret.plasmaSize = "small";

        //This sets the laser colour material
        Material yellowLaserMaterial = Resources.Load(OGGetAddress.particles + "materials/laser_material_yellow") as Material;

        //This set the laser colour light
        GameObject yellowLaserLight = Resources.Load(OGGetAddress.particles + "lights/laser_light_yellow") as GameObject;

        //This loads the particle system and the particle collider
        GameObject particleSystemGO = new GameObject();
        particleSystemGO.name = "smallplasmaparticlesystem_" + turret.gameObject.name;
        ParticleSystem particleSystem = particleSystemGO.AddComponent<ParticleSystem>();
        ParticleSystemRenderer particleSystemRenderer = particleSystemGO.GetComponent<ParticleSystemRenderer>();
        OnPlasmaTurretHit onPlasmaTurretHit = particleSystemGO.AddComponent<OnPlasmaTurretHit>();
        onPlasmaTurretHit.particleSystemScript = particleSystem;
        onPlasmaTurretHit.plasmaTurret = turret;

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

        particleSystemRenderer.material = yellowLaserMaterial;
        lights.light = yellowLaserLight.GetComponent<Light>();
        
        //This prevents the particle system playing when loaded
        particleSystem.Stop();

        turret.smallParticleSystemGO = particleSystemGO;
        turret.smallParticleSystem = particleSystem;
        turret.smallParticleSystemRenderer = particleSystemRenderer;
    }

    //This sets the collision layer for the lasers
    public static LayerMask SetLaserCollisionLayers(PlasmaTurret turret)
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
    public static IEnumerator RunTurrets(PlasmaTurret turret)
    {
        //This cycles through all the turrets and fires them
        if (turret.turretPositions != null & turret.turretGO != null & turret != null)
        {
            foreach (Transform turretPosition in  turret.turretPositions)
            {
                if (turretPosition != null & turret != null)
                {
                    //The time delay
                    bool activateTurret = false;

                    if (turretPosition.name.Contains("small") & !turret.smallTargetingMode.Contains("noweapons"))
                    {
                        if (turret.smallTurretDelayActual < Time.time)
                        {
                            activateTurret = true;
                        }
                    }
                    else if (turretPosition.name.Contains("large") & !turret.smallTargetingMode.Contains("noweapons"))
                    {
                        if (turret.largeTurretDelayActual < Time.time)
                        {
                            activateTurret = true;
                        }
                    }

                    //Get current rotation as euler
                    Vector3 eulerRotation = turret.transform.eulerAngles;

                    if (activateTurret == true)
                    {
                        //This gets key references
                        GameObject turretGO = turret.turretGO;
                        GameObject shipGO = turret.shipGO;
                        Audio audioManager = turret.audioManager;
                        string audioFile = turret.audioFile;

                        //This selects the target
                        GameObject targetGO = null;

                        if (turretPosition.name.Contains("small"))
                        {
                            targetGO = GetTargetForSmallTurret(turret, turretPosition.gameObject);
                        }
                        else
                        {
                            targetGO = GetTargetForLargeTurret(turret, turretPosition.gameObject);
                        }

                        //This selects the particle system
                        ParticleSystem particleSystem = turret.largeParticleSystem;

                        if (turretPosition.name.Contains("small"))
                        {
                            particleSystem = turret.smallParticleSystem;
                        }

                        if (targetGO != null)
                        {
                            //This sets the accuracy of the turrets
                            string accuracy = "high";

                            if (turretPosition.name.Contains("small"))
                            {
                                accuracy = turret.smallTurretAccuracy;
                            }
                            else
                            {
                                accuracy = turret.largeTurretAccuracy;
                            }

                            PositionAndAimTurret(turretGO, turretPosition.gameObject, targetGO, shipGO, accuracy);

                            //This checks if the turret is clear to fire
                            bool clearToFire = ClearToFire(turretGO, targetGO, shipGO, turretPosition);

                            if (clearToFire == true)
                            {
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
                }
            }
        }

        if (turret != null)
        {
            if (turret.smallTurretDelayActual < Time.time)
            {
                turret.smallTurretDelayActual = Time.time + Random.Range(turret.smallTurretDelay, turret.smallTurretDelay + 1);
            }

            if (turret.largeTurretDelayActual < Time.time)
            {
                turret.largeTurretDelayActual = Time.time + Random.Range(turret.smallTurretDelay, turret.smallTurretDelay + 1);
            }
        }

        yield return new WaitForSeconds(1);
    }

    #endregion

    #region targetting functions

    //This selects a target for the a large turret
    public static GameObject GetTargetForLargeTurret(PlasmaTurret turret, GameObject turretPosition)
    {
        GameObject target = null;

        if (turret.largeTargetingMode == "large_singletarget_largeship")
        {
            if (turret.largeTargetGO == null)
            {
                target = TargetingFunctions.GetClosestEnemyLargeShip_Turret(turretPosition, turret.allegiance);
                turret.largeTargetGO = target;
            }
            else
            {
                target = turret.largeTargetGO;
            }
        }
        else if (turret.largeTargetingMode == "large_singletarget_smallship")
        {
            if (turret.largeTargetGO == null)
            {
                target = TargetingFunctions.GetClosestEnemySmallShip_Turret(turretPosition, turret.allegiance);
                turret.largeTargetGO = target;
            }
            else
            {
                target = turret.largeTargetGO;
            }
        }
        else if (turret.largeTargetingMode == "large_singletarget_all")
        {
            if (turret.largeTargetGO == null)
            {
                target = TargetingFunctions.GetClosestEnemyLargeShip_Turret(turretPosition, turret.allegiance);
                turret.largeTargetGO = target;

                if (target == null)
                {
                    target = TargetingFunctions.GetClosestEnemySmallShip_Turret(turretPosition, turret.allegiance);
                    turret.largeTargetGO = target;
                }
            }
        }
        else if (turret.largeTargetingMode == "large_multipletargets_largeship")
        {
            target = TargetingFunctions.GetClosestEnemyLargeShip_Turret(turretPosition, turret.allegiance);
        }
        else if (turret.largeTargetingMode == "large_multipletargets_smallship")
        {
            target = TargetingFunctions.GetClosestEnemySmallShip_Turret(turretPosition, turret.allegiance);
        }
        else if (turret.largeTargetingMode == "large_multipletargets_all")
        {
            target = TargetingFunctions.GetClosestEnemyLargeShip_Turret(turretPosition, turret.allegiance);

            if (target == null)
            {
                target = TargetingFunctions.GetClosestEnemySmallShip_Turret(turretPosition, turret.allegiance);
            }
        }
        else if (turret.largeTargetingMode == "large_shiptarget")
        {
            if (turret.smallShip != null)
            {
                target = turret.smallShip.target;
                turret.largeTargetGO = target;
            }
            else if (turret.largeShip != null)
            {
                target = turret.largeShip.target;
                turret.largeTargetGO = target;
            }
        }

        return target;
    }

    //This selects a target for a small turret
    public static GameObject GetTargetForSmallTurret(PlasmaTurret turret, GameObject turretPosition)
    {
        GameObject target = null;

        if (turret.smallTargetingMode == "small_singletarget_largeship")
        {
            if (turret.smallTargetGO == null)
            {
                target = TargetingFunctions.GetClosestEnemyLargeShip_Turret(turretPosition, turret.allegiance);
                turret.smallTargetGO = target;
            }
            else
            {
                target = turret.smallTargetGO;
            }
        }
        else if (turret.smallTargetingMode == "small_singletarget_smallship")
        {
            if (turret.smallTargetGO == null)
            {
                target = TargetingFunctions.GetClosestEnemySmallShip_Turret(turretPosition, turret.allegiance);
                target = turret.smallTargetGO;
            }
            else
            {
                target = turret.smallTargetGO;
            }
        }
        else if (turret.smallTargetingMode == "small_singletarget_all")
        {
            if (turret.smallTargetGO == null)
            {
                target = TargetingFunctions.GetClosestEnemyLargeShip_Turret(turretPosition, turret.allegiance);
                turret.smallTargetGO = target;

                if (target == null)
                {
                    target = TargetingFunctions.GetClosestEnemySmallShip_Turret(turretPosition, turret.allegiance);
                    turret.smallTargetGO = target;
                }
            }
        }
        else if (turret.smallTargetingMode == "small_multipletargets_largeship")
        {
            target = TargetingFunctions.GetClosestEnemyLargeShip_Turret(turretPosition, turret.allegiance);
        }
        else if (turret.smallTargetingMode == "small_multipletargets_smallship")
        {
            target = TargetingFunctions.GetClosestEnemySmallShip_Turret(turretPosition, turret.allegiance);
        }
        else if (turret.smallTargetingMode == "small_multipletargets_all")
        {
            target = TargetingFunctions.GetClosestEnemyLargeShip_Turret(turretPosition, turret.allegiance);

            if (target == null)
            {
                target = TargetingFunctions.GetClosestEnemySmallShip_Turret(turretPosition, turret.allegiance);
            }
        }
        else if (turret.smallTargetingMode == "small_shiptarget")
        {
            if (turret.smallShip != null)
            {
                target = turret.smallShip.target;
                turret.smallTargetGO = target;
            }
            else if (turret.largeShip != null)
            {
                target = turret.largeShip.target;
                turret.smallTargetGO = target;
            }
        }

        return target;
    }

    //This nullifies a large turret target
    public static void NullifyLargeTurretTarget(PlasmaTurret turret)
    {
        turret.largeTargetGO = null;
    }

    //This nullifies a small turret target
    public static void NullifySmallTurretTarget(PlasmaTurret turret)
    {
        turret.smallTargetGO = null;
    }

    //This positions and aims the turret
    public static void PositionAndAimTurret(GameObject turret, GameObject turretPosition, GameObject target, GameObject ship, string accuracy = "low")
    {
        if (target != null)
        {
            //This gets the target rigidbody for the intercept point calculation
            Rigidbody targetRigidBody = target.GetComponent<Rigidbody>();

            //This gets the intercept point
            Vector3 interceptPoint = GameObjectUtils.CalculateInterceptPoint(turret.transform.position, target.transform.position, targetRigidBody.linearVelocity, 750);

            //This sets the error margin for the shot
            Vector3 errorMargin = SetTargetingErrorMargin(accuracy);

            //This positions the turret
            turret.transform.position = turretPosition.transform.position;

            //This aims the turret
            turret.transform.LookAt(interceptPoint + errorMargin);
        }
    }

    //This sets the targetting error margin for the turret
    public static Vector3 SetTargetingErrorMargin(string accuracy = "low")
    {
        float low = 100;
        float medium = 50;
        float high = 25;
        float veryHigh = 5;

        float x = 0;
        float y = 0;
        float z = 0;

        if (accuracy != null)
        {
            if (accuracy.Contains("low"))
            {
                x = Random.Range(-low, low);
                y = Random.Range(-low, low);
                z = Random.Range(-low, low);
            }
            else if (accuracy.Contains("medium"))
            {
                x = Random.Range(-medium, medium);
                y = Random.Range(-medium, medium);
                z = Random.Range(-medium, medium);
            }
            else if (accuracy.Contains("high"))
            {
                x = Random.Range(-high, high);
                y = Random.Range(-high, high);
                z = Random.Range(-high, high);
            }
            else if (accuracy.Contains("veryhigh"))
            {
                x = Random.Range(-veryHigh, veryHigh);
                y = Random.Range(-veryHigh, veryHigh);
                z = Random.Range(-veryHigh, veryHigh);
            }
        }

        return (new Vector3(x, y, z));       
    }

    //This checks the target is in sight before firing
    public static bool ClearToFire(GameObject turret, GameObject target, GameObject shipGO, Transform turretPosition)
    {
        bool clearToFire = false;

        if (turret != null & target != null)
        {
            //This checks the target is in the sights of the cannon
            Vector3 targetRelativePosition = target.transform.position - turret.transform.position;

            float targetForward = Vector3.Dot(turret.transform.forward, targetRelativePosition.normalized);

            if (targetForward > 0.9)
            {
                clearToFire = true;
            }

            //This checks if the laser cannon is upside down and prevents it from firing if it is
            if (clearToFire == true)
            {
                Vector3 up1 = turret.transform.up;
                Vector3 up2 = turretPosition.up;

                float dotProduct = Vector3.Dot(up1, up2);

                if (dotProduct < 0)
                {
                    clearToFire = false;
                }
            }
            
            //This checks if the laser cannon is restricted to fire only forward and prevents it from firing if it is
            if (clearToFire == true)
            {
                bool restrict = false;

                if (turretPosition.name.Contains("restrict"))
                {
                    restrict = true;
                }

                if (restrict == true)
                {
                    Vector3 forward1 = turret.transform.forward;
                    Vector3 forward2 = turretPosition.forward;

                    // Calculate the dot product.
                    float dotProduct = Vector3.Dot(forward1, forward2);

                    if (dotProduct < 0)
                    {
                        clearToFire = false;
                    }
                }
            }
            
           
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

            Vector3 firePoint01 = turret.transform.position + (turret.transform.right * (currentPositionRight));
            Vector3 firePoint02 = turret.transform.position + (turret.transform.right * (currentPositionLeft));

            firePoints.Add(firePoint01);
            firePoints.Add(firePoint02);

            //This gets the rest of the firepoints
            for (int i = 1; i < firePointsNo; i = i + 2)
            {
                currentPositionRight = currentPositionRight + distanceBetweenFirePoints;
                currentPositionLeft = currentPositionLeft - distanceBetweenFirePoints;

                Vector3 firePointRight = turret.transform.position + (turret.transform.right * (currentPositionRight));
                Vector3 firePointLeft = turret.transform.position + (turret.transform.right * (currentPositionLeft));

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

    //This handles an event where the laser hits something
    public static void RunCollisionEvent(GameObject objectHit, List<ParticleCollisionEvent> collisionEvents, ParticleSystem particleSystemScript, PlasmaTurret laserTurret)
    {
        //Get collision information
        List<Vector3> hitPositions = new List<Vector3>();

        int events = particleSystemScript.GetCollisionEvents(objectHit, collisionEvents); //This grabs all the collision events

        for (int i = 0; i < events; i++) //This cycles through all the collision events and deals with one at a time
        {
            Vector3 hitPosition = collisionEvents[i].intersection; //This gets the position of the collision event

            GameObject objectHitParent = ReturnParent(objectHit); //This gets the colliders object parent  

            if (laserTurret != null & objectHitParent != null)
            {
                if (objectHitParent != laserTurret.gameObject)
                {
                    //This gets key information on the object hit
                    var objectHitDetails = ObjectHitDetails(objectHit, hitPosition);

                    float shieldFront = objectHitDetails.shieldFront;
                    float shieldBack = objectHitDetails.shieldBack;
                    float forward = objectHitDetails.forward;
                    bool hasPlasma = objectHitDetails.hasPlasma;

                    Audio audioManager = GameObject.FindFirstObjectByType<Audio>();

                    //This instantiates an explosion at the hit position
                    InstantiateLaserExplosion(laserTurret.gameObject, objectHit, hitPosition, forward, shieldFront, shieldBack, laserTurret.plasmaSize, hasPlasma, audioManager);

                    //This applies damage to the target
                    ApplyDamage(laserTurret, objectHit, hitPosition);
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
    public static void InstantiateLaserExplosion(GameObject turretGO, GameObject objectHit, Vector3 hitPosition, float forward, float shieldFront, float shieldBack, string mode, bool hasPlasma, Audio audioManager)
    {
        //This selects the correct explosion colour
        string explosionChoice = "laserblast_red";

        if (forward > 0 & shieldFront > 0 || forward < 0 & shieldBack > 0)
        {
            if (hasPlasma == true)
            {
                explosionChoice = "blackhole";
            }
            else
            {
                explosionChoice = "laserblast_yellow";
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
    public static void ApplyDamage(PlasmaTurret plasmaTurret, GameObject objectHit, Vector3 hitPosition)
    {
        SmallShip smallShip = objectHit.gameObject.GetComponentInParent<SmallShip>();
        LargeShip largeShip = objectHit.gameObject.GetComponentInParent<LargeShip>();

        float damage = 0;

        if (plasmaTurret.plasmaSize == "large")
        {
            damage = plasmaTurret.largeTurretDamage;
        }
        else
        {
            damage = plasmaTurret.smallTurretDamage;
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
