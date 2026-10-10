using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TargetingSystemFunctions
{
    //This draws key data/info from the target including relative position, distance, and hostility
    public static void GetTargetInfo_SmallShip(FlightControlSystem_Small smallShip = null)
    {
        Transform shipTransform = smallShip.gameObject.transform;
        Vector3 shipPosition = shipTransform.position;

        if (smallShip.targetingSystem.target != null)
        {
            if (smallShip.targetingSystem.target.activeSelf == true)
            {
                if (smallShip.targetingSystem.targetSmallShip != null)
                {
                    smallShip.targetingSystem.targetAllegiance = smallShip.targetingSystem.targetSmallShip.allegiance;
                    smallShip.targetingSystem.targetSpeed = smallShip.targetingSystem.targetSmallShip.thrustSpeed;
                    smallShip.targetingSystem.targetHull = smallShip.targetingSystem.targetSmallShip.hullLevel;
                    smallShip.targetingSystem.targetShield = smallShip.targetingSystem.targetSmallShip.shieldLevel;
                    smallShip.targetingSystem.targetType = smallShip.targetingSystem.targetSmallShip.type;
                    smallShip.targetingSystem.targetPrefabName = smallShip.targetingSystem.targetSmallShip.prefabName;
                }

                if (smallShip.targetingSystem.targetLargeShip != null)
                {
                    smallShip.targetingSystem.targetAllegiance = smallShip.targetingSystem.targetLargeShip.allegiance;
                    smallShip.targetingSystem.targetSpeed = smallShip.targetingSystem.targetLargeShip.thrustSpeed;
                    smallShip.targetingSystem.targetHull = smallShip.targetingSystem.targetLargeShip.hullLevel;
                    smallShip.targetingSystem.targetShield = smallShip.targetingSystem.targetLargeShip.shieldLevel;
                    smallShip.targetingSystem.targetType = smallShip.targetingSystem.targetLargeShip.type;
                    smallShip.targetingSystem.targetPrefabName = smallShip.targetingSystem.targetLargeShip.prefabName;
                }

                Transform targetTransform = smallShip.targetingSystem.target.transform;
                Vector3 targetPosition = targetTransform.position;
                Vector3 targetVelocity = new Vector3(0, 0, 0);

                if (smallShip.targetingSystem.targetRigidbody != null)
                {
                    targetVelocity = smallShip.targetingSystem.targetRigidbody.linearVelocity;
                }

                Vector3 targetRelativePosition = targetPosition - shipPosition;

                smallShip.targetingSystem.targetDistance = Vector3.Distance(shipPosition, targetPosition);
                smallShip.targetingSystem.targetForward = Vector3.Dot(shipTransform.forward, targetRelativePosition.normalized);
                smallShip.targetingSystem.targetRight = Vector3.Dot(shipTransform.right, targetRelativePosition.normalized);
                smallShip.targetingSystem.targetUp = Vector3.Dot(shipTransform.up, targetRelativePosition.normalized);

                Vector3 targettingErrorMargin = smallShip.aiTargetingErrorMargin;
                float distanceToIntercept = ((targetPosition) - shipPosition).magnitude / 750f;

                Vector3 interceptPosition = (targetPosition + targettingErrorMargin) + targetVelocity * distanceToIntercept;
                Vector3 interceptRelativePosition = interceptPosition - shipPosition;

                Vector3 currentVelocity = Vector3.zero;
                float smoothTime = 0.3f;

                smallShip.targetingSystem.interceptPoint = Vector3.SmoothDamp(smallShip.targetingSystem.interceptPoint, interceptRelativePosition, ref currentVelocity, smoothTime);
                smallShip.targetingSystem.interceptDistance = Vector3.Distance(interceptPosition, shipPosition);
                smallShip.targetingSystem.interceptForward = Vector3.Dot(shipTransform.forward, interceptRelativePosition.normalized);
                smallShip.targetingSystem.interceptRight = Vector3.Dot(shipTransform.right, interceptRelativePosition.normalized);
                smallShip.targetingSystem.interceptUp = Vector3.Dot(shipTransform.up, interceptRelativePosition.normalized);
            }
        }
        else
        {
            smallShip.targetingSystem.interceptDistance = 250;
        }

        if (smallShip.targetingSystem.waypoint != null)
        {
            Vector3 waypointPosition = smallShip.targetingSystem.waypoint.transform.position;
            Vector3 waypointRelativePosition = waypointPosition - shipPosition;

            smallShip.targetingSystem.waypointDistance = Vector3.Distance(shipPosition, waypointPosition);
            smallShip.targetingSystem.waypointForward = Vector3.Dot(shipTransform.forward, waypointRelativePosition.normalized);
            smallShip.targetingSystem.waypointRight = Vector3.Dot(shipTransform.right, waypointRelativePosition.normalized);
            smallShip.targetingSystem.waypointUp = Vector3.Dot(shipTransform.up, waypointRelativePosition.normalized);
        }

    }

    #region player targetting

    //This runs all the player targetting functions
    public static void RunPlayerTargetingFunctions(FlightControlSystem_Small smallShip)
    {
        //Targetting functions
        GetClosestEnemy_SmallShipPlayer(smallShip);
        GetNextEnemy_SmallShipPlayer(smallShip);
        GetNextTarget_SmallShipPlayer(smallShip);
        GetTargetDirectlyAhead_SmallShipPlayer(smallShip);
    }

    //This gets the next target of any kind
    public static void GetNextTarget_SmallShipPlayer(FlightControlSystem_Small smallShip = null)
    {
        if (smallShip.isAI == false)
        {
            bool automaticSearch = false;

            if (smallShip.targetingSystem.target != null)
            {
                if (smallShip.targetingSystem.target.activeSelf == false)
                {
                    automaticSearch = true;
                }
            }

            if (smallShip.targetingSystem.targetPressedTime < Time.time & smallShip.getNextTarget == true || automaticSearch == true)
            {
                Scene scene = smallShip.scene;
                int countStart = 0;

                //This sets the count start according to the current target the ship has selected
                countStart = GetNextTargetNo_SmallShipPlayer(smallShip);

                //This cycles through all the possible targets ignoring objects that are null or inactive
                for (int i = countStart; i <= scene.objectPool.Count; i++)
                {
                    if (i > scene.objectPool.Count - 1) //This clears the target at the end of the list
                    {
                        smallShip.targetingSystem.target = null;
                        smallShip.targetingSystem.targetName = " ";
                        smallShip.targetingSystem.targetNumber = i;
                        smallShip.targetingSystem.targetSmallShip = null;
                        smallShip.targetingSystem.targetRigidbody = null;
                        smallShip.targetingSystem.targetPrefabName = " ";
                        break;
                    }
                    else if (scene.objectPool[i] != null & smallShip.targetingSystem.targetNumber != i) //This gets any type of ship in the scene
                    {
                        if (scene.objectPool[i].activeSelf == true & scene.objectPool[i] != smallShip.gameObject) //This ignores objects that are inactive
                        {
                            smallShip.targetingSystem.target = scene.objectPool[i];
                            smallShip.targetingSystem.targetName = scene.objectPool[i].name;

                            FlightControlSystem_Small targetSmallShip = scene.objectPool[i].GetComponent<FlightControlSystem_Small>();
                            LargeShip targetLargeShip = scene.objectPool[i].GetComponent<LargeShip>();

                            if (targetSmallShip != null)
                            {
                                smallShip.targetingSystem.targetSmallShip = targetSmallShip;
                                smallShip.targetingSystem.targetLargeShip = null;
                                smallShip.targetingSystem.targetPrefabName = targetSmallShip.prefabName;
                            }
                            else if (targetLargeShip != null)
                            {
                                smallShip.targetingSystem.targetSmallShip = null;
                                smallShip.targetingSystem.targetLargeShip = targetLargeShip;
                                smallShip.targetingSystem.targetPrefabName = targetLargeShip.prefabName;
                            }

                            smallShip.targetingSystem.targetRigidbody = scene.objectPool[i].GetComponent<Rigidbody>();
                            smallShip.targetingSystem.targetNumber = i;
                            break;
                        }
                    }
                }

                smallShip.targetingSystem.targetPressedTime = Time.time + 0.2f;

                AudioFunctions.PlayAudioClip(smallShip.audioManager, "beep01_toggle", "Cockpit", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);

                //This prevents the torpedo from immediately locking on to the new target
                smallShip.torpedoSystem.torpedoLockedOn = false;
                smallShip.torpedoSystem.torpedoLockingOn = false;
            }
        }
    }

    //This gets the next enemy target
    public static void GetNextEnemy_SmallShipPlayer(FlightControlSystem_Small smallShip = null, bool forceSearch = false)
    {
        if (smallShip.isAI == false)
        {
            bool automaticSearch = false;

            if (smallShip.targetingSystem.target != null)
            {
                if (smallShip.targetingSystem.target.activeSelf == false)
                {
                    automaticSearch = true;
                }
            }

            if (smallShip.targetingSystem.targetPressedTime < Time.time & smallShip.getNextEnemy == true || automaticSearch == true || forceSearch == true)
            {
                Scene scene = smallShip.scene;
                int countStart = 0;

                //This sets the count start according to the current target the ship has selected
                countStart = GetNextTargetNo_SmallShipPlayer(smallShip);

                for (int i = countStart; i < scene.objectPool.Count; i++)
                {
                    if (scene.objectPool[i] != null & smallShip.targetingSystem.targetNumber != i) //This gets enemy ships in the scene
                    {
                        if (scene.objectPool[i].activeSelf == true) //This ignores objects that are inactive
                        {
                            bool isHostile = false;
                            int numberTargetting = 0;

                            FlightControlSystem_Small targetSmallShip = scene.objectPool[i].GetComponent<FlightControlSystem_Small>();
                            LargeShip targetLargeShip = scene.objectPool[i].GetComponent<LargeShip>();

                            if (targetSmallShip != null)
                            {
                                numberTargetting = targetSmallShip.targetingSystem.numberTargeting;
                                isHostile = GetHostility_SmallShipPlayer(smallShip, targetSmallShip.allegiance);
                            }
                            else if (targetLargeShip != null)
                            {
                                numberTargetting = 0;
                                isHostile = GetHostility_SmallShipPlayer(smallShip, targetLargeShip.allegiance);
                            }

                            if (isHostile == true)
                            {
                                smallShip.targetingSystem.target = scene.objectPool[i];
                                smallShip.targetingSystem.targetName = scene.objectPool[i].name;
                                smallShip.targetingSystem.targetNumber = i;

                                if (targetSmallShip != null)
                                {
                                    smallShip.targetingSystem.targetSmallShip = targetSmallShip;
                                    smallShip.targetingSystem.targetLargeShip = null;
                                    smallShip.targetingSystem.targetPrefabName = targetSmallShip.prefabName;
                                }
                                else if (targetLargeShip != null)
                                {
                                    smallShip.targetingSystem.targetSmallShip = null;
                                    smallShip.targetingSystem.targetLargeShip = targetLargeShip;
                                    smallShip.targetingSystem.targetPrefabName = targetLargeShip.prefabName;
                                }

                                smallShip.targetingSystem.targetRigidbody = scene.objectPool[i].GetComponent<Rigidbody>();
                                break;
                            }
                        }
                    }
                }

                smallShip.targetingSystem.targetPressedTime = Time.time + 0.2f;

                AudioFunctions.PlayAudioClip(smallShip.audioManager, "beep01_toggle", "Cockpit", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);

                //This prevents the torpedo from immediately locking on to the new target
                smallShip.torpedoSystem.torpedoLockedOn = false;
                smallShip.torpedoSystem.torpedoLockingOn = false;
            }
        }
    }

    //This gets the closesd enemy target
    public static void GetClosestEnemy_SmallShipPlayer(FlightControlSystem_Small smallShip = null, bool externalActivation = false)
    {
        if (smallShip.isAI == false)
        {
            bool automaticSearch = false;

            if (smallShip.targetingSystem.target != null)
            {
                if (smallShip.targetingSystem.target.activeSelf == false)
                {
                    automaticSearch = true;
                }
            }

            if (smallShip.targetingSystem.targetPressedTime < Time.time & smallShip.getClosestEnemy == true || automaticSearch == true || externalActivation == true)
            {
                Scene scene = smallShip.scene;

                GameObject target = null;
                FlightControlSystem_Small tempSmallShip = null;
                FlightControlSystem_Small targetSmallShip = null;
                LargeShip tempLargeShip = null;
                LargeShip targetLargeShip = null;

                float distance = Mathf.Infinity;

                //This checks for the closest small ship first
                foreach (GameObject ship in scene.objectPool)
                {
                    if (ship != null)
                    {
                        tempSmallShip = ship.GetComponent<FlightControlSystem_Small>();
                        tempLargeShip = ship.GetComponent<LargeShip>();

                        if (ship.activeSelf == true & tempSmallShip != null)
                        {
                            bool isHostile = GetHostility_SmallShipPlayer(smallShip, tempSmallShip.allegiance);

                            if (isHostile == true & tempSmallShip.isDisabled == false & tempSmallShip.docking == false)
                            {
                                float tempDistance = Vector3.Distance(ship.transform.position, smallShip.gameObject.transform.position);

                                if (tempDistance < distance)
                                {
                                    float numberTargetting = tempSmallShip.targetingSystem.numberTargeting;

                                    if (smallShip.isAI == true & smallShip.targetingSystem.targetNumber <= 1)
                                    {
                                        target = ship;
                                        targetSmallShip = tempSmallShip;
                                        distance = tempDistance;
                                    }
                                    else if (smallShip.isAI == false)
                                    {
                                        target = ship;
                                        targetSmallShip = tempSmallShip;
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
                            tempSmallShip = ship.GetComponent<FlightControlSystem_Small>();
                            tempLargeShip = ship.GetComponent<LargeShip>();

                            if (ship.activeSelf == true & tempLargeShip != null)
                            {
                                bool isHostile = GetHostility_SmallShipPlayer(smallShip, tempLargeShip.allegiance);

                                if (isHostile == true & tempLargeShip.isDisabled == false & tempLargeShip.docking == false)
                                {
                                    float tempDistance = Vector3.Distance(ship.transform.position, smallShip.gameObject.transform.position);

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
                            tempSmallShip = ship.GetComponent<FlightControlSystem_Small>();
                            tempLargeShip = ship.GetComponent<LargeShip>();

                            if (ship.activeSelf == true & tempSmallShip != null)
                            {
                                bool isHostile = GetHostility_SmallShipPlayer(smallShip, tempSmallShip.allegiance);

                                if (isHostile == true)
                                {
                                    float tempDistance = Vector3.Distance(ship.transform.position, smallShip.gameObject.transform.position);

                                    if (tempDistance < distance)
                                    {
                                        float numberTargetting = tempSmallShip.targetingSystem.numberTargeting;

                                        if (smallShip.isAI == true & smallShip.targetingSystem.targetNumber <= 1)
                                        {
                                            target = ship;
                                            targetSmallShip = tempSmallShip;
                                            distance = tempDistance;
                                        }
                                        else if (smallShip.isAI == false)
                                        {
                                            target = ship;
                                            targetSmallShip = tempSmallShip;
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
                            tempSmallShip = ship.GetComponent<FlightControlSystem_Small>();
                            tempLargeShip = ship.GetComponent<LargeShip>();

                            if (ship.activeSelf == true & tempLargeShip != null)
                            {
                                bool isHostile = GetHostility_SmallShipPlayer(smallShip, tempLargeShip.allegiance);

                                if (isHostile == true)
                                {
                                    float tempDistance = Vector3.Distance(ship.transform.position, smallShip.gameObject.transform.position);

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
                    smallShip.targetingSystem.target = target;
                    smallShip.targetingSystem.targetName = target.name;

                    if (targetSmallShip != null)
                    {
                        smallShip.targetingSystem.targetSmallShip = targetSmallShip;
                        smallShip.targetingSystem.targetLargeShip = null;
                        smallShip.targetingSystem.targetPrefabName = targetSmallShip.prefabName;

                        if (smallShip.isAI == true)
                        {
                            targetSmallShip.targetingSystem.numberTargeting += 1;
                        }

                    }
                    else if (targetLargeShip != null)
                    {
                        smallShip.targetingSystem.targetSmallShip = null;
                        smallShip.targetingSystem.targetLargeShip = targetLargeShip;
                        smallShip.targetingSystem.targetPrefabName = targetLargeShip.prefabName;
                    }

                    smallShip.targetingSystem.targetRigidbody = target.GetComponent<Rigidbody>();
                }

                smallShip.targetingSystem.targetPressedTime = Time.time + 0.2f;

                AudioFunctions.PlayAudioClip(smallShip.audioManager, "beep01_toggle", "Cockpit", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);

                //This prevents the torpedo from immediately locking on to the new target
                smallShip.torpedoSystem.torpedoLockedOn = false;
                smallShip.torpedoSystem.torpedoLockingOn = false;
            }
        }
    }

    //This gets the target directly ahead
    public static void GetTargetDirectlyAhead_SmallShipPlayer(FlightControlSystem_Small smallShip = null)
    {
        if (smallShip.isAI == false)
        {     
            if (smallShip.targetingSystem.targetPressedTime < Time.time & smallShip.selectTargetInFront == true)
            {
                Scene scene = smallShip.scene;
                float forward = 0.9f;
                GameObject target = null;
                FlightControlSystem_Small targetSmallShip = null;
                LargeShip targetLargeShip = null;

                //This checks for the closest small ship first
                foreach (GameObject tempTarget in scene.objectPool)
                {
                    if (tempTarget != null)
                    {
                        float targetForward = 0;
                        Vector3 targetPosition = tempTarget.transform.position;
                        Vector3 targetRelativePosition = targetPosition - smallShip.transform.position;
                        targetForward = Vector3.Dot(smallShip.transform.forward, targetRelativePosition.normalized);

                        if (targetForward > forward)
                        {
                            forward = targetForward;
                            target = tempTarget;
                        }
                    }
                }

                if (target != null)
                {
                    smallShip.targetingSystem.target = target;
                    smallShip.targetingSystem.targetName = target.name;
                    targetSmallShip = target.GetComponent<FlightControlSystem_Small>();
                    targetLargeShip = target.GetComponent<LargeShip>();

                    if (targetSmallShip != null)
                    {
                        smallShip.targetingSystem.targetSmallShip = targetSmallShip;
                        smallShip.targetingSystem.targetLargeShip = null;
                        smallShip.targetingSystem.targetPrefabName = targetSmallShip.prefabName;
                    }
                    else if (targetLargeShip != null)
                    {
                        smallShip.targetingSystem.targetSmallShip = null;
                        smallShip.targetingSystem.targetLargeShip = targetLargeShip;
                        smallShip.targetingSystem.targetPrefabName = targetLargeShip.prefabName;
                    }

                    smallShip.targetingSystem.targetRigidbody = target.GetComponent<Rigidbody>();
                }

                smallShip.targetingSystem.targetPressedTime = Time.time + 0.2f;

            }
        }
    }

    //This gets the designated target and sets it as the ships target if it can be found
    public static void GetSpecificTarget_SmallShip(FlightControlSystem_Small smallShip = null, string targetName = "none")
    {
        Scene scene = smallShip.scene;
        GameObject target = null;
        FlightControlSystem_Small tempSmallShip = null;
        FlightControlSystem_Small targetSmallShip = null;
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
                        tempSmallShip = ship.GetComponent<FlightControlSystem_Small>();
                        tempLargeShip = ship.GetComponent<LargeShip>();

                        if (tempSmallShip != null)
                        {
                            targetSmallShip = tempSmallShip;
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
            smallShip.targetingSystem.target = target;
            smallShip.targetingSystem.targetName = target.name;

            if (targetSmallShip != null)
            {
                smallShip.targetingSystem.targetSmallShip = targetSmallShip;
                smallShip.targetingSystem.targetLargeShip = null;
                smallShip.targetingSystem.targetPrefabName = targetSmallShip.prefabName;
            }
            else if (targetLargeShip != null)
            {
                smallShip.targetingSystem.targetSmallShip = null;
                smallShip.targetingSystem.targetLargeShip = targetLargeShip;
                smallShip.targetingSystem.targetPrefabName = targetLargeShip.prefabName;
            }

            smallShip.targetingSystem.targetRigidbody = target.GetComponent<Rigidbody>();
        }

        if (smallShip.isAI == false)
        {
            AudioFunctions.PlayAudioClip(smallShip.audioManager, "beep01_toggle", "Cockpit", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);
        }

        //This prevents the torpedo from immediately locking on to the new target
        smallShip.torpedoSystem.torpedoLockedOn = false;
        smallShip.torpedoSystem.torpedoLockingOn = false;
    }

    //This creates the ship waypoint
    public static void CreateWaypoint_SmallShipPlayer(FlightControlSystem_Small smallShip = null)
    {
        if (smallShip.targetingSystem.waypoint == null)
        {
            Scene scene = SceneFunctions.GetScene();
            smallShip.targetingSystem.waypoint = new GameObject();
            smallShip.targetingSystem.waypoint.name = "waypoint_" + smallShip.name;
            smallShip.targetingSystem.waypoint.transform.SetParent(scene.transform);
        }
    }

    //This checks whether a target is hostile or not
    public static bool GetHostility_SmallShipPlayer(FlightControlSystem_Small smallShip = null, string targetAllegiance = "none")
    {
        bool isHostile = false;

        if (smallShip.scene != null)
        {
            if (smallShip.scene.allegiance == "none")
            {
                TextAsset allegiancesFile = Resources.Load(OGGetAddress.files + "Allegiances") as TextAsset;
                smallShip.scene.allegiance = allegiancesFile.text;
            }

            Allegiances allegiances = JsonUtility.FromJson<Allegiances>(smallShip.scene.allegiance);

            Allegiance allegiance = null;

            foreach (Allegiance tempAllegiance in allegiances.allegianceData)
            {
                if (tempAllegiance.allegiance == smallShip.allegiance)
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
    public static int GetNextTargetNo_SmallShipPlayer(FlightControlSystem_Small smallShip)
    {
        Scene scene = smallShip.scene;
        int targetNumber = 0;

        if (scene != null)
        {
            if (scene.objectPool != null)
            {
                if (smallShip.targetingSystem.target != null)
                {
                    for (int i = 0; i < scene.objectPool.Count; i++)
                    {
                        if (scene.objectPool[i] != null)
                        {
                            if (scene.objectPool[i].name == smallShip.targetingSystem.target.name)
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
    public static IEnumerator GetClosestEnemySmallShip_SmallShipAI(FlightControlSystem_Small smallShip)
    {
        Scene scene = smallShip.scene;

        GameObject target = null;
        FlightControlSystem_Small targetSmallShip = null;

        float distance = Mathf.Infinity;

        //This checks for the closest small ship first
        foreach (FlightControlSystem_Small tempSmallShip in scene.smallShips)
        {
            if (tempSmallShip != null & smallShip != null)
            {
                if (tempSmallShip.gameObject.activeSelf == true & tempSmallShip != null)
                {
                    bool isHostile = GetHostility_SmallShipPlayer(smallShip, tempSmallShip.allegiance);

                    if (isHostile == true & smallShip.isDisabled == false & smallShip.docking == false)
                    {
                        float tempDistance = Vector3.Distance(tempSmallShip.transform.position, smallShip.gameObject.transform.position);

                        if (tempDistance < distance & tempSmallShip.targetingSystem.numberTargeting < 3)
                        {
                            target = tempSmallShip.gameObject;
                            targetSmallShip = tempSmallShip;
                            distance = tempDistance;
                        }
                    }
                }

            }
        }

        if (target != null)
        {
            smallShip.targetingSystem.target = target;
            smallShip.targetingSystem.targetName = target.name;

            if (targetSmallShip != null)
            {
                smallShip.targetingSystem.targetSmallShip = targetSmallShip;
                smallShip.targetingSystem.targetLargeShip = null;
                smallShip.targetingSystem.targetPrefabName = targetSmallShip.prefabName;

                if (smallShip.isAI == true)
                {
                    targetSmallShip.targetingSystem.numberTargeting += 1;
                }
            }

            smallShip.targetingSystem.targetRigidbody = target.GetComponent<Rigidbody>();
        }

        //This prevents the torpedo from immediately locking on to the new target
        smallShip.torpedoSystem.torpedoLockedOn = false;
        smallShip.torpedoSystem.torpedoLockingOn = false;

        yield return null;
    }

    //This gets the closesd enemy target
    public static IEnumerator GetClosestEnemyLargeShip_SmallShipAI(FlightControlSystem_Small smallShip)
    {
        Scene scene = smallShip.scene;

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
                        bool isHostile = GetHostility_SmallShipPlayer(smallShip, tempLargeShip.allegiance);

                        if (isHostile == true)
                        {
                            float tempDistance = Vector3.Distance(tempLargeShip.transform.position, smallShip.gameObject.transform.position);

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
            smallShip.targetingSystem.target = target;
            smallShip.targetingSystem.targetName = target.name;

            if (targetLargeShip != null)
            {
                smallShip.targetingSystem.targetSmallShip = null;
                smallShip.targetingSystem.targetLargeShip = targetLargeShip;
                smallShip.targetingSystem.targetPrefabName = targetLargeShip.prefabName;
            }

            smallShip.targetingSystem.targetRigidbody = target.GetComponent<Rigidbody>();
        }

        //This prevents the torpedo from immediately locking on to the new target
        smallShip.torpedoSystem.torpedoLockedOn = false;
        smallShip.torpedoSystem.torpedoLockingOn = false;

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
        FlightControlSystem_Small tempSmallShip = null;
        FlightControlSystem_Small targetSmallShip = null;
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
                        tempSmallShip = ship.GetComponent<FlightControlSystem_Small>();
                        tempLargeShip = ship.GetComponent<LargeShip>();

                        if (tempSmallShip != null)
                        {
                            targetSmallShip = tempSmallShip;
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

            if (targetSmallShip != null)
            {
                largeShip.targetSmallShip = targetSmallShip;
                largeShip.targetLargeShip = null;
                largeShip.targetPrefabName = targetSmallShip.prefabName;
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
                    largeShip.targetSpeed = largeShip.targetSmallShip.thrustSpeed;
                    largeShip.targetHull = largeShip.targetSmallShip.hullLevel;
                    largeShip.targetShield = largeShip.targetSmallShip.shieldLevel;
                    largeShip.targetType = largeShip.targetSmallShip.type;
                    largeShip.targetPrefabName = largeShip.targetSmallShip.prefabName;
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
        FlightControlSystem_Small tempSmallShip = null;

        float distance = Mathf.Infinity;

        //If the target is still null it looks for the closest large ship
        if (target == null)
        {
            foreach (GameObject ship in scene.objectPool)
            {
                if (ship != null)
                {
                    tempSmallShip = ship.GetComponent<FlightControlSystem_Small>();

                    if (ship.activeSelf == true & tempSmallShip != null)
                    {
                        bool isHostile = GetHostility_Turret(scene, allegiance, tempSmallShip.allegiance);

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
            scene.smallShips = new List<FlightControlSystem_Small>();
        }

        foreach (FlightControlSystem_Small smallShip in scene.smallShips.ToArray())
        {
            if (smallShip != null)
            {
                if (smallShip.gameObject.activeSelf == true & smallShip.requestingTarget == true)
                {
                    if (smallShip.aiTargetingMode == "targetallprefsmall")
                    {
                        Task a = new Task(GetClosestEnemySmallShip_SmallShipAI(smallShip));
                        while (a.Running == true) { yield return null; }

                        if (smallShip.targetingSystem.target == null)
                        {
                            Task b = new Task(GetClosestEnemyLargeShip_SmallShipAI(smallShip));
                            while (b.Running == true) { yield return null; }
                        }
                    }
                    else if (smallShip.aiTargetingMode == "targetallpreflarge")
                    {
                        Task b = new Task(GetClosestEnemyLargeShip_SmallShipAI(smallShip));
                        while (b.Running == true) { yield return null; }

                        if (smallShip.targetingSystem.target == null)
                        {
                            Task a = new Task(GetClosestEnemySmallShip_SmallShipAI(smallShip));
                            while (a.Running == true) { yield return null; }
                        }
                    }
                    else if (smallShip.aiTargetingMode == "targetsmallshipsonly")
                    {
                        Task a = new Task(GetClosestEnemySmallShip_SmallShipAI(smallShip));
                        while (a.Running == true) { yield return null; }
                    }
                    else if (smallShip.aiTargetingMode == "targetlargeshipsonly")
                    {
                        Task a = new Task(GetClosestEnemyLargeShip_SmallShipAI(smallShip));
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
