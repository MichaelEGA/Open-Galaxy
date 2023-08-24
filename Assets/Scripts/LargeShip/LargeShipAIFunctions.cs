using System.Collections;
using UnityEngine;

public static class LargeShipAIFunctions
{

    #region Get AI Input i.e. function that calls all other functions

    //This is the base function that calls all the other AI functions except collisions which is called by an external script (evasions are called internally though)
    public static void GetAIInput(LargeShip largeShip)
    {
        SetFlightMode(largeShip);
    }

    #endregion

    #region Set Flight Mode

    //This selects the flight mode from the avaible patterns
    public static void SetFlightMode(LargeShip largeShip)
    {
        //This selects the next enemy target
        if (largeShip.target == null)
        {
            SelectTarget(largeShip);
        }

        //This chooses between attack and patrol
        if (largeShip.target != null)
        {
            if (largeShip.target.activeSelf == true)
            {
                LargeShip targetLargeShip = largeShip.target.GetComponent<LargeShip>();

                if (targetLargeShip != null)
                {
                    if (largeShip.classType == "medium")
                    {
                        if (targetLargeShip.classType == "large")
                        {
                            largeShip.aiMode = "AttackPatternBeta";
                        }
                        else
                        {
                            largeShip.aiMode = "AttackPatternAlpha";
                        }
                    }
                    else if (largeShip.classType == "small")
                    {
                        if (targetLargeShip.classType != "small")
                        {
                            largeShip.aiMode = "AttackPatternBeta";
                        }
                        else
                        {
                            largeShip.aiMode = "AttackPatternAlpha";
                        }
                    }
                    else if (largeShip.classType == "large")
                    {
                        largeShip.aiMode = "AttackPatternAlpha";
                    }
                    else
                    {
                        largeShip.aiMode = "Stationary";
                    }
                }
                else
                {
                    largeShip.aiMode = "Stationary";
                }
            }
            else
            {
                largeShip.aiMode = "Stationary";
            }
        }
        else
        {
            largeShip.aiMode = "Stationary";
        }

        //This activates the appropriate flight patterns
        if (largeShip.aiOverideMode == "none" || largeShip.aiOverideMode == "")
        {
            if (largeShip.aiMode == "AttackPatternAlpha")
            {
                AttackPatternAlpha(largeShip);
            }
            else if (largeShip.aiMode == "AttackPatternBeta")
            {
                AttackPatternBeta(largeShip);
            }
            else if (largeShip.aiMode == "Stationary")
            {
                Stationary(largeShip);
            }
        }
        else if (largeShip.aiOverideMode == "MoveToWaypoint")
        {
            MoveToWayPoint(largeShip);
        }
        else if (largeShip.aiOverideMode == "Stationary")
        {
            Stationary(largeShip);
        }
    }

    #endregion

    #region Flight Patterns

    //This is the basic flight pattern for attack
    public static void AttackPatternAlpha(LargeShip largeShip)
    {
        if (largeShip.targetDistance > 1000)
        {
            FullSpeed(largeShip);
            AngleTowardsTarget(largeShip);
        }
        else
        {
           NoSpeed(largeShip);
           //KeepTargetOnRight(largeShip);
        }
    }

    //This causes the ship to circle around it's target
    public static void AttackPatternBeta(LargeShip largeShip)
    {
        if (largeShip.targetDistance > 1500)
        {
            FullSpeed(largeShip);
            AngleTowardsTarget(largeShip);
        }
        else
        {
            FullSpeed(largeShip);
            KeepTargetOnRight(largeShip);
        }
    }

    //This angles towards the ships waypoint
    public static void MoveToWayPoint(LargeShip largeShip)
    {
        ThreeQuarterSpeed(largeShip);
        AngleTowardsWaypoint(largeShip);
    }

    //This causes the ship to come to a full stop
    public static void Stationary(LargeShip largeShip)
    {
        NoSpeed(largeShip);
    }

    #endregion

    #region Evasion and Collision Avoidance

    //This allows the ship to evade attacks and avoid collisions with other objects and ships
    public static IEnumerator Evade(LargeShip largeShip, float time, string mode, int direction)
    {

        if (largeShip != null)
        {
            largeShip.aiOverideMode = mode;

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

            largeShip.aiOverideMode = "none";

            ResetSteeringInputs(largeShip);
        }

    }

    #endregion

    #region Targetting and Waypoints

    //This selects the next target
    public static void SelectTarget(LargeShip largeShip)
    {
        if (largeShip.target == null)
        {
            Task a = new Task(SelectEnemyTarget(largeShip));
        }
        else if (largeShip.target != null)
        {
            if (largeShip.target.activeSelf == false)
            {
                Task a = new Task(SelectEnemyTarget(largeShip));
            }
        }
    }

    //This presses the select enemy target button
    public static IEnumerator SelectEnemyTarget(LargeShip largeShip)
    {
        largeShip.getNextEnemy = true;

        yield return new WaitForSeconds(0.1f);

        largeShip.getNextEnemy = false;
    }

    //This presses the select target button
    public static IEnumerator SelectNextTarget(LargeShip largeShip)
    {

        largeShip.getNextTarget = true;

        yield return new WaitForSeconds(0.1f);

        largeShip.getNextTarget = false;

    }

    //This selects a random waypoint
    public static void SelectRandomWaypoint(LargeShip largeShip)
    {
        float x = Random.Range(0, 15000);
        float y = Random.Range(0, 15000);
        float z = Random.Range(0, 15000);
        largeShip.waypoint.transform.position = new Vector3(x, y, z);
    }

    #endregion

    #region Speed Control

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

    #endregion

    #region Steering Control

    //This angles the ship towards the target vector
    public static void AngleTowardsTarget(LargeShip largeShip)
    {
        if (largeShip.targetForward < 0.95)
        {
            LargeShipFunctions.SmoothTurnInput(largeShip, largeShip.targetRight);
            LargeShipFunctions.SmoothPitchInput(largeShip, -largeShip.targetUp);
        }
        else
        {
            LargeShipFunctions.NoInput(largeShip);
        }
    }

    //This angles the ship away from the target vector
    public static void AngleAwayFromTarget(LargeShip largeShip)
    {
        if (largeShip.targetForward > -0.95)
        {
            LargeShipFunctions.SmoothTurnInput(largeShip, -largeShip.targetRight);
            LargeShipFunctions.SmoothPitchInput(largeShip, largeShip.targetUp);
        }
        else
        {
            LargeShipFunctions.NoInput(largeShip);
        }
    }

    //This angles the ship towards the target vector
    public static void AngleTowardsWaypoint(LargeShip largeShip)
    {
        if (largeShip.waypointForward < 0.95)
        {
            LargeShipFunctions.SmoothTurnInput(largeShip, largeShip.waypointRight);
            LargeShipFunctions.SmoothPitchInput(largeShip, -largeShip.waypointUp);
        }
        else
        {
            LargeShipFunctions.NoInput(largeShip);
        }
    }

    //This angles the ship away from the target vector
    public static void AngleAwayFromWaypoint(LargeShip largeShip)
    {
        if (largeShip.waypointForward > -0.95)
        {
            LargeShipFunctions.SmoothTurnInput(largeShip, -largeShip.waypointRight);
            LargeShipFunctions.SmoothPitchInput(largeShip, largeShip.waypointUp);
        }
        else
        {
            LargeShipFunctions.NoInput(largeShip);
        }
    }

    //This angles the ship towards the target vector
    public static void KeepTargetOnRight(LargeShip largeShip)
    {
        if (largeShip.targetRight > -0.95)
        {
            LargeShipFunctions.SmoothTurnInput(largeShip, -largeShip.targetForward);
            LargeShipFunctions.SmoothPitchInput(largeShip, -largeShip.targetUp);
        }
        else
        {
            LargeShipFunctions.NoInput(largeShip);
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

    //This resets all the inputs
    public static void ResetSteeringInputs(LargeShip largeShip)
    {
        largeShip.pitchInput = 0;
        largeShip.turnInput = 0;
        largeShip.rollInput = 0;
    }

    #endregion


}
