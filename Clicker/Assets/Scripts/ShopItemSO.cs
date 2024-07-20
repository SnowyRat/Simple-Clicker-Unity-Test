using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="ShopMenu",menuName = "Scriptable Objects/New Shop Item", order = 1)]
public class ShopItemSO : ScriptableObject

{
    public string title;
    public double damage;
    public int level;
    public double price100;
    public double price10;
    public double price1;
    public double baseDmg;
    public double baseCost;

    public void ResetDefaults(){
        damage = 5;
        level = 1;
        price100 = 0;
        price10 = 0;
        price1 = 0;
    }



}
