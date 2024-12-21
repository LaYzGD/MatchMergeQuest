using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/ItemType", fileName = "New ItemType")]
public class InventoryItemType : ScriptableObject
{
    [field: SerializeField] public ItemType Type { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public int Attack { get; private set; }
    [field: SerializeField] public int Health { get; private set; }
    [field: SerializeField] public InventoryItemType NextUpgrade { get; private set; }
    [field: SerializeField] public AnimatorOverrideController AnimatorOverrideController { get; private set; }
}

[Serializable]
public enum ItemType 
{
    Weapon,
    Helmet,
    Armor,
    None
}
