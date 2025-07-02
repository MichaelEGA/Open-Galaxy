using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using TMPro;
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
        AddTag(largeShip, "fireweapons");
        AddTag(largeShip, "norotation");
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
            else if (tag == "fireweapons" || tag == "noweapons")
            {
                RemoveSingleTag(largeShip, "fireweapons");
                RemoveSingleTag(largeShip, "noweapons");
            }
            else if (tag == "circletarget" || tag == "movetowaypoint" || tag == "stayinrangeoftarget" || tag == "norotation")
            {
                RemoveSingleTag(largeShip, "movetowaypoint");
                RemoveSingleTag(largeShip, "stayinrangeoftarget");
                RemoveSingleTag(largeShip, "circletarget");
                RemoveSingleTag(largeShip, "norotation");
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
                    else if (tag == "fireweapons") //Weapon control
                    {
                        FireWeapons(largeShip);
                    }
                    else if (tag == "noweapons")
                    {
                        NoWeapons(largeShip);
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

    //This allows the turrets to fire
    public static void FireWeapons(LargeShip largeShip)
    {
        largeShip.weaponsLock = false;
    }

    //This prevents the turrets from firing
    public static void NoWeapons(LargeShip largeShip)
    {
        largeShip.weaponsLock = true;
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

    //This causes the ship to come to a full stop
    public static void Stationary(LargeShip largeShip)
    {
        NoSpeed(largeShip);
    }

    //This selects a random waypoint
    public static void SelectRandomWaypoint(LargeShip largeShip)
    {
        float x = Random.Range(0, 15000);
        float y = Random.Range(0, 15000);
        float z = Random.Range(0, 15000);
        largeShip.waypoint.transform.position = new Vector3(x, y, z);
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

    #region AI Evasion and Collision Avoidance

    //This allows the ship to evade attacks and avoid collisions with other objects and ships
    public static IEnumerator Evade(LargeShip largeShip, float time, string mode, int direction)
    {
        if (TagExists(largeShip, "nospeed") == true & TagExists(largeShip, "norotation") == true)
        {
            //Do nothing
        }
        else
        {
            if (largeShip != null)
            {
                largeShip.aiEvade = true;

                time = time + Time.time;

                while (time > Time.time)
                {
                    NoSpeed(largeShip);

                    if (direction == 0)
                    {
                        TurnRight(largeShip);
                    }
                    else if (direction == 1)
                    {
                        TurnLeft(largeShip);
                    }
                    else if (direction == 2)
                    {
                        PitchUp(largeShip);
                    }
                    else if (direction == 3)
                    {
                        PitchDown(largeShip);
                    }
                    else if (direction == 4)
                    {
                        RollRight(largeShip);
                    }
                    else if (direction == 5)
                    {
                        RollLeft(largeShip);
                    }
                    else if (direction == 6)
                    {
                        FlyFoward(largeShip);
                    }

                    yield return null;

                }

                ResetSteeringInputs(largeShip);

                largeShip.aiEvade = false;
            }
        }
    }

    #endregion

    #region AI Steering Control

    //This angles the ship towards the target vector
    public static void AngleTowardsTarget(LargeShip largeShip)
    {
        if (largeShip.targetForward > 0.99)
        {
            largeShip.reducemaneuvarability = true;
        }
        else
        {
            largeShip.reducemaneuvarability = false;
        }

        AvoidGimbalLock(largeShip, largeShip.targetForward);

        if (largeShip.avoidGimbalLock == false)
        {

            if (Vector3.Dot(largeShip.transform.up, Vector3.down) < 0)
            {
                //Right way up
                LargeShipFunctions.SmoothTurnInput(largeShip, largeShip.targetRight);
                LargeShipFunctions.SmoothPitchInput(largeShip, -largeShip.targetUp);
            }
            else
            {
                //Upside down
                LargeShipFunctions.SmoothTurnInput(largeShip, -largeShip.targetRight);
                LargeShipFunctions.SmoothPitchInput(largeShip, -largeShip.targetUp);
            }
        }
    }

    //This angles the ship away from the target vector
    public static void AngleAwayFromTarget(LargeShip largeShip)
    {
        if (largeShip.targetForward > -0.99)
        {
            largeShip.reducemaneuvarability = false;
        }
        else
        {
            largeShip.reducemaneuvarability = true;
        }

        AvoidGimbalLock(largeShip, largeShip.targetForward, true);

        if (largeShip.avoidGimbalLock == false)
        {
            if (Vector3.Dot(largeShip.transform.up, Vector3.down) < 0)
            {
                //Right way up
                LargeShipFunctions.SmoothTurnInput(largeShip, -largeShip.targetRight);
                LargeShipFunctions.SmoothPitchInput(largeShip, largeShip.targetUp);
            }
            else
            {
                //Upside down
                LargeShipFunctions.SmoothTurnInput(largeShip, largeShip.targetRight);
                LargeShipFunctions.SmoothPitchInput(largeShip, -largeShip.targetUp);
            }
        }
    }

    //This angles the ship towards the target vector
    public static void AngleTowardsWaypoint(LargeShip largeShip)
    {
        if (largeShip.waypointForward > 0.99)
        {
            largeShip.reducemaneuvarability = true;
        }
        else
        {
            largeShip.reducemaneuvarability = false;
        }

        AvoidGimbalLock(largeShip, largeShip.waypointForward);

        if (largeShip.avoidGimbalLock == false)
        {
            if (Vector3.Dot(largeShip.transform.up, Vector3.down) < 0)
            {
                //Steering when ship is the right way up
                LargeShipFunctions.SmoothTurnInput(largeShip, largeShip.waypointRight);
                LargeShipFunctions.SmoothPitchInput(largeShip, -largeShip.waypointUp);
            }
            else
            {
                //Steering when the ship is upside down
                LargeShipFunctions.SmoothTurnInput(largeShip, -largeShip.waypointRight);
                LargeShipFunctions.SmoothPitchInput(largeShip, -largeShip.waypointUp);
            }
        }
    }

    //This angles the ship away from the target vector
    public static void AngleAwayFromWaypoint(LargeShip largeShip)
    {
        if (largeShip.waypointForward > -0.99)
        {
            largeShip.reducemaneuvarability = false;
        }
        else
        {
            largeShip.reducemaneuvarability = true;
        }

        AvoidGimbalLock(largeShip, largeShip.waypointForward, true);

        if (largeShip.avoidGimbalLock == false)
        {
            if (largeShip.waypointForward > -0.95)
            {
                if (Vector3.Dot(largeShip.transform.up, Vector3.down) < 0)
                {
                    //Right way up
                    LargeShipFunctions.SmoothTurnInput(largeShip, -largeShip.waypointRight);
                    LargeShipFunctions.SmoothPitchInput(largeShip, largeShip.waypointUp);
                }
                else
                {
                    //upside down
                    LargeShipFunctions.SmoothTurnInput(largeShip, largeShip.waypointRight);
                    LargeShipFunctions.SmoothPitchInput(largeShip, -largeShip.waypointUp);
                }
            }
            else
            {
                LargeShipFunctions.NoInput(largeShip);
            }
        }
    }

    //This angles the ship towards the target vector
    public static void KeepTargetOnRight(LargeShip largeShip)
    {
        if (largeShip.targetRight > -0.99)
        {
            largeShip.reducemaneuvarability = false;
        }
        else
        {
            largeShip.reducemaneuvarability = true;
        }

        if (Vector3.Dot(largeShip.transform.up, Vector3.down) < 0)
        {
            //Right way up
            LargeShipFunctions.SmoothTurnInput(largeShip, -largeShip.targetForward);
            LargeShipFunctions.SmoothPitchInput(largeShip, -largeShip.targetUp);
        }
        else
        {
            //Upside down
            LargeShipFunctions.SmoothTurnInput(largeShip, largeShip.targetForward);
            LargeShipFunctions.SmoothPitchInput(largeShip, largeShip.targetUp);
        }
    }

    //This pitches the ship up
    public static void PitchUp(LargeShip largeShip)
    {
        LargeShipFunctions.SmoothPitchInput(largeShip, 1);
    }

    //This pitches the ship down
    public static void PitchDown(LargeShip largeShip)
    {
        LargeShipFunctions.SmoothPitchInput(largeShip, -1);
    }

    //This turns the ship right
    public static void TurnRight(LargeShip largeShip)
    {
        LargeShipFunctions.SmoothTurnInput(largeShip, 1);
    }

    //This turns the ship Left
    public static void TurnLeft(LargeShip largeShip)
    {
        LargeShipFunctions.SmoothTurnInput(largeShip, -1);
    }

    //This causes the ship to roll Right
    public static void RollRight(LargeShip largeShip)
    {
        LargeShipFunctions.SmoothRollInput(largeShip, 1);
    }

    //This causes the ship to roll left
    public static void RollLeft(LargeShip largeShip)
    {
        LargeShipFunctions.SmoothRollInput(largeShip, -1);
    }

    //This causes the ship to fly forward
    public static void FlyFoward(LargeShip largeShip)
    {
        LargeShipFunctions.SmoothPitchInput(largeShip, 0);
        LargeShipFunctions.SmoothTurnInput(largeShip, 0);
    }

    //This prevents gimbal lock when the ships turn
    public static void AvoidGimbalLock(LargeShip largeShip, float forward, bool reverse = false)
    {
      
        if (reverse == false)
        {
            if (forward < -0.9)
            {
                largeShip.avoidGimbalLock = true;

                if (Vector3.Dot(largeShip.transform.up, Vector3.down) < 0)
                {
                    //Steering when ship is the right way up
                    LargeShipFunctions.SmoothTurnInput(largeShip, 1);
                    LargeShipFunctions.SmoothPitchInput(largeShip, -0);
                }
                else
                {
                    //Steering when the ship is upside down
                    LargeShipFunctions.SmoothTurnInput(largeShip, -1);
                    LargeShipFunctions.SmoothPitchInput(largeShip, -0);
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
                    LargeShipFunctions.SmoothTurnInput(largeShip, -1);
                    LargeShipFunctions.SmoothPitchInput(largeShip, 0);
                }
                else
                {
                    //upside down
                    LargeShipFunctions.SmoothTurnInput(largeShip, 1);
                    LargeShipFunctions.SmoothPitchInput(largeShip, -0);
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
        largeShip.pitchInput = 0;
        largeShip.turnInput = 0;
        largeShip.rollInput = 0;
    }

    #endregion

}
