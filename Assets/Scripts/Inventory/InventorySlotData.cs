using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/InventorySlot", fileName = "New InventorySlotData")]
public class InventorySlotData : ScriptableObject
{
    [field: SerializeField] public Color SelectedColor { get; private set; } = Color.white;
    [field: SerializeField] public Vector3 PunchScale { get; private set; } = new Vector3(1.1f, 1.1f, 1f);
    [field: SerializeField] public float AnimationTime { get; private set; } = 0.1f;
    [field: SerializeField] public float AnimationElasticity { get; private set; } = 0.1f;
    [field: SerializeField] public int AnimationVibrato { get; private set; } = 1;
    [field: SerializeField] public Ease Ease { get; private set; } = Ease.InQuad;
}
