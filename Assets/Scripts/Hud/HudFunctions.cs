using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public static class HudFunctions
{
    #region start functions

    //This loads the hud prefab
    public static void CreateHud()
    {
        GameObject hudPrefab = Resources.Load("Hud/Hud") as GameObject;
        GameObject hud = GameObject.Instantiate(hudPrefab);

        hud.name = "Hud";
        Hud hudScript = hud.AddComponent<Hud>();
        hudScript.loadTime = Time.time;

        LoadRadarPrefabs(hudScript);
        InstantiateRadarShips(hudScript);
        SetHudElements(hudScript);
        LoadRadarCamera();     
    }

    //This get the scene
    public static void UpdateKeyReferences(Hud hud)
    {
        if (hud.scene == null) //This gets the reference to the scene
        {
            hud.scene = SceneFunctions.GetScene();
        }
        else if (hud.scene != null)
        {
            if (hud.smallShip == null) //This gets the reference to the main smallship (usually the player)
            {
                if (hud.scene.mainShip != null)
                {
                    hud.smallShip = hud.scene.mainShip.GetComponent<SmallShip>();
                }
            }
            else if (hud.scene.mainShip != null) //This updates the reference to the main smallship if it changes
            {
                if (hud.smallShip.gameObject != hud.scene.mainShip)
                {
                    hud.smallShip = hud.scene.mainShip.GetComponent<SmallShip>();
                }
            }
        }

    }

    //This sets all the hud elements
    public static void SetHudElements(Hud hud)
    {
        if (hud.hudElementsSet == false)
        {
            GameObject hudGameObject = GetHudGameObject();

            GameObject shieldMeter = GameObject.Find("ShieldMeter");
            GameObject engineMeter = GameObject.Find("EngineMeter");
            GameObject laserMeter = GameObject.Find("LaserMeter");
            GameObject WEPMeter = GameObject.Find("WEPMeter");

            if (shieldMeter != null) { hud.shieldMeter = shieldMeter.GetComponent<Slider>(); }
            if (engineMeter != null) { hud.engineMeter = engineMeter.GetComponent<Slider>(); }
            if (laserMeter != null) { hud.laserMeter = laserMeter.GetComponent<Slider>(); }
            if (WEPMeter != null) { hud.WEPMeter = WEPMeter.GetComponent<Slider>(); }

            GameObject shipInfo = GameObject.Find("ShipInfo");
            GameObject shipName = GameObject.Find("ShipName");

            if (shipInfo != null) { hud.shipInfo = shipInfo.GetComponent<Text>(); }
            if (shipName != null) { hud.shipName = shipName.GetComponent<Text>(); }

            GameObject speedText = GameObject.Find("SpeedText");
            GameObject matchSpeedText = GameObject.Find("MatchSpeedText");
            GameObject WEPText = GameObject.Find("WEPText");
            GameObject activeWeaponText = GameObject.Find("ActiveWeaponText");
            GameObject weaponModeText = GameObject.Find("WeaponModeText");
            GameObject weaponNumberText = GameObject.Find("WeaponNumberText");
            GameObject hyperdriveText = GameObject.Find("HyperdriveText");

            if (matchSpeedText != null) { hud.matchSpeedText = matchSpeedText.GetComponent<Text>(); }
            if (speedText != null) { hud.speedText = speedText.GetComponent<Text>(); }
            if (WEPText != null) { hud.WEPText = WEPText.GetComponent<Text>(); }
            if (activeWeaponText != null) { hud.activeWeaponText = activeWeaponText.GetComponent<Text>(); }
            if (weaponModeText != null) { hud.weaponModeText = weaponModeText.GetComponent<Text>(); }
            if (weaponNumberText != null) { hud.weaponNumberText = weaponNumberText.GetComponent<Text>(); }
            if (hyperdriveText != null) { hud.hyperdriveText = hyperdriveText.GetComponent<Text>(); }

            GameObject shieldForwardOutside = GameObject.Find("ShieldForwardOutside");
            GameObject shieldForwardInside = GameObject.Find("ShieldForwardInside");
            GameObject hull = GameObject.Find("Hull");
            GameObject shieldRearInside = GameObject.Find("ShieldRearInside");
            GameObject shieldRearOutside = GameObject.Find("ShieldRearOutside");

            if (shieldForwardOutside != null) { hud.shieldForwardOutside = shieldForwardOutside.GetComponent<RawImage>(); }
            if (shieldForwardInside != null) { hud.shieldForwardInside = shieldForwardInside.GetComponent<RawImage>(); }
            if (hull != null) { hud.hull = hull.GetComponent<RawImage>(); }
            if (shieldRearInside != null) { hud.shieldRearInside = shieldRearInside.GetComponent<RawImage>(); }
            if (shieldRearOutside != null) { hud.shieldRearOutside = shieldRearOutside.GetComponent<RawImage>(); }

            GameObject targetDistanceText = GameObject.Find("TargetDistanceText");
            GameObject targetType = GameObject.Find("TargetType");
            GameObject targetName = GameObject.Find("TargetName");
            GameObject targetCargo = GameObject.Find("TargetCargo");
            GameObject targetSpeedText = GameObject.Find("TargetSpeedText");
            GameObject targetShieldsText = GameObject.Find("TargetShieldsText");
            GameObject targetHullText = GameObject.Find("TargetHullText");

            if (targetDistanceText != null) { hud.targetDistance = targetDistanceText.GetComponent<Text>(); }
            if (targetType != null) { hud.targetType = targetType.GetComponent<Text>(); }
            if (targetName != null) { hud.targetName = targetName.GetComponent<Text>(); }
            if (targetCargo != null) { hud.targetCargo = targetCargo.GetComponent<Text>(); }
            if (targetSpeedText != null) { hud.targetSpeedText = targetSpeedText.GetComponent<Text>(); }
            if (targetShieldsText != null) { hud.targetShieldsText = targetShieldsText.GetComponent<Text>(); }
            if (targetHullText != null) { hud.targetHullText = targetHullText.GetComponent<Text>(); }

            GameObject shipLog = GameObject.Find("ShipLog");

            if (shipLog != null) { hud.shipLog = shipLog.GetComponent<Text>(); }

            GameObject locationInfo = GameObject.Find("LocationInfo");

            if (locationInfo != null) { hud.locationInfo = locationInfo.GetComponent<Text>(); }

            GameObject reticule = GameObject.Find("Reticule");
            GameObject targetLockingReticule = GameObject.Find("TargetLockingReticule");
            GameObject targetLockedReticule = GameObject.Find("TargetLockedReticule");

            if (reticule != null) { hud.reticule = reticule.GetComponent<RawImage>(); }
            if (targetLockingReticule != null) { hud.targetLockingReticule = targetLockingReticule.GetComponent<RawImage>(); }
            if (targetLockedReticule != null) { hud.targetLockedReticule = targetLockedReticule.GetComponent<RawImage>(); }

            hud.frontRadarBrace = GameObject.Find("FrontRadarBrace");
            hud.frontRadarCircle = GameObject.Find("FrontRadarCircle");
            hud.frontRadarDot = GameObject.Find("FrontRadarDot");
            hud.frontRadarDot.SetActive(false);

            hud.rearRadarBrace = GameObject.Find("RearRadarBrace");
            hud.rearRadarCircle = GameObject.Find("RearRadarCircle");
            hud.rearRadarDot = GameObject.Find("RearRadarDot");
            hud.rearRadarDot.SetActive(false);

            hud.selectionBrace = GameObject.Find("SelectionBrace");
            hud.directionArrow = GameObject.Find("DirectionArrow");

            hud.movingReticule = GameObject.Find("MovingReticule");
            hud.centerReticule = GameObject.Find("Reticule");

            hud.hudElementsSet = true;
        }
    }

    //This loads all the models prefabs ready to instantiate
    public static void LoadRadarPrefabs(Hud hud)
    {
        Object[] radarPrefabs = Resources.LoadAll("RadarPrefabs", typeof(GameObject));
        hud.radarPrefabPool = new GameObject[radarPrefabs.Length];
        hud.radarPrefabPool = radarPrefabs;
    }

    //This prepares all the ships to display on radar
    public static void InstantiateRadarShips(Hud hud)
    {
        if (hud.radarPool == null)
        {
            hud.radarPool = new List<GameObject>();
        }

        foreach (GameObject radarPrefab in hud.radarPrefabPool)
        {
            GameObject radarObject = GameObject.Instantiate(radarPrefab);
            hud.radarPool.Add(radarObject);
            radarObject.transform.position = new Vector3(0, 0, 0);
            radarObject.layer = LayerMask.NameToLayer("radar");
            GameObjectUtils.SetLayerAllChildren(radarObject.transform, 24);
            radarObject.SetActive(false);
        }
    }

    //This loads the radar camera
    public static void LoadRadarCamera()
    {
        GameObject radarCamera = new GameObject();
        Camera camera = radarCamera.AddComponent<Camera>();
        camera.orthographic = true;
        camera.orthographicSize = 12.5f;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.cullingMask = (1 << LayerMask.NameToLayer("radar"));
        Object[] renderTexture = Resources.LoadAll("Data/HudAssets", typeof(RenderTexture));
        camera.targetTexture = renderTexture[0] as RenderTexture;
        camera.transform.position = new Vector3(0, 0, -50);
        camera.name = "Radar Camera";
    }

    #endregion

    #region current main ship display

    //Display ship info
    public static void DisplayShipInfo(Hud hud)
    {
        if (hud.shipInfo != null & hud.shipName != null & hud.smallShip != null & Time.timeScale != 0)
        {
            hud.shipInfo.text = "FLYING " + hud.smallShip.allegiance.ToUpper() + " " + hud.smallShip.type.ToUpper();
            hud.shipName.text = hud.smallShip.name.ToUpper();
        }
    }

    //Display preview of ship
    public static void DisplayShipPreview(Hud hud)
    {
        if (hud.radarPool != null & hud.smallShip != null & Time.timeScale != 0)
        {
            if (hud.smallShip.target != null)
            {
                if (hud.smallShip.target.activeSelf != false)
                {
                    if (hud.radarObject == null || hud.radarObject.activeSelf == false)
                    {
                        foreach (GameObject radarObject in hud.radarPool)
                        {
                            if (radarObject.name == hud.smallShip.targetPrefabName + "(Clone)")
                            {
                                radarObject.SetActive(true);
                                hud.radarObject = radarObject;
                            }
                            else
                            {
                                radarObject.SetActive(false);
                            }
                        }
                    }
                    else if (hud.radarObject.name != hud.smallShip.targetPrefabName + "(Clone)")
                    {
                        foreach (GameObject radarObject in hud.radarPool)
                        {

                            if (radarObject.name == hud.smallShip.targetPrefabName + "(Clone)")
                            {
                                radarObject.SetActive(true);
                                hud.radarObject = radarObject;
                            }
                            else
                            {
                                radarObject.SetActive(false);
                            }
                        }

                    }
                    else
                    {
                        if (hud.radarObject != null)
                        {
                            Quaternion relativeRotation = Quaternion.Inverse(hud.smallShip.gameObject.transform.rotation) * hud.smallShip.target.transform.rotation;
                            hud.radarObject.transform.rotation = relativeRotation;
                        }
                    }
                }
                else if (hud.radarObject != null)
                {
                    hud.radarObject.SetActive(false);
                }
            }
            else if (hud.radarObject != null)
            {
                hud.radarObject.SetActive(false);
            }
        }

    }

    //This displays shield meter of the ship
    public static void DisplayShieldMeter(Hud hud)
    {
        if (hud.shieldMeter != null & hud.smallShip != null & Time.timeScale != 0)
        {
            hud.shieldMeter.value = hud.smallShip.shieldPower;
        }
    }

    //This displays engine meter
    public static void DisplayEngineMeter(Hud hud)
    {
        if (hud.engineMeter != null & hud.smallShip != null & Time.timeScale != 0)
        {
            hud.engineMeter.value = hud.smallShip.enginePower;
        }
    }

    //This displays laser meter
    public static void DisplayLaserMeter(Hud hud)
    {
        if (hud.laserMeter != null & hud.smallShip != null & Time.timeScale != 0)
        {
            hud.laserMeter.value = hud.smallShip.laserPower;
        }
    }

    //This displays WEP meter
    public static void DisplayWEPMeter(Hud hud)
    {
        if (hud.WEPMeter != null & hud.smallShip != null & Time.timeScale != 0)
        {
            hud.WEPMeter.value = hud.smallShip.wepLevel;
        }
    }

    //This displays the speed of the ship
    public static void DisplayShipSpeed(Hud hud)
    {
        if (hud.speedText != null & hud.smallShip != null & Time.timeScale != 0)
        {
            if (hud.smallShip.shipRigidbody != null)
            {
                hud.speedText.text = (hud.smallShip.shipRigidbody.velocity.magnitude * 3.6f).ToString("000");
            } 
        }
    }

    //This displays the match speed icon
    public static void DisplayMatchSpeed(Hud hud)
    {
        if (hud.matchSpeedText != null & hud.smallShip != null & Time.timeScale != 0)
        {
            if (hud.smallShip.matchSpeed == true)
            {
                hud.matchSpeedText.text = "M";
            }
            else
            {
                hud.matchSpeedText.text = " ";
            }
        }
    }

    //This displays the shield and hull meter
    public static void DisplayShieldAndHull(Hud hud)
    {
        if (hud.shieldForwardOutside != null & hud.shieldForwardInside != null & hud.hull != null & hud.shieldRearInside != null & hud.shieldRearOutside != null & hud.smallShip != null & Time.timeScale != 0)
        {
            float shieldRating = hud.smallShip.shieldRating / 4f;
            float hullRating = hud.smallShip.hullRating;

            float shieldForwardOutside = ((hud.smallShip.frontShieldLevel - shieldRating) / shieldRating) * 100f;
            float shieldForwardInside = (hud.smallShip.frontShieldLevel / shieldRating) * 100f;
            float hull = (hud.smallShip.hullLevel / hullRating) * 100f;
            float shieldRearInside = (hud.smallShip.rearShieldLevel / shieldRating) * 100f;
            float shieldRearOutside = ((hud.smallShip.rearShieldLevel - shieldRating) / shieldRating) * 100f;

            if (shieldForwardOutside > 0)
            {
                ChangeImageAlpha(hud.shieldForwardOutside, shieldForwardOutside);
                ChangeImageAlpha(hud.shieldForwardInside, 100f);
            }
            else
            {
                ChangeImageAlpha(hud.shieldForwardOutside, 0);
                ChangeImageAlpha(hud.shieldForwardInside, shieldForwardInside);
            }

            ChangeImageAlpha(hud.hull, hull);

            if (shieldRearOutside > 0)
            {
                ChangeImageAlpha(hud.shieldRearOutside, shieldRearOutside);
                ChangeImageAlpha(hud.shieldRearInside, 100f);
            }
            else
            {
                ChangeImageAlpha(hud.shieldRearOutside, 0);
                ChangeImageAlpha(hud.shieldRearInside, shieldRearInside);
            }


        }
    }

    //This displays the speed of the ship
    public static void DisplayActiveWeapon(Hud hud)
    {
        if (hud.activeWeaponText != null & hud.smallShip != null & Time.timeScale != 0)
        {
            if (hud.smallShip.activeWeapon == "lasers")
            {
                hud.activeWeaponText.text = "LSR";
            }
            else if (hud.smallShip.activeWeapon == "torpedos")
            {
                hud.activeWeaponText.text = "TRP";
            }
            else
            {
                hud.activeWeaponText.text = " ";
            }
        }
    }

    //This displays the speed of the ship
    public static void DisplayWeaponMode(Hud hud)
    {
        if (hud.weaponModeText != null & hud.smallShip != null & Time.timeScale != 0)
        {
            if (hud.smallShip.weaponMode == "single")
            {
                hud.weaponModeText.text = "SNG";
            }
            else if (hud.smallShip.weaponMode == "dual")
            {
                hud.weaponModeText.text = "DUL";
            }
            else if (hud.smallShip.weaponMode == "quad")
            {
                hud.weaponModeText.text = "QUD";
            }
            else
            {
                hud.weaponModeText.text = "";
            }
        }
    }

    //This displays the speed of the ship
    public static void DisplayWeaponNumber(Hud hud)
    {
        if (hud.weaponNumberText != null & hud.smallShip != null & Time.timeScale != 0)
        {
            if (hud.smallShip.activeWeapon == "lasers")
            {
                hud.weaponNumberText.text = "---";
            }
            else
            {
                hud.weaponNumberText.text = hud.smallShip.torpedoNumber.ToString();
            }
        }
    }

    //This displays whether the WEP boost is on or not
    public static void DisplayWEP(Hud hud)
    {
        if (hud.WEPText != null & hud.smallShip != null & Time.timeScale != 0)
        {
            if (hud.smallShip.wep == true)
            {
                hud.WEPText.text = "ON";
            }
            else
            {
                hud.WEPText.text = "OFF";
            }
        }
    }

    #endregion

    #region target ship display

    //Display target speed
    public static void DisplayTargetDistance(Hud hud)
    {
        if (hud.targetDistance != null & hud.smallShip != null & Time.timeScale != 0)
        {
            if (hud.smallShip.target != null)
            {
                hud.targetDistance.text = (hud.smallShip.targetDistance / 1000f).ToString("0.000");
            }
            else
            {
                hud.targetDistance.text = " ";
            }
        }
    }

    //Display target speed
    public static void DisplayTargetType(Hud hud)
    {
        if (hud.targetType != null & hud.smallShip != null & Time.timeScale != 0)
        {
            if (hud.smallShip.target != null & hud.smallShip.targetAllegiance != null & hud.smallShip.targetType != null) 
            {
                hud.targetType.text = hud.smallShip.targetAllegiance.ToUpper() + " " + hud.smallShip.targetType.ToUpper();
            }
            else
            {
                hud.targetType.text = " ";
            }
        }
    }

    //Display target name
    public static void DisplayTargetName(Hud hud)
    {
        if (hud.targetName != null & hud.smallShip != null & Time.timeScale != 0)
        {
            if (hud.smallShip.target != null)
            {
                hud.targetName.text = hud.smallShip.targetName.ToUpper();
            }
            else
            {
                hud.targetName.text = " ";
            }
        }
    }

    //Display target speed
    public static void DisplayTargetSpeed(Hud hud)
    {
        if (hud.targetSpeedText != null & hud.smallShip != null & Time.timeScale != 0)
        {
            if (hud.smallShip.target != null)
            {
                if (hud.smallShip.targetSmallShip != null)
                {
                    if (hud.smallShip.targetSmallShip.shipRigidbody != null)
                    {
                        float speed = hud.smallShip.targetSmallShip.shipRigidbody.velocity.magnitude * 3.6f;

                        if (speed > hud.speed)
                        {
                            hud.speed += 1;
                        }
                        else if (speed < hud.speed)
                        {
                            hud.speed -= 1;
                        }

                        hud.targetSpeedText.text = hud.speed.ToString("000");
                    }
                }
                else if (hud.smallShip.targetLargeShip != null)
                {
                    Vector3 newPosition = hud.smallShip.targetLargeShip.transform.position;
                    var media = (newPosition - hud.lastPosition);
                    Vector3 velocity = media / Time.deltaTime;
                    hud.lastPosition = newPosition;
                    float speed = velocity.magnitude * 3.6f;

                    if (speed > hud.speed)
                    {
                        hud.speed += 1;
                    }
                    else if (speed < hud.speed)
                    {
                        hud.speed -= 1;
                    }

                    hud.targetSpeedText.text = hud.speed.ToString("000");
                }
            }
            else
            {
                hud.targetSpeedText.text = " ";
            }            
        }
    }

    //Display target speed
    public static void DisplayTargetShield(Hud hud)
    {
        if (hud.targetShieldsText != null & hud.smallShip != null & Time.timeScale != 0)
        {
            if (hud.smallShip.target != null)
            {
                hud.targetShieldsText.text = hud.smallShip.targetShield.ToString("000");
            }
            else
            {
                hud.targetShieldsText.text = " ";
            }
        }
    }

    //Display target speed
    public static void DisplayTargetHull(Hud hud)
    {
        if (hud.targetHullText != null & hud.smallShip != null & Time.timeScale != 0)
        {
            if (hud.smallShip.target != null)
            {
                hud.targetHullText.text = hud.smallShip.targetHull.ToString("000");
            }
            else
            {
                hud.targetHullText.text = " ";
            }
        }
    }

    //Display the target cargo
    public static void DisplayTargetCargo(Hud hud)
    {
        if (hud.targetCargo != null & hud.smallShip != null)
        {
            if (hud.smallShip.target != null)
            {
                if (hud.smallShip.targetSmallShip != null)
                {
                    if (hud.smallShip.targetSmallShip.scanned == false & hud.smallShip.targetDistance > 200)
                    {
                        hud.targetCargo.text = "-";
                    }
                    else if (hud.smallShip.targetSmallShip.scanned == false & hud.smallShip.targetDistance < 200)
                    {
                        hud.targetCargo.text = hud.smallShip.targetSmallShip.cargo.ToUpper();
                        hud.smallShip.targetSmallShip.scanned = true;
                        AddToShipLog(hud.smallShip.targetName.ToUpper() + " has been scanned: " + hud.smallShip.targetSmallShip.cargo.ToUpper());
                    }
                    else if (hud.smallShip.targetSmallShip.scanned == true)
                    {
                        hud.targetCargo.text = hud.smallShip.targetSmallShip.cargo.ToUpper();
                    }
                }
                else if (hud.smallShip.targetLargeShip != null)
                {
                    if (hud.smallShip.targetLargeShip.scanned == false & hud.smallShip.targetDistance > 300)
                    {
                        hud.targetCargo.text = "-";
                    }
                    else if (hud.smallShip.targetLargeShip.scanned == false & hud.smallShip.targetDistance < 300)
                    {
                        hud.targetCargo.text = hud.smallShip.targetLargeShip.cargo.ToUpper();
                        hud.smallShip.targetLargeShip.scanned = true;
                        AddToShipLog(hud.smallShip.targetName.ToUpper() + " has been scanned: " + hud.smallShip.targetLargeShip.cargo.ToUpper());
                    }
                    else if (hud.smallShip.targetLargeShip.scanned == true)
                    {
                        hud.targetCargo.text = hud.smallShip.targetLargeShip.cargo.ToUpper();
                    }
                }
            }
            else
            {
                hud.targetCargo.text = "-";
            }
        }
    }

    #endregion

    #region ship radar display

    //This displays active ships in the area on the front Radar
    public static void DisplayFrontRadar(Hud hud)
    {
        if (Time.timeScale != 0)
        {
            GameObject playerTarget = null;

            if (hud.smallShip != null)
            {
                playerTarget = hud.smallShip.target;
            }
            Vector2 radarPosition = new Vector2();
            Vector2 radarDirection = new Vector2();
            float yPositionRadar;
            float xPositionRadar;
            float forwardRadar;
            int radarNumber = 0;

            float radarRadius = ((hud.frontRadarCircle.GetComponent<RectTransform>().rect.width / 2f) / 100f) * 90f;

            if (hud.frontRadarDotsPool == null)
            {
                hud.frontRadarDotsPool = new List<GameObject>();
            }

            if (hud.frontRadarDot != null & hud.scene.objectPool != null & hud.frontRadarDotsPool != null)
            {
                //This generates the radar icons
                if (hud.frontRadarDotsPool.Count < hud.scene.objectPool.Count)
                {
                    var clone = GameObject.Instantiate(hud.frontRadarDot) as GameObject;
                    clone.transform.SetParent(hud.frontRadarCircle.transform);
                    hud.frontRadarDotsPool.Add(clone);
                }
                else if (hud.frontRadarDotsPool.Count > hud.scene.objectPool.Count)
                {
                    //This clears the radar dots when there are too many
                    foreach (GameObject radarDotTemp in hud.frontRadarDotsPool)
                    {
                        GameObject.Destroy(radarDotTemp);
                    }

                    hud.frontRadarDotsPool.Clear();
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

                            //This displays the radar dot in the correct relative position

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
                            if (radarNumber <= hud.frontRadarDotsPool.Count - 1)
                            {

                                if (ship.activeSelf == false)
                                {
                                    hud.frontRadarDotsPool[radarNumber].SetActive(false);
                                }
                                else
                                {
                                    hud.frontRadarDotsPool[radarNumber].SetActive(true);
                                }

                                hud.frontRadarDotsPool[radarNumber].transform.localPosition = radarPosition;

                                //This activates the target braces
                                if (playerTarget != null)
                                {
                                    if (ship == hud.smallShip.target)
                                    {
                                        hud.frontRadarBrace.SetActive(true);
                                        hud.frontRadarBrace.transform.SetParent(hud.frontRadarCircle.transform);
                                        hud.frontRadarBrace.transform.localPosition = radarPosition;
                                    }
                                }
                                else
                                {
                                    hud.frontRadarBrace.SetActive(false);
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

    //This displays active ships in the area on the rear Radar
    public static void DisplayRearRadar(Hud hud)
    {
        if (Time.timeScale != 0)
        {
            GameObject playerTarget = null;

            if (hud.smallShip != null)
            {
                playerTarget = hud.smallShip.target;
            }
            Vector2 radarPosition = new Vector2();
            Vector2 radarDirection = new Vector2();
            float yPositionRadar;
            float xPositionRadar;
            float rearRadar;
            int radarNumber = 0;

            float radarRadius = ((hud.rearRadarCircle.GetComponent<RectTransform>().rect.width / 2f) / 100f) * 90f;

            if (hud.rearRadarDotsPool == null)
            {
                hud.rearRadarDotsPool = new List<GameObject>();
            }

            if (hud.rearRadarDot != null & hud.scene.objectPool != null & hud.rearRadarDotsPool != null)
            {
                //This generates the radar icons
                if (hud.rearRadarDotsPool.Count < hud.scene.objectPool.Count)
                {
                    var clone = GameObject.Instantiate(hud.rearRadarDot) as GameObject;
                    clone.transform.SetParent(hud.rearRadarCircle.transform);
                    hud.rearRadarDotsPool.Add(clone);
                }
                else if (hud.rearRadarDotsPool.Count > hud.scene.objectPool.Count)
                {
                    //This clears the radar dots when there are too many
                    foreach (GameObject radarDotTemp in hud.rearRadarDotsPool)
                    {
                        GameObject.Destroy(radarDotTemp);
                    }

                    hud.rearRadarDotsPool.Clear();
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

                            //This displays the radar dot in the correct relative position

                            //This determines the objects relative position and size
                            yPositionRadar = Vector3.Dot(mainShip.transform.up, targetPositionRadar.normalized);
                            xPositionRadar = Vector3.Dot(mainShip.transform.right, targetPositionRadar.normalized);
                            rearRadar = -Vector3.Dot(mainShip.transform.forward, targetPositionRadar.normalized);

                            if (rearRadar > 0)
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
                            if (radarNumber <= hud.rearRadarDotsPool.Count - 1)
                            {

                                if (ship.activeSelf == false)
                                {
                                    hud.rearRadarDotsPool[radarNumber].SetActive(false);
                                }
                                else
                                {
                                    hud.rearRadarDotsPool[radarNumber].SetActive(true);
                                }

                                hud.rearRadarDotsPool[radarNumber].transform.localPosition = radarPosition;

                                //This activates the target braces
                                if (playerTarget != null)
                                {
                                    if (ship == hud.smallShip.target)
                                    {
                                        hud.rearRadarBrace.SetActive(true);
                                        hud.rearRadarBrace.transform.SetParent(hud.rearRadarCircle.transform);
                                        hud.rearRadarBrace.transform.localPosition = radarPosition;
                                    }
                                }
                                else
                                {
                                    hud.rearRadarBrace.SetActive(false);
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

    //This displays a brace around the target when onscreen and an arrow pointing to the target when offscreen
    public static void DisplaySelectionBraces(Hud hud)
    {
        if (Time.timeScale != 0)
        {
            GameObject selectionBrace = hud.selectionBrace;
            GameObject directionArrow = hud.directionArrow;

            if (selectionBrace != null & directionArrow != null)
            {
                selectionBrace.SetActive(false);
                directionArrow.SetActive(false);
            }

            if (hud.mainCamera == null)
            {
                if (hud.smallShip != null)
                {
                    if (hud.smallShip.mainCamera != null)
                    {
                        hud.mainCamera = hud.smallShip.mainCamera.GetComponent<Camera>();
                    }
                }
            }

            if (hud.mainCamera != null)
            {
                if (hud.smallShip != null & selectionBrace != null & directionArrow != null)
                {
                    if (hud.smallShip.target != null)
                    {
                        if (hud.smallShip.target.activeSelf != false)
                        {
                            GameObject target = hud.smallShip.target;
                            GameObject mainShip = hud.smallShip.gameObject;

                            //This gets the targets position on the camera
                            Vector3 screenPosition = hud.mainCamera.WorldToScreenPoint(target.transform.position);

                            //This sets key values
                            Vector3 targetPosition = target.transform.position - mainShip.transform.position;
                            float forward = Vector3.Dot(mainShip.transform.forward, targetPosition.normalized);
                            float up = Vector3.Dot(mainShip.transform.up, targetPosition.normalized);
                            float right = Vector3.Dot(mainShip.transform.right, targetPosition.normalized);

                            //This checks that the target is on screen
                            if (target.GetComponentInChildren<Renderer>().isVisible == true & forward > 0)
                            {
                                //This sets the braces to active when the target is on screen
                                selectionBrace.SetActive(true);
                                directionArrow.SetActive(false);

                                //This translates that position to the selection brace
                                selectionBrace.transform.position = new Vector2(screenPosition.x, screenPosition.y);

                                //This scales the brace according to world distance
                                if (screenPosition.z > 500)
                                {
                                    selectionBrace.transform.localScale = new Vector2(1, 1);
                                }
                                else
                                {
                                    selectionBrace.transform.localScale = new Vector2(2f - (0.002f * screenPosition.z), 2f - (0.002f * screenPosition.z));
                                }

                            }
                            else
                            {
                                //This sets the braces to inactive when the target is behind the camera
                                selectionBrace.SetActive(false);
                                directionArrow.SetActive(true);

                                //This gets values from atlasCommon
                                RectTransform rectTransform = hud.gameObject.GetComponent<RectTransform>();
                                float previousArrowRotation = hud.previousArrowRotation;
                                float arrowTargetRotation = hud.arrowTargetRotation;
                                float arrowLerpTime = hud.arrowLerpTime;

                                //This gets screen width and height values
                                float screenWidth = rectTransform.rect.width;
                                float screenHeight = rectTransform.rect.height;

                                //This controls the arrow rotation
                                arrowLerpTime += 5f * Time.deltaTime;
                                float arrowRotation = Mathf.Lerp(previousArrowRotation, arrowTargetRotation, arrowLerpTime);
                                directionArrow.transform.localRotation = Quaternion.Euler(0, 0, arrowRotation);

                                if (right > 0)
                                {
                                    if (up > 0.5f)
                                    {
                                        directionArrow.transform.localPosition = new Vector2((screenWidth / 2f) * ((0.5f - (up - 0.5f)) * 2f), screenHeight / 2f);

                                        if (arrowTargetRotation != -90)
                                        {
                                            previousArrowRotation = arrowRotation;
                                            arrowTargetRotation = -90;
                                            arrowLerpTime = 0;
                                        }

                                    }

                                    else if (up < 0.5f & up > 0f)
                                    {
                                        directionArrow.transform.localPosition = new Vector2(screenWidth / 2f, (screenHeight / 2f) * up * 2f);

                                        if (arrowTargetRotation != -90)
                                        {
                                            previousArrowRotation = arrowRotation;
                                            arrowTargetRotation = -90;
                                            arrowLerpTime = 0;
                                        }

                                    }

                                    else if (up < 0f & up > -0.5f)
                                    {
                                        directionArrow.transform.localPosition = new Vector2(screenWidth / 2f, (screenHeight / 2f) * up * 2f);

                                        if (arrowTargetRotation != -180)
                                        {
                                            previousArrowRotation = arrowRotation;
                                            arrowTargetRotation = -180;
                                            arrowLerpTime = 0;
                                        }

                                    }

                                    else if (up < -0.5)
                                    {
                                        directionArrow.transform.localPosition = new Vector2((screenWidth / 2f) * ((0.5f + (up + 0.5f)) * 2f), -screenHeight / 2f);

                                        if (arrowTargetRotation != -180)
                                        {
                                            previousArrowRotation = arrowRotation;
                                            arrowTargetRotation = -180;
                                            arrowLerpTime = 0;
                                        }

                                    }

                                }
                                else
                                {
                                    if (up > 0.5f)
                                    {
                                        directionArrow.transform.localPosition = new Vector2((screenWidth / 2f) * ((0.5f - (up - 0.5f)) * -2f), screenHeight / 2f);

                                        if (arrowTargetRotation != 0)
                                        {
                                            previousArrowRotation = arrowRotation;
                                            arrowTargetRotation = 0;
                                            arrowLerpTime = 0;
                                        }

                                    }

                                    else if (up < 0.5f & up > 0f)
                                    {
                                        directionArrow.transform.localPosition = new Vector2(-screenWidth / 2f, (screenHeight / 2f) * up * 2f);

                                        if (arrowTargetRotation != 0)
                                        {
                                            previousArrowRotation = arrowRotation;
                                            arrowTargetRotation = 0;
                                            arrowLerpTime = 0;
                                        }

                                    }

                                    else if (up < 0f & up > -0.5f)
                                    {
                                        directionArrow.transform.localPosition = new Vector2(-screenWidth / 2f, (screenHeight / 2f) * up * 2f);

                                        if (arrowTargetRotation != -270)
                                        {
                                            previousArrowRotation = arrowRotation;
                                            arrowTargetRotation = -270;
                                            arrowLerpTime = 0;
                                        }

                                    }

                                    else if (up < -0.5)
                                    {
                                        directionArrow.transform.localPosition = new Vector2((screenWidth / 2f) * ((0.5f + (up + 0.5f)) * -2f), -screenHeight / 2f);

                                        if (arrowTargetRotation != -270)
                                        {
                                            previousArrowRotation = arrowRotation;
                                            arrowTargetRotation = -270;
                                            arrowLerpTime = 0;
                                        }

                                    }

                                }

                                hud.previousArrowRotation = previousArrowRotation;
                                hud.arrowTargetRotation = arrowTargetRotation;
                                hud.arrowLerpTime = arrowLerpTime;

                            }
                        }
                    }
                }
            }
        }
    }

    //This displays the target lock reticule
    public static void DisplayTargetLockReticule(Hud hud)
    {
        if (hud.targetLockingReticule != null & hud.targetLockedReticule != null & hud.smallShip != null & Time.timeScale != 0)
        {
            if (hud.smallShip.activeWeapon == "torpedos")
            {
                if (hud.smallShip.torpedoLockingOn == true & hud.smallShip.torpedoLockedOn == false)
                {
                    if (hud.reticuleFlashing == false)
                    {
                        Task a = new Task(TurnReticuleOnAndOff(hud, hud.targetLockingReticule));
                    }

                    hud.targetLockedReticule.gameObject.SetActive(false);

                }
                else if (hud.smallShip.torpedoLockedOn == true)
                {
                    hud.targetLockingReticule.gameObject.SetActive(false);
                    hud.targetLockedReticule.gameObject.SetActive(true);
                }
                else
                {
                    hud.targetLockingReticule.gameObject.SetActive(false);
                    hud.targetLockedReticule.gameObject.SetActive(false);
                }
            }
            else
            {
                hud.targetLockingReticule.gameObject.SetActive(false);
                hud.targetLockedReticule.gameObject.SetActive(false);
            }
        }
    }

    //This makes the reticule flash
    public static IEnumerator TurnReticuleOnAndOff(Hud hud, RawImage reticule)
    {
        hud.reticuleFlashing = true;

        reticule.gameObject.SetActive(true);

        AudioFunctions.PlayAudioClip(hud.smallShip.audioManager, "beep02_targetlock", "Cockpit", hud.smallShip.transform.position, 0, 1, 500, 0.6f);

        yield return new WaitForSeconds(0.25f);

        reticule.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.25f);

        hud.reticuleFlashing = false;
    }

    #endregion

    #region ship log

    //This adds a message to the ship log
    public static void AddToShipLog(string message)
    {
        if (Time.timeScale != 0)
        {
            Hud hud = GetHudScript();

            if (hud != null)
            {
                if (hud.shipLog != null)
                {
                    hud.shipLog.text = (Time.time - hud.loadTime).ToString("00:00") + " " + message + "\n" + hud.shipLog.text;
                }
            }
        }
    }

    //This updates the ships objectives
    public static void UpdateObjectives(string[] objectives)
    {
        Hud hud = GetHudScript();

        if (hud != null)
        {
            //This gets the reference to the text asset if it doesn't already exist
            if (hud.objectiveLog == null)
            {
                GameObject objectiveLog = GameObject.Find("ObjectiveLog");

                if (objectiveLog != null) { hud.objectiveLog = objectiveLog.GetComponent<Text>(); }
            }
            
            //This loads each of the objectives on its own line in the order they appear on the list
            if (hud.objectiveLog != null)
            {
                hud.objectiveLog.text = "";

                foreach (string objective in objectives)
                {
                    hud.objectiveLog.text += objective + "\n";
                }
            }
        }      
    }

    #endregion

    #region location display

    //This briefly displays a message in large text in the middle of the screen
    public static void DisplayLargeMessage(string location)
    {
        if (Time.timeScale != 0)
        {
            Hud hud = GetHudScript();

            if (hud != null)
            {
                if (hud.locationInfo != null & hud.reticule != null)
                {
                    hud.locationInfo.text = location;
                    Task a = new Task(FadeTextInAndOut(hud.locationInfo, 0.5f, 3, 0.5f));
                    Task b = new Task(FadeImageOutAndIn(hud.reticule, 0.25f, 4, 0.5f));
                }
            }
        } 
    }

    #endregion

    #region moving reticule

    public static void MoveReticule(Hud hud)
    {
        if (hud.movingReticule != null & hud.centerReticule != null & hud.smallShip != null & Time.timeScale != 0)
        {
            var mouse = Mouse.current;
            float x = mouse.position.x.ReadValue();
            float y = mouse.position.y.ReadValue();
            float radiusWidth = Screen.width / 2;
            float radiusHeight = Screen.height / 2;
            float x2 = 0;
            float y2 = 0;

            hud.movingReticule.transform.position = new Vector2(x, y);

            if (hud.smallShip.invertUpDown == true)
            {
                y2 = Screen.height - y;
            }
            else
            {
                y2 = y;
            }

            if (hud.smallShip.invertLeftRight == true)
            {
                x2 = Screen.width - x;
            }
            else
            {
                x2 = x;
            }

            Vector2 rotationTarget = new Vector2(x2, y2);

            float angle = Mathf.Atan2(hud.centerReticule.transform.position.y - rotationTarget.y, hud.centerReticule.transform.position.x - rotationTarget.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            hud.movingReticule.transform.rotation = Quaternion.RotateTowards(hud.movingReticule.transform.rotation, targetRotation, 1000 * Time.deltaTime);
        }
    }

    #endregion

    #region unload hud and hud assets

    //This calls the unloading function before unloading the hud object itself
    public static void UnloadHud()
    {
        Hud hud = GetHudScript();

        if (hud != null)
        {
            UnloadRadarObjects(hud);
            GameObject.Destroy(hud.gameObject);
        }

        UnloadRadarCamera();
    }

    //This unloads the radar camera
    public static void UnloadRadarCamera()
    {
        GameObject radarCamera = GameObject.Find("Radar Camera");

        if (radarCamera != null)
        {
            GameObject.Destroy(radarCamera);
        }
    }

    //This unloads the radar display objects
    public static void UnloadRadarObjects(Hud hud)
    {
        if (hud.radarPool != null)
        {
            foreach (GameObject radarObject in hud.radarPool)
            {
                GameObject.Destroy(radarObject);
            }

            hud.radarPool.Clear();
        }       
    }

    #endregion

    #region hud utils

    //This changes the alpha of an image
    public static void ChangeImageAlpha(RawImage rawimage, float alphaPercentage)
    {
        //This ensure the input data is in the appropriate range 0-100
        if (alphaPercentage > 100) { alphaPercentage = 100; }
        else if (alphaPercentage < 0) { alphaPercentage = 0; }

        //This applies the alpha to the image
        Color newColor = rawimage.color;
        newColor.a = (1f / 100f) * alphaPercentage;
        rawimage.color = newColor;
    }

    //This gets the Hud gameobject
    public static GameObject GetHudGameObject()
    {
        GameObject hud = GameObject.Find("Hud");
        return hud;
    }

    //This gets the HUD script
    public static Hud GetHudScript()
    {
        Hud hud = GameObject.FindObjectOfType<Hud>();
        return hud;
    }

    //This fades out text
    public static IEnumerator FadeOutText(Text text, float duration)
    {
        float alpha = 1;

        while (alpha > 0)
        {
            alpha = alpha - (1f / (60f * duration));

            if (text != null)
            {       
                Color newColor = text.color;
                newColor.a = alpha;
                text.color = newColor;           
            }

            yield return new WaitForSecondsRealtime(0.016f);
        }

    }

    //This fades in an image
    public static IEnumerator FadeInText(Text text, float duration)
    {
        float alpha = 0;

        while (alpha < 1)
        {
            alpha = alpha + (1f / (60f * duration));

            if (text != null)
            {            
                Color newColor = text.color;
                newColor.a = alpha;
                text.color = newColor;               
            }

            yield return new WaitForSecondsRealtime(0.016f);
        }
    }

    //This fades an image in and out
    public static IEnumerator FadeTextInAndOut(Text text, float fadeintime = 0.5f, float holdtime = 1, float fadeOuttime = 0.5f)
    {
        Task a = new Task(FadeInText(text, fadeintime));
        while (a.Running == true) { yield return null; }
        yield return new WaitForSeconds(holdtime);
        Task b = new Task(FadeOutText(text, fadeOuttime));
        while (b.Running == true) { yield return null; }
    }

    //This fades out an image
    public static IEnumerator FadeOutImage(RawImage rawimage, float duration)
    {
        float alpha = 1;

        while (alpha > 0)
        {
            alpha = alpha - (1f / (60f * duration));

            if (rawimage != null)
            {         
                Color newColor = rawimage.color;
                newColor.a = alpha;
                rawimage.color = newColor;              
            }

            yield return new WaitForSecondsRealtime(0.016f);
        }
    }

    //This fades in an image
    public static IEnumerator FadeInImage(RawImage rawimage, float duration)
    {
        float alpha = 0;

        while (alpha < 1)
        {
            alpha = alpha + (1f / (60f * duration));

            if (rawimage != null)
            {
                Color newColor = rawimage.color;
                newColor.a = alpha;
                rawimage.color = newColor;               
            }

            yield return new WaitForSecondsRealtime(0.016f);
        }

    }

    //This fades an image in and out
    public static IEnumerator FadeImageInAndOut(RawImage rawimage, float fadeintime = 0.5f, float holdtime = 1, float fadeOuttime = 0.5f)
    {
        Task a = new Task(FadeInImage(rawimage, fadeintime));
        while (a.Running == true) { yield return null; }
        yield return new WaitForSeconds(holdtime);
        Task b = new Task(FadeOutImage(rawimage, fadeOuttime));
        while (b.Running == true) { yield return null; }
    }

    //This fades an image in and out
    public static IEnumerator FadeImageOutAndIn(RawImage rawimage, float fadeintime = 0.5f, float holdtime = 1, float fadeOuttime = 0.5f)
    {
        Task a = new Task(FadeOutImage(rawimage, fadeOuttime));
        while (a.Running == true) { yield return null; }
        yield return new WaitForSeconds(holdtime);
        Task b = new Task(FadeInImage(rawimage, fadeintime));
        while (b.Running == true) { yield return null; }
    }

    //This fades the hud until it can't be seen
    public static IEnumerator FadeOutHud(float duration)
    {
        GameObject hud = GetHudGameObject();
        CanvasGroup canvasGroup = hud.GetComponent<CanvasGroup>();

        //This sets the starting alpha value to 1
        float alpha = 1;

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

    //This coroutine can be used to fade the canvas group in
    public static IEnumerator FadeInHud(float duration)
    {
        GameObject hud = GetHudGameObject();
        CanvasGroup canvasGroup = hud.GetComponent<CanvasGroup>();

        //This sets the starting alpha value to 0
        float alpha1 = 0;

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

    #endregion

}
