using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManag : MonoBehaviour
{
    [SerializeField] private List<GameObject> monsterPrefabs;
    [SerializeField] private List<GameObject> miniBossPrefabs;
    [SerializeField] private int monstersSpawned = 0;
    private FightBossButton _fightBossButton;
    private StageCounter _stageCounter;
    private bool bossMobsSpawned = false;
    private bool miniBossSpawned = false;
    [SerializeField] private int stageCount = 1;
    private BossCountdown _bossCountdown;
    private bool bossKilled = true;
    private double monsterHP = 23.0;
    private double monsterHpBase = 23;
    private double bossHP = 80.0;
    private bool stageAlreadySpawned = false;
    private CoinValue _coinValue;
    private double startingValue = 3.0;
    private double moneyValue;
    [SerializeField] private double moneyHeld;
    private GameObject spawnedMiniBoss;
    [SerializeField] private TextMeshProUGUI goldCounterTxt;
    private ShopManag _shopManager;

    void Start()
    {
        _fightBossButton = GetComponent<FightBossButton>();
        _bossCountdown = GetComponent<BossCountdown>();
        _stageCounter = GetComponent<StageCounter>();
        _coinValue = GetComponent<CoinValue>();
        _shopManager = FindObjectOfType<ShopManag>();
        if (bossKilled)
        {
            _fightBossButton.Disable();
        }
        goldCounterTxt.text = moneyHeld.ToString("F0");
    }

    private void SetMoneyValue(int stage)
    {
        moneyValue = startingValue * Mathf.Pow(1.55f, stage - 1);
    }

    void Update()
    {
        CheckMonsters();
    }

    public void AddMoney(double moneyValue)
    {
        moneyHeld += moneyValue;
        goldCounterTxt.text = moneyHeld.ToString("F0");
        _shopManager.MoneyHeld(moneyHeld);
    }

    public double GetMoney()
    {
        return moneyHeld;
    }

    public void SetMoney(double money)
    {
        moneyHeld = money;
        goldCounterTxt.text = moneyHeld.ToString("F0");
    }

    public void SpawnMonsters(int amount)
    {
        if (stageCount != 1 && bossKilled)
        {
            monsterHP = monsterHpBase * Mathf.Pow(1.65f,stageCount-1);
        }
        if (stageCount != 1 && !bossKilled)
        {
            monsterHP = monsterHP;
        }
        for (int i = 0; i < amount; i++)
        {
            
            // Randomly select a monster prefab from the list
            GameObject randomMonster = monsterPrefabs[Random.Range(0, monsterPrefabs.Count)];

            // Get a valid random spawn position
            Vector2 spawnPosition = GetRandomSpawnPosition(i);
            //Vector2 spawnPosition = GetValidSpawnPosition(randomMonster,i);

            // Instantiate the selected monster at the spawn position
            GameObject spawnedMonster = Instantiate(randomMonster, spawnPosition, Quaternion.identity);

            spawnedMonster.GetComponent<MnsterTest>().SetHPMultiplier(stageCount, monsterHP, bossKilled);
            spawnedMonster.GetComponent<MnsterTest>().UpdateMoneyAmount(moneyValue);

            monstersSpawned++;
        }
    }
/*
    private Vector2 GetValidSpawnPosition(GameObject monsterPrefab, int EnemyCount)
    {
        Vector2 spawnPosition;
        Collider2D monsterCollider = monsterPrefab.GetComponent<Collider2D>();

        int maxAttempts = 10;
        int currentAttempt = 0;

        do
        {
            // Generate a random spawn position within the desired range
            spawnPosition = GetRandomSpawnPosition(EnemyCount);

            // Check if the position is overlapping with any existing monsters
            Collider2D[] colliders = Physics2D.OverlapBoxAll(spawnPosition, monsterCollider.bounds.size, 0f);

            if (colliders.Length == 0)
            {
                // No collisions found, valid spawn position
                break;
            }

            currentAttempt++;
        } while (currentAttempt < maxAttempts);

        // If maxAttempts is reached and no valid position is found, fallback to random position
        if (currentAttempt >= maxAttempts)
        {
            spawnPosition = GetRandomSpawnPosition(EnemyCount);
        }

        return spawnPosition;
    }
*/
private Vector2 GetRandomSpawnPosition(int EnemyCount)
{
    float spawnX = 0f;
    
    if (EnemyCount >= 1)
    {
        float randomSpacing = Random.Range(EnemyCount+1f,EnemyCount+1.5f);
        spawnX += randomSpacing; // Adjust the X-position for subsequent enemies
    }
    
    float spawnY = 0f;
    return new Vector2(spawnX, spawnY);
}

    public void CheckMonsters()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        if (monstersSpawned >= 10 && monsters.Length == 0)
        {
            SpawnMiniBoss();
        }
        else if (monsters.Length == 0 && !miniBossSpawned && bossKilled)
        {
            stageAlreadySpawned = false;
            ProgressStage();
            _fightBossButton.Disable();
            SpawnMonsters(10);
        }
        else if (monsters.Length == 0 && !miniBossSpawned && !bossKilled)
        {
            _fightBossButton.Enable();
            SpawnMonsters(10);
            monstersSpawned = 0;
        }
    }

    private void ProgressStage()
    {
        stageCount += 1;
        SetMoneyValue(stageCount);
        _stageCounter.GetGameStage(stageCount);
    }

    private void SpawnMiniBoss()
    {
        _fightBossButton.Disable();
        if (!stageAlreadySpawned && stageCount > 1)
        {
            bossHP = 1.7 * bossHP;
        }
        else if (stageCount == 1)
        {
            bossHP = 80.0;
        }

        //Spawn a mini boss
        if (!miniBossSpawned)
        {
            GameObject randomMiniBoss = miniBossPrefabs[Random.Range(0, miniBossPrefabs.Count)];
            randomMiniBoss.GetComponent<MiniBossTest>().SetHPMultiplier(stageCount, bossHP, bossKilled);
            spawnedMiniBoss = Instantiate(randomMiniBoss, new Vector2(13f, 0f), Quaternion.identity);
            _bossCountdown.StartTimer();
        }
        stageAlreadySpawned = true;
        miniBossSpawned = true;

        //Spawn the mini boss
        GameObject[] miniBosses = GameObject.FindGameObjectsWithTag("MiniBoss");
        if (spawnedMiniBoss != null && spawnedMiniBoss.GetComponent<MiniBossTest>().CurrentHealth() <= 0)
        {
            _bossCountdown.EndTimer();
        }

        if (miniBosses.Length == 0)
        {
            monstersSpawned = 0;
            miniBossSpawned = false;
            bossMobsSpawned = false;
            bossKilled = true;
            _bossCountdown.EndTimer();
        }

        if (_bossCountdown.GetCounter() <= 0)
        {
            DespawnBossAndRespawnMonsters();
        }
    }

    public void DespawnBossAndRespawnMonsters()
    {
        miniBossSpawned = false;
        bossMobsSpawned = false;
        _fightBossButton.Enable();
        monstersSpawned = 0;

        // Despawn the boss
        GameObject[] bosses = GameObject.FindGameObjectsWithTag("MiniBoss");
        foreach (GameObject boss in bosses)
        {
            Destroy(boss);
        }
        bossKilled = false;

        // Respawn the monsters
        SpawnMonsters(10);
        monstersSpawned = 0;
    }

    public void FightBossPressed()
    {
        if (!bossKilled)
        {
            _fightBossButton.Disable();
            monstersSpawned = 10;

            // Despawn the mobs
            GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
            foreach (GameObject monster in monsters)
            {
                Destroy(monster);
            }

            SpawnMiniBoss();
        }
    }

    public bool BossKilled()
    {
        return bossKilled;
    }

    public double GetMonsterHp()
    {
        return monsterHP;
    }

    public bool BossAlive()
    {
        if (miniBossSpawned)
        {
            return true;
        }
        return false;
    }

    public int GetGameStage()
    {
        return stageCount;
    }
}