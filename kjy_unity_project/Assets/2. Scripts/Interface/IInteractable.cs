using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IInteractable
{
    public event UnityAction OnInteract;
    public event UnityAction OnInteractEnd;
    public void Interact();
    
    // 플레이어가 상호작용할 대상을 향할 수 있도록 참조할 값
    public Vector3 GetPosition();
}
