using UnityEngine;
using UnityEngine.UI;

public class PlayerCurrency : MonoBehaviour
{
    public int SunCoins = 0;
    public Text plyrCurrencyText;
    public int playerCurrency;

    public void AddCoins(int amount)
    {
        SunCoins += amount;
        playerCurrency = SunCoins * 2;
        Debug.Log("Sun Coins: " + SunCoins);
        plyrCurrencyText.text = playerCurrency.ToString();

    }

    public void DeductPrice (int price)
    {
        playerCurrency -= price;
        plyrCurrencyText.text = playerCurrency.ToString();
    }
}
