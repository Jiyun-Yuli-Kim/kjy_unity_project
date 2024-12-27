using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private DialogueLoader _dialogueLoader;
    
    [SerializeField] private GameObject _uICanvas;
    [SerializeField] private TextMeshProUGUI _npcName;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private GameObject _blinker;
    [SerializeField] private Animator _uiAnimator;
    
    string[,] _kindData;
    string[,] _idolData;
    string[,] _crankyData;
    
    private void Awake()
    {
        _uICanvas.SetActive(true);
        _npcName.text = "사이다";
        _dialogueText.text = "안농나는농어야"; 
    }

    private void Start()
    {
        _dialogueLoader.OnKindLoaded.AddListener(OnKindDataLoaded);
        _dialogueLoader.StartLoad(DialogueLoader.KindDialogue);
        
        _dialogueLoader.OnIdolLoaded.AddListener(OnIdolDataLoaded);
        _dialogueLoader.StartLoad(DialogueLoader.IdolDialogue);
        
        _dialogueLoader.OnCrankyLoaded.AddListener(OnCrankyDataLoaded);
        _dialogueLoader.StartLoad(DialogueLoader.CrankyDialogue);
    }

    private void OnKindDataLoaded()
    {
        _kindData = _dialogueLoader.DialogueData; 
        _dialogueLoader.ShowCSVData(_kindData);
    }
    
    private void OnIdolDataLoaded()
    {
        _idolData = _dialogueLoader.DialogueData; 
        _dialogueLoader.ShowCSVData(_idolData);
    }
    
    private void OnCrankyDataLoaded()
    {
        _crankyData = _dialogueLoader.DialogueData; 
        _dialogueLoader.ShowCSVData(_crankyData);
    }

    private void Update()
    {
        TriggerDialogue();
    }

    private void TriggerDialogue()
    {
        if (_player._metKind && _player.isTriggered)
        {
            _player._isInteracting = true;
            Debug.Log("친절함 유형 주민과 대화");
            _player._isInteracting = false;
        }

        if (_player._metIdol && _player.isTriggered)
        {
            _player._isInteracting = true;
            Debug.Log("아이돌 유형 주민과 대화");
            _player._isInteracting = false;
        }
    }
    
    // 현재 _dialogueData[0,i] : index,
    // _dialogueData[1,i] : NPC 대사
    // _dialogueData[1,i] : 플레이어 대답
    // _dialogueData[1,i] : 다음 인덱스
}
