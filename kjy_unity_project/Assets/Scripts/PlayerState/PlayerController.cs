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
   
    [SerializeField] private PlayerInput input;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Animator _animator;
    
    [SerializeField] public float _playerMoveSpeed;
    [field : SerializeField] public float _rotateInterpolation{get; private set;} 
    [field : SerializeField] public float _dashMultiplier{get; private set;}
    [field: SerializeField] public float _angularDrag { get; private set; } = 3f;
    
    public bool _isMoving = false;
    public bool _isDashing = false;
    

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
        if (input.actions["Move"].IsPressed())
        {
            _isMoving = true;
        }

        if (input.actions["Dash"].IsPressed())
        {
            _isDashing = true;
        }
    }

    public void ResetInputBool()
    {
        if (input.actions["Dash"].WasReleasedThisFrame())
        {
            _isDashing = false;
        }
        
        if (input.actions["Move"].WasReleasedThisFrame())
        {
            _isMoving = false;
            _isDashing = false;
        }
    }
    
     public void PlayerMove()
     { 
         Vector3 dir;
    
         Vector2 move = input.actions["Move"].ReadValue<Vector2>();
         
         dir = new Vector3(move.x, 0, move.y);
         Debug.Log(dir);
         rb.velocity = dir * _playerMoveSpeed;
         
         if (dir != Vector3.zero)
         {
             _playerTransform.rotation = Quaternion.Lerp(
                 _playerTransform.rotation,
                 Quaternion.LookRotation(dir),
                 _rotateInterpolation * Time.deltaTime
             );
         }

         // else
         // {
         //     rb.angularDrag = _angularDrag;
         // }
    }

}
