using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject winScreen;
    public GameObject playerText;
    public int playerId = 1; 

    [HideInInspector] public int winnerNum = 0;

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
