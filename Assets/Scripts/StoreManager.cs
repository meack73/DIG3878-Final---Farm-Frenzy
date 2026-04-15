using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    public PriceClass[] plantPrice;
    PlayerCurrency currency;
    public MonsterSpawner monsterSpawner;
    GameManager gameManager; 
    [HideInInspector] public int currentSelected = 3;

    public Text[] texts; 
    public Image[] buttonCooldowns;
    string boardTag = "P1Board";
    
    void Start()
    {
        //Finds the currency script attached to the camera
        Camera camera = Camera.main;
        currency = camera.GetComponent<PlayerCurrency>();
        GameObject gm = GameObject.FindGameObjectWithTag("GameManager");
        gameManager = gm.GetComponent<GameManager>();
        
        if (gameManager.playerId == 2) {
            boardTag = "P2Board";
        }
        monsterSpawner = GameObject.FindGameObjectWithTag(boardTag).GetComponentInChildren<MonsterSpawner>();


        if (monsterSpawner == null)
        {
            Debug.Log("store manager cannot find monster spawner"); 
        }
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].text = plantPrice[i].price.ToString();
        }

    }

    public void CheckMonsterPrice(int monsterID)
    {
        //checks if the player has enough currency to buy selected plant
        if (currency.playerCurrency >= plantPrice[monsterID].price && !plantPrice[monsterID].onCooldown)
        {
            plantPrice[monsterID].canBuy = true;
            monsterSpawner.selectedMonster = monsterID;
            StartCoroutine(BuyCooldown(monsterID));
        }
        else if (currency.playerCurrency < plantPrice[monsterID].price || plantPrice[monsterID].onCooldown)
        {
            plantPrice[monsterID].canBuy = false;
        }

        if (plantPrice[monsterID].price == 0 && !plantPrice[monsterID].onCooldown)
        {
            plantPrice[monsterID].canBuy = true;
            monsterSpawner.selectedMonster = monsterID; 
        }
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