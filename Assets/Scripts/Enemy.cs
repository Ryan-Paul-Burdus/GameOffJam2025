using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{ 
    public NavMeshAgent agent;
    public GameObject Player;
    public Slider HealthSlider;

    [Header("Animations")]
    public Sprite[] EnemyAnimationSprites;
    private SpriteRenderer spriteRenderer;
    private int animationIndex = 0;
    private float timer;
    private float timeInterval = 0.25f;

    [Header("Enemy properties")]
    public float MaxHealth = 20f;
    public float Damage = 20f;
    private float moveSpeed = 1.2f;

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

            else if (health >= MaxHealth)
            {
                HealthSlider.gameObject.SetActive(false);
            }
        }
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        agent.updateUpAxis = false;
        agent.updateRotation = false;
        agent.speed = moveSpeed;
    }

    private void Awake()
    {
        Player = PlayerManager.Instance.Player;
    }

    private void OnEnable()
    {
        // Fit stats to the latest multiplier when spawned
        float statsMultiplier = EnemyManager.Instance.enemyStatsMultiplier;
        MaxHealth *= statsMultiplier;
        Damage *= statsMultiplier;
        moveSpeed *= statsMultiplier;

        Health = MaxHealth;
        HealthSlider.maxValue = MaxHealth;
    }

    private void Update()
    {
        // Move and rotate towards the player
        agent.SetDestination(Player.transform.position);

        UpdateEnemyAnimation();

        // Check if the agent is moving to avoid errors when stationary
        if (agent.velocity.sqrMagnitude > Mathf.Epsilon)
        {
            // Calculate the rotation that looks in the direction of the velocity
            transform.rotation = Quaternion.LookRotation(Vector3.forward, agent.velocity.normalized);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerManager.Instance.DamagePlayer(Damage);
        }
    }

    private void UpdateEnemyAnimation()
    {
        // Loop through the player animations
        if (EnemyAnimationSprites != null)
        {
            timer += Time.deltaTime;
            if (timer >= timeInterval)
            {
                timer = 0f;
                animationIndex++;

                if (animationIndex >= EnemyAnimationSprites.Length)
                {
                    animationIndex = 0;
                }

                spriteRenderer.sprite = EnemyAnimationSprites[animationIndex];
            }
        }
    }
}
