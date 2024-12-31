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
    
    private bool _isTriggered = false;

    private void Update()
    {
        CheckInteraction();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _isTriggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _isTriggered = false;
        }
    }

    private void CheckInteraction()
    {
        if (_isTriggered == false)
        {
            return;
        }

        if (_input.actions["Trigger"].WasPressedThisFrame())
        {
            Debug.Log("나무 흔들기");
            _fruit1.FruitFall();
            _fruit2.FruitFall();
            _fruit3.FruitFall();
            
            // 나무 흔드는 애니메이션
            // 과일 떨어짐
        }
    }
}
