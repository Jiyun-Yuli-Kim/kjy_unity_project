using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Data")]
public class ItemData : ScriptableObject
{
    public string ItemName;
    public ItemType ItemType;
    public Sprite ItemIcon;
    [SerializeField] public GameObject ItemPrefab;
}
