using UnityEngine;
using UnityEngine.UI;

public class ListItemDisplay : MonoBehaviour
{
    public Image ListItemImage;

    public void UpdateDisplayToShowPowerup(Powerup powerup)
    {
        ListItemImage.sprite = powerup.Image;
    }
}
