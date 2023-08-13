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
                        turretGameObject.AddComponent<Turret>();
                        turretGameObject.transform.position = turretTransform.position;
                        turretGameObject.transform.rotation = turretTransform.rotation;
                        turretGameObject.transform.SetParent(turretTransform);
                    }
                }
            }

            largeShip.turretsLoaded = true;
        }
    }




}
