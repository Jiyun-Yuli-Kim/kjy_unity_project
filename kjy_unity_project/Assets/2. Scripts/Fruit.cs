using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Fruit : MonoBehaviour, IPickupable
{
    private Rigidbody _rb;
    private Collider _col;

    private PlayerInput _input;

    private bool _isPickupable = false;
    private bool _isBeingPickedup = false;
    private bool _isGrounded = false;

    [SerializeField] private ItemData _fruitData;
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
    }

    void Update()
    {
        // CheckPickupable();
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         _isPickupable = true;
    //         // _isPickupable == true이고 Trigger 눌렸을 때(업데이트에서 처리)
    //         // PickUpFruit()
    //     }
    // }
    //
    // private void OnTriggerStay(Collider other)
    // {
    //     if (_isBeingPickedup)
    //     {
    //         return;
    //     }
    //     
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         if (_isPickupable && _input.actions["Trigger"].WasPressedThisFrame())
    //         {
    //             StartCoroutine(PickUpFruit());
    //         }
    //     }
    // }
    //
    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         _isPickupable = false;
    //     }
    // }
    //
    // private void OnCollisionStay(Collision other)
    // {
    //     if (other.gameObject.CompareTag("Ground"))
    //     {
    //         Debug.Log("Ground");
    //         if (_isGrounded)
    //         {
    //             return;
    //         }
    //
    //         _isGrounded = true;
    //         _rb.constraints = RigidbodyConstraints.FreezeAll;
    //     }
    // }

    // private void OnCollisionExit(Collision other)
    // {
    //     if (other.gameObject.tag == "Ground")
    //     {
    //         _rb.constraints = RigidbodyConstraints.None;
    //         _rb.mass = 10;
    //         _isGrounded = false;
    //     }
    // }

    // private void CheckPickupable()
    // {
    //     if (!_isPickupable)
    //     {
    //         return;
    //     }
    //     
    //     else if (_isPickupable)
    //     {
    //         if (_input.actions["Trigger"].WasPressedThisFrame())
    //         {
    //             // StartCoroutine(PickUpFruit());
    //         }
    //     }
    // }
    public void FruitFall()
    {
        _rb.constraints = RigidbodyConstraints.None;
    }
    
    public void BeingPickedUp()
    {
        StartCoroutine(PickUpFruit());
    }

    public IEnumerator PickUpFruit()
    {
        _isBeingPickedup = true;
        // 플레이어 : 줍기 애니메이션 실행 완료까지 대기
        // OnPickupEnd 발동시
        // Destroy
        // WaitUntil 줍기완료
        // 인벤토리 : Inventory.Instance.AddItem(주운거)
        yield return new WaitForSeconds(0.5f);
        _isBeingPickedup = false;
    }
}


