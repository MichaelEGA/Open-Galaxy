using UnityEngine;

public class EnergyWeaponHit : MonoBehaviour
{
    private void OnParticleCollision(GameObject attacker)
    {
        ParticleSystem ps = attacker.GetComponent<ParticleSystem>();

        FlightControlSystem_Small smallShip = this.gameObject.GetComponent<FlightControlSystem_Small>();

        EnergyWeaponFunctions.RunCollisionEvent(ps, smallShip);
    }
}
