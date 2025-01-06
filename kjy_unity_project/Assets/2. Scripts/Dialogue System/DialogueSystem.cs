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
using UnityEngine.Rendering.Universal;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance { get; private set; }
    [SerializeField] private PlayerController _player;
    private NPCController _npc;
    [SerializeField] private DialogueLoader _dialogueLoader;
    [SerializeField] private TextPresenter _textPresenter;
    
    [SerializeField] private CinemachineVirtualCamera[] cameras;
    
    // [SerializeField] private Animator _uiAnimator;

    private int _maxRange;
    public string[,] kindData;
    private int _kindMaxRange = 5;
    public string[,] idolData;
    private int _idolMaxRange = 2;
    public string[,] crankyData;
    private int _crankyMaxRange = 5;

    public UnityEvent OnDataLoaded;
    public UnityEvent OnTalkStart; // 대화시작
    public UnityEvent OnTalkEnd; // 대화종료
    
    private int _randIndex;
    private int _choice = 0;
    private int _indexOffset;

    public string textToPrint;
    
    private void Awake()
    {
        Debug.Log(_player);
        if (_player == null)
        {
            Debug.LogError("Player is null");
        }
        
        Init();

        // _dialogueLoader.StartLoad(DialogueLoader.KindDialogue);

    }

    private void Start()
    {
        
        // _dialogueLoader.OnKindLoaded.AddListener(OnKindDataLoaded);
        
        // _dialogueLoader.OnIdolLoaded.AddListener(OnIdolDataLoaded);
        // _dialogueLoader.StartLoad(DialogueLoader.IdolDialogue);
        //
        // _dialogueLoader.OnCrankyLoaded.AddListener(OnCrankyDataLoaded);
        // _dialogueLoader.StartLoad(DialogueLoader.CrankyDialogue);
        
        OnTalkStart.AddListener(StartInteraction);
        OnTalkEnd.AddListener(ResetInteraction);
    }

    private void Init()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            Instance = this;
        }

        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // private void OnKindDataLoaded()
    // {
    //     kindData = _dialogueLoader.DialogueData; 
    //     // _dialogueLoader.ShowCSVData(_kindData);
    // }
    //
    // private void OnIdolDataLoaded()
    // {
    //     idolData = _dialogueLoader.DialogueData; 
    //     // _dialogueLoader.ShowCSVData(_idolData);
    // }
    //
    // private void OnCrankyDataLoaded()
    // {
    //     crankyData = _dialogueLoader.DialogueData; 
    //     // _dialogueLoader.ShowCSVData(_crankyData);
    // }

    public IEnumerator TalkToVillager(string[,] data)
    {
        if (_player.partnerName == null)
        {
            Debug.Log("_player.partnerName is null");
        }

        yield return new WaitForSeconds(0.5f);

        // 대화 시작에 따른 각종 초기화. 줌인 + 팝업활성화 + interacting = true 
        OnTalkStart.Invoke();

        SetMaxRange(data);
        _randIndex = Random.Range(1, _maxRange);
        Debug.Log(_randIndex);

        // 랜덤으로 대사를 출력함
        Debug.Log(data);
        textToPrint = data[_randIndex, 1];
        yield return StartCoroutine(_textPresenter.StartDialogue());
        
        string[] firstchoices = data[_randIndex, 2].Split("|");
        yield return StartCoroutine(CheckChoicesCount(data, firstchoices, _randIndex));
        
        OnTalkEnd.Invoke();
    }

    private void SetMaxRange(string[,] data)
    {
        if (data == kindData)
        {
            _maxRange = _kindMaxRange;
        }

        if (data == idolData)
        {
            _maxRange = _idolMaxRange;
        }

        if (data == crankyData)
        {
            _maxRange = _crankyMaxRange;
        }
        
        _indexOffset = 100 - _maxRange;
    }

    private IEnumerator CheckChoicesCount(string[,] data, string[] choices, int index)
    {
        // 선택지 배열이 한개인 경우 즉 선택지가 따로 없는 경우
        if (choices.Length == 1)
        {
            // 다음 대사 로드
            if (data[index, 3].Trim() == "END")
            {
                OnTalkEnd.Invoke();
                yield break;
            }

            else
            {
                textToPrint = data[int.Parse(data[index, 3]) - _indexOffset, 1];
                yield return StartCoroutine(_textPresenter.LoadNextLine());

                string[] nextchoices = data[int.Parse(data[index, 3]) - _indexOffset, 2].Split("|");
                yield return CheckChoicesCount(data, nextchoices, int.Parse(data[index, 3]) - _indexOffset);
            }
        }
        
        else if (choices.Length == 2)
        {
            int i = 0;
            
            yield return StartCoroutine(_textPresenter.GetChoice(choices));
            _choice = _textPresenter.choice;
            string[] ss = data[index, 3].Split("|");
            
            if(_choice == 0)
            {
                if (ss[0].Trim() == "END")
                {
                    OnTalkEnd.Invoke();
                    yield break;
                }
                i = int.Parse(ss[0]);
            }

            if (_choice == 1)
            {
                if (ss[1].Trim() == "END")
                {
                    OnTalkEnd.Invoke();
                    yield break;
                }
                i = int.Parse(ss[1]);
            }
            textToPrint = data[i - _indexOffset, 1];
            yield return StartCoroutine(_textPresenter.LoadNextLine());
            
            string[] nextchoices = data[i - _indexOffset, 2].Split("|");
            yield return CheckChoicesCount(data, nextchoices, i - _indexOffset);
        }
        
        else if (choices.Length == 3)
        {
            int i = 0;
            
            yield return StartCoroutine(_textPresenter.GetChoice(choices));
            _choice = _textPresenter.choice;
            string[] ss = data[index, 3].Split("|");
            
            if(_choice == 0)
            {
                if (ss[0].Trim() == "END")
                {
                    OnTalkEnd.Invoke();
                    yield break;
                }
                i = int.Parse(ss[0]);
            }

            if (_choice == 1)
            {
                if (ss[1].Trim() == "END")
                {
                    OnTalkEnd.Invoke();
                    yield break;
                }
                i = int.Parse(ss[1]);
            }
            
            if (_choice == 2)
            {
                if (ss[2].Trim() == "END")
                {
                    OnTalkEnd.Invoke();
                    yield break;
                }
                i = int.Parse(ss[2]);
            }

            textToPrint = data[i - _indexOffset, 1];
            yield return StartCoroutine(_textPresenter.LoadNextLine());
            
            string[] nextchoices = data[i - _indexOffset, 2].Split("|");
            yield return CheckChoicesCount(data, nextchoices, i - _indexOffset);
        }
    }
    
    public void StartInteraction()
    {
        TalkCamOn();
        NPCLooksPlayer();
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

    public void NPCLooksPlayer()
    {
        // 지금 이것도 왜이닞 모르게 
        _player.NPC.transform.rotation = Quaternion.Lerp(
            _player.NPC.transform.rotation, 
            Quaternion.LookRotation(_player.gameObject.transform.position - _player.NPC.transform.position),
            10*Time.deltaTime);
        // _player.NPC.transform.rotation = Quaternion.LookRotation(_player.gameObject.transform.position - _player.NPC.transform.position);
    }
}
    
