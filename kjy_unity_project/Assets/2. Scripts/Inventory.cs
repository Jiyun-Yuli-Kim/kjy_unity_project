using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    private List<GameObject> _inventory = new List<GameObject>();
    public int _curItemCount = 0; 
    private int _maxItemCount = 30;
    
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
    public void AddItem(GameObject item)
    {
        _inventory.Add(item);
    }

    // 해당 아이템 삭제
    public void RemoveItem(GameObject item)
    {
        _inventory.Remove(item);
    }

    // 해당 인덱스의 아이템 삭제
    public void RemoveItem(int index)
    {
        _inventory.RemoveAt(index);
    }

}
