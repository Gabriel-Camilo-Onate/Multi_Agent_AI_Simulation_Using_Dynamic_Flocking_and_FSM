using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode : Node
{
    [SerializeField] private int _x;
    [SerializeField] private int _z;
    private Grid _grid;
    protected override void Start()
    {
    }
    public override Dictionary<Node,bool> GetNeighbors()
    {
        if (_neighbors.Count != 0) return _neighbors;
        Node neighbor = _grid.GetNode(_x, _z - 1);  //Up
        if (neighbor != null) 
            _neighbors.Add(neighbor,true);
        neighbor = _grid.GetNode(_x + 1, _z); //Right
        if (neighbor) 
            _neighbors.Add(neighbor,true);
        neighbor = _grid.GetNode(_x, _z + 1);//Down
        if (neighbor) 
            _neighbors.Add(neighbor, true);
        neighbor = _grid.GetNode(_x - 1, _z);//Left
        if (neighbor) 
            _neighbors.Add(neighbor, true);
        
        return base.GetNeighbors();
    }
    public void Initialize(int x, int z, Vector3 pos, Grid grid)
    {
        _x = x;
        _z = z;
        transform.position = pos;
        _grid = grid;
    }
}
