using UnityEngine;

[CreateAssetMenu(fileName = "NewMaterial", menuName = "AdcraftSim/Material Definition")]
public class MaterialDefinitionSO : ScriptableObject
{
    public string materialId;
    public string displayName;
    public Sprite icon;
    public StatModifier upgradeModifier;
    public int tierRequired;
}