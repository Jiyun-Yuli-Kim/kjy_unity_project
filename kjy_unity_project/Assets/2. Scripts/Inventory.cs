using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    public Item[] inventory;
    public int _curItemCount = 0; 
    private const int _maxItemCount = 30;
    
    [SerializeField] private InventoryUI _inventoryUI;
    
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
        inventory = new Item[30];
    }

    // 인벤토리 맨 뒤에 아이템 추가
    public void AddItem(Item item)
    {
        if (_curItemCount == _maxItemCount)
        {   
            Debug.LogWarning("Inventory is full");
            return;
        }

        Debug.Log(_curItemCount);
        Debug.Log(inventory.Length);
        
        inventory[_curItemCount] = item;
        Debug.Log(_inventoryUI.slots.Length);
        _inventoryUI.slots[_curItemCount].ItemNameText.text = item.data.ItemName;
        _inventoryUI.slots[_curItemCount].ItemIconSprite.sprite = item.data.ItemIcon;
        _inventoryUI.slots[_curItemCount].ItemIcon.SetActive(true);

        //_inventoryUI.Show();
        
        _curItemCount++; 
        Debug.Log($"인벤토리에 아이템 추가 : {item}");
        // ShowInventory();
    }

    // 해당 아이템 삭제
    public void RemoveItem(Item item)
    {
        if (_curItemCount == 0)
        {
            Debug.LogWarning("Inventory is empty");
            return;
        }

        inventory[_curItemCount-1] = null;
        _curItemCount--;
    }

    // 해당 인덱스의 아이템 삭제
    public void RemoveItem(int index)
    {
        if (_curItemCount == 0)
        {
            Debug.LogWarning("Inventory is empty");
            return;
        }
        inventory[index-1] = null;
    }

    public void ShowInventory()
    {
        foreach (Item item in inventory)
        {
            Debug.Log(item.data.ItemName);
        }
    }
}
