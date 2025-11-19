using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public GameObject Player;
    public GameObject AttackObject;

    public TextMeshProUGUI DamageText;
    public float Damage = 5f;

    public TextMeshProUGUI HealthText;
    public float Health = 100f;

    [Header("Movement")]
    public float MoveSpeed = 2f;
    public float DashSpeed = 10f;
    public float DashDuration = 0.1f;
    public float DashCooldown = 1f;

    [Header("Attacks")]
    public float AttackCooldown = 1.5f;
    public float AttackAreaOfSize = 3.5f;
    
    [Header("Bullets")]
    public GameObject BulletPrefab;
    public bool canAttack = true;
    public float BulletSpeed = 2f;
    public float ProjectileScale = 0.05f;
    public int TotalProjectileCount = 1;
    public float SpreadAngle = 60f;

    private void Awake()
    {
        if (Instance != null && Instance != this) // if we are the instance this is fine
        {
            Destroy(this);
            return;
        }
        Instance = this;

        HealthText.text = Health.ToString();
        DamageText.text = Damage.ToString();
    }

    #region Methods

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

    public void IncreaseHealth(float increasePercentage)
    {
        Health += Health * (increasePercentage / 100);
        HealthText.text = Health.ToString();
    }

    public void IncreaseDamage(float increasePercentage)
    {
        Damage += Damage * (increasePercentage / 100);
        DamageText.text = Damage.ToString();
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
        AttackObject.transform.localScale = new Vector3(AttackAreaOfSize, AttackAreaOfSize, 1);
    }

    public void IncreaseMoveSpeed(float increasePercentage)
    {
        MoveSpeed += MoveSpeed * (increasePercentage / 100);
    }

    public void IncreaseDashDistance(float increaseAmount)
    {
        DashDuration += increaseAmount;
    }

    public void DamagePlayer(float damageAmount)
    {
        Health -= damageAmount;

        if (Health < 0)
        {
            Health = 0;
            HealthText.text = Health.ToString();
            KillPlayer();
        }
        else
        {
            HealthText.text = Health.ToString();
        }
    }

    private void KillPlayer()
    {
        //End the game
    }

    #endregion Methods
}
