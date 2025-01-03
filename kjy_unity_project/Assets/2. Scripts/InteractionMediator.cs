using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionMediator : MonoBehaviour
{
    public static InteractionMediator Instance { get; private set; }
    
    public UnityEvent OnShakeTree;
    public UnityEvent OnShakeTreeEnd;
    public UnityEvent OnDropFruits;
    
    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            Instance = this;
        }

        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

}
