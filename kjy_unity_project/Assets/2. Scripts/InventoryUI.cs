using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject UICanvas;
    [SerializeField] private GameObject _itemPopup;
    
    public ItemSlot[] slots = new ItemSlot[30];

    private bool _canvasOn = false;
    public bool popupOpen = false;
    
    public int pointer = 0;
    
    public PlayerInput input;
    
    void Awake()
    {
        UICanvas.SetActive(false);
        _itemPopup.SetActive(false);
        slots = GetComponentsInChildren<ItemSlot>();
        Inventory.Instance.OnInventoryOpen.AddListener(OpenInventory);
        Inventory.Instance.OnInventoryClose.AddListener(CloseInventory);
    }

    void Start()
    {
        // Tester();
    }

    void Update()
    {
        CheckPointer();
    }

    void LateUpdate()
    {
        CheckPopup();
    }

    private void OpenInventory()
    {
        pointer = 0;
        slots[pointer].Highlight();
        UICanvas.SetActive(true);
        _canvasOn = true;
    }

    private void CloseInventory()
    {
        UICanvas.SetActive(false);
        _canvasOn = false;
        foreach (ItemSlot s in slots)
        {
            s.HighlightOff();
        }
    }

    public void ActivateSlot(int index)
    {
        
    }
    
    void CheckPointer()
    {
        if (popupOpen)
        {
            return;
        }

        int row = pointer / 10 +1;
        int col = pointer % 10;
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (row == 2 || row == 3)
            {
                slots[pointer].HighlightOff();
                pointer -= 10;
                slots[pointer].Highlight();
            }
            else
            {
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (row == 1 || row == 2)
            {
                slots[pointer].HighlightOff();
                pointer += 10;
                slots[pointer].Highlight();
            }
            else
            {
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (col >= 1 && col <= 9)
            {
                slots[pointer].HighlightOff();
                pointer -= 1;
                slots[pointer].Highlight();
            }
            else
            {
                return;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (col >= 0 && col <= 8)
            {
                slots[pointer].HighlightOff();
                pointer += 1;
                slots[pointer].Highlight();
            }
            else
            {
                return;
            }
        }
    }

    void CheckPopup()
    {
        if (input.actions["Trigger"].WasPressedThisFrame())
        {
            _itemPopup.SetActive(true);
            popupOpen = true;
        }

        if (popupOpen && input.actions["Revert"].WasPressedThisFrame())
        {
            _itemPopup.SetActive(false);
            popupOpen = false;
        }
    }

    void Tester()
    {
        foreach (ItemSlot slot in slots)
        {
            Debug.Log(slot.name);
        }
    }
}
