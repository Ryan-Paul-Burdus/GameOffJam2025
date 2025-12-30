using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DamageFlash : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Color flashColor = Color.red;
    public float flashDuration = 0.05f;

    private Color originalColor;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    private void OnEnable()
    {
        spriteRenderer.color = originalColor;
    }

    private IEnumerator DoFlashCoroutine()
    {
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }

    public void DoFlash()
    {
        StopAllCoroutines();
        StartCoroutine(DoFlashCoroutine());
    }
}
