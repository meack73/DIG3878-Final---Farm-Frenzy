using UnityEngine;
using UnityEngine.UI;

public class PlayerCurrency : MonoBehaviour
{
    public int SunCoins = 0;
    public Text plyrCurrencyText;

    public int playerCurrency = 0;

    void Start()
    {
        updateCurrencyUI();
    }

    void Update()
    {
        updateCurrencyUI();
    }

    public void AddCoins(int amount)
    {
        SunCoins += amount;
        playerCurrency = SunCoins * 2;
        Debug.Log("Sun Coins: " + SunCoins);
        updateCurrencyUI();
    }

    public void updateCurrencyUI()
    { 
        plyrCurrencyText.text = "Sun Coins: $" + playerCurrency;
    }
}
