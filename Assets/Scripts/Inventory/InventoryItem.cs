using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class InventoryItem : MonoBehaviour
{
    [SerializeField] private Image _image;
    public InventoryItemType ItemType { get; private set; }

    public void SetType(InventoryItemType type) 
    {
        ItemType = type;
        _image.sprite = ItemType.Sprite;
    }

    public void UpgradeItem()
    {
        var nextItemType = ItemType.NextUpgrade;
        if (nextItemType == null) return;
        
        ItemType = nextItemType;
        _image.sprite = ItemType.Sprite;
    }
}
