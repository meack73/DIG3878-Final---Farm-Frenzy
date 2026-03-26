using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public int selectedMonster = 0;
    public GameBoard gameBoard;
    public GameObject[] monsterPrefabs;
    public int[,] monsterLocations;
    
    void Awake()
    {
        gameBoard = GetComponentInParent<GameBoard>();
        if (gameBoard != null) {
            monsterLocations = new int[gameBoard.width, gameBoard.depth];

            for (int x = 0; x < monsterLocations.GetLength(0); x++) {
                for (int z = 0; z < monsterLocations.GetLength(1); z++) {
                    monsterLocations[x, z] = 0; // 0 means empty
                }
            } //1, 2, 3 for different monsters
        } 
        LogMonsterGrid();        
    }

    public void PlaceMonster(int x, int z) 
    {
        if (monsterLocations[x, z] != 0) 
        {
            Debug.Log("Space occupied!");
            return;
        }

        if (selectedMonster < 0 || selectedMonster >= monsterPrefabs.Length) return;

        // We use the board's logic to find the exact center of the tile
        float startX = -((gameBoard.width - 1) * gameBoard.tileSize) / 2f;
        float startZ = -((gameBoard.depth - 1) * gameBoard.tileSize) / 2f;
        
        Vector3 localPos = new Vector3(startX + (x * gameBoard.tileSize), 0.5f, startZ + (z * gameBoard.tileSize));
        Vector3 worldPos = transform.TransformPoint(localPos);

        // 4. Spawn and Record
        GameObject newMonster = Instantiate(monsterPrefabs[selectedMonster], worldPos, transform.rotation);
        monsterLocations[x, z] = selectedMonster + 1; // Store ID (offset by 1 so 0 stays 'Empty')
        
        Debug.Log($"Placed monster {selectedMonster} at {x}, {z}");

        if (selectedMonster == 0)
        {
            newMonster.transform.Rotate(0, 180, 0);
        } else
        {
            newMonster.transform.Rotate(0, 90, 0);
        }
        LogMonsterGrid();
    }

    public void LogMonsterGrid()
    {
        string visualGrid = "Board State:\n";
        
        for (int z = gameBoard.depth - 1; z >= 0; z--) 
        {
            for (int x = 0; x < gameBoard.width; x++)
            {
                visualGrid += monsterLocations[x, z] + " ";
            }
            visualGrid += "\n"; 
        }
        
        Debug.Log(visualGrid);
    }
    }

