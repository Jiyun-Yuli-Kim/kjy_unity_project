using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.InputSystem;

// 플레이어의 기본적인 이동을 관리합니다.
// 키보드, 게임패드 모두 대응하도록 구현
public class PlayerController : MonoBehaviour
{
    [SerializeField] public PlayerData _playerData;

    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Animator _animator;
    [SerializeField] private DialogueSystem _dialogueSystem;
    
    private StateMachine _stateMachine;
    public PlayerInput Input;
    
    private IInteractable _interactable;
    private Item _item;

    [SerializeField] public float playerMoveSpeed;
    [field: SerializeField] public float rotateInterpolation { get; private set; }
    [field: SerializeField] public float dashMultiplier { get; private set; }
    [field: SerializeField] public float angularDrag { get; private set; } = 6f;

    public bool isMoving = false;
    public bool isDashing = false;
    public bool invenOpened = false;
    
    // 로직 실행중 다른 입력을 받지 않기 위한 플래그 변수
    public bool isInteracting = false;

    public NPCController NPC;
    public string partnerName;
    public string partnerCp; // 상대방 말버릇

    public void Awake()
    {
        _stateMachine = GetComponent<StateMachine>();
        Input = GetComponent<PlayerInput>();
    }

    public void Start()
    {
        InteractionManager.Instance.OnShakeTree.AddListener(ShakeTree);
        InteractionManager.Instance.OnShakeTreeEnd.AddListener(StopShakeTree);
        InteractionManager.Instance.OnPickup.AddListener(Pickup);
        InteractionManager.Instance.OnPickupEnd.AddListener(StopPickup);
    }

    public void Update()
    {
        CheckInventory();
        
        if (isInteracting)
        {
            return;
        }
        
        GetInputBool();
    }

    public void LateUpdate()
    {
        ResetInputBool();
        
        if (isInteracting)
        {
            return;
        }
        
        CheckInteraction();

        // _rb.angularDrag = 3;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_interactable == null)
        {
            _interactable = other.gameObject.GetComponent<IInteractable>();
        }

        if (other.gameObject.tag == "Item")
        {
            if (_item == null)
            {
                _item = other.gameObject.GetComponent<Item>();
            }
        }

        if (other.gameObject.tag == "Villager")
        {
            NPC = other.GetComponent<NPCController>();
            if (NPC != null)
            {
                partnerName = NPC._npcData.NPCName;
                partnerCp = NPC._npcData.CatchPhrase;
                Debug.Log($"{partnerName}, {partnerCp}");
            }
            return;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        _interactable = null;
        _item = null;
        
        if (other.gameObject.tag == "Villager")
        {
            NPC = null;
            partnerName = null;
        }
        
        if (other.gameObject.tag == "Tree")
        {
            isInteracting = false;
        }
    }

    public void GetInputBool()
    {
        if (Input.actions["Move"].IsPressed()) // WASD, 십자방향키, 좌측 조이스틱
        {
            isMoving = true;
        }

        if (Input.actions["Dash"].IsPressed())
        {
            isDashing = true;
        }
    }

    public void ResetInputBool()
    {
        if (Input.actions["Dash"].WasReleasedThisFrame())
        {
            isDashing = false;
        }

        if (Input.actions["Move"].WasReleasedThisFrame())
        {
            isMoving = false;
            isDashing = false;
        }
    }

    public void PlayerMove()
    {
        Vector3 dir;

        Vector2 move = Input.actions["Move"].ReadValue<Vector2>();

        dir = new Vector3(move.x, 0, move.y);
        // Debug.Log(dir);
        _rb.velocity = dir * playerMoveSpeed;

        if (dir != Vector3.zero)
        {
            _playerTransform.rotation = Quaternion.Lerp(
                _playerTransform.rotation,
                Quaternion.LookRotation(dir),
                rotateInterpolation * Time.deltaTime
            );
        }
        //
        // else
        // {
        //     _rb.angularDrag = angularDrag;
        // }
    }

    public void CheckInventory()
    {
        if (Input.actions["Inventory"].WasPressedThisFrame())
        {
            isInteracting = true;
            if (invenOpened)
            {
                return;
            }
            invenOpened = true;
            InteractionManager.Instance.OnInventoryOpen.Invoke();
            Debug.Log($"인벤토리 오픈");
        }

        if (invenOpened && Input.actions["Revert"].WasReleasedThisFrame())
        {
            invenOpened = false;
            InteractionManager.Instance.OnInventoryClose.Invoke();
            isInteracting = false;
        }
    }

    public void CheckInteraction()
    {
        // 겹치는거 어떻게 처리할지 고민해야함.
        if (isInteracting)
        {
            return;
        }

        if (_interactable != null && Input.actions["Trigger"].WasPressedThisFrame())
        {
            isInteracting = true;
            _interactable.Interact();
        }

        // 일단 인풋을 다르게 받을거라 괜찮을 것 같긴 하지만... 
        if (_item != null && Input.actions["Revert"].WasPressedThisFrame())
        {
            isInteracting = true;
            _animator.SetTrigger("PickupTrigger");
            _item.BeingPickedUp();
        }
    }

    public void ShakeTree()
    {
        Debug.Log("플레이어 나무 흔들기");
        // transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.position - _interactable.GetPosition()), 2*Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(_interactable.GetPosition());
        _stateMachine.OnChangeState(StateMachine.StateType.PShake);
    }

    public void StopShakeTree()
    {
        _stateMachine.OnChangeState(StateMachine.StateType.PIdle);
        isInteracting = false;
    }

    public void Pickup()
    {
        _animator.SetTrigger("PickupTrigger");
        // _stateMachine.OnChangeState(StateMachine.StateType.PPickup);
    }

    public void StopPickup()
    {
        isInteracting = false;
        _item = null;
        // _stateMachine.OnChangeState(StateMachine.StateType.PIdle);
    }

}