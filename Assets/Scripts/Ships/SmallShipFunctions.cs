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
            TorpedoFunctions.GetTorpedoTubes(smallShip);
            smallShip.colliders = smallShip.GetComponentsInChildren<Collider>();
            LoadThrusters(smallShip);
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
        if (smallShip.isAI == false & smallShip.automaticRotationTurnAround == false & smallShip.automaticRotationSpin == false & smallShip.controlLock == false)
        {
            if (smallShip.keyboadAndMouse == false)
            {
                var gamepad = Gamepad.current;

                smallShip.controllerPitch = Mathf.MoveTowards(gamepad.rightStick.y.ReadValue(), smallShip.controllerPitch, smallShip.controllerSenstivity * Time.deltaTime);
                smallShip.controllerRoll = Mathf.MoveTowards(-gamepad.leftStick.x.ReadValue(), smallShip.controllerRoll, smallShip.controllerSenstivity * Time.deltaTime);
                smallShip.controllerTurn = Mathf.MoveTowards(gamepad.rightStick.x.ReadValue(), smallShip.controllerTurn, smallShip.controllerSenstivity * Time.deltaTime);

                //Thrust input and smoothing
                if (gamepad.leftStick.y.ReadValue() > 0.1f & smallShip.controllerThrust < 1)
                {
                    smallShip.controllerThrust = Mathf.MoveTowards(gamepad.leftStick.y.ReadValue(), smallShip.controllerThrust, smallShip.controllerSenstivity * Time.deltaTime);
                }
                else if (gamepad.leftStick.y.ReadValue() < -0.1f & smallShip.controllerThrust > -1)
                {
                    smallShip.controllerThrust = Mathf.MoveTowards(gamepad.leftStick.y.ReadValue(), smallShip.controllerThrust, smallShip.controllerSenstivity * Time.deltaTime);
                }
                else if (gamepad.leftStick.y.ReadValue() > -0.1f & gamepad.leftStick.y.ReadValue() < 0.1f)
                {
                    smallShip.controllerThrust = Mathf.MoveTowards(gamepad.leftStick.y.ReadValue(), 0, smallShip.controllerSenstivity * Time.deltaTime);
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
                smallShip.getNextEnemy = gamepad.yButton.isPressed; 
                smallShip.getClosestEnemy = gamepad.rightShoulder.isPressed;
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
        if (smallShip.isAI == false & smallShip.automaticRotationTurnAround == false & smallShip.automaticRotationSpin == false & smallShip.controlLock == false)
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
            else if (smallShip.rollInputActual <= 0)
            {
                SmoothRollInput(smallShip, -1);
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

        if (smallShip.shipRigidbody != null & smallShip.jumpingToHyperspace == false & smallShip.exitingHyperspace == false & smallShip.docking == false)
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

    //Jump to Hyperspace
    public static IEnumerator JumpToHyperspace(SmallShip smallShip)
    {
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

        DeactivateShip(smallShip);
    }

    //Exit Hyperspace
    public static IEnumerator ExitHyperspace(SmallShip smallShip)
    {
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

        AudioFunctions.PlayAudioClip(smallShip.audioManager, "hyperspace03_exit", "Explosions", smallShip.transform.position, 0, 1, 1000, 1f);

        HudFunctions.AddToShipLog(smallShip.name.ToUpper() + " just exited hyperspace");

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
                    AudioFunctions.PlayAudioClip(smallShip.audioManager, "beep03_weaponchange", "Cockpit", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);
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

                //This shakes the cockpit camera
                Task a = new Task(CockpitFunctions.CockpitDamageShake(smallShip, 1, 0.011f));              
            }
        }
    }

    //This tells the damage system that a collision has begun
    public static void StartCollision(SmallShip smallShip, GameObject collidingWith)
    {
        Debug.Log(smallShip.name + " colliding with " + collidingWith.name);

        if (smallShip.docking == false)
        {
            smallShip.isCurrentlyColliding = true;

            if (smallShip.isAI == false & smallShip.invincible == false)
            {
                AudioFunctions.PlayAudioClip(smallShip.audioManager, "impact03_crash", "Cockpit", smallShip.gameObject.transform.position, 0, 1, 500, 1, 100);
            }
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
        if (smallShip.isCurrentlyColliding == true & smallShip.invincible == false & smallShip.docking == false)
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

                    Task a = new Task(CockpitFunctions.ActivateCockpitShake(smallShip, 0.5f));

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
            if (smallShip.explosionType == "type1")
            {
                Task a = new Task(ExplosionType1(smallShip));
                smallShip.exploded = true;
            }
            else if (smallShip.explosionType == "type2")
            {
                ExplosionType2(smallShip);
                smallShip.exploded = true;
            }
            else
            {
                Task a = new Task(ExplosionType1(smallShip));
                smallShip.exploded = true;
            }
        }
    }

    //Explode after spinning
    public static IEnumerator ExplosionType1(SmallShip smallShip)
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
            if (smallShip.scene == null)
            {
                smallShip.scene = SceneFunctions.GetScene();
            }

            //This creates an explosion where the ship is
            ParticleFunctions.InstantiateExplosion(smallShip.scene.gameObject, smallShip.gameObject.transform.position, "explosion02", 12);

            //This makes an explosion sound
            AudioFunctions.PlayAudioClip(smallShip.audioManager, "mid_explosion_01", "External", smallShip.gameObject.transform.position, 1, 1, 1000, 1);

            //This tells the game that the ship has been destroyed
            HudFunctions.AddToShipLog(smallShip.name.ToUpper() + " was destroyed");

            //This deactivates the ship
            DeactivateShip(smallShip);
        } 
    }

    //Explode straight away
    public static void ExplosionType2(SmallShip smallShip)
    {
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

        if (smallShip.scene == null)
        {
            smallShip.scene = SceneFunctions.GetScene();
        }

        //This creates an explosion where the ship is
        ParticleFunctions.InstantiateExplosion(smallShip.scene.gameObject, smallShip.gameObject.transform.position, "explosion02", 12);

        //This makes an explosion sound
        AudioFunctions.PlayAudioClip(smallShip.audioManager, "mid_explosion_01", "External", smallShip.gameObject.transform.position, 1, 1, 1000, 1);

        //This tells the game that the ship has been destroyed
        HudFunctions.AddToShipLog(smallShip.name.ToUpper() + " was destroyed");

        //This deactivates the ship
        DeactivateShip(smallShip);      
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

    public static void DeactivateShip(SmallShip smallShip)
    {
        //This removes the main camera
        CockpitFunctions.RemoveMainCamera(smallShip);

        //This turns of the engine sound and release the ship audio source from the ship
        if (smallShip.audioManager != null)
        {
            if (smallShip.engineAudioSource != null)
            {
                smallShip.engineAudioSource.Stop();
                smallShip.engineAudioSource = null;
            }

            smallShip.audioManager = null;
        }

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

    #endregion

}
