using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

//These functions are called by the smallship script
public static class FlightControlSystem_SmallFunctions
{
    #region start functions

    //This prepares the ship by loading lod and colliders (if not already loaded)
    public static void PrepareShip(FlightControlSystem_Small FlightControlSystem_Small)
    {
        if (FlightControlSystem_Small.loaded == false)
        {
            if (FlightControlSystem_Small.ship.isAI == false)
            {
                SceneFunctions.IdentifyAsMainShip(FlightControlSystem_Small);
            }

            //This gets the camera position which is used for targetting as well as placing the cockpit
            if (FlightControlSystem_Small.cameraPosition == null)
            {
                Transform cameraPos = GameObjectUtils.FindChildTransformCalled(FlightControlSystem_Small.gameObject.transform, "camera");

                if (cameraPos != null)
                {
                    FlightControlSystem_Small.cameraPosition = cameraPos.gameObject;
                }
                else
                {
                    FlightControlSystem_Small.cameraPosition = FlightControlSystem_Small.gameObject;
                }
            }

            //This gets the camera position which is used for targetting as well as placing the cockpit
            if (FlightControlSystem_Small.followCameraPosition == null)
            {
                Transform followCameraPos = GameObjectUtils.FindChildTransformCalled(FlightControlSystem_Small.gameObject.transform, "followcamera");

                if (followCameraPos != null)
                {
                    FlightControlSystem_Small.followCameraPosition = followCameraPos.gameObject;
                }
            }

            if (FlightControlSystem_Small.focusCameraPosition == null)
            {
                Transform focusCameraPos = GameObjectUtils.FindChildTransformCalled(FlightControlSystem_Small.gameObject.transform, "focuscamera");

                if (focusCameraPos != null)
                {
                    FlightControlSystem_Small.focusCameraPosition = focusCameraPos.gameObject;
                }
            }

            GameObjectUtils.AddMeshColliders(FlightControlSystem_Small.gameObject, true);

            if (FlightControlSystem_Small.isAI == false)
            {
                GameObjectUtils.AddRigidbody(FlightControlSystem_Small.gameObject, 100f, 9f, 7.5f);
            }
            else
            {
                GameObjectUtils.AddRigidbody(FlightControlSystem_Small.gameObject, 100f, 9f, 7.5f);
            }

            TorpedoSystemFunctions.GetTorpedoTubes(FlightControlSystem_Small);
            FlightControlSystem_Small.colliders = FlightControlSystem_Small.GetComponentsInChildren<Collider>();
            TargetingSystemFunctions.CreateWaypoint_SmallShipPlayer(FlightControlSystem_Small);
            DockingFunctions.AddDockingPointsSmallShip(FlightControlSystem_Small.ship);
            FlightControlAI_SmallFunctions.SetTargetingErrorMargin(FlightControlSystem_Small, "low");
            FlightControlSystem_Small.ship.ogInput = OGInputFunctions.GetOGInput();

            FlightControlSystem_Small.loaded = true;
        }
    }

    #endregion

    #region ship inputs

    //This gets the input to control the ship from either OGInput script or the AI Controller
    public static void GetInput(FlightControlSystem_Small smallShip)
    {
        if (smallShip.isAI == false)
        {
            //This gets the OGInput function
            if (smallShip.ship.ogInput == null)
            {
                smallShip.ship.ogInput = OGInputFunctions.GetOGInput();
            }

            smallShip.rollInput = smallShip.ship.ogInput.rollInput;
            smallShip.thrustInput = smallShip.ship.ogInput.thrustInput;
            smallShip.pitchInput = smallShip.ship.ogInput.pitchInput;
            smallShip.turnInput = smallShip.ship.ogInput.turnInput;
            smallShip.powerToShields = smallShip.ship.ogInput.powerToShields;
            smallShip.powerToEngine = smallShip.ship.ogInput.powerToEngine;
            smallShip.powerToLasers = smallShip.ship.ogInput.powerToLasers;
            smallShip.resetPowerLevels = smallShip.ship.ogInput.resetPowerLevels;
            smallShip.getNextTarget = smallShip.ship.ogInput.getNextTarget;
            smallShip.getNextEnemy = smallShip.ship.ogInput.getNextEnemy;
            smallShip.getClosestEnemy = smallShip.ship.ogInput.getClosestEnemy;
            smallShip.selectTargetInFront = smallShip.ship.ogInput.selectTargetInFront;
            smallShip.fireWeapon = smallShip.ship.ogInput.fireWeapon;
            smallShip.rapidFire = smallShip.ship.ogInput.rapidFire;
            smallShip.weaponManagement.toggleWeapons = smallShip.ship.ogInput.toggleWeapons;
            smallShip.weaponManagement.toggleWeaponNumber = smallShip.ship.ogInput.toggleWeaponNumber;
            smallShip.matchSpeed = smallShip.ship.ogInput.matchSpeed;
            smallShip.focusCamera = smallShip.ship.ogInput.focusCamera;
            smallShip.fireCounterMeasures = smallShip.ship.ogInput.fireCounterMeasures;
        }
    }

    //This causes the ship to match the speed of it's target (not used by AI)
    public static void MatchSpeed(FlightControlSystem_Small smallShip)
    {
        if (smallShip.targetingSystem.target != null & smallShip.matchSpeed == true)
        {
            if (smallShip.targetingSystem.target.activeSelf != false)
            {
                if (smallShip.thrustSpeed > smallShip.targetingSystem.targetSpeed)
                {
                    smallShip.thrustInput = -1;
                }
                else if (smallShip.thrustSpeed < smallShip.targetingSystem.targetSpeed)
                {
                    smallShip.thrustInput = 1;
                }
            }
        }
    }

    //This autmomatically turns the ship around when it reaches the boundaries of the game area, i.e. 15000m
    public static void TurnShipAround(FlightControlSystem_Small smallShip)
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
                    smallShip.turnInput = right;
                    smallShip.pitchInput = -up;
                }
                else
                {
                    smallShip.turnInput = right * 5;
                    smallShip.pitchInput = -up * 5;
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
    public static void SpinShip(FlightControlSystem_Small smallShip)
    {
        if (smallShip.spinShip == true)
        {
            smallShip.automaticRotationSpin = true;

            smallShip.turnInput = 0;
            smallShip.pitchInput = 0;


            if (smallShip.rollInputActual > 0)
            {
                smallShip.rollInput = 1;
            }
            else if (smallShip.rollInputActual < 0)
            {
                smallShip.rollInput = -1;
            }
            else
            {
                smallShip.rollInput = 1;
            }
        }
        else
        {
            smallShip.automaticRotationSpin = false;
        }
    }

    //When activated this prevents the ship from turning from its present course
    public static void ControlLock(FlightControlSystem_Small smallShip)
    {
        if (smallShip.controlLock == true)
        {
            smallShip.turnInput = 0;
            smallShip.pitchInput = 0;
            smallShip.rollInput = 0;
        }
    }

    #endregion

    #region ship movement

    //This calculates the thrust speed of the ship
    public static void CalculateThrustSpeed(FlightControlSystem_Small smallShip)
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

            if (smallShip.ship.ogInput == null)
            {
                smallShip.ship.ogInput = OGInputFunctions.GetOGInput();
            }

            if (smallShip.ship.ogInput.keyboardAndMouse == false)
            {
                OGInputFunctions.StartShakeController(0.20f, 0.20f);
            }
            
        }
        else if (smallShip.powerMode == "lasers" || smallShip.powerMode == "shields")
        {
            actualSpeedRating = (smallShip.speedRating / 100f) * 75f;

            if (smallShip.wep == true)
            {
                OGInputFunctions.StopShakeController();
            }
            
            smallShip.wep = false;
        }
        else
        {
            if (smallShip.wep == true)
            {
                OGInputFunctions.StopShakeController();
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
    public static void CalculatePitchTurnRollSpeeds(FlightControlSystem_Small smallShip)
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

        smallShip.pitchSpeed = (140f / 100f) * (currentManeuverablity * manveurablityPercentageAsDecimal);
        smallShip.turnSpeed = (100f / 100f) * (currentManeuverablity * manveurablityPercentageAsDecimal);
        smallShip.rollSpeed = (220f / 100f) * (currentManeuverablity * manveurablityPercentageAsDecimal); //160

        if (smallShip.spinShip == true)
        {
            smallShip.rollSpeed = ((160f / 100f) * 100) * 2.5f * manveurablityPercentageAsDecimal; //10 was 2.5f before
        }

    }

    //This makes the ship move
    public static void MoveShip(FlightControlSystem_Small smallShip)
    {
        if (smallShip.shipRigidbody == null)
        {
            smallShip.shipRigidbody = smallShip.gameObject.GetComponent<Rigidbody>();
        }

        if (smallShip.shipRigidbody != null & smallShip.jumpingToHyperspace == false & smallShip.exitingHyperspace == false & smallShip.docking == false & smallShip.isDisabled == false)
        {
            //This adds makes the ship move forward
            smallShip.shipRigidbody.AddForce(smallShip.gameObject.transform.position + smallShip.gameObject.transform.forward * Time.fixedDeltaTime * smallShip.thrustSpeed * 60000);

            //This rotates the ship
            Vector3 x = Vector3.right * smallShip.pitchSpeed * smallShip.pitchInput;
            Vector3 y = Vector3.up * smallShip.turnSpeed * smallShip.turnInput;
            Vector3 z = Vector3.forward * smallShip.rollSpeed * smallShip.rollInput;

            Vector3 rotationVector = x + y + z;

            Quaternion deltaRotation = Quaternion.Euler(rotationVector * Time.deltaTime);
            smallShip.shipRigidbody.MoveRotation(smallShip.shipRigidbody.rotation * deltaRotation);
        }
    }

    //Jump to Hyperspace
    public static IEnumerator JumpToHyperspace(FlightControlSystem_Small smallShip)
    {
        if (smallShip != null)
        {
            CloseWings(smallShip);

            yield return new WaitForSeconds(3.4f); //This gives the wings time to close
        }

        if (smallShip != null)
        {
            smallShip.jumpingToHyperspace = true;

            Vector3 startPosition = smallShip.gameObject.transform.localPosition;
            Vector3 endPosition = smallShip.transform.localPosition + smallShip.gameObject.transform.forward * 5000;

            AudioFunctions.PlayAudioClip(smallShip.audioManager, "hyperspace03_exit", "Explosions", smallShip.transform.position, 1, 1, 1000, 1f);

            float timeElapsed = 0;
            float lerpDuration = 1;

            while (timeElapsed < lerpDuration)
            {
                if (smallShip != null)
                {
                    smallShip.gameObject.transform.localPosition = Vector3.Lerp(startPosition, endPosition, timeElapsed / lerpDuration);
                    timeElapsed += Time.deltaTime;
                    yield return new WaitForFixedUpdate();
                }
            }

            if (smallShip != null)
            {
                HudFunctions.AddToShipLog(smallShip.name.ToUpper() + " jumped to hyperspace");

                smallShip.jumpingToHyperspace = false;

                DamageSystemFunctions.DeactivateShip_SmallShip(smallShip);
            }
        }
    }

    //Exit Hyperspace
    public static IEnumerator ExitHyperspace(FlightControlSystem_Small smallShip)
    {
        SnapClosedWings(smallShip); //Keeps wings shut on hyperspace exit

        smallShip.exitingHyperspace = true;

        Vector3 endPosition = smallShip.transform.localPosition + smallShip.gameObject.transform.forward * 5000; 
        Vector3 startPosition = smallShip.gameObject.transform.localPosition;

        float timeElapsed = 0;
        float lerpDuration = 1;

        while (timeElapsed < lerpDuration)
        {
            if (smallShip != null)
            {
                smallShip.gameObject.transform.localPosition = Vector3.Lerp(startPosition, endPosition, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
            else
            {
                break;
            }
        }

        smallShip.gameObject.transform.localPosition = endPosition;

        AudioFunctions.PlayAudioClip(smallShip.audioManager, "hyperspace03_exit", "Explosions", smallShip.transform.position, 1, 1, 1000, 1f);

        HudFunctions.AddToShipLog(smallShip.name.ToUpper() + " just exited hyperspace");

        OpenWings(smallShip); //Opens wings after hyperspace exit

        smallShip.exitingHyperspace = false;
    }

    //A particle effect that makes the ship look like it's moving
    public static void MovementEffect(FlightControlSystem_Small smallShip)
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
                main.simulationSpeed = particleSpeed * 2;
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
}
