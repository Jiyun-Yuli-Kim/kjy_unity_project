using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject UICanvas;
    public ItemSlot[] slots = new ItemSlot[30];

    private bool _canvasOn = false;
    
    public int pointer = 0;
    
    void Awake()
    {
        UICanvas.SetActive(false);
        slots = GetComponentsInChildren<ItemSlot>();
        Inventory.Instance.OnInventoryOpen.AddListener(OpenInventory);
        Inventory.Instance.OnInventoryClose.AddListener(CloseInventory);
    }

    void Start()
    {
        slots[pointer].Highlight();
        // Tester();
    }

    void Update()
    {
        CheckPointer();
        Debug.Log(pointer);
    }

    private void OpenInventory()
    {
        pointer = 0;
        UICanvas.SetActive(true);
        _canvasOn = true;
    }

    private void CloseInventory()
    {
        UICanvas.SetActive(false);
        _canvasOn = false;
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
    
    // row 판정 : pointer / 10 + 1
    // col 판정 : pointer % 10
     
    // 하이라이터 관리는 그냥 포인터 따라감
    
    // while UICanvas가 활성화되어있는 동안
    // 인풋을 받고, 그 유효성을 체크한다
    // switch문 활용
    // 현재 PlayerController상에서 인풋 받는걸 막아둔 상태이므로 따로 판정
    // case 'w', 'W' (up) => row2, 3인 경우만 허용, 아닐시 리턴
    // case 's', 'S' (down) => row1, 2인 경우만 허용, 아닐시 리턴
    // case 'a', 'A' (left) => col 2-10인 경우만 허용, 아닐시 리턴
    // case 'd', 'D' (right) => col 1-9인 경우만 허용, 아닐시 리턴
    
    // 포인터와 실제 인벤토리 슬롯의 맵핑
    // 포인터가 있는 인덱스의 슬롯의 하이라이터 활성화하기
    
    void CheckPointer()
    {
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

    void Tester()
    {
        foreach (ItemSlot slot in slots)
        {
            Debug.Log(slot.name);
        }
    }
}
