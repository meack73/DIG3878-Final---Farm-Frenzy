using UnityEngine;

public class ShooterBehavior : MonoBehaviour
{
    private Animator animator;    
    private Rigidbody rb;
    public int health = 3;

    [Header("Combat Settings")]
    public float attackCooldown = 3.0f;   // 1 second between hits
    public float lastAttackTime = 0f;
    public bool isAttacking = false;
    public Vector3 spawnPoint = Vector3Int.zero; //clear init board tile when walking
    public Vector3Int spawnTile = Vector3Int.zero; //store spawn tile for pathfinding

    private bool isDying = false;

    private float animTimer = 0f;
    private bool isAnimating = false;
    
    public int playerId = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rb.isKinematic = false;
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (isAttacking)
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                animator.SetBool("Attack", true);
                animator.SetBool("Idle", false);
                lastAttackTime = Time.time;

                isAttacking = true;; 
            }
        } else
        {
            animator.SetBool("Attack", false);
            animator.SetBool("Idle", true);
        }

        isAttacking  = false; 

        if (health <= 0)
        {
            PlayDeath();
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
        if (isDying) return;
        health -= damage;
        Debug.Log(gameObject.name + " Health: " + health);

        if (health <= 0)
        {
            PlayDeath();
        }
    }
   
    void PlayDeath()
    {
        if (isDying) return;
        isDying = true;

        animator.SetBool("Die", true);
        GetComponent<Collider>().enabled = false;
        rb.isKinematic = true;
        Destroy(gameObject, 5f);
    }
}