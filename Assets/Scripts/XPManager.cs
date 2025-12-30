using UnityEngine;
using UnityEngine.UI;

public class XPManager : MonoBehaviour
{
    public static XPManager Instance { get; private set; }

    public GameObject xpObject;
    private Slider xpSlider;

    public float MaxXPValue = 100.0f;
    public float PlayerXP {  get; private set; }

    private void OnEnable()
    {
        xpSlider = xpObject.GetComponent<Slider>();
        xpSlider.maxValue = MaxXPValue;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this) // if we are the instance this is fine
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void AddXP(float xpAmount)
    {
        PlayerXP += xpAmount;

        float xpOverflow = PlayerXP - MaxXPValue;

        if (xpOverflow >= 0.0)
        {
            PlayerXP = xpOverflow;
            PickupManager.Instance.Show3RandomPowerups();
            UpdateMaxXP(MaxXPValue *= 1.1f);
        }

        xpSlider.value = PlayerXP;
    }

    private void UpdateMaxXP(float newMaxXP)
    {
        MaxXPValue = newMaxXP;
        xpSlider.maxValue = newMaxXP;
    }

}
