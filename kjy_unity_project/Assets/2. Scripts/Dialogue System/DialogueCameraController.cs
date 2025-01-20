using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DialogueCameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera[] _cameras;
    [SerializeField] private CinemachineTargetGroup _targetGroup;
    
    public void AddTarget(Transform target)
    {   
        _targetGroup.AddMember(target, 1, 0.5f);
    }

    public void RemoveTarget(Transform target)
    {
        _targetGroup.RemoveMember(target);
    }
    
    public void TalkCamOn()
    {
        _cameras[1].Priority = 11;
    }
    
    public void TalkCamOff()
    {
        _cameras[1].Priority = 9;
    }
}
