using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossTest : MonoBehaviour
{
    [SerializeField] private double maxHealth = 80.0; // Maximum health of the monster
    [SerializeField] private double monsterHealth;
    private SpriteRenderer spriteRenderer;
    private DamageFlash _damageFlash;
    private Animator animator;
    private MnsterHealth healthSlider;
    private double moneyAmount = 3.0;
    private double moneyMultiplier = 4.0;
    private string monsterName = "Bad bitch";
    public GameObject coinPrefab;

    void Start()
    {
        animator = GetComponent<Animator>();
        monsterHealth = maxHealth; // Initialize current health to max health
        _damageFlash = GetComponent<DamageFlash>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthSlider = FindObjectOfType<MnsterHealth>();
    }

    public double CurrentHealth()
    {
        return monsterHealth;
    }

    public void TakeDamage(double damageAmount, bool crit)
    {
        if (monsterHealth <= 0.0)
        {
            healthSlider.SetSliderVisibility(true);
            return; // The monster is already dead, so no further action is needed
        }
        if (crit)
        {
            DamagePopUpGenerator.current.CreatePopUp(transform.position, damageAmount.ToString("F1"), Color.yellow,crit);
        }
        else
        {
            DamagePopUpGenerator.current.CreatePopUp(transform.position, damageAmount.ToString("F1"), Color.white,crit);
        }
        healthSlider.SetMonsterName(monsterName);
        healthSlider.SetSliderVisibility(true);
        monsterHealth -= damageAmount; // Decrease the current health by the damage amount
        bool flipX = Random.Range(0, 2) == 0;
        spriteRenderer.flipX = flipX;
        animator.SetTrigger("Monsterhit");
        _damageFlash.CallDamageFlash();

        if (monsterHealth <= 0.0)
        {
            healthSlider.SetSliderVisibility(true);
            StartCoroutine(DieWithDelay()); // Monster has no health left, trigger death behavior
        }

        healthSlider.UpdateSliderValue(monsterHealth, maxHealth);
    }

    public void UpdateMoneyAmount()
    {
        moneyAmount = moneyAmount * 1.6;
    }

    public void SetHPMultiplier(int stageCount, double monsterHealth, bool bossKilled)
    {
        Debug.Log("MiniBoss HP Changed");
        // Adjust the monster's HP based on the stage count or any other desired logic
        // Replace coefficient with your desired value
        //int newHP = Mathf.FloorToInt(maxHealth * hpMultiplier); // Calculate the new HP
        maxHealth = monsterHealth; // Update the monster's maximum health
        this.monsterHealth = maxHealth; // Reset the monster's current health
    }

    private IEnumerator DieWithDelay()
    {
        animator.SetTrigger("MonsterDie");
        healthSlider.SetSliderVisibility(true);
        int coinCount = Random.Range(4, 8); // Randomly determine the number of coins to spawn (2-3).
        moneyAmount = Mathf.FloorToInt((float)(moneyAmount * moneyMultiplier));
        double moneyPerCoin = Mathf.FloorToInt((float)(moneyAmount / coinCount)); // Calculate the money value for each coin.
        List<CoinValue> coins = new List<CoinValue>();
        for (int i = 0; i < coinCount; i++)
        {
            GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            CoinValue coinComponent = coin.GetComponent<CoinValue>();
            coins.Add(coinComponent);
            coinComponent.SetMoneyValue(moneyPerCoin);
            Rigidbody2D coinRigidbody = coin.GetComponent<Rigidbody2D>();
            coinRigidbody.velocity = new Vector2(Random.Range(-1.5f, 1.5f), Random.Range(1f, 2f));
            coinRigidbody.gravityScale = 1.5f;
            coinComponent.StartCoinCollection();
        }
        yield return new WaitForSeconds(0.2f);
        healthSlider.SetSliderVisibility(false);
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
