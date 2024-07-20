using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManag : MonoBehaviour
{
    private double moneyHeld;
    private GameManag _gameManager;
    private PlayerControl _playerControl;
    private double cost;
    public ShopItemSO[] shopItemSO;
    public ShopTemplate[] shopPanels;
    public GameObject[] shopPanelsGO;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < shopItemSO.Length; i++)
        {
            shopPanelsGO[i].SetActive(true);
            shopItemSO[i].ResetDefaults();
        }
        _playerControl = FindObjectOfType<PlayerControl>();
        _gameManager = FindObjectOfType<GameManag>();
        moneyHeld = _gameManager.GetMoney();
        LoadPanels();
        CheckPurchaseable();
    }
    public void StartGame(){
        
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void MoneyHeld(double money)
    {
        moneyHeld = money;
        CheckPurchaseable();
    }
    public void MoneyLost(double money)
    {
        moneyHeld -= cost;
        _gameManager.SetMoney(moneyHeld);
        CheckPurchaseable();
    }
    public void LoadPanels()
    {
        for (int i = 0; i < shopItemSO.Length; i++)
        {
            shopPanels[i].title.text = shopItemSO[i].title;
            shopPanels[i].damage.text = "Dmg: " + shopItemSO[i].damage.ToString("F2");
            shopPanels[i].level.text = "Lvl: " + shopItemSO[i].level.ToString();
            shopPanels[i].price100.text = shopItemSO[i].price100.ToString("F2");
            shopPanels[i].price10.text = shopItemSO[i].price10.ToString("F2");
            shopPanels[i].price1.text = shopItemSO[i].price1.ToString("F2");
            shopPanels[i].shopItemSO = shopItemSO[i];
        }
    }
    public void UpdatePrice(double? x100 = null, double? x10 = null, double? x1 = null, int? level = null, double? dmg = null)
    {
        for (int i = 0; i < shopItemSO.Length; i++)
        {
            if (dmg != null)
            {
                shopPanels[i].damage.text = "Dmg: " + dmg.Value.ToString("F2");
                double intValue = dmg ?? 0;
                shopItemSO[i].damage = intValue;
            }

            if (level != null)
            {
                shopPanels[i].level.text = "Lvl: " + level.Value.ToString("F2");
                int intValue = level ?? 0;
                shopItemSO[i].level = intValue;
            }

            if (x100 != null)
            {
                shopPanels[i].price100.text = x100.Value.ToString("F2");
            }

            if (x10 != null)
            {
                shopPanels[i].price10.text = x10.Value.ToString("F2");
            }

            if (x1 != null)
            {
                shopPanels[i].price1.text = x1.Value.ToString("F2");
            }
        }
    }
public void Purchasex100(ShopItemSO currentItem)
{
    double cost = 0;
    int newLevel = currentItem.level + 100; // Increase the level by 100

    // Calculate the new cost and update the damage based on a formula
    for (int i = currentItem.level; i < newLevel; i++)
    {
        cost += currentItem.baseCost * Mathf.Pow(1.062f, i);
    }
    double newDamage = currentItem.baseDmg * Mathf.Pow(1.062f, newLevel - 1);

    // Update the player's level and damage
    // Replace the following lines with your desired logic to update the player's level and damage
    _playerControl.IncreaseLevel(newLevel);
    _playerControl.UpdateDamage(newDamage);

    // Deduct the cost from the moneyHeld
    moneyHeld -= cost;
    _gameManager.SetMoney(moneyHeld);

    // Update the UI panels
    UpdatePrice(level: newLevel, dmg: newDamage);
    CheckPurchaseable();
}
public void Purchasex10(ShopItemSO currentItem)
{
    double cost = 0;
    int newLevel = currentItem.level + 10; // Increase the level by 100

    // Calculate the new cost and update the damage based on a formula
    for (int i = currentItem.level; i < newLevel; i++)
    {
        cost += currentItem.baseCost * Mathf.Pow(1.062f, i);
    }
    double newDamage = currentItem.baseDmg * Mathf.Pow(1.062f, newLevel - 1);

    // Update the player's level and damage
    // Replace the following lines with your desired logic to update the player's level and damage
    _playerControl.IncreaseLevel(newLevel);
    _playerControl.UpdateDamage(newDamage);

    // Deduct the cost from the moneyHeld
    moneyHeld -= cost;
    _gameManager.SetMoney(moneyHeld);

    // Update the UI panels
    UpdatePrice(level: newLevel, dmg: newDamage);
    CheckPurchaseable();
}
public void Purchasex1(ShopItemSO currentItem)
{
    double cost = 0;
    int newLevel = currentItem.level + 1; // Increase the level by 100

    // Calculate the new cost and update the damage based on a formula
    for (int i = currentItem.level; i < newLevel; i++)
    {
        cost += currentItem.baseCost * Mathf.Pow(1.062f, i);
    }
    double newDamage = currentItem.baseDmg * Mathf.Pow(1.062f, newLevel - 1);

    // Update the player's level and damage
    // Replace the following lines with your desired logic to update the player's level and damage
    _playerControl.IncreaseLevel(newLevel);
    _playerControl.UpdateDamage(newDamage);

    // Deduct the cost from the moneyHeld
    moneyHeld -= cost;
    _gameManager.SetMoney(moneyHeld);

    // Update the UI panels
    UpdatePrice(level: newLevel, dmg: newDamage);
    CheckPurchaseable();
}

    public void CheckPurchaseable()
    {
        for (int i = 0; i < shopItemSO.Length; i++)
        {
            if (Calculate100x(shopItemSO[i]))
            {
                shopPanels[i].x100.interactable = true;
            }
            else
            {
                shopPanels[i].x100.interactable = false;
            }

            if (Calculate10x(shopItemSO[i]))
            {
                shopPanels[i].x10.interactable = true;
            }
            else
            {
                shopPanels[i].x10.interactable = false;
            }

            if (Calculate1x(shopItemSO[i]))
            {
                shopPanels[i].x1.interactable = true;
            }
            else
            {
                shopPanels[i].x1.interactable = false;
            }

        }
    }

    public bool Calculate100x(ShopItemSO currentItem)
    {
        double cost = 0;
        for (int i = currentItem.level; i < currentItem.level + 100; i++)
        {
            cost += currentItem.baseCost * Mathf.Pow(1.062f, i);
        }
        currentItem.price100 = cost;
        UpdatePrice(x100: cost);
        if (cost <= moneyHeld)
        {
            return true;
        }
        return false;
    }
    public bool Calculate10x(ShopItemSO currentItem)
    {
        double cost = 0;
        for (int i = currentItem.level; i < currentItem.level + 10; i++)
        {
            cost += currentItem.baseCost * Mathf.Pow(1.062f, i);
        }
        currentItem.price10 = cost;
        UpdatePrice(x10: cost);
        if (cost <= moneyHeld)
        {
            return true;
        }
        return false;
    }
    public bool Calculate1x(ShopItemSO currentItem)
    {
        double cost = currentItem.baseCost * Mathf.Pow(1.062f, currentItem.level);
        currentItem.price1 = cost;
        UpdatePrice(x1: cost);
        if (cost <= moneyHeld)
        {
            return true;
        }
        return false;
    }
}
