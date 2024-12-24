using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public enum StateType
    {
        PIdle, PWalk, PRun
    }
    
    protected PlayerController _controller;
    protected Animator _animator;
    protected StateMachine _stateMachine;
    
    private List<StateBase> _states = new();
    public StateBase CurrentState;
    
    private void Start()
    {
        var idle = new PlayerIdle(_controller, _animator, this);
        var walk = new PlayerWalk(_controller, _animator, this);
        var run = new PlayerRun(_controller, _animator, this);

        AddState(idle, walk, run);
    }

    private void Update()
    {
        CurrentState.OnStateUpdate();
    }

    public void AddState(params StateBase[] states)
    {
        foreach (var state in states)
        {
            _states.Add(state);
        }
    }

    public void OnChangeState(StateType type)
    {
        if (0 < (int)type && (int)type < _states.Count)
        {
            CurrentState?.OnStateExit();
            CurrentState = _states[(int)type];
            CurrentState.OnStateEnter();
        }
    }
}
