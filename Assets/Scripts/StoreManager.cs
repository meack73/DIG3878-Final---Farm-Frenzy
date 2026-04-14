using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    public PriceClass[] plantPrice;
    PlayerCurrency currency;
    public MonsterSpawner monsterSpawner;
    [HideInInspector] public int currentSelected = 3;

    public Text[] texts; 
    public Image[] buttonCooldowns;
    
    void Start()
    {
        //Finds the currency script attached to the camera
        Camera camera = Camera.main;
        currency = camera.GetComponent<PlayerCurrency>();
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].text = plantPrice[i].price.ToString();
        }

    }

    void Update()
    {
        if (monsterSpawner.selectedMonster != -1)
        {
            currentSelected = monsterSpawner.selectedMonster;
        }

        //Debug.Log ("current selected plant is " + currentSelected);
    }

    public void CheckMonsterPrice(int monsterID)
    {
        //checks if the player has enough currency to buy selected plant
        if (currency.playerCurrency >= plantPrice[monsterID].price && !plantPrice[monsterID].onCooldown)
        {
            plantPrice[monsterID].canBuy = true;
            StartCoroutine(BuyCooldown(monsterID));
        }
        else if (currency.playerCurrency < plantPrice[monsterID].price || plantPrice[monsterID].onCooldown)
        {
            plantPrice[monsterID].canBuy = false;
        }

        if (plantPrice[monsterID].price == 0 && !plantPrice[monsterID].onCooldown)
        {
            plantPrice[monsterID].canBuy = true;
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