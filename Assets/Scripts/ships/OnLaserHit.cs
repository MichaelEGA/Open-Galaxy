using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnLaserHit : MonoBehaviour
{
    private ParticleSystem ps;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
    public GameObject relatedGameObject;
    private Vector3 hitPosition;
    private Vector3 hitRotation;
    private Audio audioManager;
    private SmallShip attackingSmallShip;
    private Turret attackTurret;
    public string type = "laser";
    public Scene scene;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        attackingSmallShip = relatedGameObject.GetComponent<SmallShip>();
        attackTurret = relatedGameObject.GetComponent<Turret>();
        scene = SceneFunctions.GetScene();
    }

    private void OnParticleCollision(GameObject objectHit)
    {
        List<Vector3> hitPositions = new List<Vector3>();
        List<Vector3> hitRotations = new List<Vector3>();

        int events = ps.GetCollisionEvents(objectHit, collisionEvents); //This grabs all the collision events

        for (int i = 0; i < events; i++) //This cycles through all the collision events and deals with one at a time
        {
            hitPosition = collisionEvents[i].intersection; //This gets the position of the collision event
            hitRotation = collisionEvents[i].normal; //This gets the rotation of the collision event

            GameObject objectHitParent = ReturnParent(objectHit); //This gets the colliders object parent  

            if (objectHitParent != relatedGameObject)
            {
                //Ship that has been hit values
                SmallShip smallShip = objectHit.gameObject.GetComponentInParent<SmallShip>(); //This gets the smallship function if avaiblible
                LargeShip largeShip = objectHit.gameObject.GetComponentInParent<LargeShip>();
                Turret turret = objectHit.gameObject.GetComponentInParent<Turret>();
                bool hasPlasma = false;
                
                float shieldFront = 0f;
                float shieldBack = 0f;
                float forward = 0;

                if (smallShip != null)
                {
                    hasPlasma = smallShip.hasPlasma;

                    shieldFront = smallShip.frontShieldLevel;
                    shieldBack = smallShip.rearShieldLevel;

                    Vector3 relativePosition = smallShip.gameObject.transform.position - hitPosition;
                    forward = -Vector3.Dot(smallShip.gameObject.transform.position, relativePosition.normalized);
                }

                if (largeShip != null)
                {
                    hasPlasma = largeShip.hasPlasma;

                    shieldFront = largeShip.frontShieldLevel;
                    shieldBack = largeShip.rearShieldLevel;

                    Vector3 relativePosition = largeShip.gameObject.transform.position - hitPosition;
                    forward = -Vector3.Dot(largeShip.gameObject.transform.position, relativePosition.normalized);
                }

                //Attacking ship values
                bool rapidFire = false;

                if (attackingSmallShip != null)
                {
                    if (attackingSmallShip.weaponMode == "rapid")
                    {
                        rapidFire = true;
                    }
                }

                if (objectHitParent != null) //This prevents lasers from causing damage to the firing ship if they accidentally hit the collider
                {
                    //Acquire damage calculation from attack ship/turret
                    float damage = CalculateLaserDamage(attackingSmallShip, attackTurret);

                    if (smallShip != null)
                    {
                        if (type == "laser")
                        {
                            SmallShipFunctions.TakeDamage(smallShip, damage, hitPosition, rapidFire);
                        }
                        else if (type == "ion")
                        {
                            SmallShipFunctions.TakeSystemDamage(smallShip, damage, hitPosition);
                        }
                    }
                    else if (turret != null)
                    {
                        if (type == "laser")
                        {
                            //TurretFunctions.TakeDamage(turret, damage, hitPosition);
                        }
                        else if (type == "ion")
                        {
                            //TurretFunctions.TakeSystemsDamage(turret, damage, hitPosition);
                        }
                    }
                    else if (largeShip != null)
                    {
                        if (type == "laser")
                        {
                            LargeShipFunctions.TakeDamage(largeShip, damage, hitPosition);
                        }
                        else if (type == "ion")
                        {
                            LargeShipFunctions.TakeSystemsDamage(largeShip, damage, hitPosition);
                        }
                    }
                }

                if (audioManager == null)
                {
                    audioManager = GameObject.FindFirstObjectByType<Audio>();
                }

                if (type == "laser")
                {
                    //This selects the correct explosion colour
                    string explosionChoice = "laserblast_red";

                    if (attackingSmallShip != null)
                    {
                        if (forward > 0 & shieldFront > 0 || forward < 0 & shieldBack > 0)
                        {
                            if (hasPlasma == true)
                            {
                                explosionChoice = "blackhole";
                            }
                            else if (attackingSmallShip.laserColor == "red")
                            {
                                explosionChoice = "laserblast_red";
                            }
                            else if (attackingSmallShip.laserColor == "green")
                            {
                                explosionChoice = "laserblast_green";
                            }
                        }
                        else
                        {
                            explosionChoice = "hullstrike";
                        }
                    }

                    if (attackTurret != null)
                    {
                        if (forward > 0 & shieldFront > 0 || forward < 0 & shieldBack > 0)
                        {
                            if (hasPlasma == true)
                            {
                                explosionChoice = "blackhole";
                            }
                            if (attackTurret.laserColor == "red")
                            {
                                explosionChoice = "laserblast_red";
                            }
                            else if (attackTurret.laserColor == "green")
                            {
                                explosionChoice = "laserblast_green";
                            }
                        }
                        else
                        {
                            explosionChoice = "hullstrike";
                        }
                    }

                    //This instantiates an explosion at the point of impact
                    if (attackTurret != null)
                    {
                        //if (attackTurret.turretType == "large")
                        //{
                        //    ParticleFunctions.InstantiateExplosion(objectHit, hitPosition, explosionChoice, 100, audioManager);
                        //}
                        //else
                        //{
                        //    ParticleFunctions.InstantiateExplosion(objectHit, hitPosition, explosionChoice, 6, audioManager);
                        //}
                    }
                    else
                    {
                        if (rapidFire == false)
                        {
                            ParticleFunctions.InstantiateExplosion(objectHit, hitPosition, explosionChoice, 6, audioManager);
                        }
                        else
                        {
                            ParticleFunctions.InstantiateExplosion(objectHit, hitPosition, explosionChoice, 2, audioManager);
                        }
                    }
                }
                else if (type == "ion")
                {
                    string explosionChoice = "laserblast_ion";

                    if (forward > 0 & shieldFront > 0 || forward < 0 & shieldBack > 0)
                    {
                        if (hasPlasma == true)
                        {
                            explosionChoice = "blackhole";
                        }
                    }

                    if (attackTurret != null)
                    {
                        //if (attackTurret.turretType == "large")
                        //{
                        //    ParticleFunctions.InstantiateExplosion(objectHit, hitPosition, explosionChoice, 100, audioManager);
                        //}
                        //else
                        //{
                        //    ParticleFunctions.InstantiateExplosion(objectHit, hitPosition, explosionChoice, 3, audioManager);
                        //}
                    }
                    else
                    {
                        ParticleFunctions.InstantiateExplosion(objectHit, hitPosition, explosionChoice, 3, audioManager);
                    }
                }     
            }
        }
    }

    #region on laser hit utils

    //Calculates the damage done by the laser
    private float CalculateLaserDamage(SmallShip smallShip, Turret turret)
    {
        float damage = 0;
        float laserPower = 0;
        float laserRating = 0;
        float laserDamage = 0;

        if (smallShip != null)
        {
            laserPower = smallShip.laserPower;
            laserRating = smallShip.laserRating;
            laserDamage = 50;
        }
        else if (turret != null)
        {
            laserPower = 100;
            laserRating = 100;
            //laserDamage = turret.laserDamage;
        }

        if (laserPower > 50)
        {
            damage = (laserDamage / 100F) * laserRating;
        }
        else if (laserPower == 50f)
        {
            damage = (laserDamage / 100F) * laserRating;
        }
        else if (laserPower < 50f)
        {
            damage = (laserDamage / 100F) * laserRating;
        }

        return damage;
    }

    //This function returns the root parent of the prefab by looking for the component that will only be attached to the parent gameobject
    private GameObject ReturnParent(GameObject objectHit)
    {
        GameObject parent = null;

        SmallShip smallShip = objectHit.gameObject.GetComponentInParent<SmallShip>();
        Turret turret = objectHit.gameObject.GetComponentInParent<Turret>();
        LargeShip largeShip = objectHit.gameObject.GetComponentInParent<LargeShip>();

        if (smallShip != null)
        {
            parent = smallShip.gameObject;
        }
        else if (turret != null)
        {
            parent = turret.gameObject;
        }
        else if (largeShip != null)
        {
            parent = largeShip.gameObject;
        }

        return parent;
    }

    #endregion

}
