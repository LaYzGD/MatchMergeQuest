using System.Collections.Generic;
using UnityEngine;

public class MatchSystem : MonoBehaviour
{
    [SerializeField] private int _width = 5;
    [SerializeField] private int _height = 5;
    [SerializeField] private float _cellSize = 1;
    [SerializeField] private Transform _origin;
    [Space]
    [SerializeField] private Rune _runePrefab;
    [SerializeField] private List<RuneType> _runeTypes;

    private GridSystem<GridObject<Rune>> _gridSystem;

    private void Start()
    {
        _gridSystem = new GridSystem<GridObject<Rune>>(_width, _height, _cellSize, _origin.position);

        _gridSystem.CreateGrid(null);
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        foreach (var pos in _gridSystem.GetPositions())
        {
            Rune rune = Instantiate(_runePrefab, pos, Quaternion.identity);
            rune.SetType(_runeTypes[Random.Range(0, _runeTypes.Count)]);
            var gridObject = new GridObject<Rune>(_gridSystem, pos);
            _gridSystem.SetValue(pos, gridObject);
        }
    }
}
