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
    
    [SerializeField] private CinemachineVirtualCamera[] _cameras;
    [SerializeField] private CinemachineTargetGroup _targetGroup;
    
    // [SerializeField] private Animator _uiAnimator;

    private int _maxRange;
    public string[,] kindData;
    private int _kindMaxRange = 7;
    public string[,] idolData;
    private int _idolMaxRange = 3;
    // public string[,] crankyData;
    // private int _crankyMaxRange = 1;

    public UnityEvent OnDataLoaded;
    public UnityEvent OnTalkStart; // 대화시작
    public UnityEvent OnTalkEnd; // 대화종료
    
    private int _randIndex;
    private int _choice = 0;
    private int _indexOffset;

    private bool isTalking;

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

    public IEnumerator TalkToVillager(string[,] data)
    {
        if (isTalking)
        {
            yield break;
        }

        isTalking = true;
        
        if (_player.partnerName == null)
        {
            Debug.Log("_player.partnerName is null");
        }
        
        OnTalkStart.AddListener(StartInteraction);
        OnTalkEnd.AddListener(ResetInteraction);

        yield return new WaitForSeconds(0.5f);

        SetMaxRange(data);
        
        // 대화 시작에 따른 각종 초기화. 줌인 + 팝업활성화 + interacting = true 
        OnTalkStart.Invoke();

        _randIndex = Random.Range(1, _maxRange + 1);
        Debug.Log($"최대값 : {_maxRange} , 랜덤값 : {_randIndex}, 오프셋 : {_indexOffset}");

        // 랜덤으로 대사를 출력함
        Debug.Log(data);
        textToPrint = data[_randIndex, 1];
        yield return StartCoroutine(_textPresenter.StartDialogue());
        
        string[] firstchoices = data[_randIndex, 2].Split("|");
        yield return StartCoroutine(CheckChoicesCount(data, firstchoices, _randIndex));
        
        OnTalkEnd.Invoke();
        yield return new WaitForSeconds(1f);
        _player.isInteracting = false;
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

        // if (data == crankyData)
        // {
        //     _maxRange = _crankyMaxRange;
        // }
        
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
                yield return new WaitForSeconds(0.7f);
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
                    yield break;
                }
                i = int.Parse(ss[0]);
            }

            if (_choice == 1)
            {
                if (ss[1].Trim() == "END")
                {
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
                    yield break;
                }
                i = int.Parse(ss[0]);
            }

            if (_choice == 1)
            {
                if (ss[1].Trim() == "END")
                {
                    yield break;
                }
                i = int.Parse(ss[1]);
            }
            
            if (_choice == 2)
            {
                if (ss[2].Trim() == "END")
                {
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
        AddTarget();
        TalkCamOn();
        NPCLooksPlayer();
        _player.isInteracting = true;
    }
    
    public void ResetInteraction()
    {
        Debug.Log("대화 종료 로직");
        TalkCamOff();
        RemoveTarget();
        _textPresenter.EndDialogue();
        OnTalkStart.RemoveAllListeners();
        OnTalkEnd.RemoveAllListeners();
        isTalking = false;
    }

    public void TalkCamOn()
    {
        _cameras[1].Priority = 11;
    }
    
    public void TalkCamOff()
    {
        _cameras[1].Priority = 9;
    }

    public void AddTarget()
    {   
        Debug.Log(_player.NPC.name);
        _targetGroup.AddMember(_player.NPC.transform, 1, 0);
    }

    public void RemoveTarget()
    {
        _targetGroup.RemoveMember(_player.NPC.transform);
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
    
