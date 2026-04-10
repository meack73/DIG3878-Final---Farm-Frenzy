using UnityEngine;
using UnityEngine.InputSystem;

//script goes on cannon
public class ThrowingScript : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject rockPrefab; 
    float rockImpulse = 30f; 
    private ShooterBehavior shooter;
    private float throwDelay = 0.5f; // to sync with animation
    private float shotTimer = 0f;
    private float shotInterval = 1f; 
    private bool canShoot = true;
    private float repeatTimer = 0f;

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
        if (repeatTimer > shotInterval && canShoot) 
        {
            repeatTimer = 0f;
            if (Time.time >= shooter.lastAttackTime + shooter.attackCooldown)
            {
                canShoot = false; 
                shotTimer = 0f;
                shooter.isAttacking = true; //start animation
            }
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
        repeatTimer += Time.deltaTime;
    }

    void Shoot()
    {
        GameObject therock = (GameObject)Instantiate(rockPrefab, this.transform.position, this.transform.rotation); 
        therock.tag = bulletTag;
        therock.GetComponent<Rigidbody>().AddForce(this.transform.forward * rockImpulse, ForceMode.Impulse); //adding force to our rock
    }
}