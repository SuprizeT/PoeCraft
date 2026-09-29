using System.Text;
using TMPro; 
using UnityEngine;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private TMP_Text displayText;

    private void OnEnable() => InventorySystem.Instance.OnInventoryChanged += Refresh;
    private void OnDisable() => InventorySystem.Instance.OnInventoryChanged -= Refresh;
    private void Start() => Refresh();

    private void Refresh()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Gold: {EconomyManager.Instance.Gold}");
        sb.AppendLine("\nWeapons:");
        foreach (var w in InventorySystem.Instance.Weapons)
            sb.AppendLine($"  {w.weaponId} (Tier {w.tier})");
        sb.AppendLine("\nMaterials:");
        foreach (var kvp in InventorySystem.Instance.Materials)
            sb.AppendLine($"  {kvp.Key} x{kvp.Value}");

        displayText.text = sb.ToString();
    }
}