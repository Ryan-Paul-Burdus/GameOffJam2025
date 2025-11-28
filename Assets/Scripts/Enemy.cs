using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{ 
    public NavMeshAgent agent;
    
    public GameObject Player;

    public Slider HealthSlider;

    [Header("Animations")]
    public List<Sprite> EnemyAnimationSprites;
    private SpriteRenderer spriteRenderer;
    private int animationIndex = 0;
    private float timer;
    public float timeInterval = 0.25f;

    [Header("Enemy properties")]
    private float moveSpeed = 1f;
    public float MaxHealth = 50f;
    public float Damage = 10f;

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

        Health = MaxHealth;
        HealthSlider.maxValue = MaxHealth;
    }

    private void Update()
    {
        if (PickupManager.Instance.PickupUIVisibile || MenuManager.Instance.IsPaused)
        {
            return;
        }

        Vector3 direction = (Player.transform.position - transform.position).normalized;

        // Move and rotate towards the player
        agent.SetDestination(Player.transform.position);
        agent.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);

        UpdateEnemyAnimation();
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

                if (animationIndex >= EnemyAnimationSprites.Count)
                {
                    animationIndex = 0;
                }

                spriteRenderer.sprite = EnemyAnimationSprites[animationIndex];
            }
        }
    }
}
