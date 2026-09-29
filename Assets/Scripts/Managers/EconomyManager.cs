using System;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance { get; private set; }

    [SerializeField] private int _gold;

    public event Action<int> OnGoldChanged;

    public int Gold => _gold;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Earn(int amount)
    {
        if (amount <= 0) return;
        _gold += amount;
        OnGoldChanged?.Invoke(_gold);
    }

    public bool Spend(int amount)
    {
        if (amount <= 0 || amount > _gold) return false;
        _gold -= amount;
        OnGoldChanged?.Invoke(_gold);
        return true;
    }

    // SAVING AND LOADING GOD HELP ME
    public EconomySaveData GetSaveData() => new EconomySaveData { gold = _gold };

    public void LoadFromSave(EconomySaveData data)
    {
        _gold = data.gold;
        OnGoldChanged?.Invoke(_gold);
    }
}
