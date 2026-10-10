using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    public Ship ship;
    public GameObject smokeTrail;
    public bool isCurrentlyColliding;
    public bool isCurrentlyCollidingSmallShip;
    public float restoreDelayTime;
    public string explosionType;
    public string shieldType;
    public bool exploded;
    public bool invincible;
    public bool cannotbedisabled;
    public bool isDisabled;
    public bool warningSoundPlayed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ship = GetComponent<Ship>();
    }

    // Update is called once per frame
    void Update()
    {
        //Damage functions
        DamageSystemFunctions.TakeCollisionDamage_SmallShip(this);
        DamageSystemFunctions.SmokeTrail_SmallShip(this);
        DamageSystemFunctions.Explode_SmallShip(this);
        DamageSystemFunctions.PlayDamageWarningSound(this);

        //Systems functions
        DamageSystemFunctions.RestoreShipsSystems_SmallShip(this);
    }

    void OnCollisionEnter(Collision collision)
    {
        DamageSystemFunctions.StartCollision_SmallShip(this, collision.gameObject);

        Debug.Log("Collided with " + collision.gameObject.name + " " + collision.collider.gameObject.name + "at point" + collision.gameObject.transform.localPosition);
    }

    void OnCollisionExit(Collision collision)
    {
        DamageSystemFunctions.EndCollision_SmallShip(this);
    }
}
