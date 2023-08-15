using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TurretFunctions
{
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
                        turretGameObject.transform.position = turretTransform.position;
                        turretGameObject.transform.rotation = turretTransform.rotation;
                        turretGameObject.transform.SetParent(turretTransform);

                        Transform turretArm = GameObjectUtils.FindChildTransformContaining(turretTransform, "arm");

                        if (turretArm != null)
                        {
                            tempTurret.turretArm = turretArm.gameObject;
                        }

                        turrets.Add(tempTurret);
                    }
                }
            }

            largeShip.turrets = turrets.ToArray();

            largeShip.turretsLoaded = true;
        }
    }

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

    public static void SmoothPitchInput(Turret turret, float pitchInput)
    {
        float step = +Time.deltaTime / 0.01f;
        turret.pitchInput = Mathf.Lerp(turret.pitchInput, pitchInput, step);
    }

    public static void SmoothTurnInput(Turret turret, float turnInput)
    {
        float step = +Time.deltaTime / 0.01f;
        turret.turnInput = Mathf.Lerp(turret.turnInput, turnInput, step);
    }

    public static void RotateTurret(Turret turret)
    {
        turret.pitchInputActual = Mathf.Lerp(turret.pitchInputActual, turret.pitchInput, 0.1f);
        turret.turnInputActual = Mathf.Lerp(turret.turnInputActual, turret.turnInput, 0.1f);

        Vector3 armRotation = Vector3.right * turret.pitchInputActual * 60;
        Vector3 baseRotation = Vector3.up * turret.turnInputActual * 50;

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
            if (rotation < -30)
            {
                turret.turretBase.transform.localRotation = Quaternion.Euler(0, -30, 0);
            }
            else if (rotation > 30)
            {
                turret.turretBase.transform.localRotation = Quaternion.Euler(0, 30, 0);
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
}
