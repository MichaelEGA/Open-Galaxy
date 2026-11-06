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

    //This displays the front radar
    public static void DisplayFrontRadar(Hud_New hud)
    {
        CanvasGroup radar = FindCanvasGroup("Radar");

        if (hud.scene == null)
        {
            hud.scene = SceneFunctions.GetScene();
        }
        
        if (radar != null)
        {
            if (Time.timeScale != 0)
            {
                GameObject playerTarget = null;

                if (hud.mainShip != null)
                {
                    playerTarget = hud.mainShip.targetingSystem.target;
                }
                
                Vector2 radarPosition = new Vector2();
                Vector2 radarDirection = new Vector2();
                float yPositionRadar;
                float xPositionRadar;
                float forwardRadar;
                int radarNumber = 0;

                float radarRadius = ((radar.GetComponent<RectTransform>().rect.width / 2f) / 100f) * 90f;

                if (hud.radarTarget == null)
                {
                    hud.radarTarget = GameObject.Find("RadarTarget");
                    hud.radarTarget.SetActive(false);
                }

                if (hud.radarTargets == null)
                {
                    hud.radarTargets = new List<GameObject>();
                }

                if (hud.radarTargetSelected == null)
                {
                    hud.radarTargetSelected = GameObject.Find("RadarTargetSelected");
                }

                if (hud.radarTargetSelected != null & hud.scene.objectPool != null & hud.radarTargets != null)
                {
                    //This generates the radar icons
                    if (hud.radarTargets.Count < hud.scene.objectPool.Count)
                    {
                        var clone = GameObject.Instantiate(hud.radarTarget) as GameObject;
                        clone.transform.SetParent(radar.transform);
                        clone.transform.localScale = Vector3.one;
                        hud.radarTargets.Add(clone);
                    }
                    else if (hud.radarTargets.Count > hud.scene.objectPool.Count)
                    {
                        //This clears the radar dots when there are too many
                        foreach (GameObject radarDotTemp in hud.radarTargets)
                        {
                            GameObject.Destroy(radarDotTemp);
                        }

                        hud.radarTargets.Clear();
                    }

                    //This cycles through all the ships in the list
                    foreach (GameObject ship in hud.scene.objectPool)
                    {
                        if (ship != null)
                        {
                            GameObject mainShip = hud.scene.mainShip;

                            if (mainShip != null)
                            {
                                //This gets the vector3 of the target
                                Vector3 targetPositionRadar = ship.transform.position - mainShip.transform.position;

                                //This determines the objects relative position and size
                                yPositionRadar = Vector3.Dot(mainShip.transform.up, targetPositionRadar.normalized);
                                xPositionRadar = Vector3.Dot(mainShip.transform.right, targetPositionRadar.normalized);
                                forwardRadar = Vector3.Dot(mainShip.transform.forward, targetPositionRadar.normalized);

                                if (forwardRadar > 0)
                                {
                                    radarPosition = new Vector2(xPositionRadar * radarRadius, yPositionRadar * radarRadius);
                                }
                                else
                                {
                                    radarDirection = new Vector2(xPositionRadar, yPositionRadar) - new Vector2(0, 0);
                                    radarDirection = radarDirection.normalized * radarRadius;
                                    radarPosition = new Vector2(0, 0) + radarDirection;
                                }

                                //Activates the dots
                                if (radarNumber <= hud.radarTargets.Count - 1)
                                {
                                    if (ship.activeSelf == false)
                                    {
                                        hud.radarTargets[radarNumber].SetActive(false);
                                    }
                                    else
                                    {
                                        hud.radarTargets[radarNumber].SetActive(true);
                                    }

                                    hud.radarTargets[radarNumber].transform.localPosition = radarPosition;

                                    //This activates the target braces
                                    if (playerTarget != null)
                                    {
                                        if (ship == hud.mainShip.targetingSystem.target)
                                        {
                                            hud.radarTargetSelected.SetActive(true);
                                            hud.radarTargetSelected.transform.SetParent(radar.transform);
                                            hud.radarTargetSelected.transform.localPosition = radarPosition;
                                        }
                                    }
                                    else
                                    {
                                        hud.radarTargetSelected.SetActive(false);
                                    }

                                }

                                //This moves onto the next radar dot
                                if (radarNumber >= hud.scene.objectPool.Count - 1)
                                {
                                    radarNumber = 0;
                                }
                                else
                                {
                                    radarNumber = radarNumber + 1;
                                }
                            }
                        }
                    }
                }
            }
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
