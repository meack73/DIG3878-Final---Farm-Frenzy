using UnityEngine;
using UnityEngine.InputSystem;

//script goes on cannon
public class ThrowingScript : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject rockPrefab; 
    float rockImpulse = 30f; 
    private ShooterBehavior shooter;
    private float throwDelay = 0f; // to sync with animation
    private float shotTimer = 0f;
    private float shotInterval = 0.833f; 
    private bool canShoot = true;
    private float repeatTimer = 0f;
    private float animTimer = 0f;
    private bool isAnimating = false;
    
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
        repeatTimer += Time.deltaTime;

        if (canShoot && repeatTimer > shotInterval) 
        {
            if (Time.time >= shooter.lastAttackTime + shooter.attackCooldown)
            {
                shooter.lastAttackTime = Time.time;
                canShoot = false; 
                shotTimer = 0f;
                animTimer = 0f;
                isAnimating = true;
                shooter.isAttacking = true;
                repeatTimer = 0f;
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

        if (isAnimating)
        {
            animTimer += Time.deltaTime;
            if (animTimer >= shotInterval)
            {
                shooter.isAttacking = false;
                isAnimating = false;
            }
        }
    }

    void Shoot()
    {
        GameObject therock = (GameObject)Instantiate(rockPrefab, this.transform.position, this.transform.rotation); 
        therock.tag = bulletTag;
        therock.GetComponent<Rigidbody>().AddForce(this.transform.forward * rockImpulse, ForceMode.Impulse); //adding force to our rock
    }
}