using System.Collections;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public PriceClass[] plantPrice;
    [HideInInspector] public bool canBuy = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public IEnumerator BuyCooldown(int plantID)
    {
        canBuy = false;
        float cooldown = plantPrice[plantID].cooldown;
        yield return new WaitForSeconds(cooldown);
        canBuy = true;
    }
}
