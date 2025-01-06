using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject slot1Icon;
    private Sprite slot1IconSprite;
    // Inventory.Instance.inventory; 굳이 변수로 가져올 필요는 없다.  

    void Start()
    {
        slot1IconSprite = slot1Icon.GetComponent<Image>().sprite;
    }

    private void ShowIcon(int number)
    {
        slot1IconSprite = Inventory.Instance.inventory[number].data.ItemIcon;
    }
}
