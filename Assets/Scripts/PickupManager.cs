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

    public Color CommonRarityColor;
    public Color UncommonRarityColor;
    public Color EpicRarityColor;
    public Color LegendaryRarityColor;

    [Header("Powerups")]
    public float CommonPowerupPercentage { get; private set; } = 50.0f;
    public float UncommonPowerupPercentage { get; private set; } = 35.0f;
    public float EpicPowerupPercentage { get; private set; } = 10.0f;
    public float LegendaryPowerupPercentage { get; private set; } = 5.0f;

    public Powerup[] AllPowerups;

    [Header("Items")]
    public ItemObject ItemPrefab;
    public float ItemSpawnCooldown = 3f;

    private void Awake()
    {
        if (Instance != null && Instance != this) // if we are the instance this is fine
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(Instance);
    }

    
}
