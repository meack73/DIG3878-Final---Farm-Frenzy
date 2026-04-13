using UnityEngine;
using Photon.Pun;

public class MultipPlayerCurrency : MonoBehaviourPunCallbacks, IPunObservable
{

    public int p1SunCoins = 0;
    public int p2SunCoins = 0;

    public int p1Currency = 0;
    public int p2Currency = 0;

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
            p1Currency = p1SunCoins * 2;
            Debug.Log("Player 1 Sun Coins: " + p1SunCoins);
        }
        else if (playerNum == 2)
        {
            p2SunCoins += amount;
            p2Currency = p2SunCoins * 2;
            Debug.Log("Player 2 Sun Coins: " + p2SunCoins);
        }
    }

    public int getCurrency(int playerNum)
    {
        return playerNum == 1 ? p1Currency : p2Currency;
    }

    public int getSunCoins(int playerNum)
    {
        return playerNum == 1 ? p1SunCoins : p2SunCoins;

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(p1SunCoins);
            stream.SendNext(p2SunCoins);
            stream.SendNext(p1Currency);
            stream.SendNext(p2Currency);
        }
        else
        {
            p1SunCoins = (int)stream.ReceiveNext();
            p2SunCoins = (int)stream.ReceiveNext();
            p1Currency = (int)stream.ReceiveNext();
            p2Currency = (int)stream.ReceiveNext();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
