using NUnit.Framework;
using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    public GameObject[] EnemyPrefabs;
    public Transform[] EnemySpawnLocations;

    private bool canSpawn = true;
    public float SpawnCooldown = 5f;

    public GameObject DamageIndicatorPrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this) // if we are the instance this is fine
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        if (PickupManager.Instance.PickupUIVisibile || MenuManager.Instance.IsPaused)
        {
            return;
        }

        if (canSpawn)
        {
            StartCoroutine(SpawnEnemyCoroutine());
        }
    }

    #region Methods

    public IEnumerator SpawnEnemyCoroutine()
    {
        canSpawn = false;

        Instantiate(EnemyPrefabs[Random.Range(0, EnemyPrefabs.Length)],
            EnemySpawnLocations[Random.Range(0, EnemySpawnLocations.Length)].position,
            Quaternion.identity);

        yield return new WaitForSeconds(SpawnCooldown);
        canSpawn = true;
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
