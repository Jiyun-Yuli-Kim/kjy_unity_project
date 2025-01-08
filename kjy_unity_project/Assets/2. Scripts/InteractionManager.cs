using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; private set; }

    public UnityEvent OnPickup;
    public UnityEvent OnPickupEnd;
    
    public UnityEvent OnInventoryOpen;
    public UnityEvent OnInventoryClose;
    
    
    void Awake()
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
