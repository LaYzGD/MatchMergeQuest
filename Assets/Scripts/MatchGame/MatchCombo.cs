using System;
using UnityEngine;

public class MatchCombo
{
    private int _maxCombo;
    private int _combo;
    public event Action OnMaxCombo;

    public int Combo => _combo;

    public MatchCombo(int maxCombo) 
    {
        _maxCombo = maxCombo;
    }

    public void IncreaseCombo(int value)
    {
        _combo += value;

        if (_combo >= _maxCombo)
        {
            _combo = 0;
            OnMaxCombo?.Invoke();
        }
    }

    public void Reset()
    {
        _combo = 0;
    }
}