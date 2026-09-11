using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public static class HangarLaunchFunctions
{
    //This displays the hangar launch
    public static void DisplayHangarLaunch(bool launchShip, string hangarName, string shipName, string displayShip01 = "", string displayShip02 = "", string displayShip03 = "", string displayShip04 = "", string displayShip05 = "", string displayShip06 = "", string displayShip07 = "", string displayShip08 = "")
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
                else if (t.name == "shipPosition01")
                {
                    hangarLaunch.displayShip01 = t.gameObject;
                }
                else if (t.name == "shipPosition02")
                {
                    hangarLaunch.displayShip02 = t.gameObject;
                }
                else if (t.name == "shipPosition03")
                {
                    hangarLaunch.displayShip03 = t.gameObject;
                }
                else if (t.name == "shipPosition04")
                {
                    hangarLaunch.displayShip04 = t.gameObject;
                }
                else if (t.name == "shipPosition05")
                {
                    hangarLaunch.displayShip05 = t.gameObject;
                }
                else if (t.name == "shipPosition06")
                {
                    hangarLaunch.displayShip06 = t.gameObject;
                }
                else if (t.name == "shipPosition07")
                {
                    hangarLaunch.displayShip07 = t.gameObject;
                }
                else if (t.name == "shipPosition08")
                {
                    hangarLaunch.displayShip08 = t.gameObject;
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

            ShipType shipType = GetShipType(shipName);

            //This loads the main ship
            GameObject ship = SceneFunctions.InstantiateShipPrefab(shipType.prefab);

            //This positions the ship and stores its position
            if (ship != null & hangarLaunch.startlocation != null & hangarLaunch.endlocation != null)
            {
                SceneFunctions.ScaleGameObjectByZAxis(ship, shipType.shipLength);
                ship.transform.parent = hangar.transform;

                DockShipToPosition(ship, hangarLaunch.groundlocation.transform.position);

                //ship.transform.position = hangarLaunch.groundlocation.transform.position;
                ship.transform.rotation = hangarLaunch.groundlocation.transform.rotation;
                ship.layer = 5;
                GameObjectUtils.SetLayerAllChildren(ship.transform, 5);
                hangarLaunch.ship = ship;
                hangarLaunch.camera.transform.LookAt(hangarLaunch.ship.transform.position);

                CloseWings(ship);

                //This gets the cockpit position and instantiates the cockpit 
                Transform cockpitPosiition = GameObjectUtils.FindChildTransformCalled(ship.transform, "camera");

                foreach (GameObject objectPrefab in scene.cockpitPrefabPool)
                {
                    if (objectPrefab.name == shipType.cockpitPrefab)
                    {
                        GameObject cockpit = GameObject.Instantiate(objectPrefab) as GameObject;

                        if (cockpitPosiition != null)
                        {
                            cockpit.transform.position = cockpitPosiition.position;
                        }
                        else
                        {
                            cockpit.transform.position = ship.transform.position;
                        }

                        cockpit.transform.parent = ship.transform;
                        cockpit.transform.localRotation = Quaternion.identity;
                        cockpit.SetActive(false);

                        hangarLaunch.cockpit = cockpit;
                    }
                }
            }

            //This loads the diplay ships

            //Get ships type
            ShipType displayShipType01 = GetShipType(displayShip01);
            ShipType displayShipType02 = GetShipType(displayShip02);
            ShipType displayShipType03 = GetShipType(displayShip03);
            ShipType displayShipType04 = GetShipType(displayShip04);
            ShipType displayShipType05 = GetShipType(displayShip05);
            ShipType displayShipType06 = GetShipType(displayShip06);
            ShipType displayShipType07 = GetShipType(displayShip07);
            ShipType displayShipType08 = GetShipType(displayShip08);

            //Instantiate ships
            GameObject displayShip01GO = SceneFunctions.InstantiateShipPrefab(displayShipType01.prefab);
            GameObject displayShip02GO = SceneFunctions.InstantiateShipPrefab(displayShipType02.prefab);
            GameObject displayShip03GO = SceneFunctions.InstantiateShipPrefab(displayShipType03.prefab);
            GameObject displayShip04GO = SceneFunctions.InstantiateShipPrefab(displayShipType04.prefab);
            GameObject displayShip05GO = SceneFunctions.InstantiateShipPrefab(displayShipType05.prefab);
            GameObject displayShip06GO = SceneFunctions.InstantiateShipPrefab(displayShipType06.prefab);
            GameObject displayShip07GO = SceneFunctions.InstantiateShipPrefab(displayShipType07.prefab);
            GameObject displayShip08GO = SceneFunctions.InstantiateShipPrefab(displayShipType08.prefab);

            //Set position of ships to hangar
            DockShipToPosition(displayShip01GO, hangarLaunch.displayShip01.transform.position);
            DockShipToPosition(displayShip02GO, hangarLaunch.displayShip02.transform.position);
            DockShipToPosition(displayShip03GO, hangarLaunch.displayShip03.transform.position);
            DockShipToPosition(displayShip04GO, hangarLaunch.displayShip04.transform.position);
            DockShipToPosition(displayShip05GO, hangarLaunch.displayShip05.transform.position);
            DockShipToPosition(displayShip06GO, hangarLaunch.displayShip06.transform.position);
            DockShipToPosition(displayShip07GO, hangarLaunch.displayShip07.transform.position);
            DockShipToPosition(displayShip08GO, hangarLaunch.displayShip08.transform.position);

            //Set rotation of ships in hangar
            displayShip01GO.transform.rotation = hangarLaunch.displayShip01.transform.rotation;
            displayShip02GO.transform.rotation = hangarLaunch.displayShip02.transform.rotation;
            displayShip03GO.transform.rotation = hangarLaunch.displayShip03.transform.rotation;
            displayShip04GO.transform.rotation = hangarLaunch.displayShip04.transform.rotation;
            displayShip05GO.transform.rotation = hangarLaunch.displayShip05.transform.rotation;
            displayShip06GO.transform.rotation = hangarLaunch.displayShip06.transform.rotation;
            displayShip07GO.transform.rotation = hangarLaunch.displayShip07.transform.rotation;
            displayShip08GO.transform.rotation = hangarLaunch.displayShip08.transform.rotation;

            //Close wings on ships
            CloseWings(displayShip01GO);
            CloseWings(displayShip02GO);
            CloseWings(displayShip03GO);
            CloseWings(displayShip04GO);
            CloseWings(displayShip05GO);
            CloseWings(displayShip06GO);
            CloseWings(displayShip07GO);
            CloseWings(displayShip08GO);

            //Parent ships to hangar
            displayShip01GO.transform.parent = hangar.transform;
            displayShip02GO.transform.parent = hangar.transform;
            displayShip03GO.transform.parent = hangar.transform;
            displayShip04GO.transform.parent = hangar.transform;
            displayShip05GO.transform.parent = hangar.transform;
            displayShip06GO.transform.parent = hangar.transform;
            displayShip07GO.transform.parent = hangar.transform;
            displayShip08GO.transform.parent = hangar.transform;

            //This sets the ships to the ui layer so they can be seen
            displayShip01GO.layer = 5;
            displayShip02GO.layer = 5;
            displayShip03GO.layer = 5;
            displayShip04GO.layer = 5;
            displayShip05GO.layer = 5;
            displayShip06GO.layer = 5;
            displayShip07GO.layer = 5;
            displayShip08GO.layer = 5;

            GameObjectUtils.SetLayerAllChildren(displayShip01GO.transform, 5);
            GameObjectUtils.SetLayerAllChildren(displayShip02GO.transform, 5);
            GameObjectUtils.SetLayerAllChildren(displayShip03GO.transform, 5);
            GameObjectUtils.SetLayerAllChildren(displayShip04GO.transform, 5);
            GameObjectUtils.SetLayerAllChildren(displayShip05GO.transform, 5);
            GameObjectUtils.SetLayerAllChildren(displayShip06GO.transform, 5);
            GameObjectUtils.SetLayerAllChildren(displayShip07GO.transform, 5);
            GameObjectUtils.SetLayerAllChildren(displayShip08GO.transform, 5);
        }

        //This makes the hud invisible
        HudFunctions.SetHudTransparency(0);

        //This sets up the scene for either landing or launching
        if (launchShip == true)
        {
            if (hangarLaunch.cockpit != null)
            {
                hangarLaunch.ship.layer = 0;
                GameObjectUtils.SetLayerAllChildren(hangarLaunch.ship.transform, 0);
                hangarLaunch.cockpit.layer = 5;
                GameObjectUtils.SetLayerAllChildren(hangarLaunch.cockpit.transform, 5);

                hangarLaunch.camera.transform.SetParent(hangarLaunch.cockpit.transform);
                hangarLaunch.camera.transform.localPosition = Vector3.zero;
                hangarLaunch.camera.transform.localRotation = Quaternion.identity;
                hangarLaunch.cockpit.SetActive(true);
            }
            else
            {
                hangarLaunch.camera.transform.parent = hangarLaunch.hangar.transform;
                hangarLaunch.camera.transform.position = hangarLaunch.cameralocation.transform.position;
                hangarLaunch.camera.transform.rotation = hangarLaunch.cameralocation.transform.rotation;
            }
        }
        else
        {
            hangarLaunch.shipLaunching = false;
            Task a = new Task(LandShip(hangarLaunch));
        }
    }

    //Helper functions for setting up ships in hangar
    public static void DockShipToPosition(GameObject ship, Vector3 targetPosition)
    {
        // Find the dockingPoint01 child object
        Transform dockingPoint = GameObjectUtils.FindChildTransformContaining(ship.transform, "docking");

        if (dockingPoint != null)
        {
            // Calculate the offset between the ship and its docking point
            Vector3 offset = dockingPoint.position - ship.transform.position;

            // Move the ship so that dockingPoint01 is at the target position
            ship.transform.position = targetPosition - offset;
        }
    }

    public static void CloseWings(GameObject ship)
    {
        Transform[] wings = GameObjectUtils.FindAllChildTransformsContaining(ship.transform, "wing");

        GameObject wing01 = null;
        GameObject wing02 = null;
        GameObject wing03 = null;
        GameObject wing04 = null;

        GameObject wing01_open = null;
        GameObject wing02_open = null;
        GameObject wing03_open = null;
        GameObject wing04_open = null;

        GameObject wing01_closed = null;
        GameObject wing02_closed = null;
        GameObject wing03_closed = null;
        GameObject wing04_closed = null;

        foreach (Transform wing in wings)
        {
            if (wing.name == "wing01")
            {
                wing01 = wing.gameObject;
            }
            else if (wing.name == "wing02")
            {
                wing02 = wing.gameObject;
            }
            else if (wing.name == "wing03")
            {
                wing03 = wing.gameObject;
            }
            else if (wing.name == "wing04")
            {
                wing04 = wing.gameObject;
            }
            else if (wing.name == "wing01_open")
            {
                wing01_open = wing.gameObject;
            }
            else if (wing.name == "wing02_open")
            {
                wing02_open = wing.gameObject;
            }
            else if (wing.name == "wing03_open")
            {
                wing03_open = wing.gameObject;
            }
            else if (wing.name == "wing04_open")
            {
                wing04_open = wing.gameObject;
            }
            else if (wing.name == "wing01_closed")
            {
                wing01_closed = wing.gameObject;
            }
            else if (wing.name == "wing02_closed")
            {
                wing02_closed = wing.gameObject;
            }
            else if (wing.name == "wing03_closed")
            {
                wing03_closed = wing.gameObject;
            }
            else if (wing.name == "wing04_closed")
            {
                wing04_closed = wing.gameObject;
            }
        }

        if (wing01 != null & wing01_open != null & wing01_closed != null)
        {
            SnapToWingPosition(wing01, wing01_open.transform, wing01_closed.transform, 2, false);
        }

        if (wing02 != null & wing02_open != null & wing02_closed != null)
        {
            SnapToWingPosition(wing02, wing02_open.transform, wing02_closed.transform, 2, false);
        }

        if (wing03 != null & wing03_open != null & wing03_closed != null)
        {
            SnapToWingPosition(wing03, wing03_open.transform, wing03_closed.transform, 2, false);
        }

        if (wing04 != null & wing04_open != null & wing04_closed != null)
        {
            SnapToWingPosition(wing04, wing04_open.transform, wing04_closed.transform, 2, false);
        }

    }

    public static void SnapToWingPosition(GameObject wing, Transform openPosition, Transform closePosition, float speed, bool open)
    {
        Quaternion startRotation = closePosition.localRotation;
        Quaternion endRotation = openPosition.localRotation;

        if (open == false)
        {
            startRotation = openPosition.localRotation;
            endRotation = closePosition.localRotation;
        }

        wing.transform.localRotation = endRotation;
    }

    public static ShipType GetShipType(string shipName)
    {
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

        return shipType;
    }

    //This plays the launch/landing cutscene
    public static void PlayHangarCutscene(HangarLaunch hangarLaunch)
    {
        if (hangarLaunch.shipLaunching == true)
        {
            Task a = new Task(LaunchShip(hangarLaunch));
        }
        else
        {
            HangarLaunchFunctions.CloseHangarLaunch(hangarLaunch);
        }
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

        float wobbleSpeed = 3;
        float wobbleRange = 2;

        while (timeElapsedA < lerpDurationA)
        {
            if (hangarLaunch.ship != null)
            {
                if (hangarLaunch.cockpit == null)
                {
                    hangarLaunch.camera.transform.LookAt(hangarLaunch.ship.transform.position);
                }

                // This lerps the ship between two positions
                hangarLaunch.ship.transform.position = Vector3.Lerp(groundlocation, startPosition, timeElapsedA / lerpDurationA);

                // Add rotational wobble
                float wobbleAmount = Mathf.Sin(timeElapsedA * wobbleSpeed) * wobbleRange;
                hangarLaunch.ship.transform.rotation = Quaternion.Euler(0, 180, wobbleAmount);

                timeElapsedA += Time.unscaledDeltaTime;

                yield return new WaitForEndOfFrame();
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
                timeElapsedB += Time.unscaledDeltaTime;

                //This transitions the camera
                if (cameraTransition == false)
                {
                    if (hangarLaunch.cockpit != null)
                    {
                        hangarLaunch.cockpit.SetActive(false);
                        hangarLaunch.ship.layer = 5;
                        GameObjectUtils.SetLayerAllChildren(hangarLaunch.ship.transform, 5);
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

                yield return new WaitForEndOfFrame();
            }
        }

        //This resets the fade to transparent once the cutscene is finished 
        HudFunctions.SetBackgroundAlphaAndColour(0, colour);

        HangarLaunchFunctions.CloseHangarLaunch(hangarLaunch);
    }

    //This lands the ship
    public static IEnumerator LandShip(HangarLaunch hangarLaunch)
    {
        hangarLaunch.launchbutton.SetActive(false);

        Vector3 groundlocation = hangarLaunch.groundlocation.transform.position;
        Vector3 startPosition = hangarLaunch.startlocation.transform.position;
        Vector3 endPosition = hangarLaunch.endlocation.transform.position;

        float timeElapsedB = 0;
        float lerpDurationB = 8;
        bool cameraTransition = false;
        bool fade = false;
        string colour = "#000000";

        while (timeElapsedB < lerpDurationB)
        {
            if (hangarLaunch.ship != null)
            {
                //This lerps the ship between two positions
                hangarLaunch.camera.transform.LookAt(hangarLaunch.ship.transform.position);
                hangarLaunch.ship.transform.position = Vector3.Lerp(endPosition, startPosition, timeElapsedB / lerpDurationB);
                hangarLaunch.ship.transform.rotation = Quaternion.Euler(0, 0, 0);
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

                yield return new WaitForEndOfFrame();
            }
        }

        //This lands the ship and activates the cockpit if available
        float timeElapsedA = 0;
        float lerpDurationA = 4;

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
            hangarLaunch.camera.transform.position = hangarLaunch.cameralocation.transform.position;
            hangarLaunch.camera.transform.rotation = hangarLaunch.cameralocation.transform.rotation;
        }

        float wobbleSpeed = 3;
        float wobbleRange = 2;

        while (timeElapsedA < lerpDurationA)
        {
            if (hangarLaunch.ship != null)
            {
                if (hangarLaunch.cockpit == null)
                {
                    hangarLaunch.camera.transform.LookAt(hangarLaunch.ship.transform.position);
                }
                else
                {
                    if (hangarLaunch.cockpit.activeSelf == false)
                    {
                        hangarLaunch.camera.transform.SetParent(hangarLaunch.cockpit.transform);
                        hangarLaunch.camera.transform.localPosition = Vector3.zero;
                        hangarLaunch.camera.transform.localRotation = Quaternion.identity;
                        hangarLaunch.cockpit.SetActive(true);
                    }
                }

                // This lerps the ship between two positions
                hangarLaunch.ship.transform.position = Vector3.Lerp(startPosition, groundlocation, timeElapsedA / lerpDurationA);

                // Add rotational wobble
                float wobbleAmount = Mathf.Sin(timeElapsedA * wobbleSpeed) * wobbleRange;
                hangarLaunch.ship.transform.rotation = Quaternion.Euler(0, 0, wobbleAmount);

                timeElapsedA += Time.unscaledDeltaTime;

                yield return new WaitForEndOfFrame();
            }
        }

        hangarLaunch.launchbutton.SetActive(true);
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
