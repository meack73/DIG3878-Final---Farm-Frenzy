using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    [HideInInspector] public float p1Health = 100f;
    [HideInInspector] public float p2Health = 100f;
    public GameManager gameManager;
    
    PlayerHealth player1Health;
    PlayerHealth player2Health;

    
    void Start()
    {
        //Finds health of each player
        GameObject player1 = GameObject.FindGameObjectWithTag("Player1");
        player1Health = player1.GetComponent<PlayerHealth>();

        GameObject player2 = GameObject.FindGameObjectWithTag("Player2");
        player2Health = player2.GetComponent<PlayerHealth>();
    }
    
    void Update()
    {
        //Updates each players health
        p1Health = player1Health.playerHealth;
        p2Health = player2Health.playerHealth;

        //Detects when a player dies and ends the game
        if (p1Health <= 0)
        {
            gameManager.GameWin();
            gameManager.winnerNum = 2;
        }
        else if (p2Health <= 0)
        {
            gameManager.GameWin();
            gameManager.winnerNum = 1;
        }

        //Health Debug
        /*if (Input.GetKeyDown(KeyCode.R))
        {
            player1Health.DamagePlayer(5);
        }
        */
    }
}
