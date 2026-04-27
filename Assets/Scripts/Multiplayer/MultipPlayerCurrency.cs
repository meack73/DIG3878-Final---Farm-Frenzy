using UnityEngine;
using Photon.Pun;
using System.Xml.Serialization;
using UnityEngine.UI;  

public class MultipPlayerCurrency : MonoBehaviourPunCallbacks, IPunObservable
{

    public int p1SunCoins = 0;
    public int p2SunCoins = 0;

    public Text currencyText;

    public void addCoins(int playerNum, int amount)
    {
        photonView.RPC(nameof(RPC_addCoins), RpcTarget.All, playerNum, amount);
    }

    [PunRPC]
    private void RPC_addCoins(int playerNum, int amount)
    {
        if (playerNum == 1)
        {
            p1SunCoins += amount;
            Debug.Log("Player 1 Sun Coins: " + p1SunCoins);
        }
        else if (playerNum == 2)
        {
            p2SunCoins += amount;
            Debug.Log("Player 2 Sun Coins: " + p2SunCoins);
        }

        if (PhotonNetwork.LocalPlayer.ActorNumber == playerNum)
        {
            UpdateCurrencyUI();
        }
    }

    public int getPlayerNum()
    {
        return PhotonNetwork.LocalPlayer.ActorNumber == 1 ? 1 : 2;
    }

    public bool canAfford(int playerNum, int cost)
    {
        return getCurrency(playerNum) >= cost;
    }

    public bool spendCoins(int playerNum, int cost)
    {
        if (!canAfford(playerNum, cost))
        {
            Debug.Log("Player " + playerNum + " cannot afford this purchase.");
            return false;
        }

        photonView.RPC(nameof(RPC_spendCoins), RpcTarget.All, playerNum, cost);
        return true;
    }

    [PunRPC]
    private void RPC_spendCoins(int playerNum, int cost)
    {
        if (playerNum == 1)
        {
            p1SunCoins -= cost;
            if (p1SunCoins < 0)
            {
                p1SunCoins = 0;
            }

            Debug.Log("Player 1 spent " + cost + " currency. Remaining currency: " + p1SunCoins);
        }
        else if (playerNum == 2)
        {
            p2SunCoins -= cost;
            if (p2SunCoins < 0)
            {
                p2SunCoins = 0;
            }

            Debug.Log("Player 2 spent " + cost + " currency. Remaining currency: " + p2SunCoins);
        }

        if (PhotonNetwork.LocalPlayer.ActorNumber == playerNum)
        {
            UpdateCurrencyUI();
        }
    }

    public int getCurrency(int playerNum)
    {
        return playerNum == 1 ? p1SunCoins : p2SunCoins;
    }

    public int getSunCoins(int playerNum)
    {
        return playerNum == 1 ? p1SunCoins : p2SunCoins;
    }

    public int getLocalSunCoins()
    {
        return getSunCoins(getPlayerNum());
    }

    public int getLocalCurrency()
    {
        return getCurrency(getPlayerNum());
    }

    public void UpdateCurrencyUI()
    {
        if (currencyText == null)
        {
            return;
        }

        int localPlayer = PhotonNetwork.LocalPlayer.ActorNumber == 1 ? 1 : 2;
        int coins = (localPlayer == 1) ? p1SunCoins : p2SunCoins;

        currencyText.text = coins.ToString();       
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(p1SunCoins);
            stream.SendNext(p2SunCoins);
        }
        else
        {
            p1SunCoins = (int)stream.ReceiveNext();
            p2SunCoins = (int)stream.ReceiveNext();
            UpdateCurrencyUI();
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateCurrencyUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
