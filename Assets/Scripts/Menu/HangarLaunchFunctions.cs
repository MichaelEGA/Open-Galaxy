using System.Collections;
using UnityEngine;

public static class HangarLaunchFunctions
{
    //This displays the hangar launch
    public static void DisplayHangarLaunch(string hangarName, string shipName)
    {
        //This gets the scene reference
        Scene scene = SceneFunctions.GetScene();

        //This pauses the game
        MissionFunctions.PauseGame(false);

        //This loads the hangar
        GameObject hangarGO = Resources.Load<GameObject>("objects/hangar/hangar");
        GameObject hangar = GameObject.Instantiate(hangarGO) as GameObject;

        //This loads the hangar launch menu
        HangarLaunch hangarLaunch = GameObject.FindFirstObjectByType<HangarLaunch>();

        if (hangarLaunch == null)
        {
            //This loads the hangar menu and stores it
            GameObject hangarLaunchMenuPrefab = Resources.Load(OGGetAddress.menus + "HangarLaunch") as GameObject;
            GameObject hangarLaunchMenuGO = GameObject.Instantiate(hangarLaunchMenuPrefab);
            hangarLaunchMenuGO.name = "HangarLaunchMenu";
            hangarLaunch = hangarLaunchMenuGO.GetComponent<HangarLaunch>();
            scene.hangarLaunch = hangarLaunchMenuGO;

            //This stores the hanger gameobject
            hangarLaunch.hangar = hangar;

            Transform[] hangarTransforms = GameObjectUtils.GetAllChildTransforms(hangar.transform);

            foreach (Transform t in hangarTransforms)
            {
                if (t.name == "camera")
                {
                    hangarLaunch.camera = t.gameObject;
                }
                else if (t.name == "startlocation")
                {
                    hangarLaunch.startlocation = t.gameObject;
                }
                else if (t.name == "endlocation")
                {
                    hangarLaunch.endlocation = t.gameObject;
                }
                else if (t.name == "groundlocation")
                {
                    hangarLaunch.groundlocation = t.gameObject;
                }
                else if (t.name == "cameralocation")
                {
                    hangarLaunch.cameralocation = t.gameObject;
                }
            }

            Transform[] hangarMenuTransforms = GameObjectUtils.GetAllChildTransforms(hangarLaunchMenuGO.transform);

            foreach (Transform t in hangarMenuTransforms)
            {
                if (t.name == "LaunchShip")
                {
                    hangarLaunch.launchbutton = t.gameObject;
                }
            }

            //This get the ship information
            TextAsset shipTypesFile = Resources.Load(OGGetAddress.files + "ShipTypes") as TextAsset;
            ShipTypes shipTypes = JsonUtility.FromJson<ShipTypes>(shipTypesFile.text);

            ShipType shipType = null;

            foreach (ShipType tempShipType in shipTypes.shipTypeData)
            {
                if (tempShipType.type == shipName)
                {
                    shipType = tempShipType;
                    break;
                }
            }

            //This loads the ship
            GameObject ship = SceneFunctions.InstantiateShipPrefab(shipType.prefab);

            //This positions the ship and stores its position
            if (ship != null & hangarLaunch.startlocation != null & hangarLaunch.endlocation != null)
            {
                SceneFunctions.ScaleGameObjectByZAxis(ship, shipType.shipLength);
                ship.transform.parent = hangar.transform;
                ship.transform.position = hangarLaunch.groundlocation.transform.position;
                ship.transform.rotation = hangarLaunch.groundlocation.transform.rotation;
                ship.layer = 5;
                GameObjectUtils.SetLayerAllChildren(ship.transform, 5);
                hangarLaunch.ship = ship;
                hangarLaunch.camera.transform.LookAt(hangarLaunch.ship.transform.position);


                foreach (GameObject objectPrefab in scene.cockpitPrefabPool)
                {
                    if (objectPrefab.name == shipType.cockpitPrefab)
                    {
                        GameObject cockpit = GameObject.Instantiate(objectPrefab) as GameObject;
                        cockpit.transform.position = ship.transform.position;
                        cockpit.transform.parent = ship.transform;
                        cockpit.transform.localRotation = Quaternion.identity;
                        cockpit.SetActive(true);

                        ship.layer = 0;
                        GameObjectUtils.SetLayerAllChildren(ship.transform, 0);
                        cockpit.layer = 5;
                        GameObjectUtils.SetLayerAllChildren(cockpit.transform, 5);

                        hangarLaunch.cockpit = cockpit;

                        hangarLaunch.camera.transform.SetParent(hangarLaunch.cockpit.transform);
                        hangarLaunch.camera.transform.localPosition = Vector3.zero;
                        hangarLaunch.camera.transform.localRotation = Quaternion.identity;
                    }
                }
            }
        }

        //This makes the hud invisible
        HudFunctions.SetHudTransparency(0);
    }

    //This launches the ship
    public static IEnumerator LaunchShip(HangarLaunch hangarLaunch)
    {
        hangarLaunch.launchbutton.SetActive(false);

        Vector3 groundlocation = hangarLaunch.groundlocation.transform.position;
        Vector3 startPosition = hangarLaunch.startlocation.transform.position;
        Vector3 endPosition = hangarLaunch.endlocation.transform.position;

        float timeElapsedA = 0;
        float lerpDurationA = 2;

        if (hangarLaunch.cockpit != null)
        {
            hangarLaunch.ship.layer = 0;
            GameObjectUtils.SetLayerAllChildren(hangarLaunch.ship.transform, 0);
            hangarLaunch.cockpit.layer = 5;
            GameObjectUtils.SetLayerAllChildren(hangarLaunch.cockpit.transform, 5);
        }
        else
        {
            hangarLaunch.camera.transform.parent = hangarLaunch.hangar.transform;
            hangarLaunch.camera.transform.position = hangarLaunch.camera.transform.position;
            hangarLaunch.camera.transform.rotation = hangarLaunch.camera.transform.rotation;
        }

        while (timeElapsedA < lerpDurationA)
        {
            if (hangarLaunch.ship != null)
            {
                if (hangarLaunch.cockpit == null)
                {
                    hangarLaunch.camera.transform.LookAt(hangarLaunch.ship.transform.position);
                }

                //This lerps the ship between two positions
                hangarLaunch.ship.transform.position = Vector3.Lerp(groundlocation, startPosition, timeElapsedA / lerpDurationA);

                timeElapsedA += Time.unscaledDeltaTime;

                yield return null;
            }
        }

        float timeElapsedB = 0;
        float lerpDurationB = 4;
        bool cameraTransition = false;
        bool fade = false;
        string colour = "#000000";

        while (timeElapsedB < lerpDurationB)
        {
            if (hangarLaunch.ship != null)
            {
                //This lerps the ship between two positions
                hangarLaunch.camera.transform.LookAt(hangarLaunch.ship.transform.position);
                hangarLaunch.ship.transform.position = Vector3.Lerp(startPosition, endPosition, timeElapsedB / lerpDurationB);

                ////This makes the nose slightly dip as the ship exits
                Quaternion startRotation = hangarLaunch.ship.transform.rotation;
                Vector3 moveDir = (endPosition - startPosition).normalized;
                Quaternion forwardRotation = Quaternion.LookRotation(moveDir, Vector3.up);
                float maxPitchDegrees = 10f;
                AnimationCurve pitchCurve = null;
                bool useCurve = pitchCurve != null;
                float progress = Mathf.Clamp01(timeElapsedB / lerpDurationB);
                float pitchTiming = 0.4f;
                pitchTiming = Mathf.Clamp01(pitchTiming);
                float pitchProgress = Mathf.Clamp01(progress / Mathf.Max(pitchTiming, 1e-6f));
                float pitchFactor = useCurve ? pitchCurve.Evaluate(pitchProgress) : Mathf.Sin(pitchProgress * Mathf.PI);
                float pitchAngle = -maxPitchDegrees * -pitchFactor;
                Quaternion pitchRotation = Quaternion.Euler(pitchAngle, 0f, 0f);
                Quaternion targetRotationWithPitch = forwardRotation * pitchRotation;
                hangarLaunch.ship.transform.rotation = Quaternion.Slerp(startRotation, targetRotationWithPitch, progress);
                
                timeElapsedB += Time.unscaledDeltaTime;

                //This fades to black at end of cutscene
                if (timeElapsedB > 0.25f & cameraTransition == false)
                {
                    if (hangarLaunch.cockpit != null)
                    {
                        hangarLaunch.ship.layer = 5;
                        GameObjectUtils.SetLayerAllChildren(hangarLaunch.ship.transform, 5);
                        hangarLaunch.cockpit.SetActive(false);
                    }

                    hangarLaunch.camera.transform.parent = hangarLaunch.hangar.transform;
                    hangarLaunch.camera.transform.position = hangarLaunch.cameralocation.transform.position;
                    hangarLaunch.camera.transform.rotation = hangarLaunch.cameralocation.transform.rotation;

                    cameraTransition = true;
                }

                //This fades to black at end of cutscene
                if (timeElapsedB > 2f & fade == false)
                {
                    HudFunctions.FadeInBackground(0.5f, colour);

                    fade = true;
                }

                yield return null;
            }
        }

        //This resets the fade to transparent once the cutscene is finished 
        HudFunctions.SetBackgroundAlphaAndColour(0, colour);

        HangarLaunchFunctions.CloseHangarLaunch(hangarLaunch);
    }

    //This stops displaying the hangar launch
    public static void CloseHangarLaunch(HangarLaunch hangarLaunch)
    {
        Scene scene = SceneFunctions.GetScene();

        if (scene.hangarLaunch != null)
        {
            hangarLaunch = scene.hangarLaunch.GetComponent<HangarLaunch>();

            //This destroys the environment
            if (hangarLaunch != null)
            {
                if (hangarLaunch.hangar != null)
                {
                    GameObject.Destroy(hangarLaunch.hangar);
                }
            }

            scene.hangarLaunch.gameObject.SetActive(false);
        }

        //This makes the hud invisible
        HudFunctions.SetHudTransparency(1);

        MissionFunctions.ResumeGame();
    }
}
