using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public int selectedMonster = 0;
    public GameBoard gameBoard;
    public GameObject[] monsterPrefabs;
    private int playerId = 0;

    void Start()
    {    
        gameBoard = GetComponentInParent<GameBoard>();

        playerId = gameBoard.playerId;     
    }

    public void PlaceMonster(int x, int z) 
    {
        //Maybe remove
        if (playerId == 1 && gameBoard.monsterLocations[x, z] != 0) 
        {
            Debug.Log("Space occupied!");
            return;
        }

        if (selectedMonster < 0 || selectedMonster >= monsterPrefabs.Length) return;
        
        LogMonsterGrid();
        // We use the board's logic to find the exact center of the tile
        float startX = -((gameBoard.width - 1) * gameBoard.tileSize) / 2f;
        float startZ = -((gameBoard.depth - 1) * gameBoard.tileSize) / 2f;

        float localX = startX + (x * gameBoard.tileSize) - (gameBoard.tileSize / 2f);
        float localZ = startZ + (z * gameBoard.tileSize) + (gameBoard.tileSize / 2f);
        Vector3 localPos = new Vector3(localX, 0.5f, localZ);
        Vector3 worldPos = transform.TransformPoint(localPos);
        
        GameObject newMonster = Instantiate(monsterPrefabs[selectedMonster], worldPos, transform.rotation);        
        MonsterBehavior behavior = newMonster.GetComponent<MonsterBehavior>();
        if (behavior != null)
        {
            behavior.spawnPoint = localPos;
            behavior.spawnTile = new Vector3Int(x, 0, z);
            behavior.playerId = playerId; 
        }
        gameBoard.monsterLocations[x, z] = selectedMonster + 1; // Store ID (offset by 1 so 0 stays 'Empty')
        
        RotateMonster(newMonster);
    }

    private void RotateMonster(GameObject monster)
    {
        if (selectedMonster == 0)
        {
            monster.transform.Rotate(0, 180, 0);
        }
        else if (selectedMonster == 1 || selectedMonster == 2)
        {
            monster.transform.Rotate(0, 90, 0);
        }
        else if (selectedMonster == 3)
        {
            monster.transform.Rotate(0, 0, 180);
        }
        
    }

    public void LogMonsterGrid()
    {
        string visualGrid = "Board State:\n";
        
        for (int z = gameBoard.depth - 1; z >= 0; z--) 
        {
            for (int x = 0; x < gameBoard.width; x++)
            {
                visualGrid += gameBoard.monsterLocations[x, z] + " ";
            }
            visualGrid += "\n"; 
        }
        
        Debug.Log(visualGrid);
    }
    }

