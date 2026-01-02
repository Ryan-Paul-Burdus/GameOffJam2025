using UnityEngine;

[CreateAssetMenu(fileName = "Powerup", menuName = "Scriptable Objects/Powerup")]
public class Powerup : ScriptableObject
{
    public string Name;
    public string Description;

    public Sprite Image;

    // Have a drop down to pick from different effect types.
    // Then have the different effect properties show depending on what effect is selected
    #region Effects

    public enum PowerUpEffectType
    {
        IncreaseDamage,
        IncreaseHealth,
        IncreaseAttackSpeed,
        IncreaseProjectileSize,
        IncreaseProjectileCount,
        IncreaseAttackAreaSize,
        IncreaseMoveSpeed,
        IncreaseDashDistance,
        IncreaseDashSpeed,
        ReduceDashCooldown,
        IncreaseShockChance,
        IncreaseShockDamage,
        IncreaseCritChance,
        IncreaseCritDamage,
        IncreaseDifficulty
    }
    public PowerUpEffectType EffectType;

    [Header("Rarity amounts")]
    public float CommonAmount = 5.0f;
    public float UncommonAmount = 10.0f;
    public float EpicAmount = 20.0f;
    public float LegendaryAmount = 35.0f;

    #endregion Effects
}
