using UnityEngine;

public class MonsterHealth : MonoBehaviour
{
    public float maxHealth = 5f;
    [HideInInspector] public float monsterHealth = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Set health of monster to max health at start
        monsterHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(monsterHealth <= 0)
        {
            MonsterDeath();
        }
    }

    public void MonsterDeath()
    {
        Destroy(gameObject);
        //Spawn currency code can go here as well
    }
}
