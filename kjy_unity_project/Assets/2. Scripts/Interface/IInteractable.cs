using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void Interact();
    
    // 플레이어가 상호작용할 대상을 향할 수 있도록 참조할 값
    public Vector3 GetPosition();
}
