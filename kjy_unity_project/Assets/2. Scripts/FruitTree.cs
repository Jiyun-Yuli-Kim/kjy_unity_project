using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class FruitTree : MonoBehaviour, IInteractable
{
    // [SerializeField] private Fruit _fruit1;
    // [SerializeField] private Fruit _fruit2;
    // [SerializeField] private Fruit _fruit3;
    private Fruit[] fruits = new Fruit[3];
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

    void Update()
    {
        SpawnFruit();
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
        
        foreach (Fruit fruit in fruits)
        {
            fruit.FruitFall();
        }
        yield return new WaitForSeconds(_fallTime);
        
        _animator.SetBool("isShaking", false);
        
        InteractionMediator.Instance.OnShakeTreeEnd.Invoke();
        _player.isShakingTree = false;
        isInteracting = false;
        ClearArray();

    }

    private void SpawnFruit()
    {
        if (fruits[0] == null && fruits[1] == null && fruits[2] == null)
        {
            fruits[0] = Instantiate(_fruitPrefab, _fruit1Pos.position,
                new Quaternion(-0.107541613f, 0, 0, 0.994200587f)).GetComponent<Fruit>();
            fruits[1] = Instantiate(_fruitPrefab, _fruit2Pos.position,
                new Quaternion(-0.0911203697f, 0.19207485f, -0.0179143753f, 0.976976693f)).GetComponent<Fruit>();
            fruits[2] = Instantiate(_fruitPrefab, _fruit3Pos.position,
                new Quaternion(-0.0839739516f, -0.19348672f, 0.0112915235f, 0.977437377f)).GetComponent<Fruit>();
        }
    }

    private void ClearArray()
    {
        if (fruits != null)
        {
            for (int i = 0; i < fruits.Length; i++)
            {
                fruits[i] = null;
            }
        }
    }

}
