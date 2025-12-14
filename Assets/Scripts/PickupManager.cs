using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PickupType
{
    Powerup,
}

[System.Serializable]
public class PowerupSpawnLocation
{
    public PowerupSpawnLocation(Transform spawnLocation, bool occupied)
    {
        Location = spawnLocation;
        Occupied = occupied;
    }

    public Transform Location { get; }
    public bool Occupied { get; set; }
}

public class PickupManager : MonoBehaviour
{
    private static readonly WaitForSeconds waitFor2Seconds = new(2f);

    public static PickupManager Instance { get; private set; }

    [Header("Pickup UI")]
    public PickupUIDisplay PickupUI;

    [Header("Powerups")]
    public GameObject PowerupPrefab;
    public Powerup[] AllPowerups;
    public Transform PowerupHolder;
    public Transform PowerupSpawnHolder;
    public int MaxPowerupsAllowedOnMap = 50;
    public int CurrentPowerupsOnMap; 
    public float PowerupSpawnCooldown = 3f;

    private List<PowerupSpawnLocation> PowerupSpawnLocations = new();
    private bool canSpawnPowerup = true;
    private Powerup currentPowerup;


    private void Awake()
    {
        if (Instance != null && Instance != this) // if we are the instance this is fine
        {
            Destroy(this);
            return;
        }
        Instance = this;

        // Fill the powerup spawn locations
        PowerupSpawnLocations.Clear();
        foreach (Transform spawnLocation in PowerupSpawnHolder)
        {
            PowerupSpawnLocations.Add(new PowerupSpawnLocation(spawnLocation, false));
        }

        StartCoroutine(SpawnPowerupCoroutine(MaxPowerupsAllowedOnMap));
    }

    private void Update()
    {
        if (canSpawnPowerup && CurrentPowerupsOnMap < MaxPowerupsAllowedOnMap)
        {
            StartCoroutine(SpawnPowerupCoroutine(1));
        }
    }

    #region Powerups

    public IEnumerator SpawnPowerupCoroutine(int numberToSpawn)
    {
        canSpawnPowerup = false;

        for (int i = 0; i < numberToSpawn; i++)
        {
            // Gets an un-occupied spawn location and spawns it
            PowerupSpawnLocation[] availablePowerupLocations = PowerupSpawnLocations.Where(x => !x.Occupied).ToArray();
            int randomSpawnIndex = Random.Range(0, availablePowerupLocations.Count());
            PowerupSpawnLocation randomSpawnLocation = availablePowerupLocations[randomSpawnIndex];

            PowerupObject powerUpObjectScript = Instantiate(PowerupPrefab,
                randomSpawnLocation.Location.position,
                Quaternion.identity, PowerupHolder).GetComponent<PowerupObject>();

            powerUpObjectScript.SpawnLocationIndex = randomSpawnIndex;
            randomSpawnLocation.Occupied = true;
            CurrentPowerupsOnMap++;
        }

        yield return new WaitForSeconds(PowerupSpawnCooldown);
        canSpawnPowerup = true;
    }

    public IEnumerator PickupRandomPowerupCoroutine(int spawnLocationIndex)
    {
        // Get a random powerup to pick up and pick it up
        currentPowerup = AllPowerups[Random.Range(0, AllPowerups.Length)];
        Time.timeScale = 0.0f;
        PickupUI.ShowPowerupDisplay(currentPowerup);

        yield return waitFor2Seconds;

        // Make the space unoccupied after pickup
        PowerupSpawnLocations[spawnLocationIndex].Occupied = false;
    }

    public void TakeCurrentPowerupEffect()
    {
        PlayerManager.Instance.Score += (10 * EnemyManager.Instance.WaveNumber);

        // Use the powerup to do its effect
        switch (currentPowerup.EffectType)
        {
            case Powerup.PowerUpEffectType.IncreaseDamage:
                PlayerManager.Instance.IncreaseDamage(currentPowerup.Amount);
                break;

            case Powerup.PowerUpEffectType.IncreaseHealth:
                PlayerManager.Instance.IncreaseHealth(currentPowerup.Amount);
                break;

            case Powerup.PowerUpEffectType.IncreaseAttackSpeed:
                PlayerManager.Instance.IncreaseAttackSpeed(currentPowerup.Amount);
                break;

            case Powerup.PowerUpEffectType.IncreaseProjectileSize:
                PlayerManager.Instance.IncreaseProjectileSize(currentPowerup.Amount);
                break;

            case Powerup.PowerUpEffectType.IncreaseProjectileCount:
                PlayerManager.Instance.IncreaseProjectileCount((int)currentPowerup.Amount);
                break;

            case Powerup.PowerUpEffectType.IncreaseAttackAreaSize:
                PlayerManager.Instance.IncreaseAttackAreaOfSize(currentPowerup.Amount);
                break;

            case Powerup.PowerUpEffectType.IncreaseMoveSpeed:
                PlayerManager.Instance.IncreaseMoveSpeed(currentPowerup.Amount);
                break;

            case Powerup.PowerUpEffectType.IncreaseDashDistance:
                PlayerManager.Instance.IncreaseDashDistance(currentPowerup.Amount);
                break;

            case Powerup.PowerUpEffectType.IncreaseDashSpeed:
                PlayerManager.Instance.IncreaseDashSpeed(currentPowerup.Amount);
                break;

            case Powerup.PowerUpEffectType.ReduceDashCooldown:
                PlayerManager.Instance.ReduceDashCooldown(currentPowerup.Amount);
                break;

            case Powerup.PowerUpEffectType.IncreaseShockChance:
                break;

            case Powerup.PowerUpEffectType.IncreaseShockDamage:
                break;

            case Powerup.PowerUpEffectType.IncreaseCritChance:
                break;

            case Powerup.PowerUpEffectType.IncreaseCritDamage:
                break;

            case Powerup.PowerUpEffectType.IncreaseDifficulty:
                break;
        }

        CurrentPowerupsOnMap--;
    }

    #endregion Powerups
}
