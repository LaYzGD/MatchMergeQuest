using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/MatchSystem", fileName = "New MatchSystemData")]
public class MatchSystemData : ScriptableObject
{
    [field: SerializeField] public int Height { get; private set; } = 5;
    [field: SerializeField] public int Width { get; private set; } = 5;
    [field: SerializeField] public float CellSize { get; private set; } = 1f;
    [field: SerializeField] public List<RuneType> RuneTypes { get; private set; }
    [field: SerializeField] public float SwapSpeed { get; private set; } = 0.5f;
    [field: SerializeField] public Ease Ease { get; private set; } = Ease.InQuad;
    [field: SerializeField] public float PopDuration { get; private set; } = 0.1f;
    [field: SerializeField] public float PopSize { get; private set; } = 0.1f;
    [field: SerializeField] public int PopVibrato { get; private set; } = 1;
    [field: SerializeField] public float PopElasticity { get; private set; } = 0.5f;
    [field: SerializeField] public float DefaultOperationsDelay { get; private set; } = 0.1f;
    [field: SerializeField] public float CreateOperationsDelay { get; private set; } = 0.2f;
    [field: SerializeField] public float SwapOperationsDelay { get; private set; } = 0.5f;
    [field: SerializeField] public int MaxCombo { get; private set; } = 20;
}
