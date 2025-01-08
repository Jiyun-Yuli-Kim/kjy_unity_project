using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Rigidbody _rb;
    public Collider _col;
    public ItemData data;
    public bool isGrounded;
    
    void Start()
    {
        isGrounded = false;
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }
    
    public virtual void BeingPickedUp()
    {
        if (!isGrounded)
        {
            Debug.Log("아이템이 바닥에 있지 않아서 주울 수 없음");
            // InteractionManager.Instance.OnPickupEnd.Invoke();
            return;
        }
        // InteractionManager.Instance.OnPickup.Invoke();
        PickupItem();
        Debug.Log("픽업 이벤트 발동");
    }

    public void PickupItem()
    {
        StartCoroutine(PickupCoroutine());
    }

    public IEnumerator PickupCoroutine()
    {
        // 플레이어 애니메이션 동작시간에 맞춰 대기
        yield return new WaitForSeconds(0.8f);
        
        Inventory.Instance.AddItem(this);
        
        Destroy(this.gameObject);
        
        // InteractionManager.Instance.OnPickupEnd.Invoke();
        //
        // InteractionManager.Instance.OnPickup.RemoveListener(PickupItem);
        // InteractionManager.Instance.OnPickupEnd.RemoveListener(EndPickupItem);
        //
        Debug.Log("픽업종료");
    }

    public void EndPickupItem()
    {
        isGrounded = false;
    }
}
