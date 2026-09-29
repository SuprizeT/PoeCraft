using System.Collections.Generic;
using UnityEngine;

public enum LootType { Material, BaseWeapon }

[System.Serializable]
public struct LootEntry
{
    public LootType type;
    public string itemId;
    [Range(0f, 1f)] public float dropWeight;
}
