using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// 플레이어의 기본적인 이동을 관리합니다.
// 키보드, 게임패드 모두 대응하도록 구현
public class PlayerController : MonoBehaviour
{
   
    [SerializeField] private PlayerInput _input;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Animator _animator;
    
    [SerializeField] public float playerMoveSpeed;
    [field : SerializeField] public float rotateInterpolation{get; private set;} 
    [field : SerializeField] public float dashMultiplier{get; private set;}
    [field: SerializeField] public float angularDrag { get; private set; } = 3f;
    
    public bool isMoving = false;
    public bool isDashing = false;
    public bool isTriggered = false;

    // 플레이어가 트리거 범위 내에 있는지만을 확인하기 위한 변수
    private bool _metKind = false;
    private bool _metIdol = false;
    
    // 로직 실행중 다른 입력을 받지 않기 위한 플래그 변수
    private bool _isInteracting = false;
    
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
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Kind")
        {
            _metKind = true;
        }
        
        if (other.gameObject.tag == "Idol")
        {
            _metIdol = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Kind")
        {
            _metKind = false;
        }
        
        if (other.gameObject.tag == "Idol")
        {
            _metIdol = false;
        }
    }
    
    public void GetInputBool()
    {
        if (_input.actions["Move"].IsPressed())
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
         
         // 폴리싱) 
         // else
         // {
         //     rb.angularDrag = _angularDrag;
         // }
    }

     private void DialogueCheck()
     {
         if (_metKind && isTriggered)
         {
             _isInteracting = true;
             Debug.Log("친절함 유형 주민과 대화");
             WaitForSeconds(3f);
             _isInteracting = false;
         }

         if (_metIdol && isTriggered)
         {
             _isInteracting = true;
             Debug.Log("아이돌 유형 주민과 대화");
             WaitForSeconds(3f);
             _isInteracting = false;
         }
     }
     
     
    #region 디버그전용     
     private bool isWaiting = false;

     public void WaitForSeconds(float seconds)
     {
         isWaiting = true;
         float startTime = Time.time;

         while (Time.time < startTime + seconds)
         {
             // 대기 중
         }

         isWaiting = false;
         Debug.Log("대기 완료");
     }
     #endregion
}
