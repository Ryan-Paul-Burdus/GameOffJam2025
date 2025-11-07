using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    private float moveSpeed = 1f;
    private GameObject Player;

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

        // Damage the player if close enough
        if (Vector3.Distance(transform.position, Player.transform.position) < 0.001f)
        {
            PlayerManager.Instance.DamagePlayer();
        }
    }
}
