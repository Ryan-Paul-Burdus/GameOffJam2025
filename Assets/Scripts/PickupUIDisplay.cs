using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickupUIDisplay : MonoBehaviour
{
    public TextMeshProUGUI PickupName;
    public Image PickupImage;
    public TextMeshProUGUI PickupDescription;

    public Button ActionButton;
    public TextMeshProUGUI ActionButtonText;
    public Button CancelButton;
    public TextMeshProUGUI CancelButtonText;

    private PickupType CurrentPickupType;

    public void ShowPowerupDisplay(Powerup powerup)
    {
        CurrentPickupType = PickupType.Powerup;

        PickupName.text = powerup.Name;
        PickupImage.sprite = powerup.Image;
        PickupDescription.text = powerup.Description.Replace("$", powerup.Amount.ToString());
        ActionButtonText.text = "Accept";
        CancelButtonText.text = "Refuse";

        gameObject.SetActive(true);
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

        gameObject.SetActive(false);
        PickupManager.Instance.PickupUIVisibile = false;
    }

    public void PressCancelButton()
    {
        gameObject.SetActive(false);
        PickupManager.Instance.PickupUIVisibile = false;
        Debug.Log("Declined powerup");
    }

    #endregion Buttons
}
