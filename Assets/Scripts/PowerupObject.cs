using UnityEngine;

public class PowerupObject : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Pickup powerup
            PickupManager.Instance.PickupRandomPowerup();

            Destroy(gameObject);
        }
    }
}
