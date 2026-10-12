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

    //This adds an ai tag
    public static void AddSingleTag(FlightControlAI_Small flightControlAI, string tag)
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

    //This removes an ai tag
    public static void RemoveSingleTag(FlightControlAI_Small flightControlAI, string tag)
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

    //This runs the ai tags
    public static void RunTags(FlightControlAI_Small flightControlAI)
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

    //This checks if an ai tag exists
    public static bool TagExists(FlightControlAI_Small flightControlAI, string tag)
    {
        bool exists = false;

        foreach (string tempTag in flightControlAI.aiTags.ToArray())
        {
            if (tempTag == tag)
            {
                exists = true;
                break;
            }
        }
        
        return exists;
    }

    #endregion

    #region AI Speed Functions

    //This sets the ship at full speed
    public static void FullSpeedWithBoost(FlightControlAI_Small flightControlAI)
    {
        FlightControlSystem_Small flightControlSystem = flightControlAI.ship.flightControlSystem_Small;

        if (flightControlAI.aiMatchSpeed == false)
        {
            if (flightControlAI.boostIsActive == true)
            {
                flightControlSystem.thrustInput = 1;
            }
            else if (flightControlAI.boostIsActive == false)
            {
                if (flightControlSystem.thrustSpeed > flightControlAI.ship.speedRating)
                {
                    flightControlSystem.thrustInput = -1;
                }
                else
                {
                    flightControlSystem.thrustInput = 1;
                }
            }

            //This prevents the ship using the boost until it reaches full
            float weplimit = 50;

            if (flightControlAI.ship.energyManagementSystem.powerMode == "engines")
            {
                weplimit = 100;
            }

            if (flightControlAI.ship.energyManagementSystem.wepLevel >= weplimit)
            {
                flightControlAI.boostIsActive = true;
            }
            else if (flightControlAI.ship.energyManagementSystem.wepLevel <= 0)
            {
                flightControlAI.boostIsActive = false;
            }
        }
        else if (flightControlAI.ship.targetingSystem.target != null & flightControlAI.flyInFormation == false || flightControlAI.followTarget != null & flightControlAI.flyInFormation == true)
        {
            MatchSpeed(flightControlAI);
        }
        else
        {
            HalfSpeed(flightControlAI);
        }
        
    }

    //This sets the ship at full speed
    public static void FullSpeed(FlightControlAI_Small flightControlAI)
    {
        FlightControlSystem_Small flightControlSystem = flightControlAI.ship.flightControlSystem_Small;

        if (flightControlAI.aiMatchSpeed == false)
        {
            if (flightControlSystem.thrustSpeed > flightControlAI.ship.speedRating)
            {
                flightControlSystem.thrustInput = -1;
            }
            else
            {
                flightControlSystem.thrustInput = 1;
            }
        }
        else if (flightControlAI.ship.targetingSystem.target != null & flightControlAI.flyInFormation == false || flightControlAI.followTarget != null & flightControlAI.flyInFormation == true)
        {
            MatchSpeed(flightControlAI);
        }
        else
        {
            HalfSpeed(flightControlAI);
        } 
    }

    //This sets the ship to three quarter speed
    public static void ThreeQuarterSpeed(FlightControlAI_Small flightControlAI)
    {
        FlightControlSystem_Small flightControlSystem = flightControlAI.ship.flightControlSystem_Small;

        if (flightControlAI.aiMatchSpeed == false)
        {
            float threeQuarterSpeed = (flightControlAI.ship.speedRating / 4f) * 3;

            if (flightControlSystem.thrustSpeed > threeQuarterSpeed)
            {
                flightControlSystem.thrustInput = -1;
            }
            else
            {
                flightControlSystem.thrustInput = 1;
            }
        }
        else if (flightControlAI.ship.targetingSystem.target != null & flightControlAI.flyInFormation == false || flightControlAI.followTarget != null & flightControlAI.flyInFormation == true)
        {
            MatchSpeed(flightControlAI);
        }
        else
        {
            HalfSpeed(flightControlAI);
        }

    }

    //This sets the ship to half speed 
    public static void HalfSpeed(FlightControlAI_Small flightControlAI)
    {
        FlightControlSystem_Small flightControlSystem = flightControlAI.ship.flightControlSystem_Small;

        if (flightControlAI.aiMatchSpeed == false)
        {
            float halfSpeed = (flightControlAI.ship.speedRating / 2f);

            if (flightControlSystem.thrustSpeed > halfSpeed)
            {
                flightControlSystem.thrustInput = -1;
            }
            else
            {
                flightControlSystem.thrustInput = 1;
            }
        }
        else if (flightControlAI.ship.targetingSystem.target != null & flightControlAI.flyInFormation == false || flightControlAI.followTarget != null & flightControlAI.flyInFormation == true)
        {
            MatchSpeed(flightControlAI);
        }
        else
        {
            float halfSpeed = (flightControlAI.ship.speedRating / 2f);

            if (flightControlSystem.thrustSpeed > halfSpeed)
            {
                flightControlSystem.thrustInput = -1;
            }
            else
            {
                flightControlSystem.thrustInput = 1;
            }
        }       
    }

    //This sets the ship to quarter speed
    public static void QuarterSpeed(FlightControlAI_Small flightControlAI)
    {
        FlightControlSystem_Small flightControlSystem = flightControlAI.ship.flightControlSystem_Small;

        if (flightControlAI.aiMatchSpeed == false)
        {
            float quarterSpeed = (flightControlAI.ship.speedRating / 4f);

            if (flightControlSystem.thrustSpeed > quarterSpeed)
            {
                flightControlSystem.thrustInput = -1;
            }
            else
            {
                flightControlSystem.thrustInput = 1;
            }
        }
        else if (flightControlAI.ship.targetingSystem.target != null & flightControlAI.flyInFormation == false || flightControlAI.followTarget != null & flightControlAI.flyInFormation == true)
        {
            MatchSpeed(flightControlAI);
        }
        else
        {
            HalfSpeed(flightControlAI);
        } 
    }

    //This changes the speed of the ship dynamically to allow for a fast speed and sharp turns
    public static void DynamicSpeed(FlightControlAI_Small flightControlAI)
    {
        FlightControlSystem_Small flightControlSystem = flightControlAI.ship.flightControlSystem_Small;

        if (flightControlAI.ship.targetingSystem.target != null)
        {
            if (flightControlAI.aiMatchSpeed == false)
            {
                if (flightControlAI.ship.targetingSystem.targetForward < 0.5f)
                {
                    float halfSpeed = (flightControlAI.ship.speedRating / 2f);

                    if (flightControlSystem.thrustSpeed > halfSpeed)
                    {
                        flightControlSystem.thrustInput = -1;
                    }
                    else
                    {
                        flightControlSystem.thrustInput = 1;
                    }
                }
                else
                {
                    FullSpeed(flightControlAI);
                }
            }
            else
            {
                MatchSpeed(flightControlAI);
            }
        }
        else
        {
            HalfSpeed(flightControlAI);
        }
        
    }

    //This sets the ship to half speed (typically used when no enemies are detected)
    public static void NoSpeed(FlightControlAI_Small flightControlAI)
    {
        FlightControlSystem_Small flightControlSystem = flightControlAI.ship.flightControlSystem_Small;

        if (flightControlSystem != null)
        {
            if (flightControlSystem.thrustSpeed > 0)
            {
                flightControlSystem.thrustInput = -1;
            }
            else
            {
                flightControlSystem.thrustInput = 0;
            }
        }
    }

    //This sets the ship to half speed (typically used when no enemies are detected)
    public static void MatchSpeed(FlightControlAI_Small flightControlAI)
    {
        FlightControlSystem_Small flightControlSystem = flightControlAI.ship.flightControlSystem_Small;

        float oneThird = (flightControlAI.ship.speedRating / 3f);
        float oneHalf = (flightControlAI.ship.speedRating / 2f);

        if (flightControlSystem.thrustSpeed > flightControlAI.ship.targetingSystem.targetSpeed & flightControlSystem.thrustSpeed > oneThird)
        {
            flightControlSystem.thrustSpeed -= 1;
        }
        else
        {
            flightControlSystem.thrustSpeed += 1;
        }

        //This corrects the input for the follow target if necessary
        if (flightControlAI.followTarget != null & flightControlAI.flyInFormation == true)
        {
            flightControlSystem.thrustInput = 1;

            //This gets the formation position and distance
            Quaternion flatLeaderRotation = Quaternion.Euler(0, flightControlAI.followTarget.transform.eulerAngles.y, 0);

            Vector3 desiredPosition = flightControlAI.followTarget.transform.position + flatLeaderRotation * new Vector3(flightControlAI.xFormationPos, flightControlAI.yFormationPos, flightControlAI.zFormationPos);

            float distance = Vector3.Distance(flightControlAI.transform.position, desiredPosition);

            //This slows the ship down for the turn
            Vector3 targetRelativePosition = desiredPosition - flightControlAI.transform.position;

            float followTargetForward = Vector3.Dot(flightControlAI.transform.forward, targetRelativePosition.normalized);

            if (followTargetForward < 0.25f)
            {
                if (flightControlSystem.thrustSpeed > oneHalf)
                {
                    //smallShip.thrustInput = -1;
                    flightControlSystem.thrustSpeed -= 1;
                }
                else
                {
                    //smallShip.thrustInput = 1;
                    flightControlSystem.thrustSpeed += 1;
                }
            }

            //This dynamically adjusts the speed to stay in formation
            float breakingDistance = 500;

            float aggressionFactor = 1f;

            float decimalPercentage = (distance / breakingDistance) * aggressionFactor;

            if (distance > 15 & distance < breakingDistance)
            {
                float dynamicSpeed = ((flightControlAI.ship.speedRating - flightControlAI.followTarget.thrustSpeed) * decimalPercentage) + flightControlAI.followTarget.thrustSpeed;

                if (flightControlSystem.thrustSpeed > dynamicSpeed)
                {
                    flightControlSystem.thrustSpeed -= 1;
                }
                else if (flightControlSystem.thrustSpeed < dynamicSpeed)
                {
                    flightControlSystem.thrustSpeed += 1;
                }
            }
            else if (distance <= 15 & distance < breakingDistance)
            {
                flightControlSystem.thrustSpeed -= 1;
            }
            else
            {
                flightControlSystem.thrustSpeed += 1;
            }
        }
        
    }

    #endregion

    #region AI Weapon Control

    //This fires one laser at a time
    public static void SingleLaser(FlightControlAI_Small flightControlAI)
    {
        TargetingSystem targetingSystem = flightControlAI.ship.targetingSystem;
        WeaponManagement weaponManagement = flightControlAI.ship.weaponManagement;
        EnergyWeapon energyWeapon = flightControlAI.ship.energyWeapon;

        if (targetingSystem.target != null)
        {
            weaponManagement.weaponType = "lasers";

            if (targetingSystem.interceptForward > 0.95f & targetingSystem.interceptDistance < 2000 & targetingSystem.target.gameObject.activeSelf == true & energyWeapon.energyWeaponRecharged == true)
            {
                bool dontFire = CheckFire(flightControlAI);

                if (dontFire == false)
                {
                    weaponManagement.weaponMode = "single";
                    EnergyWeaponFunctions.InitiateFiring(energyWeapon);
                }
            }
        }
    }

    //This fires two lasers at a time
    public static void DualLasers(FlightControlAI_Small flightControlAI)
    {
        TargetingSystem targetingSystem = flightControlAI.ship.targetingSystem;
        WeaponManagement weaponManagement = flightControlAI.ship.weaponManagement;
        EnergyWeapon energyWeapon = flightControlAI.ship.energyWeapon;

        if (targetingSystem.target != null)
        {
            weaponManagement.weaponType = "lasers";

            if (targetingSystem.interceptForward > 0.95f & targetingSystem.interceptDistance < 2000 & targetingSystem.target.gameObject.activeSelf == true & energyWeapon.energyWeaponRecharged == true)
            {
                bool dontFire = CheckFire(flightControlAI);

                if (dontFire == false)
                {
                    weaponManagement.weaponMode = "dual";
                    EnergyWeaponFunctions.InitiateFiring(energyWeapon);
                }
            }
        }
    }

    //This fires all the ships lasers at once
    public static void AllLasers(FlightControlAI_Small flightControlAI)
    {
        TargetingSystem targetingSystem = flightControlAI.ship.targetingSystem;
        WeaponManagement weaponManagement = flightControlAI.ship.weaponManagement;
        EnergyWeapon energyWeapon = flightControlAI.ship.energyWeapon;

        if (targetingSystem.target != null)
        {
            weaponManagement.weaponType = "lasers";

            if (targetingSystem.interceptForward > 0.95f & targetingSystem.interceptDistance < 2000 & targetingSystem.target.gameObject.activeSelf == true & energyWeapon.energyWeaponRecharged == true)
            {
                bool dontFire = CheckFire(flightControlAI);

                if (dontFire == false)
                {
                    weaponManagement.weaponMode = "all";
                    EnergyWeaponFunctions.InitiateFiring(energyWeapon);
                }
            }
        }
    }

    //This fires one laser at a time
    public static void RapidLasers(FlightControlAI_Small flightControlAI)
    {
        TargetingSystem targetingSystem = flightControlAI.ship.targetingSystem;
        WeaponManagement weaponManagement = flightControlAI.ship.weaponManagement;
        EnergyWeapon energyWeapon = flightControlAI.ship.energyWeapon;

        if (targetingSystem.target != null)
        {
            weaponManagement.weaponType = "lasers";

            if (targetingSystem.interceptForward > 0.95f & targetingSystem.interceptDistance < 2000 & targetingSystem.target.gameObject.activeSelf == true)
            {
                bool dontFire = CheckFire(flightControlAI);

                if (dontFire == false & weaponManagement.hasRapidFire == true)
                {
                    weaponManagement.weaponMode = "rapid";
                    EnergyWeaponFunctions.InitiateFiring(energyWeapon);
                }
                else if (dontFire == false)
                {
                    weaponManagement.weaponMode = "single";
                    EnergyWeaponFunctions.InitiateFiring(energyWeapon);
                }
            }
        }
    }

    //This fires one laser at a time
    public static void SinglePlasma(FlightControlAI_Small flightControlAI)
    {
        TargetingSystem targetingSystem = flightControlAI.ship.targetingSystem;
        WeaponManagement weaponManagement = flightControlAI.ship.weaponManagement;
        EnergyWeapon energyWeapon = flightControlAI.ship.energyWeapon;

        if (targetingSystem.target != null)
        {
            weaponManagement.weaponType = "plasma";

            if (targetingSystem.interceptForward > 0.95f & targetingSystem.interceptDistance < 2000 & targetingSystem.target.gameObject.activeSelf == true & energyWeapon.energyWeaponRecharged == true)
            {
                bool dontFire = CheckFire(flightControlAI);

                if (dontFire == false)
                {
                    weaponManagement.weaponMode = "single";
                    EnergyWeaponFunctions.InitiateFiring(energyWeapon);
                }
            }
        }
    }

    //This fires two lasers at a time
    public static void DualPlasma(FlightControlAI_Small flightControlAI)
    {
        TargetingSystem targetingSystem = flightControlAI.ship.targetingSystem;
        WeaponManagement weaponManagement = flightControlAI.ship.weaponManagement;
        EnergyWeapon energyWeapon = flightControlAI.ship.energyWeapon;

        if (targetingSystem.target != null)
        {
            weaponManagement.weaponType = "plasma";

            if (targetingSystem.interceptForward > 0.95f & targetingSystem.interceptDistance < 2000 & targetingSystem.target.gameObject.activeSelf == true & energyWeapon.energyWeaponRecharged == true)
            {
                bool dontFire = CheckFire(flightControlAI);

                if (dontFire == false)
                {
                    weaponManagement.weaponMode = "dual";
                    EnergyWeaponFunctions.InitiateFiring(energyWeapon);
                }
            }
        }
    }

    //This fires all the ships lasers at once
    public static void AllPlasma(FlightControlAI_Small flightControlAI)
    {
        TargetingSystem targetingSystem = flightControlAI.ship.targetingSystem;
        WeaponManagement weaponManagement = flightControlAI.ship.weaponManagement;
        EnergyWeapon energyWeapon = flightControlAI.ship.energyWeapon;

        if (targetingSystem.target != null)
        {
            weaponManagement.weaponType = "plasma";

            if (targetingSystem.interceptForward > 0.95f & targetingSystem.interceptDistance < 2000 & targetingSystem.target.gameObject.activeSelf == true & energyWeapon.energyWeaponRecharged == true)
            {
                bool dontFire = CheckFire(flightControlAI);

                if (dontFire == false)
                {
                    weaponManagement.weaponMode = "all";
                    EnergyWeaponFunctions.InitiateFiring(energyWeapon);
                }
            }
        }
    }

    //This fires one ion at a time
    public static void SingleIon(FlightControlAI_Small flightControlAI)
    {
        TargetingSystem targetingSystem = flightControlAI.ship.targetingSystem;
        WeaponManagement weaponManagement = flightControlAI.ship.weaponManagement;
        EnergyWeapon energyWeapon = flightControlAI.ship.energyWeapon;

        if (targetingSystem.target != null)
        {
            weaponManagement.weaponType = "ion";

            if (targetingSystem.interceptForward > 0.95f & targetingSystem.interceptDistance < 2000 & targetingSystem.target.gameObject.activeSelf == true & energyWeapon.energyWeaponRecharged == true)
            {
                bool dontFire = CheckFire(flightControlAI);

                if (dontFire == false)
                {
                    weaponManagement.weaponMode = "single";
                    EnergyWeaponFunctions.InitiateFiring(energyWeapon);
                }
            }
        }
    }

    //This fires two ions at a time
    public static void DualIon(FlightControlAI_Small flightControlAI)
    {
        TargetingSystem targetingSystem = flightControlAI.ship.targetingSystem;
        WeaponManagement weaponManagement = flightControlAI.ship.weaponManagement;
        EnergyWeapon energyWeapon = flightControlAI.ship.energyWeapon;

        if (targetingSystem.target != null)
        {
            weaponManagement.weaponType = "ion";

            if (targetingSystem.interceptForward > 0.95f & targetingSystem.interceptDistance < 2000 & targetingSystem.target.gameObject.activeSelf == true & energyWeapon.energyWeaponRecharged == true)
            {
                bool dontFire = CheckFire(flightControlAI);

                if (dontFire == false)
                {
                    weaponManagement.weaponMode = "dual";
                    EnergyWeaponFunctions.InitiateFiring(energyWeapon);
                }
            }
        }
    }

    //This fires all the ships ions at once
    public static void AllIon(FlightControlAI_Small flightControlAI)
    {
        TargetingSystem targetingSystem = flightControlAI.ship.targetingSystem;
        WeaponManagement weaponManagement = flightControlAI.ship.weaponManagement;
        EnergyWeapon energyWeapon = flightControlAI.ship.energyWeapon;

        if (targetingSystem.target != null)
        {
            weaponManagement.weaponType = "ion";

            if (targetingSystem.interceptForward > 0.95f & targetingSystem.interceptDistance < 2000 & targetingSystem.target.gameObject.activeSelf == true & energyWeapon.energyWeaponRecharged == true)
            {
                bool dontFire = CheckFire(flightControlAI);

                if (dontFire == false)
                {
                    weaponManagement.weaponMode = "all";
                    EnergyWeaponFunctions.InitiateFiring(energyWeapon);
                }
            }
        }      
    }

    //This fires one ion at a time
    public static void RapidIon(FlightControlAI_Small flightControlAI)
    {
        TargetingSystem targetingSystem = flightControlAI.ship.targetingSystem;
        WeaponManagement weaponManagement = flightControlAI.ship.weaponManagement;
        EnergyWeapon energyWeapon = flightControlAI.ship.energyWeapon;

        if (targetingSystem.target != null)
        {
            weaponManagement.weaponType = "ion";

            if (targetingSystem.interceptForward > 0.95f & targetingSystem.interceptDistance < 2000 & targetingSystem.target.gameObject.activeSelf == true)
            {
                bool dontFire = CheckFire(flightControlAI);

                if (dontFire == false & weaponManagement.hasRapidFire == true)
                {
                    weaponManagement.weaponMode = "rapid";
                    EnergyWeaponFunctions.InitiateFiring(energyWeapon);
                }
                else if (dontFire == false)
                {
                    weaponManagement.weaponMode = "single";
                    EnergyWeaponFunctions.InitiateFiring(energyWeapon);
                }
            }
        }
    }

    //This fires one torpedo at a time
    public static void SingleTorpedo(FlightControlAI_Small flightControlAI)
    {
        TargetingSystem targetingSystem = flightControlAI.ship.targetingSystem;
        WeaponManagement weaponManagement = flightControlAI.ship.weaponManagement;
        TorpedoSystem torpedoSystem = flightControlAI.ship.torpedoSystem;

        if (flightControlAI.ship.targetingSystem.target != null)
        {
            weaponManagement.weaponType = "torpedos";

            if (torpedoSystem.torpedoNumber > 0)
            {
                if (targetingSystem.targetForward > 0.995f & torpedoSystem.torpedoLockedOn == true)
                {
                    weaponManagement.weaponMode = "single";
                    TorpedoSystemFunctions.FireTorpedo(flightControlAI.ship);
                }
            }
            else
            {
                weaponManagement.weaponType = "lasers";
                SingleLaser(flightControlAI);
            }
        }
    }

    //This fires two  torpedo at once
    public static void DualTorpedos(FlightControlAI_Small flightControlAI)
    {
        TargetingSystem targetingSystem = flightControlAI.ship.targetingSystem;
        WeaponManagement weaponManagement = flightControlAI.ship.weaponManagement;
        TorpedoSystem torpedoSystem = flightControlAI.ship.torpedoSystem;

        if (targetingSystem.target != null)
        {
            weaponManagement.weaponType = "torpedos";

            if (torpedoSystem.torpedoNumber > 0)
            {
                if (targetingSystem.targetForward > 0.995f & torpedoSystem.torpedoLockedOn == true)
                {
                    weaponManagement.weaponMode = "dual";
                    TorpedoSystemFunctions.FireTorpedo(flightControlAI.ship);
                }
            }
            else
            {
                weaponManagement.weaponType = "lasers";
                SingleLaser(flightControlAI);
            }
        }
    }

    //This fires from all torpedo tubes
    public static void AllTorpedos(FlightControlAI_Small flightControlAI)
    {
        TargetingSystem targetingSystem = flightControlAI.ship.targetingSystem;
        WeaponManagement weaponManagement = flightControlAI.ship.weaponManagement;
        TorpedoSystem torpedoSystem = flightControlAI.ship.torpedoSystem;

        if (targetingSystem.target != null)
        {
            weaponManagement.weaponType = "torpedos";

            if (torpedoSystem.torpedoNumber > 0)
            {
                if (targetingSystem.targetForward > 0.995f & torpedoSystem.torpedoLockedOn == true)
                {
                    weaponManagement.weaponMode = "all";
                    TorpedoSystemFunctions.FireTorpedo(flightControlAI.ship);
                }
            }
            else
            {
                weaponManagement.weaponType = "lasers";
                SingleLaser(flightControlAI);
            }
        }      
    }

    //This switches between single lasers and torpedos
    public static void DynamicWeapons_Single(FlightControlAI_Small flightControlAI)
    {
        TargetingSystem targetingSystem = flightControlAI.ship.targetingSystem;
        WeaponManagement weaponManagement = flightControlAI.ship.weaponManagement;
        TorpedoSystem torpedoSystem = flightControlAI.ship.torpedoSystem;
        EnergyWeapon energyWeapon = flightControlAI.ship.energyWeapon;

        if (targetingSystem.target != null)
        {
            if (torpedoSystem.torpedoNumber > 0 & targetingSystem.interceptDistance > 2000 & targetingSystem.target.gameObject.activeSelf == true)
            {
                weaponManagement.weaponType = "torpedos";

                if (targetingSystem.targetForward > 0.995f & torpedoSystem.torpedoLockedOn == true)
                {
                    weaponManagement.weaponMode = "single";
                    TorpedoSystemFunctions.FireTorpedo(flightControlAI.ship);
                }
            }
            else if (targetingSystem.interceptForward > 0.95f & targetingSystem.interceptDistance < 2000 & targetingSystem.target.gameObject.activeSelf == true)
            {
                if (weaponManagement.hasPlasma == false)
                {
                    weaponManagement.weaponType = "lasers";
                }
                else
                {
                    weaponManagement.weaponType = "plasma";
                }

                bool dontFire = CheckFire(flightControlAI);

                if (dontFire == false & energyWeapon.energyWeaponRecharged == true)
                {
                    weaponManagement.weaponMode = "single";
                    EnergyWeaponFunctions.InitiateFiring(energyWeapon);
                }
            }
        }
    }

    //This switches betwene dual lasers and torpedos
    public static void DynamicWeapons_Dual(FlightControlAI_Small flightControlAI)
    {
        TargetingSystem targetingSystem = flightControlAI.ship.targetingSystem;
        WeaponManagement weaponManagement = flightControlAI.ship.weaponManagement;
        TorpedoSystem torpedoSystem = flightControlAI.ship.torpedoSystem;
        EnergyWeapon energyWeapon = flightControlAI.ship.energyWeapon;

        if (targetingSystem.target != null)
        {
            if (torpedoSystem.torpedoNumber > 0 & targetingSystem.interceptDistance > 2000 & targetingSystem.target.gameObject.activeSelf == true)
            {
                weaponManagement.weaponType = "torpedos";

                if (targetingSystem.targetForward > 0.995f & torpedoSystem.torpedoLockedOn == true)
                {
                    weaponManagement.weaponMode = "dual";
                    TorpedoSystemFunctions.FireTorpedo(flightControlAI.ship);
                }
            }
            else if (targetingSystem.interceptForward > 0.95f & targetingSystem.interceptDistance < 2000 & targetingSystem.target.gameObject.activeSelf == true)
            {
                if (weaponManagement.hasPlasma == false)
                {
                    weaponManagement.weaponType = "lasers";
                }
                else
                {
                    weaponManagement.weaponType = "plasma";
                }

                bool dontFire = CheckFire(flightControlAI);

                if (dontFire == false)
                {
                    weaponManagement.weaponMode = "dual";
                    EnergyWeaponFunctions.InitiateFiring(energyWeapon);
                }
            }
        }
        
    }

    //This switches betwene dual lasers and torpedos
    public static void DynamicWeapons_All(FlightControlAI_Small flightControlAI)
    {
        TargetingSystem targetingSystem = flightControlAI.ship.targetingSystem;
        WeaponManagement weaponManagement = flightControlAI.ship.weaponManagement;
        TorpedoSystem torpedoSystem = flightControlAI.ship.torpedoSystem;
        EnergyWeapon energyWeapon = flightControlAI.ship.energyWeapon;

        if (targetingSystem.target != null)
        {
            if (torpedoSystem.torpedoNumber > 0 & targetingSystem.interceptDistance > 2000 & targetingSystem.target.gameObject.activeSelf == true)
            {
                weaponManagement.weaponType = "torpedos";

                if (targetingSystem.targetForward > 0.995f & torpedoSystem.torpedoLockedOn == true)
                {
                    weaponManagement.weaponMode = "all";
                    TorpedoSystemFunctions.FireTorpedo(flightControlAI.ship);
                }
            }
            else if (targetingSystem.interceptForward > 0.95f & targetingSystem.interceptDistance < 2000 & targetingSystem.target.gameObject.activeSelf == true)
            {
                if (weaponManagement.hasPlasma == false)
                {
                    weaponManagement.weaponType = "lasers";
                }
                else
                {
                    weaponManagement.weaponType = "plasma";
                }

                bool dontFire = CheckFire(flightControlAI);

                if (dontFire == false)
                {
                    weaponManagement.weaponMode = "all";
                    EnergyWeaponFunctions.InitiateFiring(energyWeapon);
                }
            }
        }
    }

    //This switches between rapid lasers and normal lasers
    public static void DynamicWeapons_Rapid(FlightControlAI_Small flightControlAI)
    {
        TargetingSystem targetingSystem = flightControlAI.ship.targetingSystem;
        WeaponManagement weaponManagement = flightControlAI.ship.weaponManagement;
        TorpedoSystem torpedoSystem = flightControlAI.ship.torpedoSystem;
        EnergyWeapon energyWeapon = flightControlAI.ship.energyWeapon;

        if (weaponManagement.hasPlasma == false)
        {
            weaponManagement.weaponType = "lasers";
        }
        else
        {
           weaponManagement.weaponType = "plasma";
        }

        if (targetingSystem.target != null)
        {
            if (targetingSystem.interceptForward > 0.95f & targetingSystem.target.gameObject.activeSelf == true)
            {
                if (weaponManagement.hasRapidFire == true)
                {
                    if (targetingSystem.targetShip != null)
                    {
                        if (targetingSystem.targetShip.weaponManagement.hasPlasma == true & targetingSystem.targetShip.energyManagementSystem.shieldLevel > 10)
                        {
                            weaponManagement.weaponMode = "rapid";
                            EnergyWeaponFunctions.InitiateFiring(energyWeapon);
                        }
                        else
                        {
                            weaponManagement.weaponMode = "single";
                            EnergyWeaponFunctions.InitiateFiring(energyWeapon);
                        }
                    }
                }
                else
                {
                    weaponManagement.weaponMode = "single";
                    EnergyWeaponFunctions.InitiateFiring(energyWeapon);
                }
            }
        }
    }
    
    //This checks whether a non hostile ship is in the firing line or not
    public static bool CheckFire(FlightControlAI_Small flightControlAI)
    {
        bool dontFire = false;

        if (flightControlAI != null)
        {
            RaycastHit hit;

            int layerMask = (1 << 6) | (1 << 7) | (1 << 8) | (1 << 9) | (1 << 10) | (1 << 11) | (1 << 12) | (1 << 13) | (1 << 14) | (1 << 15) | (1 << 16) | (1 << 17) | (1 << 18) | (1 << 19) | (1 << 20) | (1 << 21) | (1 << 22) | (1 << 23);

            Vector3 forwardRaycast = flightControlAI.gameObject.transform.position + (flightControlAI.gameObject.transform.forward * 10);

            if (Physics.SphereCast(forwardRaycast, 10, flightControlAI.gameObject.gameObject.transform.TransformDirection(Vector3.forward), out hit, 1000, layerMask))
            {
                FlightControlAI_Small otherSmallship = hit.collider.GetComponentInParent<FlightControlAI_Small>();

                if (otherSmallship != null)
                {
                    bool isHostile = TargetingSystemFunctions.GetHostility(flightControlAI.ship, otherSmallship.ship.allegiance);

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
    public static void LowAccuracy(FlightControlAI_Small flightControlAI)
    {
        if (flightControlAI != null)
        {
            SetTargetingErrorMargin(flightControlAI, "low");
        }
    }

    //This sets the targetting accuracy to medium
    public static void MediumAccuracy(FlightControlAI_Small flightControlAI)
    {
        if (flightControlAI != null)
        {
            SetTargetingErrorMargin(flightControlAI, "medium");
        }
    }

    //This sets the targetting accuracy to jihj
    public static void HighAccuracy(FlightControlAI_Small flightControlAI)
    {
        if (flightControlAI != null)
        {
            SetTargetingErrorMargin(flightControlAI, "high");
        }
    }

    //Targeting error margin
    public static void SetTargetingErrorMargin(FlightControlAI_Small flightControlAI, string mode)
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

        flightControlAI.aiTargetingErrorMargin = new Vector3(x, y, z);        
    }

    #endregion

    #region AI Flight Patterns

    //Chase: The enemy relenlessly pursues the player without withdrawing
    public static void Chase(FlightControlAI_Small flghtControlAI)
    {
        TargetingSystem targetingSystem = flghtControlAI.ship.targetingSystem;

        if (flghtControlAI.aiEvade == false)
        {
            if (targetingSystem.target != null)
            {
                if (targetingSystem.targetDistance > 250)
                {
                    flghtControlAI.aiMatchSpeed = false;
                    AngleTowardsTarget(flghtControlAI);
                }
                else
                {
                    flghtControlAI.aiMatchSpeed = true;
                    AngleTowardsTarget(flghtControlAI);
                }
            }
            else
            {
                PatrolRandom(flghtControlAI, true);
            }
        }

        flghtControlAI.flyInFormation = false;     
    }

    //Chase-Withdraw: Attack for 15-20 followed by withdrawal for 30-35 seconds
    public static void ChaseWithdraw(FlightControlAI_Small flightControlAI)
    {
        TargetingSystem targetingSystem = flightControlAI.ship.targetingSystem;

        if (flightControlAI.aiEvade == false)
        {
            if (targetingSystem.target != null)
            {
                if (targetingSystem.targetDistance > 250)
                {
                    flightControlAI.aiMatchSpeed = false;
                    AngleTowardsTarget(flightControlAI);
                }
                else
                {
                    if (Time.time < flightControlAI.aiAttackTime)
                    {
                        flightControlAI.aiMatchSpeed = true;
                        AngleTowardsTarget(flightControlAI);
                        flightControlAI.aiRetreatTime = Time.time + Random.Range(15, 20);
                    }
                    else
                    {
                        flightControlAI.aiMatchSpeed = false;

                        if (Time.time < flightControlAI.aiRetreatTime)
                        {
                            AngleAwayFromTarget(flightControlAI);
                        }
                        else
                        {
                            flightControlAI.aiAttackTime = Time.time + Random.Range(30, 35);
                        }
                    }
                }
            }
            else
            {
                PatrolRandom(flightControlAI, true);
            }
        }

        flightControlAI.flyInFormation = false;
    }

    //Strafe-Withdraw: Attacks enemy ship at distance before withdrawing again (typically used for attack large ships)
    public static void StrafeWithdraw(FlightControlAI_Small flightControlAI)
    {
        TargetingSystem targetingSystem = flightControlAI.ship.targetingSystem;

        if (flightControlAI != null)
        {
            if (flightControlAI.aiEvade == false)
            {
                if (flightControlAI != null)
                {
                    float attackDistance = 250;
                    float withdrawDistance = 1000;

                    if (targetingSystem.targetLargeShip != null)
                    {
                        if (targetingSystem.targetLargeShip.shipClass == "large")
                        {
                            attackDistance = 1500;
                            withdrawDistance = 3000;
                        }
                        else if (targetingSystem.targetLargeShip.shipClass == "middle")
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

                    if (targetingSystem.targetDistance > attackDistance & flightControlAI.withdraw == false)
                    {
                        AngleTowardsTarget(flightControlAI);
                    }
                    else if (targetingSystem.targetDistance < attackDistance & flightControlAI.withdraw == false)
                    {
                        flightControlAI.withdraw = true;
                    }
                    else if (targetingSystem.targetDistance > withdrawDistance & flightControlAI.withdraw == true)
                    {
                        flightControlAI.withdraw = false;
                    }
                    else
                    {
                        AngleAwayFromTarget(flightControlAI);
                    }
                }
                else
                {
                    PatrolRandom(flightControlAI, true);
                }
            }

            flightControlAI.flyInFormation = false;
        }
    }

    //This angles towards the ships waypoint
    public static void MoveToWayPoint(FlightControlAI_Small flightControlAI)
    {
        if (flightControlAI != null)
        {
            if (flightControlAI.aiEvade == false)
            {
                AngleTowardsWaypoint(flightControlAI);
            }

            flightControlAI.flyInFormation = false;
        }
    }

    //This is the basic flight pattern for patrolling
    public static void PatrolRandom(FlightControlAI_Small flightControlAI, bool stayClose = false)
    {
        TargetingSystem targetingSystem = flightControlAI.ship.targetingSystem;

        if (flightControlAI != null)
        {
            if (flightControlAI.aiEvade == false)
            {
                if (targetingSystem.waypoint != null)
                {
                    float distanceToWaypoint = Vector3.Distance(flightControlAI.gameObject.transform.position, targetingSystem.waypoint.transform.position);

                    if (distanceToWaypoint < 50)
                    {
                        SelectRandomWaypoint(flightControlAI, stayClose);
                    }

                    AngleTowardsWaypoint(flightControlAI);
                }
            }

            flightControlAI.flyInFormation = false;
        }
    }

    //This selects a random waypoint
    public static void SelectRandomWaypoint(FlightControlAI_Small flightControlAI, bool stayClose = false)
    {
        TargetingSystem targetingSystem = flightControlAI.ship.targetingSystem;

        if (flightControlAI.aiEvade == false)
        {
            float min = -flightControlAI.ship.scene.sceneRadius + 1000;
            float max = flightControlAI.ship.scene.sceneRadius - 1000;

            float x = Random.Range(min, max);
            float y = Random.Range(min, max);
            float z = Random.Range(min, max);

            //This makes ships with no target stay within range of the player/center of the scene
            if (stayClose == true)
            {
                Vector3 referencePosition = new Vector3(0, 0, 0);

                if (flightControlAI.ship.scene != null)
                {
                    Scene scene = flightControlAI.ship.scene;

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

            if (targetingSystem.waypoint != null)
            {
                targetingSystem.waypoint.transform.localPosition = new Vector3(x, y, z);
            }
        }
        
    }

    //This sets the ship to fly in formation
    public static void FormationFlying(FlightControlAI_Small flightControlAI)
    {
        if (flightControlAI != null)
        {
            if (flightControlAI.followTarget != null)
            {
                if (flightControlAI.isAI == true)
                {
                    flightControlAI.flyInFormation = true;
                }

                Quaternion flatLeaderRotation = Quaternion.Euler(0, flightControlAI.followTarget.transform.eulerAngles.y, 0);

                Vector3 desiredPosition = flightControlAI.followTarget.transform.position + flatLeaderRotation * new Vector3(flightControlAI.xFormationPos, flightControlAI.yFormationPos, flightControlAI.zFormationPos);

                float distance = Vector3.Distance(flightControlAI.transform.position, desiredPosition);

                flightControlAI.aiMatchSpeed = true;

                AngleTowardsPoint(flightControlAI, desiredPosition);
            }
            else
            {
                flightControlAI.aiMatchSpeed = false;
                flightControlAI.flyInFormation = false;
                ChaseWithdraw(flightControlAI);
            }
        }
    }

    //This prevents the ship from performing any rotations and locks the direction forward
    public static void NoRotation(FlightControlAI_Small flightControlAI)
    {
        if (flightControlAI != null)
        {
            ResetSteeringInputs(flightControlAI);
        }

        flightControlAI.flyInFormation = false;
        flightControlAI.positionLocked = false;
    }

    #endregion

    #region AI Energy Management

    //This sets shield power to maximum
    public static void EnergyToShields(FlightControlAI_Small flightControlAI)
    {
        flightControlAI.ship.energyManagementSystem.powerMode = "shields";        
    }

    //This sets engine power to maximum
    public static void EnergyToEngines(FlightControlAI_Small flightControlAI)
    {
        flightControlAI.ship.energyManagementSystem.powerMode = "engines";       
    }

    //This sets engine power to maximum
    public static void EnergyToLasers(FlightControlAI_Small flightControlAI)
    {
        flightControlAI.ship.energyManagementSystem.powerMode = "lasers";       
    }

    //This resets all energy levels
    public static void ResetEnergyLevels(FlightControlAI_Small flightControlAI)
    {
        flightControlAI.ship.energyManagementSystem.powerMode = "reset";      
    }

    //This uses the ships energy in a way that maxmises aggression but leaves lowers the defensive capabilites of the ship
    public static void EnergyAggressive(FlightControlAI_Small flightControlAI)
    {
        TargetingSystem targetingSystem = flightControlAI.ship.targetingSystem;
        EnergyManagementSystem energyManagementSystem = flightControlAI.ship.energyManagementSystem;

        if (targetingSystem.target != null)
        {
            if (targetingSystem.targetDistance < 1000 & targetingSystem.targetForward > 0)
            {
                energyManagementSystem.powerMode = "lasers";
            }
            else
            {
                energyManagementSystem.powerMode = "engines";
            }
        }
        else
        {
            energyManagementSystem.powerMode = "reset";
        }        
    }

    //This uses the ships energy in a way that maxmises defenses but minimises attack capabilities
    public static void EnergyProtective(FlightControlAI_Small flightControlAI)
    {
        TargetingSystem targetingSystem = flightControlAI.ship.targetingSystem;
        EnergyManagementSystem energyManagementSystem = flightControlAI.ship.energyManagementSystem;

        if (targetingSystem.target != null)
        {
            if (energyManagementSystem.shieldLevel < flightControlAI.ship.shieldRating / 2f)
            {
                energyManagementSystem.powerMode = "shields";
            }
            else
            {
                energyManagementSystem.powerMode = "reset";
            }
        }
        else
        {
            energyManagementSystem.powerMode = "reset";
        }
    }

    //This uses the ships energy in way that is most effective for both offense and defense
    public static void EnergyDynamic(FlightControlAI_Small flightControlAI)
    {
        TargetingSystem targetingSystem = flightControlAI.ship.targetingSystem;
        EnergyManagementSystem energyManagementSystem = flightControlAI.ship.energyManagementSystem;

        if (targetingSystem.target != null)
        {
            if (energyManagementSystem.shieldLevel < flightControlAI.ship.shieldRating / 2f)
            {
                energyManagementSystem.powerMode = "shields";
            }
            else if (targetingSystem.targetDistance < 1000 & targetingSystem.targetForward > 0)
            {
                energyManagementSystem.powerMode = "lasers";
            }
            else if (targetingSystem.targetDistance > 2000 & targetingSystem.targetForward > 0)
            {
                energyManagementSystem.powerMode = "engines";
            }
            else
            {
                energyManagementSystem.powerMode = "reset";
            }
        }
        else
        {
            energyManagementSystem.powerMode = "reset";
        }     
    }

    #endregion

    #region AI Targetting

    //This checks if the ship needs to request a new target - this function is run automatically
    public static void RequestTarget(FlightControlAI_Small flightControlAI)
    {
        TargetingSystem targetingSystem = flightControlAI.ship.targetingSystem;

        if (targetingSystem.target == null)
        {
            flightControlAI.requestingTarget = true;
        }
        else
        {
            flightControlAI.requestingTarget = false;
        }
    }

    //This clears the target if it doesn't meet certain conditions i.e. was destroyed or disabled  - this function is run automatically
    public static void ClearTarget(FlightControlAI_Small flightControlAI)
    {
        TargetingSystem targetingSystem = flightControlAI.ship.targetingSystem;

        if (targetingSystem.target != null)
        {
            if (targetingSystem.target.activeSelf == false)
            {
                targetingSystem.target = null;
            }
            else if (targetingSystem.targetShip != null)
            {
                if (targetingSystem.targetShip.damageSystem.isDisabled == true)
                {
                    targetingSystem.target = null;
                }
            }
            else if (targetingSystem.targetLargeShip != null)
            {
                if (targetingSystem.targetLargeShip.isDisabled == true)
                {
                    targetingSystem.target = null;
                }
            }
        }
    }

    //Tagets all ships but looks for small ships first
    public static void TargetAllPrefSmall(FlightControlAI_Small flightControlAI)
    {
        TargetingSystem targetingSystem = flightControlAI.ship.targetingSystem;

        if (flightControlAI.aiTargetingMode != "targetallprefsmall") //This ensures the previous target is cleared
        {
            targetingSystem.target = null;
            flightControlAI.requestingTarget = true;
        }

        flightControlAI.aiTargetingMode = "targetallprefsmall";
        
    }

    //Targets all ships but looks for large ships first
    public static void TargetAllPrefLarge(FlightControlAI_Small flightControlAI)
    {
        TargetingSystem targetingSystem = flightControlAI.ship.targetingSystem;

        if (flightControlAI != null)
        {
            if (flightControlAI.aiTargetingMode != "targetallpreflarge") //This ensures the previous target is cleared
            {
                targetingSystem.target = null;
                flightControlAI.requestingTarget = true;
            }

            flightControlAI.aiTargetingMode = "targetallpreflarge";
        }
    }

    //Targets only small ships
    public static void TargetSmallShipsOnly(FlightControlAI_Small flightControlAI)
    {
        TargetingSystem targetingSystem = flightControlAI.ship.targetingSystem;

        if (flightControlAI.aiTargetingMode != "targetsmallshipsonly") //This ensures the previous target is cleared
        {
            targetingSystem.target = null;
            flightControlAI.requestingTarget = true;
        }

        flightControlAI.aiTargetingMode = "targetsmallshipsonly";
    }

    //Targets only small ships
    public static void TargetLargeShipOnly(FlightControlAI_Small flightControlAI)
    {
        TargetingSystem targetingSystem = flightControlAI.ship.targetingSystem;

        if (flightControlAI.aiTargetingMode != "targetlargeshipsonly") //This ensures the previous target is cleared
        {
            targetingSystem.target = null;
            flightControlAI.requestingTarget = true;
        }

        flightControlAI.aiTargetingMode = "targetlargeshipsonly";      
    }

    #endregion

    #region AI Evasion and Collision Avoidance

    //This allows the ship to evade attacks and avoid collisions with other objects and ships
    public static IEnumerator Evade(Ship ship, float time, string mode, int direction)
    {
        FlightControlAI_Small flightControlAI_Small = ship.flightControlAI_Small;

        if (ship != null)
        {
            if (TagExists(ship.flightControlAI_Small, "nospeed") == true & TagExists(flightControlAI_Small, "norotation") == true || TagExists(flightControlAI_Small, "nocollisionevasion") == true || TagExists(flightControlAI_Small, "formationflying") == true)
            {
                //Do nothing
            }
            else
            {
                if (ship != null)
                {
                    flightControlAI_Small.aiEvade = true;

                    time = time + Time.time;

                    while (time > Time.time)
                    {
                        HalfSpeed(flightControlAI_Small);

                        if (direction == 0)
                        {
                            TurnRight(flightControlAI_Small);
                        }
                        else if (direction == 1)
                        {
                            TurnLeft(flightControlAI_Small);
                        }
                        else if (direction == 2)
                        {
                            PitchUp(flightControlAI_Small);
                        }
                        else if (direction == 3)
                        {
                            PitchDown(flightControlAI_Small);
                        }
                        else if (direction == 4)
                        {
                            RollRight(flightControlAI_Small);
                        }
                        else if (direction == 5)
                        {
                            RollLeft(flightControlAI_Small);
                        }
                        else if (direction == 6)
                        {
                            FlyFoward(flightControlAI_Small);
                        }

                        yield return null;

                    }

                    ResetSteeringInputs(flightControlAI_Small);

                    flightControlAI_Small.aiEvade = false;
                }
            }
        }
    }

    #endregion

    #region AI Steering Control

    //This angles the ship towards the target vector
    public static void AngleTowardsTarget(FlightControlAI_Small flightControlAI)
    {
        TargetingSystem targetingSystem = flightControlAI.ship.targetingSystem;
        FlightControlSystem_Small flightControlSystem = flightControlAI.ship.flightControlSystem_Small;

        if (flightControlAI != null)
        {
            AvoidGimbalLock(flightControlAI, targetingSystem.interceptForward);

            if (targetingSystem.target != null & flightControlAI.avoidGimbalLock == false)
            {
                if (targetingSystem.interceptForward < 0.8)
                {
                    if (Vector3.Dot(flightControlAI.transform.up, Vector3.down) < 0)
                    {
                        //Right way up
                        flightControlSystem.turnInput = targetingSystem.interceptRight;
                        flightControlSystem.pitchInput = -targetingSystem.interceptUp;

                    }
                    else
                    {
                        //Upside down
                        flightControlSystem.turnInput = -targetingSystem.interceptRight;
                        flightControlSystem.pitchInput = -targetingSystem.interceptUp;
                    }
                }
                else
                {
                    //Smoothly interpolate the multiplier from 1 to 5 to prevent a jerk
                    float t = Mathf.InverseLerp(0.8f, 1.0f, targetingSystem.interceptForward);
                    float multiplier = Mathf.SmoothStep(1f, 5f, t);

                    if (Vector3.Dot(flightControlAI.transform.up, Vector3.down) < 0)
                    {
                        //Right way up
                        flightControlSystem.turnInput = targetingSystem.interceptRight * multiplier;
                        flightControlSystem.pitchInput = -targetingSystem.interceptUp * multiplier;
                    }
                    else
                    {
                        //Upside down
                        flightControlSystem.turnInput = -targetingSystem.interceptRight * multiplier;
                        flightControlSystem.pitchInput = -targetingSystem.interceptUp * multiplier;
                    }
                }
            }
        }
    }

    //This angles the ship away from the target vector
    public static void AngleAwayFromTarget(FlightControlAI_Small flightControlAI)
    {
        TargetingSystem targetingSystem = flightControlAI.ship.targetingSystem;
        FlightControlSystem_Small flightControlSystem = flightControlAI.ship.flightControlSystem_Small;

        if (flightControlAI != null)
        {
            AvoidGimbalLock(flightControlAI, targetingSystem.interceptForward, true);

            if (targetingSystem.target != null & flightControlAI.avoidGimbalLock == false)
            {
                if (-targetingSystem.interceptForward < 1)
                {
                    if (Vector3.Dot(flightControlAI.transform.up, Vector3.down) < 0)
                    {
                        //Right way up
                        flightControlSystem.turnInput = -targetingSystem.interceptRight;
                        flightControlSystem.pitchInput = targetingSystem.interceptUp;
                    }
                    else
                    {
                        //Upside down
                        flightControlSystem.turnInput = targetingSystem.interceptRight;
                        flightControlSystem.pitchInput = -targetingSystem.interceptUp;
                    }
                }
            }
        }
    }

    //This angles the ship towards the target vector
    public static void AngleTowardsWaypoint(FlightControlAI_Small flightControlAI)
    {
        TargetingSystem targetingSystem = flightControlAI.ship.targetingSystem;
        FlightControlSystem_Small flightControlSystem = flightControlAI.ship.flightControlSystem_Small;

        AvoidGimbalLock(flightControlAI, targetingSystem.waypointForward);

        if (targetingSystem.waypoint != null & flightControlAI.avoidGimbalLock == false)
        {
            if (targetingSystem.waypointForward < 0.8)
            {
                if (Vector3.Dot(flightControlAI.transform.up, Vector3.down) < 0)
                {
                    //Right way up
                    flightControlSystem.turnInput = targetingSystem.waypointRight;
                    flightControlSystem.pitchInput = -targetingSystem.waypointUp;

                }
                else
                {
                    //Upside down
                    flightControlSystem.turnInput = -targetingSystem.waypointRight;
                    flightControlSystem.pitchInput = -targetingSystem.waypointUp;
                }
            }
            else
            {
                //Smoothly interpolate the multiplier from 1 to 5 to prevent a jerk
                float t = Mathf.InverseLerp(0.8f, 1.0f, targetingSystem.waypointForward);
                float multiplier = Mathf.SmoothStep(1f, 5f, t);

                if (Vector3.Dot(flightControlAI.transform.up, Vector3.down) < 0)
                {
                    //Right way up
                    flightControlSystem.turnInput = targetingSystem.waypointRight * multiplier;
                    flightControlSystem.pitchInput = -targetingSystem.waypointUp * multiplier;
                }
                else
                {
                    //Upside down
                    flightControlSystem.turnInput = -targetingSystem.waypointRight * multiplier;
                    flightControlSystem.pitchInput = -targetingSystem.waypointUp * multiplier;
                }
            }
        }
    }

    //This angles the ship away from the target vector
    public static void AngleAwayFromWaypoint(FlightControlAI_Small flightControlAI)
    {
        TargetingSystem targetingSystem = flightControlAI.ship.targetingSystem;
        FlightControlSystem_Small flightControlSystem = flightControlAI.ship.flightControlSystem_Small;

        AvoidGimbalLock(flightControlAI, targetingSystem.waypointForward, true);

        if (targetingSystem.waypoint != null & flightControlAI.avoidGimbalLock == false)
        {
            if (targetingSystem.waypointForward > -0.95)
            {
                if (Vector3.Dot(flightControlAI.transform.up, Vector3.down) < 0)
                {
                    //Right way up
                    flightControlSystem.turnInput = -targetingSystem.waypointRight;
                    flightControlSystem.pitchInput = targetingSystem.waypointUp;
                }
                else
                {
                    //Upside down
                    flightControlSystem.turnInput = targetingSystem.waypointRight;
                    flightControlSystem.pitchInput = -targetingSystem.waypointUp;
                }
            }
        }      
    }

    //This angles the ship towards the target vector
    public static void AngleTowardsPoint(FlightControlAI_Small flightControlAI, Vector3 point)
    {
        TargetingSystem targetingSystem = flightControlAI.ship.targetingSystem;
        FlightControlSystem_Small flightControlSystem = flightControlAI.ship.flightControlSystem_Small;

        Vector3 targetPosition = point;

        Vector3 targetRelativePosition = targetPosition - flightControlAI.transform.position;

        float targetForward = Vector3.Dot(flightControlAI.transform.forward, targetRelativePosition.normalized);
        float targetRight = Vector3.Dot(flightControlAI.transform.right, targetRelativePosition.normalized);
        float targetUp = Vector3.Dot(flightControlAI.transform.up, targetRelativePosition.normalized);

        AvoidGimbalLock(flightControlAI, targetForward);

        if (flightControlAI.avoidGimbalLock == false)
        {
            if (targetForward < 0.8)
            {
                if (Vector3.Dot(flightControlAI.transform.up, Vector3.down) < 0)
                {
                    flightControlSystem.turnInput = targetRight;
                    flightControlSystem.pitchInput = -targetUp;
                }
                else
                {
                    //Upside down
                    flightControlSystem.turnInput = -targetRight;
                    flightControlSystem.pitchInput = -targetUp;
                }
            }
            else
            {
                //Smoothly interpolate the multiplier from 1 to 5 to prevent a jerk
                float t = Mathf.InverseLerp(0.8f, 1.0f, targetForward);
                float multiplier = Mathf.SmoothStep(1f, 5f, t);

                if (Vector3.Dot(flightControlAI.transform.up, Vector3.down) < 0)
                {
                    //Right way up
                    flightControlSystem.turnInput = targetRight * multiplier;
                    flightControlSystem.pitchInput = -targetUp * multiplier;
                }
                else
                {
                    //Upside down
                    flightControlSystem.turnInput = -targetRight * multiplier;
                    flightControlSystem.pitchInput = -targetUp * multiplier;
                }
            }
        }
        
    }

    //This pitches the ship up
    public static void PitchUp(FlightControlAI_Small flightControlAI)
    {
        FlightControlSystem_Small flightControlSystem = flightControlAI.ship.flightControlSystem_Small;

        flightControlSystem.pitchInput = 1;
    }

    //This pitches the ship down
    public static void PitchDown(FlightControlAI_Small flightControlAI)
    {
        FlightControlSystem_Small flightControlSystem = flightControlAI.ship.flightControlSystem_Small;

        flightControlSystem.pitchInput = -1;
    }

    //This turns the ship right
    public static void TurnRight(FlightControlAI_Small flightControlAI)
    {
        FlightControlSystem_Small flightControlSystem = flightControlAI.ship.flightControlSystem_Small;

        flightControlSystem.turnInput = 1;
    }

    //This turns the ship Left
    public static void TurnLeft(FlightControlAI_Small flightControlAI)
    {
        FlightControlSystem_Small flightControlSystem = flightControlAI.ship.flightControlSystem_Small;

        flightControlSystem.turnInput = -1;
    }

    //This causes the ship to roll Right
    public static void RollRight(FlightControlAI_Small flightControlAI)
    {
        FlightControlSystem_Small flightControlSystem = flightControlAI.ship.flightControlSystem_Small;

        flightControlSystem.rollInput = 1;
    }

    //This causes the ship to roll left
    public static void RollLeft(FlightControlAI_Small flightControlAI)
    {
        FlightControlSystem_Small flightControlSystem = flightControlAI.ship.flightControlSystem_Small;

        flightControlSystem.rollInput = -1;
    }

    //This causes the ship to fly forward
    public static void FlyFoward(FlightControlAI_Small flightControlAI)
    {
        FlightControlSystem_Small flightControlSystem = flightControlAI.ship.flightControlSystem_Small;

        flightControlSystem.turnInput = 0;
        flightControlSystem.pitchInput = 0;
    }

    //This prevents gimbal lock when the ships turn
    public static void AvoidGimbalLock(FlightControlAI_Small flightControlAI, float forward, bool reverse = false)
    {
        FlightControlSystem_Small flightControlSystem = flightControlAI.ship.flightControlSystem_Small;

        if (reverse == false)
        {
            if (forward < -0.9)
            {
                flightControlAI.avoidGimbalLock = true;

                if (Vector3.Dot(flightControlAI.transform.up, Vector3.down) < 0)
                {
                    //Steering when ship is the right way up
                    flightControlSystem.turnInput = 1;
                    flightControlSystem.pitchInput = 0;
                }
                else
                {
                    //Steering when the ship is upside down
                    flightControlSystem.turnInput = -1;
                    flightControlSystem.pitchInput = 0;
                }
            }
            else
            {
                flightControlAI.avoidGimbalLock = false;
            }
        }
        else
        {
            if (forward > 0.9)
            {
                flightControlAI.avoidGimbalLock = true;

                if (Vector3.Dot(flightControlAI.transform.up, Vector3.down) < 0)
                {
                    //Right way up
                    flightControlSystem.turnInput = -1;
                    flightControlSystem.pitchInput = 0;
                    flightControlSystem.rollInput = 0;
                }
                else
                {
                    //upside down
                    flightControlSystem.turnInput = 1;
                    flightControlSystem.pitchInput = 0;
                }
            }
            else
            {
                flightControlAI.avoidGimbalLock = false;
            }
        }
    }

    //This resets all the inputs
    public static void ResetSteeringInputs(FlightControlAI_Small flightControlAI)
    {
        FlightControlSystem_Small flightControlSystem = flightControlAI.ship.flightControlSystem_Small;

        flightControlSystem.pitchInput = 0;
        flightControlSystem.turnInput = 0;
        flightControlSystem.rollInput = 0;       
    }

    #endregion
}