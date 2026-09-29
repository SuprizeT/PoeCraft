using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "AdcraftSim/Weapon Definition")]
public class WeaponDefinitionSO : ScriptableObject
{
    public string weaponId;
    public string displayName;
    public Sprite icon;
    public int baseSellPrice;
    public List<StatModifier> baseStats;
}