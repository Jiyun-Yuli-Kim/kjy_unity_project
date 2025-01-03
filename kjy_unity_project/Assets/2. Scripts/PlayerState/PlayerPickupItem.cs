using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickupItem : StateBase
{
    public PlayerPickupItem(PlayerController controller, Animator animator, StateMachine stateMachine) : base(controller, animator, stateMachine)
    {
        
    }
    
    public override void OnStateEnter()
    {
        Debug.Log("PlayerPickupItem.OnStateEnter");
        _animator.SetBool("isPickingUp", true);
    }
    
    public override void OnStateUpdate()
    {
    }

    public override void OnStateExit()
    {
        _animator.SetBool("isPickingUp", false);
    }
}
