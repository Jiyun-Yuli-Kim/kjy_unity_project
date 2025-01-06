using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject slot1Icon;
    private List<Item> _inventory = Inventory.Instance.inventory;

    private void ShowIcon(int number)
    {
        slot1Icon.sprite .Instance.inventory[number].data.ItemIcon = Resources.Load<Sprite>("Icon");
    }



}
