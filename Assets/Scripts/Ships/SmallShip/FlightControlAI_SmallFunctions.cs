using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//These functions are called by the small ship functions script
public static class FlightControlAI_SmallFunctions
{
    #region Base AI Functions

    //This is the base function that calls all the other AI functions except collisions which is called by an external script (evasions are called internally though)
    public static void GetAIInput(FlightControlAI_Small flightControlAI)
    {
        FlightControlSystem_Small flightControlSystem = flightControlAI.ship.flightControlSystem_Small;

        if (flightControlAI.isAI == true & flightControlSystem.automaticRotationTurnAround == false & flightControlSystem.automaticRotationSpin == false & flightControlSystem.controlLock == false)
        {
            if (flightControlAI != null)
            {
                //This adds all the default ai tags when the ship is first run
                if (flightControlAI.aiStarted == false)
                {
                    AddDefaultTags(flightControlAI);
                    flightControlAI.aiStarted = true;
                }

                //This checks if the ship needs to request a new target
                ClearTarget(flightControlAI);
                RequestTarget(flightControlAI);

                //This runs all the ai functions
                RunTags(flightControlAI);
            }
        }
            
    }

    #endregion

    #region AI Tagging System

    //This adds the default ai tags
    public static void AddDefaultTags(FlightControlAI_Small flightControlAI)
    {
        if (flightControlAI != null)
        {
            AddTag(flightControlAI, "threequarterspeed");
            AddTag(flightControlAI, "singlelaser");
            AddTag(flightControlAI, "lowaccuracy");
            AddTag(flightControlAI, "chasewithdraw");
            AddTag(flightControlAI, "resetenergylevels");
            AddTag(flightControlAI, "targetallprefsmall");
            AddTag(flightControlAI, "collisionevasiongeg");
        }
    }

    //This adds an ai tag and removes conflicting tags using the two functions below
    public static void AddTag(FlightControlAI_Small flightControlAI, string tag)
    {
        if (flightControlAI != null)
        {
            if (tag == "matchspeed" || tag == "fullspeedwithboost" || tag == "fullspeed" || tag == "threequarterspeed" || tag == "halfspeed" || tag == "quarterspeed" || tag == "dynamicspeed" || tag == "nospeed")
            {
                RemoveSingleTag(flightControlAI, "fullspeedwithboost");
                RemoveSingleTag(flightControlAI, "fullspeed");
                RemoveSingleTag(flightControlAI, "threequarterspeed");
                RemoveSingleTag(flightControlAI, "halfspeed");
                RemoveSingleTag(flightControlAI, "quarterspeed");
                RemoveSingleTag(flightControlAI, "dynamicspeed");
                RemoveSingleTag(flightControlAI, "nospeed");

            }
            else if (tag == "singlelaser" || tag == "duallasers" || tag == "alllasers" || tag == "rapidlasers" || tag == "singleplasma" || tag == "dualplasma" || tag == "allplasma" || tag == "singleion" || tag == "dualion" || tag == "allion" || tag == "rapidion" || tag == "singletorpedo" || tag == "dualtorpedos" || tag == "noweapons" || tag == "dynamicweapons_single" || tag == "dynamicweapons_dual" || tag == "dynamicweapons_all" || tag == "dynamicweapons_rapid")
            {
                RemoveSingleTag(flightControlAI, "singlelaser");
                RemoveSingleTag(flightControlAI, "duallasers");
                RemoveSingleTag(flightControlAI, "alllasers");
                RemoveSingleTag(flightControlAI, "rapidlasers");
                RemoveSingleTag(flightControlAI, "singleplasma");
                RemoveSingleTag(flightControlAI, "dualplasma");
                RemoveSingleTag(flightControlAI, "allplasma");
                RemoveSingleTag(flightControlAI, "singleion");
                RemoveSingleTag(flightControlAI, "dualion");
                RemoveSingleTag(flightControlAI, "allion");
                RemoveSingleTag(flightControlAI, "rapidion");
                RemoveSingleTag(flightControlAI, "singletorpedo");
                RemoveSingleTag(flightControlAI, "dualtorpedos");
                RemoveSingleTag(flightControlAI, "alltorpedos");
                RemoveSingleTag(flightControlAI, "dynamicweapons_single");
                RemoveSingleTag(flightControlAI, "dynamicweapons_dual");
                RemoveSingleTag(flightControlAI, "dynamicweapons_all");
                RemoveSingleTag(flightControlAI, "dynamicweapons_rapid");
                RemoveSingleTag(flightControlAI, "noweapons");
            }
            else if (tag == "lowaccuracy" || tag == "mediumaccuracy" || tag == "highaccuracy")
            {
                RemoveSingleTag(flightControlAI, "lowaccuracy");
                RemoveSingleTag(flightControlAI, "mediumaccuracy");
                RemoveSingleTag(flightControlAI, "highaccuracy");
            }
            else if (tag == "chase" || tag == "chasewithdraw" || tag == "strafewithdraw" || tag == "movetowaypoint" || tag == "patrolrandom" || tag == "norotation" || tag == "formationflying")
            {
                RemoveSingleTag(flightControlAI, "chase");
                RemoveSingleTag(flightControlAI, "chasewithdraw");
                RemoveSingleTag(flightControlAI, "strafewithdraw");
                RemoveSingleTag(flightControlAI, "movetowaypoint");
                RemoveSingleTag(flightControlAI, "patrolrandom");
                RemoveSingleTag(flightControlAI, "formationflying");
                RemoveSingleTag(flightControlAI, "norotation");
                flightControlAI.flyInFormation = false; //This deactivates formation flying when flying pattern is changed.
            }
            else if (tag == "resetenergylevels" || tag == "energytoshields" || tag == "energytoengines" || tag == "energytolasers" || tag == "energyprotective" || tag == "energyaggressive" || tag == "energydynamic")
            {
                RemoveSingleTag(flightControlAI, "resetenergylevels");
                RemoveSingleTag(flightControlAI, "energytoshields");
                RemoveSingleTag(flightControlAI, "energytoengines");
                RemoveSingleTag(flightControlAI, "energytolasers");
                RemoveSingleTag(flightControlAI, "energyprotective");
                RemoveSingleTag(flightControlAI, "energyaggressive");
                RemoveSingleTag(flightControlAI, "energydynamic");
            }
            else if (tag == "targetallprefsmall" || tag == "targetallpreflarge" || tag == "targetsmallshipsonly" || tag == "targetlargeshipsonly")
            {
                RemoveSingleTag(flightControlAI, "targetallprefsmall");
                RemoveSingleTag(flightControlAI, "targetallpreflarge");
                RemoveSingleTag(flightControlAI, "targetsmallshipsonly");
                RemoveSingleTag(flightControlAI, "targetlargeshipsonly");
            }
            else if (tag == "collisionevasion" || tag == "nocollisionevasion")
            {
                RemoveSingleTag(flightControlAI, "collisionevasion");
                RemoveSingleTag(flightControlAI, "nocollisionevasion");
            }


            AddSingleTag(flightControlAI, tag);
        }
    }

    //This adds an ai tag
    public static void AddSingleTag(FlightControlAI_Small flightControlAI, string tag)
    {
        if (flightControlAI != null)
        {
            //This checks the tag list exists
            if (flightControlAI.aiTags == null)
            {
                flightControlAI.aiTags = new List<string>();
            }

            //This adds the new tag
            if (flightControlAI.aiTags != null)
            {
                flightControlAI.aiTags.Add(tag);
            }
        }
    }

    //This removes an ai tag
    public static void RemoveSingleTag(FlightControlAI_Small flightControlAI, string tag)
    {
        if (flightControlAI != null)
        {
            //This checks the tag list exists
            if (flightControlAI.aiTags == null)
            {
                flightControlAI.aiTags = new List<string>();
            }

            //This removes the designated tag
            if (flightControlAI.aiTags != null)
            {
                //This removes the tag
                for (int i = 0; i < flightControlAI.aiTags.Count; i++)
                {
                    if (flightControlAI.aiTags[i] == tag)
                    {
                        flightControlAI.aiTags.RemoveAt(i);
                        break;
                    }
                }
            }
        }
    }

    //This runs the ai tags
    public static void RunTags(FlightControlAI_Small flightControlAI)
    {
        if (flightControlAI != null)
        {
            //This checks the tag list exists
            if (flightControlAI.aiTags == null)
            {
                flightControlAI.aiTags = new List<string>();
            }

            //This runs through the ship tags and runs the appropriate functions
            if (flightControlAI.aiTags != null)
            {
                foreach (string tag in flightControlAI.aiTags.ToArray())
                {
                    if (tag == "fullspeedwithboost") //Speed control
                    {
                        FullSpeedWithBoost(flightControlAI);
                    }
                    else if (tag == "fullspeed") //Speed control
                    {
                        FullSpeed(flightControlAI);
                    }
                    else if (tag == "threequarterspeed")
                    {
                        ThreeQuarterSpeed(flightControlAI);
                    }
                    else if (tag == "halfspeed")
                    {
                        HalfSpeed(flightControlAI);
                    }
                    else if (tag == "quarterspeed")
                    {
                        QuarterSpeed(flightControlAI);
                    }
                    else if (tag == "dynamicspeed")
                    {
                        DynamicSpeed(flightControlAI);
                    }
                    else if (tag == "nospeed")
                    {
                        NoSpeed(flightControlAI);
                    }
                    else if (tag == "singlelaser") //Weapon control
                    {
                        SingleLaser(flightControlAI);
                    }
                    else if (tag == "duallasers")
                    {
                        DualLasers(flightControlAI);
                    }
                    else if (tag == "alllasers")
                    {
                        AllLasers(flightControlAI);
                    }
                    else if (tag == "rapidlasers")
                    {
                        RapidLasers(flightControlAI);
                    }
                    else if (tag == "singleplasma")
                    {
                        SinglePlasma(flightControlAI);
                    }
                    else if (tag == "dualplasma")
                    {
                        DualPlasma(flightControlAI);
                    }
                    else if (tag == "allplasma")
                    {
                        AllPlasma(flightControlAI);
                    }
                    else if (tag == "singleion")
                    {
                        SingleIon(flightControlAI);
                    }
                    else if (tag == "dualion")
                    {
                        DualIon(flightControlAI);
                    }
                    else if (tag == "allion")
                    {
                        AllIon(flightControlAI);
                    }
                    else if (tag == "rapidion")
                    {
                        RapidIon(flightControlAI);
                    }
                    else if (tag == "singletorpedo")
                    {
                        SingleTorpedo(flightControlAI);
                    }
                    else if (tag == "dualtorpedos")
                    {
                        DualTorpedos(flightControlAI);
                    }
                    else if (tag == "alltorpedos")
                    {
                        AllTorpedos(flightControlAI);
                    }
                    else if (tag == "dynamicweapons_single")
                    {
                        DynamicWeapons_Single(flightControlAI);
                    }
                    else if (tag == "dynamicweapons_dual")
                    {
                        DynamicWeapons_Dual(flightControlAI);
                    }
                    else if (tag == "dynamicweapons_all")
                    {
                        DynamicWeapons_All(flightControlAI);
                    }
                    else if (tag == "dynamicweapons_rapid")
                    {
                        DynamicWeapons_Rapid(flightControlAI);
                    }
                    else if (tag == "noweapons")
                    {
                        //Do nothing
                    }
                    else if (tag == "lowaccuracy") //Weapon accuracy
                    {
                        LowAccuracy(flightControlAI);
                    }
                    else if (tag == "mediumaccuracy")
                    {
                        MediumAccuracy(flightControlAI);
                    }
                    else if (tag == "highaccuracy")
                    {
                        HighAccuracy(flightControlAI);
                    }
                    else if (tag == "chase") //Flight patterns
                    {
                        Chase(flightControlAI);
                    }
                    else if (tag == "chasewithdraw")
                    {
                        ChaseWithdraw(flightControlAI);
                    }
                    else if (tag == "strafewithdraw")
                    {
                        StrafeWithdraw(flightControlAI);
                    }
                    else if (tag == "movetowaypoint")
                    {
                        MoveToWayPoint(flightControlAI);
                    }
                    else if (tag == "patrolrandom")
                    {
                        PatrolRandom(flightControlAI);
                    }
                    else if (tag == "formationflying")
                    {
                        FormationFlying(flightControlAI);
                    }
                    else if (tag == "norotation")
                    {
                        NoRotation(flightControlAI);
                    }
                    else if (tag == "resetenergylevels") //Energy Management
                    {
                        ResetEnergyLevels(flightControlAI);
                    }
                    else if (tag == "energytoshields")
                    {
                        EnergyToShields(flightControlAI);
                    }
                    else if (tag == "energytoengines")
                    {
                        EnergyToEngines(flightControlAI);
                    }
                    else if (tag == "energytolasers")
                    {
                        EnergyToLasers(flightControlAI);
                    }
                    else if (tag == "energyprotective")
                    {
                        EnergyProtective(flightControlAI);
                    }
                    else if (tag == "energyaggressive")
                    {
                        EnergyAggressive(flightControlAI);
                    }
                    else if (tag == "energydynamic")
                    {
                        EnergyDynamic(flightControlAI);
                    }
                    else if (tag == "targetallprefsmall") //Targetting Preference
                    {
                        TargetAllPrefSmall(flightControlAI);
                    }
                    else if (tag == "targetallpreflarge")
                    {
                        TargetAllPrefLarge(flightControlAI);
                    }
                    else if (tag == "targetlargeshipsonly")
                    {
                        TargetLargeShipOnly(flightControlAI);
                    }
                    else if (tag == "targetsmallshipsonly")
                    {
                        TargetSmallShipsOnly(flightControlAI);
                    }
                }
            }
        }
    }

    //This checks if an ai tag exists
    public static bool TagExists(Ship ship, string tag)
    {
        bool exists = false;

        if (ship != null)
        {
            foreach (string tempTag in ship.flightControlAI_Small.aiTags.ToArray())
            {
                if (tempTag == tag)
                {
                    exists = true;
                    break;
                }
            }
        }

        return exists;
    }

    #endregion

    #region AI Speed Functions

    //This sets the ship at full speed
    public static void FullSpeedWithBoost(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.aiMatchSpeed == false)
            {
                if (smallShip.boostIsActive == true)
                {
                    smallShip.thrustInput = 1;
                }
                else if (smallShip.boostIsActive == false)
                {
                    if (smallShip.thrustSpeed > smallShip.speedRating)
                    {
                        smallShip.thrustInput = -1;
                    }
                    else
                    {
                        smallShip.thrustInput = 1;
                    }
                }

                //This prevents the ship using the boost until it reaches full
                float weplimit = 50;

                if (smallShip.powerMode == "engines")
                {
                    weplimit = 100;
                }

                if (smallShip.wepLevel >= weplimit)
                {
                    smallShip.boostIsActive = true;
                }
                else if (smallShip.wepLevel <= 0)
                {
                    smallShip.boostIsActive = false;
                }
            }
            else if (smallShip.targetingSystem.target != null & smallShip.flyInFormation == false || smallShip.followTarget != null & smallShip.flyInFormation == true)
            {
                MatchSpeed(smallShip);
            }
            else
            {
                HalfSpeed(smallShip);
            }
        }
    }

    //This sets the ship at full speed
    public static void FullSpeed(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.aiMatchSpeed == false)
            {
                if (smallShip.thrustSpeed > smallShip.speedRating)
                {
                    smallShip.thrustInput = -1;
                }
                else
                {
                    smallShip.thrustInput = 1;
                }
            }
            else if (smallShip.targetingSystem.target != null & smallShip.flyInFormation == false || smallShip.followTarget != null & smallShip.flyInFormation == true)
            {
                MatchSpeed(smallShip);
            }
            else
            {
                HalfSpeed(smallShip);
            }
        }
    }

    //This sets the ship to three quarter speed
    public static void ThreeQuarterSpeed(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.aiMatchSpeed == false)
            {
                float threeQuarterSpeed = (smallShip.speedRating / 4f) * 3;

                if (smallShip.thrustSpeed > threeQuarterSpeed)
                {
                    smallShip.thrustInput = -1;
                }
                else
                {
                    smallShip.thrustInput = 1;
                }
            }
            else if (smallShip.targetingSystem.target != null & smallShip.flyInFormation == false || smallShip.followTarget != null & smallShip.flyInFormation == true)
            {
                MatchSpeed(smallShip);
            }
            else
            {
                HalfSpeed(smallShip);
            }
        }
    }

    //This sets the ship to half speed 
    public static void HalfSpeed(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.aiMatchSpeed == false)
            {
                float halfSpeed = (smallShip.speedRating / 2f);

                if (smallShip.thrustSpeed > halfSpeed)
                {
                    smallShip.thrustInput = -1;
                }
                else
                {
                    smallShip.thrustInput = 1;
                }
            }
            else if (smallShip.targetingSystem.target != null & smallShip.flyInFormation == false || smallShip.followTarget != null & smallShip.flyInFormation == true)
            {
                MatchSpeed(smallShip);
            }
            else
            {
                float halfSpeed = (smallShip.speedRating / 2f);

                if (smallShip.thrustSpeed > halfSpeed)
                {
                    smallShip.thrustInput = -1;
                }
                else
                {
                    smallShip.thrustInput = 1;
                }
            }
        }
    }

    //This sets the ship to quarter speed
    public static void QuarterSpeed(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.aiMatchSpeed == false)
            {
                float quarterSpeed = (smallShip.speedRating / 4f);

                if (smallShip.thrustSpeed > quarterSpeed)
                {
                    smallShip.thrustInput = -1;
                }
                else
                {
                    smallShip.thrustInput = 1;
                }
            }
            else if (smallShip.targetingSystem.target != null & smallShip.flyInFormation == false || smallShip.followTarget != null & smallShip.flyInFormation == true)
            {
                MatchSpeed(smallShip);
            }
            else
            {
                HalfSpeed(smallShip);
            }
        }
    }

    //This changes the speed of the ship dynamically to allow for a fast speed and sharp turns
    public static void DynamicSpeed(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.targetingSystem.target != null)
            {
                if (smallShip.aiMatchSpeed == false)
                {
                    if (smallShip.targetingSystem.targetForward < 0.5f)
                    {
                        float halfSpeed = (smallShip.speedRating / 2f);

                        if (smallShip.thrustSpeed > halfSpeed)
                        {
                            smallShip.thrustInput = -1;
                        }
                        else
                        {
                            smallShip.thrustInput = 1;
                        }
                    }
                    else
                    {
                        FullSpeed(smallShip);
                    }
                }
                else
                {
                    MatchSpeed(smallShip);
                }
            }
            else
            {
                HalfSpeed(smallShip);
            }
        }
    }

    //This sets the ship to half speed (typically used when no enemies are detected)
    public static void NoSpeed(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.thrustSpeed > 0)
            {
                smallShip.thrustInput = -1;
            }
            else
            {
                smallShip.thrustInput = 0;
            }
        }
    }

    //This sets the ship to half speed (typically used when no enemies are detected)
    public static void MatchSpeed(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            float oneThird = (smallShip.speedRating / 3f);
            float oneHalf = (smallShip.speedRating / 2f);

            if (smallShip.thrustSpeed > smallShip.targetingSystem.targetSpeed & smallShip.thrustSpeed > oneThird)
            {
                //smallShip.thrustInput = -1;
                smallShip.thrustSpeed -= 1;
            }
            else
            {
                //smallShip.thrustInput = 1;
                smallShip.thrustSpeed += 1;
            }

            //This corrects the input for the follow target if necessary
            if (smallShip.followTarget != null & smallShip.flyInFormation == true)
            {
                smallShip.thrustInput = 1;

                //This gets the formation position and distance
                Quaternion flatLeaderRotation = Quaternion.Euler(0, smallShip.followTarget.transform.eulerAngles.y, 0);

                Vector3 desiredPosition = smallShip.followTarget.transform.position + flatLeaderRotation * new Vector3(smallShip.xFormationPos, smallShip.yFormationPos, smallShip.zFormationPos);

                float distance = Vector3.Distance(smallShip.transform.position, desiredPosition);

                //This slows the ship down for the turn
                Vector3 targetRelativePosition = desiredPosition - smallShip.transform.position;

                float followTargetForward = Vector3.Dot(smallShip.transform.forward, targetRelativePosition.normalized);

                if (followTargetForward < 0.25f)
                {
                    if (smallShip.thrustSpeed > oneHalf)
                    {
                        //smallShip.thrustInput = -1;
                        smallShip.thrustSpeed -= 1;
                    }
                    else
                    {
                        //smallShip.thrustInput = 1;
                        smallShip.thrustSpeed += 1;
                    }
                }

                //This dynamically adjusts the speed to stay in formation
                float breakingDistance = 500;

                float aggressionFactor = 1f;

                float decimalPercentage = (distance / breakingDistance) * aggressionFactor;

                if (distance > 15 & distance < breakingDistance)
                {
                    float dynamicSpeed = ((smallShip.speedRating - smallShip.followTarget.thrustSpeed) * decimalPercentage) + smallShip.followTarget.thrustSpeed;

                    if (smallShip.thrustSpeed > dynamicSpeed)
                    {
                        smallShip.thrustSpeed -= 1;
                    }
                    else if (smallShip.thrustSpeed < dynamicSpeed)
                    {
                        smallShip.thrustSpeed += 1;
                    }
                }
                else if (distance <= 15 & distance < breakingDistance)
                {
                    smallShip.thrustSpeed -= 1;
                }
                else
                {
                    smallShip.thrustSpeed += 1;
                }
            }
        }
    }

    #endregion

    #region AI Weapon Control

    //This fires one laser at a time
    public static void SingleLaser(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.targetingSystem.target != null)
            {
                smallShip.weaponManagement.weaponType = "lasers";

                if (smallShip.targetingSystem.interceptForward > 0.95f & smallShip.targetingSystem.interceptDistance < 2000 & smallShip.targetingSystem.target.gameObject.activeSelf == true & smallShip.energyWeapon.energyWeaponRecharged == true)
                {
                    bool dontFire = CheckFire(smallShip);

                    if (dontFire == false)
                    {
                        smallShip.weaponManagement.weaponMode = "single";
                        EnergyWeaponFunctions.InitiateFiring(smallShip);
                    }
                }
            }
        }
    }

    //This fires two lasers at a time
    public static void DualLasers(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {

            if (smallShip.targetingSystem.target != null)
            {
                smallShip.weaponManagement.weaponType = "lasers";

                if (smallShip.targetingSystem.interceptForward > 0.95f & smallShip.targetingSystem.interceptDistance < 2000 & smallShip.targetingSystem.target.gameObject.activeSelf == true & smallShip.energyWeapon.energyWeaponRecharged == true)
                {
                    bool dontFire = CheckFire(smallShip);

                    if (dontFire == false)
                    {
                        smallShip.weaponManagement.weaponMode = "dual";
                        EnergyWeaponFunctions.InitiateFiring(smallShip);
                    }
                }
            }
        }
    }

    //This fires all the ships lasers at once
    public static void AllLasers(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.targetingSystem.target != null)
            {
                smallShip.weaponManagement.weaponType = "lasers";

                if (smallShip.targetingSystem.interceptForward > 0.95f & smallShip.targetingSystem.interceptDistance < 2000 & smallShip.targetingSystem.target.gameObject.activeSelf == true & smallShip.energyWeapon.energyWeaponRecharged == true)
                {
                    bool dontFire = CheckFire(smallShip);

                    if (dontFire == false)
                    {
                        smallShip.weaponManagement.weaponMode = "all";
                        EnergyWeaponFunctions.InitiateFiring(smallShip);
                    }
                }
            }
        }
    }

    //This fires one laser at a time
    public static void RapidLasers(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.targetingSystem.target != null)
            {
                smallShip.weaponManagement.weaponType = "lasers";

                if (smallShip.targetingSystem.interceptForward > 0.95f & smallShip.targetingSystem.interceptDistance < 2000 & smallShip.targetingSystem.target.gameObject.activeSelf == true)
                {
                    bool dontFire = CheckFire(smallShip);

                    if (dontFire == false & smallShip.weaponManagement.hasRapidFire == true)
                    {
                        smallShip.weaponManagement.weaponMode = "rapid";
                        EnergyWeaponFunctions.InitiateFiring(smallShip);
                    }
                    else if (dontFire == false)
                    {
                        smallShip.weaponManagement.weaponMode = "single";
                        EnergyWeaponFunctions.InitiateFiring(smallShip);
                    }
                }
            }
        }
    }

    //This fires one laser at a time
    public static void SinglePlasma(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.targetingSystem.target != null)
            {
                smallShip.weaponManagement.weaponType = "plasma";

                if (smallShip.targetingSystem.interceptForward > 0.95f & smallShip.targetingSystem.interceptDistance < 2000 & smallShip.targetingSystem.target.gameObject.activeSelf == true & smallShip.energyWeapon.energyWeaponRecharged == true)
                {
                    bool dontFire = CheckFire(smallShip);

                    if (dontFire == false)
                    {
                        smallShip.weaponManagement.weaponMode = "single";
                        //PlasmaFunctions.InitiateFiring(smallShip);
                    }
                }
            }
        }
    }

    //This fires two lasers at a time
    public static void DualPlasma(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {

            if (smallShip.targetingSystem.target != null)
            {
                smallShip.weaponManagement.weaponType = "plasma";

                if (smallShip.targetingSystem.interceptForward > 0.95f & smallShip.targetingSystem.interceptDistance < 2000 & smallShip.targetingSystem.target.gameObject.activeSelf == true & smallShip.energyWeapon.energyWeaponRecharged == true)
                {
                    bool dontFire = CheckFire(smallShip);

                    if (dontFire == false)
                    {
                        smallShip.weaponManagement.weaponMode = "dual";
                        //PlasmaFunctions.InitiateFiring(smallShip);
                    }
                }
            }
        }
    }

    //This fires all the ships lasers at once
    public static void AllPlasma(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.targetingSystem.target != null)
            {
                smallShip.weaponManagement.weaponType = "plasma";

                if (smallShip.targetingSystem.interceptForward > 0.95f & smallShip.targetingSystem.interceptDistance < 2000 & smallShip.targetingSystem.target.gameObject.activeSelf == true & smallShip.energyWeapon.energyWeaponRecharged == true)
                {
                    bool dontFire = CheckFire(smallShip);

                    if (dontFire == false)
                    {
                        smallShip.weaponManagement.weaponMode = "all";
                        //PlasmaFunctions.InitiateFiring(smallShip);
                    }
                }
            }
        }
    }

    //This fires one ion at a time
    public static void SingleIon(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.targetingSystem.target != null)
            {
                smallShip.weaponManagement.weaponType = "ion";

                if (smallShip.targetingSystem.interceptForward > 0.95f & smallShip.targetingSystem.interceptDistance < 2000 & smallShip.targetingSystem.target.gameObject.activeSelf == true & smallShip.energyWeapon.energyWeaponRecharged == true)
                {
                    bool dontFire = CheckFire(smallShip);

                    if (dontFire == false)
                    {
                        smallShip.weaponManagement.weaponMode = "single";
                        //IonFunctions.InitiateFiring(smallShip);
                    }
                }
            }
        }
    }

    //This fires two ions at a time
    public static void DualIon(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.targetingSystem.target != null)
            {
                smallShip.weaponManagement.weaponType = "ion";

                if (smallShip.targetingSystem.interceptForward > 0.95f & smallShip.targetingSystem.interceptDistance < 2000 & smallShip.targetingSystem.target.gameObject.activeSelf == true & smallShip.energyWeapon.energyWeaponRecharged == true)
                {
                    bool dontFire = CheckFire(smallShip);

                    if (dontFire == false)
                    {
                        smallShip.weaponManagement.weaponMode = "dual";
                        //IonFunctions.InitiateFiring(smallShip);
                    }
                }
            }
        }
    }

    //This fires all the ships ions at once
    public static void AllIon(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.targetingSystem.target != null)
            {
                smallShip.weaponManagement.weaponType = "ion";

                if (smallShip.targetingSystem.interceptForward > 0.95f & smallShip.targetingSystem.interceptDistance < 2000 & smallShip.targetingSystem.target.gameObject.activeSelf == true & smallShip.energyWeapon.energyWeaponRecharged == true)
                {
                    bool dontFire = CheckFire(smallShip);

                    if (dontFire == false)
                    {
                        smallShip.weaponManagement.weaponMode = "all";
                        //IonFunctions.InitiateFiring(smallShip);
                    }
                }
            }
        }
    }

    //This fires one ion at a time
    public static void RapidIon(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.targetingSystem.target != null)
            {
                smallShip.weaponManagement.weaponType = "ion";

                if (smallShip.targetingSystem.interceptForward > 0.95f & smallShip.targetingSystem.interceptDistance < 2000 & smallShip.targetingSystem.target.gameObject.activeSelf == true)
                {
                    bool dontFire = CheckFire(smallShip);

                    if (dontFire == false & smallShip.weaponManagement.hasRapidFire == true)
                    {
                        smallShip.weaponManagement.weaponMode = "rapid";
                        //IonFunctions.InitiateFiring(smallShip);
                    }
                    else if (dontFire == false)
                    {
                        smallShip.weaponManagement.weaponMode = "single";
                        //IonFunctions.InitiateFiring(smallShip);
                    }
                }
            }
        }
    }

    //This fires one torpedo at a time
    public static void SingleTorpedo(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.targetingSystem.target != null)
            {
                smallShip.weaponManagement.weaponType = "torpedos";

                if (smallShip.torpedoSystem.torpedoNumber > 0)
                {
                    if (smallShip.targetingSystem.targetForward > 0.995f & smallShip.torpedoSystem.torpedoLockedOn == true)
                    {
                        smallShip.weaponManagement.weaponMode = "single";
                        TorpedoSystemFunctions.FireTorpedo(smallShip);
                    }
                }
                else
                {
                    smallShip.weaponManagement.weaponType = "lasers";
                    SingleLaser(smallShip);
                }
            }
        }
    }

    //This fires two  torpedo at once
    public static void DualTorpedos(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.targetingSystem.target != null)
            {
                smallShip.weaponManagement.weaponType = "torpedos";

                if (smallShip.torpedoSystem.torpedoNumber > 0)
                {
                    if (smallShip.targetingSystem.targetForward > 0.995f & smallShip.torpedoSystem.torpedoLockedOn == true)
                    {
                        smallShip.weaponManagement.weaponMode = "dual";
                        TorpedoSystemFunctions.FireTorpedo(smallShip);
                    }
                }
                else
                {
                    smallShip.weaponManagement.weaponType = "lasers";
                    SingleLaser(smallShip);
                }
            }
        }
    }

    //This fires from all torpedo tubes
    public static void AllTorpedos(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.targetingSystem.target != null)
            {
                smallShip.weaponManagement.weaponType = "torpedos";

                if (smallShip.torpedoSystem.torpedoNumber > 0)
                {
                    if (smallShip.targetingSystem.targetForward > 0.995f & smallShip.torpedoSystem.torpedoLockedOn == true)
                    {
                        smallShip.weaponManagement.weaponMode = "all";
                        TorpedoSystemFunctions.FireTorpedo(smallShip);
                    }
                }
                else
                {
                    smallShip.weaponManagement.weaponType = "lasers";
                    SingleLaser(smallShip);
                }
            }
        }
    }

    //This switches between single lasers and torpedos
    public static void DynamicWeapons_Single(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.targetingSystem.target != null)
            {
                if (smallShip.torpedoSystem.torpedoNumber > 0 & smallShip.targetingSystem.interceptDistance > 2000 & smallShip.targetingSystem.target.gameObject.activeSelf == true)
                {
                    smallShip.weaponManagement.weaponType = "torpedos";

                    if (smallShip.targetingSystem.targetForward > 0.995f & smallShip.torpedoSystem.torpedoLockedOn == true)
                    {
                        smallShip.weaponManagement.weaponMode = "single";
                        TorpedoSystemFunctions.FireTorpedo(smallShip);
                    }
                }
                else if (smallShip.targetingSystem.interceptForward > 0.95f & smallShip.targetingSystem.interceptDistance < 2000 & smallShip.targetingSystem.target.gameObject.activeSelf == true)
                {
                    if (smallShip.weaponManagement.hasPlasma == false)
                    {
                        smallShip.weaponManagement.weaponType = "lasers";
                    }
                    else
                    {
                        smallShip.weaponManagement.weaponType = "plasma";
                    }

                    bool dontFire = CheckFire(smallShip);

                    if (dontFire == false & smallShip.energyWeapon.energyWeaponRecharged == true)
                    {
                        smallShip.weaponManagement.weaponMode = "single";
                        EnergyWeaponFunctions.InitiateFiring(smallShip);
                    }
                }
            }
        }
    }

    //This switches betwene dual lasers and torpedos
    public static void DynamicWeapons_Dual(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.targetingSystem.target != null)
            {
                if (smallShip.torpedoSystem.torpedoNumber > 0 & smallShip.targetingSystem.interceptDistance > 2000 & smallShip.targetingSystem.target.gameObject.activeSelf == true)
                {
                    smallShip.weaponManagement.weaponType = "torpedos";

                    if (smallShip.targetingSystem.targetForward > 0.995f & smallShip.torpedoSystem.torpedoLockedOn == true)
                    {
                        smallShip.weaponManagement.weaponMode = "dual";
                        TorpedoSystemFunctions.FireTorpedo(smallShip);
                    }
                }
                else if (smallShip.targetingSystem.interceptForward > 0.95f & smallShip.targetingSystem.interceptDistance < 2000 & smallShip.targetingSystem.target.gameObject.activeSelf == true)
                {
                    if (smallShip.weaponManagement.hasPlasma == false)
                    {
                        smallShip.weaponManagement.weaponType = "lasers";
                    }
                    else
                    {
                        smallShip.weaponManagement.weaponType = "plasma";
                    }

                    bool dontFire = CheckFire(smallShip);

                    if (dontFire == false)
                    {
                        smallShip.weaponManagement.weaponMode = "dual";
                        EnergyWeaponFunctions.InitiateFiring(smallShip);
                    }
                }
            }
        }
    }

    //This switches betwene dual lasers and torpedos
    public static void DynamicWeapons_All(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.targetingSystem.target != null)
            {
                if (smallShip.torpedoSystem.torpedoNumber > 0 & smallShip.targetingSystem.interceptDistance > 2000 & smallShip.targetingSystem.target.gameObject.activeSelf == true)
                {
                    smallShip.weaponManagement.weaponType = "torpedos";

                    if (smallShip.targetingSystem.targetForward > 0.995f & smallShip.torpedoSystem.torpedoLockedOn == true)
                    {
                        smallShip.weaponManagement.weaponMode = "all";
                        TorpedoSystemFunctions.FireTorpedo(smallShip);
                    }
                }
                else if (smallShip.targetingSystem.interceptForward > 0.95f & smallShip.targetingSystem.interceptDistance < 2000 & smallShip.targetingSystem.target.gameObject.activeSelf == true)
                {
                    if (smallShip.weaponManagement.hasPlasma == false)
                    {
                        smallShip.weaponManagement.weaponType = "lasers";
                    }
                    else
                    {
                        smallShip.weaponManagement.weaponType = "plasma";
                    }

                    bool dontFire = CheckFire(smallShip);

                    if (dontFire == false)
                    {
                        smallShip.weaponManagement.weaponMode = "all";
                        EnergyWeaponFunctions.InitiateFiring(smallShip);
                    }
                }
            }
        }
    }

    //This switches between rapid lasers and normal lasers
    public static void DynamicWeapons_Rapid(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.weaponManagement.hasPlasma == false)
            {
                smallShip.weaponManagement.weaponType = "lasers";
            }
            else
            {
                smallShip.weaponManagement.weaponType = "plasma";
            }

            if (smallShip.targetingSystem.target != null)
            {
                if (smallShip.targetingSystem.interceptForward > 0.95f & smallShip.targetingSystem.target.gameObject.activeSelf == true)
                {
                    if (smallShip.weaponManagement.hasRapidFire == true)
                    {
                        if (smallShip.targetingSystem.targetSmallShip != null)
                        {
                            if (smallShip.targetingSystem.targetSmallShip.weaponManagement.hasPlasma == true & smallShip.targetingSystem.targetSmallShip.shieldLevel > 10)
                            {
                                smallShip.weaponManagement.weaponMode = "rapid";
                                EnergyWeaponFunctions.InitiateFiring(smallShip);
                            }
                            else
                            {
                                smallShip.weaponManagement.weaponMode = "single";
                                EnergyWeaponFunctions.InitiateFiring(smallShip);
                            }
                        }
                    }
                    else
                    {
                        smallShip.weaponManagement.weaponMode = "single";
                        EnergyWeaponFunctions.InitiateFiring(smallShip);
                    }
                }
            }
        }
    }
    //This checks whether a non hostile ship is in the firing line or not
    public static bool CheckFire(FlightControlAI_Small smallShip)
    {
        bool dontFire = false;

        if (smallShip != null)
        {
            RaycastHit hit;

            int layerMask = (1 << 6) | (1 << 7) | (1 << 8) | (1 << 9) | (1 << 10) | (1 << 11) | (1 << 12) | (1 << 13) | (1 << 14) | (1 << 15) | (1 << 16) | (1 << 17) | (1 << 18) | (1 << 19) | (1 << 20) | (1 << 21) | (1 << 22) | (1 << 23);

            Vector3 forwardRaycast = smallShip.gameObject.transform.position + (smallShip.gameObject.transform.forward * 10);

            if (Physics.SphereCast(forwardRaycast, 10, smallShip.gameObject.gameObject.transform.TransformDirection(Vector3.forward), out hit, 1000, layerMask))
            {
                FlightControlAI_Small otherSmallship = hit.collider.GetComponentInParent<FlightControlAI_Small>();

                if (otherSmallship != null)
                {
                    bool isHostile = TargetingSystemFunctions.GetHostility_SmallShipPlayer(smallShip, otherSmallship.ship.allegiance);

                    if (isHostile != true)
                    {
                        dontFire = true;
                    }
                }
            }
        }

        return dontFire;
    }

    #endregion

    #region AI Weapon Accuracy

    //This sets the targetting accuracy to low
    public static void LowAccuracy(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            SetTargetingErrorMargin(smallShip, "low");
        }
    }

    //This sets the targetting accuracy to medium
    public static void MediumAccuracy(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            SetTargetingErrorMargin(smallShip, "medium");
        }
    }

    //This sets the targetting accuracy to jihj
    public static void HighAccuracy(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            SetTargetingErrorMargin(smallShip, "high");
        }
    }

    //Targeting error margin
    public static void SetTargetingErrorMargin(FlightControlAI_Small smallShip, string mode)
    {
        if (smallShip != null)
        {
            float lowRange = 100;
            float mediumRange = 50;
            float highRange = 25;

            float x = 0;
            float y = 0;
            float z = 0;

            if (mode == "low")
            {
                x = Random.Range(-lowRange, lowRange);
                y = Random.Range(-lowRange, lowRange);
                z = Random.Range(-lowRange, lowRange);
            }
            else if (mode == "medium")
            {
                x = Random.Range(-mediumRange, mediumRange);
                y = Random.Range(-mediumRange, mediumRange);
                z = Random.Range(-mediumRange, mediumRange);
            }
            else if (mode == "high")
            {
                x = Random.Range(-highRange, highRange);
                y = Random.Range(-highRange, highRange);
                z = Random.Range(-highRange, highRange);
            }

            smallShip.aiTargetingErrorMargin = new Vector3(x, y, z);
        }
    }

    #endregion

    #region AI Flight Patterns

    //Chase: The enemy relenlessly pursues the player without withdrawing
    public static void Chase(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.aiEvade == false)
            {
                if (smallShip.targetingSystem.target != null)
                {
                    if (smallShip.targetingSystem.targetDistance > 250)
                    {
                        smallShip.aiMatchSpeed = false;
                        AngleTowardsTarget(smallShip);
                    }
                    else
                    {
                        smallShip.aiMatchSpeed = true;
                        AngleTowardsTarget(smallShip);
                    }
                }
                else
                {
                    PatrolRandom(smallShip, true);
                }
            }

            smallShip.flyInFormation = false;
        }
    }

    //Chase-Withdraw: Attack for 15-20 followed by withdrawal for 30-35 seconds
    public static void ChaseWithdraw(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.aiEvade == false)
            {
                if (smallShip.targetingSystem.target != null)
                {
                    if (smallShip.targetingSystem.targetDistance > 250)
                    {
                        smallShip.aiMatchSpeed = false;
                        AngleTowardsTarget(smallShip);
                    }
                    else
                    {
                        if (Time.time < smallShip.aiAttackTime)
                        {
                            smallShip.aiMatchSpeed = true;
                            AngleTowardsTarget(smallShip);
                            smallShip.aiRetreatTime = Time.time + Random.Range(15, 20);
                        }
                        else
                        {
                            smallShip.aiMatchSpeed = false;

                            if (Time.time < smallShip.aiRetreatTime)
                            {
                                AngleAwayFromTarget(smallShip);
                            }
                            else
                            {
                                smallShip.aiAttackTime = Time.time + Random.Range(30, 35);
                            }
                        }
                    }
                }
                else
                {
                    PatrolRandom(smallShip, true);
                }
            }

            smallShip.flyInFormation = false;
        }
    }

    //Strafe-Withdraw: Attacks enemy ship at distance before withdrawing again (typically used for attack large ships)
    public static void StrafeWithdraw(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.aiEvade == false)
            {
                if (smallShip != null)
                {
                    float attackDistance = 250;
                    float withdrawDistance = 1000;

                    if (smallShip.targetingSystem.targetLargeShip != null)
                    {
                        if (smallShip.targetingSystem.targetLargeShip.shipClass == "large")
                        {
                            attackDistance = 1500;
                            withdrawDistance = 3000;
                        }
                        else if (smallShip.targetingSystem.targetLargeShip.shipClass == "middle")
                        {
                            attackDistance = 1000;
                            withdrawDistance = 2000;
                        }
                        else
                        {
                            attackDistance = 500;
                            withdrawDistance = 1000;
                        }
                    }

                    if (smallShip.targetingSystem.targetDistance > attackDistance & smallShip.withdraw == false)
                    {
                        AngleTowardsTarget(smallShip);
                    }
                    else if (smallShip.targetingSystem.targetDistance < attackDistance & smallShip.withdraw == false)
                    {
                        smallShip.withdraw = true;
                    }
                    else if (smallShip.targetingSystem.targetDistance > withdrawDistance & smallShip.withdraw == true)
                    {
                        smallShip.withdraw = false;
                    }
                    else
                    {
                        AngleAwayFromTarget(smallShip);
                    }
                }
                else
                {
                    PatrolRandom(smallShip, true);
                }
            }

            smallShip.flyInFormation = false;
        }
    }

    //This angles towards the ships waypoint
    public static void MoveToWayPoint(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.aiEvade == false)
            {
                AngleTowardsWaypoint(smallShip);
            }

            smallShip.flyInFormation = false;
        }
    }

    //This is the basic flight pattern for patrolling
    public static void PatrolRandom(FlightControlAI_Small smallShip, bool stayClose = false)
    {
        if (smallShip != null)
        {
            if (smallShip.aiEvade == false)
            {
                if (smallShip.targetingSystem.waypoint != null)
                {
                    float distanceToWaypoint = Vector3.Distance(smallShip.gameObject.transform.position, smallShip.targetingSystem.waypoint.transform.position);

                    if (distanceToWaypoint < 50)
                    {
                        SelectRandomWaypoint(smallShip, stayClose);
                    }

                    AngleTowardsWaypoint(smallShip);
                }
            }

            smallShip.flyInFormation = false;
        }
    }

    //This selects a random waypoint
    public static void SelectRandomWaypoint(FlightControlAI_Small smallShip, bool stayClose = false)
    {
        if (smallShip != null)
        {
            if (smallShip.aiEvade == false)
            {
                float min = -smallShip.scene.sceneRadius + 1000;
                float max = smallShip.scene.sceneRadius - 1000;

                float x = Random.Range(min, max);
                float y = Random.Range(min, max);
                float z = Random.Range(min, max);

                //This makes ships with no target stay within range of the player/center of the scene
                if (stayClose == true)
                {
                    Vector3 referencePosition = new Vector3(0, 0, 0);

                    if (smallShip.scene != null)
                    {
                        Scene scene = smallShip.scene;

                        if (scene != null)
                        {
                            if (scene.mainShip != null)
                            {
                                referencePosition.x = scene.mainShip.transform.localPosition.x;
                                referencePosition.y = scene.mainShip.transform.localPosition.y;
                                referencePosition.z = scene.mainShip.transform.localPosition.z;
                            }
                        }
                    }

                    x = referencePosition.x + Random.Range(-2500, 2500);
                    y = referencePosition.y + Random.Range(-2500, 2500);
                    z = referencePosition.z + Random.Range(-2500, 2500);
                }

                if (smallShip.targetingSystem.waypoint != null)
                {
                    smallShip.targetingSystem.waypoint.transform.localPosition = new Vector3(x, y, z);
                }
            }
        }
    }

    //This sets the ship to fly in formation
    public static void FormationFlying(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.followTarget != null)
            {
                if (smallShip.isAI == true)
                {
                    smallShip.flyInFormation = true;
                }

                Quaternion flatLeaderRotation = Quaternion.Euler(0, smallShip.followTarget.transform.eulerAngles.y, 0);

                Vector3 desiredPosition = smallShip.followTarget.transform.position + flatLeaderRotation * new Vector3(smallShip.xFormationPos, smallShip.yFormationPos, smallShip.zFormationPos);

                float distance = Vector3.Distance(smallShip.transform.position, desiredPosition);

                smallShip.aiMatchSpeed = true;

                AngleTowardsPoint(smallShip, desiredPosition);
            }
            else
            {
                smallShip.aiMatchSpeed = false;
                smallShip.flyInFormation = false;
                ChaseWithdraw(smallShip);
            }
        }
    }

    //This prevents the ship from performing any rotations and locks the direction forward
    public static void NoRotation(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            ResetSteeringInputs(smallShip);
        }

        smallShip.flyInFormation = false;
        smallShip.positionLocked = false;
    }

    #endregion

    #region AI Energy Management

    //This sets shield power to maximum
    public static void EnergyToShields(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            smallShip.powerMode = "shields";
        }
    }

    //This sets engine power to maximum
    public static void EnergyToEngines(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            smallShip.powerMode = "engines";
        }
    }

    //This sets engine power to maximum
    public static void EnergyToLasers(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            smallShip.powerMode = "lasers";
        }
    }

    //This resets all energy levels
    public static void ResetEnergyLevels(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            smallShip.powerMode = "reset";
        }
    }

    //This uses the ships energy in a way that maxmises aggression but leaves lowers the defensive capabilites of the ship
    public static void EnergyAggressive(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.targetingSystem.target != null)
            {
                if (smallShip.targetingSystem.targetDistance < 1000 & smallShip.targetingSystem.targetForward > 0)
                {
                    smallShip.powerMode = "lasers";
                }
                else
                {
                    smallShip.powerMode = "engines";
                }
            }
            else
            {
                smallShip.powerMode = "reset";
            }
        }
    }

    //This uses the ships energy in a way that maxmises defenses but minimises attack capabilities
    public static void EnergyProtective(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.targetingSystem.target != null)
            {
                if (smallShip.shieldLevel < smallShip.shieldRating / 2f)
                {
                    smallShip.powerMode = "shields";
                }
                else
                {
                    smallShip.powerMode = "reset";
                }
            }
            else
            {
                smallShip.powerMode = "reset";
            }
        }
    }

    //This uses the ships energy in way that is most effective for both offense and defense
    public static void EnergyDynamic(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.targetingSystem.target != null)
            {
                if (smallShip.shieldLevel < smallShip.shieldRating / 2f)
                {
                    smallShip.powerMode = "shields";
                }
                else if (smallShip.targetingSystem.targetDistance < 1000 & smallShip.targetingSystem.targetForward > 0)
                {
                    smallShip.powerMode = "lasers";
                }
                else if (smallShip.targetingSystem.targetDistance > 2000 & smallShip.targetingSystem.targetForward > 0)
                {
                    smallShip.powerMode = "engines";
                }
                else
                {
                    smallShip.powerMode = "reset";
                }
            }
            else
            {
                smallShip.powerMode = "reset";
            }
        }
    }

    #endregion

    #region AI Targetting

    //This checks if the ship needs to request a new target - this function is run automatically
    public static void RequestTarget(FlightControlAI_Small smallShip)
    {
        if (smallShip.targetingSystem.target == null)
        {
            smallShip.requestingTarget = true;
        }
        else
        {
            smallShip.requestingTarget = false;
        }
    }

    //This clears the target if it doesn't meet certain conditions i.e. was destroyed or disabled  - this function is run automatically
    public static void ClearTarget(FlightControlAI_Small smallShip)
    {
        if (smallShip.targetingSystem.target != null)
        {
            if (smallShip.targetingSystem.target.activeSelf == false)
            {
                smallShip.targetingSystem.target = null;
            }
            else if (smallShip.targetingSystem.targetSmallShip != null)
            {
                if (smallShip.targetingSystem.targetSmallShip.isDisabled == true)
                {
                    smallShip.targetingSystem.target = null;
                }
            }
            else if (smallShip.targetingSystem.targetLargeShip != null)
            {
                if (smallShip.targetingSystem.targetLargeShip.isDisabled == true)
                {
                    smallShip.targetingSystem.target = null;
                }
            }
        }
    }

    //Tagets all ships but looks for small ships first
    public static void TargetAllPrefSmall(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.aiTargetingMode != "targetallprefsmall") //This ensures the previous target is cleared
            {
                smallShip.targetingSystem.target = null;
                smallShip.requestingTarget = true;
            }

            smallShip.aiTargetingMode = "targetallprefsmall";
        }
    }

    //Targets all ships but looks for large ships first
    public static void TargetAllPrefLarge(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.aiTargetingMode != "targetallpreflarge") //This ensures the previous target is cleared
            {
                smallShip.targetingSystem.target = null;
                smallShip.requestingTarget = true;
            }

            smallShip.aiTargetingMode = "targetallpreflarge";
        }
    }

    //Targets only small ships
    public static void TargetSmallShipsOnly(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.aiTargetingMode != "targetsmallshipsonly") //This ensures the previous target is cleared
            {
                smallShip.targetingSystem.target = null;
                smallShip.requestingTarget = true;
            }

            smallShip.aiTargetingMode = "targetsmallshipsonly";
        }
    }

    //Targets only small ships
    public static void TargetLargeShipOnly(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.aiTargetingMode != "targetlargeshipsonly") //This ensures the previous target is cleared
            {
                smallShip.targetingSystem.target = null;
                smallShip.requestingTarget = true;
            }

            smallShip.aiTargetingMode = "targetlargeshipsonly";
        }
    }

    #endregion

    #region AI Evasion and Collision Avoidance

    //This allows the ship to evade attacks and avoid collisions with other objects and ships
    public static IEnumerator Evade(Ship ship, float time, string mode, int direction)
    {
        if (ship != null)
        {
            if (TagExists(ship.flightControlAI_Small, "nospeed") == true & TagExists(ship, "norotation") == true || TagExists(ship, "nocollisionevasion") == true || TagExists(ship, "formationflying") == true)
            {
                //Do nothing
            }
            else
            {
                if (ship != null)
                {
                    ship.aiEvade = true;

                    time = time + Time.time;

                    while (time > Time.time)
                    {
                        HalfSpeed(ship);

                        if (direction == 0)
                        {
                            TurnRight(ship);
                        }
                        else if (direction == 1)
                        {
                            TurnLeft(ship);
                        }
                        else if (direction == 2)
                        {
                            PitchUp(ship);
                        }
                        else if (direction == 3)
                        {
                            PitchDown(ship);
                        }
                        else if (direction == 4)
                        {
                            RollRight(ship);
                        }
                        else if (direction == 5)
                        {
                            RollLeft(ship);
                        }
                        else if (direction == 6)
                        {
                            FlyFoward(ship);
                        }

                        yield return null;

                    }

                    ResetSteeringInputs(ship);

                    ship.aiEvade = false;
                }
            }
        }
    }

    #endregion

    #region AI Steering Control

    //This angles the ship towards the target vector
    public static void AngleTowardsTarget(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            AvoidGimbalLock(smallShip, smallShip.targetingSystem.interceptForward);

            if (smallShip.targetingSystem.target != null & smallShip.avoidGimbalLock == false)
            {
                if (smallShip.targetingSystem.interceptForward < 0.8)
                {
                    if (Vector3.Dot(smallShip.transform.up, Vector3.down) < 0)
                    {
                        //Right way up
                        smallShip.turnInput = smallShip.targetingSystem.interceptRight;
                        smallShip.pitchInput = -smallShip.targetingSystem.interceptUp;

                    }
                    else
                    {
                        //Upside down
                        smallShip.turnInput = -smallShip.targetingSystem.interceptRight;
                        smallShip.pitchInput = -smallShip.targetingSystem.interceptUp;
                    }
                }
                else
                {
                    //Smoothly interpolate the multiplier from 1 to 5 to prevent a jerk
                    float t = Mathf.InverseLerp(0.8f, 1.0f, smallShip.targetingSystem.interceptForward);
                    float multiplier = Mathf.SmoothStep(1f, 5f, t);

                    if (Vector3.Dot(smallShip.transform.up, Vector3.down) < 0)
                    {
                        //Right way up
                        smallShip.turnInput = smallShip.targetingSystem.interceptRight * multiplier;
                        smallShip.pitchInput = -smallShip.targetingSystem.interceptUp * multiplier;
                    }
                    else
                    {
                        //Upside down
                        smallShip.turnInput = -smallShip.targetingSystem.interceptRight * multiplier;
                        smallShip.pitchInput = -smallShip.targetingSystem.interceptUp * multiplier;
                    }
                }
            }
        }
    }

    //This angles the ship away from the target vector
    public static void AngleAwayFromTarget(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            AvoidGimbalLock(smallShip, smallShip.targetingSystem.interceptForward, true);

            if (smallShip.targetingSystem.target != null & smallShip.avoidGimbalLock == false)
            {
                if (-smallShip.targetingSystem.interceptForward < 1)
                {
                    if (Vector3.Dot(smallShip.transform.up, Vector3.down) < 0)
                    {
                        //Right way up
                        smallShip.turnInput = -smallShip.targetingSystem.interceptRight;
                        smallShip.pitchInput = smallShip.targetingSystem.interceptUp;
                    }
                    else
                    {
                        //Upside down
                        smallShip.turnInput = smallShip.targetingSystem.interceptRight;
                        smallShip.pitchInput = -smallShip.targetingSystem.interceptUp;
                    }
                }
            }
        }
    }

    //This angles the ship towards the target vector
    public static void AngleTowardsWaypoint(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            AvoidGimbalLock(smallShip, smallShip.targetingSystem.waypointForward);

            if (smallShip.targetingSystem.waypoint != null & smallShip.avoidGimbalLock == false)
            {
                if (smallShip.targetingSystem.waypointForward < 0.8)
                {
                    if (Vector3.Dot(smallShip.transform.up, Vector3.down) < 0)
                    {
                        //Right way up
                        smallShip.turnInput = smallShip.targetingSystem.waypointRight;
                        smallShip.pitchInput = -smallShip.targetingSystem.waypointUp;

                    }
                    else
                    {
                        //Upside down
                        smallShip.turnInput = -smallShip.targetingSystem.waypointRight;
                        smallShip.pitchInput = -smallShip.targetingSystem.waypointUp;
                    }
                }
                else
                {
                    //Smoothly interpolate the multiplier from 1 to 5 to prevent a jerk
                    float t = Mathf.InverseLerp(0.8f, 1.0f, smallShip.targetingSystem.waypointForward);
                    float multiplier = Mathf.SmoothStep(1f, 5f, t);

                    if (Vector3.Dot(smallShip.transform.up, Vector3.down) < 0)
                    {
                        //Right way up
                        smallShip.turnInput = smallShip.targetingSystem.waypointRight * multiplier;
                        smallShip.pitchInput = -smallShip.targetingSystem.waypointUp * multiplier;
                    }
                    else
                    {
                        //Upside down
                        smallShip.turnInput = -smallShip.targetingSystem.waypointRight * multiplier;
                        smallShip.pitchInput = -smallShip.targetingSystem.waypointUp * multiplier;
                    }
                }
            }
        }
    }

    //This angles the ship away from the target vector
    public static void AngleAwayFromWaypoint(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            AvoidGimbalLock(smallShip, smallShip.targetingSystem.waypointForward, true);

            if (smallShip.targetingSystem.waypoint != null & smallShip.avoidGimbalLock == false)
            {
                if (smallShip.targetingSystem.waypointForward > -0.95)
                {
                    if (Vector3.Dot(smallShip.transform.up, Vector3.down) < 0)
                    {
                        //Right way up
                        smallShip.turnInput = -smallShip.targetingSystem.waypointRight;
                        smallShip.pitchInput = smallShip.targetingSystem.waypointUp;
                    }
                    else
                    {
                        //Upside down
                        smallShip.turnInput = smallShip.targetingSystem.waypointRight;
                        smallShip.pitchInput = -smallShip.targetingSystem.waypointUp;
                    }
                }
            }
        }
    }

    //This angles the ship towards the target vector
    public static void AngleTowardsPoint(FlightControlAI_Small smallShip, Vector3 point)
    {
        if (smallShip != null)
        {
            Vector3 targetPosition = point;

            Vector3 targetRelativePosition = targetPosition - smallShip.transform.position;

            float targetForward = Vector3.Dot(smallShip.transform.forward, targetRelativePosition.normalized);
            float targetRight = Vector3.Dot(smallShip.transform.right, targetRelativePosition.normalized);
            float targetUp = Vector3.Dot(smallShip.transform.up, targetRelativePosition.normalized);

            AvoidGimbalLock(smallShip, targetForward);

            if (smallShip.avoidGimbalLock == false)
            {
                if (targetForward < 0.8)
                {
                    if (Vector3.Dot(smallShip.transform.up, Vector3.down) < 0)
                    {
                        smallShip.turnInput = targetRight;
                        smallShip.pitchInput = -targetUp;
                    }
                    else
                    {
                        //Upside down
                        smallShip.turnInput = -targetRight;
                        smallShip.pitchInput = -targetUp;
                    }
                }
                else
                {
                    //Smoothly interpolate the multiplier from 1 to 5 to prevent a jerk
                    float t = Mathf.InverseLerp(0.8f, 1.0f, targetForward);
                    float multiplier = Mathf.SmoothStep(1f, 5f, t);

                    if (Vector3.Dot(smallShip.transform.up, Vector3.down) < 0)
                    {
                        //Right way up
                        smallShip.turnInput = targetRight * multiplier;
                        smallShip.pitchInput = -targetUp * multiplier;
                    }
                    else
                    {
                        //Upside down
                        smallShip.turnInput = -targetRight * multiplier;
                        smallShip.pitchInput = -targetUp * multiplier;
                    }
                }
            }
        }
    }

    //This pitches the ship up
    public static void PitchUp(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            smallShip.pitchInput = 1;
        }
    }

    //This pitches the ship down
    public static void PitchDown(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            smallShip.pitchInput = -1;
        }
    }

    //This turns the ship right
    public static void TurnRight(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            smallShip.turnInput = 1;
        }
    }

    //This turns the ship Left
    public static void TurnLeft(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            smallShip.turnInput = -1;
        }
    }

    //This causes the ship to roll Right
    public static void RollRight(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            smallShip.rollInput = 1;
        }
    }

    //This causes the ship to roll left
    public static void RollLeft(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            smallShip.rollInput = -1;
        }
    }

    //This causes the ship to fly forward
    public static void FlyFoward(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            smallShip.turnInput = 0;
            smallShip.pitchInput = 0;
        }
    }

    //This prevents gimbal lock when the ships turn
    public static void AvoidGimbalLock(FlightControlAI_Small smallShip, float forward, bool reverse = false)
    {

        if (reverse == false)
        {
            if (forward < -0.9)
            {
                smallShip.avoidGimbalLock = true;

                if (Vector3.Dot(smallShip.transform.up, Vector3.down) < 0)
                {
                    //Steering when ship is the right way up
                    smallShip.turnInput = 1;
                    smallShip.pitchInput = 0;
                }
                else
                {
                    //Steering when the ship is upside down
                    smallShip.turnInput = -1;
                    smallShip.pitchInput = 0;
                }
            }
            else
            {
                smallShip.avoidGimbalLock = false;
            }
        }
        else
        {
            if (forward > 0.9)
            {
                smallShip.avoidGimbalLock = true;

                if (Vector3.Dot(smallShip.transform.up, Vector3.down) < 0)
                {
                    //Right way up
                    smallShip.turnInput = -1;
                    smallShip.pitchInput = 0;
                    smallShip.rollInput = 0;
                }
                else
                {
                    //upside down
                    smallShip.turnInput = 1;
                    smallShip.pitchInput = 0;
                }
            }
            else
            {
                smallShip.avoidGimbalLock = false;
            }
        }


    }

    //This resets all the inputs
    public static void ResetSteeringInputs(FlightControlAI_Small smallShip)
    {
        if (smallShip != null)
        {
            smallShip.pitchInput = 0;
            smallShip.turnInput = 0;
            smallShip.rollInput = 0;
        }
    }

    #endregion
}