using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShakeTree : StateBase
{
    public PlayerShakeTree(PlayerController controller, Animator animator, StateMachine stateMachine) : base(controller, animator, stateMachine)
    {
        
    }
    
    public override void OnStateEnter()
    {
        _animator.SetBool("isShaking", true);
    }
    
    public override void OnStateUpdate()
    {
        // if (!_controller.isShakingTree)
        // {
        //     _stateMachine.OnChangeState(StateMachine.StateType.PIdle);
        // }
    }

    public override void OnStateExit()
    {
        _animator.SetBool("isShaking", false);
    }
}
