using UnityEngine;
using UnityEngine.InputSystem;

//script goes on cannon
public class ThrowingScript : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject rockPrefab; 
    float rockImpulse = 30f; 
    public ShooterBehavior shooter;
    public float throwDelay = 0.2f;
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
        if (repeatTimer > 3f && canShoot) 
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
                shooter.isAttacking = false;
                shooter.lastAttackTime = Time.time;
            }
        }
        repeatTimer += Time.deltaTime;
    }

    void Shoot()
    {
        GameObject therock = (GameObject)Instantiate(rockPrefab, this.transform.position, this.transform.rotation); 
        therock.GetComponent<Rigidbody>().AddForce(this.transform.forward * rockImpulse, ForceMode.Impulse); //adding force to our rock
    }
}