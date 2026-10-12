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
        if (FlightControlSystem_Small.ship.loaded == false)
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

            if (FlightControlSystem_Small.ship.isAI == false)
            {
                GameObjectUtils.AddRigidbody(FlightControlSystem_Small.gameObject, 100f, 9f, 7.5f);
            }
            else
            {
                GameObjectUtils.AddRigidbody(FlightControlSystem_Small.gameObject, 100f, 9f, 7.5f);
            }


            FlightControlSystem_Small.colliders = FlightControlSystem_Small.GetComponentsInChildren<Collider>();

            DockingFunctions.AddDockingPointsSmallShip(FlightControlSystem_Small.ship);

            FlightControlSystem_Small.ship.loaded = true;
        }
    }

    #endregion

    #region ship inputs

    //This gets the input to control the ship from either OGInput script or the AI Controller
    public static void GetInput(FlightControlSystem_Small flightControlSystem)
    {
        if (flightControlSystem.ship.isAI == false)
        {
            //This gets the OGInput function
            if (flightControlSystem.ship.ogInput == null)
            {
                flightControlSystem.ship.ogInput = OGInputFunctions.GetOGInput();
            }

            flightControlSystem.rollInput = flightControlSystem.ship.ogInput.rollInput;
            flightControlSystem.thrustInput = flightControlSystem.ship.ogInput.thrustInput;
            flightControlSystem.pitchInput = flightControlSystem.ship.ogInput.pitchInput;
            flightControlSystem.turnInput = flightControlSystem.ship.ogInput.turnInput;
            flightControlSystem.matchSpeed = flightControlSystem.ship.ogInput.matchSpeed;
            flightControlSystem.focusCamera = flightControlSystem.ship.ogInput.focusCamera;
        }
    }

    //This causes the ship to match the speed of it's target (not used by AI)
    public static void MatchSpeed(FlightControlSystem_Small flightControlSystem)
    {
        TargetingSystem targetingSystem = flightControlSystem.ship.targetingSystem;

        if (targetingSystem.target != null & flightControlSystem.matchSpeed == true)
        {
            if (targetingSystem.target.activeSelf != false)
            {
                if (flightControlSystem.thrustSpeed > targetingSystem.targetSpeed)
                {
                    flightControlSystem.thrustInput = -1;
                }
                else if (flightControlSystem.thrustSpeed < targetingSystem.targetSpeed)
                {
                    flightControlSystem.thrustInput = 1;
                }
            }
        }
    }

    //This autmomatically turns the ship around when it reaches the boundaries of the game area, i.e. 15000m
    public static void TurnShipAround(FlightControlSystem_Small flightControlSystem)
    {
        if (flightControlSystem.ship.scene != null)
        {
            Vector3 center = flightControlSystem.ship.scene.transform.position;
            Vector3 currentPosition = flightControlSystem.gameObject.transform.position;

            float currentDistance = Vector3.Distance(currentPosition, center);

            if (currentDistance > flightControlSystem.ship.scene.sceneRadius)
            {
                flightControlSystem.automaticRotationTurnAround = true;

                Vector3 targetRelativePosition = center - currentPosition;

                float forward = Vector3.Dot(flightControlSystem.gameObject.transform.forward, targetRelativePosition.normalized);
                float right = Vector3.Dot(flightControlSystem.gameObject.transform.right, targetRelativePosition.normalized);
                float up = Vector3.Dot(flightControlSystem.gameObject.transform.up, targetRelativePosition.normalized);

                if (forward < 0.8)
                {
                    flightControlSystem.turnInput = right;
                    flightControlSystem.pitchInput = -up;
                }
                else
                {
                    flightControlSystem.turnInput = right * 5;
                    flightControlSystem.pitchInput = -up * 5;
                }

                flightControlSystem.thrustInput = 1;

                if (flightControlSystem.messageSent == false & flightControlSystem.ship.isAI == false)
                {
                    HudFunctions.AddToShipLog("WARNING: Too far out turning around");
                    flightControlSystem.messageSent = true;
                }
            }
            else
            {
                flightControlSystem.automaticRotationTurnAround = false;
                flightControlSystem.messageSent = false;
            }
        }
    }

    //This automatically spins the ship on the x-axis when its hit by a torpedo or destroyed
    public static void SpinShip(FlightControlSystem_Small flightControlSystem)
    {
        if (flightControlSystem.spinShip == true)
        {
            flightControlSystem.automaticRotationSpin = true;

            flightControlSystem.turnInput = 0;
            flightControlSystem.pitchInput = 0;


            if (flightControlSystem.rollInputActual > 0)
            {
                flightControlSystem.rollInput = 1;
            }
            else if (flightControlSystem.rollInputActual < 0)
            {
                flightControlSystem.rollInput = -1;
            }
            else
            {
                flightControlSystem.rollInput = 1;
            }
        }
        else
        {
            flightControlSystem.automaticRotationSpin = false;
        }
    }

    //When activated this prevents the ship from turning from its present course
    public static void ControlLock(FlightControlSystem_Small flightControlSystem)
    {
        if (flightControlSystem.controlLock == true)
        {
            flightControlSystem.turnInput = 0;
            flightControlSystem.pitchInput = 0;
            flightControlSystem.rollInput = 0;
        }
    }

    #endregion

    #region ship movement

    //This calculates the thrust speed of the ship
    public static void CalculateThrustSpeed(FlightControlSystem_Small flightControlSystem)
    {
        //This calculates the normal accleration and speed rating
        float acclerationAmount = (0.5f / 100f) * flightControlSystem.ship.accelerationRating;
        float actualSpeedRating = flightControlSystem.ship.speedRating;

        //This calculates the accleration and speedrating according to different power modes
        if (flightControlSystem.ship.energyManagementSystem.powerMode == "reset" & flightControlSystem.thrustInput > 0 & flightControlSystem.ship.energyManagementSystem.wepLevel > 1 || flightControlSystem.ship.energyManagementSystem.powerMode == "engines" & flightControlSystem.thrustInput > 0 & flightControlSystem.ship.energyManagementSystem.wepLevel > 1)
        {
            actualSpeedRating = flightControlSystem.ship.speedRating + flightControlSystem.ship.wepRating;
            acclerationAmount = acclerationAmount * 2;
            flightControlSystem.wep = true;

            if (flightControlSystem.ship.ogInput == null)
            {
                flightControlSystem.ship.ogInput = OGInputFunctions.GetOGInput();
            }

            if (flightControlSystem.ship.ogInput.keyboardAndMouse == false)
            {
                OGInputFunctions.StartShakeController(0.20f, 0.20f);
            }
            
        }
        else if (flightControlSystem.ship.energyManagementSystem.powerMode == "lasers" || flightControlSystem.ship.energyManagementSystem.powerMode == "shields")
        {
            actualSpeedRating = (flightControlSystem.ship.speedRating / 100f) * 75f;

            if (flightControlSystem.wep == true)
            {
                OGInputFunctions.StopShakeController();
            }
            
            flightControlSystem.wep = false;
        }
        else
        {
            if (flightControlSystem.wep == true)
            {
                OGInputFunctions.StopShakeController();
            }

            flightControlSystem.wep = false;
        }

        //This controls the throttle of the ship, and prevents it going above the speed rating or below zero
        if (flightControlSystem.thrustSpeed > actualSpeedRating)
        {
            flightControlSystem.thrustSpeed = flightControlSystem.thrustSpeed - acclerationAmount * 4;
        }
        else if (flightControlSystem.thrustInput < 0 & flightControlSystem.thrustTimeStamp < Time.time)
        {
            flightControlSystem.thrustSpeed = flightControlSystem.thrustSpeed - acclerationAmount;
            flightControlSystem.thrustTimeStamp = Time.time + 0.01f;
        }
        else if (flightControlSystem.thrustInput > 0 & flightControlSystem.thrustTimeStamp < Time.time)
        {
            flightControlSystem.thrustSpeed = flightControlSystem.thrustSpeed + acclerationAmount;
            flightControlSystem.thrustTimeStamp = Time.time + 0.01f;
        }

        if (flightControlSystem.thrustSpeed < 0)
        {
            flightControlSystem.thrustSpeed = 0;
        }
    }

    //This calculates pitch, turn, and roll according to the speed of the vehicle
    public static void CalculatePitchTurnRollSpeeds(FlightControlSystem_Small flightControlSystem)
    {
        float peakManeuverSpeed = flightControlSystem.ship.speedRating / 2f;
        float currentManeuverablity = 0f;
        float manveurablityPercentageAsDecimal = 0f;

        if (flightControlSystem.thrustSpeed <= peakManeuverSpeed & flightControlSystem.thrustSpeed > (peakManeuverSpeed / 3f))
        {
            currentManeuverablity = (100f / peakManeuverSpeed) * flightControlSystem.thrustSpeed;
        }
        else if (flightControlSystem.thrustSpeed >= peakManeuverSpeed & flightControlSystem.thrustSpeed < (flightControlSystem.ship.speedRating - (peakManeuverSpeed / 3f)))
        {
            currentManeuverablity = (100f / peakManeuverSpeed) * (peakManeuverSpeed - (flightControlSystem.thrustSpeed - peakManeuverSpeed));
        }
        else
        {
            currentManeuverablity = (100f / peakManeuverSpeed) * (peakManeuverSpeed / 3f);
        }

        manveurablityPercentageAsDecimal = (flightControlSystem.ship.maneuverabilityRating / 100f);

        flightControlSystem.pitchSpeed = (140f / 100f) * (currentManeuverablity * manveurablityPercentageAsDecimal);
        flightControlSystem.turnSpeed = (100f / 100f) * (currentManeuverablity * manveurablityPercentageAsDecimal);
        flightControlSystem.rollSpeed = (220f / 100f) * (currentManeuverablity * manveurablityPercentageAsDecimal); //160

        if (flightControlSystem.spinShip == true)
        {
            flightControlSystem.rollSpeed = ((160f / 100f) * 100) * 2.5f * manveurablityPercentageAsDecimal; //10 was 2.5f before
        }

    }

    //This makes the ship move
    public static void MoveShip(FlightControlSystem_Small flightControlSystem)
    {
        if (flightControlSystem.shipRigidbody == null)
        {
            flightControlSystem.shipRigidbody = flightControlSystem.gameObject.GetComponent<Rigidbody>();
        }

        if (flightControlSystem.shipRigidbody != null & flightControlSystem.ship.jumpingToHyperspace == false & flightControlSystem.ship.exitingHyperspace == false & flightControlSystem.docking == false & flightControlSystem.ship.damageSystem.isDisabled == false)
        {
            //This adds makes the ship move forward
            flightControlSystem.shipRigidbody.AddForce(flightControlSystem.gameObject.transform.position + flightControlSystem.gameObject.transform.forward * Time.fixedDeltaTime * flightControlSystem.thrustSpeed * 60000);

            //This rotates the ship
            Vector3 x = Vector3.right * flightControlSystem.pitchSpeed * flightControlSystem.pitchInput;
            Vector3 y = Vector3.up * flightControlSystem.turnSpeed * flightControlSystem.turnInput;
            Vector3 z = Vector3.forward * flightControlSystem.rollSpeed * flightControlSystem.rollInput;

            Vector3 rotationVector = x + y + z;

            Quaternion deltaRotation = Quaternion.Euler(rotationVector * Time.deltaTime);
            flightControlSystem.shipRigidbody.MoveRotation(flightControlSystem.shipRigidbody.rotation * deltaRotation);
        }
    }

    //Jump to Hyperspace
    public static IEnumerator JumpToHyperspace(FlightControlSystem_Small flightControlSystem)
    {
        if (flightControlSystem != null)
        {
            WingSystemFunctions.CloseWings(flightControlSystem.ship.wingSystems);

            yield return new WaitForSeconds(3.4f); //This gives the wings time to close
        }

        if (flightControlSystem != null)
        {
            flightControlSystem.ship.jumpingToHyperspace = true;

            Vector3 startPosition = flightControlSystem.gameObject.transform.localPosition;
            Vector3 endPosition = flightControlSystem.transform.localPosition + flightControlSystem.gameObject.transform.forward * 5000;

            AudioFunctions.PlayAudioClip(flightControlSystem.ship.audioManager, "hyperspace03_exit", "Explosions", flightControlSystem.transform.position, 1, 1, 1000, 1f);

            float timeElapsed = 0;
            float lerpDuration = 1;

            while (timeElapsed < lerpDuration)
            {
                if (flightControlSystem != null)
                {
                    flightControlSystem.gameObject.transform.localPosition = Vector3.Lerp(startPosition, endPosition, timeElapsed / lerpDuration);
                    timeElapsed += Time.deltaTime;
                    yield return new WaitForFixedUpdate();
                }
            }

            if (flightControlSystem != null)
            {
                HudFunctions.AddToShipLog(flightControlSystem.name.ToUpper() + " jumped to hyperspace");

                flightControlSystem.ship.jumpingToHyperspace = false;

                DamageSystemFunctions.DeactivateShip_SmallShip(flightControlSystem.ship.damageSystem);
            }
        }
    }

    //Exit Hyperspace
    public static IEnumerator ExitHyperspace(FlightControlSystem_Small flightControlSystem)
    {
        WingSystemFunctions.SnapClosedWings(flightControlSystem.ship.wingSystems); //Keeps wings shut on hyperspace exit

        flightControlSystem.ship.exitingHyperspace = true;

        Vector3 endPosition = flightControlSystem.transform.localPosition + flightControlSystem.gameObject.transform.forward * 5000; 
        Vector3 startPosition = flightControlSystem.gameObject.transform.localPosition;

        float timeElapsed = 0;
        float lerpDuration = 1;

        while (timeElapsed < lerpDuration)
        {
            if (flightControlSystem != null)
            {
                flightControlSystem.gameObject.transform.localPosition = Vector3.Lerp(startPosition, endPosition, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
            else
            {
                break;
            }
        }

        flightControlSystem.gameObject.transform.localPosition = endPosition;

        AudioFunctions.PlayAudioClip(flightControlSystem.ship.audioManager, "hyperspace03_exit", "Explosions", flightControlSystem.transform.position, 1, 1, 1000, 1f);

        HudFunctions.AddToShipLog(flightControlSystem.name.ToUpper() + " just exited hyperspace");

        WingSystemFunctions.OpenWings(flightControlSystem.ship.wingSystems); //Opens wings after hyperspace exit

        flightControlSystem.ship.exitingHyperspace = false;
    }

    //A particle effect that makes the ship look like it's moving
    public static void MovementEffect(FlightControlSystem_Small flightControlSystem)
    {
        if (flightControlSystem.ship.isAI == false)
        {
            if(flightControlSystem.movementEffect == null)
            {
                Object tempMovementEffect = PoolUtils.FindPrefabObjectInPool(flightControlSystem.ship.scene.particlePrefabPool, "MovementEffect");

                if (tempMovementEffect != null)
                {
                    GameObject movementEffect = GameObject.Instantiate(tempMovementEffect) as GameObject;
                    
                    if (movementEffect != null)
                    {
                        movementEffect.transform.SetParent(flightControlSystem.gameObject.transform);
                        movementEffect.transform.position = flightControlSystem.cameraPosition.transform.position + new Vector3(0, 0, 5);
                        movementEffect.transform.localRotation = Quaternion.identity;
                        flightControlSystem.movementEffect = movementEffect.GetComponent<ParticleSystem>();
                    }
                }
            }
            else if (flightControlSystem.movementEffect.gameObject.activeSelf == false & flightControlSystem.thrustSpeed > 10)
            {
                flightControlSystem.movementEffect.gameObject.SetActive(true);
            }
            else if (flightControlSystem.thrustSpeed > 10)
            {
                float particleSpeed = (4f / flightControlSystem.ship.speedRating) * flightControlSystem.thrustSpeed;
                var main = flightControlSystem.movementEffect.main;
                main.simulationSpeed = particleSpeed * 2;
            }
            else if (flightControlSystem.thrustSpeed < 10)
            {
                flightControlSystem.movementEffect.gameObject.SetActive(false);
            }
        }
        else
        {
            if(flightControlSystem.movementEffect != null)
            {
                flightControlSystem.movementEffect.gameObject.SetActive(false);
            }
        }
    }

    #endregion
}
