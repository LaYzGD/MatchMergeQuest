using UnityEngine;

public class GridObject<T>
{
    private GridSystem<GridObject<T>> _grid;
    private float _xPos;
    private float _yPos;
    private T _object;

    public T Object => _object;

    public Vector2 Coordinates => new Vector2(_xPos, _yPos);

    public GridObject(GridSystem<GridObject<T>> grid, Vector2 position) 
    {
        _grid = grid;
        _xPos = position.x;
        _yPos = position.y;
    }

    public void SetValue(T obj)
    {
        _object = obj;
    }
}
