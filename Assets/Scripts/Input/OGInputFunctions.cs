using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public static class OGInputFunctions
{
    //This updates the input
    public static void UpdateInput(OGInput ogInput)
    {
        //This gets key references if they're mission
        if (ogInput.scene == null)
        {
            ogInput.scene = SceneFunctions.GetScene();
        }

        //This gets the input
        if (ogInput.keyboardAndMouse == true)
        {
            GetKeyboardAndMouseInput(ogInput);
        }
        else
        {
            GetControllerInput(ogInput);
        }

        //This detects a change in input
        DetectInputType(ogInput);
        UpdateInputSettings(ogInput);
    }

    //This gets the keyboard input
    public static void GetKeyboardAndMouseInput(OGInput ogInput)
    {
        
        // Mouse Input function
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

        // Store target values
        ogInput.targetMouseInput.x = Mathf.Clamp(x, -1.0f, 1.0f);
        ogInput.targetMouseInput.y = Mathf.Clamp(-y, -1.0f, 1.0f); // Note: -y for pitch

        // Lerp toward the target smoothly
        ogInput.currentMouseInput = Vector2.Lerp(ogInput.currentMouseInput, ogInput.targetMouseInput, ogInput.lerpSpeed * Time.deltaTime);

        // Use the smoothed values
        ogInput.pitchInput = (ogInput.invertUpDown) ? -ogInput.currentMouseInput.y : ogInput.currentMouseInput.y;
        ogInput.turnInput = (ogInput.invertLeftRight) ? -ogInput.currentMouseInput.x : ogInput.currentMouseInput.x;

        ogInput.rollInput = -Input.GetAxis("LeftHorizontal");
        ogInput.thrustInput = Input.GetAxis("LeftVertical");


        if (ogInput.missionManager == null)
        {
            ogInput.missionManager = MissionFunctions.GetMissionManager();
        }

        //Keyboard inputs
        var keyboard = Keyboard.current;

        if (ogInput.missionManager != null)
        {
            if (ogInput.missionManager.controlsReleased == true) //This checks that the controls aren't being used by the choice node
            {
                ogInput.powerToShields = keyboard.leftArrowKey.isPressed;
                ogInput.powerToEngine = keyboard.upArrowKey.isPressed;
                ogInput.powerToLasers = keyboard.rightArrowKey.isPressed;
                ogInput.resetPowerLevels = keyboard.downArrowKey.isPressed;
            }
        }
        
        ogInput.getNextTarget = keyboard.rKey.isPressed;
        ogInput.getNextEnemy = keyboard.tKey.isPressed;
        ogInput.getClosestEnemy = keyboard.fKey.isPressed;
        ogInput.selectTargetInFront = keyboard.gKey.isPressed;
        ogInput.fireWeapon = mouse.leftButton.isPressed;
        ogInput.rapidFire = mouse.middleButton.isPressed;
        ogInput.toggleWeapons = keyboard.tabKey.isPressed;
        ogInput.toggleWeaponNumber = keyboard.capsLockKey.isPressed;
        ogInput.matchSpeed = keyboard.eKey.isPressed;
        ogInput.focusCamera = mouse.rightButton.isPressed;
        ogInput.fireCounterMeasures = keyboard.spaceKey.isPressed;
    }

    //This gets the controller input
    public static void GetControllerInput(OGInput ogInput)
    {
        // Parameters (tweak these in inspector or as constants)
        float deadzone = 0.12f;
        float inputCurveExponent = 3f;    // 3 = cubic, >1 gives finer center control
        float smoothingSpeed = 3f;        // 6 higher = snappier smoothing
        float thrustAccel = 2.5f;         // throttle change speed (units/sec)
        float instantStickThreshold = 0.95f; // full-stick snaps to 1/-1

        // Helper: applies deadzone and an odd power curve, preserving sign
        float ApplyResponseCurve(float v)
        {
            if (Mathf.Abs(v) <= deadzone) return 0f;
            // remap so we keep continuity across deadzone
            float sign = Mathf.Sign(v);
            float norm = (Mathf.Abs(v) - deadzone) / (1f - deadzone); // 0..1
            return sign * Mathf.Pow(norm, inputCurveExponent);
        }

        // Frame-rate independent smoothing factor
        float SmoothingAlpha(float speed)
        {
            // speed in units/sec. Convert to per-frame alpha: 1 - exp(-speed * dt)
            return 1f - Mathf.Exp(-speed * Time.deltaTime);
        }

        // In Update() or input method:
        var gamepad = Gamepad.current;
        if (gamepad == null) return;

        // Read raw sticks
        Vector2 left = gamepad.leftStick.ReadValue();
        Vector2 right = gamepad.rightStick.ReadValue();

        // Raw -> deadzone -> curve
        float pitchInputRaw = ApplyResponseCurve(right.y);
        float rollInputRaw = -ApplyResponseCurve(left.x); // keep your sign convention
        float turnInputRaw = ApplyResponseCurve(right.x);

        // Smooth (frame-rate independent)
        float alpha = SmoothingAlpha(smoothingSpeed);
        ogInput.smoothedPitch = Mathf.Lerp(ogInput.smoothedPitch, pitchInputRaw, alpha);
        ogInput.smoothedRoll = Mathf.Lerp(ogInput.smoothedRoll, rollInputRaw, alpha);
        ogInput.smoothedTurn = Mathf.Lerp(ogInput.smoothedTurn, turnInputRaw, alpha);

        // Map to controller outputs (sensitivity still applies)
        ogInput.controllerPitch = ogInput.smoothedPitch * ogInput.controllerSensitivity;
        ogInput.controllerRoll = ogInput.smoothedRoll * ogInput.controllerSensitivity;
        ogInput.controllerTurn = ogInput.smoothedTurn * ogInput.controllerSensitivity;

        // Throttle: use analog smoothing and instant-snap at full stick
        float rawThrottle = left.y; // -1..1
        float targetThrottle;
        if (rawThrottle >= instantStickThreshold) targetThrottle = 1f;
        else if (rawThrottle <= -instantStickThreshold) targetThrottle = -1f;
        else targetThrottle = Mathf.Abs(rawThrottle) > deadzone ? rawThrottle : 0f;

        // Smooth throttle with acceleration (frame-rate independent)
        ogInput.controllerThrust = Mathf.MoveTowards(ogInput.controllerThrust, targetThrottle, thrustAccel * Time.deltaTime);

        // Map to final ship inputs (with inversion)
        ogInput.pitchInput = ogInput.invertUpDown ? ogInput.controllerPitch : -ogInput.controllerPitch;
        ogInput.turnInput = ogInput.invertLeftRight ? -ogInput.controllerTurn : ogInput.controllerTurn;
        ogInput.thrustInput = ogInput.controllerThrust;
        ogInput.rollInput = ogInput.controllerRoll;

        // Button inputs (fix boolean operator)
        if (ogInput.missionManager == null)
            ogInput.missionManager = MissionFunctions.GetMissionManager();

        if (ogInput.missionManager != null && ogInput.missionManager.controlsReleased)
        {
            ogInput.powerToShields = gamepad.dpad.left.isPressed;
            ogInput.powerToEngine = gamepad.dpad.up.isPressed;
            ogInput.powerToLasers = gamepad.dpad.right.isPressed;
            ogInput.resetPowerLevels = gamepad.dpad.down.isPressed;
        }

        ogInput.getNextTarget = gamepad.leftShoulder.isPressed;
        ogInput.getClosestEnemy = gamepad.xButton.isPressed;
        ogInput.selectTargetInFront = gamepad.yButton.isPressed;
        ogInput.fireWeapon = gamepad.rightTrigger.isPressed;
        ogInput.rapidFire = gamepad.rightShoulder.isPressed;
        ogInput.toggleWeapons = gamepad.bButton.isPressed;
        ogInput.toggleWeaponNumber = gamepad.aButton.isPressed;
        ogInput.matchSpeed = gamepad.leftStickButton.isPressed;
        ogInput.focusCamera = gamepad.leftTrigger.isPressed;
    }

    public static float ApplyInputCurve(float input)
    {
        float deadzone = 0.15f;
        if (Mathf.Abs(input) < deadzone) return 0;

        float normalized = (input - Mathf.Sign(input) * deadzone) / (1 - deadzone);
        return Mathf.Sign(normalized) * Mathf.Pow(Mathf.Abs(normalized), 1.8f);
    } //subfunction for get controller input

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
    public static IEnumerator ShakeControllerForSetTime(float duration, float leftMotorSpeed, float rightMotorSpeed)
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
    public static void DetectInputType(OGInput ogInput)
    {
        bool swap = false;

        if (ogInput.keyboardAndMouse == true)
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
            ogInput.keyboardAndMouse = !ogInput.keyboardAndMouse;
        }       
    }

    //This returns the input script in the game
    public static OGInput GetOGInput()
    {
        OGInput ogInput = GameObject.FindAnyObjectByType<OGInput>();

        if (ogInput == null)
        {
            CreateOGInput();
        }

        return ogInput;
    }

    //This creates the OGinput gamescript object
    public static OGInput CreateOGInput()
    {
        GameObject gameObject = new GameObject();
        gameObject.name = "OGInput";
        OGInput ogInput = gameObject.AddComponent<OGInput>();
        return ogInput;
    }

    //This updates the OGInput setttings
    public static void UpdateInputSettings(OGInput ogInput)
    {
        if (ogInput != null)
        {
            if (ogInput.settings == null)
            {
                ogInput.settings = OGSettingsFunctions.GetSettings();
            }

            ogInput.controllerSensitivity = ogInput.settings.controllersensitivity;
            ogInput.invertUpDown = ogInput.settings.invertY;
            ogInput.invertLeftRight = ogInput.settings.invertX;
        }
    }

}

