using System.Collections;
using UnityEngine;

public class Fade : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [Range(0f, 10f)]
    public float timeToFade = 1f;

    [Range(0f, 1)]
    public float MinimumOpacity = 0f;

    [Range(0f, 1)]
    public float MaximumOpacity = 1f;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    private void Update()
    {
        if (PickupManager.Instance.PickupUIVisibile || MenuManager.Instance.IsPaused)
        {
            return;
        }

        if (spriteRenderer.color.a == MaximumOpacity)
        {
            StartCoroutine(FadeTo(MinimumOpacity));
        }
        else if (spriteRenderer.color.a == MinimumOpacity)
        {
            StartCoroutine(FadeTo(MaximumOpacity));
        }
    }

    private IEnumerator FadeTo(float targetOpacity)
    {
        Color spriteColor = spriteRenderer.color;
        float time = 0;

        while (time < timeToFade)
        {
            time +=  Time.deltaTime;
            float blend = Mathf.Clamp01(time / timeToFade);

            spriteRenderer.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, Mathf.Lerp(spriteColor.a, targetOpacity, blend));
            yield return null;
        }
    }
}
