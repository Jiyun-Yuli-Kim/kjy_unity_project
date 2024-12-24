using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerController _playerController;
    void Update()
    {
        _animator.SetFloat("Speed", _playerController._playerCurSpeed);
        Debug.Log(_playerController._playerCurSpeed);
    }
}
