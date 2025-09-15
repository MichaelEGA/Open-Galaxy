using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnIonHit : MonoBehaviour
{
    public ParticleSystem particleSystemScript;
    public List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
    public SmallShip smallShip;
    private void OnParticleCollision(GameObject objectHit)
    {
        IonFunctions.RunCollisionEvent(objectHit, collisionEvents, particleSystemScript, smallShip);
    }
}
