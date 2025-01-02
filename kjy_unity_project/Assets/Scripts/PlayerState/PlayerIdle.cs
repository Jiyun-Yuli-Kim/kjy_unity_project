using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class PlayerIdle : StateBase
{
    public PlayerIdle(PlayerController controller, Animator animator, StateMachine stateMachine) : base(controller, animator, stateMachine)
    {
        
    }

    public override void OnStateEnter()
    {
        _animator.SetBool("isMoving", false);
        _animator.SetBool("isDashing", false);
    }

    public override void OnStateUpdate()
    {
        if (_controller.isMoving)
        {
            _stateMachine.OnChangeState(StateMachine.StateType.PWalk);
        }

        if (_controller.isShakingTree)
        {
            _stateMachine.OnChangeState(StateMachine.StateType.PShake);
        }
    }

    public override void OnStateExit()
    {

    }
}
