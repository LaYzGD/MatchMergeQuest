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
    [SerializeField] private float _popDuration = 0.1f;
    [SerializeField] private float _popSize = 0.1f;
    [SerializeField] private int _popVibrato = 1;
    [SerializeField] private float _popElasticity = 0.5f;
    [Space]
    [SerializeField] private float _operationsDelay = 0.1f;

    private GridSystem<GridObject<Rune>> _gridSystem;

    private GridObject<Rune> _selectedCell;

    private void Start()
    {
        _gridSystem = new GridSystem<GridObject<Rune>>(_width, _height, _cellSize, _origin.position);
        _gridSystem.OnValueChanged += UpdateGridObjectCoordinates;
        _inputReader.Click += SelectRune;
        _gridSystem.CreateGrid(null);
        InitializeGrid();
    }

    private void FillGridCell(Vector2 pos)
    {
        Rune rune = Instantiate(_runePrefab, pos, Quaternion.identity);
        rune.SetType(_runeTypes[Random.Range(0, _runeTypes.Count)]);
        var gridObject = new GridObject<Rune>(_gridSystem, pos);
        gridObject.SetValue(rune);
        _gridSystem.SetValue(pos, gridObject);
    }

    private void InitializeGrid()
    {
        foreach (var pos in _gridSystem.GetPositions())
        {
            FillGridCell(pos);
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

        yield return StartCoroutine(CheckGridLogic());
    }

    private IEnumerator CheckGridLogic()
    {
        var matches = FindMatches();

        if (matches.Count > 0)
        {
            yield return StartCoroutine(DestroyMatchedRunes(matches));
            yield return StartCoroutine(MakeRunesFall());
            yield return StartCoroutine(FillEmptyCellsWithNewRunes());
        }
    }

    private IEnumerator SwapRunes(GridObject<Rune> selectedRune, GridObject<Rune> nextRune)
    {
        var selectedRuneCoordinates = selectedRune.Coordinates;
        var nextRuneCoordinates = nextRune.Coordinates;

        _gridSystem.SetValue(nextRuneCoordinates, selectedRune);
        _gridSystem.SetValue(selectedRuneCoordinates, nextRune);

        selectedRune.Object.transform.DOLocalMove(nextRuneCoordinates, _swapSpeed).SetEase(_ease);
        nextRune.Object.transform.DOLocalMove(selectedRuneCoordinates, _swapSpeed).SetEase(_ease);
        
        yield return new WaitForSeconds(_swapSpeed);
    }

    private IEnumerator DestroyMatchedRunes(List<Vector2> matches) 
    {
        foreach(var match in matches) 
        {
            var rune = _gridSystem.GetValue(match).Object;
            _gridSystem.SetValue(match, null);

            rune.transform.DOPunchScale(Vector2.one * _popSize, _popDuration, _popVibrato, _popElasticity);
            yield return new WaitForSeconds(_operationsDelay);
            rune.gameObject.SetActive(false);
        }
    }

    private IEnumerator FillEmptyCellsWithNewRunes()
    {
        var positions = _gridSystem.GetPositions();

        foreach (var pos in positions)
        {
            if (_gridSystem.GetValue(pos) == null)
            {
                FillGridCell(pos);
                yield return new WaitForSeconds(_operationsDelay);
            }
        }

        DeselectCell();

        StartCoroutine(CheckGridLogic());
    }

    private IEnumerator MakeRunesFall()
    {
        var positions = _gridSystem.GetPositionsAsArray();

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                if (_gridSystem.GetValue(positions[y,x]) == null) 
                {
                    for (int i = y + 1; i < _height; i++)
                    {
                        var cell = _gridSystem.GetValue(positions[i, x]);
                        if (cell != null) 
                        {
                            var rune = cell.Object;
                            _gridSystem.SetValue(positions[y, x], cell);
                            rune.transform.DOLocalMove(cell.Coordinates, _swapSpeed).SetEase(_ease);
                            _gridSystem.SetValue(positions[i, x], null);
                            yield return new WaitForSeconds(_operationsDelay);
                            break;
                        }
                    }
                }
            }
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

    private void UpdateGridObjectCoordinates(Vector2 newCoord, GridObject<Rune> obj)
    {
        if (obj == null) return;

        obj.UpdateCoordinates(newCoord);
    }

    private void DeselectCell()
    {
        _selectedCell = null;
    }

    private void OnDestroy()
    {
        _inputReader.Click -= SelectRune;
        _gridSystem.OnValueChanged -= UpdateGridObjectCoordinates;
    }
}
