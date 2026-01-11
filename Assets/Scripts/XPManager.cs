using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class XPManager : MonoBehaviour
{
    public static XPManager Instance { get; private set; }

    public XP XpScript;
    public ObjectPool<XP> XpPool;

    private float minTimeToXPLerpPosition = 0.2f;
    private float maxTimeToXPLerpPosition = 0.5f;
    private float minDistanceForXPLerpPosition = 0.5f;
    private float maxDistanceForXPLerpPosition = 1.5f;

    public Slider xpSlider;
    public float MaxXPValue = 100.0f;
    public float CurrentPlayerXP {  get; private set; }

    [Header("XP values")]
    public float SmallXPValue = 10.0f;
    public float MediumXPValue = 100.0f;
    public float LargeXPValue = 1000.0f;

    [Header("XP sprites")]
    public Sprite SmallXPSprite;
    public Sprite MediumXPSprite;
    public Sprite LargeXPSprite;

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
        CurrentPlayerXP += xpAmount;

        // Keep checking for overflow til there is none
        while (CurrentPlayerXP - MaxXPValue >= 0.0)
        {
            // Set the new current xp to the overflow
            CurrentPlayerXP -= MaxXPValue;

            // Show the 3 random powerups
            // TODO: Make this add a powerup to a queue for the gui to look through each time
            PickupManager.Instance.AddNewRandomPowerupGUIToQueue();

            // Updates the max xp to the new max
            UpdateMaxXP(MaxXPValue *= 1.1f);
        }

        // TODO: Animate the slider going from the old value to the new one
        xpSlider.value = CurrentPlayerXP;
    }

    private void UpdateMaxXP(float newMaxXP)
    {
        MaxXPValue = newMaxXP;
        xpSlider.maxValue = newMaxXP;
    }

    public void SpawnXP(Enemy enemy, float xpValue)
    {
        // Start spawn location
        Vector3 startSpawnLocation = enemy.transform.position;

        float timeToXPLerpPosition = Random.Range(minTimeToXPLerpPosition, maxTimeToXPLerpPosition);
        float distanceToXPLerpPosition = Random.Range(minDistanceForXPLerpPosition, maxDistanceForXPLerpPosition);

        // Final location
        Vector2 finalSpawnLocation = (Vector2)startSpawnLocation + distanceToXPLerpPosition * Random.insideUnitCircle;

        Sprite xpSprite;
        if (xpValue >= LargeXPValue)
        {
            xpSprite = LargeXPSprite;
        }
        else if (xpValue >= MediumXPValue)
        {
            xpSprite = MediumXPSprite;
        }
        else
        {
            xpSprite = SmallXPSprite;
        }

        // Get xp to spawn
        XP xp = XpPool.Get();
        xp.transform.position = startSpawnLocation;
        xp.UpdateXPDisplay(xpSprite, xpValue);
        StartCoroutine(xp.UpdateSpawnPositionCoroutine(timeToXPLerpPosition, finalSpawnLocation));

        /// TODO: un comment this and refactor slightly if we decide on having multiple xp blobs again instead of 1 per enemy
        //List<(Sprite xpSprite, float xpValue)> xpToSpawn = GetXpSpawnCollection(xpValue);
        //foreach ((Sprite xpSprite, float xpValue) item in xpToSpawn)
        //{
        //    float timeToXPLerpPosition = Random.Range(minTimeToXPLerpPosition, maxTimeToXPLerpPosition);
        //    float distanceToXPLerpPosition = Random.Range(minDistanceForXPLerpPosition, maxDistanceForXPLerpPosition);

        //    // Final location
        //    Vector2 finalSpawnLocation = (Vector2)startSpawnLocation + distanceToXPLerpPosition * Random.insideUnitCircle;

        //    // Get xp to spawn
        //    XP xp = XpPool.Get();
        //    xp.transform.position = startSpawnLocation;
        //    xp.UpdateXPDisplay(item.xpSprite, item.xpValue);
        //    StartCoroutine(xp.UpdateSpawnPositionCoroutine(timeToXPLerpPosition, finalSpawnLocation));
        //}
    }

    private List<(Sprite xpSprite, float xpValue)> GetXpSpawnCollection(float totalXpForXpSpawn)
    {
        var xpList = new List<(Sprite xpSprite, float xpValue)>();
        if (totalXpForXpSpawn <= 0f)
            return xpList;

        const int maxSpawn = 5;
        float xpLeftToAdd = totalXpForXpSpawn;

        while (xpLeftToAdd > 0f)
        {
            if (xpLeftToAdd >= LargeXPValue)
            {
                xpList.Add((LargeXPSprite, LargeXPValue));
                xpLeftToAdd -= LargeXPValue;
                continue;
            }

            if (xpLeftToAdd >= MediumXPValue)
            {
                xpList.Add((MediumXPSprite, MediumXPValue));
                xpLeftToAdd -= MediumXPValue;
                continue;
            }

            if (xpLeftToAdd >= SmallXPValue)
            {
                xpList.Add((SmallXPSprite, SmallXPValue));
                xpLeftToAdd -= SmallXPValue;
                continue;
            }

            // Add the small left over value/s
            xpList.Add((SmallXPSprite, xpLeftToAdd));
            xpLeftToAdd -= xpLeftToAdd;
        }

        return xpList;
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
