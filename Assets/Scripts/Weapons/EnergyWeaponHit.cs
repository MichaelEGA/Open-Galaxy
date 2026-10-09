using UnityEngine;

public class EnergyWeaponHit : MonoBehaviour
{
    private void OnParticleCollision(GameObject attacker)
    {
        ParticleSystem ps = attacker.GetComponent<ParticleSystem>();

        SmallShip smallShip = this.gameObject.GetComponent<SmallShip>();

        EnergyWeaponFunctions.RunCollisionEvent(ps, smallShip);
    }
}
