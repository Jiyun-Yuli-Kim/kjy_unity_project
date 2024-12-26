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
    public bool isInteracting = false;


    public void Update()
    {
        GetInputBool();
    }

    public void LateUpdate()
    {
        ResetInputBool();
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
        
        if (_input.actions["Interact"].WasPressedThisFrame())
        {
            isInteracting = true;
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

         // else
         // {
         //     rb.angularDrag = _angularDrag;
         // }
    }

}
