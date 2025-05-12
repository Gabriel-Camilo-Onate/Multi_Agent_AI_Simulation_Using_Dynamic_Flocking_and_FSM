using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM<T>
{
    private Dictionary<T, IState> _allStates=new Dictionary<T, IState>();
    private IState _currentState;
    private void Awake()
    {
        if (_currentState == null)
        {
            _currentState = new BlankState();
        }
    }
    public void OnUpdate()
    {
        if(_currentState!=null)
        _currentState.OnUpdate();
    }
    public void AddState(T newStateKey,IState newStateValue)
    {
        if (!_allStates.ContainsKey(newStateKey))
        {
            _allStates.Add(newStateKey, newStateValue);
        }
    }
    public void ChangeState(T newState)
    {
        if (_allStates.ContainsKey(newState))
        {
            _currentState?.OnExit();
            _currentState = _allStates[newState];
            _currentState?.OnEnter();
        }
    }
    private class BlankState : IState
    {
        public void OnEnter()
        {
            throw new System.NotImplementedException();
        }

        public void OnExit()
        {
            throw new System.NotImplementedException();
        }

        public void OnUpdate()
        {
            throw new System.NotImplementedException();
        }
    }
}
