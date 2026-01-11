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
    private float powerupAmount;

    public Image PowerupPanelBackground;

    public void UpdatePowerupDisplay(Powerup newPowerup, Rarity newRarity)
    {
        CurrentPickupType = PickupType.Powerup;
        powerup = newPowerup;

        PowerupName.text = newPowerup.Name;
        PowerupImage.sprite = newPowerup.Image;

        switch (newRarity)
        {
            case Rarity.Common:
                powerupAmount = newPowerup.CommonAmount;
                PowerupPanelBackground.color = PickupManager.Instance.CommonRarityColor;
                break;

            case Rarity.Uncommon:
                powerupAmount = newPowerup.UncommonAmount;
                PowerupPanelBackground.color = PickupManager.Instance.UncommonRarityColor;
                break;

            case Rarity.Epic:
                powerupAmount = newPowerup.EpicAmount;
                PowerupPanelBackground.color = PickupManager.Instance.EpicRarityColor;
                break;

            case Rarity.Legendary:
                powerupAmount = newPowerup.LegendaryAmount;
                PowerupPanelBackground.color = PickupManager.Instance.LegendaryRarityColor;
                break;
        };

        PowerupDescription.text = newPowerup.Description.Replace("$", powerupAmount.ToString());
    }

    #region Buttons

    public void PressActionButton()
    {
        //Do action
        switch (CurrentPickupType)
        {
            case PickupType.Powerup:
                PickupManager.Instance.TakeCurrentPowerupEffect(powerup, powerupAmount);

                if (PickupManager.Instance.randomPowerupGUIsInQueue <= 0)
                {
                    PickupManager.Instance.PickupUI.SetActive(false);
                    Time.timeScale = 1.0f;
                }
                break;
        }
    }

    #endregion Buttons
}
