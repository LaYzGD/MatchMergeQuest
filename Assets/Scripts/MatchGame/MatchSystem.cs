using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Zenject;

public class MatchSystem : MonoBehaviour
{
    [SerializeField] private Transform _origin;
    [Space]
    [SerializeField] private RunePool _runePool;
    [SerializeField] private VFXPool _sparkVFXPool;
    [SerializeField] private VFXPool _explosionVFXPool;

    private AudioPlayer _audioPlayer;
    private Camera _mainCamera;
    private InputReader _inputReader;
    private MatchComboUI _comboUI;
    private Points _points;

    private MatchSystemData _data;
    private MatchAudioData _audioData;

    private MatchCombo _matchCombo;
    private GridSystem<GridObject<Rune>> _gridSystem;
    private bool _isOperationPerfoming = false;
    private GridObject<Rune> _selectedCell;
    private bool _makeExplosion = false;

    public bool IsUIPanelShown { get; set; } = false;

    [Inject]
    public void Construct(AudioPlayer audioPlayer, Camera mainCamera, InputReader inputReader, MatchComboUI comboUi, MatchSystemData data, MatchAudioData audioData, Points points)
    {
        _audioPlayer = audioPlayer;
        _mainCamera = mainCamera;
        _inputReader = inputReader;
        _comboUI = comboUi;
        _data = data;
        _audioData = audioData;
        _points = points;
    }

    private void Start()
    {
        _gridSystem = new GridSystem<GridObject<Rune>>(_data.Width, _data.Height, _data.CellSize, _origin.position);
        _matchCombo = new MatchCombo(_data.MaxCombo);
        _isOperationPerfoming = false;
        _runePool.Init();
        _gridSystem.OnValueChanged += UpdateGridObjectCoordinates;
        _matchCombo.OnMaxCombo += CreateExplosion;
        _inputReader.Click += SelectRune;
        _gridSystem.CreateGrid(null);
        InitializeGrid();
    }

    private void FillGridCell(Vector2 pos)
    {
        Rune rune = _runePool.SpawnRune();
        rune.SetType(_data.RuneTypes[Random.Range(0, _data.RuneTypes.Count)]);
        rune.transform.position = pos;
        var gridObject = new GridObject<Rune>(_gridSystem, pos);
        gridObject.SetValue(rune);
        _gridSystem.SetValue(pos, gridObject);
        rune.transform.DOPunchScale(Vector2.one * _data.PopSize, _data.PopDuration, _data.PopVibrato, _data.PopElasticity);
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
        if (_isOperationPerfoming || IsUIPanelShown)
        {
            return;
        }

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
            _isOperationPerfoming = false;
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
        _isOperationPerfoming = true;
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
        else 
        {
            _isOperationPerfoming = false;
            _matchCombo.Reset();
            _comboUI.ShowText(false);
        }
    }

    private IEnumerator SwapRunes(GridObject<Rune> selectedRune, GridObject<Rune> nextRune)
    {
        var selectedRuneCoordinates = selectedRune.Coordinates;
        var nextRuneCoordinates = nextRune.Coordinates;

        _gridSystem.SetValue(nextRuneCoordinates, selectedRune);
        _gridSystem.SetValue(selectedRuneCoordinates, nextRune);

        selectedRune.Object.transform.DOLocalMove(nextRuneCoordinates, _data.SwapSpeed).SetEase(_data.Ease);
        nextRune.Object.transform.DOLocalMove(selectedRuneCoordinates, _data.SwapSpeed).SetEase(_data.Ease);
        //_audioPlayer.PlaySound(_data.SwapSound);

        DeselectCell();
        yield return new WaitForSecondsRealtime(_data.SwapOperationsDelay);
    }

    private IEnumerator DestroyMatchedRunes(List<Vector2> matches) 
    {
        int counter = 1;
        _comboUI.ShowText(true);

        foreach(var match in matches) 
        {
            var rune = _gridSystem.GetValue(match).Object;
            _gridSystem.SetValue(match, null);

            rune.transform.DOPunchScale(Vector2.one * _data.PopSize, _data.PopDuration, _data.PopVibrato, _data.PopElasticity);
            _matchCombo.IncreaseCombo(counter);
            _comboUI.IncreaseCombo(_matchCombo.Combo);
            yield return new WaitForSecondsRealtime(_data.DefaultOperationsDelay);
            _sparkVFXPool.SpawnVFX(match);
            _audioPlayer.PlaySound(_audioData.PopSound);
            _points.AddScore(_data.PointsPerRune);
            rune.DestroyRune();
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
                _audioPlayer.PlaySound(_audioData.CreateSound);
                yield return new WaitForSecondsRealtime(_data.CreateOperationsDelay);
            }
        }

        if (_makeExplosion)
        {
            StartCoroutine(DestroyAllRunes());
        }
        else
        {
            StartCoroutine(CheckGridLogic());
        }
    }

    private IEnumerator MakeRunesFall()
    {
        var positions = _gridSystem.GetPositionsAsArray();

        for (int y = 0; y < _data.Height; y++)
        {
            for (int x = 0; x < _data.Width; x++)
            {
                if (_gridSystem.GetValue(positions[y,x]) == null) 
                {
                    for (int i = y + 1; i < _data.Height; i++)
                    {
                        var cell = _gridSystem.GetValue(positions[i, x]);
                        if (cell != null) 
                        {
                            var rune = cell.Object;
                            _gridSystem.SetValue(positions[y, x], cell);
                            rune.transform.DOLocalMove(cell.Coordinates, _data.SwapSpeed).SetEase(_data.Ease);
                            _gridSystem.SetValue(positions[i, x], null);
                            yield return new WaitForSecondsRealtime(_data.DefaultOperationsDelay);
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
        for (int y = 0; y < _data.Height; y++)
        {
            for (int x = 0; x < _data.Width - 2; x++)
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
        for (int x = 0; x < _data.Width; x++)
        {
            for (int y = 0; y < _data.Height - 2; y++)
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

    private void CreateExplosion()
    {
        _makeExplosion = true;
    }

    private IEnumerator DestroyAllRunes()
    {
        var positions = _gridSystem.GetPositions();

        //_explosionVFXPool.SpawnVFX(_gridSystem.GetCenter());

        foreach (var position in positions)
        {
            var cell = _gridSystem.GetValue(position);
            if (cell == null)
            {
                continue;
            }
            _explosionVFXPool.SpawnVFX(cell.Coordinates);
            _audioPlayer.PlaySound(_audioData.ExplodeSound);
            var rune = cell.Object;
            _gridSystem.SetValue(position, null);
            _points.AddScore(_data.PointsPerRune);
            rune.DestroyRune();
            yield return new WaitForSecondsRealtime(_data.DefaultOperationsDelay);
        }

        _makeExplosion = false;
        StartCoroutine(FillEmptyCellsWithNewRunes());
        yield return null;
    }

    private void SelectCell(Vector2 pos)
    {
        var cell = _gridSystem.GetValue(pos);
        
        if (cell == null)
        {
            return;
        }

        _audioPlayer.PlaySound(_audioData.SelectSound);
        _selectedCell = cell;
        _selectedCell.Object.ShowSelectedSprite(true);
    }

    private void UpdateGridObjectCoordinates(Vector2 newCoord, GridObject<Rune> obj)
    {
        if (obj == null) return;

        obj.UpdateCoordinates(newCoord);
    }

    private void DeselectCell()
    {
        _selectedCell.Object.ShowSelectedSprite(false);
        _selectedCell = null;
    }

    public void BlockGame(bool flag) 
    {
        IsUIPanelShown = flag;
    }

    private void OnDestroy()
    {
        _inputReader.Click -= SelectRune;
        _gridSystem.OnValueChanged -= UpdateGridObjectCoordinates;
        _matchCombo.OnMaxCombo -= CreateExplosion;
    }
}
