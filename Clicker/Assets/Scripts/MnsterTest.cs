using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MnsterTest : MonoBehaviour
{
    [SerializeField] private double maxHealth = 29; // Maximum health of the monster
    [SerializeField] private double monsterHealth; // Current health of the monster
    private SpriteRenderer spriteRenderer;
    private DamageFlash _damageFlash;
    private Animator animator;
    private MnsterHealth healthSlider;
    private double moneyAmount = 3;
    private string monsterName = "Slime";
    public GameObject coinPrefab;

    private void Start()
    {
        animator = GetComponent<Animator>();
        monsterHealth = maxHealth; // Initialize current health to max health
        _damageFlash = GetComponent<DamageFlash>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthSlider = FindObjectOfType<MnsterHealth>();
            if (spriteRenderer != null)
    {
        // Always flip the sprite horizontally when spawned
        spriteRenderer.flipX = true;
    }
    }

    public double CurrentHealth()
    {
        return monsterHealth;
    }

    public void TakeDamage(double damageAmount, bool crit)
    {
        if (monsterHealth <= 0)
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
        Debug.Log("Name set");
        healthSlider.SetSliderVisibility(true);
        Debug.Log("Slider set");
        monsterHealth -= damageAmount; // Decrease the current health by the damage amount
        //bool flipX = Random.Range(0, 2) == 0;
        //spriteRenderer.flipX = true;
        animator.SetTrigger("Monsterhit");
        _damageFlash.CallDamageFlash();

        if (monsterHealth <= 0)
        {
            healthSlider.SetSliderVisibility(true);
            StartCoroutine(DieWithDelay()); // Monster has no health left, trigger death behavior
        }

        healthSlider.UpdateSliderValue(monsterHealth, maxHealth);
    }

    public void UpdateMoneyAmount(double money)
    {
        moneyAmount = money;
    }

    public void SetHPMultiplier(int stageCount, double monsterHealth, bool bossKilled)
    {
        if (stageCount != 1 && bossKilled)
        {
            // Adjust the monster's HP based on the stage count or any other desired logic
            // Replace coefficient with your desired value
            maxHealth = monsterHealth; // Update the monster's maximum health
            this.monsterHealth = maxHealth; // Reset the monster's current health
        }
        else if (stageCount != 1)
        {
            maxHealth = monsterHealth;
        }
    }

    private IEnumerator DieWithDelay()
    {
        animator.SetTrigger("MonsterDie");
        healthSlider.SetSliderVisibility(true);
        int coinCount = Random.Range(2, 4); // Randomly determine the number of coins to spawn (2-3).
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
