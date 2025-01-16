using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject UICanvas;
    public ItemSlot[] slots = new ItemSlot[30];

    public int pointer;
    
    void Awake()
    {
        UICanvas.SetActive(false);
        slots = GetComponentsInChildren<ItemSlot>();
        Inventory.Instance.OnInventoryOpen.AddListener(OpenInventory);
        Inventory.Instance.OnInventoryClose.AddListener(CloseInventory);
    }

    void Start()
    {
        // Tester();
    }

    private void OpenInventory()
    {
        UICanvas.SetActive(true);
    }

    private void CloseInventory()
    {
        UICanvas.SetActive(false);
    }

    public void ActivateSlot(int index)
    {
        
    }
    
    // 포인터 관리.. 지금 슬롯은 slots 배열로 관리중
    // 포인터 초기 위치는 0
    //  0  1  2  3  4  5  6  7  8  9
    // 10 11 12 13 14 15 16 17 18 19
    // 20 21 22 23 24 25 26 27 28 29 
    // row와 col 변수로 관리한다
    // row1의 경우: 아래로 이동 가능 [1, x] -> [2, x]
    // row2: 위아래 모두 가능
    // row3: 위로만 가능
    // col1: 우측만 가능
    // col2-9: 좌우 모두 가능
    // col10: 좌측만 가능 
    
    // 하이라이터 관리는 그냥 포인터 따라감
    
    void Tester()
    {
        foreach (ItemSlot slot in slots)
        {
            Debug.Log(slot.name);
        }
    }
}
