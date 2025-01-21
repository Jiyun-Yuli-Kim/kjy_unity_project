using System.Collections;
using Boxophobic.Utils;
using Cinemachine;
using TMPro;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance { get; private set; }
    [SerializeField] private PlayerController _player;
    private NPCController _npc;
    [SerializeField] private DialogueLoader _dialogueLoader;
    [SerializeField] private TextPresenter _presenter;
    
    [SerializeField] DialogueCameraController _dialogueCam;
    // [SerializeField] private CinemachineVirtualCamera[] _cameras;
    // [SerializeField] private CinemachineTargetGroup _targetGroup;
    
    private int _maxRange;
    public string[,] kindData;
    private int _kindMaxRange = 7;
    public string[,] idolData;
    private int _idolMaxRange = 3;
    public string[,] crankyData;
    private int _crankyMaxRange = 1;

    public UnityEvent OnDataLoaded;
    public UnityEvent OnTalkStart; // 대화시작
    public UnityEvent OnTalkEnd; // 대화종료
    
    private int _randIndex;
    private int _indexOffset;

    private bool isTalking;

    public int choice = 0;
    
    private void Start()
    {
        Debug.Log(_player);
        if (_player == null)
        {
            Debug.LogError("Player is null");
        }
        Init();
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

    private string[,] SetData()
    {
        switch (_player.partnerType)
        {
            case Personalities.Kind:
                return kindData;
            case Personalities.Idol:
                return idolData;
            case Personalities.Cranky:
                return crankyData;
            default:
                return null;
        }
    }

    // ITalkable의 Talk 매서드에 의해 발동되는 코루틴
    public IEnumerator TalkToVillager()
    {
        // 캐릭터 성격에 따른 데이터 세팅
        var data = SetData();
        // 데이터 타입에 해당하는 maxRange를 불러온다. 오프셋 설정때문에 필요
        SetMaxRange(data);
        Debug.Log($"data loaded : {data[1,1]}");
        
        // 대화가 진행중이거나 데이터가 없으면 더이상 진행되지 않음
        if (isTalking || data == null)
        {
            yield break;
        }
        
        isTalking = true;
        
        OnTalkStart.AddListener(StartInteraction);
        OnTalkEnd.AddListener(ResetInteraction);
        
        yield return new WaitForSeconds(1f);

        // 대화 시작에 따른 각종 초기화. 줌인 + interacting 
        OnTalkStart.Invoke();
        
        // 카메라 세팅 대기
        yield return new WaitForSeconds(1.5f);
        
        // UI를 활성화하고, 첫 대사를 제시하고, 인풋을 받는다
        yield return StartCoroutine(StartDialogue(data));
        
        // 사용자에게 가능한 선택지를 제시하고 이후 대화 로직을 진행한다.
        string[] firstchoices = data[_randIndex, 2].Split("|");
        yield return StartCoroutine(CheckChoicesCount(data, firstchoices, _randIndex));
        
        OnTalkEnd.Invoke();
        yield return new WaitForSeconds(1f);
        _player.isInteracting = false;
    }
    
    private IEnumerator StartDialogue(string[,] data)
    {
        _presenter.DialogueCanvasOn();

        // maxRange 범위 안에서 첫 대사를 랜덤으로 불러온다.
        _randIndex = Random.Range(1, _maxRange + 1);
        Debug.Log($"최대값 : {_maxRange} , 랜덤값 : {_randIndex}, 오프셋 : {_indexOffset}");
        _presenter.SetDialogueText(data[_randIndex, 1]);
        _presenter.SetNPCName(_player.partnerName);

        yield return new WaitForSeconds(0.7f);
        _presenter.blinker.SetActive(true);

        yield return new WaitUntil(() => _player.input.actions["Trigger"].WasPressedThisFrame());
        _presenter.blinker.SetActive(false);
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
                _presenter.SetDialogueText(data[int.Parse(data[index, 3]) - _indexOffset, 1]);
                yield return StartCoroutine(LoadNextLine());

                string[] nextchoices = data[int.Parse(data[index, 3]) - _indexOffset, 2].Split("|");
                yield return CheckChoicesCount(data, nextchoices, int.Parse(data[index, 3]) - _indexOffset);
            }
        }
        
        else if (choices.Length == 2)
        {
            int i = 0;
            
            yield return StartCoroutine(GetChoice(choices));
            string[] ss = data[index, 3].Split("|");
            
            if(choice == 0)
            {
                if (ss[0].Trim() == "END")
                {
                    yield break;
                }
                i = int.Parse(ss[0]);
            }

            if (choice == 1)
            {
                if (ss[1].Trim() == "END")
                {
                    yield break;
                }
                i = int.Parse(ss[1]);
            }
            _presenter.SetDialogueText(data[i - _indexOffset, 1]);
            yield return StartCoroutine(LoadNextLine());
            
            string[] nextchoices = data[i - _indexOffset, 2].Split("|");
            yield return CheckChoicesCount(data, nextchoices, i - _indexOffset);
        }
        
        else if (choices.Length == 3)
        {
            int i = 0;
            
            yield return StartCoroutine(GetChoice(choices));
            choice = choice;
            string[] ss = data[index, 3].Split("|");
            
            if(choice == 0)
            {
                if (ss[0].Trim() == "END")
                {
                    yield break;
                }
                i = int.Parse(ss[0]);
            }

            if (choice == 1)
            {
                if (ss[1].Trim() == "END")
                {
                    yield break;
                }
                i = int.Parse(ss[1]);
            }
            
            if (choice == 2)
            {
                if (ss[2].Trim() == "END")
                {
                    yield break;
                }
                i = int.Parse(ss[2]);
            }

            _presenter.SetDialogueText(data[i - _indexOffset, 1]);
            yield return StartCoroutine(LoadNextLine());
            
            string[] nextchoices = data[i - _indexOffset, 2].Split("|");
            yield return CheckChoicesCount(data, nextchoices, i - _indexOffset);
        }
    }
    
    public IEnumerator LoadNextLine()
    {
        yield return new WaitForSeconds(0.5f);
        _presenter.blinker.SetActive(true);

        yield return new WaitUntil(() => _player.input.actions["Trigger"].WasPressedThisFrame());
        _presenter.blinker.SetActive(false);
    }
    
    public IEnumerator GetChoice(string[] choices)
    {
        SetChoices(choices);
        yield return null;

        if (choices.Length == 2)
        {
            choice = 0;
            while (!_player.input.actions["Trigger"].WasPressedThisFrame())
            {
                if (choice == 0 && _player.input.actions["South"].WasPressedThisFrame())
                {
                    _presenter.Select2();
                    choice = 1;
                }

                if (choice == 1 && _player.input.actions["North"].WasPressedThisFrame())
                {
                    _presenter.Select1();
                    choice = 0;
                }

                yield return null;
            }
            _presenter.TwoOpsPopupOff();
        }

        if (choices.Length == 3)
        {
            choice = 0;
            while (!_player.input.actions["Trigger"].WasPressedThisFrame())
            {
                if (choice == 0 && _player.input.actions["South"].WasPressedThisFrame())
                {
                    _presenter.SelectB();
                    choice = 1;
                }

                else if (choice == 1 && _player.input.actions["North"].WasPressedThisFrame())
                {
                    _presenter.SelectA();
                    choice = 0;
                }

                else if (choice == 1 && _player.input.actions["South"].WasPressedThisFrame())
                {
                    _presenter.SelectC();
                    choice = 2;
                }

                else if (choice == 2 && _player.input.actions["North"].WasPressedThisFrame())
                {
                    _presenter.SelectB();
                    choice = 1;
                }

                yield return null;
            }
            _presenter.ThreeOpsPopupOff();
        }
    }
    
    public void SetChoices(string[] choices)
    {
        if (choices.Length == 2)
        {
            _presenter.SetChoice1(choices[0]);
            _presenter.SetChoice2(choices[1]);
            _presenter.TwoOpsPopupOn();
        }
        if (choices.Length == 3)
        {
            _presenter.SetChoiceA(choices[0]);
            _presenter.SetChoiceB(choices[1]);
            _presenter.SetChoiceC(choices[2]);
            _presenter.ThreeOpsPopupOn();
        }
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
    
    public void StartInteraction()
    {
        _dialogueCam.AddTarget(_player.NPC.transform);
        _dialogueCam.TalkCamOn();
        NPCLooksPlayer();
        _player.isInteracting = true;
    }
    
    public void ResetInteraction()
    {
        Debug.Log("대화 종료 로직");
        _dialogueCam.TalkCamOff();
        _dialogueCam.RemoveTarget(_player.NPC.transform);
        _presenter.DialogueCanvasOff();
        OnTalkStart.RemoveAllListeners();
        OnTalkEnd.RemoveAllListeners();
        isTalking = false;
    }
    
    public void NPCLooksPlayer()
    {
        _player.NPC.transform.rotation = Quaternion.Lerp(
            _player.NPC.transform.rotation, 
            Quaternion.LookRotation(_player.gameObject.transform.position - _player.NPC.transform.position),
            10*Time.deltaTime);
        // _player.NPC.transform.rotation = Quaternion.LookRotation(_player.gameObject.transform.position - _player.NPC.transform.position);
    }
}
    
