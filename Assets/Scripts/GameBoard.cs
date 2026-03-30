using UnityEngine;

public class GameBoard : MonoBehaviour
{
    public GameObject tilePrefab; // P2 has one without outline
    private GameObject separator;
    public int width = 8;  
    public int depth = 5; 
    private Color lightGreen = new Color(0.45f, 0.8f, 0.2f);
    private Color darkGreen = new Color(0.4f, 0.7f, 0.15f);
    public int tileSize = 3;
    public int playerNumber;
    private float offset;
    
    void Start()
    {
        GenerateGrid();
        if (playerNumber == 1)
        {
            CreateSeparator();
        }
    }

    void CreateSeparator()
    {
        if (separator != null) return; // Only create one separator
        separator = GameObject.CreatePrimitive(PrimitiveType.Cube);
        separator.transform.localScale = new Vector3(1, 0.5f, depth * tileSize);
        separator.transform.position = new Vector3(0, 0, 0);
        separator.GetComponent<Renderer>().material.color = Color.red;
    }

    void GenerateGrid()
    {
        offset = tileSize * width / 2 + 0.5f; //separator width
        if (gameObject.CompareTag("Player1"))
        {
            playerNumber = 1;
            transform.position = new Vector3(-offset, 0, 0);
            transform.rotation = Quaternion.identity;
        } else if (gameObject.CompareTag("Player2"))
        {
            playerNumber = 2;            
            transform.position = new Vector3(offset, 0, 0);
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        
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
                Debug.Log($"Tile {newTile.name} assigned data: {data.xIndex}, {data.zIndex}");
                Renderer rend = newTile.GetComponent<Renderer>();
                if ((x + z) % 2 == 0)
                    rend.material.color = lightGreen;
                else
                    rend.material.color = darkGreen;
            }
        }
    }
}