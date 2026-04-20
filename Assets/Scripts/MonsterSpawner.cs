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

        if (selectedMonster == 6)
        {
            PlacePumpkin(x,z); 
            return;
        }
        if (gameBoard.monsterLocations[x, z] != 0)
            return;

        string tileName = $"Tile_{x}_{z}";
        Transform tile = gameBoard.transform.Find(tileName);

        if (tile == null) return;


        GameObject newMonster = Instantiate(
            monsterPrefabs[selectedMonster],
            tile.position,
            Quaternion.identity
        );

        PlantBehavior b = newMonster.GetComponent<PlantBehavior>();
        if (b != null)
        {
            b.spawnTile = new Vector3Int(x, 0, z);
            b.playerId = playerId;
        }

        if (selectedMonster > 1)
            gameBoard.monsterLocations[x, z] = selectedMonster + 1;

        RotateMonster(newMonster);
    }

    private void PlacePumpkin(int x, int z)
    {
        string tileName = $"Tile_{x}_{z}";
        Transform tile = gameBoard.transform.Find(tileName);
        if (tile == null) return;

        // find a monster at that tile
        Collider[] hits = Physics.OverlapBox(tile.position, new Vector3(0.5f, 1f, 0.5f));
        foreach (Collider hit in hits)
        {
            PlantBehavior plant = hit.GetComponent<PlantBehavior>();
            if (plant != null && plant.playerId == playerId)
            {
                // check it doesnt already have a pumpkin
                if (plant.GetComponentInChildren<PumpkinBehavior>() != null)
                {
                    Debug.Log("already has a pumpkin");
                    return;
                }

                GameObject pumpkin = Instantiate(monsterPrefabs[6], plant.transform.position, Quaternion.identity);
                pumpkin.transform.Rotate(-90, 0, 0);

                Vector3 pos = pumpkin.transform.position;
                pumpkin.transform.position = new Vector3(pos.x, 0.3f, pos.z);
                
                pumpkin.transform.SetParent(plant.transform);
                pumpkin.GetComponent<PumpkinBehavior>().playerId = playerId;
                return;
            }
        }

        Debug.Log("no friendly monster at that tile");
    }

    private void RotateMonster(GameObject monster)
    {
        if (playerId == 2 && selectedMonster != 4 && selectedMonster != 5)
        {
            monster.transform.Rotate(0,180,0); 
        }

        if (selectedMonster == 0 || selectedMonster == 1) //cactus and mushroom
        {
            monster.transform.Rotate(0, 90, 0);
        }
        else if (selectedMonster == 2) //shooter
        {
            monster.transform.Translate(0, 0.5f, 0);
            monster.transform.Rotate(0, 180, 0);
        }
        else if (selectedMonster == 3) //sunflower
        {
            if (playerId == 1)
            {
                monster.transform.Translate(0f, 1.3f, 0);
            }
            else if (playerId == 2)
            {
                monster.transform.Translate(0, 1.3f, 0);
                monster.transform.Rotate(0, 180, 0);
            }
        } 
        else if (selectedMonster == 4) //walnut
        {
            monster.transform.Rotate(0, 180, 0);
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
