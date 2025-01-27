using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Fruit : Item
{
    // private Rigidbody _rb;
    // private Collider _col;
    // private bool _isBeingPickedup = false;

    // 데이터 필드는 어차피 부모클래스인 Item에서 정의했으므로..
    // [SerializeField] private ItemData _fruitData;

    // void Start()
    // {
    //     isGrounded = false;
    //     _rb = GetComponent<Rigidbody>();
    //     _col = GetComponent<Collider>();
    // }

    // 초기 과일이 스폰된 상태에는 Constraints가 모두 설정되어있음
    // FruitTree상에서 이벤트를 통해 FruitFall을 호출하고있다. 이때 constraints가 일시 해제된다.
    // 그럼이제 여기서는 땅에 닿았는지를 판정해서 다시 freeze해주면 될 것 같다.
    // 다만 이제 여기서 반동을 줄거라면? 조금 복잡해지겠죠
    // 이벤트에 등록해서 한번 처리해볼게.... 가 아니다 이거 너무 복잡해진다.

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ground")
        {
            FreezeFruit();
        }
    }

    public void FruitFall()
    {
        _rb.constraints = RigidbodyConstraints.None;
    }

    public void FreezeFruit()
    {
        _rb.constraints = RigidbodyConstraints.FreezeAll;
    }
}


