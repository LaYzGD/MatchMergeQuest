using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Rune : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private RuneType _type;

    private Action<Rune> _onDestroy;

    public RuneType Type => _type;

    public void Init(Action<Rune> destroyAction)
    {
        _onDestroy = destroyAction;
    }

    public void SetType(RuneType type)
    {
        _type = type;
        _spriteRenderer.sprite = _type.Sprite;
    }

    public void ShowSelectedSprite(bool isSelected)
    {
        _spriteRenderer.sprite = isSelected ? _type.SelectedSprite : _type.Sprite;
    }

    public void DestroyRune()
    {
        _onDestroy(this);
    }
}
