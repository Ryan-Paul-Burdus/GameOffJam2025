using System;
using System.Collections.Generic;
using System.Linq;
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
            AttackNearestEnemy();
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

    private void AttackNearestEnemy()
    {
        float spreadAngle = PlayerManager.Instance.SpreadAngle;
        float attackareaSize = PlayerManager.Instance.AttackAreaOfSize;
        float projectileCount = PlayerManager.Instance.TotalProjectileCount;
        Vector3 playerPosition = PlayerManager.Instance.Player.transform.position;

        if (targets.Count <= 0)
        {
            return;
        }

        GameObject closestEnemy = null;
        float closestDistance = float.PositiveInfinity;
        Vector2 playerPos2D = playerPosition;

        foreach (GameObject target in targets)
        {
            Vector2 toTarget = (Vector2)target.transform.position - playerPos2D;
            float targetDistance = toTarget.magnitude;
            if (targetDistance <= 0f)
            {
                continue;
            }

            Vector2 direction = toTarget / targetDistance;

            // RaycastAll along the exact distance to the target (+ small epsilon).
            float rayLength = targetDistance + 0.05f;
            RaycastHit2D[] hits = Physics2D.RaycastAll(playerPos2D, direction, rayLength);

            // Draw debug ray scaled by length so it's visible and accurate.
            Debug.DrawRay(playerPos2D, direction * rayLength, Color.red, 0.1f);

            // Find the closest hit (RaycastAll is not guaranteed to be ordered in all cases).
            Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
            RaycastHit2D firstHit = hits.First(x => !x.collider.CompareTag("Player") && !x.collider.CompareTag("PlayerAttachment"));

            // If the hit isn't an enemy then move onto the next target
            if (!firstHit.collider.CompareTag("Enemy"))
            {
                continue;
            }

            // Use the actual target distance to find the nearest enemy.
            if (targetDistance < closestDistance)
            {
                closestEnemy = target;
                closestDistance = targetDistance;
            }
        }

        currentTarget = closestEnemy;

        if (currentTarget != null)
        {
            angleStep = spreadAngle / projectileCount;
            centeringOffset = (spreadAngle / 2) - (angleStep / 2);
            lookRotation = Quaternion.LookRotation(Vector3.forward, (currentTarget.transform.position - playerPosition).normalized).eulerAngles.z;

            for (int i = 0; i < projectileCount; i++)
            {
                currentBulletIndex = i;
                BulletPool.Get();
            }

            WaveAttackAbility.IsCoolingDown = true;
        }
    }

    #region Bullet pooling

    private Bullet CreatePooledBulletObject()
    {
        Bullet bullet = Instantiate(BulletScript, PlayerManager.Instance.Player.transform.position, Quaternion.identity);
        bullet.gameObject.SetActive(false);
        return bullet;
    }

    private void GetBulletFromPool(Bullet bullet)
    {
        bullet.hitEnemy = false;

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

    #endregion Methods
}
