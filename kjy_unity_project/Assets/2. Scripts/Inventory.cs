using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    private List<Item> _inventory = new List<Item>();
    public int _curItemCount = 0; 
    private const int _maxItemCount = 30;
    
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

    // 인벤토리 맨 뒤에 아이템 추가
    public void AddItem(Item item)
    {
        if (_curItemCount == _maxItemCount)
        {   
            Debug.LogWarning("Inventory is full");
            return;
        }

        _inventory.Add(item);
        _curItemCount++;
        Debug.Log($"인벤토리에 아이템 추가 : {item}");
        ShowInventory();
    }

    // 해당 아이템 삭제
    public void RemoveItem(Item item)
    {
        if (_curItemCount == 0)
        {
            Debug.LogWarning("Inventory is empty");
            return;
        }

        _inventory.Remove(item);
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
        _inventory.RemoveAt(index);
    }

    public void ShowInventory()
    {
        foreach (Item item in _inventory)
        {
            Debug.Log(item.data.ItemName);
        }
    }

}
