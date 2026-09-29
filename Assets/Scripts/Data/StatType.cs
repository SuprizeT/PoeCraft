using UnityEngine;

public enum StatType { Damage, AttackSpeed, CritChance, Durability }

[System.Serializable]
public struct StatModifier
{
    public StatType stat;
    public float value;
}
