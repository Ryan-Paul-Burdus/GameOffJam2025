using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PickupType
{
    Powerup,
}

public enum Rarity
{
    Common,
    Uncommon,
    Epic,
    Legendary
}

[System.Serializable]
public struct ItemSpawnLocation
{
    public ItemSpawnLocation(Transform spawnLocation, bool occupied)
    {
        Location = spawnLocation;
        Occupied = occupied;
    }

    public Transform Location { get; }
    public bool Occupied { get; set; }
}

public class PickupManager : MonoBehaviour
{

    public static PickupManager Instance { get; private set; }

    [Header("Pickup UI")]
    public GameObject PickupUI;

    public Color CommonRarityColor;
    public Color UncommonRarityColor;
    public Color EpicRarityColor;
    public Color LegendaryRarityColor;

    [Header("Powerups")]
    public PowerupDisplay[] PowerupPickupDisplays;
    public Powerup[] AllPowerups;

    private float commonPowerupPercentage = 50.0f;
    private float uncommonPowerupPercentage = 35.0f;
    private float epicPowerupPercentage = 10.0f;
    private float legendaryPowerupPercentage = 5.0f;

    [Header("Items")]
    public ItemObject ItemPrefab;
    public Transform ItemsHolder;
    public Transform ItemSpawnPointHolder;
    public int MaxItemsAllowedOnMap = 50;
    public int CurrentItemsOnMap;
    public float ItemSpawnCooldown = 3f;

    private readonly List<ItemSpawnLocation> ItemSpawnLocations = new();
    private bool canSpawnItem = true;


    private void Awake()
    {
        if (Instance != null && Instance != this) // if we are the instance this is fine
        {
            Destroy(this);
            return;
        }
        Instance = this;

        // Fill the powerup spawn locations
        ItemSpawnLocations.Clear();
        foreach (Transform spawnLocation in ItemSpawnPointHolder)
        {
            ItemSpawnLocations.Add(new ItemSpawnLocation(spawnLocation, false));
        }

        StartCoroutine(SpawnItemCoroutine(MaxItemsAllowedOnMap));
    }

    private void Update()
    {
        if (canSpawnItem && CurrentItemsOnMap < MaxItemsAllowedOnMap)
        {
            StartCoroutine(SpawnItemCoroutine(1));
        }
    }

    #region Powerups

    private Rarity RollChanceOnRarityTypeForPowerup()
    {
        float randomPercentage = Random.Range(0.0f, 100.0f);

        if (randomPercentage >= commonPowerupPercentage)
        {
            return Rarity.Common;
        }
        else if (randomPercentage >= uncommonPowerupPercentage)
        {
            return Rarity.Uncommon;
        }
        else if (randomPercentage >= epicPowerupPercentage)
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
            Show3RandomPowerups();
        }
    }

    public void Show3RandomPowerups()
    {
        Time.timeScale = 0.0f;
        PlayerManager.Instance.PlayerMovement.IsDashing = false;

        // Show 3 random powerups
        for (int i = 0; i < 3; i++)
        {
            Powerup currentPowerup = AllPowerups[Random.Range(0, AllPowerups.Length)];
            PowerupPickupDisplays[i].UpdatePowerupDisplay(currentPowerup, RollChanceOnRarityTypeForPowerup());
        }
        PickupUI.SetActive(true);

        //// Make the space unoccupied after pickup
        //PowerupSpawnLocations[spawnLocationIndex].Occupied = false;
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

        CurrentItemsOnMap--;
    }

    #endregion Powerups

    #region Items

    public IEnumerator SpawnItemCoroutine(int numberToSpawn)
    {
        canSpawnItem = false;

        for (int i = 0; i < numberToSpawn; i++)
        {
            // Gets an un-occupied spawn location and spawns it
            ItemSpawnLocation[] availableItemLocations = ItemSpawnLocations.Where(x => !x.Occupied).ToArray();
            int randomSpawnIndex = Random.Range(0, availableItemLocations.Count());
            ItemSpawnLocation randomSpawnLocation = availableItemLocations[randomSpawnIndex];

            ItemObject ItemScript = Instantiate(ItemPrefab,
                randomSpawnLocation.Location.position,
                Quaternion.identity, ItemsHolder);

            ItemScript.SpawnLocationIndex = randomSpawnIndex;
            randomSpawnLocation.Occupied = true;
            CurrentItemsOnMap++;
        }

        yield return new WaitForSeconds(ItemSpawnCooldown);
        canSpawnItem = true;
    }

    #endregion Items
}
