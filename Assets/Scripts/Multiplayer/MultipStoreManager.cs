using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MultipStoreManager : MonoBehaviour
{

    public PriceClass[] plantPrice;
    //GameManager gameManager;
    public MultipPlayerCurrency currency;
    public MultipMonsterSpawner monsterSpawner;
    public MultipGameBoard gameManager;
    [HideInInspector] public int currentSelected = 3;

    public MultipMonsterSpawner p1MonsterSpawner;
    public MultipMonsterSpawner p2MonsterSpawner;

    public Text[] texts;
    public Image[] buttonCooldowns;
    string boardTag = "P1Board";

    void Start()
    {
        GameObject gm = GameObject.FindGameObjectWithTag("GameManager");

        if (gm == null)
        {
            Debug.LogError("No GameManager object found.");
            return;
        }

        currency = gm.GetComponent<MultipPlayerCurrency>();

        if (currency == null)
        {
            Debug.LogError("No MultipPlayerCurrency found on GameManager.");
            return;
        }

        int localPlayerID = Photon.Pun.PhotonNetwork.LocalPlayer.ActorNumber == 1 ? 1 : 2;

        if (localPlayerID == 1)
        {
            monsterSpawner = p1MonsterSpawner;
        }
        else
        {
            monsterSpawner = p2MonsterSpawner;
        }

        if (monsterSpawner == null)
        {
            Debug.LogError("Store selected spawner is NULL for Player " + localPlayerID);
            return;
        }

        Debug.Log("Store using spawner for Player " + localPlayerID);

        for (int i = 0; i < texts.Length && i < plantPrice.Length; i++)
        {
            if (texts[i] != null)
            {
                texts[i].text = plantPrice[i].price.ToString();
            }
        }

    }

    public bool CheckMonsterPrice(int monsterID)
    {
        //checks if the player has enough currency to buy selected plant

        if (monsterID < 0 || monsterID >= plantPrice.Length)
        {
            Debug.LogWarning("Invalid monster ID: " + monsterID);
            return false;
        }

        if (currency == null)
        {
            Debug.LogError("MultipStoreManager currency is NULL");
            return false;
        }

        if (monsterSpawner == null)
        {
            Debug.LogError("MultipStoreManager monsterSpawner is NULL");
            return false;
        }

        int playerNum = currency.getPlayerNum();

        if (currency.canAfford(playerNum, plantPrice[monsterID].price) && !plantPrice[monsterID].onCooldown)
        {
            plantPrice[monsterID].canBuy = true;
            monsterSpawner.selectedMonster = monsterID;
            return true;
        }
        else
        {
            plantPrice[monsterID].canBuy = false;
            return false;
        }

        if (plantPrice[monsterID].price == 0 && !plantPrice[monsterID].onCooldown)
        {
            plantPrice[monsterID].canBuy = true;
            monsterSpawner.selectedMonster = monsterID;
            return true;
        }

        plantPrice[monsterID].canBuy = false;
        return false;
    }

    public IEnumerator BuyCooldown(int plantID)
    {
        Debug.Log("Buy Cooldown Started for plant " + plantID);
        Abilities ability = buttonCooldowns[plantID].GetComponent<Abilities>();
        ability.StartCooldown(plantPrice[plantID].cooldown);

        plantPrice[plantID].onCooldown = true;
        float cooldown = plantPrice[plantID].cooldown;
        yield return new WaitForSeconds(cooldown);
        plantPrice[plantID].onCooldown = false;
    }
}