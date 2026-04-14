using UnityEngine;
using UnityEngine.UI;

public class LocalCurrencyUI : MonoBehaviour
{

    [SerializeField] private MultipPlayerCurrency multipCurrency;
    [SerializeField] private Text currencyText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (multipCurrency == null)
        {
            multipCurrency = FindObjectOfType<MultipPlayerCurrency>();
        }

        if (currencyText == null)
        {
            Debug.Log("Currency Text component is not assigned in the inspector.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (multipCurrency == null || currencyText == null)
        {
            return;
        }

        currencyText.text = "Currency: " + multipCurrency.getCurrency(multipCurrency.getPlayerNum());
    }
}
