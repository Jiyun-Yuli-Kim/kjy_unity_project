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
    
    // 랜덤이동 구현용
    // [SerializeField] public float UpdateInterval;
    // private float _curTime;
    // [SerializeField] NavMeshAgent _agent;
    // 테스트용 목적지
    // [SerializeField] float _speed;
    
    public event UnityAction OnInteract;
    public event UnityAction OnInteractEnd;

    public string[,] _dialogueData;

    public bool isStrolling;
    
    public void Start()
    {
        DialogueSystem.Instance.OnDataLoaded.AddListener(SetData);
        // _curTime = UpdateInterval;
        // _agent.speed = _speed;
    }
    
    void Update()
    {
        // _curTime += Time.deltaTime;
        // if (_curTime >= UpdateInterval)
        // {
        //     Vector3 randpos = GetRandPosOnNavMesh();
        //     _agent.SetDestination(randpos);
        //     _curTime = 0;
        // }
    }

    // private Vector3 GetRandPosOnNavMesh()
    // {
    //     Vector3 randomPos = Random.insideUnitSphere*40;
    //     randomPos += transform.position;
    //     NavMeshHit navHit;
    //     if (NavMesh.SamplePosition(randomPos, out navHit, 20f, NavMesh.AllAreas))
    //     {
    //         return navHit.position;
    //     }
    //     else
    //     {
    //         return transform.position;
    //     }
    // }

    public void Interact()
    {
        Talk(_dialogueData);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void Talk(string[,] data)
    {
        // DialogueSystem.Instance.OnTalkStart.AddListener(DialogueSystem.Instance.StartInteraction);
        // DialogueSystem.Instance.OnTalkStart.AddListener(DialogueSystem.Instance.ResetInteraction);
        StartCoroutine(DialogueSystem.Instance.TalkToVillager(data));
    }

    private void SetData()
    {
        if (npcData.personality == Personalities.Kind)
        {
            _dialogueData = DialogueSystem.Instance.kindData;
            Debug.Log($"kind data loaded,{_dialogueData[1,1]}");
        }

        if (npcData.personality == Personalities.Idol)
        {
            _dialogueData = DialogueSystem.Instance.idolData;
            Debug.Log($"idol data loaded,{_dialogueData[1,1]}");
        }

        // if (npcData.personality == Personalities.Cranky)
        // {
        //     _dialogueData = DialogueSystem.Instance.crankyData;
        //     Debug.Log($"cranky data loaded,{_dialogueData[1,1]}");
        // }
    }

}
