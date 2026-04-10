using UnityEngine;

public class SpawnOpponentMonsters : MonoBehaviour
{
    public MonsterSpawner monsterSpawner;
    public GameBoard gameBoard;

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
                if (gameBoard.monsterLocations[x, z] != 0) // If there's a monster here
                {
                    monsterSpawner.selectedMonster = gameBoard.monsterLocations[x, z] - 1; // Get monster ID (adjusting for offset)
                    monsterSpawner.PlaceMonster(x, z); // Place monster at specific tile
                }
            }
        }
    }

}
