using UnityEngine;

public class Test : MonoBehaviour
{
    public GameDatabaseSO db;
    void Start()
    {
        db.Initialize();
        var zone = db.GetZone("forest");
        Debug.Log(zone != null ? $"Found zone: {zone.displayName}" : "Lookup failed");
    }
}
