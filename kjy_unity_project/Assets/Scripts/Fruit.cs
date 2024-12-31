using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Fruit : MonoBehaviour
{
    private Rigidbody _rb;
    private Collider _col;
    
    private PlayerInput _input;
    
    private bool _isPickupable = false;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
    }

    void Update()
    {
        CheckPickupable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // _isPickupable = true 주울 수 있는 상태가 된다
            // _isPickupable == true이고 Trigger 눌렸을 때(업데이트에서 처리)
            // PickUpFruit()
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Ground" && _rb.velocity.magnitude < 0.1f)
        {
            _rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    private void CheckPickupable()
    {
        if (!_isPickupable)
        {
            return;
        }
        
        else if (_isPickupable)
        {
            if (_input.actions["Trigger"].WasPressedThisFrame())
            {
                PickUpFruit();
            }
        }
    }

    public void FruitFall()
    {
        _rb.constraints = RigidbodyConstraints.None;
    }

    public void PickUpFruit()
    {
        // 직접적으로 과일을 줍는 로직
    }

}
