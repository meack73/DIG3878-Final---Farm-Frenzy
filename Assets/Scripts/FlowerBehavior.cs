using UnityEngine;

public class FlowerBehavior : MonoBehaviour
{
    private Animator animator;    
    private Rigidbody rb;

    public int health = 3;
    public Vector3 spawnPoint = Vector3Int.zero; 
    public Vector3Int spawnTile = Vector3Int.zero; 

    private float damageTimer = 0f;
    private float damageCooldown = 3.0f; // same as monster attack cooldown 

    public int playerId = 0; 
    public bool coinSpawn = false; 
    private float coinSpawnAnimationCooldown = 2.5f; 
    private float coinSpawnAnimationTimer = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Idle", true);
    }
    
    void Update()
    {
        if(coinSpawn)
        {
            CoinAnimation();
        } else
        {
            animator.SetBool("Coin", false); 
            animator.SetBool("Idle", true);
        }
    }

    public void CoinAnimation()
    {
        coinSpawnAnimationTimer += Time.deltaTime;

        if (coinSpawn && coinSpawnAnimationTimer >= coinSpawnAnimationCooldown)
        {        
            if (coinSpawn)
            {
                animator.SetBool("Coin", true);
                coinSpawn = false;
            }
            
            coinSpawnAnimationTimer = 0f;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Stationary plants should take damage from Bullets (monsters) or Enemy projectiles
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            TakeDamage(1);        
        } 

        if (collision.gameObject.CompareTag("Monster") && collision.gameObject.GetComponent<MonsterBehavior>().playerId != playerId)
        {
            TakeDamage(1);
            Debug.Log("Plant hit by Monster!");
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Monster") && collision.gameObject.GetComponent<MonsterBehavior>().playerId != playerId)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageCooldown)
            {
                TakeDamage(1);
                damageTimer = 0f;
            }
        }
    }

    void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            PlayDeath();
        }
    }

    void PlayDeath()
    {
        //animator.SetBool("Die", true);
        GetComponent<Collider>().enabled = false; 
        //rb.isKinematic = true; 
        Destroy(gameObject, 5f);
    }
}