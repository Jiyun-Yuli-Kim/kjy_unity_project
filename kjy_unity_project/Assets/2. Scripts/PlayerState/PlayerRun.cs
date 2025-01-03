using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class PlayerRun : StateBase
// {
//     public PlayerRun(PlayerController controller, Animator animator, StateMachine stateMachine) : base(controller, animator, stateMachine)
//     {
//         
//     }
//
//     private float tempSpeed;
//
//     public override void OnStateEnter()
//     {
//         tempSpeed = _controller._playerMoveSpeed;
//         _controller._playerMoveSpeed *= _controller._dashMultiplier;
//         _animator.SetBool("isMoving", true);
//         _animator.SetBool("isDashing", true);
//     }
//
//     public override void OnStateUpdate()
//     {
//         if (!_controller._isDashing)
//         {
//             _stateMachine.OnChangeState(StateMachine.StateType.PWalk);
//         }
//         
//         _controller.PlayerMove();
//     }
//
//     public override void OnStateExit()
//     {
//         _controller._playerMoveSpeed = tempSpeed;
//         _animator.SetBool("isDashing", false);
//     }
// }
