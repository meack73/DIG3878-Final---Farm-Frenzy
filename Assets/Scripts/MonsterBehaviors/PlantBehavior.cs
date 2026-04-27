using UnityEngine;
using System.Collections;

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

    [Header("VFX")]
    public GameObject materialObj;

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
            StartCoroutine(DamageVFX());
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

        IEnumerator DamageVFX()
    {
        Renderer rend = materialObj.GetComponent<Renderer>();
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        rend.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor("_BaseColor", Color.red);
        rend.SetPropertyBlock(propertyBlock);

        yield return new WaitForSeconds(0.1f);
        propertyBlock.Clear();
        rend.SetPropertyBlock(propertyBlock);
        

        //rend.materials = mats;
    }
}