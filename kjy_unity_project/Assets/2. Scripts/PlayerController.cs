using UnityEngine;
using UnityEngine.InputSystem;

// 플레이어의 기본적인 이동을 관리합니다.
// 키보드, 게임패드 모두 대응하도록 구현
public class PlayerController : MonoBehaviour
{
    [SerializeField] public PlayerData _playerData;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Animator _animator;
    // [SerializeField] private DialogueSystem _dialogueSystem;
    
    private StateMachine _stateMachine;
    public PlayerInput input;
    
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
    public Personalities partnerType;
    public string partnerCp; // 상대방 말버릇

    public void Awake()
    {
        _stateMachine = GetComponent<StateMachine>();
        input = GetComponent<PlayerInput>();
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
            if (_interactable != null)
            {
                var component = _interactable as Component;
                if (component.CompareTag("Tree"))
                {
                    Debug.Log("Found Tree");
                    _interactable.OnInteract += ShakeTree;
                    _interactable.OnInteractEnd += StopShakeTree;
                }
            }
        }

        if (other.gameObject.tag == "Item")
        {
            if (_item == null)
            {
                _item = other.gameObject.GetComponent<Item>();
                _item.OnPickup += Pickup;
                _item.OnPickupEnd += StopPickup;
                Debug.Log(_item.name);
            }
        }

        if (other.gameObject.tag == "Villager")
        {
            NPC = other.GetComponent<NPCController>();
            if (NPC != null)
            {
                partnerName = NPC.npcData.NPCName;
                partnerType = NPC.npcData.personality;
                partnerCp = NPC.npcData.CatchPhrase;
                Debug.Log($"{partnerName}, {partnerCp}");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _interactable = null;

        if (_item != null)
        {
            _item.OnPickup -= Pickup;
            _item.OnPickupEnd -= StopPickup;
            _item = null;
        }
        
        if (other.gameObject.tag == "Villager")
        {
            NPC = null;
            partnerName = null;
        }

        if (other.gameObject.tag == "Tree")
        {
            var component = _interactable as Component;
            if (component != null && component.CompareTag("Tree"))
            {
                _interactable.OnInteract -= ShakeTree;
                _interactable.OnInteractEnd -= StopShakeTree;
            }
        }
        
        isInteracting = false;
    }

    public void GetInputBool()
    {
        if (input.actions["Move"].IsPressed()) // WASD, 십자방향키, 좌측 조이스틱
        {
            isMoving = true;
        }

        if (input.actions["Dash"].IsPressed())
        {
            isDashing = true;
        }
    }

    public void ResetInputBool()
    {
        if (input.actions["Dash"].WasReleasedThisFrame())
        {
            isDashing = false;
        }

        if (input.actions["Move"].WasReleasedThisFrame())
        {
            isMoving = false;
            isDashing = false;
        }
    }

    public void PlayerMove()
    {
        Vector3 dir;

        Vector2 move = input.actions["Move"].ReadValue<Vector2>();

        dir = new Vector3(move.x, 0, move.y);
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
        if (invenOpened && input.actions["Revert"].WasPressedThisFrame())
        {
            invenOpened = false;
            Inventory.Instance.OnInventoryClose.Invoke();
            isInteracting = false;
        }
        
        if (isInteracting || invenOpened)
        {
            return;
        }

        if (input.actions["Inventory"].WasPressedThisFrame())
        {
            isInteracting = true;
            if (invenOpened)
            {
                return;
            }
            invenOpened = true;
            Inventory.Instance.OnInventoryOpen.Invoke();
        }
    }

    public void CheckInteraction()
    {
        // 겹치는거 어떻게 처리할지 고민해야함.
        if (isInteracting)
        {
            return;
        }
        
        if (_interactable != null && input.actions["Trigger"].WasPressedThisFrame())
        {
            isInteracting = true;
            _interactable.Interact();
        }
        
        // 일단 인풋을 다르게 받을거라 괜찮을 것 같긴 하지만... 
        if (_item != null && input.actions["Revert"].WasPressedThisFrame())
        {
            isInteracting = true;
            _item.BeingPickedUp();
        }
    }

    public void ShakeTree()
    {
        // transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.position - _interactable.GetPosition()), 2*Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(_interactable.GetPosition());
        _stateMachine.OnChangeState(StateMachine.StateType.PShake);
        Debug.Log("PShake 상태로 전환");
    }

    public void StopShakeTree()
    {
        _stateMachine.OnChangeState(StateMachine.StateType.PIdle);
        isInteracting = false;
    }

    public void Pickup()
    {
        _animator.SetTrigger("PickupTrigger");
    }

    public void StopPickup()
    {
        isInteracting = false;
        _item.OnPickup -= Pickup;
        _item.OnPickupEnd -= StopPickup;
        _item = null;
    }

}