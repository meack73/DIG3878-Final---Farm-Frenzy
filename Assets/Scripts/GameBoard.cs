using UnityEngine;

public class GameBoard : MonoBehaviour
{
    public GameObject tilePrefab; // P2 has one without outline
    public int width = 8;  
    public int depth = 5; 
    private Color lightGreen = new Color(0.45f, 0.8f, 0.2f);
    private Color darkGreen = new Color(0.4f, 0.7f, 0.15f);
    public int tileSize = 3;
    public int playerId;
    private float offset;
    
    public int[,] monsterLocations;

    void Awake()
    {
        if (gameObject.CompareTag("P1Board")) playerId = 1;
        else if (gameObject.CompareTag("P2Board")) playerId = 2;

        Debug.Log("GAME BOARD AWAKE");

        monsterLocations = new int[width, depth];
        offset = tileSize * width / 2 + 0.5f; //separator width
        if (playerId == 1)
        {
            transform.position = new Vector3(-offset, 0, 0);
            transform.rotation = Quaternion.identity;
        } else if (playerId == 2)
        {         
            transform.position = new Vector3(offset, 0, 0);
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        //FOR TESTING MULTILPLAYER 
        //in multiplayer version, constantly import and update monsterLocations

        GenerateGrid();
        monsterLocations = new int[width, depth];
        if (playerId == 1) {

            for (int x = 0; x < monsterLocations.GetLength(0); x++) {
                for (int z = 0; z < monsterLocations.GetLength(1); z++) {
                    monsterLocations[x, z] = 0; // 0 means empty
                }
            } //1, 2, 3 for different monsters
        }

        else //edit for opponent board testing
        {
           monsterLocations = new int[,]
            {
                {0, 0, 1, 1, 0},
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0},
                {3, 0, 4, 0, 0},
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0},
                {0, 4, 3, 2, 1}
            };
        }
    }

    void GenerateGrid()
    {
        float startX = -((width - 1) * tileSize) / 2f;
        float startZ = -((depth - 1) * tileSize) / 2f;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                // Spawn the tile at X and Z coordinates
                Vector3 localSpawnPos = new Vector3(startX + (x * tileSize), 0, startZ + (z * tileSize));                
                
                GameObject newTile = Instantiate(tilePrefab, transform);
                newTile.transform.localPosition = localSpawnPos;
                newTile.transform.localRotation = Quaternion.identity;

                newTile.name = $"Tile_{x}_{z}"; //names are Tile_1_1

                //Debug.Log($"Spawned tile at {x}, {z} with local position {localSpawnPos}");

                TileData data = newTile.GetComponent<TileData>();
                if (data == null) data = newTile.AddComponent<TileData>(); 

                data.xIndex = x; 
                data.zIndex = z;
                Renderer rend = newTile.GetComponent<Renderer>();
                if ((x + z) % 2 == 0)
                    rend.material.color = lightGreen;
                else
                    rend.material.color = darkGreen;
            }
        }
    }
}