using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class PlayerIdle : StateBase
{
    public PlayerIdle(PlayerController controller, Animator animator, StateMachine stateMachine) : base(controller, animator, stateMachine)
    {
        
    }

    void OnStateEnter()
    {
        _animator.SetBool("isMoving", false);
        _animator.SetBool("isDashing", false);
    }

    void OnStateUpdate()
    {
        if (_controller._isMoving)
        {
            _stateMachine.OnChangeState(StateMachine.StateType.PWalk);
        }
    }

    void OnStateExit()
    {
        
    }
}
