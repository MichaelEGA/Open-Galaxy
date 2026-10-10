using UnityEngine;

public static class EnergyManagementSystemFunctions
{
    //This calculates the ships power distribution
    public static void CalculatePower(EnergyManagementSystem energyManagementSystem)
    {
        if (energyManagementSystem.powerPressedTime < Time.time)
        {
            //This checks the current power mode
            if (energyManagementSystem.powerToLasers == true)
            {
                energyManagementSystem.powerMode = "lasers";

                if (energyManagementSystem.ship.isAI == false)
                {
                    AudioFunctions.PlayAudioClip(energyManagementSystem.ship.audioManager, "beep01_toggle", "Cockpit", energyManagementSystem.gameObject.transform.position, 0, 1, 500, 1, 100);
                }

            }
            else if (energyManagementSystem.powerToEngine == true)
            {
                energyManagementSystem.powerMode = "engines";

                if (energyManagementSystem.ship.isAI == false)
                {
                    AudioFunctions.PlayAudioClip(energyManagementSystem.ship.audioManager, "beep01_toggle", "Cockpit", energyManagementSystem.gameObject.transform.position, 0, 1, 500, 1, 100);
                }

            }
            else if (energyManagementSystem.powerToShields == true)
            {
                energyManagementSystem.powerMode = "shields";

                if (energyManagementSystem.ship.isAI == false)
                {
                    AudioFunctions.PlayAudioClip(energyManagementSystem.ship.audioManager, "beep01_toggle", "Cockpit", energyManagementSystem.gameObject.transform.position, 0, 1, 500, 1, 100);
                }

            }
            else if (energyManagementSystem.resetPowerLevels == true)
            {
                energyManagementSystem.powerMode = "reset";

                if (energyManagementSystem.ship.isAI == false)
                {
                    AudioFunctions.PlayAudioClip(energyManagementSystem.ship.audioManager, "beep01_toggle", "Cockpit", energyManagementSystem.gameObject.transform.position, 0, 1, 500, 1, 100);
                }

            }

            energyManagementSystem.powerPressedTime = Time.time + 0.2f;

        }

        //This sets the ships power according the mode
        if (energyManagementSystem.ship.shieldRating != 0)
        {
            if (energyManagementSystem.powerMode == "lasers")
            {
                if (energyManagementSystem.energyWeaponPower < 100) { energyManagementSystem.energyWeaponPower += 1; }
                if (energyManagementSystem.enginePower > 25) { energyManagementSystem.enginePower -= 1; }
                if (energyManagementSystem.shieldPower > 25) { energyManagementSystem.shieldPower -= 1; }
            }
            else if (energyManagementSystem.powerMode == "engines")
            {
                if (energyManagementSystem.energyWeaponPower > 25) { energyManagementSystem.energyWeaponPower -= 1; }
                if (energyManagementSystem.enginePower < 100) { energyManagementSystem.enginePower += 1; }
                if (energyManagementSystem.shieldPower > 25) { energyManagementSystem.shieldPower -= 1; }
            }
            else if (energyManagementSystem.powerMode == "shields")
            {
                if (energyManagementSystem.energyWeaponPower > 25) { energyManagementSystem.energyWeaponPower -= 1; }
                if (energyManagementSystem.enginePower > 25) { energyManagementSystem.enginePower -= 1; }
                if (energyManagementSystem.shieldPower < 100) { energyManagementSystem.shieldPower += 1; }
            }
            else if (energyManagementSystem.powerMode == "reset")
            {
                if (energyManagementSystem.energyWeaponPower > 50) { energyManagementSystem.energyWeaponPower -= 1; } else if (energyManagementSystem.energyWeaponPower < 50) { energyManagementSystem.energyWeaponPower += 1; }
                if (energyManagementSystem.enginePower > 50) { energyManagementSystem.enginePower -= 1; } else if (energyManagementSystem.enginePower < 50) { energyManagementSystem.enginePower += 1; }
                if (energyManagementSystem.shieldPower > 50) { energyManagementSystem.shieldPower -= 1; } else if (energyManagementSystem.shieldPower < 50) { energyManagementSystem.shieldPower += 1; }
            }
        }
        else
        {
            if (energyManagementSystem.powerMode == "lasers")
            {
                if (energyManagementSystem.energyWeaponPower < 100) { energyManagementSystem.energyWeaponPower += 1; }
                if (energyManagementSystem.enginePower > 25) { energyManagementSystem.enginePower -= 1; }
                if (energyManagementSystem.shieldPower > 0) { energyManagementSystem.shieldPower -= 1; }
            }
            else if (energyManagementSystem.powerMode == "engines")
            {
                if (energyManagementSystem.energyWeaponPower > 25) { energyManagementSystem.energyWeaponPower -= 1; }
                if (energyManagementSystem.enginePower < 100) { energyManagementSystem.enginePower += 1; }
                if (energyManagementSystem.shieldPower > 0) { energyManagementSystem.shieldPower -= 1; }
            }
            else if (energyManagementSystem.powerMode == "shields")
            {
                if (energyManagementSystem.energyWeaponPower > 50) { energyManagementSystem.energyWeaponPower -= 1; } else if (energyManagementSystem.energyWeaponPower < 50) { energyManagementSystem.energyWeaponPower += 1; }
                if (energyManagementSystem.enginePower > 50) { energyManagementSystem.enginePower -= 1; } else if (energyManagementSystem.enginePower < 50) { energyManagementSystem.enginePower += 1; }
                if (energyManagementSystem.shieldPower > 0) { energyManagementSystem.shieldPower -= 1; }
            }
            else if (energyManagementSystem.powerMode == "reset")
            {
                if (energyManagementSystem.energyWeaponPower > 50) { energyManagementSystem.energyWeaponPower -= 1; } else if (energyManagementSystem.energyWeaponPower < 50) { energyManagementSystem.energyWeaponPower += 1; }
                if (energyManagementSystem.enginePower > 50) { energyManagementSystem.enginePower -= 1; } else if (energyManagementSystem.enginePower < 50) { energyManagementSystem.enginePower += 1; }
                if (energyManagementSystem.shieldPower > 0) { energyManagementSystem.shieldPower -= 1; }
            }
        }

    }

    //This calculates the ships power levels
    public static void CalculateLevels(EnergyManagementSystem energyManagementSystem)
    {
        //This sets the recharge and discharge rate if not set for wep
        if (energyManagementSystem.wepRecharge == 0) { energyManagementSystem.wepRecharge = 0.1f; }
        if (energyManagementSystem.wepDischarge == 0) { energyManagementSystem.wepDischarge = 0.25f; }

        //This sets the ships wep power levels
        if (energyManagementSystem.powerMode == "engines")
        {
            if (energyManagementSystem.ship.flightControlSystem_Small.wep == false & energyManagementSystem.ship.flightControlSystem_Small.thrustInput <= 0)
            {
                if (energyManagementSystem.wepLevel < 100) { energyManagementSystem.wepLevel += energyManagementSystem.wepRecharge; }
            }
            else
            {
                if (energyManagementSystem.wepLevel > 0) { energyManagementSystem.wepLevel -= energyManagementSystem.wepDischarge; }
            }
        }
        else if (energyManagementSystem.powerMode == "reset")
        {
            if (energyManagementSystem.ship.flightControlSystem_Small.wep == false & energyManagementSystem.ship.flightControlSystem_Small.thrustInput <= 0)
            {
                if (energyManagementSystem.wepLevel > 50) { energyManagementSystem.wepLevel -= energyManagementSystem.wepDischarge; }
                else if (energyManagementSystem.wepLevel < 50) { energyManagementSystem.wepLevel += energyManagementSystem.wepRecharge; }
            }
            else
            {
                if (energyManagementSystem.wepLevel > 0) { energyManagementSystem.wepLevel -= energyManagementSystem.wepDischarge; }
            }
        }
        else
        {
            if (energyManagementSystem.wepLevel > 0) { energyManagementSystem.wepLevel -= energyManagementSystem.wepDischarge; }
        }

        //This sets the recharge and discharge rate if not set for shields
        if (energyManagementSystem.shieldRecharge == 0) { energyManagementSystem.shieldRecharge = 0.01f; }
        if (energyManagementSystem.shieldDischarge == 0) { energyManagementSystem.shieldDischarge = 0.01f; }

        //This sets the ships shield power levels
        if (energyManagementSystem.ship.shieldRating != 0)
        {
            if (energyManagementSystem.powerMode == "shields")
            {
                if (energyManagementSystem.frontShieldLevel < energyManagementSystem.ship.shieldRating / 2f)
                {
                    energyManagementSystem.frontShieldLevel += energyManagementSystem.shieldRecharge;
                }

                if (energyManagementSystem.rearShieldLevel < energyManagementSystem.ship.shieldRating / 2f)
                {
                    energyManagementSystem.rearShieldLevel += energyManagementSystem.shieldRecharge;
                }

                energyManagementSystem.shieldLevel = energyManagementSystem.rearShieldLevel + energyManagementSystem.frontShieldLevel;

            }
            else if (energyManagementSystem.powerMode != "shields" & energyManagementSystem.powerMode != "reset")
            {
                if (energyManagementSystem.frontShieldLevel > 0)
                {
                    energyManagementSystem.frontShieldLevel -= energyManagementSystem.shieldDischarge;
                }

                if (energyManagementSystem.rearShieldLevel > 0)
                {
                    energyManagementSystem.rearShieldLevel -= energyManagementSystem.shieldDischarge;
                }

                energyManagementSystem.shieldLevel = energyManagementSystem.rearShieldLevel + energyManagementSystem.frontShieldLevel;

            }
        }
    }
}
