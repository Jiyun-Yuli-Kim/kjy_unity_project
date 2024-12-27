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
        Debug.Log(_player);
        if (_player == null)
        {
            Debug.LogError("Player is null");
        }
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
        // _dialogueLoader.ShowCSVData(_kindData);
    }
    
    private void OnIdolDataLoaded()
    {
        _idolData = _dialogueLoader.DialogueData; 
        // _dialogueLoader.ShowCSVData(_idolData);
    }
    
    private void OnCrankyDataLoaded()
    {
        _crankyData = _dialogueLoader.DialogueData; 
        // _dialogueLoader.ShowCSVData(_crankyData);
    }

    private void Update()
    {
    }
    
    public IEnumerator TalkToKindVillager() 
    {
        Debug.Log("코루틴 정상 시행");
        _npcName.text = _player.partnerName;
        if (_player.partnerName == null)
        {
            Debug.Log("_player.partnerName is null");
        }
        Debug.Log(_player.partnerName);

        _uICanvas.SetActive(true);
        yield return new WaitForSeconds(3f);

        _dialogueText.text = "안농나는농어야"; 
    }

    // 현재 _dialogueData[0,i] : index,
    // _dialogueData[1,i] : NPC 대사
    // _dialogueData[1,i] : 플레이어 대답
    // _dialogueData[1,i] : 다음 인덱스
}
