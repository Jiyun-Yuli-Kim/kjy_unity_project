using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPCStateBase
{
    public NPCController _controller;
    public Animator _animator;
    public NPCStateMachine _stateMachine;
    
    // 생성자를 통해 PlayerController, Animator를 가질 것을 강제함.
    public NPCStateBase(PlayerController controller, Animator animator, StateMachine stateMachine)
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
