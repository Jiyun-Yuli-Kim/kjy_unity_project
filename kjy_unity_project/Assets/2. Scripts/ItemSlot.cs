using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] public GameObject highlighterE;
    [SerializeField] public GameObject highlighterF;
    [SerializeField] public GameObject ItemIcon;
    [SerializeField] public GameObject ItemNameBubble;
    [SerializeField] public TextMeshProUGUI ItemNameText;

    private void Awake()
    {
        highlighterE.SetActive(false);
        highlighterF.SetActive(false);
        ItemIcon.SetActive(false);
        ItemNameBubble.SetActive(false);
    }

    private void Update()
    {
        Tester();
    }

    public void AddIcon()
    {
        
    }

    void Tester()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            highlighterE.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            highlighterF.SetActive(true);
            ItemIcon.SetActive(true);
            ItemNameText.text = "아이템";
            ItemNameBubble.SetActive(true);
        }        
        if (Input.GetKeyDown(KeyCode.E))
        {
            highlighterE.SetActive(false);
            highlighterF.SetActive(false);
            ItemIcon.SetActive(false);
            ItemNameBubble.SetActive(false);        
        }
    }
}
