using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class FruitTree : MonoBehaviour, IInteractable
{
    [SerializeField] private Fruit _fruit1;
    [SerializeField] private Fruit _fruit2;
    [SerializeField] private Fruit _fruit3;
    [SerializeField] private GameObject _fruitPrefab;
    [SerializeField] private GameObject _parent;
    
    [SerializeField] private Transform _fruit1Pos;
    [SerializeField] private Transform _fruit2Pos;
    [SerializeField] private Transform _fruit3Pos;
    
    [SerializeField] private Collider _collider;
    [SerializeField] private Collider _trigger;

    [SerializeField] private PlayerInput _input;
    [SerializeField] private float _fallTime =2f;
    
    private Animator _animator;
    private PlayerController _player;
    private StateMachine _stateMachine;
    
    public bool isInteracting = false;

    public UnityEvent OnInteraction;

    void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
        _animator = GetComponent<Animator>();
    }
    
    public void Interact()
    {
        if (isInteracting)
        {
            return;
        }

        isInteracting = true;
        _player.isShakingTree = true;
        InteractionMediator.Instance.OnShakeTree.Invoke();
        Debug.Log("나무에 대한 Interact 로직 작동");
        StartCoroutine(ShakeAndDrop());
    }

    private IEnumerator ShakeAndDrop()
    {

        _animator.SetBool("isShaking", true);
        yield return new WaitForSeconds(0.3f);
        
        Fruit[] fruits = _parent.GetComponentsInChildren<Fruit>();
        foreach (Fruit fruit in fruits)
        {
            fruit.FruitFall();
        }
        yield return new WaitForSeconds(_fallTime);
        
        _animator.SetBool("isShaking", false);
        
        InteractionMediator.Instance.OnShakeTreeEnd.Invoke();
        _player.isShakingTree = false;
        isInteracting = false;
    }

}
