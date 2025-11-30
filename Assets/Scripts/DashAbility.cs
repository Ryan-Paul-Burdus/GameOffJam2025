using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DashAbility : MonoBehaviour
{
    public Ability Ability;

    public Image Icon;
    public TextMeshProUGUI InputText;
    public Slider CooldownSlider;

    public float MaxCooldown;

    public bool IsDashCoolingDown;
    private float time = 0;

    private void Start()
    {
        CooldownSlider.maxValue = MaxCooldown;
        CooldownSlider.value = MaxCooldown;

        InputText.text = Ability.Input;
        Icon.sprite = Ability.Icon;
    }

    private void Update()
    {
        if (PickupManager.Instance.PickupUIVisibile || MenuManager.Instance.IsPaused)
        {
            return;
        }

        if (IsDashCoolingDown)
        {
            CooldownSlider.value = 0;

            time += Time.deltaTime;
            CooldownSlider.value = time;

            if (time >= MaxCooldown)
            {
                time = 0;
                IsDashCoolingDown = false;
            }
        }
    }
}
