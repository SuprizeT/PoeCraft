using System;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance { get; private set; }

    [SerializeField] private int _capacity = 30;
    private readonly List<WeaponInstance> _weapons = new();
    private readonly Dictionary<string, int> _materials = new();

    public event Action OnInventoryChanged;

    public IReadOnlyList<WeaponInstance> Weapons => _weapons;
    public IReadOnlyDictionary<string, int> Materials => _materials;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool IsFull() => _weapons.Count + _materials.Count >= _capacity;

    public bool AddWeapon(WeaponInstance weapon)
    {
        if (IsFull()) return false;
        _weapons.Add(weapon);
        OnInventoryChanged?.Invoke();
        return true;
    }

    public void RemoveWeapon(WeaponInstance weapon)
    {
        _weapons.Remove(weapon);
        OnInventoryChanged?.Invoke();
    }

    public bool AddMaterial(string materialId, int qty)
    {
        if (!_materials.ContainsKey(materialId) && IsFull()) return false;
        _materials.TryGetValue(materialId, out int current);
        _materials[materialId] = current + qty;
        OnInventoryChanged?.Invoke();
        return true;
    }

    public bool RemoveMaterial(string materialId, int qty)
    {
        if (!_materials.TryGetValue(materialId, out int current) || current < qty) return false;
        _materials[materialId] = current - qty;
        if (_materials[materialId] <= 0) _materials.Remove(materialId);
        OnInventoryChanged?.Invoke();
        return true;
    }

    // SAVING AND LOADING GOD HELP ME
    public InventorySaveData GetSaveData()
    {
        var data = new InventorySaveData { weapons = new List<WeaponInstance>(_weapons), materialIds = new List<string>(), materialQtys = new List<int>() };
        foreach (var kvp in _materials)
        {
            data.materialIds.Add(kvp.Key);
            data.materialQtys.Add(kvp.Value);
        }
        return data;
    }

    public void LoadFromSave(InventorySaveData data)
    {
        _weapons.Clear();
        _weapons.AddRange(data.weapons);
        _materials.Clear();
        for (int i = 0; i < data.materialIds.Count; i++)
            _materials[data.materialIds[i]] = data.materialQtys[i];
        OnInventoryChanged?.Invoke();
    }
}
