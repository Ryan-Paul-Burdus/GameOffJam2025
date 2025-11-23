using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum PickupType
{
    Powerup,
}

[System.Serializable]
public struct PowerupSpawnLocation
{
    public Transform Location;
    public bool Occupied;
}

public class PickupManager : MonoBehaviour
{
    public static PickupManager Instance { get; private set; }

    [Header("Pickup UI")]
    public PickupUIDisplay PickupUI;
    public bool PickupUIVisibile = false;

    [Header("Powerups")]
    public GameObject PowerupPrefab;
    public Powerup[] AllPowerups;
    public PowerupSpawnLocation[] PowerupSpawnLocations;
    public int MaxPowerupsAllowedOnMap = 10;
    public int CurrentPowerupsOnMap = 0; 
    public float PowerupSpawnCooldown = 5f;
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
    }

    private void Update()
    {
        if (PickupUIVisibile)
        {
            return;
        }

        if (canSpawnPowerup && CurrentPowerupsOnMap < MaxPowerupsAllowedOnMap)
        {
            StartCoroutine(SpawnPowerupCoroutine());
        }
    }

    #region Powerups

    public IEnumerator SpawnPowerupCoroutine()
    {
        canSpawnPowerup = false;

        // Gets an un-occupied spawn location and spawns it
        List<PowerupSpawnLocation> availablePowerupLocations = PowerupSpawnLocations.Where(x => !x.Occupied).ToList();
        int randomSpawnIndex = Random.Range(0, availablePowerupLocations.Count());
        PowerupSpawnLocation randomSpawnLocation = availablePowerupLocations[randomSpawnIndex];

        PowerupObject powerUpObjectScript = Instantiate(PowerupPrefab,
            randomSpawnLocation.Location.position,
            Quaternion.identity).GetComponent<PowerupObject>();

        powerUpObjectScript.SpawnLocationIndex = randomSpawnIndex;
        randomSpawnLocation.Occupied = true;
        CurrentPowerupsOnMap++;

        yield return new WaitForSeconds(PowerupSpawnCooldown);
        canSpawnPowerup = true;
    }

    public IEnumerator PickupRandomPowerupCoroutine(int spawnLocationIndex)
    {
        // Get a random powerup to pick up and pick it up
        currentPowerup = AllPowerups[Random.Range(0, AllPowerups.Length)];
        //currentPowerup = AllPowerups[1];
        PickupUIVisibile = true;
        PickupUI.ShowPowerupDisplay(currentPowerup);

        yield return new WaitForSeconds(2f);

        // Make the space unoccupied after pickup
        PowerupSpawnLocations[spawnLocationIndex].Occupied = false;
    }

    public void TakeCurrentPowerupEffect()
    {
        Debug.Log(currentPowerup.EffectType);

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
