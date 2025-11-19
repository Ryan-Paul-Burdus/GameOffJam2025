using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum PickupType
{
    Powerup,
}

public class PickupManager : MonoBehaviour
{
    public static PickupManager Instance { get; private set; }

    [Header("Pickup UI")]
    public PickupUIDisplay PickupUI;
    public bool PickupUIVisibile = false;

    [Header("Powerups")]
    public Powerup[] AllPowerups;
    public GameObject PowerupPrefab;
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

    #region Powerups

    public void PickupRandomPowerup()
    {
        currentPowerup = AllPowerups[Random.Range(0, AllPowerups.Length)];
        //currentPowerup = AllPowerups[1];
        PickupUIVisibile = true;
        PickupUI.ShowPowerupDisplay(currentPowerup);
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
                break;

            case Powerup.PowerUpEffectType.ReduceDashCooldown:
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
    }

    #endregion Powerups
}
