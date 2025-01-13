using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class NPCStroll : NPCStateBase
{
    [SerializeField] public float UpdateInterval;
    private float _curTime;
    [SerializeField] NavMeshAgent _agent;
    
    public NPCStroll(NPCController controller, Animator animator, NPCStateMachine stateMachine) : base(controller, animator, stateMachine)
    {
        
    }
    

    // 마을 배회 시작
    public override void OnStateEnter()
    {
        // NPC를 집 앞의 SpawnPoint에 Instantiate한다.
        _curTime = UpdateInterval;

    }

    public override void OnStateUpdate()
    {
        if (DateTime.Now.Hour >= _controller.npcData.StrollEndHour)
        {
            _stateMachine.OnChangeState(NPCStateMachine.StateType.NHome);
        }
        
        // NavMesh를 통한 랜덤 이동 구현
        _curTime += Time.deltaTime;
        if (_curTime >= UpdateInterval)
        {
            Vector3 randpos = GetRandPosOnNavMesh();
            _agent.SetDestination(randpos);
            _curTime = 0;
        }
    }

    public override void OnStateExit()
    {
        // NPC를 삭제한다
        Destroy(_controller.gameObject);
    }
    
    private Vector3 GetRandPosOnNavMesh()
    {
        Vector3 randomPos = Random.insideUnitSphere*40;
        randomPos += _controller.transform.position;
        NavMeshHit navHit;
        if (NavMesh.SamplePosition(randomPos, out navHit, 20f, NavMesh.AllAreas))
        {
            return navHit.position;
        }
        else
        {
            return _controller.transform.position;
        }
    }
}
