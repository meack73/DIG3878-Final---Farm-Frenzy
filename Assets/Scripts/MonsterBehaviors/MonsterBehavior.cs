using UnityEngine; 

public class MonsterBehavior : PlantBehavior
{
    private Animator animator;
    private Rigidbody rb;
 
    [Header("Stats")]
    private float speed = 1.5f;
 
    [Header("Combat Settings")]
    public float attackCooldown = 2.0f;
    public float lastAttackTime = 0f;
    public bool isAttacking = false;
 
    private float postAttackDelay = 2.0f;
    private float lastStopAttackTime = -999f;
 
    private PlantBehavior currentTarget;     // Single reference covers all plant types
    private MultipPlayerHealth targetHouse = null;
    private string targetHouseTag = "Player1";
    
    [Header("Audio")]
    public AudioClip attack;

    protected override void Start()
    {        
        base.Start(); //runs parent class plant behavior (audiosource)
        animator = GetComponent<Animator>();
        animator.SetBool("Idle", true);
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.freezeRotation = true;

        targetHouseTag = playerId == 1 ? "Player1" : "Player2";
    }

    void Update()
    {
        if (health <= 0) return;

        if (!isAttacking && Time.time < lastStopAttackTime + postAttackDelay)
        {
            IdleState();
            return;
        }

        if (isAttacking)
        {
            AttackState();
  
        } 
        else
        {
            MovementState();
        }
    
    }

// ------------------------------- Set states -------------------------------------------- //
    void SetAnimationState(string state)
    {
        animator.SetBool("Walk", state == "Walk");
        animator.SetBool("Idle", state == "Idle");
        animator.SetBool("Attack", state == "Attack");
        animator.SetBool("Die", state == "Die");
    }

    void IdleState()
    {
        rb.linearVelocity = new Vector3(0, 0, 0);
        SetAnimationState("Idle");
    }

    void MovementState()
    {
        Vector3 moveDir = transform.forward * speed;
        rb.linearVelocity = new Vector3(moveDir.x, 0, moveDir.z);
        SetAnimationState("Walk");
    }

    void AttackState()
    {
            rb.linearVelocity = Vector3.zero;
            SetAnimationState("Attack");

            if (currentTarget != null && currentTarget.health <= 0)
            {
                StopAttack();
                return;
            }

           
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                SetAnimationState("Attack");
                audioSource.PlayOneShot(attack);

                lastAttackTime = Time.time;

                if (currentTarget != null)
                    currentTarget.TakeDamage(1);
                else if (targetHouse != null)
                    targetHouse.damagePlayer(10f);

            }

            else if (Time.time >= lastAttackTime + 0.5f) // Reset attack animation halfway through cooldown
            {
                SetAnimationState("Idle");
            }
    }
    
    void StopAttack()
    {
        currentTarget = null;
        targetHouse = null;
        isAttacking = false;
        lastStopAttackTime = Time.time;
    }

// ------------------------------- Collisions -------------------------------------------- //

    void OnCollisionStay(Collision collision)
    {   
        PlantBehavior plant = collision.gameObject.GetComponent<PlantBehavior>();
        
        if (plant == null)
            plant = collision.gameObject.GetComponentInParent<PlantBehavior>();
        
        if (plant != null && plant.playerId != playerId && plant.health > 0)
        {
            isAttacking = true;
            currentTarget = plant;
        }
        else if (collision.gameObject.CompareTag(targetHouseTag))
        {
            isAttacking = true;
            targetHouse = collision.gameObject.GetComponent<MultipPlayerHealth>();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Monster") || collision.gameObject.CompareTag(targetHouseTag))
        {
            PlantBehavior plant = collision.gameObject.GetComponent<PlantBehavior>();

            if (plant != null && plant.health <= 0)
            {
                lastStopAttackTime = Time.time;
            }

            isAttacking = false;
            currentTarget = null;
            targetHouse = null;
        }
    }

    protected override void Die()
    {
        SetAnimationState("Die");
        GetComponent<Collider>().enabled = false;
        rb.isKinematic = true;
        audioSource.PlayOneShot(death);
        Destroy(gameObject, 2f);
    }
}