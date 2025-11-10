using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject Target;
    public float BulletSpeed = 10f;

    private void Update()
    {
        if (Target != null)
        {
            // Move and rotate towards the target
            transform.SetPositionAndRotation(
                Vector3.MoveTowards(transform.position, Target.transform.position, BulletSpeed * Time.deltaTime),
                Quaternion.LookRotation(Vector3.forward, (Target.transform.position - transform.position).normalized)
                );

            // Destroy target if close enough
            if (Vector3.Distance(transform.position, Target.transform.position) < 0.001f)
            {
                EnemyManager.Instance.TakeDamage(Target);
                Destroy(gameObject);
            }
        }
    }
}
