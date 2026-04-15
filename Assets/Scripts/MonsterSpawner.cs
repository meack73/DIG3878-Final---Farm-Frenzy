using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public int selectedMonster = 0;
    public GameBoard gameBoard;
    public GameObject[] monsterPrefabs;
    public int playerId = 0;

    public AudioClip placeSFX;
    public AudioSource speaker;

    void Start()
    {    
        gameBoard = GetComponentInParent<GameBoard>();

        if (gameBoard == null)
        {
            Debug.Log("cannot find gameboard"); 
        }
        playerId = gameBoard.playerId;    
    }

    public void PlaceMonster(int x, int z)
    {

        if (selectedMonster < 0 || selectedMonster >= monsterPrefabs.Length)
            return;

        if (gameBoard.monsterLocations[x, z] != 0)
            return;

        string tileName = $"Tile_{x}_{z}";
        Transform tile = gameBoard.transform.Find(tileName);

        if (tile == null)
        {
            Debug.LogError(tileName + " not found");
            return;
        }

        Vector3 spawnPos = tile.position;

        GameObject newMonster = Instantiate(
            monsterPrefabs[selectedMonster],
            spawnPos,
            Quaternion.identity
        );

        gameBoard.monsterLocations[x, z] = selectedMonster + 1;

        // assign data
        if (selectedMonster == 0 || selectedMonster == 1)
        {
            var b = newMonster.GetComponent<MonsterBehavior>();
            if (b != null)
            {
                b.spawnTile = new Vector3Int(x, 0, z);
                b.playerId = playerId;
            }
        }
        else if (selectedMonster == 2)
        {
            var b = newMonster.GetComponent<ShooterBehavior>();
            if (b != null)
            {
                b.spawnTile = new Vector3Int(x, 0, z);
                b.playerId = playerId;
            }
        }
        else if (selectedMonster == 3)
        {
            var b = newMonster.GetComponent<FlowerBehavior>();
            if (b != null)
            {
                b.spawnTile = new Vector3Int(x, 0, z);
                b.playerId = playerId;
            }
        }
        else if (selectedMonster == 4)
        {
            var b = newMonster.GetComponent<WalnutBehavior>();
            if (b != null)
            {
                b.spawnTile = new Vector3Int(x, 0, z);
                b.playerId = playerId;
            }
        }

        RotateMonster(newMonster);
    }

    private void RotateMonster(GameObject monster)
    {
        if (playerId == 2 && selectedMonster != 4)
        {
            monster.transform.Rotate(0,180,0); 
        }
        
        if (selectedMonster == 2) //shooter
        {
            monster.transform.Rotate(0, 180, 0);
            monster.transform.Translate(-1.5f, 0, 0);
        }
        else if (selectedMonster == 0 || selectedMonster == 1) //cactus and mushroom
        {
            monster.transform.Rotate(0, 90, 0);
            monster.transform.Translate(0, -0.5f, 0);
        }
        else if (selectedMonster == 3) //sunflower
        {
            if (playerId == 1)
            {
                monster.transform.Translate(1f, 1f, 0);
            }
            else if (playerId == 2)
            {
                monster.transform.Translate(-1.5f, 1f, 0);
                monster.transform.Rotate(0, 180, 0);
            }
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
