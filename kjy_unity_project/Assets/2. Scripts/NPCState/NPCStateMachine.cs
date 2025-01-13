using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStateMachine : MonoBehaviour
{
    public enum StateType
    {
        PIdle, PWalk, PShake 
        // PPickup
    }
    
    public NPCStateBase CurrentState;
    private NPCController _npcController;
    private Animator _animator;
    
    private List<NPCStateBase> _states = new();

    private void Awake()
    {
        _npcController = GetComponent<NPCController>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // PlayerIdle playerIdle = new PlayerIdle(_npcController, _animator, this);
        // PlayerWalk playerWalk = new PlayerWalk(_npcController, _animator, this);
        // PlayerShakeTree playerShakeTree = new PlayerShakeTree(_npcController, _animator, this);
        // // PlayerPickup playerPickup = new PlayerPickup(_playerController, _animator, this);
        // AddState(playerIdle, playerWalk, playerShakeTree);
        //
        // OnChangeState(StateType.PIdle);
    }

    private void Update()
    {
        CurrentState.OnStateUpdate();
    }

    public void AddState(params StateBase[] states)
    {
        foreach (var state in states)
        {
            // _states.Add(state);
        }
    }

    public void OnChangeState(StateType type)
    {
        if (0 <= (int)type && (int)type < _states.Count)
        {
            CurrentState?.OnStateExit();
            // Debug.Log($"현재상태 {CurrentState}에서 나감");
            CurrentState = _states[(int)type];
            CurrentState.OnStateEnter();
            // Debug.Log($"다음상태 {CurrentState}(으)로 돌입");

        }
    }
}