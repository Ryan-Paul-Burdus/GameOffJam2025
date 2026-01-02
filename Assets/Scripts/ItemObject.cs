using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public int SpawnLocationIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Pickup item

            Destroy(gameObject);
        }
    }
}
