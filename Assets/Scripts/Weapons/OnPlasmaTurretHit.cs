using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlasmaTurretHit : MonoBehaviour
{
    public ParticleSystem particleSystemScript;
    public List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
    public PlasmaTurret plasmaTurret;

    private void OnParticleCollision(GameObject objectHit)
    {
        PlasmaTurretFunctions.RunCollisionEvent(objectHit, collisionEvents, particleSystemScript, plasmaTurret);
    }
}
