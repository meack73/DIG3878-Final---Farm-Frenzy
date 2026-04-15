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

    public int playerId = 0;

    [Header("Audio")]
    public AudioClip shoot;
    public AudioClip death;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rb.isKinematic = false;
        rb.freezeRotation = true;
    }

    void Update()
    {
        bool inAttackWindow = Time.time < lastAttackTime + attackCooldown;

        animator.SetBool("Attack", inAttackWindow);
        animator.SetBool("Idle", !inAttackWindow);
    }

    public void PlaySound()
    {
        audioSource.PlayOneShot(shoot);
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
        audioSource.PlayOneShot(death);
        Destroy(gameObject, 2f);
    }
}