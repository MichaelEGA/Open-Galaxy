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
        GameObject hudPrefab = Resources.Load(OGGetAddress.hud + "hud") as GameObject;
        GameObject hud = GameObject.Instantiate(hudPrefab);

        hud.name = "Hud";
        Hud hudScript = hud.AddComponent<Hud>();
        hudScript.loadTime = Time.time;

        LoadRadarPrefabs(hudScript);
        InstantiateRadarShips(hudScript);
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

    //This loads all the models prefabs ready to instantiate
    public static void LoadRadarPrefabs(Hud hud)
    {
        Object[] radarPrefabs = Resources.LoadAll(OGGetAddress.radar, typeof(GameObject));
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
        Object[] renderTexture = Resources.LoadAll(OGGetAddress.hud, typeof(RenderTexture));
        camera.targetTexture = renderTexture[0] as RenderTexture;
        camera.transform.position = new Vector3(0, 0, -50);
        camera.name = "Radar Camera";
    }

    #endregion

    #region current main ship display

    //Display ship info
    public static void DisplayShipInfo(Hud hud)
    {
        //This looks for the ship info object to see if they have been loaded into the hud or not
        if (hud.shipInfo == null)
        {
            GameObject shipInfo = GameObject.Find("ShipInfo");
            if (shipInfo != null) { hud.shipInfo = shipInfo.GetComponent<Text>(); }
        }

        if (hud.shipName == null)
        {
            GameObject shipName = GameObject.Find("ShipName");
            if (shipName != null) { hud.shipName = shipName.GetComponent<Text>(); }
        }

        //This displays the ships info
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
        if (hud.shieldMeter == null)
        {
            GameObject shieldMeter = GameObject.Find("ShieldMeter");
            if (shieldMeter != null) { hud.shieldMeter = shieldMeter.GetComponent<Slider>(); }
        }

        if (hud.shieldMeter != null & hud.smallShip != null & Time.timeScale != 0)
        {
            hud.shieldMeter.value = hud.smallShip.shieldPower;
        }
    }

    //This displays engine meter
    public static void DisplayEngineMeter(Hud hud)
    {
        if (hud.engineMeter == null)
        {
            GameObject engineMeter = GameObject.Find("EngineMeter");
            if (engineMeter != null) { hud.engineMeter = engineMeter.GetComponent<Slider>(); }
        }

        if (hud.engineMeter != null & hud.smallShip != null & Time.timeScale != 0)
        {
            hud.engineMeter.value = hud.smallShip.enginePower;
        }
    }

    //This displays laser meter
    public static void DisplayLaserMeter(Hud hud)
    {
        if (hud.laserMeter == null)
        {
            GameObject laserMeter = GameObject.Find("LaserMeter");
            if (laserMeter != null) { hud.laserMeter = laserMeter.GetComponent<Slider>(); }
        }

        if (hud.laserMeter != null & hud.smallShip != null & Time.timeScale != 0)
        {
            hud.laserMeter.value = hud.smallShip.laserPower;
        }
    }

    //This displays WEP meter
    public static void DisplayWEPMeter(Hud hud)
    {
        if (hud.WEPMeter == null)
        {
            GameObject WEPMeter = GameObject.Find("WEPMeter");
            if (WEPMeter != null) { hud.WEPMeter = WEPMeter.GetComponent<Slider>(); }
        }

        if (hud.WEPMeter != null & hud.smallShip != null & Time.timeScale != 0)
        {
            hud.WEPMeter.value = hud.smallShip.wepLevel;
        }
    }

    //This updates the hyperspace meter
    public static void DisplayHyperspaceMeter(Hud hud)
    {
        if (hud.HyperspaceMeter == null)
        {
            GameObject HyperspaceMeter = GameObject.Find("HyperspaceMeter");
            if (HyperspaceMeter != null) { hud.HyperspaceMeter = HyperspaceMeter.GetComponent<Slider>(); }
        }

        if (hud.HyperspaceMeter != null)
        {
            hud.HyperspaceMeter.value = hud.hyperspaceValue;
        }
    }

    //This displays the speed of the ship
    public static void DisplayShipSpeed(Hud hud)
    {
        if (hud.speedText == null)
        {
            GameObject speedText = GameObject.Find("SpeedText");
            if (speedText != null) { hud.speedText = speedText.GetComponent<Text>(); }
        }

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
        if (hud.matchSpeedText == null)
        {
            GameObject matchSpeedText = GameObject.Find("MatchSpeedText");
            if (matchSpeedText != null) { hud.matchSpeedText = matchSpeedText.GetComponent<Text>(); }
        }

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
        if (hud.shieldForwardOutside == null)
        {
            GameObject shieldForwardOutside = GameObject.Find("ShieldForwardOutside");
            if (shieldForwardOutside != null) { hud.shieldForwardOutside = shieldForwardOutside.GetComponent<RawImage>(); }
        }

        if (hud.shieldForwardInside == null)
        {
            GameObject shieldForwardInside = GameObject.Find("ShieldForwardInside");
            if (shieldForwardInside != null) { hud.shieldForwardInside = shieldForwardInside.GetComponent<RawImage>(); }
        }

        if (hud.hull == null)
        {
            GameObject hull = GameObject.Find("Hull");
            if (hull != null) { hud.hull = hull.GetComponent<RawImage>(); }
        }

        if (hud.shieldRearInside == null)
        {
            GameObject shieldRearInside = GameObject.Find("ShieldRearInside");
            if (shieldRearInside != null) { hud.shieldRearInside = shieldRearInside.GetComponent<RawImage>(); }
        }

        if (hud.shieldRearOutside == null)
        {
            GameObject shieldRearOutside = GameObject.Find("ShieldRearOutside");
            if (shieldRearOutside != null) { hud.shieldRearOutside = shieldRearOutside.GetComponent<RawImage>(); }
        }

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
        if (hud.activeWeaponText == null)
        {
            GameObject activeWeaponText = GameObject.Find("ActiveWeaponText");
            if (activeWeaponText != null) { hud.activeWeaponText = activeWeaponText.GetComponent<Text>(); }
        }

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
        if (hud.weaponModeText == null)
        {
            GameObject weaponModeText = GameObject.Find("WeaponModeText");
            if (weaponModeText != null) { hud.weaponModeText = weaponModeText.GetComponent<Text>(); }
        }

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
        if (hud.weaponNumberText == null)
        {
            GameObject weaponNumberText = GameObject.Find("WeaponNumberText");
            if (weaponNumberText != null) { hud.weaponNumberText = weaponNumberText.GetComponent<Text>(); }
        }

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
        if (hud.WEPText == null)
        {
            GameObject WEPText = GameObject.Find("WEPText");
            if (WEPText != null) { hud.WEPText = WEPText.GetComponent<Text>(); }
        }

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

    //This displays whether the WEP boost is on or not
    public static void DisplayHyperdrive(Hud hud)
    {
        if (hud.hyperdriveText == null)
        {
            GameObject hyperdriveText = GameObject.Find("HyperdriveText");
            if (hyperdriveText != null) { hud.hyperdriveText = hyperdriveText.GetComponent<Text>(); }
        }

        if (hud.hyperdriveText != null & hud.smallShip != null & Time.timeScale != 0)
        {
            if (hud.hyperdriveActive == true)
            {
                hud.hyperdriveText.text = "ON";
            }
            else
            {
                hud.hyperdriveText.text = "OFF";
            }
        }
    }

    #endregion

    #region target ship display

    //Display target speed
    public static void DisplayTargetDistance(Hud hud)
    {
        if (hud.targetDistanceText == null)
        {
            GameObject targetDistanceText = GameObject.Find("TargetDistanceText");
            if (targetDistanceText != null) { hud.targetDistanceText = targetDistanceText.GetComponent<Text>(); }
        }

        if (hud.targetDistanceText != null & hud.smallShip != null & Time.timeScale != 0)
        {
            if (hud.smallShip.target != null)
            {
                hud.targetDistanceText.text = (hud.smallShip.targetDistance / 1000f).ToString("0.000");
            }
            else
            {
                hud.targetDistanceText.text = " ";
            }
        }
    }

    //Display target speed
    public static void DisplayTargetType(Hud hud)
    {
        if (hud.targetType == null)
        {
            GameObject targetType = GameObject.Find("TargetType");
            if (targetType != null) { hud.targetType = targetType.GetComponent<Text>(); }
        }

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
        if (hud.targetName == null)
        {
            GameObject targetName = GameObject.Find("TargetName");
            if (targetName != null) { hud.targetName = targetName.GetComponent<Text>(); }
        }

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
        if (hud.targetSpeedText == null)
        {
            GameObject targetSpeedText = GameObject.Find("TargetSpeedText");
            if (targetSpeedText != null) { hud.targetSpeedText = targetSpeedText.GetComponent<Text>(); }
        }

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
        if (hud.targetShieldsText == null)
        {
            GameObject targetShieldsText = GameObject.Find("TargetShieldsText");
            if (targetShieldsText != null) { hud.targetShieldsText = targetShieldsText.GetComponent<Text>(); }
        }

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
        if (hud.targetHullText == null)
        {
            GameObject targetHullText = GameObject.Find("TargetHullText");
            if (targetHullText != null) { hud.targetHullText = targetHullText.GetComponent<Text>(); }
        }

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
        if (hud.targetCargo == null)
        {
            GameObject targetCargo = GameObject.Find("TargetCargo");
            if (targetCargo != null) { hud.targetCargo = targetCargo.GetComponent<Text>(); }
        }

        if (hud.targetCargo != null & hud.smallShip != null)
        {
            if (hud.smallShip.target != null)
            {
                if (hud.smallShip.targetSmallShip != null)
                {
                    float distance = Vector3.Distance(hud.smallShip.transform.position, hud.smallShip.targetSmallShip.transform.position);

                    if (hud.smallShip.targetSmallShip.scanned == false & distance > 200)
                    {
                        hud.targetCargo.text = "-";
                    }
                    else if (hud.smallShip.targetSmallShip.scanned == false & distance < 200)
                    {
                        hud.targetCargo.text = hud.smallShip.targetSmallShip.cargo.ToUpper();
                        hud.smallShip.targetSmallShip.scanned = true;
                        AddToShipLog(hud.smallShip.targetName.ToUpper() + " has been scanned: " + hud.smallShip.targetSmallShip.cargo.ToUpper());

                        if (hud.smallShip.audioManager != null)
                        {
                            AudioFunctions.PlayAudioClip(hud.smallShip.audioManager, "beep04_double", "Cockpit", hud.smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);
                        }                  
                    }
                    else if (hud.smallShip.targetSmallShip.scanned == true)
                    {
                        hud.targetCargo.text = hud.smallShip.targetSmallShip.cargo.ToUpper();
                    }
                }
                else if (hud.smallShip.targetLargeShip != null)
                {
                    float distance = Vector3.Distance(hud.smallShip.transform.position, hud.smallShip.targetLargeShip.transform.position);

                    if (hud.smallShip.targetLargeShip.scanned == false & distance > 300)
                    {
                        hud.targetCargo.text = "-";
                    }
                    else if (hud.smallShip.targetLargeShip.scanned == false & distance < 300)
                    {
                        hud.targetCargo.text = hud.smallShip.targetLargeShip.cargo.ToUpper();
                        hud.smallShip.targetLargeShip.scanned = true;
                        AddToShipLog(hud.smallShip.targetName.ToUpper() + " has been scanned: " + hud.smallShip.targetLargeShip.cargo.ToUpper());

                        if (hud.smallShip.audioManager != null)
                        {
                            AudioFunctions.PlayAudioClip(hud.smallShip.audioManager, "beep04_double", "Cockpit", hud.smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);
                        }
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

            if (hud.frontRadarCircle == null)
            {
                hud.frontRadarCircle = GameObject.Find("FrontRadarCircle");
            }

            float radarRadius = ((hud.frontRadarCircle.GetComponent<RectTransform>().rect.width / 2f) / 100f) * 90f;

            if (hud.frontRadarDot == null)
            {
                hud.frontRadarDot = GameObject.Find("FrontRadarDot");
                hud.frontRadarDot.SetActive(false);
            }

            if (hud.frontRadarDotsPool == null)
            {
                hud.frontRadarDotsPool = new List<GameObject>();
            }

            if (hud.frontRadarBrace == null)
            {
                hud.frontRadarBrace = GameObject.Find("FrontRadarBrace");
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

            if (hud.rearRadarCircle == null)
            {
                hud.rearRadarCircle = GameObject.Find("RearRadarCircle");
            }


            Vector2 radarPosition = new Vector2();
            Vector2 radarDirection = new Vector2();
            float yPositionRadar;
            float xPositionRadar;
            float rearRadar;
            int radarNumber = 0;

            float radarRadius = ((hud.rearRadarCircle.GetComponent<RectTransform>().rect.width / 2f) / 100f) * 90f;

            if (hud.rearRadarDot == null)
            {
                hud.rearRadarDot = GameObject.Find("RearRadarDot");
                hud.rearRadarDot.SetActive(false);
            }

            if (hud.rearRadarDotsPool == null)
            {
                hud.rearRadarDotsPool = new List<GameObject>();
            }

            if (hud.rearRadarBrace == null)
            {
                hud.rearRadarBrace = GameObject.Find("RearRadarBrace");
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
        if (hud.selectionBrace == null)
        {
            hud.selectionBrace = GameObject.Find("SelectionBrace");
        }

        if (hud.directionArrow == null)
        {
            hud.directionArrow = GameObject.Find("DirectionArrow");
        }

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
                                //if (screenPosition.z > 500)
                                //{
                                //    selectionBrace.transform.localScale = new Vector2(1, 1);
                                //}
                                //else
                                //{
                                //    selectionBrace.transform.localScale = new Vector2(2f - (0.002f * screenPosition.z), 2f - (0.002f * screenPosition.z));
                                //}

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

    //This displays the nav point marker when the destination is in view and an arrow pointing to the destination when offscreen
    public static void DisplayNavPointMarker(Hud hud)
    {
        if (Time.timeScale != 0)
        {
            if (hud.navSelectionBrace == null)
            {
                hud.navSelectionBrace = GameObject.Find("NavSelectionBrace");        
            }

            if (hud.navDirectionArrow == null)
            {
                hud.navDirectionArrow = GameObject.Find("NavDirectionArrow");
            }

            if (hud.scene.navPointMarker == null)
            {
                hud.scene.navPointMarker = new GameObject();
                hud.scene.navPointMarker.name = "NavPointMarker";
            }

            GameObject navSelectionBrace = hud.navSelectionBrace;
            GameObject navDirectionArrow = hud.navDirectionArrow;
            GameObject navPointMarker = hud.scene.navPointMarker;

            if (navSelectionBrace != null & navDirectionArrow != null)
            {
                navSelectionBrace.SetActive(false);
                navDirectionArrow.SetActive(false);
            }

            if (hud.starfieldCamera == null)
            {
                if (hud.smallShip != null)
                {
                    if (hud.smallShip.mainCamera != null)
                    {
                        hud.starfieldCamera = hud.scene.starfieldCamera.GetComponent<Camera>();
                    }
                }
            }

            if (hud.starfieldCamera != null & navPointMarker != null & navSelectionBrace != null & navDirectionArrow != null)
            {
                GameObject starfieldTargetPosition = navPointMarker;
                GameObject starfieldCurrentPosition = hud.starfieldCamera.gameObject;

                //This gets the targets position on the camera
                Vector3 screenPosition = hud.starfieldCamera.WorldToScreenPoint(starfieldTargetPosition.transform.position);

                //This sets key values
                Vector3 targetPosition = starfieldTargetPosition.transform.position - starfieldCurrentPosition.transform.position;
                float forward = Vector3.Dot(starfieldCurrentPosition.transform.forward, targetPosition.normalized);
                float up = Vector3.Dot(starfieldCurrentPosition.transform.up, targetPosition.normalized);
                float right = Vector3.Dot(starfieldCurrentPosition.transform.right, targetPosition.normalized);

                Vector3 viewPos = hud.starfieldCamera.WorldToViewportPoint(starfieldTargetPosition.transform.position);

                bool onscreen = false;

                if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
                {
                    onscreen = true;
                }
                else
                {
                    onscreen = false;
                }

                //This checks that the target is on screen
                if (forward > 0 & onscreen == true)
                {
                    //This sets the braces to active when the target is on screen
                    navSelectionBrace.SetActive(true);
                    navDirectionArrow.SetActive(false);

                    //This translates that position to the selection brace
                    navSelectionBrace.transform.position = new Vector2(screenPosition.x, screenPosition.y);
                }
                else
                {
                    //This sets the braces to inactive when the target is behind the camera
                    navSelectionBrace.SetActive(false);
                    navDirectionArrow.SetActive(true);

                    //This gets values from atlasCommon
                    RectTransform rectTransform = hud.gameObject.GetComponent<RectTransform>();
                    float previousArrowRotation = hud.navPreviousArrowRotation;
                    float arrowTargetRotation = hud.navArrowTargetRotation;
                    float arrowLerpTime = hud.navArrowLerpTime;

                    //This gets screen width and height values
                    float screenWidth = rectTransform.rect.width;
                    float screenHeight = rectTransform.rect.height;

                    //This controls the arrow rotation
                    arrowLerpTime += 5f * Time.deltaTime;
                    float arrowRotation = Mathf.Lerp(previousArrowRotation, arrowTargetRotation, arrowLerpTime);
                    navDirectionArrow.transform.localRotation = Quaternion.Euler(0, 0, arrowRotation);

                    if (right > 0)
                    {
                        if (up > 0.5f)
                        {
                            navDirectionArrow.transform.localPosition = new Vector2((screenWidth / 2f) * ((0.5f - (up - 0.5f)) * 2f), screenHeight / 2f);

                            if (arrowTargetRotation != -90)
                            {
                                previousArrowRotation = arrowRotation;
                                arrowTargetRotation = -90;
                                arrowLerpTime = 0;
                            }

                        }

                        else if (up < 0.5f & up > 0f)
                        {
                            navDirectionArrow.transform.localPosition = new Vector2(screenWidth / 2f, (screenHeight / 2f) * up * 2f);

                            if (arrowTargetRotation != -90)
                            {
                                previousArrowRotation = arrowRotation;
                                arrowTargetRotation = -90;
                                arrowLerpTime = 0;
                            }

                        }

                        else if (up < 0f & up > -0.5f)
                        {
                            navDirectionArrow.transform.localPosition = new Vector2(screenWidth / 2f, (screenHeight / 2f) * up * 2f);

                            if (arrowTargetRotation != -180)
                            {
                                previousArrowRotation = arrowRotation;
                                arrowTargetRotation = -180;
                                arrowLerpTime = 0;
                            }

                        }

                        else if (up < -0.5)
                        {
                            navDirectionArrow.transform.localPosition = new Vector2((screenWidth / 2f) * ((0.5f + (up + 0.5f)) * 2f), -screenHeight / 2f);

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
                            navDirectionArrow.transform.localPosition = new Vector2((screenWidth / 2f) * ((0.5f - (up - 0.5f)) * -2f), screenHeight / 2f);

                            if (arrowTargetRotation != 0)
                            {
                                previousArrowRotation = arrowRotation;
                                arrowTargetRotation = 0;
                                arrowLerpTime = 0;
                            }

                        }

                        else if (up < 0.5f & up > 0f)
                        {
                            navDirectionArrow.transform.localPosition = new Vector2(-screenWidth / 2f, (screenHeight / 2f) * up * 2f);

                            if (arrowTargetRotation != 0)
                            {
                                previousArrowRotation = arrowRotation;
                                arrowTargetRotation = 0;
                                arrowLerpTime = 0;
                            }

                        }

                        else if (up < 0f & up > -0.5f)
                        {
                            navDirectionArrow.transform.localPosition = new Vector2(-screenWidth / 2f, (screenHeight / 2f) * up * 2f);

                            if (arrowTargetRotation != -270)
                            {
                                previousArrowRotation = arrowRotation;
                                arrowTargetRotation = -270;
                                arrowLerpTime = 0;
                            }

                        }

                        else if (up < -0.5)
                        {
                            navDirectionArrow.transform.localPosition = new Vector2((screenWidth / 2f) * ((0.5f + (up + 0.5f)) * -2f), -screenHeight / 2f);

                            if (arrowTargetRotation != -270)
                            {
                                previousArrowRotation = arrowRotation;
                                arrowTargetRotation = -270;
                                arrowLerpTime = 0;
                            }

                        }

                    }

                    hud.navPreviousArrowRotation = previousArrowRotation;
                    hud.navArrowTargetRotation = arrowTargetRotation;
                    hud.navArrowLerpTime = arrowLerpTime;
                }
            }
        }
    }

    //This displays the target lock reticule
    public static void DisplayTargetLockReticule(Hud hud)
    {
        if (hud.targetLockingReticule == null)
        {
            GameObject targetLockingReticule = GameObject.Find("TargetLockingReticule");
            if (targetLockingReticule != null) { hud.targetLockingReticule = targetLockingReticule.GetComponent<RawImage>(); }
            targetLockingReticule.SetActive(false);
        }

        if (hud.targetLockedReticule == null)
        {
            GameObject targetLockedReticule = GameObject.Find("TargetLockedReticule");
            if (targetLockedReticule != null) { hud.targetLockedReticule = targetLockedReticule.GetComponent<RawImage>(); }
            targetLockedReticule.SetActive(false);
        }

        //Debug.Log("this function is running");

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

                    if (hud.lockBeep != null)
                    {
                        hud.lockBeep.volume = 0.6f;
                    }
                }
                else
                {
                    hud.targetLockingReticule.gameObject.SetActive(false);
                    hud.targetLockedReticule.gameObject.SetActive(false);

                    if (hud.lockBeep != null)
                    {
                        hud.lockBeep.volume = 0f;
                    }
                }
                
            }
            else
            {
                hud.targetLockingReticule.gameObject.SetActive(false);
                hud.targetLockedReticule.gameObject.SetActive(false);

                if (hud.lockBeep != null)
                {
                    hud.lockBeep.volume = 0f;
                }
            }
        }
    }

    //This makes the reticule flash
    public static IEnumerator TurnReticuleOnAndOff(Hud hud, RawImage reticule)
    {
        hud.reticuleFlashing = true;

        reticule.gameObject.SetActive(true);

        if (hud.lockBeep == null)
        {
            hud.lockBeep = AudioFunctions.PlayAudioClip(hud.smallShip.audioManager, "beep_targetlock", "Cockpit", hud.smallShip.transform.position, 0, 1, 500, 0.6f);
            hud.lockBeep.loop = true;
        }

        if (hud.lockBeep != null)
        {

            hud.lockBeep.volume = 0.5f;
        }
            
        yield return new WaitForSeconds(0.25f);

        reticule.gameObject.SetActive(false);

        if (hud.lockBeep != null)
        {
            hud.lockBeep.volume = 0;
        }

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
            Hud hud = GetHud();

            if (hud != null)
            {
                if (hud.shipLog == null)
                {
                    GameObject shipLog = GameObject.Find("ShipLog");
                    if (shipLog != null) { hud.shipLog = shipLog.GetComponent<Text>(); }
                }

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
        Hud hud = GetHud();

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

    //This updates the location of the ship and the selected location
    public static void UpdateLocation(string currentLocation, string selectedLocation)
    {
        Hud hud = GetHud();

        if (hud != null)
        {
            if (hud.destination == null)
            {
                GameObject destinationGO = GameObject.Find("Destination");
                hud.destination = destinationGO.GetComponent<Text>();
            }

            if (hud.destination != null)
            {
                hud.destination.text = "LOC: " + currentLocation.ToUpper() + "\n" + "DES: " + selectedLocation.ToUpper();
            }
        }      
    }

    #endregion

    #region location display

    //This briefly displays a message in large text in the middle of the screen
    public static void DisplayTitle(string location, int fontsize, string colour = "#FFFFFF")
    {
        if (Time.timeScale != 0)
        {
            Hud hud = GetHud();

            if (fontsize <= 0)
            {
                fontsize = 25;
            }

            if (hud != null)
            {
                if (hud.locationInfo == null)
                {
                    GameObject locationInfo = GameObject.Find("LocationInfo");
                    if (locationInfo != null) { hud.locationInfo = locationInfo.GetComponent<Text>(); }
                }

                if (hud.reticule == null)
                {
                    GameObject reticule = GameObject.Find("Reticule");
                    if (reticule != null) { hud.reticule = reticule.GetComponent<RawImage>(); }
                }

                if (hud.locationInfo != null & hud.reticule != null)
                {
                    if (!colour.Contains("#"))
                    {
                        Color newColour = Color.white;

                        if (ColorUtility.TryParseHtmlString(colour, out newColour))
                        {
                            //Do nothing
                        }

                        hud.locationInfo.color = newColour;
                    }

                    hud.locationInfo.text = location;
                    hud.locationInfo.fontSize = fontsize;
                    Task a = new Task(FadeTextInAndOut(hud.locationInfo, 0.5f, 3, 0.5f)); //This fades the title in and out
                    Task b = new Task(FadeImageOutAndIn(hud.reticule, 0.25f, 4, 0.5f)); //This briefly fades the hud reticule to avoid a clash with the title
                }
            }
        } 
    }

    #endregion

    #region moving reticule

    //This shows the position of the mouse on the screen when using mouse control
    public static void MoveReticule(Hud hud)
    {
        if (hud.movingReticule == null)
        {
            hud.movingReticule = GameObject.Find("MovingReticule");
        }

        if (hud.movingReticleImage == null & hud.movingReticule != null)
        {
            hud.movingReticleImage = hud.movingReticule.GetComponentInChildren<RawImage>();
        }

        if (hud.centerReticule == null)
        {
            hud.centerReticule = GameObject.Find("Reticule");
        }

        if (hud.movingReticule != null & hud.centerReticule != null & hud.smallShip != null & Time.timeScale != 0)
        {
            if (hud.smallShip.keyboadAndMouse == true)
            {
                hud.movingReticule.SetActive(true);

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
            else
            {
                hud.movingReticule.SetActive(false);
            }
        }

        //This fades the mouse reticle as it gets closer to the center of the scene
        if (hud.movingReticule != null & hud.centerReticule != null & hud.movingReticleImage != null)
        {
            float distance = Vector2.Distance(hud.movingReticule.transform.position, hud.centerReticule.transform.position);

            if (distance < 40 & distance > 10)
            {
                float alpha = (1f / 30f) * distance;

                Color newColor = hud.movingReticleImage.color;
                newColor.a = alpha;
                hud.movingReticleImage.color = newColor;
            }
            else if (distance < 10)
            {
                Color newColor = hud.movingReticleImage.color;
                newColor.a = 0;
                hud.movingReticleImage.color = newColor;
            }
        }
    }

    #endregion

    #region unload hud and hud assets

    //This calls the unloading function before unloading the hud object itself
    public static void UnloadHud()
    {
        Hud hud = GetHud();

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
    public static Hud GetHud()
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

    //This sets the colour of the hud
    public static void SetHudColour(string colour)
    {
        //This gets the hud reference
        Hud hud = GetHud();

        //This sets the fog color to match the skybox
        Color newColour;

        if (ColorUtility.TryParseHtmlString(colour, out newColour))
        {
            //Do nothing
        }

        //This applies the new colour to different aspects of the hud
        if (hud != null)
        {
            //The shield meter
            if (hud.shieldMeter == null)
            {
                GameObject shieldMeter = GameObject.Find("ShieldMeter");
                if (shieldMeter != null) { hud.shieldMeter = shieldMeter.GetComponent<Slider>(); }
            }

            if (hud.shieldMeter != null)
            {
                RectTransform fillRectTransform = hud.shieldMeter.fillRect;

                if (fillRectTransform != null)
                {
                    Image image = fillRectTransform.GetComponent<Image>();

                    if (image != null)
                    {
                        image.color = newColour;
                    }
                }
            }

            //The engine meter
            if (hud.engineMeter == null)
            {
                GameObject engineMeter = GameObject.Find("EngineMeter");
                if (engineMeter != null) { hud.engineMeter = engineMeter.GetComponent<Slider>(); }
            }

            if (hud.engineMeter != null)
            {
                RectTransform fillRectTransform = hud.engineMeter.fillRect;

                if (fillRectTransform != null)
                {
                    Image image = fillRectTransform.GetComponent<Image>();

                    if (image != null)
                    {
                        image.color = newColour;
                    }
                }
            }

            //The laser meter
            if (hud.laserMeter == null)
            {
                GameObject laserMeter = GameObject.Find("LaserMeter");
                if (laserMeter != null) { hud.laserMeter = laserMeter.GetComponent<Slider>(); }
            }

            if (hud.laserMeter != null)
            {
                RectTransform fillRectTransform = hud.laserMeter.fillRect;

                if (fillRectTransform != null)
                {
                    Image image = fillRectTransform.GetComponent<Image>();

                    if (image != null)
                    {
                        image.color = newColour;
                    }
                }
            }

            //The wep meter
            if (hud.WEPMeter == null)
            {
                GameObject WEPMeter = GameObject.Find("WEPMeter");
                if (WEPMeter != null) { hud.WEPMeter = WEPMeter.GetComponent<Slider>(); }
            }

            if (hud.WEPMeter != null)
            {
                RectTransform fillRectTransform = hud.WEPMeter.fillRect;

                if (fillRectTransform != null)
                {
                    Image image = fillRectTransform.GetComponent<Image>();

                    if (image != null)
                    {
                        image.color = newColour;
                    }
                }
            }

            //Target speed text
            if (hud.targetSpeedText == null)
            {
                GameObject targetSpeedText = GameObject.Find("TargetSpeedText");
                if (targetSpeedText != null) { hud.targetSpeedText = targetSpeedText.GetComponent<Text>(); }
            }

            if (hud.targetSpeedText != null)
            {
                Text text = hud.targetSpeedText.GetComponent<Text>();

                if (text != null)
                {
                    text.color = newColour;
                }
            }

            //Target shield text
            if (hud.targetShieldsText == null)
            {
                GameObject targetShieldsText = GameObject.Find("TargetShieldsText");
                if (targetShieldsText != null) { hud.targetShieldsText = targetShieldsText.GetComponent<Text>(); }
            }

            if (hud.targetShieldsText != null)
            {
                Text text = hud.targetShieldsText.GetComponent<Text>();

                if (text != null)
                {
                    text.color = newColour;
                }
            }

            //Target hull text
            if (hud.targetHullText == null)
            {
                GameObject targetHullText = GameObject.Find("TargetHullText");
                if (targetHullText != null) { hud.targetHullText = targetHullText.GetComponent<Text>(); }
            }

            if (hud.targetHullText != null)
            {
                Text text = hud.targetHullText.GetComponent<Text>();

                if (text != null)
                {
                    text.color = newColour;
                }
            }

            //Target dist text
            if (hud.targetDistanceText == null)
            {
                GameObject targetDistanceText = GameObject.Find("TargetDistanceText");
                if (targetDistanceText != null) { hud.targetDistanceText = targetDistanceText.GetComponent<Text>(); }
            }

            if (hud.targetDistanceText != null)
            {
                Text text = hud.targetDistanceText.GetComponent<Text>();

                if (text != null)
                {
                    text.color = newColour;
                }
            }

            //Ship speed text
            if (hud.speedText == null)
            {
                GameObject SpeedText = GameObject.Find("SpeedText");
                if (SpeedText != null) { hud.speedText = SpeedText.GetComponent<Text>(); }
            }

            if (hud.speedText != null)
            {
                Text text = hud.speedText.GetComponent<Text>();

                if (text != null)
                {
                    text.color = newColour;
                }
            }

            //Ship match speed text
            if (hud.matchSpeedText == null)
            {
                GameObject matchSpeedText = GameObject.Find("MatchSpeedText");
                if (matchSpeedText != null) { hud.matchSpeedText = matchSpeedText.GetComponent<Text>(); }
            }

            if (hud.matchSpeedText != null)
            {
                Text text = hud.matchSpeedText.GetComponent<Text>();

                if (text != null)
                {
                    text.color = newColour;
                }
            }

            //Ship match speed text
            if (hud.weaponNumberText == null)
            {
                GameObject weaponNumberText = GameObject.Find("WeaponNumberText");
                if (weaponNumberText != null) { hud.weaponNumberText = weaponNumberText.GetComponent<Text>(); }
            }

            if (hud.weaponNumberText != null)
            {
                Text text = hud.weaponNumberText.GetComponent<Text>();

                if (text != null)
                {
                    text.color = newColour;
                }
            }

            //The front radar dot
            if (hud.frontRadarDot == null)
            {
                GameObject frontRadarDot = GameObject.Find("FrontRadarDot");
                if (frontRadarDot != null) { hud.frontRadarDot = frontRadarDot; }
            }

            if (hud.frontRadarDot != null)
            {
                RawImage rawImage = hud.frontRadarDot.GetComponent<RawImage>();

                if (rawImage != null)
                {
                    rawImage.color = newColour;
                }
            }
           
            //The rear radar dot
            if (hud.rearRadarDot == null)
            {
                GameObject rearRadarDot = GameObject.Find("RearRadarDot");
                if (rearRadarDot != null) { hud.rearRadarDot = rearRadarDot; }
            }


            if (hud.rearRadarDot != null)
            {
                RawImage rawImage = hud.rearRadarDot.GetComponent<RawImage>();

                if (rawImage != null)
                {
                    rawImage.color = newColour;
                }
            }

            //The selection brace
            if (hud.selectionBrace == null)
            {
                GameObject selectionBrace = GameObject.Find("SelectionBrace");
                if (selectionBrace != null) { hud.selectionBrace = selectionBrace; }
            }

            if (hud.selectionBrace != null)
            {
                RawImage rawImage = hud.selectionBrace.GetComponentInChildren<RawImage>();

                if (rawImage != null)
                {
                    rawImage.color = newColour;
                }
            }

            //The selection brace
            if (hud.directionArrow == null)
            {
                GameObject directionArrow = GameObject.Find("DirectionArrow");
                if (directionArrow != null) { hud.directionArrow = directionArrow; }
            }

            if (hud.directionArrow != null)
            {
                RawImage rawImage = hud.directionArrow.GetComponentInChildren<RawImage>();

                if (rawImage != null)
                {
                    rawImage.color = newColour;
                }
            }
        }
    }

    #endregion

}
