using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// 플레이어의 기본적인 이동을 관리합니다.
// 일단 키보드로 구현
public class PlayerController : MonoBehaviour
{
   
    [SerializeField] private PlayerInput input;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform _playerTransform;

    [SerializeField] private float _playerMoveSpeed;
    [field: SerializeField] public float _playerCurSpeed { get; private set; }


    [Range(1,20)]
    [SerializeField] private float _rotateInterpolation;
    [Range(0,2)]
    [SerializeField] private float _dashMultiplier;
    
    private void FixedUpdate()
    { 
        float tempSpeed = _playerMoveSpeed;
        Vector3 dir;

        if (input.actions["Dash"].IsPressed())
        {
            _playerMoveSpeed *= _dashMultiplier;
        }
        Vector2 move = input.actions["Move"].ReadValue<Vector2>();
        dir = new Vector3(move.x, 0, move.y);
        Debug.Log(dir);
        // rb.AddForce(dir*_playerMoveSpeed, ForceMode.Force);
        rb.velocity = dir * _playerCurSpeed;
        if (dir != Vector3.zero)
        {
            _playerTransform.rotation = Quaternion.Lerp(
                _playerTransform.rotation,
                Quaternion.LookRotation(dir),
                _rotateInterpolation * Time.deltaTime
            );
        }

        _playerCurSpeed = dir.magnitude*_playerMoveSpeed;
        _playerMoveSpeed = tempSpeed;
    }
}
