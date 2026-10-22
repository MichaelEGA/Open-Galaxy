using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public static class TargetingDisplayFunctions
{
    #region target display loading and unloading functions

    //This loads the targeting display
    public static TargetingDisplay LoadTargetingDisplay()
    {
        //Load tageting display prefab
        GameObject targetDisplayPrefab = Resources.Load(OGGetAddress.hud + "TargetingDisplay") as GameObject;
        GameObject targetingDisplayGO = GameObject.Instantiate(targetDisplayPrefab);
        TargetingDisplay targetingDisplay = targetingDisplayGO.AddComponent<TargetingDisplay>();

        //Create targeting display Camera
        GameObject targetingDisplayCameraGO = new GameObject();
        Camera targetingDisplayCamera = null;

        targetingDisplayCameraGO = new GameObject();
        targetingDisplayCameraGO.name = "TargetingDisplayCamera";
        targetingDisplayCamera = targetingDisplayCameraGO.AddComponent<Camera>();
        targetingDisplayCamera.nearClipPlane = 0.01f;
        targetingDisplayCamera.cullingMask = LayerMask.GetMask("targetingDisplay");
        var starfieldCameraData = targetingDisplayCamera.GetUniversalAdditionalCameraData();
        starfieldCameraData.renderPostProcessing = true;

        //Add targeting display camera reference to script
        targetingDisplay.targetingSystemCamera = targetingDisplayCamera;

        //Set canvas to camera
        targetingDisplay.targetingDisplayCanvas = targetingDisplay.GetComponent<Canvas>();
        targetingDisplay.targetingDisplayCanvas.worldCamera = targetingDisplayCamera;

        return targetingDisplay;
    }

    //This prepares the targeting display
    public static void PrepareTargetingDisplay(TargetingDisplay targetingDisplay)
    {
        Transform[] targetingDisplayTransforms = GameObjectUtils.GetAllChildTransforms(targetingDisplay.gameObject.transform);

        foreach (Transform displayTransforms in targetingDisplayTransforms)
        {
            //Get Canvas Groups
            if (displayTransforms.name == "MainScreen")
            {
                targetingDisplay.mainScreen = displayTransforms.GetComponent<CanvasGroup>();
            }

            if (displayTransforms.name == "Targeting")
            {
                targetingDisplay.targeting = displayTransforms.GetComponent<CanvasGroup>();
            }

            if (displayTransforms.name == "Messages")
            {
                targetingDisplay.messages = displayTransforms.GetComponent<CanvasGroup>();
            }

            if (displayTransforms.name == "Objectives")
            {
                targetingDisplay.objectives = displayTransforms.GetComponent<CanvasGroup>();
            }

            if (displayTransforms.name == "NotificationBoxSmall")
            {
                targetingDisplay.NotificationBoxSmall = displayTransforms.GetComponent<CanvasGroup>();
            }

            if (displayTransforms.name == "NotificationBoxLarge")
            {
                targetingDisplay.NotificationBoxLarge = displayTransforms.GetComponent<CanvasGroup>();
            }

            //Get Textboxes
            if (displayTransforms.name == "TopBar")
            {
                targetingDisplay.topBar = displayTransforms.GetComponent<Text>();
            }

            if (displayTransforms.name == "BottomBar")
            {
                targetingDisplay.bottomBar = displayTransforms.GetComponent<Text>();
            }

            if (displayTransforms.name == "TargetingText_Type")
            {
                targetingDisplay.targetingText_Type = displayTransforms.GetComponent<Text>();
            }

            if (displayTransforms.name == "TargetingText_Name")
            {
                targetingDisplay.targetingText_Name = displayTransforms.GetComponent<Text>();
            }

            if (displayTransforms.name == "TargetingText_Cargo")
            {
                targetingDisplay.targetingText_Cargo = displayTransforms.GetComponent<Text>();
            }

            if (displayTransforms.name == "TargetingText_Speed")
            {
                targetingDisplay.targetingText_Speed = displayTransforms.GetComponent<Text>();
            }

            if (displayTransforms.name == "TargetingText_Distance")
            {
                targetingDisplay.targetingText_Distance = displayTransforms.GetComponent<Text>();
            }

            if (displayTransforms.name == "TargetingText_Shield")
            {
                targetingDisplay.targetingText_Shield = displayTransforms.GetComponent<Text>();
            }

            if (displayTransforms.name == "TargetingText_Hull")
            {
                targetingDisplay.targetingText_Hull = displayTransforms.GetComponent<Text>();
            }

            if (displayTransforms.name == "TargetingText_System")
            {
                targetingDisplay.targetingText_System = displayTransforms.GetComponent<Text>();
            }

            if (displayTransforms.name == "MessagesText")
            {
                targetingDisplay.messagesText = displayTransforms.GetComponent<Text>();
            }

            if (displayTransforms.name == "ObjectivesText")
            {
                targetingDisplay.objectivesText = displayTransforms.GetComponent<Text>();
            }

            if (displayTransforms.name == "NotBoxSmallText")
            {
                targetingDisplay.notBoxSmallText = displayTransforms.GetComponent<Text>();
            }

            if (displayTransforms.name == "notBoxLargeText")
            {
                targetingDisplay.notBoxLargeText = displayTransforms.GetComponent<Text>();
            }
        }
    }

    public static void UnloadTargetingDisplay()
    {

    }

    #endregion

    #region

    //This updates the ship variables
    public static void UpdateShipVaribles(TargetingDisplay targetingDisplay)
    {
        Ship ship = targetingDisplay.ship;

        if (ship != null)
        {
            Text topBarText = targetingDisplay.topBar;
            Text bottomBarText = targetingDisplay.bottomBar;

            //Get all values
            string weaponMode = "[" + ship.weaponManagement.weaponMode.ToUpper() + "]";
            string selectedWeapon = "[" + ship.weaponManagement.weaponType.ToUpper() + "]";
            string weaponNumber = "[" + ship.weaponManagement.toggleWeaponNumber.ToString().ToUpper() + "]";
            string wep = "[WEP:" + ship.flightControlSystem_Small.wep.ToString().ToUpper() + "]";
            string matchSpeed = "[MATCH SPD:" + ship.flightControlSystem_Small.matchSpeed.ToString().ToUpper() + "]";

            //Modify values
            if (selectedWeapon == "ion" || selectedWeapon == "lasers" || selectedWeapon == "plasma")
            {
                weaponNumber = "[INFINITY]";
            }

            topBarText.text = "--------------------------------------\r\nHOLLAND UNIFIED TARGETING SYSTEM \r\n" + weaponMode + "  " + selectedWeapon + " " + weaponNumber + "\r\n" + wep + " " + matchSpeed + "\r\n--------------------------------------";
        }
    }

    //This updates the target variables
    public static void UpdateTargetVariables()
    {

    }

    #endregion
}
