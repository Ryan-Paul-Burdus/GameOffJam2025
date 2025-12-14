using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;

    private float maxBulletLifetime = 2.0f;
    private float bulletLifetime;

    private PlayerAttacks playerAttacksScript;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAttacksScript = PlayerManager.Instance.PlayerAttacks;
    }

    private void OnEnable()
    {
        bulletLifetime = maxBulletLifetime;
    }

    private void Update()
    {
        if (bulletLifetime <= 0f)
        {
            playerAttacksScript.BulletPool.Release(this);
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
            playerAttacksScript.BulletPool.Release(this);
        }

        else if (collision.CompareTag("Map Obstacle"))
        {
            playerAttacksScript.BulletPool.Release(this);
        }
    }
}
