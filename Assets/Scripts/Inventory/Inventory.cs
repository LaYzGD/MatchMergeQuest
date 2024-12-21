using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private InventorySlot[] _slots;
    [SerializeField] private InventoryData _data;
    [SerializeField] private Observer<InventoryItemType> _onEquip;
    [SerializeField] private Observer<ItemType> _onUnEquip;

    [SerializeField] private GameObject _failedView;
    [SerializeField] private GameObject _successView;
    [SerializeField] private Image _legendaryWeaponImage;
    
    private float _currentLuck;

    private InventorySlot _selectedSlot;

    private void Awake()
    {
        _currentLuck = _data.DefaultLuck;
        print(_currentLuck);
    }

    public void AddRandomArmor() => AddItem(_data.AllCommonArmor[Random.Range(0, _data.AllCommonArmor.Length)]);

    public void AddRandomWeapon() => AddItem(_data.AllCommonWeapons[Random.Range(0, _data.AllCommonWeapons.Length)]);

    public void AddRandomLegendaryWeapon() 
    {
        float rand = Random.Range(0f, _data.MaxtLuck);
        
        if (_currentLuck < rand)
        {
            _failedView.SetActive(true);
            return;
        }

        var item = _data.AllLegendaryWeapons[Random.Range(0, _data.AllLegendaryWeapons.Length)];
        _successView.SetActive(true);
        _legendaryWeaponImage.sprite = item.Sprite;
        AddItem(item); 
    }

    public void IncreaseLuck() 
    {
        if (_currentLuck >= _data.MaxtLuck)
        {
            _currentLuck = _data.MaxtLuck;
            return;
        }

        _currentLuck += _data.LuckIncreasement;
    }

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

        if (_selectedSlot == slot)
        {
            _selectedSlot.Select(false);
            _selectedSlot = null;
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
                _onEquip.Set(null, false);
                _onUnEquip.Set(slot.Item.ItemType.Type);
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
            _onUnEquip.Set(ItemType.None, false);
            return;
        }

        var item = _selectedSlot.Item.ItemType;
        _selectedSlot.AddItem(slot.Item.ItemType);
        _onUnEquip.Set(ItemType.None, false);
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
