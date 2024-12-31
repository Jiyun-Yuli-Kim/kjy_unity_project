using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    private Rigidbody _rb;
    private Collider _col;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Ground" && _rb.velocity.magnitude < 0.1f)
        {
            _rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    public void FruitFall()
    {
        _rb.constraints = RigidbodyConstraints.None;
    }

}
