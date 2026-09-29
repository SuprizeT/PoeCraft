using System.Collections.Generic;

[System.Serializable]
public class InventorySaveData
{
    public List<WeaponInstance> weapons;
    public List<string> materialIds; 
    public List<int> materialQtys;
}