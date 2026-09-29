public enum AdventurerState { Idle, OnExpedition }

[System.Serializable]
public class Adventurer
{
    public string id;
    public string name;
    public AdventurerState state;
    public string currentZoneId;
    public long expeditionEndUtcTicks; 
}