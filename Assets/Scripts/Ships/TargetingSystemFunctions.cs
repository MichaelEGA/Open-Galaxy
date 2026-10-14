using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TargetingSystemFunctions
{
    public static void GetInput(TargetingSystem targetingSystem)
    {
        if (targetingSystem.ship.isAI == false)
        {
            targetingSystem.getNextTarget = targetingSystem.ship.ogInput.getNextTarget;
            targetingSystem.getNextEnemy = targetingSystem.ship.ogInput.getNextEnemy;
            targetingSystem.getClosestEnemy = targetingSystem.ship.ogInput.getClosestEnemy;
            targetingSystem.selectTargetInFront = targetingSystem.ship.ogInput.selectTargetInFront;
        }
    }

    //This draws key data/info from the target including relative position, distance, and hostility
    public static void GetTargetInfo_SmallShip(TargetingSystem targetingSystem = null)
    {
        Transform shipTransform = targetingSystem.gameObject.transform;
        Vector3 shipPosition = shipTransform.position;

        if (targetingSystem.target != null)
        {
            if (targetingSystem.target.activeSelf == true)
            {
                if (targetingSystem.targetShip != null)
                {
                    targetingSystem.targetAllegiance = targetingSystem.targetShip.allegiance;
                    targetingSystem.targetSpeed = targetingSystem.targetShip.flightControlSystem_Small.thrustSpeed;
                    targetingSystem.targetHull = targetingSystem.targetShip.energyManagementSystem.hullLevel;
                    targetingSystem.targetShield = targetingSystem.targetShip.energyManagementSystem.shieldLevel;
                    targetingSystem.targetType = targetingSystem.targetShip.type;
                    targetingSystem.targetPrefabName = targetingSystem.targetShip.prefab;
                }

                if (targetingSystem.targetLargeShip != null)
                {
                    targetingSystem.targetAllegiance = targetingSystem.targetLargeShip.allegiance;
                    targetingSystem.targetSpeed = targetingSystem.targetLargeShip.thrustSpeed;
                    targetingSystem.targetHull = targetingSystem.targetLargeShip.hullLevel;
                    targetingSystem.targetShield = targetingSystem.targetLargeShip.shieldLevel;
                    targetingSystem.targetType = targetingSystem.targetLargeShip.type;
                    targetingSystem.targetPrefabName = targetingSystem.targetLargeShip.prefabName;
                }

                Transform targetTransform = targetingSystem.target.transform;
                Vector3 targetPosition = targetTransform.position;
                Vector3 targetVelocity = new Vector3(0, 0, 0);

                if (targetingSystem.targetRigidbody != null)
                {
                    targetVelocity = targetingSystem.targetRigidbody.linearVelocity;
                }

                Vector3 targetRelativePosition = targetPosition - shipPosition;

                targetingSystem.targetDistance = Vector3.Distance(shipPosition, targetPosition);
                targetingSystem.targetForward = Vector3.Dot(shipTransform.forward, targetRelativePosition.normalized);
                targetingSystem.targetRight = Vector3.Dot(shipTransform.right, targetRelativePosition.normalized);
                targetingSystem.targetUp = Vector3.Dot(shipTransform.up, targetRelativePosition.normalized);

                Vector3 targettingErrorMargin = targetingSystem.ship.flightControlAI_Small.aiTargetingErrorMargin;
                float distanceToIntercept = ((targetPosition) - shipPosition).magnitude / 750f;
                
                Vector3 interceptPosition = (targetPosition + targettingErrorMargin) + targetVelocity * distanceToIntercept;
                Vector3 interceptRelativePosition = interceptPosition - shipPosition;

                Vector3 currentVelocity = Vector3.zero;
                float smoothTime = 0.3f;

                targetingSystem.interceptPoint = Vector3.SmoothDamp(targetingSystem.interceptPoint, interceptRelativePosition, ref currentVelocity, smoothTime);
                targetingSystem.interceptDistance = Vector3.Distance(interceptPosition, shipPosition);
                targetingSystem.interceptForward = Vector3.Dot(shipTransform.forward, interceptRelativePosition.normalized);
                targetingSystem.interceptRight = Vector3.Dot(shipTransform.right, interceptRelativePosition.normalized);
                targetingSystem.interceptUp = Vector3.Dot(shipTransform.up, interceptRelativePosition.normalized);
            }
        }
        else
        {
            targetingSystem.interceptDistance = 250;
        }

        if (targetingSystem.waypoint != null)
        {
            Vector3 waypointPosition = targetingSystem.waypoint.transform.position;
            Vector3 waypointRelativePosition = waypointPosition - shipPosition;

            targetingSystem.waypointDistance = Vector3.Distance(shipPosition, waypointPosition);
            targetingSystem.waypointForward = Vector3.Dot(shipTransform.forward, waypointRelativePosition.normalized);
            targetingSystem.waypointRight = Vector3.Dot(shipTransform.right, waypointRelativePosition.normalized);
            targetingSystem.waypointUp = Vector3.Dot(shipTransform.up, waypointRelativePosition.normalized);
        }

    }

    #region player targetting

    //This runs all the player targetting functions
    public static void RunPlayerTargetingFunctions(TargetingSystem targetingSystem)
    {
        //Targetting functions
        GetClosestEnemy_SmallShipPlayer(targetingSystem);
        GetNextEnemy_SmallShipPlayer(targetingSystem);
        GetNextTarget_SmallShipPlayer(targetingSystem);
        GetTargetDirectlyAhead_SmallShipPlayer(targetingSystem);
    }

    //This gets the next target of any kind
    public static void GetNextTarget_SmallShipPlayer(TargetingSystem targetingSystem = null)
    {
        if (targetingSystem.ship.isAI == false)
        {
            bool automaticSearch = false;

            if (targetingSystem.target != null)
            {
                if (targetingSystem.target.activeSelf == false)
                {
                    automaticSearch = true;
                }
            }

            if (targetingSystem.targetPressedTime < Time.time & targetingSystem.getNextTarget == true || automaticSearch == true)
            {
                Scene scene = targetingSystem.ship.scene;
                int countStart = 0;

                //This sets the count start according to the current target the ship has selected
                countStart = GetNextTargetNo_SmallShipPlayer(targetingSystem);

                //This cycles through all the possible targets ignoring objects that are null or inactive
                for (int i = countStart; i <= scene.objectPool.Count; i++)
                {
                    if (i > scene.objectPool.Count - 1) //This clears the target at the end of the list
                    {
                        targetingSystem.target = null;
                        targetingSystem.targetName = " ";
                        targetingSystem.targetNumber = i;
                        targetingSystem.targetShip = null;
                        targetingSystem.targetRigidbody = null;
                        targetingSystem.targetPrefabName = " ";
                        break;
                    }
                    else if (scene.objectPool[i] != null & targetingSystem.targetNumber != i) //This gets any type of ship in the scene
                    {
                        if (scene.objectPool[i].activeSelf == true & scene.objectPool[i] != targetingSystem.gameObject) //This ignores objects that are inactive
                        {
                            targetingSystem.target = scene.objectPool[i];
                            targetingSystem.targetName = scene.objectPool[i].name;

                            Ship ship = scene.objectPool[i].GetComponent<Ship>();
                            LargeShip targetLargeShip = scene.objectPool[i].GetComponent<LargeShip>();

                            if (ship != null)
                            {
                                targetingSystem.targetShip = ship;
                                targetingSystem.targetLargeShip = null;
                                targetingSystem.targetPrefabName = ship.prefab;
                            }
                            else if (targetLargeShip != null)
                            {
                                targetingSystem.targetShip = null;
                                targetingSystem.targetLargeShip = targetLargeShip;
                                targetingSystem.targetPrefabName = targetLargeShip.prefabName;
                            }

                            targetingSystem.targetRigidbody = scene.objectPool[i].GetComponent<Rigidbody>();
                            targetingSystem.targetNumber = i;
                            break;
                        }
                    }
                }

                targetingSystem.targetPressedTime = Time.time + 0.2f;

                AudioFunctions.PlayAudioClip(targetingSystem.ship.audioManager, "beep01_toggle", "Cockpit", targetingSystem.gameObject.transform.position, 0, 1, 500, 1, 100);

                //This prevents the torpedo from immediately locking on to the new target
                targetingSystem.ship.torpedoSystem.torpedoLockedOn = false;
                targetingSystem.ship.torpedoSystem.torpedoLockingOn = false;
            }
        }
    }

    //This gets the next enemy target
    public static void GetNextEnemy_SmallShipPlayer(TargetingSystem targetingSystem = null, bool forceSearch = false)
    {
        if (targetingSystem.ship.isAI == false)
        {
            bool automaticSearch = false;

            if (targetingSystem.target != null)
            {
                if (targetingSystem.target.activeSelf == false)
                {
                    automaticSearch = true;
                }
            }

            if (targetingSystem.targetPressedTime < Time.time & targetingSystem.getNextEnemy == true || automaticSearch == true || forceSearch == true)
            {
                Scene scene = targetingSystem.ship.scene;
                int countStart = 0;

                //This sets the count start according to the current target the ship has selected
                countStart = GetNextTargetNo_SmallShipPlayer(targetingSystem);

                for (int i = countStart; i < scene.objectPool.Count; i++)
                {
                    if (scene.objectPool[i] != null & targetingSystem.targetNumber != i) //This gets enemy ships in the scene
                    {
                        if (scene.objectPool[i].activeSelf == true) //This ignores objects that are inactive
                        {
                            bool isHostile = false;
                            int numberTargetting = 0;

                            Ship targetSmallShip = scene.objectPool[i].GetComponent<Ship>();
                            LargeShip targetLargeShip = scene.objectPool[i].GetComponent<LargeShip>();

                            if (targetSmallShip != null)
                            {
                                numberTargetting = targetSmallShip.targetingSystem.numberTargeting;
                                isHostile = GetHostility(targetSmallShip, targetSmallShip.allegiance);
                            }
                            else if (targetLargeShip != null)
                            {
                                numberTargetting = 0;
                                isHostile = GetHostility(targetSmallShip, targetLargeShip.allegiance);
                            }

                            if (isHostile == true)
                            {
                                targetingSystem.target = scene.objectPool[i];
                                targetingSystem.targetName = scene.objectPool[i].name;
                                targetingSystem.targetNumber = i;

                                if (targetSmallShip != null)
                                {
                                    targetingSystem.targetShip = targetSmallShip;
                                    targetingSystem.targetLargeShip = null;
                                    targetingSystem.targetPrefabName = targetSmallShip.prefab;
                                }
                                else if (targetLargeShip != null)
                                {
                                    targetingSystem.targetShip = null;
                                    targetingSystem.targetLargeShip = targetLargeShip;
                                    targetingSystem.targetPrefabName = targetLargeShip.prefabName;
                                }

                                targetingSystem.targetRigidbody = scene.objectPool[i].GetComponent<Rigidbody>();
                                break;
                            }
                        }
                    }
                }

                targetingSystem.targetPressedTime = Time.time + 0.2f;

                AudioFunctions.PlayAudioClip(targetingSystem.ship.audioManager, "beep01_toggle", "Cockpit", targetingSystem.gameObject.transform.position, 0, 1, 500, 1, 100);

                //This prevents the torpedo from immediately locking on to the new target
                targetingSystem.ship.torpedoSystem.torpedoLockedOn = false;
                targetingSystem.ship.torpedoSystem.torpedoLockingOn = false;
            }
        }
    }

    //This gets the closesd enemy target
    public static void GetClosestEnemy_SmallShipPlayer(TargetingSystem targetingSystem = null, bool externalActivation = false)
    {
        if (targetingSystem.ship.isAI == false)
        {
            bool automaticSearch = false;

            if (targetingSystem.target != null)
            {
                if (targetingSystem.target.activeSelf == false)
                {
                    automaticSearch = true;
                }
            }

            if (targetingSystem.targetPressedTime < Time.time & targetingSystem.getClosestEnemy == true || automaticSearch == true || externalActivation == true)
            {
                Scene scene = targetingSystem.ship.scene;

                GameObject target = null;
                Ship tempShip = null;
                Ship targetShip = null;
                LargeShip tempLargeShip = null;
                LargeShip targetLargeShip = null;

                float distance = Mathf.Infinity;

                //This checks for the closest small ship first
                foreach (GameObject ship in scene.objectPool)
                {
                    if (ship != null)
                    {
                        tempShip = ship.GetComponent<Ship>();
                        tempLargeShip = ship.GetComponent<LargeShip>();

                        if (ship.activeSelf == true & tempShip != null)
                        {
                            bool isHostile = GetHostility(targetingSystem.ship, tempShip.allegiance);

                            if (isHostile == true & tempShip.damageSystem.isDisabled == false & tempShip.flightControlSystem_Small.docking == false)
                            {
                                float tempDistance = Vector3.Distance(ship.transform.position, targetingSystem.gameObject.transform.position);

                                if (tempDistance < distance)
                                {
                                    float numberTargetting = tempShip.targetingSystem.numberTargeting;

                                    if (targetingSystem.ship.isAI == true & targetingSystem.targetNumber <= 1)
                                    {
                                        target = ship;
                                        targetShip = tempShip;
                                        distance = tempDistance;
                                    }
                                    else if (targetingSystem.ship.isAI == false)
                                    {
                                        target = ship;
                                        targetShip = tempShip;
                                        distance = tempDistance;
                                    }
                                }
                            }
                        }

                    }
                }

                //If the target is still null it looks for the closest large ship
                if (target == null)
                {
                    foreach (GameObject ship in scene.objectPool)
                    {
                        if (ship != null)
                        {
                            tempShip = ship.GetComponent<Ship>();
                            tempLargeShip = ship.GetComponent<LargeShip>();

                            if (ship.activeSelf == true & tempLargeShip != null)
                            {
                                bool isHostile = GetHostility(targetingSystem.ship, tempLargeShip.allegiance);

                                if (isHostile == true & tempLargeShip.isDisabled == false & tempLargeShip.docking == false)
                                {
                                    float tempDistance = Vector3.Distance(ship.transform.position, targetingSystem.gameObject.transform.position);

                                    if (tempDistance < distance)
                                    {
                                        target = ship;
                                        targetLargeShip = tempLargeShip;
                                        distance = tempDistance;
                                    }
                                }
                            }
                        }
                    }
                }

                //This checks for the closest small ship again but this time includes disabled ships
                if (target == null)
                {
                    foreach (GameObject ship in scene.objectPool)
                    {
                        if (ship != null)
                        {
                            tempShip = ship.GetComponent<Ship>();
                            tempLargeShip = ship.GetComponent<LargeShip>();

                            if (ship.activeSelf == true & tempShip != null)
                            {
                                bool isHostile = GetHostility(targetingSystem.ship, tempShip.allegiance);

                                if (isHostile == true)
                                {
                                    float tempDistance = Vector3.Distance(ship.transform.position, targetingSystem.gameObject.transform.position);

                                    if (tempDistance < distance)
                                    {
                                        float numberTargetting = tempShip.targetingSystem.numberTargeting;

                                        if (targetingSystem.ship.isAI == true & targetingSystem.targetNumber <= 1)
                                        {
                                            target = ship;
                                            targetShip = tempShip;
                                            distance = tempDistance;
                                        }
                                        else if (targetingSystem.ship.isAI == false)
                                        {
                                            target = ship;
                                            targetShip = tempShip;
                                            distance = tempDistance;
                                        }
                                    }
                                }
                            }

                        }
                    }
                }

                //If the target is still null it looks for the closest large ship even if it's disabled
                if (target == null)
                {
                    foreach (GameObject ship in scene.objectPool)
                    {
                        if (ship != null)
                        {
                            tempShip = ship.GetComponent<Ship>();
                            tempLargeShip = ship.GetComponent<LargeShip>();

                            if (ship.activeSelf == true & tempLargeShip != null)
                            {
                                bool isHostile = GetHostility(targetingSystem.ship, tempLargeShip.allegiance);

                                if (isHostile == true)
                                {
                                    float tempDistance = Vector3.Distance(ship.transform.position, targetingSystem.gameObject.transform.position);

                                    if (tempDistance < distance)
                                    {
                                        target = ship;
                                        targetLargeShip = tempLargeShip;
                                        distance = tempDistance;
                                    }
                                }
                            }
                        }
                    }
                }

                //This applies the chosen target if it isn't null
                if (target != null)
                {
                    targetingSystem.target = target;
                    targetingSystem.targetName = target.name;

                    if (targetShip != null)
                    {
                        targetingSystem.targetShip = targetShip;
                        targetingSystem.targetLargeShip = null;
                        targetingSystem.targetPrefabName = targetShip.prefab;

                        if (targetingSystem.ship.isAI == true)
                        {
                            targetShip.targetingSystem.numberTargeting += 1;
                        }

                    }
                    else if (targetLargeShip != null)
                    {
                        targetingSystem.targetShip = null;
                        targetingSystem.targetLargeShip = targetLargeShip;
                        targetingSystem.targetPrefabName = targetLargeShip.prefabName;
                    }

                    targetingSystem.targetRigidbody = target.GetComponent<Rigidbody>();
                }

                targetingSystem.targetPressedTime = Time.time + 0.2f;

                AudioFunctions.PlayAudioClip(targetingSystem.ship.audioManager, "beep01_toggle", "Cockpit", targetingSystem.gameObject.transform.position, 0, 1, 500, 1, 100);

                //This prevents the torpedo from immediately locking on to the new target
                targetingSystem.ship.torpedoSystem.torpedoLockedOn = false;
                targetingSystem.ship.torpedoSystem.torpedoLockingOn = false;
            }
        }
    }

    //This gets the target directly ahead
    public static void GetTargetDirectlyAhead_SmallShipPlayer(TargetingSystem targetingSystem = null)
    {
        if (targetingSystem.ship.isAI == false)
        {     
            if (targetingSystem.targetPressedTime < Time.time & targetingSystem.selectTargetInFront == true)
            {
                Scene scene = targetingSystem.ship.scene;
                float forward = 0.9f;
                GameObject target = null;
                Ship targetShip = null;
                LargeShip targetLargeShip = null;

                //This checks for the closest small ship first
                foreach (GameObject tempTarget in scene.objectPool)
                {
                    if (tempTarget != null)
                    {
                        float targetForward = 0;
                        Vector3 targetPosition = tempTarget.transform.position;
                        Vector3 targetRelativePosition = targetPosition - targetingSystem.transform.position;
                        targetForward = Vector3.Dot(targetingSystem.transform.forward, targetRelativePosition.normalized);

                        if (targetForward > forward)
                        {
                            forward = targetForward;
                            target = tempTarget;
                        }
                    }
                }

                if (target != null)
                {
                    targetingSystem.target = target;
                    targetingSystem.targetName = target.name;
                    targetShip = target.GetComponent<Ship>();
                    targetLargeShip = target.GetComponent<LargeShip>();

                    if (targetShip != null)
                    {
                        targetingSystem.targetShip = targetShip;
                        targetingSystem.targetLargeShip = null;
                        targetingSystem.targetPrefabName = targetShip.prefab;
                    }
                    else if (targetLargeShip != null)
                    {
                        targetingSystem.targetShip = null;
                        targetingSystem.targetLargeShip = targetLargeShip;
                        targetingSystem.targetPrefabName = targetLargeShip.prefabName;
                    }

                    targetingSystem.targetRigidbody = target.GetComponent<Rigidbody>();
                }

                targetingSystem.targetPressedTime = Time.time + 0.2f;

            }
        }
    }

    //This gets the designated target and sets it as the ships target if it can be found
    public static void GetSpecificTarget_SmallShip(TargetingSystem targetingSystem = null, string targetName = "none")
    {
        Scene scene = targetingSystem.ship.scene;
        GameObject target = null;
        Ship tempShip = null;
        Ship targetShip = null;
        LargeShip tempLargeShip = null;
        LargeShip targetLargeShip = null;

        foreach (GameObject ship in scene.objectPool)
        {
            if (ship != null)
            {

                if (ship.activeSelf == true)
                {

                    if (ship.name.Contains(targetName))
                    {
                        target = ship;
                        tempShip = ship.GetComponent<Ship>();
                        tempLargeShip = ship.GetComponent<LargeShip>();

                        if (tempShip != null)
                        {
                            targetShip = tempShip;
                        }
                        else if (tempLargeShip != null)
                        {
                            targetLargeShip = tempLargeShip;                           
                        }

                        break;
                    }
                }
            }
        }

        if (target != null)
        {
            targetingSystem.target = target;
            targetingSystem.targetName = target.name;

            if (targetShip != null)
            {
                targetingSystem.targetShip = targetShip;
                targetingSystem.targetLargeShip = null;
                targetingSystem.targetPrefabName = targetShip.prefab;
            }
            else if (targetLargeShip != null)
            {
                targetingSystem.targetShip = null;
                targetingSystem.targetLargeShip = targetLargeShip;
                targetingSystem.targetPrefabName = targetLargeShip.prefabName;
            }

            targetingSystem.targetRigidbody = target.GetComponent<Rigidbody>();
        }

        if (targetingSystem.ship.isAI == false)
        {
            AudioFunctions.PlayAudioClip(targetingSystem.ship.audioManager, "beep01_toggle", "Cockpit", targetingSystem.gameObject.transform.position, 0, 1, 500, 1, 100);
        }

        //This prevents the torpedo from immediately locking on to the new target
        targetingSystem.ship.torpedoSystem.torpedoLockedOn = false;
        targetingSystem.ship.torpedoSystem.torpedoLockingOn = false;
    }

    //This creates the ship waypoint
    public static void CreateWaypoint_SmallShipPlayer(TargetingSystem targetingSystem = null)
    {
        if (targetingSystem.waypoint == null)
        {
            Scene scene = SceneFunctions.GetScene();
            targetingSystem.waypoint = new GameObject();
            targetingSystem.waypoint.name = "waypoint_" + targetingSystem.name;
            targetingSystem.waypoint.transform.SetParent(scene.transform);
        }
    }

    //This checks whether a target is hostile or not
    public static bool GetHostility(Ship ship = null, string targetAllegiance = "none")
    {
        bool isHostile = false;

        if (ship.scene != null)
        {
            if (ship.scene.allegiance == "none")
            {
                TextAsset allegiancesFile = Resources.Load(OGGetAddress.files + "Allegiances") as TextAsset;
                ship.scene.allegiance = allegiancesFile.text;
            }

            Allegiances allegiances = JsonUtility.FromJson<Allegiances>(ship.scene.allegiance);

            Allegiance allegiance = null;

            foreach (Allegiance tempAllegiance in allegiances.allegianceData)
            {
                if (tempAllegiance.allegiance == ship.allegiance)
                {
                    allegiance = tempAllegiance;
                }
            }

            if (allegiance.enemy01 == targetAllegiance)
            {
                isHostile = true;
            }
            else if (allegiance.enemy02 == targetAllegiance)
            {
                isHostile = true;
            }
            else if (allegiance.enemy03 == targetAllegiance)
            {
                isHostile = true;
            }
            else if (allegiance.enemy04 == targetAllegiance)
            {
                isHostile = true;
            }
            else if (allegiance.enemy05 == targetAllegiance)
            {
                isHostile = true;
            }
            else if (allegiance.enemy06 == targetAllegiance)
            {
                isHostile = true;
            }
            else if (allegiance.enemy07 == targetAllegiance)
            {
                isHostile = true;
            }
            else if (allegiance.enemy08 == targetAllegiance)
            {
                isHostile = true;
            }
            else if (allegiance.enemy09 == targetAllegiance)
            {
                isHostile = true;
            }
            else if (allegiance.enemy10 == targetAllegiance)
            {
                isHostile = true;
            }
        }

        return isHostile;
    }

    //This gets the target number of the currently selected ship
    public static int GetNextTargetNo_SmallShipPlayer(TargetingSystem targetingSystem)
    {
        Scene scene = targetingSystem.ship.scene;
        int targetNumber = 0;

        if (scene != null)
        {
            if (scene.objectPool != null)
            {
                if (targetingSystem.target != null)
                {
                    for (int i = 0; i < scene.objectPool.Count; i++)
                    {
                        if (scene.objectPool[i] != null)
                        {
                            if (scene.objectPool[i].name == targetingSystem.target.name)
                            {
                                targetNumber = i + 1;
                            }
                        }
                    }

                    if (targetNumber >= scene.objectPool.Count)
                    {
                        targetNumber = 0;
                    }
                }
            }
        }

        return targetNumber;
    }

    #endregion

    #region AI smallship targetting

    //This gets the closesd enemy target
    public static IEnumerator GetClosestEnemySmallShip_SmallShipAI(TargetingSystem targetingSystem)
    {
        Scene scene = targetingSystem.ship.scene;

        GameObject target = null;
        Ship targetShip = null;

        float distance = Mathf.Infinity;

        //This checks for the closest small ship first
        foreach (Ship ship in scene.smallShips)
        {
            if (ship != null & targetingSystem != null)
            {
                if (ship.gameObject.activeSelf == true & ship != null)
                {
                    bool isHostile = GetHostility(targetingSystem.ship, ship.allegiance);

                    if (isHostile == true & targetingSystem.ship.damageSystem.isDisabled == false & targetingSystem.ship.flightControlSystem_Small.docking == false)
                    {
                        float tempDistance = Vector3.Distance(ship.transform.position, targetingSystem.gameObject.transform.position);

                        if (tempDistance < distance & ship.targetingSystem.numberTargeting < 3)
                        {
                            target = ship.gameObject;
                            targetShip = ship;
                            distance = tempDistance;
                        }
                    }
                }

            }
        }

        if (target != null)
        {
            targetingSystem.target = target;
            targetingSystem.targetName = target.name;

            if (targetShip != null)
            {
                targetingSystem.targetShip = targetShip;
                targetingSystem.targetLargeShip = null;
                targetingSystem.targetPrefabName = targetShip.prefab;

                if (targetingSystem.ship.isAI == true)
                {
                    targetShip.targetingSystem.numberTargeting += 1;
                }
            }

            targetingSystem.targetRigidbody = target.GetComponent<Rigidbody>();
        }

        //This prevents the torpedo from immediately locking on to the new target
        targetingSystem.ship.torpedoSystem.torpedoLockedOn = false;
        targetingSystem.ship.torpedoSystem.torpedoLockingOn = false;

        yield return null;
    }

    //This gets the closesd enemy target
    public static IEnumerator GetClosestEnemyLargeShip_SmallShipAI(TargetingSystem targetingSystem)
    {
        Scene scene = targetingSystem.ship.scene;

        GameObject target = null;
        LargeShip targetLargeShip = null;

        float distance = Mathf.Infinity;

        //If the target is still null it looks for the closest large ship
        if (target == null)
        {
            foreach (LargeShip tempLargeShip in scene.largeShips)
            {
                if (tempLargeShip != null)
                {
                    if (tempLargeShip.gameObject.activeSelf == true & tempLargeShip != null)
                    {
                        bool isHostile = GetHostility(targetingSystem.ship, tempLargeShip.allegiance);

                        if (isHostile == true)
                        {
                            float tempDistance = Vector3.Distance(tempLargeShip.transform.position, targetingSystem.gameObject.transform.position);

                            if (tempDistance < distance)
                            {
                                target = tempLargeShip.gameObject;
                                targetLargeShip = tempLargeShip;
                                distance = tempDistance;
                            }
                        }
                    }
                }
            }
        }

        if (target != null)
        {
            targetingSystem.target = target;
            targetingSystem.targetName = target.name;

            if (targetLargeShip != null)
            {
                targetingSystem.targetShip = null;
                targetingSystem.targetLargeShip = targetLargeShip;
                targetingSystem.targetPrefabName = targetLargeShip.prefabName;
            }

            targetingSystem.targetRigidbody = target.GetComponent<Rigidbody>();
        }

        //This prevents the torpedo from immediately locking on to the new target
        targetingSystem.ship.torpedoSystem.torpedoLockedOn = false;
        targetingSystem.ship.torpedoSystem.torpedoLockingOn = false;

        yield return null;
    }

    #endregion

    #region AI largeship targetting

    //This gets the closesd enemy target
    public static IEnumerator GetClosestEnemyLargeShip_LargeShipAI(LargeShip largeShip)
    {
        Scene scene = largeShip.scene;

        GameObject target = null;
        LargeShip tempLargeShip = null;
        LargeShip targetLargeShip = null;

        float distance = Mathf.Infinity;

        //If the target is still null it looks for the closest large ship
        if (target == null)
        {
            foreach (GameObject ship in scene.objectPool)
            {
                if (ship != null)
                {
                    tempLargeShip = ship.GetComponent<LargeShip>();

                    if (ship.activeSelf == true & tempLargeShip != null)
                    {
                        bool isHostile = GetHostility_LargeShipAI(largeShip, tempLargeShip.allegiance);

                        if (isHostile == true)
                        {
                            float tempDistance = Vector3.Distance(ship.transform.position, largeShip.gameObject.transform.position);

                            if (tempDistance < distance)
                            {
                                target = ship;
                                targetLargeShip = tempLargeShip;
                                distance = tempDistance;
                            }
                        }
                    }
                }
            }
        }

        if (target != null)
        {
            largeShip.target = target;
            largeShip.targetName = target.name;

            if (targetLargeShip != null)
            {
                largeShip.targetSmallShip = null;
                largeShip.targetLargeShip = targetLargeShip;
                largeShip.targetPrefabName = targetLargeShip.prefabName;
            }

            largeShip.targetRigidbody = target.GetComponent<Rigidbody>();
        }

        yield return null;
    }

    //This gets the designated target and sets it as the ships target if it can be found
    public static void GetSpecificTarget_LargeShipAI(LargeShip largeShip = null, string targetName = "none")
    {
        Scene scene = largeShip.scene;
        GameObject target = null;
        Ship tempShip = null;
        Ship targetShip = null;
        LargeShip tempLargeShip = null;
        LargeShip targetLargeShip = null;

        foreach (GameObject ship in scene.objectPool)
        {
            if (ship != null)
            {
                if (ship.activeSelf == true)
                {
                    if (ship.name.Contains(targetName))
                    {
                        target = ship;
                        tempShip = ship.GetComponent<Ship>();
                        tempLargeShip = ship.GetComponent<LargeShip>();

                        if (tempShip != null)
                        {
                            targetShip = tempShip;
                        }
                        else if (tempLargeShip != null)
                        {
                            targetLargeShip = tempLargeShip;
                        }

                        break;
                    }
                }
            }
        }

        if (target != null)
        {
            largeShip.target = target;
            largeShip.targetName = target.name;

            if (targetShip != null)
            {
                largeShip.targetSmallShip = targetShip;
                largeShip.targetLargeShip = null;
                largeShip.targetPrefabName = targetShip.prefab;
            }
            else if (targetLargeShip != null)
            {
                largeShip.targetSmallShip = null;
                largeShip.targetLargeShip = targetLargeShip;
                largeShip.targetPrefabName = targetLargeShip.prefabName;
            }

            largeShip.targetRigidbody = target.GetComponent<Rigidbody>();
        }
    }

    //This draws key data/info from the target including relative position, distance, and hostility
    public static void GetTargetInfo_LargeShipAI(LargeShip largeShip = null)
    {
        Transform shipTransform = largeShip.gameObject.transform;
        Vector3 shipPosition = shipTransform.position;

        if (largeShip.target != null)
        {
            if (largeShip.target.activeSelf == true)
            {
                if (largeShip.targetSmallShip != null)
                {
                    largeShip.targetAllegiance = largeShip.targetSmallShip.allegiance;
                    largeShip.targetSpeed = largeShip.targetSmallShip.flightControlSystem_Small.thrustSpeed;
                    largeShip.targetHull = largeShip.targetSmallShip.energyManagementSystem.hullLevel;
                    largeShip.targetShield = largeShip.targetSmallShip.energyManagementSystem.shieldLevel;
                    largeShip.targetType = largeShip.targetSmallShip.type;
                    largeShip.targetPrefabName = largeShip.targetSmallShip.prefab;
                }

                if (largeShip.targetLargeShip != null)
                {
                    largeShip.targetAllegiance = largeShip.targetLargeShip.allegiance;
                    largeShip.targetSpeed = largeShip.targetLargeShip.thrustSpeed;
                    largeShip.targetHull = largeShip.targetLargeShip.hullLevel;
                    largeShip.targetShield = largeShip.targetLargeShip.shieldLevel;
                    largeShip.targetType = largeShip.targetLargeShip.type;
                    largeShip.targetPrefabName = largeShip.targetLargeShip.prefabName;
                }

                Transform targetTransform = largeShip.target.transform;
                Vector3 targetPosition = targetTransform.position;
                Vector3 targetVelocity = new Vector3(0, 0, 0);

                if (largeShip.targetRigidbody != null)
                {
                    targetVelocity = largeShip.targetRigidbody.linearVelocity;
                }

                Vector3 targetRelativePosition = targetPosition - shipPosition;

                largeShip.targetDistance = Vector3.Distance(shipPosition, targetPosition);
                largeShip.targetForward = Vector3.Dot(shipTransform.forward, targetRelativePosition.normalized);
                largeShip.targetRight = Vector3.Dot(shipTransform.right, targetRelativePosition.normalized);
                largeShip.targetUp = Vector3.Dot(shipTransform.up, targetRelativePosition.normalized);
            }
        }

        if (largeShip.waypoint != null)
        {
            Vector3 waypointPosition = largeShip.waypoint.transform.position;
            Vector3 waypointRelativePosition = waypointPosition - shipPosition;

            largeShip.waypointDistance = Vector3.Distance(shipPosition, waypointPosition);
            largeShip.waypointForward = Vector3.Dot(shipTransform.forward, waypointRelativePosition.normalized);
            largeShip.waypointRight = Vector3.Dot(shipTransform.right, waypointRelativePosition.normalized);
            largeShip.waypointUp = Vector3.Dot(shipTransform.up, waypointRelativePosition.normalized);
        }

    }

    //This creates the ship waypoint
    public static void CreateWaypoint_LargeShipAI(LargeShip largeShip = null)
    {
        if (largeShip.waypoint == null)
        {
            Scene scene = SceneFunctions.GetScene();
            largeShip.waypoint = new GameObject();
            largeShip.waypoint.name = "waypoint_" + largeShip.name;
            largeShip.waypoint.transform.SetParent(scene.transform);
        }
    }

    //This checks whether a target is hostile or not
    public static bool GetHostility_LargeShipAI(LargeShip largeShip = null, string targetAllegiance = "none")
    {
        bool isHostile = false;

        if (largeShip.scene != null)
        {
            //This gets the Json ship data
            if (largeShip.scene.allegiance == "none")
            {
                TextAsset allegiancesFile = Resources.Load(OGGetAddress.files + "Allegiances") as TextAsset;
                largeShip.scene.allegiance = allegiancesFile.text;
            }

            Allegiances allegiances = JsonUtility.FromJson<Allegiances>(largeShip.scene.allegiance);

            Allegiance allegiance = null;

            foreach (Allegiance tempAllegiance in allegiances.allegianceData)
            {
                if (tempAllegiance.allegiance == largeShip.allegiance)
                {
                    allegiance = tempAllegiance;
                }
            }

            if (allegiance.enemy01 == targetAllegiance)
            {
                isHostile = true;
            }
            else if (allegiance.enemy02 == targetAllegiance)
            {
                isHostile = true;
            }
            else if (allegiance.enemy03 == targetAllegiance)
            {
                isHostile = true;
            }
            else if (allegiance.enemy04 == targetAllegiance)
            {
                isHostile = true;
            }
            else if (allegiance.enemy05 == targetAllegiance)
            {
                isHostile = true;
            }
            else if (allegiance.enemy06 == targetAllegiance)
            {
                isHostile = true;
            }
            else if (allegiance.enemy07 == targetAllegiance)
            {
                isHostile = true;
            }
            else if (allegiance.enemy08 == targetAllegiance)
            {
                isHostile = true;
            }
            else if (allegiance.enemy09 == targetAllegiance)
            {
                isHostile = true;
            }
            else if (allegiance.enemy10 == targetAllegiance)
            {
                isHostile = true;
            }
        }

        return isHostile;
    }

    #endregion

    #region turret targetting

    public static GameObject GetClosestEnemyLargeShip_Turret(GameObject turret, string allegiance)
    {
        Scene scene = SceneFunctions.GetScene();

        GameObject target = null;
        LargeShip tempLargeShip = null;

        float distance = Mathf.Infinity;

        //If the target is still null it looks for the closest large ship
        if (target == null)
        {
            foreach (GameObject ship in scene.objectPool)
            {
                if (ship != null)
                {
                    tempLargeShip = ship.GetComponent<LargeShip>();

                    if (ship.activeSelf == true & tempLargeShip != null)
                    {
                        bool isHostile = GetHostility_Turret(scene, allegiance, tempLargeShip.allegiance);

                        if (isHostile == true)
                        {
                            float tempDistance = Vector3.Distance(ship.transform.position, turret.transform.position);

                            if (tempDistance < distance)
                            {
                                target = ship;
                                distance = tempDistance;
                            }
                        }
                    }
                }
            }
        }

        return target;
    }

    public static GameObject GetClosestEnemySmallShip_Turret(GameObject turret, string allegiance)
    {
        Scene scene = SceneFunctions.GetScene();

        GameObject target = null;
        Ship tempShip = null;

        float distance = Mathf.Infinity;

        //If the target is still null it looks for the closest large ship
        if (target == null)
        {
            foreach (GameObject shipGO in scene.objectPool)
            {
                if (shipGO != null)
                {
                    tempShip = shipGO.GetComponent<Ship>();

                    if (shipGO.activeSelf == true & tempShip != null)
                    {
                        bool isHostile = GetHostility_Turret(scene, allegiance, tempShip.allegiance);

                        if (isHostile == true)
                        {
                            float tempDistance = Vector3.Distance(shipGO.transform.position, turret.transform.position);

                            if (tempDistance < distance)
                            {
                                target = shipGO;
                                distance = tempDistance;
                            }
                        }
                    }
                }
            }
        }

        return target;
    }

    //This checks whether a target is hostile or not
    public static bool GetHostility_Turret(Scene scene, string turrretAllegiance, string targetAllegiance = "none")
    {
        bool isHostile = false;

        if (scene != null)
        {
            //This gets the Json ship data
            if (scene.allegiance == "none")
            {
                TextAsset allegiancesFile = Resources.Load(OGGetAddress.files + "Allegiances") as TextAsset;
                scene.allegiance = allegiancesFile.text;
            }

            Allegiances allegiances = JsonUtility.FromJson<Allegiances>(scene.allegiance);

            Allegiance allegiance = null;

            foreach (Allegiance tempAllegiance in allegiances.allegianceData)
            {
                if (tempAllegiance.allegiance == turrretAllegiance)
                {
                    allegiance = tempAllegiance;
                }
            }

            if (allegiance.enemy01 == targetAllegiance)
            {
                isHostile = true;
            }
            else if (allegiance.enemy02 == targetAllegiance)
            {
                isHostile = true;
            }
            else if (allegiance.enemy03 == targetAllegiance)
            {
                isHostile = true;
            }
            else if (allegiance.enemy04 == targetAllegiance)
            {
                isHostile = true;
            }
            else if (allegiance.enemy05 == targetAllegiance)
            {
                isHostile = true;
            }
            else if (allegiance.enemy06 == targetAllegiance)
            {
                isHostile = true;
            }
            else if (allegiance.enemy07 == targetAllegiance)
            {
                isHostile = true;
            }
            else if (allegiance.enemy08 == targetAllegiance)
            {
                isHostile = true;
            }
            else if (allegiance.enemy09 == targetAllegiance)
            {
                isHostile = true;
            }
            else if (allegiance.enemy10 == targetAllegiance)
            {
                isHostile = true;
            }
        }

        return isHostile;
    }

    #endregion

    #region AI target allocation

    //Allocate targets to all the turrets one at a time to save processing power
    public static IEnumerator AllocateTargets_ShipsAI(Scene scene)
    {
        scene.allocatingTargets = true;

        //This selects targets for smallship ai
        if (scene.smallShips == null)
        {
            scene.smallShips = new List<Ship>();
        }

        foreach (Ship ship in scene.smallShips.ToArray())
        {
            if (ship != null)
            {
                if (ship.gameObject.activeSelf == true & ship.flightControlAI_Small.requestingTarget == true)
                {
                    if (ship.flightControlAI_Small.aiTargetingMode == "targetallprefsmall")
                    {
                        Task a = new Task(GetClosestEnemySmallShip_SmallShipAI(ship.targetingSystem));
                        while (a.Running == true) { yield return null; }

                        if (ship.targetingSystem.target == null)
                        {
                            Task b = new Task(GetClosestEnemyLargeShip_SmallShipAI(ship.targetingSystem));
                            while (b.Running == true) { yield return null; }
                        }
                    }
                    else if (ship.flightControlAI_Small.aiTargetingMode == "targetallpreflarge")
                    {
                        Task b = new Task(GetClosestEnemyLargeShip_SmallShipAI(ship.targetingSystem));
                        while (b.Running == true) { yield return null; }

                        if (ship.targetingSystem.target == null)
                        {
                            Task a = new Task(GetClosestEnemySmallShip_SmallShipAI(ship.targetingSystem));
                            while (a.Running == true) { yield return null; }
                        }
                    }
                    else if (ship.flightControlAI_Small.aiTargetingMode == "targetsmallshipsonly")
                    {
                        Task a = new Task(GetClosestEnemySmallShip_SmallShipAI(ship.targetingSystem));
                        while (a.Running == true) { yield return null; }
                    }
                    else if (ship.flightControlAI_Small.aiTargetingMode == "targetlargeshipsonly")
                    {
                        Task a = new Task(GetClosestEnemyLargeShip_SmallShipAI(ship.targetingSystem));
                        while (a.Running == true) { yield return null; }
                    }
                }
            }
        }

        //This selects targets for largeships
        if (scene.largeShips == null)
        {
            scene.largeShips = new List<LargeShip>();
        }

        foreach (LargeShip largeShip in scene.largeShips.ToArray())
        {
            if (largeShip != null)
            {
                if (largeShip.gameObject.activeSelf == true & largeShip.requestingTarget == true)
                {
                    if (largeShip.target == null)
                    {
                        Task a = new Task(TargetingSystemFunctions.GetClosestEnemyLargeShip_LargeShipAI(largeShip));
                        while (a.Running == true) { yield return null; }

                    }
                    else if (largeShip.target != null)
                    {
                        if (largeShip.target.activeSelf == false)
                        {
                            Task a = new Task(TargetingSystemFunctions.GetClosestEnemyLargeShip_LargeShipAI(largeShip));
                            while (a.Running == true) { yield return null; }
                        }
                    }
                }
            }
        }

        scene.allocatingTargets = false;

        yield return null;
    }

    #endregion
}
