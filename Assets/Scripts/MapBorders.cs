using System.Linq;
using UnityEngine;

public class MapBorders : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemySpawnBorder"))
        {
            EnemySpawnLocation bla = EnemyManager.Instance.EnemySpawnLocations.Single(x => x.Location.gameObject.name == gameObject.name);
            bla.IsWithinPlayArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemySpawnBorder"))
        {
            EnemySpawnLocation bla = EnemyManager.Instance.EnemySpawnLocations.Single(x => x.Location.gameObject.name == gameObject.name);
            bla.IsWithinPlayArea = false;
        }
    }
}
