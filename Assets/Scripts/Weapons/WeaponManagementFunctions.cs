using UnityEngine;

public class WeaponManagementFunctions 
{
    public static void GetInput(WeaponManagement weaponManagement)
    {
        if (weaponManagement.ship.isAI == false)
        {
            weaponManagement.fireWeapon = weaponManagement.ship.ogInput.fireWeapon;
            weaponManagement.rapidFire = weaponManagement.ship.ogInput.rapidFire;
            weaponManagement.fireCounterMeasures = weaponManagement.ship.ogInput.fireCounterMeasures;
            weaponManagement.toggleWeapons = weaponManagement.ship.ogInput.toggleWeapons;
            weaponManagement.toggleWeaponNumber = weaponManagement.ship.ogInput.toggleWeaponNumber;
        }
    }

    //This toggles between different types of weapons
    public static void ToggleWeapons(WeaponManagement weaponManagement)
    {

        TorpedoSystem torpedoTubes = weaponManagement.GetComponent<TorpedoSystem>();
        bool hasTorpedos = true;
        float torpedoNumber = torpedoTubes.torpedoNumber;


        if (weaponManagement.hasPlasma == false)
        {
            if (weaponManagement.toggleWeapons == true & weaponManagement.toggleWeaponPressedTime < Time.time & weaponManagement.ship.damageSystem.isDisabled == false & weaponManagement.preventWeaponChange == false)
            {
                if (hasTorpedos == true & torpedoNumber > 0 & weaponManagement.hasIon == true)
                {
                    if (weaponManagement.weaponType == "" || weaponManagement.weaponType == "---" || weaponManagement.weaponType == "plasma")
                    {
                        weaponManagement.weaponType = "lasers";
                        weaponManagement.weaponMode = "single";
                    }

                    if (weaponManagement.weaponType == "lasers")
                    {
                        weaponManagement.weaponType = "ion";
                        weaponManagement.weaponMode = "single";
                    }
                    else if (weaponManagement.weaponType == "ion")
                    {
                        weaponManagement.weaponType = "torpedos";
                        weaponManagement.weaponMode = "single";
                    }
                    else if (weaponManagement.weaponType == "torpedos")
                    {
                        weaponManagement.weaponType = "lasers";
                        weaponManagement.weaponMode = "single";
                    }

                    if (weaponManagement.ship.isAI == false)
                    {
                        AudioFunctions.PlayAudioClip(weaponManagement.ship.audioManager, "beep03_weaponchange", "Cockpit", weaponManagement.gameObject.transform.position, 0, 1, 500, 1, 100);
                    }
                }
                else if (hasTorpedos == true & torpedoNumber > 0 & weaponManagement.hasIon == false)
                {
                    if (weaponManagement.weaponType == "" || weaponManagement.weaponType == "---" || weaponManagement.weaponType == "ion" || weaponManagement.weaponType == "plasma")
                    {
                        weaponManagement.weaponType = "lasers";
                        weaponManagement.weaponMode = "single";
                    }

                    if (weaponManagement.weaponType == "lasers")
                    {
                        weaponManagement.weaponType = "torpedos";
                        weaponManagement.weaponMode = "single";
                    }
                    else if (weaponManagement.weaponType == "torpedos")
                    {
                        weaponManagement.weaponType = "lasers";
                        weaponManagement.weaponMode = "single";
                    }

                    if (weaponManagement.ship.isAI == false)
                    {
                        AudioFunctions.PlayAudioClip(weaponManagement.ship.audioManager, "beep03_weaponchange", "Cockpit", weaponManagement.gameObject.transform.position, 0, 1, 500, 1, 100);
                    }
                }
                else if (hasTorpedos == false & weaponManagement.hasIon == true || hasTorpedos == true & torpedoNumber <= 0 & weaponManagement.hasIon == true)
                {
                    if (weaponManagement.weaponType == "" || weaponManagement.weaponType == "---" || weaponManagement.weaponType == "torpedos" || weaponManagement.weaponType == "plasma")
                    {
                        weaponManagement.weaponType = "lasers";
                        weaponManagement.weaponMode = "single";
                    }

                    if (weaponManagement.weaponType == "lasers")
                    {
                        weaponManagement.weaponType = "ion";
                        weaponManagement.weaponMode = "single";
                    }
                    else if (weaponManagement.weaponType == "ion")
                    {
                        weaponManagement.weaponType = "lasers";
                        weaponManagement.weaponMode = "single";
                    }

                    if (weaponManagement.ship.isAI == false)
                    {
                        AudioFunctions.PlayAudioClip(weaponManagement.ship.audioManager, "beep03_weaponchange", "Cockpit", weaponManagement.gameObject.transform.position, 0, 1, 500, 1, 100);
                    }
                }
                else
                {
                    weaponManagement.weaponType = "lasers";
                }

                weaponManagement.toggleWeaponPressedTime = Time.time + 0.25f;
            }
            else if (weaponManagement.ship.damageSystem.isDisabled == true)
            {
                weaponManagement.weaponType = "---";
                weaponManagement.weaponMode = "---";
            }

            if (hasTorpedos == true & torpedoNumber <= 0 & weaponManagement.weaponType == "torpedos")
            {
                weaponManagement.weaponType = "lasers";
                weaponManagement.weaponMode = "single";
            }

            weaponManagement.toggleWeapons = false;
        }
        else
        {
            if (weaponManagement.toggleWeapons == true & weaponManagement.toggleWeaponPressedTime < Time.time & weaponManagement.ship.damageSystem.isDisabled == false & weaponManagement.preventWeaponChange == false)
            {
                if (hasTorpedos == true & torpedoNumber > 0)
                {
                    if (weaponManagement.weaponType == "" || weaponManagement.weaponType == "---" || weaponManagement.weaponType == "ion" || weaponManagement.weaponType == "lasers")
                    {
                        weaponManagement.weaponType = "plasma";
                        weaponManagement.weaponMode = "single";
                    }

                    if (weaponManagement.weaponType == "plasma")
                    {
                        weaponManagement.weaponType = "torpedos";
                        weaponManagement.weaponMode = "single";
                    }
                    else if (weaponManagement.weaponType == "torpedos")
                    {
                        weaponManagement.weaponType = "plasma";
                        weaponManagement.weaponMode = "single";
                    }

                    if (weaponManagement.ship.isAI == false)
                    {
                        AudioFunctions.PlayAudioClip(weaponManagement.ship.audioManager, "beep03_weaponchange", "Cockpit", weaponManagement.gameObject.transform.position, 0, 1, 500, 1, 100);
                    }
                }
                else if (hasTorpedos == true & torpedoNumber <= 0)
                {
                    weaponManagement.weaponType = "plasma";
                    weaponManagement.weaponMode = "single";

                    if (weaponManagement.ship.isAI == false)
                    {
                        AudioFunctions.PlayAudioClip(weaponManagement.ship.audioManager, "beep03_weaponchange", "Cockpit", weaponManagement.gameObject.transform.position, 0, 1, 500, 1, 100);
                    }
                }
                else
                {
                    weaponManagement.weaponType = "plasma";
                }

                weaponManagement.toggleWeaponPressedTime = Time.time + 0.25f;
            }
            else if (weaponManagement.ship.damageSystem.isDisabled == true)
            {
                weaponManagement.weaponType = "---";
                weaponManagement.weaponMode = "---";
            }

            if (hasTorpedos == true & torpedoNumber <= 0 & weaponManagement.weaponType == "torpedos")
            {
                weaponManagement.weaponType = "plasma";
                weaponManagement.weaponMode = "single";
            }

            weaponManagement.toggleWeapons = false;
        }
    }

    //This manually sets the weapon on a smallship
    public static void SetWeapons(WeaponManagement weaponManagement, string weapon, string mode = "single")
    {
        weaponManagement.weaponType = weapon;
        weaponManagement.weaponMode = mode;

        if (weaponManagement.ship.isAI == false)
        {
            AudioFunctions.PlayAudioClip(weaponManagement.ship.audioManager, "beep03_weaponchange", "Cockpit", weaponManagement.gameObject.transform.position, 0, 1, 500, 1, 100);
        }
    }

    //This get's the weapon type
    public static int GetWeaponType(WeaponManagement weaponManagement)
    {
        int weaponType = 0;

        if (weaponManagement != null)
        {
            if (weaponManagement.weaponType == "laser")
            {
                weaponType = 0;
            }
            else if (weaponManagement.weaponType == "ion")
            {
                weaponType = 1;
            }
            else if (weaponManagement.weaponType == "plasma")
            {
                weaponType = 2;
            }
        }

        return weaponType;
    }

    //This get's the weapon mode
    public static int GetWeaponMode(WeaponManagement weaponManagement)
    {
        int weaponMode = 0;

        if (weaponManagement != null)
        {
            if (weaponManagement.weaponMode == "single")
            {
                weaponMode = 0;
            }
            if (weaponManagement.weaponMode == "dual")
            {
                weaponMode = 1;
            }
            if (weaponManagement.weaponMode == "all")
            {
                weaponMode = 2;
            }
            else if (weaponManagement.weaponMode == "rapid")
            {
                weaponMode = 3;
            }
        }

        return weaponMode;
    }

}
