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
        // play crack/break effect
        Destroy(gameObject);
    }
}