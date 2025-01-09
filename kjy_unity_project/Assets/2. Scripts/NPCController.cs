using System.Collections;
using System.Collections.Generic;
using Boxophobic.Utils;
using UnityEngine;
using UnityEngine.Events;

public class NPCController : MonoBehaviour, IInteractable, ITalkable
{
    [SerializeField] public NPCData npcData;
    // 플레이어 방향을 따라 고개 돌리도록
    [SerializeField] public GameObject NPCHead;
    
    public event UnityAction OnInteract;
    public event UnityAction OnInteractEnd;

    private string[,] _dialogueData;
    
    public void Start()
    {
        DialogueSystem.Instance.OnDataLoaded.AddListener(SetData);
    }

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
