using UnityEngine;

public class CharacterEquipment : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _helmet;
    [SerializeField] private SpriteRenderer _armor;
    [SerializeField] private SpriteRenderer _weaponMain;

    private Sprite _defaultHelmet;
    private Sprite _defaultArmor;
    private Sprite _defaultWeapon;

    private void Start()
    {
        _defaultHelmet = _helmet.sprite;
        _defaultArmor = _armor.sprite;
        _defaultWeapon = _weaponMain.sprite;
    }

    public void OnEquip(InventoryItemType itemType)
    {
        var type = itemType.Type;

        switch (type) 
        {
            case ItemType.Weapon:
                _weaponMain.sprite = itemType.Sprite;
                break;
            case ItemType.Helmet:
                _helmet.sprite = itemType.Sprite;
                break;
            case ItemType.Armor:
                _armor.sprite = itemType.Sprite;
                break;
        }
    }

    public void UnEquip(InventoryItemType itemType) 
    {
        var type = itemType.Type;

        switch (type)
        {
            case ItemType.Weapon:
                _weaponMain.sprite = _defaultWeapon;
                break;
            case ItemType.Helmet:
                _helmet.sprite = _defaultHelmet;
                break;
            case ItemType.Armor:
                _armor.sprite = _defaultArmor;
                break;
        }
    }
}
