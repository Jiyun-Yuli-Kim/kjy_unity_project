using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, IInteractable, ITalkable
{
    [SerializeField] public NPCData _npcData;
    // 플레이어 방향을 따라 고개 돌리도록
    [SerializeField] public GameObject NPCHead;
    [SerializeField] private PlayerController _player;

    Interact()
    {
        
    }

    Talk()
    {
        StartCoroutine(DialogueSystem.Instance.TalkToVillager());
    }

}
