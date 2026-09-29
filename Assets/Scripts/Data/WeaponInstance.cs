using System.Collections.Generic;

[System.Serializable]
public class WeaponInstance
{
    public string instanceId; // UNIQUE PER WEAPON
    public string weaponId;  // MAKE SURE THIS MATCH SO ID
    public int tier; 
    public List<StatModifier> appliedModifiers = new();

    public WeaponInstance(string instanceId, string weaponId)
    {
        this.instanceId = instanceId;
        this.weaponId = weaponId;
        tier = 0;
    }
}