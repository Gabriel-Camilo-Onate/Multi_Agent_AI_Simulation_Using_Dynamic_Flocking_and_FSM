using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class LeaderPFMovement : IState
{
    private Leader _agent;
    private Action updateDelegate;
    private Vector3 lastPoint;
    public LeaderPFMovement(Leader agent)
    {
        _agent = agent;
    }
    public void OnEnter()
    {
        if (!GameManager.instance.pathfinding.InSight(_agent.transform.position, _agent.targetPoint))
        {
            FindAPosiblePath();
            if (_agent.waypoints == null)
            {
                _agent.fsm.ChangeState(Leader.LeaderStates.Idle);
                return;
            }
            bool continueSmoothing=true;
            while(continueSmoothing)
            {
                if(_agent.waypoints.Count > 2)
                {
                    if(GameManager.instance.pathfinding.InSight(_agent.transform.position, _agent.waypoints[1].transform.position))
                    {
                        _agent.waypoints.RemoveAt(0);
                    }
                    else
                    {
                        continueSmoothing = false;
                    }
                }
                else
                {
                    continueSmoothing = false;
                }
            }
        }
        else
        {
            _agent.fsm.ChangeState(Leader.LeaderStates.MoveToPoint);
        }
        updateDelegate = FollowPath;
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
        updateDelegate?.Invoke();
    }
    void FollowPath()
    {
        if (_agent.waypoints== null || _agent.waypoints.Count <= 0) return;

        float distance = Vector3.Distance(_agent.waypoints[0].transform.position, _agent.transform.position);
        if (distance < GameManager.instance.minDistanceToReachWaypointWhenPF)
        {
            if (_agent.waypoints.Count == 1)
            {
                if (GameManager.instance.pathfinding.InSight(_agent.transform.position, _agent.targetPoint))
                {
                    lastPoint = _agent.targetPoint;
                    updateDelegate = ArriveLastPoint;
                }
                else
                {
                    lastPoint = _agent.waypoints[0].transform.position;
                    updateDelegate = ArriveLastPoint;
                }
                return;
            }
            _agent.waypoints.RemoveAt(0);
        }
        _agent.ApplyForce(_agent.Seek(_agent.waypoints[0].transform.position));
        if (GameManager.instance.pathfinding.InSight(_agent.transform.position, _agent.targetPoint))
        {
            while (_agent.waypoints.Count > 1)
            {
                _agent.waypoints.RemoveAt(1);
            }
        }
    }
    private void ArriveLastPoint()
    {
        Vector3 arriveVector = _agent.Arrive(lastPoint);
        _agent.ApplyForce(arriveVector); 
        if (Vector3.Distance(_agent.transform.position, _agent.targetPoint) <= GameManager.instance.minDistanceToReachWaypointWhenPF)
        {
            _agent.ApplyForce(-_agent.velocity);
            _agent.fsm.ChangeState(Leader.LeaderStates.Idle);
        }
    }
    private void FindAPosiblePath()
    {
        _agent.waypoints = GameManager.instance.pathfinding.ConstructPathThetaStar
            (Grid.Instance.GetClosestWaypointPosition(_agent.transform.position)
            , Grid.Instance.GetClosestWaypointPosition(_agent.targetPoint));
        
        if (_agent.waypoints == null)
        {
            _agent.waypoints = GameManager.instance.pathfinding.ConstructPathToNearPlace(
                Grid.Instance.GetClosestWaypointPosition(_agent.transform.position),
                Grid.Instance.GetClosestWaypointPosition(_agent.targetPoint));
        }
        if (_agent.waypoints == null)
        {
            _agent.fsm.ChangeState(Leader.LeaderStates.Idle);
        }
    }
}
