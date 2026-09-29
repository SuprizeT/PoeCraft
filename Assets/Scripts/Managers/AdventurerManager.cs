using System;
using System.Collections.Generic;
using UnityEngine;

public class AdventurerManager : MonoBehaviour
{
    public static AdventurerManager Instance { get; private set; }

    [SerializeField] private List<Adventurer> _roster = new();
    [SerializeField] private int _maxSlots = 1;

    public event Action OnRosterChanged;
    public IReadOnlyList<Adventurer> Roster => _roster;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        if (_roster.Count == 0) SeedStarterAdventurer();
    }

    private void Update() => CheckReturns();

    private void SeedStarterAdventurer()
    {
        _roster.Add(new Adventurer { id = Guid.NewGuid().ToString(), name = "John", state = AdventurerState.Idle });
    }

    public bool Dispatch(string adventurerId, string zoneId)
    {
        var adventurer = _roster.Find(a => a.id == adventurerId);
        if (adventurer == null || adventurer.state != AdventurerState.Idle) return false;

        var zone = ExpeditionSystem.Instance.GetZone(zoneId);
        if (zone == null) return false;

        adventurer.state = AdventurerState.OnExpedition;
        adventurer.currentZoneId = zoneId;
        adventurer.expeditionEndUtcTicks = DateTime.UtcNow.AddSeconds(zone.durationSeconds).Ticks;

        // NotificationManager.Instance?.ScheduleReturn(new DateTime(adventurer.expeditionEndUtcTicks, DateTimeKind.Utc));

        OnRosterChanged?.Invoke();
        return true;
    }

    public void CheckReturns()
    {
        long nowTicks = DateTime.UtcNow.Ticks;
        foreach (var adventurer in _roster)
        {
            if (adventurer.state != AdventurerState.OnExpedition) continue;
            if (nowTicks < adventurer.expeditionEndUtcTicks) continue;

            var result = ExpeditionSystem.Instance.ResolveExpedition(adventurer.currentZoneId);
            ApplyLoot(result);

            adventurer.state = AdventurerState.Idle;
            adventurer.currentZoneId = null;
            adventurer.expeditionEndUtcTicks = 0;
            OnRosterChanged?.Invoke();
        }
    }

    private void ApplyLoot(LootResult result)
    {
        foreach (var kvp in result.materialsGained)
            InventorySystem.Instance.AddMaterial(kvp.Key, kvp.Value);
        foreach (var weapon in result.weaponsGained)
            InventorySystem.Instance.AddWeapon(weapon);
    }

    public bool AddSlot()
    {
        _maxSlots++;
        _roster.Add(new Adventurer { id = Guid.NewGuid().ToString(), name = $"Adventurer {_roster.Count + 1}", state = AdventurerState.Idle });
        OnRosterChanged?.Invoke();
        return true;
    }

    // SAVING AND LOADING GOD HELP ME
    public AdventurerSaveData GetSaveData() => new() { adventurers = new List<Adventurer>(_roster), maxSlots = _maxSlots };

    public void LoadFromSave(AdventurerSaveData data)
    {
        _roster.Clear();
        _roster.AddRange(data.adventurers);
        _maxSlots = data.maxSlots;
        OnRosterChanged?.Invoke();
    }
}