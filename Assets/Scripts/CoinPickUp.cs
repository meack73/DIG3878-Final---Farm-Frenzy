using UnityEngine;

public class CoinPickUp : MonoBehaviour
{
    public int coinValue = 1;

    void OnMouseDown()
    {
        PlayerCurrency playerCurrency = FindObjectOfType<PlayerCurrency>();
        if (playerCurrency != null)
        {
            playerCurrency.AddCoins(coinValue);
            Debug.Log("Sun Coin picked up");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Player has no currency");
        }
    }
}
