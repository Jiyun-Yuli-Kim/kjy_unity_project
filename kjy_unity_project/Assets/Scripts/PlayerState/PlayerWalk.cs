using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalk : StateBase
{
    public PlayerWalk(PlayerController controller, Animator animator, StateMachine stateMachine) : base(controller, animator, stateMachine)
    {
        
    }

    public override void OnStateEnter()
    {
        _animator.SetBool("isMoving", true);
    }

    public override void OnStateUpdate()
    {
        _controller.PlayerMove();
        
        if (_controller._isDashing)
        {
            _stateMachine.OnChangeState(StateMachine.StateType.PRun);
        }

        if (!_controller._isMoving)
        {
            _stateMachine.OnChangeState(StateMachine.StateType.PIdle);
        }
    }

    public override void OnStateExit()
    {
        _animator.SetBool("isMoving", false);
    }
}
