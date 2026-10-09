using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    public SmallShip smallShip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        smallShip = GetComponent<SmallShip>();
    }

    // Update is called once per frame
    void Update()
    {
        //Damage functions
        DamageSystemFunctions.TakeCollisionDamage_SmallShip(smallShip);
        DamageSystemFunctions.SmokeTrail_SmallShip(smallShip);
        DamageSystemFunctions.Explode_SmallShip(smallShip);
        DamageSystemFunctions.PlayDamageWarningSound(smallShip);

        //Systems functions
        DamageSystemFunctions.RestoreShipsSystems_SmallShip(smallShip);
    }
}
