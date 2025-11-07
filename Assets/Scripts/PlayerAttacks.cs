using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    private readonly List<GameObject> Targets = new();
    private GameObject CurrentTarget;

    private bool canAttack = true;
    public float AttackCooldown = 1.5f;
    public GameObject BulletPrefab;


    private void Update()
    {
        // Attack an enemy if possible
        if (canAttack && Targets.Count > 0)
        {
            FindNearestEnemy();
            if (CurrentTarget != null)
            {
                StartCoroutine(AttackEnemyCoroutine());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Targets.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Targets.Remove(collision.gameObject);
        }
    }

    #region Methods

    //TODO: Make this return the nearest enemy and let it be accessed more openly so
    //      anything can be done with that closest enemy
    private void FindNearestEnemy()
    {
        if (Targets.Count <= 0)
        {
            return;
        }

        GameObject closestEnemy = Targets[0];
        float closestDistance = 9999;
        foreach (GameObject target in Targets)
        {
            float currentTargetDistance = (target.transform.position - gameObject.transform.position).magnitude;
            if (currentTargetDistance < closestDistance)
            {
                closestEnemy = target;
                closestDistance = currentTargetDistance;
            }
        }

        CurrentTarget = closestEnemy;
    }

    private IEnumerator AttackEnemyCoroutine()
    {
        canAttack = false;

        // Fire bullet to enemy
        Bullet bullet = Instantiate(BulletPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();
        bullet.Target = CurrentTarget;

        // Startthe cooldown for next shot
        yield return new WaitForSeconds(AttackCooldown);
        canAttack = true;
    }

    #endregion Methods
}
