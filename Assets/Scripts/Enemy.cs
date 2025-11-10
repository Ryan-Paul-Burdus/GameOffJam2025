using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    private float moveSpeed = 1f;
    public GameObject Player;

    private void Awake()
    {
        Player = PlayerManager.Instance.Player;
    }

    private void Update()
    {
        // Move and rotate towards the player
        transform.SetPositionAndRotation(
            Vector3.MoveTowards(transform.position, Player.transform.position, moveSpeed * Time.deltaTime),
            Quaternion.LookRotation(Vector3.forward, (Player.transform.position - transform.position).normalized)
            );

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerManager.Instance.DamagePlayer(10);
        }
    }
}
