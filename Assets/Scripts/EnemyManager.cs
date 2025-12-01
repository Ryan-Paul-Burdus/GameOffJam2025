using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
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

    public GameObject[] EnemyPrefabs;
    public EnemySpawnLocation[] EnemySpawnLocations;
    public Transform EnemyHolder;

    [Header("Spawning")]
    public int WaveNumber = 1;
    public bool IsBossWave => false;// WaveNumber % 10 == 0;

    private float CooldownBetweenEnemySpawns = 10f;
    private int TimesToSpawnEnemiesInCurentWave = 5;
    private int EnemiesToSpawnAtOnce = 5;
    private int totalEnemiesInWave;
    private bool canSpawnEnemy = true;
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
    }

    private void Update()
    {
        if (PickupManager.Instance.PickupUIVisibile || MenuManager.Instance.IsPaused)
        {
            return;
        }

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
                StartCoroutine(SpawnEnemyWave());
            }
        }
    }

    #endregion Unity methods

    #region Methods

    private IEnumerator SpawnEnemyWave()
    {
        canSpawnWave = false;

        // Keep spawning enemies 
        int enemiesSpawnedThisWave = 0;
        while (enemiesSpawnedThisWave < totalEnemiesInWave)
        {
            if (canSpawnEnemy)
            {
                StartCoroutine(SpawnEnemyCoroutine(EnemiesToSpawnAtOnce));
                enemiesSpawnedThisWave += EnemiesToSpawnAtOnce;
            }
            yield return new WaitForSeconds(CooldownBetweenEnemySpawns);
        }
    }

    private IEnumerator SpawnEnemyCoroutine(int numberOfEnemiesToSpawn)
    {
        canSpawnEnemy = false;

        List<EnemySpawnLocation> possibleLocationsToSpawn = EnemySpawnLocations.Where(x => x.IsWithinPlayArea).ToList();

        if (possibleLocationsToSpawn.Count > 0)
        {
            for (int i = 0; i < numberOfEnemiesToSpawn; i++)
            {
                NavMeshHit hit;
                EnemySpawnLocation location = possibleLocationsToSpawn[Random.Range(0, possibleLocationsToSpawn.Count)];

                if (NavMesh.SamplePosition(location.Location.position, out hit, 15f, NavMesh.AllAreas))
                {
                    GameObject enemy = Instantiate(EnemyPrefabs[Random.Range(0, EnemyPrefabs.Length)], EnemyHolder);
                    enemy.GetComponent<Enemy>().agent.Warp(hit.position);

                }

                possibleLocationsToSpawn.Remove(location);
            }
        }

        yield return new WaitForSeconds(CooldownBetweenEnemySpawns);
        canSpawnEnemy = true;
    }

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
            KillEnemy(enemyObject);
        }

        EnemyTakingDamage = false;
    }

    public void KillEnemy(GameObject enemyObject)
    {
        Destroy(enemyObject);

        EnemiesLeftInWave--;
        PlayerManager.Instance.Score += Mathf.FloorToInt(enemyObject.GetComponent<Enemy>().MaxHealth);
    }

    #endregion Methods
}
