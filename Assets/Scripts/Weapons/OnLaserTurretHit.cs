using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnLaserTurretHit : MonoBehaviour
{
    public ParticleSystem particleSystemScript;
    public List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
    public LaserTurret laserTurret;

    private void OnParticleCollision(GameObject objectHit)
    {
        LaserTurretFunctions.RunCollisionEvent(objectHit, collisionEvents, particleSystemScript, laserTurret);
    }
}
