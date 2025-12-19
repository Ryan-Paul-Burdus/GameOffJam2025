using UnityEngine;
using UnityEngine.Pool;

public class DamageIndicatorManager : MonoBehaviour
{
    public static DamageIndicatorManager Instance { get; private set; }

    public DamageIndicator DamageIndicatorScript;
    public ObjectPool<DamageIndicator> DamageIndicatorPool;

    private void Awake()
    {
        if (Instance != null && Instance != this) // if we are the instance this is fine
        {
            Destroy(this);
            return;
        }
        Instance = this;

        DamageIndicatorPool = new ObjectPool<DamageIndicator>(CreatePooledDamageIndicatorObject, 
            GetDamageIndicatorFromPool, ReturnDamageIndicatorToPool, null, false, 10, 150);
    }

    private DamageIndicator CreatePooledDamageIndicatorObject()
    {
        DamageIndicator damageIndicator = Instantiate(DamageIndicatorScript);
        damageIndicator.gameObject.SetActive(false);
        return damageIndicator;
    }

    private void GetDamageIndicatorFromPool(DamageIndicator indicator)
    {
        indicator.gameObject.SetActive(true);
    }

    private void ReturnDamageIndicatorToPool(DamageIndicator indicator)
    {
        indicator.gameObject.SetActive(false);
    }

    public void SpawnDamageIndicator(GameObject target, float damageAmount)
    {
        DamageIndicator damageIndicator = DamageIndicatorPool.Get();
        Debug.Log("Indicator got");
        damageIndicator.InitialisePosition(target);
        damageIndicator.DamageText.text = damageAmount.ToString();
    }
}
