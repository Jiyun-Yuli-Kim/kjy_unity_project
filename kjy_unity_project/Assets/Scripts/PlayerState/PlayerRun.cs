using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRun : StateBase
{
    public PlayerRun(PlayerController controller, Animator animator, StateMachine stateMachine) : base(controller, animator, stateMachine)
    {
        
    }

    private float tempSpeed;

    void OnStateEnter()
    {
        _animator.SetBool("isDashing", true);
    }

    void OnStateUpdate()
    {
        if (!_controller._isDashing)
        {
            OnStateExit();        
        }

        tempSpeed = _controller._playerMoveSpeed;
        _controller._playerMoveSpeed *= _controller._dashMultiplier;
        
        _controller.PlayerMove();
    }

    void OnStateExit()
    {
        _controller._playerMoveSpeed = tempSpeed;
        _animator.SetBool("isDashing", false);
    }
}
