using System.Collections;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public PriceClass[] plantPrice;
    PlayerCurrency currency;
    public MonsterSpawner monsterSpawner;
    [HideInInspector] public int currentSelected = 3;

    void Start()
    {
        //Finds the currency script attached to the camera
        Camera camera = Camera.main;
        currency = camera.GetComponent<PlayerCurrency>();
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
            //StartCoroutine(BuyCooldown(monsterID));
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
        plantPrice[plantID].onCooldown = true;
        float cooldown = plantPrice[plantID].cooldown;
        yield return new WaitForSeconds(cooldown);
        plantPrice[plantID].onCooldown = false;
    }
}
