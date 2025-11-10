using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Item[] AllItemTypes;

    public ItemManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this) // if we are the instance this is fine
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }
}
