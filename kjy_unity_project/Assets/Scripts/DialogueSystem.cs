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
    private PlayerInput _input;
    
    [SerializeField] private CinemachineVirtualCamera[] cameras;
    
    [SerializeField] private GameObject _uICanvas;
    [SerializeField] private GameObject _2opsPopup;
    [SerializeField] private GameObject _3opsPopup;

    [SerializeField] private TextMeshProUGUI _npcName;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private TextMeshProUGUI _choice1;
    [SerializeField] private TextMeshProUGUI _choice2;
    [SerializeField] private TextMeshProUGUI _choice3;

    [SerializeField] private Transform _pos1;
    [SerializeField] private Transform _pos2;
    [SerializeField] private Transform _pos3;
    
    [SerializeField] private GameObject _pointer;
    [SerializeField] private GameObject _blinker;
    [SerializeField] private Animator _uiAnimator;
    
    string[,] _kindData;
    string[,] _idolData;
    string[,] _crankyData;
    
    public UnityEvent OnTalkStart; // 대화시작
    public UnityEvent OnTalkEnd; // 대화종료
    public UnityEvent OnLineStart; // 대사시작
    public UnityEvent OnLineEnd; // 대사종료

    // 첫 대사의 개수
    private int _kindMaxRange = 5;
    
    private void Awake()
    {
        Debug.Log(_player);
        if (_player == null)
        {
            Debug.LogError("Player is null");
        }
        
        _input = _player.GetComponent<PlayerInput>();
        
        _uICanvas.SetActive(false);
        _2opsPopup.SetActive(false);
        _blinker.SetActive(false);

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
        
        OnTalkStart.AddListener(TalkCamOn);
        OnLineStart.AddListener(EndBlink);
        OnLineEnd.AddListener(StartBlink);
        OnTalkEnd.AddListener(ResetInteraction);
        OnTalkEnd.AddListener(TalkCamOff);
        
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
        _npcName.text = _player.partnerName;
        if (_player.partnerName == null)
        {
            Debug.Log("_player.partnerName is null");
        }
        
        OnTalkStart.Invoke();
        
        yield return new WaitForSeconds(0.5f);
        _uICanvas.SetActive(true);

        int randIndex = Random.Range(1, _kindMaxRange);

        LoadLine(_kindData, randIndex);
        
        yield return new WaitWhile(() => !_player.GetInputAB());
        _blinker.SetActive(false);

        // 먼저 선택들을 파싱해 배열에 넣고
        // 배열크기에 따라 다른 팝업을 띄운다
        string[] firstchoices = _kindData[randIndex, 2].Split("|");
        if (firstchoices.Length == 1)
        {
            // 다음 대사 로드
            LoadLine(_kindData, int.Parse(_kindData[randIndex,3]));
        }
        else if (firstchoices.Length == 2)
        {
            ShowChoices(firstchoices);
            int c = GetChoice();
            LoadNextLine(firstchoices, c);
        }

        // UI 연동 계획
        // 디폴트는 첫번째 선택지이다
        // 키보드나 게임패드 왼쪽 조이스틱의 south를 누르면 아래 옵션으로 포인터 이동
        
        _dialogueText.text = "다음에 로드할 텍스트입니다";
        yield return new WaitForSeconds(1f);
        OnLineEnd.Invoke();
        
        yield return new WaitWhile(() => !_player.GetInputAB());
        OnTalkEnd.Invoke();

    }

    // 인덱스를 매개변수로 받아 해당하는 인덱스의 대사를 띄움
    private void LoadLine(string[,] data, int i)
    {
        string line = data[i, 1];
        _dialogueText.text = line;
        OnLineEnd.Invoke();
    }

    // 선택지 배열을 매개변수로 받아 선택지를 팝업에 띄움
    private void ShowChoices(string[] choices)
    {
        _choice1.text = choices[0];
        _choice2.text = choices[1];
        _pointer.transform.position = _pos1.position;
        _2opsPopup.SetActive(true);
    }

    // 두개의 옵션중 플레이어의 선택을 받아 int값으로 반환함
    private int GetChoice()
    {
        int c = 0;
        
        while (true)
        {
            if (c == 0 && _player.isSouth)
            {
                _pointer.transform.position = _pos2.position;
                c++;
            }

            if (c == 1 && _player.isNorth)
            {
                _pointer.transform.position = _pos1.position;
                c--;
            }

            if (_player.isTriggered)
            {
                _2opsPopup.SetActive(false);
                return c;
            }

        }
    }

    private void LoadNextLine(string[] choices, int i)
    {
        string[] nextIndex = choices[3].Split("|");
        if (nextIndex.Length == 1)
        {
            if (nextIndex[0] == "9999")
            {
                // 대화종료로직
            }
            
            LoadLine( _kindData , int.Parse(nextIndex[0]));
            
        }
    }

    /*
     *경우의 수[1] 선택창이 없는 대화
     *이니시에이팅은 1열의 인덱스(_dialogueData[0,i]) 이걸 랜덤으로 불러온다
     *2열은 플레이어의 대답 항목인데(_dialogueData[1,i]), 여기서 항목이 "/"라면 3열의 포인터를 읽는다(int(_dialogueData[2,i])= int j)
     *3열 인덱스(j)에 해당하는 답변을 찾아 다음 대화를 로드(_dialogueData[0,j])
     *마찬가지로 반복하다가 _dialogueData[2,x] == 9999가 되는 순간 대화를 종료한다.
     *
     * 경우의 수[1] 선택창이 두개 있는 대화
     * 선택창 있는 대화에서, 새 대답 배열 a = Split('|')._dialogueData[1,i]
     * 새 배열의 카운트를 확인. 1개인 경우 위의 로직 그대로
     * 두 개인 경우, 포인터 항목도 마찬가지로 새 포인터 배열 p = Split('|')._dialogueData[2,i] 이렇게 쪼개준다.
     * a[0]을 선택한 경우, (_dialogueData[0, int(p[0])])에 해당하는 데이터 다시 로드
     *
     */

    // 현재 _dialogueData[0,i] : index,
    // _dialogueData[1,i] : NPC 대사
    // _dialogueData[2,i] : 플레이어 대답
    // _dialogueData[3,i] : 다음 인덱스



    public void ResetInteraction()
    {
        _player._isInteracting = false;
        _uICanvas.SetActive(false);
    }

    public void StartBlink()
    {
        _blinker.SetActive(true);
    }
    
    public void EndBlink()
    {
        _blinker.SetActive(true);
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
    
