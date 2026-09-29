using UnityEngine;

public class ExpeditionSystem : MonoBehaviour
{
    public static ExpeditionSystem Instance { get; private set; }

    [SerializeField] private GameDatabaseSO _database;
    private const int LootRollsPerExpedition = 3;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public ZoneDefinitionSO GetZone(string zoneId) => _database.GetZone(zoneId);

    public LootResult ResolveExpedition(string zoneId)
    {
        var result = new LootResult();
        var zone = _database.GetZone(zoneId);
        if (zone == null || zone.lootTable == null || zone.lootTable.Count == 0) return result;

        float totalWeight = 0f;
        foreach (var entry in zone.lootTable) totalWeight += entry.dropWeight;

        for (int i = 0; i < LootRollsPerExpedition; i++)
        {
            float roll = Random.Range(0f, totalWeight);
            float cumulative = 0f;
            foreach (var entry in zone.lootTable)
            {
                cumulative += entry.dropWeight;
                if (roll <= cumulative)
                {
                    Apply(entry, result);
                    break;
                }
            }
        }
        return result;
    }

    private void Apply(LootEntry entry, LootResult result)
    {
        if (entry.type == LootType.Material)
        {
            result.materialsGained.TryGetValue(entry.itemId, out int current);
            result.materialsGained[entry.itemId] = current + 1;
        }
        else
        {
            result.weaponsGained.Add(new WeaponInstance(System.Guid.NewGuid().ToString(), entry.itemId));
        }
    }
}