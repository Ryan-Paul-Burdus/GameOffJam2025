using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityObject : MonoBehaviour
{
    public Ability Ability;

    public Image Icon;
    public TextMeshProUGUI InputText;
    public Slider CooldownSlider;

    public float MaxCooldown;

    public bool IsCoolingDown;
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
        if (IsCoolingDown)
        {
            CooldownSlider.value = 0;

            time += Time.deltaTime;
            CooldownSlider.value = time;

            if (time >= MaxCooldown)
            {
                time = 0;
                IsCoolingDown = false;
            }
        }
    }
}
