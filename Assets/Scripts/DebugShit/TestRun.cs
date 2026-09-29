using UnityEngine;

public class TestHarness : MonoBehaviour
{
    [SerializeField] private string zoneIdToTest = "forest"; 

    private void OnGUI()
    {
        //if (Time.frameCount % 60 == 0) Debug.Log("OnGUI alive, frame " + Time.frameCount);
        GUI.Label(new Rect(10, 10, 300, 20), $"Gold: {EconomyManager.Instance.Gold}");

        int y = 40;
        foreach (var adventurer in AdventurerManager.Instance.Roster)
        {
            string status = adventurer.state == AdventurerState.Idle
                ? "Idle"
                : $"On expedition, {(adventurer.expeditionEndUtcTicks - System.DateTime.UtcNow.Ticks) / System.TimeSpan.TicksPerSecond}s left";
            GUI.Label(new Rect(10, y, 400, 20), $"{adventurer.name}: {status}");

            if (adventurer.state == AdventurerState.Idle && GUI.Button(new Rect(420, y, 160, 20), "Dispatch"))
            {
                bool success = AdventurerManager.Instance.Dispatch(adventurer.id, zoneIdToTest);
                Debug.Log(success ? "Dispatch succeeded" : $"Dispatch FAILED - check zoneId '{zoneIdToTest}' matches a real zone, and adventurer state");
            }

            y += 25;
        }

        GUI.Label(new Rect(10, y + 10, 400, 20),
            $"Weapons: {InventorySystem.Instance.Weapons.Count}  |  Materials: {InventorySystem.Instance.Materials.Count} types");

        if (GUI.Button(new Rect(10, y + 40, 160, 25), "Add Adventurer Slot"))
            AdventurerManager.Instance.AddSlot();
    }
}