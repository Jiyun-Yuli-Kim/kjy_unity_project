using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FruitTree : MonoBehaviour
{
    [SerializeField] private Fruit _fruit1;
    [SerializeField] private Fruit _fruit2;
    [SerializeField] private Fruit _fruit3;
    
    [SerializeField] private Collider _collider;
    [SerializeField] private Collider _trigger;

    [SerializeField] private PlayerInput _input;
    [SerializeField] private float _fallTime =2f;
    
    private Animator _animator;
    private PlayerController _player;
    private StateMachine _stateMachine;
    
    private bool _isTriggered = false;
    private bool _isFalling = false;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        StartCoroutine(CheckInteraction());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _isTriggered = true;
            _player = other.gameObject.GetComponent<PlayerController>();
            _stateMachine = _player.GetComponent<StateMachine>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _isTriggered = false;
            _player = null;
            _stateMachine = null;
        }
    }

    private IEnumerator CheckInteraction()
    {
        if (_isTriggered == false || _isFalling == true)
        {
            yield break;
        }

        if (_input.actions["Trigger"].WasPressedThisFrame())
        {
            _isFalling = true;
            _animator.SetBool("isShaking", true);
            _stateMachine.OnChangeState(StateMachine.StateType.PShake);
            yield return new WaitForSeconds(0.3f);
            _fruit1.FruitFall();
            _fruit2.FruitFall();
            _fruit3.FruitFall();
            yield return new WaitForSeconds(_fallTime);
            _animator.SetBool("isShaking", false);
            _stateMachine.OnChangeState(StateMachine.StateType.PIdle);
            _isFalling = false;
        }
    }
}
