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
    // [SerializeField] private TextMeshProUGUI _choice3;
    
    [SerializeField] private GameObject _highlighter1;
    [SerializeField] private GameObject _highlighter2;
    [SerializeField] private GameObject _blinker;
    [SerializeField] private Animator _uiAnimator;
    
    string[,] _kindData;
    string[,] _idolData;
    string[,] _crankyData;
    
    public UnityEvent OnTalkStart; // 대화시작
    public UnityEvent OnTalkEnd; // 대화종료
    public UnityEvent OnLineStart; // 대사시작
    public UnityEvent OnLineEnd; // 대사종료

    private bool _onChoiceEnd = false;
    
    // 첫 대사의 개수
    private int _kindMaxRange = 5;
    private int _randIndex;
    private int _choice = 0;
    
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
        _highlighter1.SetActive(false);
        _highlighter2.SetActive(false);
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
        
        OnTalkStart.AddListener(StartInteraction);
        OnLineStart.AddListener(EndBlink);
        OnLineEnd.AddListener(StartBlink);
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
        _npcName.text = _player.partnerName;
        if (_player.partnerName == null)
        {
            Debug.Log("_player.partnerName is null");
        }
        
        // yield return new WaitForSeconds(0.5f);

        // 대화 시작에 따른 각종 초기화. 줌인 + 팝업활성화 + interacting = true 
        OnTalkStart.Invoke();
        
        // 대화 종료시 빠져나올 수 있도록 루프 설정
        while (_player.isInteracting)
        {
            _randIndex = Random.Range(1, _kindMaxRange);

            // 랜덤으로 대사를 출력함
            LoadLine(_kindData, _randIndex);
            yield return new WaitWhile(() => !_input.actions["Trigger"].WasPressedThisFrame());
            _blinker.SetActive(false);

            // 먼저 선택들을 파싱해 배열에 넣고
            // 배열크기에 따라 다른 팝업을 띄운다
            // firstchoices-플레이어가 선택할 수 있는 답변들의 배열 ["이야기하자!", "다음에 봐"]
            string[] firstchoices = _kindData[_randIndex, 2].Split("|");
            
            // 선택지 배열이 한개인 경우 즉 선택지가 따로 없는 경우
            if (firstchoices.Length == 1)
            {
                Debug.Log("선택지가 한개인 경우");
                // 다음 대사 로드
                LoadLine(_kindData, int.Parse(_kindData[_randIndex, 3])-95);
                yield return new WaitWhile(() => !_input.actions["Trigger"].WasPressedThisFrame());
                _blinker.SetActive(false);
                // LoadNextLine(_kindData, int.Parse(_kindData[_randIndex, 3]));

            }
            else if (firstchoices.Length == 2)
            {
                ShowChoices(firstchoices);
                StartCoroutine(GetChoice());
                yield return new WaitWhile(() => !_onChoiceEnd);
                LoadNextLine(_kindData, _randIndex);
            }

            // _dialogueText.text = "다음에 로드할 텍스트입니다";
            // yield return new WaitForSeconds(1f);
            // OnLineEnd.Invoke();

            yield return new WaitWhile(() => !_input.actions["Trigger"].WasPressedThisFrame());
            OnTalkEnd.Invoke();
        }
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
        _highlighter1.SetActive(true);
        _2opsPopup.SetActive(true);
    }

    // 두개의 옵션중 플레이어의 선택을 받아 int값으로 반환함
    IEnumerator GetChoice()
    {
        _player.isChoosing = true;
        Debug.Log("선택 코루틴 정상시행");
        while (!_input.actions["Trigger"].WasPressedThisFrame())
        {
            if (_choice == 0 && _input.actions["South"].WasPressedThisFrame())
            { 
                _highlighter1.SetActive(false);
                _highlighter2.SetActive(true);
                _choice=1;
            }

            if (_choice == 1 && _input.actions["North"].WasPressedThisFrame())
            {
                _highlighter2.SetActive(false);
                _highlighter1.SetActive(true);
                _choice=0;
            }
            
            yield return null;
        }
            _highlighter2.SetActive(false);
            _2opsPopup.SetActive(false);
            _onChoiceEnd = true;
            _player.isChoosing = false;
    }

    // 전체 데이터셋과 인덱스를 매개변수로 받아 다음 대사의 인덱스를 찾고 해당 대사를 로드한다.
    private void LoadNextLine(string[,] data, int i)
    {
        string[] nextIndex = data[i, 3].Split("|");
        if (nextIndex.Length == 1)
        {
            if (nextIndex[0] == "9999")
            {
                Debug.Log("대화종료 로직 시행");
                _player.isInteracting = false;
                OnTalkEnd.Invoke();
                return;
                // 대화종료로직
            }
            
            LoadLine( _kindData , int.Parse(nextIndex[0])-95);
        }

        if (nextIndex.Length == 2)
        {
            if (_choice == 0)
            {
                Debug.Log("1번 선택에 따른 로직 시행");
                LoadLine( _kindData , int.Parse(nextIndex[0])-95);
            }

            if (_choice == 1)
            {
                Debug.Log("2번 선택에 따른 로직 시행");
                // LoadLine( _kindData , int.Parse(nextIndex[1])-95);
            }
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
    
    public void StartInteraction()
    {
        TalkCamOn();
        _player.isInteracting = true;
        _uICanvas.SetActive(true);
    }
    
    public void ResetInteraction()
    {
        TalkCamOff();
        _player.isInteracting = false;
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
    
