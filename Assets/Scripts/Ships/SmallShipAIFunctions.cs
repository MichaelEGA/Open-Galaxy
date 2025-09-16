using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//These functions are called by the small ship functions script
public static class SmallShipAIFunctions
{
    #region Base AI Functions

    //This is the base function that calls all the other AI functions except collisions which is called by an external script (evasions are called internally though)
    public static void GetAIInput(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            //This adds all the default ai tags when the ship is first run
            if (smallShip.aiStarted == false)
            {
                AddDefaultTags(smallShip);
                smallShip.aiStarted = true;
            }

            //This checks if the ship needs to request a new target
            ClearTarget(smallShip);
            RequestTarget(smallShip);
            
            //This runs all the ai functions
            RunTags(smallShip);
        }
    }

    #endregion

    #region AI Tagging System

    //This adds the default ai tags
    public static void AddDefaultTags(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            AddTag(smallShip, "threequarterspeed");
            AddTag(smallShip, "singlelaser");
            AddTag(smallShip, "lowaccuracy");
            AddTag(smallShip, "chasewithdraw");
            AddTag(smallShip, "resetenergylevels");
            AddTag(smallShip, "targetallprefsmall");
        }
    }

    //This adds an ai tag and removes conflicting tags using the two functions below
    public static void AddTag(SmallShip smallShip, string tag)
    {
        if (smallShip != null)
        {
            if (tag == "matchspeed" || tag == "fullspeedwithboost" || tag == "fullspeed" || tag == "threequarterspeed" || tag == "halfspeed" || tag == "quarterspeed" || tag == "dynamicspeed" || tag == "nospeed")
            {
                RemoveSingleTag(smallShip, "fullspeedwithboost");
                RemoveSingleTag(smallShip, "fullspeed");
                RemoveSingleTag(smallShip, "threequarterspeed");
                RemoveSingleTag(smallShip, "halfspeed");
                RemoveSingleTag(smallShip, "quarterspeed");
                RemoveSingleTag(smallShip, "dynamicspeed");
                RemoveSingleTag(smallShip, "nospeed");
                           
            }
            else if (tag == "singlelaser" || tag == "duallasers" || tag == "alllasers" || tag == "rapidlasers" || tag == "singleplasma" || tag == "dualplasma" || tag == "allplasma" || tag == "singleion" || tag == "dualion" || tag == "allion" || tag == "rapidion" || tag == "singletorpedo" || tag == "dualtorpedos" || tag == "noweapons" || tag == "dynamicweapons_single" || tag == "dynamicweapons_dual" || tag == "dynamicweapons_all" || tag == "dynamicweapons_rapid")
            {
                RemoveSingleTag(smallShip, "singlelaser");
                RemoveSingleTag(smallShip, "duallasers");
                RemoveSingleTag(smallShip, "alllasers");
                RemoveSingleTag(smallShip, "rapidlasers");
                RemoveSingleTag(smallShip, "singleplasma");
                RemoveSingleTag(smallShip, "dualplasma");
                RemoveSingleTag(smallShip, "allplasma");
                RemoveSingleTag(smallShip, "singleion");
                RemoveSingleTag(smallShip, "dualion");
                RemoveSingleTag(smallShip, "allion");
                RemoveSingleTag(smallShip, "rapidion");
                RemoveSingleTag(smallShip, "singletorpedo");
                RemoveSingleTag(smallShip, "dualtorpedos");
                RemoveSingleTag(smallShip, "alltorpedos");
                RemoveSingleTag(smallShip, "dynamicweapons_single");
                RemoveSingleTag(smallShip, "dynamicweapons_dual");
                RemoveSingleTag(smallShip, "dynamicweapons_all");
                RemoveSingleTag(smallShip, "dynamicweapons_rapid");
                RemoveSingleTag(smallShip, "noweapons");
            }
            else if (tag == "lowaccuracy" || tag == "mediumaccuracy" || tag == "highaccuracy")
            {
                RemoveSingleTag(smallShip, "lowaccuracy");
                RemoveSingleTag(smallShip, "mediumaccuracy");
                RemoveSingleTag(smallShip, "highaccuracy");
            }
            else if (tag == "chase" || tag == "chasewithdraw" || tag == "strafewithdraw" || tag == "movetowaypoint" || tag == "patrolrandom" || tag == "norotation" || tag == "formationflying")
            {
                RemoveSingleTag(smallShip, "chase");
                RemoveSingleTag(smallShip, "chasewithdraw");
                RemoveSingleTag(smallShip, "strafewithdraw");
                RemoveSingleTag(smallShip, "movetowaypoint");
                RemoveSingleTag(smallShip, "patrolrandom");
                RemoveSingleTag(smallShip, "formationflying");
                RemoveSingleTag(smallShip, "norotation");
                smallShip.flyInFormation = false; //This deactivates formation flying when flying pattern is changed.
            }
            else if (tag == "resetenergylevels" || tag == "energytoshields" || tag == "energytoengines" || tag == "energytolasers" || tag == "energyprotective" || tag == "energyaggressive" || tag == "energydynamic")
            {
                RemoveSingleTag(smallShip, "resetenergylevels");
                RemoveSingleTag(smallShip, "energytoshields");
                RemoveSingleTag(smallShip, "energytoengines");
                RemoveSingleTag(smallShip, "energytolasers");
                RemoveSingleTag(smallShip, "energyprotective");
                RemoveSingleTag(smallShip, "energyaggressive");
                RemoveSingleTag(smallShip, "energydynamic");
            }
            else if (tag == "targetallprefsmall" || tag == "targetallpreflarge" || tag == "targetsmallshipsonly" || tag == "targetlargeshipsonly")
            {
                RemoveSingleTag(smallShip, "targetallprefsmall");
                RemoveSingleTag(smallShip, "targetallpreflarge");
                RemoveSingleTag(smallShip, "targetsmallshipsonly");
                RemoveSingleTag(smallShip, "targetlargeshipsonly");
                smallShip.requestingTarget = true; //This forces the ship to select a new target on the basis of the selected tag.
            }

            AddSingleTag(smallShip, tag);
        }
    }

    //This adds an ai tag
    public static void AddSingleTag(SmallShip smallShip, string tag)
    {
        if (smallShip != null)
        {
            //This checks the tag list exists
            if (smallShip.aiTags == null)
            {
                smallShip.aiTags = new List<string>();
            }

            //This adds the new tag
            if (smallShip.aiTags != null)
            {
                smallShip.aiTags.Add(tag);
            }
        }
    }

    //This removes an ai tag
    public static void RemoveSingleTag(SmallShip smallShip, string tag)
    {
        if (smallShip != null)
        {
            //This checks the tag list exists
            if (smallShip.aiTags == null)
            {
                smallShip.aiTags = new List<string>();
            }

            //This removes the designated tag
            if (smallShip.aiTags != null)
            {
                //This removes the tag
                for (int i = 0; i < smallShip.aiTags.Count; i++)
                {
                    if (smallShip.aiTags[i] == tag)
                    {
                        smallShip.aiTags.RemoveAt(i);
                        break;
                    }
                }
            }
        }
    }

    //This runs the ai tags
    public static void RunTags(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            //This checks the tag list exists
            if (smallShip.aiTags == null)
            {
                smallShip.aiTags = new List<string>();
            }

            //This runs through the ship tags and runs the appropriate functions
            if (smallShip.aiTags != null)
            {
                foreach (string tag in smallShip.aiTags.ToArray())
                {
                    if (tag == "fullspeedwithboost") //Speed control
                    {
                        FullSpeedWithBoost(smallShip);
                    }
                    else if (tag == "fullspeed") //Speed control
                    {
                        FullSpeed(smallShip);
                    }
                    else if (tag == "threequarterspeed")
                    {
                        ThreeQuarterSpeed(smallShip);
                    }
                    else if (tag == "halfspeed")
                    {
                        HalfSpeed(smallShip);
                    }
                    else if (tag == "quarterspeed")
                    {
                        QuarterSpeed(smallShip);
                    }
                    else if (tag == "dynamicspeed")
                    {
                        DynamicSpeed(smallShip);
                    }
                    else if (tag == "nospeed")
                    {
                        NoSpeed(smallShip);
                    }
                    else if (tag == "singlelaser") //Weapon control
                    {
                        SingleLaser(smallShip);
                    }
                    else if (tag == "duallasers")
                    {
                        DualLasers(smallShip);
                    }
                    else if (tag == "alllasers")
                    {
                        AllLasers(smallShip);
                    }
                    else if (tag == "rapidlasers")
                    {
                        RapidLasers(smallShip);
                    }
                    else if (tag == "singleplasma")
                    {
                        SinglePlasma(smallShip);
                    }
                    else if (tag == "dualplasma")
                    {
                        DualPlasma(smallShip);
                    }
                    else if (tag == "allplasma")
                    {
                        AllPlasma(smallShip);
                    }
                    else if (tag == "singleion") 
                    {
                        SingleIon(smallShip);
                    }
                    else if (tag == "dualion")
                    {
                        DualIon(smallShip);
                    }
                    else if (tag == "allion")
                    {
                        AllIon(smallShip);
                    }
                    else if (tag == "rapidion")
                    {
                        RapidIon(smallShip);
                    }
                    else if (tag == "singletorpedo")
                    {
                        SingleTorpedo(smallShip);
                    }
                    else if (tag == "dualtorpedos")
                    {
                        DualTorpedos(smallShip);
                    }
                    else if (tag == "alltorpedos")
                    {
                        AllTorpedos(smallShip);
                    }
                    else if (tag == "dynamicweapons_single")
                    {
                        DynamicWeapons_Single(smallShip);
                    }
                    else if (tag == "dynamicweapons_dual")
                    {
                        DynamicWeapons_Dual(smallShip);
                    }
                    else if (tag == "dynamicweapons_all")
                    {
                        DynamicWeapons_All(smallShip);
                    }
                    else if (tag == "dynamicweapons_rapid")
                    {
                        DynamicWeapons_Rapid(smallShip);
                    }
                    else if (tag == "noweapons")
                    {
                        //Do nothing
                    }
                    else if (tag == "lowaccuracy") //Weapon accuracy
                    {
                        LowAccuracy(smallShip);
                    }
                    else if (tag == "mediumaccuracy")
                    {
                        MediumAccuracy(smallShip);
                    }
                    else if (tag == "highaccuracy")
                    {
                        HighAccuracy(smallShip);
                    }
                    else if (tag == "chase") //Flight patterns
                    {
                        Chase(smallShip);
                    }
                    else if (tag == "chasewithdraw")
                    {
                        ChaseWithdraw(smallShip);
                    }
                    else if (tag == "strafewithdraw")
                    {
                        StrafeWithdraw(smallShip);
                    }
                    else if (tag == "movetowaypoint")
                    {
                        MoveToWayPoint(smallShip);
                    }
                    else if (tag == "patrolrandom")
                    {
                        PatrolRandom(smallShip);
                    }
                    else if (tag == "formationflying")
                    {
                        FormationFlying(smallShip);
                    }
                    else if (tag == "norotation")
                    {
                        NoRotation(smallShip);
                    }
                    else if (tag == "resetenergylevels") //Energy Management
                    {
                        ResetEnergyLevels(smallShip);
                    }
                    else if (tag == "energytoshields") 
                    {
                        EnergyToShields(smallShip);
                    }
                    else if (tag == "energytoengines")
                    {
                        EnergyToEngines(smallShip);
                    }
                    else if (tag == "energytolasers")
                    {
                        EnergyToLasers(smallShip);
                    }
                    else if (tag == "energyprotective")
                    {
                        EnergyProtective(smallShip);
                    }
                    else if (tag == "energyaggressive")
                    {
                        EnergyAggressive(smallShip);
                    }
                    else if (tag == "energydynamic")
                    {
                        EnergyDynamic(smallShip);
                    }
                    else if (tag == "targetallprefsmall") //Targetting Preference
                    {
                        TargetAllPrefSmall(smallShip);
                    }
                    else if (tag == "targetallpreflarge")
                    {
                        TargetAllPrefLarge(smallShip);
                    }
                    else if (tag == "targetlargeshipsonly")
                    {
                        TargetLargeShipOnly(smallShip);
                    }
                    else if (tag == "targetsmallshipsonly")
                    {
                        TargetSmallShipsOnly(smallShip);
                    }
                }
            }
        }
    }

    //This checks if an ai tag exists
    public static bool TagExists(SmallShip smallShip, string tag)
    {
        bool exists = false;

        if (smallShip != null)
        {
            foreach (string tempTag in smallShip.aiTags.ToArray())
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
    public static void FullSpeedWithBoost(SmallShip smallShip)
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
            else if (smallShip.target != null & smallShip.flyInFormation == false || smallShip.followTarget != null & smallShip.flyInFormation == true)
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
    public static void FullSpeed(SmallShip smallShip)
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
            else if (smallShip.target != null & smallShip.flyInFormation == false || smallShip.followTarget != null & smallShip.flyInFormation == true)
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
    public static void ThreeQuarterSpeed(SmallShip smallShip)
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
            else if (smallShip.target != null & smallShip.flyInFormation == false || smallShip.followTarget != null & smallShip.flyInFormation == true)
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
    public static void HalfSpeed(SmallShip smallShip)
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
            else if (smallShip.target != null & smallShip.flyInFormation == false || smallShip.followTarget != null & smallShip.flyInFormation == true)
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
    public static void QuarterSpeed(SmallShip smallShip)
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
            else if (smallShip.target != null & smallShip.flyInFormation == false || smallShip.followTarget != null & smallShip.flyInFormation == true)
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
    public static void DynamicSpeed(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.target != null)
            {
                if (smallShip.aiMatchSpeed == false)
                {
                    if (smallShip.targetForward < 0.5f)
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
    public static void NoSpeed(SmallShip smallShip)
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
    public static void MatchSpeed(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            float oneThird = (smallShip.speedRating / 3f);

            if (smallShip.thrustSpeed > smallShip.targetSpeed & smallShip.thrustSpeed > oneThird)
            {
                smallShip.thrustInput = -1;
            }
            else
            {
                smallShip.thrustInput = 1;
            }

            //This corrects the input for the follow target if necessary
            if (smallShip.followTarget != null & smallShip.flyInFormation == true)
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
        }        
    }

    #endregion

    #region AI Weapon Control

    //This fires one laser at a time
    public static void SingleLaser(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.target != null)
            {
                smallShip.activeWeapon = "lasers";

                if (smallShip.interceptForward > 0.95f & smallShip.interceptDistance < 2000 & smallShip.target.gameObject.activeSelf == true)
                {
                    bool dontFire = CheckFire(smallShip);

                    if (dontFire == false)
                    {
                        smallShip.weaponMode = "single";
                        LaserFunctions.InitiateFiring(smallShip);
                    }
                }
            }
        }
    }

    //This fires two lasers at a time
    public static void DualLasers(SmallShip smallShip)
    {
        if (smallShip != null)
        {

            if (smallShip.target != null)
            {
                smallShip.activeWeapon = "lasers";

                if (smallShip.interceptForward > 0.95f & smallShip.interceptDistance < 2000 & smallShip.target.gameObject.activeSelf == true)
                {
                    bool dontFire = CheckFire(smallShip);

                    if (dontFire == false)
                    {
                        smallShip.weaponMode = "dual";
                        LaserFunctions.InitiateFiring(smallShip);
                    }
                }
            }
        }
    }

    //This fires all the ships lasers at once
    public static void AllLasers(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.target != null)
            {
                smallShip.activeWeapon = "lasers";

                if (smallShip.interceptForward > 0.95f & smallShip.interceptDistance < 2000 & smallShip.target.gameObject.activeSelf == true)
                {
                    bool dontFire = CheckFire(smallShip);

                    if (dontFire == false)
                    {
                        smallShip.weaponMode = "all";
                        LaserFunctions.InitiateFiring(smallShip);
                    }
                }
            }
        }
    }

    //This fires one laser at a time
    public static void RapidLasers(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.target != null)
            {
                smallShip.activeWeapon = "lasers";

                if (smallShip.interceptForward > 0.95f & smallShip.interceptDistance < 2000 & smallShip.target.gameObject.activeSelf == true)
                {
                    bool dontFire = CheckFire(smallShip);

                    if (dontFire == false & smallShip.hasRapidFire == true)
                    {
                        smallShip.weaponMode = "rapid";
                        LaserFunctions.InitiateFiring(smallShip);
                    }
                    else if (dontFire == false)
                    {
                        smallShip.weaponMode = "single";
                        LaserFunctions.InitiateFiring(smallShip);
                    }
                }
            }
        }
    }

    //This fires one laser at a time
    public static void SinglePlasma(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.target != null)
            {
                smallShip.activeWeapon = "plasma";

                if (smallShip.interceptForward > 0.95f & smallShip.interceptDistance < 2000 & smallShip.target.gameObject.activeSelf == true)
                {
                    bool dontFire = CheckFire(smallShip);

                    if (dontFire == false)
                    {
                        smallShip.weaponMode = "single";
                        PlasmaFunctions.InitiateFiring(smallShip);
                    }
                }
            }
        }
    }

    //This fires two lasers at a time
    public static void DualPlasma(SmallShip smallShip)
    {
        if (smallShip != null)
        {

            if (smallShip.target != null)
            {
                smallShip.activeWeapon = "plasma";

                if (smallShip.interceptForward > 0.95f & smallShip.interceptDistance < 2000 & smallShip.target.gameObject.activeSelf == true)
                {
                    bool dontFire = CheckFire(smallShip);

                    if (dontFire == false)
                    {
                        smallShip.weaponMode = "dual";
                        PlasmaFunctions.InitiateFiring(smallShip);
                    }
                }
            }
        }
    }

    //This fires all the ships lasers at once
    public static void AllPlasma(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.target != null)
            {
                smallShip.activeWeapon = "plasma";

                if (smallShip.interceptForward > 0.95f & smallShip.interceptDistance < 2000 & smallShip.target.gameObject.activeSelf == true)
                {
                    bool dontFire = CheckFire(smallShip);

                    if (dontFire == false)
                    {
                        smallShip.weaponMode = "all";
                        PlasmaFunctions.InitiateFiring(smallShip);
                    }
                }
            }
        }
    }

    //This fires one ion at a time
    public static void SingleIon(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.target != null)
            {
                smallShip.activeWeapon = "ion";

                if (smallShip.interceptForward > 0.95f & smallShip.interceptDistance < 2000 & smallShip.target.gameObject.activeSelf == true)
                {
                    bool dontFire = CheckFire(smallShip);

                    if (dontFire == false)
                    {
                        smallShip.weaponMode = "single";
                        IonFunctions.InitiateFiring(smallShip);
                    }
                }
            }
        }
    }

    //This fires two ions at a time
    public static void DualIon(SmallShip smallShip)
    {
        if (smallShip != null)
        {

            if (smallShip.target != null)
            {
                smallShip.activeWeapon = "ion";

                if (smallShip.interceptForward > 0.95f & smallShip.interceptDistance < 2000 & smallShip.target.gameObject.activeSelf == true)
                {
                    bool dontFire = CheckFire(smallShip);

                    if (dontFire == false)
                    {
                        smallShip.weaponMode = "dual";
                        IonFunctions.InitiateFiring(smallShip);
                    }
                }
            }
        }
    }

    //This fires all the ships ions at once
    public static void AllIon(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.target != null)
            {
                smallShip.activeWeapon = "ion";

                if (smallShip.interceptForward > 0.95f & smallShip.interceptDistance < 2000 & smallShip.target.gameObject.activeSelf == true)
                {
                    bool dontFire = CheckFire(smallShip);

                    if (dontFire == false)
                    {
                        smallShip.weaponMode = "all";
                        IonFunctions.InitiateFiring(smallShip);
                    }
                }
            }
        }
    }

    //This fires one ion at a time
    public static void RapidIon(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.target != null)
            {
                smallShip.activeWeapon = "ion";

                if (smallShip.interceptForward > 0.95f & smallShip.interceptDistance < 2000 & smallShip.target.gameObject.activeSelf == true)
                {
                    bool dontFire = CheckFire(smallShip);

                    if (dontFire == false & smallShip.hasRapidFire == true)
                    {
                        smallShip.weaponMode = "rapid";
                        IonFunctions.InitiateFiring(smallShip);
                    }
                    else if (dontFire == false)
                    {
                        smallShip.weaponMode = "single";
                        IonFunctions.InitiateFiring(smallShip);
                    }
                }
            }
        }
    }

    //This fires one torpedo at a time
    public static void SingleTorpedo(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.target != null)
            {
                smallShip.activeWeapon = "torpedos";

                if (smallShip.torpedoNumber > 0)
                {
                    if (smallShip.targetForward > 0.995f & smallShip.torpedoLockedOn == true)
                    {
                        smallShip.weaponMode = "single";
                        TorpedoFunctions.FireTorpedo(smallShip);
                    }
                }
                else
                {
                    smallShip.activeWeapon = "lasers";
                    SingleLaser(smallShip);
                }
            }
        }
    }

    //This fires two  torpedo at once
    public static void DualTorpedos(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.target != null)
            {
                smallShip.activeWeapon = "torpedos";

                if (smallShip.torpedoNumber > 0)
                {
                    if (smallShip.targetForward > 0.995f & smallShip.torpedoLockedOn == true)
                    {
                        smallShip.weaponMode = "dual";
                        TorpedoFunctions.FireTorpedo(smallShip);
                    }
                }
                else
                {
                    smallShip.activeWeapon = "lasers";
                    SingleLaser(smallShip);
                }
            }
        }
    }

    //This fires from all torpedo tubes
    public static void AllTorpedos(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.target != null)
            {
                smallShip.activeWeapon = "torpedos";

                if (smallShip.torpedoNumber > 0)
                {
                    if (smallShip.targetForward > 0.995f & smallShip.torpedoLockedOn == true)
                    {
                        smallShip.weaponMode = "all";
                        TorpedoFunctions.FireTorpedo(smallShip);
                    }
                }
                else
                {
                    smallShip.activeWeapon = "lasers";
                    SingleLaser(smallShip);
                }
            }
        }
    }

    //This switches between single lasers and torpedos
    public static void DynamicWeapons_Single(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.target != null)
            {
                if (smallShip.torpedoNumber > 0 & smallShip.interceptDistance > 2000 & smallShip.target.gameObject.activeSelf == true)
                {
                    smallShip.activeWeapon = "torpedos";

                    if (smallShip.targetForward > 0.995f & smallShip.torpedoLockedOn == true)
                    {
                        smallShip.weaponMode = "single";
                        TorpedoFunctions.FireTorpedo(smallShip);
                    }
                }
                else if (smallShip.interceptForward > 0.95f & smallShip.interceptDistance < 2000 & smallShip.target.gameObject.activeSelf == true)
                {
                    if (smallShip.hasPlasma == false)
                    {
                        smallShip.activeWeapon = "lasers";
                    }
                    else
                    {
                        smallShip.activeWeapon = "plasma";
                    }

                    bool dontFire = CheckFire(smallShip);

                    if (dontFire == false)
                    {
                        smallShip.weaponMode = "single";
                        LaserFunctions.InitiateFiring(smallShip);
                    }
                }
            }
        }
    }

    //This switches betwene dual lasers and torpedos
    public static void DynamicWeapons_Dual(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.target != null)
            {
                if (smallShip.torpedoNumber > 0 & smallShip.interceptDistance > 2000 & smallShip.target.gameObject.activeSelf == true)
                {
                    smallShip.activeWeapon = "torpedos";

                    if (smallShip.targetForward > 0.995f & smallShip.torpedoLockedOn == true)
                    {
                        smallShip.weaponMode = "dual";
                        TorpedoFunctions.FireTorpedo(smallShip);
                    }
                }
                else if (smallShip.interceptForward > 0.95f & smallShip.interceptDistance < 2000 & smallShip.target.gameObject.activeSelf == true)
                {
                    if (smallShip.hasPlasma == false)
                    {
                        smallShip.activeWeapon = "lasers";
                    }
                    else
                    {
                        smallShip.activeWeapon = "plasma";
                    }

                    bool dontFire = CheckFire(smallShip);

                    if (dontFire == false)
                    {
                        smallShip.weaponMode = "dual";
                        LaserFunctions.InitiateFiring(smallShip);
                    }
                }
            }
        }
    }

    //This switches betwene dual lasers and torpedos
    public static void DynamicWeapons_All(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.target != null)
            {
                if (smallShip.torpedoNumber > 0 & smallShip.interceptDistance > 2000 & smallShip.target.gameObject.activeSelf == true)
                {
                    smallShip.activeWeapon = "torpedos";

                    if (smallShip.targetForward > 0.995f & smallShip.torpedoLockedOn == true)
                    {
                        smallShip.weaponMode = "all";
                        TorpedoFunctions.FireTorpedo(smallShip);
                    }
                }
                else if (smallShip.interceptForward > 0.95f & smallShip.interceptDistance < 2000 & smallShip.target.gameObject.activeSelf == true)
                {
                    if (smallShip.hasPlasma == false)
                    {
                        smallShip.activeWeapon = "lasers";
                    }
                    else
                    {
                        smallShip.activeWeapon = "plasma";
                    }

                    bool dontFire = CheckFire(smallShip);

                    if (dontFire == false)
                    {
                        smallShip.weaponMode = "all";
                        LaserFunctions.InitiateFiring(smallShip);
                    }
                }
            }
        }
    }

    //This switches between rapid lasers and normal lasers
    public static void DynamicWeapons_Rapid(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.hasPlasma == false)
            {
                smallShip.activeWeapon = "lasers";
            }
            else
            {
                smallShip.activeWeapon = "plasma";
            }

            if (smallShip.target != null)
            {
                if (smallShip.interceptForward > 0.95f & smallShip.target.gameObject.activeSelf == true)
                {
                    if (smallShip.hasRapidFire == true)
                    {
                        if (smallShip.targetSmallShip != null)
                        {
                            if (smallShip.targetSmallShip.hasPlasma == true & smallShip.targetSmallShip.shieldLevel > 10)
                            {
                                smallShip.weaponMode = "rapid";
                                LaserFunctions.InitiateFiring(smallShip);
                            }
                            else
                            {
                                smallShip.weaponMode = "single";
                                LaserFunctions.InitiateFiring(smallShip);
                            }
                        }
                    }
                    else
                    {
                        smallShip.weaponMode = "single";
                        LaserFunctions.InitiateFiring(smallShip);
                    }
                }
            }
        }
    }
    //This checks whether a non hostile ship is in the firing line or not
    public static bool CheckFire(SmallShip smallShip)
    {
        bool dontFire = false;

        if (smallShip != null)
        {
            RaycastHit hit;

            int layerMask = (1 << 6) | (1 << 7) | (1 << 8) | (1 << 9) | (1 << 10) | (1 << 11) | (1 << 12) | (1 << 13) | (1 << 14) | (1 << 15) | (1 << 16) | (1 << 17) | (1 << 18) | (1 << 19) | (1 << 20) | (1 << 21) | (1 << 22) | (1 << 23);

            Vector3 forwardRaycast = smallShip.gameObject.transform.position + (smallShip.gameObject.transform.forward * 10);

            if (Physics.SphereCast(forwardRaycast, 10, smallShip.gameObject.gameObject.transform.TransformDirection(Vector3.forward), out hit, 1000, layerMask))
            {
                SmallShip otherSmallship = hit.collider.GetComponentInParent<SmallShip>();

                if (otherSmallship != null)
                {
                    bool isHostile = TargetingFunctions.GetHostility_SmallShipPlayer(smallShip, otherSmallship.allegiance);

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
    public static void LowAccuracy(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            SetTargetingErrorMargin(smallShip, "low");
        }
    }

    //This sets the targetting accuracy to medium
    public static void MediumAccuracy(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            SetTargetingErrorMargin(smallShip, "medium");
        }
    }

    //This sets the targetting accuracy to jihj
    public static void HighAccuracy(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            SetTargetingErrorMargin(smallShip, "high");
        }
    }

    //Targeting error margin
    public static void SetTargetingErrorMargin(SmallShip smallShip, string mode)
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
    public static void Chase(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.aiEvade == false)
            {
                if (smallShip.target != null)
                {
                    if (smallShip.targetDistance > 250)
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
                    PatrolRandom(smallShip);
                }
            }

            smallShip.flyInFormation = false;
        }
    }

    //Chase-Withdraw: Attack for 15-20 followed by withdrawal for 30-35 seconds
    public static void ChaseWithdraw(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.aiEvade == false)
            {
                if (smallShip.target != null)
                {
                    if (smallShip.targetDistance > 250)
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
                    PatrolRandom(smallShip);
                }
            }

            smallShip.flyInFormation = false;
        }
    }

    //Strafe-Withdraw: Attacks enemy ship at distance before withdrawing again (typically used for attack large ships)
    public static void StrafeWithdraw(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.aiEvade == false)
            {
                if (smallShip != null)
                {
                    float attackDistance = 250;
                    float withdrawDistance = 1000;

                    if (smallShip.targetLargeShip != null)
                    {
                        if (smallShip.targetLargeShip.shipClass == "large")
                        {
                            attackDistance = 1500;
                            withdrawDistance = 3000;
                        }
                        else if (smallShip.targetLargeShip.shipClass == "middle")
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

                    if (smallShip.targetDistance > attackDistance & smallShip.withdraw == false)
                    {
                        AngleTowardsTarget(smallShip);
                    }
                    else if (smallShip.targetDistance < attackDistance & smallShip.withdraw == false)
                    {
                        smallShip.withdraw = true;
                    }
                    else if (smallShip.targetDistance > withdrawDistance & smallShip.withdraw == true)
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
                    PatrolRandom(smallShip);
                }
            }

            smallShip.flyInFormation = false;
        }
    }

    //This angles towards the ships waypoint
    public static void MoveToWayPoint(SmallShip smallShip)
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
    public static void PatrolRandom(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.aiEvade == false)
            {
                if (smallShip.waypoint != null)
                {
                    float distanceToWaypoint = Vector3.Distance(smallShip.gameObject.transform.position, smallShip.waypoint.transform.position);

                    if (distanceToWaypoint < 50)
                    {
                        SelectRandomWaypoint(smallShip);
                    }

                    AngleTowardsWaypoint(smallShip);
                }
            }

            smallShip.flyInFormation = false;
        }
    }

    //This selects a random waypoint
    public static void SelectRandomWaypoint(SmallShip smallShip)
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

                if (smallShip.waypoint != null)
                {
                    smallShip.waypoint.transform.position = new Vector3(x, y, z);
                }
            }
        }
    }

    //This sets the ship to fly in formation
    public static void FormationFlying(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.aiEvade == false)
            {
                if (smallShip.followTarget != null)
                {
                    if (smallShip.isAI == true)
                    {
                        smallShip.flyInFormation = true;
                    }

                    Transform target = smallShip.followTarget.transform;

                    Vector3 adjustedPosition = target.position + new Vector3(smallShip.xFormationPos, smallShip.yFormationPos, smallShip.zFormationPos);

                    float distance = Vector3.Distance(smallShip.transform.position, adjustedPosition);

                    smallShip.aiMatchSpeed = true;
                    AngleTowardsPoint(smallShip, adjustedPosition);
                }
                else
                {
                    smallShip.aiMatchSpeed = false;
                    smallShip.flyInFormation = false;
                    ChaseWithdraw(smallShip);
                }
            }
        }
    }

    //This prevents the ship from performing any rotations and locks the direction forward
    public static void NoRotation(SmallShip smallShip)
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
    public static void EnergyToShields(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            smallShip.powerMode = "shields";
        }
    }

    //This sets engine power to maximum
    public static void EnergyToEngines(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            smallShip.powerMode = "engines";
        }
    }

    //This sets engine power to maximum
    public static void EnergyToLasers(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            smallShip.powerMode = "lasers";
        }
    }

    //This resets all energy levels
    public static void ResetEnergyLevels(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            smallShip.powerMode = "reset";
        }
    }

    //This uses the ships energy in a way that maxmises aggression but leaves lowers the defensive capabilites of the ship
    public static void EnergyAggressive(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.target != null)
            {
                if (smallShip.targetDistance < 1000 & smallShip.targetForward > 0)
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
    public static void EnergyProtective(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.target != null)
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
    public static void EnergyDynamic(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.target != null)
            {
                if (smallShip.shieldLevel < smallShip.shieldRating / 2f)
                {
                    smallShip.powerMode = "shields";
                }
                else if (smallShip.targetDistance < 1000 & smallShip.targetForward > 0)
                {
                    smallShip.powerMode = "lasers";
                }
                else if (smallShip.targetDistance > 2000 & smallShip.targetForward > 0)
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
    public static void RequestTarget(SmallShip smallShip)
    {
        if (smallShip.target == null)
        {
            smallShip.requestingTarget = true;
        }
        else
        {
            smallShip.requestingTarget = false;
        }
    }

    //This clears the target if it doesn't meet certain conditions i.e. was destroyed or disabled  - this function is run automatically
    public static void ClearTarget(SmallShip smallShip)
    {
        if (smallShip.target != null)
        {
            if (smallShip.target.activeSelf == false)
            {
                smallShip.target = null;
            }
            else if (smallShip.targetSmallShip != null)
            {
                if (smallShip.targetSmallShip.isDisabled == true)
                {
                    smallShip.target = null;
                }
            }
            else if (smallShip.targetLargeShip != null)
            {
                if (smallShip.targetLargeShip.isDisabled == true)
                {
                    smallShip.target = null;
                }
            }
        }
    }

    //Tagets all ships but looks for small ships first
    public static void TargetAllPrefSmall(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.aiTargetingMode != "targetallprefsmall") //This ensures the previous target is cleared
            {
                smallShip.target = null;
                smallShip.requestingTarget = true;
            }

            smallShip.aiTargetingMode = "targetallprefsmall";
        }
    }

    //Targets all ships but looks for large ships first
    public static void TargetAllPrefLarge(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.aiTargetingMode != "targetallpreflarge") //This ensures the previous target is cleared
            {
                smallShip.target = null;
                smallShip.requestingTarget = true;
            }

            smallShip.aiTargetingMode = "targetallpreflarge";
        }
    }

    //Targets only small ships
    public static void TargetSmallShipsOnly(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.aiTargetingMode != "targetsmallshipsonly") //This ensures the previous target is cleared
            {
                smallShip.target = null;
                smallShip.requestingTarget = true;
            }

            smallShip.aiTargetingMode = "targetsmallshipsonly";
        }
    }

    //Targets only small ships
    public static void TargetLargeShipOnly(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.aiTargetingMode != "targetlargeshipsonly") //This ensures the previous target is cleared
            {
                smallShip.target = null;
                smallShip.requestingTarget = true;
            }

            smallShip.aiTargetingMode = "targetlargeshipsonly";
        }
    }

    #endregion

    #region AI Evasion and Collision Avoidance

    //This allows the ship to evade attacks and avoid collisions with other objects and ships
    public static IEnumerator Evade(SmallShip smallShip, float time, string mode, int direction)
    {
        if (smallShip != null)
        {
            if (TagExists(smallShip, "nospeed") == true & TagExists(smallShip, "norotation") == true)
            {
                //Do nothing
            }
            else
            {
                if (smallShip != null)
                {
                    smallShip.aiEvade = true;

                    time = time + Time.time;

                    while (time > Time.time)
                    {
                        HalfSpeed(smallShip);

                        if (direction == 0)
                        {
                            TurnRight(smallShip);
                        }
                        else if (direction == 1)
                        {
                            TurnLeft(smallShip);
                        }
                        else if (direction == 2)
                        {
                            PitchUp(smallShip);
                        }
                        else if (direction == 3)
                        {
                            PitchDown(smallShip);
                        }
                        else if (direction == 4)
                        {
                            RollRight(smallShip);
                        }
                        else if (direction == 5)
                        {
                            RollLeft(smallShip);
                        }
                        else if (direction == 6)
                        {
                            FlyFoward(smallShip);
                        }

                        yield return null;

                    }

                    ResetSteeringInputs(smallShip);

                    smallShip.aiEvade = false;
                }
            }
        }
    }

    #endregion

    #region AI Steering Control

    //This angles the ship towards the target vector
    public static void AngleTowardsTarget(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            AvoidGimbalLock(smallShip, smallShip.interceptForward);

            if (smallShip.target != null & smallShip.avoidGimbalLock == false)
            {
                if (smallShip.interceptForward < 0.8)
                {
                    if (Vector3.Dot(smallShip.transform.up, Vector3.down) < 0)
                    {
                        //Right way up
                        SmallShipFunctions.SmoothTurnInput(smallShip, smallShip.interceptRight);
                        SmallShipFunctions.SmoothPitchInput(smallShip, -smallShip.interceptUp);
                    }
                    else
                    {
                        //Upside down
                        SmallShipFunctions.SmoothTurnInput(smallShip, -smallShip.interceptRight);
                        SmallShipFunctions.SmoothPitchInput(smallShip, -smallShip.interceptUp);
                    }
                }
                else
                {
                    if (Vector3.Dot(smallShip.transform.up, Vector3.down) < 0)
                    {
                        //Right way up
                        SmallShipFunctions.SmoothTurnInput(smallShip, smallShip.interceptRight * 5);
                        SmallShipFunctions.SmoothPitchInput(smallShip, -smallShip.interceptUp * 5);
                    }
                    else
                    {
                        //Upside down
                        SmallShipFunctions.SmoothTurnInput(smallShip, -smallShip.interceptRight * 5);
                        SmallShipFunctions.SmoothPitchInput(smallShip, -smallShip.interceptUp * 5);
                    }
                }
            }
        }
    }

    //This angles the ship away from the target vector
    public static void AngleAwayFromTarget(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            AvoidGimbalLock(smallShip, smallShip.interceptForward, true);

            if (smallShip.target != null & smallShip.avoidGimbalLock == false)
            {
                if (-smallShip.interceptForward < 1)
                {
                    if (Vector3.Dot(smallShip.transform.up, Vector3.down) < 0)
                    {
                        //Right way up
                        SmallShipFunctions.SmoothTurnInput(smallShip, -smallShip.interceptRight);
                        SmallShipFunctions.SmoothPitchInput(smallShip, smallShip.interceptUp);
                    }
                    else
                    {
                        //Upside down
                        SmallShipFunctions.SmoothTurnInput(smallShip, smallShip.interceptRight);
                        SmallShipFunctions.SmoothPitchInput(smallShip, -smallShip.interceptUp);
                    }
                }
            }
        }
    }

    //This angles the ship towards the target vector
    public static void AngleTowardsWaypoint(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            AvoidGimbalLock(smallShip, smallShip.waypointForward);

            if (smallShip.waypoint != null & smallShip.avoidGimbalLock == false)
            {
                if (smallShip.waypointForward < 0.8)
                {
                    if (Vector3.Dot(smallShip.transform.up, Vector3.down) < 0)
                    {
                        //Right way up
                        SmallShipFunctions.SmoothTurnInput(smallShip, smallShip.waypointRight);
                        SmallShipFunctions.SmoothPitchInput(smallShip, -smallShip.waypointUp);
                    }
                    else
                    {
                        //Upside down
                        SmallShipFunctions.SmoothTurnInput(smallShip, -smallShip.waypointRight);
                        SmallShipFunctions.SmoothPitchInput(smallShip, -smallShip.waypointUp);
                    }
                }
                else
                {
                    if (Vector3.Dot(smallShip.transform.up, Vector3.down) < 0)
                    {
                        //Right way up
                        SmallShipFunctions.SmoothTurnInput(smallShip, smallShip.waypointRight * 5);
                        SmallShipFunctions.SmoothPitchInput(smallShip, -smallShip.waypointUp * 5);
                    }
                    else
                    {
                        //Upside down
                        SmallShipFunctions.SmoothTurnInput(smallShip, -smallShip.waypointRight * 5);
                        SmallShipFunctions.SmoothPitchInput(smallShip, -smallShip.waypointUp * 5);
                    }
                }
            }
        }
    }

    //This angles the ship away from the target vector
    public static void AngleAwayFromWaypoint(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            AvoidGimbalLock(smallShip, smallShip.waypointForward, true);

            if (smallShip.waypoint != null & smallShip.avoidGimbalLock == false)
            {
                if (smallShip.waypointForward > -0.95)
                {
                    if (Vector3.Dot(smallShip.transform.up, Vector3.down) < 0)
                    {
                        //Right way up
                        SmallShipFunctions.SmoothTurnInput(smallShip, -smallShip.waypointRight);
                        SmallShipFunctions.SmoothPitchInput(smallShip, smallShip.waypointUp);
                    }
                    else
                    {
                        //Upside down
                        SmallShipFunctions.SmoothTurnInput(smallShip, smallShip.waypointRight);
                        SmallShipFunctions.SmoothPitchInput(smallShip, -smallShip.waypointUp);
                    }
                }
            }
        }
    }

    //This angles the ship towards the target vector
    public static void AngleTowardsPoint(SmallShip smallShip, Vector3 point)
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
                        //Right way up
                        SmallShipFunctions.SmoothTurnInput(smallShip, targetRight);
                        SmallShipFunctions.SmoothPitchInput(smallShip, -targetUp);
                    }
                    else
                    {
                        //Upside down
                        SmallShipFunctions.SmoothTurnInput(smallShip, -targetRight);
                        SmallShipFunctions.SmoothPitchInput(smallShip, -targetUp);
                    }
                }
                else
                {
                    if (Vector3.Dot(smallShip.transform.up, Vector3.down) < 0)
                    {
                        //Right way up
                        SmallShipFunctions.SmoothTurnInput(smallShip, targetRight * 5);
                        SmallShipFunctions.SmoothPitchInput(smallShip, -targetUp * 5);
                    }
                    else
                    {
                        //Upside down
                        SmallShipFunctions.SmoothTurnInput(smallShip, -targetRight * 5);
                        SmallShipFunctions.SmoothPitchInput(smallShip, -targetUp * 5);
                    }
                }
            }
        }
    }

    //This pitches the ship up
    public static void PitchUp(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            SmallShipFunctions.SmoothPitchInput(smallShip, 1);
        }
    }

    //This pitches the ship down
    public static void PitchDown(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            SmallShipFunctions.SmoothPitchInput(smallShip, -1);
        }
    }

    //This turns the ship right
    public static void TurnRight(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            SmallShipFunctions.SmoothTurnInput(smallShip, 1);
        }
    }

    //This turns the ship Left
    public static void TurnLeft(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            SmallShipFunctions.SmoothTurnInput(smallShip, -1);
        }
    }

    //This causes the ship to roll Right
    public static void RollRight(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            SmallShipFunctions.SmoothRollInput(smallShip, 1);
        }
    }

    //This causes the ship to roll left
    public static void RollLeft(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            SmallShipFunctions.SmoothRollInput(smallShip, -1);
        }
    }

    //This causes the ship to fly forward
    public static void FlyFoward(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            SmallShipFunctions.SmoothPitchInput(smallShip, 0);
            SmallShipFunctions.SmoothTurnInput(smallShip, 0);
        }
    }

    //This prevents gimbal lock when the ships turn
    public static void AvoidGimbalLock(SmallShip smallShip, float forward, bool reverse = false)
    {

        if (reverse == false)
        {
            if (forward < -0.9)
            {
                smallShip.avoidGimbalLock = true;

                if (Vector3.Dot(smallShip.transform.up, Vector3.down) < 0)
                {
                    //Steering when ship is the right way up
                    SmallShipFunctions.SmoothTurnInput(smallShip, 1);
                    SmallShipFunctions.SmoothPitchInput(smallShip, -0);
                }
                else
                {
                    //Steering when the ship is upside down
                    SmallShipFunctions.SmoothTurnInput(smallShip, -1);
                    SmallShipFunctions.SmoothPitchInput(smallShip, -0);
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
                    SmallShipFunctions.SmoothTurnInput(smallShip, -1);
                    SmallShipFunctions.SmoothPitchInput(smallShip, 0);
                }
                else
                {
                    //upside down
                    SmallShipFunctions.SmoothTurnInput(smallShip, 1);
                    SmallShipFunctions.SmoothPitchInput(smallShip, -0);
                }
            }
            else
            {
                smallShip.avoidGimbalLock = false;
            }
        }


    }

    //This resets all the inputs
    public static void ResetSteeringInputs(SmallShip smallShip)
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
