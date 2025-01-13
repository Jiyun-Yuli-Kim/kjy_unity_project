using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStateMachine : MonoBehaviour
{
    public enum StateType
    {
        NStroll, NHome
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
        NPCStroll npcStroll = new NPCStroll(_npcController, _animator, this);
        NPCHome npcHome = new NPCHome(_npcController, _animator, this);
        AddState(npcStroll, npcHome);
        OnChangeState(StateType.NStroll);
    }

    private void Update()
    {
        CurrentState.OnStateUpdate();
    }

    public void AddState(params NPCStateBase[] states)
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
            // Debug.Log($"현재상태 {CurrentState}에서 나감");
            CurrentState = _states[(int)type];
            CurrentState.OnStateEnter();
            // Debug.Log($"다음상태 {CurrentState}(으)로 돌입");
        }
    }
}
