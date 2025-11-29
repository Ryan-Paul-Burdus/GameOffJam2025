using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public struct EnemySpawnLocation
{
    public Transform Location;
    public bool IsWithinPlayArea;
}

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    public GameObject[] EnemyPrefabs;
    public EnemySpawnLocation[] EnemySpawnLocations;
    public Transform EnemyHolder;

    [Header("Spawning")]
    public int WaveNumber = 1;

    private float CooldownBetweenWaves = 25f;
    private float CooldownBetweenEnemySpawns = 10f;
    private int TimesToSpawnEnemiesInCurentWave = 5;
    private int EnemiesToSpawnAtOnce = 5;
    private int totalEnemiesInWave;
    private bool canSpawnEnemy = true;
    private bool canSpawnWave = true;

    [Header("Damage")]
    public GameObject DamageIndicatorPrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this) // if we are the instance this is fine
        {
            Destroy(this);
            return;
        }
        Instance = this;

        totalEnemiesInWave = EnemiesToSpawnAtOnce * TimesToSpawnEnemiesInCurentWave;
    }

    private void Update()
    {
        if (PickupManager.Instance.PickupUIVisibile || MenuManager.Instance.IsPaused)
        {
            return;
        }

        if (canSpawnWave)
        {
            if (WaveNumber % 10 == 0)
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

    #region Methods

    public IEnumerator SpawnEnemyWave()
    {
        canSpawnWave = false;
        int enemiesSpawnedThisWave = 0;
        
        // Keep spawning enemies 
        while (enemiesSpawnedThisWave < totalEnemiesInWave)
        {
            if (canSpawnEnemy)
            {
                StartCoroutine(SpawnEnemyCoroutine(EnemiesToSpawnAtOnce));
                enemiesSpawnedThisWave += EnemiesToSpawnAtOnce;
            }
            yield return new WaitForSeconds(CooldownBetweenEnemySpawns);
        }
        
        // Increase values at the end of the wave
        EnemiesToSpawnAtOnce = Mathf.FloorToInt(EnemiesToSpawnAtOnce * 1.2f);
        totalEnemiesInWave = EnemiesToSpawnAtOnce * TimesToSpawnEnemiesInCurentWave;

        yield return new WaitForSeconds(CooldownBetweenWaves);

        WaveNumber++;
        canSpawnWave = true;
    }

    public IEnumerator SpawnEnemyCoroutine(int numberOfEnemiesToSpawn)
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
            Destroy(enemyObject);
        }
    }

    #endregion Methods
}
