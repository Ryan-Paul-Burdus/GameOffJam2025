using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayPickupDisplay : MonoBehaviour
{
    [Header("Pickup UI")]
    public GameObject PickupUI;
    public int randomPowerupGUIIsInQueue = 0;

    [Header("Powerups")]
    public PowerupDisplay[] PowerupPickupDisplays;

    [Header("Items")]
    public Transform ItemsHolder;
    public Transform ItemSpawnPointHolder;

    #region Powerups

    private Rarity RollChanceOnRarityTypeForPowerup()
    {
        float randomPercentage = Random.Range(0.0f, 100.0f);

        if (randomPercentage >= PickupManager.Instance.CommonPowerupPercentage)
        {
            return Rarity.Common;
        }
        else if (randomPercentage >= PickupManager.Instance.UncommonPowerupPercentage)
        {
            return Rarity.Uncommon;
        }
        else if (randomPercentage >= PickupManager.Instance.EpicPowerupPercentage)
        {
            return Rarity.Epic;
        }
        else
        {
            return Rarity.Legendary;
        }
    }

    /// <summary>
    /// Spawning callback for testing
    /// </summary>
    /// <param name="context"></param>
    public void Open3RandomPowerups(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            AddNewRandomPowerupGUIToQueue();
        }
    }

    public void AddNewRandomPowerupGUIToQueue()
    {
        Time.timeScale = 0.0f;
        PlayerManager.Instance.PlayerMovement.IsDashing = false;

        if (randomPowerupGUIIsInQueue++ <= 0)
        {
            ShowNewRandom3Powerups();
            PickupUI.SetActive(true);
        }
    }

    public void ShowNewRandom3Powerups()
    {
        for (int i = 0; i < 3; i++)
        {
            Powerup currentPowerup = PickupManager.Instance.AllPowerups[Random.Range(0, PickupManager.Instance.AllPowerups.Length)];
            PowerupPickupDisplays[i].UpdatePowerupDisplay(currentPowerup, RollChanceOnRarityTypeForPowerup());
        }
    }

    public void TakeCurrentPowerupEffect(Powerup currentPowerup, float amount)
    {
        PlayerManager.Instance.Score += (10 * EnemyManager.Instance.WaveNumber);

        // Use the powerup to do its effect
        switch (currentPowerup.EffectType)
        {
            case Powerup.PowerUpEffectType.IncreaseDamage:
                PlayerManager.Instance.IncreaseDamage(amount);
                break;

            case Powerup.PowerUpEffectType.IncreaseHealth:
                PlayerManager.Instance.IncreaseHealth(amount);
                break;

            case Powerup.PowerUpEffectType.IncreaseAttackSpeed:
                PlayerManager.Instance.IncreaseAttackSpeed(amount);
                break;

            case Powerup.PowerUpEffectType.IncreaseProjectileSize:
                PlayerManager.Instance.IncreaseProjectileSize(amount);
                break;

            case Powerup.PowerUpEffectType.IncreaseProjectileCount:
                PlayerManager.Instance.IncreaseProjectileCount((int)amount);
                break;

            case Powerup.PowerUpEffectType.IncreaseAttackAreaSize:
                PlayerManager.Instance.IncreaseAttackAreaOfSize(amount);
                break;

            case Powerup.PowerUpEffectType.IncreaseMoveSpeed:
                PlayerManager.Instance.IncreaseMoveSpeed(amount);
                break;

            case Powerup.PowerUpEffectType.IncreaseDashDistance:
                PlayerManager.Instance.IncreaseDashDistance(amount);
                break;

            case Powerup.PowerUpEffectType.IncreaseDashSpeed:
                PlayerManager.Instance.IncreaseDashSpeed(amount);
                break;

            case Powerup.PowerUpEffectType.ReduceDashCooldown:
                PlayerManager.Instance.ReduceDashCooldown(amount);
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

        // Remove a powerup GUI from the queue and check if there is another to show after
        if (--randomPowerupGUIIsInQueue > 0)
        {
            ShowNewRandom3Powerups();
        }
    }

    #endregion Powerups

    #region Items

    //public IEnumerator SpawnItemCoroutine(int numberToSpawn)
    //{
    //    canSpawnItem = false;

    //    for (int i = 0; i < numberToSpawn; i++)
    //    {
    //        // Gets an un-occupied spawn location and spawns it
    //        ItemSpawnLocation[] availableItemLocations = ItemSpawnLocations.Where(x => !x.Occupied).ToArray();
    //        int randomSpawnIndex = Random.Range(0, availableItemLocations.Count());
    //        ItemSpawnLocation randomSpawnLocation = availableItemLocations[randomSpawnIndex];

    //        ItemObject ItemScript = Instantiate(ItemPrefab,
    //            randomSpawnLocation.Location.position,
    //            Quaternion.identity, ItemsHolder);

    //        ItemScript.SpawnLocationIndex = randomSpawnIndex;
    //        randomSpawnLocation.Occupied = true;
    //        CurrentItemsOnMap++;
    //    }

    //    yield return new WaitForSeconds(ItemSpawnCooldown);
    //    canSpawnItem = true;
    //}

    #endregion Items
}
