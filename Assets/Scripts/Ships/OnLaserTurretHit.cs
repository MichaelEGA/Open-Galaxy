using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnLaserTurretHit : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
    public Vector3 hitPosition;
    public Audio audioManager;
    public LaserTurret laserTurret;
    public Scene scene;
    public string mode = "small";

    private void OnParticleCollision(GameObject objectHit)
    {
        List<Vector3> hitPositions = new List<Vector3>();

        int events = particleSystem.GetCollisionEvents(objectHit, collisionEvents); //This grabs all the collision events

        for (int i = 0; i < events; i++) //This cycles through all the collision events and deals with one at a time
        {

            //REWRITE SCRIPT FROM SCRATCH
            //1.GET COLLISION INFORMATION
            //2. GET KEY VALUES FROM OBJECT/SHIP COLLIDED WITH
            //3. INSTANTIATE THE CORRECT EXPLOSION
            //4. APPLY THE CORRECT DAMAGE TO TARGET

            hitPosition = collisionEvents[i].intersection; //This gets the position of the collision event

            GameObject objectHitParent = ReturnParent(objectHit); //This gets the colliders object parent  

            if (objectHitParent != laserTurret.gameObject)
            {
                //This gets key values from the object that has been hit
                SmallShip smallShip = objectHit.gameObject.GetComponentInParent<SmallShip>(); //This gets the smallship function if avaiblible
                LargeShip largeShip = objectHit.gameObject.GetComponentInParent<LargeShip>();
                LaserTurret turret = objectHit.gameObject.GetComponentInParent<LaserTurret>();
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

                if (objectHitParent != null) //This prevents lasers from causing damage to the firing ship if they accidentally hit the collider
                {
                    //Acquire damage calculation from attack ship/turret
                }

                if (audioManager == null)
                {
                    audioManager = GameObject.FindFirstObjectByType<Audio>();
                }

                //This selects the correct explosion colour
                string explosionChoice = "laserblast_red";

                if (laserTurret != null)
                {
                    if (forward > 0 & shieldFront > 0 || forward < 0 & shieldBack > 0)
                    {
                        if (hasPlasma == true)
                        {
                            explosionChoice = "blackhole";
                        }
                        if (laserTurret.laserColor == "red")
                        {
                            explosionChoice = "laserblast_red";
                        }
                        else if (laserTurret.laserColor == "green")
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
                if (mode == "large")
                {
                    ParticleFunctions.InstantiateExplosion(objectHit, hitPosition, explosionChoice, 100, audioManager);
                }
                else
                {
                    ParticleFunctions.InstantiateExplosion(objectHit, hitPosition, explosionChoice, 6, audioManager);
                }

                
            }
        }
    }

    #region on laser hit utils
    //This function returns the root parent of the prefab by looking for the component that will only be attached to the parent gameobject
    private GameObject ReturnParent(GameObject objectHit)
    {
        GameObject parent = null;

        SmallShip smallShip = objectHit.gameObject.GetComponentInParent<SmallShip>();
        LaserTurret turret = objectHit.gameObject.GetComponentInParent<LaserTurret>();
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
