using UnityEngine;
using System.Collections; 

public class WalnutBehavior : MonoBehaviour
{
    private Animator animator;    
    private Rigidbody rb;
    
    [Header("Stats")]
    public int health = 3;
    public Vector3 spawnPoint = Vector3Int.zero; 
    public Vector3Int spawnTile = Vector3Int.zero; 

    //private float damageTimer = 0f;
    //private float damageCooldown = 3.0f; // same as monster attack cooldown 

    public int playerId = 0; 
    public bool coinSpawn = false; 
    private bool isDying = false;

    [Header("Audio")]
    public AudioClip death;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.freezeRotation = true;
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
        if (isDying || health <= 0) return;
        health -= damage;

        if (health <= 0)
        {
            isDying = true;
            StartCoroutine(Die());
        } 
    }

    IEnumerator Die()
    {
        float duration = 0.5f;
        float time = 0f;

        Quaternion startRot = transform.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(90, 0, 0);

        while (time < duration)
        {
            transform.rotation = Quaternion.Lerp(startRot, endRot, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRot;
        GetComponent<Collider>().enabled = false;
        audioSource.PlayOneShot(death);
        Destroy(gameObject, 2f);
    }
}