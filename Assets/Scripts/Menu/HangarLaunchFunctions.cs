using UnityEngine;

public static class HangarLaunchFunctions
{
    public static void DisplayHangarLaunch()
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
            GameObject hangarLaunchMenuPrefab = Resources.Load(OGGetAddress.menus + "HangarLaunch") as GameObject;
            GameObject hangarLaunchMenuGO = GameObject.Instantiate(hangarLaunchMenuPrefab);
            hangarLaunchMenuGO.name = "HangarLaunchMenu";
            hangarLaunch = hangarLaunchMenuGO.GetComponent<HangarLaunch>();
            scene.hangarLaunch = hangarLaunchMenuGO;
            hangarLaunch.hangar = hangar;
        }

        //This makes the hud invisible
        HudFunctions.SetHudTransparency(0);
    }

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
