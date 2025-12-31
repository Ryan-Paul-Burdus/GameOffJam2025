using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerupDisplay : MonoBehaviour
{
    public TextMeshProUGUI PowerupName;
    public Image PowerupImage;
    public TextMeshProUGUI PowerupDescription;

    private PickupType CurrentPickupType;

    private Powerup powerup;

    public void UpdatePowerupDisplay(Powerup newPowerup)
    {
        CurrentPickupType = PickupType.Powerup;
        powerup = newPowerup;

        PowerupName.text = newPowerup.Name;
        PowerupImage.sprite = newPowerup.Image;
        PowerupDescription.text = newPowerup.Description.Replace("$", newPowerup.Amount.ToString());
    }

    #region Buttons

    public void PressActionButton()
    {
        //Do action
        switch (CurrentPickupType)
        {
            case PickupType.Powerup:
                PickupManager.Instance.TakeCurrentPowerupEffect(powerup);
                break;
        }

        PickupManager.Instance.PickupUI.SetActive(false);
        Time.timeScale = 1.0f;
    }

    #endregion Buttons
}
