using UnityEngine;
using Photon.Pun;

public class MultipMonsterSpawner : MonoBehaviourPunCallbacks
{
    public int selectedMonster = 0;
    public MultipGameBoard gameBoard;
    public GameObject[] monsterPrefabs;
    private int playerId = 0;

    void Start()
    {
        Debug.Log("MONSTER SPAWNER START");
        gameBoard = GetComponentInParent<MultipGameBoard>();

        playerId = gameBoard.playerId;
    }

    public void PlaceMonster(int x, int z)
    {
        int localPlayerID = PhotonNetwork.LocalPlayer.ActorNumber == 1 ? 1 : 2; // Determine local player ID based on Photon ActorNumber
        if (localPlayerID != playerId)
        {
            Debug.Log("You can only place on you board!");
            return;
        }

        photonView.RPC(nameof(RPC_PlaceMonster), RpcTarget.All, x, z, selectedMonster);
    }

    [PunRPC]
    public void RPC_PlaceMonster(int x, int z, int selectedMonster)
    {
        if (x < 0 || x >= gameBoard.width || z < 0 || z >= gameBoard.depth) return;

        if (selectedMonster < 0 || selectedMonster >= monsterPrefabs.Length) return;

        if (gameBoard.monsterLocations[x, z] != 0)
        {
            Debug.Log("Space occupied!");
            return;
        }

        SpawnMonsterLocal(x, z, selectedMonster);
    }

    private void SpawnMonsterLocal(int x, int z, int selectedMonster)
    { 
        // We use the board's logic to find the exact center of the tile
        float startX = -((gameBoard.width - 1) * gameBoard.tileSize) / 2f;
        float startZ = -((gameBoard.depth - 1) * gameBoard.tileSize) / 2f;

        float localX = startX + (x * gameBoard.tileSize) - (gameBoard.tileSize / 2f);
        float localZ = startZ + (z * gameBoard.tileSize) + (gameBoard.tileSize / 2f);
        Vector3 localPos = new Vector3(localX, 0.5f, localZ);
        Vector3 worldPos = transform.TransformPoint(localPos);

        GameObject newMonster = Instantiate(monsterPrefabs[selectedMonster], worldPos, transform.rotation);
        if (selectedMonster == 1 || selectedMonster == 2) //Cactus or mushroom
        {
            MonsterBehavior behavior = newMonster.GetComponent<MonsterBehavior>();
            if (behavior != null)
            {
                behavior.spawnPoint = localPos;
                behavior.spawnTile = new Vector3Int(x, 0, z);
                behavior.playerId = playerId;
            }
        }
        else if (selectedMonster == 0) //shooter
        {
            ShooterBehavior behavior = newMonster.GetComponent<ShooterBehavior>();
            if (behavior != null)
            {
                behavior.spawnPoint = localPos;
                behavior.spawnTile = new Vector3Int(x, 0, z);
                behavior.playerId = playerId;
            }
        }
        else //sunflower
        {
            FlowerBehavior behavior = newMonster.GetComponent<FlowerBehavior>();
            if (behavior != null)
            {
                behavior.spawnPoint = localPos;
                behavior.spawnTile = new Vector3Int(x, 0, z);
                behavior.playerId = playerId;
            }
        }

        gameBoard.monsterLocations[x, z] = selectedMonster + 1; // Store ID (offset by 1 so 0 stays 'Empty')

        RotateMonster(newMonster, selectedMonster);
        LogMonsterGrid();
    }

    private void RotateMonster(GameObject monster, int selectedMonster)
    {
        if (selectedMonster == 0)
        {
            monster.transform.Rotate(0, 180, 0);
        }
        else if (selectedMonster == 1 || selectedMonster == 2)
        {
            monster.transform.Rotate(0, 90, 0);
            monster.transform.Translate(0, -0.5f, 0);
        }
        else if (selectedMonster == 3)
        {
            monster.transform.Rotate(0, 0, 180);
            monster.transform.Translate(0, -0.5f, 0);
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

