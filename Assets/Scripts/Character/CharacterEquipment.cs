using UnityEngine;

public class CharacterEquipment : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _helmet;
    [SerializeField] private SpriteRenderer _armor;
    [SerializeField] private SpriteRenderer _weaponMain;
    [SerializeField] private Animator _animator;

    private Sprite _defaultHelmet;
    private Sprite _defaultArmor;
    private Sprite _defaultWeapon;
    private RuntimeAnimatorController _defaultController;

    private void Start()
    {
        _defaultHelmet = _helmet.sprite;
        _defaultArmor = _armor.sprite;
        _defaultWeapon = _weaponMain.sprite;
        _defaultController = _animator.runtimeAnimatorController;
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

        var animator = itemType.AnimatorOverrideController;

        if (animator != null)
        {
            _animator.runtimeAnimatorController = animator;
        }
    }

    public void OnUnequip(ItemType type) 
    {
        switch (type)
        {
            case ItemType.Weapon:
                _weaponMain.sprite = _defaultWeapon;
                _animator.runtimeAnimatorController = _defaultController;
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
