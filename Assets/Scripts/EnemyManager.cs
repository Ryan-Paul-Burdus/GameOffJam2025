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

    #endregion Methods
}
