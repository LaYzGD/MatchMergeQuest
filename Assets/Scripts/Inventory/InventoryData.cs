using UnityEngine;

[CreateAssetMenu(menuName = "Data/InventoryData", fileName = "New InventoryData")]
public class InventoryData : ScriptableObject
{
    [field: SerializeField] public InventoryItemType[] AllCommonWeapons { get; private set; }
    [field: SerializeField] public InventoryItemType[] AllCommonArmor { get; private set; }
    [field: SerializeField] public InventoryItemType[] AllLegendaryWeapons { get; private set; }
}
