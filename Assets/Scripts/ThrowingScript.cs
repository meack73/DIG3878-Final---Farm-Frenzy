using UnityEngine;
using UnityEngine.InputSystem;

//script goes on cannon
public class ThrowingScript : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject rockPrefab; 
    float rockImpulse = 30f; 
    public MonsterBehavior monsterBehavior;
    private float throwDelay = 0.5f;
    private float shotTimer = 0f;
    private bool canShoot = true;
    private float repeatTimer = 0f;

    void Update()
    {
        if (repeatTimer > 3f && canShoot) 
        {
            repeatTimer = 0f;
            if (Time.time >= monsterBehavior.lastAttackTime + monsterBehavior.attackCooldown)
            {
                canShoot = false; 
                shotTimer = 0f;
                monsterBehavior.isAttacking = true; //start animation
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
        therock.GetComponent<Rigidbody>().AddForce(this.transform.forward * rockImpulse, ForceMode.Impulse); //adding force to our rock
    }
}