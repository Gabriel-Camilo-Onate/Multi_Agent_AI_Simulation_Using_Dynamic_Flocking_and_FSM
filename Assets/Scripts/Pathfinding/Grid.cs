using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    GameObject[,] grid;
    [SerializeField] int _width;
    [SerializeField] int _height;
    [SerializeField] float _offset=1;
    [SerializeField] GameObject _waypoint;
    [SerializeField] private List<Node> _waypoints=new List<Node>();
    public List<Node> waypoints { get { return _waypoints; } }
    private Vector3 _startPointOffset;
    public static Grid Instance;

    [SerializeField] private bool _createGrid;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
    void Start()
    {
        if (_createGrid)
        {
            _startPointOffset = new Vector3(_width/2, 0, _height/2);
            grid = new GameObject[_width, _height];

            for (int x = 0; x < _width; x++)
            {
                for (int z = 0; z < _height; z++)
                {
                    GameObject waypoint = Instantiate(_waypoint);
                    grid[x, z] = waypoint;
                    waypoint.GetComponent<GridNode>().Initialize(x, z, new Vector3((x * waypoint.transform.localScale.x) * _offset,0, (z * waypoint.transform.localScale.z) * _offset) - _startPointOffset, this);
                    waypoints.Add(waypoint.GetComponent<GridNode>());
                }
            }
        }
    }
    public void AddNode(Node nodeToAdd)
    {
        if (!waypoints.Contains(nodeToAdd))
        {
            waypoints.Add(nodeToAdd);
        }
    }
    public Node GetNode(int x, int z)
    {
        if (x < 0 || x > _width - 1 || z < 0 || z > _height - 1) return null;

        return grid[x, z].GetComponent<Node>();
    }
    public Node GetClosestWaypointPosition(Vector3 position)
    {
        Node closestWaypointPosition;
        float distance=0;
        if (waypoints.Count > 0)
        {
            distance = Vector3.Distance(position, waypoints[0].transform.position);
            closestWaypointPosition = waypoints[0];
        }
        else
        {
            Debug.Log("Error fatal, no hay waypoints");
            GameObject waypoint = Instantiate(_waypoint);
            waypoint.GetComponent<GridNode>().Initialize(0, 0, Vector3.zero, this);
            return waypoint.GetComponent<GridNode>();
        }
        foreach (var waypoint in waypoints)
        {
            if(Vector3.Distance(position,waypoint.transform.position) < distance 
                && GameManager.instance.pathfinding.InSight(position,waypoint.transform.position))
            {
                closestWaypointPosition = waypoint;
                distance = Vector3.Distance(position, waypoint.transform.position);
            }
        }
        return closestWaypointPosition;
    }
    public void CheckWaypoints()
    {
        foreach (var waypoint in waypoints)
        {
            waypoint.CheckNeighbors();
        }
    }
}
