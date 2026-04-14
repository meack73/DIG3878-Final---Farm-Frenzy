using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

//script goes on cannon
public class ThrowingScript : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject rockPrefab; 
    float rockImpulse = 30f; 
    public ShooterBehavior shooter;
    public float throwDelay = 0.5f;
    private float shotTimer = 0f;
    private bool canShoot = true;
    private float repeatTimer = 0.833f;

    private string bulletTag = "P1Bullet";

    void Start()
    {
        shooter = GetComponentInParent<ShooterBehavior>();
        if (shooter.playerId == 2)
        {
            bulletTag = "P2Bullet";
        }
    }

    void Update()
    {
        bool attackReady = Time.time >= shooter.lastAttackTime + shooter.attackCooldown;

        if (attackReady && canShoot)
        {
            canShoot = false;
            shotTimer = 0f;
            shooter.lastAttackTime = Time.time; // opens the animation window
        }

        if (!canShoot)
        {
            shotTimer += Time.deltaTime;
            if (shotTimer >= throwDelay)
            {
                Shoot();
                canShoot = true;
            }
        }
    }

    void Shoot()
    {
        GameObject therock = (GameObject)Instantiate(rockPrefab, this.transform.position, this.transform.rotation); 
        therock.GetComponent<Rigidbody>().AddForce(this.transform.forward * rockImpulse, ForceMode.Impulse); //adding force to our rock
    }
}