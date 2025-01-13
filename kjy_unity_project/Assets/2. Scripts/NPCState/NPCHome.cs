using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHome : NPCStateBase
{
    public NPCHome(NPCController controller, Animator animator, NPCStateMachine stateMachine) : base(controller, animator, stateMachine)
    {
    }
    
    public override void OnStateEnter()
    {
        // NPC를 집 앞에 Instantiate한다
    }

    public override void OnStateUpdate()
    {
        // 집 안에서 랜덤 배회 
    }

    public override void OnStateExit()
    {
        // NPC를 현재 씬에서 제거한다
    }
}
