using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public ItemSlot[] slots = new ItemSlot[30];
    
    void Awake()
    {
        slots = GetComponentsInChildren<ItemSlot>();
    }

    void Start()
    {
        Tester();
    }

    private void ShowIcon(int number)
    {
        // slot1IconSprite = Inventory.Instance.inventory[number].data.ItemIcon;
    }

    void Tester()
    {
        foreach (ItemSlot slot in slots)
        {
            Debug.Log(slot.name);
        }
    }
    
}
