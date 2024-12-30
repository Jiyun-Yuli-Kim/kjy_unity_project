using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Cinemachine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private DialogueLoader _dialogueLoader;
    [SerializeField] private TextPresenter _textPresenter;
    
    [SerializeField] private CinemachineVirtualCamera[] cameras;
    
    // [SerializeField] private Animator _uiAnimator;
    
    string[,] _kindData;
    string[,] _idolData;
    string[,] _crankyData;
    
    public UnityEvent OnTalkStart; // 대화시작
    public UnityEvent OnTalkEnd; // 대화종료
    
    // 첫 대사의 개수
    private int _kindMaxRange = 5;
    private int _randIndex;
    private int _choice = 0;
    private int _indexOffset = 95;

    public string textToPrint;
    
    private void Awake()
    {
        Debug.Log(_player);
        if (_player == null)
        {
            Debug.LogError("Player is null");
        }
        
        // _uICanvas.SetActive(false);
        // _2opsPopup.SetActive(false);
        // _highlighter1.SetActive(false);
        // _highlighter2.SetActive(false);
        // _blinker.SetActive(false);
    }

    private void Start()
    {
        _dialogueLoader.OnKindLoaded.AddListener(OnKindDataLoaded);
        _dialogueLoader.StartLoad(DialogueLoader.KindDialogue);
        
        // _dialogueLoader.OnIdolLoaded.AddListener(OnIdolDataLoaded);
        // _dialogueLoader.StartLoad(DialogueLoader.IdolDialogue);
        //
        // _dialogueLoader.OnCrankyLoaded.AddListener(OnCrankyDataLoaded);
        // _dialogueLoader.StartLoad(DialogueLoader.CrankyDialogue);
        
        OnTalkStart.AddListener(StartInteraction);
        OnTalkEnd.AddListener(ResetInteraction);
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

    public IEnumerator TalkToKindVillager()
    {
        if (_player.partnerName == null)
        {
            Debug.Log("_player.partnerName is null");
        }

        yield return new WaitForSeconds(0.5f);

        // 대화 시작에 따른 각종 초기화. 줌인 + 팝업활성화 + interacting = true 
        OnTalkStart.Invoke();

        _randIndex = Random.Range(1, _kindMaxRange);

        // 랜덤으로 대사를 출력함
        textToPrint = _kindData[_randIndex, 1];
        StartCoroutine(_textPresenter.StartDialogue());
        
        string[] firstchoices = _kindData[_randIndex, 2].Split("|");
        CheckChoicesCount(_kindData, firstchoices, _randIndex);
        
        OnTalkEnd.Invoke();
    }

    private void CheckChoicesCount(string[,] data, string[] choices, int index)
    {
        // 선택지 배열이 한개인 경우 즉 선택지가 따로 없는 경우
        if (choices.Length == 1)
        {
            // 다음 대사 로드
            if (data[index, 3] == "9999")
            {
                // 대화 끝내기 로직
                OnTalkEnd.Invoke();
            }
           
            textToPrint = data[j = int.Parse(data[index, 3])-_indexOffset, 1];
            StartCoroutine(_textPresenter.LoadNextLine());
            
            string[] nextchoices = data[int.Parse(data[index, 3]) - _indexOffset, 2].Split("|");
            CheckChoicesCount(data, nextchoices, int.Parse(data[index, 3]) - _indexOffset);
        }
        
        else if (choices.Length == 2)
        {
            Debug.Log("선택지가 두개인 경우");

            int i = 0;
            
            _textPresenter.SetChoices(choices);

            StartCoroutine(_textPresenter.GetChoice());
            _choice = _textPresenter.choice;
            string[] ss = data[index, 3].Split("|");
            
            if(_choice == 0)
            {
                if (ss[0] == "9999")
                {
                    OnTalkEnd.Invoke();
                    return;
                }
                i = int.Parse(ss[0]);
            }

            if (_choice == 1)
            {
                if (ss[1] == "9999")
                {
                    OnTalkEnd.Invoke();
                    return;
                }
                i = int.Parse(ss[1]);
            }

            textToPrint = data[i - _indexOffset, 1];
            StartCoroutine(_textPresenter.LoadNextLine());
            
            string[] nextchoices = data[i - _indexOffset, 2].Split("|");
            CheckChoicesCount(data, nextchoices, i - _indexOffset);
        }
    }
    
    public void StartInteraction()
    {
        TalkCamOn();
        _player.isInteracting = true;
    }
    
    public void ResetInteraction()
    {
        TalkCamOff();
        _player.isInteracting = false;
        _textPresenter.EndDialogue();
    }

    public void TalkCamOn()
    {
        cameras[1].Priority = 11;
    }
    
    public void TalkCamOff()
    {
        cameras[1].Priority = 9;
    }
}
    
