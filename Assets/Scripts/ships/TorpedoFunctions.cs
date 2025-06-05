using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TorpedoFunctions
{
    #region start functions

    //This finds all torpedo launchers on the ship
    public static void GetTorpedoTubes(SmallShip smallShip)
    {
        Transform tube1 = smallShip.gameObject.transform.Find("missilebank01/missilebank01-01");
        Transform tube2 = smallShip.gameObject.transform.Find("missilebank01/missilebank01-02");
        Transform tube3 = smallShip.gameObject.transform.Find("missilebank01/missilebank01-03");
        Transform tube4 = smallShip.gameObject.transform.Find("missilebank01/missilebank01-04");

        if (tube1 != null)
        {
            smallShip.torpedoTube1 = tube1.gameObject;
        }

        if (tube2 != null)
        {
            smallShip.torpedoTube2 = tube2.gameObject;
        }

        if (tube3 != null)
        {
            smallShip.torpedoTube3 = tube3.gameObject;
        }
        else
        {
            tube3 = smallShip.gameObject.transform.Find("missilebank02/missilebank02-01");

            if (tube3 != null)
            {
                smallShip.torpedoTube3 = tube3.gameObject;
            }
        }

        if (tube4 != null)
        {
            smallShip.torpedoTube4 = tube4.gameObject;
        }
        else
        {
            tube4 = smallShip.gameObject.transform.Find("missilebank02/missilebank02-02");

            if (tube4 != null)
            {
                smallShip.torpedoTube4 = tube4.gameObject;
            }
        }

        if (tube1 == null & tube2 == null & tube3 == null & tube4 == null)
        {
            smallShip.hasTorpedos = false;
        }
        else
        {
            smallShip.hasTorpedos = true;
        }

    }

    #endregion

    #region fire torpedo

    //This locks onto the ship
    public static void EstablishLockOn(SmallShip smallShip)
    {
        if (smallShip.target != null & smallShip.activeWeapon == "torpedos" & smallShip.torpedoNumber > 0)
        {
            if (smallShip.target.activeSelf == true)
            {
                if (smallShip.targetForward > 0.99f & smallShip.targetDistance < 4000)
                {

                    if (smallShip.torpedoLockingOn != true)
                    {
                        smallShip.torpedoLockOnTime = Time.time + 5;
                        smallShip.torpedoLockingOn = true;
                    }

                    if (smallShip.torpedoLockOnTime < Time.time)
                    {
                        smallShip.torpedoLockedOn = true;
                    }

                }
                else
                {
                    smallShip.torpedoLockingOn = false;
                    smallShip.torpedoLockedOn = false;
                }
            }
            else
            {
                smallShip.torpedoLockingOn = false;
                smallShip.torpedoLockedOn = false;
            }
        }
        else
        {
            smallShip.torpedoLockingOn = false;
            smallShip.torpedoLockedOn = false;
        }
    }

    //This causes the player to fire a torpedo
    public static void FireTorpedoPlayer(SmallShip smallShip)
    {
        if (smallShip.fireWeapon == true & smallShip.isAI == false)
        {
            FireTorpedo(smallShip);
        }
    }

    //This fires a torpedo
    public static void FireTorpedo(SmallShip smallShip)
    {
        if (smallShip.hasTorpedos == true & smallShip.torpedoNumber > 0 & smallShip.activeWeapon == "torpedos" & smallShip.torpedoPressedTime < Time.time & smallShip.weaponsLock == false)
        {
            float spatialBlend = 1;
            string mixer = "External";

            if (smallShip.isAI == false)
            {
                spatialBlend = 0;
                mixer = "Cockpit";
            }

            List<GameObject> torpedoTubes = new List<GameObject>();

            if (smallShip.torpedoTube1 != null)
            {
                torpedoTubes.Add(smallShip.torpedoTube1);
            }

            if (smallShip.torpedoTube2 != null)
            {
                torpedoTubes.Add(smallShip.torpedoTube2);
            }

            if (smallShip.torpedoTube3 != null)
            {
                torpedoTubes.Add(smallShip.torpedoTube3);
            }

            if (smallShip.torpedoTube4 != null)
            {
                torpedoTubes.Add(smallShip.torpedoTube4);
            }

            if (smallShip.weaponMode == "single")
            {
                if (smallShip.torpedoCycleNumber + 1 > torpedoTubes.Count) { smallShip.torpedoCycleNumber = 0; }
                Torpedo torpedo = CreateTorpedo(smallShip, smallShip.target, torpedoTubes[smallShip.torpedoCycleNumber].transform.position);

                if (torpedo != null)
                {
                    AudioFunctions.PlayAudioClip(smallShip.audioManager, torpedo.launchAudio, mixer, torpedoTubes[smallShip.torpedoCycleNumber].transform.position, spatialBlend, 1, 500, 0.9f);
                }

                smallShip.torpedoCycleNumber += 1;

                smallShip.torpedoNumber -= 1;

            }
            else if (smallShip.weaponMode == "dual")
            {
                if (smallShip.torpedoCycleNumber + 1 > torpedoTubes.Count) { smallShip.torpedoCycleNumber = 0; }
                Torpedo torpedo01 = CreateTorpedo(smallShip, smallShip.target, torpedoTubes[smallShip.torpedoCycleNumber].transform.position);

                if (torpedo01 != null)
                {
                    AudioFunctions.PlayAudioClip(smallShip.audioManager, torpedo01.launchAudio, mixer, torpedoTubes[smallShip.torpedoCycleNumber].transform.position, spatialBlend, 1, 500, 0.9f);
                }

                smallShip.torpedoCycleNumber += 1;

                if (smallShip.torpedoCycleNumber + 1 > torpedoTubes.Count) { smallShip.torpedoCycleNumber = 0; }
                Torpedo torpedo02 = CreateTorpedo(smallShip, smallShip.target, torpedoTubes[smallShip.torpedoCycleNumber].transform.position);

                if (torpedo02 != null)
                {
                    AudioFunctions.PlayAudioClip(smallShip.audioManager, torpedo02.launchAudio, mixer, torpedoTubes[smallShip.torpedoCycleNumber].transform.position, spatialBlend, 1, 500, 0.9f);
                }

                smallShip.torpedoCycleNumber += 1;

                smallShip.torpedoNumber -= 2;

            }
            else if (smallShip.weaponMode == "all")
            {
                if (smallShip.torpedoCycleNumber + 1 > torpedoTubes.Count) { smallShip.torpedoCycleNumber = 0; }
                Torpedo torpedo01 = CreateTorpedo(smallShip, smallShip.target, torpedoTubes[smallShip.torpedoCycleNumber].transform.position);

                if (torpedo01 != null)
                {
                    AudioFunctions.PlayAudioClip(smallShip.audioManager, torpedo01.launchAudio, mixer, torpedoTubes[smallShip.torpedoCycleNumber].transform.position, spatialBlend, 1, 500, 0.9f);
                }

                smallShip.torpedoCycleNumber += 1;

                if (smallShip.torpedoCycleNumber + 1 > torpedoTubes.Count) { smallShip.torpedoCycleNumber = 0; }
                Torpedo torpedo02 = CreateTorpedo(smallShip, smallShip.target, torpedoTubes[smallShip.torpedoCycleNumber].transform.position);

                if (torpedo02 != null)
                {
                    AudioFunctions.PlayAudioClip(smallShip.audioManager, torpedo02.launchAudio, mixer, torpedoTubes[smallShip.torpedoCycleNumber].transform.position, spatialBlend, 1, 500, 0.9f);
                }

                smallShip.torpedoCycleNumber += 1;

                if (smallShip.torpedoCycleNumber + 1 > torpedoTubes.Count) { smallShip.torpedoCycleNumber = 0; }
                Torpedo torpedo03 = CreateTorpedo(smallShip, smallShip.target, torpedoTubes[smallShip.torpedoCycleNumber].transform.position);

                if (torpedo03 != null)
                {
                    AudioFunctions.PlayAudioClip(smallShip.audioManager, torpedo03.launchAudio, mixer, torpedoTubes[smallShip.torpedoCycleNumber].transform.position, spatialBlend, 1, 500, 0.9f);
                }

                smallShip.torpedoCycleNumber += 1;

                if (smallShip.torpedoCycleNumber + 1 > torpedoTubes.Count) { smallShip.torpedoCycleNumber = 0; }
                Torpedo torpedo04 = CreateTorpedo(smallShip, smallShip.target, torpedoTubes[smallShip.torpedoCycleNumber].transform.position);

                if (torpedo04 != null)
                {
                    AudioFunctions.PlayAudioClip(smallShip.audioManager, torpedo04.launchAudio, mixer, torpedoTubes[smallShip.torpedoCycleNumber].transform.position, spatialBlend, 1, 500, 0.9f);

                }
                smallShip.torpedoCycleNumber += 1;

                smallShip.torpedoNumber -= 4;

            }

            //This shakes the camera
            Task a = new Task(CockpitFunctions.ActivateCockpitShake(smallShip, 0.5f));

            smallShip.torpedoPressedTime = Time.time + 2.5f;
        }
    }

    #endregion

    #region create torpedo

    //This creates a new torpedo either by grabbing an inactive torpedo from the pool of the same type or instantiating a new one if none are availilble in the pool
    public static Torpedo CreateTorpedo(SmallShip smallShip, GameObject target, Vector3 position)
    {
        Torpedo torpedoScript = null;
        Scene scene = smallShip.scene;

        if (scene != null)
        {
            if (scene.torpedosPool == null)
            {
                scene.torpedosPool = new List<GameObject>();
            }
        }

        //This grabs the torpedo from the torpedo pool if availible
        GameObject torpedo = null;

        if (torpedo != null)
        {
            torpedoScript = torpedo.GetComponent<Torpedo>();
            torpedoScript.firingShip = smallShip;
            torpedoScript.destroyAfter = Time.time + 30;
            torpedoScript.fireTime = Time.time;
            torpedoScript.target = null;
            torpedoScript.target = smallShip.target;
            torpedoScript.torpedoRigidbody.linearVelocity = new Vector3(0f, 0f, 0f);
            torpedoScript.torpedoRigidbody.angularVelocity = new Vector3(0f, 0f, 0f);
            torpedoScript.audioManager = smallShip.audioManager;
            torpedo.transform.position = position;
            torpedo.transform.rotation = smallShip.transform.rotation;
            torpedo.transform.SetParent(smallShip.scene.transform);
            torpedo.SetActive(true);
        }

        //This instantiates a brand new torpedo if there are none in the scene
        if (torpedo == null)
        {
            //This gets the Json torpedo data
            TextAsset torpedoTypesFile = Resources.Load(OGGetAddress.files + "TorpedoTypes") as TextAsset;
            TorpedoTypes torpedoTypes = JsonUtility.FromJson<TorpedoTypes>(torpedoTypesFile.text);

            TorpedoType torpedoType = null;

            foreach (TorpedoType tempTorpedoType in torpedoTypes.torpedoTypeData)
            {
                if (tempTorpedoType.name == smallShip.torpedoType)
                {
                    torpedoType = tempTorpedoType;
                    break;
                }
            }

            if (torpedoType != null)
            {
                torpedo = SceneFunctions.InstantiateShipPrefab(torpedoType.prefab);
                torpedo.name = torpedoType.name;
                torpedo.layer = smallShip.gameObject.layer;
                GameObjectUtils.SetLayerAllChildren(torpedo.transform, 25);

                torpedoScript = torpedo.AddComponent<Torpedo>();
                torpedoScript.type = torpedoType.name;

                torpedoScript.pitchSpeed = (120 / 100f) * torpedoType.maneuverabilityRating;
                torpedoScript.turnSpeed = (100 / 100f) * torpedoType.maneuverabilityRating;
                torpedoScript.rollSpeed = (160 / 100f) * torpedoType.maneuverabilityRating;
                torpedoScript.thrustSpeed = torpedoType.speedRating;
                torpedoScript.damagePower = torpedoType.damageRating;
                torpedoScript.explosionAudio = torpedoType.explosionAudio;
                torpedoScript.launchAudio = torpedoType.launchAudio;
                torpedoScript.audioManager = smallShip.audioManager;
                AttachParticleTrail(smallShip, torpedoScript, torpedoType.trailColor);

                if (smallShip.torpedoLockedOn == true)
                {
                    torpedoScript.target = target;
                }

                torpedoScript.firingShip = smallShip;
                torpedoScript.destroyAfter = Time.time + 30;

                GameObjectUtils.AddColliders(torpedo, true);
                torpedoScript.torpedoRigidbody = GameObjectUtils.AddRigidbody(torpedo, 100f, 9f, 7.5f);
                torpedoScript.torpedoRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

                torpedo.transform.position = position;
                torpedo.transform.rotation = smallShip.transform.rotation;
                torpedo.transform.SetParent(smallShip.scene.transform);

                scene.torpedosPool.Add(torpedo);
            }
        }

        return torpedoScript;
    }

    //This attaches the particle trail to the torpedo
    public static void AttachParticleTrail(SmallShip smallShip, Torpedo torpedo, string color)
    {
        Object trailObject = PoolUtils.FindPrefabObjectInPool(smallShip.scene.particlePrefabPool, color);

        if (trailObject != null)
        {
            GameObject trail = GameObject.Instantiate(trailObject) as GameObject;
            trail.transform.SetParent(torpedo.gameObject.transform);
            trail.transform.localPosition = new Vector3(0, 0, 0.5f);
            trail.transform.localScale = new Vector3(50, 50, 50);
        }
    }

    //This causes the torpedo to ignore the firing ship collider and other torpedo colliders
    public static void IgnoreColliders(Torpedo torpedo)
    {
        Physics.IgnoreLayerCollision(25, 25, true);

        Collider torpedoCollider = torpedo.gameObject.GetComponentInChildren<Collider>();

        foreach (Collider collider in torpedo.firingShip.colliders)
        {
            Physics.IgnoreCollision(collider, torpedoCollider, true);
        }

    }

    #endregion

    #region toggle weapon mode

    //This toggles the ships weapon mode for torpedos
    public static void ToggleWeaponMode(SmallShip smallShip)
    {
        if (smallShip.toggleWeaponNumber == true & Time.time > smallShip.laserModePressedTime & smallShip.activeWeapon == "torpedos")
        {
            if (smallShip.weaponMode == "single" & smallShip.torpedoTube2 != null & smallShip.torpedoNumber > 1)
            {
                smallShip.weaponMode = "dual";
            }
            else if (smallShip.weaponMode == "dual" & smallShip.torpedoTube3 != null & smallShip.torpedoTube4 != null & smallShip.torpedoNumber > 3)
            {
                smallShip.weaponMode = "all";
            }
            else
            {
                smallShip.weaponMode = "single";
            }

            smallShip.laserModePressedTime = Time.time + 0.2f;

            if (smallShip.isAI == false)
            {
                AudioFunctions.PlayAudioClip(smallShip.audioManager, "beep01_toggle", "Cockpit", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);
            }

        }
    }

    #endregion

    #region torpedo input

    //This gets target information
    public static void GetTargetInfo(Torpedo torpedo)
    {
        if (torpedo.target != null)
        {
            if (torpedo.target.activeSelf != false & torpedo.fireTime + 2 < Time.time)
            {
                Transform torpedoTransform = torpedo.transform;
                Vector3 targetPosition = torpedo.target.transform.position;
                Vector3 targetRelativePosition = targetPosition - torpedoTransform.position;

                torpedo.targetForward = Vector3.Dot(torpedoTransform.forward, targetRelativePosition.normalized);
                torpedo.targetRight = Vector3.Dot(torpedoTransform.right, targetRelativePosition.normalized);
                torpedo.targetUp = Vector3.Dot(torpedoTransform.up, targetRelativePosition.normalized);
            }
        }
    }

    //This angles the torpedo toward the target if lock on was achieved
    public static void AngleTowardsTarget(Torpedo torpedo)
    {

        bool flyStraight = false;

        if (torpedo.target != null)
        {
            if (torpedo.target.activeSelf != false)
            {
                if (torpedo.targetForward < 0.8)
                {
                    SmoothTurnInput(torpedo, torpedo.targetRight);
                    SmoothPitchInput(torpedo, -torpedo.targetUp);
                }
                else
                {
                    SmoothTurnInput(torpedo, torpedo.targetRight * 5);
                    SmoothPitchInput(torpedo, -torpedo.targetUp * 5);
                }
            }
            else
            {
                flyStraight = true;
            }
        }
        else
        {
            flyStraight = true;
        }

        if (flyStraight == true)
        {
            SmoothTurnInput(torpedo, 0);
            SmoothPitchInput(torpedo, 0);
        }

    }

    //This smoothly transitions between different pitch, turn, and roll inputs by lerping between different values like the ai is using a joystick or controller
    public static void SmoothPitchInput(Torpedo torpedo, float pitchInput)
    {
        float step = +Time.deltaTime / 0.01f;
        torpedo.pitchInput = Mathf.Lerp(torpedo.pitchInput, pitchInput, step);
    }

    public static void SmoothTurnInput(Torpedo torpedo, float turnInput)
    {
        float step = +Time.deltaTime / 0.01f;
        torpedo.turnInput = Mathf.Lerp(torpedo.turnInput, turnInput, step);
    }

    public static void SmoothRollInput(Torpedo torpedo, float rollInput)
    {
        float step = +Time.deltaTime / 0.01f;
        torpedo.rollInput = Mathf.Lerp(torpedo.rollInput, rollInput, step);
    }

    #endregion

    #region torpedo movement

    //This moves and turns the torpedo
    public static void TorpedoMove(Torpedo torpedo)
    {
        if (torpedo.torpedoRigidbody != null)
        {
            //This rotates the ship
            Vector3 x = Vector3.right * torpedo.pitchSpeed * torpedo.pitchInput;
            Vector3 y = Vector3.up * torpedo.turnSpeed * torpedo.turnInput;
            Vector3 z = Vector3.forward * torpedo.rollSpeed * torpedo.rollInput;

            Vector3 rotationVector = x + y + z;

            Quaternion deltaRotation = Quaternion.Euler(rotationVector * Time.deltaTime);
            torpedo.torpedoRigidbody.MoveRotation(torpedo.torpedoRigidbody.rotation * deltaRotation);

            //This adds makes the ship move forward
            torpedo.torpedoRigidbody.AddForce(torpedo.gameObject.transform.position + torpedo.gameObject.transform.forward * Time.fixedDeltaTime * torpedo.thrustSpeed * 60000);
        }     
    }

    #endregion

    #region damage

    //This causes the ship to take damage from torpedos
    public static void CauseTorpedoDamage(SmallShip firingShip, GameObject other, Torpedo torpedo, Vector3 hitPosition)
    {
        SmallShip smallShip = other.GetComponentInParent<SmallShip>();
        LargeShip largeShip = other.GetComponentInParent<LargeShip>();

        if (Time.time > 10)
        {
            if (smallShip != null)
            {
                if (smallShip.gameObject != firingShip.gameObject)
                {
                    SmallShipFunctions.TakeDamage(smallShip, torpedo.damagePower, hitPosition);
                    Task a = new Task(SmallShipFunctions.ShipSpinSequence(smallShip, 2));
                }
            }
            else if (largeShip != null)
            {
                LargeShipFunctions.TakeDamage(largeShip, torpedo.damagePower * 4, hitPosition);
            }
        }
    }

    //Destroy close to target
    public static void DestroyCloseToTarget(Torpedo torpedo)
    {
        if (torpedo.target != null)
        {
            float distance = Vector3.Distance(torpedo.gameObject.transform.position, torpedo.target.transform.position);

            if (distance < 25)
            {
                CauseTorpedoDamage(torpedo.firingShip, torpedo.target, torpedo, torpedo.transform.position);

                ParticleFunctions.InstantiateExplosion(torpedo.target, torpedo.transform.position, "explosion_torpedo", 3f, torpedo.audioManager, "mid_explosion_02", 1500, "Explosions");
                DeactivateTorpedo(torpedo);
            }
        }
    }

    //Activate explosion after 30 seconds 
    public static void DestroyAfterTime(Torpedo torpedo)
    {
        if (Time.time > torpedo.destroyAfter)
        {
            DeactivateTorpedo(torpedo);
        }
    }

    //This causes a torpedo to explode on impact, deactivates and puts it pack in the torpedo pool
    public static void DeactivateTorpedo(Torpedo torpedo)
    {
        torpedo.gameObject.SetActive(false);
    }

    #endregion
}
