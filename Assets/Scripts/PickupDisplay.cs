using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerupDisplay : MonoBehaviour
{
    public TextMeshProUGUI PowerupName;
    public Image PowerupImage;
    public TextMeshProUGUI PowerupDescription;

    private PickupType CurrentPickupType;

    public void UpdatePowerupDisplay(Powerup powerup)
    {
        CurrentPickupType = PickupType.Powerup;

        PowerupName.text = powerup.Name;
        PowerupImage.sprite = powerup.Image;
        PowerupDescription.text = powerup.Description.Replace("$", powerup.Amount.ToString());
    }

    #region Buttons

    public void PressActionButton()
    {
        //Do action
        switch (CurrentPickupType)
        {
            case PickupType.Powerup:
                PickupManager.Instance.TakeCurrentPowerupEffect();
                break;
        }

        PickupManager.Instance.PickupUI.SetActive(false);
        Time.timeScale = 1.0f;
    }

    #endregion Buttons
}
