using UnityEngine;

public class MonsterBehavior : MonoBehaviour
{
    private Animator animator;    
    private Rigidbody rb;

    public int health = 3;
    public float attackCooldown = 1.0f;   // 1 second between hits
    public float lastAttackTime = 0f;
    public bool isAttacking = false;
    public Vector3 spawnPoint = Vector3Int.zero; //clear init board tile when walking
    public Vector3Int spawnTile = Vector3Int.zero; //store spawn tile for pathfinding
    
    public int playerId = 0; 
    private float speed = 1.5f;

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
            Vector3 moveDir = transform.forward * speed;
            rb.linearVelocity = new Vector3(moveDir.x, rb.linearVelocity.y, moveDir.z);
            animator.SetBool("Walk", false);

            if (Time.time >= lastAttackTime + attackCooldown)
            {
                animator.SetBool("Attack", true);

                lastAttackTime = Time.time;
                isAttacking = false; 
            }
        } else
        {
            if (gameObject.name.Contains("CactusPBR") || gameObject.name.Contains("MushroomAngryPBR"))
            {
                animator.SetBool("Attack", false);
                animator.SetBool("Walk", true);
                animator.SetBool("Idle", false);
                Vector3 moveDir = transform.forward * speed;
                rb.linearVelocity = new Vector3(moveDir.x, rb.linearVelocity.y, moveDir.z);

            } else if (gameObject.name.Contains("PlantShooterAsset"))
            {
                animator.SetBool("Attack", false);
                animator.SetBool("Idle", true);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && !gameObject.name.Contains("PlantShooterAsset"))
        {
            Destroy(collision.gameObject);
            health--;            
        } else if (collision.gameObject.CompareTag("Monster") || collision.gameObject.CompareTag("House"))
        {
            isAttacking = true;
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            animator.SetBool("Walk", false);
        }

        if (health <= 0)
        {
            PlayDeath();
        }
        
    }
    

    void PlayDeath()
    {
        animator.SetBool("Die", true);
        animator.SetBool("Attack", false);
        animator.SetBool("Idle", false);
        if(GetComponent<Collider>()) GetComponent<Collider>().enabled = false; 
        Destroy(gameObject, 3f);
    }
}