using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private InventorySlot[] _slots;
    [SerializeField] private InventoryData _data;
    [SerializeField] private Observer<InventoryItemType> _onEquip;
    [SerializeField] private Observer<InventoryItemType> _onUnEquip;

    private InventorySlot _selectedSlot;

    public void AddRandomArmor() => AddItem(_data.AllCommonArmor[Random.Range(0, _data.AllCommonArmor.Length)]);

    public void AddRandomWeapon() => AddItem(_data.AllCommonWeapons[Random.Range(0, _data.AllCommonWeapons.Length)]);

    public void AddRandomLegendaryWeapon() => AddItem(_data.AllLegendaryWeapons[Random.Range(0, _data.AllLegendaryWeapons.Length)]);

    public void SelectItem(InventorySlot slot)
    {
        if (!slot.HasItem)
        {
            if (_selectedSlot != null)
            {
                _selectedSlot.Select(false);
            }

            _selectedSlot = null;
            return;
        }

        if (_selectedSlot == null)
        {
            _selectedSlot = slot;
            _selectedSlot.Select(true);
            return;
        }

        if (slot.Item.ItemType != _selectedSlot.Item.ItemType || slot.Item.ItemType.NextUpgrade == null)
        {
            _selectedSlot.Select(false);
            _selectedSlot = slot;
            _selectedSlot.Select(true);
            return;
        }

        slot.Item.UpgradeItem();
        _selectedSlot.Select(false);
        _selectedSlot.RemoveItem();
        _selectedSlot = null;
    }

    public void EquipItem(InventorySlot slot)
    {
        if (_selectedSlot == null)
        {
            if (slot.HasItem)
            {
                AddItem(slot.Item.ItemType);
                _onEquip.Set(slot.Item.ItemType, false);
                _onUnEquip.Set(slot.Item.ItemType);
                slot.RemoveItem();
            }

            return;
        }

        if (slot.BaseType != _selectedSlot.Item.ItemType.Type)
        {
            _selectedSlot.Select(false);
            _selectedSlot = null;
            return;
        }

        if (!slot.HasItem)
        {
            slot.AddItem(_selectedSlot.Item.ItemType);
            _selectedSlot.RemoveItem();
            _selectedSlot.Select(false);
            _selectedSlot = null;
            _onEquip.Set(slot.Item.ItemType);
            return;
        }

        var item = _selectedSlot.Item.ItemType;
        _selectedSlot.AddItem(slot.Item.ItemType);
        slot.AddItem(item);
        _onEquip.Set(slot.Item.ItemType);
        _selectedSlot.Select(false);
        _selectedSlot = null;
    }

    private void AddItem(InventoryItemType type)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].HasItem) 
            {
                continue; 
            }

            _slots[i].AddItem(type);
            return;
        }
    }

    private void OnDestroy()
    {
        _onEquip.Dispose();
        _onUnEquip.Dispose();
    }
}
