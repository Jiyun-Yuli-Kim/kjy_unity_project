using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public enum StateType
    {
        PIdle, PWalk, PRun
    }
    
    public StateBase CurrentState;
    
    private List<StateBase> _states = new();
    
    private void Awake()
    {
        CurrentState._controller = FindObjectOfType<PlayerController> ();
        CurrentState._animator = FindObjectOfType<Animator>();
        CurrentState._stateMachine = this;
    }

    private void Start()
    {
        PlayerIdle playerIdle = new PlayerIdle(CurrentState._controller, CurrentState._animator, CurrentState._stateMachine);
        PlayerWalk playerWalk = new PlayerWalk(CurrentState._controller, CurrentState._animator, CurrentState._stateMachine);
        PlayerRun playerRun = new PlayerRun(CurrentState._controller, CurrentState._animator, CurrentState._stateMachine);
        AddState(playerIdle, playerWalk, playerRun);
        
        OnChangeState(StateType.PIdle);
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
