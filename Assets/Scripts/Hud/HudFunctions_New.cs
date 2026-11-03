using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public static class HudFunctions_New
{

    #region hud start functions

    //This finds the main ship reference
    public static void FindMainShip(Hud_New hud_New)
    {
        Scene scene = SceneFunctions.GetScene();

        if (scene != null)
        {
            if (scene.mainShip != null)
            {
                hud_New.mainShip = scene.mainShip.GetComponent<Ship>();
            }
        }
    }

    #endregion

    #region hud main functions

    //This displays ship info on the hud
    public static void DisplayShipInfo(Hud_New hud_New)
    {
        Text shipInfoText = FindText("ShipInfo");
        Ship mainShip = hud_New.mainShip;

        if (shipInfoText != null & mainShip != null)
        {
            string speed = (mainShip.flightControlSystem_Small.shipRigidbody.linearVelocity.magnitude * 3.6f).ToString("000");

            string throttle = ((int)((mainShip.flightControlSystem_Small.thrustSpeed / mainShip.speedRating) * 100)).ToString("000");

            Debug.Log(" thrust speed is " + mainShip.flightControlSystem_Small.thrustSpeed + " speed rating is " + mainShip.speedRating + " and thrust is " + throttle);

            string weapponType = mainShip.weaponManagement.weaponType.ToString();
            
            if (weapponType == "lasers")
            {
                weapponType = "LSR";
            }

            if (weapponType == "ion")
            {
                weapponType = "ION";
            }

            if (weapponType == "torpedos")
            {
                weapponType = "TRP";
            }

            if (weapponType == "plasma")
            {
                weapponType = "PLS";
            }

            string weaponMode = mainShip.weaponManagement.weaponMode.ToString();
            
            if (weaponMode == "single")
            {
                weaponMode = "SNG";
            }

            if (weaponMode == "dual")
            {
                weaponMode = "DL";
            }

            if (weaponMode == "all")
            {
                weaponMode = "ALL";
            }

            string weaponNum = "INF";

            if (mainShip.weaponManagement.weaponType == "torpedoes")
            {
                weaponNum = mainShip.weaponManagement.torpedoTubes.torpedoNumber.ToString();
            }

            string matchSpeed = "OFF";

            if (mainShip.flightControlSystem_Small.matchSpeed == true)
            {
                matchSpeed = "ON";
            }

            string wep = "OFF";

            if (mainShip.flightControlSystem_Small.wep == true)
            {
                wep = "ON";
            }

            shipInfoText.text =
                "SPD:" + speed +
                "\nTHR:" + throttle +
                "\nMSP:" + matchSpeed +
                "\nWEP:" + wep +
                "\nWPN:" + weapponType +
                "\nMDE:" + weaponMode +
                "\nNUM:" + weaponNum +
                "\nHULL:" + ((int)mainShip.energyManagementSystem.hullLevel).ToString().ToUpper() +
                "\nSHD:" + ((int)mainShip.energyManagementSystem.shieldLevel).ToString().ToUpper();
        }
    }

    #endregion

    #region utils

    //This uses a string to find a specific text box
    public static Text FindText(string textBoxName)
    {
        Hud_New hud_New = GameObject.FindFirstObjectByType<Hud_New>();

        Text text = null;

        if (hud_New != null)
        {
            if (hud_New.textBoxes == null)
            {
                hud_New.textBoxes = new List<Text>();

                Text[] allText = hud_New.transform.GetComponentsInChildren<Text>();

                foreach (Text tempText in allText)
                {
                    hud_New.textBoxes.Add(tempText);
                }
            }

            if (hud_New.textBoxes != null)
            {
                foreach (Text tempText in hud_New.textBoxes.ToArray())
                {
                    if (tempText.name.Contains(textBoxName))
                    {
                        text = tempText;
                        break;
                    }
                }
            }
        }

        return text;
    }

    //This uses a string to find a specific canvas group
    public static CanvasGroup FindCanvasGroup(string canvasGroupName)
    {
        Hud_New hud_New = GameObject.FindFirstObjectByType<Hud_New>();

        CanvasGroup canvasGroup = null;

        if (hud_New != null)
        {
            if (hud_New.canvasGroups == null)
            {
                hud_New.canvasGroups = new List<CanvasGroup>();

                CanvasGroup[] allCanvasGroups = hud_New.transform.GetComponentsInChildren<CanvasGroup>();

                foreach (CanvasGroup tempCanvasGroup in allCanvasGroups)
                {
                    hud_New.canvasGroups.Add(tempCanvasGroup);
                }
            }

            if (hud_New.canvasGroups != null)
            { 
                foreach(CanvasGroup tempCanvasGroup in hud_New.canvasGroups.ToArray())
                {
                    if (tempCanvasGroup.name.Contains(canvasGroupName))
                    {
                        canvasGroup = tempCanvasGroup;
                        break;
                    }
                }
            }
        }

        return canvasGroup;
    }

    #endregion
}
