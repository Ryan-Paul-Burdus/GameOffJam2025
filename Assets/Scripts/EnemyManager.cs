using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using UnityEngine.UI;

[System.Serializable]
public struct EnemySpawnLocation
{
    public Transform Location;
    public bool IsWithinPlayArea;
}

public class EnemyManager : MonoBehaviour
{
    #region Properties

    public static EnemyManager Instance { get; private set; }

    public Enemy EnemyScript;
    public EnemySpawnLocation[] EnemySpawnLocations;
    public Transform EnemyHolder;
    public ObjectPool<Enemy> EnemyPool;


    [Header("Spawning")]
    public int WaveNumber = 1;
    public bool IsBossWave => false;// WaveNumber % 10 == 0;

    private float CooldownBetweenEnemySpawns = 10f;
    private int TimesToSpawnEnemiesInCurentWave = 5;
    private int EnemiesToSpawnAtOnce = 5;
    private int totalEnemiesInWave;
    private bool canSpawnWave = true;

    public float enemyStatsMultiplier => 1f + (0.15f * (WaveNumber - 1));

    [Header("Wave slider properties")]
    public TextMeshProUGUI WaveText;
    public Slider WaveSlider;

    [SerializeField] private int enemiesLeftInWave;
    public int EnemiesLeftInWave
    {
        get => enemiesLeftInWave;
        set
        {
            enemiesLeftInWave = value;
            WaveSlider.value = enemiesLeftInWave;

            int numberNeededToShowEnemiesLeft = Mathf.CeilToInt((totalEnemiesInWave / 100.0f) * 20.0f);

            if (enemiesLeftInWave < numberNeededToShowEnemiesLeft)
            {
                WaveText.text = $"{enemiesLeftInWave} remaining";

                if (!WaveText.enabled)
                {
                    WaveText.enabled = true;
                }
            }
            else if (WaveText.enabled)
            {
                WaveText.enabled = false;
            }
        }
    }

    [Header("Damage")]
    public GameObject DamageIndicatorPrefab;
    public bool EnemyTakingDamage = false;

    #endregion Properties

    #region Unity methods

    private void Awake()
    {
        if (Instance != null && Instance != this) // if we are the instance this is fine
        {
            Destroy(this);
            return;
        }
        Instance = this;

        totalEnemiesInWave = EnemiesToSpawnAtOnce * TimesToSpawnEnemiesInCurentWave;
        WaveSlider.maxValue = totalEnemiesInWave;
        EnemiesLeftInWave = totalEnemiesInWave;

        //TODO: Make multiple pools, for each enemy type (when needed)
        // Set up the enemy pool and its events
        EnemyPool = new ObjectPool<Enemy>(CreatePooledEnemyObject, GetEnemyFromPool, ReturnEnemyToPool, null, false, 200, 50_000);
    }

    private void Update()
    {
        // Increase values at the end of the wave
        if (EnemiesLeftInWave <= 0)
        {
            if (!IsBossWave)
            {
                EnemiesToSpawnAtOnce = Mathf.FloorToInt(EnemiesToSpawnAtOnce * 1.2f);
                totalEnemiesInWave = EnemiesToSpawnAtOnce * TimesToSpawnEnemiesInCurentWave;
                WaveSlider.maxValue = totalEnemiesInWave;
                EnemiesLeftInWave = totalEnemiesInWave;
                WaveNumber++;
            }

            canSpawnWave = true;
        }

        // Start next wave if possible
        if (canSpawnWave)
        {
            if (IsBossWave)
            {
                // Spawn a boss wave 
            }
            else
            {
                // Spawn a normal wave
                StartCoroutine(SpawnEnemyWaveCoroutine());
            }
        }
    }

    #endregion Unity methods

    #region Methods

    #region Spawning

    #region Enemy pooling

    /// <summary>
    /// Spawning callback for testing
    /// </summary>
    /// <param name="context"></param>
    public void GetEnemy(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            EnemyPool.Get();
        }
    }

    /// <summary>
    /// Called when enemyPool.Get is called and there is no more instances available, thus creating anew enemy to add to the pool
    /// </summary>
    /// <returns>A new enemy for the pool</returns>
    private Enemy CreatePooledEnemyObject()
    {
        Enemy enemy = Instantiate(EnemyScript, EnemyHolder);
        enemy.gameObject.SetActive(false);
        return enemy;
    }

    /// <summary>
    /// Called when enemyPool.Get is called, enables and spawns an enemy from the pool
    /// </summary>
    /// <param name="enemy">The enemy to spawn</param>
    private void GetEnemyFromPool(Enemy enemy)
    {
        SetSpawnedEnemyLocation(enemy);
        enemy.gameObject.SetActive(true);
    }

    /// <summary>
    /// Called when enemyPool.Release is called, sends back enemy to the pool
    /// </summary>
    /// <param name="enemy">The enemy to return to the pool</param>
    private void ReturnEnemyToPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    #endregion Enemy pooling

    /// <summary>
    /// Spawns a wave of enemies
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnEnemyWaveCoroutine()
    {
        canSpawnWave = false;

        // Keep spawning enemies 
        int enemiesSpawnedThisWave = 0;
        while (enemiesSpawnedThisWave < totalEnemiesInWave)
        {
            for (int i = 0; i < EnemiesToSpawnAtOnce; i++)
            {
                EnemyPool.Get();
            }
            enemiesSpawnedThisWave += EnemiesToSpawnAtOnce;

            yield return new WaitForSeconds(CooldownBetweenEnemySpawns);
        }
    }

    /// <summary>
    /// Sets the spawn point of an enemy to a random spawn point where possible 
    /// </summary>
    /// <param name="enemy">The enemy to spawn</param>
    private void SetSpawnedEnemyLocation(Enemy enemy)
    {
        List<EnemySpawnLocation> possibleLocationsToSpawn = EnemySpawnLocations.Where(x => x.IsWithinPlayArea).ToList();

        if (possibleLocationsToSpawn.Count > 0)
        {
            NavMeshHit hit;
            EnemySpawnLocation location = possibleLocationsToSpawn[Random.Range(0, possibleLocationsToSpawn.Count)];

            if (NavMesh.SamplePosition(location.Location.position, out hit, 15f, NavMesh.AllAreas))
            {
                enemy.agent.Warp(hit.position);
            }
        }
    }

    #endregion Spawning

    #region Damage

    /// <summary>
    /// Damages an enemy 
    /// </summary>
    /// <param name="enemyObject">The enemy object being damaged</param>
    public void TakeDamage(GameObject enemyObject)
    {
        float damage = PlayerManager.Instance.Damage;

        // Create damage indicator
        DamageIndicator indicator = Instantiate(DamageIndicatorPrefab, enemyObject.transform.position, Quaternion.identity).GetComponent<DamageIndicator>();
        indicator.SetDamageText(damage);

        // Do damage to the enemy
        Enemy enemyScript = enemyObject.GetComponent<Enemy>();
        enemyScript.Health -= damage;

        if (enemyScript.Health <= 0)
        {
            KillEnemy(enemyScript);
        }

        EnemyTakingDamage = false;
    }

    /// <summary>
    /// Kills an enemy
    /// </summary>
    /// <param name="enemy"></param>
    public void KillEnemy(Enemy enemy)
    {
        EnemyPool.Release(enemy);

        EnemiesLeftInWave--;
        PlayerManager.Instance.Score += Mathf.FloorToInt(enemy.MaxHealth);
    }

    #endregion Damage

    #endregion Methods
}
