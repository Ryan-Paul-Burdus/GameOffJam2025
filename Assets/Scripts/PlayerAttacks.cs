using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    private readonly List<GameObject> Targets = new();
    private GameObject CurrentTarget;

    private void Update()
    {
        if (PickupManager.Instance.PickupUIVisibile || MenuManager.Instance.IsPaused)
        {
            return;
        }

        // Attack an enemy if possible
        if (PlayerManager.Instance.canAttack && Targets.Count > 0)
        {
            FindNearestEnemy();
            if (CurrentTarget != null)
            {
                StartCoroutine(PlayerManager.Instance.AttackEnemyCoroutine(CurrentTarget));
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

    #endregion Methods
}
