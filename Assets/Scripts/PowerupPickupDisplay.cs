using UnityEngine;
using UnityEngine.UI;

public class PowerupPickupDisplay : MonoBehaviour
{
    public Powerup Powerup;

    public Text NameText;
    public Text Description;
    public Image Image;


    private void Start()
    {
        NameText.text = Powerup.Name;
        Description.text = Powerup.Description.Replace("$", Powerup.Amount.ToString());
        Image.sprite = Powerup.Image;
    }
}
