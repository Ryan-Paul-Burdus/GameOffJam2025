using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    private readonly List<GameObject> targets = new();
    private GameObject currentTarget;

    public GameObject BulletPrefab;
    public bool CanAttack = true;
    public bool AttackCoolingDown;
    public AbilityObject WaveAttackAbility;

    private void Update()
    {
        // Attack an enemy if possible
        if (!WaveAttackAbility.IsCoolingDown && targets.Count > 0)
        {
            FindNearestEnemy();
            if (currentTarget != null)
            {

                float angleStep = PlayerManager.Instance.SpreadAngle / PlayerManager.Instance.TotalProjectileCount;
                float centeringOffset = (PlayerManager.Instance.SpreadAngle / 2) - (angleStep / 2);
                float lookRotation = Quaternion.LookRotation(Vector3.forward, (currentTarget.transform.position - PlayerManager.Instance.Player.transform.position).normalized).eulerAngles.z;

                for (int i = 0; i < PlayerManager.Instance.TotalProjectileCount; i++)
                {
                    float currentBulletAngle = angleStep * i;
                    Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, lookRotation + currentBulletAngle - centeringOffset));

                    // Fire bullet to enemy
                    GameObject bulletObj = Instantiate(BulletPrefab, PlayerManager.Instance.Player.transform.position, rotation);
                    bulletObj.transform.localScale = new Vector2(PlayerManager.Instance.ProjectileScale, PlayerManager.Instance.ProjectileScale);

                    Bullet bullet = bulletObj.GetComponent<Bullet>();
                    bullet.Target = currentTarget;
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
