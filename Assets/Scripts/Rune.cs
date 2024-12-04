using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Rune : MonoBehaviour
{
    private RuneType _type;

    public RuneType Type => _type;

    public void SetType(RuneType type)
    {
        _type = type;
        GetComponent<SpriteRenderer>().sprite = _type.Sprite;
    }
}
