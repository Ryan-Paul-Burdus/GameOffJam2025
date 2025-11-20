using UnityEngine;

public class PowerupObject : MonoBehaviour
{
    public int SpawnLocationIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Pickup powerup
            StartCoroutine(PickupManager.Instance.PickupRandomPowerupCoroutine(SpawnLocationIndex));

            Destroy(gameObject);
        }
    }
}
