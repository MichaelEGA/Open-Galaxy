using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{

    private ParticleSystem ps;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
    public GameObject relatedGameObject;
    private Vector3 hitPosition;
    private Vector3 hitRotation;
    private Audio audioManager;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
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
                if (objectHitParent != null) //This prevents lasers from causing damage to the firing ship if they accidentally hit the collider
                {
                    SmallShip smallShip = objectHit.gameObject.GetComponentInParent<SmallShip>(); //This gets the smallship function if avaiblible
                    SmallShip attackingShip = relatedGameObject.GetComponent<SmallShip>();

                    if (smallShip != null) //This checks if the object being hit is a small ship
                    {
                        LaserFunctions.TakeLaserDamage(smallShip, attackingShip, hitPosition); //This causes the smallship to take damage
                    }
                }

                if (audioManager == null)
                {
                    audioManager = GameObject.FindObjectOfType<Audio>();
                }

                LaserFunctions.InstantiateExplosion(objectHit, hitPosition, hitRotation, "explosion01", 6, audioManager);
                
            }

        }

    }

    //This function returns the root parent of the prefab by looking for the component that will only be attached to the parent gameobject
    private GameObject ReturnParent(GameObject objectHit)
    {
        GameObject parent = null;

        SmallShip smallShip = objectHit.gameObject.GetComponentInParent<SmallShip>();

        if (smallShip != null)
        {
            parent = smallShip.gameObject;
        }

        return parent;
    }



}
