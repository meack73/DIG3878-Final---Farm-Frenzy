using UnityEngine;

public class CoinPickUp : MonoBehaviour
{
    public int coinValue = 1;

    public AudioClip coinSFX;
    GameObject publicSpeaker;
    AudioSource audioSource;

    void Start()
    {
        publicSpeaker = GameObject.FindWithTag("Speaker");
        audioSource = publicSpeaker.GetComponent<AudioSource>();
    }

    void OnMouseDown()
    {
        PlayerCurrency playerCurrency = FindObjectOfType<PlayerCurrency>();
        if (playerCurrency != null)
        {
            playerCurrency.AddCoins(coinValue);
            Debug.Log("Sun Coin picked up");
            audioSource.PlayOneShot(coinSFX);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Player has no currency");
        }
    }
}
