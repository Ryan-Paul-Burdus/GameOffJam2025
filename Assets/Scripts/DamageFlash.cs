using System.Collections;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    private Material material;

    public Color flashColor = Color.red;
    public float flashDuration = 0.05f;

    private void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
    }

    private IEnumerator DoFlashCoroutine()
    {
        material.SetColor("_FlashColor", flashColor);

        float currentFlashAmount = 0.0f;
        float elapsedTime = 0.0f;

        while (elapsedTime < flashDuration)
        {
            elapsedTime += Time.deltaTime;

            currentFlashAmount = Mathf.Lerp(1.0f, 0.0f, (elapsedTime / flashDuration));
            material.SetFloat("_FlashAmount", currentFlashAmount);

            yield return null;
        }

        
    }

    public void DoFlash()
    {
        StopAllCoroutines();
        StartCoroutine(DoFlashCoroutine());
    }

    public void ResetFlash()
    {
        material.SetFloat("_FlashAmount", 0.0f);
    }
}
