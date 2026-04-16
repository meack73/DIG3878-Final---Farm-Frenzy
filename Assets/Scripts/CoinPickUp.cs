using UnityEngine;

public class CoinPickUp : MonoBehaviour
{
    public int coinValue = 1;

    public AudioClip coinSFX;
    GameObject publicSpeaker;
    AudioSource audioSource;
    PlayerCurrency playerCurrency;

    void Start()
    {
        publicSpeaker = GameObject.FindWithTag("Speaker");
        audioSource = publicSpeaker.GetComponent<AudioSource>();
        playerCurrency = FindObjectOfType<PlayerCurrency>();
    }

    public void CollectCoin(GameObject coin)
    {
        if (playerCurrency != null)
        {
            playerCurrency.AddCoins(coinValue);
            Debug.Log("Sun Coin picked up");
            coin.SetActive(false);
            audioSource.PlayOneShot(coinSFX);
            Destroy(coin, 0.5f);
        }
        else
        {
            Debug.Log("Player has no currency");
        }
    }
}
