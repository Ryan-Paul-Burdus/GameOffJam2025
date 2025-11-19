using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject Target;

    private Rigidbody2D rb;
    private float bulletLifetime = 2.0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //transform.rotation = Quaternion.LookRotation(Vector3.forward, (Target.transform.position - transform.position).normalized);
    }

    private void Update()
    {
        if (PickupManager.Instance.PickupUIVisibile)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

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
        if (collision.CompareTag("Enemy"))
        {
            EnemyManager.Instance.TakeDamage(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
