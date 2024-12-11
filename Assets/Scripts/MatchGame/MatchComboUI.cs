using TMPro;
using UnityEngine;
using DG.Tweening;

public class MatchComboUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _comboText;
    [SerializeField] private readonly string _comboString = "COMBO";
    [Space]
    [SerializeField] private float _punchScale = 0.1f;
    [SerializeField] private float _punchDuration = 0.1f;
    [SerializeField] private int _punchVibrato = 2;
    [SerializeField] private float _punchElasticity = 0.5f;
    [SerializeField] private Ease _ease = Ease.InQuad;

    public void ShowText(bool flag) 
    {
        _comboText.gameObject.SetActive(flag);
    }

    public void IncreaseCombo(int combo)
    {
        _comboText.text = $"{_comboString}: {combo}";
        _comboText.rectTransform.DOPunchScale(Vector2.one * _punchScale, _punchDuration, _punchVibrato, _punchElasticity).SetEase(_ease);
    }
}
