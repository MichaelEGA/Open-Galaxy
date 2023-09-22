using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

            GameObjectUtils.AddColliders(smallShip.gameObject, true);
            GameObjectUtils.AddRigidbody(smallShip.gameObject, 100f, 9f, 7.5f);
            smallShip.LODs = GameObjectUtils.GetLODs(smallShip.gameObject);
            LaserFunctions.GetCannons(smallShip);
            LaserFunctions.LoadLaserParticleSystem(smallShip);
            TorpedoFunctions.GetTorpedoTubes(smallShip);
            SmallShipAIFunctions.SetAISkillLevel(smallShip);
            smallShip.colliders = smallShip.GetComponentsInChildren<Collider>();
            LoadThrusters(smallShip);
            TargetingFunctions.CreateWaypoint(smallShip);
            smallShip.loaded = true;
        }
                
    }

    //This attaches a particle system to the ship engines to simulate thrust
    public static void LoadThrusters(SmallShip smallShip)
    {
        Transform thruster1 = smallShip.gameObject.transform.Find("thrusters/thruster");
        Transform thruster2 = smallShip.gameObject.transform.Find("thrusters/thruster.001");
        Transform thruster3 = smallShip.gameObject.transform.Find("thrusters/thruster.002");
        Transform thruster4 = smallShip.gameObject.transform.Find("thrusters/thruster.003");

        if (thruster1 != null)
        {
            AttachParticleThruster(smallShip, thruster1);
        }

        if (thruster2 != null)
        {
            AttachParticleThruster(smallShip, thruster2);
        }

        if (thruster3 != null)
        {
            AttachParticleThruster(smallShip, thruster3);
        }

        if (thruster4 != null)
        {
            AttachParticleThruster(smallShip, thruster4);
        }
    }

    //This attaches the particle trail to the torpedo
    public static void AttachParticleThruster(SmallShip smallShip, Transform thruster)
    {
        if (smallShip.scene == null)
        {
            smallShip.scene = SceneFunctions.GetScene();
        }

        if (smallShip.scene != null)
        {
            Object thrusterObject = PoolUtils.FindPrefabObjectInPool(smallShip.scene.particlePrefabPool, smallShip.thrustType);

            if (thrusterObject != null)
            {
                GameObject trail = GameObject.Instantiate(thrusterObject) as GameObject;
                trail.transform.position = thruster.position;
                trail.transform.SetParent(smallShip.gameObject.transform);
            }
        }
    }

    #endregion

    #region ship inputs

    //This gets the input from the game controller
    public static void GetGameControllerInput(SmallShip smallShip)
    {
        if (smallShip.isAI == false & smallShip.automaticRotationTurnAround == false & smallShip.automaticRotationSpin == false)
        {
            if (smallShip.keyboadAndMouse == false)
            {
                var gamepad = Gamepad.current;

                //Pitch input and smoothing (makes the using the sticks feel less jerky)
                if (gamepad.rightStick.y.ReadValue() > 0.1f & smallShip.controllerPitch < 1)
                {
                    smallShip.controllerPitch += smallShip.controllerSenstivity;
                }
                else if (gamepad.rightStick.y.ReadValue() < -0.1f & smallShip.controllerPitch > -1)
                {
                    smallShip.controllerPitch -= smallShip.controllerSenstivity;
                }
                else if (gamepad.rightStick.y.ReadValue() > -0.1f & gamepad.rightStick.y.ReadValue() < 0.1f)
                {
                    smallShip.controllerPitch = 0; //Controller dead zone
                }

                //Roll input and smoothing
                if (-gamepad.leftStick.x.ReadValue() > 0.1f & smallShip.controllerRoll < 1)
                {
                    smallShip.controllerRoll += smallShip.controllerSenstivity;
                }
                else if (-gamepad.leftStick.x.ReadValue() < -0.1f & smallShip.controllerRoll > -1)
                {
                    smallShip.controllerRoll -= smallShip.controllerSenstivity;
                }
                else if (-gamepad.leftStick.x.ReadValue() > -0.1f & -gamepad.leftStick.x.ReadValue() < 0.1f)
                {
                    smallShip.controllerRoll = 0;
                }

                //Turn input and smoothing
                if (gamepad.rightStick.x.ReadValue() > 0.1f & smallShip.controllerTurn < 1)
                {
                    smallShip.controllerTurn += smallShip.controllerSenstivity;
                }
                else if (gamepad.rightStick.x.ReadValue() < -0.1f & smallShip.controllerTurn > -1)
                {
                    smallShip.controllerTurn -= smallShip.controllerSenstivity;
                }
                else if (gamepad.rightStick.x.ReadValue() > -0.1f & gamepad.rightStick.x.ReadValue() < 0.1f)
                {
                    smallShip.controllerTurn = 0;
                }

                //Thrust input and smoothing
                if (gamepad.leftStick.y.ReadValue() > 0.1f & smallShip.controllerThrust < 1)
                {
                    smallShip.controllerThrust += smallShip.controllerSenstivity;
                }
                else if (gamepad.leftStick.y.ReadValue() < -0.1f & smallShip.controllerThrust > -1)
                {
                    smallShip.controllerThrust -= smallShip.controllerSenstivity;
                }
                else if (gamepad.leftStick.y.ReadValue() > -0.1f & gamepad.leftStick.y.ReadValue() < 0.1f)
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
                smallShip.getNextEnemy = gamepad.rightShoulder.isPressed;
                smallShip.getClosestEnemy = gamepad.yButton.isPressed;
                smallShip.fireWeapon = gamepad.rightTrigger.isPressed;
                smallShip.toggleWeapons = gamepad.bButton.isPressed;
                smallShip.toggleWeaponNumber = gamepad.aButton.isPressed;
                smallShip.lookRight = gamepad.rightStickButton.isPressed;
                smallShip.lookLeft = gamepad.leftStickButton.isPressed;
                smallShip.matchSpeed = gamepad.leftTrigger.isPressed;
            }
        }
    }

    //This gets the input from the keyboard and mouse
    public static void GetKeyboardAndMouseInput(SmallShip smallShip)
    {
        if (smallShip.isAI == false & smallShip.automaticRotationTurnAround == false & smallShip.automaticRotationSpin == false)
        {
            if (smallShip.keyboadAndMouse == true)
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
                smallShip.fireWeapon = mouse.leftButton.isPressed;
                smallShip.toggleWeapons = keyboard.tabKey.isPressed;
                smallShip.toggleWeaponNumber = keyboard.capsLockKey.isPressed;
                smallShip.lookRight = keyboard.eKey.isPressed;
                smallShip.lookLeft = keyboard.qKey.isPressed;
                smallShip.matchSpeed = mouse.rightButton.isPressed;
            }
        }
    }

    //This swaps the input depending on what the player is using
    public static void DetectInputType(SmallShip smallShip)
    {
        if (smallShip.isAI == false)
        {
            bool swap = false;

            if (smallShip.keyboadAndMouse == true)
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
                smallShip.keyboadAndMouse = !smallShip.keyboadAndMouse;
            }
        }
    }

    //This gets the AI input
    public static void GetAIInput(SmallShip smallShip)
    {
        if (smallShip.isAI == true & smallShip.automaticRotationTurnAround == false & smallShip.automaticRotationSpin == false)
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

            if (smallShip.rollInput > 0)
            {
                SmoothRollInput(smallShip, 1);
            }
            else if (smallShip.rollInput <= 0)
            {
                SmoothRollInput(smallShip, -1);
            }
        }
        else
        {
            smallShip.automaticRotationSpin = false;
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
                    AudioFunctions.PlayAudioClip(smallShip.audioManager, "beep01_toggle", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);
                }

            }
            else if (smallShip.powerToEngine == true)
            {
                smallShip.powerMode = "engines";

                if (smallShip.isAI == false)
                {
                    AudioFunctions.PlayAudioClip(smallShip.audioManager, "beep01_toggle", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);
                }

            }
            else if (smallShip.powerToShields == true)
            {
                smallShip.powerMode = "shields";

                if (smallShip.isAI == false)
                {
                    AudioFunctions.PlayAudioClip(smallShip.audioManager, "beep01_toggle", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);
                }

            }
            else if (smallShip.resetPowerLevels == true)
            {
                smallShip.powerMode = "reset";

                if (smallShip.isAI == false)
                {
                    AudioFunctions.PlayAudioClip(smallShip.audioManager, "beep01_toggle", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);
                }

            }

            smallShip.powerPressedTime = Time.time + 0.2f;

        }
        

        //This sets the ships power according the mode
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
        if (smallShip.powerMode == "reset")
        {
            actualSpeedRating = (smallShip.speedRating / 100f) * 80f;
        }
        else if (smallShip.powerMode == "lasers" || smallShip.powerMode == "shields")
        {
            actualSpeedRating = (smallShip.speedRating / 100f) * 75f;
        }

        //This calculates the WEP "War Emergency Power" i.e. WWII word for boost
        if (smallShip.powerMode == "engines" & smallShip.thrustInput > 0 & smallShip.wepLevel > 1)
        {
            smallShip.wep = true;
            actualSpeedRating = smallShip.speedRating + smallShip.wepRating;
            acclerationAmount = acclerationAmount * 2;
        }
        else
        {
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
            smallShip.rollSpeed = ((160f / 100f) * 100) * 2;
        }

    }

    //This makes the ship move
    public static void MoveShip(SmallShip smallShip)
    {
        if (smallShip.shipRigidbody == null)
        {
            smallShip.shipRigidbody = smallShip.gameObject.GetComponent<Rigidbody>();
        }
       
        if (smallShip.shipRigidbody != null)
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
                        movementEffect.transform.position = smallShip.gameObject.transform.position + new Vector3(0, 0, 5);
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

    #region level of detail control

    //This function checks what LOD should be displayed
    public static void LODCheck(SmallShip smallShip)
    {
        if (smallShip.isAI == true)
        {
            if (smallShip.scene == null)
            {
                smallShip.scene = SceneFunctions.GetScene();
            }

            if (smallShip.scene != null)
            {
                if (smallShip.scene.mainShip != null)
                {
                    smallShip.distanceToPlayer = Vector3.Distance(smallShip.gameObject.transform.position, smallShip.scene.mainShip.transform.position);

                    if (Mathf.Abs(smallShip.savedPlayerDistance) - Mathf.Abs(smallShip.distanceToPlayer) > 500)
                    {
                        float distance = 500;
                        int i = 0;

                        foreach (GameObject lod in smallShip.LODs)
                        {
                            if (smallShip.distanceToPlayer < distance)
                            {
                                if (lod.name == "detail" + i.ToString())
                                {
                                    smallShip.currentLOD = lod;
                                }
                                else
                                {
                                    lod.SetActive(false);
                                }

                                distance += 500;
                                i++;
                            }
                        }

                        smallShip.currentLOD.SetActive(true);

                        smallShip.savedPlayerDistance = smallShip.distanceToPlayer;
                    }
                }
            }
        }        
    }

    #endregion

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

    #region weapons

    public static void ToggleWeapons(SmallShip smallShip)
    {
        if (smallShip.toggleWeapons == true & smallShip.toggleWeaponPressedTime < Time.time & smallShip.torpedoNumber > 0)
        {

            if (smallShip.hasTorpedos == true)
            {
                if (smallShip.activeWeapon == "")
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
                    AudioFunctions.PlayAudioClip(smallShip.audioManager, "beep03_weaponchange", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);
                }

                smallShip.toggleWeapons = false;

            }
            else
            {
                smallShip.activeWeapon = "lasers";
            }

            

            smallShip.toggleWeaponPressedTime = Time.time + 0.25f;
        }
        else if (smallShip.torpedoNumber <= 0)
        {
            //This switches the weapons back to lasers when there are no more torpedos
            if (smallShip.activeWeapon == "torpedos")
            {
                smallShip.weaponMode = "single";
            }

            smallShip.activeWeapon = "lasers";
        }
    }

    #endregion weapons

    #region damage

    //This causes the ship to take damage from lasers and torpedoes
    public static void TakeDamage(SmallShip smallShip, float damage, Vector3 hitPosition)
    {
        if (Time.time - smallShip.loadTime > 10)
        {
            Vector3 relativePosition = smallShip.gameObject.transform.position - hitPosition;
            float forward = -Vector3.Dot(smallShip.gameObject.transform.position, relativePosition.normalized);

            if (smallShip.hullLevel > 0)
            {
                if (forward > 0)
                {
                    if (smallShip.frontShieldLevel > 0)
                    {
                        smallShip.frontShieldLevel = smallShip.frontShieldLevel - damage;
                        smallShip.shieldLevel = smallShip.shieldLevel - damage;
                    }
                    else
                    {
                        if (smallShip.hullLevel - damage < 5 & smallShip.invincible == true)
                        {
                            smallShip.hullLevel = 5;
                        }
                        else
                        {
                            smallShip.hullLevel = smallShip.hullLevel - damage;
                        }
                    }
                }
                else
                {
                    if (smallShip.rearShieldLevel > 0)
                    {
                        smallShip.rearShieldLevel = smallShip.rearShieldLevel - damage;
                        smallShip.shieldLevel = smallShip.shieldLevel - damage;
                    }
                    else
                    {
                        if (smallShip.hullLevel - damage < 5 & smallShip.invincible == true)
                        {
                            smallShip.hullLevel = 5;
                        }
                        else
                        {
                            smallShip.hullLevel = smallShip.hullLevel - damage;
                        }
                    }
                }

                if (smallShip.frontShieldLevel < 0) { smallShip.frontShieldLevel = 0; }
                if (smallShip.rearShieldLevel < 0) { smallShip.rearShieldLevel = 0; }
                if (smallShip.shieldLevel < 0) { smallShip.shieldLevel = 0; }

                //This shakes the camera
                if (smallShip.isAI == false)
                {
                    Task a = new Task(SmallShipFunctions.ActivateCockpitShake(smallShip, 0.5f));
                }
            }
        }
    }

    //This tells the damage system that a collision has begun
    public static void StartCollision(SmallShip smallShip, GameObject collidingWith)
    {
        smallShip.isCurrentlyColliding = true;

        if (smallShip.isAI == false)
        {
            AudioFunctions.PlayAudioClip(smallShip.audioManager, "crash", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);
        }        
    }

    //This tells the damage system that a collision has ended
    public static void EndCollision(SmallShip smallShip)
    {
        smallShip.isCurrentlyColliding = false;
    }

    //This called when the ship collides with something causing it to take collision damage
    public static void TakeCollisionDamage(SmallShip smallShip)
    {
        if (smallShip.isCurrentlyColliding == true & smallShip.invincible == false)
        {
            if (Time.time - smallShip.loadTime > 10)
            {

                if (smallShip.hullLevel > 0 & smallShip.invincible == false)
                {

                    if (smallShip.invincible == true & smallShip.hullLevel - 5 < 5)
                    {
                        smallShip.hullLevel = 5;
                    }
                    else
                    {
                        smallShip.hullLevel -= 5;
                    }

                    if (smallShip.hullLevel < 0)
                    {
                        smallShip.hullLevel = 0;
                    }

                    Task a = new Task(ActivateCockpitShake(smallShip, 0.5f));

                }
            }
        }    
    }

    //This causes a smoke trail to appear behind the damaged ship
    public static void SmokeTrail(SmallShip smallShip)
    {
        if (smallShip.hullLevel < 10 & smallShip.smokeTrail == null & smallShip.scene != null)
        {
            Object tempSmokeTrail = PoolUtils.FindPrefabObjectInPool(smallShip.scene.particlePrefabPool, "SmokeTrail");

            if (tempSmokeTrail != null)
            {
                GameObject smokeTrail = GameObject.Instantiate(tempSmokeTrail) as GameObject;
                smallShip.smokeTrail = smokeTrail;
                smokeTrail.transform.SetParent(smallShip.transform);
                smokeTrail.transform.localPosition = new Vector3(0, 0, 0);
                smokeTrail.layer = smallShip.gameObject.layer;
                smokeTrail.SetActive(true);
            }           
        }
        else if (smallShip.hullLevel < 10 & smallShip.smokeTrail != null)
        {
            smallShip.smokeTrail.SetActive(true);
        }
        else if (smallShip.hullLevel > 10 & smallShip.smokeTrail != null)
        {
            smallShip.smokeTrail.SetActive(false);
        }
    }

    //This causes the ship to explode
    public static void Explode(SmallShip smallShip)
    {
        if (smallShip.hullLevel <= 0 & smallShip.exploded == false || smallShip.hullLevel <= 0 & smallShip.exploded == false & smallShip.isCurrentlyColliding == true)
        {
            Task a = new Task(ExplosionSequence(smallShip));
            smallShip.exploded = true;
        }
    }

    public static IEnumerator ExplosionSequence(SmallShip smallShip)
    {
        //Stops listing the ship as targetting another ship
        if (smallShip.target != null)
        {
            if (smallShip.target.gameObject.activeSelf == true)
            {
                if (smallShip.targetSmallShip != null)
                {
                    smallShip.targetSmallShip.numberTargeting -= 1;
                }
            }
        }

        if (smallShip.isCurrentlyColliding == false)
        {
            smallShip.spinShip = true;
            float time = Random.Range(2, 6);
            yield return new WaitForSeconds(time);
        }


        if (smallShip != null)
        {
            //This removes the main camera
            RemoveMainCamera(smallShip);

            if (smallShip.scene == null)
            {
                smallShip.scene = SceneFunctions.GetScene();
            }

            //This creates an explosion where the ship is
            ParticleFunctions.InstantiateExplosion(smallShip.scene.gameObject, smallShip.gameObject.transform.position, "explosion02", 12);

            //This makes an explosion sound
            AudioFunctions.PlayAudioClip(smallShip.audioManager, "mid_explosion_01", smallShip.gameObject.transform.position, 1, 1, 1000, 1, 100);

            //This turns of the engine sound and release the ship audio source from the ship
            if (smallShip.audioManager != null)
            {
                smallShip.engineAudioSource.Stop();
                smallShip.engineAudioSource = null;
                smallShip.audioManager = null;
            }

            HudFunctions.AddToShipLog(smallShip.name.ToUpper() + " was destroyed");

            //This deactives the cockpit
            if (smallShip.cockpit != null)
            {
                smallShip.cockpit.SetActive(false);
            }

            //This resets the ship for the next load if needed
            smallShip.exploded = false;

            //This deactivates the ship
            smallShip.gameObject.SetActive(false);
        } 
    }

    public static IEnumerator ShipSpinSequence(SmallShip smallShip, float time)
    {
        smallShip.spinShip = true;

        if (smallShip.isCurrentlyColliding == false)
        {
            yield return new WaitForSeconds(time);
        }

        smallShip.spinShip = false;
    }

    #endregion

    #region cockpit

    //This activates the ships cockpit if it's a player ship
    public static void ActivateCockpit(SmallShip smallShip)
    {
        if (smallShip.isAI == false & smallShip.scene != null & smallShip.cockpit == null)
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
            if (smallShip.scene.cockpitPool != null)
            {
                foreach (GameObject cockpit in smallShip.scene.cockpitPool)
                {
                    if (cockpit.name.Contains(smallShip.cockpitName))
                    {
                        cockpit.SetActive(true);
                        smallShip.cockpit = cockpit;

                        if (smallShip.cockpitAnchor != null)
                        {
                            smallShip.cockpit.transform.SetParent(smallShip.cockpitAnchor.transform);
                        }
                    }
                    else
                    {
                        cockpit.SetActive(false);
                    }
                }
            }
        }
    }

    //This runs HudSpeedShake and HudShake
    public static void RunCockpitShake(SmallShip smallShip)
    {
        if (smallShip.isAI == false & Time.timeScale != 0)
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
                    AudioFunctions.PlayCockpitShakeNoise(smallShip);
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
        smallShip.cockpitShake = true;

        yield return new WaitForSeconds(time);

        smallShip.cockpitShake = false;
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
                float x = Random.Range(-1f, 1f) * smallShip.speedShakeMagnitude;
                float y = Random.Range(-1f, 1f) * smallShip.speedShakeMagnitude;

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

            smallShip.cockpit.transform.localPosition = smallShip.basePosition;
        }

       smallShip.cockpitSpeedShake = false;
    }

    //This shakes the hud glass
    public static IEnumerator CockpitDamageShake(SmallShip smallShip, float time, float magnitude)
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

    //This dynamically adjusts the position of the cockpit camera to simulate the movement of the pilots head and body
    public static void CockpitCameraMovement(SmallShip smallShip)
    {
        if (smallShip.isAI == false & Time.timeScale != 0)
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
                float yLocation = 0 * gForceMagnitude;
                float zLocation = 0 - 0.0005f * smallShip.thrustSpeed;

                float xRotation = 0.0005f * smallShip.pitchInput * 100 * 0.5f;
                float yRotation = 0; //float yRotation = 0.01f * headTurnInput * 100;
                float zRotation = 0.0005f * smallShip.rollInput * 100;

                float step = 0.45f * Time.time;

                //baseLocation = new Vector3(0, 0.235f, 0);
                Vector3 dynamicLocation = new Vector3(xLocation, yLocation, zLocation);
                Quaternion baseRotation = new Quaternion(0, 0, 0, 1);
                Quaternion dynamicRotation = new Quaternion(xRotation, yRotation, zRotation, 1);

                //This causes the Camera to respond to the starfighters movements
                smallShip.cockpitCamera.transform.localPosition = Vector3.MoveTowards(new Vector3(0, 0, 0), dynamicLocation, step);
                smallShip.cockpitCamera.transform.localRotation = Quaternion.RotateTowards(baseRotation, dynamicRotation, step);
            }
        }        
    }

    //Cockpit anchor rotation
    public static void CockpitAnchorRotation(SmallShip smallShip)
    {
        if (smallShip.isAI == false)
        {
            if (smallShip.cockpitAnchor != null)
            {
                smallShip.cockpitAnchor.transform.rotation = smallShip.transform.rotation;
            }
        }
    }

    #endregion
}
