using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class OGCameraFunctions : MonoBehaviour
{
    #region intialisation functions

    //This activates the film camera
    public static void ActivateOGCamera()
    {
        //This checks the film camera script exists
        OGCamera ogCamera = GetOGCamera();

        if (ogCamera == null)
        {
            ogCamera  = CreateOGCamera();
        }

        //This activates the film camera
        if (ogCamera.active == false)
        {
            Task a = new Task(HudFunctions.FadeOutHud(0.1f));
            ogCamera.active = true;
        }
    }

    //This deactivates the film camera
    public static void DeactivateOGCamera()
    {
        //This checks the film camera script exists
        OGCamera ogCamera = GetOGCamera();
        
        if (ogCamera == null)
        {
            ogCamera = CreateOGCamera();
        }

        if (ogCamera.active == true)
        {
            Task a = new Task(HudFunctions.FadeInHud(0.1f));
            ogCamera.active = false;
        }
    }

    //This gets the main camera
    public static OGCamera CreateOGCamera()
    {
        //This creates the game object for the script
        GameObject ogCameraScriptGO = new GameObject();
        ogCameraScriptGO.name = "ogcameraGO";

        //This creates a second gameobject to control the pan and the zoom
        GameObject panandzoomGO = new GameObject();
        panandzoomGO.name = "panandzoomGO";
        panandzoomGO.transform.parent = ogCameraScriptGO.transform;

        //This creates a third gameobject to control the rotation
        GameObject rotateGO = new GameObject();
        rotateGO.name = "rotateGO";
        rotateGO.transform.parent = panandzoomGO.transform;

        //This adds the script and gets the scene reference
        OGCamera ogCamera = ogCameraScriptGO.AddComponent<OGCamera>();
        ogCamera.scene = SceneFunctions.GetScene();
        ogCamera.moveGO = panandzoomGO;
        ogCamera.rotateGO = rotateGO;

        //This gets the actual camera
        GameObject mainCamera = ogCamera.scene.mainCamera;
        ogCamera.mainCameraGO = mainCamera;

        //This parents the camera to the pan and the zoom gameobject
        mainCamera.transform.parent = rotateGO.transform;
        mainCamera.transform.localPosition = Vector3.zero;
        mainCamera.transform.rotation = Quaternion.identity;

        //This parents the script object to the scene
        ogCamera.transform.parent = ogCamera.scene.transform;

        //This makes sure the player layer is visible if needed
        if (ogCamera.viewType == "thirdperson")
        {
            ViewPlayerLayer();
        }

        return ogCamera;
    }

    //This sets the values of the film camera
    public static void SetOGCameraValues(string mode, string shotTypes, Vector3 position, Quaternion rotation, string targetName, bool shakecamera, float shakerate, float shakestrength, bool moveActive, string moveAxis, float moveSpeed, bool rotateActive, string rotateAxis, float rotateSpeed)
    {
        ResetOGCameraVaules();

        OGCamera ogCamera = GetOGCamera();

        if (ogCamera != null)
        {
            //Set mode type
            ogCamera.mode = mode;

            //Set shot type
            ogCamera.shotType = shotTypes;
            
            //Set key values
            ogCamera.rotation = rotation;
            ogCamera.position = position;
            ogCamera.targetName = targetName;

            //Set shake camera vaules
            ogCamera.shakeCamera = shakecamera;
            ogCamera.shakeRate = shakerate;
            ogCamera.shakeStrength = shakestrength;
            
            //Set move camera values
            ogCamera.moveActive = moveActive;
            ogCamera.moveAxis = moveAxis;
            ogCamera.moveSpeed = moveSpeed;
            
            //Set rotate camera values
            ogCamera.rotateActive = rotateActive;
            ogCamera.rotationAxis = rotateAxis;
            ogCamera.secondaryRotationSpeed = rotateSpeed;

            //Set secondary values
            ogCamera.blackbars = true; //MOVE THIS TO HUD FUNCTIONS

            //Other functions
            FadeInBlackBars(ogCamera);
        }
    }

    public static void ResetOGCameraVaules()
    {
        OGCamera ogCamera = GetOGCamera();

        if (ogCamera != null)
        {
            //Reset all key vaules
            ogCamera.transform.parent = null;
            ogCamera.moveGO.transform.localPosition = Vector3.zero;
            ogCamera.rotateGO.transform.localRotation = Quaternion.identity;
            ogCamera.mainCameraGO.transform.localPosition = Vector3.zero;
            RestoreTargetShipLayer(ogCamera); //This restores previously selected ships real layer before nulling the value
            ogCamera.targetShip = null;
            ogCamera.targetShipCameraGO = null;
            ogCamera.cockpitGO = null;
            ogCamera.staticShotTaken = false;
        }

        HidePlayerLayer();
        SceneFunctions.DeactivateCockpits();
    }

    #endregion

    #region

    //This runs the camera choosing between game mode and film mode
    public static void RunCamera(OGCamera ogCamera)
    {
        if (ogCamera.mode == "gamemode")
        {
            RunGameCamera(ogCamera);
        }
        else if (ogCamera.mode == "filmmode")
        {
            RunFilmCamera(ogCamera);
        }
    }

    #endregion

    #region game camera mode and view modes

    //This runs the camera in game mode
    public static void RunGameCamera(OGCamera ogCamera)
    {
        ToggleGameView(ogCamera);
        
        if (ogCamera.viewType == "firstperson")
        {
            FirstPersonView(ogCamera);
        }
        else if (ogCamera.viewType == "thirdperson")
        {
            ThirdPersonView(ogCamera);
        }
    }

    //This toggles between the different game views
    public static void ToggleGameView(OGCamera ogCamera)
    {
        //This toggles between game camera modes
        var keyboard = Keyboard.current;
        var controller = Gamepad.current;

        if (keyboard != null)
        {
            if (keyboard.f1Key.isPressed == true & Time.time > ogCamera.toggleGameCameraPressTime + 0.5f)
            {
                if (ogCamera.viewType == "firstperson")
                {
                    ogCamera.viewType = "thirdperson";
                    ResetOGCameraVaules();
                    ViewPlayerLayer();
                }
                else
                {
                    ogCamera.viewType = "firstperson";
                    ResetOGCameraVaules();
                    HidePlayerLayer();
                }

                ogCamera.toggleGameCameraPressTime = Time.time;
            }
        }

        if (controller != null)
        {
            if (controller.rightStickButton.isPressed == true & Time.time > ogCamera.toggleGameCameraPressTime + 0.5f)
            {
                if (ogCamera.viewType == "firstperson")
                {
                    ogCamera.viewType = "thirdperson";
                }
                else
                {
                    ogCamera.viewType = "firstperson";
                }

                ogCamera.toggleGameCameraPressTime = Time.time;
            }
        }
    }

    //This sets the camera to play from the first person viewpoint
    public static void FirstPersonView(OGCamera ogCamera)
    {
        if (ogCamera.targetShip == null)
        {
            SearchForPlayerShip(ogCamera);
        }

        if (ogCamera.cockpitCameraGO == null)
        {
            ogCamera.cockpitCameraGO = GameObject.Find("Cockpit Camera");
        }

        if (ogCamera.targetShip != null)
        {
            if (ogCamera.targetShipCameraGO == null)
            {
                Transform targetShipCameraGO = GameObjectUtils.FindChildTransformCalled(ogCamera.targetShip.transform, "camera");
                ogCamera.targetShipCameraGO = targetShipCameraGO.gameObject;
            }

            if (ogCamera.targetShipCameraGO != null)
            {
                ogCamera.transform.parent = ogCamera.targetShipCameraGO.transform;
                ogCamera.transform.localPosition = Vector3.zero;

                if (ogCamera.cockpitGO == null)
                {
                    SmallShip smallShip = ogCamera.targetShip.GetComponent<SmallShip>();

                    if (smallShip != null)
                    {
                        string cockpitName = smallShip.cockpitName;
                        ogCamera.cockpitGO = SceneFunctions.ActivateCockpit(cockpitName);
                        SetTargetShipToPlayerLayer(ogCamera); //This sets the ships layer to player to prevent the ship being seen in cockpit view
                    }
                }

                if (ogCamera.cockpitGO != null & ogCamera.cockpitCameraGO != null)
                {
                    ogCamera.cockpitGO.transform.rotation = ogCamera.targetShip.transform.rotation;
                    ogCamera.cockpitCameraGO.transform.rotation = ogCamera.targetShip.transform.rotation;
                }

            }
        }
    }

    //This sets the camera to play from the third person viewpoint
    public static void ThirdPersonView(OGCamera ogCamera)
    {
        if (ogCamera.targetShip == null)
        {
            SearchForPlayerShip(ogCamera);
        }

        if (ogCamera.targetShip != null)
        {
            SmallShip smallShip = ogCamera.targetShip.GetComponent<SmallShip>();

            if (smallShip != null)
            {
                Transform target = ogCamera.targetShip.transform; // The target to follow (the ship itself);
                GameObject followCamera = ogCamera.transform.gameObject;

                float followSpeed = 16f;
                float rotationSpeed = 6;

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

    #region film camera mode and shot types

    //This runs the camera in film mode
    public static void RunFilmCamera(OGCamera ogCamera)
    {
        if (ogCamera.shotType == "freelook")
        {
            FreeLookMode(ogCamera);
        }
        else if (ogCamera.shotType == "staticshot")
        {
            StaticShot(ogCamera);
        }
        else if (ogCamera.shotType == "staticshotlocked")
        {
            StaticShotLocked(ogCamera);
        }
        else if (ogCamera.shotType == "relativestaticshot")
        {
            RelativeStaticShot(ogCamera);
        }
        else if (ogCamera.shotType == "relativestaticlocked")
        {
            RelativeStaticShotLocked(ogCamera);
        }
        else if (ogCamera.shotType == "trackingshot")
        {
            TrackingShot(ogCamera);
        }
        else if (ogCamera.shotType == "relativetrackingshot")
        {
            RelativeTrackingShot(ogCamera);
        }
        else if (ogCamera.shotType == "mountedshot")
        {
            MountedShot(ogCamera);
        }
        else if (ogCamera.shotType == "mountedshotlocked")
        {
            MountedShotLocked(ogCamera);
        }
        else if (ogCamera.shotType == "cockpitshot")
        {
            CockpitShot(ogCamera);
        }

        //Run Secondary Functions
        ShakeCamera(ogCamera);
        MoveCameraAlongAxis(ogCamera);
        RotateCameraOnAxis(ogCamera);
    }

    //This allows the player to move the camera freely
    public static void FreeLookMode(OGCamera ogCamera)
    {
        if (ogCamera.scene != null)
        {
            ogCamera.transform.parent = ogCamera.scene.transform;
        }
        
        var currentMoveSpeed = Input.GetKey(KeyCode.LeftShift) ? ogCamera.fastMovementSpeed : ogCamera.movementSpeed;

        if (Input.GetKey(KeyCode.W))
            ogCamera.transform.position += ogCamera.transform.forward * currentMoveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S))
            ogCamera.transform.position += - ogCamera.transform.forward * currentMoveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
            ogCamera.transform.position += -ogCamera.transform.right * currentMoveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D))
            ogCamera.transform.position += ogCamera.transform.right * currentMoveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.Q)) // Move up in world space
            ogCamera.transform.position += Vector3.up * currentMoveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.E)) // Move down in world space
            ogCamera.transform.position += -Vector3.up * currentMoveSpeed * Time.deltaTime;

        // 2. Rotation (Free Look)
        if (Input.GetKeyDown(KeyCode.Mouse1)) // Right-click to start looking
        {
            ogCamera.looking = true;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1)) // Right-click release to stop
        {
            ogCamera.looking = false;
        }

        if (ogCamera.looking)
        {
            float newRotationX = ogCamera.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * ogCamera.freeLookSensitivity;
            // Negative sign for Y-axis to invert, matching standard camera controls
            float newRotationY = ogCamera.transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * ogCamera.freeLookSensitivity;

            // Apply rotation, clamping the vertical look to prevent flipping
            // Note: This simple implementation is just for horizontal/vertical look
            ogCamera.transform.localEulerAngles = new Vector3(newRotationY, newRotationX, 0f);
        }
    }

    //This set the position and rotation of the camera for a static shot
    public static void StaticShot(OGCamera ogCamera)
    {
        ogCamera.transform.parent = ogCamera.scene.transform;
        ogCamera.transform.localPosition = ogCamera.position;
        ogCamera.transform.localRotation = ogCamera.rotation;
    }

    //This set the position and rotation of the camera for a static shot
    public static void StaticShotLocked(OGCamera ogCamera)
    {
        if (ogCamera.targetShip == null)
        {
            SearchForShip(ogCamera);
        }

        if (ogCamera.targetShip != null & ogCamera.staticShotTaken == false) //This check prevents the camera moving with the ship
        {
            ogCamera.transform.parent = ogCamera.scene.transform;
            ogCamera.transform.localPosition = ogCamera.position;
            ogCamera.transform.rotation = Quaternion.identity;
            ogCamera.transform.LookAt(ogCamera.targetShip.transform.position);
            ogCamera.staticShotTaken = true;
        }
    }

    //This sets the position and rotation of the camera for a static shot relative to a ship
    public static void RelativeStaticShot(OGCamera ogCamera)
    {
        if (ogCamera.targetShip == null)
        {
            SearchForShip(ogCamera);
        }

        if (ogCamera.targetShip != null & ogCamera.staticShotTaken == false) //This check prevents the camera moving with the ship
        {
            ogCamera.transform.parent = ogCamera.targetShip.transform; //This parents it to the ship object to get the right position
            ogCamera.transform.localPosition = ogCamera.position;
            ogCamera.transform.localRotation = ogCamera.rotation;
            ogCamera.transform.parent = ogCamera.scene.transform; //This unparents it from the ship object so the camera doesn't move with the ship
            ogCamera.staticShotTaken = true;
        }
    }

    //This sets the position and rotation of the camera for a static shot relative to a ship
    public static void RelativeStaticShotLocked(OGCamera ogCamera)
    {
        if (ogCamera.targetShip == null)
        {
            SearchForShip(ogCamera);
        }

        if (ogCamera.targetShip != null & ogCamera.staticShotTaken == false) //This check prevents the camera moving with the ship
        {
            ogCamera.transform.parent = ogCamera.targetShip.transform; //This parents it to the ship object to get the right position
            ogCamera.transform.localPosition = ogCamera.position;
            ogCamera.transform.rotation = Quaternion.identity;
            ogCamera.transform.LookAt(ogCamera.targetShip.transform.position);
            ogCamera.transform.parent = ogCamera.scene.transform; //This unparents it from the ship object so the camera doesn't move with the ship
            ogCamera.staticShotTaken = true;
        }
    }

    //The camera is placed in scene space and tracks the targeted ship
    public static void TrackingShot(OGCamera ogCamera)
    {
        if (ogCamera.targetShip == null)
        {
            SearchForShip(ogCamera);
        }

        if (ogCamera.targetShip != null)
        {
            ogCamera.transform.parent = ogCamera.scene.transform;
            ogCamera.transform.localPosition = ogCamera.position;
            ogCamera.transform.rotation = Quaternion.identity;
            ogCamera.transform.LookAt(ogCamera.targetShip.transform.position);
        }
    }

    //The camera is placed relative to a ship and tracks the targeted ship
    public static void RelativeTrackingShot(OGCamera ogCamera)
    {
        if (ogCamera.targetShip == null)
        {
            SearchForShip(ogCamera);
        }

        if (ogCamera.targetShip != null)
        {
            if (ogCamera.staticShotTaken == false)
            {
                ogCamera.transform.parent = ogCamera.targetShip.transform; //This parents it to the ship object to get the right position
                ogCamera.transform.localPosition = ogCamera.position;
                ogCamera.transform.rotation = Quaternion.identity;
                ogCamera.transform.parent = ogCamera.scene.transform;
                ogCamera.staticShotTaken = true;
            }
           

            ogCamera.transform.LookAt(ogCamera.targetShip.transform.position);
        }
    }

    //This locks the camera on the target ship while moving along with the ship
    public static void MountedShot(OGCamera ogCamera)
    {
        if (ogCamera.targetShip == null)
        {
            SearchForShip(ogCamera);
        }
        
        if (ogCamera.targetShip != null)
        {
            ogCamera.transform.parent = ogCamera.targetShip.transform;
            ogCamera.transform.localPosition = ogCamera.position;
            ogCamera.transform.localRotation = ogCamera.rotation;
        }
    }

    //This locks the camera on the target ship while moving along with the ship
    public static void MountedShotLocked(OGCamera ogCamera)
    {
        if (ogCamera.targetShip == null)
        {
            SearchForShip(ogCamera);
        }

        if (ogCamera.targetShip != null)
        {
            ogCamera.transform.parent = ogCamera.targetShip.transform;
            ogCamera.transform.localPosition = ogCamera.position;
            
            if (ogCamera.staticShotTaken == false)
            {
                ogCamera.transform.rotation = Quaternion.identity;
                ogCamera.transform.LookAt(ogCamera.targetShip.transform.position);
                ogCamera.staticShotTaken = true;
            }
        }
    }

    //This sets the camera to the cockpit view
    public static void CockpitShot(OGCamera ogCamera)
    {
        if (ogCamera.targetShip == null)
        {
            SearchForShip(ogCamera);
        }

        if (ogCamera.cockpitCameraGO == null)
        {
            ogCamera.cockpitCameraGO = GameObject.Find("Cockpit Camera");
        }

        if (ogCamera.targetShip != null)
        {
            if (ogCamera.targetShipCameraGO == null)
            {
                Transform targetShipCameraGO = GameObjectUtils.FindChildTransformCalled(ogCamera.targetShip.transform, "camera");
                ogCamera.targetShipCameraGO = targetShipCameraGO.gameObject;
            }

            if (ogCamera.targetShipCameraGO != null)
            {
                ogCamera.transform.parent = ogCamera.targetShipCameraGO.transform;
                ogCamera.transform.localPosition = Vector3.zero;

                if (ogCamera.cockpitGO == null)
                {
                    SmallShip smallShip = ogCamera.targetShip.GetComponent<SmallShip>();

                    if (smallShip != null)
                    {
                        string cockpitName = smallShip.cockpitName;
                        ogCamera.cockpitGO = SceneFunctions.ActivateCockpit(cockpitName);
                        SetTargetShipToPlayerLayer(ogCamera); //This sets the ships layer to player to prevent the ship being seen in cockpit view
                    }
                }

                if (ogCamera.cockpitGO != null & ogCamera.cockpitCameraGO != null)
                {
                    ogCamera.cockpitGO.transform.rotation = ogCamera.targetShip.transform.rotation;
                    ogCamera.cockpitCameraGO.transform.rotation = ogCamera.targetShip.transform.rotation;
                }

            }
        }
    }

    #endregion

    #region secondary camera functions

    //This fades in black bars on the top and bottom of the screen for a more cinematic effect
    public static void FadeInBlackBars(OGCamera ogCamera)
    {
        if (ogCamera.blackbars == false & ogCamera.blackbarsActive == true)
        {
            Task a = new Task(HudFunctions.FadeOutBlackBars(0.1f));
            ogCamera.blackbarsActive = false;
        }
        else if (ogCamera.blackbars == true & ogCamera.blackbarsActive == false)
        {
            Task a = new Task(HudFunctions.FadeInBlackBars(0.1f));
            ogCamera.blackbarsActive = true;
        }
    }

    //This shakes the camera using noise
    public static void ShakeCamera(OGCamera ogCamera)
    {
        if (ogCamera.mainCameraGO != null & ogCamera.shakeCamera == true)
        {
            float x = Mathf.PerlinNoise(Time.time * ogCamera.shakeRate, 0) * 2 - 1; // Generates noise
            float y = Mathf.PerlinNoise(0, Time.time * ogCamera.shakeRate) * 2 - 1;

            Vector3 shakePosition = new Vector3(x, y, 0) * ogCamera.shakeStrength;
            Vector3 currentVelocity = Vector3.zero;
            Vector3 startingPosition = Vector3.zero;

            ogCamera.mainCameraGO.transform.localPosition = startingPosition + shakePosition; //Vector3.zero is the starting position

            ogCamera.mainCameraGO.transform.localPosition = Vector3.SmoothDamp(ogCamera.mainCameraGO.transform.localPosition, startingPosition + shakePosition, ref currentVelocity, ogCamera.smoothTime);
        }
    }

    //This pans and zooms the camera left or right 
    public static void MoveCameraAlongAxis(OGCamera ogCamera)
    {
        if (ogCamera.moveActive == true)
        {
            Vector3 moveAxis = Vector3.up;

            if (ogCamera.moveAxis == "xaxis")
            {
                moveAxis = Vector3.right;
            }
            else if (ogCamera.moveAxis == "yaxis")
            {
                moveAxis = Vector3.up;
            }
            else if (ogCamera.moveAxis == "zaxis")
            {
                moveAxis = Vector3.forward;
            }

            ogCamera.moveGO.transform.Translate(moveAxis * ogCamera.moveSpeed * Time.deltaTime);
        }
    }

    //This rotates the camera
    public static void RotateCameraOnAxis(OGCamera ogCamera)
    {
        if (ogCamera.rotateActive == true)
        {
            Vector3 rotationAxis = Vector3.up;

            if (ogCamera.rotationAxis == "xaxis")
            {
                rotationAxis = Vector3.right;
            }
            else if (ogCamera.rotationAxis == "yaxis")
            {
                rotationAxis = Vector3.up;
            }
            else if (ogCamera.rotationAxis == "zaxis")
            {
                rotationAxis = Vector3.forward;
            }

            ogCamera.rotateGO.transform.Rotate(rotationAxis * ogCamera.secondaryRotationSpeed * Time.deltaTime);
        }
    }

    #endregion

    #region film camera tools

    //This searches for and returns the target ship
    public static void SearchForShip(OGCamera ogCamera)
    {
        Scene scene = ogCamera.scene;

        if (scene != null)
        {
            if (scene.objectPool != null)
            {
                foreach (GameObject tempShip in scene.objectPool.ToList())
                {
                    if (tempShip != null)
                    {
                        if (tempShip.name.Contains(ogCamera.targetName))
                        {
                            ogCamera.targetShip = tempShip;
                            SaveShipLayer(ogCamera); //This saves the ship layer to restore later if needed
                            break;
                        }
                    }
                }
            }
        }
    }

    //This searches for and returns the target ship
    public static void SearchForPlayerShip(OGCamera ogCamera)
    {
        Scene scene = ogCamera.scene;

        if (scene != null)
        {
            if (scene.objectPool != null)
            {
                foreach (GameObject tempShip in scene.objectPool.ToList())
                {
                    if (tempShip != null)
                    {
                        SmallShip smallShip = tempShip.GetComponent<SmallShip>();

                        if (smallShip != null)
                        {
                            if (smallShip.isAI == false)
                            {
                                ogCamera.targetShip = tempShip;
                                SaveShipLayer(ogCamera); //This saves the ship layer to restore later if needed
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    //This saves the ships layer to restore later is needed
    public static void SaveShipLayer(OGCamera ogCamera)
    {
        if (ogCamera.targetShip != null)
        {
            ogCamera.savedLayerMask = ogCamera.targetShip.layer;
        }
    }

    //This sets the ship to player target layer
    public static void SetTargetShipToPlayerLayer(OGCamera ogCamera)
    {
        if (ogCamera.targetShip != null)
        {
            ogCamera.savedLayerMask = ogCamera.targetShip.layer;

            ogCamera.targetShip.layer = LayerMask.NameToLayer("collision_player");

            GameObjectUtils.SetLayerAllChildren(ogCamera.targetShip.transform, LayerMask.NameToLayer("collision_player"));

            SmallShip smallShip = ogCamera.targetShip.GetComponent<SmallShip>();

            if (smallShip != null)
            {
                LaserFunctions.ChangeCollisionLayerToPlayer(smallShip);
            }
        }
    }

    //This restores the ships layer
    public static void RestoreTargetShipLayer(OGCamera ogCamera)
    {
        if (ogCamera.targetShip != null)
        {
            ogCamera.targetShip.layer = ogCamera.savedLayerMask;

            GameObjectUtils.SetLayerAllChildren(ogCamera.targetShip.transform, ogCamera.savedLayerMask);

            SmallShip smallShip = ogCamera.targetShip.GetComponent<SmallShip>();

            if (smallShip != null)
            {
                LaserFunctions.ResetCollisionLayers(smallShip);
            }
        }
    }

    //This makes the player layer visible
    public static void ViewPlayerLayer()
    {
        GameObject mainCameraGO = GameObject.Find("Main Camera");

        if (mainCameraGO != null)
        {
            Camera mainCamera = mainCameraGO.GetComponent<Camera>();
            mainCamera.cullingMask = LayerMask.GetMask("Default", "collision_player", "collision_asteroid", "collision01", "collision02", "collision03", "collision04", "collision05", "collision06", "collision07", "collision08", "collision09", "collision10", "collision11", "collision12", "collision13", "collision14", "collision15", "collision16");
        }
    }

    //This makes the player layer invisible
    public static void HidePlayerLayer()
    {
        GameObject mainCameraGO = GameObject.Find("Main Camera");

        if (mainCameraGO != null)
        {
            Camera mainCamera = mainCameraGO.GetComponent<Camera>();
            mainCamera.cullingMask = LayerMask.GetMask("Default", "collision_asteroid", "collision01", "collision02", "collision03", "collision04", "collision05", "collision06", "collision07", "collision08", "collision09", "collision10", "collision11", "collision12", "collision13", "collision14", "collision15", "collision16");
        }
    }

    //This gets the film camera script and script gameobject
    public static OGCamera GetOGCamera()
    {
        OGCamera ogCamera = FindFirstObjectByType<OGCamera>();

        return ogCamera;
    }

    #endregion
}
