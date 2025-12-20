using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

public static class CockpitFunctions
{
    //This runs all the cockpit functions
    public static void RunCockpitFunctions(SmallShip smallShip)
    {
        if (smallShip.isAI == false)
        {
            //Camera Functions
            SetMainCamera(smallShip);

            //Cockpit Functions
            ActivateCockpit(smallShip);
            RunCockpitShake(smallShip);
            CockpitCameraMovement(smallShip);
            CockpitAnchorRotation(smallShip);
        }
    }

    #region camera control

    //This function sets the camera to the ship position
    public static void SetMainCamera(SmallShip smallShip)
    {
        if (smallShip.isAI == false & smallShip.cameraAttached == false)
        {
            GameObject mainCamera = smallShip.mainCamera;

            if (mainCamera == null & smallShip.hullLevel > 0)
            {
                mainCamera = GameObject.Find("Main Camera");
                smallShip.mainCamera = mainCamera;
            }

            if (mainCamera != null & smallShip != null)
            {
                if (smallShip.cameraPosition == null)
                {
                    Transform cameraPos = GameObjectUtils.FindChildTransformCalled(smallShip.gameObject.transform, "camera");

                    if (cameraPos != null)
                    {
                        smallShip.cameraPosition = cameraPos.gameObject;
                    }
                    else
                    {
                        smallShip.cameraPosition = smallShip.gameObject;
                    }
                }
                else if (mainCamera.transform.parent != smallShip.cameraPosition.transform)
                {
                    mainCamera.transform.position = smallShip.cameraPosition.transform.position;
                    mainCamera.transform.rotation = smallShip.cameraPosition.transform.rotation;
                    mainCamera.transform.SetParent(smallShip.cameraPosition.transform);
                    smallShip.cameraAttached = true;
                    smallShip.cameraLocalPosition = smallShip.cameraPosition.transform.localPosition;
                }
            }
        }
    }

    //This removes the main camera (i.e. when the ship is destroyed or the camera switches to another ship)
    public static void RemoveMainCamera(SmallShip smallShip)
    {
        if (smallShip.mainCamera != null)
        {
            smallShip.mainCamera.transform.parent = null;
            smallShip.mainCamera = null;
            smallShip.cameraAttached = false;
        }
    }

    //This runs the movement of the third person camera
    public static void FollowCamera(SmallShip smallShip)
    {
        if (smallShip != null)
        {
            if (smallShip.isAI == false)
            {
                var keyboard = Keyboard.current;
                var controller = Gamepad.current;

                if (keyboard != null)
                {
                    if (keyboard.f1Key.isPressed == true & Time.time > smallShip.toggleCameraPressTime + 0.5f)
                    {
                        //if (smallShip.scene.followCameraIsActive == false)
                        //{
                        //    //SceneFunctions.ActivateFollowCamera(true);
                        //}
                        //else
                        //{
                        //    //SceneFunctions.ActivateFollowCamera(false);
                        //}

                        smallShip.toggleCameraPressTime = Time.time;
                    }
                }

                if (controller != null)
                {
                    if (controller.rightStickButton.isPressed == true & Time.time > smallShip.toggleCameraPressTime + 0.5f)
                    {
                        //if (smallShip.scene.followCameraIsActive == false)
                        //{
                        //    //SceneFunctions.ActivateFollowCamera(true);
                        //}
                        //else
                        //{
                        //   // SceneFunctions.ActivateFollowCamera(false);
                        //}

                        smallShip.toggleCameraPressTime = Time.time;
                    }
                }


                if (smallShip.followCamera == null)
                {
                    Scene scene = SceneFunctions.GetScene();

                    //if (scene.followCamera != null)
                    //{
                    //    smallShip.followCamera = scene.followCamera;
                    //    smallShip.followCamera.transform.SetParent(scene.transform);
                    //}
                }

                Transform target = smallShip.transform; // The target to follow (the ship itself);
                
                float followSpeed = 16f;   // How quickly the camera moves default 8
                float rotationSpeed = 6; // How quickly the camera rotates default 6
                GameObject followCamera = smallShip.followCamera;

                //This sets the cameras primary position
                Vector3 primaryPositionOffset = new Vector3(0, smallShip.shipLength / 2f, smallShip.shipLength * -1);

                Vector3 primaryPosition = target.TransformPoint(primaryPositionOffset);

                if (smallShip.followCameraPosition != null)
                {
                    primaryPosition = smallShip.followCameraPosition.transform.position;
                }

                primaryPosition = smallShip.scene.transform.InverseTransformPoint(primaryPosition);

                //This sets the cameras secondary position
                Vector3 secondaryPositionOffset = new Vector3(0, smallShip.shipLength / 4f, 0);

                Vector3 secondaryPosition = target.TransformPoint(secondaryPositionOffset);

                if (smallShip.focusCameraPosition != null)
                {
                    secondaryPosition = smallShip.focusCameraPosition.transform.position;
                }

                //This caluclates the dynamic position of the camera
                if (smallShip.inHyperspace == false & smallShip.docking == false)
                {
                    if (smallShip.focusCamera == false)
                    {
                        followCamera.transform.SetParent(smallShip.scene.transform);
                        followCamera.transform.localPosition = Vector3.Lerp(followCamera.transform.localPosition, primaryPosition, followSpeed * Time.deltaTime);

                        //This should prevent the camera passing its designated position
                        if (Vector3.Dot((primaryPosition - followCamera.transform.localPosition), (followCamera.transform.localPosition - followCamera.transform.localPosition)) < 0)
                        {
                            followCamera.transform.localPosition = primaryPosition;
                        }
                    }
                    else
                    {
                        rotationSpeed = 12;
                        followCamera.transform.SetParent(smallShip.transform);
                        followCamera.transform.position = secondaryPosition;
                    }

                    //smoothly rotate the camera to look at the target
                    Quaternion desiredRotation = target.rotation;
                    followCamera.transform.rotation = Quaternion.Slerp(followCamera.transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
                }
                else
                {
                    followCamera.transform.SetParent(smallShip.transform);
                }
            }
        }
    }

    #endregion

    #region cockpit

    //This activates the ships cockpit if it's a player ship
    public static void ActivateCockpit(SmallShip smallShip)
    {
        if (smallShip.scene != null & smallShip.cockpit == null)
        {
            //This anchors the cockpit camera to the cockpit scene
            if (smallShip.cockpitCamera == null)
            {
                smallShip.cockpitCamera = GameObject.Find("Cockpit Camera");
            }

            //This loads the cockpit and sets it to the anchor
            if (smallShip.cockpit == null)
            {
                smallShip.cockpit = SceneFunctions.ActivateCockpit(smallShip.cockpitName);
                smallShip.scene.cockpit = smallShip.cockpit;
            }
        }
    }

    //This runs HudSpeedShake and HudShake
    public static void RunCockpitShake(SmallShip smallShip)
    {
        if (Time.timeScale != 0)
        {
            if (smallShip.thrustSpeed > smallShip.speedRating / 2f & smallShip.cockpitDamageShake != true)
            {
                if (smallShip.thrustSpeed > smallShip.speedRating + 10)
                {
                    float shakeMagnitude = 0.001f;

                    if (smallShip.speedShakeMagnitude < shakeMagnitude)
                    {
                        smallShip.speedShakeMagnitude += 0.00005f; //This slowly increases the shake magnitude until it reaches 0.001f
                    }

                }
                else if (smallShip.speedShakeMagnitude > 0)
                {
                    smallShip.speedShakeMagnitude -= 0.00005f;
                }
            }
            else if (smallShip.speedShakeMagnitude > 0)
            {
                smallShip.speedShakeMagnitude -= 0.00005f;
            }

            if (smallShip.speedShakeMagnitude > 0)
            {
                if (smallShip.cockpitDamageShake == false & smallShip.cockpitSpeedShake == false)
                {
                    Task a = new Task(CockpitSpeedShake(smallShip));
                }
            }

            if (smallShip.cockpitShake == true)
            {
                if (smallShip.cockpitDamageShake == false)
                {
                    Task a = new Task(CockpitDamageShake(smallShip, 1, 0.001f));
                }
            }
        }
    }

    //This tells the hud that it should run the hud shake function
    public static IEnumerator ActivateCockpitShake(SmallShip smallShip, float time)
    {
        if (smallShip.isAI == false)
        {
            smallShip.cockpitShake = true;

            yield return new WaitForSeconds(time);

            smallShip.cockpitShake = false;
        }
    }

    //This shakes the hud glass
    public static IEnumerator CockpitSpeedShake(SmallShip smallShip)
    {
        smallShip.cockpitSpeedShake = true;

        if (smallShip.cockpit != null & smallShip.basePosition != null)
        {
            float time = Time.time + 1;

            while (time > Time.time)
            {
                if (smallShip != null) //This needs to be checked as conditions can change while the inumerator is running
                {
                    float x = Random.Range(-1f, 1f) * smallShip.speedShakeMagnitude;
                    float y = Random.Range(-1f, 1f) * smallShip.speedShakeMagnitude;

                    if (smallShip != null)
                    {

                    }

                    smallShip.cockpit.transform.localPosition = new Vector3(x, y, smallShip.basePosition.z);

                    if (smallShip.cockpitDamageShake == true)
                    {
                        break;
                    }

                    if (Time.timeScale == 0)
                    {
                        break;
                    }

                    yield return null;
                }
                else
                {
                    break;
                }

            }

            if (smallShip != null)  //This needs to be checked as conditions can change while the inumerator is running
            {
                smallShip.cockpit.transform.localPosition = smallShip.basePosition;
            }
        }

        if (smallShip != null)  //This needs to be checked as conditions can change while the inumerator is running
        {
            smallShip.cockpitSpeedShake = false;
        }
    }

    //This shakes the hud glass
    public static IEnumerator CockpitDamageShake(SmallShip smallShip, float time, float magnitude)
    {
        if (smallShip.isAI == false)
        {
            smallShip.cockpitDamageShake = true;

            time = Time.time + time;

            while (time > Time.time)
            {
                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;

                if (smallShip.cockpit != null)
                {
                    smallShip.cockpit.transform.localPosition = new Vector3(x, y, smallShip.basePosition.z);
                }

                if (Time.timeScale == 0)
                {
                    break;
                }

                yield return null;
            }

            if (smallShip.cockpit != null)
            {
                smallShip.cockpit.transform.localPosition = smallShip.basePosition;
            }

            smallShip.cockpitDamageShake = false;
        }
    }

    //This dynamically adjusts the position of the cockpit camera to simulate the movement of the pilots head and body
    public static void CockpitCameraMovement(SmallShip smallShip)
    {
        if (Time.timeScale != 0)
        {
            if (smallShip.cockpitCamera == null)
            {
                smallShip.cockpitCamera = GameObject.Find("Cockpit Camera");
            }

            if (smallShip.cockpitCamera != null)
            {
                float gForceMagnitude;

                //This smooth changes the thrust speed to prevent the camera lurching backwards and forwards between two values
                if (smallShip.smoothThrust < smallShip.thrustSpeed)
                {
                    smallShip.smoothThrust += 0.1f;
                }
                else if (smallShip.smoothThrust > smallShip.thrustSpeed)
                {
                    smallShip.smoothThrust += 0.1f;
                }

                if (smallShip.smoothThrust <= smallShip.speedRating)
                {
                    gForceMagnitude = 5f / 125f * smallShip.smoothThrust;
                }
                else
                {
                    gForceMagnitude = 5f / 125f * 50f;
                }

                //This gives the cockpit a little bit of sway so that's it's never completely static
                smallShip.movementTime += Time.deltaTime;

                float randomnumber1 = Random.Range(-10, 10);
                float randomnumber2 = Random.Range(-10, 10);

                if (randomnumber1 > 0)
                {
                    smallShip.randomisationX += 0.001f;
                }
                else if (randomnumber1 < 0)
                {
                    smallShip.randomisationX -= 0.001f;
                }

                if (randomnumber2 > 0)
                {
                    smallShip.randomisationY += 0.001f;
                }
                else if (randomnumber2 < 0)
                {
                    smallShip.randomisationY -= 0.001f;
                }

                float variation = 0.015f; //This makes minor adjustments to the variation according to speed

                if (smallShip.thrustSpeed < 10)
                {
                    variation = 0.005f;
                }
                else if (smallShip.thrustSpeed < 20)
                {
                    variation = 0.01f;
                }

                //This calculates the final movement
                float xLocation = 0 + (0.0001f * smallShip.turnInput * 100 * gForceMagnitude) + (variation * (Mathf.PerlinNoise(smallShip.randomisationX, smallShip.movementTime) - 0.5f)); 
                float yLocation = 0 + (0.0001f * smallShip.pitchInput * 100 * gForceMagnitude) + (variation * (Mathf.PerlinNoise(smallShip.randomisationY, smallShip.movementTime) - 0.5f));
                float zLocation = 0 - 0.0005f * smallShip.thrustSpeed;

                float xRotation = 3f * smallShip.pitchInput;
                float yRotation = 3f * smallShip.turnInput;
                float zRotation = 3f * smallShip.rollInput;

                //float step = 0.05f * Time.deltaTime;
                float step = 2f * Time.deltaTime;
                float step2 = 10f * Time.deltaTime;

                Vector3 dynamicLocation = new Vector3(xLocation, yLocation, zLocation);
                Quaternion dynamicRotation = Quaternion.Euler(xRotation, yRotation, zRotation);

                //This causes the Camera to respond to the starfighters movements
                if (smallShip.focusCamera == false)
                {
                    smallShip.currentPosition = Vector3.MoveTowards(smallShip.currentPosition, dynamicLocation, step);
                    smallShip.cockpitCamera.transform.localPosition = smallShip.currentPosition;

                    smallShip.currentRotation = Quaternion.RotateTowards(smallShip.currentRotation, dynamicRotation, step2);
                    smallShip.cockpitCamera.transform.localRotation = smallShip.currentRotation;
                }
                else
                {
                    Vector3 focusPosition = new Vector3(0, 0, 0.25f);
                    Quaternion focusRotation = Quaternion.Euler(0, 0, 0);

                    step = 10f * Time.deltaTime;

                    smallShip.currentPosition = Vector3.MoveTowards(smallShip.currentPosition, focusPosition, step);
                    smallShip.cockpitCamera.transform.localPosition = smallShip.currentPosition;

                    smallShip.currentRotation = Quaternion.RotateTowards(smallShip.currentRotation, focusRotation, step2);
                    smallShip.cockpitCamera.transform.localRotation = smallShip.currentRotation;
                }
            }
        }
    }

    //Cockpit anchor rotation
    public static void CockpitAnchorRotation(SmallShip smallShip)
    {
        if (smallShip.cockpitCamera != null & smallShip.cockpit != null)
        {
            smallShip.cockpitCamera.transform.rotation = smallShip.transform.rotation;
            smallShip.cockpit.transform.rotation = smallShip.transform.rotation;
        }       
    }

    #endregion


}
