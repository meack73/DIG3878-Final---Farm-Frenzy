using UnityEngine;
using System.Collections; 

public class PumpkinBehavior : MonoBehaviour
{
    [Header("Stats")]
    public int health = 3;
    public int playerId;
    public Vector3Int spawnTile;

    [Header("Materials")]
    public Material orange;
    public Material cracked;
    public Material veryCracked;

    private Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        UpdateMaterial();
        if (health <= 0)
            Die();
    }


    private void UpdateMaterial()
    {
        if (health == 3)
            rend.material = orange;
        else if (health == 2)
            rend.material = cracked;
        else
            rend.material = veryCracked;
    }

    private void Die()
    {
        StartCoroutine(DeathSquish());
    }

    private IEnumerator DeathSquish()
    {
        float duration = 0.6f;
        float time = 0f;
        Vector3 originalScale = transform.localScale;
        Vector3 squishScale =  new Vector3(originalScale.x, originalScale.y, 0f); 

        // squish down
        while (time < duration)
        {
            transform.localScale = Vector3.Lerp(originalScale, squishScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}