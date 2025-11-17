using NUnit.Framework;
using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    public GameObject[] EnemyPrefabs;
    public Transform[] SpawnPoints;

    private bool canSpawn = true;
    public float SpawnCooldown = 2f;

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
        if (PickupManager.Instance.PickupUIVisibile)
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
            SpawnPoints[Random.Range(0, SpawnPoints.Length)].position,
            Quaternion.identity);

        yield return new WaitForSeconds(SpawnCooldown);
        canSpawn = true;
    }

    public void TakeDamage(GameObject enemyObject)
    {
        DamageIndicator indicator = Instantiate(DamageIndicatorPrefab, enemyObject.transform.position, Quaternion.identity).GetComponent<DamageIndicator>();
        indicator.SetDamageText(PlayerManager.Instance.Damage);

        Destroy(enemyObject);
    }

    #endregion Methods
}
