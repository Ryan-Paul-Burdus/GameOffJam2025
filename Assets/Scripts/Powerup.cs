using UnityEngine;

[CreateAssetMenu(fileName = "Powerup", menuName = "Scriptable Objects/Powerup")]
public class Powerup : ScriptableObject
{
    public string Name;
    public string Description;

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

    public float Amount;
    public Sprite Image;

    #endregion Effects
}
