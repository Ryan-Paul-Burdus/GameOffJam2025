using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public GameObject Player;

    public TextMeshProUGUI DamageText;
    public float Damage = 5f;

    public TextMeshProUGUI HealthText;
    public float Health = 100f;

    public float AttackCooldown = 1.5f;

    private void Awake()
    {
        if (Instance != null && Instance != this) // if we are the instance this is fine
        {
            Destroy(this);
            return;
        }
        Instance = this;

        HealthText.text = Health.ToString();
        DamageText.text = Damage.ToString();
    }

    public void IncreaseHealth(float increasePercentage)
    {
        Health += Health * (increasePercentage / 100);
        HealthText.text = Health.ToString();
    }

    public void IncreaseDamage(float increasePercentage)
    {
        Damage += Damage * (increasePercentage / 100);
        DamageText.text = Damage.ToString();
    }

    public void DamagePlayer(float damageAmount)
    {
        Health -= damageAmount;

        if (Health < 0)
        {
            Health = 0;
            HealthText.text = Health.ToString();
            KillPlayer();
        }
        else
        {
            HealthText.text = Health.ToString();
        }
    }

    private void KillPlayer()
    {
        //End the game
    }
}
