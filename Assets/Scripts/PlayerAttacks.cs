using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEditor.FilePathAttribute;

public class PlayerAttacks : MonoBehaviour
{
    private readonly List<GameObject> targets = new();
    private GameObject currentTarget;

    public Bullet BulletScript;
    public ObjectPool<Bullet> BulletPool;

    private int currentBulletIndex;
    private float angleStep;
    private float centeringOffset;
    private float lookRotation;

    public bool CanAttack = true;
    public bool AttackCoolingDown;
    public AbilityObject WaveAttackAbility;

    private void Awake()
    {
        BulletPool = new ObjectPool<Bullet>(CreatePooledBulletObject, GetBulletFromPool, ReturnBulletToPool, null, false, 50, 500);
    }

    private void Update()
    {
        // Attack an enemy if possible
        if (!WaveAttackAbility.IsCoolingDown && targets.Count > 0)
        {
            FindNearestEnemy();

            if (currentTarget != null)
            {

                angleStep = PlayerManager.Instance.SpreadAngle / PlayerManager.Instance.TotalProjectileCount;
                centeringOffset = (PlayerManager.Instance.SpreadAngle / 2) - (angleStep / 2);
                lookRotation = Quaternion.LookRotation(Vector3.forward, (currentTarget.transform.position - PlayerManager.Instance.Player.transform.position).normalized).eulerAngles.z;

                for (int i = 0; i < PlayerManager.Instance.TotalProjectileCount; i++)
                {
                    currentBulletIndex = i;
                    BulletPool.Get();
                }

                WaveAttackAbility.IsCoolingDown = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            targets.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            targets.Remove(collision.gameObject);
        }
    }

    #region Methods

    #region Bullet pooling

    private Bullet CreatePooledBulletObject()
    {
        Bullet bullet = Instantiate(BulletScript, PlayerManager.Instance.Player.transform.position, Quaternion.identity);
        bullet.gameObject.SetActive(false);
        return bullet;
    }

    private void GetBulletFromPool(Bullet bullet)
    {
        float currentBulletAngle = angleStep * currentBulletIndex;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, lookRotation + currentBulletAngle - centeringOffset));

        bullet.transform.SetPositionAndRotation(PlayerManager.Instance.Player.transform.position, rotation);
        bullet.transform.localScale = new Vector2(PlayerManager.Instance.ProjectileScale, PlayerManager.Instance.ProjectileScale);

        bullet.gameObject.SetActive(true);
    }

    private void ReturnBulletToPool(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    #endregion Bullet pooling

    private void FindNearestEnemy()
    {
        if (targets.Count <= 0)
        {
            return;
        }

        GameObject closestEnemy = targets[0];
        float closestDistance = 9999;
        foreach (GameObject target in targets)
        {
            float currentTargetDistance = (target.transform.position - gameObject.transform.position).magnitude;
            if (currentTargetDistance < closestDistance)
            {
                closestEnemy = target;
                closestDistance = currentTargetDistance;
            }
        }

        currentTarget = closestEnemy;
    }

    #endregion Methods
}
