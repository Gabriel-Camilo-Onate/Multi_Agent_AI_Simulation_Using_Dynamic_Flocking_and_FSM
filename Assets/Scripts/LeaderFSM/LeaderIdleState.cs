using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderIdleState : IState
{
    private Leader _agent;
    public LeaderIdleState(Leader agent)
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
    }

}
