using UnityEngine;
using Photon.Pun;

public class MultipCoinPickUp : MonoBehaviourPun
{

    public int coinValue = 1;
    public int ownerPlayerId = 0; // The player ID that owns this coin

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

    void OnMouseDown()
    {
        Debug.Log("COIN CLICKED");
        CollectCoin();
    }

    public void SetOwner(int playerId)
    {
        ownerPlayerId = playerId;
    }

    public void CollectCoin()
    {

        if (playerCurrency == null)
        {
            playerCurrency = FindObjectOfType<MultipPlayerCurrency>();
        }

        if (playerCurrency != null)
        {
            int localPlayerNum = PhotonNetwork.LocalPlayer.ActorNumber == 1 ? 1 : 2;

            if (ownerPlayerId != 0 && ownerPlayerId != localPlayerNum)
            {
                Debug.Log("This coin belongs to another player!");
                return;
            }

            photonView.RPC(nameof(RPC_CollectCoin), RpcTarget.All, localPlayerNum);
        }
        else
        {
            Debug.Log("Player has no currency manager");
            return;
        }
    }

    [PunRPC]
    void RPC_CollectCoin(int playerNum)
    {
        if (playerCurrency == null)
        {
            playerCurrency = FindObjectOfType<MultipPlayerCurrency>();
        }

        if (playerCurrency != null && PhotonNetwork.LocalPlayer.ActorNumber == playerNum)
        {
            playerCurrency.addCoins(playerNum, coinValue);
            Debug.Log("Player " + playerNum + " collected a coin worth " + coinValue + " currency.");
        }

        if (audioSource != null && coinSFX != null)
        {
            audioSource.PlayOneShot(coinSFX);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
        else
        {
            photonView.RPC(nameof(RPC_RequestDestroy), RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    void RPC_RequestDestroy()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

}
