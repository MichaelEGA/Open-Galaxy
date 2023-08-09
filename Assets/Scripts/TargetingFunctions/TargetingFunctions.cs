using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TargetingFunctions 
{
    #region small ship targetting

    //This gets the next target of any kind
    public static void GetNextTarget(SmallShip smallShip = null)
    {
        bool automaticSearch = false;

        if (smallShip.target != null)
        {
            if (smallShip.target.activeSelf == false)
            {
                automaticSearch = true;
            }
        }

        if (smallShip.targetPressedTime < Time.time & smallShip.getNextTarget == true || automaticSearch == true)
        {
            Scene scene = smallShip.scene;
            int countStart = 0;

            //This sets the count start according to the current target the ship has selected
            if (smallShip.targetNumber > scene.objectPool.Count - 1)
            {
                countStart = 0;
            }
            else
            {
                countStart = smallShip.targetNumber;
            }

            //This cycles through all the possible targets ignoring objects that are null or inactive
            for (int i = countStart; i <= scene.objectPool.Count; i++)
            {
                if (i > scene.objectPool.Count - 1) //This clears the target at the end of the list
                {
                    smallShip.target = null;
                    smallShip.targetName = " ";
                    smallShip.targetNumber = i;
                    smallShip.targetSmallShip = null;
                    smallShip.targetRigidbody = null;
                    smallShip.targetPrefabName = " ";
                    break;
                }
                else if (scene.objectPool[i] != null & smallShip.targetNumber != i) //This gets any type of ship in the scene
                {
                    if (scene.objectPool[i].activeSelf == true) //This ignores objects that are inactive
                    {
                        smallShip.target = scene.objectPool[i];
                        smallShip.targetName = scene.objectPool[i].name;

                        SmallShip targetSmallShip = scene.objectPool[i].GetComponent<SmallShip>();

                        if (targetSmallShip != null)
                        {
                            smallShip.targetSmallShip = targetSmallShip;
                            smallShip.targetPrefabName = targetSmallShip.prefabName;
                        }
                        else
                        {
                            smallShip.targetSmallShip = null;
                            smallShip.targetPrefabName = " ";
                        }

                        smallShip.targetRigidbody = scene.objectPool[i].GetComponent<Rigidbody>();
                        smallShip.targetNumber = i;
                        break;
                    }
                }
            }

            smallShip.targetPressedTime = Time.time + 0.2f;

            if (smallShip.isAI == false)
            {
                AudioFunctions.PlayAudioClip(smallShip.audioManager, "beep01_toggle", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);
            }

            //This prevents the torpedo from immediately locking on to the new target
            smallShip.torpedoLockedOn = false;
            smallShip.torpedoLockingOn = false;

        }
    }

    //This gets the next enemy target
    public static void GetNextEnemy(SmallShip smallShip = null)
    {
        bool automaticSearch = false;

        if (smallShip.target != null)
        {
            if (smallShip.target.activeSelf == false)
            {
                automaticSearch = true;
            }
        }

        if (smallShip.targetPressedTime < Time.time & smallShip.getNextEnemy == true || automaticSearch == true)
        {
            Scene scene = smallShip.scene;
            int countStart = 0;

            //This sets the count start according to the current target the ship has selected
            if (smallShip.targetNumber > scene.objectPool.Count - 1)
            {
                countStart = 0;
            }
            else
            {
                countStart = smallShip.targetNumber;
            }

            for (int i = countStart; i <= scene.objectPool.Count; i++)
            {
                if (i > scene.objectPool.Count - 1) //This clears the target at the end of the list
                {
                    smallShip.target = null;
                    smallShip.targetName = " ";
                    smallShip.targetNumber = i;
                    smallShip.targetSmallShip = null;
                    smallShip.targetRigidbody = null;
                    smallShip.targetPrefabName = " ";
                    break;
                }
                else if (scene.objectPool[i] != null & smallShip.targetNumber != i) //This gets enemy ships in the scene
                {
                    if (scene.objectPool[i].activeSelf == true) //This ignores objects that are inactive
                    {
                        bool isHostile = false;
                        int numberTargetting = 0;
                        SmallShip targetSmallShip = scene.objectPool[i].GetComponent<SmallShip>();

                        if (targetSmallShip != null)
                        {
                            numberTargetting = targetSmallShip.numberTargeting;
                            isHostile = GetHostility(smallShip, targetSmallShip.allegiance);
                        }

                        if (isHostile == true)
                        {

                            if (smallShip.isAI == true & numberTargetting <= 1)
                            {
                                smallShip.target = scene.objectPool[i];
                                smallShip.targetName = scene.objectPool[i].name;
                                smallShip.targetNumber = i;

                                if (targetSmallShip != null)
                                {
                                    smallShip.targetSmallShip = targetSmallShip;
                                    smallShip.targetPrefabName = targetSmallShip.prefabName;
                                }
                                else
                                {
                                    smallShip.targetSmallShip = null;
                                    smallShip.targetPrefabName = " ";
                                }

                                smallShip.targetRigidbody = scene.objectPool[i].GetComponent<Rigidbody>();
                                targetSmallShip.numberTargeting += 1;
                                break;
                            }
                            else if (smallShip.isAI == false)
                            {
                                smallShip.target = scene.objectPool[i];
                                smallShip.targetName = scene.objectPool[i].name;
                                smallShip.targetNumber = i;

                                if (targetSmallShip != null)
                                {
                                    smallShip.targetSmallShip = targetSmallShip;
                                    smallShip.targetPrefabName = targetSmallShip.prefabName;
                                }
                                else
                                {
                                    smallShip.targetSmallShip = null;
                                    smallShip.targetPrefabName = " ";
                                }

                                smallShip.targetRigidbody = scene.objectPool[i].GetComponent<Rigidbody>();
                                break;
                            }

                        }

                    }
                }
            }

            smallShip.targetPressedTime = Time.time + 0.2f;

            if (smallShip.isAI == false)
            {
                AudioFunctions.PlayAudioClip(smallShip.audioManager, "beep01_toggle", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);
            }

            //This prevents the torpedo from immediately locking on to the new target
            smallShip.torpedoLockedOn = false;
            smallShip.torpedoLockingOn = false;

        }
    }

    //This gets the closesd enemy target
    public static void GetClosestEnemy(SmallShip smallShip = null, bool externalActivation = false)
    {

        bool automaticSearch = false;

        if (smallShip.target != null)
        {
            if (smallShip.target.activeSelf == false)
            {
                automaticSearch = true;
            }
        }

        if (smallShip.targetPressedTime < Time.time & smallShip.getClosestEnemy == true || automaticSearch == true || externalActivation == true)
        {
            Scene scene = smallShip.scene;

            GameObject target = null;
            SmallShip tempSmallShip = null;
            SmallShip targetSmallShip = null;

            float distance = Mathf.Infinity;

            foreach (GameObject ship in scene.objectPool)
            {
                if (ship != null)
                {
                    tempSmallShip = ship.GetComponent<SmallShip>();

                    if (ship.activeSelf == true & tempSmallShip != null)
                    {
                        bool isHostile = GetHostility(smallShip, tempSmallShip.allegiance);

                        if (isHostile == true)
                        {
                            float tempDistance = Vector3.Distance(ship.transform.position, smallShip.gameObject.transform.position);

                            if (tempDistance < distance)
                            {
                                float numberTargetting = tempSmallShip.numberTargeting;

                                if (smallShip.isAI == true & smallShip.targetNumber <= 1)
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

            if (target != null)
            {
                smallShip.target = target;
                smallShip.targetName = target.name;

                if (targetSmallShip != null)
                {
                    smallShip.targetSmallShip = targetSmallShip;
                    smallShip.targetPrefabName = targetSmallShip.prefabName;
                }
                else
                {
                    smallShip.targetSmallShip = null;
                    smallShip.targetPrefabName = " ";
                }

                smallShip.targetRigidbody = target.GetComponent<Rigidbody>();

                if (smallShip.isAI == true)
                {
                    targetSmallShip.numberTargeting += 1;
                }
            }

            smallShip.targetPressedTime = Time.time + 0.2f;

            if (smallShip.isAI == false)
            {
                AudioFunctions.PlayAudioClip(smallShip.audioManager, "beep01_toggle", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);
            }

            //This prevents the torpedo from immediately locking on to the new target
            smallShip.torpedoLockedOn = false;
            smallShip.torpedoLockingOn = false;

        }
    }

    //This gets the designated target and sets it as the ships target if it can be found
    public static void GetSpecificTarget(SmallShip smallShip = null, string targetName = "none")
    {
        Scene scene = smallShip.scene;
        GameObject target = null;
        SmallShip tempSmallShip = null;
        SmallShip targetSmallShip = null;

        foreach (GameObject ship in scene.objectPool)
        {
            if (ship != null)
            {
                if (ship.activeSelf == true)
                {
                    if (ship.name.Contains(targetName))
                    {
                        target = ship;
                        tempSmallShip = ship.GetComponent<SmallShip>();

                        if (tempSmallShip != null)
                        {
                            targetSmallShip = tempSmallShip;
                        }

                        break;
                    }
                }
            }
        }

        if (target != null)
        {
            smallShip.target = target;
            smallShip.targetName = target.name;

            if (targetSmallShip != null)
            {
                smallShip.targetSmallShip = targetSmallShip;
                smallShip.targetPrefabName = targetSmallShip.prefabName;
            }
            else
            {
                smallShip.targetSmallShip = null;
                smallShip.targetPrefabName = " ";
            }

            smallShip.targetRigidbody = target.GetComponent<Rigidbody>();

            if (smallShip.isAI == true)
            {
                targetSmallShip.numberTargeting += 1;
            }
        }

        if (smallShip.isAI == false)
        {
            AudioFunctions.PlayAudioClip(smallShip.audioManager, "beep01_toggle", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);
        }

        //This prevents the torpedo from immediately locking on to the new target
        smallShip.torpedoLockedOn = false;
        smallShip.torpedoLockingOn = false;

    }

    //This draws key data/info from the target including relative position, distance, and hostility
    public static void GetTargetInfo(SmallShip smallShip = null)
    {
        Transform shipTransform = smallShip.gameObject.transform;
        Vector3 shipPosition = shipTransform.position;

        if (smallShip.target != null)
        {
            if (smallShip.target.activeSelf == true)
            {
                if (smallShip.targetSmallShip != null)
                {
                    smallShip.targetAllegiance = smallShip.targetSmallShip.allegiance;
                    smallShip.targetSpeed = smallShip.targetSmallShip.thrustSpeed;
                    smallShip.targetHull = smallShip.targetSmallShip.hullLevel;
                    smallShip.targetShield = smallShip.targetSmallShip.shieldLevel;
                    smallShip.targetType = smallShip.targetSmallShip.type;
                    smallShip.targetPrefabName = smallShip.targetSmallShip.prefabName;
                }

                Transform targetTransform = smallShip.target.transform;
                Vector3 targetPosition = targetTransform.position;
                Vector3 targetVelocity = new Vector3(0, 0, 0);

                if (smallShip.targetRigidbody != null)
                {
                    targetVelocity = smallShip.targetRigidbody.velocity;
                }

                Vector3 targetRelativePosition = targetPosition - shipPosition;

                smallShip.targetDistance = Vector3.Distance(shipPosition, targetPosition);
                smallShip.targetForward = Vector3.Dot(shipTransform.forward, targetRelativePosition.normalized);
                smallShip.targetRight = Vector3.Dot(shipTransform.right, targetRelativePosition.normalized);
                smallShip.targetUp = Vector3.Dot(shipTransform.up, targetRelativePosition.normalized);

                Vector3 targettingErrorMargin = SmallShipAIFunctions.TargetingErrorMargin(smallShip);
                float distanceToIntercept = ((targetPosition) - shipPosition).magnitude / 500f;
                Vector3 interceptPosition = (targetPosition + targettingErrorMargin) + targetVelocity * distanceToIntercept;

                Vector3 interceptRelativePosition = interceptPosition - shipPosition;

                smallShip.interceptDistance = Vector3.Distance(interceptPosition, shipPosition);
                smallShip.interceptForward = Vector3.Dot(shipTransform.forward, interceptRelativePosition.normalized);
                smallShip.interceptRight = Vector3.Dot(shipTransform.right, interceptRelativePosition.normalized);
                smallShip.interceptUp = Vector3.Dot(shipTransform.up, interceptRelativePosition.normalized);
            }
        }
        else
        {
            smallShip.interceptDistance = 250;
        }

        if (smallShip.waypoint != null)
        {
            Vector3 waypointPosition = smallShip.waypoint.transform.position;
            Vector3 waypointRelativePosition = waypointPosition - shipPosition;

            smallShip.waypointDistance = Vector3.Distance(shipPosition, waypointPosition);
            smallShip.waypointForward = Vector3.Dot(shipTransform.forward, waypointRelativePosition.normalized);
            smallShip.waypointRight = Vector3.Dot(shipTransform.right, waypointRelativePosition.normalized);
            smallShip.waypointUp = Vector3.Dot(shipTransform.up, waypointRelativePosition.normalized);
        }

    }

    //This creates the ship waypoint
    public static void CreateWaypoint(SmallShip smallShip = null)
    {
        if (smallShip.waypoint == null)
        {
            Scene scene = SceneFunctions.GetScene();
            smallShip.waypoint = new GameObject();
            smallShip.waypoint.name = "waypoint_" + smallShip.name;
            smallShip.waypoint.transform.SetParent(scene.transform);
        }
    }

    //This checks whether a target is hostile or not
    public static bool GetHostility(SmallShip smallShip = null, string targetAllegiance = "none")
    {

        bool isHostile = false;

        //This gets the Json ship data
        TextAsset allegiancesFile = Resources.Load("Data/Files/Allegiances") as TextAsset;
        Allegiances allegiances = JsonUtility.FromJson<Allegiances>(allegiancesFile.text);

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

        return isHostile;

    }

    #endregion

    #region small ship targetting

    //This gets the next target of any kind
    public static void GetNextTarget_LargeShip(LargeShip largeShip = null)
    {
        bool automaticSearch = false;

        if (largeShip.target != null)
        {
            if (largeShip.target.activeSelf == false)
            {
                automaticSearch = true;
            }
        }

        if (largeShip.targetPressedTime < Time.time & largeShip.getNextTarget == true || automaticSearch == true)
        {
            Scene scene = largeShip.scene;
            int countStart = 0;

            //This sets the count start according to the current target the ship has selected
            if (largeShip.targetNumber > scene.objectPool.Count - 1)
            {
                countStart = 0;
            }
            else
            {
                countStart = largeShip.targetNumber;
            }

            //This cycles through all the possible targets ignoring objects that are null or inactive
            for (int i = countStart; i <= scene.objectPool.Count; i++)
            {
                if (i > scene.objectPool.Count - 1) //This clears the target at the end of the list
                {
                    largeShip.target = null;
                    largeShip.targetName = " ";
                    largeShip.targetNumber = i;
                    largeShip.targetSmallShip = null;
                    largeShip.targetRigidbody = null;
                    largeShip.targetPrefabName = " ";
                    break;
                }
                else if (scene.objectPool[i] != null & largeShip.targetNumber != i) //This gets any type of ship in the scene
                {
                    if (scene.objectPool[i].activeSelf == true) //This ignores objects that are inactive
                    {
                        largeShip.target = scene.objectPool[i];
                        largeShip.targetName = scene.objectPool[i].name;

                        SmallShip targetSmallShip = scene.objectPool[i].GetComponent<SmallShip>();

                        if (targetSmallShip != null)
                        {
                            largeShip.targetSmallShip = targetSmallShip;
                            largeShip.targetPrefabName = targetSmallShip.prefabName;
                        }
                        else
                        {
                            largeShip.targetSmallShip = null;
                            largeShip.targetPrefabName = " ";
                        }

                        largeShip.targetRigidbody = scene.objectPool[i].GetComponent<Rigidbody>();
                        largeShip.targetNumber = i;
                        break;
                    }
                }
            }

            largeShip.targetPressedTime = Time.time + 0.2f;
        }
    }

    //This gets the next enemy target
    public static void GetNextEnemy_LargeShip(LargeShip largeShip = null)
    {
        bool automaticSearch = false;

        if (largeShip.target != null)
        {
            if (largeShip.target.activeSelf == false)
            {
                automaticSearch = true;
            }
        }

        if (largeShip.targetPressedTime < Time.time & largeShip.getNextEnemy == true || automaticSearch == true)
        {
            Scene scene = largeShip.scene;
            int countStart = 0;

            //This sets the count start according to the current target the ship has selected
            if (largeShip.targetNumber > scene.objectPool.Count - 1)
            {
                countStart = 0;
            }
            else
            {
                countStart = largeShip.targetNumber;
            }

            for (int i = countStart; i <= scene.objectPool.Count; i++)
            {
                if (i > scene.objectPool.Count - 1) //This clears the target at the end of the list
                {
                    largeShip.target = null;
                    largeShip.targetName = " ";
                    largeShip.targetNumber = i;
                    largeShip.targetSmallShip = null;
                    largeShip.targetRigidbody = null;
                    largeShip.targetPrefabName = " ";
                    break;
                }
                else if (scene.objectPool[i] != null & largeShip.targetNumber != i) //This gets enemy ships in the scene
                {
                    if (scene.objectPool[i].activeSelf == true) //This ignores objects that are inactive
                    {
                        bool isHostile = false;
                        int numberTargetting = 0;
                        SmallShip targetSmallShip = scene.objectPool[i].GetComponent<SmallShip>();

                        if (targetSmallShip != null)
                        {
                            numberTargetting = targetSmallShip.numberTargeting;
                            isHostile = GetHostility_LargeShip(largeShip, targetSmallShip.allegiance);
                        }

                        if (isHostile == true)
                        {
                            if (numberTargetting <= 1)
                            {
                                largeShip.target = scene.objectPool[i];
                                largeShip.targetName = scene.objectPool[i].name;
                                largeShip.targetNumber = i;

                                if (targetSmallShip != null)
                                {
                                    largeShip.targetSmallShip = targetSmallShip;
                                    largeShip.targetPrefabName = targetSmallShip.prefabName;
                                }
                                else
                                {
                                    largeShip.targetSmallShip = null;
                                    largeShip.targetPrefabName = " ";
                                }

                                largeShip.targetRigidbody = scene.objectPool[i].GetComponent<Rigidbody>();
                                targetSmallShip.numberTargeting += 1;
                                break;
                            }
                        }
                    }
                }
            }

            largeShip.targetPressedTime = Time.time + 0.2f;
        }
    }

    //This gets the closesd enemy target
    public static void GetClosestEnemy_LargeShip(LargeShip largeShip = null, bool externalActivation = false)
    {

        bool automaticSearch = false;

        if (largeShip.target != null)
        {
            if (largeShip.target.activeSelf == false)
            {
                automaticSearch = true;
            }
        }

        if (largeShip.targetPressedTime < Time.time & largeShip.getClosestEnemy == true || automaticSearch == true || externalActivation == true)
        {
            Scene scene = largeShip.scene;

            GameObject target = null;
            SmallShip tempSmallShip = null;
            SmallShip targetSmallShip = null;

            float distance = Mathf.Infinity;

            foreach (GameObject ship in scene.objectPool)
            {
                if (ship != null)
                {
                    tempSmallShip = ship.GetComponent<SmallShip>();

                    if (ship.activeSelf == true & tempSmallShip != null)
                    {
                        bool isHostile = GetHostility_LargeShip(largeShip, tempSmallShip.allegiance);

                        if (isHostile == true)
                        {
                            float tempDistance = Vector3.Distance(ship.transform.position, largeShip.gameObject.transform.position);

                            if (tempDistance < distance)
                            {
                                float numberTargetting = tempSmallShip.numberTargeting;

                                if (largeShip.targetNumber <= 1)
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

            if (target != null)
            {
                largeShip.target = target;
                largeShip.targetName = target.name;

                if (targetSmallShip != null)
                {
                    largeShip.targetSmallShip = targetSmallShip;
                    largeShip.targetPrefabName = targetSmallShip.prefabName;
                }
                else
                {
                    largeShip.targetSmallShip = null;
                    largeShip.targetPrefabName = " ";
                }

                largeShip.targetRigidbody = target.GetComponent<Rigidbody>();

                targetSmallShip.numberTargeting += 1;
            }

            largeShip.targetPressedTime = Time.time + 0.2f;
        }
    }

    //This gets the designated target and sets it as the ships target if it can be found
    public static void GetSpecificTarget_LargeShip(LargeShip largeShip = null, string targetName = "none")
    {
        Scene scene = largeShip.scene;
        GameObject target = null;
        SmallShip tempSmallShip = null;
        SmallShip targetSmallShip = null;

        foreach (GameObject ship in scene.objectPool)
        {
            if (ship != null)
            {
                if (ship.activeSelf == true)
                {
                    if (ship.name.Contains(targetName))
                    {
                        target = ship;
                        tempSmallShip = ship.GetComponent<SmallShip>();

                        if (tempSmallShip != null)
                        {
                            targetSmallShip = tempSmallShip;
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
                largeShip.targetPrefabName = targetSmallShip.prefabName;
            }
            else
            {
                largeShip.targetSmallShip = null;
                largeShip.targetPrefabName = " ";
            }

            largeShip.targetRigidbody = target.GetComponent<Rigidbody>();

            targetSmallShip.numberTargeting += 1;          
        }
    }

    //This draws key data/info from the target including relative position, distance, and hostility
    public static void GetTargetInfo_LargeShip(LargeShip largeShip = null)
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

                Transform targetTransform = largeShip.target.transform;
                Vector3 targetPosition = targetTransform.position;
                Vector3 targetVelocity = new Vector3(0, 0, 0);

                if (largeShip.targetRigidbody != null)
                {
                    targetVelocity = largeShip.targetRigidbody.velocity;
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
    public static void CreateWaypoint_LargeShip(LargeShip largeShip = null)
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
    public static bool GetHostility_LargeShip(LargeShip largeShip = null, string targetAllegiance = "none")
    {

        bool isHostile = false;

        //This gets the Json ship data
        TextAsset allegiancesFile = Resources.Load("Data/Files/Allegiances") as TextAsset;
        Allegiances allegiances = JsonUtility.FromJson<Allegiances>(allegiancesFile.text);

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

        return isHostile;

    }

    #endregion
}
