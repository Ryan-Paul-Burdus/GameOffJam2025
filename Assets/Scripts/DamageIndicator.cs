using TMPro;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    private Vector3 initPos;
    private Vector3 targetPos;
    private float timer;

    public TextMeshProUGUI DamageText;
    public float TextLifetime = 1.2f;
    public float MinDistance = 0f;
    public float MaxDistance = 0.5f;

    private void Start()
    {
        float direction = Random.Range(-70, 71);
        initPos = transform.position;
        float distance = Random.Range(MinDistance, MaxDistance);
        targetPos = initPos + (Quaternion.Euler(0, 0, direction) * new Vector3(distance, distance, 0));
        transform.localScale = Vector3.zero;

    }

    private void Update()
    {
        timer += Time.deltaTime;

        float fraction = TextLifetime / 2f;

        if (timer > TextLifetime)
        {
            Destroy(gameObject);
        }
        else if (timer > fraction)
        {
            DamageText.color = Color.Lerp(DamageText.color, Color.clear, (timer - fraction) / (TextLifetime - fraction));
        }

        transform.position = Vector3.Lerp(initPos, targetPos, Mathf.Sin(timer / TextLifetime));
        transform.localScale = Vector3.Lerp(new Vector3(0.2f, 0.2f, 0.2f), Vector3.one, Mathf.Sin(timer / TextLifetime));
    }

    public void SetDamageText(float damage)
    {
        DamageText.text = damage.ToString();
    }
}
