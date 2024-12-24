using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase
{
    [SerializeField] protected PlayerController _controller;
    [SerializeField] protected Animator _animator;
    [SerializeField] protected StateMachine _stateMachine;
    
    // 생성자를 통해 PlayerController, Animator를 가질 것을 강제함.
    public StateBase(PlayerController controller, Animator animator, StateMachine stateMachine)
    {
        _controller = controller;
        _animator = animator;
        _stateMachine = stateMachine;
    }

    public virtual void OnStateEnter()
    {
    }

    public virtual void OnStateExit()
    {
    }
    
    public virtual void OnStateUpdate()
    {
    }
}
