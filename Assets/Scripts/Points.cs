using UnityEngine;

public class Points : MonoBehaviour
{
    [SerializeField] private LevelUpData _levelUpData;
    [Space]
    [SerializeField] private Observer<int> _scorePoints;
    [SerializeField] private Observer<int> _levelPoints;

    private int _currentRequiredXP;

    private void Start()
    {
        _levelPoints.Set(_levelUpData.StartingLevel, false);
        _currentRequiredXP = _levelUpData.StartRequiredXP;
    }

    public void AddScore(int score)
    {
        if (score <= 0)
        {
            return;
        }

        _scorePoints.Value += score;

        if (_scorePoints.Value >= _currentRequiredXP)
        {
            var currentXP = _scorePoints.Value;
            _scorePoints.Set(currentXP - _currentRequiredXP);
            _levelPoints.Value++;
            _currentRequiredXP += _levelUpData.GetNextStep(_levelPoints.Value);
        }
    }

    private void OnDestroy()
    {
        _scorePoints.Dispose();
        _levelPoints.Dispose();
    }
}
