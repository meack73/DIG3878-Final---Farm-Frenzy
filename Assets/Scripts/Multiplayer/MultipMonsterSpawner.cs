using UnityEngine;
using Photon.Pun;

public class MultipMonsterSpawner : MonoBehaviourPunCallbacks
{
    public int selectedMonster = -1;
    public MultipGameBoard gameBoard;
    public GameObject[] monsterPrefabs;
    private int playerId = 0;

    public int[] monsterCosts;
    public MultipPlayerCurrency currencyManager;

    public AudioClip placeSFX;
    public AudioSource speaker;

    void Start()
    {
        Debug.Log("MONSTER SPAWNER START");
        gameBoard = GetComponentInParent<MultipGameBoard>();

        playerId = gameBoard.playerId;

        if (currencyManager == null)
        {
            currencyManager = FindObjectOfType<MultipPlayerCurrency>();
        }
    }

    public void PlaceMonster(int x, int z)
    {

        if (selectedMonster == -1)
        {
            Debug.Log("No monster selected.");
            return;
        }

        if (gameBoard == null)
        {
            Debug.LogError("MultipMonsterSpawner gameBoard is NULL on " + gameObject.name);
            return;
        }

        if (currencyManager == null)
        {
            Debug.LogError("MultipMonsterSpawner currencyManager is NULL on " + gameObject.name);
            return;
        }


        int localPlayerID = PhotonNetwork.LocalPlayer.ActorNumber == 1 ? 1 : 2; // Determine local player ID based on Photon ActorNumber
        if (localPlayerID != playerId)
        {
            Debug.Log("You can only place on you board!");
            return;
        }

        if (x < 0 || x >= gameBoard.width || z < 0 || z >= gameBoard.depth)
        {
            Debug.Log("Invalid tile coordinates!");
            return;
        }

        if (selectedMonster < 0 || selectedMonster >= monsterPrefabs.Length)
        {
            Debug.Log("Invalid monster selection!");
            return;
        }
        
        if(gameBoard.monsterLocations[x, z] != 0)
        {
            Debug.Log("Space occupied!");
            return;
        }

        if (monsterCosts == null || selectedMonster >= monsterCosts.Length)
        {
            Debug.LogError("Monster costs not properly defined!");
            return;
        }

        int cost = monsterCosts[selectedMonster];

        if (currencyManager == null)
        {
            Debug.LogError("Currency manager not found!");
            return;
        }

        if (!currencyManager.spendCoins(playerId, cost))
        {
            Debug.Log("Not enough currency to place this monster!");
            return;
        }

        photonView.RPC(nameof(RPC_PlaceMonster), RpcTarget.All, x, z, selectedMonster);
        selectedMonster = -1; // Reset selection after placing
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

        if (selectedMonster != 0 && selectedMonster != 1)
        {
            gameBoard.monsterLocations[x, z] = selectedMonster + 1;
        }

        assignMonsterData(newMonster, selectedMonster, x, z);

        RotateMonster(newMonster, selectedMonster);

        if (speaker != null && placeSFX != null)
        {
            speaker.PlayOneShot(placeSFX);
        }

        LogMonsterGrid();
    }

    private void assignMonsterData(GameObject newMonster, int selectedMonster, int x, int z)
    {
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
    }

    private void RotateMonster(GameObject monster, int selectedMonster)
    {
        if (playerId == 2 && selectedMonster != 4)
        {
            monster.transform.Rotate(0, 180, 0);
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
        else if (selectedMonster == 4)
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
