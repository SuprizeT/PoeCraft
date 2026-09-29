using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewZone", menuName = "AdcraftSim/Zone Definition")]
public class ZoneDefinitionSO : ScriptableObject
{
    public string zoneId;
    public string displayName;
    public float durationSeconds;
    public List<LootEntry> lootTable;
}