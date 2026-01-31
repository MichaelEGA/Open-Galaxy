using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public static class LargeShipAIFunctions
{
    #region Base AI Functions

    //This is the base function that calls all the other AI functions except collisions which is called by an external script (evasions are called internally though)
    public static void GetAIInput(LargeShip largeShip)
    {
        //This adds all the default ai tags when the ship is first run
        if (largeShip.aiStarted == false)
        {
            AddDefaultTags(largeShip);
            largeShip.aiStarted = true;
        }

        //This clears old targets and requests new targets
        ClearTarget(largeShip);
        RequestTarget(largeShip);

        //This runs all the ai functions
        RunTags(largeShip);
    }

    #endregion

    #region AI Tagging System

    //This adds the default ai tags
    public static void AddDefaultTags(LargeShip largeShip)
    {
        AddTag(largeShip, "nospeed");
        AddTag(largeShip, "norotation");
        AddTag(largeShip, "large_singletarget_largeship");
        AddTag(largeShip, "small_multipletargets_smallship");
        AddTag(largeShip, "large_low");
        AddTag(largeShip, "small_low");
    }

    //This adds an ai tag and removes conflicting tags using the two functions below
    public static void AddTag(LargeShip largeShip, string tag)
    {
        if (largeShip != null)
        {
            if (tag == "matchspeed" || tag == "fullspeed" || tag == "threequarterspeed" || tag == "halfspeed" || tag == "quarterspeed" || tag == "dynamicspeed" || tag == "nospeed")
            {
                RemoveSingleTag(largeShip, "fullspeed");
                RemoveSingleTag(largeShip, "threequarterspeed");
                RemoveSingleTag(largeShip, "halfspeed");
                RemoveSingleTag(largeShip, "quarterspeed");
                RemoveSingleTag(largeShip, "dynamicspeed");
                RemoveSingleTag(largeShip, "nospeed");
            }
            else if (tag == "circletarget" || tag == "movetowaypoint" || tag == "stayinrangeoftarget" || tag == "norotation")
            {
                RemoveSingleTag(largeShip, "movetowaypoint");
                RemoveSingleTag(largeShip, "stayinrangeoftarget");
                RemoveSingleTag(largeShip, "circletarget");
                RemoveSingleTag(largeShip, "norotation");
            }
            else if (tag == "small_singletarget_largeship" || tag == "small_singletarget_smallship" || tag == "small_singletarget_all" || tag == "small_multipletargets_largeship" || tag == "small_multipletargets_smallship" || tag == "small_multipletargets_all" || tag == "small_shiptarget" || tag == "small_noweapons")
            {
                RemoveSingleTag(largeShip, "small_singletarget_largeship");
                RemoveSingleTag(largeShip, "small_singletarget_smallship");
                RemoveSingleTag(largeShip, "small_singletarget_all");
                RemoveSingleTag(largeShip, "small_multipletargets_largeship");
                RemoveSingleTag(largeShip, "small_multipletargets_smallship");
                RemoveSingleTag(largeShip, "small_multipletargets_all");
                RemoveSingleTag(largeShip, "small_shiptarget");
                RemoveSingleTag(largeShip, "small_noweapons");
            }
            else if (tag == "large_singletarget_largeship" || tag == "large_singletarget_smallship" || tag == "large_singletarget_all" || tag == "large_multipletargets_largeship" || tag == "large_multipletargets_smallship" || tag == "large_multipletargets_all" || tag == "large_shiptarget" || tag == "large_noweapons")
            {
                RemoveSingleTag(largeShip, "large_singletarget_largeship");
                RemoveSingleTag(largeShip, "large_singletarget_smallship");
                RemoveSingleTag(largeShip, "large_singletarget_all");
                RemoveSingleTag(largeShip, "large_multipletargets_largeship");
                RemoveSingleTag(largeShip, "large_multipletargets_smallship");
                RemoveSingleTag(largeShip, "large_multipletargets_all");
                RemoveSingleTag(largeShip, "large_shiptarget");
                RemoveSingleTag(largeShip, "large_noweapons");
            }
            else if (tag == "small_low" || tag == "small_medium" || tag == "small_high" || tag == "small_veryhigh")
            {
                RemoveSingleTag(largeShip, "small_low");
                RemoveSingleTag(largeShip, "small_medium");
                RemoveSingleTag(largeShip, "small_high");
                RemoveSingleTag(largeShip, "small_veryhigh");
            }
            else if (tag == "large_low" || tag == "large_medium" || tag == "large_high" || tag == "large_veryhigh")
            {
                RemoveSingleTag(largeShip, "large_low");
                RemoveSingleTag(largeShip, "large_medium");
                RemoveSingleTag(largeShip, "large_high");
                RemoveSingleTag(largeShip, "large_veryhigh");
            }

            AddSingleTag(largeShip, tag);
        }
    }

    //This adds an ai tag
    public static void AddSingleTag(LargeShip largeShip, string tag)
    {
        if (largeShip != null)
        {
            //This checks the tag list exists
            if (largeShip.aiTags == null)
            {
                largeShip.aiTags = new List<string>();
            }

            //This adds the new tag
            if (largeShip.aiTags != null)
            {
                largeShip.aiTags.Add(tag);
            }
        }
    }

    //This removes an ai tag
    public static void RemoveSingleTag(LargeShip largeShip, string tag)
    {
        if (largeShip != null)
        {
            //This checks the tag list exists
            if (largeShip.aiTags == null)
            {
                largeShip.aiTags = new List<string>();
            }

            //This removes the designated tag
            if (largeShip.aiTags != null)
            {
                //This removes the tag
                for (int i = 0; i < largeShip.aiTags.Count; i++)
                {
                    if (largeShip.aiTags[i] == tag)
                    {
                        largeShip.aiTags.RemoveAt(i);
                        break;
                    }
                }
            }
        }
    }

    //This runs the ai tags
    public static void RunTags(LargeShip largeShip)
    {
        if (largeShip != null)
        {
            //This checks the tag list exists
            if (largeShip.aiTags == null)
            {
                largeShip.aiTags = new List<string>();
            }

            //This runs through the ship tags and runs the appropriate functions
            if (largeShip.aiTags != null)
            {
                foreach (string tag in largeShip.aiTags.ToArray())
                {
                    if (tag == "fullspeed") //Speed control
                    {
                        FullSpeed(largeShip);
                    }
                    else if (tag == "threequarterspeed")
                    {
                        ThreeQuarterSpeed(largeShip);
                    }
                    else if (tag == "halfspeed")
                    {
                        HalfSpeed(largeShip);
                    }
                    else if (tag == "quarterspeed")
                    {
                        QuarterSpeed(largeShip);
                    }
                    else if (tag == "dynamicspeed")
                    {
                        DynamicSpeed(largeShip);
                    }
                    else if (tag == "nospeed")
                    {
                        NoSpeed(largeShip);
                    }
                    else if (tag == "movetotargetrange") //Flight patterns
                    {
                        MoveToTargetRange(largeShip);
                    }
                    else if (tag == "circletarget")
                    {
                        CircleTarget(largeShip);
                    }
                    else if (tag == "movetowaypoint")
                    {
                        MoveToWayPoint(largeShip);
                    }
                    else if (tag == "norotation")
                    {
                        NoRotation(largeShip);
                    }
                    else if (tag == "large_singletarget_largeship") //Turret control
                    {
                        TurretTargetModeSelection(largeShip, tag);
                    }
                    else if (tag == "large_singletarget_smallship") 
                    {
                        TurretTargetModeSelection(largeShip, tag);
                    }
                    else if (tag == "large_singletarget_all") 
                    {
                        TurretTargetModeSelection(largeShip, tag);
                    }
                    else if (tag == "large_multipletargets_largeship") 
                    {
                        TurretTargetModeSelection(largeShip, tag);
                    }
                    else if (tag == "large_multipletargets_smallship") 
                    {
                        TurretTargetModeSelection(largeShip, tag);
                    }
                    else if (tag == "large_multipletargets_all") 
                    {
                        TurretTargetModeSelection(largeShip, tag);
                    }
                    else if (tag == "large_shiptarget") 
                    {
                        TurretTargetModeSelection(largeShip, tag);
                    }
                    else if (tag == "large_noweapons")
                    {
                        TurretTargetModeSelection(largeShip, tag);
                    }
                    else if (tag == "small_singletarget_largeship") 
                    {
                        TurretTargetModeSelection(largeShip, tag);
                    }
                    else if (tag == "small_singletarget_smallship") 
                    {
                        TurretTargetModeSelection(largeShip, tag);
                    }
                    else if (tag == "small_singletarget_all") 
                    {
                        TurretTargetModeSelection(largeShip, tag);
                    }
                    else if (tag == "small_multipletargets_largeship") 
                    {
                        TurretTargetModeSelection(largeShip, tag);
                    }
                    else if (tag == "small_multipletargets_smallship") 
                    {
                        TurretTargetModeSelection(largeShip, tag);
                    }
                    else if (tag == "small_multipletargets_all") 
                    {
                        TurretTargetModeSelection(largeShip, tag);
                    }
                    else if (tag == "small_shiptarget") 
                    {
                        TurretTargetModeSelection(largeShip, tag);
                    }
                    else if (tag == "small_noweapons")
                    {
                        TurretTargetModeSelection(largeShip, tag);
                    }
                    else if (tag == "small_low") //Turret accuracy
                    {
                        TurretAccuracySelection(largeShip, tag);
                    }
                    else if (tag == "small_medium")
                    {
                        TurretAccuracySelection(largeShip, tag);
                    }
                    else if (tag == "small_high")
                    {
                        TurretAccuracySelection(largeShip, tag);
                    }
                    else if (tag == "small_veryhigh")
                    {
                        TurretAccuracySelection(largeShip, tag);
                    }
                    else if (tag == "large_low")
                    {
                        TurretAccuracySelection(largeShip, tag);
                    }
                    else if (tag == "large_medium")
                    {
                        TurretAccuracySelection(largeShip, tag);
                    }
                    else if (tag == "large_high")
                    {
                        TurretAccuracySelection(largeShip, tag);
                    }
                    else if (tag == "large_veryhigh")
                    {
                        TurretAccuracySelection(largeShip, tag);
                    }
                }
            }
        }
    }

    //This checks if an ai tag exists
    public static bool TagExists(LargeShip largeShip, string tag)
    {
        bool exists = false;

        foreach (string tempTag in largeShip.aiTags.ToArray())
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
  
    //This sets the ship to half speed (typically used when no enemies are detected)
    public static void FullSpeed(LargeShip largeShip)
    {
        largeShip.thrustInput = 1;
    }

    //This sets the ship to half speed (typically used when no enemies are detected)
    public static void ThreeQuarterSpeed(LargeShip largeShip)
    {
        float threeQuarterSpeed = (largeShip.speedRating / 4f) * 3;

        if (largeShip.thrustSpeed > threeQuarterSpeed)
        {
            largeShip.thrustInput = -1;
        }
        else
        {
            largeShip.thrustInput = 1;
        }
    }

    //This sets the ship to half speed (typically used when no enemies are detected)
    public static void HalfSpeed(LargeShip largeShip)
    {
        float halfSpeed = (largeShip.speedRating / 2f);

        if (largeShip.thrustSpeed > halfSpeed)
        {
            largeShip.thrustInput = -1;
        }
        else
        {
            largeShip.thrustInput = 1;
        }
    }

    //This sets the ship to half speed (typically used when no enemies are detected)
    public static void QuarterSpeed(LargeShip largeShip)
    {
        float quarterSpeed = (largeShip.speedRating / 4f);

        if (largeShip.thrustSpeed > quarterSpeed)
        {
            largeShip.thrustInput = -1;
        }
        else
        {
            largeShip.thrustInput = 1;
        }
    }

    //This changes the speed of the ship dynamically to allow for a fast speed and sharp turns
    public static void DynamicSpeed(LargeShip largeShip)
    {
        if (largeShip != null)
        {
            if (TagExists(largeShip, "movetowaypoint"))
            {
                if (largeShip.waypointForward < 0.95f)
                {
                    if (largeShip.thrustSpeed > 0)
                    {
                        largeShip.thrustInput = -1;
                    }
                }
                else
                {
                    if (largeShip.thrustSpeed < largeShip.speedRating)
                    {
                        largeShip.thrustInput = 1;
                    }
                    else
                    {
                        largeShip.thrustInput = -1;
                    }
                }
            }
            else if (largeShip.target != null)
            {
                if (largeShip.targetDistance > 1000)
                {
                    if (largeShip.targetForward < 0.5f)
                    {
                        float halfSpeed = (largeShip.speedRating / 2f);

                        if (largeShip.thrustSpeed > halfSpeed)
                        {
                            largeShip.thrustInput = -1;
                        }
                        else
                        {
                            largeShip.thrustInput = 1;
                        }
                    }
                    else
                    {
                        FullSpeed(largeShip);
                    }
                }
                else
                {
                    MatchSpeed(largeShip);
                }
            }
            else
            {
                NoSpeed(largeShip);
            }
        }
    }

    //This sets the ship to half speed (typically used when no enemies are detected)
    public static void NoSpeed(LargeShip largeShip)
    {
        if (largeShip.thrustSpeed > 0)
        {
            largeShip.thrustInput = -1;
        }
        else
        {
            largeShip.thrustInput = 0;
        }
    }

    //This sets the ship to half speed (typically used when no enemies are detected)
    public static void MatchSpeed(LargeShip largeShip)
    {
        float oneThird = (largeShip.speedRating / 3f);

        if (largeShip.thrustSpeed > largeShip.targetSpeed & largeShip.thrustSpeed > oneThird)
        {
            largeShip.thrustInput = -1;
        }
        else
        {
            largeShip.thrustInput = 1;
        }
    }


    #endregion

    #region AI Weapon Control

    //This sets the targeting mode of the turret
    public static void TurretTargetModeSelection(LargeShip largeShip, string tag)
    {
        LaserTurret laserTurret = largeShip.gameObject.GetComponent<LaserTurret>();
        
        if (laserTurret != null)
        {
            if (tag.Contains("large_"))
            {
                if (tag != laserTurret.largeTargetingMode)
                {
                    LaserTurretFunctions.NullifyLargeTurretTarget(laserTurret);
                    laserTurret.largeTargetingMode = tag;
                }
            }
            else if (tag.Contains("small_"))
            {
                if (tag != laserTurret.smallTargetingMode)
                {
                    LaserTurretFunctions.NullifySmallTurretTarget(laserTurret);
                    laserTurret.smallTargetingMode = tag;
                }
            }
        }

        PlasmaTurret plasmaTurret = largeShip.gameObject.GetComponent<PlasmaTurret>();

        if (plasmaTurret != null)
        {
            if (tag.Contains("large_"))
            {
                if (tag != plasmaTurret.largeTargetingMode)
                {
                    PlasmaTurretFunctions.NullifyLargeTurretTarget(plasmaTurret);
                    plasmaTurret.largeTargetingMode = tag;
                }
            }
            else if (tag.Contains("small_"))
            {
                if (tag != plasmaTurret.smallTargetingMode)
                {
                    PlasmaTurretFunctions.NullifySmallTurretTarget(plasmaTurret);
                    plasmaTurret.smallTargetingMode = tag;
                }
            }
        }
    }

    //This sets the accuracy of the turrets
    public static void TurretAccuracySelection(LargeShip largeShip, string tag)
    {
        LaserTurret laserTurret = largeShip.gameObject.GetComponent<LaserTurret>();

        if (laserTurret != null)
        {
            if (tag.Contains("large_"))
            {
                laserTurret.largeTurretAccuracy = tag;
            }
            else if (tag.Contains("small_"))
            {
                laserTurret.smallTurretAccuracy = tag;
            }
        }

        PlasmaTurret plasmaTurret = largeShip.gameObject.GetComponent<PlasmaTurret>();

        if (plasmaTurret != null)
        {
            if (tag.Contains("large_"))
            {
                plasmaTurret.largeTurretAccuracy = tag;
            }
            else if (tag.Contains("small_"))
            {
                plasmaTurret.smallTurretAccuracy = tag;
            }
        }
    }


    #endregion

    #region AI Flight Patterns

    //This is the basic flight pattern for attack
    public static void MoveToTargetRange(LargeShip largeShip)
    {
        if (largeShip.targetDistance > 1000)
        {
            AngleTowardsTarget(largeShip);
        }
        else
        {
            KeepTargetOnRight(largeShip);
        }
    }

    //This causes the ship to circle around it's target
    public static void CircleTarget(LargeShip largeShip)
    {
        if (largeShip.targetDistance > 1500)
        {
            AngleTowardsTarget(largeShip);
        }
        else
        {
            KeepTargetOnRight(largeShip);
        }
    }

    //This angles towards the ships waypoint
    public static void MoveToWayPoint(LargeShip largeShip)
    {
        AngleTowardsWaypoint(largeShip);
    }

    //This prevents the ship from rotating and locks the direction forward
    public static void NoRotation(LargeShip largeShip)
    {
        ResetSteeringInputs(largeShip);
    }

    #endregion

    #region AI Targetting

    //This checks if the ship needs to request a new target - this function is run automatically
    public static void RequestTarget(LargeShip largeShip)
    {
        if (largeShip.target == null)
        {
            largeShip.requestingTarget = true;
        }
        else
        {
            largeShip.requestingTarget = false;
        }
    }

    //This clears the target if it doesn't meet certain conditions i.e. was destroyed or disabled  - this function is run automatically
    public static void ClearTarget(LargeShip largeShip)
    {
        if (largeShip.target != null)
        {
            if (largeShip.target.activeSelf == false)
            {
                largeShip.target = null;
            }
            else if (largeShip.targetSmallShip != null)
            {
                if (largeShip.targetSmallShip.isDisabled == true)
                {
                    largeShip.target = null;
                }
            }
            else if (largeShip.targetLargeShip != null)
            {
                if (largeShip.targetLargeShip.isDisabled == true)
                {
                    largeShip.target = null;
                }
            }
        }
    }

    #endregion

    #region AI Steering Control

    //This angles the ship towards the target vector
    public static void AngleTowardsTarget(LargeShip largeShip)
    {
        AvoidGimbalLock(largeShip, largeShip.targetForward);

        largeShip.damperner = 1 - Mathf.Clamp01(largeShip.targetForward);

        if (largeShip.avoidGimbalLock == false)
        {
                if (Vector3.Dot(largeShip.transform.up, Vector3.down) < 0)
                {
                    largeShip.turnInput = largeShip.targetRight;
                    largeShip.pitchInput = -largeShip.targetUp;
                }
                else
                {
                    largeShip.turnInput = -largeShip.targetRight;
                    largeShip.pitchInput = -largeShip.targetUp;
                }
        }

    }

    //This angles the ship away from the target vector
    public static void AngleAwayFromTarget(LargeShip largeShip)
    {
        AvoidGimbalLock(largeShip, largeShip.targetForward, true);

        largeShip.damperner = (Mathf.Clamp01(largeShip.targetForward) * -1) + 1;

        if (largeShip.avoidGimbalLock == false)
        {
            if (Vector3.Dot(largeShip.transform.up, Vector3.down) < 0)
            {
                largeShip.turnInput = -largeShip.targetRight;
                largeShip.pitchInput = largeShip.targetUp;
            }
            else
            {
                largeShip.turnInput = largeShip.targetRight;
                largeShip.pitchInput = -largeShip.targetUp;
            }
        }    
    }

    //This angles the ship towards the target vector
    public static void AngleTowardsWaypoint(LargeShip largeShip)
    {
        AvoidGimbalLock(largeShip, largeShip.waypointForward);

        largeShip.damperner = 1 - Mathf.Clamp01(largeShip.waypointForward);

        if (largeShip.avoidGimbalLock == false)
        {
            if (Vector3.Dot(largeShip.transform.up, Vector3.down) < 0)
            {
                largeShip.turnInput = largeShip.waypointRight;
                largeShip.pitchInput = -largeShip.waypointUp;
            }
            else
            {
                largeShip.turnInput = -largeShip.waypointRight;
                largeShip.pitchInput = -largeShip.waypointUp;
            }
        }
    }

    //This angles the ship away from the target vector
    public static void AngleAwayFromWaypoint(LargeShip largeShip)
    {
        AvoidGimbalLock(largeShip, largeShip.waypointForward, true);

        largeShip.damperner = (Mathf.Clamp01(largeShip.waypointForward) * -1) + 1;

        if (largeShip.avoidGimbalLock == false)
        {
            if (Vector3.Dot(largeShip.transform.up, Vector3.down) < 0)
            {
                //Right way up
                largeShip.turnInput = -largeShip.waypointRight;
                largeShip.pitchInput = largeShip.waypointUp;
            }
            else
            {
                //upside down
                largeShip.turnInput = largeShip.waypointRight;
                largeShip.pitchInput = -largeShip.waypointUp;
            }
        }
    }

    //This angles the ship towards the target vector
    public static void KeepTargetOnRight(LargeShip largeShip)
    {
        largeShip.damperner = 1;

        if (Vector3.Dot(largeShip.transform.up, Vector3.down) < 0)
        {
            //Right way up
            SmoothTurnInput(largeShip, -largeShip.targetForward);
            SmoothPitchInput(largeShip, -largeShip.targetUp);
        }
        else
        {
            //Upside down
            SmoothTurnInput(largeShip, largeShip.targetForward);
            SmoothPitchInput(largeShip, largeShip.targetUp);
        }
    }

    //This pitches the ship up
    public static void PitchUp(LargeShip largeShip)
    {
        largeShip.damperner = 1;
        SmoothPitchInput(largeShip, 1);
    }

    //This pitches the ship down
    public static void PitchDown(LargeShip largeShip)
    {
        largeShip.damperner = 1;
        SmoothPitchInput(largeShip, -1);
    }

    //This turns the ship right
    public static void TurnRight(LargeShip largeShip)
    {
        largeShip.damperner = 1;
        SmoothTurnInput(largeShip, 1);
    }

    //This turns the ship Left
    public static void TurnLeft(LargeShip largeShip)
    {
        largeShip.damperner = 1;
        SmoothTurnInput(largeShip, -1);
    }

    //This causes the ship to roll Right
    public static void RollRight(LargeShip largeShip)
    {
        largeShip.damperner = 1;
        SmoothRollInput(largeShip, 1);
    }

    //This causes the ship to roll left
    public static void RollLeft(LargeShip largeShip)
    {
        largeShip.damperner = 1;
        SmoothRollInput(largeShip, -1);
    }

    //This causes the ship to fly forward
    public static void FlyFoward(LargeShip largeShip)
    {
        largeShip.damperner = 1;
        SmoothPitchInput(largeShip, 0);
        SmoothTurnInput(largeShip, 0);
    }

    //This prevents gimbal lock when the ships turn
    public static void AvoidGimbalLock(LargeShip largeShip, float forward, bool reverse = false)
    {
        largeShip.damperner = 1;

        if (reverse == false)
        {
            if (forward < -0.9)
            {
                largeShip.avoidGimbalLock = true;

                if (Vector3.Dot(largeShip.transform.up, Vector3.down) < 0)
                {
                    //Steering when ship is the right way up
                    SmoothTurnInput(largeShip, 1);
                    SmoothPitchInput(largeShip, -0);
                }
                else
                {
                    //Steering when the ship is upside down
                    SmoothTurnInput(largeShip, -1);
                    SmoothPitchInput(largeShip, -0);
                }
            }
            else
            {
                largeShip.avoidGimbalLock = false;
            }
        }
        else
        {
            if (forward > 0.9)
            {
                largeShip.avoidGimbalLock = true;

                if (Vector3.Dot(largeShip.transform.up, Vector3.down) < 0)
                {
                    //Right way up
                    SmoothTurnInput(largeShip, -1);
                    SmoothPitchInput(largeShip, 0);
                }
                else
                {
                    //upside down
                    SmoothTurnInput(largeShip, 1);
                    SmoothPitchInput(largeShip, -0);
                }
            }
            else
            {
                largeShip.avoidGimbalLock = false;
            }
        }


    }

    //This resets all the inputs
    public static void ResetSteeringInputs(LargeShip largeShip)
    {
        largeShip.damperner = 1;
        largeShip.pitchInput = 0;
        largeShip.turnInput = 0;
        largeShip.rollInput = 0;
    }

    #endregion

    #region

    //For AI Input. These functions smoothly transitions between different pitch, turn, and roll inputs by lerping between different values like the ai is using a joystick or controller
    public static void SmoothPitchInput(LargeShip largeShip, float pitchInput)
    {
        float currentVelocity = 0;
        float smoothTime = 0.1f;

        largeShip.pitchInput = Mathf.SmoothDamp(largeShip.pitchInput, pitchInput, ref currentVelocity, smoothTime);
    }

    public static void SmoothTurnInput(LargeShip largeShip, float turnInput)
    {
        float currentVelocity = 0;
        float smoothTime = 0.1f;

        largeShip.turnInput = Mathf.SmoothDamp(largeShip.turnInput, turnInput, ref currentVelocity, smoothTime);
    }

    public static void SmoothRollInput(LargeShip largeShip, float rollInput)
    {
        float currentVelocity = 0;
        float smoothTime = 0.1f;

        largeShip.rollInput = Mathf.SmoothDamp(largeShip.rollInput, rollInput, ref currentVelocity, smoothTime);
    }

    #endregion

}
