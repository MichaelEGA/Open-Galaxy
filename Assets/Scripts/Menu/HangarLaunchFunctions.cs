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
                    hangarLaunch.hangarCamera = t.gameObject;
                }
                else if (t.name == "startlocation")
                {
                    hangarLaunch.startlocation = t.gameObject;
                }
                else if (t.name == "endlocation")
                {
                    hangarLaunch.endlocation = t.gameObject;
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

            //This positions the ship
            if (ship != null & hangarLaunch.startlocation != null & hangarLaunch.endlocation != null)
            {
                ship.transform.parent = hangar.transform;
                ship.transform.position = hangarLaunch.startlocation.transform.position;
                ship.transform.rotation = hangarLaunch.startlocation.transform.rotation;
                ship.layer = 5;
                GameObjectUtils.SetLayerAllChildren(ship.transform, 5);
                Debug.Log("Was Run");
            }

        }

        //This makes the hud invisible
        HudFunctions.SetHudTransparency(0);
    }

    //This launches the ship
    public static IEnumerator LaunchShip(HangarLaunch hangarLaunch)
    {
        yield return null;

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
