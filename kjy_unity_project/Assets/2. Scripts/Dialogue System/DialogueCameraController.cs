using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DialogueCameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera[] _cameras;
    [SerializeField] private CinemachineTargetGroup _targetGroup;
    
    [SerializeField] private float distanceFromCenter = 3f; // 타겟 그룹 중심에서의 거리
    [SerializeField] private float heightOffset = 1f; // 카메라의 높이 오프셋
    
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
        Vector3 groupCenter = _targetGroup.transform.position;
        Vector3 rotation = Quaternion.Euler(0, 30, 0) * _targetGroup.m_Targets[0].target.transform.right;
        Vector3 rightOffset =  rotation * distanceFromCenter;

        Vector3 cameraPosition = groupCenter + rightOffset + Vector3.up * heightOffset;
        _cameras[1].transform.position = cameraPosition;
        _cameras[1].transform.LookAt(groupCenter);
        
        _cameras[1].Priority = 11;
    }
    
    public void TalkCamOff()
    {
        _cameras[1].Priority = 9;
    }
}
