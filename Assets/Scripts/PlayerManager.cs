using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    #region Properties

    public static PlayerManager Instance { get; private set; }

    public GameObject Player;
    private PlayerMovement playerMovement;

    public GameObject DamageIndicatorPrefab;

    [Header("Animations")]
    public List<Sprite> PlayerAnimationSprites;
    private SpriteRenderer spriteRenderer;
    private int animationIndex = 0;
    private float timer;
    public float timeInterval = 0.15f;
    
    [Header("Damage")]
    public float Damage = 10f;

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
    public GameObject AttackAreaObject;
    public float AttackCooldown = 1.5f;
    public float AttackAreaOfSize = 0.2f;
    
    [Header("Bullets")]
    public GameObject BulletPrefab;
    public bool canAttack = true;
    public float BulletSpeed = 10f;
    public float ProjectileScale = 1f;
    public int TotalProjectileCount = 1;
    public float SpreadAngle = 50f;

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
        playerMovement = Player.GetComponent<PlayerMovement>();

        Health = MaxHealth;
        HealthSlider.maxValue = MaxHealth;
    }

    private void Update()
    {
        if (PickupManager.Instance.PickupUIVisibile || MenuManager.Instance.IsPaused)
        {
            return;
        }

        // Baked player animations
        UpdatePlayerAnimation();
    }

    #endregion Unity events

    #region Methods

    private void UpdatePlayerAnimation()
    {
        if (playerMovement.IsDashing)
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

                    if (animationIndex >= PlayerAnimationSprites.Count)
                    {
                        animationIndex = 0;
                    }

                    spriteRenderer.sprite = PlayerAnimationSprites[animationIndex];
                }
            }
        }
    }

    #region Enemies

    public IEnumerator AttackEnemyCoroutine(GameObject currentTarget)
    {
        canAttack = false;

        float angleStep = SpreadAngle / TotalProjectileCount;
        float centeringOffset = (SpreadAngle / 2) - (angleStep / 2);
        float lookRotation = Quaternion.LookRotation(Vector3.forward, (currentTarget.transform.position - Player.transform.position).normalized).eulerAngles.z;

        for (int i = 0; i < TotalProjectileCount; i++)
        {
            float currentBulletAngle = angleStep * i;
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, lookRotation + currentBulletAngle - centeringOffset));

            // Fire bullet to enemy
            GameObject bulletObj = Instantiate(BulletPrefab, Player.transform.position, rotation);
            bulletObj.transform.localScale = new Vector2(ProjectileScale, ProjectileScale);

            Bullet bullet = bulletObj.GetComponent<Bullet>();
            bullet.Target = currentTarget;
        }

        // Startthe cooldown for next shot
        yield return new WaitForSeconds(AttackCooldown);

        canAttack = true;
    }

    #endregion Enemies

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
    }

    #endregion Powerups

    #region Damage player

    public void DamagePlayer(float damageAmount)
    {
        // Create damage indicator
        DamageIndicator indicator = Instantiate(DamageIndicatorPrefab, Player.transform.position, Quaternion.identity).GetComponent<DamageIndicator>();
        indicator.SetDamageText(damageAmount);

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
        MenuManager.Instance.OpenGameOverMenu();
    }

    #endregion Damage player

    #endregion Methods
}
