using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Zenject;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image _image;
    [field: SerializeField] public ItemType BaseType { get; private set; } = ItemType.None;
    [field: SerializeField] public InventoryItem Item { get; private set; }

    private InventorySlotData _data;
    private Color _defaultColor;
    private Vector3 _defaultScale;

    public bool HasItem { get; private set; } = false;

    [Inject]
    public void Construct(InventorySlotData data)
    {
        _data = data;
        _defaultColor = _image.color;
        _defaultScale = _image.rectTransform.localScale;
    }

    public void Select(bool flag)
    {
        _image.color = flag ? _data.SelectedColor : _defaultColor;
    }

    public void OnEnter()
    {
        _image.rectTransform.DOScale(_data.PunchScale, _data.AnimationTime);
    }

    public void OnExit()
    {
        _image.rectTransform.DOScale(_defaultScale, _data.AnimationTime);
    }

    public void RemoveItem()
    {
        Item.gameObject.SetActive(false);
        HasItem = false;
    }

    public void AddItem(InventoryItemType type)
    {
        Item.SetType(type);
        Item.gameObject.SetActive(true);
        HasItem = true;
    }
}
