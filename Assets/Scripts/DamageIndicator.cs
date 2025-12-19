using TMPro;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    public Vector3 initialPosition;
    private Vector3 targetPos;
    private Vector3 initialScale = new(1, 1, 1);
    private float timer;

    public TextMeshProUGUI DamageText;
    public Color originalTextColor;

    private float textLifetime = 1.2f;

    public float MinDistance = 0f;
    public float MaxDistance = 0.5f;
    private float travelDistance;
    private float travelDirection;

    private void Start()
    {
        travelDirection = Random.Range(-70, 71);
        travelDistance = Random.Range(MinDistance, MaxDistance);
    }

    private void OnEnable()
    {
        DamageText.color = originalTextColor;
        transform.localScale = initialScale;
    }

    public void InitialisePosition(GameObject target)
    {
        transform.position = target.transform.position;
        initialPosition = target.transform.position;
        targetPos = initialPosition + (Quaternion.Euler(0, 0, travelDirection) * new Vector3(travelDistance, travelDistance, 0));
    }

    private void Update()
    {
        timer += Time.deltaTime;

        float fraction = textLifetime / 2f;

        if (timer > textLifetime)
        {
            timer = 0.0f;
            DamageIndicatorManager.Instance.DamageIndicatorPool.Release(this);
            Debug.Log("Indicator released from indicator update");
        }
        else if (timer > fraction)
        {
            DamageText.color = Color.Lerp(DamageText.color, Color.clear, (timer - fraction) / (textLifetime - fraction));
        }

        transform.position = Vector3.Lerp(initialPosition, targetPos, Mathf.Sin(timer / textLifetime));
        transform.localScale = Vector3.Lerp(initialScale, Vector3.one, Mathf.Sin(timer / textLifetime));
    }
}
