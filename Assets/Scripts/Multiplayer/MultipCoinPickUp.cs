using UnityEngine;

public class MultipCoinPickUp : MonoBehaviour
{
    [SerializeField] private int coinValue = 1;
    [SerializeField] private MultipPlayerCurrency multipCurrency;

    void Start()
    {
        if (multipCurrency == null)
        {
            multipCurrency = FindObjectOfType<MultipPlayerCurrency>();
        }
    }

    void OnMouseDown()
    {
        if (multipCurrency == null)
        {
            Debug.Log("no currency found");
            return;
        }

        int playerNum = multipCurrency.getPlayerNum();
        multipCurrency.addCoins(playerNum, coinValue);

        Debug.Log("Sun Coin picked up by Player " + playerNum);
        Destroy(gameObject);
    }
}
