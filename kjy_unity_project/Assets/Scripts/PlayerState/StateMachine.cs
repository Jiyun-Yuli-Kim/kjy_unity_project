using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public enum StateType
    {
        PIdle, PWalk, PRun
    }
    
    private List<StateBase> _states = new();
    public StateBase CurrentState { get; private set; }

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
        CurrentState.OnStateExit();
        CurrentState = _states[(int)type];
        CurrentState.OnStateEnter();
    }

}
