using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class FruitTree : MonoBehaviour, IInteractable
{
    // 과일을 나무에서 떨어뜨리는 기능 담당
    // 규칙에 따라 과일을 3개씩 스폰함
    // 플레이어로부터 인풋을 받았을 때 흔들리는 애니메이션 재생
    
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
    
    // public bool isInteracting = false;
    
    void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (fruits[0] == null && fruits[1] == null && fruits[2] == null)
        {
            SpawnFruit();
        }
    }

    public void Interact()
    {
        // if (isInteracting)
        // {
        //     return;
        // }
        //
        // isInteracting = true;
        InteractionManager.Instance.OnShakeTree.Invoke();
        Debug.Log("나무에 대한 Interact 로직 작동");
        StartCoroutine(ShakeAndDrop());
    }

    public Vector3 GetPosition()
    {
        return transform.position;
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
        
        InteractionManager.Instance.OnShakeTreeEnd.Invoke();
        ClearArray();
    }

    private void SpawnFruit()
    {

            fruits[0] = Instantiate(_fruitPrefab, _fruit1Pos.position,
                new Quaternion(-0.107541613f, 0, 0, 0.994200587f)).GetComponent<Fruit>();
            fruits[0].isGrounded = false;
            fruits[1] = Instantiate(_fruitPrefab, _fruit2Pos.position,
                new Quaternion(-0.0911203697f, 0.19207485f, -0.0179143753f, 0.976976693f)).GetComponent<Fruit>();
            fruits[1].isGrounded = false;
            fruits[2] = Instantiate(_fruitPrefab, _fruit3Pos.position,
                new Quaternion(-0.0839739516f, -0.19348672f, 0.0112915235f, 0.977437377f)).GetComponent<Fruit>();
            fruits[2].isGrounded = false;
            Debug.Log($"{fruits[0].isGrounded}, {fruits[1].isGrounded}, {fruits[2].isGrounded}");
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
