using UnityEngine;
using Photon.Pun;

public class MultipCoinPickUp : MonoBehaviour
{

    public int coinValue = 1;

    public AudioClip coinSFX;
    GameObject publicSpeaker;
    AudioSource audioSource;
    public MultipPlayerCurrency playerCurrency;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        publicSpeaker = GameObject.FindGameObjectWithTag("Speaker");
        if (publicSpeaker != null)
        {
            audioSource = publicSpeaker.GetComponent<AudioSource>();
        }
        
        if (playerCurrency == null)
        {
            playerCurrency = FindObjectOfType<MultipPlayerCurrency>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CollectCoin(GameObject coin)
    {
        if (playerCurrency != null)
        {
            int localPlayerNum = PhotonNetwork.LocalPlayer.ActorNumber == 1 ? 1 : 2;
            playerCurrency.addCoins(localPlayerNum, coinValue);

            Debug.Log("Sun Coin picked up by Player " + localPlayerNum);

            coin.SetActive(false);

            if(audioSource != null && coinSFX != null)
            {
                audioSource.PlayOneShot(coinSFX);
            }

            Destroy(coin, 0.5f);
        }
        else
        {
            Debug.Log("Player has no currency manager");
        }
    }


}
