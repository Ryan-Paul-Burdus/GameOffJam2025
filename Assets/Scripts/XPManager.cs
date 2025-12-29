using UnityEngine;
using UnityEngine.UI;

public class XPManager : MonoBehaviour
{
    public static XPManager Instance { get; private set; }

    public GameObject xpObject;
    private Slider xpSlider;

    public float maxXPValue;
    public float PlayerXP {  get; private set; }

    private void OnEnable()
    {
        xpSlider = xpObject.GetComponent<Slider>();
        xpSlider.maxValue = maxXPValue;
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

        float xpOverflow = PlayerXP - maxXPValue;

        if (xpOverflow >= 0.0)
        {
            PlayerXP = xpOverflow;
            PickupManager.Instance.Show3RandomPowerups();
        }

        xpSlider.value = PlayerXP;
    }

}
