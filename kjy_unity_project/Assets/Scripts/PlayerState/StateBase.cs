using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase
{
    protected PlayerController _controller;
    protected Animator _animator;
    protected StateMachine _stateMachine;
    
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
