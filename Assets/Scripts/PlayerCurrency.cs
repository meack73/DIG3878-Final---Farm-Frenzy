using UnityEngine;
using UnityEngine.UI;

public class PlayerCurrency : MonoBehaviour
{
    public int SunCoins = 0;
    public Text plyrCurrency;

    public void AddCoins(int amount)
    {
        SunCoins += amount;
        Debug.Log("Sun Coins: " + SunCoins);
        plyrCurrency.text = "Sun Coins: $" + SunCoins * 2;

    }
}
