using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class FruitTree : MonoBehaviour, IInteractable
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

    public UnityEvent OnInteraction;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // StartCoroutine(CheckInteraction());
    }
    
    public void Interact()
    {
        if (_isTriggered == false || _isFalling == true)
        {
            // yield break;
        }

        if (_input.actions["Trigger"].WasPressedThisFrame())
        {
            _isFalling = true;
            _stateMachine.OnChangeState(StateMachine.StateType.PShake);
            _animator.SetBool("isShaking", true);
            // yield return new WaitForSeconds(0.3f);
            
            _fruit1.FruitFall();
            _fruit2.FruitFall();
            _fruit3.FruitFall();
            // yield return new WaitForSeconds(_fallTime);
            
            _animator.SetBool("isShaking", false);
            _stateMachine.OnChangeState(StateMachine.StateType.PIdle);
            _isFalling = false;
        }
    }
}
