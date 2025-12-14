using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject Target;

    private Rigidbody2D rb;
    private float bulletLifetime = 2.0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (bulletLifetime <= 0f)
        {
            Destroy(gameObject);
        }
        else
        {
            bulletLifetime -= Time.deltaTime;
        }

        rb.linearVelocity = transform.up * PlayerManager.Instance.BulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !EnemyManager.Instance.EnemyTakingDamage)
        {
            EnemyManager.Instance.EnemyTakingDamage = true;
            EnemyManager.Instance.TakeDamage(collision.gameObject);
            Destroy(gameObject);
        }

        else if (collision.CompareTag("Map Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
