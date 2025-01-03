using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalk : StateBase
{
    public PlayerWalk(PlayerController controller, Animator animator, StateMachine stateMachine) : base(controller, animator, stateMachine)
    {
        
    }

    private float tempSpeed;
    private bool isCurDashing = false;

    public override void OnStateEnter()
    {
        _animator.SetBool("isMoving", true);
    }

    public override void OnStateUpdate()
    {
        // move 눌렸을 때 이동, move&dash 눌렸을 때 달리기 
        if (_controller.isMoving)
        {

            if (_controller.isDashing && !isCurDashing)
            {   
                isCurDashing = true;
                _animator.SetBool("isDashing", true);
                tempSpeed = _controller.playerMoveSpeed;
                _controller.playerMoveSpeed *= _controller.dashMultiplier;
            }

            if(!_controller.isDashing && isCurDashing)
            {
                isCurDashing = false;
                _controller.playerMoveSpeed = tempSpeed;
                _animator.SetBool("isDashing", false);
            }

            _controller.PlayerMove();
        }

        if (!_controller.isMoving)
        {
            _stateMachine.OnChangeState(StateMachine.StateType.PIdle);
        }
    }

    public override void OnStateExit()
    {
        _animator.SetBool("isMoving", false);
    }
}
