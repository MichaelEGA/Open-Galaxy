using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ExitMenuFunctions
{
    //This display the exit menu
    public static void DisplayExitMenu(bool isDisplaying)
    {
        GameObject exitMenu = GameObject.Find("ExitMenu");

        if (isDisplaying == true)
        {

            if (exitMenu == null)
            {
                GameObject exitMenuPrefab = Resources.Load(OGGetAddress.menus + "ExitMenu") as GameObject;
                exitMenu = GameObject.Instantiate(exitMenuPrefab);
                exitMenu.name = "ExitMenu";
            }

            if (exitMenu != null)
            {
                Time.timeScale = 0;

                exitMenu.SetActive(true);

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                //This selects the button for when players are using the controller
                exitMenu.GetComponentInChildren<Button>().Select();
            }
        }
        else
        {
            Time.timeScale = 1;

            if (exitMenu != null)
            {
                exitMenu.SetActive(false);
            }

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
