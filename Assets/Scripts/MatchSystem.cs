using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MatchSystem : MonoBehaviour
{
    [SerializeField] private int _width = 5;
    [SerializeField] private int _height = 5;
    [SerializeField] private float _cellSize = 1;
    [SerializeField] private Transform _origin;
    [Space]
    [SerializeField] private Rune _runePrefab;
    [SerializeField] private List<RuneType> _runeTypes;
    [Space]
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private InputReader _inputReader;
    [Space]
    [SerializeField] private float _swapSpeed = 0.5f;
    [SerializeField] private Ease _ease;

    private GridSystem<GridObject<Rune>> _gridSystem;

    private GridObject<Rune> _selectedCell;

    private void Start()
    {
        _gridSystem = new GridSystem<GridObject<Rune>>(_width, _height, _cellSize, _origin.position);
        _inputReader.Click += SelectRune;
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
            gridObject.SetValue(rune);
            _gridSystem.SetValue(pos, gridObject);
        }
    }

    private void SelectRune()
    {
        Vector2 gridPos;
        bool isSelected;
        var mousePos = _mainCamera.ScreenToWorldPoint(_inputReader.SelectedCellPosition);
        _gridSystem.TryGetCoordinates(mousePos, out gridPos, out isSelected);

        if (!isSelected) 
        {
            return;
        }

        if (_selectedCell != null && _selectedCell == _gridSystem.GetValue(gridPos))
        {
            DeselectCell();
            return;
        }

        if (_selectedCell == null)
        {
            SelectCell(gridPos);
            return;
        }

        StartCoroutine(GameLoop(_selectedCell, gridPos));
    }

    private IEnumerator GameLoop(GridObject<Rune> selectedRune, Vector2 gridPos) 
    {
        yield return StartCoroutine(SwapRunes(selectedRune, _gridSystem.GetValue(gridPos)));

        var matches = FindMatches();

        yield return StartCoroutine(DestroyMatchedRunes(matches));

        DeselectCell();
    }

    private IEnumerator SwapRunes(GridObject<Rune> selectedRune, GridObject<Rune> nextRune)
    {
        selectedRune.Object.transform.DOLocalMove(nextRune.Coordinates, _swapSpeed).SetEase(_ease);
        nextRune.Object.transform.DOLocalMove(selectedRune.Coordinates, _swapSpeed).SetEase(_ease);
        _gridSystem.SetValue(nextRune.Coordinates, selectedRune);
        _gridSystem.SetValue(selectedRune.Coordinates, nextRune);

        yield return new WaitForSeconds(_swapSpeed);
    }

    private IEnumerator DestroyMatchedRunes(List<Vector2> matches) 
    {
        foreach(var match in matches) 
        {
            var rune = _gridSystem.GetValue(match).Object;
            _gridSystem.SetValue(match, null);

            yield return new WaitForSeconds(0.1f);
            rune.gameObject.SetActive(false);
        }
    }

    private List<Vector2> FindMatches()
    {
        var positions = _gridSystem.GetPositionsAsArray();
        var matches = new HashSet<Vector2>();
        //HORIZONTAL
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width - 2; x++)
            {
                var cellA = _gridSystem.GetValue(positions[y, x]);
                var cellB = _gridSystem.GetValue(positions[y, x + 1]);
                var cellC = _gridSystem.GetValue(positions[y, x + 2]);


                if (cellA == null || cellB == null || cellC == null)
                {
                    continue;
                }

                //print($"A: {cellA.Coordinates}, B: {cellB.Coordinates}, C: {cellC.Coordinates}");

                if (cellA.Object.Type == cellB.Object.Type
                        && cellB.Object.Type == cellC.Object.Type)
                {
                    matches.Add(cellA.Coordinates);
                    matches.Add(cellB.Coordinates);
                    matches.Add(cellC.Coordinates);
                }
            }
        }
        //VERTICAL
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height - 2; y++)
            {
                var cellA = _gridSystem.GetValue(positions[y, x]);
                var cellB = _gridSystem.GetValue(positions[y + 1, x]);
                var cellC = _gridSystem.GetValue(positions[y + 2, x]);


                if (cellA == null || cellB == null || cellC == null)
                {
                    continue;
                }

                if (cellA.Object.Type == cellB.Object.Type
                        && cellB.Object.Type == cellC.Object.Type)
                {
                    matches.Add(cellA.Coordinates);
                    matches.Add(cellB.Coordinates);
                    matches.Add(cellC.Coordinates);
                }
            }
        }

        return new List<Vector2>(matches);
    }

    private void SelectCell(Vector2 pos)
    {
        _selectedCell = _gridSystem.GetValue(pos);
    }

    private void DeselectCell()
    {
        _selectedCell = null;
    }

    private void OnDestroy()
    {
        _inputReader.Click -= SelectRune;
    }
}
