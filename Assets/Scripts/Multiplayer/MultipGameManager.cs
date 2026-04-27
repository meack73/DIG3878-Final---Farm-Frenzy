using UnityEngine;
using Photon.Pun;
using TMPro;

public class MultipGameManager : MonoBehaviourPunCallbacks
{

    public GameObject winScreen;
    public GameObject playerText;

    public AudioSource speaker;
    public AudioClip victorySFX;

    [HideInInspector] public int winnerNum = 0;

    private bool gameEnded = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (winScreen != null)
        {
            winScreen.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Winner(int winPlrNum)
    {
        if (gameEnded)
        {
            return;
        }

        photonView.RPC(nameof(RPC_Winner), RpcTarget.All, winPlrNum);
    }

    [PunRPC]
    private void RPC_Winner(int winPlrNum)
    {
        gameEnded = true;
        winnerNum = winPlrNum;

        if (winScreen != null)
        {
            winScreen.SetActive(true);
            speaker.PlayOneShot(victorySFX);
        }

        if (playerText != null)
        {
            TextMeshProUGUI nameText = playerText.GetComponent<TextMeshProUGUI>();
            if (winnerNum == 1)
            {
                nameText.text = "Player 1 Wins";
            }
            else if (winnerNum == 2)
            {
                nameText.text = "Player 2 Wins";
            }
        }

    }

    public bool hasGameEnded()
    {
        return gameEnded;
    }
}
