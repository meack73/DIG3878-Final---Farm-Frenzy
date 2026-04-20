using UnityEngine;

public class ShooterBehavior : PlantBehavior
{
    private Animator animator;    
    private Rigidbody rb;

    [Header("Combat Settings")]
    public float attackCooldown = 3.0f;   // 1 second between hits
    public float lastAttackTime = 0f;
    public bool isAttacking = false;

    [Header("Audio")]
    public AudioClip shoot;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.freezeRotation = true;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        bool inAttackWindow = Time.time < lastAttackTime + attackCooldown;

        animator.SetBool("Attack", inAttackWindow);
        animator.SetBool("Idle", !inAttackWindow);
    }

// ------------------------------- Set states -------------------------------------------- //
    void SetAnimationState(string state)
    {
        animator.SetBool("Idle", state == "Idle");
        animator.SetBool("Attack", state == "Attack");
        animator.SetBool("Die", state == "Die");
    }

    public void PlaySound()
    {
        audioSource.PlayOneShot(shoot);
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