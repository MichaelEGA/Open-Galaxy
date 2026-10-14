using UnityEngine;

public class EnergyWeaponHit : MonoBehaviour
{
    private void OnParticleCollision(GameObject attacker)
    {
        ParticleSystem ps = attacker.GetComponent<ParticleSystem>();

        Ship ship = this.gameObject.GetComponent<Ship>();

        EnergyWeaponFunctions.RunCollisionEvent(ps, ship);
    }
}
