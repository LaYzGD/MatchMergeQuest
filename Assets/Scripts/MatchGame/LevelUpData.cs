using UnityEngine;

[CreateAssetMenu(menuName = "Data/LevelUpData", fileName = "New LevelUpData")]
public class LevelUpData : ScriptableObject
{
    [field: SerializeField] public int StartingLevel { get; private set; } = 1;
    [field: SerializeField] public int StartRequiredXP { get; private set; } = 1000;

    [SerializeField] private float _requiredXPStep = 100;
    [SerializeField] private float _requiredXPMultiplier = 0.5f;

    public int GetNextStep(int currentLevel)
    {
        return Mathf.RoundToInt(_requiredXPStep * currentLevel * _requiredXPMultiplier);
    }
}
