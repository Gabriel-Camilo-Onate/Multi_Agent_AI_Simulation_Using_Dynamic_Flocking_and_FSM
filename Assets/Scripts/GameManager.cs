using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public LayerMask backgroundLayer;
    public LayerMask wallLayer;
    private List<Leader> _teamLeader = new List<Leader>();
    [SerializeField] private float _leaderArriveRadius;

    public float leaderArriveRadius { get { return _leaderArriveRadius;}}
    
    [SerializeField] private float _leaderMinArriveRadius;
    public float leaderMinArriveRadius { get { return _leaderMinArriveRadius; } }

    [SerializeField] private float _leaderArriveMaxForce;
    public float leaderArriveMaxForce { get { return _leaderArriveMaxForce; } }

    [SerializeField] private float _minDistanceToReachWaypointWhenPF;
    public float minDistanceToReachWaypointWhenPF { get { return _minDistanceToReachWaypointWhenPF; } }

    [SerializeField] private float _leaderMinimalVelocityToLookAtDirection;
    public float leaderMinimalVelocityToLookAtDirection { get { return _leaderMinimalVelocityToLookAtDirection; } }

    [SerializeField] private float _leaderViewRadius;
    public float leaderViewRadius { get { return _leaderViewRadius; } }
    [SerializeField] private float _leaderViewAngle;
    public float leaderViewAngle { get { return _leaderViewAngle; } }
    [SerializeField] private float _minionViewRadius;
    public float minionViewRadius { get { return _minionViewRadius; } }
    [SerializeField] private float _minionViewAngle;
    public float minionViewAngle { get { return _minionViewAngle; } }

    public Pathfinding pathfinding;

    [SerializeField] private DynamicWall _dynamicWall;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        if (!pathfinding)
        {
            pathfinding = GetComponent<Pathfinding>();
        }
    }

    void Update()
    {
        InputHandler();
    }
    private void InputHandler()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse0))
        {
            if(_teamLeader.Count>0)
            {
                RaycastHit hit;
                if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, backgroundLayer))
                {
                    _teamLeader[0].SetPoint(hit.point + Vector3.up * 0.5f);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKey(KeyCode.Mouse1))
        {
            if (_teamLeader.Count > 1)
            {
                RaycastHit hit;
                if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, backgroundLayer))
                {
                    _teamLeader[1].SetPoint(hit.point+Vector3.up*0.5f);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            SpawnDynamicWall();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            SpawnDynamicWall(90);
        }
    }
    public void SpawnDynamicWall(int yRotation=0)
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, wallLayer))
        {
            if (hit.collider.gameObject.GetComponent<DynamicWall>() != null)
            {
                Destroy(hit.collider.gameObject);
            }
        }
        else
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, backgroundLayer))
            {
                DynamicWall dynamicWall = Instantiate(_dynamicWall, hit.point+Vector3.up*1.5f, Quaternion.Euler(0, yRotation, 0));
                dynamicWall.ResizeWall();
            }
        }
        Grid.Instance.CheckWaypoints();
        NotifyAgentsChangeWall();
    }
    public void NotifyAgentsChangeWall()
    {
        foreach (var leader in _teamLeader)
        {
            StartCoroutine(leader.NotifyChangeWall());
        }
    }
    public void AddLeader(Leader leader)
    {
        if (!_teamLeader.Contains(leader))
        {
            _teamLeader.Add(leader);
        }
    }
}
