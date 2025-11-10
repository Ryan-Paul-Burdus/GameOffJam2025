using UnityEngine;

public class PowerupObject : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Pickup powerup
            PowerupManager.Instance.PickupRandomPowerup();

            Destroy(gameObject);
        }
    }
}
