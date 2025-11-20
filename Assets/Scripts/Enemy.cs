using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public GameObject Player;

    public Slider HealthSlider;

    [Header("Enemy properties")]
    public GameObject EnemyObject;
    private float moveSpeed = 1f;

    public float MaxHealth = 50f;
    private float health;
    public float Health
    {
        get => health;
        set
        {
            health = value; 
            HealthSlider.value = health;

            if (!HealthSlider.gameObject.activeSelf && health < MaxHealth)
            {
                HealthSlider.gameObject.SetActive(true);
            }
        }
    }

    private void Awake()
    {
        Player = PlayerManager.Instance.Player;

        Health = MaxHealth;
        HealthSlider.maxValue = MaxHealth;
    }

    private void Update()
    {
        if (PickupManager.Instance.PickupUIVisibile)
        {
            return;
        }

        Vector3 direction = (Player.transform.position - transform.position).normalized;

        // Move and rotate towards the player
        transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, moveSpeed * Time.deltaTime);
        EnemyObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
    }
}
