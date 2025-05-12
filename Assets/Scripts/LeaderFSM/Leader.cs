using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leader : Entity
{

    private Vector3 _targetPoint;
    public Vector3 targetPoint {get{return _targetPoint;}}
    public enum LeaderStates
    {
        Idle,
        MoveToPoint,
        MoveInPath,
        Attack,
        Defeated,
        Dead
    }
    [SerializeField] private FSM<LeaderStates> _fsm;
    public FSM<LeaderStates> fsm { get { return _fsm; } }
    public List<Node> waypoints=new List<Node>();
    
    protected override void Start()
    {
        base.Start();
        GameManager.instance.AddLeader(this);
        _targetPoint = transform.position;
        _fsm = new FSM<LeaderStates>();
        _fsm.AddState(LeaderStates.Idle,new LeaderIdleState(this));
        _fsm.AddState(LeaderStates.MoveToPoint,new LeaderMoveToPointState(this));
        _fsm.AddState(LeaderStates.MoveInPath,new LeaderPFMovement(this));
        _fsm.ChangeState(LeaderStates.Idle);
    }
    void Update()
    {
        _fsm.OnUpdate();
        if(FOVManager.instance.IsInFieldOfView(team, GameManager.instance.leaderViewRadius, GameManager.instance.leaderViewAngle,this))
        {
            Debug.Log(""+gameObject.name+"       "+ _targetsToAttack.Count); // aca ya puedo pasar al modo de ataque
                                                                             // o lo que sea en base a lo que me salga
        }
        Movement();
    }
    private void Movement()
    {
        if(_velocity.magnitude>GameManager.instance.leaderMinimalVelocityToLookAtDirection)
            transform.forward = _velocity;

        transform.position += _velocity * Time.deltaTime;
    }
    public void SetPoint(Vector3 point)
    {
        _targetPoint = point;
        if (GameManager.instance.pathfinding.InSight(transform.position, targetPoint))
        {
            fsm.ChangeState(LeaderStates.MoveToPoint);
        }
        else
        {
            fsm.ChangeState(LeaderStates.MoveInPath);
        }
    }
    public Vector3 Seek(Vector3 pos)
    {
        Vector3 desired = pos - transform.position;
        Vector3 steering = desired * _speed;
        steering = Vector3.ClampMagnitude(steering, _maxForce);
        return steering;
    }
    public Vector3 Arrive(Vector3 positionToArrive)
    {
        Vector3 desired = positionToArrive - transform.position;
        desired.Normalize();

        float speed = _speed;
        float distanceToTarget = Vector3.Distance(positionToArrive, transform.position);
        if (distanceToTarget < GameManager.instance.leaderArriveRadius)
        {
            speed = _speed * (distanceToTarget / GameManager.instance.leaderArriveRadius);
        }
        desired *= speed;

        Vector3 steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, GameManager.instance.leaderArriveMaxForce);
        return steering;
    }
    public void ApplyForce(Vector3 force)
    {
        _velocity += force;
        _velocity = Vector3.ClampMagnitude(_velocity, _speed);
    }
    public override bool IsMinion()
    {
        return false;
    }
    public IEnumerator NotifyChangeWall()
    {
        yield return new WaitForSeconds(0.01f);
        if (Vector3.Distance(transform.position,_targetPoint) > GameManager.instance.leaderMinimalVelocityToLookAtDirection)
        {
            if (!GameManager.instance.pathfinding.InSight(transform.position, targetPoint))
            {
                _fsm.ChangeState(LeaderStates.MoveInPath);
            }
            else
            {
                _fsm.ChangeState(LeaderStates.MoveToPoint);
            }
        }
    }
   
}
