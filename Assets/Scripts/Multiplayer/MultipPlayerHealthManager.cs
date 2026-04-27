using UnityEngine;
using Photon.Pun;

public class MultipPlayerHealthManager : MonoBehaviourPunCallbacks, IPunObservable
{

    [SerializeField] private float maxPlayerHealth = 100f;
    [SerializeField] private MultipGameManager gameManager;

    public float p1Health;
    public float p2Health;

    bool gameEnded = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        p1Health = maxPlayerHealth;
        p2Health = maxPlayerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void checkForWinner()
    {
        if (gameEnded)
        {
            return;
        }

        if (p1Health <= 0f)
        {
            gameEnded = true;

            if (gameManager != null)
            {
                gameManager.winnerNum = 2;
                gameManager.Winner(2);
            }
        }
        else if (p2Health <= 0f)
        {
            gameEnded = true;

            if (gameManager != null)
            {
                gameManager.winnerNum = 1;
                gameManager.Winner(1);
            }
        }
    }

    //damage & heal using RPC to sync across all players
    [PunRPC]
    private void RPC_ApplyDamage(int playerNum, float damage)
    {
        if (gameEnded)
        {
            return;
        }

        if (playerNum == 1)
            p1Health = Mathf.Max(0, p1Health - damage);
        else
            p2Health = Mathf.Max(0, p2Health - damage);


        if (p1Health < 0)
        {
            p1Health = 0;
            gameEnded = true;
        }

        if (p2Health < 0)
        {
            p2Health = 0;
            gameEnded = true;
        }

        checkForWinner();
        Debug.Log("RPC_ApplyDamage DONE: P1=" + p1Health + " P2=" + p2Health);
    }

    [PunRPC]
    private void RPC_ApplyHeal(int playerNum, float healAmount)
    {
        if (gameEnded)
        {
            return;
        }

        if (playerNum == 1)
            p1Health = Mathf.Min(maxPlayerHealth, p1Health + healAmount);
        else
            p2Health = Mathf.Min(maxPlayerHealth, p2Health + healAmount);


        if (p1Health > maxPlayerHealth)
        {
            p1Health = maxPlayerHealth;
        }

        if (p2Health > maxPlayerHealth)
        {
            p2Health = maxPlayerHealth;
        }

    }

    public void damagePlayer(int playerNum, float damage)
    {
        if (playerNum != 1 && playerNum != 2)
        {
            Debug.Log("Invalid player");
            return;
        }

        Debug.Log("damagePlayer CALLED: player=" + playerNum + " damage=" + damage);

        //RPC syncs the damage applied for all players
        photonView.RPC(nameof(RPC_ApplyDamage), RpcTarget.All, playerNum, damage);

    }

    public void healPlayer(int playerNum, float healAmount)
    {
        if (playerNum != 1 && playerNum != 2)
        {
            Debug.Log("Invalid player");
            return;
        }

        photonView.RPC(nameof(RPC_ApplyHeal), RpcTarget.All, playerNum, healAmount);
    }

    public float getHealth(int playerNum)
    {
        if (playerNum == 1)
        {
            return p1Health;
        }
        else if (playerNum == 2)
        {
            return p2Health;
        }

        Debug.Log("Invalid player");
        return 0;
    }

    public float getMaxHealth()
    {
        return maxPlayerHealth;
    }

    //check if game has ended & updates health
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo information)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(p1Health);
            stream.SendNext(p2Health);
            stream.SendNext(gameEnded);
        }
        else
        {
            p1Health = (float)stream.ReceiveNext();
            p2Health = (float)stream.ReceiveNext();
            gameEnded = (bool)stream.ReceiveNext();
        }
    }
}
