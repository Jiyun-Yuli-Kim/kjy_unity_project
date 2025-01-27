using System.Collections;
using System.Collections.Generic;
using Boxophobic.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class NPCController : MonoBehaviour, IInteractable, ITalkable
{
    [SerializeField] public NPCData npcData;
    // 플레이어 방향을 따라 고개 돌리도록
    [SerializeField] public GameObject NPCHead;
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] public Animator animator;
    
    public event UnityAction OnInteract;
    public event UnityAction OnInteractEnd;
    
    public bool isStrolling;

    public void Interact()
    {
        Talk();
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void Talk()
    {
        DialogueSystem.Instance.OnTalkStart.AddListener(StopMoving);
        DialogueSystem.Instance.OnTalkEnd.AddListener(ResumeMoving);
        StartCoroutine(DialogueSystem.Instance.TalkToVillager());
    }

    public void StopMoving()
    {
        agent.isStopped = true;
    }

    public void ResumeMoving()
    {
        agent.isStopped = false;
    }

}
