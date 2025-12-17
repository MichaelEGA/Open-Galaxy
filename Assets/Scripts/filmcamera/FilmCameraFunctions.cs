using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FilmCameraFunctions : MonoBehaviour
{
    #region intialisation functions

    //This activates the film camera
    public static void ActivateFilmCamera()
    {
        //This checks the film camera script exists
        FilmCamera filmCamera = GetFilmCamera();

        if (filmCamera == null)
        {
            filmCamera  = CreateFilmCamera();
        }

        //This activates the film camera
        SceneFunctions.ActivateFilmCamera(true);
        Task a = new Task(HudFunctions.FadeOutHud(0.1f));
    }

    //This deactivates the film camera
    public static void DeactivateFilmCamera()
    {
        //This checks the film camera script exists
        FilmCamera filmCamera = GetFilmCamera();
        
        if (filmCamera == null)
        {
            filmCamera = CreateFilmCamera();
        }

        SceneFunctions.ActivateFilmCamera(false);
        Task a = new Task(HudFunctions.FadeInHud(0.1f));
    }

    //This gets the main camera
    public static FilmCamera CreateFilmCamera()
    {
        //This creates the game object for the script
        GameObject filmCameraScriptGO = new GameObject();
        filmCameraScriptGO.name = "filmcameraGO";

        //This adds the script and gets the scene reference
        FilmCamera filmCameraScript = filmCameraScriptGO.AddComponent<FilmCamera>();
        filmCameraScript.scene = SceneFunctions.GetScene();

        //This gets the actual camera
        GameObject filmCamera = filmCameraScript.scene.filmCamera;
        filmCameraScript.filmCamera = filmCamera;

        //This parents the camera to the script gameobject
        filmCamera.transform.parent = filmCameraScriptGO.transform;
        filmCamera.transform.localPosition = Vector3.zero;
        filmCamera.transform.rotation = Quaternion.identity;

        //This parents the script object to the scene
        filmCameraScript.transform.parent = filmCameraScript.scene.transform;

        return filmCameraScript;
    }

    //This sets the values of the film camera
    public static void SetFilmCameraValues(string mode, bool blackbars, Vector3 position, Quaternion rotation, string targetName)
    {
        FilmCamera filmCamera = GetFilmCamera();

        if (filmCamera != null)
        {
            filmCamera.mode = mode;
            filmCamera.blackbars = blackbars;
            filmCamera.rotation = rotation;
            filmCamera.position = position;
            filmCamera.targetName = targetName;
            FadeInBlackBars(filmCamera);
        }
    }

    #endregion

    #region camera modes

    //This switches the camera between different control modes
    public static void RunFilmCamera(FilmCamera filmCamera)
    {
        if (filmCamera.mode == "freelook")
        {
            FreeLookMode(filmCamera);
        }
        else if (filmCamera.mode == "staticshot")
        {
            //
        }
        else if (filmCamera.mode == "mountedshot")
        {
            MountedShot(filmCamera);
        }
        else if (filmCamera.mode == "mountedshotlocked")
        {
            MountedShotLocked(filmCamera);
        }
        else if (filmCamera.mode == "trackingshotstatic")
        {
           //
        }
        else if (filmCamera.mode == "trackingshotdynamic")
        {
           //
        }
    }

    //This allows the player to move the camera freely
    public static void FreeLookMode(FilmCamera filmCamera)
    {
        if (filmCamera.scene != null)
        {
            filmCamera.transform.parent = filmCamera.scene.transform;
        }
        
        var currentMoveSpeed = Input.GetKey(KeyCode.LeftShift) ? filmCamera.fastMovementSpeed : filmCamera.movementSpeed;

        if (Input.GetKey(KeyCode.W))
            filmCamera.transform.position += filmCamera.transform.forward * currentMoveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S))
            filmCamera.transform.position += - filmCamera.transform.forward * currentMoveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
            filmCamera.transform.position += -filmCamera.transform.right * currentMoveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D))
            filmCamera.transform.position += filmCamera.transform.right * currentMoveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.Q)) // Move up in world space
            filmCamera.transform.position += Vector3.up * currentMoveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.E)) // Move down in world space
            filmCamera.transform.position += -Vector3.up * currentMoveSpeed * Time.deltaTime;

        // 2. Rotation (Free Look)
        if (Input.GetKeyDown(KeyCode.Mouse1)) // Right-click to start looking
        {
            filmCamera.looking = true;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1)) // Right-click release to stop
        {
            filmCamera.looking = false;
        }

        if (filmCamera.looking)
        {
            float newRotationX = filmCamera.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * filmCamera.freeLookSensitivity;
            // Negative sign for Y-axis to invert, matching standard camera controls
            float newRotationY = filmCamera.transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * filmCamera.freeLookSensitivity;

            // Apply rotation, clamping the vertical look to prevent flipping
            // Note: This simple implementation is just for horizontal/vertical look
            filmCamera.transform.localEulerAngles = new Vector3(newRotationY, newRotationX, 0f);
        }
    }

    //This locks the camera on the target ship while moving along with the ship
    public static void MountedShot(FilmCamera filmCamera)
    {
        if (filmCamera.targetShip == null)
        {
            SearchForShip(filmCamera);
        }

        if (filmCamera.targetShip != null)
        {
            filmCamera.transform.parent = filmCamera.targetShip.transform;
            filmCamera.transform.localPosition = filmCamera.position;
            filmCamera.transform.rotation = filmCamera.rotation;
        }
    }

    //This locks the camera on the target ship while moving along with the ship
    public static void MountedShotLocked(FilmCamera filmCamera)
    {
        if (filmCamera.targetShip == null)
        {
            SearchForShip(filmCamera);
        }

        if (filmCamera.targetShip != null)
        {
            filmCamera.transform.parent = filmCamera.targetShip.transform;
            filmCamera.transform.localPosition = filmCamera.position;
            filmCamera.transform.LookAt(filmCamera.targetShip.transform.position);
        }
    }

    //This searches for and returns the target ship
    public static void SearchForShip(FilmCamera filmCamera)
    {
        Scene scene = filmCamera.scene;

        if (scene != null)
        {
            if (scene.objectPool != null)
            {
                foreach (GameObject tempShip in scene.objectPool.ToList())
                {
                    if (tempShip != null)
                    {
                        if (tempShip.name.Contains(filmCamera.targetName))
                        {
                            filmCamera.targetShip = tempShip;
                        }
                    }
                }
            }
        }
    }

    #endregion

    #region other camera effects

    //This fades in black bars on the top and bottom of the screen for a more cinematic effect
    public static void FadeInBlackBars(FilmCamera filmCamera)
    {
        if (filmCamera.blackbars == false)
        {
            Task a = new Task(HudFunctions.FadeOutBlackBars(0.1f));
        }
        else if (filmCamera.blackbars == true)
        {
            Task a = new Task(HudFunctions.FadeInBlackBars(0.1f));
        }
    }


    #endregion

    #region film camera tools

    //This gets the film camera script and script gameobject
    public static FilmCamera GetFilmCamera()
    {
        FilmCamera filmCamera = FindFirstObjectByType<FilmCamera>();

        return filmCamera;
    }

    #endregion
}
