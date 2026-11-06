using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class HudFunctions
{

    #region hud start functions

    //This creates the hud
    public static void CreateHud()
    {
        GameObject hudPrefab = Resources.Load(OGGetAddress.hud + "Hud") as GameObject;
        GameObject hudGO = GameObject.Instantiate(hudPrefab);

        hudGO.name = "Hud";
        Hud hudScript = hudGO.AddComponent<Hud>();
    }

    //This finds the main ship reference
    public static void FindMainShip(Hud hud)
    {
        if (hud.scene == null)
        {
            hud.scene = SceneFunctions.GetScene();
        }

        if (hud.scene != null)
        {
            if (hud.scene.mainShip != null)
            {
                hud.mainShip = hud.scene.mainShip.GetComponent<Ship>();
            }
        }
    }

    #endregion

    #region hud main functions

    //This displays ship info on the hud
    public static void DisplayShipInfo(Hud hud)
    {
        Text shipInfoText = FindText("ShipInfo");
        Ship mainShip = hud.mainShip;

        if (shipInfoText != null & mainShip != null)
        {
            if (mainShip.flightControlSystem_Small.shipRigidbody != null)
            {
                string speed = (mainShip.flightControlSystem_Small.shipRigidbody.linearVelocity.magnitude * 3.6f).ToString("000");

                string throttle = ((int)((mainShip.flightControlSystem_Small.thrustSpeed / mainShip.speedRating) * 100)).ToString("000");

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
    }

    //This displays the front radar
    public static void DisplayFrontRadar(Hud hud)
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

    //This displays the ship meters
    public static void DisplayEnergyManagementMeters(Hud hud)
    {
        Slider shieldSlider = FindSlider("ShieldPower");
        Slider engineSlider = FindSlider("EnginePower");
        Slider laserSlider = FindSlider("LaserPower");
        Slider WEPSlider = FindSlider("WEPPower");
        Slider laserChargeSlider = FindSlider("LaserCharge");

        if (shieldSlider != null & hud.mainShip != null & Time.timeScale != 0)
        {
            shieldSlider.value = hud.mainShip.energyManagementSystem.shieldPower;
        }

        if (engineSlider != null & hud.mainShip != null & Time.timeScale != 0)
        {
            engineSlider.value = hud.mainShip.energyManagementSystem.enginePower;
        }

        if (laserSlider != null & hud.mainShip != null & Time.timeScale != 0)
        {
            laserSlider.value = hud.mainShip.energyManagementSystem.energyWeaponPower;
        }

        if (WEPSlider != null & hud.mainShip != null & Time.timeScale != 0)
        {
            WEPSlider.value = hud.mainShip.energyManagementSystem.wepLevel;
        }

        if (laserChargeSlider != null & hud.mainShip != null & Time.timeScale != 0)
        {
            laserChargeSlider.value = hud.mainShip.energyManagementSystem.energyWeaponCharge;
        }
    }

    //This displays all possible tagets on the heads up display
    public static void DisplayTargets(Hud hud)
    {
        if (hud.scene == null)
        {
            hud.scene = SceneFunctions.GetScene();
        }

        if (hud.mainCamera == null)
        {
            OGCamera ogCamera = OGCameraFunctions.GetOGCamera();

            if (ogCamera != null)
            {
                hud.mainCamera = ogCamera.mainCamera.GetComponent<Camera>();
            }           
        }

        if (Time.timeScale != 0)
        {
            GameObject playerTarget = null;

            if (hud.mainShip != null)
            {
                playerTarget = hud.mainShip.targetingSystem.target;
            }

            int targetNumber = 0;

            if (hud.target == null)
            {
                hud.target = GameObject.Find("Target");
                hud.target.SetActive(false);
            }

            if (hud.targets == null)
            {
                hud.targets = new List<GameObject>();
            }

            if (hud.targetSelected == null)
            {
                hud.targetSelected = GameObject.Find("TargetSelected");
            }

            if (hud.targetSelected != null & hud.scene.objectPool != null & hud.targets != null)
            {
                //This generates the radar icons
                if (hud.radarTargets.Count <= hud.scene.objectPool.Count)
                {
                    var clone = GameObject.Instantiate(hud.target) as GameObject;
                    clone.transform.SetParent(hud.transform);
                    clone.transform.localScale = Vector3.one;
                    hud.targets.Add(clone);
                }
                else if (hud.targets.Count > hud.scene.objectPool.Count)
                {
                    //This clears the radar dots when there are too many
                    foreach (GameObject targetTemp in hud.targets)
                    {
                        GameObject.Destroy(targetTemp);
                    }

                    hud.targets.Clear();
                }

                //This cycles through all the ships in the list
                foreach (GameObject target in hud.scene.objectPool)
                {
                    if (target != null)
                    {
                        GameObject mainShip = hud.scene.mainShip;

                        if (mainShip != null)
                        {
                            //This gets the vector3 of the target
                            Vector3 targetPositionRadar = target.transform.position - mainShip.transform.position;

                            Vector3 screenPosition = hud.mainCamera.WorldToScreenPoint(target.transform.position);

                            bool isVisible = IsInCameraView(hud.mainCamera, target.GetComponentInChildren<Renderer>());
                               
                            //Activates the dots
                            if (targetNumber < hud.targets.Count)
                            {
                                if (target.activeSelf == false || isVisible == false)
                                {
                                    hud.targets[targetNumber].SetActive(false);
                                }
                                else
                                {
                                    hud.targets[targetNumber].SetActive(true);
                                }

                                hud.targets[targetNumber].transform.position = new Vector2(screenPosition.x, screenPosition.y);

                                //This activates the target braces
                                if (playerTarget != null)
                                {
                                    if (target == hud.mainShip.targetingSystem.target & isVisible == true)
                                    {
                                        hud.targetSelected.SetActive(true);
                                        hud.targetSelected.transform.SetParent(hud.transform);
                                        hud.targetSelected.transform.position = new Vector2(screenPosition.x, screenPosition.y);
                                    }
                                    else
                                    {
                                        hud.targetSelected.SetActive(false);
                                    }
                                }
                                else
                                {
                                    hud.targetSelected.SetActive(false);
                                }

                            }

                            //This moves onto the next radar dot
                            if (targetNumber >= hud.scene.objectPool.Count)
                            {
                                targetNumber = 0;
                            }
                            else
                            {
                                targetNumber = targetNumber + 1;
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
        Hud hud = GameObject.FindFirstObjectByType<Hud>();

        Text text = null;

        if (hud != null)
        {
            if (hud.textBoxes == null)
            {
                hud.textBoxes = new List<Text>();

                Text[] allText = hud.transform.GetComponentsInChildren<Text>();

                foreach (Text tempText in allText)
                {
                    hud.textBoxes.Add(tempText);
                }
            }

            if (hud.textBoxes != null)
            {
                foreach (Text tempText in hud.textBoxes.ToArray())
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
        Hud hud = GameObject.FindFirstObjectByType<Hud>();

        CanvasGroup canvasGroup = null;

        if (hud != null)
        {
            if (hud.canvasGroups == null)
            {
                hud.canvasGroups = new List<CanvasGroup>();

                CanvasGroup[] allCanvasGroups = hud.transform.GetComponentsInChildren<CanvasGroup>();

                foreach (CanvasGroup tempCanvasGroup in allCanvasGroups)
                {
                    hud.canvasGroups.Add(tempCanvasGroup);
                }
            }

            if (hud.canvasGroups != null)
            { 
                foreach(CanvasGroup tempCanvasGroup in hud.canvasGroups.ToArray())
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

    //This uses a string to find a specific slider
    public static Slider FindSlider(string sliderName)
    {
        Hud hud = GameObject.FindFirstObjectByType<Hud>();

        Slider slider = null;

        if (hud != null)
        {
            if (hud.sliders == null)
            {
                hud.sliders = new List<Slider>();

                Slider[] allSliders = hud.transform.GetComponentsInChildren<Slider>();

                foreach (Slider tempSlider in allSliders)
                {
                    hud.sliders.Add(tempSlider);
                }
            }

            if (hud.sliders != null)
            {
                foreach (Slider tempSlider in hud.sliders.ToArray())
                {
                    if (tempSlider.name.Contains(sliderName))
                    {
                        slider = tempSlider;
                        break;
                    }
                }
            }
        }

        return slider;
    }

    //Checks if an object is in the players view
    public static bool IsInCameraView(Camera cam, Renderer targetRenderer)
    {
        if (targetRenderer == null) return false;

        // Calculate the 6 planes that form the camera's viewing frustum
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);

        // Test if the renderer's bounding box is inside those planes
        return GeometryUtility.TestPlanesAABB(planes, targetRenderer.bounds);
    }

    #endregion
}
