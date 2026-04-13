using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class MultipPlayerHealth : MonoBehaviour
{

    [SerializeField] private MultipPlayerHealthManager healthManager;
    [SerializeField] private int playerNum = 1;
    [SerializeField] private Image healthBar;

    private bool isPlayerOne;
    public float playerHealth = 0;
    public float maxPlayerHealth = 100f;

    private void updateHealthBar()
    {
        healthBar.fillAmount = playerHealth / maxPlayerHealth;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxPlayerHealth = healthManager.getMaxHealth();
        playerHealth = healthManager.getHealth(playerNum);
        updateHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        playerHealth = healthManager.getHealth(playerNum);
        updateHealthBar();
    }

    public void damagePlayer(float damage)
    {
        healthManager.damagePlayer(playerNum, damage);
    }

    public void healPLayer(int healAmount)
    {
        healthManager.healPlayer(playerNum, healAmount);
        Debug.Log("Player healed - Current health: " + healthManager.getHealth(playerNum));
    }

}
