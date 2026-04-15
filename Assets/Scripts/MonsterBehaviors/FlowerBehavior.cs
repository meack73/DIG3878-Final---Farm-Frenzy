using UnityEngine;
using System.Collections; 

public class FlowerBehavior : MonoBehaviour
{
    private Animator animator;    
    private Rigidbody rb;

    [Header("Stats")]
    public int health = 3;
    public Vector3 spawnPoint = Vector3Int.zero; 
    public Vector3Int spawnTile = Vector3Int.zero; 

    //private float damageTimer = 0f;
    //private float damageCooldown = 3.0f; // same as monster attack cooldown 

    public int playerId = 0; 
    public bool coinSpawn = false; 
    private float coinSpawnAnimationCooldown = 2.5f; 
    private float coinSpawnAnimationTimer = 0f;
    private bool isDying = false;

    [Header("Audio")]
    public AudioClip death;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        animator.SetBool("Idle", true);
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.freezeRotation = true;
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
        string enemyBulletTag = playerId == 1 ? "P2Bullet" : "P1Bullet";
        if (collision.gameObject.CompareTag(enemyBulletTag))
        {
            Destroy(collision.gameObject);
            TakeDamage(1);        
        } 
    }
  
    public void TakeDamage(int damage)
    {
        if (isDying || health <= 0) return;
        health -= damage;

        if (health <= 0)
        {
            isDying = true;
            StartCoroutine(Die());
        } 
    }

    IEnumerator Die()
    {
        float duration = 0.7f;
        float time = 0f;

        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.Euler(90, 0, 0);

        while (time < duration)
        {
            transform.rotation = Quaternion.Lerp(startRot, endRot, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRot;
        GetComponent<Collider>().enabled = false;
        audioSource.PlayOneShot(death);
        Destroy(gameObject, 2f);
    }
}