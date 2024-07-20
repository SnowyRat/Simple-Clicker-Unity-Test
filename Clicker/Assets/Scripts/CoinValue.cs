using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinValue : MonoBehaviour
{
    [SerializeField] private double moneyValue;
    [SerializeField] private double startingValue = 3.0;
    
    void Start()
    {
    }
    
    public void SetMoneyValue(double moneyCount)
    {
        moneyValue = moneyCount;
    }
    
    public double GetMoneyValue()
    {
        return moneyValue;
    }

    void Update()
    {
    }

    private IEnumerator CollectCoin()
    {
        yield return new WaitForSeconds(0.3f);
        Debug.Log("First Coin Loop");
        GameManag gameManager = FindObjectOfType<GameManag>();
        
        Rigidbody2D coinRigidbody = GetComponent<Rigidbody2D>();
        coinRigidbody.velocity = Vector2.zero; // Set velocity to zero to stop movement
        coinRigidbody.gravityScale = 0f;
        
        yield return new WaitForSeconds(0.5f);
        gameManager.AddMoney(moneyValue);
        Debug.Log("Second Coin Loop");
        
        Destroy(gameObject);
    }
    
    public void StartCoinCollection()
    {
        StartCoroutine(CollectCoin());
    }
}
