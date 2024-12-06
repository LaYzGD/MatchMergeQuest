using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridSystem<T>
{
    private int _width;
    private int _height;
    private float _cellSize;
    private Vector2 _origin;
    private Dictionary<Vector2, T> _positionValuePairs;

    public event Action<Vector2, T> OnValueChanged; 

    public GridSystem(int width, int height, float cellSize, Vector2 origin)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _origin = origin;
    }

    public void CreateGrid(T defaultValue)
    {
        _positionValuePairs = new Dictionary<Vector2, T>();
        
        float currentXPos = _origin.x;
        float currentYPos = _origin.y;
        
        for (int i = 0; i < _height; i++)
        {
            for (int j = 0; j < _width; j++)
            {
                Vector2 cellCoordinates = new Vector2(currentXPos + (_cellSize / 2), currentYPos);

                _positionValuePairs.Add(cellCoordinates, defaultValue);
                currentXPos += _cellSize;
            }
            currentXPos = _origin.x;
            currentYPos += _cellSize; 
        }   
    }

    public List<Vector2> GetPositions() 
    {
        return _positionValuePairs.Keys.ToList();
    }

    public Vector2 GetCenter()
    {
        return _origin + new Vector2((_width / 2) + 1, (_height / 2) + 1) * _cellSize;
    }

    public Vector2[,] GetPositionsAsArray() 
    {
        Vector2[,] positionsArray = new Vector2[_height,_width];

        float currentXPos = _origin.x;
        float currentYPos = _origin.y;

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                Vector2 pos = new Vector2(currentXPos + (_cellSize / 2), currentYPos);
                positionsArray[y, x] = pos;
                currentXPos += _cellSize;
            }
            currentXPos = _origin.x;
            currentYPos += _cellSize;
        }

        return positionsArray;
    }

    public void TryGetCoordinates(Vector2 mousePos, out Vector2 returnValue, out bool value) 
    {
        value = false;
        returnValue = Vector2.zero;

        foreach (var pos in _positionValuePairs.Keys) 
        {
            var leftCellSide = pos.x - _cellSize / 2;
            var rightCellSide = pos.x + _cellSize / 2;
            var downCellSide = pos.y - _cellSize / 2;
            var upCellSide = pos.y + _cellSize / 2;
            if (leftCellSide <= mousePos.x && mousePos.x <= rightCellSide 
                && downCellSide <= mousePos.y && mousePos.y <= upCellSide)
            {
                value = true;
                returnValue = pos; 
                break;
            }
        }
    }

    public void SetValue(Vector2 coordinates, T value) 
    {
        _positionValuePairs[coordinates] = value;
        OnValueChanged?.Invoke(coordinates, value);
    }

    public T GetValue(Vector2 coordinates) 
    {
        return _positionValuePairs.GetValueOrDefault(coordinates);
    }
}
