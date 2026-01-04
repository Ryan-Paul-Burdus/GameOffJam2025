using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class XPManager : MonoBehaviour
{
    public static XPManager Instance { get; private set; }

    public XP XpScript;
    public ObjectPool<XP> XpPool;

    private float minTimeToXPLerpPosition = 0.2f;
    private float maxTimeToXPLerpPosition = 0.5f;
    private float minDistanceForXPLerpPosition = 0.5f;
    private float maxDistanceForXPLerpPosition = 1.5f;

    [Header("XP values")]
    public Slider xpSlider;
    public float MaxXPValue = 100.0f;
    public float PlayerXP {  get; private set; }

    [Header("XP sprites")]
    public Sprite smallXP;
    public Sprite mediumXP;
    public Sprite largeXP;

    private void OnEnable()
    {
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

        XpPool = new ObjectPool<XP>(CreatePooledXpObject, GetXpFromPool, ReturnXpToPool, null, false, 50, 500);
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

    public void SpawnXP(Enemy enemy, float newXpValue)
    {
        float timeToXPLerpPosition = Random.Range(minTimeToXPLerpPosition, maxTimeToXPLerpPosition);
        float distanceToXPLerpPosition = Random.Range(minDistanceForXPLerpPosition, maxDistanceForXPLerpPosition);

        // Start spawn location
        Vector3 startSpawnLocation = enemy.transform.position;

        // Final location
        Vector2 finalSpawnLocation = (Vector2)enemy.transform.position + distanceToXPLerpPosition * Random.insideUnitCircle;

        // Get xp to spawn
        XP xp = XpPool.Get();
        xp.transform.position = startSpawnLocation;
        xp.UpdateXPDisplay(mediumXP, newXpValue);
        StartCoroutine(xp.UpdateSpawnPositionCoroutine(timeToXPLerpPosition, finalSpawnLocation));
    }


    #region XP pooling

    private XP CreatePooledXpObject()
    {
        XP xp = Instantiate(XpScript);
        xp.gameObject.SetActive(false);
        return xp;
    }

    private void GetXpFromPool(XP xp)
    {
        xp.gameObject.SetActive(true);
    }

    private void ReturnXpToPool(XP xp)
    {
        xp.gameObject.SetActive(false);
    }

    #endregion XP pooling
}
