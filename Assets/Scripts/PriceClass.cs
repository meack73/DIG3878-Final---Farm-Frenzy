using System;
using UnityEngine;

[System.Serializable]
public class PriceClass
{
    public float cooldown;
    public int price;
    [HideInInspector] public bool canBuy = false;
    [HideInInspector] public bool onCooldown = false;
}
