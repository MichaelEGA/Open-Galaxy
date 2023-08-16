using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TurretFunctions
{

    #region loading functions

    //This loads all the turrets on the ship
    public static void LoadTurrets(LargeShip largeShip)
    {
        if (largeShip.turretsLoaded == false)
        {
            List<Turret> turrets = new List<Turret>();
            Transform[] turretTransforms = GameObjectUtils.FindAllChildTransformsContaining(largeShip.transform, "turret", "turrettransforms");

            GameObject turretLarge = Resources.Load("TurretPrefabs/" + largeShip.prefabName + "_turretlarge") as GameObject;
            GameObject turretSmall = Resources.Load("TurretPrefabs/" + largeShip.prefabName + "_turretsmall") as GameObject;

            foreach (Transform turretTransform in turretTransforms)
            {
                if (turretTransform != null)
                {
                    GameObject turretGameObject = null;

                    if (turretTransform.name == "turretsmall")
                    {
                        turretGameObject = GameObject.Instantiate(turretSmall);
                    }
                    else if (turretTransform.name == "turretlarge")
                    {
                        turretGameObject = GameObject.Instantiate(turretLarge);
                    }

                    if (turretGameObject != null)
                    {                
                        Turret tempTurret = turretGameObject.AddComponent<Turret>();
                        tempTurret.largeShip = largeShip;
                        tempTurret.turretBase = turretGameObject;

                        Transform turretArm = GameObjectUtils.FindChildTransformContaining(turretGameObject.transform, "arm");

                        if (turretArm != null)
                        {
                            tempTurret.turretArm = turretArm.gameObject;
                        }


                        turretGameObject.transform.position = turretTransform.position;
                        turretGameObject.transform.rotation = turretTransform.rotation;
                        turretGameObject.transform.SetParent(turretTransform);

                        CheckTurretType(tempTurret);

                        turrets.Add(tempTurret);
                    }
                }
            }

            largeShip.turrets = turrets.ToArray();

            largeShip.turretsLoaded = true;
        }
    }

    //This inputs the rotation values
    public static void CheckTurretType(Turret turret)
    {
        if (turret.gameObject != null)
        {
            if (turret.gameObject.name.Contains("isd_turretlarge"))
            {
                turret.yRotationMax = 30;
                turret.yRotationMin = -30;
                turret.turretSpeed = 70;
            }
            else if (turret.gameObject.name.Contains("isd_turretsmall"))
            {
                turret.yRotationMax = 90;
                turret.yRotationMin = -90;
                turret.turretSpeed = 90;
            }
            else if (turret.gameObject.name.Contains("cr90_turretlarge"))
            {
                turret.yRotationMax = 180;
                turret.yRotationMin = -180;
                turret.turretSpeed = 80;
            }
        }
    }

    #endregion

    #region turret input

    //This gets the input for the turret from ship
    public static void TurretInput(Turret turret, LargeShip largeShip)
    {
        if (largeShip.target != null)
        {
            Vector3 targetRelativePosition = largeShip.target.transform.position - turret.turretArm.transform.position;

            float targetForward = Vector3.Dot(turret.turretArm.transform.forward, targetRelativePosition.normalized);
            float targetRight = Vector3.Dot(turret.turretArm.transform.right, targetRelativePosition.normalized);
            float targetUp = Vector3.Dot(turret.turretArm.transform.up, targetRelativePosition.normalized);

            TurretFunctions.SmoothTurnInput(turret, targetRight);
            TurretFunctions.SmoothPitchInput(turret, -targetUp);     
        }      
    }

    //This smoothly transitions between input variables as if the computer is using a mouse a joystick
    public static void SmoothPitchInput(Turret turret, float pitchInput)
    {
        float step = +Time.deltaTime / 0.01f;
        turret.pitchInput = Mathf.Lerp(turret.pitchInput, pitchInput, step);
    }

    //This smoothly transitions between input variables as if the computer is using a mouse a joystick
    public static void SmoothTurnInput(Turret turret, float turnInput)
    {
        float step = +Time.deltaTime / 0.01f;
        turret.turnInput = Mathf.Lerp(turret.turnInput, turnInput, step);
    }

    #endregion

    #region turretRotation

    //This rotates the turret
    public static void RotateTurret(Turret turret)
    {
        turret.pitchInputActual = Mathf.Lerp(turret.pitchInputActual, turret.pitchInput, 0.1f);
        turret.turnInputActual = Mathf.Lerp(turret.turnInputActual, turret.turnInput, 0.1f);

        float armSpeed = (120f / 100f) * turret.turretSpeed;
        float baseSpeed = (100f / 100f) * turret.turretSpeed;

        Vector3 armRotation = Vector3.right * turret.pitchInputActual * armSpeed;
        Vector3 baseRotation = Vector3.up * turret.turnInputActual * baseSpeed;

        Vector3 armRotation2 = new Vector3(armRotation.x, 0, 0);
        Vector3 baseRotation2 = new Vector3(0, baseRotation.y, 0);

        if (turret.turretBase != null)
        {
            //This applies the rotation
            turret.turretBase.transform.Rotate(baseRotation2, Time.fixedDeltaTime, Space.World);

            //This tracks the rotation and prevents it from going beyond predefined parameters
            float rotation = turret.turretBase.transform.localRotation.eulerAngles.y;

            //This keeps the rotation value between 180 and -180
            if (rotation > 180)
            {
                rotation -= 360;
            }
            else if (rotation < -180)
            {
                rotation += 360;
            }

            //This cause the turret to stop rotating at its limites
            if (rotation < turret.yRotationMin)
            {
                turret.turretBase.transform.localRotation = Quaternion.Euler(0, turret.yRotationMin, 0);
            }
            else if (rotation > turret.yRotationMax)
            {
                turret.turretBase.transform.localRotation = Quaternion.Euler(0, turret.yRotationMax, 0);
            }
        }

        if (turret.turretArm != null)
        {
            //This applies the rotation
            turret.turretArm.transform.Rotate(armRotation2, Time.fixedDeltaTime, Space.World);

            //This tracks the rotation and prevents it from going beyond predefined parameters
            float rotation = turret.turretArm.transform.localRotation.eulerAngles.x;

            //This keeps the rotation value between 180 and -180
            if (rotation > 180)
            {
                rotation -= 360;
            }
            else if (rotation < -180)
            {
                rotation += 360;
            }

            //This cause the turret to stop rotating at its limites
            if (rotation < -90)
            {
                turret.turretArm.transform.localRotation = Quaternion.Euler(-90, 0, 0);
            }
            else if (rotation > 0)
            {
                turret.turretArm.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }

        }   
    }

    #endregion
}
