using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject winScreen;
    public GameObject playerText;

    [HideInInspector] public int winnerNum = 0;

    private bool gameEnded = false;

    void Start()
    {
        if (winScreen != null)
        {
            winScreen.SetActive(false);
        }
    }

    public void Winner(int winPlrNum)
    {
        if (gameEnded)
        {
            return;
        }

        gameEnded = true;
        winnerNum = winPlrNum;

        if (winScreen != null)
        {
            winScreen.SetActive(true);
        }


        if (winnerNum == 1)
        {
            TextMeshProUGUI nameText = playerText.GetComponent<TextMeshProUGUI>();
            nameText.text = "Player 1 Wins";
        }
        else if (winnerNum == 2)
        {
            TextMeshProUGUI nameText = playerText.GetComponent<TextMeshProUGUI>();
            nameText.text = "Player 2 Wins";
        }

    }

    
    public void GameWin()
    {
        winScreen.SetActive(true);

        if (winnerNum == 1)
        {
            TextMeshProUGUI nameText = playerText.GetComponent<TextMeshProUGUI>();
            nameText.text = "Player 1 Wins";
        }
        else if (winnerNum == 2)
        {
            TextMeshProUGUI nameText = playerText.GetComponent<TextMeshProUGUI>();
            nameText.text = "Player 2 Wins";
        }
    }
    
}
