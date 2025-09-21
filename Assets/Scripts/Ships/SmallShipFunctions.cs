using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

//These functions are called by the smallship script
public static class SmallShipFunctions
{
    #region start functions

    //This prepares the ship by loading lod and colliders (if not already loaded)
    public static void PrepareShip(SmallShip smallShip)
    {
        if (smallShip.loaded == false)
        {
            if (smallShip.isAI == false)
            {
                SceneFunctions.IdentifyAsMainShip(smallShip);
            }
            else if (smallShip.attachCameraToAI == true)
            {
                SceneFunctions.IdentifyAsMainShip(smallShip);
            }

            //This gets the camera position which is used for targetting as well as placing the cockpit
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

            //This gets the camera position which is used for targetting as well as placing the cockpit
            if (smallShip.followCameraPosition == null)
            {
                Transform followCameraPos = GameObjectUtils.FindChildTransformCalled(smallShip.gameObject.transform, "followcamera");

                if (followCameraPos != null)
                {
                    smallShip.followCameraPosition = followCameraPos.gameObject;
                }
            }

            if (smallShip.focusCameraPosition == null)
            {
                Transform focusCameraPos = GameObjectUtils.FindChildTransformCalled(smallShip.gameObject.transform, "focuscamera");

                if (focusCameraPos != null)
                {
                    smallShip.focusCameraPosition = focusCameraPos.gameObject;
                }
            }

            GameObjectUtils.AddMeshColliders(smallShip.gameObject, true);

            if (smallShip.isAI == false)
            {
                GameObjectUtils.AddRigidbody(smallShip.gameObject, 100f, 9f, 7.5f);
            }
            else
            {
                GameObjectUtils.AddRigidbody(smallShip.gameObject, 100f, 9f, 7.5f);
            }

            TorpedoFunctions.GetTorpedoTubes(smallShip);
            smallShip.colliders = smallShip.GetComponentsInChildren<Collider>();
            TargetingFunctions.CreateWaypoint_SmallShipPlayer(smallShip);
            DockingFunctions.AddDockingPointsSmallShip(smallShip);
            SmallShipAIFunctions.SetTargetingErrorMargin(smallShip, "low");

            smallShip.loaded = true;
        }
    }

    //This loads the laser particle system if its missing
    public static void LoadLaserParticleSystem(SmallShip smallShip)
    {
        if (smallShip.laserParticleSystem == null)
        {
            LaserFunctions.GetCannons(smallShip);
            LaserFunctions.LoadLaserParticleSystem(smallShip);
            LaserFunctions.LoadLaserMuzzleFlashParticleSystem(smallShip);
        }

        if (smallShip.ionParticleSystem == null)
        {
            IonFunctions.GetCannons(smallShip);
            IonFunctions.LoadIonMuzzleFlashParticleSystem(smallShip);
            IonFunctions.LoadIonParticleSystem(smallShip);
        }

        if (smallShip.plasmaParticleSystem == null)
        {
            PlasmaFunctions.GetCannons(smallShip);
            PlasmaFunctions.LoadPlasmaMuzzleFlashParticleSystem(smallShip);
            PlasmaFunctions.LoadPlasmaParticleSystem(smallShip);
        }
    }

    #endregion

    #region ship inputs

    //This gets the input from the keyboard and mouse
    public static void GetKeyboardAndMouseInput(SmallShip smallShip)
    {
        if (smallShip.isAI == false & smallShip.automaticRotationTurnAround == false & smallShip.automaticRotationSpin == false & smallShip.controlLock == false)
        {
            if (smallShip.keyboardAndMouse == true)
            {
                //Mouse and Keyboard Input
                var mouse = Mouse.current;
                float x = 0;
                float y = 0;
                float radiusWidth = Screen.width / 2;
                float radiusHeight = Screen.height / 2;

                if (mouse != null)
                {
                    x = mouse.position.x.ReadValue() - radiusWidth;
                    y = mouse.position.y.ReadValue() - radiusHeight;
                }

                x = x / radiusWidth;
                y = y / radiusHeight;

                var keyboard = Keyboard.current;

                float pitchInput = -Mathf.Clamp(y, -1.0f, 1.0f);
                smallShip.rollInput = -Input.GetAxis("LeftHorizontal");
                float turnInput = Mathf.Clamp(x, -1.0f, 1.0f);
                smallShip.thrustInput = Input.GetAxis("LeftVertical");

                if (smallShip.invertUpDown == true)
                {
                    smallShip.pitchInput = -pitchInput;
                }
                else
                {
                    smallShip.pitchInput = pitchInput;
                }

                if (smallShip.invertLeftRight == true)
                {
                    smallShip.turnInput = -turnInput;
                }
                else
                {
                    smallShip.turnInput = turnInput;
                }

                smallShip.powerToShields = keyboard.leftArrowKey.isPressed;
                smallShip.powerToEngine = keyboard.upArrowKey.isPressed;
                smallShip.powerToLasers = keyboard.rightArrowKey.isPressed;
                smallShip.resetPowerLevels = keyboard.downArrowKey.isPressed;
                smallShip.getNextTarget = keyboard.rKey.isPressed;
                smallShip.getNextEnemy = keyboard.tKey.isPressed;
                smallShip.getClosestEnemy = keyboard.fKey.isPressed;
                smallShip.selectTargetInFront = keyboard.gKey.isPressed;
                smallShip.fireWeapon = mouse.leftButton.isPressed;
                smallShip.toggleWeapons = keyboard.tabKey.isPressed;
                smallShip.toggleWeaponNumber = keyboard.capsLockKey.isPressed;
                smallShip.matchSpeed = keyboard.eKey.isPressed;
                smallShip.focusCamera = mouse.rightButton.isPressed;
                smallShip.fireCounterMeasures = keyboard.spaceKey.isPressed;
            }
        }
    }

    //This gets the input from the controller
    public static void GetControllerInput(SmallShip smallShip)
    {
        if (smallShip.isAI == false & smallShip.automaticRotationTurnAround == false & smallShip.automaticRotationSpin == false & smallShip.controlLock == false)
        {
            if (smallShip.keyboardAndMouse == false)
            {
                var gamepad = Gamepad.current;

                float pitchInput = gamepad.rightStick.y.ReadValue();
                float rollInput = -gamepad.leftStick.x.ReadValue();
                float turnInput = gamepad.rightStick.x.ReadValue();

                smallShip.controllerPitch = pitchInput * smallShip.controllerSenstivity;
                smallShip.controllerRoll = rollInput * smallShip.controllerSenstivity;
                smallShip.controllerTurn = turnInput * smallShip.controllerSenstivity;

                //Thrust input and smoothing
                if (gamepad.leftStick.y.ReadValue() > 0.95f & smallShip.controllerThrust < 1)
                {
                    smallShip.controllerThrust = 1;
                }
                else if (gamepad.leftStick.y.ReadValue() < -0.95f & smallShip.controllerThrust > -1)
                {
                    smallShip.controllerThrust = -1;
                }
                else if (gamepad.leftStick.y.ReadValue() > -0.95f & gamepad.leftStick.y.ReadValue() < 0.95f)
                {
                    smallShip.controllerThrust = 0;
                }

                //Actual ship inputs
                if (smallShip.invertUpDown == true)
                {
                    smallShip.pitchInput = smallShip.controllerPitch;
                }
                else
                {
                    smallShip.pitchInput = -smallShip.controllerPitch;
                }

                if (smallShip.invertLeftRight == true)
                {
                    smallShip.turnInput = -smallShip.controllerTurn;
                }
                else
                {
                    smallShip.turnInput = smallShip.controllerTurn;
                }

                smallShip.thrustInput = smallShip.controllerThrust;
                smallShip.rollInput = smallShip.controllerRoll;

                //Button inputs
                smallShip.powerToShields = gamepad.dpad.left.isPressed;
                smallShip.powerToEngine = gamepad.dpad.up.isPressed;
                smallShip.powerToLasers = gamepad.dpad.right.isPressed;
                smallShip.resetPowerLevels = gamepad.dpad.down.isPressed;
                smallShip.getNextTarget = gamepad.leftShoulder.isPressed;
                smallShip.getNextEnemy = gamepad.xButton.isPressed;
                smallShip.getClosestEnemy = gamepad.rightShoulder.isPressed;
                smallShip.selectTargetInFront = gamepad.yButton.isPressed;
                smallShip.fireWeapon = gamepad.rightTrigger.isPressed;
                smallShip.toggleWeapons = gamepad.bButton.isPressed;
                smallShip.toggleWeaponNumber = gamepad.aButton.isPressed;
                smallShip.matchSpeed = gamepad.leftStickButton.isPressed;
                smallShip.focusCamera = gamepad.leftTrigger.isPressed;
            }
        }
    }

    //This starts shaking the controller
    public static void StartShakeController(float leftMotorSpeed, float rightMotorSpeed)
    {
        var gamepad = Gamepad.current;

        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(leftMotorSpeed, rightMotorSpeed);
        }
    }

    //This stops shaking the controller
    public static void StopShakeController()
    {
        var gamepad = Gamepad.current;

        if (gamepad != null)
        {
            // Stop the motors after the duration
            gamepad.SetMotorSpeeds(0f, 0f);
        }
    }

    //This shakes the controller
    public static  IEnumerator ShakeControllerForSetTime(float duration, float leftMotorSpeed, float rightMotorSpeed)
    {
        var gamepad = Gamepad.current;

        if (gamepad != null)
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                gamepad.SetMotorSpeeds(leftMotorSpeed, rightMotorSpeed);
                elapsed += Time.unscaledDeltaTime;
                yield return null; // Wait for the next frame
            }

            // Stop the motors after the duration
            gamepad.SetMotorSpeeds(0f, 0f);
        }
    }

    //This swaps the input depending on what the player is using
    public static void DetectInputType(SmallShip smallShip)
    {
        if (smallShip.isAI == false)
        {
            bool swap = false;

            if (smallShip.keyboardAndMouse == true)
            {
                var gamepad = Gamepad.current;

                if (gamepad != null)
                {
                    if (gamepad.dpad.left.isPressed == true) { swap = true; }
                    else if (gamepad.dpad.left.isPressed) { swap = true; }
                    else if (gamepad.dpad.up.isPressed) { swap = true; }
                    else if (gamepad.dpad.right.isPressed) { swap = true; }
                    else if (gamepad.dpad.down.isPressed) { swap = true; }
                    else if (gamepad.leftShoulder.isPressed) { swap = true; }
                    else if (gamepad.rightShoulder.isPressed) { swap = true; }
                    else if (gamepad.rightTrigger.isPressed) { swap = true; }
                    else if (gamepad.bButton.isPressed) { swap = true; }
                    else if (gamepad.aButton.isPressed) { swap = true; }
                    else if (gamepad.xButton.isPressed) { swap = true; }
                    else if (gamepad.yButton.isPressed) { swap = true; }
                    else if (gamepad.startButton.isPressed) { swap = true; }
                    else if (gamepad.selectButton.isPressed) { swap = true; }
                    else if (gamepad.rightStickButton.isPressed) { swap = true; }
                    else if (gamepad.leftStickButton.isPressed) { swap = true; }
                    else if (gamepad.leftTrigger.isPressed) { swap = true; }
                }
            }
            else
            {
                var keyboard = Keyboard.current;
                var mouse = Mouse.current;

                if (keyboard != null)
                {
                    if (keyboard.anyKey.wasPressedThisFrame == true) { swap = true; }
                }

                if (mouse != null)
                {
                    if (mouse.leftButton.isPressed == true) { swap = true; }
                    else if (mouse.rightButton.isPressed == true) { swap = true; }
                }
            }

            if (swap == true)
            {
                smallShip.keyboardAndMouse = !smallShip.keyboardAndMouse;
            }
        }
    }

    //This gets the AI input
    public static void GetAIInput(SmallShip smallShip)
    {
        if (smallShip.isAI == true & smallShip.automaticRotationTurnAround == false & smallShip.automaticRotationSpin == false & smallShip.controlLock == false)
        {
            SmallShipAIFunctions.GetAIInput(smallShip);
        }
    }

    //This causes the ship to match the speed of it's target (not used by AI)
    public static void MatchSpeed(SmallShip smallShip)
    {
        if (smallShip.target != null & smallShip.matchSpeed == true)
        {
            if (smallShip.target.activeSelf != false)
            {
                if (smallShip.thrustSpeed > smallShip.targetSpeed)
                {
                    smallShip.thrustInput = -1;
                }
                else if (smallShip.thrustSpeed < smallShip.targetSpeed)
                {
                    smallShip.thrustInput = 1;
                }
            }
        }
    }

    //This autmomatically turns the ship around when it reaches the boundaries of the game area, i.e. 15000m
    public static void TurnShipAround(SmallShip smallShip)
    {
        if (smallShip.scene != null)
        {
            Vector3 center = smallShip.scene.transform.position;
            Vector3 currentPosition = smallShip.gameObject.transform.position;

            float currentDistance = Vector3.Distance(currentPosition, center);

            if (currentDistance > smallShip.scene.sceneRadius)
            {
                smallShip.automaticRotationTurnAround = true;

                Vector3 targetRelativePosition = center - currentPosition;

                float forward = Vector3.Dot(smallShip.gameObject.transform.forward, targetRelativePosition.normalized);
                float right = Vector3.Dot(smallShip.gameObject.transform.right, targetRelativePosition.normalized);
                float up = Vector3.Dot(smallShip.gameObject.transform.up, targetRelativePosition.normalized);

                if (forward < 0.8)
                {
                    SmoothTurnInput(smallShip, right);
                    SmoothPitchInput(smallShip, -up);
                }
                else
                {
                    SmoothTurnInput(smallShip, right * 5);
                    SmoothPitchInput(smallShip, -up * 5);
                }

                smallShip.thrustInput = 1;

                if (smallShip.messageSent == false & smallShip.isAI == false)
                {
                    HudFunctions.AddToShipLog("WARNING: Too far out turning around");
                    smallShip.messageSent = true;
                }
            }
            else
            {
                smallShip.automaticRotationTurnAround = false;
                smallShip.messageSent = false;
            }
        }
    }

    //This automatically spins the ship on the x-axis when its hit by a torpedo or destroyed
    public static void SpinShip(SmallShip smallShip)
    {
        if (smallShip.spinShip == true)
        {
            smallShip.automaticRotationSpin = true;
            SmoothTurnInput(smallShip, 0);
            SmoothPitchInput(smallShip, 0);

            if (smallShip.rollInputActual > 0)
            {
                SmoothRollInput(smallShip, 1);
            }
            else if (smallShip.rollInputActual < 0)
            {
                SmoothRollInput(smallShip, -1);
            }
            else
            {
                SmoothRollInput(smallShip, 1);
            }
        }
        else
        {
            smallShip.automaticRotationSpin = false;
        }
    }

    //When activated this prevents the ship from turning from its present course
    public static void ControlLock(SmallShip smallShip)
    {
        if (smallShip.controlLock == true)
        {
            SmoothTurnInput(smallShip, 0);
            SmoothPitchInput(smallShip, 0);
            SmoothRollInput(smallShip, 0);
        }
    }

    //For AI Input. These functions smoothly transitions between different pitch, turn, and roll inputs by lerping between different values like the ai is using a joystick or controller
    public static void SmoothPitchInput(SmallShip smallShip, float pitchInput)
    {
        float step = +Time.deltaTime / 0.01f;
        smallShip.pitchInput = Mathf.Lerp(smallShip.pitchInput, pitchInput, step);
    }

    public static void SmoothTurnInput(SmallShip smallShip, float turnInput)
    {
        float step = +Time.deltaTime / 0.01f;
        smallShip.turnInput = Mathf.Lerp(smallShip.turnInput, turnInput, step);
    }

    public static void SmoothRollInput(SmallShip smallShip, float rollInput)
    {
        float step = +Time.deltaTime / 0.01f;
        smallShip.rollInput = Mathf.Lerp(smallShip.rollInput, rollInput, step);
    }

    #endregion

    #region energy management
    
    //This calculates the ships power distribution
    public static void CalculatePower(SmallShip smallShip)
    {
        if (smallShip.powerPressedTime < Time.time)
        {
            //This checks the current power mode
            if (smallShip.powerToLasers == true)
            {
                smallShip.powerMode = "lasers";

                if (smallShip.isAI == false)
                {
                    AudioFunctions.PlayAudioClip(smallShip.audioManager, "beep01_toggle", "Cockpit", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);
                }

            }
            else if (smallShip.powerToEngine == true)
            {
                smallShip.powerMode = "engines";

                if (smallShip.isAI == false)
                {
                    AudioFunctions.PlayAudioClip(smallShip.audioManager, "beep01_toggle", "Cockpit", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);
                }

            }
            else if (smallShip.powerToShields == true)
            {
                smallShip.powerMode = "shields";

                if (smallShip.isAI == false)
                {
                    AudioFunctions.PlayAudioClip(smallShip.audioManager, "beep01_toggle", "Cockpit", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);
                }

            }
            else if (smallShip.resetPowerLevels == true)
            {
                smallShip.powerMode = "reset";

                if (smallShip.isAI == false)
                {
                    AudioFunctions.PlayAudioClip(smallShip.audioManager, "beep01_toggle", "Cockpit", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);
                }

            }

            smallShip.powerPressedTime = Time.time + 0.2f;

        }

        //This sets the ships power according the mode
        if (smallShip.shieldRating != 0)
        {
            if (smallShip.powerMode == "lasers")
            {
                if (smallShip.laserPower < 100) { smallShip.laserPower += 1; }
                if (smallShip.enginePower > 25) { smallShip.enginePower -= 1; }
                if (smallShip.shieldPower > 25) { smallShip.shieldPower -= 1; }
            }
            else if (smallShip.powerMode == "engines")
            {
                if (smallShip.laserPower > 25) { smallShip.laserPower -= 1; }
                if (smallShip.enginePower < 100) { smallShip.enginePower += 1; }
                if (smallShip.shieldPower > 25) { smallShip.shieldPower -= 1; }
            }
            else if (smallShip.powerMode == "shields")
            {
                if (smallShip.laserPower > 25) { smallShip.laserPower -= 1; }
                if (smallShip.enginePower > 25) { smallShip.enginePower -= 1; }
                if (smallShip.shieldPower < 100) { smallShip.shieldPower += 1; }
            }
            else if (smallShip.powerMode == "reset")
            {
                if (smallShip.laserPower > 50) { smallShip.laserPower -= 1; } else if (smallShip.laserPower < 50) { smallShip.laserPower += 1; }
                if (smallShip.enginePower > 50) { smallShip.enginePower -= 1; } else if (smallShip.enginePower < 50) { smallShip.enginePower += 1; }
                if (smallShip.shieldPower > 50) { smallShip.shieldPower -= 1; } else if (smallShip.shieldPower < 50) { smallShip.shieldPower += 1; }
            }
        }
        else
        {
            if (smallShip.powerMode == "lasers")
            {
                if (smallShip.laserPower < 100) { smallShip.laserPower += 1; }
                if (smallShip.enginePower > 25) { smallShip.enginePower -= 1; }
                if (smallShip.shieldPower > 0) { smallShip.shieldPower -= 1; }
            }
            else if (smallShip.powerMode == "engines")
            {
                if (smallShip.laserPower > 25) { smallShip.laserPower -= 1; }
                if (smallShip.enginePower < 100) { smallShip.enginePower += 1; }
                if (smallShip.shieldPower > 0) { smallShip.shieldPower -= 1; }
            }
            else if (smallShip.powerMode == "shields")
            {
                if (smallShip.laserPower > 50) { smallShip.laserPower -= 1; } else if (smallShip.laserPower < 50) { smallShip.laserPower += 1; }
                if (smallShip.enginePower > 50) { smallShip.enginePower -= 1; } else if (smallShip.enginePower < 50) { smallShip.enginePower += 1; }
                if (smallShip.shieldPower > 0) { smallShip.shieldPower -= 1; }
            }
            else if (smallShip.powerMode == "reset")
            {
                if (smallShip.laserPower > 50) { smallShip.laserPower -= 1; } else if (smallShip.laserPower < 50) { smallShip.laserPower += 1; }
                if (smallShip.enginePower > 50) { smallShip.enginePower -= 1; } else if (smallShip.enginePower < 50) { smallShip.enginePower += 1; }
                if (smallShip.shieldPower > 0) { smallShip.shieldPower -= 1; }
            }
        }

       

    }

    //This calculates the ships power levels
    public static void CalculateLevels(SmallShip smallShip)
    {
        //This sets the recharge and discharge rate if not set for wep
        if (smallShip.wepRecharge == 0) { smallShip.wepRecharge = 0.1f; }
        if (smallShip.wepDischarge == 0) { smallShip.wepDischarge = 0.25f; }

        //This sets the ships wep power levels
        if (smallShip.powerMode == "engines")
        {
            if (smallShip.wep == false & smallShip.thrustInput <= 0)
            {
                if (smallShip.wepLevel < 100) { smallShip.wepLevel += smallShip.wepRecharge; }
            }
            else
            {
                if (smallShip.wepLevel > 0) { smallShip.wepLevel -= smallShip.wepDischarge; }
            }
        }
        else if (smallShip.powerMode == "reset")
        {
            if (smallShip.wep == false & smallShip.thrustInput <= 0)
            {
                if (smallShip.wepLevel > 50) { smallShip.wepLevel -= smallShip.wepDischarge; }
                else if (smallShip.wepLevel < 50) { smallShip.wepLevel += smallShip.wepRecharge; }
            }
            else
            {
                if (smallShip.wepLevel > 0) { smallShip.wepLevel -= smallShip.wepDischarge; }
            }
        }
        else
        {
            if (smallShip.wepLevel > 0) { smallShip.wepLevel -= smallShip.wepDischarge; }
        }

        //This sets the recharge and discharge rate if not set for shields
        if (smallShip.shieldRecharge == 0) { smallShip.shieldRecharge = 0.01f; }
        if (smallShip.shieldDischarge == 0) { smallShip.shieldDischarge = 0.01f; }

        //This sets the ships shield power levels
        if (smallShip.shieldRating != 0)
        {
            if (smallShip.powerMode == "shields")
            {
                if (smallShip.frontShieldLevel < smallShip.shieldRating / 2f)
                {
                    smallShip.frontShieldLevel += smallShip.shieldRecharge;
                }

                if (smallShip.rearShieldLevel < smallShip.shieldRating / 2f)
                {
                    smallShip.rearShieldLevel += smallShip.shieldRecharge;
                }

                smallShip.shieldLevel = smallShip.rearShieldLevel + smallShip.frontShieldLevel;

            }
            else if (smallShip.powerMode != "shields" & smallShip.powerMode != "reset")
            {
                if (smallShip.frontShieldLevel > 0)
                {
                    smallShip.frontShieldLevel -= smallShip.shieldDischarge;
                }

                if (smallShip.rearShieldLevel > 0)
                {
                    smallShip.rearShieldLevel -= smallShip.shieldDischarge;
                }

                smallShip.shieldLevel = smallShip.rearShieldLevel + smallShip.frontShieldLevel;

            }
        }
    }

    #endregion

    #region ship movement

    //This calculates the thrust speed of the ship
    public static void CalculateThrustSpeed(SmallShip smallShip)
    {
        //This calculates the normal accleration and speed rating
        float acclerationAmount = (0.5f / 100f) * smallShip.accelerationRating;
        float actualSpeedRating = smallShip.speedRating;

        //This calculates the accleration and speedrating according to different power modes
        if (smallShip.powerMode == "reset" & smallShip.thrustInput > 0 & smallShip.wepLevel > 1 || smallShip.powerMode == "engines" & smallShip.thrustInput > 0 & smallShip.wepLevel > 1)
        {
            actualSpeedRating = smallShip.speedRating + smallShip.wepRating;
            acclerationAmount = acclerationAmount * 2;
            smallShip.wep = true;
            StartShakeController(0.20f, 0.20f);
        }
        else if (smallShip.powerMode == "lasers" || smallShip.powerMode == "shields")
        {
            actualSpeedRating = (smallShip.speedRating / 100f) * 75f;

            if (smallShip.wep == true)
            {
                StopShakeController();
            }
            
            smallShip.wep = false;
        }
        else
        {
            if (smallShip.wep == true)
            {
                StopShakeController();
            }

            smallShip.wep = false;
        }

        //This controls the throttle of the ship, and prevents it going above the speed rating or below zero
        if (smallShip.thrustSpeed > actualSpeedRating)
        {
            smallShip.thrustSpeed = smallShip.thrustSpeed - acclerationAmount * 4;
        }
        else if (smallShip.thrustInput < 0 & smallShip.thrustTimeStamp < Time.time)
        {
            smallShip.thrustSpeed = smallShip.thrustSpeed - acclerationAmount;
            smallShip.thrustTimeStamp = Time.time + 0.01f;
        }
        else if (smallShip.thrustInput > 0 & smallShip.thrustTimeStamp < Time.time)
        {
            smallShip.thrustSpeed = smallShip.thrustSpeed + acclerationAmount;
            smallShip.thrustTimeStamp = Time.time + 0.01f;
        }

        if (smallShip.thrustSpeed < 0)
        {
            smallShip.thrustSpeed = 0;
        }

    }

    //This calculates pitch, turn, and roll according to the speed of the vehicle
    public static void CalculatePitchTurnRollSpeeds(SmallShip smallShip)
    {
        float peakManeuverSpeed = smallShip.speedRating / 2f;
        float currentManeuverablity = 0f;
        float manveurablityPercentageAsDecimal = 0f;

        if (smallShip.thrustSpeed <= peakManeuverSpeed & smallShip.thrustSpeed > (peakManeuverSpeed / 3f))
        {
            currentManeuverablity = (100f / peakManeuverSpeed) * smallShip.thrustSpeed;
        }
        else if (smallShip.thrustSpeed >= peakManeuverSpeed & smallShip.thrustSpeed < (smallShip.speedRating - (peakManeuverSpeed / 3f)))
        {
            currentManeuverablity = (100f / peakManeuverSpeed) * (peakManeuverSpeed - (smallShip.thrustSpeed - peakManeuverSpeed));
        }
        else
        {
            currentManeuverablity = (100f / peakManeuverSpeed) * (peakManeuverSpeed / 3f);
        }

        manveurablityPercentageAsDecimal = (smallShip.maneuverabilityRating / 100f);

        smallShip.pitchSpeed = (120f / 100f) * (currentManeuverablity * manveurablityPercentageAsDecimal);
        smallShip.turnSpeed = (100f / 100f) * (currentManeuverablity * manveurablityPercentageAsDecimal);
        smallShip.rollSpeed = (160f / 100f) * (currentManeuverablity * manveurablityPercentageAsDecimal);

        if (smallShip.spinShip == true)
        {
            smallShip.rollSpeed = ((160f / 100f) * 100) * 2.5f * manveurablityPercentageAsDecimal;
        }

    }

    //This makes the ship move
    public static void MoveShip(SmallShip smallShip)
    {
        if (smallShip.shipRigidbody == null)
        {
            smallShip.shipRigidbody = smallShip.gameObject.GetComponent<Rigidbody>();
        }

        if (smallShip.shipRigidbody != null & smallShip.jumpingToHyperspace == false & smallShip.exitingHyperspace == false & smallShip.docking == false & smallShip.isDisabled == false & smallShip.flyInFormation == false)
        {
            //This smoothly increases and decreases pitch, turn, and roll to provide smooth movement;
            float step = +Time.deltaTime / 0.1f;
            smallShip.pitchInputActual = Mathf.Lerp(smallShip.pitchInputActual, smallShip.pitchInput, step);
            smallShip.turnInputActual = Mathf.Lerp(smallShip.turnInputActual, smallShip.turnInput, step);
            smallShip.rollInputActual = Mathf.Lerp(smallShip.rollInputActual, smallShip.rollInput, step);

            //This adds makes the ship move forward
            smallShip.shipRigidbody.AddForce(smallShip.gameObject.transform.position + smallShip.gameObject.transform.forward * Time.fixedDeltaTime * smallShip.thrustSpeed * 60000);

            //This rotates the ship
            Vector3 x = Vector3.right * smallShip.pitchSpeed * smallShip.pitchInputActual;
            Vector3 y = Vector3.up * smallShip.turnSpeed * smallShip.turnInputActual;
            Vector3 z = Vector3.forward * smallShip.rollSpeed * smallShip.rollInputActual;

            Vector3 rotationVector = x + y + z;

            Quaternion deltaRotation = Quaternion.Euler(rotationVector * Time.deltaTime);
            smallShip.shipRigidbody.MoveRotation(smallShip.shipRigidbody.rotation * deltaRotation);
        }
        else if (smallShip.flyInFormation == true)
        {
            // Build a flat rotation from the leader's yaw
            Quaternion flatLeaderRotation = Quaternion.Euler(0, smallShip.followTarget.transform.eulerAngles.y, 0);

            // Apply offset using only yaw rotation (no pitch/roll)
            Vector3 desiredPosition = smallShip.followTarget.transform.position + flatLeaderRotation * new Vector3(smallShip.xFormationPos, smallShip.yFormationPos, smallShip.zFormationPos);

            float distance = Vector3.Distance(smallShip.transform.position, desiredPosition);

            float positionLerpSpeed = 3f;   // Smooth movement
            float rotationLerpSpeed = 3f;   // Smooth rotation

            if (distance > 500)
            {
                positionLerpSpeed = 1f;   // Smooth movement
                rotationLerpSpeed = 1f;   // Smooth rotation
            }
           
            // Move smoothly to the desired position
            smallShip.transform.position = Vector3.Lerp(smallShip.transform.position, desiredPosition, Time.deltaTime * positionLerpSpeed);

            // Optionally match rotation (can be adjusted for looser formations)
            smallShip.transform.rotation = smallShip.followTarget.transform.rotation;
        }
    }

    //Jump to Hyperspace
    public static IEnumerator JumpToHyperspace(SmallShip smallShip)
    {
        CloseWings(smallShip);

        smallShip.jumpingToHyperspace = true;

        Vector3 startPosition = smallShip.gameObject.transform.localPosition;
        Vector3 endPosition = smallShip.transform.localPosition + smallShip.gameObject.transform.forward * 30000;

        float timeElapsed = 0;
        float lerpDuration = 1;

        while (timeElapsed < lerpDuration)
        {
            smallShip.gameObject.transform.localPosition = Vector3.Lerp(startPosition, endPosition, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        HudFunctions.AddToShipLog(smallShip.name.ToUpper() + " jumped to hyperspace");

        smallShip.jumpingToHyperspace = false;

        DamageFunctions.DeactivateShip_SmallShip(smallShip);
    }

    //Exit Hyperspace
    public static IEnumerator ExitHyperspace(SmallShip smallShip)
    {
        SnapClosedWings(smallShip); //Keeps wings shut on hyperspace exit

        smallShip.exitingHyperspace = true;

        Vector3 endPosition = smallShip.transform.localPosition + smallShip.gameObject.transform.forward * 30000; 
        Vector3 startPosition = smallShip.gameObject.transform.localPosition;

        float timeElapsed = 0;
        float lerpDuration = 1;

        while (timeElapsed < lerpDuration)
        {
            smallShip.gameObject.transform.localPosition = Vector3.Lerp(startPosition, endPosition, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        smallShip.gameObject.transform.localPosition = endPosition;

        AudioFunctions.PlayAudioClip(smallShip.audioManager, "hyperspace03_exit", "Explosions", smallShip.transform.position, 1, 1, 1000, 1f);

        HudFunctions.AddToShipLog(smallShip.name.ToUpper() + " just exited hyperspace");

        OpenWings(smallShip); //Opens wings after hyperspace exit

        smallShip.exitingHyperspace = false;
    }

    //A particle effect that makes the ship look like it's moving
    public static void MovementEffect(SmallShip smallShip)
    {
        if (smallShip.isAI == false)
        {
            if(smallShip.movementEffect == null)
            {
                Object tempMovementEffect = PoolUtils.FindPrefabObjectInPool(smallShip.scene.particlePrefabPool, "MovementEffect");

                if (tempMovementEffect != null)
                {
                    GameObject movementEffect = GameObject.Instantiate(tempMovementEffect) as GameObject;
                    
                    if (movementEffect != null)
                    {
                        movementEffect.transform.SetParent(smallShip.gameObject.transform);
                        movementEffect.transform.position = smallShip.cameraPosition.transform.position + new Vector3(0, 0, 5);
                        movementEffect.transform.localRotation = Quaternion.identity;
                        smallShip.movementEffect = movementEffect.GetComponent<ParticleSystem>();
                    }
                }
            }
            else if (smallShip.movementEffect.gameObject.activeSelf == false & smallShip.thrustSpeed > 10)
            {
                smallShip.movementEffect.gameObject.SetActive(true);
            }
            else if (smallShip.thrustSpeed > 10)
            {
                float particleSpeed = (4f / smallShip.speedRating) * smallShip.thrustSpeed;
                var main = smallShip.movementEffect.main;
                main.simulationSpeed = particleSpeed;
            }
            else if (smallShip.thrustSpeed < 10)
            {
                smallShip.movementEffect.gameObject.SetActive(false);
            }
        }
        else
        {
            if(smallShip.movementEffect != null)
            {
                smallShip.movementEffect.gameObject.SetActive(false);
            }
        }
    }

    #endregion

    #region weapons

    //This toggles between different types of weapons
    public static void ToggleWeapons(SmallShip smallShip)
    {
        if (smallShip.hasPlasma == false)
        {
            if (smallShip.toggleWeapons == true & smallShip.toggleWeaponPressedTime < Time.time & smallShip.isDisabled == false & smallShip.preventWeaponChange == false)
            {
                if (smallShip.hasTorpedos == true & smallShip.torpedoNumber > 0 & smallShip.hasIon == true)
                {
                    if (smallShip.activeWeapon == "" || smallShip.activeWeapon == "---" || smallShip.activeWeapon == "plasma")
                    {
                        smallShip.activeWeapon = "lasers";
                        smallShip.weaponMode = "single";
                    }

                    if (smallShip.activeWeapon == "lasers")
                    {
                        smallShip.activeWeapon = "ion";
                        smallShip.weaponMode = "single";
                    }
                    else if (smallShip.activeWeapon == "ion")
                    {
                        smallShip.activeWeapon = "torpedos";
                        smallShip.weaponMode = "single";
                    }
                    else if (smallShip.activeWeapon == "torpedos")
                    {
                        smallShip.activeWeapon = "lasers";
                        smallShip.weaponMode = "single";
                    }

                    if (smallShip.isAI == false)
                    {
                        AudioFunctions.PlayAudioClip(smallShip.audioManager, "beep03_weaponchange", "Cockpit", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);
                    }
                }
                else if (smallShip.hasTorpedos == true & smallShip.torpedoNumber > 0 & smallShip.hasIon == false)
                {
                    if (smallShip.activeWeapon == "" || smallShip.activeWeapon == "---" || smallShip.activeWeapon == "ion" || smallShip.activeWeapon == "plasma")
                    {
                        smallShip.activeWeapon = "lasers";
                        smallShip.weaponMode = "single";
                    }

                    if (smallShip.activeWeapon == "lasers")
                    {
                        smallShip.activeWeapon = "torpedos";
                        smallShip.weaponMode = "single";
                    }
                    else if (smallShip.activeWeapon == "torpedos")
                    {
                        smallShip.activeWeapon = "lasers";
                        smallShip.weaponMode = "single";
                    }

                    if (smallShip.isAI == false)
                    {
                        AudioFunctions.PlayAudioClip(smallShip.audioManager, "beep03_weaponchange", "Cockpit", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);
                    }
                }
                else if (smallShip.hasTorpedos == false & smallShip.hasIon == true || smallShip.hasTorpedos == true & smallShip.torpedoNumber <= 0 & smallShip.hasIon == true)
                {
                    if (smallShip.activeWeapon == "" || smallShip.activeWeapon == "---" || smallShip.activeWeapon == "torpedos" || smallShip.activeWeapon == "plasma")
                    {
                        smallShip.activeWeapon = "lasers";
                        smallShip.weaponMode = "single";
                    }

                    if (smallShip.activeWeapon == "lasers")
                    {
                        smallShip.activeWeapon = "ion";
                        smallShip.weaponMode = "single";
                    }
                    else if (smallShip.activeWeapon == "ion")
                    {
                        smallShip.activeWeapon = "lasers";
                        smallShip.weaponMode = "single";
                    }

                    if (smallShip.isAI == false)
                    {
                        AudioFunctions.PlayAudioClip(smallShip.audioManager, "beep03_weaponchange", "Cockpit", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);
                    }
                }
                else
                {
                    smallShip.activeWeapon = "lasers";
                }

                smallShip.toggleWeaponPressedTime = Time.time + 0.25f;
            }
            else if (smallShip.isDisabled == true)
            {
                smallShip.activeWeapon = "---";
                smallShip.weaponMode = "---";
            }

            if (smallShip.hasTorpedos == true & smallShip.torpedoNumber <= 0 & smallShip.activeWeapon == "torpedos")
            {
                smallShip.activeWeapon = "lasers";
                smallShip.weaponMode = "single";
            }

            smallShip.toggleWeapons = false;
        }
        else
        {
            if (smallShip.toggleWeapons == true & smallShip.toggleWeaponPressedTime < Time.time & smallShip.isDisabled == false & smallShip.preventWeaponChange == false)
            {
                if (smallShip.hasTorpedos == true & smallShip.torpedoNumber > 0)
                {
                    if (smallShip.activeWeapon == "" || smallShip.activeWeapon == "---" || smallShip.activeWeapon == "ion" || smallShip.activeWeapon == "lasers")
                    {
                        smallShip.activeWeapon = "plasma";
                        smallShip.weaponMode = "single";
                    }

                    if (smallShip.activeWeapon == "plasma")
                    {
                        smallShip.activeWeapon = "torpedos";
                        smallShip.weaponMode = "single";
                    }
                    else if (smallShip.activeWeapon == "torpedos")
                    {
                        smallShip.activeWeapon = "plasma";
                        smallShip.weaponMode = "single";
                    }

                    if (smallShip.isAI == false)
                    {
                        AudioFunctions.PlayAudioClip(smallShip.audioManager, "beep03_weaponchange", "Cockpit", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);
                    }
                }
                else if (smallShip.hasTorpedos == true & smallShip.torpedoNumber <= 0)
                {
                    smallShip.activeWeapon = "plasma";
                    smallShip.weaponMode = "single";

                    if (smallShip.isAI == false)
                    {
                        AudioFunctions.PlayAudioClip(smallShip.audioManager, "beep03_weaponchange", "Cockpit", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);
                    }
                }
                else
                {
                    smallShip.activeWeapon = "plasma";
                }

                smallShip.toggleWeaponPressedTime = Time.time + 0.25f;
            }
            else if (smallShip.isDisabled == true)
            {
                smallShip.activeWeapon = "---";
                smallShip.weaponMode = "---";
            }

            if (smallShip.hasTorpedos == true & smallShip.torpedoNumber <= 0 & smallShip.activeWeapon == "torpedos")
            {
                smallShip.activeWeapon = "plasma";
                smallShip.weaponMode = "single";
            }

            smallShip.toggleWeapons = false;
        }
    }

    //This manually sets the weapon on a smallship
    public static void SetWeapons(SmallShip smallShip, string weapon, string mode = "single")
    {
        smallShip.activeWeapon = weapon;
        smallShip.weaponMode = mode;

        if (smallShip.isAI == false)
        {
            AudioFunctions.PlayAudioClip(smallShip.audioManager, "beep03_weaponchange", "Cockpit", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);
        }
    }

    #endregion weapons

    #region open and close wings

    //Opens the wings
    public static void OpenWings(SmallShip smallShip)
    {
        if (smallShip.wingsOpen != true)
        {
            //This searches for movable wings on the ship if they haven't already been loaded
            if (smallShip.wings == null)
            {
                FindMovableWings(smallShip);
            }

            //This indicates to other functions whether the wings are open or closed
            smallShip.wingsOpen = true;

            //This activates the wing rotation
            if (smallShip.wing01 != null & smallShip.wing01_open != null & smallShip.wing01_closed != null)
            {
                Task a = new Task(RotateToWingPosition(smallShip.wing01, smallShip.wing01_open.transform, smallShip.wing01_closed.transform, 3.4f, true));
                AddTaskToPool(smallShip, a);

                //This plays the wings open and close sound
                float spatialBlend = 1f;
                string mixer = "External";

                if (smallShip.isAI == false)
                {
                    spatialBlend = 0;
                    mixer = "Cockpit";
                }

                AudioFunctions.PlayAudioClip(smallShip.audioManager, "wings_open", mixer, smallShip.transform.position, spatialBlend, 1, 500, 0.6f);
            }

            if (smallShip.wing02 != null & smallShip.wing02_open != null & smallShip.wing02_closed != null)
            {
                Task a = new Task(RotateToWingPosition(smallShip.wing02, smallShip.wing02_open.transform, smallShip.wing02_closed.transform, 3.4f, true));
                AddTaskToPool(smallShip, a);
            }

            if (smallShip.wing03 != null & smallShip.wing03_open != null & smallShip.wing03_closed != null)
            {
                Task a = new Task(RotateToWingPosition(smallShip.wing03, smallShip.wing03_open.transform, smallShip.wing03_closed.transform, 3.4f, true));
                AddTaskToPool(smallShip, a);
            }

            if (smallShip.wing04 != null & smallShip.wing04_open != null & smallShip.wing04_closed != null)
            {
                Task a = new Task(RotateToWingPosition(smallShip.wing04, smallShip.wing04_open.transform, smallShip.wing04_closed.transform, 3.4f, true));
                AddTaskToPool(smallShip, a);
            }
        }
    }

    //Closes the wings
    public static void CloseWings(SmallShip smallShip)
    {
        if (smallShip.wingsOpen != false)
        {
            //This searches for movable wings on the ship if they haven't already been loaded
            if (smallShip.wings == null)
            {
                FindMovableWings(smallShip);
            }

            //This indicates to other functions whether the wings are open or closed
            smallShip.wingsOpen = false;

            //This activates the wing rotation
            if (smallShip.wing01 != null & smallShip.wing01_open != null & smallShip.wing01_closed != null)
            {
                Task a = new Task(RotateToWingPosition(smallShip.wing01, smallShip.wing01_open.transform, smallShip.wing01_closed.transform, 3.4f, false));
                AddTaskToPool(smallShip, a);

                //This plays the wings open and close sound
                float spatialBlend = 1f;
                string mixer = "External";

                if (smallShip.isAI == false)
                {
                    spatialBlend = 0;
                    mixer = "Cockpit";
                }

                AudioFunctions.PlayAudioClip(smallShip.audioManager, "wings_close", mixer, smallShip.transform.position, spatialBlend, 1, 500, 0.6f);
            }

            if (smallShip.wing02 != null & smallShip.wing02_open != null & smallShip.wing02_closed != null)
            {
                Task a = new Task(RotateToWingPosition(smallShip.wing02, smallShip.wing02_open.transform, smallShip.wing02_closed.transform, 3.4f, false));
                AddTaskToPool(smallShip, a);
            }

            if (smallShip.wing03 != null & smallShip.wing03_open != null & smallShip.wing03_closed != null)
            {
                Task a = new Task(RotateToWingPosition(smallShip.wing03, smallShip.wing03_open.transform, smallShip.wing03_closed.transform, 3.4f, false));
                AddTaskToPool(smallShip, a);
            }

            if (smallShip.wing04 != null & smallShip.wing04_open != null & smallShip.wing04_closed != null)
            {
                Task a = new Task(RotateToWingPosition(smallShip.wing04, smallShip.wing04_open.transform, smallShip.wing04_closed.transform, 3.4f, false));
                AddTaskToPool(smallShip, a);
            }
        }
    }

    //This rotates a wing to the designated position
    public static IEnumerator RotateToWingPosition(GameObject wing, Transform openPosition, Transform closePosition, float speed, bool open)
    {
        Quaternion startRotation = closePosition.localRotation;
        Quaternion endRotation = openPosition.localRotation;

        if (open == false)
        {
            startRotation = openPosition.localRotation;
            endRotation = closePosition.localRotation;
        }

        float timeElapsed = 0;
        float lerpDuration = speed;

        while (timeElapsed < lerpDuration)
        {
            if (wing != null)
            {
                wing.transform.localRotation = Quaternion.Lerp(startRotation, endRotation, timeElapsed / lerpDuration);
            }

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        if (wing != null)
        {
            wing.transform.localRotation = endRotation;
        }
    }

    //Snaps the wing open
    public static void SnapOpenWings(SmallShip smallShip)
    {
        if (smallShip.wingsOpen != true)
        {
            //This searches for movable wings on the ship if they haven't already been loaded
            if (smallShip.wings == null)
            {
                FindMovableWings(smallShip);
            }

            //This indicates to other functions whether the wings are open or closed
            smallShip.wingsOpen = true;

            //This activates the wing rotation
            if (smallShip.wing01 != null & smallShip.wing01_open != null & smallShip.wing01_closed != null)
            {
                SnapToWingPosition(smallShip.wing01, smallShip.wing01_open.transform, smallShip.wing01_closed.transform, 2, true);
            }

            if (smallShip.wing02 != null & smallShip.wing02_open != null & smallShip.wing02_closed != null)
            {
                SnapToWingPosition(smallShip.wing02, smallShip.wing02_open.transform, smallShip.wing02_closed.transform, 2, true);
            }

            if (smallShip.wing03 != null & smallShip.wing03_open != null & smallShip.wing03_closed != null)
            {
                SnapToWingPosition(smallShip.wing03, smallShip.wing03_open.transform, smallShip.wing03_closed.transform, 2, true);
            }

            if (smallShip.wing04 != null & smallShip.wing04_open != null & smallShip.wing04_closed != null)
            {
                SnapToWingPosition(smallShip.wing04, smallShip.wing04_open.transform, smallShip.wing04_closed.transform, 2, true);
            }
        }
    }

    //Snaps the wings shut
    public static void SnapClosedWings(SmallShip smallShip)
    {
        if (smallShip.wingsOpen != false)
        {
            //This searches for movable wings on the ship if they haven't already been loaded
            if (smallShip.wings == null)
            {
                FindMovableWings(smallShip);
            }

            //This indicates to other functions whether the wings are open or closed
            smallShip.wingsOpen = false;

            //This activates the wing rotation
            if (smallShip.wing01 != null & smallShip.wing01_open != null & smallShip.wing01_closed != null)
            {
                SnapToWingPosition(smallShip.wing01, smallShip.wing01_open.transform, smallShip.wing01_closed.transform, 2, false);
            }

            if (smallShip.wing02 != null & smallShip.wing02_open != null & smallShip.wing02_closed != null)
            {
                SnapToWingPosition(smallShip.wing02, smallShip.wing02_open.transform, smallShip.wing02_closed.transform, 2, false);
            }

            if (smallShip.wing03 != null & smallShip.wing03_open != null & smallShip.wing03_closed != null)
            {
                SnapToWingPosition(smallShip.wing03, smallShip.wing03_open.transform, smallShip.wing03_closed.transform, 2, false);
            }

            if (smallShip.wing04 != null & smallShip.wing04_open != null & smallShip.wing04_closed != null)
            {
                SnapToWingPosition(smallShip.wing04, smallShip.wing04_open.transform, smallShip.wing04_closed.transform, 2, false);
            }
        }
    }

    //This snaps a wing to the desinated position
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

    //This finds any wings that can be open and closed on the craft    
    public static void FindMovableWings(SmallShip smallShip)
    {
        smallShip.wings = GameObjectUtils.FindAllChildTransformsContaining(smallShip.transform, "wing");

        if (smallShip.wings != null)
        {
            foreach (Transform wing in smallShip.wings)
            {
                if (wing.name == "wing01")
                {
                    smallShip.wing01 = wing.gameObject;
                }
                else if (wing.name == "wing02")
                {
                    smallShip.wing02 = wing.gameObject;
                }
                else if (wing.name == "wing03")
                {
                    smallShip.wing03 = wing.gameObject;
                }
                else if (wing.name == "wing04")
                {
                    smallShip.wing04 = wing.gameObject;
                }
                else if (wing.name == "wing01_open")
                {
                    smallShip.wing01_open = wing.gameObject;
                }
                else if (wing.name == "wing02_open")
                {
                    smallShip.wing02_open = wing.gameObject;
                }
                else if (wing.name == "wing03_open")
                {
                    smallShip.wing03_open = wing.gameObject;
                }
                else if (wing.name == "wing04_open")
                {
                    smallShip.wing04_open = wing.gameObject;
                }
                else if (wing.name == "wing01_closed")
                {
                    smallShip.wing01_closed = wing.gameObject;
                }
                else if (wing.name == "wing02_closed")
                {
                    smallShip.wing02_closed = wing.gameObject;
                }
                else if (wing.name == "wing03_closed")
                {
                    smallShip.wing03_closed = wing.gameObject;
                }
                else if (wing.name == "wing04_closed")
                {
                    smallShip.wing04_closed = wing.gameObject;
                }
            }
        }
    }

    #endregion

    #region smallship task manager

    //This adds a task to the pool
    public static void AddTaskToPool(SmallShip smallShip, Task task)
    {
        if (smallShip.tasks == null)
        {
            smallShip.tasks = new List<Task>();
        }

        smallShip.tasks.Add(task);
    }

    //This ends all task in the ppol
    public static void EndAllTasks(SmallShip smallShip)
    {
        if (smallShip.tasks != null)
        {
            foreach (Task task in smallShip.tasks)
            {
                if (task != null)
                {
                    task.Stop();
                }
            }
        }
    }

    #endregion

}
