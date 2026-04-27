using UnityEngine;

public abstract class PlantBehavior : MonoBehaviour
{
    [Header("Stats")]
    public int health = 3;
    public int playerId = 0;
    public Vector3 spawnPoint = Vector3.zero;
    public Vector3Int spawnTile = Vector3Int.zero;

    protected bool isDying = false;

    [Header("Audio")]
    public AudioClip death;
    protected AudioSource audioSource;

    protected virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public virtual void TakeDamage(int damage)
    {
        PumpkinBehavior pumpkin = GetComponentInChildren<PumpkinBehavior>();
        
        if (pumpkin != null)
        {
            pumpkin.TakeDamage(damage); // pumpkin absorbs it, monster health untouched
        }
        else
        {
            health -= damage;
            if (health <= 0)
            {
                isDying = true;
                Die();
            }
        }
    }

    protected abstract void Die();

    protected virtual void OnCollisionEnter(Collision collision)
    {
        string enemyBulletTag = playerId == 1 ? "P2Bullet" : "P1Bullet";
        if (collision.gameObject.CompareTag(enemyBulletTag))
        {
            Destroy(collision.gameObject);
            TakeDamage(1);
        }
    }
}