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

        LoadRadarCamera();
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

    //This adds information to the ship log
    public static void AddToMissionLog(string message)
    {
        Text missionLog = FindText("MissionLog");

        missionLog.text = message.ToUpper();
    }

    //This adds information to the ship log
    public static void AddToMessageLog(string message)
    {
        Text missionLog = FindText("MessageLog");

        missionLog.text = "<< " + message + " >>";
    }

    //This displays a hint on the screen
    public static void DisplayHint(string hintText)
    {
        Text text = FindText("HintText");

        text.text = hintText;

        CanvasGroup hintCanvas = FindCanvasGroup("Hint");

        Task a = new Task(FadeInAndOutCanvasGroup(hintCanvas, 5));
    }

    //This displays a hint on the screen
    public static void DisplayChoice(string choiceUp, string choiceDown, string choiceRight, string choiceLeft, float displayTime)
    {
        Text text = FindText("HintText");

        text.text = "UP" + choiceUp + "\n" + "DOWN" + choiceDown + "\n" + "RIGHT" + choiceRight + "\n" + "LEFT" + choiceLeft;

        CanvasGroup hintCanvas = FindCanvasGroup("Hint");

        Task a = new Task(FadeInAndOutCanvasGroup(hintCanvas, displayTime));
    }

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
                                if (target.activeSelf == false || isVisible == false || target == playerTarget || target == mainShip.gameObject)
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
                                    if (target == playerTarget & isVisible == true)
                                    {
                                        hud.targetSelected.SetActive(true);
                                        hud.targetSelected.transform.SetParent(hud.transform);
                                        hud.targetSelected.transform.position = new Vector2(screenPosition.x, screenPosition.y);
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
                    else
                    {
                        hud.targets[targetNumber].SetActive(false);
                    }

                    //This disables the selected target
                    if (playerTarget == null)
                    {
                        hud.targetSelected.SetActive(false);
                    }
                    else
                    {
                        bool isVisible = IsInCameraView(hud.mainCamera, playerTarget.GetComponentInChildren<Renderer>());

                        if (isVisible == false)
                        {
                            hud.targetSelected.SetActive(false);
                        }
                    }
                }
            }
        }
    }

    //This displays target information
    public static void DisplayTargetInformation(Hud hud)
    {
        if (hud.mainShip != null)
        {
            Ship targetShip = hud.mainShip.targetingSystem.targetShip;

            //Get references
            Text targetName_text = FindText("RadarShipName");
            Text targetCargo_text = FindText("RadarShipCargo");
            Text targetDistance_text = FindText("TargetDistance");

            Slider targetShield = FindSlider("TargetShield");
            Slider targetHull = FindSlider("TargetHull");
            Slider targetSystems = FindSlider("TargetSystems");

            //Set information
            if (targetShip != null)
            {
                targetName_text.text = targetShip.name.ToUpper();
                targetCargo_text.text = targetShip.cargo.ToUpper();
                targetDistance_text.text = hud.mainShip.targetingSystem.targetDistance.ToString("0000");

                targetShield.maxValue = targetShip.shieldRating;
                targetShield.value = targetShip.energyManagementSystem.shieldLevel;

                targetHull.maxValue = targetShip.hullRating;
                targetHull.value = targetShip.energyManagementSystem.hullLevel;

                targetSystems.maxValue = targetShip.systemsRating;
                targetSystems.value = targetShip.energyManagementSystem.systemsLevel;
            }
            else
            {
                targetName_text.text = "";
                targetCargo_text.text = "";
                targetDistance_text.text = "0000";

                targetShield.value = 0;
                targetHull.value = 0;
                targetSystems.value = 0;
            }
        }

        


    }

    //This displays the intercept point
    public static void DisplayInterceptPoint(Hud hud)
    {
        CanvasGroup interceptPoint = FindCanvasGroup("InterceptPoint");

        if (Time.timeScale != 0)
        {
            GameObject interceptPointGO = interceptPoint.gameObject;

            if (interceptPointGO != null)
            {
                interceptPointGO.SetActive(false);
            }

            if (hud.mainCamera == null)
            {
                OGCamera ogCamera = OGCameraFunctions.GetOGCamera();

                if (ogCamera != null)
                {
                    hud.mainCamera = ogCamera.mainCamera.GetComponent<Camera>();
                }
            }

            if (hud.mainCamera != null)
            {
                if (hud.mainShip != null & interceptPointGO != null)
                {
                    if (hud.mainShip.targetingSystem.target != null)
                    {
                        if (hud.mainShip.targetingSystem.target.activeSelf != false & hud.mainShip.targetingSystem.targetRigidbody != null & hud.mainShip.targetingSystem.targetShip != null)
                        {
                            GameObject target = hud.mainShip.targetingSystem.target;
                            GameObject mainShip = hud.mainShip.gameObject;

                            //This gets the intercept point
                            Vector3 interceptPosition = GameObjectUtils.CalculateInterceptPoint(mainShip.transform.position, target.transform.position, hud.mainShip.targetingSystem.targetRigidbody.linearVelocity, 750);

                            //This gets the targets position on the camera
                            Vector3 screenPosition = hud.mainCamera.WorldToScreenPoint(interceptPosition);

                            //This sets key values
                            Vector3 targetPosition = target.transform.position - mainShip.transform.position;
                            float forward = Vector3.Dot(mainShip.transform.forward, targetPosition.normalized);
                            float up = Vector3.Dot(mainShip.transform.up, targetPosition.normalized);
                            float right = Vector3.Dot(mainShip.transform.right, targetPosition.normalized);

                            //This checks that the target is on screen
                            if (target.GetComponentInChildren<Renderer>().isVisible == true & forward > 0)
                            {
                                //This sets the intercept point to active when the target is on screen
                                interceptPointGO.SetActive(true);

                                //This translates that position to the intercept point
                                interceptPointGO.transform.position = new Vector2(screenPosition.x, screenPosition.y);
                            }
                            else
                            {
                                //This sets the intercept point to inactive when the target is behind the camera
                                interceptPointGO.SetActive(false);
                            }
                        }
                    }
                }
            }
        }
    }

    //This updates the ships objectives
    public static void UpdateObjectives(string[] objectives)
    {
        Hud hud = GetHud();

        Text objectivesText = FindText("Objectives");

        if (hud != null)
        {
            //This loads each of the objectives on its own line in the order they appear on the list
            objectivesText.text = "";

            foreach (string objective in objectives)
            {
                objectivesText.text += objective + "\n";
            }
        }
    }

    #endregion

    #region radar ship preview functions

    //This loads the radar camera
    public static void LoadRadarCamera()
    {
        GameObject radarCamera = new GameObject();
        Camera camera = radarCamera.AddComponent<Camera>();
        camera.orthographic = true;
        camera.orthographicSize = 12.5f;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.cullingMask = (1 << LayerMask.NameToLayer("radar"));
        Object[] renderTexture = Resources.LoadAll(OGGetAddress.hud, typeof(RenderTexture));
        camera.targetTexture = renderTexture[0] as RenderTexture;
        camera.transform.position = new Vector3(0, 0, -50);
        camera.name = "Radar Camera";
    }

    //Display preview of ship
    public static void DisplayShipPreview(Hud hud)
    {
        if (hud.wireframePool == null)
        {
            hud.wireframePool = new List<GameObject>();
        }

        if (hud.wireframePool != null & hud.mainShip != null & Time.timeScale != 0)
        {
            if (hud.mainShip.targetingSystem.target != null)
            {
                if (hud.mainShip.targetingSystem.target.activeSelf != false)
                {
                    if (hud.wireframeObject == null || hud.wireframeObject.activeSelf == false || !hud.wireframeObject.name.Contains(hud.mainShip.targetingSystem.targetPrefabName))
                    {
                        bool foundShip = false;

                        foreach (GameObject radarObject in hud.wireframePool)
                        {
                            if (radarObject.name == hud.mainShip.targetingSystem.targetPrefabName + "(Clone)")
                            {
                                radarObject.SetActive(true);
                                hud.wireframeObject = radarObject;
                                foundShip = true;
                            }
                            else
                            {
                                radarObject.SetActive(false);
                            }
                        }

                        if (foundShip == false)
                        {
                            foreach (Object ship in hud.scene.shipsPrefabPool)
                            {
                                if (ship.name == hud.mainShip.targetingSystem.targetPrefabName)
                                {
                                    //This instantiates the radar object,sets it to the correct position, sets its layer, scales the object and removes any colliders on the object
                                    GameObject radarObject = GameObject.Instantiate(ship) as GameObject;
                                    hud.wireframePool.Add(radarObject);
                                    radarObject.transform.position = new Vector3(0, 0, 0);
                                    radarObject.layer = LayerMask.NameToLayer("radar");
                                    GameObjectUtils.SetLayerAllChildren(radarObject.transform, 24);
                                    radarObject.SetActive(true);
                                    SceneFunctions.ScaleGameObjectByXYZAxis(radarObject, 10);
                                    GameObjectUtils.RemoveColliders(radarObject);

                                    //This checks the wireframe material is loaded
                                    if (hud.wireframeMaterial == null)
                                    {
                                        Material wireframeMaterial = Resources.Load(OGGetAddress.hud + "wireframe_material") as Material;
                                        hud.wireframeMaterial = wireframeMaterial;
                                    }

                                    //This applies the wireframe material to all objects
                                    if (hud.wireframeMaterial != null)
                                    {
                                        GameObjectUtils.ApplyMaterialToAllMeshes(radarObject, hud.wireframeMaterial);
                                    }

                                    //This creates an anchor for all the radar objects
                                    GameObject radarshipdisplayanchor = GameObject.Find("radarshipdisplayanchor");

                                    if (radarshipdisplayanchor == null)
                                    {
                                        radarshipdisplayanchor = new GameObject("radarshipdisplayanchor");
                                    }

                                    radarObject.transform.SetParent(radarshipdisplayanchor.transform);

                                    //This sets the radar object in the hud
                                    hud.wireframeObject = radarObject;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (hud.wireframeObject != null)
                        {
                            Quaternion relativeRotation = Quaternion.Inverse(hud.mainShip.gameObject.transform.rotation) * hud.mainShip.targetingSystem.target.transform.rotation;
                            hud.wireframeObject.transform.rotation = relativeRotation;
                        }
                    }
                }
                else if (hud.wireframeObject != null)
                {
                    hud.wireframeObject.SetActive(false);
                }
            }
            else if (hud.wireframeObject != null)
            {
                hud.wireframeObject.SetActive(false);
            }
        }

    }

    #endregion

    #region other functions

    //This causes the screen to flash
    public static void ScreenFlash()
    {
        Hud hud = HudFunctions.GetHud();

        CanvasGroup screenFlash = FindCanvasGroup("ScreenFlash");

        Task a = new Task(FlashCanvasGroup(screenFlash, 0.025f));
    }

    //This fades in and out a selected canvas group
    public static void FadeInAndOutChosenCanvas(string canvasGroupName, float displayTime = 5, float minAlpha = 0)
    {
        CanvasGroup canvasGroup = FindCanvasGroup(canvasGroupName);

        if (canvasGroup != null)
        {
            Task a = new Task(FadeInAndOutCanvasGroup(canvasGroup, displayTime, minAlpha));
        }
    }

    #endregion

    #region utils

    //This gets the hud script
    public static Hud GetHud()
    {
        Hud hud = GameObject.FindFirstObjectByType<Hud>();

        return hud;
    }

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

    //This fades in the black bars
    public static IEnumerator FadeInCanvasGroup(CanvasGroup canvasGroup, float duration)
    {
        if (canvasGroup != null)
        {
            if (canvasGroup.alpha < 1)
            {
                //This sets the starting alpha value to 0
                float alpha1 = canvasGroup.alpha;

                //This fades in the canvas
                while (alpha1 < 1)
                {
                    alpha1 = alpha1 + (1f / (60f * duration));

                    if (canvasGroup != null)
                    {
                        canvasGroup.alpha = alpha1;
                    }

                    yield return new WaitForSecondsRealtime(0.016f);
                }
            }
        }
    }

    //This fades out the black bars
    public static IEnumerator FadeOutCanvasGroup(CanvasGroup canvasGroup, float duration)
    {
        if (canvasGroup != null)
        {
            if (canvasGroup.alpha > 0)
            {
                //This sets the starting alpha value to 1
                float alpha = canvasGroup.alpha;

                //This fades the canvas out
                while (alpha > 0)
                {
                    alpha = alpha - (1f / (60f * duration));

                    if (canvasGroup != null)
                    {
                        canvasGroup.alpha = alpha;
                    }

                    yield return new WaitForSecondsRealtime(0.016f);
                }
            }
        }
    }

    //This fades out the a canvas group
    public static IEnumerator FadeInAndOutCanvasGroup(CanvasGroup canvasGroup, float duration, float minAlpha = 0)
    {
        if (canvasGroup != null)
        {
            //This sets the starting alpha value to 0
            float alpha = canvasGroup.alpha;

            //This fades in the canvas
            while (alpha < 1)
            {
                alpha = alpha + (1f / (60f * 0.5f));

                if (canvasGroup != null)
                {
                    canvasGroup.alpha = alpha;
                }

                yield return new WaitForSecondsRealtime(0.016f);
            }

            //This sets the starting alpha value to 1
            alpha = canvasGroup.alpha;

            yield return new WaitForSecondsRealtime(duration - 1f);

            //This fades the canvas out
            while (alpha > minAlpha)
            {
                alpha = alpha - (1f / (60f * (0.5f)));

                if (canvasGroup != null)
                {
                    canvasGroup.alpha = alpha;
                }

                yield return new WaitForSecondsRealtime(0.016f);
            }
        }
    }

    //This fades out the a canvas group
    public static IEnumerator FlashCanvasGroup(CanvasGroup canvasGroup, float duration)
    {
        if (canvasGroup != null)
        {
            //This sets the starting alpha value to 0
            float alpha = canvasGroup.alpha;

            //This fades in the canvas
            while (alpha < 1)
            {
                alpha = alpha + (1f / (60f * duration));

                if (canvasGroup != null)
                {
                    canvasGroup.alpha = alpha;
                }

                yield return new WaitForSecondsRealtime(0.016f);
            }

            //This sets the starting alpha value to 1
            alpha = canvasGroup.alpha;

            //This fades the canvas out
            while (alpha > 0)
            {
                alpha = alpha - (1f / (60f * (duration / 2f)));

                if (canvasGroup != null)
                {
                    canvasGroup.alpha = alpha;
                }

                yield return new WaitForSecondsRealtime(0.016f);
            }
        }
    }

    #endregion
}
