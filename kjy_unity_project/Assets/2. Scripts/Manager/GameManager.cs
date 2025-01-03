using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public SceneChanger SceneChange { get; private set; }

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
    }

    private void Init()
    {
        SetSingleton();
        SceneChange = GetComponent<SceneChanger>();
    }

    private void SetSingleton()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            Instance = this;
        }
        
        else if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }
}