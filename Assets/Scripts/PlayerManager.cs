using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    #region Properties

    public static PlayerManager Instance { get; private set; }

    public GameObject Player;
    public PlayerMovement PlayerMovement;

    [Header("Animations")]
    public Sprite[] PlayerAnimationSprites;
    private SpriteRenderer spriteRenderer;
    private int animationIndex = 0;
    private float timer;
    public float timeInterval = 0.15f;
    
    [Header("Health")]
    public Slider HealthSlider;
    public float MaxHealth = 100f;
    private float health;
    public float Health
    {
        get => health;
        set
        {
            health = value;
            HealthSlider.value = health;

            if (!HealthSlider.gameObject.activeSelf && health < MaxHealth)
            {
                HealthSlider.gameObject.SetActive(true);
            }
        }
    }

    [Header("Movement")]
    public float MoveSpeed = 2f;
    public float DashSpeed = 15f;
    public float DashDuration = 0.2f;
    public float DashCooldown = 5f;

    [Header("Attacks")]
    public float Damage = 10f;
    public GameObject AttackAreaObject;
    public PlayerAttacks PlayerAttacks;
    public float AttackCooldown = 1f;
    public float AttackAreaOfSize = 0.2f;

    [Header("Bullets")]
    public float BulletSpeed = 15f;
    public float ProjectileScale = 1f;
    public int TotalProjectileCount = 1;
    public float SpreadAngle = 30f;

    [Header("Score")]
    public TextMeshProUGUI ScoreText;
    private int score = 0;
    public int Score
    {
        get => score;
        set
        {
            score = value;
            ScoreText.text = score.ToString();
        }
    }
   

    #endregion Properties

    #region Unity events

    private void Awake()
    {
        if (Instance != null && Instance != this) // if we are the instance this is fine
        {
            Destroy(this);
            return;
        }
        Instance = this;

        spriteRenderer = Player.GetComponent<SpriteRenderer>();
        PlayerMovement = Player.GetComponent<PlayerMovement>();
        PlayerMovement.DashAbility.MaxCooldown = DashCooldown;

        PlayerAttacks = AttackAreaObject.GetComponent<PlayerAttacks>();
        PlayerAttacks.WaveAttackAbility.MaxCooldown = AttackCooldown;

        HealthSlider.maxValue = MaxHealth;
        Health = MaxHealth;
    }

    private void Update()
    {
        // Baked player animations
        UpdatePlayerAnimation();
    }

    #endregion Unity events

    #region Methods

    private void UpdatePlayerAnimation()
    {
        if (PlayerMovement.IsDashing)
        {
            spriteRenderer.sprite = PlayerAnimationSprites[0];
            timer = 0f;
            animationIndex = 0;
        }
        else
        {
            // Loop through the player animations
            if (PlayerAnimationSprites != null)
            {
                timer += Time.deltaTime;
                if (timer >= timeInterval)
                {
                    timer = 0f;
                    animationIndex++;

                    if (animationIndex >= PlayerAnimationSprites.Length)
                    {
                        animationIndex = 0;
                    }

                    spriteRenderer.sprite = PlayerAnimationSprites[animationIndex];
                }
            }
        }
    }

    #region Powerups

    public void IncreaseHealth(float increasePercentage)
    {
        Health += Health * (increasePercentage / 100);
    }

    public void IncreaseDamage(float increasePercentage)
    {
        Damage += Damage * (increasePercentage / 100);
    }

    public void IncreaseAttackSpeed(float increasePercentage)
    {
        AttackCooldown -= AttackCooldown * (increasePercentage / 100);
    }

    public void IncreaseProjectileSize(float increasePercentage)
    {
        ProjectileScale += ProjectileScale * (increasePercentage / 100);
    }

    public void IncreaseProjectileCount(int projectilesToAdd)
    {
        TotalProjectileCount += projectilesToAdd;
    }

    public void IncreaseAttackAreaOfSize(float increasePercentage)
    {
        AttackAreaOfSize += AttackAreaOfSize * (increasePercentage / 100);
        AttackAreaObject.transform.localScale = new Vector3(AttackAreaOfSize, AttackAreaOfSize, 1);
    }

    public void IncreaseMoveSpeed(float increasePercentage)
    {
        MoveSpeed += MoveSpeed * (increasePercentage / 100);
    }

    public void IncreaseDashDistance(float increaseAmount)
    {
        DashDuration += increaseAmount / 10;
    }

    public void IncreaseDashSpeed(float increasePercentage)
    {
        DashSpeed += DashSpeed * (increasePercentage / 100);
    }

    public void ReduceDashCooldown(float reductionPrecentage)
    {
        DashCooldown -= DashCooldown * (reductionPrecentage / 100);
        PlayerMovement.DashAbility.MaxCooldown = DashCooldown;
    }

    #endregion Powerups

    #region Damage player

    public void DamagePlayer(float damageAmount)
    {
        DamageIndicatorManager.Instance.SpawnDamageIndicator(Player, damageAmount);
        Health -= damageAmount;

        if (Health <= 0)
        {
            Health = 0;
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        // Do anything needed before killing the player

        //End the game
        GameplayUIManager.Instance.OpenGameOverScreen(ScoreText.text);
    }

    #endregion Damage player

    #endregion Methods
}
