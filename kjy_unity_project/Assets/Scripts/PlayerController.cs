using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.InputSystem;

// 플레이어의 기본적인 이동을 관리합니다.
// 키보드, 게임패드 모두 대응하도록 구현
public class PlayerController : MonoBehaviour
{
    [SerializeField] public PlayerData _playerData;  
   
    [SerializeField] private PlayerInput _input;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Animator _animator;
    [SerializeField] private DialogueSystem _dialogueSystem;
     
    [SerializeField] public float playerMoveSpeed;
    [field : SerializeField] public float rotateInterpolation{get; private set;} 
    [field : SerializeField] public float dashMultiplier{get; private set;}
    [field: SerializeField] public float angularDrag { get; private set; } = 6f;
    
    public bool isMoving = false;
    public bool isDashing = false;
    public bool isTriggered = false;
    public bool isReverted = false;


    // 플레이어가 트리거 범위 내에 있는지만을 확인하기 위한 변수
    public bool _metKind { get; private set; } = false;
    public bool _metIdol { get; private set; } = false;
    
    // 로직 실행중 다른 입력을 받지 않기 위한 플래그 변수
    public bool _isInteracting = false;

    public string partnerName;
    public string partnerCp; // 상대방 말버릇
    
    private void Awake()
    {

    }
    

    public void Update()
    {
        if (_isInteracting)
        {
            return;
        }

        GetInputBool();
        DialogueCheck();
    }

    public void LateUpdate()
    {
        ResetInputBool();
        _rb.angularDrag = 3;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Kind")
        {
            _metKind = true;
            var npcComp = other.GetComponent<NPCController>();
            if (npcComp != null)
            {
                partnerName = npcComp._npcData.NPCName;
                partnerCp = npcComp._npcData.CatchPhrase;
                Debug.Log($"{partnerName}, {partnerCp}");
            }
            return;
        }
        
        if (other.gameObject.tag == "Idol")
        {
            _metIdol = true;
            var npcComp = other.GetComponent<NPCController>();
            if (npcComp != null)
            {
                partnerName = npcComp._npcData.NPCName;
                Debug.Log(partnerName);
            }
            return;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Kind")
        {
            _metKind = false;
            partnerName = null;
        }
        
        if (other.gameObject.tag == "Idol")
        {
            _metIdol = false;
            partnerName = null;
        }
    }
    
    public void GetInputBool()
    {
        if (_isInteracting)
        {
            return;
        }

        if (_input.actions["Move"].IsPressed()) // WASD, 십자방향키, 좌측 조이스틱
        {
            isMoving = true;
        }

        if (_input.actions["Dash"].IsPressed())
        {
            isDashing = true;
        }
        
        if (_input.actions["Trigger"].WasPressedThisFrame())
        {
            isTriggered = true;
        }
        
        if (_input.actions["Revert"].WasPressedThisFrame())
        {
            isReverted = true;
            Debug.Log("B버튼 눌림");
        }
        
    }

    public void ResetInputBool()
    {
        if (_input.actions["Dash"].WasReleasedThisFrame())
        {
            isDashing = false;
        }
        
        if (_input.actions["Move"].WasReleasedThisFrame())
        {
            isMoving = false;
            isDashing = false;
        }
        
        isTriggered = false;
        isReverted = false;
    }

    public bool GetInputAB()
    {
        if (_input.actions["Trigger"].IsPressed() || _input.actions["Revert"].IsPressed())
        {
            return true;
        }
        return false;
    }
    public void PlayerMove()
     { 
         Vector3 dir;
    
         Vector2 move = _input.actions["Move"].ReadValue<Vector2>();
         
         dir = new Vector3(move.x, 0, move.y);
         // Debug.Log(dir);
         _rb.velocity = dir * playerMoveSpeed;
         
         if (dir != Vector3.zero)
         {
             _playerTransform.rotation = Quaternion.Lerp(
                 _playerTransform.rotation,
                 Quaternion.LookRotation(dir),
                 rotateInterpolation * Time.deltaTime
             );
         }
         
         // 폴리싱 
         else
         {
             _rb.angularDrag = angularDrag;
         }
    }

     private void DialogueCheck()
     {
         if (_metKind && isTriggered)
         {
             _isInteracting = true;
             //_dialogueSystem.대화시행코루틴
             StartCoroutine(_dialogueSystem.TalkToKindVillager());
         }
     
         if (_metIdol && isTriggered)
         {
             _isInteracting = true;
             Debug.Log("아이돌 유형 주민과 대화");
             _isInteracting = false;
         }
     }
}