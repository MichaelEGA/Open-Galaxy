using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            ShakeCockpitDuringHyperspace(smallShip);
        }
    }

    #region camera control

    //This function sets the camera to the ship position
    public static void SetMainCamera(SmallShip smallShip)
    {
        if (smallShip.isAI == false & smallShip.cameraAttached == false)
        {
            GameObject mainCamera = smallShip.mainCamera;

            if (mainCamera == null)
            {
                mainCamera = GameObject.Find("Main Camera");
                smallShip.mainCamera = mainCamera;
            }

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

    //This removes the main camera (i.e. when the ship is destroyed or the camera switches to another ship)
    public static void RemoveMainCamera(SmallShip smallShip)
    {
        if (smallShip.mainCamera != null)
        {
            smallShip.mainCamera.transform.SetParent(null);
            smallShip.mainCamera = null;
            smallShip.cameraAttached = false;
        }
    }

    #endregion

    #region cockpit

    //This activates the ships cockpit if it's a player ship
    public static void ActivateCockpit(SmallShip smallShip)
    {
        if (smallShip.scene != null & smallShip.cockpit == null)
        {
            //This creates the cockpit scene anchor
            if (smallShip.cockpitAnchor == null)
            {
                smallShip.cockpitAnchor = GameObject.Find("Cockpit Anchor");

                if (smallShip.cockpitAnchor == null)
                {
                    smallShip.cockpitAnchor = new GameObject();
                    smallShip.cockpitAnchor.name = "Cockpit Anchor";
                }

                smallShip.cockpitAnchor.transform.rotation = Quaternion.identity;
            }

            //This anchors the cockpit camera to the cockpit scene
            if (smallShip.cockpitCamera == null)
            {
                smallShip.cockpitCamera = GameObject.Find("Cockpit Camera");

                if (smallShip.cockpitCamera != null)
                {
                    smallShip.cockpitCamera.transform.rotation = Quaternion.identity;
                    smallShip.cockpitCamera.transform.SetParent(smallShip.cockpitAnchor.transform);
                }
            }

            //This loads the cockpit and sets it to the anchor
            if (smallShip.cockpit == null)
            {
                smallShip.cockpit = SceneFunctions.InstantiateCockpitPrefab(smallShip.cockpitName);
                smallShip.scene.cockpit = smallShip.cockpit;

                if (smallShip.cockpit != null)
                {
                    if (smallShip.cockpitAnchor != null)
                    {
                        smallShip.cockpit.transform.SetParent(smallShip.cockpitAnchor.transform);

                        if (smallShip.scene.hyperspaceTunnel == null)
                        {
                            smallShip.scene.hyperspaceTunnel = GameObject.Instantiate(smallShip.scene.hyperspaceTunnelPrefab);
                        }

                        smallShip.scene.hyperspaceTunnel.transform.SetParent(smallShip.cockpitAnchor.transform);
                        smallShip.scene.hyperspaceTunnel.SetActive(false);
                    }
                }
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
                if (smallShip.turnInput > 0.75f || smallShip.turnInput < -0.75f || smallShip.pitchInput > 0.75f || smallShip.pitchInput < -0.75f || smallShip.thrustSpeed > smallShip.speedRating + 5 || smallShip.rollInput > 0.75f || smallShip.rollInput < -75f)
                {
                    float shakeMagnitude = 0.001f;

                    if (smallShip.speedShakeMagnitude < shakeMagnitude)
                    {
                        smallShip.speedShakeMagnitude += 0.00005f;
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

    //This shakes the cockpit during hyperspace
    public static void ShakeCockpitDuringHyperspace(SmallShip smallShip)
    {
        if (smallShip.inHyperspace == true)
        {
            float x = Random.Range(-1f, 1f) * 0.0025f;
            float y = Random.Range(-1f, 1f) * 0.0025f;

            if (smallShip.cockpit != null)
            {
                smallShip.cockpit.transform.localPosition = new Vector3(x, y, smallShip.basePosition.z);
            }
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

                if (smallShip.thrustSpeed <= smallShip.speedRating)
                {
                    gForceMagnitude = 5f / 125f * smallShip.thrustSpeed;
                }
                else
                {
                    gForceMagnitude = 5f / 125f * 50f;
                }

                float xLocation = 0 + (0.0001f * smallShip.turnInput * 100 * gForceMagnitude);
                float yLocation = 0 + (0.0001f * smallShip.pitchInput * 100 * gForceMagnitude);
                float zLocation = 0 - 0.0005f * smallShip.thrustSpeed;

                float xRotation = 3f * smallShip.pitchInput;
                float yRotation = 3f * smallShip.turnInput;
                float zRotation = 3f * smallShip.rollInput;

                float step = 0.05f * Time.deltaTime;
                float step2 = 10f * Time.deltaTime;

                Vector3 dynamicLocation = new Vector3(xLocation, yLocation, zLocation);
                Quaternion dynamicRotation = Quaternion.Euler(xRotation, yRotation, zRotation);

                //This causes the Camera to respond to the starfighters movements
                smallShip.currentPosition = Vector3.MoveTowards(smallShip.currentPosition, dynamicLocation, step);
                smallShip.cockpitCamera.transform.localPosition = smallShip.currentPosition;

                smallShip.currentRotation = Quaternion.RotateTowards(smallShip.currentRotation, dynamicRotation, step2);
                smallShip.cockpitCamera.transform.localRotation = smallShip.currentRotation;
            }
        }
    }

    //Cockpit anchor rotation
    public static void CockpitAnchorRotation(SmallShip smallShip)
    {
        if (smallShip.cockpitAnchor != null)
        {
            smallShip.cockpitAnchor.transform.rotation = smallShip.transform.rotation;
        }       
    }

    #endregion


}
