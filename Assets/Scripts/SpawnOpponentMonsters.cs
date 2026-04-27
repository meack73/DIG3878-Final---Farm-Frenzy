using UnityEngine;

public class SpawnOpponentMonsters : MonoBehaviour
{
    public MonsterSpawner monsterSpawner;
    public GameBoard gameBoard;

    public int[,] monsterLocations = new int[,]
                {
                    {0, 0, 1, 1, 0},
                    {0, 0, 0, 0, 5},
                    {0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0},
                    {3, 0, 4, 0, 0},
                    {0, 0, 0, 5, 0},
                    {0, 0, 0, 0, 0},
                    {6, 4, 3, 2, 1}
                };
    void Start()
    {
        monsterSpawner = GetComponent<MonsterSpawner>();
        gameBoard = GetComponentInParent<GameBoard>();
        
        if (gameBoard != null && monsterSpawner != null) {
            SpawnInitialMonsters();
        }
    }

    public void SpawnInitialMonsters()
    {
        for (int x = 0; x < gameBoard.width; x++)
        {
            for (int z = 0; z < gameBoard.depth; z++)
            {
                if (monsterLocations[x, z] != 0) // If there's a monster here
                {
                    monsterSpawner.selectedMonster = monsterLocations[x, z] - 1; // Get monster ID (adjusting for offset)
                    monsterSpawner.PlaceMonster(x, z); // Place monster at specific tile
                }
            }
        }
    }

}
