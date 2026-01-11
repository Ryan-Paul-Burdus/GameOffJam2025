using System.Collections;
using UnityEngine;

public class XP : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;
    public float xpValue;

    public void UpdateXPDisplay(Sprite xpSprite, float newXpValue)
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360)));

        SpriteRenderer.sprite = xpSprite;

        xpValue = newXpValue;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Stop spawning position coroutine if its still spawning
            StopAllCoroutines();

            // Collect the XP
            XPManager.Instance.AddXP(xpValue);

            // Release the xp object from the XP pool
            XPManager.Instance.XpPool.Release(this);
        }
    }

    public IEnumerator UpdateSpawnPositionCoroutine(float timeToXPLerpPosition, Vector3 finalSpawnLocation)
    {
        // Move from spawn location to final location
        float time = 0;
        while (time < timeToXPLerpPosition)
        {
            time += Time.deltaTime;
            float blend = Mathf.Clamp01(time / timeToXPLerpPosition);

            transform.position = Vector3.Lerp(transform.position, finalSpawnLocation, blend);
            yield return null;
        }
    }
}
