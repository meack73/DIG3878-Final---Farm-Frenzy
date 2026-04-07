using UnityEngine; 

public class MonsterBehavior : MonoBehaviour
{
    private Animator animator;    
    private Rigidbody rb;

    [Header("Stats")]
    public int health = 3;
    public int playerId = 0;
    private float speed = 1.5f;

    [Header("Combat Settings")]
    public float attackCooldown = 2.0f;   // 1 second between hits
    public float lastAttackTime = 0f;
    public bool isAttacking = false;
    public Vector3 spawnPoint = Vector3Int.zero; //clear init board tile when walking
    public Vector3Int spawnTile = Vector3Int.zero; //store spawn tile for pathfinding
    private MonsterBehavior currentTarget; //current plant target 

    private string TargetHouse = "Player1";
    private PlayerHealth targetHouse = null;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rb.isKinematic = false;
        rb.freezeRotation = true;

        if (playerId == 1)
        {
            TargetHouse = "Player1";
        }
        else
        {
            TargetHouse = "Player2";
        }   
    }

    void Update()
    {
        if (health <= 0)
        {
            return;
        }

        if (isAttacking)
        {
            
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            animator.SetBool("Walk", false);

            if (currentTarget != null && currentTarget.health <= 0)
            {
                currentTarget = null;
                isAttacking = false;
                return;
            }
            
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                animator.SetBool("Attack", true);
                lastAttackTime = Time.time;

                if (currentTarget != null)
                {
                    currentTarget.TakeDamage(1);
                }
                else if (targetHouse != null)
                {
                    Debug.Log("House health" + targetHouse.playerHealth);
                    targetHouse.DamagePlayer(1f);
                }
            }

            else if (Time.time >= lastAttackTime + 0.5f) // Reset attack animation halfway through cooldown
            {
                animator.SetBool("Attack", false);
            }
        } 
        else
        {
            HandleMovement();
        }
    
    }
    void HandleMovement()
    {
        Vector3 moveDir = transform.forward * speed;
        rb.linearVelocity = new Vector3(moveDir.x, rb.linearVelocity.y, moveDir.z);
        animator.SetBool("Walk", true);
        animator.SetBool("Attack", false);
    }

    void OnCollisionStay(Collision collision)
    {            
        if (collision.gameObject.CompareTag("Monster"))
        {
            MonsterBehavior other = collision.gameObject.GetComponent<MonsterBehavior>();
            
            if (other != null && other.playerId != playerId && other.health > 0)
            {
                isAttacking = true;
                currentTarget = other; 
            }
        }

        else if (collision.gameObject.CompareTag(TargetHouse))
        {
            isAttacking = true;
            targetHouse = collision.gameObject.GetComponent<PlayerHealth>();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Monster") || collision.gameObject.CompareTag(TargetHouse))
        {
            isAttacking = false;
            currentTarget = null;
            targetHouse = null;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            TakeDamage(1);
        }
    }

    void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(gameObject.name + " Health: " + health);

        if (health <= 0)
        {
            PlayDeath();
        }
    }

    void PlayDeath()
    {
        animator.SetBool("Die", true);
        GetComponent<Collider>().enabled = false;
        rb.isKinematic = true;
        Destroy(gameObject, 5f);
    }
}