using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDatabase", menuName = "AdcraftSim/Game Database")]
public class GameDatabaseSO : ScriptableObject
{
    [Header("Put Assets in here")]
    public List<WeaponDefinitionSO> weapons;
    public List<MaterialDefinitionSO> materials;
    public List<ZoneDefinitionSO> zones;

    private Dictionary<string, WeaponDefinitionSO> _weaponLookup;
    private Dictionary<string, MaterialDefinitionSO> _materialLookup;
    private Dictionary<string, ZoneDefinitionSO> _zoneLookup;

    //calls once and acts sort of like manager
    public void Initialize()
    {
        _weaponLookup = new Dictionary<string, WeaponDefinitionSO>();
        foreach (var w in weapons)
        {
            if (!_weaponLookup.TryAdd(w.weaponId, w))
                Debug.LogError($"Duplicate weaponId '{w.weaponId}' on {w.name}");
        }

        _materialLookup = new Dictionary<string, MaterialDefinitionSO>();
        foreach (var m in materials)
        {
            if (!_materialLookup.TryAdd(m.materialId, m))
                Debug.LogError($"Duplicate materialId '{m.materialId}' on {m.name}");
        }

        _zoneLookup = new Dictionary<string, ZoneDefinitionSO>();
        foreach (var z in zones)
        {
            if (!_zoneLookup.TryAdd(z.zoneId, z))
                Debug.LogError($"Duplicate zoneId '{z.zoneId}' on {z.name}");
        }
    }

    public WeaponDefinitionSO GetWeapon(string id) { EnsureInitialized(); return Lookup(_weaponLookup, id); }
    public MaterialDefinitionSO GetMaterial(string id) { EnsureInitialized(); return Lookup(_materialLookup, id); }
    public ZoneDefinitionSO GetZone(string id) { EnsureInitialized(); return Lookup(_zoneLookup, id); }

    private void EnsureInitialized()
    {
        if (_zoneLookup == null) Initialize();
    }


    private T Lookup<T>(Dictionary<string, T> dict, string id) where T : Object
    {
        if (dict.TryGetValue(id, out var result)) return result;
        Debug.LogError($"No {typeof(T).Name} found for id '{id}' - check for a typo.");
        return null;
    }
}