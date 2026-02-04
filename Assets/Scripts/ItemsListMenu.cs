using UnityEditorInternal.VersionControl;
using UnityEngine;

public class ItemsListMenu : MonoBehaviour
{
    public ListItemDisplay ItemPrefab;
    public Transform PowerupHolder;

    private void Start()
    {
        LoadPowerupList();
    }

    public void LoadPowerupList()
    {
        foreach (Powerup item in PickupManager.Instance.AllPowerups)
        {
            ListItemDisplay listItem = Instantiate(ItemPrefab, PowerupHolder);
            listItem.UpdateDisplayToShowPowerup(item);
        }
    }
}
