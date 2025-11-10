using UnityEngine;
using UnityEngine.UI;

public class PowerupManager : MonoBehaviour
{
    public static PowerupManager Instance { get; private set; }

    public Powerup[] AllPowerups;

    public GameObject PowerupPrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this) // if we are the instance this is fine
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void PickupRandomPowerup()
    {
        Powerup randomPowerup = AllPowerups[Random.Range(0, AllPowerups.Length)];

        // Use the powerup to do its effect
        if (randomPowerup.EffectType is Powerup.PowerUpEffectType.IncreaseDamage)
        {
            PlayerManager.Instance.IncreaseDamage(randomPowerup.Amount);
        }
        else if (randomPowerup.EffectType is Powerup.PowerUpEffectType.IncreaseHealth)
        {
            PlayerManager.Instance.IncreaseHealth(randomPowerup.Amount);
        }
    }
}
