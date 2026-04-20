using UnityEngine;
using System.Collections; 

public class PumpkinBehavior : MonoBehaviour
{
    public int health = 3;
    public int playerId;
    public Vector3Int spawnTile;
    
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            Die();
    }

    private void Die()
    {
        // play crack/break effect
        Destroy(gameObject);
    }
}