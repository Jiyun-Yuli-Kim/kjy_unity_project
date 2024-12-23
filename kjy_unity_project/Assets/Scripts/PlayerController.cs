using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// 플레이어의 기본적인 이동과 그에따른 애니메이션을 관리합니다.
// 일단 키보드로 구현
public class PlayerController : MonoBehaviour
{
   
    [SerializeField] private PlayerInput input;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform _playerTransform;
    
    [Range(1,20)]
    [SerializeField] private float _playerMoveSpeed;
    [SerializeField] private float _rotateInterpolation;
    [SerializeField] private float _dashMultiplier;
    
    private void Update()
    { 
        float tempSpeed = _playerMoveSpeed;

        if (input.actions["Dash"].IsPressed())
        {
            _playerMoveSpeed *= _dashMultiplier;
        }
        Vector2 move = input.actions["Move"].ReadValue<Vector2>();
        Vector3 dir = new Vector3(move.x, 0, move.y);
        rb.AddForce(dir*_playerMoveSpeed, ForceMode.Force);
        if (dir != Vector3.zero)
        {
            _playerTransform.rotation = Quaternion.Lerp(
                _playerTransform.rotation,
                Quaternion.LookRotation(dir),
                _rotateInterpolation * Time.deltaTime
            );
        }

        _playerMoveSpeed = tempSpeed;
    }
}
