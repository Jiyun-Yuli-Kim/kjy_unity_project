using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public enum StateType
    {
        PIdle, PWalk, PShake, PPickup
    }
    
    public StateBase CurrentState;
    private PlayerController _playerController;
    private Animator _animator;
    
    private List<StateBase> _states = new();

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        PlayerIdle playerIdle = new PlayerIdle(_playerController, _animator, this);
        PlayerWalk playerWalk = new PlayerWalk(_playerController, _animator, this);
        PlayerShakeTree playerShakeTree = new PlayerShakeTree(_playerController, _animator, this);
        PlayerPickup playerPickup = new PlayerPickup(_playerController, _animator, this);
        AddState(playerIdle, playerWalk, playerShakeTree, playerPickup);
        
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
        if (0 <= (int)type && (int)type < _states.Count)
        {
            CurrentState?.OnStateExit();
            CurrentState = _states[(int)type];
            CurrentState.OnStateEnter();
        }
    }
}
