using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

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
        if (filmCamera.active == false)
        {
            SceneFunctions.ActivateFilmCamera(true);
            Task a = new Task(HudFunctions.FadeOutHud(0.1f));
            filmCamera.active = true;
        }
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

        if (filmCamera.active == true)
        {
            SceneFunctions.ActivateFilmCamera(false);
            Task a = new Task(HudFunctions.FadeInHud(0.1f));
            filmCamera.active = false;
        }
    }

    //This gets the main camera
    public static FilmCamera CreateFilmCamera()
    {
        //This creates the game object for the script
        GameObject filmCameraScriptGO = new GameObject();
        filmCameraScriptGO.name = "filmcameraGO";

        //This creates a second gameobject to control the pan and the zoom
        GameObject panandzoomGO = new GameObject();
        panandzoomGO.name = "panandzoomGO";
        panandzoomGO.transform.parent = filmCameraScriptGO.transform;

        //This creates a third gameobject to control the rotation
        GameObject rotateGO = new GameObject();
        rotateGO.name = "rotateGO";
        rotateGO.transform.parent = panandzoomGO.transform;

        //This adds the script and gets the scene reference
        FilmCamera filmCameraScript = filmCameraScriptGO.AddComponent<FilmCamera>();
        filmCameraScript.scene = SceneFunctions.GetScene();
        filmCameraScript.moveGO = panandzoomGO;
        filmCameraScript.rotateGO = rotateGO;

        //This gets the actual camera
        GameObject filmCamera = filmCameraScript.scene.filmCamera;
        filmCameraScript.filmCameraGO = filmCamera;

        //This parents the camera to the pan and the zoom gameobject
        filmCamera.transform.parent = rotateGO.transform;
        filmCamera.transform.localPosition = Vector3.zero;
        filmCamera.transform.rotation = Quaternion.identity;

        //This parents the script object to the scene
        filmCameraScript.transform.parent = filmCameraScript.scene.transform;

        return filmCameraScript;
    }

    //This sets the values of the film camera
    public static void SetFilmCameraValues(string mode, bool blackbars, Vector3 position, Quaternion rotation, string targetName, bool shakecamera, float shakerate, float shakestrength, bool moveActive, string moveAxis, float moveSpeed, bool rotateActive, string rotateAxis, float rotateSpeed)
    {
        FilmCamera filmCamera = GetFilmCamera();

        if (filmCamera != null)
        {
            //Reset all key vaules
            filmCamera.transform.parent = null;
            filmCamera.moveGO.transform.localPosition = Vector3.zero;
            filmCamera.rotateGO.transform.localRotation = Quaternion.identity;
            filmCamera.filmCameraGO.transform.localPosition = Vector3.zero;
            RestoreTargetShipLayer(filmCamera); //This restores previously selected ships real layer before nulling the value
            filmCamera.targetShip = null;
            filmCamera.targetShipCameraGO = null;
            filmCamera.cockpitGO = null;
            filmCamera.staticShotTaken = false;

            //Set mode type
            filmCamera.mode = mode;
            
            //Set key values
            filmCamera.rotation = rotation;
            filmCamera.position = position;
            filmCamera.targetName = targetName;

            //Set shake camera vaules
            filmCamera.shakeCamera = shakecamera;
            filmCamera.shakeRate = shakerate;
            filmCamera.shakeStrength = shakestrength;
            
            //Set move camera values
            filmCamera.moveActive = moveActive;
            filmCamera.moveAxis = moveAxis;
            filmCamera.moveSpeed = moveSpeed;
            
            //Set rotate camera values
            filmCamera.rotateActive = rotateActive;
            filmCamera.rotationAxis = rotateAxis;
            filmCamera.secondaryRotationSpeed = rotateSpeed;

            //Set secondary values
            filmCamera.blackbars = blackbars; //MOVE THIS TO HUD FUNCTIONS

            //Other functions
            SceneFunctions.DeactivateCockpits();
            FadeInBlackBars(filmCamera);
        }
    }

    #endregion

    #region primary camera modes

    //This switches the camera between different control modes
    public static void RunFilmCamera(FilmCamera filmCamera)
    {
        if (filmCamera.mode == "freelook")
        {
            FreeLookMode(filmCamera);
        }
        else if (filmCamera.mode == "staticshot")
        {
            StaticShot(filmCamera);
        }
        else if (filmCamera.mode == "staticshotlocked")
        {
            StaticShotLocked(filmCamera);
        }
        else if (filmCamera.mode == "relativestaticshot")
        {
            RelativeStaticShot(filmCamera);
        }
        else if (filmCamera.mode == "relativestaticlocked")
        {
            RelativeStaticShotLocked(filmCamera);
        }
        else if (filmCamera.mode == "trackingshot")
        {
            TrackingShot(filmCamera);
        }
        else if (filmCamera.mode == "relativetrackingshot")
        {
            RelativeTrackingShot(filmCamera);
        }
        else if (filmCamera.mode == "mountedshot")
        {
            MountedShot(filmCamera);
        }
        else if (filmCamera.mode == "mountedshotlocked")
        {
            MountedShotLocked(filmCamera);
        }
        else if (filmCamera.mode == "cockpitshot")
        {
            CockpitShot(filmCamera);
        }

        //Run Secondary Functions
        ShakeCamera(filmCamera);
        MoveCameraAlongAxis(filmCamera);
        RotateCameraOnAxis(filmCamera);
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

    //This set the position and rotation of the camera for a static shot
    public static void StaticShot(FilmCamera filmCamera)
    {
        filmCamera.transform.parent = filmCamera.scene.transform;
        filmCamera.transform.localPosition = filmCamera.position;
        filmCamera.transform.localRotation = filmCamera.rotation;
    }

    //This set the position and rotation of the camera for a static shot
    public static void StaticShotLocked(FilmCamera filmCamera)
    {
        if (filmCamera.targetShip == null)
        {
            SearchForShip(filmCamera);
        }

        if (filmCamera.targetShip != null & filmCamera.staticShotTaken == false) //This check prevents the camera moving with the ship
        {
            filmCamera.transform.parent = filmCamera.scene.transform;
            filmCamera.transform.localPosition = filmCamera.position;
            filmCamera.transform.rotation = Quaternion.identity;
            filmCamera.transform.LookAt(filmCamera.targetShip.transform.position);
            filmCamera.staticShotTaken = true;
        }
    }

    //This sets the position and rotation of the camera for a static shot relative to a ship
    public static void RelativeStaticShot(FilmCamera filmCamera)
    {
        if (filmCamera.targetShip == null)
        {
            SearchForShip(filmCamera);
        }

        if (filmCamera.targetShip != null & filmCamera.staticShotTaken == false) //This check prevents the camera moving with the ship
        {
            filmCamera.transform.parent = filmCamera.targetShip.transform; //This parents it to the ship object to get the right position
            filmCamera.transform.localPosition = filmCamera.position;
            filmCamera.transform.localRotation = filmCamera.rotation;
            filmCamera.transform.parent = filmCamera.scene.transform; //This unparents it from the ship object so the camera doesn't move with the ship
            filmCamera.staticShotTaken = true;
        }
    }

    //This sets the position and rotation of the camera for a static shot relative to a ship
    public static void RelativeStaticShotLocked(FilmCamera filmCamera)
    {
        if (filmCamera.targetShip == null)
        {
            SearchForShip(filmCamera);
        }

        if (filmCamera.targetShip != null & filmCamera.staticShotTaken == false) //This check prevents the camera moving with the ship
        {
            filmCamera.transform.parent = filmCamera.targetShip.transform; //This parents it to the ship object to get the right position
            filmCamera.transform.localPosition = filmCamera.position;
            filmCamera.transform.rotation = Quaternion.identity;
            filmCamera.transform.LookAt(filmCamera.targetShip.transform.position);
            filmCamera.transform.parent = filmCamera.scene.transform; //This unparents it from the ship object so the camera doesn't move with the ship
            filmCamera.staticShotTaken = true;
        }
    }

    //The camera is placed in scene space and tracks the targeted ship
    public static void TrackingShot(FilmCamera filmCamera)
    {
        if (filmCamera.targetShip == null)
        {
            SearchForShip(filmCamera);
        }

        if (filmCamera.targetShip != null)
        {
            filmCamera.transform.parent = filmCamera.scene.transform;
            filmCamera.transform.localPosition = filmCamera.position;
            filmCamera.transform.rotation = Quaternion.identity;
            filmCamera.transform.LookAt(filmCamera.targetShip.transform.position);
        }
    }

    //The camera is placed relative to a ship and tracks the targeted ship
    public static void RelativeTrackingShot(FilmCamera filmCamera)
    {
        if (filmCamera.targetShip == null)
        {
            SearchForShip(filmCamera);
        }

        if (filmCamera.targetShip != null)
        {
            if (filmCamera.staticShotTaken == false)
            {
                filmCamera.transform.parent = filmCamera.targetShip.transform; //This parents it to the ship object to get the right position
                filmCamera.transform.localPosition = filmCamera.position;
                filmCamera.transform.rotation = Quaternion.identity;
                filmCamera.transform.parent = filmCamera.scene.transform;
                filmCamera.staticShotTaken = true;
            }
           

            filmCamera.transform.LookAt(filmCamera.targetShip.transform.position);
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
            filmCamera.transform.localRotation = filmCamera.rotation;
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
            
            if (filmCamera.staticShotTaken == false)
            {
                filmCamera.transform.rotation = Quaternion.identity;
                filmCamera.transform.LookAt(filmCamera.targetShip.transform.position);
                filmCamera.staticShotTaken = true;
            }
        }
    }

    //This sets the camera to the cockpit view
    public static void CockpitShot(FilmCamera filmCamera)
    {
        if (filmCamera.targetShip == null)
        {
            SearchForShip(filmCamera);
        }

        if (filmCamera.cockpitCameraGO == null)
        {
            filmCamera.cockpitCameraGO = GameObject.Find("Cockpit Camera");
        }

        if (filmCamera.targetShip != null)
        {
            if (filmCamera.targetShipCameraGO == null)
            {
                Transform targetShipCameraGO = GameObjectUtils.FindChildTransformCalled(filmCamera.targetShip.transform, "camera");
                filmCamera.targetShipCameraGO = targetShipCameraGO.gameObject;
            }

            if (filmCamera.targetShipCameraGO != null)
            {
                filmCamera.transform.parent = filmCamera.targetShipCameraGO.transform;
                filmCamera.transform.localPosition = Vector3.zero;

                if (filmCamera.cockpitGO == null)
                {
                    SmallShip smallShip = filmCamera.targetShip.GetComponent<SmallShip>();

                    if (smallShip != null)
                    {
                        string cockpitName = smallShip.cockpitName;
                        filmCamera.cockpitGO = SceneFunctions.ActivateCockpit(cockpitName);
                        SetTargetShipToPlayerLayer(filmCamera); //This sets the ships layer to player to prevent the ship being seen in cockpit view
                    }
                }

                if (filmCamera.cockpitGO != null & filmCamera.cockpitCameraGO != null)
                {
                    filmCamera.cockpitGO.transform.rotation = filmCamera.targetShip.transform.rotation;
                    filmCamera.cockpitCameraGO.transform.rotation = filmCamera.targetShip.transform.rotation;
                }

            }
        }
    }

    #endregion

    #region secondary camera modes

    //This fades in black bars on the top and bottom of the screen for a more cinematic effect
    public static void FadeInBlackBars(FilmCamera filmCamera)
    {
        if (filmCamera.blackbars == false & filmCamera.blackbarsActive == true)
        {
            Task a = new Task(HudFunctions.FadeOutBlackBars(0.1f));
            filmCamera.blackbarsActive = false;
        }
        else if (filmCamera.blackbars == true & filmCamera.blackbarsActive == false)
        {
            Task a = new Task(HudFunctions.FadeInBlackBars(0.1f));
            filmCamera.blackbarsActive = true;
        }
    }

    //This shakes the camera using noise
    public static void ShakeCamera(FilmCamera filmCamera)
    {
        if (filmCamera.filmCameraGO != null & filmCamera.shakeCamera == true)
        {
            float x = Mathf.PerlinNoise(Time.time * filmCamera.shakeRate, 0) * 2 - 1; // Generates noise
            float y = Mathf.PerlinNoise(0, Time.time * filmCamera.shakeRate) * 2 - 1;

            Vector3 shakePosition = new Vector3(x, y, 0) * filmCamera.shakeStrength;
            Vector3 currentVelocity = Vector3.zero;
            Vector3 startingPosition = Vector3.zero;

            filmCamera.filmCameraGO.transform.localPosition = startingPosition + shakePosition; //Vector3.zero is the starting position

            filmCamera.filmCameraGO.transform.localPosition = Vector3.SmoothDamp(filmCamera.filmCameraGO.transform.localPosition, startingPosition + shakePosition, ref currentVelocity, filmCamera.smoothTime);
        }
    }

    //This pans and zooms the camera left or right 
    public static void MoveCameraAlongAxis(FilmCamera filmCamera)
    {
        if (filmCamera.moveActive == true)
        {
            Vector3 moveAxis = Vector3.up;

            if (filmCamera.moveAxis == "xaxis")
            {
                moveAxis = Vector3.right;
            }
            else if (filmCamera.moveAxis == "yaxis")
            {
                moveAxis = Vector3.up;
            }
            else if (filmCamera.moveAxis == "zaxis")
            {
                moveAxis = Vector3.forward;
            }

            filmCamera.moveGO.transform.Translate(moveAxis * filmCamera.moveSpeed * Time.deltaTime);
        }
    }

    //This rotates the camera
    public static void RotateCameraOnAxis(FilmCamera filmCamera)
    {
        if (filmCamera.rotateActive == true)
        {
            Vector3 rotationAxis = Vector3.up;

            if (filmCamera.rotationAxis == "xaxis")
            {
                rotationAxis = Vector3.right;
            }
            else if (filmCamera.rotationAxis == "yaxis")
            {
                rotationAxis = Vector3.up;
            }
            else if (filmCamera.rotationAxis == "zaxis")
            {
                rotationAxis = Vector3.forward;
            }

            filmCamera.rotateGO.transform.Rotate(rotationAxis * filmCamera.secondaryRotationSpeed * Time.deltaTime);
        }
    }

    #endregion

    #region film camera tools

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
                            SaveShipLayer(filmCamera); //This saves the ship layer to restore later if needed
                            break;
                        }
                    }
                }
            }
        }
    }

    //This saves the ships layer to restore later is needed
    public static void SaveShipLayer(FilmCamera filmCamera)
    {
        if (filmCamera.targetShip != null)
        {
            filmCamera.savedLayerMask = filmCamera.targetShip.layer;
        }
    }

    //This sets the ship to player target layer
    public static void SetTargetShipToPlayerLayer(FilmCamera filmCamera)
    {
        if (filmCamera.targetShip != null)
        {
            filmCamera.savedLayerMask = filmCamera.targetShip.layer;

            filmCamera.targetShip.layer = LayerMask.NameToLayer("collision_player");

            GameObjectUtils.SetLayerAllChildren(filmCamera.targetShip.transform, LayerMask.NameToLayer("collision_player"));
        }
    }

    //This restores the ships layer
    public static void RestoreTargetShipLayer(FilmCamera filmCamera)
    {
        if (filmCamera.targetShip != null)
        {
            filmCamera.targetShip.layer = filmCamera.savedLayerMask;

            GameObjectUtils.SetLayerAllChildren(filmCamera.targetShip.transform, filmCamera.savedLayerMask);
        }
    }

    //This gets the film camera script and script gameobject
    public static FilmCamera GetFilmCamera()
    {
        FilmCamera filmCamera = FindFirstObjectByType<FilmCamera>();

        return filmCamera;
    }

    #endregion
}
