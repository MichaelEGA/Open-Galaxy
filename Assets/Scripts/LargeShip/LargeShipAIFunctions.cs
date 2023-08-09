using System.Collections;
using System.Collections.Generic;
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
            //SelectTarget(largeShip);
        }

        //This chooses between attack and patrol
        if (largeShip.target != null)
        {
            if (largeShip.target.activeSelf == true)
            {
                largeShip.aiMode = "AttackPatternAlpha";
            }
            else
            {
                largeShip.aiMode = "PatrolPatternAlpha";
            }
        }
        else
        {
            largeShip.aiMode = "PatrolPatternAlpha";
        }

        //This activates the appropriate flight patterns
        if (largeShip.aiOverideMode == "none" || largeShip.aiOverideMode == "")
        {
            if (largeShip.aiMode == "AttackPatternAlpha")
            {
                //AttackPatternAlpha(largeShip);
            }
            else if (largeShip.aiMode == "PatrolPatternAlpha")
            {
                //PatrolPatternAlpha(largeShip);
            }
        }
        else if (largeShip.aiOverideMode == "MoveToWaypoint")
        {
            //MoveToWayPoint(largeShip);
        }
        else if (largeShip.aiOverideMode == "Patrol")
        {
            //PatrolPatternAlpha(largeShip);
        }
        else if (largeShip.aiOverideMode == "Stationary")
        {
            //Stationary(largeShip);
        }
    }

    #endregion

}
