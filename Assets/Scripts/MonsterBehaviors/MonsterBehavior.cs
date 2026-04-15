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
    public float attackCooldown = 2.0f;  
    public float lastAttackTime = 0f;
    public bool isAttacking = false;

    private float postAttackDelay = 2.0f;
    private float lastStopAttackTime = -999f;

    public Vector3 spawnPoint = Vector3Int.zero; //clear init board tile when walking
    public Vector3Int spawnTile = Vector3Int.zero; //store spawn tile for pathfinding
    private MonsterBehavior currentTarget; //current plant target 
    private FlowerBehavior currentFlower;
    private ShooterBehavior currentShooter; 
    private WalnutBehavior currentWalnut; 

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

        if (!isAttacking && Time.time < lastStopAttackTime + postAttackDelay)
        {
            rb.linearVelocity = new Vector3(0, 0, 0);

            animator.SetBool("Walk", false);
            animator.SetBool("Attack", false);
            animator.SetBool("Idle", true);

            return;
        }

        if (isAttacking)
        {
            
            rb.linearVelocity = Vector3.zero;
            animator.SetBool( "Walk", false);
            animator.SetBool("Idle", false);

            if (currentTarget != null && currentTarget.health <= 0)
            {
                currentTarget = null;
                isAttacking = false;
                lastStopAttackTime = Time.time;
                return;
            }

            else if (currentFlower != null && currentFlower.health <= 0)
            {
                currentFlower = null;
                isAttacking = false;
                lastStopAttackTime = Time.time;
                return;
            }

            else if (currentShooter != null && currentShooter.health <= 0)
            {
                currentShooter = null;
                isAttacking = false;
                lastStopAttackTime = Time.time;
                return;
            }
                        
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                animator.SetBool("Attack", true);
                lastAttackTime = Time.time;

                if (currentTarget != null)
                    currentTarget.TakeDamage(1);
                else if (currentFlower != null)
                    currentFlower.TakeDamage(1);
                else if (currentShooter != null)
                    currentShooter.TakeDamage(1);
                else if (currentWalnut != null)
                    currentWalnut.TakeDamage(1);
                else if (targetHouse != null)
                    targetHouse.DamagePlayer(1f);

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
        rb.linearVelocity = new Vector3(moveDir.x, 0, moveDir.z);

        animator.SetBool("Walk", true);
        animator.SetBool("Attack", false);
        animator.SetBool("Idle", false); 
    }

    void OnCollisionStay(Collision collision)
    {   
        MonsterBehavior monster = collision.gameObject.GetComponent<MonsterBehavior>();
        FlowerBehavior flower = collision.gameObject.GetComponent<FlowerBehavior>();
        ShooterBehavior shooter = collision.gameObject.GetComponent<ShooterBehavior>();
        WalnutBehavior walnut = collision.gameObject.GetComponent<WalnutBehavior>(); 
        
        if (monster != null && monster.playerId != playerId && monster.health > 0)
        {
            isAttacking = true;
            currentTarget = monster;
        }
        else if (flower != null && flower.playerId != playerId && flower.health > 0)
        {
            isAttacking = true;
            currentFlower = flower;
        }
        else if (shooter != null && shooter.playerId != playerId && shooter.health > 0)
        {
            isAttacking = true;
            currentShooter = shooter;
        }
        else if (walnut != null && walnut.playerId != playerId && walnut.health > 0)
        {
            isAttacking = true;
            currentWalnut = walnut;
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
            MonsterBehavior other = collision.gameObject.GetComponent<MonsterBehavior>();
            FlowerBehavior flower = collision.gameObject.GetComponent<FlowerBehavior>();
            ShooterBehavior shooter = collision.gameObject.GetComponent<ShooterBehavior>();
            WalnutBehavior walnut = collision.gameObject.GetComponent<WalnutBehavior>();

            if ((other != null && other.health <= 0) || (walnut != null && walnut.health <= 0) || (flower != null && flower.health <= 0) || (shooter != null && shooter.health <= 0))
            {
                lastStopAttackTime = Time.time;
            }

            isAttacking = false;
            currentTarget = null;
            currentFlower = null;
            currentShooter = null;
            currentWalnut = null;
            targetHouse = null;
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