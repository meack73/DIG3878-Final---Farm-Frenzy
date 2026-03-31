using UnityEngine;

public class MonsterBehavior : MonoBehaviour
{
    private Animator animator;
    public int health = 3;
    public float attackCooldown = 1.0f;   // 1 second between hits
    public float lastAttackTime = 0f;
    public bool isAttacking = false;
    public Vector3 spawnPoint = Vector3Int.zero; //clear init board tile when walking
    public Vector3Int spawnTile = Vector3Int.zero; //store spawn tile for pathfinding
    public int playerId = 0; 

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Idle", true);
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
                isAttacking = false; 
            } else
            {
                isAttacking = false;
            }
        } else
        {
            animator.SetBool("Attack", false);
            animator.SetBool("Idle", true);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Collider"))
        {
            Destroy(collision.gameObject);
            health--;

            if (health <= 0)
            {
                PlayDeath();
            }
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