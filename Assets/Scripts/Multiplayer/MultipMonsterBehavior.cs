using UnityEngine;

public class MultipMonsterBehavior : PlantBehavior
{
    private MultipPlayerHealthManager healthManager;

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

    private bool attackingHouse = false;
    private int targetPlayerNum = 0;
    private string targetHouseTag = "Player1";

    [Header("Audio")]
    public AudioClip attack;

    protected override void Start()
    {
        base.Start();

        healthManager = FindObjectOfType<MultipPlayerHealthManager>();

        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("Idle", true);
        }

        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.freezeRotation = true;
        }
    }

    public void InitMonster(int ownerPlayerId)
    {
        playerId = ownerPlayerId;

        targetPlayerNum = playerId == 1 ? 2 : 1;
        targetHouseTag = targetPlayerNum == 1 ? "Player1" : "Player2";

        healthManager = FindObjectOfType<MultipPlayerHealthManager>();

        Debug.Log("INIT MULTIP MONSTER: Owner Player " + playerId +
                  " attacking Player " + targetPlayerNum +
                  " base with tag " + targetHouseTag);
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
            lastAttackTime = Time.time;

            if (audioSource != null && attack != null)
            {
                audioSource.PlayOneShot(attack);
            }

            if (currentTarget != null)
            {
                currentTarget.TakeDamage(1);
            }
            else if (attackingHouse)
            {
                if (healthManager != null)
                {
                    Debug.Log("MultipMonster damaging Player " + targetPlayerNum);
                    healthManager.damagePlayer(targetPlayerNum, 1f);
                }
                else
                {
                    Debug.LogWarning("No MultipPlayerHealthManager found.");
                }
            }

        }

        else if (Time.time >= lastAttackTime + 0.5f) // Reset attack animation halfway through cooldown
        {
            SetAnimationState("Idle");
        }
    }

    void StopAttack()
    {
        currentTarget = null;
        attackingHouse = false;
        isAttacking = false;
        lastStopAttackTime = Time.time;
    }

    // ------------------------------- Collisions -------------------------------------------- //

    void OnCollisionStay(Collision collision)
    {
        PlantBehavior plant = collision.gameObject.GetComponent<PlantBehavior>();

        if (plant != null && plant.playerId != playerId && plant.health > 0)
        {
            isAttacking = true;
            currentTarget = plant;
            attackingHouse = false;
        }
        else if (collision.gameObject.CompareTag(targetHouseTag))
        {
            isAttacking = true;
            attackingHouse = true;
            currentTarget = null;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        PlantBehavior plant = collision.gameObject.GetComponent<PlantBehavior>();

        if (plant != null && plant == currentTarget)
        {
            StopAttack();
        }

        if (collision.gameObject.CompareTag(targetHouseTag))
        {
            StopAttack();
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
