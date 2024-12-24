using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalk : StateBase
{
    public PlayerWalk(PlayerController controller, Animator animator, StateMachine stateMachine) : base(controller, animator, stateMachine)
    {
        
    }

    private float tempSpeed;

    public override void OnStateEnter()
    {
        _animator.SetBool("isMoving", true);
    }

    public override void OnStateUpdate()
    {
        // move 눌렸을 때 이동, move&dash 눌렸을 때 달리기 
        if (_controller._isMoving)
        {

            if (_controller._isDashing)
            {
                _animator.SetBool("isDashing", true);
                tempSpeed = _controller._playerMoveSpeed;
                _controller._playerMoveSpeed *= _controller._dashMultiplier;
            }
            _controller.PlayerMove();
            _controller._playerMoveSpeed = tempSpeed;
            _animator.SetBool("isDashing", false);
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
