using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using Unity.VisualScripting;


public class TestCamChanger : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    [SerializeField] private CinemachineVirtualCamera[] cameras;

    private void OnEnable()
    {
        buttons[0].onClick.AddListener(() => SetActiveCam(0));
        buttons[1].onClick.AddListener(() => SetActiveCam(1));
        buttons[2].onClick.AddListener(() => SetActiveCam(2));
        buttons[3].onClick.AddListener(() => SetActiveCam(3));
        buttons[4].onClick.AddListener(() => SetActiveCam(4));
    }

    public void SetActiveCam(int index)
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            if (i == index)
            {
                cameras[i].Priority = 11;
            }

            else
            {
                cameras[i].Priority = 10;
            }
        }

    }

    private void OnDisable()
    {
        buttons[0].onClick.RemoveListener(() => SetActiveCam(0));
        buttons[1].onClick.RemoveListener(() => SetActiveCam(1));
        buttons[2].onClick.RemoveListener(() => SetActiveCam(2));
        buttons[3].onClick.RemoveListener(() => SetActiveCam(3));
        buttons[4].onClick.RemoveListener(() => SetActiveCam(4));
    }
}
