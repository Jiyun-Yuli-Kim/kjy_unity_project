using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private float _playerMoveSpeed;
    // [field: SerializeField] public float _playerCurSpeed { get; private set; }
    [Range(1,20)]
    [SerializeField] private float _rotateInterpolation;
    [Range(0,2)]
    [SerializeField] private float _dashMultiplier;

    private bool _isMoving = false;
    private bool _isDashing = false;
    
    private void FixedUpdate()
    { 
        float tempSpeed = _playerMoveSpeed;
        Vector3 dir;

        if (input.actions["Dash"].IsPressed())
        {
            _isDashing = true;
            _playerMoveSpeed *= _dashMultiplier;
        }
        
        Vector2 move = input.actions["Move"].ReadValue<Vector2>();
        if (move != Vector2.zero)
        {
            _isMoving = true;
        }

        dir = new Vector3(move.x, 0, move.y);
        
        // rb.AddForce(dir*_playerMoveSpeed, ForceMode.Force);
        
        rb.velocity = dir * _playerMoveSpeed;
        
        if (dir != Vector3.zero)
        {
            _playerTransform.rotation = Quaternion.Lerp(
                _playerTransform.rotation,
                Quaternion.LookRotation(dir),
                _rotateInterpolation * Time.deltaTime
            );
        }

        if (input.actions["Dash"].WasReleasedThisFrame())
        {
            _isDashing = false;
        }

        if (input.actions["Move"].WasReleasedThisFrame())
        {
            _isMoving = false;
        }
        
        Debug.Log($"move? : {_isMoving}, dash? : {_isDashing}");
        
        // _playerCurSpeed = dir.magnitude*_playerMoveSpeed;
        _playerMoveSpeed = tempSpeed;
    }
}
