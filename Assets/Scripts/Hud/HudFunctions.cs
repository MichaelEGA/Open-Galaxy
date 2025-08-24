using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEditor;

public static class HudFunctions
{
    #region start functions

    //This loads the hud prefab
    public static void CreateHud()
    {
        GameObject hudPrefab = Resources.Load(OGGetAddress.hud + "hud") as GameObject;
        GameObject hudGO = GameObject.Instantiate(hudPrefab);

        hudGO.name = "Hud";
        Hud hudScript = hudGO.AddComponent<Hud>();
        hudScript.startTime = Time.time;

        GameObject fadePrefab = Resources.Load(OGGetAddress.hud + "Fade") as GameObject;
        GameObject fadeGO = GameObject.Instantiate(fadePrefab);
        fadeGO.name = "Fade";

        Scene scene = SceneFunctions.GetScene();
        scene.fade = fadeGO;

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
        if (hud.radarPool == null)
        {
            hud.radarPool = new List<GameObject>();
        }

        if (hud.radarPool != null & hud.smallShip != null & Time.timeScale != 0)
        {
            if (hud.smallShip.target != null)
            {
                if (hud.smallShip.target.activeSelf != false)
                {
                    if (hud.radarObject == null || hud.radarObject.activeSelf == false || !hud.radarObject.name.Contains(hud.smallShip.targetPrefabName))
                    {
                        bool foundShip = false;

                        foreach (GameObject radarObject in hud.radarPool)
                        {
                            if (radarObject.name == hud.smallShip.targetPrefabName + "(Clone)")
                            {
                                radarObject.SetActive(true);
                                hud.radarObject = radarObject;
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
                                if (ship.name == hud.smallShip.targetPrefabName)
                                {
                                    //This instantiates the radar object,sets it to the correct position, sets its layer, scales the object and removes any colliders on the object
                                    GameObject radarObject = GameObject.Instantiate(ship) as GameObject;
                                    hud.radarPool.Add(radarObject);
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

                                    hud.radarObject = radarObject;
                                }
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
                hud.speedText.text = (hud.smallShip.shipRigidbody.linearVelocity.magnitude * 3.6f).ToString("000");
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
            else if (hud.smallShip.activeWeapon == "ion")
            {
                hud.activeWeaponText.text = "ION";
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
            else if (hud.smallShip.weaponMode == "all")
            {
                hud.weaponModeText.text = "ALL";
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
            else if (hud.smallShip.activeWeapon == "ion")
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
    public static void DisplaySystemsStrength(Hud hud)
    {
        if (hud.systemsText == null)
        {
            GameObject systemsText = GameObject.Find("SystemsText");
            if (systemsText != null) { hud.systemsText = systemsText.GetComponent<Text>(); }
        }

        if (hud.systemsText != null & hud.smallShip != null & Time.timeScale != 0)
        {
            hud.systemsText.text = hud.smallShip.systemsLevel.ToString("000");
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
                hud.targetDistanceText.text = "---";
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
                hud.targetType.text = "---";
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
                hud.targetName.text = "---";
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
                        float speed = hud.smallShip.targetSmallShip.shipRigidbody.linearVelocity.magnitude * 3.6f;

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
                hud.targetSpeedText.text = "---";
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
                hud.targetShieldsText.text = "---";
            }
        }
    }

    //Display target systems
    public static void DisplayTargetSystems(Hud hud)
    {
        if (hud.targetSystemsText == null)
        {
            GameObject targetSystemsText = GameObject.Find("TargetSystemsText");
            if (targetSystemsText != null) { hud.targetSystemsText = targetSystemsText.GetComponent<Text>(); }
        }

        if (hud.targetSystemsText != null & hud.smallShip != null & Time.timeScale != 0)
        {
            if (hud.smallShip.target != null)
            {
                if (hud.smallShip.targetSmallShip != null)
                {
                    hud.targetSystemsText.text = hud.smallShip.targetSmallShip.systemsLevel.ToString("000");
                }
                else if (hud.smallShip.targetLargeShip != null)
                {
                    hud.targetSystemsText.text = hud.smallShip.targetLargeShip.systemsLevel.ToString("000");
                }
                else
                {
                    hud.targetSystemsText.text = "---";
                }
            }
            else
            {
                hud.targetSystemsText.text = "---";
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
                hud.targetHullText.text = "---";
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
                        hud.targetCargo.text = "---";
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
                        hud.targetCargo.text = "---";
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
                hud.targetCargo.text = "---";
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

            if (hud.secondaryCamera == null)
            {
                if (hud.smallShip != null)
                {
                    if (hud.smallShip.followCamera != null)
                    {
                        hud.secondaryCamera = hud.smallShip.followCamera.GetComponent<Camera>();
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

                            if (hud.scene.followCameraIsActive == true)
                            {
                                screenPosition = hud.secondaryCamera.WorldToScreenPoint(target.transform.position);
                            }

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

    //This displays a target where the intercept point for the target ship is
    public static void DisplayInterceptPoint(Hud hud)
    {
        if (hud.interceptPoint == null)
        {
            hud.interceptPoint = GameObject.Find("InterceptPoint");
        }

        if (Time.timeScale != 0)
        {
            GameObject interceptPoint = hud.interceptPoint;

            if (interceptPoint != null)
            {
                interceptPoint.SetActive(false);
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

            if (hud.secondaryCamera == null)
            {
                if (hud.smallShip != null)
                {
                    if (hud.smallShip.followCamera != null)
                    {
                        hud.secondaryCamera = hud.smallShip.followCamera.GetComponent<Camera>();
                    }
                }
            }

            if (hud.mainCamera != null)
            {
                if (hud.smallShip != null & interceptPoint != null)
                {
                    if (hud.smallShip.target != null)
                    {
                        if (hud.smallShip.target.activeSelf != false & hud.smallShip.targetRigidbody != null & hud.smallShip.targetSmallShip != null)
                        {
                            GameObject target = hud.smallShip.target;
                            GameObject mainShip = hud.smallShip.gameObject;

                            //This gets the intercept point
                            Vector3 interceptPosition = GameObjectUtils.CalculateInterceptPoint(mainShip.transform.position, target.transform.position, hud.smallShip.targetRigidbody.linearVelocity, 750);

                            //This gets the targets position on the camera
                            Vector3 screenPosition = hud.mainCamera.WorldToScreenPoint(interceptPosition);

                            if (hud.scene.followCameraIsActive == true)
                            {
                                screenPosition = hud.secondaryCamera.WorldToScreenPoint(interceptPosition);
                            }

                            //This sets key values
                            Vector3 targetPosition = target.transform.position - mainShip.transform.position;
                            float forward = Vector3.Dot(mainShip.transform.forward, targetPosition.normalized);
                            float up = Vector3.Dot(mainShip.transform.up, targetPosition.normalized);
                            float right = Vector3.Dot(mainShip.transform.right, targetPosition.normalized);

                            //This checks that the target is on screen
                            if (target.GetComponentInChildren<Renderer>().isVisible == true & forward > 0)
                            {
                                //This sets the intercept point to active when the target is on screen
                                interceptPoint.SetActive(true);

                                //This translates that position to the intercept point
                                interceptPoint.transform.position = new Vector2(screenPosition.x, screenPosition.y);
                            }
                            else
                            {
                                //This sets the intercept point to inactive when the target is behind the camera
                                interceptPoint.SetActive(false);
                            }
                        }
                    }
                }
            }
        }
    }

    //This displays the nav point marker when the destination is in view and an arrow pointing to the destination when offscreen
    public static void DisplayWaypointMarker(Hud hud)
    {
        if (Time.timeScale != 0)
        {
            if (hud.waypointText == null)
            {
                GameObject waypointText = GameObject.Find("WaypointText");
                if (waypointText != null) { hud.waypointText = waypointText.GetComponent<Text>(); }
            }

            if (hud.waypointTitle == null)
            {
                GameObject waypointTitle = GameObject.Find("WaypointTitle");
                if (waypointTitle != null) { hud.waypointTitle = waypointTitle.GetComponent<Text>(); }
            }

            if (hud.waypointMarker == null)
            {
                hud.waypointMarker = GameObject.Find("WaypointMarker");        
            }

            if (hud.waypointArrow == null)
            {
                hud.waypointArrow = GameObject.Find("WaypointArrow");
            }

            if (hud.scene.waypointObject == null & hud.smallShip != null)
            {
                hud.scene.waypointObject = hud.smallShip.waypoint;
            }

            GameObject waypointMarker = hud.waypointMarker;
            GameObject waypointArrow = hud.waypointArrow;
            GameObject waypointObject = hud.scene.waypointObject;

            if (waypointMarker != null & waypointArrow != null)
            {
                waypointMarker.SetActive(false);
                waypointArrow.SetActive(false);
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

            if (hud.secondaryCamera == null)
            {
                if (hud.smallShip != null)
                {
                    if (hud.smallShip.followCamera != null)
                    {
                        hud.secondaryCamera = hud.smallShip.followCamera.GetComponent<Camera>();
                    }
                }
            }

            if (hud.waypointIsActive == true & hud.mainCamera != null & waypointObject != null & waypointMarker != null & waypointArrow != null & hud.waypointText != null & hud.waypointTitle != null)
            {
                GameObject waypointGO = waypointObject;
                GameObject shipGO = hud.smallShip.gameObject;

                //This gets the targets position on the camera
                Vector3 screenPosition = hud.mainCamera.WorldToScreenPoint(waypointGO.transform.position);

                if (hud.scene.followCameraIsActive == true)
                {
                    screenPosition = hud.secondaryCamera.WorldToScreenPoint(waypointGO.transform.position);
                }

                //This gets the distance to the waypoint
                float distance = Vector3.Distance(waypointGO.transform.position, shipGO.transform.position);
                hud.waypointText.text = (distance / 1000f).ToString("0.000");

                //This sets the title of the waypoint
                hud.waypointTitle.text = hud.waypointTitleString;

                //This sets key values
                Vector3 targetPosition = waypointGO.transform.position - shipGO.transform.position;
                float forward = Vector3.Dot(shipGO.transform.forward, targetPosition.normalized);
                float up = Vector3.Dot(shipGO.transform.up, targetPosition.normalized);
                float right = Vector3.Dot(shipGO.transform.right, targetPosition.normalized);

                Vector3 viewPos = hud.mainCamera.WorldToViewportPoint(waypointGO.transform.position);

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
                    waypointMarker.SetActive(true);
                    waypointArrow.SetActive(false);

                    //This translates that position to the selection brace
                    waypointMarker.transform.position = new Vector2(screenPosition.x, screenPosition.y);
                }
                else
                {
                    //This sets the braces to inactive when the target is behind the camera
                    waypointMarker.SetActive(false);
                    waypointArrow.SetActive(true);

                    //This gets values from atlasCommon
                    RectTransform rectTransform = hud.gameObject.GetComponent<RectTransform>();
                    float previousArrowRotation = hud.waypointPreviousArrowRotation;
                    float arrowTargetRotation = hud.waypointArrowTargetRotation;
                    float arrowLerpTime = hud.waypointArrowLerpTime;

                    //This gets screen width and height values
                    float screenWidth = rectTransform.rect.width;
                    float screenHeight = rectTransform.rect.height;

                    //This controls the arrow rotation
                    arrowLerpTime += 5f * Time.deltaTime;
                    float arrowRotation = Mathf.Lerp(previousArrowRotation, arrowTargetRotation, arrowLerpTime);
                    waypointArrow.transform.localRotation = Quaternion.Euler(0, 0, arrowRotation);

                    if (right > 0)
                    {
                        if (up > 0.5f)
                        {
                            waypointArrow.transform.localPosition = new Vector2((screenWidth / 2f) * ((0.5f - (up - 0.5f)) * 2f), screenHeight / 2f);

                            if (arrowTargetRotation != -90)
                            {
                                previousArrowRotation = arrowRotation;
                                arrowTargetRotation = -90;
                                arrowLerpTime = 0;
                            }

                        }

                        else if (up < 0.5f & up > 0f)
                        {
                            waypointArrow.transform.localPosition = new Vector2(screenWidth / 2f, (screenHeight / 2f) * up * 2f);

                            if (arrowTargetRotation != -90)
                            {
                                previousArrowRotation = arrowRotation;
                                arrowTargetRotation = -90;
                                arrowLerpTime = 0;
                            }

                        }

                        else if (up < 0f & up > -0.5f)
                        {
                            waypointArrow.transform.localPosition = new Vector2(screenWidth / 2f, (screenHeight / 2f) * up * 2f);

                            if (arrowTargetRotation != -180)
                            {
                                previousArrowRotation = arrowRotation;
                                arrowTargetRotation = -180;
                                arrowLerpTime = 0;
                            }

                        }

                        else if (up < -0.5)
                        {
                            waypointArrow.transform.localPosition = new Vector2((screenWidth / 2f) * ((0.5f + (up + 0.5f)) * 2f), -screenHeight / 2f);

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
                            waypointArrow.transform.localPosition = new Vector2((screenWidth / 2f) * ((0.5f - (up - 0.5f)) * -2f), screenHeight / 2f);

                            if (arrowTargetRotation != 0)
                            {
                                previousArrowRotation = arrowRotation;
                                arrowTargetRotation = 0;
                                arrowLerpTime = 0;
                            }

                        }

                        else if (up < 0.5f & up > 0f)
                        {
                            waypointArrow.transform.localPosition = new Vector2(-screenWidth / 2f, (screenHeight / 2f) * up * 2f);

                            if (arrowTargetRotation != 0)
                            {
                                previousArrowRotation = arrowRotation;
                                arrowTargetRotation = 0;
                                arrowLerpTime = 0;
                            }

                        }

                        else if (up < 0f & up > -0.5f)
                        {
                            waypointArrow.transform.localPosition = new Vector2(-screenWidth / 2f, (screenHeight / 2f) * up * 2f);

                            if (arrowTargetRotation != -270)
                            {
                                previousArrowRotation = arrowRotation;
                                arrowTargetRotation = -270;
                                arrowLerpTime = 0;
                            }

                        }

                        else if (up < -0.5)
                        {
                            waypointArrow.transform.localPosition = new Vector2((screenWidth / 2f) * ((0.5f + (up + 0.5f)) * -2f), -screenHeight / 2f);

                            if (arrowTargetRotation != -270)
                            {
                                previousArrowRotation = arrowRotation;
                                arrowTargetRotation = -270;
                                arrowLerpTime = 0;
                            }

                        }

                    }

                    hud.waypointPreviousArrowRotation = previousArrowRotation;
                    hud.waypointArrowTargetRotation = arrowTargetRotation;
                    hud.waypointArrowLerpTime = arrowLerpTime;
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
                        AddTaskToPool(hud, a);
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

    //This displays a target where the intercept point for the target ship is
    public static void DisplayReticule(Hud hud)
    {
        if (hud.reticule == null)
        {
            GameObject reticule = GameObject.Find("Reticule");
            if (reticule != null) { hud.reticule = reticule.GetComponent<RawImage>(); }
        }

        if (Time.timeScale != 0)
        {

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

            if (hud.secondaryCamera == null)
            {
                if (hud.smallShip != null)
                {
                    if (hud.smallShip.followCamera != null)
                    {
                        hud.secondaryCamera = hud.smallShip.followCamera.GetComponent<Camera>();
                    }
                }
            }

            if (hud.secondaryCamera != null & hud.mainCamera != null)
            {
                if (hud.smallShip != null & hud.reticule != null)
                {
                    if (hud.smallShip.cameraPosition != null)
                    {
                        GameObject target = hud.smallShip.target;
                        GameObject mainShip = hud.smallShip.gameObject;

                        //This gets the intercept point
                        Vector3 reticulePosition = hud.smallShip.cameraPosition.transform.position + (hud.smallShip.cameraPosition.transform.forward * hud.smallShip.interceptDistance);

                        //This gets the targets position on the camera
                        Vector3 screenPosition = hud.mainCamera.WorldToScreenPoint(reticulePosition);

                        if (hud.scene.followCameraIsActive == true)
                        {
                            screenPosition = hud.secondaryCamera.WorldToScreenPoint(reticulePosition);
                        }

                        //This translates that position to the intercept point
                        hud.reticule.gameObject.transform.position = new Vector2(screenPosition.x, screenPosition.y);
                    }
                }
            }
            else
            {
                if (hud.reticule != null)
                {
                    hud.reticule.gameObject.transform.position = new Vector2(0, 0);
                }
            }
        }
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
                //This caculates the time in 60 second increments
                float timer = Time.time - hud.startTime;
                int minutes = Mathf.FloorToInt(timer / 60F);
                int seconds = Mathf.FloorToInt(timer - minutes * 60);
                string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);

                if (hud.shipLog == null)
                {
                    GameObject shipLog = GameObject.Find("ShipLog");
                    if (shipLog != null) { hud.shipLog = shipLog.GetComponent<Text>(); }
                }

                if (hud.shipLog != null)
                {
                    hud.shipLog.text = niceTime + " " + message + "\n" + hud.shipLog.text;
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

    #endregion

    #region player information

    //This briefly displays a message in large text in the middle of the screen
    public static void DisplayTitle(string title, int fontsize, string colour = "#FFFFFF")
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
                if (hud.title == null)
                {
                    GameObject locationInfo = GameObject.Find("LocationInfo");
                    if (locationInfo != null) { hud.title = locationInfo.GetComponent<Text>(); }
                }

                if (hud.reticule == null)
                {
                    GameObject reticule = GameObject.Find("Reticule");
                    if (reticule != null) { hud.reticule = reticule.GetComponent<RawImage>(); }
                }

                if (hud.title != null & hud.reticule != null)
                {
                    if (!colour.Contains("#"))
                    {
                        Color newColour = Color.white;

                        if (ColorUtility.TryParseHtmlString(colour, out newColour))
                        {
                            //Do nothing
                        }

                        hud.title.color = newColour;
                    }

                    hud.title.text = title;
                    hud.title.fontSize = fontsize;
                    Task a = new Task(FadeTextInAndOut(hud.title, 0.5f, 3, 0.5f)); //This fades the title in and out
                    AddTaskToPool(hud, a);
                    Task b = new Task(FadeRawImageOutAndIn(hud.reticule, 0.25f, 4, 0.5f)); //This briefly fades the hud reticule to avoid a clash with the title
                    AddTaskToPool(hud, b);
                }
            }
        } 
    }

    //This briefly displays a message in large text in the middle of the screen
    public static void DisplayHint(string hintText)
    {
        if (Time.timeScale != 0)
        {
            Hud hud = GetHud();

            if (hud != null)
            {
                if (hud.hintTextGO == null)
                {
                    GameObject hintTextGO = GameObject.Find("HintText");
                    if (hintTextGO != null) { hud.hintTextGO = hintTextGO.GetComponent<Text>(); }
                }

                if (hud.hintTextGO != null)
                {
                    hud.hintTextGO.text = hintText;
                    Task a = new Task(FadeTextInAndOut(hud.hintTextGO, 0.5f, 10, 0.5f)); //This fades the text in and out
                    AddTaskToPool(hud, a);
                }

                if (hud.hintImage == null)
                {
                    GameObject hintImage = GameObject.Find("Hint");
                    if (hintImage != null) { hud.hintImage = hintImage.GetComponent<Image>(); }
                }

                if (hud.hintImage != null)
                {
                    Task a = new Task(FadeImageInAndOut(hud.hintImage, 0.5f, 10, 0.5f)); //This fades the background image in and out
                    AddTaskToPool(hud, a);
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

        if (hud.smallShip != null)
        {
            if (hud.movingReticule != null & hud.centerReticule != null & hud.smallShip != null & Time.timeScale != 0 & hud.smallShip.keyboardAndMouse == true)
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

                //This fades the mouse reticle as it gets closer to the center of the scene
                if (hud.movingReticleImage != null)
                {
                    float distance = Vector2.Distance(hud.movingReticule.transform.position, hud.centerReticule.transform.position);

                    if (distance < 200 & distance > 10)
                    {
                        float alpha = (1f / 190f) * distance;

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
            else
            {
                hud.movingReticule.SetActive(false);
            }
        }
        else
        {
            hud.movingReticule.SetActive(false);
        }


    }

    #endregion

    #region keyboard and controller tabs

    public static void ToggleTabs(Hud hud)
    {
        if (hud.keyboardTags == null)
        {
            hud.keyboardTags = GameObjectUtils.FindAllChildTransformsContaining(hud.transform, "Tab_Keyboard");
        }

        if (hud.controllerTags == null)
        {
            hud.controllerTags = GameObjectUtils.FindAllChildTransformsContaining(hud.transform, "Tab_Controller");
        }

        if (hud.smallShip != null)
        {
            if (hud.smallShip.keyboardAndMouse == true & hud.keyboardActive == false)
            {
                foreach (Transform tag in hud.keyboardTags)
                {
                    tag.gameObject.SetActive(true);
                }

                foreach (Transform tag in hud.controllerTags)
                {
                    tag.gameObject.SetActive(false);
                }

                hud.keyboardActive = true;
            }
            else if (hud.smallShip.keyboardAndMouse == false & hud.keyboardActive == true)
            {
                foreach (Transform tag in hud.keyboardTags)
                {
                    tag.gameObject.SetActive(false);
                }

                foreach (Transform tag in hud.controllerTags)
                {
                    tag.gameObject.SetActive(true);
                }

                hud.keyboardActive = false;
            }
        }

    }

    #endregion

    #region fade and flashes

    //Fades in from designated colour
    public static void FadeInBackground(float time, string colour)
    {
        CanvasGroup fadeGroup = null;
        RawImage fadeImage = null;

        //This gets the references
        GameObject fadeImageGO = GameObject.Find("FadeImage");
        if (fadeImageGO != null) { fadeImage = fadeImageGO.GetComponent<RawImage>(); }
        if (fadeImageGO != null) { fadeGroup = fadeImageGO.GetComponentInParent<CanvasGroup>();  }

        //This changes the colour
        Color newColour;

        if (ColorUtility.TryParseHtmlString(colour, out newColour))
        {
            //Do nothing
        }

        fadeImage.color = newColour;

        //This fades the fade in
        if (fadeGroup != null & fadeImage != null)
        {
            Task a = new Task(MainMenuFunctions.FadeInCanvas(fadeGroup, 1));
        }
    }

    //Fades out from designated colour
    public static void FadeOutBackground(float time, string colour)
    {
        CanvasGroup fadeGroup = null;
        RawImage fadeImage = null;

        //This gets the references
        GameObject fadeImageGO = GameObject.Find("FadeImage");
        if (fadeImageGO != null) { fadeImage = fadeImageGO.GetComponent<RawImage>(); }
        if (fadeImageGO != null) { fadeGroup = fadeImageGO.GetComponentInParent<CanvasGroup>(); }

        //This changes the colour of the fade
        Color newColour;

        if (ColorUtility.TryParseHtmlString(colour, out newColour))
        {
            //Do nothing
        }

        fadeImage.color = newColour;

        //This fades the fade out
        if (fadeGroup != null & fadeImage != null)
        {
            Task a = new Task(MainMenuFunctions.FadeOutCanvas(fadeGroup, 1));
        }
    }

    #endregion

    #region unload hud and hud assets

    //This calls the unloading function before unloading the hud object itself
    public static void UnloadHud()
    {
        EndAllTasks();

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
        Hud hud = GetHud();
        Task a = new Task(FadeInText(text, fadeintime));
        AddTaskToPool(hud, a);
        while (a.Running == true) { yield return null; }
        yield return new WaitForSeconds(holdtime);
        Task b = new Task(FadeOutText(text, fadeOuttime));
        AddTaskToPool(hud, b);
        while (b.Running == true) { yield return null; }
    }

    //This fades out a raw image
    public static IEnumerator FadeOutRawImage(RawImage rawimage, float duration)
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

    //This fades in a raw image
    public static IEnumerator FadeInRawImage(RawImage rawimage, float duration)
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

    //This fades out an image
    public static IEnumerator FadeOutImage(Image image, float duration)
    {
        float alpha = 1;

        while (alpha > 0)
        {
            alpha = alpha - (1f / (60f * duration));

            if (image != null)
            {
                Color newColor = image.color;
                newColor.a = alpha;
                image.color = newColor;
            }

            yield return new WaitForSecondsRealtime(0.016f);
        }
    }

    //This fades in an image
    public static IEnumerator FadeInImage(Image image, float duration)
    {
        float alpha = 0;

        while (alpha < 1)
        {
            alpha = alpha + (1f / (60f * duration));

            if (image != null)
            {
                Color newColor = image.color;
                newColor.a = alpha;
                image.color = newColor;
            }

            yield return new WaitForSecondsRealtime(0.016f);
        }

    }

    //This fades a raw image in and out
    public static IEnumerator FadeRawImageInAndOut(RawImage rawimage, float fadeintime = 0.5f, float holdtime = 1, float fadeOuttime = 0.5f)
    {
        Hud hud = GetHud();
        Task a = new Task(FadeInRawImage(rawimage, fadeintime));
        AddTaskToPool(hud, a);
        while (a.Running == true) { yield return null; }
        yield return new WaitForSeconds(holdtime);
        Task b = new Task(FadeOutRawImage(rawimage, fadeOuttime));
        AddTaskToPool(hud, b);
        while (b.Running == true) { yield return null; }
    }

    //This fades an image in out
    public static IEnumerator FadeImageInAndOut(Image image, float fadeintime = 0.5f, float holdtime = 1, float fadeOuttime = 0.5f)
    {
        Hud hud = GetHud();
        Task a = new Task(FadeInImage(image, fadeintime));
        AddTaskToPool(hud, a);
        while (a.Running == true) { yield return null; }
        yield return new WaitForSeconds(holdtime);
        Task b = new Task(FadeOutImage(image, fadeOuttime));
        AddTaskToPool(hud, b);
        while (b.Running == true) { yield return null; }
    }

    //This fades an image in and out
    public static IEnumerator FadeRawImageOutAndIn(RawImage rawimage, float fadeintime = 0.5f, float holdtime = 1, float fadeOuttime = 0.5f)
    {
        Hud hud = GetHud();
        Task a = new Task(FadeOutRawImage(rawimage, fadeOuttime));
        AddTaskToPool(hud, a);
        while (a.Running == true) { yield return null; }
        yield return new WaitForSeconds(holdtime);
        Task b = new Task(FadeInRawImage(rawimage, fadeintime));
        AddTaskToPool(hud, b);
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

            //System text
            if (hud.systemsText == null)
            {
                GameObject systemsText = GameObject.Find("SystemsText");
                if (systemsText != null) { hud.systemsText = systemsText.GetComponent<Text>(); }
            }

            if (hud.systemsText != null)
            {
                Text text = hud.systemsText.GetComponent<Text>();

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

            //Target shield text
            if (hud.targetSystemsText == null)
            {
                GameObject targetSystemsText = GameObject.Find("TargetSystemsText");
                if (targetSystemsText != null) { hud.targetShieldsText = targetSystemsText.GetComponent<Text>(); }
            }

            if (hud.targetSystemsText != null)
            {
                Text text = hud.targetSystemsText.GetComponent<Text>();

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

    //This manually sets the alpha of the hud, the value must be between 0 and 1
    public static void SetHudTransparency(float alpha)
    {
        //This fixes any wrongly inputed values
        if (alpha > 1)
        {
            alpha = 1;
        }
        else if (alpha < 0)
        {
            alpha = 0;
        }

        //This applies the alpha to the canvas group
        GameObject hud = GetHudGameObject();
        CanvasGroup canvasGroup = hud.GetComponent<CanvasGroup>();

        canvasGroup.alpha = alpha;
    }

    #endregion

    #region scene task manager

    //This adds a task to the pool
    public static void AddTaskToPool(Hud hud, Task task)
    {
        if (hud.tasks == null)
        {
            hud.tasks = new List<Task>();
        }

        hud.tasks.Add(task);
    }

    //This ends all task in the ppol
    public static void EndAllTasks()
    {
        Hud hud = GetHud();

        if (hud != null)
        {
            if (hud.tasks != null)
            {
                foreach (Task task in hud.tasks)
                {
                    if (task != null)
                    {
                        task.Stop();
                    }
                }
            }
        }
    }

    #endregion

}
