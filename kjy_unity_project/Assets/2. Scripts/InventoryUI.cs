using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject UICanvas;
    public ItemSlot[] slots = new ItemSlot[30];
    
    void Awake()
    {
        UICanvas.SetActive(false);
        slots = GetComponentsInChildren<ItemSlot>();
        InteractionManager.Instance.OnInventoryOpen.AddListener(OpenInventory);
        InteractionManager.Instance.OnInventoryClose.AddListener(CloseInventory);
    }

    void Start()
    {
        Tester();
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

    void Tester()
    {
        foreach (ItemSlot slot in slots)
        {
            Debug.Log(slot.name);
        }
    }
}
