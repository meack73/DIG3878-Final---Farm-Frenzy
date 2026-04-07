using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public PlayerHealthManager healthManager;

    public float maxPlayerHealth = 100f;

    [HideInInspector] public float playerHealth = 0.0f;
    [SerializeField] private Image healthBar;

    void Start()
    {
        //Sets the player's health to the max health at Start
         playerHealth = maxPlayerHealth;

    }
      
    // Update is called once per frame
    void Update()
    {

    }

    public void DamagePlayer(float damage)
    {
        //Deducts damage amount from the Player's health
        playerHealth -= damage;

        //Updates the healthbar UI to reflect damage taken
        if (healthBar != null)
        {
            healthBar.fillAmount = playerHealth / maxPlayerHealth;
        }

    }

    /*
    //Example of damage dealing script
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag ("Player1") || collision.gameObject.CompareTag ("Player2"))
        {
            GameObject playerHit = collision.gameObject;
            PlayerHealth health = playerHit.GetComponent<PlayerHealth>();
            health.DamagePlayer(5f);
        }
    }
    */

    //healing from sunflower!
    public void Heal(int amount)
    {
        playerHealth += amount;

        if (playerHealth > maxPlayerHealth)
        {
            playerHealth = maxPlayerHealth;
        }

        Debug.Log("Player healed. Current health: " + playerHealth);
        if (healthBar != null)
        {
            healthBar.fillAmount = playerHealth / maxPlayerHealth;

        }
    }
}
