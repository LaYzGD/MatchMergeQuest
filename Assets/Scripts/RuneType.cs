using UnityEngine;

[CreateAssetMenu(menuName = "Data/Runes", fileName = "NewRuneType")]
public class RuneType : ScriptableObject
{
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public Sprite SelectedSprite { get; private set; }
}
