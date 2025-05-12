using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderMoveToPointState : IState
{
    private Leader _agent;
    public LeaderMoveToPointState(Leader agent)
    {
        _agent = agent;
    }
    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
        GoToPoint();
    }
    private void GoToPoint()
    {
        if(GameManager.instance.pathfinding.InSight(_agent.transform.position, _agent.targetPoint))
        {
            if (Vector3.Distance(_agent.transform.position, _agent.targetPoint) < GameManager.instance.leaderArriveRadius)
            {
                _agent.ApplyForce(_agent.Arrive(_agent.targetPoint));
                if (_agent.velocity.magnitude <= GameManager.instance.leaderMinimalVelocityToLookAtDirection)
                {
                    _agent.ApplyForce(-_agent.velocity);
                    _agent.fsm.ChangeState(Leader.LeaderStates.Idle);
                }
            }
            else
            {
                _agent.ApplyForce(_agent.Seek(_agent.targetPoint));
            }
        }
        else
        {
            _agent.fsm.ChangeState(Leader.LeaderStates.MoveInPath);
        }
    }

}
