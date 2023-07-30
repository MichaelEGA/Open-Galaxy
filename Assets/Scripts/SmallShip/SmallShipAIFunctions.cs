using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//These functions are called by the small ship functions script
public static class SmallShipAIFunctions
{

    #region Get AI Input i.e. function that calls all other functions

    //This is the base function that calls all the other AI functions except collisions which is called by an external script (evasions are called internally though)
    public static void GetAIInput(SmallShip smallShip)
    {
        SetFlightMode(smallShip);      
    }

    #endregion

    #region Set Flight Mode

    //This selects the flight mode from the avaible patterns
    public static void SetFlightMode(SmallShip smallShip)
    {
        //This selects the next enemy target
        if (smallShip.target == null)
        {
            SelectTarget(smallShip);
        }

        //This chooses between attack and patrol
        if (smallShip.target != null)
        {
            if (smallShip.target.activeSelf == true)
            {
                smallShip.aiMode = "AttackPatternAlpha";
            }
            else
            {
                smallShip.aiMode = "PatrolPatternAlpha";
            }             
        }
        else
        {
            smallShip.aiMode = "PatrolPatternAlpha";
        }

        //This activates the appropriate flight patterns
        if (smallShip.aiOverideMode == "none" || smallShip.aiOverideMode == "")
        {
            if (smallShip.aiMode == "AttackPatternAlpha")
            {
                AttackPatternAlpha(smallShip);
            }
            else if (smallShip.aiMode == "PatrolPatternAlpha")
            {
                PatrolPatternAlpha(smallShip);
            }
        }   
        else if (smallShip.aiOverideMode == "MoveToWaypoint")
        {
            MoveToWayPoint(smallShip);
        }
        else if (smallShip.aiOverideMode == "Patrol")
        {
            PatrolPatternAlpha(smallShip);
        }
        else if (smallShip.aiOverideMode == "Stationary")
        {
            Stationary(smallShip);
        }
    }

    #endregion

    #region Flight Patterns

    //This is the basic flight pattern for attack
    public static void AttackPatternAlpha(SmallShip smallShip)
    {
        FireControl(smallShip);
        AttackSpeedPowerSettings(smallShip);

        if (smallShip.targetDistance > 250)
        {
            FullSpeed(smallShip);
            AngleTowardsTarget(smallShip);
        }
        else
        {
            if (Time.time < smallShip.aiAttackTime)
            {
                MatchSpeed(smallShip);
                AngleTowardsTarget(smallShip);
                SetRetreatTime(smallShip);
            }
            else
            {

                HalfSpeed(smallShip);

                if (Time.time < smallShip.aiRetreatTime)
                {
                    AngleAwayFromTarget(smallShip);
                }
                else
                {
                    SetAttackTime(smallShip);
                    SelectRandomWaypoint(smallShip);
                }
            }

        }
    }

    //This is the basic flight pattern for patrolling
    public static void PatrolPatternAlpha(SmallShip smallShip)
    {
        if (smallShip.waypoint != null)
        {
            FireControl(smallShip);
            HalfSpeed(smallShip);
            PatrolSpeedPowerSettings(smallShip);

            float distanceToWaypoint = Vector3.Distance(smallShip.gameObject.transform.position, smallShip.waypoint.transform.position);

            if (distanceToWaypoint < 50)
            {
                SelectRandomWaypoint(smallShip);
            }

            AngleTowardsWaypoint(smallShip);
        }
    }

    //This angles towards the ships waypoint
    public static void MoveToWayPoint(SmallShip smallShip)
    {
        FireControl(smallShip);
        ThreeQuarterSpeed(smallShip);
        PatrolSpeedPowerSettings(smallShip);
        AngleTowardsWaypoint(smallShip);
    }

    //This causes the ship to come to a full stop
    public static void Stationary(SmallShip smallShip)
    {
        NoSpeed(smallShip);
    }

    #endregion

    #region Evasion and Collision Avoidance

    //This allows the ship to evade attacks and avoid collisions with other objects and ships
    public static IEnumerator Evade(SmallShip smallShip, float time, string mode, int direction)
    {

        if (smallShip != null)
        {
            smallShip.aiOverideMode = mode;

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

            smallShip.aiOverideMode = "none";

            ResetSteeringInputs(smallShip);
        }

    }

    #endregion

    #region AI Skill Level Functions

    //This sets ai skill level values
    public static void SetAISkillLevel(SmallShip smallShip)
    {
        SetRetreatTime(smallShip);
        SetAttackTime(smallShip);
        SetSpeedWhileTurning(smallShip);
    }

    //This sets the retreat time of the ship
    public static void SetRetreatTime(SmallShip smallShip)
    {
        if (smallShip.aiSkillLevel == "easy")
        {
            smallShip.aiRetreatTime = Time.time + Random.Range(15, 20);
        }
        else if (smallShip.aiSkillLevel == "medium")
        {
            smallShip.aiRetreatTime = Time.time + Random.Range(10, 15);
        }
        else if (smallShip.aiSkillLevel == "hard")
        {
            smallShip.aiRetreatTime = Time.time + Random.Range(5, 15);
        }
    }

    //This sets the attack time of the ship
    public static void SetAttackTime(SmallShip smallShip)
    {
        if (smallShip.aiSkillLevel == "easy")
        {
            smallShip.aiAttackTime = Time.time + Random.Range(30, 35);
        }
        else if (smallShip.aiSkillLevel == "medium")
        {
            smallShip.aiAttackTime = Time.time + Random.Range(30, 45);
        }
        else if (smallShip.aiSkillLevel == "hard")
        {
            smallShip.aiAttackTime = Time.time + Random.Range(30, 60);
        }
    }

    //This sets the attack time of the ship
    public static void SetSpeedWhileTurning(SmallShip smallShip)
    {
        if (smallShip.aiSkillLevel == "easy")
        {
            smallShip.aiSpeedWhileTurning = Random.Range(1.0f, 1.5f);
        }
        else if (smallShip.aiSkillLevel == "medium")
        {
            smallShip.aiSpeedWhileTurning = Random.Range(1.5f, 1.75f);
        }
        else if (smallShip.aiSkillLevel == "hard")
        {
            smallShip.aiSpeedWhileTurning = Random.Range(1.75f, 2.0f);
        }
    }

    //Targeting error margin
    public static Vector3 TargetingErrorMargin(SmallShip smallShip)
    {

        Vector3 errorMargin = new Vector3();

        float easyRange = 100;
        float mediumRange = 50;
        float hardRange = 25;

        float x = 0;
        float y = 0;
        float z = 0;

        if (smallShip.aiSkillLevel == "easy")
        {
            x = Random.Range(-easyRange, easyRange);
            y = Random.Range(-easyRange, easyRange);
            z = Random.Range(-easyRange, easyRange);
        }
        else if (smallShip.aiSkillLevel == "medium")
        {
            x = Random.Range(-mediumRange, mediumRange);
            y = Random.Range(-mediumRange, mediumRange);
            z = Random.Range(-mediumRange, mediumRange);
        }
        else if (smallShip.aiSkillLevel == "hard")
        {
            x = Random.Range(-hardRange, hardRange);
            y = Random.Range(-hardRange, hardRange);
            z = Random.Range(-hardRange, hardRange);
        }

        return errorMargin = new Vector3(x, y, z);
    }

    #endregion

    #region Targetting and Waypoints

    //This selects the next target
    public static void SelectTarget(SmallShip smallShip)
    {
        if (smallShip.target == null)
        {
            Task a = new Task(SelectEnemyTarget(smallShip));
        }
        else if (smallShip.target != null)
        {
            if (smallShip.target.activeSelf == false)
            {
                Task a = new Task(SelectEnemyTarget(smallShip));
            }
        }
    }

    //This presses the select enemy target button
    public static IEnumerator SelectEnemyTarget(SmallShip smallShip)
    {
        smallShip.getNextEnemy = true;

        yield return new WaitForSeconds(0.1f);

        smallShip.getNextEnemy = false;
    }

    //This presses the select target button
    public static IEnumerator SelectNextTarget(SmallShip smallShip)
    {

        smallShip.getNextTarget = true;

        yield return new WaitForSeconds(0.1f);

        smallShip.getNextTarget = false;

    }

    //This selects a random waypoint
    public static void SelectRandomWaypoint(SmallShip smallship)
    {
        float x = Random.Range(0, 15000);
        float y = Random.Range(0, 15000);
        float z = Random.Range(0, 15000);
        smallship.waypoint.transform.position = new Vector3(x, y, z);
    }

    #endregion

    #region Fire Control

    //This controls the lasers
    public static void FireControl(SmallShip smallShip)
    {
        if (smallShip.target != null)
        {
            if (smallShip.interceptForward > 0.90f & smallShip.interceptDistance < 500)
            {
                smallShip.fireWeapon = true;
            }
            else
            {
                smallShip.fireWeapon = false;
            }
        }
        else
        {
            smallShip.fireWeapon = false;
        } 
    }

    #endregion

    #region Energy Management

    //This controls the power guages
    public static void AttackSpeedPowerSettings(SmallShip smallShip)
    {

        if (smallShip.targetDistance > 500 & smallShip.targetForward > 0)
        {
            smallShip.powerMode = "engines";
        }
        else if (smallShip.targetDistance < 500 & smallShip.targetForward < 0)
        {
            smallShip.powerMode = "reset";
        }
        else if (smallShip.targetDistance < 500 & smallShip.targetForward > 0)
        {
            smallShip.powerMode = "lasers";
        }
        else
        {
            smallShip.powerMode = "reset";
        }
    }

    //Reset power guages
    public static void PatrolSpeedPowerSettings(SmallShip smallShip)
    {
        smallShip.powerMode = "reset";
    }

    #endregion

    #region Speed Control

    //This sets the ship to half speed (typically used when no enemies are detected)
    public static void MatchSpeed(SmallShip smallShip)
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
    }

    //This sets the ship to half speed (typically used when no enemies are detected)
    public static void FullSpeed(SmallShip smallShip)
    {
        smallShip.thrustInput = 1;
    }

    //This sets the ship to half speed (typically used when no enemies are detected)
    public static void ThreeQuarterSpeed(SmallShip smallShip)
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

    //This sets the ship to half speed (typically used when no enemies are detected)
    public static void HalfSpeed(SmallShip smallShip)
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

    //This sets the ship to half speed (typically used when no enemies are detected)
    public static void QuarterSpeed(SmallShip smallShip)
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

    //This sets the ship to half speed (typically used when no enemies are detected)
    public static void NoSpeed(SmallShip smallShip)
    {
        if (smallShip.thrustSpeed > 0)
        {
            smallShip.thrustInput = -1;
        }
        else
        {
            smallShip.thrustInput = 1;
        }
    }

    #endregion

    #region Steering Control

    //This angles the ship towards the target vector
    public static void AngleTowardsTarget(SmallShip smallShip)
    {

        if (smallShip.interceptForward < 0.8)
        {
            SmallShipFunctions.SmoothTurnInput(smallShip, smallShip.interceptRight);
            SmallShipFunctions.SmoothPitchInput(smallShip, -smallShip.interceptUp);
        }
        else
        {
            SmallShipFunctions.SmoothTurnInput(smallShip, smallShip.interceptRight * 5);
            SmallShipFunctions.SmoothPitchInput(smallShip, -smallShip.interceptUp * 5);
        }

    }

    //This angles the ship away from the target vector
    public static void AngleAwayFromTarget(SmallShip smallShip)
    {
        if (-smallShip.interceptForward < 1)
        {
            SmallShipFunctions.SmoothTurnInput(smallShip, -smallShip.interceptRight);
            SmallShipFunctions.SmoothPitchInput(smallShip, smallShip.interceptUp);
        }
    }

    //This angles the ship towards the target vector
    public static void AngleTowardsWaypoint(SmallShip smallShip)
    {
        if (smallShip.waypointForward < 0.8)
        {
            SmallShipFunctions.SmoothTurnInput(smallShip, smallShip.waypointRight);
            SmallShipFunctions.SmoothPitchInput(smallShip, -smallShip.waypointUp);
        }
        else
        {
            SmallShipFunctions.SmoothTurnInput(smallShip, smallShip.waypointRight * 5);
            SmallShipFunctions.SmoothPitchInput(smallShip, -smallShip.waypointUp * 5);
        }
    }

    //This angles the ship away from the target vector
    public static void AngleAwayFromWaypoint(SmallShip smallShip)
    {
        if (-smallShip.waypointForward < 1)
        {
            SmallShipFunctions.SmoothTurnInput(smallShip, -smallShip.waypointRight);
            SmallShipFunctions.SmoothPitchInput(smallShip, smallShip.waypointUp);
        }
    }

    //This pitches the ship up
    public static void PitchUp(SmallShip smallShip)
    {
        SmallShipFunctions.SmoothPitchInput(smallShip, 1);
    }

    //This pitches the ship down
    public static void PitchDown(SmallShip smallShip)
    {
        SmallShipFunctions.SmoothPitchInput(smallShip, -1);
    }

    //This turns the ship right
    public static void TurnRight(SmallShip smallShip)
    {
        SmallShipFunctions.SmoothTurnInput(smallShip, 1);
    }

    //This turns the ship Left
    public static void TurnLeft(SmallShip smallShip)
    {
        SmallShipFunctions.SmoothTurnInput(smallShip, -1);
    }

    //This causes the ship to roll Right
    public static void RollRight(SmallShip smallShip)
    {
        SmallShipFunctions.SmoothRollInput(smallShip, 1);
    }

    //This causes the ship to roll left
    public static void RollLeft(SmallShip smallShip)
    {
        SmallShipFunctions.SmoothRollInput(smallShip, -1);
    }

    //This causes the ship to fly forward
    public static void FlyFoward(SmallShip smallShip)
    {
        SmallShipFunctions.SmoothPitchInput(smallShip, 0);
        SmallShipFunctions.SmoothTurnInput(smallShip, 0);
    }

    //This resets all the inputs
    public static void ResetSteeringInputs(SmallShip smallShip)
    {
        smallShip.pitchInput = 0;
        smallShip.turnInput = 0;
        smallShip.rollInput = 0;
    }

    #endregion

}
