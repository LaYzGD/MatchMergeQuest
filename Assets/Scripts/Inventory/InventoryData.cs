using UnityEngine;

[CreateAssetMenu(menuName = "Data/InventoryData", fileName = "New InventoryData")]
public class InventoryData : ScriptableObject
{
    [field: SerializeField] public InventoryItemType[] AllCommonWeapons { get; private set; }
    [field: SerializeField] public InventoryItemType[] AllCommonArmor { get; private set; }
    [field: SerializeField] public InventoryItemType[] AllLegendaryWeapons { get; private set; }
    [field: SerializeField] public float DefaultLuck { get; private set; } = 0.05f;
    [field: SerializeField] public float MaxtLuck { get; private set; } = 1f;
    [field: SerializeField] public float LuckIncreasement { get; private set; } = 0.05f;
}
